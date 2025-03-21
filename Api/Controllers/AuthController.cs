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


        /// <summary>
        /// Authenticates an employee and returns JWT token
        /// </summary>
        /// <param name="loginDto">Login credentials</param>
        /// <returns>Authentication response with JWT token</returns>
        /// <response code="200">Returns the authentication response with token</response>
        /// <response code="400">If the credentials are invalid</response>
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


        /// <summary>
        /// Registers a new employee (requires Employee role)
        /// </summary>
        /// <param name="registerDto">Registration data</param>
        /// <returns>Authentication response with JWT token</returns>
        /// <response code="200">Returns the authentication response with token</response>
        /// <response code="400">If the registration data is invalid</response>
        /// <response code="401">If user is not authenticated</response>
        /// <response code="403">If user is not authorized as Employee</response>
        [HttpPost("register")]
        [Authorize(Roles = "Employee")]
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
