using EFund.Common.Models.DTO.Auth;
using FluentValidation;

namespace EFund.Validation.Auth;

public class ForgotPasswordDTOValidator : AbstractValidator<ForgotPasswordDTO>
{
    public ForgotPasswordDTOValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}