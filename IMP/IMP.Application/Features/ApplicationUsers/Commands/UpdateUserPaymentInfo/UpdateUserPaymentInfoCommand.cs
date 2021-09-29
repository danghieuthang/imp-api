using AutoMapper;
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

namespace IMP.Application.Features.ApplicationUsers.Commands.UpdateUserPaymentInfo
{
    public class UpdateUserPaymentInfoCommand : ICommand<PaymentInforViewModel>
    {
        [JsonIgnore]
        public int ApplicationUserId { get; set; }
        public int BankId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }

        public class UpdateUserPaymentInfoCommandHandler : IRequestHandler<UpdateUserPaymentInfoCommand, Response<PaymentInforViewModel>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IGenericRepository<PaymentInfor> _paymentInfoRepository;
            private readonly IGenericRepository<ApplicationUser> _applicationUserRepository;
            private readonly IMapper _mapper;
            public UpdateUserPaymentInfoCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _paymentInfoRepository = _unitOfWork.Repository<PaymentInfor>();
                _applicationUserRepository = _unitOfWork.Repository<ApplicationUser>();
            }
            public async Task<Response<PaymentInforViewModel>> Handle(UpdateUserPaymentInfoCommand request, CancellationToken cancellationToken)
            {
                var user = await _applicationUserRepository.FindSingleAsync(x => x.Id == request.ApplicationUserId, includeProperties: x => x.PaymentInfor);
                var paymentInfo = user?.PaymentInfor;
                bool isAdd = false;
                if (paymentInfo == null)
                {
                    paymentInfo = new PaymentInfor();
                    isAdd = true;
                }

                paymentInfo.BankId = request.BankId;
                paymentInfo.UserId = request.ApplicationUserId;
                paymentInfo.AccountNumber = request.AccountNumber;
                paymentInfo.AccountName = request.AccountName.ToUpper();

                if (isAdd)
                {
                    paymentInfo = await _paymentInfoRepository.AddAsync(paymentInfo);
                    await _unitOfWork.CommitAsync();
                }
                else
                {
                    _paymentInfoRepository.Update(paymentInfo);
                }

                await _unitOfWork.CommitAsync();

                paymentInfo = await _paymentInfoRepository.FindSingleAsync(x => x.Id == paymentInfo.Id, x => x.Bank);
                var view = _mapper.Map<PaymentInforViewModel>(paymentInfo);
                return new Response<PaymentInforViewModel>(view);
            }
        }
    }
}
