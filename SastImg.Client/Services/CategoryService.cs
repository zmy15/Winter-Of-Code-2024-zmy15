using SastImg.Client.Service.API;
using SastImg.Client.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SastImg.Client.Services
{
    public class CategoryService
    {
        List<AlbumCategory> AlbumCategories = new();
        public CategoryService() 
        { 

        }
        public async Task<ICollection<AlbumDto>> GetAlbumByCategories(long categoryId)
        {
            var response = await App.API!.Album.GetAlbumsAsync(categoryId, null, null);
            if (response.Content != null)
            {
                return response.Content!;
            }
            return new List<AlbumDto>();
        }
        public async Task<ICollection<ImageDto>> GetImagesByAlbum(long albumId)
        {
            var response = await App.API!.Image.GetImagesAsync(null, albumId, null);
            if (response.Content != null)
            {
                return response.Content!;
            }
            return new List<ImageDto>();
        }
        public async Task<List<AlbumCategory>> GetCategories()
        {
            AlbumCategories.Clear();
            var localFolder = ApplicationData.Current.LocalFolder;
            var response = await App.API!.Category.GetCategoryAsync();
            if (response.Content != null)
            {
                foreach (var item in response.Content!)
                {
                    if (item != null)
                    {
                        var AlbumInfo = await GetAlbumByCategories(item.Id);
                        var albumCollection = new ObservableCollection<Album>();
                        foreach (var album in AlbumInfo)
                        {
                            var ImageInfo = await GetImagesByAlbum(album.Id);
                            var id = ImageInfo.FirstOrDefault()?.Id;
                            string filePath;
                            if (id != null)
                            {
                                filePath = localFolder.Path + $"\\{id}.png";
                                await App.ImageService.GetImageThumbnail(id);
                            }
                            else
                            {
                                filePath = "ms-appx:///Assets/NoImage.png";
                            }
                            albumCollection.Add(new Album
                            {
                                AlbumName = album.Title,
                                PhotoCount = ImageInfo.Count,
                                Thumbnail = filePath
                            });
                        }
                        var category = new AlbumCategory
                        {
                            CategoryName = item.Name,
                            CategortId = item.Id,
                            Albums = albumCollection
                        };
                        AlbumCategories.Add(category);
                    }
                }
            }
            return AlbumCategories;
        }
    }
}
