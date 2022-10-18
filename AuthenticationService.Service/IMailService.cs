using AuthenticationService.Model.Content;

namespace AuthenticationService.Service;

public interface IMailService
{
    Task SendMail(MailContent mailContent);

    Task SendEmailAsync(string email, string subject, string htmlMessage);
}
