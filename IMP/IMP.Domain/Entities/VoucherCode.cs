using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class VoucherCode : BaseEntity
    {
        [ForeignKey("CampaignId")]
        public int CampaignId { get; set; }
        [ForeignKey("ApplicationUser")]
        public int InfluencerId { get; set; }
        [MaxLength(256)]
        public string Code { get; set; }
        public ICollection<VoucherTransaction> VoucherTransactions { get; set; }


    }
}
