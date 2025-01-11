using MediatR;
using SendGridPOC.ConstantsAndEnums;
using SendGridPOC.Data;
using SendGridPOC.Email;
using SendGridPOC.Models;
using System.Net.Mail;

namespace SendGridPOC.Commands
{
    public class RegisterUserCommand : IRequest<bool>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, bool>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ISendGridEmailService _emailService;

        public RegisterUserHandler(ApplicationDbContext dbContext, ISendGridEmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }

        public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            var otpExpiry = DateTime.UtcNow.AddMinutes(5);

            var user = new User
            {
                Email = request.Email,
                Password = request.Password,
                OTP = otp,
                OtpExpiry = otpExpiry,
                CreatedOn = DateTime.UtcNow
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Send OTP Email
            var templateData = new Dictionary<string, string>
            {
                { "OTP", otp }
            };

            var emailMessage = new EmailMessage(
                user.Email,
                "Your OTP for Registration",
                templateData,
                EmailTemplateType.OtpEmail
            );

            await _emailService.Send(emailMessage);

            return true;
        }
    }
}
