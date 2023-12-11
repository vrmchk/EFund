using EFund.Common.Enums;

namespace EFund.Common.Models.DTO.Error;

public class ForbiddenErrorDTO : ErrorDTO
{
    public ForbiddenErrorDTO(string message) : base(ErrorCode.Forbidden, message) { }
}