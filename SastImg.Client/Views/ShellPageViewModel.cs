using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SastImg.Client.Views.Dialogs;

namespace SastImg.Client.Views;
public partial class ShellPageViewModel : ObservableObject
{
    const string DefaultUsername =  "Not logged in";

    public ShellPageViewModel ( )
    {
        App.AuthService.LoginStateChanged += OnLoginStatusChanged;
    }

    public string Username => IsLoggedIn ? (App.AuthService.Username ?? DefaultUsername) : DefaultUsername;

    public bool IsLoggedIn => App.AuthService.IsLoggedIn;

    public void OnLoginStatusChanged (bool isLogin, string? username)
    {
        OnPropertyChanged(nameof(IsLoggedIn));
        OnPropertyChanged(nameof(Username));
    }

    public ICommand LoginCommand => new RelayCommand(async ( ) =>
    {
        var dialog = new LoginDialog();
        await dialog.ShowAsync();
    });

    public ICommand LogoutCommand => new RelayCommand(( ) =>
    {
        App.AuthService.Logout();
    });
}
