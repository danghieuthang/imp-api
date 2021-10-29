using System.Collections.Generic;

namespace IMP.Application.Models.Compaign
{
    public class InfluencerConfigurationViewModel
    {
        public int PlatformId { get; set; }
        public int NumberOfInfluencer { get; set; }
        public int LevelId { get; set; }
        public int? AgeFrom { get; set; }
        public int? AgeTo { get; set; }
        public bool UnlimitedAge { get; set; }
        public string Gender { get; set; }

        public string Interests { get; set; }
        public string Jobs { get; set; }

        public bool? ChildStatus { get; set; }
        public bool? MaritalStatus { get; set; }
        public bool? Pregnant { get; set; }

        public List<string> Others { get; set; }

        public int CampaignId { get; set; }

        public List<InfluencerConfigurationLocationViewModel> Locations { get; set; }
    }

    public class InfluencerConfigurationLocationViewModel
    {
        public int LocationId { get; set; }
    }
}