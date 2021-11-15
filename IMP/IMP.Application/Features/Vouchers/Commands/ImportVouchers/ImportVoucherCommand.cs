using AutoMapper;
using FluentValidation;
using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Vouchers.Commands.ImportVouchers
{
    public class ImportVoucherCommand : ICommand<bool>
    {
        [FromForm(Name = "file")]
        public IFormFile File { get; set; }
        public class ImportVoucherCommandValidator : AbstractValidator<ImportVoucherCommand>
        {
            public ImportVoucherCommandValidator()
            {
                List<string> valideExtensions = new List<string> { ".xlsx", ".xls" };
                RuleFor(x => x.File).Must(x => valideExtensions.Contains(Path.GetExtension(x.FileName))).WithMessage("File không đúng định dạng xlsx.");
            }
        }

        public class ImportVoucherCommandHandler : CommandHandler<ImportVoucherCommand, bool>
        {
            private readonly IGenericRepository<Voucher> _voucherRepository;
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public ImportVoucherCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
                _voucherRepository = unitOfWork.Repository<Voucher>();
            }

            public override async Task<Response<bool>> Handle(ImportVoucherCommand request, CancellationToken cancellationToken)
            {
                string path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Uploads/Vouchers");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                List<Voucher> vouchers = new();

                //Save file
                string fileName = Path.GetFileName(request.File.FileName);
                string filePath = Path.Combine(path, fileName);
                string fileExtension = fileName.Split(".")[^1];
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ISheet sheet;
                    request.File.CopyTo(stream);
                    //
                    stream.Position = 0;
                    // Excel  97-2000 formats
                    if (fileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                        sheet = hssfwb.GetSheetAt(0);
                    }
                    else // 2007 excel format
                    {
                        XSSFWorkbook wssfwb = new XSSFWorkbook(stream);
                        sheet = wssfwb.GetSheetAt(0);
                    }
                    try
                    {
                        // Get data
                        for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null) continue;
                            // get quantity
                            int quantity;
                            int.TryParse(row.GetCell(row.FirstCellNum + 4).ToString(), out quantity);

                            //Get Discount Value
                            decimal discountValue;
                            decimal.TryParse(row.GetCell(row.FirstCellNum + 2).ToString(), out discountValue);

                            //Get discount Value type
                            int? discountValueType = null;
                            switch (row.GetCell(row.FirstCellNum + 3).ToString().ToLower())
                            {
                                case "mức cố định":
                                    discountValueType = (int)DiscountValueType.Fixed;
                                    break;
                                case "theo phần trăm":
                                    discountValueType = (int)DiscountValueType.Percentage;
                                    break;

                                case "miễn phí giao hàng":
                                    discountValueType = (int)DiscountValueType.FreeShipping;
                                    break;
                            }

                            if (discountValueType.HasValue)
                            {
                                vouchers.Add(new Voucher
                                {
                                    Code = row.GetCell(row.FirstCellNum).ToString(),
                                    VoucherName = row.GetCell(row.FirstCellNum + 1).ToString(),
                                    Quantity = quantity,
                                    BrandId = _authenticatedUserService.BrandId.Value,
                                    DiscountValue = discountValue,
                                    DiscountValueType = discountValueType.Value
                                });
                            }

                        }
                    }
                    catch
                    {
                        throw new IMP.Application.Exceptions.ValidationException(new ValidationError("file", "File không hơp lệ."));
                    }

                }

                foreach (var voucher in vouchers)
                {
                    _voucherRepository.Add(voucher);
                }

                await UnitOfWork.CommitAsync();

                return new Response<bool>(true, message: "Import thành công.");
            }
        }
    }
}
