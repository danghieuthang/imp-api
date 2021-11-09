﻿using IMP.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models.ViewModels
{
    public class CampaignMemberViewModel : BaseViewModel<int>
    {
        public int CampaignId { get; set; }
        public InfluencerViewModel Influencer { get; set; }
        public int Status { get; set; }
    }
}
