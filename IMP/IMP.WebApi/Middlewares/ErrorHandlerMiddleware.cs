using IMP.Application.Exceptions;
using IMP.Application.Wrappers;
using IMP.WebApi.Policies;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace IMP.WebApi.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger = Log.ForContext<ErrorHandlerMiddleware>();

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<string>() { Succeeded = false, Message = error?.Message };

                switch (error)
                {
                    case Application.Exceptions.ApiException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case ValidationException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Errors = e.Errors;
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        _logger.Error(error, "Internal Server Error");
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = new SnakeNamingPolicy()
                };
                var result = JsonSerializer.Serialize(responseModel, options);

                await response.WriteAsync(result);
            }
        }
    }
}
