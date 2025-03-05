using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SastImg.Client.Controls
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        // 将 bool 值转换为 Visibility
        // 如果值为 true，则返回 Visible；否则返回 Collapsed
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool flag)
            {
                return flag ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        // 将 Visibility 转换回 bool
        // 如果为 Visible，则返回 true；否则返回 false
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            return false;
        }
    }
}
