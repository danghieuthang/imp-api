using System;
using System.Collections.Generic;
using System.Text;

namespace IMP.Domain.Common
{
    public interface IEntity<T>
    {
        public T Id { get; set; }
    }
}
