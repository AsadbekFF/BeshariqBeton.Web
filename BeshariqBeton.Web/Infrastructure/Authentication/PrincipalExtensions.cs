﻿using BeshariqBeton.Common.Enums;
using System.Security.Claims;
using System.Security.Principal;

namespace BeshariqBeton.Web.Infrastructure.Authentication
{
    public static class PrincipalExtensions
    {
        public static bool IsAuthenticated(this IPrincipal principal)
        {
            return principal.Identity.IsAuthenticated;
        }

        public static int GetUserId(this IPrincipal principal)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            var claim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId);
            return claim == null ? 0 : int.Parse(claim.Value);
        }

        public static string GetUsername(this IPrincipal principal)
        {
            return principal.Identity.Name;
        }

        public static UserRole GetUserRole(this IPrincipal principal)
        {
            if (principal.IsInRole(UserRoleConstants.Administrator))
                return UserRole.Admin;

            return UserRole.User;
        }

        public static bool IsAdministrator(this IPrincipal principal)
        {
            return principal.IsInRole(UserRoleConstants.Administrator);
        }

        public static bool IsUser(this IPrincipal principal)
        {
            return principal.IsInRole(UserRoleConstants.User);
        }

        public static bool IsRememberMe(this IPrincipal principal)
        {
            var rememberMeClaim = GetClaim(principal, CustomClaimTypes.RememberMe);
            return rememberMeClaim.Value == true.ToString();
        }

        public static byte GetSessionTimeout(this IPrincipal principal)
        {
            var sessionTimeoutClaim = GetClaim(principal, CustomClaimTypes.SessionTimeout);

            if (sessionTimeoutClaim.Value == null)
                return 0;

            return byte.Parse(sessionTimeoutClaim.Value);
        }

        private static Claim GetClaim(IPrincipal principal, string claimName)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            return claimsIdentity.Claims.FirstOrDefault(c => c.Type == claimName);
        }
    }
}
