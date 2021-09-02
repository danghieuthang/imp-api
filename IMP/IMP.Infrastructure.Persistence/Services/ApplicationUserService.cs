using IMP.Application.Interfaces.Repositories;
using IMP.Domain.Entities;
using IMP.Infrastructure.Persistence.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Interfaces.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IApplicationUserRepositoryAsync _applicationUserRepositoryAsync;

        public ApplicationUserService(IApplicationUserRepositoryAsync applicationUserRepositoryAsync)
        {
            _applicationUserRepositoryAsync = applicationUserRepositoryAsync;
        }

        public async Task<ApplicationUser> CreateUser(string userName)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                PaymentInfor = new PaymentInfor(),
                Wallet = new Wallet()
            };
            return await _applicationUserRepositoryAsync.AddAsync(user);
        }

        public async Task DeleteUser(string userName)
        {
            var user = await _applicationUserRepositoryAsync.GetByUserName(userName);
            if (user != null)
            {
                await _applicationUserRepositoryAsync.DeleteAsync(user);
            }
        }

        private string GetUserNameFromEmail(string email)
        {
            return email.Split("@")[0];
        }
    }
}
