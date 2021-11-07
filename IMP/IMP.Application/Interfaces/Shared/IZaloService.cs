using IMP.Application.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Interfaces.Shared
{
    public interface IZaloService
    {
        Task<ProviderUserDetail> ValidationAccessToken(string accessToken);
        Task<bool> SendMessageAsync(string clientAccessToken);
        Task<string> GetLoginUrlAsync(string callBackUrl);
    }
}
