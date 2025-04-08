using System.Net;
using System.Net.Mail;
using FinaData.Core.Handlers;

namespace FinaData.Api.Handlers
{
    public class EmailHandler : IEmailHandler
    {
        private readonly IConfiguration _config;

        public EmailHandler(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendAsync(string to, string subject, string confirmationLink)
        {
            var htmlBody = $@"
                            <!DOCTYPE html>
                            <html>
                            <head>
                                <meta charset='UTF-8'/>
                                <title>Confirmação de E-mail</title>
                                <style>
                                    /* Exemplo de estilo inline */
                                    .container {{
                                        font-family: Arial, sans-serif;
                                        padding: 20px;
                                    }}
                                    .title {{
                                        color: #333333;
                                    }}
                                    .button {{
                                        display: inline-block;
                                        margin-top: 20px;
                                        padding: 10px 15px;
                                        text-decoration: none;
                                        background-color: #3FA4FF;
                                        color: #ffffff;
                                        border-radius: 4px;
                                    }}
                                </style>
                            </head>
                            <body>
                                <div class='container'>
                                    <h2 class='title'>Bem-vindo(a) ao sistema de Controle Financeiro!</h2>
                                    <p>Obrigado por se registrar. Clique no botão abaixo para confirmar seu e-mail:</p>
                                    <a class='button' href='{confirmationLink}'>Confirmar E-mail</a>
                                    <p>Se você não fez este registro, ignore este e-mail.</p>
                                    <hr />
                                    <small>Este é um e-mail automático, por favor não responda.</small>
                                </div>
                            </body>
                            </html>
                            ";

            using var client = new SmtpClient
            {
                Host = _config["Email:Smtp:Host"],
                Port = int.Parse(_config["Email:Smtp:Port"] ?? "587"),
                EnableSsl = true,
                Credentials = new NetworkCredential(_config["Email:Smtp:User"], _config["Email:Smtp:Pass"])
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_config["Email:Smtp:User"], "Equipe FinaData"),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };
            mail.To.Add(new MailAddress(to));

            await client.SendMailAsync(mail);
        }
    }
}
