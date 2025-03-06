using SastImg.Client.Service.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SastImg.Client.Services
{
    /// <summary>
    /// 有关标签的服务
    /// </summary>
    public class TagService
    {
        public TagService() 
        { 

        }
        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<TagDto>> GetTagsAsync()
        {
            var response = await App.API!.Tag.GetTagsAsync(null);
            if (response.IsSuccessStatusCode)
            {
                return response.Content;
            }
            return new List<TagDto>();
        }
    }
}
