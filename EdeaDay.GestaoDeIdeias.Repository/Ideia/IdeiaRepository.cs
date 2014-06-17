using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EdeaDay.GestaoDeIdeias.Model;

namespace EdeaDay.GestaoDeIdeias.Repository
{
    public class IdeiaRepository : Repository<Eday, Ideia>, IIdeiaRepository
    {
        public IdeiaRepository(Eday context) : base(context) { }

        public IEnumerable<Ideia> FindPorUsuario(Usuario usuario)
        {
            var ideias = from i in Context.Ideias
                         join iu in Context.Colaboradores
                         on i.IdeiaID equals iu.Ideia.IdeiaID
                         where iu.Usuario.UsuarioID.Equals(usuario.UsuarioID)
                         select i;

            return ideias.Distinct().ToList().OrderBy(ideia => ideia.Titulo);
        }

        public IEnumerable<Ideia> FindComVagasAbertas()
        {
            var ideias = from i in Context.Ideias
                         where i.Vagas.Any()
                         select i;

            return ideias;
        }

        public byte[] ObterImagemPorIdeiaId(int ideiaId)
        {
            var imagem = from i in Context.Ideias
                         where i.IdeiaID == ideiaId
                         select i.Foto;

            return imagem.SingleOrDefault();
        }
    }
}
