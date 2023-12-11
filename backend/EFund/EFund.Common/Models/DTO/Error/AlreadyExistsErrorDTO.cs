using EFund.Common.Enums;

namespace EFund.Common.Models.DTO.Error;

public class AlreadyExistsErrorDTO : ErrorDTO
{
    public AlreadyExistsErrorDTO(string message) : base(ErrorCode.AlreadyExists, message) { }
}