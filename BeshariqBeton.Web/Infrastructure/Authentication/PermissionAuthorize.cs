using Microsoft.AspNetCore.Authorization;

namespace BeshariqBeton.Web.Infrastructure.Authentication
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        public PermissionAuthorizeAttribute(string standardPermissionName)
        {
            Policy = standardPermissionName;
        }
    }
}
