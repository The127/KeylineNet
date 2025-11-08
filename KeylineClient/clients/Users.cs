using System.Net.Http.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace KeylineClient.clients;

[UsedImplicitly]
public class ListUsersResponseDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("username")]
    public required string Username { get; set; }

    [JsonPropertyName("displayName")]
    public required string DisplayName { get; set; }
    
    [JsonPropertyName("primaryEmail")]
    public required string PrimaryEmail { get; set; }
    
    [JsonPropertyName("isServiceUser")]
    public bool IsServiceUser { get; set; }
}

public interface IUsersClient
{
    Task<PagedResponse<ListUsersResponseDto>> ListAsync(CancellationToken cancellationToken = default);
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
}