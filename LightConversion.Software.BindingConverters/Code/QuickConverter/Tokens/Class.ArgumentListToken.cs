// Copyright 2019 Light Conversion, UAB
// Licensed under the Apache 2.0, see LICENSE.md for more details.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LightConversion.Software.BindingConverters.QuickConverter.Tokens {
    public class ArgumentListToken : TokenBase {
        private readonly char _open;
        private readonly char _close;
        private readonly bool _findAssignments;
        private readonly Type _assignmentType;
        private readonly bool _allowSubLists;
        private readonly bool _allowTypeCasts;

        public override Type ReturnType {
            get { return typeof(object); }
        }

        public override TokenBase[] Children {
            get { return Arguments.ToArray(); }
        }

        public TokenBase[] Arguments { get; private set; }

        internal ArgumentListToken(char open, char close, Type assignmentType) {
            _open = open;
            _close = close;
            _findAssignments = true;
            _assignmentType = assignmentType;
            _allowSubLists = false;
            _allowTypeCasts = false;
        }

        internal ArgumentListToken(char open, char close, bool allowSubLists = false) {
            _open = open;
            _close = close;
            _findAssignments = false;
            _assignmentType = null;
            _allowSubLists = allowSubLists;
            _allowTypeCasts = false;
        }

        internal ArgumentListToken(bool allowTypeCasts, char open, char close) {
            _open = open;
            _close = close;
            _findAssignments = false;
            _assignmentType = null;
            _allowSubLists = false;
            _allowTypeCasts = allowTypeCasts;
        }

        internal override bool TryGetToken(ref string text, out TokenBase token, bool requireReturnValue = true) {
            token = null;
            var list = new List<TokenBase>();
            List<string> split;
            string temp = text;
            if (!TrySplitByCommas(ref temp, _open, _close, out split)) return false;
            foreach (string str in split) {
                TokenBase newToken;
                string s = str.Trim();
                if (_allowSubLists && s.StartsWith(_open.ToString()) && s.EndsWith(_close.ToString())) {
                    if (new ArgumentListToken(_open, _close).TryGetToken(ref s, out newToken)) list.Add(newToken);
                    else return false;
                } else if (_findAssignments) {
                    if (new LambdaAssignmentToken(_assignmentType).TryGetToken(ref s, out newToken)) list.Add(newToken);
                    else return false;
                } else if (_allowTypeCasts) {
                    if (new TypeCastToken(false).TryGetToken(ref s, out newToken)) {
                        string nameTemp = "$" + s;
                        TokenBase tokenTemp;
                        if (!new ParameterToken().TryGetToken(ref nameTemp, out tokenTemp) || !String.IsNullOrWhiteSpace(nameTemp)) return false;
                        (newToken as TypeCastToken).Target = tokenTemp;
                        list.Add(newToken);
                    } else {
                        string nameTemp = "$" + s;
                        TokenBase tokenTemp;
                        if (!new ParameterToken().TryGetToken(ref nameTemp, out tokenTemp) || !String.IsNullOrWhiteSpace(nameTemp)) return false;
                        list.Add(tokenTemp);
                    }
                } else {
                    if (EquationTokenizer.TryEvaluateExpression(str.Trim(), out newToken)) list.Add(newToken);
                    else return false;
                }
            }
            token = new ArgumentListToken('\0', '\0') { Arguments = list.ToArray() };
            text = temp;
            return true;
        }

        internal override Expression GetExpression(List<ParameterExpression> parameters, Dictionary<string, ConstantExpression> locals, List<DataContainer> dataContainers, Type dynamicContext, LabelTarget label, bool requiresReturnValue = true) {
            throw new NotImplementedException();
        }
    }
}