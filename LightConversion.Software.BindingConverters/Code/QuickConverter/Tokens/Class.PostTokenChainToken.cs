// Copyright 2019 Light Conversion, UAB
// Licensed under the Apache 2.0, see LICENSE.md for more details.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LightConversion.Software.BindingConverters.QuickConverter.Tokens {
    public class PostTokenChainToken : TokenBase, IPostToken {
        internal PostTokenChainToken(TokenBase token) {
            Target = token;
        }

        public override Type ReturnType {
            get { return Target.ReturnType; }
        }

        public override TokenBase[] Children {
            get { return new[] { Target }; }
        }

        public TokenBase Target { get; private set; }

        internal override bool SetPostTarget(TokenBase target) {
            Target = target;
            return true;
        }

        internal override bool TryGetToken(ref string text, out TokenBase token, bool requireReturnValue = true) {
            token = null;
            return true;
        }

        internal override Expression GetExpression(List<ParameterExpression> parameters, Dictionary<string, ConstantExpression> locals, List<DataContainer> dataContainers, Type dynamicContext, LabelTarget label, bool requiresReturnValue = true) {
            label = Expression.Label(requiresReturnValue ? typeof(object) : typeof(void));
            return Expression.Label(label, Target.GetExpression(parameters, locals, dataContainers, dynamicContext, label, requiresReturnValue));
        }
    }
}