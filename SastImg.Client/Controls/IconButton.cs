using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SastImg.Client.Controls;
public sealed class IconButton : Button
{
    public IconButton ( )
    {
        this.DefaultStyleKey = typeof(IconButton);
    }



    public IconElement Icon
    {
        get { return (IconElement)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IconProperty =
    DependencyProperty.Register("Icon", typeof(IconElement), typeof(IconButton), new PropertyMetadata(null));


}
