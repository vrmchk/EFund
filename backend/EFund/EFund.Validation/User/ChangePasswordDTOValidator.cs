using EFund.Common.Models.DTO.User;
using EFund.Validation.Extensions;
using FluentValidation;

namespace EFund.Validation.User;

public class ChangePasswordDTOValidator : AbstractValidator<ChangePasswordDTO>
{
    public ChangePasswordDTOValidator()
    {
        RuleFor(dto => dto.OldPassword)
            .NotEmpty();

        RuleFor(dto => dto.NewPassword)
            .NotEmpty()
            .Password();
    }
}