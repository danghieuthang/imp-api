using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class InfluencerPlatform : BaseEntity
    {
        [ForeignKey("ApplicationUser")]
        public int InfluencerId { get; set; }
        public ApplicationUser Influencer { get; set; }

        [ForeignKey("Platform")]
        public int PlatformId { get; set; }
        public Platform Platform { get; set; }

        [StringLength(256)]
        public string Url { get; set; }
        public int Follower { get; set; }
        [StringLength(256)]
        public string Category { get; set; }
    }
}
