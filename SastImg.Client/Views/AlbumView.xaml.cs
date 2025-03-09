using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SastImg.Client.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AlbumView : Page
    {
        public AlbumViewModel ViewModel { get; set; }
        public AlbumView()
        {
            ViewModel = new AlbumViewModel();
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }
        private void ScrollViewer_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (scrollViewer?.ScrollableWidth > 0)
            {
                // 获取鼠标滚轮的滚动增量（正值或负值）
                var delta = e.GetCurrentPoint(scrollViewer).Properties.MouseWheelDelta;
                scrollViewer.ChangeView(scrollViewer.HorizontalOffset - delta, null, null, true);
                e.Handled = true;
            }
        }
    }
}
