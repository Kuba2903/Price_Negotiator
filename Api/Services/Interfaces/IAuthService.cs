using Api.DTO_s;

namespace Api.Services.Interfaces
{
    public interface IAuthService
    {
        AuthResponseDto Login(LoginDto loginDto);
        AuthResponseDto Register(RegisterEmployeeDto registerDto);
    }
}
