using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using EdeaDay.GestaoDeIdeias.Site.Model;

namespace EdeaDay.GestaoDeIdeias.Site.Helpers
{
    public static class LoginUrlHelper
    {
        public static string ObterUrlParaGoogleAuthentication(this UrlHelper urlHelper)
        {
            var googleParameters = new GoogleParameters();

            var url = new StringBuilder();

            var context = urlHelper.RequestContext.HttpContext;

            var state = ObterState(context);

            url.Append("https://accounts.google.com/o/oauth2/auth?");
            url.Append(
                "scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fuserinfo.email+https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fuserinfo.profile&");
            url.AppendFormat("redirect_uri={0}&", googleParameters.RedirectUrl);
            url.AppendFormat("state={0}&", state);
            url.Append("response_type=code&");
            url.AppendFormat("client_id={0}&", googleParameters.ClientId);
            url.Append("access_type=offline");

            return url.ToString();
        }

        private static object ObterState(HttpContextBase context)
        {
            var sessionId = context.Session != null ? context.Session.SessionID : string.Empty;

            var returnUrl = context.Request.Url != null ? context.Request.Url.Query.Replace("?ReturnUrl=", string.Empty) : string.Empty;

            var securityToken = string.IsNullOrEmpty(sessionId)
                                    ? string.Empty
                                    : string.Format("security_token%3D{0}", sessionId);

            var url = string.IsNullOrEmpty(returnUrl) ? string.Empty : string.Format("url%3D{0}", returnUrl);

            return string.Format("{0}%26{1}", securityToken, url);
        }
    }
}