using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Features.Blocks.Commands.UpdateBlockPosition
{
    public class UpdateBlockPositionCommandValidator : AbstractValidator<UpdateBlockPositionCommand>
    {
        public UpdateBlockPositionCommandValidator()
        {
            // RuleFor(x => x.ToPosition).Must((x, y) => x.FromPosition != x.ToPosition).WithMessage("Vị trí bị trùng");
        }
    }
}
