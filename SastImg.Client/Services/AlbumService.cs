using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SastImg.Client.Services
{
    public class AlbumService
    {
        public AlbumService() 
        {

        }
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
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
