using FluentValidation.Results;
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
                var error = new ValidationError(GetSnakeCaseName(failure.PropertyName), failure.ErrorMessage);
                Errors.Add(error);
            }
        }
        private string GetSnakeCaseName(string field)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(char.ToLower(field[0]));

            for (int i = 1; i < field.Length; i++)
            {
                if (char.IsUpper(field[i]))
                {
                    builder.Append('_');
                }
                builder.Append(char.ToLower(field[i]));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Create Validation Exception with a Validation Error
        /// </summary>
        /// <param name="error">The Validation Error</param>
        public ValidationException(ValidationError error)
            : this()
        {
            Errors.Add(error);
        }

        public ValidationException(IEnumerable<ValidationError> errors)
            : this()
        {
            Errors.AddRange(errors);
        }



    }
}
