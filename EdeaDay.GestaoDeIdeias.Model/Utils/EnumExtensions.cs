using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EdeaDay.GestaoDeIdeias.Model.Utils
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            return GetAttributeDescription(value);
        }

        public static IDictionary<int, string> ToDictionary(this Enum value)
        {
            var fields = value.GetType().GetFields();

            var descriptionFields = new Dictionary<int, string>();

            foreach (var field in fields)
            {
                if (!field.FieldType.IsEnum)
                    continue;

                var enumObject = Enum.Parse(value.GetType(), field.Name);

                var description = GetAttributeDescription((Enum)enumObject);
                
                var enumValue = (int)field.GetValue(value);

                descriptionFields[enumValue] = description;
            }

            return descriptionFields;
        }

        private static string GetAttributeDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length < 1)
                return value.ToString();

            return attributes[0].Description;
        }

    }
}
