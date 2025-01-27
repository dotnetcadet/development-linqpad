<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.Configuration</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Configuration.Abstractions</NuGetReference>
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <Namespace>Microsoft.Extensions.Configuration</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.Primitives</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
</Query>

void Main()
{
	var services = new ServiceCollection();

	var builder = new ConfigurationBuilder();

	builder.
		builder.Add(new GithubConfigurationSource());

	var root = builder.Build();


}

public sealed class GithubConfigurationOptions {
	public string? OrganizationOrUsername { get; set; }
}


internal class GithubConfigurationSource : IConfigurationSource
{

	public GithubConfigurationSource()
	{

	}


	public IConfigurationProvider Build(IConfigurationBuilder builder)
	{
		return new GithubConfigurationProvider();
	}
}

internal class GithubConfigurationProvider : IConfigurationProvider
{
	private readonly Task<Response> load;

	public GithubConfigurationProvider()
	{
		load = new ValueTask<Response>(Task.Run(async () =>
		{
			using var client = new HttpClient();
			
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
			client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
			
			
			
			return default(Response);
		));
	}

	private void Initialize()
	{

	}

	public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
	{
		throw new NotImplementedException();
	}

	public IChangeToken GetReloadToken()
	{
		throw new NotImplementedException();
	}

	public void Load()
	{
		throw new NotImplementedException();
	}

	public void Set(string key, string value)
	{
		throw new NotImplementedException();
	}

	public bool TryGet(string key, out string value)
	{
		throw new NotImplementedException();
	}



	#region Models

	partial class Response
	{
		[JsonPropertyName("total_count")]
		public long? Total { get; set; }

		[JsonPropertyName("variables")]
		public IEnumerable<Variable>? Variables { get; set; }
	}
	partial class Variable
	{
		[JsonPropertyName("name")]
		public string? Name { get; set; }

		[JsonPropertyName("value")]
		public string? Value { get; set; }
	}

	#endregion
}
