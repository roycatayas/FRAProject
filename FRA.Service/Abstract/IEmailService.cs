using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FRA.Service.Models;

namespace FRA.Service.Abstract
{
    public interface IEmailService
    {
        Task SendMailAsync(IEnumerable<EmailRecipient> recipients, string subject, string body);
    }
}
