using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class BlockPlatform : BaseEntity
    {
        [ForeignKey("InfluencerPlatform")]
        public int InfluencerPlatformId { get; set; }
        public InfluencerPlatform InfluencerPlatform { get; set; }

        [ForeignKey("Block")]
        public int BlockId { get; set; }
        public Block Block { get; set; }

        public int Position { get; set; }
        public bool IsActived { get; set; }
    }
}
