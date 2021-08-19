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
        [MaxLength(256)]
        public string Title { get; set; }
        [MaxLength(256)]
        public string BackgroundPhoto { get; set; }
        public int PositionPage { get; set; }
    }
}
