namespace IMP.Application.Models.ViewModels
{
    public class ActivityCommentViewModel : BaseViewModel<int>
    {
        public int MemberActivityId { get; set; }

        public int ApplicationUserId { get; set; }

        public string Comment { get; set; }
    }
}