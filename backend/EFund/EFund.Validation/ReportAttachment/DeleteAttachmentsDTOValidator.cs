using EFund.Common.Models.DTO.ReportAttachment;
using FluentValidation;

namespace EFund.Validation.ReportAttachment;

public class DeleteAttachmentsDTOValidator : AbstractValidator<DeleteAttachmentsDTO>
{
    public DeleteAttachmentsDTOValidator()
    {
        RuleFor(x => x.AttachmentIds)
            .NotNull();

        RuleForEach(x => x.AttachmentIds)
            .NotEmpty();
    }
}