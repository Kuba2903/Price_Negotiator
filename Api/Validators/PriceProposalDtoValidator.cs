using Api.DTO_s;
using FluentValidation;

namespace Api.Validators
{
    public class PriceProposalDtoValidator : AbstractValidator<PriceProposalDTO>
    {
        public PriceProposalDtoValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0);
            RuleFor(x => x.PriceProposed).GreaterThan(0);
        }
    }
}
