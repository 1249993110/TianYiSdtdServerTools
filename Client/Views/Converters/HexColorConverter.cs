using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TianYiSdtdServerTools.Client.Views.Converters
{
    public class HexColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var str = value.ToString();
            if (str.Length > 0)
            {
                return "[" + str.Substring(1) + "]";
            }
            return "[" + str + "]";
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}