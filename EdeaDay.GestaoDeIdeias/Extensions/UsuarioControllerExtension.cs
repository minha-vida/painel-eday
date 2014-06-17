using System.Web.Mvc;
using EdeaDay.GestaoDeIdeias.Model.Infrastructure;
using EdeaDay.GestaoDeIdeias.Repository;
using EdeaDay.GestaoDeIdeias.Site.Model;

namespace EdeaDay.GestaoDeIdeias.Site.Extensions
{
    public static class UsuarioControllerExtension
    {
        public static UserSession UserSession(this Controller controller, IUsuarioRepository _repository)
        {
            var stateManager = ObterStateManager();

            var userSession = new UserSession(stateManager, _repository);

            return userSession;
        }

        private static IUserStateManager ObterStateManager()
        {
            return new FormsAuthenticationStateManager();
        }
    }
}