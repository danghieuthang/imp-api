using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMP.Domain.Common;

namespace IMP.Domain.Entities
{
    public class Otp : BaseEntity
    {
        public int Code { get; set; }
        public DateTime ExpiredTime { get; set; }
        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
    }
}
