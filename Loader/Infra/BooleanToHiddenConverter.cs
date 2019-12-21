namespace Components.Infra
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    internal class BooleanToHiddenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool v && v)
            {
                return Visibility.Visible;
            }

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility v && v == Visibility.Hidden)
            {
                return false;
            }

            return true;
        }
    }
}
