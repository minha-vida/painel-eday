using System.ComponentModel.DataAnnotations.Schema;
using EdeaDay.GestaoDeIdeias.Model.Enums;

namespace EdeaDay.GestaoDeIdeias.Model
{
    [Table("Colaboradores")]
    public class Colaborador
    {
        public virtual int ColaboradorID { get; set; }

        public virtual int IdeiaID { get; set; }
        
        public virtual string UsuarioID { get; set; }

        public virtual Cargo Cargo { get; set; }

        public virtual Ideia Ideia { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
