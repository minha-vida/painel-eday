using System;
using System.ComponentModel.DataAnnotations;
using EdeaDay.GestaoDeIdeias.Model.Utils;

namespace EdeaDay.GestaoDeIdeias.Model
{
    public class Comentario
    {
        public virtual int ComentarioId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Escreva um texto para seu comentário")]
        public virtual string Texto { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual DateTime Data { get; set; }

        public object ToAnonymousType(string usuarioIdFromRequester)
        {
            return new
            {
                Id = ComentarioId,
                ComentarioDoUsuarioAtual = usuarioIdFromRequester == Usuario.UsuarioID,
                Texto = Texto,
                Data = Data.ToTimeElapsedNotation(),
                Usuario = Usuario.ToAnonymousType()
            };
        }
    }
}
