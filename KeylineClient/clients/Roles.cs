using System.Net.Http.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace KeylineClient.clients;

[UsedImplicitly]
public class CreateRoleRequestDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; } = "";
}

[UsedImplicitly]
public class CreateRoleResponseDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}

public interface IRolesClient
{
    Task<CreateRoleResponseDto> CreateAsync(CreateRoleRequestDto request, CancellationToken cancellationToken = default);
    IRoleClient this[Guid roleId] { get; }
}

public class RolesClient(
    HttpClient httpClient,
    string virtualServer,
    string projectSlug
)
    : IRolesClient
{
    public async Task<CreateRoleResponseDto> CreateAsync(CreateRoleRequestDto request, CancellationToken cancellationToken = default)
    {
        var uri = new Uri($"api/virtual-servers/{virtualServer}/projects/{projectSlug}/roles", UriKind.Relative);
        
        var response = await httpClient.PostAsJsonAsync(uri, request, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadFromJsonAsync<CreateRoleResponseDto>(cancellationToken);
        return result!;
    }

    public IRoleClient this[Guid roleId] => 
        new RoleClient(httpClient, virtualServer, projectSlug, roleId);
}

[UsedImplicitly]
public class GetRoleResponseDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; } = "";
    
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
    
    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; set; }
}

public interface IRoleClient
{
    Task<GetRoleResponseDto> GetAsync(CancellationToken cancellationToken = default);
}

public class RoleClient(
    HttpClient httpClient,
    string virtualServer,
    string projectSlug,
    Guid roleId
)
    : IRoleClient
{
    public async Task<GetRoleResponseDto> GetAsync(CancellationToken cancellationToken = default)
    {
        var uri = new Uri($"api/virtual-servers/{virtualServer}/projects/{projectSlug}/roles/{roleId}", UriKind.Relative);

        var response = await httpClient.GetAsync(uri, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<GetRoleResponseDto>(cancellationToken);
        return result!;
    }
}