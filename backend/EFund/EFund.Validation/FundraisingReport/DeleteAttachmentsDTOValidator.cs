using EFund.Common.Models.DTO.FundraisingReport;
using FluentValidation;

namespace EFund.Validation.FundraisingReport;

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