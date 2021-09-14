﻿using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class ApplicationUser : BaseEntity
    {
        #region personal infomation
        [MaxLength(256)]
        public string UserName { get; set; }
        [MaxLength(10)]
        public string PhoneNumber { get; set; }
        [MaxLength(256)]
        public string FirstName { get; set; }
        [MaxLength(256)]
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        [NotMapped]
        public int? Age
        {
            get
            {
                if (BirthDate == null)
                {
                    return null;
                }
                var today = DateTime.Today;
                int age = today.Year - BirthDate.Value.Year;
                if (today.Date < BirthDate.Value.AddYears(age - 1))
                {
                    age--;
                }
                return age;
            }
        }
        [StringLength(256)]
        public string Gender { get; set; }
        [ForeignKey("Location")]
        public int? LocationId { get; set; }
        public Location Location { get; set; }
        #endregion 

        #region additional infomation
        [StringLength(256)]
        public string Job { get; set; }
        [StringLength(256)]
        public string Interests { get; set; }
        public bool? ChildStatus { get; set; }
        public bool? MaritalStatus { get; set; }
        public string Pet { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }
        #endregion

        [ForeignKey("Wallet")]
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }

        [ForeignKey("PaymentInfor")]
        public int? PaymentInforId { get; set; }
        public PaymentInfor PaymentInfor { get; set; }

        [ForeignKey("Ranking")]
        public int? RankingId { get; set; }
        public Ranking Ranking { get; set; }

        public ICollection<Page> Pages { get; set; }

        [NotMapped]
        public bool IsActivate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(PhoneNumber)
                    || string.IsNullOrWhiteSpace(FirstName)
                    || BirthDate == null)
                {
                    return false;
                }
                return true;

            }
        }
    }
}
