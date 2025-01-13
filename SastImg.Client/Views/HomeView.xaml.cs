using Microsoft.UI.Xaml.Controls;

namespace SastImg.Client.Views;

public sealed partial class HomeView : Page
{
    public HomeViewModel ViewModel;
    public HomeView ( )
    {
        ViewModel = new HomeViewModel();
        this.InitializeComponent();
    }
}
