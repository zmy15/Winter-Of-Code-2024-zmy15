using Refit;
using SastImg.Client.Service.API;
using SastImg.Client.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SastImg.Client.Services
{
    /// <summary>
    /// 有关图片的服务
    /// </summary>
    public class ImageService
    {
        private string filePath;

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 获取相册下的所有图片
        /// </summary>
        /// <param name="albumId"></param>
        /// <returns></returns>
        public async Task<ICollection<ImageDto>> GetImagesByAlbum(long albumId)
        {
            var response = await App.API!.Image.GetImagesAsync(null, albumId, null);
            if (response.Content != null)
            {
                return response.Content!;
            }
            return new List<ImageDto>();
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="title"></param>
        /// <param name="filePath"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public async Task<bool> UploadImageAsync(long albumId, string title,string filePath, ICollection<long> tags)
        {
            using (var imageStream = File.OpenRead(filePath))
            {
                var image = new StreamPart(imageStream, Path.GetFileName(filePath), "image/png");
                var response = await App.API!.Image.AddImageAsync(albumId: albumId, title: title, image: image, tags: null);
                return response.IsSuccessStatusCode;
            }
        }
        /// <summary>
        /// 获取图片的详细信息
        /// </summary>
        /// <param name="ImageId"></param>
        /// <returns></returns>
        public async Task<DetailedImage> GetDetailedImage(long ImageId)
        {
            var response = await App.API!.Image.GetDetailedImageAsync(ImageId);
            if (response.Content != null)
            {
                return response.Content!;
            }
            return new DetailedImage();
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveImage(long albumId, long imageId)
        {
            var response = await App.API!.Image.RemoveImageAsync(albumId, imageId);
            return response.IsSuccessStatusCode;
        }
        /// <summary>
        /// 喜欢图片
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public async Task<bool> LikeImage(long albumId, long imageId)
        {
            var response = await App.API!.Image.LikeImageAsync(albumId, imageId);
            return response.IsSuccessStatusCode;
        }
        /// <summary>
        /// 取消喜欢图片
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public async Task<bool> UnlikeImage(long albumId, long imageId)
        {
            var response = await App.API!.Image.UnlikeImageAsync(albumId, imageId);
            return response.IsSuccessStatusCode;
        }
    }
}
