using EFund.Common.Models.DTO.User;
using FluentValidation;

namespace EFund.Validation.User;

public class BlockUserDTOValidator : AbstractValidator<BlockUserDTO>
{
    public BlockUserDTOValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}