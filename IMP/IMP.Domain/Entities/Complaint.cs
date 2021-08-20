using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class Complaint : BaseEntity
    {
        [ForeignKey("ApplicationUser")]
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("ApplicationUser")]
        public int FeedbackUserId { get; set; }
        public ApplicationUser FeedbackUser { get; set; }

        [ForeignKey("Campaign")]
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }

        [MaxLength(256)]
        public string Title { get; set; }
        [MaxLength(2000)]
        public string Content { get; set; }
        public int ProcessingStatus { get; set; }
        [MaxLength(2000)]
        public string FeedbackContent { get; set; }
        public DateTime? FeedbackTime { get; set; }
        [NotMapped]
        public DateTime Time { get { return Created; } }
    }
}
