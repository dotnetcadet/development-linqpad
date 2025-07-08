<Query Kind="Program">
  <Namespace>Microsoft.AspNetCore.Builder</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>Microsoft.AspNetCore.Hosting</Namespace>
  <Namespace>Microsoft.AspNetCore.Http</Namespace>
  <Namespace>Microsoft.AspNetCore.Hosting.Server</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

void Main()
{
	var builder = WebApplication.CreateBuilder();
	
	builder.WebHost.ConfigureKestrel(options =>
	{
		options.ListenAnyIP(8081);
	});
	
	var app = builder.Build();	
	

	app.MapGet("/test/{id}", async context =>
	{
		context.Response.StatusCode = (int)HttpStatusCode.NoContent;
	});
	
	app.UseRouting();
	
	app.Run();
}

// You can define other methods, fields, classes and namespaces here