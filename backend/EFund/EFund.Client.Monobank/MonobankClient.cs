using System.Text.Json;
using EFund.Client.Monobank.Extensions;
using EFund.Client.Monobank.Models.Requests;
using EFund.Client.Monobank.Models.Responses;
using EFund.Common.Enums;
using EFund.Common.Models.Configs;
using LanguageExt;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace EFund.Client.Monobank;

public class MonobankClient : IMonobankClient
{
    private readonly MonobankConfig _config;
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<MonobankClient> _logger;

    public MonobankClient(MonobankConfig config,
        IHttpClientFactory clientFactory,
        ILogger<MonobankClient> logger)
    {
        _config = config;
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public Task<Either<ErrorCode, ClientInfo>> GetClientInfoAsync(ClientInfoRequest request)
    {
        return SendRequestAsync<ClientInfo>("/personal/client-info", request);
    }

    public Task<Either<ErrorCode, IEnumerable<Transaction>>> GetStatementAsync(StatementRequest request)
    {
        var resource = $"/personal/statement/{request.Account}/{request.From}";

        if (request.To != 0)
            resource += $"/{request.To}";

        return SendRequestAsync<IEnumerable<Transaction>>(resource, request);
    }

    private async Task<Either<ErrorCode, TResponse>> SendRequestAsync<TResponse>(string resource, RequestBase request)
    {
        var restRequest = new RestRequest($"{_config.BaseAddress}{resource}")
            .AddHeader("X-Token", request.Token);

        var client = _clientFactory.CreateRestClient(_config.HttpClientName);

        var response = await client.ExecuteAsync<TResponse>(restRequest);

        if (!response.IsSuccessStatusCode || string.IsNullOrWhiteSpace(response.Content))
        {
            _logger.LogError("Failed to execute request on {Resource}. {Content}", restRequest.Resource, response.Content);
            return ErrorCode.ExternalError;
        }

        var result = JsonSerializer.Deserialize<TResponse>(response.Content, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        if (result == null)
        {
            _logger.LogError("Failed to convert json to model {Name}. {Content}", typeof(TResponse).Name, response.Content);
            return ErrorCode.ExternalError;
        }

        return result;
    }
}
