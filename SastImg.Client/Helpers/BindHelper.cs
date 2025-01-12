namespace SastImg.Client.Helpers;

public static partial class BindHelper
{
    public static bool Not (bool value) // x:Bind 时对值取非使用
    {
        return !value;
    }
}
