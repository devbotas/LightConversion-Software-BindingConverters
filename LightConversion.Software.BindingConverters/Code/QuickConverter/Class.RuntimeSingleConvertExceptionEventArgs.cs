// Copyright 2019 Light Conversion, UAB
// Licensed under the Apache 2.0, see LICENSE.md for more details.

using System;

namespace LightConversion.Software.BindingConverters.QuickConverter {
    public class RuntimeSingleConvertExceptionEventArgs : QuickConverterEventArgs {
        public override QuickConverterEventType Type {
            get { return QuickConverterEventType.RuntimeCodeException; }
        }

        public object P { get; }

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

        public object Value { get; }

        public object Parameter { get; }

        public DynamicSingleConverter Converter { get; }

        public Exception Exception { get; }

        public string DebugView { get; }

        internal RuntimeSingleConvertExceptionEventArgs(string expression, string debugView, object p, object value, object[] values, object parameter, DynamicSingleConverter converter, Exception exception) : base(expression) {
            DebugView = debugView;
            P = p;
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
            Value = value;
            Parameter = parameter;
            Converter = converter;
            Exception = exception;
        }
    }
}