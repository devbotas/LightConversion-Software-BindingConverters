// Copyright 2019 Light Conversion, UAB
// Licensed under the Apache 2.0, see LICENSE.md for more details.

using System;
using System.Windows.Markup;

namespace LightConversion.Software.BindingConverters.QuickConverter {
    public class MarkupExtensionExceptionEventArgs : QuickConverterEventArgs {
        public override QuickConverterEventType Type {
            get { return QuickConverterEventType.MarkupException; }
        }

        public MarkupExtension MarkupExtension { get; }

        public Exception Exception { get; }

        internal MarkupExtensionExceptionEventArgs(string expression, MarkupExtension markupExtension, Exception exception) : base(expression) {
            MarkupExtension = markupExtension;
            Exception = exception;
        }
    }
}