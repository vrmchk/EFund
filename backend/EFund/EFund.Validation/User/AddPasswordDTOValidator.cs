using EFund.Common.Models.DTO.User;
using EFund.Validation.Extensions;
using FluentValidation;

namespace EFund.Validation.User;

public class AddPasswordDTOValidator : AbstractValidator<AddPasswordDTO>
{
    public AddPasswordDTOValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .Password();
    }
}