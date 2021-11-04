﻿using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Vouchers.Queries.GetVoucherById
{
    public class GetVoucherByIdQuery : IGetByIdQuery<Voucher, VoucherViewModel>
    {
        public int Id { get; set; }
        public class GetVoucherByIdQueryHandler : GetByIdQueryHandler<GetVoucherByIdQuery, Voucher, VoucherViewModel>
        {
            public GetVoucherByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<VoucherViewModel>> Handle(GetVoucherByIdQuery request, CancellationToken cancellationToken)
            {
                var voucher = await Repository.FindSingleAsync(
                        predicate: x => x.Id == request.Id,
                        include: vouchers => vouchers.Include(voucher => voucher.CampaignVouchers).Include(voucher => voucher.VoucherCodes));
                if (voucher == null)
                {
                    throw new KeyNotFoundException();
                }
                var voucherView = Mapper.Map<VoucherViewModel>(voucher);
                return new Response<VoucherViewModel>(voucherView);

            }
        }
    }
}