using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;

namespace KeylineClient.auth;

public class ServiceUserAuthenticator(
    string tokenEndpointUrl,
    Func<CancellationToken, Task<string>> privateKeyPemProvider,
    string serviceUser,
    string kid,
    string audience
) : IAuthenticator
{
    public ServiceUserAuthenticator(
        string tokenEndpointUrl,
        string privateKeyPem,
        string serviceUser,
        string kid,
        string audience
    )
        : this(
            tokenEndpointUrl,
            _ => Task.FromResult(privateKeyPem),
            serviceUser,
            kid,
            audience
        )
    {
    }

    public async Task<AutenticationResult> AuthenticateAsync(CancellationToken cancellationToken = default)
    {
        var signedToken = await CreateSignedTokenAsync(cancellationToken);
        var exchangedToken = await ExchangeTokenAsync(signedToken, cancellationToken);
        return new AutenticationResult(exchangedToken);
    }

    private SigningCredentials ParsePrivateKeyPem(string privateKeyPem)
    {
        try
        {
            var rsa = RSA.Create();
            rsa.ImportFromPem(privateKeyPem);
            var rsaSecurityKey = new RsaSecurityKey(rsa)
            {
                KeyId = kid,
            };
            return new SigningCredentials(
                rsaSecurityKey, 
                SecurityAlgorithms.RsaSha256
            );
        }
        catch (ArgumentException)
        {
            // ignore
        }

        try
        {
            var ec = ECDsa.Create();
            ec.ImportFromPem(privateKeyPem);
            var ecDsaSecurityKey = new ECDsaSecurityKey(ec)
            {
                KeyId = kid,
            };
            return new SigningCredentials(
                ecDsaSecurityKey, 
                SecurityAlgorithms.EcdsaSha256
            );
        }
        catch (ArgumentException)
        {
            // ignore
        }
        
        throw new ArgumentException("Invalid private key, must be RSA or ECDSA");
    }
    
    private async Task<string> CreateSignedTokenAsync(CancellationToken cancellationToken)
    {
        var privateKeyPem = await privateKeyPemProvider(cancellationToken);
        var privateKey = ParsePrivateKeyPem(privateKeyPem);

        
        var jsonWebTokenHandler = new JsonWebTokenHandler();
        var signedToken = jsonWebTokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = serviceUser,
            Subject = new ClaimsIdentity([
                new Claim("sub", serviceUser),
                new Claim("scopes", "openid")
            ]),
            Audience = audience,
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = privateKey,
        });
        
        return signedToken;
    }

    private async Task<string> ExchangeTokenAsync(string signedToken, CancellationToken cancellationToken)
    {
        var httpClient = new HttpClient();
        var formUrlEncodedContent = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "urn:ietf:params:oauth:grant-type:token-exchange",
            ["subject_token_type"] = "urn:ietf:params:oauth:token-type:access_token",
            ["subject_token"] = signedToken,
        });
        var response = await httpClient.PostAsync(tokenEndpointUrl, formUrlEncodedContent, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseBodyString = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var doc = await JsonDocument.ParseAsync(responseBodyString, cancellationToken: cancellationToken);
        return doc.RootElement.GetProperty("access_token").GetString() ?? throw new Exception("No access token in response");
    }
}