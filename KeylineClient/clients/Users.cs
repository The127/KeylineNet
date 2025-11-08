using System.Net.Http.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace KeylineClient.clients;

[UsedImplicitly]
public class ListUsersResponseDto
{
    [JsonPropertyName("id")] public Guid Id { get; set; }

    [JsonPropertyName("username")] public required string Username { get; set; }

    [JsonPropertyName("displayName")] public required string DisplayName { get; set; }

    [JsonPropertyName("primaryEmail")] public required string PrimaryEmail { get; set; }

    [JsonPropertyName("isServiceUser")] public bool IsServiceUser { get; set; }
}

public interface IUsersClient
{
    Task<PagedResponse<ListUsersResponseDto>> ListAsync(CancellationToken cancellationToken = default);
    IUserClient this[Guid userId] { get; }
}

public class UsersClient(
    HttpClient httpClient,
    string virtualServer
)
    : IUsersClient
{
    public async Task<PagedResponse<ListUsersResponseDto>> ListAsync(CancellationToken cancellationToken = default)
    {
        var uri = new Uri($"api/virtual-servers/{virtualServer}/users");

        var response = await httpClient.GetAsync(uri, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PagedResponse<ListUsersResponseDto>>(cancellationToken);
        return result!;
    }

    public IUserClient this[Guid userId] =>
        new UserClient(httpClient, virtualServer, userId);
}

public class GetUserResponseDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("username")]
    public required string Username { get; set; }
    
    [JsonPropertyName("displayName")]
    public required string DisplayName { get; set; }
    
    [JsonPropertyName("primaryEmail")]
    public required string PrimaryEmail { get; set; }
    
    [JsonPropertyName("emailVerified")]
    public bool EmailVerified { get; set; }
    
    [JsonPropertyName("isServiceUser")]
    public bool IsServiceUser { get; set; }

    [JsonPropertyName("createdAt")] 
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")] 
    public DateTime UpdatedAt { get; set; }
}

public class PatchUserRequestDto
{
    [JsonPropertyName("displayName")]
    public string? DisplayName { get; set; }
}

public interface IUserClient
{
    Task<GetUserResponseDto> GetAsync(CancellationToken cancellationToken = default);
    Task PatchAsync(PatchUserRequestDto request, CancellationToken cancellationToken = default);
}

public class UserClient(
    HttpClient httpClient,
    string virtualServer,
    Guid userId
)
    : IUserClient
{
    public async Task<GetUserResponseDto> GetAsync(CancellationToken cancellationToken = default)
    {
        var uri = new Uri($"api/virtual-servers/{virtualServer}/users/{userId}", UriKind.Relative);

        var response = await httpClient.GetAsync(uri, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<GetUserResponseDto>(cancellationToken);
        return result!;
    }

    public async Task PatchAsync(PatchUserRequestDto request, CancellationToken cancellationToken = default)
    {
        var uri = new Uri($"api/virtual-servers/{virtualServer}/users/{userId}", UriKind.Relative);

        var response = await httpClient.PatchAsJsonAsync(uri, request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}