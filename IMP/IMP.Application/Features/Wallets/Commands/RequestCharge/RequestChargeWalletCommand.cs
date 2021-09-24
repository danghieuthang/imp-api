using AutoMapper;
using FluentValidation;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Wallets.Commands.RequestCharge
{
    public class RequestChargeWalletCommand : ICommand<ChargeWalletResponse>
    {
        [JsonIgnore]
        public int ApplicationUserId { get; set; }
        public int Amount { get; set; }
        public class RequestChargeWalletCommandValidator : AbstractValidator<RequestChargeWalletCommand>
        {
            public RequestChargeWalletCommandValidator()
            {
                RuleFor(x => x.Amount).GreaterThan(1000).WithMessage("Tiền nạp phải lớn hơn 1000 VND.");
            }
        }

        public class RequestChargeWalletCommandHandler : CommandHandler<RequestChargeWalletCommand, ChargeWalletResponse>
        {
            private readonly IVnPayService _vnPayService;

            public RequestChargeWalletCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IVnPayService vnPayService) : base(unitOfWork, mapper)
            {
                _vnPayService = vnPayService;
            }

            public override async Task<Response<ChargeWalletResponse>> Handle(RequestChargeWalletCommand request, CancellationToken cancellationToken)
            {
                var wallet = await UnitOfWork.Repository<Wallet>().FindSingleAsync(x => x.ApplicationUserId == request.ApplicationUserId);
                if (wallet != null)
                {
                    string paymentInfo = "Request Charge Wallet " + wallet.Id;
                    string url = _vnPayService.CreatePaymentUrl(amount: request.Amount, walletId: wallet.Id, paymentInfo: paymentInfo);
                    return new Response<ChargeWalletResponse>(new ChargeWalletResponse { Url = url });
                }
                return new Response<ChargeWalletResponse>(error: new Models.ValidationError("wallet_id", "Không tồn tại wallet"));
            }
        }
    }
}
