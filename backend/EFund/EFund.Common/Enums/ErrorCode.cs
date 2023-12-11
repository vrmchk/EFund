namespace EFund.Common.Enums;

public enum ErrorCode
{
    Unknown,
    ValidationFailed,
    NotFound,
    AlreadyExists,
    IncorrectParameters,
    Expired,
    Forbidden,
    IdentityError,
    ExternalError
}