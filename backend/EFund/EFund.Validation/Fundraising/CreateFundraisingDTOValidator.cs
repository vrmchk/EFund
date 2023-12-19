using EFund.Common.Models.DTO.Fundraising;
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
            .NotNull();

        RuleForEach(x => x.Tags)
            .NotNull();
    }
}