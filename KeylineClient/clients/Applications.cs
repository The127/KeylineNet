using System.Net.Http.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace KeylineClient.clients;

[UsedImplicitly]
public class ListApplicationsResponseDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("displayName")]
    public required string DisplayName { get; set; }
    
    [JsonPropertyName("type")]
    public required string Type { get; set; }
        
    [JsonPropertyName("systemApplication")]
    public bool SystemApplication { get; set; }
}

[UsedImplicitly]
public class CreateApplicationRequestDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("displayName")]
    public required string DisplayName { get; set; }
    
    [JsonPropertyName("redirectUris")]
    public required List<string> RedirectUris { get; set; }
    
    [JsonPropertyName("postLogoutUris")]
    public List<string> PostLogoutUris { get; set; } = [];
    
    [JsonPropertyName("type")]
    public required string Type { get; set; }
    
    [JsonPropertyName("accessTokenHeaderType")]
    public string? AccessTokenHeaderType { get; set; }
}

[UsedImplicitly]
public class CreateApplicationResponseDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("secret")]
    public string? Secret { get; set; }
}

public interface IApplicationsClient
{
    Task<PagedResponse<ListApplicationsResponseDto>> List(CancellationToken cancellationToken = default);
    Task<CreateApplicationResponseDto> CreateAsync(CreateApplicationRequestDto request, CancellationToken cancellationToken = default);
}

public class ApplicationsClient(
    HttpClient httpClient,
    string virtualServer,
    string projectSlug
) : IApplicationsClient
{
    public async Task<PagedResponse<ListApplicationsResponseDto>> List(CancellationToken cancellationToken = default)
    {
        var uri = new Uri($"api/virtual-servers/{virtualServer}/projects/{projectSlug}/applications", UriKind.Relative);
        
        var response = await httpClient.GetAsync(uri, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PagedResponse<ListApplicationsResponseDto>>(cancellationToken);
        return result!;
    }

    public async Task<CreateApplicationResponseDto> CreateAsync(CreateApplicationRequestDto request, CancellationToken cancellationToken = default)
    {
        var uri = new Uri($"api/virtual-servers/{virtualServer}/projects/{projectSlug}/applications", UriKind.Relative);

        var response = await httpClient.PostAsJsonAsync(uri, request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<CreateApplicationResponseDto>(cancellationToken);
        return result!;
    }
}
