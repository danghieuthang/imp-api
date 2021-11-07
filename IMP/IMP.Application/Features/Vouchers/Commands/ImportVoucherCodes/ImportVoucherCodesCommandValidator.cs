using FluentValidation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Features.Vouchers.Commands.ImportVoucherCodes
{
    public class ImportVoucherCodesCommandValidator : AbstractValidator<ImportVoucherCodesCommand>
    {
        public ImportVoucherCodesCommandValidator()
        {
            List<string> valideExtensions = new List<string> { ".xlsx", ".xls" };
            RuleFor(x => x.File).Must(x => valideExtensions.Contains(Path.GetExtension(x.FileName))).WithMessage("File không đúng định dạng xlsx.");
        }
    }
}
