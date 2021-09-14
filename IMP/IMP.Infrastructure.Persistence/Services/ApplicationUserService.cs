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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepositoryAsync<ApplicationUser> _applicationUserRepositoryAsync;

        public ApplicationUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _applicationUserRepositoryAsync = _unitOfWork.Repository<ApplicationUser>();
        }

        public async Task<ApplicationUser> CreateUser(string userName)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                PaymentInfor = new PaymentInfor(),
                Wallet = new Wallet()
            };
            user = await _applicationUserRepositoryAsync.AddAsync(user);
            await _unitOfWork.CommitAsync();
            return user;
        }

        public async Task DeleteUser(string userName)
        {
            var user = await _applicationUserRepositoryAsync.FindSingleAsync(x => x.UserName.ToLower() == userName.ToLower());
            if (user != null)
            {
                _applicationUserRepositoryAsync.Delete(user);
                await _unitOfWork.CommitAsync();
            }
        }

        private string GetUserNameFromEmail(string email)
        {
            return email.Split("@")[0];
        }
    }
}
