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
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
    }
    public abstract class Entity<T> : IEntity<T>, ISoftDeletable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
