<Query Kind="Program">
<NuGetReference Version="8.0.0">System.IO.Pipelines</NuGetReference>
<Namespace>System</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>System.Threading</Namespace>
<Namespace>System.Threading.Tasks</Namespace>
<Namespace>Assimalign.OGraph</Namespace>
<Namespace>Assimalign.Cohesion.Net.Http</Namespace>
<Namespace>Assimalign.Cohesion.Hosting</Namespace>
<Namespace>Assimalign.Cohesion.Logging</Namespace>
<Namespace>Assimalign.Cohesion.DependencyInjection</Namespace>
<Namespace>Assimalign.Cohesion.Configuration</Namespace>
<Namespace>Assimalign.Cohesion.OGraph</Namespace>
<Namespace>System.Security.Claims</Namespace>
<Namespace>Assimalign.OGraph.Server</Namespace>
<Namespace>System.Reflection</Namespace>
</Query>
#load ".\assimalign.ograph"
#load ".\assimalign.cohesion.configuration"
#load ".\assimalign.cohesion.dependencyinjection"
#load ".\assimalign.cohesion.logging"
#load ".\assimalign.cohesion.net"
#load ".\assimalign.cohesion.core"
#load ".\assimalign.cohesion.hosting"
#load ".\assimalign.cohesion.loadbalancer"
#load ".\assimalign.cohesion.net.cryptography"
#load ".\assimalign.cohesion.net.http"
#load ".\assimalign.cohesion.net.transports"
#load ".\assimalign.cohesion.net.udt"
#load ".\assimalign.cohesion.net.websockets"
#load ".\assimalign.ograph.client"
#load ".\assimalign.ograph.core"
#load ".\assimalign.ograph.gdm"
#load ".\assimalign.ograph.server"
#load ".\assimalign.ograph.syntax"

void Main()
{

}

#region Assimalign.Cohesion.OGraph(net8.0)
namespace Assimalign.Cohesion.OGraph
{
	#region \
	public sealed class OGraphExecutor : IOGraphExecutor, IHttpContextExecutor
	{
	    public OGraphExecutor()
	    {
	    }
	    public Task ExecuteAsync(IHttpContext context, CancellationToken cancellationToken = default)
	    {
	        throw new NotImplementedException();
	    }
	    public Task ExecuteAsync(IOGraphExecutorContext context, CancellationToken cancellationToken = default)
	    {
	        throw new NotImplementedException();
	    }
	}
	#endregion
	#region \Hosting
	public sealed class OGraphApplicationBuilder
	{
	    internal OGraphApplicationBuilder()
	    {
	        Services = new ServiceProviderBuilder();
	        Configuration = new ConfigurationBuilder();
	    }
	    public IConfigurationBuilder Configuration { get; init; }
	    public IServiceProviderBuilder Services { get; init; }
	}
	#endregion
	#region \Hosting\Extensions
	public static class OGraphHostBuilderExtensions
	{
	    public static OGraphApplicationBuilder AddOGraphServer(this IHostBuilder builder)
	    {
	        var app = new OGraphApplicationBuilder();
	        builder.AddHttpServer(server =>
	        {
	            var serviceProvider = app.Services.Build();
	            var configurationBuilder = app.Configuration;
	            server.ConfigureServer(options =>
	            {
	                options.UseExecutor(new OGraphExecutor());
	            });
	            var configuration = configurationBuilder.Build();
	            server.ConfigureServiceProvider(serviceProvider);
	        });
	        return app;
	    }
	}
	#endregion
	#region \Internal
	internal class OGraphExecutorContext : IOGraphExecutorContext
	{
	    public IHttpContext HttpContext { get; set; }
	    public IOGraphRequest Request => throw new NotImplementedException();
	    public IOGraphResponse Response => throw new NotImplementedException();
	    public IServiceProvider? ServiceProvider => throw new NotImplementedException();
	    public ClaimsPrincipal ClaimsPrincipal => throw new NotImplementedException();
	}
	internal class OGraphHttpExecutor : IHttpContextExecutor
	{
	    public readonly IOGraphExecutor executor;
	    public OGraphHttpExecutor(IOGraphExecutor executor)
    {
        this.executor = executor;
    }
	    public OGraphExecutorContext Content { get; } = new();
	    public Task ExecuteAsync(IHttpContext context, CancellationToken cancellationToken = default)
	    {
	        throw new NotImplementedException();
	    }
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
}
#endregion
