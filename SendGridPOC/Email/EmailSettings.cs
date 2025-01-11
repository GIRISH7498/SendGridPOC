namespace SendGridPOC.Email
{
    public class EmailSettings
    {
        public const string EmailConfiguration = "EmailConfiguration";
        public string ApiKey { get; set; }
        public string From { get; set; }
        public string Username { get; set; }
    }
}
