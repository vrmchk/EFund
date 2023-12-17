using EFund.Common.Models.DTO.Tag;
using FluentValidation;

namespace EFund.Validation.Tag;

public class CreateTagDTOValidator : AbstractValidator<CreateTagDTO>
{
    public CreateTagDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}