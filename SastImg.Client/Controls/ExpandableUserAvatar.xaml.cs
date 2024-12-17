using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Labs.WinUI;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using SastImg.Client.Helpers;
using Windows.Foundation;
using Windows.Graphics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SastImg.Client.Controls;
public sealed partial class ExpandableUserAvatar : UserControl
{
    public StartTransitionAction StartAction;
    public StartTransitionAction EndAction;
    private CancellationTokenSource? _startCts;
    public ExpandableUserAvatar ( )
    {
        this.InitializeComponent();

        StartAction = new StartTransitionAction()
        {
            Source = FirstControl,
            Target = SecondControl,
            Transition = this.MyTransitionHelper,
        };

        EndAction = new StartTransitionAction()
        {
            Source = SecondControl,
            Target = FirstControl,
            Transition = this.MyTransitionHelper,
        };

        FirstControl.PointerEntered += async (object sender, PointerRoutedEventArgs e) =>
        {
            Debug.WriteLine("FirstControl.PointerEntered");
            _startCts?.Dispose();
            _startCts = new CancellationTokenSource();

            await Task.Delay(600);
            DispatcherQueue.TryEnqueue(( ) =>
            {
                SetTitleBarInteractivityArea();
            });
            try
            {
                await Task.Delay(100, _startCts.Token);
            }
            catch ( Exception )
            { }
            if ( !_startCts.Token.IsCancellationRequested )
            {
                DispatcherQueue.TryEnqueue(( ) =>
                {
                    ResetTitleBarInteractivityArea();
                    EndAction?.Execute(sender, new());
                });
            }
        };

        SecondControl.PointerEntered += (object sender, PointerRoutedEventArgs e) =>
        {
            Debug.WriteLine("SecondControl.PointerEntered");
            _startCts?.Cancel();
        };

        SecondControl.PointerExited += (object sender, PointerRoutedEventArgs e) =>
        {
            Debug.WriteLine("SecondControl.PointerExited");
            ResetTitleBarInteractivityArea();
        };
    }

    private void SetTitleBarInteractivityArea ( )
    {
        Debug.WriteLine("SetTitleBarInteractivityArea");
        if ( App.MainWindow is Window window )
        {
            var nonClientInputSrc = InputNonClientPointerSource.GetForWindowId(window.AppWindow.Id);
            var interactiveArea = SecondControl as FrameworkElement;
            GeneralTransform transform = interactiveArea.TransformToVisual(null);
            Rect bounds = transform.TransformBounds(new Rect(0, 0, interactiveArea.ActualWidth, interactiveArea.ActualHeight));

            var scale = WindowHelper.GetRasterizationScaleForElement(App.MainWindow.Content);
            var transparentRect = new Windows.Graphics.RectInt32(
                _X: (int)Math.Round(bounds.X * scale),
                _Y: (int)Math.Round(bounds.Y * scale),
                _Width: (int)Math.Round(bounds.Width * scale),
                _Height: (int)Math.Round(bounds.Height * scale)
            );
            var old_rects = nonClientInputSrc.GetRegionRects(NonClientRegionKind.Passthrough);
            if ( FlyoutNonClientArea is not null )
            {
                old_rects = old_rects.Where(old_rects => old_rects != FlyoutNonClientArea).ToArray();
                FlyoutNonClientArea = null;
            }

            if ( !old_rects.Contains(transparentRect) )
            {
                FlyoutNonClientArea = transparentRect;
                nonClientInputSrc.SetRegionRects(NonClientRegionKind.Passthrough, old_rects.Append(transparentRect).ToArray()); // areas defined will be click through and can host button and textboxes
            }
        }
    }

    private void ResetTitleBarInteractivityArea ( )
    {
        Debug.WriteLine("ResetTitleBarInteractivityArea");
        if ( App.MainWindow is Window window )
        {
            if ( FlyoutNonClientArea is not null )
            {
                var nonClientInputSrc = InputNonClientPointerSource.GetForWindowId(window.AppWindow.Id);
                var old_rects = nonClientInputSrc.GetRegionRects(NonClientRegionKind.Passthrough);
                old_rects = old_rects.Where(old_rects => old_rects != FlyoutNonClientArea).ToArray();
                FlyoutNonClientArea = null;
                nonClientInputSrc.SetRegionRects(NonClientRegionKind.Passthrough, old_rects);
            }
        }
    }

    private static RectInt32? FlyoutNonClientArea;

    #region Properties



    public object FlyoutContent
    {
        get { return (object)GetValue(FlyoutContentProperty); }
        set { SetValue(FlyoutContentProperty, value); }
    }

    // Using a DependencyProperty as the backing store for FlyoutContent.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FlyoutContentProperty =
    DependencyProperty.Register("ExpandedContent", typeof(object), typeof(ExpandableUserAvatar), new PropertyMetadata(null));

    public string UserName
    {
        get { return (string)GetValue(UserNameProperty); }
        set { SetValue(UserNameProperty, value); }
    }

    // Using a DependencyProperty as the backing store for UserName.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty UserNameProperty =
        DependencyProperty.Register("UserName", typeof(string), typeof(ExpandableUserAvatar), new PropertyMetadata(null));

    public string Email
    {
        get { return (string)GetValue(EmailProperty); }
        set { SetValue(EmailProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Email.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EmailProperty =
        DependencyProperty.Register("Email", typeof(string), typeof(ExpandableUserAvatar), new PropertyMetadata(null));



    #endregion
}
