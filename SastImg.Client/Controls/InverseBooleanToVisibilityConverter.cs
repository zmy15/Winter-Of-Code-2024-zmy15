using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SastImg.Client.Controls
{
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        // 将 bool 值反向转换为 Visibility
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
            {
                // 当为 true 时返回 Collapsed，false 返回 Visible
                return boolValue ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Visible;
        }

        // 可选实现 ConvertBack，将 Visibility 反向转换为 bool 值
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Visibility visibility)
            {
                return visibility != Visibility.Visible;
            }
            return false;
        }
    }
}
