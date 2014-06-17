namespace EdeaDay.GestaoDeIdeias.Repository.Comentario
{
    public class ComentarioRepository : Repository<Eday, Model.Comentario>, IComentarioRepository
    {
        public ComentarioRepository(Eday context) : base(context)
        {
        }
    }
}
