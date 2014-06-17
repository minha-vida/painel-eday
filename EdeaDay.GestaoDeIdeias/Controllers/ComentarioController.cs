using System;
using System.Linq;
using System.Web.Mvc;
using EdeaDay.GestaoDeIdeias.Model;
using EdeaDay.GestaoDeIdeias.Repository;
using EdeaDay.GestaoDeIdeias.Repository.Comentario;
using EdeaDay.GestaoDeIdeias.Site.Extensions;

namespace EdeaDay.GestaoDeIdeias.Site.Controllers
{
    [Authorize]
    public class ComentarioController : Controller
    {
        private readonly Eday _context = new Eday();

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IIdeiaRepository _ideiaRepository;
        private readonly IComentarioRepository _comentarioRepository;

        public ComentarioController()
        {
            _comentarioRepository = new ComentarioRepository(_context);
            _ideiaRepository = new IdeiaRepository(_context);
            _usuarioRepository = new UsuarioRepository(_context);
        }

        [HttpGet]
        public ActionResult ComentariosEmIdeia(Ideia ideia, int comentariosPreCarregados)
        {
            var ideiaComentada = _ideiaRepository.Find(ideia.IdeiaID);

            var comentariosParaJson = ObterAnonymousObjectDosComentarios(ideiaComentada, comentariosPreCarregados);

            return Json(comentariosParaJson, JsonRequestBehavior.AllowGet);
        }

        private object ObterAnonymousObjectDosComentarios(Ideia ideiaComentada, int comentariosPreCarregados)
        {
            const int valorMaximoDeComentariosParaLista = 10;

            var comentariosDaIdeia = ideiaComentada.Comentarios.OrderByDescending(t => t.Data)
                                                   .Skip(comentariosPreCarregados)
                                                   .Take(valorMaximoDeComentariosParaLista);

            var usuarioId = this.UserSession(_usuarioRepository).UsuarioAtual().UsuarioID;

            var comentarios = comentariosDaIdeia.Select(t => t.ToAnonymousType(usuarioId)).ToList();

            comentariosPreCarregados += comentarios.Count();

            var objetoJson = new
                {
                    Comentarios = comentarios,
                    ComentariosRestantes = ideiaComentada.Comentarios.Count - comentariosPreCarregados
                };

            return objetoJson;
        }

        [HttpPost]
        public ActionResult ComentarioIdeia(Ideia ideia, string textoComentario)
        {
            var usuario = this.UserSession(_usuarioRepository).UsuarioAtual();

            var ideiaComentada = _ideiaRepository.Find(ideia.IdeiaID);

            var comentario = new Comentario
                {
                    Data = DateTime.Now,
                    Texto = textoComentario,
                    Usuario = usuario
                };

            ideiaComentada.Comentarios.Add(comentario);

            _ideiaRepository.Save();

            this.EnviarEmailDeNotificacaoDoComentarioNaIdeia(ideiaComentada, comentario);

            var comentarios = new { Comentarios = new[] { comentario.ToAnonymousType(usuario.UsuarioID) } };

            return Json(comentarios);
        }

        [HttpPost]
        public ActionResult ExcluirComentario(Comentario comentario)
        {
            var usuario = this.UserSession(_usuarioRepository).UsuarioAtual();

            var comentarioParaExclusao = _comentarioRepository.Find(comentario.ComentarioId);

            if(comentarioParaExclusao == null)
                return new EmptyResult();

            if (comentarioParaExclusao.Usuario.UsuarioID != usuario.UsuarioID)
            {
                throw new AccessViolationException("Somente o usuário que criou o comentário pode excluí-lo.");
            }

            _comentarioRepository.Delete(comentarioParaExclusao);

            _comentarioRepository.Save();

            return new EmptyResult();
        }
    }
}
