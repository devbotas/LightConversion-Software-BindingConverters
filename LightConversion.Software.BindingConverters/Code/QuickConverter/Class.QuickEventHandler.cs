// Copyright 2019 Light Conversion, UAB
// Licensed under the Apache 2.0, see LICENSE.md for more details.

using System;
using System.Diagnostics;
using System.Windows;

namespace LightConversion.Software.BindingConverters.QuickConverter {
    public abstract class QuickEventHandler {
        public string HandlerExpression { get; }

        public object[] Values { get; }

        public string HandlerExpressionDebugView { get; }

        public Exception LastException { get; protected set; }
        public int ExceptionCount { get; protected set; }

        protected Delegate Handler;
        protected string[] Parameters;
        protected DataContainer[] DataContainers;

        protected QuickEventHandler(Delegate handler, string[] parameters, object[] values, string expression, string expressionDebug, DataContainer[] dataContainers) {
            Handler = handler;
            Parameters = parameters;
            Values = values;
            HandlerExpression = expression;
            HandlerExpressionDebugView = expressionDebug;
            DataContainers = dataContainers;
        }
    }

    internal class QuickEventHandler<T1, T2> : QuickEventHandler {
        private object[] _parArray;
        private int _dataContextIndex = -1;
        private int _eventArgsIndex = -1;
        private readonly int[] _pIndex = { -1, -1, -1, -1, -1 };
        private readonly bool _ignoreIfNotOriginalSource;
        private readonly bool _setHandled;

        private object _lastSender;

        public QuickEventHandler(Delegate handler, string[] parameters, object[] values, string expression, string expressionDebug, DataContainer[] dataContainers, bool ignoreIfNotOriginalSource, bool setHandled) : base(handler, parameters, values, expression, expressionDebug, dataContainers) {
            _ignoreIfNotOriginalSource = ignoreIfNotOriginalSource;
            _setHandled = setHandled;
        }

        public void Handle(T1 sender, T2 args) {
            if (_ignoreIfNotOriginalSource && args is RoutedEventArgs && (args as RoutedEventArgs).OriginalSource != (args as RoutedEventArgs).Source) return;

            if (!SetupParameters(sender, args)) return;

            try {
                Handler.DynamicInvoke(_parArray);
            } catch (Exception e) {
                LastException = e;
                ++ExceptionCount;
                if (Debugger.IsAttached) Console.WriteLine("QuickEvent Exception (\"" + HandlerExpression + "\") - " + e.Message + (e.InnerException != null ? " (Inner - " + e.InnerException.Message + ")" : ""));
                EquationTokenizer.ThrowQuickConverterEvent(new RuntimeEventHandlerExceptionEventArgs(sender, args, HandlerExpression, HandlerExpressionDebugView, Values, this, e));
            } finally {
                if (_dataContextIndex >= 0) _parArray[_dataContextIndex] = null;
                if (_eventArgsIndex >= 0) _parArray[_eventArgsIndex] = null;
                if (DataContainers != null) {
                    foreach (var container in DataContainers) container.Value = null;
                }
                if (_setHandled && args is RoutedEventArgs) (args as RoutedEventArgs).Handled = true;
            }
        }

        private bool SetupParameters(object sender, object args) {
            if (_lastSender != sender) _parArray = null;
            _lastSender = sender;
            string failMessage = null;
            if (_parArray == null) {
                _parArray = new object[Parameters.Length];
                for (int i = 0; i < Parameters.Length; ++i) {
                    var par = Parameters[i];
                    switch (par) {
                        case "sender":
                            _parArray[i] = sender;
                            break;

                        case "eventArgs":
                            _eventArgsIndex = i;
                            break;

                        case "dataContext":
                            _dataContextIndex = i;
                            break;

                        default:
                            if (par.Length == 2 && par[0] == 'V' && Char.IsDigit(par[1])) _parArray[i] = Values[par[1] - '0'];
                            else if (par.Length == 2 && par[0] == 'P' && par[1] >= '0' && par[1] <= '4') _pIndex[par[1] - '0'] = i;
                            else if (sender is FrameworkElement) {
                                _parArray[i] = (sender as FrameworkElement).FindName(par);
                                if (_parArray[i] == null) failMessage = "Could not find target for $" + par + ".";
                            } else failMessage = "Sender is not a framework element. Finding targets by name only works when sender is a framework element.";
                            break;
                    }
                }
            }
            if (_dataContextIndex >= 0 && sender is FrameworkElement) _parArray[_dataContextIndex] = (sender as FrameworkElement).DataContext;
            if (_eventArgsIndex >= 0) _parArray[_eventArgsIndex] = args;
            for (int i = 0; i <= 4; ++i) {
                if (_pIndex[i] == -1) continue;
                if (!(sender is DependencyObject)) {
                    failMessage = "Cannot access $P0-$P4 when sender is not a DependencyObject.";
                    break;
                }
                if (i == 0) _parArray[_pIndex[i]] = QuickEvent.GetP0(sender as DependencyObject);
                else if (i == 1) _parArray[_pIndex[i]] = QuickEvent.GetP0(sender as DependencyObject);
                else if (i == 2) _parArray[_pIndex[i]] = QuickEvent.GetP0(sender as DependencyObject);
                else if (i == 3) _parArray[_pIndex[i]] = QuickEvent.GetP0(sender as DependencyObject);
                else if (i == 4) _parArray[_pIndex[i]] = QuickEvent.GetP0(sender as DependencyObject);
            }
            if (failMessage != null) {
                EquationTokenizer.ThrowQuickConverterEvent(new RuntimeEventHandlerExceptionEventArgs(sender, args, HandlerExpression, HandlerExpressionDebugView, Values, this, new Exception(failMessage)));
                return false;
            }
            return true;
        }
    }
}