using Api.DTO_s;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public ActionResult<AuthResponseDto> Login([FromBody] LoginDto loginDto)
        {
            var response = _authService.Login(loginDto);
            return Ok(response);
        }

        [HttpPost("register")]
        [Authorize(Roles = "Employee")] // only existing worker can register new one
        public ActionResult<AuthResponseDto> Register([FromBody] RegisterEmployeeDto registerDto)
        {
            var response = _authService.Register(registerDto);
            return Ok(response);
        }
    }
}
