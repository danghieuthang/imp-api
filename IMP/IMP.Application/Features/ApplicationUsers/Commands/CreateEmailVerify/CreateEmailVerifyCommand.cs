using System.Security.Cryptography;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using IMP.Domain.Settings;
using Microsoft.Extensions.Options;

namespace IMP.Application.Features.ApplicationUsers.Commands.CreateEmailVerify
{
    public class CreateEmailVerifyCommand : ICommand<string>
    {
        public int InfluencerId { get; set; }
        public class CreateEmailVefiryCommandHandler : IRequestHandler<CreateEmailVerifyCommand, Response<string>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly OtpSettings _otpSettings;
            private readonly IEmailService _emailService;

            public CreateEmailVefiryCommandHandler(IUnitOfWork unitOfWork, IOptions<OtpSettings> options, IEmailService emailService)
            {
                _unitOfWork = unitOfWork;
                _otpSettings = options.Value;
                _emailService = emailService;
            }

            public async Task<Response<string>> Handle(CreateEmailVerifyCommand request, CancellationToken cancellationToken)
            {
                var user = await _unitOfWork.Repository<ApplicationUser>().GetByIdAsync(request.InfluencerId);
                if (user == null)
                {
                    throw new ValidationException(new ValidationError("user", "User không tồn tại."));
                }
                if (string.IsNullOrEmpty(user.Email))
                {
                    throw new ValidationException(new ValidationError("user", "User chưa có email."));
                }
                int code = RandomNumberGenerator.GetInt32(1000, 9999);
                await _unitOfWork.Repository<Otp>().AddAsync(new Otp
                {
                    Code = code,
                    ExpiredTime = DateTime.UtcNow.AddMinutes(_otpSettings.ExpiredTime),
                    ApplicationUserId = user.Id
                });
                await _unitOfWork.CommitAsync();
                _ = Task.Run(() =>
                {
                    _emailService.SendAsync(new Application.Models.Email.EmailRequest() { To = user.Email, Body = $"Mã xác thực email của bạn là: <p>{code}</p>. Mã chỉ có hiệu lực trong vòng {_otpSettings.ExpiredTime} phút.", Subject = "Xác thực email IMP Platform" });
                });
                return new Response<string>(data: $"Code có hiệu lực trong vòng {_otpSettings.ExpiredTime} phút.");
            }
        }
    }
}