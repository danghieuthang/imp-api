using Google.Apis.Auth;
using IMP.Application.Interfaces;
using IMP.Application.Models.Account;
using IMP.Domain.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Shared.Services
{

    public class GoogleService : IGoogleService
    {
        private readonly GoogleSettings _settings;

        public GoogleService(IOptions<GoogleSettings> options)
        {
            _settings = options.Value;
        }

        public async Task<ProviderUserDetail> ValidateIdToken(string idToken)
        {
            if (string.IsNullOrWhiteSpace(idToken))
            {
                return null;
            }

            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
                return new ProviderUserDetail
                {
                    Email = payload.Email,
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    Locale = payload.Locale,
                    Name = payload.Name,
                    ProviderUserId = payload.Subject
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
