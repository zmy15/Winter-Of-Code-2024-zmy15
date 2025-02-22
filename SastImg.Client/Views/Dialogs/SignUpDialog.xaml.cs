using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SastImg.Client.Views.Dialogs;

[ObservableObject]
public sealed partial class SignUpDialog : ContentDialog
{
    [ObservableProperty]
    private string _username = "";

    [ObservableProperty]
    private string _password = "";

    [ObservableProperty]
    private bool _isSignUp = false;

    [ObservableProperty]
    private bool _isSignUpFailed = false;

    [ObservableProperty]
    private bool _usernameExists = false;

    private CancellationTokenSource? _SignUpCts;

    public SignUpDialog()
    {
        XamlRoot = App.MainWindow?.Content.XamlRoot;
        this.InitializeComponent();
        this.PrimaryButtonClick += SignUpDialog_PrimaryButtonClick;
        this.CloseButtonClick += SignUpDialog_CloseButtonClick;
    }

    private void SignUpDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        _SignUpCts?.Cancel();
    }

    private async void SignUpDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        var deferral = args.GetDeferral();
        _SignUpCts = new();
        IsSignUp = true;
        IsSignUpFailed = false;
        UsernameExists = false;
        try
        {
            var result = await App.API!.Account.CheckUsernameExistenceAsync(Username);
            if (result.IsSuccessStatusCode == false)
            {
                args.Cancel = true;
                IsSignUp = false;
                UsernameExists = true;
            }
            if (await App.AuthService.SignUpAsync(Username, Password))
            {
                // 对话框关闭后显示登陆成功弹窗
                this.Closed += (ContentDialog sender, ContentDialogClosedEventArgs args) =>
                {
                    if (args.Result is not ContentDialogResult.Primary)
                        return;
                    var successDialog = new ContentDialog()
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "注册成功",
                        CloseButtonText = "确定"
                    };
                    var _ = successDialog.ShowAsync();
                };
            }
            else
            {
                // 对话框关闭后显示登陆失败弹窗
                this.Closed += (ContentDialog sender, ContentDialogClosedEventArgs args) =>
                {
                    if (args.Result is not ContentDialogResult.Primary)
                        return;
                    var successDialog = new ContentDialog()
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "注册失败",
                        CloseButtonText = "确定"
                    };
                    var _ = successDialog.ShowAsync();
                };
            }
        }
        catch (System.Exception)
        {
            args.Cancel = true;
            IsSignUp = false;
            IsSignUpFailed = true;
        }
        deferral.Complete();
    }
}
