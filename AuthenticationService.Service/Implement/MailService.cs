using AuthenticationService.Model.Content;
using AuthenticationService.Settings;
using System.Net;
using System.Net.Mail;

namespace AuthenticationService.Service.Implement;

public class MailService : IMailService
{
    private readonly MailSettings mailSettings;


    // mailSetting được Inject qua dịch vụ hệ thống
    // Có inject Logger để xuất log
    public MailService(AppSetting settings)
    {
        mailSettings = settings.MailSettings;
    }

    // Gửi email, theo nội dung trong mailContent
    public async Task SendMail(MailContent mailContent)
    {
        using (SmtpClient client = new SmtpClient(mailSettings.Host, mailSettings.Port))
        {
            try
            {
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(mailSettings.Mail, mailSettings.Password);
                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add(mailContent.To);
                mailMessage.From = new MailAddress(mailSettings.Mail);
                mailMessage.Subject = mailContent.Subject;
                mailMessage.Body = mailContent.Body;
                await client.SendMailAsync(mailMessage);
                client.Dispose();
            }
            catch (Exception ex)
            {
                // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                System.IO.Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                client.Dispose();
            }
        }
    }
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        await SendMail(new MailContent()
        {
            To = email,
            Subject = subject,
            Body = htmlMessage
        });
    }
}
