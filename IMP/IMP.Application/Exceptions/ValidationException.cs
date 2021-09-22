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
        private readonly int _code;
        public int Code => _code;
        public List<ValidationError> Errors { get; }


        public ValidationException() : base("One or more validation failures have occurred.")
        {
            Errors = new List<ValidationError>();
        }

        /// <summary>
        /// Create Validation Exception with a list Validation Failure
        /// </summary>
        /// <param name="failures"></param>
        public ValidationException(IEnumerable<ValidationFailure> failures, int code = 0)
            : this()
        {
            foreach (var failure in failures)
            {
                var error = new ValidationError(failure.PropertyName.GetSnakeCaseName(), failure.ErrorMessage);
                Errors.Add(error);
            }
            this._code = code;
        }
        /// <summary>
        /// Create Validation Exception with a Validation Error
        /// </summary>
        /// <param name="error">The Validation Error</param>
        public ValidationException(ValidationError error, int code = 0)
            : this()
        {
            Errors.Add(error);
            _code = code;
        }

        public ValidationException(IEnumerable<ValidationError> errors, int code = 0)
            : this()
        {
            Errors.AddRange(errors);
            _code = code;
        }
    }
}
