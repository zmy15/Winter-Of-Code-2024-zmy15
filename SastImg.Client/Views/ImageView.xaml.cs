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
            // ����ͨ�� Window.Current.Content ��ȡ��Ԫ��
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
                // �� Window.Current.Content Ϊ null����ʹ��ҳ��ĸ�����
                targetWidth = LayoutRoot.ActualWidth;
                targetHeight = LayoutRoot.ActualHeight;
            }

            // ʹ�� Image �ؼ���ʵ�ʳߴ���Ϊԭʼ�ߴ�
            double originalWidth = ImageControl.ActualWidth;
            double originalHeight = ImageControl.ActualHeight;
            if (originalWidth == 0 || originalHeight == 0)
            {
                // ��ͼƬ��δ��ɲ��������˳����ӳٴ���
                return;
            }

            // ��������߶ȷ������������
            double scaleX = targetWidth / originalWidth;
            double scaleY = targetHeight / originalHeight;
            // ȡ��Сֵȷ�����ᳬ���߿�
            double scale = Math.Min(scaleX, scaleY);

            // ���� Storyboard �������ȱ����� ScaleX �� ScaleY
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
            // ����Ƴ�ʱ���ָ��ȱ���������Ϊ 1
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
