using System;
using System.Globalization;
using System.Xml.Serialization;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace Volunteerio.Converters
{
    class JsonToObjectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return JsonConvert.DeserializeObject((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}
