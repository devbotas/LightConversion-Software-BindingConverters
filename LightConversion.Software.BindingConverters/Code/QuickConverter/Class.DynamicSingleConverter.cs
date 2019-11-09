// Copyright 2019 Light Conversion, UAB
// Licensed under the Apache 2.0, see LICENSE.md for more details.

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Data;
using Microsoft.CSharp.RuntimeBinder;
using Expression = System.Linq.Expressions.Expression;

namespace LightConversion.Software.BindingConverters.QuickConverter {
    public class DynamicSingleConverter : IValueConverter {
        private static readonly Type _namedObjectType = typeof(DependencyObject).Assembly.GetType("MS.Internal.NamedObject", false);

        private static readonly ConcurrentDictionary<Type, Func<object, object>> CastFunctions = new ConcurrentDictionary<Type, Func<object, object>>();

        public string ConvertExpression { get; }
        public string ConvertBackExpression { get; }
        public Exception LastException { get; private set; }
        public int ExceptionCount { get; private set; }

        public string ConvertExpressionDebugView { get; }
        public string ConvertBackExpressionDebugView { get; }

        public Type PType { get; }

        public Type ValueType { get; }

        public object[] Values {
            get { return _values; }
        }

        private readonly Func<object, object[], object, object> _converter;
        private readonly Func<object, object[], object, object> _convertBack;
        private readonly object[] _values;
        private readonly DataContainer[] _toDataContainers;
        private readonly DataContainer[] _fromDataContainers;
        private readonly IValueConverter _chainedConverter;

        public DynamicSingleConverter(Func<object, object[], object, object> converter, Func<object, object[], object, object> convertBack, object[] values, string convertExp, string convertExpDebug, string convertBackExp, string convertBackExpDebug, Type pType, Type vType, DataContainer[] toDataContainers, DataContainer[] fromDataContainers, IValueConverter chainedConverter) {
            _converter = converter;
            _convertBack = convertBack;
            _values = values;
            _toDataContainers = toDataContainers;
            _fromDataContainers = fromDataContainers;
            ConvertExpression = convertExp;
            ConvertBackExpression = convertBackExp;
            ConvertExpressionDebugView = convertExpDebug;
            ConvertBackExpressionDebugView = convertBackExpDebug;
            PType = pType;
            ValueType = vType;
            _chainedConverter = chainedConverter;
        }

        private object DoConversion(object value, Type targetType, object parameter, Func<object, object[], object, object> func, bool convertingBack, CultureInfo culture) {
            if (convertingBack) {
                if (_chainedConverter != null) {
                    try {
                        value = _chainedConverter.ConvertBack(value, targetType, parameter, culture);
                    } catch (Exception e) {
                        EquationTokenizer.ThrowQuickConverterEvent(new ChainedConverterExceptionEventArgs(ConvertExpression, value, targetType, parameter, culture, true, _chainedConverter, this, e));
                        return DependencyProperty.UnsetValue;
                    }

                    if (value == DependencyProperty.UnsetValue || value == System.Windows.Data.Binding.DoNothing) return value;
                }

                if (ValueType != null && !ValueType.IsInstanceOfType(value)) return System.Windows.Data.Binding.DoNothing;
            } else {
                if (value == DependencyProperty.UnsetValue || _namedObjectType.IsInstanceOfType(value) || (PType != null && !PType.IsInstanceOfType(value))) return DependencyProperty.UnsetValue;
            }

            object result = value;
            if (func != null) {
                try {
                    result = func(result, _values, parameter);
                } catch (Exception e) {
                    LastException = e;
                    ++ExceptionCount;
                    if (Debugger.IsAttached) Console.WriteLine("QuickMultiConverter Exception (\"" + (convertingBack ? ConvertBackExpression : ConvertExpression) + "\") - " + e.Message + (e.InnerException != null ? " (Inner - " + e.InnerException.Message + ")" : ""));
                    if (convertingBack) EquationTokenizer.ThrowQuickConverterEvent(new RuntimeSingleConvertExceptionEventArgs(ConvertBackExpression, ConvertBackExpressionDebugView, null, value, _values, parameter, this, e));
                    else EquationTokenizer.ThrowQuickConverterEvent(new RuntimeSingleConvertExceptionEventArgs(ConvertExpression, ConvertExpressionDebugView, value, null, _values, parameter, this, e));
                    return DependencyProperty.UnsetValue;
                } finally {
                    var dataContainers = convertingBack ? _fromDataContainers : _toDataContainers;
                    if (dataContainers != null) {
                        foreach (var container in dataContainers) container.Value = null;
                    }
                }
            }

            if (result == DependencyProperty.UnsetValue || result == System.Windows.Data.Binding.DoNothing) return result;

            if (!convertingBack && _chainedConverter != null) {
                try {
                    result = _chainedConverter.Convert(result, targetType, parameter, culture);
                } catch (Exception e) {
                    EquationTokenizer.ThrowQuickConverterEvent(new ChainedConverterExceptionEventArgs(ConvertExpression, result, targetType, parameter, culture, false, _chainedConverter, this, e));
                    return DependencyProperty.UnsetValue;
                }
            }

            if (result == DependencyProperty.UnsetValue || result == System.Windows.Data.Binding.DoNothing || result == null || targetType == null || targetType == typeof(object)) return result;

            if (targetType == typeof(string)) return result.ToString();

            Func<object, object> cast;
            if (!CastFunctions.TryGetValue(targetType, out cast)) {
                ParameterExpression par = Expression.Parameter(typeof(object));
                cast = Expression.Lambda<Func<object, object>>(Expression.Convert(Expression.Dynamic(Binder.Convert(CSharpBinderFlags.ConvertExplicit, targetType, typeof(object)), targetType, par), typeof(object)), par).Compile();
                CastFunctions.TryAdd(targetType, cast);
            }
            if (cast != null) {
                try {
                    result = cast(result);
                } catch {
                    CastFunctions[targetType] = null;
                }
            }
            return result;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return DoConversion(value, targetType, parameter, _converter, false, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return DoConversion(value, targetType, parameter, _convertBack, true, culture);
        }
    }
}