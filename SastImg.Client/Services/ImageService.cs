using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public async Task<bool> DownloadAllImagesAsync()
        {
            var response = await App.API!.Image.GetImagesAsync(null,null,null);
            if (response.Content != null)
            {
                foreach (var item in response.Content!)
                {
                    if (item != null)
                    {
                        _id = item.Id;
                        _albumId = item.AlbumId;
                        await GetImageAsync();
                    }
                }
                return true;
            }
            return false;
        }
        public async Task<bool> GetImageAsync()
        {
            string filePath = $"F:\\C#\\Winter-Of-Code-2024-zmy15\\image\\{_id}.png";
            var response = await App.API!.Image.GetImageAsync(_id, 0);
            if (response.IsSuccessStatusCode && response.Content != null)
            {
                // 创建文件流并写入从 API 获取的内容
                using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                await response.Content.CopyToAsync(fileStream);
                return true;
            }
            return false;
        }
        public async Task<bool> RemoveImageAsync()
        {
            var response = await App.API!.Image.RemoveImageAsync(_albumId, _id);
            return response.IsSuccessStatusCode;
        }
    }
}
