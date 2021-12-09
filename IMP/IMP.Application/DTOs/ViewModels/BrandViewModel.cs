using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models.ViewModels
{
    public class BrandViewModel : BaseViewModel<int>
    {
        public string Code { get; set; }
        public string CompanyName { get; set; }
        public string Image { get; set; }
        public string Website { get; set; }
        public string Representative { get; set; }
        public string Fanpage { get; set; }
        [JsonProperty("job")]
        public List<string> JobB { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Introduction { get; set; }
        public bool IsCompany { get; set; }
        public int? WalletId { get; set; }
    }

    public class BrandFullViewModel : BaseViewModel<int>
    {
        public string Code { get; set; }
        public string CompanyName { get; set; }
        public string Image { get; set; }
        public string Website { get; set; }
        public string Representative { get; set; }
        public string Fanpage { get; set; }
        [JsonProperty("job")]
        public List<string> JobB { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Introduction { get; set; }
        public bool IsCompany { get; set; }
        public int? WalletId { get; set; }
        public string SecretKey { get; set; }
    }
}
