using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Domain.Entities
{
    public class Notification : BaseEntity
    {
        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int Type { get; set; }
        [StringLength(256)]
        public string Url { get; set; }
        [StringLength(256)]
        public string Message { get; set; }
        public int RedirectId { get; set; }
    }
}
