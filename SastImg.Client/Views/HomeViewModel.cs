using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;
using SastImg.Client.Service.API;
using SastImg.Client.Services;
using System.Collections.Generic;
using System.Windows.Input;
using System.IO;
using Windows.Storage;
using System.Diagnostics;

namespace SastImg.Client.Views;

public partial class HomeViewModel : ObservableObject
{
    public HomeViewModel()
    {

    }
    [ObservableProperty]
    private string _massage;
    [ObservableProperty]
    private string _color;

    public ICommand ClearCache => new RelayCommand(async () => {
        if(await DeleteAllFilesInLocalFolderAsync())
        {
            Massage = "清理成功";
            Color = "Green";
        }
        else
        {
            Massage = "清理失败";
            Color = "Red";
        }
    });
    public static async Task<bool> DeleteAllFilesInLocalFolderAsync()
    {
        try
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            var files = await localFolder.GetFilesAsync();
            foreach (var file in files)
            {
                await file.DeleteAsync();
            }
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"删除文件时发生错误: {ex.Message}");
            return false;
        }
    }
}