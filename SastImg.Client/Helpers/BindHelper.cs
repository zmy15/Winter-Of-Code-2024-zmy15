using Microsoft.UI.Xaml;

namespace SastImg.Client.Helpers;

public static partial class BindHelper
{
    public static bool Not (bool value) // x:Bind 时对值取非使用
    {
        return !value;
    }

    public static Visibility BoolToVisibility (bool value, bool inversed) // x:Bind 时对值转换为 Visibility 使用
    {
        return inversed ? (value ? Visibility.Collapsed : Visibility.Visible) : (value ? Visibility.Visible : Visibility.Collapsed);
    }
}
