using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.WalletTransactions.Queries.GetTransactionsByWalletId
{
    public class GetWalletTransactionByWalletIdQuery : PageRequest, IListQuery<WalletTransactionViewModel>
    {
        [FromQuery(Name = "transaction_type")]
        public TransactionType? TranscationType { get; set; }
        [FromQuery(Name = "transaction_status")]
        public WalletTransactionStatus? TransactionStatus { get; set; }
        private int _applicationUserId;
        [FromQuery(Name = "from_date")]
        public DateTime? FromDate { get; set; }
        [FromQuery(Name = "to_date")]
        public DateTime? ToDate { get; set; }

        public void SetApplicationUserId(int walletId)
        {
            _applicationUserId = walletId;
        }

        public class GetWalletTransactionByWalletIdQueryHandler : ListQueryHandler<GetWalletTransactionByWalletIdQuery, WalletTransactionViewModel>
        {
            private readonly IGenericRepository<WalletTransaction> _walletTransactionRepository;
            public GetWalletTransactionByWalletIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _walletTransactionRepository = unitOfWork.Repository<WalletTransaction>();
            }

            public override async Task<Response<IPagedList<WalletTransactionViewModel>>> Handle(GetWalletTransactionByWalletIdQuery request, CancellationToken cancellationToken)
            {
                var page = await _walletTransactionRepository.GetPagedList(predicate: x =>
                   (x.WalletTo.ApplicationUserId == request._applicationUserId
                    || x.WalletFrom.ApplicationUserId == request._applicationUserId
                    || x.Sender.Id == request._applicationUserId
                    || x.Receiver.Id == request._applicationUserId)
                    && (!request.FromDate.HasValue || x.Created.Date >= request.FromDate.Value.Date)
                    && (!request.ToDate.HasValue || x.Created.Date <= request.ToDate.Value.Date),
                    include: x => x.Include(t => t.Sender).Include(t => t.Receiver),
                        orderBy: request.OrderField,
                        orderByDecensing: request.OrderBy == OrderBy.DESC,
                        pageIndex: request.PageIndex,
                        pageSize: request.PageSize,
                        cancellationToken: cancellationToken);

                page.Items = page.Items.Select(x =>
                {
                    var tran = x;
                    if (x.SenderId != request._applicationUserId)
                    {
                        x.SenderBalance = null;
                    }
                    if (x.ReceiverId != request._applicationUserId)
                    {
                        x.ReceiverBalance = null;
                    }
                    return x;
                }).ToList();

                var pageView = page.ToResponsePagedList<WalletTransactionViewModel>(Mapper);
                return new Response<IPagedList<WalletTransactionViewModel>>(pageView);
            }
        }
    }
}
