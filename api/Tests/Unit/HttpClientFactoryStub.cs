namespace Tests.Unit;

public class HttpClientFactoryStub : IHttpClientFactory
{
    private readonly Func<string, HttpClient> _clientFactory;

    public HttpClientFactoryStub(HttpClient client)
    {
        if (client == null) throw new ArgumentNullException(nameof(client));
        _clientFactory = _ => client;
    }

    public HttpClientFactoryStub(Func<string, HttpClient> clientFactory)
    {
        _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
    }

    public HttpClient CreateClient(string name) => _clientFactory(name);
}

public class HttpMessageHandlerStub : HttpMessageHandler
{
    private readonly HttpResponseMessage _response;

    public HttpMessageHandlerStub(HttpResponseMessage response)
    {
        _response = response ?? throw new ArgumentNullException(nameof(response));
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_response);
    }
}
