using IMP.Application.Interfaces.Services;
using IMP.Application.Models.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Shared.Services
{
    public class FacebookAnalysisService : IFacebookAnalysisService
    {
        private readonly string _endpoint;
        public FacebookAnalysisService(IConfiguration configuration)
        {
            _endpoint = configuration["SocialAnalysisUrl"];
        }

        public async Task<SocialContent> GetPost(string postUrl)
        {
            string urlEndpoint = _endpoint + $"facebook/get-post?url={postUrl}";
            var uri = new Uri(urlEndpoint);

            var client = new HttpClient();

            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                dynamic fbContentObj = JsonConvert.DeserializeObject(content);
                if (fbContentObj["success"] == true)
                {
                    SocialContent socialContent = new SocialContent();
                    socialContent.Content = fbContentObj["data"]["text"];
                    socialContent.Shares = fbContentObj["data"]["shares"];
                    socialContent.Likes = fbContentObj["data"]["likes"];
                    socialContent.Comments = fbContentObj["data"]["comments"];
                    socialContent.Hashtags = GetHashtagsOfText(socialContent.Content).Select(x => new HashtagChecker
                    {
                        Hashtag = x
                    }).ToList();
                    return socialContent;
                }
            }
            return null;
        }

        private List<string> GetHashtagsOfText(string text)
        {
            var hashtags = text.Split("#");
            return hashtags.Reverse().Take(hashtags.Length - 1).ToList();
        }

    }
}
