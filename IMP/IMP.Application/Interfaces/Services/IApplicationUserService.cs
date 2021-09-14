using IMP.Domain.Entities;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Persistence.Services
{
    public interface IApplicationUserService
    {
        Task<ApplicationUser> CreateUser(string userName);
        Task DeleteUser(string userName);
        Task UpdateUsername(string oldUsername, string newUsername);
    }
}