using EFund.Common.Models.DTO.Common;
using FluentValidation;

namespace EFund.Validation.Common;

public class PaginationDTOValidator : AbstractValidator<PaginationDTO>
{
    public PaginationDTOValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1);
    }
}