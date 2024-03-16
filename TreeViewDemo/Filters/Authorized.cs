using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TreeViewDemo.Data;
using TreeViewDemo.Models;

namespace TreeViewDemo.Filters
{
    public class Authorized : ActionFilterAttribute, IAuthorizationFilter
    {
        private bool IsAuthenticated { get; set; }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(m => m is AccessAnonymous))
            {
                return;
            }

            var db = (AppDbContext)context.HttpContext.RequestServices.GetService(typeof(AppDbContext));
            AppUser user = null;

            if (context != null)
            {
                var cookie = context.HttpContext.Request.Cookies[Globals.AuthenticationCookieName];
                user = db.AppUserLoginHistories.Where(m => m.Token == cookie).Select(m => m.User).FirstOrDefault();
            }

            IsAuthenticated = user != null;

            if (IsAuthenticated) return;
            context.Result = new RedirectResult("~/Account/Login");
        }
    }

    public class AccessAnonymous : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context) { }
    }
}
