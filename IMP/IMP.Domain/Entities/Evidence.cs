using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class Evidence : BaseEntity
    {
        [ForeignKey("MemberActivity")]
        public int MemberActivityId { get; set; }
        public MemberActivity MemberActivity { get; set; }

        [StringLength(256)]
        public string ImageUrl { get; set; }
        [StringLength(256)]
        public string VideoUrl { get; set; }
        [StringLength(256)]
        public string LinkUrl { get; set; }
    }
}
