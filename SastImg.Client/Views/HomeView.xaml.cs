using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SastImg.Client.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class HomeView : Page
{
    public HomeViewModel ViewModel;
    public HomeView ( )
    {
        ViewModel = new HomeViewModel();
        this.InitializeComponent();
    }

    private async void Button_Click (object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        LoginButton.Content = "Try Logging...";
        if ( App.API is { } api )
        {
            var result = await api.Login(new() { Username = "shirasagi", Password = "123456" });
            LoginButton.Content = result.Token;
        }
    }
}
