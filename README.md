Painel eDay
===========

Software livre com o objetivo de organizar e divulgar projetos e idéias de maneira simples, fornecendo suporte para empreendedores e para quem deseja colaborar.

##Considerações:
Este software foi criado com o intuito de ajudar o pessoal do [Minha Vida](http://www.minhavida.com.br) a organizar suas ideias e criar times que fizessem a inovação acontecer!

Ele é um pouco antigo e o código não é o último estado da arte. O desenvolvi em parceria com o [vinicaval](https://github.com/vinicaval) quando ainda éramos novatos em MVC. Foi um jeito de aprendermos e contribuirmos para a inovação do grupo como um todo. :)

Qualquer tipo de modificação realizada no software deve ser aberta para todos, já que estamos sob a licença do GPL v2.

##Features:
* Criação de ideias apenas com um titulo, uma descrição (do tamanho de um tweet) e um logo
* Espaço para comentários nas ideias
* Abertura de vagas nas ideias para que outras pessoas se juntem a você
* Controlador de inscrições com aprovação de apenas um cadidato por vaga
* Envio de e-mails na criação de uma nova ideia, nova inscrição na vaga e aprovação na vaga
* Perfis dos usuários com todos os projetos que eles já participaram

##Como fazer?
Comece criando um app no Google no Cloud Console (https://console.developers.google.com/).
Feito isso, obtenha seu client secret e ID na aba "Credentials", dentro de "APIs & auth".
Coloque essas informações em suas respectivas chaves no web.config.

```xml
<!--Configure your google app settings here-->
<add key="GoogleClientId" value="LOCAL_GOOGLE_CLIENT_ID" />
<add key="GoogleClientSecret" value="LOCAL_GOOGLE_CLIENT_SECRECT" />
```

Para enviar notificação para os outros usuários, configure um email do google no web.config, na parte de mailsettings.

```xml
<mailSettings>
  <smtp from="ideas@company.com" deliveryMethod="Network">
    <network host="smtp.gmail.com" enableSsl="true" port="587" userName="ideas@company.com" password="password" />
  </smtp>
</mailSettings>
```

Para limitar o acesso para apenas usuários da sua empresa, mude o método ValidateUser da classe Usuario, dentro do projeto EdeaDay.GestaoDeIdeias.Model.

```csharp
private bool ValidateUser()
{
    var validado = System.Text.RegularExpressions.Regex.Match(Email, "([\\w-\\.]+)@minhavida.com.br");

    return validado.Success;
}
```

