using EFund.Client.Monobank.Models.Requests;
using EFund.Client.Monobank.Models.Responses;
using EFund.Common.Enums;
using LanguageExt;

namespace EFund.Client.Monobank;

public interface IMonobankClient
{
    Task<Either<ErrorCode, ClientInfo>> GetClientInfoAsync(ClientInfoRequest request);
    Task<Either<ErrorCode, IEnumerable<Transaction>>> GetStatementAsync(StatementRequest request);
}