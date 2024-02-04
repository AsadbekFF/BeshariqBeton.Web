using BeshariqBeton.Common.Entities;
using BeshariqBeton.Common.Security;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BeshariqBeton.Web.Infrastructure.Authentication
{
    public static class AuthorizationServiceExtensions
    {
        public static async Task<bool> AuthorizeAsync(this IAuthorizationService authorizationService, ClaimsPrincipal user,
            StandardPermission standardPermission)
        {
            if (standardPermission == null)
                return false;

            return (await authorizationService.AuthorizeAsync(user, null, new StandardPermissionRequirement(standardPermission))).Succeeded;
        }

        public static async Task<bool> AuthorizeAsync(this IAuthorizationService authorizationService, ClaimsPrincipal user,
            IAuthorizationRequirement requirement)
        {
            if (user == null)
                return false;

            return (await authorizationService.AuthorizeAsync(user, null, requirement)).Succeeded;
        }
    }
}
