using Api.DTO_s;
using FluentValidation;

namespace Api.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDTO>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.BasePrice).GreaterThan(0);
            RuleFor(x => x.Description).MaximumLength(1000);
        }
    }
}
