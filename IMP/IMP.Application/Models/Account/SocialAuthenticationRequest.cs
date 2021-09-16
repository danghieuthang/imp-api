using IMP.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models.Account
{
    public class SocialAuthenticationRequest
    {
        public string Token { get; set; }

        /// <summary>
        /// Google,Facebook,Instagram
        /// </summary>
        public string Provider { get; set; }
        /// <summary>
        /// Web,Mobile
        /// </summary>
        public int AppId { get; set; }
    }

    public class SocialRegisterRequest: SocialAuthenticationRequest
    {
        public RegisterRole Role { get; set; }
    }
}
