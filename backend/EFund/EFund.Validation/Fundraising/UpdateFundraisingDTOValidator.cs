using EFund.Common.Models.DTO.Fundraising;
using FluentValidation;

namespace EFund.Validation.Fundraising;

public class UpdateFundraisingDTOValidator : AbstractValidator<UpdateFundraisingDTO>
{
    public UpdateFundraisingDTOValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty();

        RuleFor(x => x.Tags)
            .NotEmpty();

        RuleForEach(x => x.Tags)
            .NotEmpty();
    }
}