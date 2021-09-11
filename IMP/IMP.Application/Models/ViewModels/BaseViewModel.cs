using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models
{
    public abstract class BaseViewModel<TKey>
    {
        public TKey Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
    }

    public abstract class BaseCreateModel
    {

    }

    public abstract class BaseUpdateModel<TKey>
    {
        public TKey Id { get; set; }
    }

}
