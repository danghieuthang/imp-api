using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Common
{

    public abstract class BaseEntity : Entity<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new virtual int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public bool IsDelete { get; set; }
    }

}
