using System;
using System.Collections.Generic;
using System.Text;

namespace IMP.Application.Interfaces
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}
