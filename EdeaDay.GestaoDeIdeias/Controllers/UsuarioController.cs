using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EdeaDay.GestaoDeIdeias.Model;
using EdeaDay.GestaoDeIdeias.Repository;
using EdeaDay.GestaoDeIdeias.Site.Extensions;

namespace EdeaDay.GestaoDeIdeias.Site.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly Eday _context = new Eday();

        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController()
        {
            _usuarioRepository = new UsuarioRepository(_context);
        }

        public ActionResult Login()
        {
            return PartialView("_Login", this.UserSession(_usuarioRepository).UsuarioAtual());
        }

        public ActionResult Logout()
        {
            this.UserSession(_usuarioRepository).LogoutUsuario();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Perfil(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirecionarUsuarioParaOProprioPerfil();
            }

            var usuario = _usuarioRepository.Find(id);

            if (usuario == null)
            {
                throw new HttpException(404, "Usuário não encontrado");
            }

            return View("Perfil", usuario);
        }

        private ActionResult RedirecionarUsuarioParaOProprioPerfil()
        {
            var usuarioAtualId = this.UserSession(_usuarioRepository).UsuarioAtual().UsuarioID;

            return RedirectToAction("Perfil", new {id = usuarioAtualId});
        }

        [ChildActionOnly]
        public ActionResult BoxBrainstorm()
        {
            var lstBrainstorm = this.UserSession(_usuarioRepository).UsuarioAtual().Brainstorms;

            return PartialView("_BoxBrainstorm", lstBrainstorm);
        }

        [HttpPost]
        public ActionResult CriarBrainstorm(string textoBrainstorm)
        {
            var usuario = this.UserSession(_usuarioRepository).UsuarioAtual();

            var novoBrainstorm = new Brainstorm
            {
                DataDeCriacao = DateTime.Now,
                Texto = textoBrainstorm
            };

            usuario.Brainstorms.Add(novoBrainstorm);

            try
            {
                _usuarioRepository.Edit(usuario);

                _usuarioRepository.Save();

                return Json(new { id=novoBrainstorm.BrainstormID }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return new HttpStatusCodeResult(500);
            }

        }

        [HttpPost]
        public ActionResult EditarBrainstorm(int id, string texto)
        {
            var usuario = this.UserSession(_usuarioRepository).UsuarioAtual();

            var brainstorm = (from brain in usuario.Brainstorms
                             where brain.BrainstormID == id
                             select brain).First();

            brainstorm.Texto = texto;
            try
            {
                _usuarioRepository.Edit(usuario);

                _usuarioRepository.Save();

                return Json(new { status = "ok" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return new HttpStatusCodeResult(500);
            }

        }

        [HttpPost]
        public ActionResult DeletarBrainstorm(int id)
        {
            var usuario = this.UserSession(_usuarioRepository).UsuarioAtual();

            var brainstorm = (from brain in usuario.Brainstorms
                              where brain.BrainstormID == id
                              select brain).First();

            usuario.Brainstorms.Remove(brainstorm);
            
            try
            {
                _usuarioRepository.DeleteBrainstorm(brainstorm);
                _usuarioRepository.Save();

                return Json(new { status = "ok" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(500);
            }

        }
    }
}
