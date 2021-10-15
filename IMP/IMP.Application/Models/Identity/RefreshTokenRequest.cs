using System;
namespace IMP.Application.Models.Account
{
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
        public RefreshTokenRequest()
        {
        }
    }
    public class RevokeTokenRequest : RefreshTokenRequest
    {

    }
}
