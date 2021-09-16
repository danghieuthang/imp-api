using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.ApplicationUsers.Commands.UpdateUserInfomation
{
    public class UpdateUserInformationCommand : ICommand<ApplicationUserViewModel>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Nickname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public int? LocationId { get; set; }
        [JsonProperty("jobs")]
        public List<string> JobR { get; set; }
        [JsonProperty("interests")]
        public List<string> InterestsR { get; set; }
        public bool? ChildStatus { get; set; }
        public bool? MaritalStatus { get; set; }
        [JsonProperty("pets")]
        public List<string> PetR { get; set; }
        public string Description { get; set; }
    }

    public class UpdateUserInformationCommandHandler : IRequestHandler<UpdateUserInformationCommand, Response<ApplicationUserViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepositoryAsync<ApplicationUser> _applicationUserRepostoryAsync;
        private readonly IMapper _mapper;

        public UpdateUserInformationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _applicationUserRepostoryAsync = _unitOfWork.Repository<ApplicationUser>();
        }

        public async Task<Response<ApplicationUserViewModel>> Handle(UpdateUserInformationCommand request, CancellationToken cancellationToken)
        {
            var entity = await _applicationUserRepostoryAsync.FindSingleAsync(x => x.Id == request.Id);
            _mapper.Map(request, entity);

            // process list
            entity.Job = string.Join(";", request.JobR);
            entity.Interests = string.Join(";", request.InterestsR);
            entity.Pet = string.Join(";", request.PetR);

            _applicationUserRepostoryAsync.Update(entity);
            await _unitOfWork.CommitAsync();

            var view = _mapper.Map<ApplicationUserViewModel>(entity);
            view.InterestsR = entity.Interests?.Split(";").ToList();
            view.JobsR = entity.Job?.Split(";").ToList();
            view.PetsR = entity.Pet?.Split(";").ToList();

            return new Response<ApplicationUserViewModel>(view);
        }
    }
}
