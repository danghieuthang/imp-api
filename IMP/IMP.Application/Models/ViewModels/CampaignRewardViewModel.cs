namespace IMP.Application.Models.Compaign
{
    public class CampaignRewardViewModel : BaseViewModel<int>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }
}