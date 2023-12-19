using EFund.Common.Models.DTO.User;
using FluentValidation;

namespace EFund.Validation.User;

public class UserActionDTOValidator : AbstractValidator<UserActionDTO>
{
    public UserActionDTOValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}