using System.Collections.Generic;

namespace IMP.Application.Models.Compaign
{
    public class CampaignActivityViewModel : BaseViewModel<int>
    {
        public int ActivityTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HowToDo { get; set; }
        public string ResultName { get; set; }
        public int? EvidenceTypeId { get; set; }
    }

    public class ActivityResultViewModel
    {
        public string Name { get; set; }
        public int Type { get; set; }
    }
    public class CampaignActivityUpdateModel
    {
        public int Id { get; set; }
        public int ActivityTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HowToDo { get; set; }
        public string ResultName { get; set; }
        public int EvidenceTypeId { get; set; }
    }
    public class ActivityResultUpdateModel
    {
        public string Name { get; set; }
        public int Type { get; set; }
    }

}