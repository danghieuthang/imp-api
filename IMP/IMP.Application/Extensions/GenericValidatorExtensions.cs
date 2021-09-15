using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Extensions
{
    public static class GenericValidatorExtensions
    {
        public static IRuleBuilderOptions<T, IList<TElement>> ListMustContainFewerThan<T, TElement>(this IRuleBuilder<T, IList<TElement>> ruleBuilder, int num)
        {
            return ruleBuilder.Must(list => list.Count < num).WithMessage($"List không được chứa nhiều hơn {num} item.");
        }

        /// <summary>
        /// Custom validation for string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="maxLength">Max length of property.</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> Required<T>(this IRuleBuilder<T, string> ruleBuilder, int maxLength)
        {
            return ruleBuilder
                .NotNull().NotEmpty().WithMessage("{PropertyName} chưa có dữ liệu.")
                .MaximumLength(256).WithMessage("{PropertyName} không thể quá " + maxLength + " ký tự.");
        }

        public static IRuleBuilderOptions<T, string> MustMaxLength<T>(this IRuleBuilder<T, string> ruleBuilder, int maxLength)
        {
            return ruleBuilder
                .MaximumLength(256).WithMessage("{PropertyName} không thể quá " + maxLength + " ký tự.");
        }

        public static IRuleBuilderOptions<T, DateTime?> IsValidDate<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
        {
            return ruleBuilder.GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("'{PropertyValue}' không hợp lệ.");
        }

        public static IRuleBuilderOptions<T, DateTime?> IsValidBirthDate<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
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


        public static IRuleBuilderOptions<T, IFormFile> RequireFile<T>(this IRuleBuilder<T, IFormFile> ruleBuilder)
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
        public static IRuleBuilderOptions<T, long> ValidFileSize<T>(this IRuleBuilder<T, long> ruleBuilder, int maxSize)
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
        public static IRuleBuilderOptions<T, string> ValidaFileType<T>(this IRuleBuilder<T, string> ruleBuilder, IEnumerable<string> fileTypes)
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
        public static IRuleBuilderOptions<T, int> IsExistId<T>(this IRuleBuilder<T, int> ruleBuilder, Func<int, CancellationToken, Task<bool>> checkValidate)
        {
            return ruleBuilder.MustAsync(checkValidate).WithMessage("'{PropertyValue}' không tồn tại");
        }

        /// <summary>
        /// Check validate order field
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="type">The view model</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> IsValidOrderField<T>(this IRuleBuilder<T, string> ruleBuilder, Type type)
        {
            return ruleBuilder.Must(
                 (x) =>
                {
                    if (string.IsNullOrEmpty(x)) return true;
                    foreach (var prop in type.GetProperties())
                    {
                        if (prop.Name.Equals(x, StringComparison.CurrentCultureIgnoreCase))
                        {
                            return true;
                        }
                    }
                    return false;
                }).WithMessage("'{PropertyValue}' không tồn tại");
        }

        public static IRuleBuilderOptions<T, string> IsValidUrl<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.NotNull().NotEmpty().Matches(@"[(http(s)?):\/\/(www\.)?a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)")
                .WithMessage("'{PropertyValue}' không phải một Url hợp lệ.");
        }

    }
}
