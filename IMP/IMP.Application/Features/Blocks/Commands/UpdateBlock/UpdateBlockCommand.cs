using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IMP.Application.Exceptions;
using IMP.Application.Features.Blocks.Commands.CreateBlock;
using IMP.Application.Features.Pages.Commands.UpdatePage;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IMP.Application.Features.Blocks.Commands.UpdateBlock
{

    public class UpdateBlockCommand : UpdateBlockRequest, ICommand<BlockViewModel>
    {
        [JsonIgnore]
        public int InfluencerId { get; set; }

        public class UpdateBlockCommandHandler : CommandHandler<UpdateBlockCommand, BlockViewModel>
        {
            private readonly IGenericRepository<Block> _blockRepository;
            private readonly IGenericRepository<BlockItem> _blockItemRepository;

            public UpdateBlockCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _blockItemRepository = unitOfWork.Repository<BlockItem>();
                _blockRepository = unitOfWork.Repository<Block>();
            }

            public override async Task<Response<BlockViewModel>> Handle(UpdateBlockCommand request, CancellationToken cancellationToken)
            {
                var blockDomain = await _blockRepository.FindSingleAsync(x => x.Id == request.Id,
                            include: x => x.Include(block => block.Items)
                                            .Include(block => block.ChildBlocks).ThenInclude(x => x.Items));
                if (blockDomain != null)
                {
                    var blockRequest = Mapper.Map<Block>(request);

                    // Process Child blocks
                    ProcessBlock(blockDomain, blockRequest);

                    Mapper.Map(request, blockDomain);

                    _blockRepository.Update(blockDomain);

                    await UnitOfWork.CommitAsync();

                    var view = Mapper.Map<BlockViewModel>(blockDomain);
                    return new Response<BlockViewModel>(view);
                }
                throw new ValidationException(new ValidationError("id", "Block không tồn tại."));
            }

            /// <summary>
            /// Process Block before update
            /// </summary>
            /// <param name="domainBlock"></param>
            /// <param name="requestBlock"></param>
            private void ProcessBlock(Block domainBlock, Block requestBlock)
            {

                DeleteItemsOfBlock(domainBlock);

                var blockRequestIds = requestBlock.ChildBlocks.Select(x => x.Id).ToList();
                foreach (var block in domainBlock.ChildBlocks)
                {
                    // If block not in request, then remove block from page
                    if (!blockRequestIds.Contains(block.Id))
                    {
                        // Remove item in block
                        if (block.Items.Count > 0)
                        {
                            DeleteItemsOfBlock(block);
                        }
                        _blockRepository.Delete(block);
                        continue;
                    }

                    // remove items if block in request.
                    if (block.Items.Count > 0)
                    {
                        DeleteItemsOfBlock(block);
                    }
                }
            }

            private void DeleteItemsOfBlock(Block block)
            {
                foreach (var item in block.Items)
                {
                    _blockItemRepository.DeleteCompletely(item);
                }
            }

            private async Task ProcessItem(Block block, Block blockRequest)
            {
                // Update value for exist item
                var domainItems = block.Items.ToDictionary(x => x.Key);
                foreach (var item in blockRequest.Items)
                {
                    if (domainItems.TryGetValue(item.Key, out BlockItem blockItem))
                    {
                        if (blockItem.Value != item.Value)
                        {
                            blockItem.Value = item.Value;
                            _blockItemRepository.Update(blockItem);
                        }
                    }
                    else
                    {
                        item.BlockId = block.Id;
                        await _blockItemRepository.AddAsync(item);
                    }
                }
            }
        }
    }
}