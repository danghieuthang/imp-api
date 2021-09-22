using System.Threading.Tasks;
using IMP.Domain.SocialPlatforms;

namespace IMP.Application.Interfaces
{
    public interface ITiktokService
    {
        Task<SocialPlatformUser> VerifyUser(string username, string hashtag);
    }
}