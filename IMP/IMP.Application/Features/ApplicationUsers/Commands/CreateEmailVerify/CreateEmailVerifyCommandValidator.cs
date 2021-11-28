using FluentValidation;
using IMP.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Features.ApplicationUsers.Commands.CreateEmailVerify
{
    public class CreateEmailVerifyCommandValidator : AbstractValidator<CreateEmailVerifyCommand>
    {
        public CreateEmailVerifyCommandValidator()
        {
            RuleFor(x => x.Email).MustValidEmail(allowNull: true);
        }
    }
}
