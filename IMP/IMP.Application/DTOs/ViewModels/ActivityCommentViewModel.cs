using Newtonsoft.Json;

namespace IMP.Application.Models.ViewModels
{
    public class ActivityCommentViewModel : BaseViewModel<int>
    {
        public int MemberActivityId { get; set; }

        [JsonProperty("created_by")]
        public UserBasicViewModel ApplicationUser { get; set; }

        public string Comment { get; set; }
    }
}