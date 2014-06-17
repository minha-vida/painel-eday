using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdeaDay.GestaoDeIdeias.Model
{
    public class Brainstorm
    {
        public virtual int BrainstormID { get; set; }

        [Required(AllowEmptyStrings=false)]
        public virtual string Texto { get; set; }

        public virtual DateTime DataDeCriacao { get; set; }
    }
}
