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
        public int BrandId { get; set; }
        public int Id { get; set; }
        public class DeleteVoucherCommandHandler : DeleteCommandHandler<Voucher, DeleteVoucherCommand>
        {
            public DeleteVoucherCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
            {
            }


            public override async Task<Response<int>> Handle(DeleteVoucherCommand request, CancellationToken cancellationToken)
            {
                var voucher = await RepositoryAsync.FindSingleAsync(x => x.Id == request.Id, includeProperties: x => x.Campaign);
                if (voucher != null)
                {
                    if (voucher.Campaign.BrandId == request.BrandId)
                    {
                        RepositoryAsync.Delete(voucher);
                        await UnitOfWork.CommitAsync();
                    }
                    else
                        throw new ValidationException(new ValidationError("id", $"Không có quyền xoá."));
                }
                throw new ValidationException(new ValidationError("id", $"'{request.Id}' không tồn tại."));

            }

        }
    }
}