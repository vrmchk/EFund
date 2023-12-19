using EFund.Common.Models.DTO.User;
using FluentValidation;

namespace EFund.Validation.User;

public class MakeAdminValidator : AbstractValidator<MakeAdminDTO>
{
    public MakeAdminValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}