using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace KeylineClient.clients;

public class ListGroupsResponseDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }
}

public interface IGroupsClient
{
    Task<PagedResponse<ListGroupsResponseDto>> ListAsync(CancellationToken cancellationToken = default);
}

public class GroupsClient(
    HttpClient httpClient,
    string virtualServer
)
    : IGroupsClient
{
    public async Task<PagedResponse<ListGroupsResponseDto>> ListAsync(CancellationToken cancellationToken = default)
    {
        var uri = new Uri($"api/virtual-servers/{virtualServer}/groups");

        var response = await httpClient.GetAsync(uri, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PagedResponse<ListGroupsResponseDto>>(cancellationToken);
        return result!;
    }
}