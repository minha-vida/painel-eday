using System.Web.Mvc;
using EdeaDay.GestaoDeIdeias.Model;
using EdeaDay.GestaoDeIdeias.Model.Enums;

namespace EdeaDay.GestaoDeIdeias.Site.Helpers
{
    public static class ImagemHelper
    {
        public static string ObterUrlLogoIdeia(this UrlHelper urlHelper, Ideia ideia)
        {
            var imagePath = ideia.Foto == null || ideia.Foto.Length.Equals(0)
                                ? urlHelper.Content("~/Content/images/logos/logoPadrao.jpg")
                                : string.Format("/image.ashx?iid={0}",ideia.IdeiaID);

            return imagePath;
        }

        public static string ObterUrlMedalhaIdeia(this UrlHelper urlHelper, Ideia ideia)
        {
            switch (ideia.Status)
            {
                case (Status.Concreto):
                    return urlHelper.Content("~/Content/images/medalhas/medalhaOuro.png");
                case (Status.Amadurecendo):
                    return urlHelper.Content("~/Content/images/medalhas/medalhaPrata.png");
                default:
                    return urlHelper.Content("~/Content/images/medalhas/medalhaBronze.png");
            }
        }

        public static string ObterUrlImagemPerfil(this UrlHelper urlHelper, Usuario usuario)
        {
            var imagePath = string.IsNullOrEmpty(usuario.Foto)
                                ? urlHelper.Content("~/Content/images/figuras/avatarPadrao.jpg")
                                : usuario.Foto;

            return imagePath;
        }
    }
}