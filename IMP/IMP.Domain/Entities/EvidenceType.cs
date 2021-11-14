using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Domain.Entities
{
    public class EvidenceType : BaseEntity
    {
        [StringLength(256)]
        public string Name { get; set; }

        public ICollection<CampaignActivity> CampaignActivities { get; set; }
        public ICollection<Evidence> Evidences { get; set; }
    }
}
