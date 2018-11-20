using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FRA.Service.Abstract;
using FRA.Service.Models;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace FRA.Web.Infrastructure.Services
{
    public class MessageServices : IEmailSender
    {
        private readonly IEmailService _emailService;

        public MessageServices(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return _emailService.SendMailAsync(new List<EmailRecipient>
            {
                new EmailRecipient { Name = string.Empty, EmailAddress = email }
            }, subject, message);
        }
    }
}
