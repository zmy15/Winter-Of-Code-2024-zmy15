using Microsoft.UI.Xaml.Data;
using SastImg.Client.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SastImg.Client.Controls
{
    public class AppendButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is IEnumerable<object> items)
            {
                var list = items.ToList();

                list.Add(null); // 使用null作为创建按钮的标识
                return list;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
