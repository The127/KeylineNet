using System.Net.Http.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace KeylineClient.clients;

[UsedImplicitly]
public class ApplicationListResponseDto
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

public interface IApplicationsClient
{
    Task<PagedResponse<ApplicationListResponseDto>> List(CancellationToken cancellationToken = default);
}

public class ApplicationsClient(
    HttpClient http,
    string virtualServer,
    string projectSlug
) : IApplicationsClient
{
    public async Task<PagedResponse<ApplicationListResponseDto>> List(CancellationToken cancellationToken = default)
    {
        var response = await http.GetAsync(
            $"api/virtual-servers/{virtualServer}/projects/{projectSlug}/applications", 
            cancellationToken
        );
        
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PagedResponse<ApplicationListResponseDto>>(cancellationToken: cancellationToken);
        return result!;
    }
}