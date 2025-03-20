using Api.DTO_s;
using Api.Services.Interfaces;
using Data.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly Dictionary<string, Employee> _employees = new();
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
            var defaultEmployee = new Employee
            {
                Id = 1,
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Email = "admin@shop.com"
            };
            _employees[defaultEmployee.Username] = defaultEmployee;
        }

        public AuthResponseDto Login(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public AuthResponseDto Register(RegisterEmployeeDto registerDto)
        {
            throw new NotImplementedException();
        }
    }
}
