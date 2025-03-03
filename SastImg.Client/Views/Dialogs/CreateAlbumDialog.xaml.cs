using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SastImg.Client.Views.Dialogs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [ObservableObject]
    public sealed partial class CreateAlbumDialog : ContentDialog
    {
        [ObservableProperty]
        private string _title = "";

        [ObservableProperty]
        private string _description = "";

        [ObservableProperty]
        private long _categoryId = 1;

        [ObservableProperty]
        private string _accessLevel = "";

        [ObservableProperty]
        private bool _isCreated = false;

        [ObservableProperty]
        private bool _isCreatedFailed = false;

        private CancellationTokenSource? _CreateAlbumCts;

        public CreateAlbumDialog(long CategortId)
        {
            XamlRoot = App.MainWindow?.Content.XamlRoot;
            this.InitializeComponent();
            this.PrimaryButtonClick += CreateAlbumDialog_PrimaryButtonClick;
            this.CloseButtonClick += CreateAlbumDialog_CloseButtonClick;
            CategoryId = CategortId;
        }
        private void CreateAlbumDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            _CreateAlbumCts?.Cancel();
        }

        private async void CreateAlbumDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var deferral = args.GetDeferral();
            _CreateAlbumCts = new();
            IsCreated = true;
            IsCreatedFailed = false;
            try
            {
                
                if (await Services.AlbumService.CreateAlbumAsync(Title, Description, CategoryId, AccessLevel))
                {
                    // �Ի���رպ���ʾ��½�ɹ�����
                    this.Closed += (ContentDialog sender, ContentDialogClosedEventArgs args) =>
                    {
                        if (args.Result is not ContentDialogResult.Primary)
                            return;
                        var successDialog = new ContentDialog()
                        {
                            XamlRoot = this.XamlRoot,
                            Title = "�����ɹ�",
                            CloseButtonText = "ȷ��"
                        };
                        var _ = successDialog.ShowAsync();
                    };
                }
                else
                {
                    // �Ի���رպ���ʾ��½ʧ�ܵ���
                    this.Closed += (ContentDialog sender, ContentDialogClosedEventArgs args) =>
                    {
                        if (args.Result is not ContentDialogResult.Primary)
                            return;
                        var successDialog = new ContentDialog()
                        {
                            XamlRoot = this.XamlRoot,
                            Title = "����ʧ��",
                            CloseButtonText = "ȷ��"
                        };
                        var _ = successDialog.ShowAsync();
                    };
                }
            }
            catch (System.Exception)
            {
                args.Cancel = true;
                IsCreated = false;
                IsCreatedFailed = true;
            }
            deferral.Complete();
        }
    }
}
