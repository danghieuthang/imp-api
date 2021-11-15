using IMP.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMP.Application.Wrappers
{
    public class Response<T>
    {
        public Response()
        {
        }
        public Response(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }
        public Response(string message, int code = 0)
        {
            Succeeded = false;
            Message = message;
            Errors = new List<ValidationError>();
            Code = code;
        }
        public Response(ValidationError error, string message = null, int code = 0)
        {
            Succeeded = false;
            Message = message;
            Errors = new List<ValidationError> { error };
            Code = code;
        }

        public Response(List<ValidationError> errors, string message = null, int code = 0)
        {
            Succeeded = false;
            Message = message;
            Errors = errors;
            Code = code;
        }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<ValidationError> Errors { get; set; }
        public int Code { get; set; }
        public T Data { get; set; }
    }
}
