using System.Collections.Generic;

namespace IMP.Application.Models.Compaign
{
    public class TargetConfigurationViewModel
    {
        public int? AgeFrom { get; set; }
        public int? AgeTo { get; set; }
        public bool UnlimitedAge { get; set; }

        public int Gender { get; set; }

        public List<string> Interests { get; set; }
        public List<string> Jobs { get; set; }

        public bool? ChildStatus { get; set; }
        public bool? MaritalStatus { get; set; }
        public bool? Pregnant { get; set; }
        public List<string> Purposes { get; set; }
        public List<TargetConfigurationLocationViewModel> Locations { get; set; }
    }

    public class TargetConfigurationLocationViewModel : InfluencerConfigurationLocationViewModel
    {

    }
}