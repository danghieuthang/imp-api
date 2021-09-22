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

namespace IMP.Application.Features.BlockTypes.Commands.UpdateBlockType
{
    public class UpdateBlockTypeCommand : IRequest<Response<BlockTypeViewModel>>
    {
        [FromForm(Name = "id")]
        public int Id { get; set; }
        [FromForm(Name = "name")]
        public string Name { get; set; }
        [FromForm(Name = "image")]
        public IFormFile ImageFile { get; set; }
        [FromForm(Name = "description")]
        public string Description { get; set; }
        public class UpdateBlockTypeComandHandler : IRequestHandler<UpdateBlockTypeCommand, Response<BlockTypeViewModel>>
        {

            private readonly IUnitOfWork _unitOfWork;
            private readonly IGenericRepository<BlockType> _blockTypeRepository;
            private readonly IMapper _mapper;
            private readonly IFirebaseService _firebaseService;

            public UpdateBlockTypeComandHandler(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseService firebaseService)
            {
                _unitOfWork = unitOfWork;
                _blockTypeRepository = _unitOfWork.Repository<BlockType>();
                _mapper = mapper;
                _firebaseService = firebaseService;
            }

            public async Task<Response<BlockTypeViewModel>> Handle(UpdateBlockTypeCommand request, CancellationToken cancellationToken)
            {
                var blockType = await _blockTypeRepository.GetByIdAsync(request.Id);
                if (blockType != null)
                {
                    blockType.Name = request.Name;
                    blockType.Description = request.Description;
                    string imageFileUrl = await _firebaseService.UploadFile(request.ImageFile.OpenReadStream(), request.ImageFile.FileName, "admin", "campaign-types");
                    if (imageFileUrl != null)
                    {
                        blockType.Image = imageFileUrl;
                    }
                    _blockTypeRepository.Update(blockType);
                    await _unitOfWork.CommitAsync();
                }
                var blockTypeView = _mapper.Map<BlockTypeViewModel>(blockType);
                return new Response<BlockTypeViewModel>(blockTypeView);
            }
        }
    }
}
