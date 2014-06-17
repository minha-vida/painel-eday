using System.Configuration;
using EdeaDay.GestaoDeIdeias.Helpers;

namespace EdeaDay.GestaoDeIdeias.Site.Model
{
    public class GoogleParameters : IAuthenticationParameters
    {
        public string ClientSecret { get; internal set; }

        public string ClientId { get; internal set; }

        public string RedirectUrl { get; internal set; }

        public GoogleParameters()
        {
            RedirectUrl = ConfigurationManager.AppSettings["GoogleRedirectUrl"];
            ClientId = ConfigurationManager.AppSettings["GoogleClientId"];
            ClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"];
        }
    }
}