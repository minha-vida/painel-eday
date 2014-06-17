using EdeaDay.GestaoDeIdeias.Model;
using System.Linq;

namespace EdeaDay.GestaoDeIdeias.Repository
{
    public class UsuarioRepository : Repository<Eday, Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(Eday context) : base(context) { }

        public override void Edit(Usuario entity)
        {
            //var user = Context.Usuarios.Attach(entity);
            //user.Foto = entity.Foto;
            //user.Nome = entity.Nome;

            //var user = Context.Usuarios.Find(entity.UsuarioID);

            //user.Foto = entity.Foto;
            //user.Nome = entity.Nome;
            //user.Brainstorms.Clear();

            //foreach (var brainstorm in entity.Brainstorms)
            //{
            //    user.Brainstorms.Add(brainstorm);
            //}

            base.Edit(entity);
        }

        // TODO: Remover esse método
        public void Detach(Usuario user)
        {
            Context.Entry(user).State = System.Data.EntityState.Detached;
        }

        public void DeleteBrainstorm(Brainstorm brainstorm)
        {
            Context.Brainstorm.Remove(brainstorm);
        }
        
        public bool UserNotExists(Usuario user)
        {
            var notExistingUser = GetAll().Count(u => u.UsuarioID.Equals(user.UsuarioID)) == 0;

            return notExistingUser;
        }
    }
}
