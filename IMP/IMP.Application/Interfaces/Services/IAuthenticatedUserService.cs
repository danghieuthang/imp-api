using System;
using System.Collections.Generic;
using System.Text;

namespace IMP.Application.Interfaces
{
    public interface IAuthenticatedUserService
    {
        string UserId { get; }
        string AppId { get; }
        bool? IsAdmin { get; }
        int ApplicationUserId { get; }
        int? BrandId { get; }
    }
}
