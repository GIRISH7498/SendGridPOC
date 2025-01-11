namespace SendGridPOC.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string OTP { get; set; }
        public DateTime OtpExpiry { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
