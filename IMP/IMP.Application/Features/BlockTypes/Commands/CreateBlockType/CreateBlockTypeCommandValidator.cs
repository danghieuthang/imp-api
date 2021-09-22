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

namespace IMP.Application.Features.BlockTypes.Commands.CreateBlockType
{
    public class CreateBlockTypeCommandValidator : AbstractValidator<CreateBlockTypeCommand>
    {
        private readonly IGenericRepository<BlockType> _blockTypeRepository;

        public CreateBlockTypeCommandValidator(IUnitOfWork unitOfWork, IOptions<FileSettings> options)
        {
            _blockTypeRepository = unitOfWork.Repository<BlockType>();
            RuleFor(x => x.Name).MustRequired(256)
                .MustAsync(IsUniQueBlockType).WithMessage("'{PropertyValue}' đã tồn tại.");

            RuleFor(x => x.Description).MustRequired(2000);
            RuleFor(x => x.ImageFile).SetValidator(new FileValidator(options));
        }

        public async Task<bool> IsUniQueBlockType(string name, CancellationToken cancellationToken)
        {
            return await _blockTypeRepository.FindSingleAsync(x => x.Name.ToLower().Equals(name.ToLower())) == null;

        }
    }
}
