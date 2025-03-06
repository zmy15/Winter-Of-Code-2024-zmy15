using SastImg.Client.Service.API;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SastImg.Client.Services
{
    /// <summary>
    /// 有关相册的服务
    /// </summary>
    public class AlbumService
    {
        public AlbumService()
        {

        }
        /// <summary>
        /// 创建相册
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Description"></param>
        /// <param name="CategoryId"></param>
        /// <param name="AccessLevel"></param>
        /// <returns></returns>
        public static async Task<bool> CreateAlbumAsync(string Title, string Description, long CategoryId, string AccessLevel)
        {
            var AccessLevelint = AccessLevel switch
            {
                "仅作者" => 0,
                "管理员可读" => 1,
                "管理员可写" => 2,
                "所有人可读" => 3,
                "所有人可写" => 4,
                _ => 0,
            };
            try
            {
                var response = await App.API!.Album.CreateAlbumAsync(new() { Title = Title, Description = Description, CategoryId = CategoryId, AccessLevel = AccessLevelint });
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 获取相册信息
        /// </summary>
        /// <param name="AlbumId"></param>
        /// <returns></returns>
        public async Task<DetailedAlbum> GetAlbumDetail(long AlbumId)
        {
            var response = await App.API!.Album.GetDetailedAlbumAsync(AlbumId);
            if (response.IsSuccessStatusCode)
            {
                return response.Content!;
            }
            return new DetailedAlbum();
        }
        /// <summary>
        /// 获取一个分类下的所有相册
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<ICollection<AlbumDto>> GetAlbumByCategories(long categoryId)
        {
            var response = await App.API!.Album.GetAlbumsAsync(categoryId, null, null);
            if (response.Content != null)
            {
                return response.Content!;
            }
            return new List<AlbumDto>();
        }
        /// <summary>
        /// 更新相册的访问权限
        /// </summary>
        /// <param name="AlbumId"></param>
        /// <param name="AccessLevel"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAccessLevel​(long AlbumId, int AccessLevel)
        {
            var accessLevel = new UpdateAccessLevelRequest() { AccessLevel = AccessLevel };
            var response = await App.API!.Album.UpdateAlbumAccessLevelAsync(AlbumId, accessLevel);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 更新相册的描述
        /// </summary>
        /// <param name="AlbumId"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        public async Task<bool> UpdateDescription​(long AlbumId, string Description)
        {
            var description = new UpdateDescriptionRequest() { Description = Description };
            var response = await App.API!.Album.UpdateAlbumDescriptionAsync(AlbumId, description);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 更新相册的标题
        /// </summary>
        /// <param name="AlbumId"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        public async Task<bool> UpdateTitle​(long AlbumId, string Title)
        {
            var title = new UpdateTitleRequest() { Title = Title };
            var response = await App.API!.Album.UpdateAlbumTitleAsync(AlbumId, title);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 更新相册的信息
        /// </summary>
        /// <param name="AlbumId"></param>
        /// <param name="AccessLevel"></param>
        /// <param name="Description"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAlbumInfo(long AlbumId, int? AccessLevel, string? Description, string? Title)
        {
            if (AccessLevel != null)
            {
                await UpdateAccessLevel(AlbumId, (int)AccessLevel);
            }
            if (Description != null)
            {
                await UpdateDescription(AlbumId, Description);
            }
            if (Title != null)
            {
                await UpdateTitle(AlbumId, Title);
            }
            return true;
        }
        /// <summary>
        /// 移除相册
        /// </summary>
        /// <param name="AlbumId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAlbum(long AlbumId)
        {
            var response = await App.API!.Album.RemoveAlbumAsync(AlbumId);
            return response.IsSuccessStatusCode;
        }

    }
}
