using MediatR;
using Microsoft.AspNetCore.Mvc;
using SendGridPOC.Commands;

namespace SendGridPOC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);
            if (result)
                return Ok(new { Message = "User registered and OTP sent successfully." });

            return BadRequest(new { Message = "Registration failed." });
        }
    }

}
