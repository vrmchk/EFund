using EFund.Common.Models.DTO.ReportAttachment;
using FluentValidation;

namespace EFund.Validation.ReportAttachment;

public class UpdateAttachmentDTOValidator : AbstractValidator<UpdateAttachmentDTO>
{
    public UpdateAttachmentDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}