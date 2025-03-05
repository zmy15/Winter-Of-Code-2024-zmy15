using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace SastImg.Client.Controls
{
    public class BooleanToBrushConverter : IValueConverter
    {
        // 当 IsChecked 为 true 时返回红色，否则返回灰色
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isChecked = false;
            if (value is bool)
            {
                isChecked = (bool)value;
            }
            return isChecked ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
