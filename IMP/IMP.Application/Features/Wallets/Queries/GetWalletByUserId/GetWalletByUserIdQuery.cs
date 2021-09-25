﻿using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Wallets.Queries.GetWalletByUserId
{
    public class GetWalletByUserIdQuery : IQuery<WalletViewModel>
    {
        public int ApplicationUserId { get; set; }
        public class GetWalletByUserIdQueryHandler : QueryHandler<GetWalletByUserIdQuery, WalletViewModel>
        {
            private readonly IGenericRepository<Wallet> _walletRepository;
            public GetWalletByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _walletRepository = unitOfWork.Repository<Wallet>();
            }

            public override async Task<Response<WalletViewModel>> Handle(GetWalletByUserIdQuery request, CancellationToken cancellationToken)
            {
                var wallet = await _walletRepository.FindSingleAsync(x => x.ApplicationUserId == request.ApplicationUserId);
                if (wallet != null)
                {
                    var walletView = Mapper.Map<WalletViewModel>(wallet);
                    return new Response<WalletViewModel>(walletView);
                }
                return new Response<WalletViewModel>(error: new Models.ValidationError("application_user_id", "Không hợp lệ."));
            }
        }
    }
}