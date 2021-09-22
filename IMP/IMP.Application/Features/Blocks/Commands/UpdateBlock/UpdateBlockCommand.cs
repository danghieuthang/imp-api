using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using Newtonsoft.Json;

namespace IMP.Application.Features.Blocks.Commands.UpdateBlock
{
    public class UpdateBlockCommand : ICommand<BlockViewModel>
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public int BlockTypeId { get; set; }
        public int ParentId { get; set; }
        public string Title { get; set; }
        public string Avatar { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; }
        public string Text { get; set; }
        public string TextArea { get; set; }
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
        public int Position { get; set; }
        [JsonIgnore]
        public int InfluencerId { get; set; }

        public class UpdateBlockCommandHandler : IRequestHandler<UpdateBlockCommand, Response<BlockViewModel>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IGenericRepository<Block> _blockRepository;
            private readonly IMapper _mapper;

            public UpdateBlockCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _blockRepository = _unitOfWork.Repository<Block>();
                _mapper = mapper;
            }

            public async Task<Response<BlockViewModel>> Handle(UpdateBlockCommand request, CancellationToken cancellationToken)
            {
                var block = await _blockRepository.GetByIdAsync(request.Id);
                if (block != null)
                {
                    _mapper.Map(request, block);
                    _blockRepository.Update(block);
                    await _unitOfWork.CommitAsync();
                    var view = _mapper.Map<BlockViewModel>(block);
                    return new Response<BlockViewModel>(view);
                }
                throw new ValidationException(new ValidationError("id","Block không tồn tại."));
            }
        }
    }
}