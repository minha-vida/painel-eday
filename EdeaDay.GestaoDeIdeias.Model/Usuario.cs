using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EdeaDay.GestaoDeIdeias.Model.Enums;

namespace EdeaDay.GestaoDeIdeias.Model
{
    public class Usuario
    {
        public virtual string UsuarioID { get; set; }

        public virtual string Nome { get; set; }

        [EmailAddress]
        public virtual string Email { get; set; }

        public virtual string Foto { get; set; }


        public virtual ICollection<Colaborador> Ideias { get; set; }

        public virtual ICollection<Brainstorm> Brainstorms { get; set; }

        public virtual ICollection<Vaga> VagasQueSeCandidatou { get; set; }


        [NotMapped]
        public string AccessToken { get; set; }

        [NotMapped]
        public bool IsValidUser
        {
            get
            {
                return ValidateUser();
            }
        }

        private bool ValidateUser()
        {
            var validado = System.Text.RegularExpressions.Regex.Match(Email, "([\\w-\\.]+)@minhavida.com.br");

            return validado.Success;
        }

        public IEnumerable<Cargo> UserOccupiedPositions()
        {
            var cargos = Ideias.Select(m => m.Cargo).Distinct();

            return cargos.ToList();
        }

        public override bool Equals(object obj)
        {
            var usuario = obj as Usuario;
            
            return usuario != null && usuario.UsuarioID.Equals(UsuarioID);
        }

        public object ToAnonymousType()
        {
            return new
                {
                    Id = UsuarioID,
                    Nome = Nome,
                    Email = Email,
                    Foto = Foto
                };
        }
    }
}
