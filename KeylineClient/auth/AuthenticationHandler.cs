using System.Net.Http.Headers;

namespace KeylineClient.auth;

public class AuthenticationHandler(IAuthenticator authenticator) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var result = await authenticator.AuthenticateAsync(cancellationToken);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
        return await base.SendAsync(request, cancellationToken);
    }
}