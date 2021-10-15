﻿using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Domain.Entities
{
    [Table("Brands")]
    public class Brand : BaseEntity
    {
        [StringLength(256)]
        public string Code { get; set; }
        [StringLength(256)]
        public string CompanyName { get; set; }
        public bool IsCampany { get; set; }

        [StringLength(256)]
        public string Website { get; set; }
        [StringLength(256)]
        public string Representative { get; set; }
        [StringLength(256)]
        public string Fanpage { get; set; }
        [StringLength(256)]
        public string Job { get; set; }
        [StringLength(256)]
        public string Phone { get; set; }
        [StringLength(256)]
        public string Address { get; set; }
        public bool IsEmailVerified { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [ForeignKey("Wallet")]
        public int? WalletId { get; set; }
        public Wallet Wallet { get; set; }
    }
}
