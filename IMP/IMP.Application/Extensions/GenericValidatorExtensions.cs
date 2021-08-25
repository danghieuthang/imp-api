using FluentValidation;
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
                .NotEmpty().WithMessage("{PropertyName} must be required")
                .NotNull()
                .MaximumLength(256).WithMessage("{PropertyName} must not exeed " + maxLength + " characters.");
        }


    }
}
