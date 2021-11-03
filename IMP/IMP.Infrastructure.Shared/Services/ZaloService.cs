using IMP.Application.Interfaces.Shared;
using IMP.Application.Models.Account;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Shared.Services
{
    public class ZaloService : IZaloService
    {
        public ZaloService()
        {

        }
        public async Task<ProviderUserDetail> ValidationAccessToken(string accessToken)
        {
            string verifyTokenEndpoint = string.Format("https://graph.zalo.me/v2.0/me?access_token={0}&fields=id,name,picture", accessToken);
            var uri = new Uri(verifyTokenEndpoint);

            var client = new HttpClient();
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                dynamic userObj = JsonConvert.DeserializeObject(content);
                if (userObj["error"] == 0)
                    return new ProviderUserDetail
                    {
                        ProviderUserId = userObj["id"],
                        Avatar = userObj["picture"]["data"]["url"],
                        FirstName = userObj["name"]
                    };
            }
            return null;
        }
    }
}
