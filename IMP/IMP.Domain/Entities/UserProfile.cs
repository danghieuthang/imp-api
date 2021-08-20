using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    [Table("Profile")]
    public class UserProfile : BaseEntity
    {
        [ForeignKey("ApplicationUser")]
        public int UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [MaxLength(256)]
        public string Email { get; set; }
        [MaxLength(256)]
        public string AvatarUrl { get; set; }
        [MaxLength(50)]
        public string Gender { get; set; }
        [MaxLength(256)]
        public string NickName { get; set; }
        [MaxLength(50)]
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
    }
}
