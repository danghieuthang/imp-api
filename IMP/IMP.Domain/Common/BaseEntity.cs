using System;
using System.Collections.Generic;
using System.Text;

namespace IMP.Domain.Common
{
    public abstract class BaseEntity
    {
        public virtual int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public bool IsDelete { get; set; }
    }
}
