using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TianYiSdtdServerTools.Client.Models.SdtdServerInfo;

namespace TianYiSdtdServerTools.Client.ViewModels.ViewConvert
{
    public class ConnectStateConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = string.Empty;
            ConnectionState connectionState = (ConnectionState)value;
            switch (connectionState)
            {
                case ConnectionState.AutoReconnecting:
                    result = "正在自动重连";
                    break;
                case ConnectionState.Connected:
                    result = "已连接";
                    break;
                case ConnectionState.Connecting:
                    result = "正在连接";
                    break;
                case ConnectionState.Disconnected:
                    result = "未连接";
                    break;
                case ConnectionState.Disconnecting:
                    result = "正在断开";
                    break;
                case ConnectionState.PasswordVerifying:
                    result = "正在验证密码";
                    break;
                case ConnectionState.PasswordIncorrect:
                    result = "密码错误";
                    break;
                default:
                    result = "未知状态";
                    break;
            }
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
