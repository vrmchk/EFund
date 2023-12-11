using EFund.Common.Enums;

namespace EFund.Common.Models.DTO.Error;

public class ValidationFailedErrorDTO : ErrorDTO
{
    public ValidationFailedErrorDTO(IDictionary<string, string[]> body)
        : base(ErrorCode.ValidationFailed, "Validation error")
    {
        Body = body;
    }

    public IDictionary<string, string[]> Body { get; private set; }
}