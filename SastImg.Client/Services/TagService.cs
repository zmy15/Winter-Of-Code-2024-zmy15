using SastImg.Client.Service.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SastImg.Client.Services
{
    public class TagService
    {
        public TagService() 
        { 

        }
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
