using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using EdeaDay.GestaoDeIdeias.Model;
using EdeaDay.GestaoDeIdeias.Model.Utils;
using EdeaDay.GestaoDeIdeias.Repository;
using EdeaDay.GestaoDeIdeias.Site.Extensions;
using EdeaDay.GestaoDeIdeias.Site.ViewModel;
using System.Web;

namespace EdeaDay.GestaoDeIdeias.Site.Controllers
{
    [Authorize]
    public class IdeiaController : Controller
    {
        private readonly Eday _context = new Eday();

        readonly IIdeiaRepository _ideiaRepository;
        readonly IUsuarioRepository _usuarioRepository;

        public IdeiaController()
        {
            _usuarioRepository = new UsuarioRepository(_context);
            _ideiaRepository = new IdeiaRepository(_context);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            var ideia = _ideiaRepository.Find(id);

            if (ideia == null)
            {
                throw new HttpException(404, "Ideia não encontrada");
            }

            return View(ideia);
        }

        [HttpPost]
        public ActionResult Add(Ideia ideia)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var usuario = this.UserSession(_usuarioRepository).UsuarioAtual();

            var novaIdeia = _ideiaRepository.Create();

            novaIdeia.Titulo = ideia.Titulo;
            novaIdeia.Descricao = ideia.Descricao;
            novaIdeia.DataInicial = DateTime.Now;
            novaIdeia.Status = GestaoDeIdeias.Model.Enums.Status.Inicial;

            novaIdeia.Foto = ObterFotoDoForm(Request.Files["flFotoBoxIdealizar"], novaIdeia.Foto);


            var colaborador = new Colaborador
                                  {
                                      IdeiaID = novaIdeia.IdeiaID,
                                      UsuarioID = usuario.UsuarioID,
                                      Cargo = GestaoDeIdeias.Model.Enums.Cargo.Idealizador
                                  };

            novaIdeia.Envolvidos.Add(colaborador);

            _ideiaRepository.Add(novaIdeia);

            _ideiaRepository.Save();

            this.EnviarEmailDeNotificacaoDeIdeiaCriada(novaIdeia);

            return RedirectToAction("Manager", new { id = novaIdeia.IdeiaID });
        }

        private static byte[] ObterFotoDoForm(HttpPostedFileBase file, byte[] fotoDefault)
        {
            var validImageTypes = new List<string> { "image/jpeg", "image/pjpeg" };

            if (file == null)
            {
                return fotoDefault;
            }

            if (validImageTypes.Contains(file.ContentType))
            {
                var foto = ConverterImagemEmBinario(file);

                foto = foto.Length.Equals(0) ? fotoDefault : foto;

                foto = foto.Length > 1048576 ? fotoDefault : foto;

                return foto;
            }

            return fotoDefault;
        }

        public ActionResult Manager(int id)
        {
            var ideia = _ideiaRepository.Find(id);

            var usuario = this.UserSession(_usuarioRepository).UsuarioAtual();

            if (!ideia.UserIsIdealizador(usuario))
            {
                return RedirectToAction("Details", "Ideia", new { id });
            }

            return View(ideia);
        }

        [HttpPost]
        public ActionResult Manager(Ideia ideia)
        {
            var ideiaAtual = _ideiaRepository.Find(ideia.IdeiaID);

            var usuario = this.UserSession(_usuarioRepository).UsuarioAtual();

            if (!ideiaAtual.UserIsIdealizador(usuario))
            {
                throw new AccessViolationException("Somente o idealizador da ideia pode utilizar o Manager!");
            }

            ideiaAtual.Titulo = ideia.Titulo;
            ideiaAtual.Descricao = ideia.Descricao;
            ideiaAtual.Status = ideia.Status;
            ideiaAtual.DataInicial = ideia.DataInicial;

            ideiaAtual.Foto = ObterFotoDoForm(Request.Files["flFoto"], ideiaAtual.Foto);

            _ideiaRepository.Edit(ideiaAtual);

            _ideiaRepository.Save();

            return View(ideiaAtual);
        }

        [ChildActionOnly]
        public ActionResult BoxFixoListaIdeias()
        {
            var viewModel = new IdeiaViewModel();
            var usuario = this.UserSession(_usuarioRepository).UsuarioAtual();

            viewModel.UsuarioAtual = usuario;
            viewModel.Ideias = _ideiaRepository.FindPorUsuario(usuario);

            return PartialView("_BoxFixoListaIdeias", viewModel);
        }

        public ActionResult ListaDeIdeias(string id)
        {
            IEnumerable<Ideia> ideias = new List<Ideia>();
            
            var usuario = this.UserSession(_usuarioRepository).UsuarioAtual();

            switch (id)
            {
                case "1":
                    ideias = _ideiaRepository.FindPorUsuario(usuario);
                    break;
                case "2":
                    ideias = _ideiaRepository.FindComVagasAbertas();
                    break;
                case "3":
                    ideias = _ideiaRepository.All;
                    break;
            }
            
            var viewModel = new IdeiaViewModel
                {
                    Ideias = ideias.ToList(),
                    UsuarioAtual = usuario
                };

            return View("_ListaDeIdeias", viewModel);
        }

        private static byte[] ConverterImagemEmBinario(HttpPostedFileBase imagem)
        {
            var image = new byte[imagem.ContentLength];
            imagem.InputStream.Read(image, 0, image.Length);

            return image;
        }
    }
}
