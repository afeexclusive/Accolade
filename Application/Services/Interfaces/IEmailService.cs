using Application.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task<EmailSenderDetails> TestConnection(string email, string password);
        Task<bool> TestConnection(string email, string password, string domain, int port);
    }
}
