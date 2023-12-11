using EFund.Common.Enums;

namespace EFund.Common.Models.DTO.Error;

public class ErrorDTO
{
    public ErrorDTO(ErrorCode errorCode, string message)
    {
        Message = message;
        ErrorCode = errorCode;
    }

    public ErrorCode ErrorCode { get; }
    public string Message { get; }
}