// Copyright 2019 Light Conversion, UAB
// Licensed under the Apache 2.0, see LICENSE.md for more details.

using LightConversion.Software.BindingConverters.QuickConverter.Tokens;

namespace LightConversion.Software.BindingConverters.QuickConverter {
    public class TokenizationSuccessEventArgs : QuickConverterEventArgs {
        public override QuickConverterEventType Type {
            get { return QuickConverterEventType.TokenizationSuccess; }
        }

        public TokenBase Root { get; set; }

        internal TokenizationSuccessEventArgs(string expression, TokenBase root) : base(expression) {
            Root = root;
        }
    }
}