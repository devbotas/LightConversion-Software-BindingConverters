// Copyright 2019 Light Conversion, UAB
// Licensed under the Apache 2.0, see LICENSE.md for more details.

using System;
using System.Windows;
using System.Windows.Data;

namespace LightConversion.Software.BindingConverters {
    /// <summary>
    /// Performs operation 'AND' on the input boolean array, and converts to WPF Visibility structure.
    /// </summary>
    /// <remarks></remarks>
    [ValueConversion(typeof(bool[]), typeof(Visibility))]
    public class MultiAndToVisibilityConverter : IMultiValueConverter {
        /// <inheritdoc/>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            var localParameter = true;
            if (parameter != null) localParameter = System.Convert.ToBoolean(parameter);

            var temp = true;
            foreach (bool value in values) {
                temp = temp && value;
            }

            if ((temp == true) && (localParameter == true)) return Visibility.Visible;
            if ((temp == false) && (localParameter == true)) return Visibility.Collapsed;
            if ((temp == true) && (localParameter == false)) return Visibility.Collapsed;
            return Visibility.Visible;
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotSupportedException("ConvertBack should never be called");
        }

    }
}
