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
                        var AlbumInfo = await App.AlbumService.GetAlbumByCategories(item.Id);
                        var albumCollection = new ObservableCollection<Album>();
                        foreach (var album in AlbumInfo)
                        {
                            var ImageInfo = await App.ImageService.GetImagesByAlbum(album.Id);
                            var id = ImageInfo.FirstOrDefault()?.Id;
                            string filePath;
                            if (id != null)
                            {
                                filePath = localFolder.Path + $"\\{id}_Thumbnail.png";
                                await App.ImageService.DownloadImage(id, 1);
                            }
                            else
                            {
                                filePath = "ms-appx:///Assets/NoImage.png";
                            }
                            albumCollection.Add(new Album
                            {
                                AlbumName = album.Title,
                                PhotoCount = ImageInfo.Count,
                                Thumbnail = filePath,
                                AlbumId = album.Id
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
