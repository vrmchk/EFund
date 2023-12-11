using EFund.Common.Enums;

namespace EFund.Common.Models.DTO.Error;

public class ExternalErrorDTO : ErrorDTO
{
    public ExternalErrorDTO(string message) : base(ErrorCode.ExternalError, message) { }
}