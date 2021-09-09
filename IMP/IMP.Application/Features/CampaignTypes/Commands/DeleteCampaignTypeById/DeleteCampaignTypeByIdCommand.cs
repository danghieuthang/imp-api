using AutoMapper;
using IMP.Application.DTOs;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Repositories;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.CampaignTypes.Commands.DeleteCampaignTypeById
{
    public class DeleteCampaignTypeByIdCommand : IDeleteCommand<CampaignType>
    {
        public int Id { get; set; }
        public class DeleteCampaignTypeByIdCommandHandler : DeleteCommandHandler<CampaignType, DeleteCampaignTypeByIdCommand>
        {
            public DeleteCampaignTypeByIdCommandHandler(ICampaignTypeRepositoryAsync repositoryAsync) : base(repositoryAsync)
            {
            }
        }
    }
}
