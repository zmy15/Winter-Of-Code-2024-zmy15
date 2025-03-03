﻿using System;
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

        public async Task<bool> GetImageThumbnail(long? id)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            if (id != null)
            {
                string filePath = localFolder.Path + $"\\{id}.png";
                var response = await App.API!.Image.GetImageAsync((long)id, 1);
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
    }
}
