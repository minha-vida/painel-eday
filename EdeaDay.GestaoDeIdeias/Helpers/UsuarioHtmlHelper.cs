using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using EdeaDay.GestaoDeIdeias.Model;
using EdeaDay.GestaoDeIdeias.Model.Utils;

namespace EdeaDay.GestaoDeIdeias.Site.Helpers
{
    public static class UsuarioHtmlHelper
    {
        public static string ObterListaDeCargosDoUsuarioSeparadoPorVirgula(this HtmlHelper htmlHelper, Usuario usuario)
        {
            const string commaSeparator = ", ";

            var cargos = usuario.UserOccupiedPositions().Select(cargo => cargo.GetDescription());

            var cargosSeparadosPorVirgula = string.Join(commaSeparator, cargos);

            var indiceDaUltimaVirgula = cargosSeparadosPorVirgula.LastIndexOf(commaSeparator, StringComparison.Ordinal);

            if (indiceDaUltimaVirgula.Equals(-1))
                return cargosSeparadosPorVirgula;

            var ultimaParteDaSeparacao = cargosSeparadosPorVirgula.Substring(indiceDaUltimaVirgula);

            cargosSeparadosPorVirgula = cargosSeparadosPorVirgula.Substring(0, indiceDaUltimaVirgula);

            var ultimaParteDaSeparacaoComE = ultimaParteDaSeparacao.Replace(commaSeparator, " e ");

            cargosSeparadosPorVirgula = string.Concat(cargosSeparadosPorVirgula, ultimaParteDaSeparacaoComE);

            return cargosSeparadosPorVirgula;
        }
    }
}