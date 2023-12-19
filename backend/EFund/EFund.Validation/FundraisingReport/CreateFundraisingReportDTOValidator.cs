using EFund.Common.Models.DTO.FundraisingReport;
using FluentValidation;

namespace EFund.Validation.FundraisingReport;

public class CreateFundraisingReportDTOValidator : AbstractValidator<CreateFundraisingReportDTO>
{
    public CreateFundraisingReportDTOValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty();

        RuleFor(x => x.FundraisingId)
            .NotEmpty();
    }
}