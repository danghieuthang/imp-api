using IMP.Application.Enums;
using IMP.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IMP.WebApi.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue("uid");
            AppId = httpContextAccessor.HttpContext?.User?.FindFirstValue("appid");
            IsAdmin = httpContextAccessor.HttpContext?.User?.Claims.Where(x => x.Type == ClaimTypes.Role).Any(x => x.Value == Roles.Administrator.ToString());
        }

        public string UserId { get; }

        public string AppId { get; }

        public bool? IsAdmin { get; }
        public int ApplicationUserId
        {
            get
            {
                int id;
                if (int.TryParse(AppId, out id))
                {
                    return id;
                }
                return 0;
            }
        }
    }
}
