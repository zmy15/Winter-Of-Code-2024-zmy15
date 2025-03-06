using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace SastImg.Client.Views
{
    public partial class ImageViewModel : ObservableObject
    {
        public ImageViewModel()
        {
            // 喜欢图片命令
            IncreaseLikesCommand = new RelayCommand(async () => {
                if(await App.ImageService.LikeImage(ImageAlbumId,ImageData.ImageId))
                {
                    Initialize();
                }
            });
            // 取消喜欢图片命令
            DecreaseLikesCommand = new RelayCommand(async () => {
                if(await App.ImageService.UnlikeImage(ImageAlbumId, ImageData.ImageId))
                {
                    Initialize();
                }
            });
        }
        [ObservableProperty]
        public ImageItem _imageData;
        [ObservableProperty]
        public string _imageTitle = "";
        [ObservableProperty]
        public long _imageId = 0;
        [ObservableProperty]
        public long _imageuploaderId = 0;
        [ObservableProperty]
        public long _imageAlbumId = 0;
        [ObservableProperty]
        public string _imageTagsName = "暂无标签";
        [ObservableProperty]
        public DateTimeOffset _imageUploadedAt = DateTimeOffset.Now;
        [ObservableProperty]
        public int _imageLikes = 0;
        [ObservableProperty]
        private string _imageUrl;

        private bool _isLiked;
        public bool IsLiked
        {
            get => _isLiked;
            set
            {
                if (_isLiked != value)
                {
                    _isLiked = value;
                    OnPropertyChanged();

                    // 执行相应的命令
                    if (_isLiked)
                        IncreaseLikesCommand.Execute(null);
                    else
                        DecreaseLikesCommand.Execute(null);
                }
            }
        }
        public ICommand IncreaseLikesCommand { get; }
        public ICommand DecreaseLikesCommand { get; }

        partial void OnImageDataChanged(ImageItem imageItem)
        {
            Initialize();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// 初始化图片数据
        /// </summary>
        public async void Initialize()
        {
            var detailedImage = await App.ImageService.GetDetailedImage(ImageData.ImageId);
            if (detailedImage == null)
            {
                return;
            }
            await App.ImageService.DownloadImage(ImageData.ImageId, 0);
            var tags = await App.TagService.GetTagsAsync();
            ImageTitle = detailedImage.Title;
            ImageId = detailedImage.Id;
            ImageuploaderId = detailedImage.UploaderId;
            ImageAlbumId = detailedImage.AlbumId;
            ImageUploadedAt = detailedImage.UploadedAt;
            ImageLikes = detailedImage.Likes;
            ImageUrl = ApplicationData.Current.LocalFolder.Path + $"\\{ImageData.ImageId}.png";

            if (detailedImage.Tags != null)
            {
                var ImageTagsName = tags.Where(tag => detailedImage.Tags.Contains(tag.Id))
                            .Select(tag => tag.Name)
                            .DefaultIfEmpty(string.Empty)
                            .Aggregate((current, next) => string.IsNullOrEmpty(current) ? next : current + "," + next);
            }
        }
        /// <summary>
        /// 移除图片命令
        /// </summary>
        public ICommand RemoveImage => new RelayCommand<ImageItem>(async imageItem =>
        {
            if(await App.ImageService.RemoveImage(ImageAlbumId,imageItem.ImageId) && App.Shell.MainFrame.CanGoBack)
            {
                App.Shell.MainFrame.GoBack();
            }
        });
    }
}
