using System.Web.Mvc;
using EdeaDay.GestaoDeIdeias.Repository;
using EdeaDay.GestaoDeIdeias.Site.Extensions;

namespace EdeaDay.GestaoDeIdeias.Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly Eday _context = new Eday();

        private IUsuarioRepository _usuarioRepository;

        public HomeController()
        {
            _usuarioRepository = new UsuarioRepository(_context);
        }

        public ActionResult Index()
        {
            if (this.UserSession(_usuarioRepository).UsuarioAtual() != null)
            {
                return RedirectToAction("Index", "Ideia");
            }

            return View();
        }
     
        public ActionResult Error()
        {
            return View();
        }

        public ActionResult NotFoundError()
        {
            return View();
        }
    }
}
