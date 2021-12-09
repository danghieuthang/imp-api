using IMP.Application.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IMP.Application.Models.Account
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [DefaultValue(RegisterRole.Influencer)]
        public RegisterRole Role { get; set; }
    }
}
