using EFund.Common.Enums;
using EFund.Common.Models.DTO.User;
using EFund.Validation.Extensions;
using FluentValidation;

namespace EFund.Validation.User;

public class UserActionDTOValidator : AbstractValidator<UserActionDTO>
{
    public UserActionDTOValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Action)
            .IsEnum(typeof(UserAction));
    }
}