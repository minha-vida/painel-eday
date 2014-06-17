using System.Collections.Generic;
using EdeaDay.GestaoDeIdeias.Model;

namespace EdeaDay.GestaoDeIdeias.Repository
{
    public interface IIdeiaRepository : IRepository<Ideia>
    {
        IEnumerable<Ideia> FindPorUsuario(Usuario usuario);
        IEnumerable<Ideia> FindComVagasAbertas();
        byte[] ObterImagemPorIdeiaId(int ideiaId);
    }
}
