// Copyright 2019 Light Conversion, UAB
// Licensed under the Apache 2.0, see LICENSE.md for more details.

using System;
using System.Globalization;
using System.Windows.Data;

namespace LightConversion.Software.BindingConverters.QuickConverter {
    public class ChainedConverterExceptionEventArgs : QuickConverterEventArgs {
        public override QuickConverterEventType Type {
            get { return QuickConverterEventType.ChainedConverterException; }
        }

        public IValueConverter ChainedConverter { get; }

        public object InputValue { get; }

        public Type TargetType { get; }

        public object Parameter { get; }

        public CultureInfo Culture { get; }

        public bool ConvertingBack { get; }

        public object ParentConverter { get; }

        public Exception Exception { get; }

        internal ChainedConverterExceptionEventArgs(string expression, object inputValue, Type targetType, object parameter, CultureInfo culture, bool convertingBack, IValueConverter chainedConverter, object parentConverter, Exception exception) : base(expression) {
            InputValue = inputValue;
            TargetType = targetType;
            Parameter = parameter;
            Culture = culture;
            ConvertingBack = convertingBack;
            ChainedConverter = chainedConverter;
            ParentConverter = parentConverter;
            Exception = exception;
        }
    }
}