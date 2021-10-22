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
    public class CampaignCondition : BaseEntity
    {
        public int? AgeFrom { get; set; }
        public int? AgeTo { get; set; }
        public bool UnlimitedAge { get; set; }
        public int? Gender { get; set; }
        [ForeignKey("Location")]
        public int LocationId { get; set; }

        [StringLength(256)]
        public string Interests { get; set; }
        [StringLength(256)]
        public string Jobs { get; set; }

        public bool? ChildStatus { get; set; }
        public bool? MaritalStatus { get; set; }



        [ForeignKey("Campaign")]
        public int CampaignId { get; set; }

    }
}
