using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Features.EvidenceTypes.Queries.GetAllEvidenceType
{
    public class GetAllEvidenceTypeQuery : IGetAllQuery<EvidenceTypeViewModel>
    {
        public class GetAllEvidenceTypeQueryHandler : GetAllQueryHandler<GetAllEvidenceTypeQuery, EvidenceType, EvidenceTypeViewModel>
        {
            public GetAllEvidenceTypeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }
        }
    }
}
