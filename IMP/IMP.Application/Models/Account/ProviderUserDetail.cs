using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models.Account
{
    public class ProviderUserDetail
    {
        private string _name;
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Locale { get; set; }
        public string Name
        {
            get
            {
                if (_name == null)
                {
                    return FirstName + " " + LastName;
                };
                return _name;
            }
            set { _name = value; }

        }
        public string ProviderUserId { get; set; }
    }
}
