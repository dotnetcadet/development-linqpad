<Query Kind="Program">
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	using var client = new HttpClient();
	using var request1 = new HttpRequestMessage()
	{
		Method = HttpMethod.Get,
		RequestUri = new Uri("https://localhost:8084"),
		Version = Version.Parse("2.0.0"),
		VersionPolicy = HttpVersionPolicy.RequestVersionExact
	};
	using var request2 = new HttpRequestMessage()
	{
		Method = HttpMethod.Get,
		RequestUri = new Uri("https://localhost:8084"),
		Version = Version.Parse("2.0.0"),
		VersionPolicy = HttpVersionPolicy.RequestVersionExact
	};
	
	// Both Requests should be made over the same connection and should 
	// be 
	var responses = await Task.WhenAll(
		client.SendAsync(request1), 
		client.SendAsync(request2));
	
}

// You can define other methods, fields, classes and namespaces here
