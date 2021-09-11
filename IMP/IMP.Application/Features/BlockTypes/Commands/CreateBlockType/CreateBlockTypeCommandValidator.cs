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
        private readonly IGenericRepositoryAsync<int, BlockType> _blockTypeRepositoryAsync;

        public CreateBlockTypeCommandValidator(IGenericRepositoryAsync<int, BlockType> blockTypeRepositoryAsync, IOptions<FileSettings> options)
        {
            _blockTypeRepositoryAsync = blockTypeRepositoryAsync;
            RuleFor(x => x.Name).Required(256)
                .MustAsync(IsUniQueBlockType).WithMessage("'{PropertyValue}' đã tồn tại.");

            RuleFor(x => x.Description).Required(2000);
            RuleFor(x => x.ImageFile).SetValidator(new FileValidator(options));
        }

        public async Task<bool> IsUniQueBlockType(string name, CancellationToken cancellationToken)
        {
            return await _blockTypeRepositoryAsync.FindSingleAsync(x => x.Name.ToLower().Equals(name.ToLower())) == null;

        }
    }
}
