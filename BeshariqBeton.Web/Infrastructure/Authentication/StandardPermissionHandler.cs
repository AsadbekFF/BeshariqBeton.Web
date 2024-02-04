using Microsoft.AspNetCore.Authorization;

namespace BeshariqBeton.Web.Infrastructure.Authentication
{
    public class StandardPermissionHandler : AuthorizationHandler<StandardPermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StandardPermissionRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                return Task.CompletedTask;
            }

            if (context.User.HasClaim(CustomClaimTypes.StandardPermission, requirement.Permission.SystemName))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
