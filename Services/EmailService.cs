using MailKit.Net.Smtp;
using MimeKit;
using service;
using System.Threading.Tasks;

public class EmailService:IEmailService
{
    private readonly string _smtpServer = "smtp.gmail.com";  
    private readonly string _smtpUsername = "nwadekar1202@gmail.com";      
    private readonly string _smtpPassword = "ovlq ymro wpis zumi";          
    private readonly int _smtpPort = 587; 

    public async Task SendEmailAsync(string toEmail, string subject, string messageBody)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("LatestUpdate", _smtpUsername));
        message.To.Add(new MailboxAddress("Farmer",toEmail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = messageBody 
        };

        message.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())
        {
           try
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, false); // Use false for non-SSL connection if you’re using SMTP Port 587
                await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
              
                throw new Exception($"Email sending failed: {ex.Message}");
            }
        }
    }
}
