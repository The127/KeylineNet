using System.Net.Http.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace KeylineClient.clients;

[UsedImplicitly]
public record CreateVirtualServerRequestDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("displayName")]
    public required string DisplayName { get; set; }

    [JsonPropertyName("enableRegistration")]
    public required bool EnableRegistration { get; set; }

    [JsonPropertyName("signingAlgorithm")]
    public required string SigningAlgorithm { get; set; }

    [JsonPropertyName("require2fa")]
    public required bool Require2Fa { get; set; }

    [JsonPropertyName("admin")]
    public CreateVirtualServerRequestDtoAdminDto? Admin { get; set; }

    [JsonPropertyName("serviceUsers")]
    public List<CreateVirtualServerRequestDtoServiceUserDto> ServiceUsers { get; set; } = [];

    [JsonPropertyName("projects")]
    public List<CreateVirtualServerRequestDtoProjectDto> Projects { get; set; } = [];
}

public record CreateVirtualServerRequestDtoAdminDto
{
    [JsonPropertyName("username")]
    public required string Username { get; set; }

    [JsonPropertyName("displayName")]
    public required string DisplayName { get; set; }

    [JsonPropertyName("primaryEmail")]
    public required string PrimaryEmail { get; set; }

    [JsonPropertyName("passwordHash")]
    public required string PasswordHash { get; set; }
}

public record CreateVirtualServerRequestDtoServiceUserDto
{
    [JsonPropertyName("username")]
    public required string Username { get; set; }

    [JsonPropertyName("roles")]
    public required List<string> Roles { get; set; }

    [JsonPropertyName("publicKey")]
    public required CreateVirtualServerRequestDtoPublicKeyDto PublicKey { get; set; }
}

public record CreateVirtualServerRequestDtoPublicKeyDto
{
    [JsonPropertyName("pem")]
    public required string Pem { get; set; }

    [JsonPropertyName("kid")]
    public required string Kid { get; set; }
}

public record CreateVirtualServerRequestDtoProjectDto
{
    [JsonPropertyName("slug")]
    public required string Slug { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = "";

    [JsonPropertyName("roles")]
    public List<CreateVirtualServerRequestDtoProjectDtoRoleDto> Roles { get; set; } = [];

    [JsonPropertyName("applications")]
    public List<CreateVirtualServerRequestDtoProjectDtoApplicationDto> Applications { get; set; } = [];

    [JsonPropertyName("resourceServers")]
    public List<CreateVirtualServerRequestDtoProjectDtoResourceServerDto> ResourceServers { get; set; } = [];
}

public record CreateVirtualServerRequestDtoProjectDtoRoleDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = "";
}

public record CreateVirtualServerRequestDtoProjectDtoApplicationDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("displayName")]
    public required string DisplayName { get; set; }

    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("hashedSecret")]
    public string? HashedSecret { get; set; }

    [JsonPropertyName("redirectUris")]
    public required List<string> RedirectUris { get; set; }

    [JsonPropertyName("postLogoutUris")]
    public List<string> PostLogoutUris { get; set; } = [];
}

public record CreateVirtualServerRequestDtoProjectDtoResourceServerDto
{
    [JsonPropertyName("slug")]
    public required string Slug { get; set; }
    
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = "";
}

public interface IVirtualServersClient
{
    Task CreateAsync(CreateVirtualServerRequestDto request, CancellationToken cancellationToken = default);
}

public class VirtualServersClient(HttpClient httpClient) : IVirtualServersClient
{
    public async Task CreateAsync(CreateVirtualServerRequestDto request, CancellationToken cancellationToken = default)
    {
        var uri = new Uri($"api/virtual-servers", UriKind.Relative);

        var response = await httpClient.PostAsJsonAsync(uri, request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}