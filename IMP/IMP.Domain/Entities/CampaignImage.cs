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
    public class CampaignImage : BaseEntity
    {
        [ForeignKey("Campaign")]
        public int CampaignId { get; set; }
        [StringLength(256)]
        public string Url { get; set; }
        public int Position { get; set; }
    }
}
