using EFund.Common.Enums;

namespace EFund.Common.Models.DTO.Error;

public class ExpiredErrorDTO : ErrorDTO
{
    public ExpiredErrorDTO(string message) : base(ErrorCode.Expired, message) { }
}