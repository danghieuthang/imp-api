using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class CampaignType : BaseEntity
    {
        [ForeignKey("CampaignType")]
        public int ParentId { get; set; }
        public CampaignType Parent { get; set; }

        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(256)]
        public string Image { get; set; }
        [StringLength(2000)]
        public string Description { get; set; }
        public bool IsActived { get; set; }

        public ICollection<CampaignType> ChildCampaignTypes { get; set; }
        public ICollection<Campaign> Campaigns { get; set; }
    }
}
