using EFund.Common.Models.DTO.User;
using FluentValidation;

namespace EFund.Validation.User;

public class UpdateUserDTOValidator : AbstractValidator<UpdateUserDTO>
{
    public UpdateUserDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}