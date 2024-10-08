﻿using BeshariqBeton.Common.Entities;
using BeshariqBeton.Common.Utilities;
using System.Security.Claims;

namespace BeshariqBeton.Web.Infrastructure.Authentication
{
    public static class ClaimsProvider
    {
        public static List<Claim> GetUserClaims(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, user.Username),
                new(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToValue().ToString()),
                new(CustomClaimTypes.UserId, user.Id.ToString()),
                new(CustomClaimTypes.DateTimeIssued, DateTime.UtcNow.Ticks.ToString()),
            };

            // Standard permissions
            foreach (var permission in user.StandardPermissions)
                claims.Add(new Claim(CustomClaimTypes.StandardPermission, permission.StandardPermission.SystemName));

            return claims;
        }

        public static List<Claim> GetUserClaims(User user, bool rememberMe, byte sessionTimeout)
        {
            var claims = GetUserClaims(user);

            claims.Add(new Claim(CustomClaimTypes.RememberMe, rememberMe.ToString()));
            claims.Add(new Claim(CustomClaimTypes.SessionTimeout, sessionTimeout.ToString()));

            return claims;
        }
    }
}
