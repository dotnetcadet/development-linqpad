<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	var client = default(IDevOpsClient);
	
	client.Projects
}




#region Abstractions

public interface IDevOpsClient {
	IDevOpsProjectCollectionRequestBuilder Projects { get; }
	IDevOpsTeamCollectionRequestBuilder Teams { get; }
	
}
public interface IDevOpsProjectCollectionRequestBuilder {
	
	IDevOpsProjectCollectionRequestBuilder ContinuationToken(string token);
	IDevOpsProjectCollectionRequestBuilder Take(int take);
	IDevOpsProjectCollectionRequestBuilder Skip(int skip);
	Task CreateAsync();
}
public interface IDevOpsProjectRequestBuilder {
	
}
public interface IDevOpsTeamCollectionRequestBuilder {
	
}
public interface IDevOpsTeamRequestBuilder {
	
}

#endregion

#region Models
public abstract class Entity {
	
}
public class Project : Entity {
	public string? Name { get; set; }
}
#endregion


public sealed class Route {

	public const string Host = "https://dev.azure.com/";
	public const string Project = "/_apis/projects";
		
	
	
	
}