using IMP.Application.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Interfaces
{
    public interface IFacebookService
    {
        Task<ProviderUserDetail> ValidationAccessToken(string accessToken);
    }
}
