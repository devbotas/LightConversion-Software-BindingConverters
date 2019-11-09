// Copyright 2019 Light Conversion, UAB
// Licensed under the Apache 2.0, see LICENSE.md for more details.

using System.Threading;

namespace LightConversion.Software.BindingConverters.QuickConverter {
    public class DataContainer {
        private readonly ThreadLocal<object> _value = new ThreadLocal<object>();
        public object Value {
            get { return _value.Value; }
            set { _value.Value = value; }
        }
    }
}