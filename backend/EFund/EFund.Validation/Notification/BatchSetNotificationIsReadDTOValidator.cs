using EFund.Common.Models.DTO.Notification;
using FluentValidation;

namespace EFund.Validation.Notification;

public class BatchSetNotificationIsReadDTOValidator : AbstractValidator<BatchSetNotificationIsReadDTO>
{
    public BatchSetNotificationIsReadDTOValidator()
    {
        RuleFor(x => x.NotificationIds)
            .NotEmpty();
    }
}