using AutoMapper;
using FluentValidation;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.VoucherCodes.Commands.AssignVoucherCodeForCampaignMember
{
    public class AssignVoucherCodesForCampaignMemberCommand : ICommand<bool>
    {
        public int CampaignMemberId { get; set; }

        public List<int> VoucherCodeIds { get; set; }
        public class AssignVoucherCodesForCampaignMemberCommandHandler : CommandHandler<AssignVoucherCodesForCampaignMemberCommand, bool>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public AssignVoucherCodesForCampaignMemberCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<bool>> Handle(AssignVoucherCodesForCampaignMemberCommand request, CancellationToken cancellationToken)
            {
                var campaignMember = await UnitOfWork.Repository<CampaignMember>().FindSingleAsync(x => x.Id == request.CampaignMemberId, x => x.Campaign, x => x.Campaign.Vouchers);
                if (campaignMember == null)
                {
                    throw new IMP.Application.Exceptions.ValidationException(new ValidationError("campaign_member_id", "Không tồn tại."));
                }
                if (campaignMember.Campaign.BrandId != _authenticatedUserService.BrandId)
                {
                    throw new IMP.Application.Exceptions.ValidationException(new ValidationError("campaign_member_id", "Không có quyền thêm voucher code trong chiến dịch này."));
                };

                var voucherCodes = UnitOfWork.Repository<VoucherCode>().GetAll(predicate: x => request.VoucherCodeIds.Contains(x.Id)).ToList();

                // check validation voucher codes
                if (voucherCodes.Count != request.VoucherCodeIds.Count)
                {
                    var errors = new List<ValidationError>();
                    var voucherCodeIds = voucherCodes.Select(x => x.Id).ToList();
                    foreach (int voucherId in request.VoucherCodeIds)
                    {
                        if (voucherCodeIds.Contains(voucherId))
                        {
                            errors.Add(new ValidationError(voucherId.ToString(), "Không tồn tại."));
                        }
                    }
                    throw new IMP.Application.Exceptions.ValidationException(errors: errors);
                }

                // check voucher code is already assign
                var alreadyExistErrors = new List<ValidationError>();
                foreach (var voucherCode in voucherCodes)
                {
                    if (!campaignMember.Campaign.Vouchers.Any(x => x.Id == voucherCode.VoucherId))
                    {
                        alreadyExistErrors.Add(new ValidationError(voucherCode.Id.ToString(), "Không nằm trong các voucher của chiến dịch."));
                    }
                }
                if (alreadyExistErrors.Count > 0)
                {
                    throw new IMP.Application.Exceptions.ValidationException(errors: alreadyExistErrors);
                }

                foreach (var voucherCode in voucherCodes)
                {
                    if (voucherCode.CampaignMemberId != null)
                    {
                        alreadyExistErrors.Add(new ValidationError(voucherCode.Id.ToString(), "Đã được cấp cho thành viên khác."));
                    }
                }


                // Assign voucher code for influencer
                voucherCodes.ForEach(x =>
                {
                    x.CampaignMemberId = campaignMember.Id;
                    UnitOfWork.Repository<VoucherCode>().Update(x);
                });

                await UnitOfWork.CommitAsync();
                return new Response<bool>(data: true);
            }
        }
    }
}
