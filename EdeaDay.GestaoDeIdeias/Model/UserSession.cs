using System;
using System.Web;
using System.Web.Mvc;
using EdeaDay.GestaoDeIdeias.Model;
using EdeaDay.GestaoDeIdeias.Model.Infrastructure;
using EdeaDay.GestaoDeIdeias.Repository;

namespace EdeaDay.GestaoDeIdeias.Site.Model
{
    public class UserSession
    {
        private readonly IUsuarioRepository _repository;

        private readonly IUserStateManager _stateManager;

        public UserSession(IUserStateManager stateManager, IUsuarioRepository repository = null)
        {
            _repository = repository ?? ObterRepository();

            _stateManager = stateManager;
        }

        [Authorize]
        public Usuario UsuarioAtual()
        {
            var user = _repository.Find(_stateManager.GetCurrentUserId());

            return user;
        }

        public void LoginUsuario(Usuario usuario, HttpContextBase context)
        {
            if (usuario == null || !usuario.IsValidUser)
            {
                throw new AccessViolationException("Usuário inválido. No momento, apenas usuários internos são permitidos.");
            }

            _stateManager.Login(usuario.UsuarioID, context);

            _repository.Upsert(usuario, _repository.UserNotExists);

            _repository.Save();
        }

        public void LogoutUsuario()
        {
            _stateManager.LogoffCurrentUser();
        }

        private static IUsuarioRepository ObterRepository()
        {
            return new UsuarioRepository(new Eday());
        }
    }
}