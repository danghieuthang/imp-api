using IMP.Domain.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Domain.Entities
{
    public class TargetConfiguration : BaseEntity
    {
        public TargetConfiguration()
        {
            Locations = new List<TargetConfigurationLocation>();
        }
        public int? AgeFrom { get; set; }
        public int? AgeTo { get; set; }
        public bool UnlimitedAge { get; set; }

        [StringLength(256)]
        public string Gender { get; set; }

        [StringLength(256)]
        public string Interests { get; set; }
        [StringLength(256)]
        public string Jobs { get; set; }

        public bool? ChildStatus { get; set; }
        public bool? MaritalStatus { get; set; }
        public bool? Pregnant { get; set; }
        [StringLength(2000)]
        public string Purpose { get; set; }

        [ForeignKey("Campaign")]
        public int CampaignId { get; set; }
        public ICollection<TargetConfigurationLocation> Locations { get; set; }
    }

    public class TargetConfigurationLocation : BaseEntity
    {
        [ForeignKey("TargetConfiguration")]
        public int TargetConfigurationId { get; set; }
        [ForeignKey("Location")]
        public int LocationId { get; set; }
    }
}
