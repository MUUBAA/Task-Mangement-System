using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Data.Dto;
using TaskManagement.Service.AuthService;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IJwtTokenService jwtTokenService) : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService;

        // POST: api/auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<LoginResponseDto> Login([FromBody] LoginRequestDto dto)
        {
            var user = StaticUsers.Validate(dto.UserName, dto.Password);
            if (user is null)
                return Unauthorized("Invalid username or password.");

            var token = _jwtTokenService.CreateToken(user);

            return Ok(new LoginResponseDto
            {
                AccessToken = token,
                UserId = user.UserId,
                UserName = user.UserName,
                Role = user.Role
            });
        }

        [HttpGet("user")]
        [Authorize]
        public ActionResult<object> Me()
        {
            return Ok(new
            {
                UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
                UserName = User.Identity?.Name,
                Role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
            });
        }
    }
}
