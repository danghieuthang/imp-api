using AutoMapper;
using IMP.Application.DTOs;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces.Repositories;
using IMP.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.CampaignTypes.Commands.DeleteCampaignTypeById
{
    public class DeleteCampaignTypeByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteCampaignTypeByIdCommandHandler : IRequestHandler<DeleteCampaignTypeByIdCommand, Response<int>>
        {
            private readonly ICampaignTypeRepositoryAsync _campaignTypeRepositoryAsync;
            private readonly IMapper _mapper;
            public DeleteCampaignTypeByIdCommandHandler(ICampaignTypeRepositoryAsync campaignTypeRepositoryAsync, IMapper mapper)
            {
                _campaignTypeRepositoryAsync = campaignTypeRepositoryAsync;
                _mapper = mapper;
            }
            public async Task<Response<int>> Handle(DeleteCampaignTypeByIdCommand request, CancellationToken cancellationToken)
            {
                var entity = await _campaignTypeRepositoryAsync.GetByIdAsync(request.Id);
                if (entity == null)
                {
                    var error = new ValidationError("id", $"'{request.Id}' không tồn tại");
                    throw new ValidationException(error);
                }

                await _campaignTypeRepositoryAsync.DeleteAsync(entity);
                return new Response<int>(entity.Id);
            }
        }
    }
}
