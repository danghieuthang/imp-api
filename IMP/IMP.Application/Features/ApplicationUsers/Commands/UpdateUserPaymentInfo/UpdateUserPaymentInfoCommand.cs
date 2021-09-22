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
                var entity = await _paymentInfoRepository.FindSingleAsync(x => x.UserId == request.ApplicationUserId);
                bool isAdd = false;
                if (entity == null)
                {
                    entity = new PaymentInfor();
                    isAdd = true;
                }

                entity.BankId = request.BankId;
                entity.UserId = request.ApplicationUserId;
                entity.AccountNumber = request.AccountNumber;
                entity.AccountName = request.AccountName.ToUpper();

                if (isAdd)
                {
                    entity = await _paymentInfoRepository.AddAsync(entity);
                    await _unitOfWork.CommitAsync();
                }
                else
                {
                    _paymentInfoRepository.Update(entity);
                }

                var user = await _applicationUserRepository.GetByIdAsync(request.ApplicationUserId);
                user.PaymentInforId = entity.Id;
                _applicationUserRepository.Update(user);

                await _unitOfWork.CommitAsync();

                entity = await _paymentInfoRepository.FindSingleAsync(x => x.Id == entity.Id, x => x.Bank);
                var view = _mapper.Map<PaymentInforViewModel>(entity);
                return new Response<PaymentInforViewModel>(view);
            }
        }
    }
}
