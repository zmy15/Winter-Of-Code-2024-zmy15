using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Animation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SastImg.Client.Views
{
    public sealed partial class ImageView : Page
    {
        public ImageViewModel ViewModel = new();
        public ImageView()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
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
            ViewModel.ImageData = e.Parameter as ImageItem;
        }
        private void RightPanel_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            // 尝试通过 Window.Current.Content 获取根元素
            FrameworkElement rootElement = Window.Current?.Content as FrameworkElement;
            double targetWidth = 0;
            double targetHeight = 0;

            if (rootElement != null)
            {
                targetWidth = rootElement.ActualWidth;
                targetHeight = rootElement.ActualHeight;
            }
            else
            {
                // 若 Window.Current.Content 为 null，则使用页面的根布局
                targetWidth = LayoutRoot.ActualWidth;
                targetHeight = LayoutRoot.ActualHeight;
            }

            // 使用 Image 控件的实际尺寸作为原始尺寸
            double originalWidth = ImageControl.ActualWidth;
            double originalHeight = ImageControl.ActualHeight;
            if (originalWidth == 0 || originalHeight == 0)
            {
                // 若图片尚未完成测量，则退出或延迟处理
                return;
            }

            // 计算宽度与高度方向的缩放因子
            double scaleX = targetWidth / originalWidth;
            double scaleY = targetHeight / originalHeight;
            // 取较小值确保不会超出边框
            double scale = Math.Min(scaleX, scaleY);

            // 创建 Storyboard 动画，等比缩放 ScaleX 与 ScaleY
            var sb = new Storyboard();

            var animX = new DoubleAnimation()
            {
                To = scale,
                Duration = new Duration(TimeSpan.FromMilliseconds(300))
            };
            Storyboard.SetTarget(animX, RightPanelTransform);
            Storyboard.SetTargetProperty(animX, "ScaleX");

            var animY = new DoubleAnimation()
            {
                To = scale,
                Duration = new Duration(TimeSpan.FromMilliseconds(300))
            };
            Storyboard.SetTarget(animY, RightPanelTransform);
            Storyboard.SetTargetProperty(animY, "ScaleY");

            sb.Children.Add(animX);
            sb.Children.Add(animY);
            sb.Begin();
        }

        private void RightPanel_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            // 鼠标移出时，恢复等比缩放因子为 1
            var sb = new Storyboard();

            var animX = new DoubleAnimation()
            {
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(300))
            };
            Storyboard.SetTarget(animX, RightPanelTransform);
            Storyboard.SetTargetProperty(animX, "ScaleX");

            var animY = new DoubleAnimation()
            {
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(300))
            };
            Storyboard.SetTarget(animY, RightPanelTransform);
            Storyboard.SetTargetProperty(animY, "ScaleY");

            sb.Children.Add(animX);
            sb.Children.Add(animY);
            sb.Begin();
        }
    }
}
