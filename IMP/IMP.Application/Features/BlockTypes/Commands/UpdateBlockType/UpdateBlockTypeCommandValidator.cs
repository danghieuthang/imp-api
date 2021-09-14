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
        private readonly IGenericRepositoryAsync<BlockType> _blockTypeRepositoryAsync;

        public UpdateBlockTypeCommandValidator(IUnitOfWork unitOfWork, IOptions<FileSettings> options)
        {
            _blockTypeRepositoryAsync = unitOfWork.Repository<BlockType>();
            RuleFor(x => x.Id).IsExistId(IsExistAsync);
            RuleFor(x => x.Name).Required(256)
                .MustAsync(async (x, y, z) =>
                {
                    return await IsUniQueBlockType(x.Id, y, z);
                }).WithMessage("'{PropertyValue}' đã tồn tại.");

            RuleFor(x => x.Description).Required(2000);
            RuleFor(x => x.ImageFile).SetValidator(new FileValidator(options));
        }

        public async Task<bool> IsExistAsync(int id, CancellationToken cancellationToken)
        {
            return await _blockTypeRepositoryAsync.IsExistAsync(id);
        }

        public async Task<bool> IsUniQueBlockType(int id, string name, CancellationToken cancellationToken)
        {
            var entity = await _blockTypeRepositoryAsync.FindSingleAsync(x => x.Id != id && name.ToLower() == x.Name.ToLower());
            return entity == null;
        }
    }
}
