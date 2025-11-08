namespace KeylineClient.auth;

public record KeyInfo(string PrivateKeyPem, string Kid);

public interface IPrivateKeyProvider
{
    Task<KeyInfo> GetPrivateKeyAsync(CancellationToken cancellationToken = default);
}

public class StaticPrivateKeyProvider(
    string privateKeyPem,
    string kid
) : IPrivateKeyProvider
{
    public Task<KeyInfo> GetPrivateKeyAsync(CancellationToken cancellationToken = default) => 
        Task.FromResult(new KeyInfo(privateKeyPem, kid));
}

public static class PrivateKeyProviderFactory
{
    public static IPrivateKeyProvider Static(string privateKeyPem, string kid) =>
        new StaticPrivateKeyProvider(privateKeyPem, kid);
}