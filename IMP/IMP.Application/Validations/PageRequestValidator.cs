using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Validations
{
    /// <summary>
    /// Defines the default validator for page request
    /// </summary>
    /// <typeparam name="TViewModel">The type of view model</typeparam>
    /// <typeparam name="TRequest">The type of page request</typeparam>
    public class PageRequestValidator<TRequest, TViewModel> : AbstractValidator<TRequest>
        where TViewModel : notnull
        where TRequest : IListQuery<TViewModel>
    {
        public PageRequestValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(0).WithMessage("Giá trị không được nhỏ hơn 0.");
            RuleFor(x => x.PageSize).MustPositiveInteger();
            RuleFor(x => x.OrderField).MustValidOrderField(typeof(TViewModel));
        }
    }
}
