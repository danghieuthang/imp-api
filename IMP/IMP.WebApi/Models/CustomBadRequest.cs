using System.Linq;
using IMP.Application.Helpers;
using IMP.Application.Models;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IMP.WebApi.Models
{
    public class CustomBadRequest : ValidationProblemDetails
    {
        public CustomBadRequest(ActionContext context)
        {
            Status = 400;
            ConstructErrorMessages(context);
        }
        private void ConstructErrorMessages(ActionContext context)
        {
            foreach (var keyModelStatePair in context.ModelState)
            {
                var key = keyModelStatePair.Key.GetSnakeCaseName();
                var errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    if (errors.Count == 1)
                    {
                        var errorMessage = GetErrorMessage(errors[0]);
                        Errors.Add(key, new[] { errorMessage });
                    }
                    else
                    {
                        var errorMessages = new string[errors.Count];
                        for (var i = 0; i < errors.Count; i++)
                        {
                            errorMessages[i] = GetErrorMessage(errors[i]);
                        }
                        Errors.Add(key, errorMessages);
                    }
                }
            }
        }
        string GetErrorMessage(ModelError error)
        {
            return string.IsNullOrEmpty(error.ErrorMessage) ?
                "The input was not valid." :
            error.ErrorMessage;
        }

        public Response<string> ToResponse()
        {
            var errors = Errors.Select(x => new ValidationError(x.Key, x.Value[0])).ToList();
            return new Response<string>
            {
                Succeeded = false,
                Errors = errors,
                Message = "Request not valid"
            };
        }
    }
}