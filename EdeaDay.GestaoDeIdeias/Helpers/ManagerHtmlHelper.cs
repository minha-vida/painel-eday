using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using EdeaDay.GestaoDeIdeias.Model;
using EdeaDay.GestaoDeIdeias.Model.Enums;
using EdeaDay.GestaoDeIdeias.Model.Infrastructure;
using EdeaDay.GestaoDeIdeias.Model.Utils;
using EdeaDay.GestaoDeIdeias.Repository;

namespace EdeaDay.GestaoDeIdeias.Site.Helpers
{
    public static class ManagerHtmlHelper
    {
        public static MvcHtmlString RenderSelectListForCargo(this HtmlHelper htmlHelper, Ideia ideia)
        {
            var htmlSelectList = new StringBuilder();

            htmlSelectList.Append("<select id='Cargo' name='Cargo'>");

            var cargos = ObterCargosPossiveisDaIdeia(ideia);

            foreach (var item in cargos)
            {
                htmlSelectList.AppendFormat("<option value='{0}'>{1}</option>", item.Key, item.Value);
            }

            htmlSelectList.Append("</select>");

            return new MvcHtmlString(htmlSelectList.ToString());
        }

        #region private methods
        private static IEnumerable<KeyValuePair<int, string>> ObterCargosPossiveisDaIdeia(Ideia ideia)
        {
            var enumParaConversao = typeof(Cargo);

            var fields = enumParaConversao.GetFields();

            var descriptionFields = new Dictionary<int, string>();

            foreach (var field in fields)
            {
                Cargo cargo;

                if (!field.FieldType.IsEnum)
                    continue;

                if (!Enum.TryParse(field.Name, true, out cargo))
                    continue;

                if (CargoDeveSerIgnorado(cargo, ideia))
                    continue;

                var enumObject = Enum.Parse(enumParaConversao, field.Name);

                var description = ((Enum)enumObject).GetDescription();

                var enumValue = (int)field.GetValue(enumParaConversao);


                descriptionFields[enumValue] = description;
            }

            return descriptionFields;
        }

        private static bool CargoDeveSerIgnorado(Cargo cargo, Ideia ideia)
        {
            var field = cargo.GetType().GetField(cargo.ToString());

            var attributes = (IgnorarCargoAttribute[])field.GetCustomAttributes(typeof(IgnorarCargoAttribute), false);

            var cargoDeveSerIgnorado = attributes.Any();

            if (cargoDeveSerIgnorado && attributes.First().PermitirApenasUmPorIdeia)
            {
                return IdeiaPossuiVagaOuEnvolvidoComEsteCargo(cargo, ideia);
            }

            return cargoDeveSerIgnorado;
        }

        private static bool IdeiaPossuiVagaOuEnvolvidoComEsteCargo(Cargo cargo, Ideia ideia)
        {
            var jaExisteUmEnvolvidoComEsteCargo = ideia.Envolvidos.Any(m => m.Ideia.IdeiaID == ideia.IdeiaID && m.Cargo == cargo);

            var jaExisteUmaVagaComEsteCargo = ideia.Vagas.Any(m => m.Ideia.IdeiaID == ideia.IdeiaID && m.Cargo == cargo);

            return jaExisteUmEnvolvidoComEsteCargo || jaExisteUmaVagaComEsteCargo;
        }
        #endregion
    }
}