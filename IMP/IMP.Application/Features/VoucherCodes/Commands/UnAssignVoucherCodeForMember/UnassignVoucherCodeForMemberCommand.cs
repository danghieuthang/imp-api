using AutoMapper;
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

namespace IMP.Application.Features.VoucherCodes.Commands.UnAssignVoucherCodeForMember
{
    public class UnassignVoucherCodeForMemberCommand : ICommand<bool>
    {
        public int Id { get; set; }
        public class UnassignVoucherCodeForMemberCommandHandler : CommandHandler<UnassignVoucherCodeForMemberCommand, bool>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public UnassignVoucherCodeForMemberCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<bool>> Handle(UnassignVoucherCodeForMemberCommand request, CancellationToken cancellationToken)
            {
                var voucherCode = await UnitOfWork.Repository<VoucherCode>().FindSingleAsync(x => x.Id == request.Id, x => x.Voucher);
                if (voucherCode == null)
                {
                    throw new ValidationException(new ValidationError("id", "Không tồn tại."));
                }
                if (voucherCode.Voucher.BrandId != _authenticatedUserService.BrandId)
                {
                    throw new ValidationException(new ValidationError("id", "Không có quyền."));
                }

                voucherCode.CampaignMemberId = null;
                UnitOfWork.Repository<VoucherCode>().Update(voucherCode);

                await UnitOfWork.CommitAsync();
                return new Response<bool>(data: true);
            }
        }
    }
}
