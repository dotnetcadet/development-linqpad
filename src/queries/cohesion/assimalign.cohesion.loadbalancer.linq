<Query Kind="Program">
<NuGetReference Version="8.0.0">System.IO.Pipelines</NuGetReference>
<Namespace>System</Namespace>
<Namespace>System.Reflection</Namespace>
<Namespace>Assimalign.Cohesion.Hosting</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>System.Threading.Tasks</Namespace>
</Query>
#load ".\assimalign.cohesion.net.http"
#load ".\assimalign.cohesion.core"
#load ".\assimalign.cohesion.net.transports"

void Main()
{

}

#region Assimalign.Cohesion.LoadBalancer(net8.0)
namespace Assimalign.Cohesion.LoadBalancer
{
	#region \obj\Debug\net8.0
	#endregion
	#region \Server
	public static class HostBuilderExtensions
	{
	    public static IHostBuilder AddLoadBalancer(this IHostBuilder builder)
	    {
	        return builder;
	    }
	}
	#endregion
}
#endregion
