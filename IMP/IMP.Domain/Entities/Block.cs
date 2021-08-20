using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class Block : BaseEntity
    {
        [ForeignKey("Page")]
        public int PageId { get; set; }
        public Page Page { get; set; }

        [ForeignKey("BlockType")]
        public int BlockTypeId { get; set; }
        public BlockType BlockType { get; set; }

        [ForeignKey("Block")]
        public int ParentId { get; set; }
        public Block Parent { get; set; }

        [MaxLength(256)]
        public string Title { get; set; }
        [MaxLength(256)]
        public string Avatar { get; set; }
        [MaxLength(256)]
        public string Bio { get; set; }
        [MaxLength(256)]
        public string Location { get; set; }
        [MaxLength(256)]
        public string Text { get; set; }
        [MaxLength(2000)]
        public string TextArea { get; set; }
        [MaxLength(256)]
        public string ImageUrl { get; set; }
        [MaxLength(256)]
        public string VideoUrl { get; set; }
        public int Position { get; set; }
        public bool IsActived { get; set; }

        public ICollection<BlockPlatform> BlockPlatforms { get; set; }

    }
}
