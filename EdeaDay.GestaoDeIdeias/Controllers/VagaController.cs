using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using EdeaDay.GestaoDeIdeias.Model;
using EdeaDay.GestaoDeIdeias.Model.Enums;
using EdeaDay.GestaoDeIdeias.Model.Utils;
using EdeaDay.GestaoDeIdeias.Repository;
using EdeaDay.GestaoDeIdeias.Site.Extensions;

namespace EdeaDay.GestaoDeIdeias.Site.Controllers
{
    [Authorize]
    public class VagaController : Controller
    {
        private readonly Eday _context = new Eday();

        readonly IVagaRepository _vagaRepository;
        readonly IIdeiaRepository _ideiaRepository;
        readonly IUsuarioRepository _usuarioRepository;

        public VagaController()
        {
            _usuarioRepository = new UsuarioRepository(_context);
            _ideiaRepository = new IdeiaRepository(_context);
            _vagaRepository = new VagaRepository(_context);
        }

        public ActionResult Add(FormCollection formCollection)
        {
            var formCargoId = formCollection["Cargo"];
            var formIdeiaId = formCollection["IdeiaID"];

            var ideia = _ideiaRepository.Find(int.Parse(formIdeiaId));

            var cargo = (Cargo)int.Parse(formCargoId);

            var vaga = _vagaRepository.Create();

            vaga.Ideia = ideia;

            vaga.Cargo = cargo;

            ideia.Vagas.Add(vaga);

            _ideiaRepository.Edit(ideia);

            _ideiaRepository.Save();

            return PartialView("_BoxAdicionarVagas", ideia);
        }

        public ActionResult Subscribe(Guid id)
        {
            var vaga = _vagaRepository.Find(id);

            var usuario = this.UserSession(_usuarioRepository).UsuarioAtual();

            if (vaga.Ideia.UserIsIdealizador(usuario))
            {
                throw new NotSupportedException("Desculpe, idealizadores não podem ocupar outros cargos em suas próprias ideias.");
            }

            vaga.Candidatos.Add(usuario);

            _vagaRepository.Edit(vaga);

            _vagaRepository.Save();

            var idealizador = vaga.Ideia.Idealizador;

            this.EnviarEmailDeSucessoDaInscricao(usuario, vaga);

            this.EnviarEmailDeAprovacaoDeInscricao(idealizador, usuario, vaga);

            return PartialView("_BoxInscricao", vaga.Ideia);
        }

        public ActionResult AcceptSubscription(Guid id, string usuarioId)
        {
            var vaga = _vagaRepository.Find(id);

            var ideia = vaga.Ideia;

            var usuarioAtual = this.UserSession(_usuarioRepository).UsuarioAtual();

            if (!ideia.UserIsIdealizador(usuarioAtual))
            {
                throw new AccessViolationException("Você não tem permissão para aceitar esta vaga. Apenas o Idealizador pode aceitar vagas.");
            }

            var usuario = _usuarioRepository.Find(usuarioId);

            var colaborador = new Colaborador
                {
                    Cargo = vaga.Cargo,
                    Ideia = vaga.Ideia,
                    IdeiaID = vaga.Ideia.IdeiaID,
                    Usuario = usuario,
                    UsuarioID = usuario.UsuarioID
                };

            ideia.Envolvidos.Add(colaborador);

            this.EnviarEmailDeInscricaoAprovada(usuario, vaga);

            ideia.Vagas.Remove(vaga);

            _ideiaRepository.Save();

            return RedirectToAction("Details", "Ideia", new { id = ideia.IdeiaID });
        }

        public ActionResult ListSubscription(Guid id)
        {
            var vaga = _vagaRepository.Find(id);

            return PartialView("_BoxInscricoes", vaga);
        }

        public ActionResult Delete(Guid id)
        {
            var vaga = _vagaRepository.Find(id);

            var ideia = _ideiaRepository.Find(vaga.Ideia.IdeiaID);

            var usuario = this.UserSession(_usuarioRepository).UsuarioAtual();

            if (!ideia.UserIsIdealizador(usuario))
            {
                throw new AccessViolationException("Você não tem permissão para excluir esta vaga. Apenas o Idealizador pode excluir vagas.");
            }

            _vagaRepository.Delete(vaga);

            _vagaRepository.Save();

            return PartialView("_BoxAdicionarVagas", ideia);
        }
    }
}
