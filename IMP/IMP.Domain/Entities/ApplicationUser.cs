using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IMP.Domain.Entities
{
    public class ApplicationUser : BaseEntity
    {
        [MaxLength(256)]
        public string UserName { get; set; }
    }
}
