using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using EdeaDay.GestaoDeIdeias.Model;
using EdeaDay.GestaoDeIdeias.Model.Infrastructure;
using EdeaDay.GestaoDeIdeias.Repository;
using EdeaDay.GestaoDeIdeias.Site.Extensions;
using EdeaDay.GestaoDeIdeias.Site.Model;
using Newtonsoft.Json;
using System.Web;

namespace EdeaDay.GestaoDeIdeias.Site.Controllers
{
    public class GoogleServiceController : Controller
    {
        private readonly Eday _context = new Eday();

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly GoogleParameters _googleParameters;

        public GoogleServiceController()
        {
            _usuarioRepository = new UsuarioRepository(_context);
            _googleParameters = new GoogleParameters();
        }

        public ActionResult GetGoogleUser(string state, string code, string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception("Ocorreu um erro com a sua solicitação. Tente novamente mais tarde.");
            }

            var stateResponse = HttpUtility.ParseQueryString(state);

            var accessToken = HandleAccessTokenRequest(code);

            var user = LoadUserProfileInformation(accessToken);

            this.UserSession(_usuarioRepository).LoginUsuario(user, HttpContext);

            if (string.IsNullOrEmpty(stateResponse["url"]))
                return RedirectToAction("Index", "Ideia");

            return Redirect(stateResponse["url"]);
        }

        private static Usuario LoadUserProfileInformation(string accessToken)
        {
            const string url = "https://www.googleapis.com/oauth2/v1/userinfo?access_token={0}";

            var tokenUrl = string.Format(url, accessToken);

            var response = GetRequest(tokenUrl);

            var reader = new StreamReader(response.GetResponseStream());

            var json = reader.ReadToEnd();
            var profile = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            var user = new Usuario
                           {
                               UsuarioID = profile.ContainsKey("id") ? profile["id"] : null,
                               Nome = profile.ContainsKey("name") ? profile["name"] : null,
                               Email = profile.ContainsKey("email") ? profile["email"] : null,
                               Foto = profile.ContainsKey("picture") ? profile["picture"] : null,
                               AccessToken = accessToken
                           };

            return user;
        }

        private string HandleAccessTokenRequest(string code)
        {
            const string url = "https://accounts.google.com/o/oauth2/token";

            var body = new StringBuilder();
            body.AppendFormat("code={0}&", code);
            body.AppendFormat("client_id={0}&", _googleParameters.ClientId);
            body.AppendFormat("client_secret={0}&", _googleParameters.ClientSecret);
            body.AppendFormat("redirect_uri={0}&", _googleParameters.RedirectUrl);
            body.Append("grant_type=authorization_code");

            var response = PostRequest(url, body.ToString());

            string json;

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                json = reader.ReadToEnd();
            }

            var jsonData = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            var accessToken = jsonData["access_token"];

            return accessToken;
        }

        //TODO: Método deve ser isolado em um projeto próprio de utilitários ou similar
        private static HttpWebResponse GetRequest(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(new Uri(url));

            var response = (HttpWebResponse)request.GetResponse();

            return response;
        }

        //TODO: Método deve ser isolado em um projeto próprio de utilitários ou similar
        private static HttpWebResponse PostRequest(string url, string body)
        {
            var request = (HttpWebRequest)WebRequest.Create(new Uri(url));

            request.Method = "POST";
            request.KeepAlive = true;
            request.ContentLength = body.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(body);
                writer.Flush();
            }

            var response = (HttpWebResponse)request.GetResponse();

            return response;
        }
    }
}
