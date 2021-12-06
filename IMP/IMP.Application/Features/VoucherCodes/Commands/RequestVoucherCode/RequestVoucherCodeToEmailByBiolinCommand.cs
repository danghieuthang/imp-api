using AutoMapper;
using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Services;
using IMP.Application.Models.Email;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.VoucherCodes.Commands.RequestVoucherCode
{
    public class RequestVoucherCodeToEmailByBiolinkCommand : ICommand<bool>
    {
        public int CampaignId { get; set; }
        public int VoucherId { get; set; }
        public string Biolink { get; set; }
        public string Email { get; set; }
        public class RequestVoucherCodeToEmailByBiolinkCommandValidator : AbstractValidator<RequestVoucherCodeToEmailByBiolinkCommand>
        {
            public RequestVoucherCodeToEmailByBiolinkCommandValidator()
            {
                RuleFor(x => x.Email).MustValidEmail(allowNull: false);
            }
        }

        public class RequetVoucherCodeByBioLinkAndCampaignCommand : CommandHandler<RequestVoucherCodeToEmailByBiolinkCommand, bool>
        {
            private static string EmailTemplate = "send_voucher_qrcode_template.html";
            private readonly IEmailService _emailService;
            private readonly IQRCodeService _qRCodeService;
            public RequetVoucherCodeByBioLinkAndCampaignCommand(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, IQRCodeService qRCodeService) : base(unitOfWork, mapper)
            {
                _emailService = emailService;
                _qRCodeService = qRCodeService;
            }

            public override async Task<Response<bool>> Handle(RequestVoucherCodeToEmailByBiolinkCommand request, CancellationToken cancellationToken)
            {
                var user = await UnitOfWork.Repository<ApplicationUser>().FindSingleAsync(x => x.Pages.Any(y => y.BioLink.ToLower() == request.Biolink.ToLower()), x => x.VoucherCodeApplicationUsers);
                if (user == null)
                {
                    return new Response<bool>(false);
                }

                var vouchers = await UnitOfWork.Repository<CampaignVoucher>().GetAll(
                        predicate: x => x.VoucherId == request.VoucherId
                            && x.CampaignId == request.CampaignId
                            && x.IsBestInfluencerReward == false
                            && x.IsDefaultReward == false,
                        selector: x => x.VoucherId).ToListAsync();

                var voucherCodes = await UnitOfWork.Repository<VoucherCode>().GetAll(
                        predicate: x => x.CampaignMember.InfluencerId == user.Id
                            && x.CampaignMember.CampaignId == request.CampaignId
                            && x.VoucherId == request.VoucherId
                            && x.QuantityUsed < x.Quantity
                            && (x.Voucher.ToDate == null || x.Voucher.ToDate > DateTime.UtcNow),
                        include: x => x.Include(y => y.Voucher)
                            ).Take(1).ToListAsync();

                if (!voucherCodes.Any())
                {
                    return new Response<bool>(message: "Đã hết voucher code.");
                }

                _ = Task.Run(async () =>
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "App_Datas", "EmailTemplates", EmailTemplate);
                    if (!System.IO.File.Exists(path))
                    {
                        throw new Exception("Email templates not found");
                    }

                    var image = await _qRCodeService.CreateQRCode("https://www.youtube.com/watch?v=LdyG3pzJ2cc");
                    string filePath = $"Files/Images/QRcodes";
                    string imageFile = $"{filePath}/{Guid.NewGuid()}.png";
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    File.WriteAllBytes(imageFile, image);

                    StringBuilder emailContent = new StringBuilder();
                    emailContent.Append(File.ReadAllText(path));
                    emailContent.Replace("@CODE", voucherCodes.FirstOrDefault().Code);
                    emailContent.Replace("@DATEEXPIRED", voucherCodes.FirstOrDefault().Voucher.ToDate?.ToString("dd-MM-yyyy"));
                    emailContent.Replace("@IMAGE", voucherCodes.FirstOrDefault().Voucher.Image);
                    emailContent.Replace("@QRCODE", "cid:{0}");
                    emailContent.Replace("@NAME", voucherCodes.FirstOrDefault().Voucher.VoucherName);
                    emailContent.Replace("@DESCRIPTION", voucherCodes.FirstOrDefault().Voucher.Description);

                    string content = emailContent.ToString();

                    await _emailService.SendAsync(
                        new EmailRequest
                        {
                            To = request.Email,
                            Body = content,
                            Subject = "Bạn vừa nhận được mã giảm giá từ IMP",
                            AttactmentFile = imageFile
                        });

                    File.Delete(imageFile);
                });

                return new Response<bool>(true);
            }
        }
    }
}
