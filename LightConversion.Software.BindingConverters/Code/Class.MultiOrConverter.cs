// Copyright 2019 Light Conversion, UAB
// Licensed under the Apache 2.0, see LICENSE.md for more details.

using System;
using System.Windows.Data;

namespace LightConversion.Software.BindingConverters {
    /// <summary>
    /// Performs operation 'OR' on the input boolean array.
    /// </summary>
    /// <remarks></remarks>
    [ValueConversion(typeof(bool[]), typeof(bool))]
    public class MultiOrConverter : IMultiValueConverter {
        /// <inheritdoc/>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            var temp = false;
            foreach (bool value in values) {
                temp = temp || value;
            }
            return temp;
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotSupportedException("ConvertBack should never be called");
        }

    }
}
