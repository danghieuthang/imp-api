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
    public class Product: BaseEntity
    {
        [StringLength(256)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        [StringLength(50)]
        public string Currency { get; set; }
        [ForeignKey("Campaign")]
        public int CampaignId { get; set; }

    }
}
