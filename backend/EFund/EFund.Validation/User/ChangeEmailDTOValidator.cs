using EFund.Common.Models.DTO.User;
using FluentValidation;

namespace EFund.Validation.User;

public class ChangeEmailDTOValidator : AbstractValidator<ChangeEmailDTO>
{
    public ChangeEmailDTOValidator()
    {
        RuleFor(x => x.NewEmail)
            .NotEmpty()
            .EmailAddress();
    }
}