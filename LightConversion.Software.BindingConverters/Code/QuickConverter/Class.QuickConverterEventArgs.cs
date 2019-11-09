// Copyright 2019 Light Conversion, UAB
// Licensed under the Apache 2.0, see LICENSE.md for more details.

namespace LightConversion.Software.BindingConverters.QuickConverter {
    public delegate void QuickConverterEventHandler(QuickConverterEventArgs args);

    public abstract class QuickConverterEventArgs {
        public abstract QuickConverterEventType Type { get; }

        public string Expression { get; }

        internal QuickConverterEventArgs(string expression) {
            Expression = expression;
        }
    }
}