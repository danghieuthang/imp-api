using IMP.Application.Interfaces.Shared;
using IMP.Application.Models.Account;
using IMP.Domain.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZaloDotNetSDK;

namespace IMP.Infrastructure.Shared.Services
{
    public class ZaloService : IZaloService
    {
        private readonly ZaloSettings _zaloSettings;
        public ZaloService(IOptions<ZaloSettings> options)
        {
            _zaloSettings = options.Value;
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

        public async Task<bool> SendMessageAsync(string clientAccessToken)
        {
            var user = await ValidationAccessToken(clientAccessToken);
            if (user == null)
            {
                return false;
            }
            ZaloClient client = new ZaloClient(_zaloSettings.AccessToken);
            JObject resultQuantam = client.getListFollower(0, 20);

            JObject result = client.sendTextMessageToUserId(user.ProviderUserId, "This is message");
            return true;
        }

        public async Task<string> GetLoginUrlAsync(string callBackUrl)
        {
            ZaloAppInfo appInfo = new ZaloAppInfo(_zaloSettings.AppId, _zaloSettings.SecretKey, callBackUrl);
            ZaloAppClient appClient = new ZaloAppClient(appInfo);
            return appClient.getLoginUrl();
        }
    }
}
