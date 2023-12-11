using EFund.Common.Enums;

namespace EFund.Common.Models.DTO.Error;

public class IncorrectParametersErrorDTO : ErrorDTO
{
    public IncorrectParametersErrorDTO(string message) : base(ErrorCode.IncorrectParameters, message) { }
}