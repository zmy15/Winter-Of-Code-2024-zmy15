using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using SastImg.Client.Helpers;
using SastImg.Client.Services;
using SastImg.Client.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SastImg.Client;

public partial class App : Application
{
    public App ( )
    {
        this.InitializeComponent();
        API = new SastImgAPI("http://sastwoc2024.shirasagi.space:5263/");
    }

    protected override void OnLaunched (Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        MainWindow = new Window()
        {
            SystemBackdrop = new MicaBackdrop(),
            Title = "SAST Image",
            Content = new ShellPage()
        };
        MainWindow.AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;
        MainWindow.AppWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
        MainWindow.Activate();
        WindowHelper.TrackWindow(MainWindow);
    }

    public static Window? MainWindow;
    public static SastImgAPI? API;
    public static AuthService AuthService = new();
}
