using EFund.Common.Enums;

namespace EFund.Common.Models.DTO.Error;

public class IdentityErrorDTO : ErrorDTO
{
    public IdentityErrorDTO(string message) : base(ErrorCode.IdentityError, message) { }
}