using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SastImg.Client.Controls
{
    public class AlbumTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AlbumTemplate { get; set; }
        public DataTemplate ButtonTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            return item == null ? ButtonTemplate : AlbumTemplate;
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return SelectTemplateCore(item);
        }
    }
}
