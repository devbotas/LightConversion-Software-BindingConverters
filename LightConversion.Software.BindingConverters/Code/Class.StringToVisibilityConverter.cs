// Copyright 2019 Light Conversion, UAB
// Licensed under the Apache 2.0, see LICENSE.md for more details.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LightConversion.Software.BindingConverters {
    /// <summary>
    /// Convert string to visibility. When string is not empty or null return Visible, otherwise Collapsed.
    /// </summary>
    public class StringToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value != null) {
                var input = (string)value;
                if (!string.IsNullOrEmpty(input)) return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}