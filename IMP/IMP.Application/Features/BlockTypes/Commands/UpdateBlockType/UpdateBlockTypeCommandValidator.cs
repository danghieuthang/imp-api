using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Application.Validations;
using IMP.Domain.Entities;
using IMP.Domain.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.BlockTypes.Commands.UpdateBlockType
{
    public class UpdateBlockTypeCommandValidator : AbstractValidator<UpdateBlockTypeCommand>
    {
        private readonly IGenericRepository<BlockType> _blockTypeRepository;

        public UpdateBlockTypeCommandValidator(IUnitOfWork unitOfWork, IOptions<FileSettings> options)
        {
            _blockTypeRepository = unitOfWork.Repository<BlockType>();
            RuleFor(x => x.Id).MustExistEntityId(IsExistAsync);
            RuleFor(x => x.Name).MustRequired(256)
                .MustAsync(async (x, y, z) =>
                {
                    return await IsUniQueBlockType(x.Id, y, z);
                }).WithMessage("'{PropertyValue}' đã tồn tại.");

            RuleFor(x => x.Description).MustRequired(2000);
            RuleFor(x => x.ImageFile).SetValidator(new FileValidator(options));
        }

        public async Task<bool> IsExistAsync(int id, CancellationToken cancellationToken)
        {
            return await _blockTypeRepository.IsExistAsync(id);
        }

        public async Task<bool> IsUniQueBlockType(int id, string name, CancellationToken cancellationToken)
        {
            var entity = await _blockTypeRepository.FindSingleAsync(x => x.Id != id && name.ToLower() == x.Name.ToLower());
            return entity == null;
        }
    }
}
