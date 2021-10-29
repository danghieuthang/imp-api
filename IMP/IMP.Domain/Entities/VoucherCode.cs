﻿using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class VoucherCode : BaseEntity
    {
        [ForeignKey("ApplicationUser")]
        public int? InfluencerId { get; set; }

        [ForeignKey("Voucher")]
        public int VoucherId { get; set; }
        [ForeignKey("Campaign")]
        public int? CampaignId { get; set; }

        [MaxLength(256)]
        public string Code { get; set; }

        public ApplicationUser Influencer { get; set; }
        public Voucher Voucher { get; set; }
        public Campaign Campaign { get; set; }
        public ICollection<VoucherTransaction> VoucherTransactions { get; set; }


    }
}
