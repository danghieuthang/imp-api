using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Banks.Queries.GetAllBank
{
    public class GetAllBankQuery : IGetAllQuery<BankViewModel>
    {
        public class GetAllBankQueryHandler : GetAllQueryHandler<GetAllBankQuery, Bank, BankViewModel>
        {
            public GetAllBankQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<IEnumerable<BankViewModel>>> Handle(GetAllBankQuery request, CancellationToken cancellationToken)
            {
                var banks = await CacheRepository.GetAllAsync();
                var bankViews = Mapper.Map<IEnumerable<BankViewModel>>(banks);
                return new Response<IEnumerable<BankViewModel>>(bankViews);
            }
        }
    }
}
