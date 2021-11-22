using System.Collections.ObjectModel;
using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class ApplicationUser : BaseEntity
    {
        public ApplicationUser()
        {
            Otps = new Collection<Otp>();
            TransactionsReceived = new Collection<WalletTransaction>();
            TransactionsSent = new Collection<WalletTransaction>();
        }
        #region personal infomation
        [MaxLength(256)]
        public string Avatar { get; set; }
        [MaxLength(256)]
        public string Email { get; set; }

        public bool IsEmailVerified
        {
            get; set;
        }
        [MaxLength(256)]
        public string Nickname { get; set; }
        [MaxLength(15)]
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

        #region additional information
        [StringLength(256)]
        public string Job { get; set; }
        [StringLength(256)]
        public string Interests { get; set; }
        public bool? ChildStatus { get; set; }
        public bool? MaritalStatus { get; set; }
        public bool? Pregnant { get; set; }

        public string Pet { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }
        #endregion

        public Wallet Wallet { get; set; }
        public PaymentInfor PaymentInfor { get; set; }

        public Ranking Ranking { get; set; }

        [ForeignKey("Brand")]
        public int? BrandId { get; set; }
        public Brand Brand { get; set; }

        public ICollection<Page> Pages { get; set; }
        public ICollection<Otp> Otps { get; set; }

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
        public bool IsApproved { get; set; }

        public virtual ICollection<InfluencerPlatform> InfluencerPlatforms { get; set; }
        public virtual ICollection<WalletTransaction> TransactionsSent { get; set; }
        public virtual ICollection<WalletTransaction> TransactionsReceived { get; set; }
        public virtual ICollection<VoucherCodeApplicationUser> VoucherCodeApplicationUsers { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
