namespace KeylineClient.auth;

public interface IAuthenticator
{
    Task<AutenticationResult> AuthenticateAsync(CancellationToken cancellationToken = default);
}