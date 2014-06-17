using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EdeaDay.GestaoDeIdeias.Model.Enums;

namespace EdeaDay.GestaoDeIdeias.Model
{
    public class Ideia
    {
        public virtual int IdeiaID { get; set; }

        [Display(Name = "Título")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Digite um título")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "O título da sua ideia não pode ter mais de 50 caracteres")]
        public virtual string Titulo { get; set; }

        [Display(Name = "Descrição")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Faça uma breve descrição da sua ideia")]
        [MaxLength(190)]
        [StringLength(190, ErrorMessage = "Sua descrição deve ter no máximo 190 caracteres")]
        public virtual string Descricao { get; set; }

        public virtual byte[] Foto { get; set; }

        public virtual Status Status { get; set; }

        [Display(Name = "Data de Início")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public virtual DateTime DataInicial { get; set; }

        public virtual ICollection<Colaborador> Envolvidos { get; set; }

        public virtual ICollection<Vaga> Vagas { get; set; }

        public virtual ICollection<Comentario> Comentarios { get; set; }

        [NotMapped]
        public Usuario Idealizador
        {
            get { return FindIdealizador(); }
        }

        private Usuario FindIdealizador()
        {
            var idealizador = Envolvidos.First(i => i.Cargo.Equals(Cargo.Idealizador));

            return idealizador.Usuario;
        }

        public bool UserIsIdealizador(Usuario usuario)
        {
            return usuario.Equals(Idealizador);
        }

        public IEnumerable<Colaborador> FindCargosForUser(Usuario usuario)
        {
            return Envolvidos.Where(u => u.Usuario.UsuarioID == usuario.UsuarioID);
        }
    }
}
