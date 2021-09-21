using FluentValidation.Results;
using IMP.Application.Helpers;
using IMP.Application.Models;
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

        /// <summary>
        /// Create Validation Exception with a list Validation Failure
        /// </summary>
        /// <param name="failures"></param>
        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            foreach (var failure in failures)
            {
                var error = new ValidationError(failure.PropertyName.GetSnakeCaseName(), failure.ErrorMessage);
                Errors.Add(error);
            }
        }
        /// <summary>
        /// Create Validation Exception with a Validation Error
        /// </summary>
        /// <param name="error">The Validation Error</param>
        public ValidationException(params ValidationError[] errors)
            : this()
        {
            Errors.AddRange(errors);
        }

        public ValidationException(IEnumerable<ValidationError> errors)
            : this()
        {
            Errors.AddRange(errors);
        }



    }
}
