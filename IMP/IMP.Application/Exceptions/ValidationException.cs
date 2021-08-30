using FluentValidation.Results;
using IMP.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMP.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException() : base("One or more validation failures have occurred.")
        {
            Errors = new List<ValidationError>();
        }
        public List<ValidationError> Errors { get; }
        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            foreach (var failure in failures)
            {
                var error = new ValidationError(failure.PropertyName, failure.ErrorMessage);
                Errors.Add(error);
            }
        }

    }
}
