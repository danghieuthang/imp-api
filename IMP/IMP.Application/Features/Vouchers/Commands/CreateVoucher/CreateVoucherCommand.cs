using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Newtonsoft.Json;

namespace IMP.Application.Features.Vouchers.Commands.CreateVoucher
{
    public class CreateVoucherCommand : ICommand<VoucherViewModel>
    {
        [JsonIgnore]
        public int BrandId { get; set; }
        public int CampaignId { get; set; }
        public string VoucherName { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public TimeSpan? FromTime { get; set; }
        public TimeSpan? ToTime { get; set; }
        public string Description { get; set; }
        public string Action { get; set; }
        public string Condition { get; set; }
        public string Target { get; set; }

        public class CreateVoucherCommandHandler : CommandHandler<CreateVoucherCommand, VoucherViewModel>
        {
            public CreateVoucherCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<VoucherViewModel>> Handle(CreateVoucherCommand request, CancellationToken cancellationToken)
            {
                var voucher = Mapper.Map<Voucher>(request);
                await UnitOfWork.Repository<Voucher>().AddAsync(voucher);
                await UnitOfWork.CommitAsync();
                var voucherView = Mapper.Map<VoucherViewModel>(voucher);
                return new Response<VoucherViewModel>(voucherView);
            }
        }
    }
}