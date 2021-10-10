using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IMP.Domain.Common;

namespace IMP.Domain.Entities
{
    public class BlockItem : BaseEntity
    {
        [StringLength(256)]
        public string Key { get; set; }

        public string Value { get; set; }
    }

}