using EFund.Common.Models.DTO.Fundraising;
using EFund.Validation.Extensions;
using FluentValidation;

namespace EFund.Validation.Fundraising;

public class UpdateFundraisingDTOValidator : AbstractValidator<UpdateFundraisingDTO>
{
    public UpdateFundraisingDTOValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty();

        RuleFor(x => x.Tags)
            .NotEmpty()
            .CountLessThanOrEqualTo(5)
            .WithMessage("Fundraising can have up to 5 tags");

        RuleForEach(x => x.Tags)
            .NotEmpty();
    }
}