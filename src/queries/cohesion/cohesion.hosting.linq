<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	await HostBuilder.Create(options =>
	{
		options.ServiceStartupTimeout = TimeSpan.FromSeconds(2);
	})
		.AddService(context => new TestService(context))
		.AddService(context => new TestService(context))
		.Build()
		.RunAsync();
}

public class TestService : IHostService
{
	private CancellationTokenSource stoppingCts;
	private Task executing;
	
	private readonly IHostContext context;
	
	public TestService(IHostContext context)
	{
		this.context = context;
	}
	
	
	public async Task StartAsync(CancellationToken cancellationToken)
	{
		stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
		
		executing = ExecuteAsync(stoppingCts.Token);
		
		if (executing.IsCompleted)
		{
			return;
		}
		
		await Task.Delay(3000);
		
		//return Task.CompletedTask;
	}

	public async Task StopAsync(CancellationToken cancellationToken)
	{
		// Stop called without start
		if (executing == null)
		{
			return;
		}

		try
		{
			// Signal cancellation to the executing method
			stoppingCts!.Cancel();
		}
		finally
		{
			// Wait until the task completes or the stop token triggers
			var tcs = new TaskCompletionSource<object>();
			using CancellationTokenRegistration registration = cancellationToken.Register(s => ((TaskCompletionSource<object>)s!).SetCanceled(), tcs);
			// Do not await the _executeTask because cancelling it will throw an OperationCanceledException which we are explicitly ignoring
			await Task.WhenAny(executing, tcs.Task).ConfigureAwait(false);
		}
	}
	
	long timespan;

	protected virtual async Task ExecuteAsync(CancellationToken cancellationToken) {
		while (true)
		{
			Console.WriteLine($"Test Service - {this.GetHashCode()} is running");
			await Task.Delay(2000);
			timespan += 2000;
			timespan.Dump();
			if (cancellationToken.IsCancellationRequested|| timespan > (2000 * 3))
			{
				context.Shutdown();
				break;
			}
		}
	}
}



#region Abstractions
/// <summary>
/// A application host.
/// </summary>
public interface IHost : IDisposable, IAsyncDisposable
{
	/// <summary>
	/// The Host Context.
	/// </summary>
	IHostContext Context { get; }
	/// <summary>
	/// Starts all the services in the host
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task RunAsync(CancellationToken cancellationToken = default);
}
/// <summary>
/// Host Information.
/// </summary>
public interface IHostContext
{
	/// <summary>
	/// 
	/// </summary>
	string ContentRootPath { get; set; }
	/// <summary>
	/// 
	/// </summary>
	IHostEnvironment Environment { get; }
	/// <summary>
	/// A collection of hosted services.
	/// </summary>
	IEnumerable<IHostService> HostedServices { get; }
	/// <summary>
	/// The Host Service Provider.
	/// </summary>
	IServiceProvider? ServiceProvider { get; }
	/// <summary>
	/// Signals the host to shutdown
	/// </summary>
	void Shutdown();
}
/// <summary>
/// 
/// </summary>
public interface IHostEnvironment
{
	/// <summary>
	/// The name of the environment
	/// </summary>
	string? Name { get; set; }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="environment"></param>
	/// <returns></returns>
	bool IsEnvironment(string? environment);
}
/// <summary>
/// A builder pattern for creating a <see cref="IHost"/>.
/// </summary>
public interface IHostBuilder
{
	/// <summary>
	/// Adds a service to host to be started.
	/// </summary>
	/// <param name="service">The service managed by the host.</param>
	/// <returns>The same instance of <see cref="IHostBuilder"/></returns>
	IHostBuilder AddService(IHostService service);
	/// <summary>
	/// 
	/// </summary>
	/// <param name="configure"></param>
	/// <returns></returns>
	IHostBuilder AddService(Func<IHostContext, IHostService> configure);
	/// <summary>
	/// Adds a <see cref="IServiceProvider"/> to the host context.
	/// </summary>
	/// <param name="serviceProvider"></param>
	/// <returns></returns>
	IHostBuilder AddServiceProvider(IServiceProvider serviceProvider);
	/// <summary>
	/// Builds the <see cref="IHost"/>.
	/// </summary>
	/// <returns></returns>
	IHost Build();
}
/// <summary>
/// 
/// </summary>
public interface IHostLifecycleService : IHostService
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task StartingAsync(CancellationToken cancellationToken);
	/// <summary>
	/// 
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task StartedAsync(CancellationToken cancellationToken);
	/// <summary>
	/// 
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task StoppingAsync(CancellationToken cancellationToken);
	/// <summary>
	/// 
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task StoppedAsync(CancellationToken cancellationToken);
}
/// <summary>
/// A managed host service.
/// </summary>
public interface IHostService
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task StartAsync(CancellationToken cancellationToken);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task StopAsync(CancellationToken cancellationToken);
}
/// <summary>
/// 
/// </summary>
public interface IHostServiceBuilder
{
	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	IHostService Build();
}
#endregion

#region Exceptions
/// <summary>
/// 
/// </summary>
public abstract class HostException : Exception
{
	public HostException(string message) : base(message) { }
	public HostException(string message, Exception innerException) : base(message, innerException) { }
}
internal sealed class InvalidHostBuildException : HostException
{
	public InvalidHostBuildException(string message) : base(message)
	{
	}
}

#endregion

#region Implementation
/// <summary>
/// The default host options.
/// </summary>
public sealed class HostOptions
{
	/// <summary>
	/// Specify whether the services should be started concurrently.
	/// </summary>
	public bool StartServicesConcurrently { get; set; }
	/// <summary>
	/// Specify the timeout for each service startup. Default is 0.
	/// </summary>
	public TimeSpan? ServiceStartupTimeout { get; set; }
	/// <summary>
	/// 
	/// </summary>
	public TimeSpan? ServiceShutdownTimeout { get; set; }
}
public sealed class HostBuilder : IHostBuilder
{
	private static Host? instance;
	
	private readonly HostOptions options;
	private readonly List<Action<HostContext>> onServiceAdd = new();
	private readonly List<Action<HostContext>> onServiceProviderAdd = new();

	public HostBuilder(HostOptions options)
	{
		if (options is null)
		{
			throw new ArgumentNullException(nameof(options));
		}
		this.options = options;
	}
	
	/// <summary>
	/// A singleton instance of the built host.
	/// </summary>
	public static IHost? Instance => instance;

	/// <inheritdoc/>
	public IHostBuilder AddService(IHostService service)
	{
		if (service is null)
		{
			throw new ArgumentNullException(nameof(service));
		}
		onServiceAdd.Add(context =>
		{
			context.HostedServices.Add(service);	
		});
		return this;
	}

	/// <inheritdoc/>
	public IHostBuilder AddService(Func<IHostContext, IHostService> configure) 
	{
		if (configure is null)
		{
			throw new ArgumentNullException(nameof(configure));
		}
		onServiceAdd.Add(context =>
		{
			context.HostedServices.Add(configure.Invoke(context));
		});
		return this;
	}

	/// <inheritdoc/>
	public IHostBuilder AddServiceProvider(IServiceProvider serviceProvider)
	{
		if (serviceProvider is null)
		{
			throw new ArgumentNullException(nameof(serviceProvider));
		}
		onServiceProviderAdd.Add(context =>
		{
			context.ServiceProvider = serviceProvider;
		});
		return this;
	}


	/// <inheritdoc/>
	public IHost Build()
	{
		if (instance is not null)
		{
			return instance;
			throw new InvalidHostBuildException("The host has already been built.");
		}
		
		instance = new Host(options);

		OnBuild(onServiceProviderAdd);
		OnBuild(onServiceAdd);

		return instance;
	}
	
	
	private void OnBuild(IList<Action<HostContext>> actions)
	{
		foreach (var action in actions)
		{
			action.Invoke(instance.Context);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public static IHostBuilder Create() => new HostBuilder(new());
	/// <summary>
	/// 
	/// </summary>
	public static IHostBuilder Create(Action<HostOptions> configure) 
	{
		if (configure is null)
		{
			throw new ArgumentNullException(nameof(configure));
		}
		var options = new HostOptions();
		
		configure.Invoke(options);
		
		return new HostBuilder(options);
	}

	
	//public static IHostBuilder Create(string[] args) => new HostBuilder();
}
internal sealed class Host : IHost
{
	private readonly HostOptions options;

	public Host(HostOptions options)
	{
		this.options = options;
	}
	
	public HostContext Context { get; } = new();

	IHostContext IHost.Context => Context;

	public async Task RunAsync(CancellationToken cancellationToken = default)
	{
		// Let's controle the task completion of 'RunAsync()` by manually setting the 
		// results when Cancellation is Requested
		var taskCompletionSource = new TaskCompletionSource<Host>(TaskCreationOptions.RunContinuationsAsynchronously);
		
		// Create a cancellation token source to pass
		var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
		
		// Set the Shutdown handle
		Context.ShutdownCallback = () => 
		{
			cancellationTokenSource.Cancel();	
		};
		
		// Let's register a callback to compelete the task 
		cancellationTokenSource.Token.Register(state =>
		{
			var source = (TaskCompletionSource<Host>)state!;
			
			source.SetResult(this);

		}, taskCompletionSource);

		await StartAsync(cancellationTokenSource.Token).ConfigureAwait(false);

		await taskCompletionSource.Task.ConfigureAwait(false);

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

		shutdownCancellationToken.Register(() =>
		{
			// TODO: Need to change implementation for safer shutdown process
			Context.Shutdown();
		});

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
		DisposeAsync().GetAwaiter().GetResult();
	}

	public async ValueTask DisposeAsync()
	{
		

		await Task.CompletedTask;
	}
}
internal sealed class HostContext : IHostContext
{
	public Action? ShutdownCallback { get; set; }
	
	public List<IHostService> HostedServices { get; init; } = new();

	public string ContentRootPath { get; set; } 

	public IHostEnvironment Environment{ get; set; } 

	public IServiceProvider? ServiceProvider { get; set; }

	IEnumerable<IHostService> IHostContext.HostedServices => HostedServices;

	public void Shutdown()
	{
		if (ShutdownCallback is null)
		{
			throw new InvalidOperationException("Host has not started.");
		}
		ShutdownCallback.Invoke();
	}
}

internal class HostEnvironment : IHostEnvironment
{
	public string? Name { get; set; }
	public bool IsEnvironment(string? environment)
	{
		return string.Equals(Name, environment, StringComparison.OrdinalIgnoreCase);
	}
}

#endregion