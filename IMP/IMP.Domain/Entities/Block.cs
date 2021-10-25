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
        public Block()
        {
            Items = new List<BlockItem>();
            ChildBlocks = new List<Block>();
        }
        [ForeignKey("Page")]
        public int? PageId { get; set; }

        [ForeignKey("BlockType")]
        public int BlockTypeId { get; set; }

        [ForeignKey("Block")]
        public int? ParentId { get; set; }
        public string Variant { get; set; }
        public int Position { get; set; }
        public bool Enable { get; set; }

        public Page Page { get; set; }
        public BlockType BlockType { get; set; }
        public Block Parent { get; set; }
        public ICollection<BlockItem> Items { get; set; }
        public ICollection<Block> ChildBlocks { get; set; }

    }
}
