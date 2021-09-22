using System.Windows.Input;
using System;
using Newtonsoft.Json;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using System.Threading.Tasks;
using IMP.Application.Wrappers;
using System.Threading;
using AutoMapper;
using IMP.Domain.Entities;
using IMP.Application.Exceptions;
using IMP.Application.Models;

namespace IMP.Application.Features.Vouchers.Commands.UpdateVoucher
{
    public class UpdateVoucherCommand : ICommand<VoucherViewModel>
    {
        [JsonIgnore]
        public int BrandId { get; set; }
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public string VoucherName { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public int UserQuantity { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public TimeSpan? FromTime { get; set; }
        public TimeSpan? ToTime { get; set; }
        public string Description { get; set; }
        public string Action { get; set; }
        public string Condition { get; set; }
        public string Target { get; set; }

        public class UpdateVoucherCommandHandler : CommandHandler<UpdateVoucherCommand, VoucherViewModel>
        {
            public UpdateVoucherCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<VoucherViewModel>> Handle(UpdateVoucherCommand request, CancellationToken cancellationToken)
            {
                var repository = UnitOfWork.Repository<Voucher>();
                var voucher = await repository.GetByIdAsync(request.Id);
                if (voucher != null)
                {
                    Mapper.Map(request, voucher);
                    repository.Update(voucher);
                    await UnitOfWork.CommitAsync();

                    var voucherView = Mapper.Map<VoucherViewModel>(voucher);
                    return new Response<VoucherViewModel>(voucherView);
                }
                throw new ValidationException(new ValidationError("id", $"'{request.Id}' không tồn tại."));
            }
        }
    }
}