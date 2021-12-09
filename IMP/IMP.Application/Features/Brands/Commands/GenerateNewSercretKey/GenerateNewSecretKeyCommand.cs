using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Brands.Commands.GenerateNewSercretKey
{
    public class GenerateNewSecretKeyCommand : ICommand<string>
    {
        public string Message { get; set; }
        public class GenerateNewSecretKeyCommandHandler : CommandHandler<GenerateNewSecretKeyCommand, string>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            private static string PrivateKey = "ngayxuaEmDenNhuMotCongio@123";
            public GenerateNewSecretKeyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<string>> Handle(GenerateNewSecretKeyCommand request, CancellationToken cancellationToken)
            {
                var brand = await UnitOfWork.Repository<Brand>().GetByIdAsync(_authenticatedUserService.BrandId.Value);
                if (brand != null)
                {
                    string newSecretKey = Utils.Utils.HmacSHA512(PrivateKey, request.Message);
                    brand.SecretKey = newSecretKey;
                    UnitOfWork.Repository<Brand>().Update(brand);
                    await UnitOfWork.CommitAsync();
                    return new Response<string>(data: newSecretKey);
                }
                throw new KeyNotFoundException();
            }
        }
    }
}
