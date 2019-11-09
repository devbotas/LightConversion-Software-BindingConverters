// Copyright 2019 Light Conversion, UAB
// Licensed under the Apache 2.0, see LICENSE.md for more details.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;

namespace LightConversion.Software.BindingConverters.QuickConverter.Tokens {
    public class BinaryOperatorToken : TokenBase {
        private static readonly ExpressionType[] Types = { default(ExpressionType), default(ExpressionType), default(ExpressionType), ExpressionType.Multiply, ExpressionType.Divide, ExpressionType.Modulo, ExpressionType.Add, ExpressionType.Subtract, ExpressionType.GreaterThanOrEqual, ExpressionType.LessThanOrEqual, ExpressionType.GreaterThan, ExpressionType.LessThan, ExpressionType.Equal, ExpressionType.NotEqual, ExpressionType.AndAlso, ExpressionType.AndAlso, ExpressionType.OrElse, ExpressionType.And, ExpressionType.And, ExpressionType.Or, ExpressionType.ExclusiveOr, default(ExpressionType) };

        public override Type ReturnType {
            get { return Operation >= Operator.GreaterOrEqual && Operation <= Operator.Or ? typeof(bool) : typeof(object); }
        }

        public override TokenBase[] Children {
            get { return new[] { Left, Right }; }
        }

        public TokenBase Left { get; }
        public TokenBase Right { get; }
        public Operator Operation { get; internal set; }

        internal BinaryOperatorToken(TokenBase left, TokenBase right, Operator operation) {
            Left = left;
            Right = right;
            Operation = operation;
        }

        internal override bool TryGetToken(ref string text, out TokenBase token, bool requireReturnValue = true) {
            throw new NotImplementedException();
        }

        internal override Expression GetExpression(List<ParameterExpression> parameters, Dictionary<string, ConstantExpression> locals, List<DataContainer> dataContainers, Type dynamicContext, LabelTarget label, bool requiresReturnValue = true) {
            if (Operation == Operator.And || Operation == Operator.AlternateAnd) return Expression.Convert(Expression.AndAlso(Expression.Convert(Left.GetExpression(parameters, locals, dataContainers, dynamicContext, label), typeof(bool)), Expression.Convert(Right.GetExpression(parameters, locals, dataContainers, dynamicContext, label), typeof(bool))), typeof(object));
            if (Operation == Operator.Or) return Expression.Convert(Expression.OrElse(Expression.Convert(Left.GetExpression(parameters, locals, dataContainers, dynamicContext, label), typeof(bool)), Expression.Convert(Right.GetExpression(parameters, locals, dataContainers, dynamicContext, label), typeof(bool))), typeof(object));
            CallSiteBinder binder = Binder.BinaryOperation(CSharpBinderFlags.None, Types[(int)Operation], dynamicContext ?? typeof(object), new[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) });
            return Expression.Dynamic(binder, typeof(object), Left.GetExpression(parameters, locals, dataContainers, dynamicContext, label), Right.GetExpression(parameters, locals, dataContainers, dynamicContext, label));
        }
    }
}