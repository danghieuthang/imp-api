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
    public class Product : BaseEntity
    {
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(256)]
        public string Code { get; set; }
        public decimal Price { get; set; }
        public bool? IsReward { get; set; }
        [StringLength(50)]
        public string Currency { get; set; }
        [ForeignKey("Campaign")]
        public int CampaignId { get; set; }
        [StringLength(256)]
        public string Image { get; set; }
        [StringLength(2000)]
        public string Description { get; set; }

    }
}
