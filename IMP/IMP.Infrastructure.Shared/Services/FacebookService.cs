using IMP.Application.Interfaces;
using IMP.Application.Models.Account;
using IMP.Domain.Settings;
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

    public class FacebookService : IFacebookService
    {
        private readonly FacebookSettings _settings;
        public FacebookService(IOptions<FacebookSettings> options)
        {
            _settings = options.Value;
        }
        public async Task<ProviderUserDetail> ValidationAccessToken(string accessToken)
        {
            string verifyTokenEndpoint = string.Format("https://graph.facebook.com/me?access_token={0}&fields=email,name,first_name,last_name", accessToken);
            string verifyAppEndpoint = string.Format("https://graph.facebook.com/app?access_token={0}", accessToken);

            var uri = new Uri(verifyTokenEndpoint);

            var client = new HttpClient();
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                dynamic userObj = JsonConvert.DeserializeObject(content);

                uri = new Uri(verifyAppEndpoint);
                response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    content = await response.Content.ReadAsStringAsync();
                    dynamic appObj = JsonConvert.DeserializeObject(content);

                    if (appObj["id"] == _settings.ClientId)
                    {
                        ProviderUserDetail user = new();
                        user.Email = userObj["email"];
                        user.FirstName = userObj["first_name"];
                        user.LastName = userObj["last_name"];
                        return user;
                    }
                }

            }
            return null;
        }
    }
}
