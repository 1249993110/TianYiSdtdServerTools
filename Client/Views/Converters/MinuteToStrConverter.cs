using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TianYiSdtdServerTools.Client.Views.Converters
{
    public class MinuteToStrConverter : IValueConverter
    {
        private static string MinuteToStr(double minute)
        {
            double hour = Math.Floor(minute / 60);
            minute -= hour * 60;
            return (hour > 0 ? hour + "小时" : string.Empty) + minute + "分钟";
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return MinuteToStr((int)value);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
