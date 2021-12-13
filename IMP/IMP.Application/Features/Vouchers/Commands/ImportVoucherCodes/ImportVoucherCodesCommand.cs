using AutoMapper;
using IMP.Application.Exceptions;
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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Vouchers.Commands.ImportVoucherCodes
{
    public class ImportVoucherCodesCommand : ICommand<bool>
    {
        [FromForm(Name = "voucher_id")]
        public int VoucherId { get; set; }
        [FromForm(Name = "file")]
        public IFormFile File { get; set; }

    }

    public class ImportVoucherCodesCommandHandler : CommandHandler<ImportVoucherCodesCommand, bool>
    {
        private readonly IGenericRepository<VoucherCode> _voucherCodeRepository;
        private readonly IGenericRepository<Voucher> _voucherRepository;
        public ImportVoucherCodesCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _voucherCodeRepository = unitOfWork.Repository<VoucherCode>();
            _voucherRepository = unitOfWork.Repository<Voucher>();
        }

        public override async Task<Response<bool>> Handle(ImportVoucherCodesCommand request, CancellationToken cancellationToken)
        {

            string path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Uploads/VoucherCodes");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var voucher = await _voucherRepository.FindSingleAsync(x => x.Id == request.VoucherId, x => x.VoucherCodes);
            if (voucher == null)
            {
                throw new ValidationException(new ValidationError("voucher_id", "Không hợp lệ."));
            }

            List<VoucherCode> codes = new List<VoucherCode>();

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
                        //int quantity;

                        //int.TryParse(row.GetCell(row.FirstCellNum + 1).ToString(), out quantity);
                        codes.Add(new VoucherCode
                        {
                            Code = row.GetCell(row.FirstCellNum).ToString().ToUpper(),
                            Quantity = voucher.Quantity,
                            VoucherId = request.VoucherId,
                        });
                    }
                }
                catch
                {
                    throw new ValidationException(new ValidationError("file", "File không hơp lệ."));
                }

            }

            var newVoucherCodes = new List<VoucherCode>();

            foreach (var code in voucher.VoucherCodes)
            {
                var codeExist = codes.Where(x => x.Code.ToUpper() == code.Code.ToUpper()).FirstOrDefault();
                if (codeExist != null)
                {
                    code.Quantity = codeExist.Quantity;
                    codes.Remove(codeExist);
                }

                newVoucherCodes.Add(code);
            }
            voucher.VoucherCodes = newVoucherCodes.Union(codes.Distinct(new VoucherCodeComparer())).ToList();
            _voucherRepository.Update(voucher);
            await UnitOfWork.CommitAsync();

            return new Response<bool>(true, message: "Import thành công.");
        }

        internal class VoucherCodeComparer : IEqualityComparer<VoucherCode>
        {
            public bool Equals(VoucherCode code1, VoucherCode code2)
            {
                if (code1.Code.ToString().ToUpper() == code2.Code.ToString().ToUpper())
                {
                    return true;
                }
                return false;
            }

            public int GetHashCode([DisallowNull] VoucherCode obj)
            {
                return obj.Code.ToUpper().GetHashCode();
            }
        }
    }
}
