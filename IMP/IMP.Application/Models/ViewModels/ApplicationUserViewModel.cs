using IMP.Application.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models.ViewModels
{
    public class ApplicationUserViewModel : BaseViewModel<int>
    {
        public string Email { get; set; }
        public bool IsEmailVerified { get; set; }
        public string Avatar { get; set; }
        public string Nickname { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; }
        public int? LocationId { get; set; }

        [JsonProperty("jobs")]
        public List<string> JobsR { get; set; }

        [JsonProperty("interests")]
        public List<string> InterestsR { get; set; }
        public bool? ChildStatus { get; set; }
        public bool? MaritalStatus { get; set; }
        public bool? Pregnant { get; set; }
        [JsonProperty("pets")]
        public List<string> PetsR { get; set; }
        public string Description { get; set; }
        public int WalletId { get; set; }

        public PaymentInforViewModel PaymentInfor { get; set; }
        public RankingViewModel Ranking { get; set; }
        public string Role { get; set; }
    }
    public class AdminViewModel : BaseViewModel<int>
    {
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Nickname { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class InfluencerViewModel : BaseViewModel<int>
    {
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Nickname { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; }
        public int? LocationId { get; set; }

        [JsonProperty("jobs")]
        public List<string> JobsR { get; set; }

        [JsonProperty("interests")]
        public List<string> InterestsR { get; set; }
        public bool? ChildStatus { get; set; }
        public bool? MaritalStatus { get; set; }
        public bool? Pregnant { get; set; }
        [JsonProperty("pets")]
        public List<string> PetsR { get; set; }
        public string Description { get; set; }
        public int WalletId { get; set; }

        public PaymentInforViewModel PaymentInfor { get; set; }
        public RankingViewModel Ranking { get; set; }
        public List<InfluencerPlatformViewModel> InfluencerPlatforms { get; set; }
        public LocationViewModel Location { get; set; }

    }
    public class PaymentInforViewModel : BaseViewModel<int>
    {
        public BankViewModel Bank { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
    }

    public class BankViewModel : BaseViewModel<int>
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
