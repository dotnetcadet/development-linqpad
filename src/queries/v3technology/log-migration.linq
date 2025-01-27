<Query Kind="Program">
  <NuGetReference>Azure.Identity</NuGetReference>
  <NuGetReference>Azure.Monitor.Query</NuGetReference>
  <NuGetReference>Microsoft.ApplicationInsights.AspNetCore</NuGetReference>
  <Namespace>Azure.Monitor.Query</Namespace>
  <Namespace>Microsoft.ApplicationInsights</Namespace>
  <Namespace>Microsoft.ApplicationInsights.DataContracts</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Azure.Identity</Namespace>
</Query>

const string workspaceId = "87355881-318d-4419-8d81-f98344e8195d"; 

async Task Main()
{
	var query = new LogsQueryClient(new DefaultAzureCredential(new DefaultAzureCredentialOptions()
	{
		TenantId = "231e7b61-54b4-43d5-b83a-8b0c035f6784"
	}));
	var client = new TelemetryClient(new()
	{
		ConnectionString = ""
	});
	
	var response = await query.QueryWorkspaceAsync(workspaceId, "AppPageViews", QueryTimeRange.All);
	
	
	foreach (var row in response.Value.Table.Rows)
	{
		
	}
	
	
 client.TrackPageView(new PageViewTelemetry() 
 {
 	
 });
}

// You can define other methods, fields, classes and namespaces here
