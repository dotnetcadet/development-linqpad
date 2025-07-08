<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

async Task Main()
{
	using var client = new HttpClient();
	
	var result = await client.SendAsync(new()
	{
		Method = new HttpMethod("GET"),
		RequestUri = new Uri("https://localhost:8080/api/items"),
		Version = Version.Parse("3.0"),
		VersionPolicy = HttpVersionPolicy.RequestVersionExact,
	});

	result.Dump();
}

// You can define other methods, fields, classes and namespaces here