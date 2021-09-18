using FluentValidation;
using IMP.Application.Interfaces;
using IMP.Domain.Entities;

namespace IMP.Application.Features.Pages.Commands.DeletePage
{
    public class DeletePageCommandValidator : AbstractValidator<DeletePageCommand>
    {
        public DeletePageCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.Id).MustAsync(async (x, y, z) =>
              {
                  return await unitOfWork.Repository<Page>().IsExistAsync(x => x.Id == y && x.InfluencerId == x.InfluencerId);
              }).WithMessage("Không có quyền xóa.");
        }
    }
}