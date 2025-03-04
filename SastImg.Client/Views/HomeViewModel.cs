using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;
using SastImg.Client.Service.API;
using SastImg.Client.Services;
using System.Collections.Generic;
using System.Windows.Input;
using System.IO;
using System.Drawing;

namespace SastImg.Client.Views;

public class HomeViewModel : ObservableObject
{
public HomeViewModel()
    {

    }

    private RelayCommand getImage;
    public ICommand GetImage => getImage ??= new RelayCommand(PerformGetImage);

    private async void PerformGetImage()
    {
        //await App.ImageService.GetImageThumbnail();
    }
}