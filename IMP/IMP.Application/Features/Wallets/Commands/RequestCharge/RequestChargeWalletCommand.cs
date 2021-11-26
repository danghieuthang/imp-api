using AutoMapper;
using FluentValidation;
using IMP.Application.Constants;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Wallets.Commands.RequestCharge
{
    public class RequestChargeWalletCommand : ICommand<ChargeWalletResponse>
    {
        public int Amount { get; set; }
        public string ReturnUrl { get; set; }
        public class RequestChargeWalletCommandValidator : AbstractValidator<RequestChargeWalletCommand>
        {
            public RequestChargeWalletCommandValidator()
            {
                RuleFor(x => x.Amount).GreaterThan(50000).WithMessage("Tiền nạp phải lớn hơn 50000 VND.").WithErrorCode(ErrorConstants.Application.WalletTransaction.AmountNotValid.ToString());
                RuleFor(x => x.ReturnUrl).MustValidUrl();
            }
        }

        public class RequestChargeWalletCommandHandler : CommandHandler<RequestChargeWalletCommand, ChargeWalletResponse>
        {
            private readonly IVnPayService _vnPayService;
            private readonly IAuthenticatedUserService _authenticatedUserService;

            public RequestChargeWalletCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IVnPayService vnPayService, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _vnPayService = vnPayService;
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<ChargeWalletResponse>> Handle(RequestChargeWalletCommand request, CancellationToken cancellationToken)
            {
                Wallet wallet;
                if (_authenticatedUserService.BrandId.HasValue)
                {
                    var brand = await UnitOfWork.Repository<Brand>().FindSingleAsync(x => x.Id == _authenticatedUserService.BrandId.Value, x => x.Wallet);
                    wallet = brand.Wallet;
                }
                else
                {
                    wallet = await UnitOfWork.Repository<Wallet>().FindSingleAsync(x => x.ApplicationUserId == _authenticatedUserService.ApplicationUserId);
                }

                if (wallet != null)
                {
                    string paymentInfo = "Request Charge Wallet " + wallet.Id;
                    string url = _vnPayService.CreatePaymentUrl(amount: request.Amount, walletId: wallet.Id, paymentInfo: paymentInfo, returnUrl: request.ReturnUrl);
                    return new Response<ChargeWalletResponse>(new ChargeWalletResponse { Url = url });
                }
                return new Response<ChargeWalletResponse>(error: new Models.ValidationError("wallet_id", "Không tồn tại wallet"));
            }
        }
    }
}
