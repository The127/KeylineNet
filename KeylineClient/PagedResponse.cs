using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace KeylineClient;

[UsedImplicitly]
public class PagedResponse<T>
{
    [JsonPropertyName("items")]
    public List<T> Items { get; set; } = null!;
    
    [JsonPropertyName("pagination")]
    public PaginationInfo PaginationInfo { get; set; } = null!;
}

[UsedImplicitly]
public class PaginationInfo
{
    [JsonPropertyName("size")]
    public int Size { get; set; } 
    
    [JsonPropertyName("page")]
    public int Page { get; set; }
    
    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
    
    [JsonPropertyName("totalItems")]
    public int TotalItems { get; set; }
}