<Query Kind="Program">
  <Namespace>Microsoft.AspNetCore.Builder</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>Microsoft.AspNetCore.Hosting</Namespace>
  <Namespace>Microsoft.AspNetCore.Http</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

void Main()
{

	var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
	{
		
	});


	var path = builder.Environment.ContentRootPath.Dump();
	
	Directory.GetFiles(path).Dump();
	builder.WebHost.ConfigureKestrel(options =>
	{
		options.ListenAnyIP(8081);
	});

	var app = builder.Build();
	
	
	app.MapGet("/test/{id}", async context =>
	{
		context.Response.StatusCode = (int)HttpStatusCode.NoContent;
	});
	app.Run();

}

// You can define other methods, fields, classes and namespaces here