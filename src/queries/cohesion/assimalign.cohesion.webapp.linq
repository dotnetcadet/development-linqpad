<Query Kind="Program">
<NuGetReference Version="8.0.0">System.IO.Pipelines</NuGetReference>
<Namespace>System</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>System.Threading.Tasks</Namespace>
<Namespace>Hosting</Namespace>
<Namespace>Configuration</Namespace>
<Namespace>Net.Http</Namespace>
<Namespace>DependencyInjection</Namespace>
<Namespace>System.Threading</Namespace>
<Namespace>Assimalign.Cohesion.Net.Http</Namespace>
<Namespace>System.Reflection</Namespace>
</Query>
#load ".\assimalign.cohesion.configuration"
#load ".\assimalign.cohesion.dependencyinjection"
#load ".\assimalign.cohesion.hosting"
#load ".\assimalign.cohesion.logging"
#load ".\assimalign.cohesion.net"
#load ".\assimalign.cohesion.core"
#load ".\assimalign.cohesion.loadbalancer"
#load ".\assimalign.cohesion.net.cryptography"
#load ".\assimalign.cohesion.net.http"
#load ".\assimalign.cohesion.net.transports"
#load ".\assimalign.cohesion.net.udt"
#load ".\assimalign.cohesion.net.websockets"

void Main()
{

}

#region Assimalign.Cohesion.WebApp(net8.0)
namespace Assimalign.Cohesion.WebApp
{
	#region \
	public class WebApiApplicationBuilder
	{
	    internal WebApiApplicationBuilder()
	    {
	    }
	    public HttpServerBuilder Server { get; }
	    public ConfigurationManager Configuration { get; }
	    public IServiceCollection Services { get; set; }
	    public static WebApiApplicationBuilder Create()
	    {
	        return default;
	    }
	}
	#endregion
	#region \Internal\Execution
	internal class HttpContextExecutor : IHttpContextExecutor
	{
	    public HttpContextExecutor()
	    {
	    }
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
