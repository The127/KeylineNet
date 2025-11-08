using KeylineClient.auth;

namespace KeylineClient;

public class ClientFactory(string baseUrl, string virtualServer)
{
    private IAuthenticator? _authenticator;
    
    public ClientFactory WithServiceUserAuth(string audience, string serviceUserName, IPrivateKeyProvider privateKeyProvider)
    {
        _authenticator = new ServiceUserAuthenticator(
            $"{baseUrl}/oidc/{virtualServer}/token",
            privateKeyProvider,
            serviceUserName,
            audience
        );
        return this;
    }
    
    public IKeylineClient Create()
    {
        var authHandler = new AuthenticationHandler(_authenticator ?? throw new InvalidOperationException("No authenticator set"))
        {
            InnerHandler = new HttpClientHandler(),
        };
        
        var httpClient = new HttpClient(authHandler);
        httpClient.BaseAddress = new Uri(baseUrl);
        
        return new KeylineClient(httpClient, virtualServer);
    }
}