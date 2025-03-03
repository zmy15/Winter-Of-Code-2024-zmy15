using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using SastImg.Client.Services;
using SastImg.Client.Service.API;
using Microsoft.UI.Xaml;
using SastImg.Client.Views.Dialogs;
using System.Windows.Input;
using Microsoft.UI.Xaml.Controls;
namespace SastImg.Client.Views
{
    /// <summary>
    /// 相册数据模型（包含视图命令）
    /// </summary>
    public partial class Album : ObservableObject
    {
        // 相册名称
        [ObservableProperty]
        private string _albumName;

        // 缩略图路径/URL
        [ObservableProperty]
        private string _thumbnail;

        // 照片数量
        [ObservableProperty]
        private int _photoCount;
    }

    /// <summary>
    /// 相册分类容器
    /// </summary>
    public class AlbumCategory : ObservableObject
    {
        public string CategoryName { get; set; }
        public ObservableCollection<Album> Albums { get; set; }
        public long CategortId { get; set; }
    }

    /// <summary>
    /// 主视图模型（包含分类数据）
    /// </summary>
    public partial class AlbumViewModel : ObservableObject
    {
        // 分类数据集合
        [ObservableProperty]
        public ObservableCollection<AlbumCategory> _albumCategories = new();

        // 选中的相册（用于绑定）
        [ObservableProperty]
        private Album _selectedAlbum;

        [ObservableProperty]
        private bool _isLoading;

        public AlbumViewModel()
        {
            InitializeAsync().ConfigureAwait(false);
        }

        public async Task InitializeAsync()
        {
            var categories = await App.CategoryService.GetCategories();
            AlbumCategories.Clear();
            foreach (var c in categories)
            {
                AlbumCategories.Add(c);
            }
        }
        public ICommand CreateAlbumCommand => new RelayCommand<AlbumCategory>(async (category) =>
        {
            var dialog = new CreateAlbumDialog(category!.CategortId);
            await dialog.ShowAsync();
        });
        public ICommand NavigateToAlbumDetailCommand => new RelayCommand<Album>(album =>
        {
            
        });
    }
}