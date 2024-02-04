using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeshariqBeton.Web.Utilities
{
    public static class RouteHelper
    {
        public static bool IsEditPage(this IHtmlHelper html)
        {
            var action = html.ViewContext.RouteData.Values["action"].ToString().ToLower();
            return action == "edit";
        }

        public static bool IsAddPage(this IHtmlHelper html)
        {
            var action = html.ViewContext.RouteData.Values["action"].ToString().ToLower();
            return action == "add";
        }
    }
}
