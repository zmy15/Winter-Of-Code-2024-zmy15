using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using SastImg.Client.Services;
using SastImg.Client.Service.API;
using Microsoft.UI.Xaml;
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

        /// <summary>
        /// 相册点击命令（CommunityToolkit自动生成RelayCommand）
        /// </summary>
        [RelayCommand]
        private void Selected()
        {
            // 实际处理逻辑应通过消息或服务传递到ViewModel
            System.Diagnostics.Debug.WriteLine($"相册点击: {AlbumName}");
        }
    }

    /// <summary>
    /// 相册分类容器
    /// </summary>
    public class AlbumCategory : ObservableObject
    {
        public string CategoryName { get; set; }
        public ObservableCollection<Album> Albums { get; set; }
    }

    /// <summary>
    /// 主视图模型（包含分类数据）
    /// </summary>
    public partial class AlbumViewModel : ObservableObject
    {
        // 分类数据集合
        [ObservableProperty]
        private ObservableCollection<AlbumCategory> _albumCategories = new();

        // 选中的相册（用于绑定）
        [ObservableProperty]
        private Album _selectedAlbum;

        [ObservableProperty]
        private bool _isLoading;

        [RelayCommand]
        private void AlbumSelected(object parameter)
        {
            if (parameter is Album selectedAlbum)
            {
                // 处理相册选择逻辑
                Debug.WriteLine($"选中相册：{selectedAlbum.AlbumName}");
            }
        }

        public AlbumViewModel()
        {
            InitializeAsync().ConfigureAwait(false);
        }

        public async Task InitializeAsync()
        {
            var categories = await App.CategoryService.GetCategories();
            AlbumCategories.Clear();
            Debug.WriteLine(AlbumCategories.Count);
            foreach (var c in categories)
            {
                AlbumCategories.Add(c);
            }
            Debug.WriteLine(AlbumCategories.Count);
        }

        /// <summary>
        /// 全局相册选择命令（建议通过Messenger实现跨组件通信）
        /// </summary>
        [RelayCommand]
        private void SelectAlbum(Album album)
        {
            SelectedAlbum = album;
            WeakReferenceMessenger.Default.Send(album, "AlbumSelected");
        }
        
    }
}