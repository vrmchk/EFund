using EFund.Common.Enums;
using EFund.Common.Models.DTO.Fundraising;
using FluentValidation;

namespace EFund.Validation.Fundraising;

public class UpdateFundraisingStatusDTOValidator : AbstractValidator<UpdateFundraisingStatusDTO>
{
    public UpdateFundraisingStatusDTOValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .Must(x => x
                is FundraisingStatus.Open
                or FundraisingStatus.Closed
                or FundraisingStatus.ReadyForReview
                or FundraisingStatus.Hidden
                or FundraisingStatus.Deleted)
            .WithMessage("User is unable to set this status");
    }
}