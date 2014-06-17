using EdeaDay.GestaoDeIdeias.Model;
using System.Collections.Generic;
using System.Linq;
using EdeaDay.GestaoDeIdeias.Repository;

namespace EdeaDay.GestaoDeIdeias.Site.ViewModel
{
    public class IdeiaViewModel
    {
        public IEnumerable<Ideia> Ideias { get; set; }

        public Usuario UsuarioAtual { get; set; }
    }
}
