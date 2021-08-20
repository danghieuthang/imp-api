using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IMP.Domain.Entities
{
    public class Platform : BaseEntity
    {
        [MaxLength(256)]
        public string Name { get; set; }

        public ICollection<Campaign> Campaigns { get; set; }
        public ICollection<InfluencerPlatform> InfluencerPlatforms { get; set; }
    }
}
