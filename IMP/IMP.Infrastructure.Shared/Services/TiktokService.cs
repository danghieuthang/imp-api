using System.Text;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using IMP.Application.Interfaces;
using IMP.Domain.SocialPlatforms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace IMP.Infrastructure.Shared.Services
{
    public class TiktokService : ITiktokService
    {
        private readonly string _endpoint;
        public TiktokService(IConfiguration _config)
        {
            _endpoint = _config["SocialAnalysisUrl"];
        }

        public async Task<SocialPlatformUser> VerifyUser(string username, string hashtag)
        {
            string verifyEndpoint = _endpoint + "tiktok/verify-user";
            var uri = new Uri(verifyEndpoint);

            var client = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = uri;

            //create body
            var body = "{\"username\":\"" + username + "\", \"hash_tag\": \"" + hashtag + "\"}";
            var stringContent = new StringContent(body, Encoding.UTF8, "application/json");

            httpRequestMessage.Content = stringContent;

            var response = await client.SendAsync(httpRequestMessage);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic socialUser = JsonConvert.DeserializeObject(responseContent);
                if (socialUser["success"] == true)
                {
                    var socialPlatformUser = new SocialPlatformUser
                    {
                        Name = socialUser["data"]["user"]["nickname"],
                        Avatar = socialUser["data"]["user"]["avatarThumb"],
                        Follower = socialUser["data"]["stats"]["followerCount"]
                    };
                    return socialPlatformUser;
                }
            }
            return null;
        }
    }
}