using IMP.Application.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Interfaces.Services
{
    public interface IFacebookAnalysisService
    {
        /// <summary>
        /// Get information of a public post in facebook
        /// </summary>
        /// <param name="postUrl"></param>
        /// <returns></returns>
        Task<SocialContent> GetPost(string postUrl);
    }
}
