using Api.DTO_s;
using Api.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;
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
        private readonly IValidator<LoginDto> login_validator;
        private readonly IValidator<RegisterEmployeeDto> register_validator;
        public AuthController(IAuthService authService, IValidator<LoginDto> login_validator, IValidator<RegisterEmployeeDto> register_validator)
        {
            _authService = authService;
            this.login_validator = login_validator;
            this.register_validator = register_validator;
        }

        [HttpPost("login")]
        public ActionResult<AuthResponseDto> Login([FromBody] LoginDto loginDto)
        {
            ValidationResult result = login_validator.Validate(loginDto);
            if (result.IsValid)
            {
                var response = _authService.Login(loginDto);
                return Ok(response);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("register")]
        [Authorize(Roles = "Employee")] // only existing worker can register new one
        public ActionResult<AuthResponseDto> Register([FromBody] RegisterEmployeeDto registerDto)
        {
            ValidationResult result = register_validator.Validate(registerDto);
            if (result.IsValid)
            {
                var response = _authService.Register(registerDto);
                return Ok(response);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
    }
}
