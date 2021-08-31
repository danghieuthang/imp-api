using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Domain.Entities
{
    public class Milestone : BaseEntity
    {
        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        public ICollection<CampaignMilestone> CampaignMilestones { get; set; }
    }
}
