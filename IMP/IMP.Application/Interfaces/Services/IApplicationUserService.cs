using IMP.Domain.Entities;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Persistence.Services
{
    public interface IApplicationUserService
    {
        Task<ApplicationUser> CreateUser(string email = null);
        Task DeleteUser(string userName);
        Task DeleteUser(int id);
        Task UpdateUsername(string oldUsername, string newUsername);
    }
}