using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.WalletTransactions.Queries.GetAllTransactions
{
    public class GetAllTransactionsQuery : PageRequest, IListQuery<WalletTransactionViewModel>
    {
        [FromQuery(Name = "transaction-type")]
        public TransactionType? TranscationType { get; set; }
        [FromQuery(Name = "transaction-status")]
        public WalletTransactionStatus? TransactionStatus { get; set; }
        [FromQuery(Name = "from-date")]
        public DateTime? FromDate { get; set; }
        [FromQuery(Name = "to-date")]
        public DateTime? ToDate { get; set; }
        [FromQuery(Name = "transaction-no")]
        public string TransactionNo { get; set; }

        public class GetAllTransactionQueryHandler : ListQueryHandler<GetAllTransactionsQuery, WalletTransactionViewModel>
        {
            private readonly IGenericRepository<WalletTransaction> _walletTransactionRepository;

            public GetAllTransactionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _walletTransactionRepository = unitOfWork.Repository<WalletTransaction>();
            }
            public override async Task<Response<IPagedList<WalletTransactionViewModel>>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
            {
                var page = await _walletTransactionRepository.GetPagedList(predicate: x =>
                  (!request.TranscationType.HasValue || x.TransactionType == (int)request.TranscationType.Value)
                  && (!request.TransactionStatus.HasValue || x.TransactionStatus == (int)request.TransactionStatus.Value)
                  && (string.IsNullOrEmpty(request.TransactionNo) || x.TransactionNo.Contains(request.TransactionNo))
                  && (!request.FromDate.HasValue || x.Created.Date >= request.FromDate.Value.Date)
                  && (!request.ToDate.HasValue || x.Created.Date <= request.ToDate.Value.Date),
                      orderBy: request.OrderField,
                      orderByDecensing: request.OrderBy == OrderBy.DESC,
                      pageIndex: request.PageIndex,
                      pageSize: request.PageSize,
                      cancellationToken: cancellationToken);
                var pageView = page.ToResponsePagedList<WalletTransactionViewModel>(mapper: Mapper);
                return new Response<IPagedList<WalletTransactionViewModel>>(pageView);
            }
        }
    }
}
