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
    public class ActivityResult : BaseEntity
    {
        [StringLength(256)]
        public string Name { get; set; }
        public int Type { get; set; }
        [ForeignKey("CampaignActivity")]
        public int CampaignActivityId { get; set; }
        public CampaignActivity CampaignActivity { get; set; }
    }
}
