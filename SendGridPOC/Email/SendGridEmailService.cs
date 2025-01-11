using DotLiquid;
using SendGrid.Helpers.Mail;
using SendGrid;
using SendGridPOC.ConstantsAndEnums;
using System.Reflection;

namespace SendGridPOC.Email
{
    public interface ISendGridEmailService
    {
        Task Send(EmailMessage emailMessage);
    }

    public class SendGridEmailService : ISendGridEmailService
    {
        private readonly EmailSettings _emailConfig;

        public SendGridEmailService(EmailSettings emailConfig)
        {
            _emailConfig = emailConfig;
        }

        private SendGridClient CreateSendGridClient()
        {
            return new SendGridClient(_emailConfig.ApiKey);
        }

        private async Task SendAsync(SendGridClient client, SendGridMessage msg)
        {
            var response = await client.SendEmailAsync(msg);
            if ((int)response.StatusCode >= 400)
            {
                throw new Exception($"Failed to send email. Status code: {response.StatusCode}");
            }
        }

        public async Task Send(EmailMessage emailMessage)
        {
            var client = CreateSendGridClient();
            var msg = CreateEmailMessage(emailMessage);
            await SendAsync(client, msg);
        }

        private SendGridMessage CreateEmailMessage(EmailMessage message)
        {
            var htmlBody = GenerateEmailBody(message.EmailTemplateType, message.TemplateData);
            var from = new EmailAddress(_emailConfig.From, _emailConfig.Username);
            return MailHelper.CreateSingleEmail(from, message.To, message.Subject, null, htmlBody);
        }

        private string GenerateEmailBody(EmailTemplateType emailTemplate, IDictionary<string, string> templateData)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"{assembly.GetName().Name}.EmailTemplates.{emailTemplate}.template.html";

            string templateContent = ReadFromStream(assembly, resourceName);

            if (!string.IsNullOrEmpty(templateContent))
            {
                var template = Template.Parse(templateContent);
                var objectTemplateData = templateData.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value);
                var emailBody = template.Render(Hash.FromDictionary(objectTemplateData));
                return emailBody;
            }

            throw new Exception("Email template not found or empty.");
        }

        private string ReadFromStream(Assembly assembly, string templatePath)
        {
            using var stream = assembly.GetManifestResourceStream(templatePath);
            if (stream == null)
                throw new Exception($"Email template {templatePath} not found.");

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
