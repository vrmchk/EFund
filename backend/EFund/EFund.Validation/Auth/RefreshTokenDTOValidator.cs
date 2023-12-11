using EFund.Common.Models.DTO.Auth;
using FluentValidation;

namespace EFund.Validation.Auth;

public class RefreshTokenDTOValidator : AbstractValidator<RefreshTokenDTO>
{
    public RefreshTokenDTOValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty();

        RuleFor(x => x.RefreshToken)
            .NotEmpty();
    }
}