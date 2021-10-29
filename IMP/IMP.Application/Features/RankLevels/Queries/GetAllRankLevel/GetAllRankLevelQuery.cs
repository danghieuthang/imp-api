using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMP.Domain.Entities;
using AutoMapper;

namespace IMP.Application.Features.RankLevels.Queries.GetAllRankLevel
{
    public class GetAllRankLevelQuery : IGetAllQuery<RankLevelViewModel>
    {
        public class GetAllRankLevelQueryHandler : GetAllQueryHandler<GetAllRankLevelQuery, RankLevel, RankLevelViewModel>
        {
            public GetAllRankLevelQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }
        }
    }
}
