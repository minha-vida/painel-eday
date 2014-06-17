using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EdeaDay.GestaoDeIdeias.Model
{
    public class Vaga
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid VagaID { get; set; }

        public virtual Enums.Cargo Cargo { get; set; }

        public virtual Ideia Ideia { get; set; }

        public virtual ICollection<Usuario> Candidatos { get; set; }
    }
}
