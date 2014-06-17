using System;

namespace EdeaDay.GestaoDeIdeias.Model.Infrastructure
{
    public class IgnorarCargoAttribute : Attribute
    {
        public bool PermitirApenasUmPorIdeia { get; set; }
    }
}