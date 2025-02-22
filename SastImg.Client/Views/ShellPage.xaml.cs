using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.System;

namespace SastImg.Client.Views;
public sealed partial class ShellPage : Page
{
    private ShellPageViewModel vm = new();

    public ShellPage ()
    {
        this.InitializeComponent();
        // ������ʾ��ҳ
        MainFrame.Navigate(typeof(HomeView));
        NavView.SelectedItem = NavView.MenuItems[0];
    }

    private async void NavigationView_ItemInvoked (NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if ( args.InvokedItemContainer is NavigationViewItem item )
        {
            switch ( item.Tag )
            {
                case "Home":
                    MainFrame.Navigate(typeof(HomeView));
                    break;
                case "Album":
                    MainFrame.Navigate(typeof(AlbumView));
                    break;
                case "Settings":
                    MainFrame.Navigate(typeof(SettingsView));
                    break;
                case "GitHub":
                    await Launcher.LaunchUriAsync(new Uri("https://github.com/NJUPT-SAST-Csharp/Winter-Of-Code-2024"));
                    break;
            }
        };
    }
    private void TitleBar_BackButtonClick(object sender, RoutedEventArgs e)
    {
        if (MainFrame.CanGoBack)
        {
            MainFrame.GoBack();
        }
    }
}
