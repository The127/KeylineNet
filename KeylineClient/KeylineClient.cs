using KeylineClient.clients;

namespace KeylineClient;

public interface IKeylineClient
{
    IProjectsClient Projects { get; }
    IGroupsClient Groups { get; }
}

public class KeylineClient(HttpClient httpClient, string virtualServer) : IKeylineClient
{
    public IProjectsClient Projects  =>
        new ProjectsClient(httpClient, virtualServer);
    
    public IGroupsClient Groups =>
        new GroupsClient(httpClient, virtualServer);
}