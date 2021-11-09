using IMP.Application.Enums;
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
        private readonly IGenericRepository<ApplicationUser> _applicationUserRepository;

        public ApplicationUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _applicationUserRepository = _unitOfWork.Repository<ApplicationUser>();
        }

        public async Task<ApplicationUser> CreateUser(RegisterRole role, string email = null, string avatar = null)
        {
            var user = new ApplicationUser
            {
                Email = email,
                Avatar = avatar,
                PaymentInfor = new PaymentInfor(),
                Wallet = new Wallet(),
                IsEmailVerified = email != null
            };
            // if role is brand
            if (role == RegisterRole.Brand)
            {
                user.Brand = new Brand
                {
                    Email = email
                };
            }
            else if (role == RegisterRole.Influencer)
            {
                user.Ranking = new Ranking
                {
                    RankLevelId = 1,
                    Score = 0;
                }
            }
            user = await _applicationUserRepository.AddAsync(user);
            await _unitOfWork.CommitAsync();

            user.Wallet.ApplicationUserId = user.Id;
            _unitOfWork.Repository<Wallet>().Update(user.Wallet);

            if (role == RegisterRole.Brand)
            {
                // Update wallet for brand
                var brand = user.Brand;
                brand.WalletId = user.Wallet.Id;
                _unitOfWork.Repository<Brand>().Update(brand);
            }

            await _unitOfWork.CommitAsync();

            return user;
        }

        public async Task DeleteUser(string userName)
        {
            var user = await _applicationUserRepository.FindSingleAsync(x => x.Email.ToLower() == userName.ToLower());
            if (user != null)
            {
                _applicationUserRepository.Delete(user);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task DeleteUser(int id)
        {
            var user = await _applicationUserRepository.GetByIdAsync(id);
            if (user != null)
            {
                _applicationUserRepository.Delete(user);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task UpdateUsername(string oldUsername, string newUsername)
        {
            var user = await _applicationUserRepository.FindSingleAsync(x => x.Email == oldUsername);
            if (user != null)
            {
                user.Email = newUsername;
                _applicationUserRepository.Update(user);
                await _unitOfWork.CommitAsync();
            }
        }

        private string GetUserNameFromEmail(string email)
        {
            return email.Split("@")[0];
        }
    }
}
