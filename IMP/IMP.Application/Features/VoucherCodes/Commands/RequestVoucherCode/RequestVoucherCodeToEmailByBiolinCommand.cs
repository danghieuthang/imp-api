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
            private static string EmailTemplate = "send_voucher_code_template.html";
            private readonly IEmailService _emailService;
            private readonly IQRCodeService _qRCodeService;
            public RequetVoucherCodeByBioLinkAndCampaignCommand(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, IQRCodeService qRCodeService) : base(unitOfWork, mapper)
            {
                _emailService = emailService;
                _qRCodeService = qRCodeService;
            }

            public override async Task<Response<bool>> Handle(RequestVoucherCodeToEmailByBiolinkCommand request, CancellationToken cancellationToken)
            {
                var user = await UnitOfWork.Repository<ApplicationUser>().FindSingleAsync(x => x.Pages.Any(y => y.BioLink.ToLower() == request.Biolink.ToLower()));
                if (user == null)
                {
                    return new Response<bool>(false);
                }

                var campaignMember = await UnitOfWork.Repository<CampaignMember>().FindSingleAsync(x => x.CampaignId == request.CampaignId && x.InfluencerId == user.Id);
                if (campaignMember == null)
                {
                    return new Response<bool>(false);
                }

                #region check voucher
                var vouchers = await UnitOfWork.Repository<CampaignVoucher>().FindSingleAsync(
                        predicate: x => x.VoucherId == request.VoucherId
                            && x.CampaignId == request.CampaignId
                            && x.IsBestInfluencerReward == false
                            && x.IsDefaultReward == false
                            && (x.Voucher.ToDate == null
                                || (x.Voucher.ToDate.HasValue && x.Voucher.ToDate.Value.CompareTo(DateTime.UtcNow) > 0) // Get a voucher that hasn't expired yet
                                ),
                        include: x => x.Include(y => y.Voucher));

                var voucher = vouchers?.Voucher;
                if (voucher == null)
                {
                    return new Response<bool>(message: "Mã giảm giá không tồn tại hoặc đã hết hạn.");
                }
                #endregion

                #region get voucher code
                var voucherCode = await UnitOfWork.Repository<VoucherCode>().FindSingleAsync( // get voucher code that assigned for influencer and can used
                        predicate: x => x.VoucherId == voucher.Id
                            && ((x.CampaignMemberId.HasValue && x.CampaignMemberId.Value == campaignMember.Id))
                            && (
                                (x.Quantity == 1 && x.QuantityUsed == 0 && (x.Expired == null || (x.Expired.HasValue && x.Expired.Value.CompareTo(DateTime.UtcNow) < 0)))  // Voucher 1 lần sài && chưa ai sài && không bị ai giữ
                                || (x.Quantity > 1 && x.QuantityUsed < x.Quantity) // Voucher nhiều lần sài và còn sử dụng được
                               )
                    );

                if (voucherCode == null)
                {
                    // get voucher that not assigned for influencer and can used
                    voucherCode = await UnitOfWork.Repository<VoucherCode>().FindSingleAsync(
                       predicate: x => x.VoucherId == voucher.Id
                           && (x.CampaignMemberId == null)
                           && (
                               (x.Quantity == 1 && x.QuantityUsed == 0 && (x.Expired == null || (x.Expired.HasValue && x.Expired.Value.CompareTo(DateTime.UtcNow) < 0)))  // Voucher 1 lần sài && chưa ai sài && không bị ai giữ
                               || (x.Quantity > 1 && x.QuantityUsed < x.Quantity) // Voucher nhiều lần sài và còn sử dụng được
                              )
                   );
                }


                if (voucherCode == null)
                {
                    return new Response<bool>(message: "Đã hết mã giảm giá.");
                }
                #endregion

                #region update quantity get for voucher influencer
                voucherCode.QuantityGet++;

                if (voucher.Quantity == 1 && voucher.HoldTime.HasValue)
                {
                    voucherCode.Expired = DateTime.UtcNow.Add(voucher.HoldTime.Value);
                    UnitOfWork.Repository<VoucherCode>().Update(voucherCode);
                }

                if (voucherCode.CampaignMemberId == null)
                {
                    voucherCode.CampaignMemberId = campaignMember.Id;
                    UnitOfWork.Repository<VoucherCode>().Update(voucherCode);

                }

                var voucherInfluencer = await UnitOfWork.Repository<VoucherInfluencer>().FindSingleAsync(
                        x => x.InfluencerId == user.Id
                            && x.VoucherId == voucher.Id);

                if (voucherInfluencer == null)
                {
                    voucherInfluencer = new VoucherInfluencer
                    {
                        InfluencerId = user.Id,
                        VoucherId = voucher.Id,
                        QuantityGet = 1,
                        QuantityUsed = 0,
                    };
                }
                else
                {
                    voucherInfluencer.QuantityGet++;
                }

                UnitOfWork.Repository<VoucherInfluencer>().Update(voucherInfluencer);
                await UnitOfWork.CommitAsync();

                #endregion

                #region send voucher to email
                _ = Task.Run(async () =>
                {
                    //string dataEncrypt = Utils.Utils.Encrypt($"{voucherCode.Code};{request.CampaignId};{user.Id};{voucher.Id}");
                    //// Create a file path of qr code file
                    //string filePath = await CreateQRCode(dataEncrypt);

                    var emailRequest = BuildEmailRequest(voucher, voucherCode, request.Email, filePath: filePath);

                    await _emailService.SendAsync(emailRequest);

                    // Delete image after send email
                    //File.Delete(filePath);
                });
                #endregion

                return new Response<bool>(true);
            }

            private EmailRequest BuildEmailRequest(Voucher voucher, VoucherCode voucherCode, string email, string filePath)
            {
                string templaePath = Path.Combine(Directory.GetCurrentDirectory(), "App_Datas", "EmailTemplates", EmailTemplate);
                if (!System.IO.File.Exists(templaePath))
                {
                    throw new Exception("Email templates not found");
                }

                StringBuilder emailContent = new StringBuilder();
                emailContent.Append(File.ReadAllText(templaePath));

                if (voucher.Quantity == 1 && voucher.HoldTime.HasValue)
                {
                    emailContent.Replace("@DATEEXPIRED", voucherCode.Expired.Value.ToString("dd-MM-yyyy"));
                }
                else
                {
                    emailContent.Replace("@DATEEXPIRED", voucher.ToDate.Value.ToString("dd-MM-yyyy"));
                }

                emailContent.Replace("@IMAGE", voucher.Image);
                emailContent.Replace("@CODE", voucherCode.Code);
                //emailContent.Replace("@QRCODE", "cid:{0}");
                emailContent.Replace("@NAME", voucher.VoucherName);
                emailContent.Replace("@DESCRIPTION", voucher.Description);

                string content = emailContent.ToString();

                return new EmailRequest
                {
                    To = email,
                    Body = content,
                    Subject = "Bạn vừa nhận được mã giảm giá từ IMP",
                    AttactmentFile = filePath
                };
            }

            private async Task<string> CreateQRCode(string data)
            {
                var image = await _qRCodeService.CreateQRCode($"https://api.influencermarketingplatform.nothleft.online/api/v1/voucher-codes/checking-voucher-code?data={data}");
                string filePath = $"Files/Images/QRcodes";
                string imageFile = $"{filePath}/{Guid.NewGuid()}.png";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                File.WriteAllBytes(imageFile, image);
                return imageFile;
            }
        }
    }
}
