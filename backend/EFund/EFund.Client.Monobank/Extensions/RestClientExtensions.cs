using RestSharp;

namespace EFund.Client.Monobank.Extensions;

public static class RestClientExtensions
{
    public static IRestClient CreateRestClient(this IHttpClientFactory clientFactory, string name)
    {
        var client = clientFactory.CreateClient(name);
        return new RestClient(client);
    }
}