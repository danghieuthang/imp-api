using IMP.Application.Models.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Infrastructure.Identity.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            RefreshTokens = new();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsChangeUsername { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public bool OwnsToken(string token)
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }
        public int? ApplicationUserId { get; set; }
    }
}
