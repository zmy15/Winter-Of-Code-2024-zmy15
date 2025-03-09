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
using System.Xml.Linq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

        [ObservableProperty]
        private long _albumId;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Album other = (Album)obj;
            return AlbumName == other.AlbumName && Thumbnail == other.Thumbnail && PhotoCount == other.PhotoCount && AlbumId == other.AlbumId;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(AlbumName, Thumbnail, PhotoCount, AlbumId);
        }
    }

    /// <summary>
    /// 相册分类容器
    /// </summary>
    public class AlbumCategory : ObservableObject
    {
        public string CategoryName { get; set; }
        public string CategoryDescription​ { get; set; }
        public ObservableCollection<Album> Albums { get; set; }
        public long CategoryId { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            AlbumCategory other = (AlbumCategory)obj;
            return CategoryId == other.CategoryId &&
                   AlbumsEquals(Albums, other.Albums);
        }

        public bool AlbumsEquals(ObservableCollection<Album> albums1, ObservableCollection<Album> albums2)
        {
            if (albums1 == null || albums2 == null)
            {
                return albums1 == albums2;
            }
            if (albums1.Count != albums2.Count)
            {
                return false;
            }
            for (int i = 0; i < albums1.Count; i++)
            {
                if (!albums1[i].Equals(albums2[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            int hash = HashCode.Combine(CategoryName, CategoryDescription, CategoryId);
            foreach (var album in Albums)
            {
                hash = HashCode.Combine(hash, album.GetHashCode());
            }
            return hash;
        }
    }
    public partial class AlbumViewModel : ObservableObject
    {
        // 分类数据集合
        [ObservableProperty]
        public ObservableCollection<AlbumCategory> _albumCategories = new();

        [ObservableProperty]
        private bool _isLoading;

        public AlbumViewModel()
        {
            InitializeAsync();
        }
        /// <summary>
        /// 初始化分类相册数据
        /// </summary>
        /// <returns></returns>
        public async void InitializeAsync()
        {
            var categories = await App.CategoryService.GetCategories();
            var comparer = new AlbumCategoryComparer();
            foreach (var c in categories)
            {
                AlbumCategories.Add(c);
            }
        }
        /// <summary>
        /// 创建相册命令
        /// </summary>
        public ICommand CreateAlbumCommand => new RelayCommand<AlbumCategory>(async (category) =>
        {
            var dialog = new CreateAlbumDialog(category!.CategoryId);
            await dialog.ShowAsync();

            var categories = await App.CategoryService.GetCategories();
            var comparer = new AlbumCategoryComparer();
            foreach (var c in categories)
            {
                UpdateAlbumCategories(c, comparer);
            }
        });
        /// <summary>
        /// 打开相册详情页命令
        /// </summary>
        public ICommand NavigateToAlbumDetailCommand => new RelayCommand<Album>((album) =>
        {
            App.Shell!.MainFrame.Navigate(typeof(AlbumDetailView), album);
        });
        public void UpdateAlbumCategories(AlbumCategory updatedCategory, IEqualityComparer<AlbumCategory> comparer)
        {
            for (int i = 0; i < AlbumCategories.Count; i++)
            {
                if (AlbumCategories[i].CategoryId == updatedCategory.CategoryId)
                {
                    if (!AlbumCategories[i].AlbumsEquals(updatedCategory.Albums, AlbumCategories[i].Albums))
                    {
                        AlbumCategories.Remove(AlbumCategories[i]);
                        AlbumCategories.Insert(i, updatedCategory);
                        return;
                    }
                }
            }
        }
    }
    public class AlbumCategoryComparer : IEqualityComparer<AlbumCategory>
    {
        public bool Equals(AlbumCategory x, AlbumCategory y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(AlbumCategory obj)
        {
            return obj.GetHashCode();
        }
    }
}