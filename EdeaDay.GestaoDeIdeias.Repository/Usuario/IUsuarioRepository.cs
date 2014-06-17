using EdeaDay.GestaoDeIdeias.Model;

namespace EdeaDay.GestaoDeIdeias.Repository
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        bool UserNotExists(Usuario user);

        void DeleteBrainstorm(Brainstorm brainstorm);
    }
}
