using System.Web;
using System.Web.Mvc;

namespace EdeaDay.GestaoDeIdeias.Site.Extensions
{
    public class UrlProvider
    {
        private readonly HttpRequestBase _request;

        public UrlProvider(HttpRequestBase request)
        {
            _request = request;
        }

        public string ObterUrlParaManagerDaIdeia(int ideiaId)
        {
            return ObterUrlPorControllerEAction("Manager", "Ideia", new { id = ideiaId });
        }

        public string ObterUrlParaDetalhesDaIdeia(int ideiaId)
        {
            return ObterUrlPorControllerEAction("Details", "Ideia", new { id = ideiaId });
        }

        public string ObterUrlParaAprovacaoDaIdeia(int ideiaId)
        {
            return ObterUrlPorControllerEAction("Aprovar", "Ideia", new { id = ideiaId });
        }

        public string ObterUrlParaAprovacaoDeVaga(string vagaId, string usuarioId)
        {
            return ObterUrlPorControllerEAction("AcceptSubscription", "Vaga",
                                                new { id = vagaId, usuarioId = usuarioId });
        }

        public string ObterUrlPorControllerEAction(string nomeControllerDestino, string nomeActionDestino, object routeValues)
        {
            var urlHelper = new UrlHelper(_request.RequestContext);

            var endereco = urlHelper.Action(nomeControllerDestino, nomeActionDestino, routeValues);

            var requestUrl = _request.Url;

            var url = requestUrl.AbsoluteUri.Replace(requestUrl.PathAndQuery, string.Empty);

            return string.Format("{0}{1}", url, endereco);
        }

    }
}