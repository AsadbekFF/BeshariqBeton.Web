using BeshariqBeton.Common.Entities;
using Microsoft.AspNetCore.Authorization;

namespace BeshariqBeton.Web.Infrastructure.Authentication
{
    public class StandardPermissionRequirement : IAuthorizationRequirement
    {
        public StandardPermissionRequirement(StandardPermission permission)
        {
            Permission = permission;
        }

        public StandardPermission Permission { get; set; }
    }
}
