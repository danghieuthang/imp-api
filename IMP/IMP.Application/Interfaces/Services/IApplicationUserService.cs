using IMP.Application.Enums;
using IMP.Domain.Entities;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Persistence.Services
{
    public interface IApplicationUserService
    {
        Task<ApplicationUser> CreateUser(RegisterRole userRole,string email = null, string avatar = null);
        Task DeleteUser(string userName);
        Task DeleteUser(int id);
        Task UpdateUsername(string oldUsername, string newUsername);
    }
}