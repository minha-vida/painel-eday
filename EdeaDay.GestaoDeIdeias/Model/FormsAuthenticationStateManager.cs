using System;
using System.Web;
using System.Web.Security;
using EdeaDay.GestaoDeIdeias.Model.Infrastructure;

namespace EdeaDay.GestaoDeIdeias.Site.Model
{
    public class FormsAuthenticationStateManager : IUserStateManager
    {
        public void Login(string userId, HttpContextBase context)
        {
            FormsAuthentication.SetAuthCookie(userId, true);

            var ticket = new FormsAuthenticationTicket(2, userId, DateTime.Now, DateTime.Now.AddYears(50), true, string.Empty, FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket))
            {
                Domain = FormsAuthentication.CookieDomain,
                Expires = DateTime.Now.AddYears(50),
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                Path = FormsAuthentication.FormsCookiePath
            };
            
            context.Response.Cookies.Add(cookie);
        }

        public void LogoffCurrentUser()
        {
            FormsAuthentication.SignOut();
        }

        public string GetCurrentUserId()
        {
            return HttpContext.Current.User.Identity.Name;
        }
    }
}