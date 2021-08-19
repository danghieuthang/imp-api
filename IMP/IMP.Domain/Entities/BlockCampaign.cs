using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class BlockCampaign: BaseEntity
    {
        [ForeignKey("Campaign")]
        public int CampaignId { get; set; }
        [ForeignKey("Block")]
        public int BlockId { get; set; }
        public int Position { get; set; }
        public bool IsActived { get; set; }

    }
}
