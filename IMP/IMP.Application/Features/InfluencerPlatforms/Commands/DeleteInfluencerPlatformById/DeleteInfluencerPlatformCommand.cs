using IMP.Application.Interfaces;
using IMP.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Features.InfluencerPlatforms.Commands.DeleteInfluencerPlatformById
{
    public class DeleteInfluencerPlatformCommand : IDeleteCommand<InfluencerPlatform>
    {
        public int Id { get; set; }

        public class DeleteInfluencerPlatformCommandHandler : DeleteCommandHandler<InfluencerPlatform, DeleteInfluencerPlatformCommand>
        {
            public DeleteInfluencerPlatformCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
            {
            }
        }
    }
}
