using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using SastImg.Client.Helpers;
using SastImg.Client.Service.API;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace SastImg.Client.Views
{
    public partial class AlbumDetailViewModel : ObservableObject
    {
        public AlbumDetailViewModel()
        {

        }

        [ObservableProperty]
        private Album _albumData;

        [ObservableProperty]
        private DetailedAlbum _detailedAlbums = new();

        [ObservableProperty]
        private long _id = 0;
        [ObservableProperty]
        private string _title = "";
        [ObservableProperty]
        private string _imageTitle = "";
        [ObservableProperty]
        private string _description = "";
        [ObservableProperty]
        private long _author = 0;
        [ObservableProperty]
        private long _category = 0;
        [ObservableProperty]
        private string _accessLevel = "";
        [ObservableProperty]
        private DateTimeOffset _createdAt = DateTimeOffset.Now;
        [ObservableProperty]
        private DateTimeOffset _updatedAt = DateTimeOffset.Now;
        [ObservableProperty]
        private ObservableCollection<ImageItem> _images;
        [ObservableProperty]
        public ObservableCollection<Option> _options;

        [ObservableProperty]
        private string _filePath;
        [ObservableProperty]
        private BitmapImage _imageSource;

        [ObservableProperty]
        private string _massage;
        [ObservableProperty]
        private string _color;

        private bool? _isAllChecked;
        public bool? IsAllChecked
        {
            get => _isAllChecked;
            set
            {
                if (_isAllChecked != value)
                {
                    _isAllChecked = value;
                    OnPropertyChanged();
                    SetAllOptionsCheckedState(value);
                }
            }
        }
        partial void OnAlbumDataChanged(Album value)
        {
            Initialize();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void Initialize()
        {
            DetailedAlbums = await App.AlbumService.GetAlbumDetail(AlbumData.AlbumId);
            Id = DetailedAlbums.Id;
            Title = DetailedAlbums.Title;
            Description = DetailedAlbums.Description;
            Author = DetailedAlbums.Author;
            Category = DetailedAlbums.Category;
            var AccessLevelint = DetailedAlbums.AccessLevel;
            AccessLevel = AccessLevelint switch
            {
                0 => "仅作者",
                1 => "管理员可读",
                2 => "管理员可写",
                3 => "所有人可读",
                4 => "所有人可写",
                _ => "仅作者",
            };
            CreatedAt = DetailedAlbums.CreatedAt;
            UpdatedAt = DetailedAlbums.UpdatedAt;

            var response = await App.ImageService.GetImagesByAlbum(Id);
            foreach (var item in response)
            {
                await App.ImageService.DownloadImage(item.Id, 1);
            }
            Images = new ObservableCollection<ImageItem>();
            foreach (var item in response)
            {
                Images.Add(new ImageItem { ImageUrl = ApplicationData.Current.LocalFolder.Path + $"\\{item.Id}_Thumbnail.png" });
            }
            var tags = await App.TagService.GetTagsAsync();
            Options = new ObservableCollection<Option>();
            foreach (var tag in tags)
            {
                Options.Add(new Option { Name = tag.Name ,TagId = tag.Id});
            }
            foreach (var option in Options)
            {
                option.PropertyChanged += Option_PropertyChanged;
            }
        }
        private void Option_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Option.IsChecked))
            {
                UpdateIsAllChecked();
            }
        }
        private void UpdateIsAllChecked()
        {
            int checkedCount = 0;
            int uncheckedCount = 0;

            foreach (var option in Options)
            {
                if (option.IsChecked)
                {
                    checkedCount++;
                }
                else
                {
                    uncheckedCount++;
                }
            }

            if (checkedCount == Options.Count)
            {
                IsAllChecked = true;
            }
            else if (uncheckedCount == Options.Count)
            {
                IsAllChecked = false;
            }
            else
            {
                IsAllChecked = null; // Indeterminate 状态
            }
        }

        private void SetAllOptionsCheckedState(bool? isChecked)
        {
            if (isChecked.HasValue)
            {
                foreach (var option in Options)
                {
                    option.IsChecked = isChecked.Value;
                }
            }
        }
        public ICommand ChangeAlbum => new RelayCommand<Album>(_ =>
        {

        });
        public ICommand PickPhotoCommand => new RelayCommand<Album>(async _ =>
        {
            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            // Get the current window handle
            var window = App.MainWindow;
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            // Open the picker for the user to pick a file
            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                FilePath = file.Path;
                ImageSource = new BitmapImage(new Uri(file.Path));
            }
        });
        public ICommand UploadImageCommand => new RelayCommand<Album>(async (album) =>
        {
            ICollection<long> tagId = [];
            var selectedOptions = Options.Where(o => o.IsChecked).Select(o => o.TagId).ToList();
            foreach (var option in selectedOptions)
            {
                tagId.Add(option);
            }
            if (await App.ImageService.UploadImageAsync(album.AlbumId, ImageTitle, FilePath, tagId))
            {
                Massage = "上传成功";
                Color = "Green";
            }
            else
            {
                Massage = "上传失败";
                Color = "Red";
            }
        });
    }
    public class ImageItem
    {
        public string ImageUrl { get; set; }
    }
}