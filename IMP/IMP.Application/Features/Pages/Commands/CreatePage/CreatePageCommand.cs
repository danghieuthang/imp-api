using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IMP.Application.Helpers;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IMP.Application.Features.Pages.Commands.CreatePage
{
    public class CreatePageCommand : ICommand<PageViewModel>
    {
        [JsonIgnore]
        public int InfluencerId { get; set; }
        public string BackgroundType { get; set; }
        public int FontSize { get; set; }
        public string Background { get; set; }
        public string BioLink { get; set; }
        public string FontFamily { get; set; }
        public string TextColor { get; set; }

        public class CreatePageCommandHandler : CommandHandler<CreatePageCommand, PageViewModel>
        {
            private readonly IGenericRepository<Page> _pageRepository;
            public CreatePageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _pageRepository = unitOfWork.Repository<Page>();
            }

            public override async Task<Response<PageViewModel>> Handle(CreatePageCommand request, CancellationToken cancellationToken)
            {

                var entity = Mapper.Map<Page>(request);
                if (string.IsNullOrEmpty(entity.BioLink))
                {
                    entity.BioLink = await GenerateBioLink();
                }
                entity = await _pageRepository.AddAsync(entity);
                await UnitOfWork.CommitAsync();
                var view = Mapper.Map<PageViewModel>(entity);
                return new Response<PageViewModel>(view);
            }

            /// <summary>
            /// Generate biolink
            /// </summary>
            /// <returns></returns>
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
        }
    }
}