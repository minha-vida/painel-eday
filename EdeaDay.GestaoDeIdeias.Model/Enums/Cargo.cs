using System.ComponentModel;
using EdeaDay.GestaoDeIdeias.Model.Infrastructure;

namespace EdeaDay.GestaoDeIdeias.Model.Enums
{
    public enum Cargo
    {
        [IgnorarCargo]
        Idealizador = 1,

        [IgnorarCargo(PermitirApenasUmPorIdeia = true)]
        Padrinho,

        Desenvolvedor,

        Designer,

        Analista,

        [Description("Web Designer")]
        WebDesigner,

        [Description("Product Owner")]
        ProductOwner
    }
}
