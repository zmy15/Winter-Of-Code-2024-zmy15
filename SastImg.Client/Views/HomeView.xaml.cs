using Microsoft.UI.Xaml.Controls;

namespace SastImg.Client.Views;

public sealed partial class HomeView : Page
{
    public HomeViewModel ViewModel { get; set; }
    public HomeView ( )
    {
        ViewModel = new HomeViewModel();
        this.InitializeComponent();
        this.DataContext = ViewModel;
    }
}
