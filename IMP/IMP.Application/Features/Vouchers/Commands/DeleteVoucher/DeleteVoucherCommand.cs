using System.Threading;
using System.Threading.Tasks;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;

namespace IMP.Application.Features.Vouchers.Commands.DeleteVoucher
{
    public class DeleteVoucherCommand : IDeleteCommand<Voucher>
    {
        public int Id { get; set; }
        public class DeleteVoucherCommandHandler : DeleteCommandHandler<Voucher, DeleteVoucherCommand>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public DeleteVoucherCommandHandler(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork)
            {
                _authenticatedUserService = authenticatedUserService;
            }


            public override async Task<Response<int>> Handle(DeleteVoucherCommand request, CancellationToken cancellationToken)
            {
                var user = await UnitOfWork.Repository<ApplicationUser>().GetByIdAsync(_authenticatedUserService.ApplicationUserId);
                var voucher = await Repository.FindSingleAsync(x => x.Id == request.Id);
                if (voucher != null)
                {
                    if (voucher.BrandId == user.BrandId)
                    {
                        Repository.Delete(voucher);
                        await UnitOfWork.CommitAsync();
                        return new Response<int>(request.Id);
                    }
                    else
                        throw new ValidationException(new ValidationError("id", $"Không có quyền xoá."));
                }
                throw new ValidationException(new ValidationError("id", $"'{request.Id}' không tồn tại."));

            }

        }
    }
}