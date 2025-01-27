<Query Kind="Program">
<NuGetReference Version="8.0.0">System.IO.Pipelines</NuGetReference>
<Namespace>System</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>System.Threading.Tasks</Namespace>
<Namespace>System.Reflection</Namespace>
</Query>
#load ".\assimalign.cohesion.net"
#load ".\assimalign.cohesion.configuration"
#load ".\assimalign.cohesion.core"
#load ".\assimalign.cohesion.hosting"
#load ".\assimalign.cohesion.loadbalancer"
#load ".\assimalign.cohesion.logging"
#load ".\assimalign.cohesion.net.cryptography"
#load ".\assimalign.cohesion.net.http"
#load ".\assimalign.cohesion.net.transports"
#load ".\assimalign.cohesion.net.udt"
#load ".\assimalign.cohesion.net.websockets"

void Main()
{

}

#region Assimalign.Cohesion.MessageHub(net8.0)
namespace Assimalign.Cohesion.MessageHub
{
	#region \Extensions
	public static class HostingExtensions
	{
	    public static IHostBuilder AddMessageHub(this IHostBuilder builder)
	    {
	        return builder;
	    }
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
}
#endregion
