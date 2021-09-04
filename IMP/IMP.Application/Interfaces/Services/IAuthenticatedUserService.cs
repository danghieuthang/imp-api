using System;
using System.Collections.Generic;
using System.Text;

namespace IMP.Application.Interfaces
{
    public interface IAuthenticatedUserService
    {
        string UserId { get; }
    }
}
