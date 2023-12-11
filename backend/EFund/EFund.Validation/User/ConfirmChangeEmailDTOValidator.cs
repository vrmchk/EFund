using EFund.Common.Models.DTO.User;
using FluentValidation;

namespace EFund.Validation.User;

public class ConfirmChangeEmailDTOValidator : AbstractValidator<ConfirmChangeEmailDTO>
{
    public ConfirmChangeEmailDTOValidator()
    {
        RuleFor(x => x.NewEmail)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Code)
            .NotEmpty();
    }
}