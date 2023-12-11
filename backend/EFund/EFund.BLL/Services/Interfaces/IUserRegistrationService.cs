using EFund.Common.Models.Utility;
using LanguageExt;

namespace EFund.BLL.Services.Interfaces;

public interface IUserRegistrationService
{
    Task<Either<ErrorModel, int>> GenerateEmailConfirmationCodeAsync(Guid userId);
    Task<Option<ErrorModel>> CanConfirmEmailAsync(Guid userId, int code);
    Task<Either<ErrorModel, int>> RegenerateEmailConfirmationCodeAsync(Guid userId);
}