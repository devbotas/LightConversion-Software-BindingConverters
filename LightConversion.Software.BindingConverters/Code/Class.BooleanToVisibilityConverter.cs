// Copyright 2019 Light Conversion, UAB
// Licensed under the Apache 2.0, see LICENSE.md for more details.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LightConversion.Software.BindingConverters {
    /// <summary>
    /// Performs operation 'AND' on the input boolean array.
    /// </summary>
    /// <remarks></remarks>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : IValueConverter {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var localParameter = true;
            if (parameter != null) localParameter = System.Convert.ToBoolean(parameter);
            var input = System.Convert.ToBoolean(value);

            if ((input == true) && (localParameter == true)) return Visibility.Visible;
            if ((input == false) && (localParameter == true)) return Visibility.Collapsed;
            if ((input == true) && (localParameter == false)) return Visibility.Collapsed;

            return Visibility.Visible;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotSupportedException("ConvertBack should never be called");
        }
    }
}