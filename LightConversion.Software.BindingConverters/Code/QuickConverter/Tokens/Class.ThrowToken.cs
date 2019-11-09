// Copyright 2019 Light Conversion, UAB
// Licensed under the Apache 2.0, see LICENSE.md for more details.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LightConversion.Software.BindingConverters.QuickConverter.Tokens {
    public class ThrowToken : TokenBase {
        internal ThrowToken() {}

        public override Type ReturnType {
            get { return typeof(object); }
        }

        public override TokenBase[] Children {
            get { return new[] { Exception }; }
        }

        public TokenBase Exception { get; private set; }

        internal override bool TryGetToken(ref string text, out TokenBase token, bool requireReturnValue = true) {
            token = null;
            if (!text.StartsWith("throw")) return false;
            string temp = text.Substring(5).TrimStart();

            TokenBase valToken = null;
            if (!EquationTokenizer.TryGetValueToken(ref temp, out valToken)) return false;

            text = temp;
            token = new ThrowToken() { Exception = valToken };
            return true;
        }

        internal override Expression GetExpression(List<ParameterExpression> parameters, Dictionary<string, ConstantExpression> locals, List<DataContainer> dataContainers, Type dynamicContext, LabelTarget label, bool requiresReturnValue = true) {
            return Expression.Throw(Exception.GetExpression(parameters, locals, dataContainers, dynamicContext, label), typeof(object));
        }
    }
}