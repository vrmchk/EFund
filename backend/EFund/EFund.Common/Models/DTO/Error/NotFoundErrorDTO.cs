using EFund.Common.Enums;

namespace EFund.Common.Models.DTO.Error;

public class NotFoundErrorDTO : ErrorDTO
{
    public NotFoundErrorDTO(string message) : base(ErrorCode.NotFound, message) { }
}