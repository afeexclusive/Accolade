using Application.Services.Interfaces;
using PD.EmailSender.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class EmailService : IEmailService
    {
        public async Task<EmailSenderDetails> TestConnection(string email, string password)
        {
            var result = await SendMail.AuthenticateSenderDomain(email, password);
            return new EmailSenderDetails
            {
                IsAuthenticated = result.IsAuthenticated,
                Domain = result.Settings?.Domain,
                Email = result.Settings?.Email,
                Password = result.Settings?.Password,
                Port = result.Settings == null ? 0 : result.Settings.Port
            };
        }

        public async Task<bool> TestConnection(string email, string password, string domain, int port)
        {
            var result = await SendMail.AuthenticateSenderDomain(email, password, domain, port);
            return result.IsAuthenticated;
        }
    }

    public class EmailSenderDetails
    {
        public bool IsAuthenticated { get; set; }
        public string Domain { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

}
