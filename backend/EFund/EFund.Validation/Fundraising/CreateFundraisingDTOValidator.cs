using EFund.Common.Models.DTO.Fundraising;
using EFund.Validation.Extensions;
using FluentValidation;

namespace EFund.Validation.Fundraising;

public class CreateFundraisingDTOValidator : AbstractValidator<CreateFundraisingDTO>
{
    public CreateFundraisingDTOValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty();

        RuleFor(x => x.MonobankJarId)
            .NotEmpty();

        RuleFor(x => x.Tags)
            .NotNull()
            .CountLessThanOrEqualTo(5)
            .WithMessage("Fundraising can have up to 5 tags");

        RuleForEach(x => x.Tags)
            .NotNull();
    }
}