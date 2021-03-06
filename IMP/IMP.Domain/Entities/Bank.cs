using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Domain.Entities
{
    public class Bank : BaseEntity
    {
        [MaxLength(256)]
        public string Code { get; set; }

        [MaxLength(256)]
        public string Name { get; set; }

        public ICollection<PaymentInfor> PaymentInfors { get; set; }
    }
}
