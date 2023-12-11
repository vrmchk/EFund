using EFund.Common.Models.Utility;
using EFund.Email.Models.Base;
using LanguageExt;

namespace EFund.Email.Services.Interfaces;

public interface IEmailSender
{
    Task<Option<ErrorModel>> SendEmailAsync<T>(string to, T message)
        where T : EmailMessageBase;
}