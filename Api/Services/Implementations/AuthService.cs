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
            if (!_employees.TryGetValue(loginDto.Username, out var employee))
            {
                throw new Exception("Invalid username or password");
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, employee.PasswordHash))
            {
                throw new Exception("Invalid username or password");
            }

            var token = GenerateJwtToken(employee);

            return new AuthResponseDto
            {
                Token = token,
                Username = employee.Username
            };
        }

        public AuthResponseDto Register(RegisterEmployeeDto registerDto)
        {
            if (_employees.ContainsKey(registerDto.Username))
            {
                throw new Exception("Username already exists");
            }

            var employee = new Employee
            {
                Id = _employees.Count + 1,
                Username = registerDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Email = registerDto.Email
            };

            _employees[employee.Username] = employee;

            var token = GenerateJwtToken(employee);

            return new AuthResponseDto
            {
                Token = token,
                Username = employee.Username
            };
        }

        private string GenerateJwtToken(Employee employee)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, employee.Username),
            new Claim(ClaimTypes.Email, employee.Email),
            new Claim(ClaimTypes.Role, "Employee"),
            new Claim("UserId", employee.Id.ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
