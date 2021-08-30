using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Extensions
{
    public static class GenericValidatorExtensions
    {
        public static IRuleBuilderOptions<T, IList<TElement>> ListMustContainFewerThan<T, TElement>(this IRuleBuilder<T, IList<TElement>> ruleBuilder, int num)
        {
            return ruleBuilder.Must(list => list.Count < num).WithMessage("The list contains too many items");
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
                .NotEmpty().WithMessage("{PropertyName} chưa có dữ liệu.")
                .NotNull()
                .MaximumLength(256).WithMessage("{PropertyName} không thể quá " + maxLength + " ký tự.");
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


    }
}
