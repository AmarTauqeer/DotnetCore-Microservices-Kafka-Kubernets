using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementService.MailService.Models;

namespace UserManagementService.MailService.Services
{
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}
