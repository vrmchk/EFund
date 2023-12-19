using EFund.Common.Models.DTO.User;
using FluentValidation;

namespace EFund.Validation.User;

public class InviteAdminDTOValidator : AbstractValidator<InviteAdminDTO>
{
    public InviteAdminDTOValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}