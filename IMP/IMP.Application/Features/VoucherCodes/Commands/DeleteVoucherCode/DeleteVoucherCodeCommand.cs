using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.VoucherCodes.DeleteVoucherCode
{
    public class DeleteVoucherCodeCommand : IDeleteCommand<VoucherCode>
    {
        public int ApplicationUserId { get; set; }
        public int Id { get; set; }
        public class DeleteVoucherCommandHanlder : DeleteCommandHandler<VoucherCode, DeleteVoucherCodeCommand>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public DeleteVoucherCommandHanlder(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<int>> Handle(DeleteVoucherCodeCommand request, CancellationToken cancellationToken)
            {
                var entity = await Repository.FindSingleAsync(x => x.Id == request.Id, include: x => x.Include(y => y.Voucher));

                if (entity == null)
                {
                    var error = new ValidationError("id", $"'{request.Id}' không tồn tại");
                    throw new ValidationException(error);
                }

                if (entity.Voucher.BrandId != _authenticatedUserService.BrandId)
                {
                    var error = new ValidationError("id", $"Không có quyền xóa.");
                    throw new ValidationException(error);
                }

                Repository.Delete(entity);
                await UnitOfWork.CommitAsync();

                return new Response<int>(entity.Id);
            }
        }
    }
}
