// Copyright 2019 Light Conversion, UAB
// Licensed under the Apache 2.0, see LICENSE.md for more details.

using System;
using System.Windows;

namespace LightConversion.Software.BindingConverters.QuickConverter {
    public class RuntimeEventHandlerExceptionEventArgs : QuickConverterEventArgs {
        public override QuickConverterEventType Type {
            get { return QuickConverterEventType.RuntimeCodeException; }
        }

        public object Sender { get; }

        public object EventArgs { get; }

        public object V0 { get; }
        public object V1 { get; }
        public object V2 { get; }
        public object V3 { get; }
        public object V4 { get; }
        public object V5 { get; }
        public object V6 { get; }
        public object V7 { get; }
        public object V8 { get; }
        public object V9 { get; }

        public object P0 { get; }
        public object P1 { get; }
        public object P2 { get; }
        public object P3 { get; }
        public object P4 { get; }

        public QuickEventHandler Handler { get; }

        public Exception Exception { get; }

        public string DebugView { get; }

        internal RuntimeEventHandlerExceptionEventArgs(object sender, object eventArgs, string expression, string debugView, object[] values, QuickEventHandler handler, Exception exception) : base(expression) {
            Sender = sender;
            EventArgs = eventArgs;
            DebugView = debugView;
            V0 = values[0];
            V1 = values[1];
            V2 = values[2];
            V3 = values[3];
            V4 = values[4];
            V5 = values[5];
            V6 = values[6];
            V7 = values[7];
            V8 = values[8];
            V9 = values[9];
            if (sender is DependencyObject) {
                P0 = QuickEvent.GetP0(sender as DependencyObject);
                P1 = QuickEvent.GetP1(sender as DependencyObject);
                P2 = QuickEvent.GetP2(sender as DependencyObject);
                P3 = QuickEvent.GetP3(sender as DependencyObject);
                P4 = QuickEvent.GetP4(sender as DependencyObject);
            }
            Handler = handler;
            Exception = exception;
        }
    }
}