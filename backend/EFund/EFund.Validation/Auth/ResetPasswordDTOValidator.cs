using EFund.Common.Models.DTO.Auth;
using EFund.Validation.Extensions;
using FluentValidation;

namespace EFund.Validation.Auth;

public class ResetPasswordDTOValidator : AbstractValidator<ResetPasswordDTO>
{
    public ResetPasswordDTOValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Token)
            .NotEmpty();

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .Password();
    }
}