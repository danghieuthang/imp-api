//using AutoMapper;
//using IMP.Application.Exceptions;
//using IMP.Application.Interfaces;
//using IMP.Application.Models;
//using IMP.Application.Wrappers;
//using IMP.Domain.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace IMP.Application.Features.Campaigns.Commands.ApplyToCampaign
//{
//    public class ApplyToCampaignCommand : ICommand<bool>
//    {
//        public int CampaignId { get; set; }

//        public class ApplyToCampaignCommandHandler : CommandHandler<ApplyToCampaignCommand, bool>
//        {
//            public ApplyToCampaignCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
//            {
//            }

//            public override async Task<Response<bool>> Handle(ApplyToCampaignCommand request, CancellationToken cancellationToken)
//            {
//                var campaign = await UnitOfWork.Repository<Campaign>().GetByIdAsync(request.CampaignId);
//                if (campaign == null)
//                {
//                    throw new ValidationException(new ValidationError)
//                }
//            }
//        }
//    }
//}
