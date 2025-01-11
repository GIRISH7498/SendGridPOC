using SendGrid.Helpers.Mail;
using SendGridPOC.ConstantsAndEnums;

namespace SendGridPOC.Email
{
    public class EmailMessage
    {
        public EmailAddress To { get; set; }
        public string Subject { get; set; }
        public IDictionary<string, string> TemplateData { get; set; }
        public EmailTemplateType EmailTemplateType { get; set; }

        public EmailMessage(string to, string subject, IDictionary<string, string> templateData, EmailTemplateType emailTemplateType)
        {
            To = new EmailAddress(to);
            Subject = subject;
            TemplateData = templateData;
            EmailTemplateType = emailTemplateType;
        }
    }
}
