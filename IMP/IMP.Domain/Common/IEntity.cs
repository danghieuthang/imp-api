using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Common
{
    public interface IEntity<T>
    {
        public T Id { get; set; }
    }

    public abstract class Entity<T> : IEntity<T>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }
    }
}
