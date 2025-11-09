using System.Net.Http.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace KeylineClient.clients;

[UsedImplicitly]
public class ListProjectsResponseDto
{
    [JsonPropertyName("id")] public Guid Id { get; set; }

    [JsonPropertyName("slug")] public required string Slug { get; set; }

    [JsonPropertyName("name")] public required string Name { get; set; }

    [JsonPropertyName("systemProject")] public bool SystemProject { get; set; }
}

[UsedImplicitly]
public class CreateProjectRequestDto
{
    [JsonPropertyName("slug")] public required string Slug { get; set; }

    [JsonPropertyName("name")] public required string Name { get; set; }

    [JsonPropertyName("description")] public string? Description { get; set; }
}

public class CreateProjectResponseDto
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
}

public interface IProjectsClient
{
    Task<PagedResponse<ListProjectsResponseDto>> ListAsync(CancellationToken cancellationToken = default);

    Task<CreateProjectResponseDto> CreateAsync(CreateProjectRequestDto request,
        CancellationToken cancellationToken = default);

    IProjectClient this[string slug] { get; }
}

public class ProjectsClient(
    HttpClient httpClient,
    string virtualServer
)
    : IProjectsClient
{
    public async Task<PagedResponse<ListProjectsResponseDto>> ListAsync(CancellationToken cancellationToken = default)
    {
        var uri = new Uri($"api/virtual-servers/{virtualServer}/projects", UriKind.Relative);

        var response = await httpClient.GetAsync(uri, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result =
            await response.Content.ReadFromJsonAsync<PagedResponse<ListProjectsResponseDto>>(cancellationToken);
        return result!;
    }

    public async Task<CreateProjectResponseDto> CreateAsync(CreateProjectRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var uri = new Uri($"api/virtual-servers/{virtualServer}/projects", UriKind.Relative);
        
        var response = await httpClient.PostAsJsonAsync(uri, request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<CreateProjectResponseDto>(cancellationToken);
        return result!;
    }

    public IProjectClient this[string slug] =>
        new ProjectClient(httpClient, virtualServer, slug);
}

public class GetProjectResponseDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("slug")]
    public required string Slug { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("systemProject")]
    public bool SystemProject { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
    
    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; set; }
}

public interface IProjectClient
{
    Task<GetProjectResponseDto> GetAsync(CancellationToken cancellationToken = default);
    IApplicationsClient Applications { get; }
    IRolesClient Roles { get; }
}

public class ProjectClient(
    HttpClient httpClient,
    string virtualServer,
    string slug
) : IProjectClient 
{
    public async Task<GetProjectResponseDto> GetAsync(CancellationToken cancellationToken = default)
    {
        var uri = new Uri($"api/virtual-servers/{virtualServer}/projects/{slug}", UriKind.Relative);
        
        var response = await httpClient.GetAsync(uri, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<GetProjectResponseDto>(cancellationToken);
        return result!;
    }

    public IApplicationsClient Applications =>
        new ApplicationsClient(httpClient, virtualServer, slug);
    
    public IRolesClient Roles => 
        new RolesClient(httpClient, virtualServer, slug);
}
