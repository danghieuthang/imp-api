using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Extensions
{
    public static class GenericValidatorExtensions
    {
        /// <summary>
        /// Check list must contain fewer than {num} item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, IList<TElement>> ListMustContainFewerThan<T, TElement>(this IRuleBuilder<T, IList<TElement>> ruleBuilder, int num)
        {
            return ruleBuilder.Must(list => list.Count <= num).WithMessage($"List không được chứa nhiều hơn {num} item.");
        }
        public static IRuleBuilderOptions<T, int> MustPositiveInteger<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder.GreaterThan(0).WithMessage("Giá trị phải lớn hơn 0.");
        }
        /// <summary>
        /// Custom validation for string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="maxLength">Max length of property.</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> MustRequired<T>(this IRuleBuilder<T, string> ruleBuilder, int maxLength)
        {
            return ruleBuilder
                .NotNull().NotEmpty().WithMessage("{PropertyName} chưa có dữ liệu.")
                .MaximumLength(maxLength).WithMessage("{PropertyName} không thể quá " + maxLength + " ký tự.");
        }

        /// <summary>
        /// Check valid maxlength of string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> MustMaxLength<T>(this IRuleBuilder<T, string> ruleBuilder, int maxLength)
        {
            return ruleBuilder
                .MaximumLength(maxLength).WithMessage("{PropertyName} không thể quá " + maxLength + " ký tự.");
        }

        /// <summary>
        /// Check date is feature date
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, DateTime?> MustValidDate<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
        {
            return ruleBuilder.GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("Ngày không hợp lệ.");
        }
        public static IRuleBuilderOptions<T, DateTime> MustGreaterThanNow<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder.GreaterThan(DateTime.UtcNow).WithMessage("Ngày phải lớn hơn hiện tại.");
        }
        /// <summary>
        /// Check date is feature date
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, DateTime> MustValidDate<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder.GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("Ngày không hợp lệ.");
        }
        public static IRuleBuilderOptions<T, TimeSpan?> MustValidTime<T>(this IRuleBuilder<T, TimeSpan?> ruleBuilder)
        {
            return ruleBuilder.Must(x =>
            {
                if (!x.HasValue)
                {
                    return true;
                }
                return x.Value.TotalHours <= 24;
            }).WithMessage("Thời gian không hợp lệ.");
        }


        /// <summary>
        /// Check date múst be greater than a date provide
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <param name="fromDate">From Date</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, DateTime?> MustGreaterThan<T>(this IRuleBuilder<T, DateTime?> ruleBuilder, DateTime? fromDate)
        {
            return ruleBuilder.GreaterThanOrEqualTo(fromDate.Value).WithMessage($"Ngày phải lớn hơn {fromDate.Value}.");
        }
        // <summary>
        /// Check date múst be greater than a date provide
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <param name="fromDate">From Date</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, DateTime> MustGreaterThan<T>(this IRuleBuilder<T, DateTime> ruleBuilder, DateTime fromDate)
        {
            return ruleBuilder.GreaterThanOrEqualTo(fromDate).WithMessage($"Ngày phải lớn hơn {fromDate}.");
        }

        /// <summary>
        /// Check Date is a birth date
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, DateTime?> MustValidBirthDate<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
        {
            return ruleBuilder.Must((x) =>
            {
                if (x == null)
                {
                    return true;
                }
                return x.Value.CompareTo(DateTime.UtcNow) < 0;
            }).WithMessage("'{PropertyValue}' không hợp lệ.");
        }

        /// <summary>
        /// Check file is null or empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, IFormFile> MustRequireFile<T>(this IRuleBuilder<T, IFormFile> ruleBuilder)
        {
            return ruleBuilder
                .NotNull().WithMessage("File không hợp lệ.");
        }
        /// <summary>
        /// Validation for file size
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="maxSize">Maximum file size</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, long> MustValidFileSize<T>(this IRuleBuilder<T, long> ruleBuilder, int maxSize)
        {
            return ruleBuilder
                .NotNull().LessThanOrEqualTo(maxSize).WithMessage("{PropertyName} có dung lượng tệp phải nhỏ hơn " + maxSize + ".");
        }

        /// <summary>
        /// Validation for file type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="fileTypes">The file types allow.</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> MustValidaFileType<T>(this IRuleBuilder<T, string> ruleBuilder, IEnumerable<string> fileTypes)
        {
            return ruleBuilder
                .NotNull().WithMessage("{PropertyName} chưa có dữ liệu.")
                .Must(x => fileTypes.Contains(x))
                .WithMessage("{PropertyName} có kiểu dữ liệu không hợp lệ.");
        }

        /// <summary>
        /// Check validate a id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="checkValidate">The function check</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, int> MustExistEntityId<T>(this IRuleBuilder<T, int> ruleBuilder, Func<int, CancellationToken, Task<bool>> checkValidate)
        {
            return ruleBuilder.MustAsync(checkValidate).WithMessage("'{PropertyValue}' không tồn tại.");
        }

        /// <summary>
        /// Check validate order field
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="type">The view model</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> MustValidOrderField<T>(this IRuleBuilder<T, string> ruleBuilder, Type type)
        {
            return ruleBuilder.Must(
                 (x) =>
                {
                    if (string.IsNullOrEmpty(x)) return true;
                    var types = type.GetProperties();
                    foreach (string field in x.Split(","))
                    {
                        if (!types.Any(t => t.Name.Equals(field.Trim(), StringComparison.CurrentCultureIgnoreCase)))
                        {
                            return false;
                        }
                    }
                    return true;
                }).WithMessage("'{PropertyValue}' không tồn tại");
        }

        /// <summary>
        /// Check valid url
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> MustValidUrl<T>(this IRuleBuilder<T, string> ruleBuilder, bool allowNull = false)
        {
            string pattern = @"[(http(s)?):\/\/(www\.)?a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            if (allowNull)
            {
                return ruleBuilder
              .Must(x =>
              {
                  if (string.IsNullOrEmpty(x))
                  {
                      return true;
                  }
                  return regex.IsMatch(x);
              })
              .WithMessage($"Url không hợp lệ.");
            }
            return ruleBuilder
                .NotNull().WithMessage("Url chưa có.")
                .Must(x =>
                {
                    return regex.IsMatch(x);
                })
                .WithMessage($"Url không hợp lệ.");
        }

        /// <summary>
        /// Check valid phone number
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="allowNull"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> MustValidPhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder, bool allowNull = true)
        {
            string pattern = @"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]{3,15}$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            if (allowNull)
            {
                return ruleBuilder
                .Must(x =>
                {
                    if (string.IsNullOrEmpty(x))
                    {
                        return true;
                    }
                    return regex.IsMatch(x);
                })
                .WithMessage("Số điện thoại không hợp lệ.");
            }
            return ruleBuilder
                .NotNull().WithMessage("Số điện thoại chưa có.")
                .Must(x =>
                {
                    if (string.IsNullOrEmpty(x))
                    {
                        return true;
                    }
                    return regex.IsMatch(x);
                })
                .WithMessage("Số điện thoại không hợp lệ.");
        }

        /// <summary>
        /// Check valid bank account number
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> MustValidBankAccountNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            string pattern = @"^[0-9]{7,14}$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return ruleBuilder
                .NotNull().WithMessage("Số tài khoản ngân hàng chưa có.")
                .Must(x =>
                {
                    if (string.IsNullOrEmpty(x))
                    {
                        return true;
                    }
                    return regex.IsMatch(x);
                })
                .WithMessage("Số tài khoản ngân hàng không hợp lệ.");
        }

        /// <summary>
        /// Check valid people name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="allowNull"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> MustValidPeopleName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            string pattern = @"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return ruleBuilder
                .NotNull().WithMessage("Tên chưa có.")
                .Must(x =>
                {
                    if (string.IsNullOrEmpty(x))
                    {
                        return true;
                    }
                    return regex.IsMatch(x);
                })
                .WithMessage("Tên không hợp lệ.");
        }

        public static IRuleBuilderOptions<T, string> MustValidEmail<T>(this IRuleBuilder<T, string> ruleBuilder, bool allowNull = true)
        {
            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return ruleBuilder
                .NotNull().WithMessage("Email chưa có.")
                .Must(x =>
                {
                    if (string.IsNullOrEmpty(x))
                    {
                        return true;
                    }
                    return regex.IsMatch(x);
                })
                .WithMessage("Email không hợp lệ.");
        }

        public static IRuleBuilderOptions<T, string> MustValidNickname<T>(this IRuleBuilder<T, string> ruleBuilder, bool allowNull = false)
        {
            string pattern = @"^(?!.*\.\.)(?!.*\.$)[^\W][\w.]{0,29}$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            if (allowNull)
            {
                return ruleBuilder
               .Must(x =>
               {
                   if (string.IsNullOrEmpty(x))
                   {
                       return true;
                   }
                   return regex.IsMatch(x);
               }).WithMessage("Nickname không hợp lệ.");
            }
            return ruleBuilder
              .NotNull().WithMessage("Nickname chưa có.")
              .Must(x =>
              {
                  if (string.IsNullOrEmpty(x))
                  {
                      return true;
                  }
                  return regex.IsMatch(x);
              }).WithMessage("Nickname không hợp lệ.");
        }

        public static IRuleBuilderOptions<T, string> MustValidNickname<T>(this IRuleBuilder<T, string> ruleBuilder, string message = null)
        {
            string pattern = @"^(?!.*\.\.)(?!.*\.$)[^\W][\w.]{0,29}$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            return ruleBuilder
           .Must(x =>
           {
               if (string.IsNullOrEmpty(x))
               {
                   return true;
               }
               return regex.IsMatch(x);
           }).WithMessage(message);

        }
    }
}
