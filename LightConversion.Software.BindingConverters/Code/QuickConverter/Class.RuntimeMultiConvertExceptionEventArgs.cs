// Copyright 2019 Light Conversion, UAB
// Licensed under the Apache 2.0, see LICENSE.md for more details.

using System;

namespace LightConversion.Software.BindingConverters.QuickConverter {
    public class RuntimeMultiConvertExceptionEventArgs : QuickConverterEventArgs {
        public override QuickConverterEventType Type {
            get { return QuickConverterEventType.RuntimeCodeException; }
        }

        public object P0 { get; }
        public object P1 { get; }
        public object P2 { get; }
        public object P3 { get; }
        public object P4 { get; }
        public object P5 { get; }
        public object P6 { get; }
        public object P7 { get; }
        public object P8 { get; }
        public object P9 { get; }

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

        public DynamicMultiConverter Converter { get; }

        public Exception Exception { get; }

        public string DebugView { get; }

        internal RuntimeMultiConvertExceptionEventArgs(string expression, string debugView, object[] p, int[] pIndices, object value, object[] values, object parameter, DynamicMultiConverter converter, Exception exception) : base(expression) {
            DebugView = debugView;
            if (p != null) {
                for (int i = 0; i < p.Length; ++i) {
                    switch (pIndices[i]) {
                        case 0:
                            P0 = p[i];
                            break;

                        case 1:
                            P1 = p[i];
                            break;

                        case 2:
                            P2 = p[i];
                            break;

                        case 3:
                            P3 = p[i];
                            break;

                        case 4:
                            P4 = p[i];
                            break;

                        case 5:
                            P5 = p[i];
                            break;

                        case 6:
                            P6 = p[i];
                            break;

                        case 7:
                            P7 = p[i];
                            break;

                        case 8:
                            P8 = p[i];
                            break;

                        case 9:
                            P9 = p[i];
                            break;
                    }
                }
            }
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