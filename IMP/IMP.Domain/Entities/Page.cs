using IMP.Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class Page : BaseEntity
    {
        [ForeignKey("ApplicationUser")]
        public int InfluencerId { get; set; }
        public ApplicationUser Influencer { get; set; }

        [StringLength(256)]
        public string BackgroundType { get; set; }
        public double FontSize { get; set; }
        [StringLength(256)]
        public string Background { get; set; }
        [StringLength(256)]
        public string BioLink { get; set; }
        [StringLength(256)]
        public string FontFamily { get; set; }
        [StringLength(256)]
        public string TextColor { get; set; }
        public ICollection<Block> Blocks { get; set; }
        public int Status { get; set; }
    }
}
