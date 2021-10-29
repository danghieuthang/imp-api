using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class Ranking : BaseEntity
    {
        [ForeignKey("RankLevel")]
        public int RankLevelId { get; set; }

        [ForeignKey("ApplicationUser")]
        public int InfluencerId { get; set; }

        public int Score { get; set; }
        public RankLevel RankLevel { get; set; }
        public ApplicationUser Influencer { get; set; }

    }
}
