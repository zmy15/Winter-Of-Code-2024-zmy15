using Refit;
using SastImg.Client.Service.API;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SastImg.Client.Services
{
    public class ImageService
    {
        private static long _id;
        private long _uploaderId;
        private long _albumId;
        private string _title;
        private ICollection<long>? _tags;
        private DateTimeOffset _uploadedAt;
        private DateTimeOffset? _removedAt;
        private string filePath;

        public async Task<bool> DownloadImage(long? id, int kind)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            if (id != null)
            {
                if (kind == 1)
                {
                    filePath = localFolder.Path + $"\\{id}_Thumbnail.png";
                }
                else
                {
                    filePath = localFolder.Path + $"\\{id}.png";
                }
                if (File.Exists(filePath))
                {
                    return true;
                }
                var response = await App.API!.Image.GetImageAsync((long)id, kind);
                if (response.IsSuccessStatusCode && response.Content != null)
                {
                    // 创建文件流并写入从 API 获取的内容
                    using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                    await response.Content.CopyToAsync(fileStream);
                    return true;
                }
                return false;
            }
            return false;
        }
        public async Task<bool> RemoveImageAsync()
        {
            var response = await App.API!.Image.RemoveImageAsync(_albumId, _id);
            return response.IsSuccessStatusCode;
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
        public async Task<bool> UploadImageAsync(long albumId, string title,string filePath, ICollection<long> tags)
        {
            using (var imageStream = File.OpenRead(filePath))
            {
                var image = new StreamPart(imageStream, Path.GetFileName(filePath), "image/png");
                var response = await App.API!.Image.AddImageAsync(albumId: albumId, title: title, image: image, tags: null);
                return response.IsSuccessStatusCode;
            }
        }
    }
}
