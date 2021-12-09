using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models
{
    public class ValidationError
    {
        public string Field { get; set; }
        public string Message { get; set; }
        public ValidationError(string field, string message)
        {
            Field = field;
            Message = message;
        }
    }
}
