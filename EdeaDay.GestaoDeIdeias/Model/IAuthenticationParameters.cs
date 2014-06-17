using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdeaDay.GestaoDeIdeias.Helpers
{
    public interface IAuthenticationParameters
    {
        string ClientSecret { get; }
        string ClientId { get; }
        string RedirectUrl { get; }
    }
}
