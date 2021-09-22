using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using Newtonsoft.Json;

namespace IMP.Application.Features.ApplicationUsers.Commands.VerifyEmail
{
    public class VerifyEmailCommand : ICommand<string>
    {
        [JsonIgnore]
        public int InfluencerId { get; set; }
        public int Code { get; set; }
        public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, Response<string>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IGenericRepository<ApplicationUser> _repository;

            public VerifyEmailCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
                _repository = _unitOfWork.Repository<ApplicationUser>();
            }

            public async Task<Response<string>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
            {
                var user = await _repository.FindSingleAsync(x => x.Id == request.InfluencerId, x => x.Otps);
                if (user?.Otps.Count > 0)
                {
                    var otp = user.Otps.FirstOrDefault(x => x.Code == request.Code);
                    if (otp != null)
                    {
                        if (otp.ExpiredTime < DateTime.UtcNow)
                        {
                            throw new ValidationException(new ValidationError("code", "Code đã hết hạn."));
                        }
                        user.IsEmailVerified = true;
                        _unitOfWork.Repository<Otp>().Delete(otp);
                        _repository.Update(user);
                        await _unitOfWork.CommitAsync();
                        return new Response<string>(data: $"'{user.Email}' đã được xắc thực thành công.");
                    }
                }
                throw new ValidationException(new ValidationError("code", "Code không tồn tại."));

            }
        }
    }
}