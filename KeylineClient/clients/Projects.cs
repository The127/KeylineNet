namespace KeylineClient.clients;

public interface IProjectsClient
{
    IProjectClient this[string slug] { get; }
}

public class ProjectsClient(
    HttpClient httpClient, 
    string virtualServer
) 
   : IProjectsClient
{
    public IProjectClient this[string slug] =>
        new ProjectClient(httpClient, virtualServer, slug);
}

public interface IProjectClient
{
    IApplicationsClient Applications { get; }
}

public class ProjectClient(
    HttpClient httpClient,
    string virtualServer,
    string slug
) : IProjectClient
{
    public IApplicationsClient Applications => 
        new ApplicationsClient(httpClient, virtualServer, slug);
}