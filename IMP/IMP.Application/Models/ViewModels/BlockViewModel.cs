using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMP.Application.Models.Compaign;

namespace IMP.Application.Models.ViewModels
{
    public class BlockViewModel : BaseViewModel<int>
    {
        public int PageId { get; set; }
        public int BlockTypeId { get; set; }
        public int ParentId { get; set; }
        public string Title { get; set; }
        public string Avatar { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; }
        public string Text { get; set; }
        public string TextArea { get; set; }
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
        public int Position { get; set; }
        public bool IsActived { get; set; }
    }

    public class BlockPlatformViewModel : BaseViewModel<int>
    {
        public InfluencerPlatformViewModel InfluencerPlatform { get; set; }
        public int BlockId { get; set; }
        public int Position { get; set; }
        public bool IsActived { get; set; }
    }
    public class BlockCampaignViewModel : BaseViewModel<int>
    {
        public CampaignViewModel Campaign { get; set; }
        public int BlockId { get; set; }
        public int Position { get; set; }
        public bool IsActived { get; set; }
    }
}
