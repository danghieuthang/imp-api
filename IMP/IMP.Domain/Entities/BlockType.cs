using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IMP.Domain.Entities
{
    public class BlockType : BaseEntity
    {
        [MaxLength(256)]
        public string Name { get; set; }
        [MaxLength(256)]
        public string Image { get; set; }
        [MaxLength(256)]
        public string Description { get; set; }
        public bool IsActived { get; set; }
    }
}
