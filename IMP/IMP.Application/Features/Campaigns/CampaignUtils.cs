using IMP.Application.Enums;
using IMP.Application.Models;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Features.Campaigns
{
    internal static class CampaignUtils
    {
        public static List<ValidationError> CheckSuitability(Campaign campaign, ApplicationUser user)
        {
            List<ValidationError> errors = new List<ValidationError>();

            // check marital
            if (campaign.InfluencerConfiguration.MaritalStatus.HasValue && campaign.InfluencerConfiguration.MaritalStatus != user.MaritalStatus)
            {
                errors.Add(new ValidationError("marital_status", "Tình trạng hôn nhân không hợp lệ."));
            }

            // check child status
            if (campaign.InfluencerConfiguration.ChildStatus.HasValue && campaign.InfluencerConfiguration.ChildStatus != user.ChildStatus)
            {
                errors.Add(new ValidationError("child_status", "Tình trạng con cái không hợp lệ."));
            }

            // check pregnant
            //if (campaign.InfluencerConfiguration.Pregnant.HasValue && campaign.InfluencerConfiguration.Pregnant != user.Pregnant.Value)
            //{
            //    errors.Add(new ValidationError("", $"Tình trạng mang thai không hợp lệ."));
            //}

            if (campaign.InfluencerConfiguration.PlatformId.HasValue
                && !user.InfluencerPlatforms.Select(x => x.PlatformId).Contains(campaign.InfluencerConfiguration.PlatformId.Value))
            {
                errors.Add(new ValidationError("platform_id", $"Influencer chưa có hợp lệ."));
            }

            // check gender
            if (campaign.InfluencerConfiguration.Gender.HasValue && campaign.InfluencerConfiguration.Gender.Value != (int)Genders.None)
            {
                int gender;
                string ge = user.Gender == null ? "" : user.Gender.ToLower();

                switch (ge)
                {
                    case "male":
                        gender = (int)Genders.Male;
                        break;
                    case "female":
                        gender = (int)Genders.Female;
                        break;
                    case "other":
                        gender = (int)Genders.Other;
                        break;
                    default:
                        gender = (int)Genders.None;
                        break;
                }

                if (gender != campaign.InfluencerConfiguration.Gender.Value && gender != (int)Genders.None)
                {
                    errors.Add(new ValidationError("gender", $"Giới tính không hợp lệ."));
                }
            }

            return errors;
        }
    }
}
