using Api.DTO_s;
using FluentValidation;

namespace Api.Validators
{
    public class RegisterEmployeeDtoValidator : AbstractValidator<RegisterEmployeeDto>
    {
        public RegisterEmployeeDtoValidator()
        {
            RuleFor(x => x.Username).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(50);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
