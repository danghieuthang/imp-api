//using IMP.Application.Models.Account;
//using IMP.Domain.Settings;
//using Microsoft.Extensions.Options;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;

//namespace IMP.Infrastructure.Shared.Services
//{
//    public class InstagramService
//    {
//        private readonly InstagramSettings settings;

//        public InstagramService(IOptions<InstagramSettings> options)
//        {
//            this.settings = options.Value;
//        }

//        public async Task<ProviderUserDetail> ValidationAccessToken(string accessToken)
//        {
//            string verifyTokenEndpoint = string.Format("https://graph.instagram.com/me?access_token={0}&fields=id,username,media_count", accessToken);

//            var uri = new Uri(verifyTokenEndpoint);

//            var client = new HttpClient();
//            var response = await client.GetAsync(uri);

//            if (response.IsSuccessStatusCode)
//            {
//                var content = await response.Content.ReadAsStringAsync();
//                dynamic userObj = JsonConvert.DeserializeObject(content);

//                return new ProviderUserDetail
//                {
//                    Name = userObj["username"],
//                    Email = userObj["id"],
//                };
//            }
//            return null;
//        }

//        public async Task<ProviderUserDetail> ValidationAccessToken(string code, string accessToken = null)
//        {
//            string verifyTokenEndpoint = string.Format("https://api.instagram.com/oauth/access_token");

//            var uri = new Uri(verifyTokenEndpoint);

//            var httpRequestMessage = new HttpRequestMessage();
//            httpRequestMessage.Method = HttpMethod.Post;
//            httpRequestMessage.RequestUri = uri;

//            var parameters = new List<KeyValuePair<string, string>>
//            {
//                new KeyValuePair<string, string>("client_id", settings.ClientId),
//                new KeyValuePair<string, string>("client_secret", settings.ClientSecret),
//                new KeyValuePair<string, string>("code", code),
//                new KeyValuePair<string, string>("grant_type", "authorization_code"),
//                new KeyValuePair<string, string>("redirect_uri", "https://localhost:8000/"),
//            }

//            if (response.IsSuccessStatusCode)
//            {
//                var content = await response.Content.ReadAsStringAsync();
//                dynamic userObj = JsonConvert.DeserializeObject(content);

//                return new ProviderUserDetail
//                {
//                    Name = userObj["username"],
//                    Email = userObj["id"],
//                };
//            }
//            return null;
//        }
//    }
//}
