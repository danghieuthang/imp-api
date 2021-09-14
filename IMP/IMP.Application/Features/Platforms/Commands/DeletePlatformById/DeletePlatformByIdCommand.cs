using IMP.Application.Models;
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
using IMP.Application.Interfaces;
using IMP.Domain.Entities;

namespace IMP.Application.Features.Platforms.Commands.DeletePlatformById
{
    public class DeletePlatformByIdCommand : IDeleteCommand<Platform>
    {
        public int Id { get; set; }

        public class DeletePlatformByIdCommandHandler : DeleteCommandHandler<Platform, DeletePlatformByIdCommand>
        {
            public DeletePlatformByIdCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
            {
            }
        }
    }
}
