using SastImg.Client.Service.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SastImg.Client.Services
{
    public class UserService
    {
        public async Task<UserProfileDto> GetProfile​(long UserId)
        {
            var response = await App.API!.User.GetProfileInfoAsync(UserId);
            if (response.IsSuccessStatusCode)
            {
                return response.Content!;
            }
            return new UserProfileDto();
        }
    }
}