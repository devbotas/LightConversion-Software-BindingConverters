// Copyright 2019 Light Conversion, UAB
// Licensed under the Apache 2.0, see LICENSE.md for more details.

namespace LightConversion.Software.BindingConverters.QuickConverter {
    public class TokenizationFailureEventArgs : QuickConverterEventArgs {
        public override QuickConverterEventType Type {
            get { return QuickConverterEventType.TokenizationFailure; }
        }

        internal TokenizationFailureEventArgs(string expression) : base(expression) {}
    }
}