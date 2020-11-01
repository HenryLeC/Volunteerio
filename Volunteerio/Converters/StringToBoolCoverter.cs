using System;
using System.Globalization;
using Xamarin.Forms;

namespace Volunteerio
{
    class StringToBoolCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value == "True";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "True" : "False";
        }
    }
}
