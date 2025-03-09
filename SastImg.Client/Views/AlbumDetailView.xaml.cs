using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SastImg.Client.Views
{
    public sealed partial class AlbumDetailView : Page
    {
        public AlbumDetailViewModel ViewModel = new();
        public string filePath;
        public AlbumDetailView()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (App.Shell.MainFrame.CanGoBack)
            {
                App.Shell.MainFrame.GoBack();
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.AlbumData = e.Parameter as Album;
        }
    }
}
