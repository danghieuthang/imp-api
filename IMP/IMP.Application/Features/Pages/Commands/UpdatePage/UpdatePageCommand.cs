using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IMP.Application.Features.Blocks.Commands.CreateBlock;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using IMP.Application.Helpers;
using IMP.Application.Enums;

namespace IMP.Application.Features.Pages.Commands.UpdatePage
{
    public class UpdateBlockRequest
    {
        public int Id { get; set; }
        public int BlockTypeId { get; set; }
        public string Variant { get; set; }
        public int Position { get; set; }
        [JsonProperty("items")]
        public JObject Data { get; set; }
        public bool Enable { get; set; }
        public List<UpdateBlockRequest> ChildBlocks { get; set; }
    }
    public class UpdatePageCommand : ICommand<PageViewModel>
    {
        public int Id { get; set; }
        [JsonIgnore]
        public int InfluencerId { get; set; }
        public string BackgroundType { get; set; }
        public double FontSize { get; set; }
        public string Background { get; set; }
        public string BioLink { get; set; }
        public string FontFamily { get; set; }
        public string TextColor { get; set; }
        public PageStatus Status { get; set; }
        public List<UpdateBlockRequest> Blocks { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public class UpdatePageCommandHandler : CommandHandler<UpdatePageCommand, PageViewModel>
        {
            private readonly IGenericRepository<Page> _pageRepository;

            public UpdatePageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _pageRepository = unitOfWork.Repository<Page>();
            }

            public override async Task<Response<PageViewModel>> Handle(UpdatePageCommand request, CancellationToken cancellationToken)
            {
                var pageRequest = Mapper.Map<Page>(request);

                var pageDomain = await _pageRepository.FindSingleAsync(x => x.Id == request.Id,
                    include: pages =>
                        pages.Include(page => page.Blocks)
                                .ThenInclude(block => block.Items)
                             .Include(y => y.Blocks)
                                .ThenInclude(block => block.ChildBlocks).ThenInclude(x => x.Items));

                if (pageDomain.Status != (int)PageStatus.UnPublished)
                {
                    return new Response<PageViewModel>(error: new Models.ValidationError("id", "Chỉ được update page chưa published." ));
                }
                if (pageDomain != null)
                {
                    await RemoveBlockBeforeUpdate(pageDomain, pageRequest);
                    // re get data after remove
                    //pageDomain = await _pageRepository.FindSingleAsync(x => x.Id == request.Id,
                    //    include: pages =>
                    //        pages.Include(page => page.Blocks)
                    //                .ThenInclude(block => block.Items)
                    //             .Include(y => y.Blocks)
                    //                .ThenInclude(block => block.ChildBlocks).ThenInclude(x => x.Items));

                    //await UpdatePage(pageDomain);
                    if (string.IsNullOrEmpty(pageRequest.BioLink))
                    {
                        pageRequest.BioLink = pageDomain.BioLink;
                    }

                    _pageRepository.Update(pageRequest);
                    await UnitOfWork.CommitAsync();

                    var view = Mapper.Map<PageViewModel>(pageRequest);
                    return new Response<PageViewModel>(view);
                }
                return new Response<PageViewModel>(error: new Models.ValidationError("id", "KHông tồn tại."));

            }
            private async Task<string> GenerateBioLink()
            {
                var random = new Random();
                // random biolink length
                int size = random.Next(10, 15);
                string biolink = StringHelper.RandomString(size: size, lowerCase: true);
                while (await _pageRepository.IsExistAsync(x => x.BioLink == biolink))
                {
                    biolink = StringHelper.RandomString(size: size, lowerCase: true);
                }
                return biolink;
            }
            public async Task UpdatePage(Page page)
            {
                foreach (var block in page.Blocks)
                {
                    block.PageId = page.Id;
                    await CreateOrUpdateBlock(block);
                }

            }

            public async Task CreateOrUpdateBlock(Block block)
            {
                // block id= 0 then create new block
                if (block.Id == 0)
                {
                    await UnitOfWork.Repository<Block>().AddAsync(block);
                    await UnitOfWork.CommitAsync();
                }
                else
                {
                    // Add item to block update
                    var items = block.Items.Select(x => new BlockItem { Key = x.Key, Value = x.Value, BlockId = block.Id }).ToList();
                    await UnitOfWork.Repository<BlockItem>().AddManyAsync(items);
                    await UnitOfWork.CommitAsync();

                    // Create or update child block
                    foreach (var childBlock in block.ChildBlocks)
                    {
                        childBlock.ParentId = block.Id;
                        await CreateOrUpdateBlock(childBlock);
                        await UnitOfWork.CommitAsync();
                    }
                }

            }
            public async Task RemoveBlockBeforeUpdate(Page pageDomain, Page pageRequest)
            {
                await RemoveBlockBeforeUpdate(domainBlocks: pageDomain.Blocks, pageRequest.Blocks);
            }

            public async Task RemoveBlockBeforeUpdate(IEnumerable<Block> domainBlocks, IEnumerable<Block> requestBlocks)
            {
                if (domainBlocks == null || domainBlocks.Count() == 0)
                {
                    return;
                }

                if (requestBlocks == null)
                {
                    requestBlocks = new List<Block>();
                }

                var requestBlockIds = requestBlocks.Select(x => x.Id).ToList();
                foreach (var domainBlock in domainBlocks)
                {
                    // remove the block not post in request
                    if (!requestBlockIds.Contains(domainBlock.Id))
                    {
                        UnitOfWork.Repository<Block>().Delete(domainBlock);
                        await UnitOfWork.CommitAsync();
                    }
                    else
                    {
                        // remove item in block
                        if (domainBlock.Items != null)
                        {
                            foreach (var item in domainBlock.Items)
                            {
                                UnitOfWork.Repository<BlockItem>().DeleteCompletely(item);
                            }
                        }
                        await RemoveBlockBeforeUpdate(domainBlocks: domainBlock.ChildBlocks, requestBlocks: requestBlocks.Where(x => x.Id == domainBlock.Id).FirstOrDefault().ChildBlocks);
                        await UnitOfWork.CommitAsync();
                    }
                }
            }

        }

    }
}