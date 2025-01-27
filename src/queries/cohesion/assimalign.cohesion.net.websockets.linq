<Query Kind="Program">
<NuGetReference Version="8.0.0">System.IO.Pipelines</NuGetReference>
<Namespace>System</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>System.Threading.Tasks</Namespace>
<Namespace>Assimalign.Cohesion.Net.Transports</Namespace>
<Namespace>System.Net</Namespace>
<Namespace>System.Net.WebSockets</Namespace>
<Namespace>System.Reflection</Namespace>
<Namespace>System.Runtime.CompilerServices</Namespace>
<Namespace>System.Threading</Namespace>
<Namespace>Assimalign.Cohesion.Hosting</Namespace>
<Namespace>Assimalign.Cohesion.Net.WebSockets</Namespace>
</Query>
#load ".\assimalign.cohesion.configuration"
#load ".\assimalign.cohesion.hosting"
#load ".\assimalign.cohesion.logging"
#load ".\assimalign.cohesion.net.cryptography"
#load ".\assimalign.cohesion.net.transports"
#load ".\assimalign.cohesion.core"

void Main()
{

}

#region Assimalign.Cohesion.Net.WebSockets(net8.0)
namespace Assimalign.Cohesion.Net.WebSockets
{
	#region \Client
	public sealed class WebSocketClient
	{
	    public WebSocketClient()
	    {
	    }
	    public async Task ConnectAsync()
	    {
	        var transport = Transport.CreateTcpClient(options =>
	        {
	            options.EndPoint = new IPEndPoint(IPAddress.Loopback, 8085);
	        });
	        var connection = await transport.InitializeAsync();
	        var stream = connection.Pipe.GetStream();
	        var websocket = ClientWebSocket.CreateFromStream(stream, new WebSocketCreationOptions()
	        {
	        });
	    }
	}
	public sealed class WebSocketClientOptions
	{
	    public EndPoint EndPoint { get; set; }
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
	#region \Properties
	#endregion
	#region \Server
	public sealed class WebSocketServer : IHostService
	{
	    private readonly IList<ITransport> transports;
	    internal WebSocketServer(WebSocketServerOptions options)
	    {
	        this.transports = options.Transports;
	    }
	    public Task StartAsync(CancellationToken cancellationToken = default)
	    {
	        return ProcessAsync(cancellationToken);
	    }
	    private async Task ProcessAsync(CancellationToken cancellationToken = default)
	    {
	        while (true)
	        {
	            await foreach (var transportConnection in ProcessTransportConnectionsAsync().WithCancellation(cancellationToken))
	            {
	                try
	                {
	                    var stream = transportConnection.Pipe.GetStream();
	                    var socket = WebSocket.CreateFromStream(stream, new WebSocketCreationOptions()
	                    {
	                        IsServer = true,
	                    });
	                    var queued = ThreadPool.UnsafeQueueUserWorkItem(async socket =>
	                    {
	                        try
	                        {
	                        }
	                        catch (Exception exception)
	                        {
	                        }
	                    }, socket, false);
	                }
	                catch (Exception exception)
	                {
	                    continue;
	                }
	            }
	        }
	        async IAsyncEnumerable<ITransportConnection> ProcessTransportConnectionsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
	        {
	            // Will use this as
	            var taskQueue = new Dictionary<Task<ITransportConnection?>, int>();
	            while (true)
	            {
	                // Queue/Re-Queue
	                foreach (var transport in this.transports)
	                {
	                    var hashCode = transport.GetHashCode();
	                    if (!taskQueue.Values.Contains(hashCode))
	                    {
	                        // The underlying transports should handle exceptions and restart accepting 
	                        // connections which is why checking null is all that is needed.
	                        taskQueue.Add(transport.InitializeAsync(cancellationToken), hashCode);
	                    }
	                }
	                var tasks = taskQueue.Select(task => task.Key);
	                var taskCompleted = await Task.WhenAny(tasks);
	                taskQueue.Remove(taskCompleted);
	                var transportConnection = await taskCompleted;
	                // If null, most likely result of connection being aborted.
	                if (transportConnection is null)
	                {
	                    continue;
	                }
	                yield return transportConnection;
	            }
	        }
	    }
	    public Task StopAsync(CancellationToken cancellationToken = default)
	    {
	        throw new NotImplementedException();
	    }
	}
	public sealed class WebSocketServerBuilder : IHostServiceBuilder
	{
	    private Func<IServiceProvider> serviceProviderAction;
	    private IList<Action<IServiceProvider, WebSocketServerOptions>> settings;
	    public WebSocketServerBuilder()
	    {
	        this.settings = new List<Action<IServiceProvider, WebSocketServerOptions>>();
	        this.serviceProviderAction = () => default!;
	    }
	    public WebSocketServerBuilder ConfigureServer(Action<WebSocketServerOptions> configure)
	    {
	        return ConfigureServer((serviceProvider, options) =>
	        {
	            configure.Invoke(options);
	        });
	    }
	    public WebSocketServerBuilder ConfigureServer(Action<IServiceProvider, WebSocketServerOptions> configure)
	    {
	        if (configure is null)
	        {
	            throw new ArgumentNullException(nameof(configure));
	        }
	        settings.Add(configure);
	        return this;
	    }
	    public WebSocketServerBuilder ConfigureServer<T1>(Action<T1, WebSocketServerOptions> configure)
	    {
	        return ConfigureServer((serviceProvider, options) =>
	        {
	            configure.Invoke(serviceProvider.GetService(typeof(T1)) is T1 instance ? instance : default!, options);
	        });
	    }
	    public WebSocketServerBuilder ConfigureServer<T1, T2>(Action<T1, T2, WebSocketServerOptions> configure)
	    {
	        return ConfigureServer((serviceProvider, options) =>
	        {
	            var instance1 = default(T1)!;
	            var instance2 = default(T2)!;
	            if (serviceProvider.GetService(typeof(T1)) is T1 cast1)
	            {
	                instance1 = cast1;
	            }
	            if (serviceProvider.GetService(typeof(T2)) is T2 cast2)
	            {
	                instance2 = cast2;
	            }
	            configure.Invoke(instance1, instance2, options);
	        });
	    }
	    public WebSocketServerBuilder ConfigureServer<T1, T2, T3>(Action<T1, T2, T3, WebSocketServerOptions> configure)
	    {
	        return ConfigureServer((serviceProvider, options) =>
	        {
	            var instance1 = default(T1)!;
	            var instance2 = default(T2)!;
	            var instance3 = default(T3)!;
	            if (serviceProvider.GetService(typeof(T1)) is T1 cast1)
	            {
	                instance1 = cast1;
	            }
	            if (serviceProvider.GetService(typeof(T2)) is T2 cast2)
	            {
	                instance2 = cast2;
	            }
	            if (serviceProvider.GetService(typeof(T3)) is T3 cast3)
	            {
	                instance3 = cast3;
	            }
	            configure.Invoke(instance1, instance2, instance3, options);
	        });
	    }
	    public WebSocketServerBuilder ConfigureServer<T1, T2, T3, T4>(Action<T1, T2, T3, T4, WebSocketServerOptions> configure)
	    {
	        return ConfigureServer((serviceProvider, options) =>
	        {
	            var instance1 = default(T1)!;
	            var instance2 = default(T2)!;
	            var instance3 = default(T3)!;
	            var instance4 = default(T4)!;
	            if (serviceProvider.GetService(typeof(T1)) is T1 cast1)
	            {
	                instance1 = cast1;
	            }
	            if (serviceProvider.GetService(typeof(T2)) is T2 cast2)
	            {
	                instance2 = cast2;
	            }
	            if (serviceProvider.GetService(typeof(T3)) is T3 cast3)
	            {
	                instance3 = cast3;
	            }
	            if (serviceProvider.GetService(typeof(T4)) is T4 cast4)
	            {
	                instance4 = cast4;
	            }
	            configure.Invoke(instance1, instance2, instance3, instance4, options);
	        });
	    }
	    public WebSocketServerBuilder ConfigureServer<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5, WebSocketServerOptions> configure)
	    {
	        return ConfigureServer((serviceProvider, options) =>
	        {
	            var instance1 = default(T1)!;
	            var instance2 = default(T2)!;
	            var instance3 = default(T3)!;
	            var instance4 = default(T4)!;
	            var instance5 = default(T5)!;
	            if (serviceProvider.GetService(typeof(T1)) is T1 cast1)
	            {
	                instance1 = cast1;
	            }
	            if (serviceProvider.GetService(typeof(T2)) is T2 cast2)
	            {
	                instance2 = cast2;
	            }
	            if (serviceProvider.GetService(typeof(T3)) is T3 cast3)
	            {
	                instance3 = cast3;
	            }
	            if (serviceProvider.GetService(typeof(T4)) is T4 cast4)
	            {
	                instance4 = cast4;
	            }
	            if (serviceProvider.GetService(typeof(T5)) is T5 cast5)
	            {
	                instance5 = cast5;
	            }
	            configure.Invoke(instance1, instance2, instance3, instance4, instance5, options);
	        });
	    }
	    public WebSocketServerBuilder ConfigureServiceProvider(IServiceProvider serviceProvider)
	    {
	        if (serviceProvider is null)
	        {
	            throw new ArgumentNullException(nameof(serviceProvider));
	        }
	        this.serviceProviderAction = () => serviceProvider;
	        return this;
	    }
	    public WebSocketServerBuilder ConfigureServiceProvider(Func<IServiceProvider> configure)
	    {
	        if (configure is null)
	        {
	            throw new ArgumentNullException(nameof(configure));
	        }
	        this.serviceProviderAction = configure;
	        return this;
	    }
	    IHostService IHostServiceBuilder.Build()
	    {
	        var options = new WebSocketServerOptions();
	        var serviceProvider = serviceProviderAction.Invoke();
	        foreach (var setting in settings)
	        {
	            setting.Invoke(serviceProvider, options);
	        }
	        return new WebSocketServer(options);
	    }
	}
	public sealed class WebSocketServerOptions
	{
	    public WebSocketServerOptions()
	    {
	        this.Transports = new List<ITransport>();
	    }
	    internal List<ITransport> Transports { get; }
	    public string ServerName { get; set; } = "Cohesion.Net WebSocket Server";
	    public void AddTransport(ITransport transport)
	    {
	        ValidateTransport(transport);
	        this.Transports.Add(transport);
	    }
	    public void AddTransport(Func<ITransport> configure)
	    {
	        var transport = configure.Invoke();
	        ValidateTransport(transport);
	        this.Transports.Add(transport);
	    }
	    public void AddTcpTransport(Action<TcpServerTransportOptions> configure)
	    {
	        if (configure is null)
	        {
	            throw new ArgumentNullException(nameof(configure));
	        }
	        var options = new TcpServerTransportOptions();
	        configure.Invoke(options);
	        this.Transports.Add(new TcpServerTransport(options));
	    }
	    private void ValidateTransport(ITransport transport)
	    {
	        if (transport is null)
	        {
	            throw new ArgumentNullException(nameof(transport));
	        }
	        if (transport.TransportType == TransportType.Client)
	        {
	            throw new ArgumentException("Transport must be a server configure.", nameof(transport));
	        }
	        if (transport.ProtocolType != ProtocolType.Tcp && transport.ProtocolType != ProtocolType.Udp)
	        {
	            throw new ArgumentException("Transport must be a TCP or QUIC configure.", nameof(transport));
	        }
	    }
	}
	#endregion
	#region \Server\Abstractions
	public interface IWebSocketContext
	{
	}
	public interface IWebSocketExecutor
	{
	}
	#endregion
	#region \Server\Extensions
	public static class HostBuilderExtensions
	{
	    public static IHostBuilder AddWebSocketServer(this IHostBuilder builder, Action<WebSocketServerBuilder> configure)
	    {
	        if (configure is null)
	        {
	            throw new ArgumentNullException(nameof(configure));
	        }
	        var serverBuilder = new WebSocketServerBuilder();
	        configure.Invoke(serverBuilder);
	        return builder.AddService(((IHostServiceBuilder)serverBuilder).Build());
	    }
	}
	#endregion
}
#endregion
