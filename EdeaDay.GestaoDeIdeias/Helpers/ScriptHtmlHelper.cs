using System;
using System.Web.Mvc;

namespace EdeaDay.GestaoDeIdeias.Site.Helpers
{
    public static class ScriptHtmlHelper
    {
        public static MvcHtmlString InserirScriptSemCache(this HtmlHelper htmlHelper, string pathRelativo)
        {
            const string scriptPattern = "<script src='{0}?v={1}'></script>";

            var cacheKey = DateTime.Today.Ticks;

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var url = urlHelper.Content(pathRelativo);

            var script = string.Format(scriptPattern, url, cacheKey);

            return new MvcHtmlString(script);
        }

        public static MvcHtmlString InserirCssSemCache(this HtmlHelper htmlHelper, string pathRelativo)
        {
            const string scriptPattern = "<link href='{0}?v={1}' rel='stylesheet' type='text/css' />";

            var cacheKey = DateTime.Today.Ticks;

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var url = urlHelper.Content(pathRelativo);

            var script = string.Format(scriptPattern, url, cacheKey);

            return new MvcHtmlString(script);
        }
    }
}