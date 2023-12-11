using EFund.Common.Models.DTO.Auth;
using FluentValidation;

namespace EFund.Validation.Auth;

public class ResendConfirmationCodeDTOValidator : AbstractValidator<ResendConfirmationCodeDTO>
{
    public ResendConfirmationCodeDTOValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}