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
    public class InfluencerConfiguration : BaseEntity
    {
        public InfluencerConfiguration()
        {
            Locations = new List<InfluencerConfigurationLocation>();
        }
        [ForeignKey("Platform")]
        public int PlatformId { get; set; }
        public int NumberOfInfluencer { get; set; }
        [ForeignKey("RankLevel")]
        public int LevelId { get; set; }
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
        public string OtherCondition { get; set; }

        [ForeignKey("Campaign")]
        public int CampaignId { get; set; }

        public Platform Platform { get; set; }
        public RankLevel RankLevel { get; set; }
        public ICollection<InfluencerConfigurationLocation> Locations { get; set; }
    }

    public class InfluencerConfigurationLocation : BaseEntity
    {
        [ForeignKey("InfluencerConfiguration")]
        public int InfluencerConfigurationId { get; set; }
        [ForeignKey("Location")]
        public int LocationId { get; set; }
    }
}
