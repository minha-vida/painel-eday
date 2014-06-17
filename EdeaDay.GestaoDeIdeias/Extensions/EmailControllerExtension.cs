using System.Configuration;
using System.Net.Mail;
using System.Text;
using EdeaDay.GestaoDeIdeias.Model;
using EdeaDay.GestaoDeIdeias.Model.Utils;
using EdeaDay.GestaoDeIdeias.Site.Controllers;

namespace EdeaDay.GestaoDeIdeias.Site.Extensions
{
    public static class EmailControllerExtension
    {
        #region Ideia

        public static void EnviarEmailDeNotificacaoDeIdeiaCriada(this IdeiaController controller, Ideia ideia)
        {
            const string chaveDaListaDeEmailsDeNotificacao = "EmailsParaNotificarNovasIdeias";

            var mensagem = new StringBuilder();

            var urlProvider = new UrlProvider(controller.Request);

            mensagem.Append("Prezado(a), ");
            mensagem.Append("<br />");
            mensagem.Append("<br />");
            mensagem.AppendFormat("A ideia \"{0}\" acabou de ser criada!", ideia.Titulo);
            mensagem.Append("<br />");
            mensagem.Append("<br />");
            mensagem.AppendFormat("Para maiores informações acesse: {0}", urlProvider.ObterUrlParaDetalhesDaIdeia(ideia.IdeiaID));
            mensagem.Append("<br />");

            var assunto = string.Format("A ideia \"{0}\" foi criada!", ideia.Titulo);

            var listaDeNotificacao = ConfigurationManager.AppSettings[chaveDaListaDeEmailsDeNotificacao].Split(';');

            foreach (var email in listaDeNotificacao)
            {
                EnviarEmail(email, assunto, mensagem.ToString());
            }
        }

        #endregion

        #region Vaga

        public static void EnviarEmailDeInscricaoAprovada(this VagaController controller, Usuario usuario, Vaga vaga)
        {
            var mensagem = new StringBuilder();

            mensagem.AppendFormat("Caro(a) {0},", usuario.Nome);
            mensagem.Append("<br />");
            mensagem.AppendFormat("Sua inscrição para o cargo de {0} na ideia \"{1}\" foi aprovada!", vaga.Cargo.GetDescription(), vaga.Ideia.Titulo);
            mensagem.Append("<br />");
            mensagem.Append("Seguem os dados da inscrição aprovada: ");
            mensagem.Append("<br />");
            mensagem.Append("<ul>");
            mensagem.Append("<li>");
            mensagem.AppendFormat("Ideia: {0}", vaga.Ideia.Titulo);
            mensagem.Append("</li>");
            mensagem.Append("<li>");
            mensagem.AppendFormat("Cargo: {0}", vaga.Cargo.GetDescription());
            mensagem.Append("</li>");
            mensagem.Append("</ul>");
            mensagem.Append("<br />");

            var assunto = string.Format("Inscrição aprovada para o cargo de {0}!", vaga.Cargo.GetDescription());

            EnviarEmail(usuario.Email, assunto, mensagem.ToString());
        }

        public static void EnviarEmailDeAprovacaoDeInscricao(this VagaController controller, Usuario idealizador, Usuario usuario, Vaga vaga)
        {
            var mensagem = new StringBuilder();

            var urlProvider = new UrlProvider(controller.Request);

            mensagem.AppendFormat("Caro(a) {0},", idealizador.Nome);
            mensagem.Append("<br />");
            mensagem.AppendFormat("Seguem as informações de uma inscrição para a vaga de {0}: ", vaga.Cargo.GetDescription());
            mensagem.Append("<br />");
            mensagem.Append("<ul>");
            mensagem.Append("<li>");
            mensagem.AppendFormat("Ideia: {0}", vaga.Ideia.Titulo);
            mensagem.Append("</li>");
            mensagem.Append("<li>");
            mensagem.AppendFormat("Cargo: {0}", vaga.Cargo.GetDescription());
            mensagem.Append("</li>");
            mensagem.Append("<li>");
            mensagem.AppendFormat("Usuário: {0}", usuario.Nome);
            mensagem.Append("</li>");
            mensagem.Append("</ul>");
            mensagem.Append("Confira as inscrições desta vaga através do Manager da sua ideia no Painel eDay!");
            mensagem.Append("<br />");
            mensagem.Append("Dentro da área de \"Vagas\", clique no botão \"Inscrições\" ao lado da vaga desejada e aprove o candidato de sua preferência.");
            mensagem.Append("<br />");
            mensagem.Append(urlProvider.ObterUrlParaManagerDaIdeia(vaga.Ideia.IdeiaID));
            mensagem.Append("<br />");
            mensagem.Append("<br />");

            var assunto = string.Format("Inscrição para a vaga de {0}!", vaga.Cargo.GetDescription());

            EnviarEmail(idealizador.Email, assunto, mensagem.ToString());
        }

        public static void EnviarEmailDeSucessoDaInscricao(this VagaController controller, Usuario usuario, Vaga vaga)
        {
            var mensagem = new StringBuilder();

            mensagem.AppendFormat("Caro(a) {0},", usuario.Nome);
            mensagem.Append("<br />");
            mensagem.Append("Seguem as informações de sua inscrição recente: ");
            mensagem.Append("<br />");
            mensagem.Append("<ul>");
            mensagem.Append("<li>");
            mensagem.AppendFormat("Ideia: {0}", vaga.Ideia.Titulo);
            mensagem.Append("</li>");
            mensagem.Append("<li>");
            mensagem.AppendFormat("Cargo: {0}", vaga.Cargo.GetDescription());
            mensagem.Append("</li>");
            mensagem.Append("</ul>");
            mensagem.Append("<br />");
            mensagem.Append("Você será avisado(a) caso sua inscrição seja aprovada.");
            mensagem.Append("<br />");

            EnviarEmail(usuario.Email, "Sua inscrição foi registrada com sucesso!", mensagem.ToString());
        }

        #endregion

        #region Comentário
        public static void EnviarEmailDeNotificacaoDoComentarioNaIdeia(this ComentarioController controller, Ideia ideia, Comentario comentario)
        {
            var mensagem = new StringBuilder();

            var urlProvider = new UrlProvider(controller.Request);

            mensagem.AppendFormat("Caro(a) {0},", ideia.Idealizador.Nome);
            mensagem.Append("<br />");
            mensagem.Append("<br />");
            mensagem.AppendFormat("{0} deixou um comentário na ideia \"{1}\":", comentario.Usuario.Nome, ideia.Titulo);
            mensagem.Append("<br />");
            mensagem.AppendFormat("\"{0}\"", comentario.Texto);
            mensagem.Append("<br />");
            mensagem.Append("<br />");
            mensagem.Append("Confira todos os comentários da ideia em sua página de detalhes:");
            mensagem.Append("<br />");
            mensagem.AppendFormat("{0}", urlProvider.ObterUrlParaDetalhesDaIdeia(ideia.IdeiaID));
            mensagem.Append("<br />");
            mensagem.Append("<br />");

            var assunto = string.Format("Novo comentário na ideia \"{0}\"!", ideia.Titulo);

            EnviarEmail(ideia.Idealizador.Email, assunto, mensagem.ToString());
        }
        #endregion

        private static void EnviarEmail(string to, string subject, string message)
        {
            const string from = "paineleday@minhavida.com.br";

            var mail = new MailMessage(new MailAddress(from, "Painel eDay"), new MailAddress(to))
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            //NOTE: The configurations are listed inside the System.Net node in the web.config file
            var client = new SmtpClient();

            client.SendAsync(mail, null);
        }
    }
}