using EFund.Common.Models.DTO.FundraisingReport;
using FluentValidation;

namespace EFund.Validation.FundraisingReport;

public class UpdateFundraisingReportDTOValidator : AbstractValidator<UpdateFundraisingReportDTO>
{
    public UpdateFundraisingReportDTOValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty();
    }
}