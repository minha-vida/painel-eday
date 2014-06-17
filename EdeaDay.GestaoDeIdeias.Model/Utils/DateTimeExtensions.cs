using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdeaDay.GestaoDeIdeias.Model.Utils
{
    public static class DateTimeExtensions
    {
        public static string ToTimeElapsedNotation(this DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;

            var message = new StringBuilder();

            if (timeSpan.Days >= 1)
            {
                message.AppendFormat("{0} dia(s) atrás", timeSpan.Days);
            }
            else
            {
                if (timeSpan.Hours == 0 && timeSpan.Minutes == 0)
                {
                    message.Append("menos de 1 minuto atrás");
                }
                else
                {
                    if (timeSpan.Hours > 0)
                        message.AppendFormat("{0} hora(s) atrás", timeSpan.Hours);
                    else
                        message.AppendFormat("{0} minuto(s) atrás", timeSpan.Minutes);
                }
            }

            return message.ToString();
        }
    }
}
