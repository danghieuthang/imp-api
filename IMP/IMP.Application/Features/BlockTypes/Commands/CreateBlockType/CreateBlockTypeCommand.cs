using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.BlockTypes.Commands.CreateBlockType
{
    public class CreateBlockTypeCommand : IRequest<Response<BlockTypeViewModel>>
    {
        [FromForm(Name = "name")]
        public string Name { get; set; }
        [FromForm(Name = "image")]
        public IFormFile ImageFile { get; set; }
        [FromForm(Name = "description")]
        public string Description { get; set; }

        public class CreateBlockTypeComandHandler : IRequestHandler<CreateBlockTypeCommand, Response<BlockTypeViewModel>>
        {
            private readonly IGenericRepositoryAsync<int, BlockType> _blockTypeRepositoryAsync;
            private readonly IMapper _mapper;
            private readonly IFirebaseService _firebaseService;

            public CreateBlockTypeComandHandler(IGenericRepositoryAsync<int, BlockType> blockTypeRepositoryAsync, IMapper mapper, IFirebaseService firebaseService)
            {
                _blockTypeRepositoryAsync = blockTypeRepositoryAsync;
                _mapper = mapper;
                _firebaseService = firebaseService;
            }

            public async Task<Response<BlockTypeViewModel>> Handle(CreateBlockTypeCommand request, CancellationToken cancellationToken)
            {
                var blockType = _mapper.Map<BlockType>(request);
                string imageFileUrl = await _firebaseService.UploadFile(request.ImageFile.OpenReadStream(), request.ImageFile.FileName, "admin", "campaign-types");
                if (imageFileUrl != null)
                {
                    blockType.Image = imageFileUrl;
                }

                blockType = await _blockTypeRepositoryAsync.AddAsync(blockType);
                var blockTypeView = _mapper.Map<BlockTypeViewModel>(blockType);
                return new Response<BlockTypeViewModel>(blockTypeView);
            }
        }
    }
}
