using EFS_23298_23327.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace EFS_23298_23327.Data
{
    public class EnviaEmail : IEmailSender
    {
        private string host;
        private int port;
        private bool enableSSL;
        private string username;
        private string password;
        public EnviaEmail(string host, int port, bool enableSSL, string userName, string password) {
            this.host = host;
            this.port = port;
            this.enableSSL = enableSSL;
            this.username = userName;
            this.password = password;


        }



        public Task SendEmailAsync(string email, string subject, string htmlMessage) {
            SmtpClient client = new SmtpClient {
                Port = port,
               
                Host = host, 
                EnableSsl = enableSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(username,password)
            };
            return client.SendMailAsync(username, email, subject, htmlMessage);

        }
    }
}
