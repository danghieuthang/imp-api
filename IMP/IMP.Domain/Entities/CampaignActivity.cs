using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class CampaignActivity : BaseEntity
    {
        [ForeignKey("Campaign")]
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }

        [ForeignKey("ActivityType")]
        public int ActivityTypeId { get; set; }
        public ActivityType ActivityType { get; set; }

        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(256)]
        public string Description { get; set; }
        public string HowToDo { get; set; }
        public ICollection<MemberActivity> MemberActivities { get; set; }
        public ICollection<ActivityResult> ActivityResults { get; set; }
    }
}
