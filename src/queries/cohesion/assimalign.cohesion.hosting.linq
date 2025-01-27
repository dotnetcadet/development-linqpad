<Query Kind="Program">
<Namespace>System</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>Assimalign.Cohesion.Hosting.Internal</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>System.Threading.Tasks</Namespace>
<Namespace>System.Threading</Namespace>
<Namespace>System.Reflection</Namespace>
<Namespace>System.Diagnostics.CodeAnalysis</Namespace>
<Namespace>System.Runtime.CompilerServices</Namespace>
</Query>
#load ".\assimalign.cohesion.core"

void Main()
{

}

#region Assimalign.Cohesion.Hosting(net8.0)
namespace Assimalign.Cohesion.Hosting
{
	#region \
	public sealed class HostBuilder : IHostBuilder
	{
	    private bool isBuilt;
	    private readonly HostOptions options;
	    private readonly List<Action<HostContext>> onServiceAdd = new();
	    private readonly List<Action<HostContext>> onServiceProviderAdd = new();
	    public HostBuilder(HostOptions options)
	    {
	        if (options is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(options));
	        }
	        this.options = options;
	    }
	    public IHostBuilder AddService(IHostService service)
	    {
	        if (service is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(service));
	        }
	        onServiceAdd.Add(context =>
	        {
	            context.HostedServices.Add(service);
	        });
	        return this;
	    }
	    public IHostBuilder AddService(Func<IHostContext, IHostService> configure)
	    {
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        onServiceAdd.Add(context =>
	        {
	            context.HostedServices.Add(configure.Invoke(context));
	        });
	        return this;
	    }
	    public IHostBuilder AddServiceProvider(IServiceProvider serviceProvider)
	    {
	        if (serviceProvider is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(serviceProvider));
	        }
	        onServiceProviderAdd.Add(context =>
	        {
	            context.ServiceProvider = serviceProvider;
	        });
	        return this;
	    }
	    public IHostBuilder AddServiceProvider(Func<IHostContext, IServiceProvider> method)
	    {
	        if (method is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(method));
	        }
	        onServiceProviderAdd.Add(context =>
	        {
	            context.ServiceProvider = method.Invoke(context);
	        });
	        return this;
	    }
	    public IHost Build()
	    {
	        if (isBuilt == true)
	        {
	            ThrowHelper.ThrowInvalidOperationException("The host has already been built.");
	        }
	        var host = new Host(options);
	        OnBuild(host, onServiceProviderAdd);
	        OnBuild(host, onServiceAdd);
	        isBuilt = true;
	        return host;
	    }
	    private void OnBuild(Host host, IList<Action<HostContext>> actions)
	    {
	        foreach (var action in actions)
	        {
	            action.Invoke(host.Context);
	        }
	    }
	    public static IHostBuilder Create() => new HostBuilder(new());
	    public static IHostBuilder Create(Action<HostOptions> configure)
	    {
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        var options = new HostOptions();
	        configure.Invoke(options);
	        return new HostBuilder(options);
	    }
	}
	public sealed class HostOptions
	{
	    public bool StartServicesConcurrently { get; set; }
	    public TimeSpan? ServiceStartupTimeout { get; set; }
	    public TimeSpan? ServiceShutdownTimeout { get; set; }
	    public string? Environment { get; set; } = AppEnvironment.GetEnvironmentName();
	    internal Action<IHostContext> Trace { get; set; } = trace => { };
	    public void OnTrace(Action<IHostContext> action)
	    {
	        if (action is null)
	        {
	            throw new ArgumentNullException(nameof(action));
	        }
	        Trace = action;
	    }
	    public void OnTrace<T>(Action<T, HostTrace> action)
	    {
	    }
	}
	class HostServiceWrapper
	{
	}
	public enum HostState
	{
	    Unknown = 0,
	    Starting,
	    Running,
	    Stopping
	}
	public sealed class HostTrace
	{
	    public Guid? Id { get; } = Guid.NewGuid();
	    public string? Message { get; init; }
	}
	#endregion
	#region \Abstractions
	public interface IHost : IDisposable
	{
	    IHostContext Context { get; }
	    Task RunAsync(CancellationToken cancellationToken = default);
	}
	public interface IHostBuilder
	{
	    IHostBuilder AddService(IHostService service);
	    IHostBuilder AddService(Func<IHostContext, IHostService> configure);
	    IHostBuilder AddServiceProvider(IServiceProvider serviceProvider);
	    IHostBuilder AddServiceProvider(Func<IHostContext, IServiceProvider> serviceProvider);
	    IHost Build();
	}
	public interface IHostContext
	{
	    HostState State { get; }
	    string? ContentRootPath { get; }
	    IHostEnvironment Environment { get; }
	    IEnumerable<IHostService> HostedServices { get; }
	    IServiceProvider? ServiceProvider { get; }
	    void Shutdown();
	}
	public interface IHostEnvironment
	{
	    string? Name { get; }
	    bool IsEnvironment(string? environment);
	}
	public interface IHostLifecycleService : IHostService
	{
	    Task StartingAsync(CancellationToken cancellationToken);
	    Task StartedAsync(CancellationToken cancellationToken);
	    Task StoppingAsync(CancellationToken cancellationToken);
	    Task StoppedAsync(CancellationToken cancellationToken);
	}
	public interface IHostService
	{
	    Task StartAsync(CancellationToken cancellationToken);
	    Task StopAsync(CancellationToken cancellationToken);
	}
	public interface IHostServiceBuilder
	{
	    public IHostService Build();
	}
	#endregion
	#region \Exceptions
	public abstract class HostException : Exception
	{
		public HostException(string message): base(message) { }
		public HostException(string message, Exception innerException)  : base(message, innerException) { }
	}
	#endregion
	#region \Extensions
	public static class HostExtensions
	{
	    public static void Run(this IHost host)
	    {
	        host.RunAsync().GetAwaiter().GetResult();
	    }
	}
	public static class HostBuilderExtensions
	{
	    public static IHostBuilder AddService(this IHostBuilder builder, Func<IServiceProvider, IHostService> method)
	    {
	        return builder.AddService(context =>
	        {
	            if (context.ServiceProvider is null)
	            {
	                ThrowHelper.ThrowInvalidOperationException("No Service provider was created for the host.");
	            }
	            return method.Invoke(context.ServiceProvider);
	        });
	    }
	    public static IHostBuilder AddService<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]TService>(this IHostBuilder builder) 
	        where TService : IHostService
	    {
	        return builder.AddService(context =>
	        {
	            if (context.ServiceProvider is null)
	            {
	                ThrowHelper.ThrowInvalidOperationException("");
	            }
	            var serviceProvider = context.ServiceProvider;
	            var type = typeof(TService);
	            var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
	            // If using DI to instantiate the hosted service the type can only have one public constructure.
	            if (constructors.Length > 1)
	            {
	                // TODO: Throw exception;
	            }
	            var constructor = constructors[0];
	            var parameters = constructor.GetParameters();
	            // If only a public parameterless constructure then return instance
	            if (parameters.Length == 0)
	            {
	                return Activator.CreateInstance<TService>();
	            }
	            var arguments = new object[parameters.Length];
	            for (int i = 0; i < parameters.Length; i++)
	            {
	                var argument = serviceProvider.GetService(parameters[i].ParameterType);
	                if (argument is null)
	                {
	                    throw new Exception("Unable to resolve service");
	                }
	                arguments[i] = argument;
	            }
	            var instance = Activator.CreateInstance(type, arguments);
	            if (instance is null || instance is not TService service)
	            {
	                throw new Exception();
	            }
	            return service;
	        });
	    }
	}
	public static class HostEnvironmentExtensions
	{
	    private static readonly string development = nameof(development);
	    private static readonly string staging = nameof(staging);
	    private static readonly string test = nameof(test);
	    private static readonly string production = nameof(production);
	    private static readonly string uat = nameof(uat);
	    private static readonly string qa = nameof(qa);
	    public static bool IsDevelopment(this IHostEnvironment environment)
	    {
	        ThrowIfNull(environment);
	        return environment.IsEnvironment(development);
	    }
	    public static bool IsStaging(this IHostEnvironment environment)
	    {
	        ThrowIfNull(environment);
	        return environment.IsEnvironment(staging);
	    }
	    public static bool IsTest(this IHostEnvironment environment)
	    {
	        ThrowIfNull(environment);
	        return environment.IsEnvironment(test);
	    }
	    public static bool IsProduction(this IHostEnvironment environment)
	    {
	        ThrowIfNull(environment);
	        return environment.IsEnvironment(production);
	    }
	    public static bool IsUat(this IHostEnvironment environment)
	    {
	        ThrowIfNull(environment);
	        return environment.IsEnvironment(uat);
	    }
	    public static bool IsQa(this IHostEnvironment environment)
	    {
	        ThrowIfNull(environment);
	        return environment.IsEnvironment(qa);
	    }
	    private static void ThrowIfNull(IHostEnvironment environment)
	    {
	        if (environment is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(environment));
	        }
	    }
	}
	#endregion
	#region \Internal
	internal sealed class Host : IHost
	{
	    private readonly HostOptions options;
	    public Host(HostOptions options)
	    {
	        this.options = options;
	        this.Context = new()
	        {
	            Environment = new HostEnvironment()
	            {
	                Name = options.Environment
	            }
	        };
	    }
	    public HostContext Context { get; }
	    IHostContext IHost.Context => Context;
	    public async Task RunAsync(CancellationToken cancellationToken = default)
	    {
	        // Let's control the task completion of 'RunAsync()` by manually setting the 
	        // results when Cancellation is Requested
	        var taskCompletionSource = new TaskCompletionSource<Host>(TaskCreationOptions.RunContinuationsAsynchronously);
	        // Create a cancellation token source to pass
	        var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
	        // Set the Shutdown handle
	        Context.ShutdownCallback = () =>
	        {
	            cancellationTokenSource.Cancel();
	        };
	        // Let's register a callback to complete the task 
	        cancellationTokenSource.Token.Register(state =>
	        {
	            options.Trace(Context);
	            var source = (TaskCompletionSource<Host>)state!;
	            source.SetResult(this);
	        }, taskCompletionSource);
	        Context.State = HostState.Starting;
	        // Begin trace
	        options.Trace(Context);
	        await StartAsync(cancellationTokenSource.Token).ConfigureAwait(false);
	        Context.State = HostState.Running;
	        options.Trace(Context);
	        await taskCompletionSource.Task.ConfigureAwait(false);
	        Context.State = HostState.Stopping;
	        options.Trace(Context);
	        await StopAsync(cancellationTokenSource.Token).ConfigureAwait(false);
	    }
	    private async Task StartAsync(CancellationToken cancellationToken)
	    {
	        var startCancellationToken = cancellationToken;
	        if (options.ServiceStartupTimeout is not null)
	        {
	            var timeoutCancellationTokenSource = new CancellationTokenSource(options.ServiceStartupTimeout.Value);
	            var linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
	                timeoutCancellationTokenSource.Token,
	                cancellationToken);
	            startCancellationToken = linkedCancellationTokenSource.Token;
	        }
	        startCancellationToken.Register(() =>
	        {
	            // TODO: Need to change implementation for safer shutdown process
	            Context.Shutdown();
	        });
	        var services = Context.HostedServices;
	        if (options.StartServicesConcurrently)
	        {
	            var tasks = new List<Task>();
	            for (int i = 0; i < services.Count; i++)
	            {
	                var service = services[i];
	                if (service is IHostLifecycleService lifecycleService)
	                {
	                    tasks.Add(Task.Run(async () =>
	                    {
	                        await lifecycleService.StartingAsync(startCancellationToken).ConfigureAwait(false);
	                        await lifecycleService.StartAsync(startCancellationToken).ConfigureAwait(false);
	                        await lifecycleService.StartedAsync(startCancellationToken).ConfigureAwait(false);
	                    }));
	                }
	                else
	                {
	                    tasks.Add(service.StartAsync(startCancellationToken));
	                }
	            }
	            await Task.WhenAll(tasks);
	        }
	        else
	        {
	            for (int i = 0; i < services.Count; i++)
	            {
	                var service = services[i];
	                if (service is IHostLifecycleService lifecycleService)
	                {
	                    await lifecycleService.StartingAsync(startCancellationToken).ConfigureAwait(false);
	                    await lifecycleService.StartAsync(startCancellationToken).ConfigureAwait(false);
	                    await lifecycleService.StartedAsync(startCancellationToken).ConfigureAwait(false);
	                }
	                else
	                {
	                    await service.StartAsync(startCancellationToken).ConfigureAwait(false);
	                }
	            }
	        }
	    }
	    private async Task StopAsync(CancellationToken cancellationToken)
	    {
	        var shutdownCancellationToken = cancellationToken;
	        if (options.ServiceShutdownTimeout is not null)
	        {
	            var timeoutCancellationTokenSource = new CancellationTokenSource(options.ServiceShutdownTimeout.Value);
	            var linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
	                timeoutCancellationTokenSource.Token,
	                cancellationToken);
	            shutdownCancellationToken = linkedCancellationTokenSource.Token;
	        }
	        var services = Context.HostedServices;
	        for (int i = 0; i < services.Count; i++)
	        {
	            var service = services[i];
	            if (service is IHostLifecycleService lifecycleService)
	            {
	                await lifecycleService.StoppingAsync(shutdownCancellationToken).ConfigureAwait(false);
	                await lifecycleService.StopAsync(shutdownCancellationToken).ConfigureAwait(false);
	                await lifecycleService.StoppedAsync(shutdownCancellationToken).ConfigureAwait(false);
	            }
	            else
	            {
	                await service.StopAsync(shutdownCancellationToken).ConfigureAwait(false);
	            }
	        }
	    }
	    public void Dispose()
	    {
	    }
	}
	internal sealed class HostContext : IHostContext
	{
	    private object stateLock = new object();
	    private HostState state;
	    public HostState State
	    {
	        get
	        {
	            lock (stateLock)
	            {
	                return state;
	            }
	        }
	        set => state = value;
	    }
	    public Action? ShutdownCallback { get; set; }
	    public string? ContentRootPath { get; set; }
	    public IHostEnvironment Environment { get; init; } = default!;
	    public IServiceProvider? ServiceProvider { get; set; }
	    public List<IHostService> HostedServices { get; init; } = new();
	    IEnumerable<IHostService> IHostContext.HostedServices => HostedServices;
	    public void Shutdown()
	    {
	        if (ShutdownCallback is null)
	        {
	            ThrowHelper.ThrowInvalidOperationException("Host has not started.");
	        }
	        ShutdownCallback.Invoke();
	    }
	}
	internal class HostEnvironment : IHostEnvironment
	{
	    public string? Name { get; init; }
	    public bool IsEnvironment(string? environment)
	    {
	        return string.Equals(Name, environment, StringComparison.OrdinalIgnoreCase);
	    }
	}
	#endregion
	#region \Internal\Exceptions
	internal class BadStartException : HostException
	{
	    public BadStartException(string message, Exception innerException) : base(message, innerException)
	    {
	    }
	}
	internal sealed class InvalidHostBuildException : HostException
	{
	    public InvalidHostBuildException(string message) : base(message)
	    {
	    }
	}
	#endregion
	#region \Internal\Utilities
	internal static class ThrowHelper
	{
	    [DoesNotReturn]
	    internal static void ThrowInvalidOperationException(string message) =>
	        throw new InvalidOperationException(message);
	    [DoesNotReturn]
	    internal static void ThrowArgumentNullException(string paramName) =>
	        throw new ArgumentNullException(paramName);
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
	#region \Properties
	#endregion
}
#endregion
