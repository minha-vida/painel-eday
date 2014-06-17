using EdeaDay.GestaoDeIdeias.Model;

namespace EdeaDay.GestaoDeIdeias.Repository
{
    public class VagaRepository : Repository<Eday, Vaga>, IVagaRepository
    {
        public VagaRepository(Eday context) : base(context) { }
    }
}
