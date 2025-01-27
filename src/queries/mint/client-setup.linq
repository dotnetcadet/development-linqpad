<Query Kind="Program">
  <NuGetReference>Azure.Identity</NuGetReference>
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Logging.Console</NuGetReference>
  <NuGetReference Prerelease="true">Microsoft.Graph.Beta</NuGetReference>
  <Namespace>Azure.Identity</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>Microsoft.Extensions.Logging.Console</Namespace>
  <Namespace>Microsoft.Graph.Beta</Namespace>
  <Namespace>Microsoft.Graph.Beta.Models</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <RemoveNamespace>System.Text.RegularExpressions</RemoveNamespace>
</Query>

using static System.Console;


const string ClientId = "";
const string ClientSecret = "";
const string TenantId = "14e9b5bc-c1d0-45dd-8a3b-554ca1a622ad";

async Task Main()
{
	var serviceProvider = new ServiceCollection()
		.AddLogging(builder =>
		{
			builder.AddSimpleConsole(options =>
			{
				options.ColorBehavior = LoggerColorBehavior.Enabled;
				options.IncludeScopes = true;
				options.SingleLine = false;
			});
		})
		.AddSingleton<GraphServiceClient>(serviceProvider =>
		{
			var options = new ClientSecretCredentialOptions
			{
				AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
			};

			var credential = new ClientSecretCredential(
				TenantId,
				ClientId,
				ClientSecret,
				options);

			return new GraphServiceClient(
				credential,
				["https://graph.microsoft.com/.default"]);
		})
		.AddSingleton<IStep, Step1>()
		.AddSingleton<IStep, Step2>()
		.AddSingleton<IStep, AzureGroupUpcertStep>()
		.AddSingleton<IStepExecutor, StepExecutor>()
		.BuildServiceProvider();



	var client = serviceProvider.GetRequiredService<GraphServiceClient>();


	var servicePrincipals = await client.ServicePrincipals.GetAsync(options =>
	{
		options.QueryParameters.Filter = "startsWith(DisplayName, 'mint')";
	});
	
	foreach (var sp in servicePrincipals.Value)
	{
		sp.
	}


	servicePrincipals?.Value?.Dump();

	//	var executor = serviceProvider.GetRequiredService<IStepExecutor>();
	//	
	//	
	//	
	//	
	//
	//	var context = new StepContex(new KeyValuePair<string, object>[]
	//	{
	//		new KeyValuePair<string, object>(StepStateKey.ClientName, "mint")
	//	});
	//
	//	await executor.ExecuteAsync(context);







	// using Azure.Identity;




	//var user = await client.Users.PostAsync(new()
	//{
	//	DisplayName = $"Chase Crawford",
	//	GivenName = "Chase",
	//	Surname = "Crawford",
	//	PasswordProfile = new()
	//	{
	//		ForceChangePasswordNextSignIn =true,
	//		Password = "testpwd234$"
	//	},
	//	Identities = new()
	//					{
	//						new()
	//						{
	//							SignInType = "emailAddress",
	//							Issuer = "v3techdev.onmicrosoft.com",
	//							IssuerAssignedId = "chasecrawford2018@gmail.com"
	//						}
	//					}
	//});



	//var user = await client.Users.GetAsync(options =>
	//{
	//	options.QueryParameters.Filter = "userPrincipalName eq 'chasecrawford2018_gmail.com@v3techdev.onmicrosoft.com'";
	//});


}

#region Abstactions
public interface IStep
{
	int Order { get; }
	string Name { get; }
	Task ExecuteAsync(IStepContext context, StepHandler next, CancellationToken cancellationToken = default);
}
public interface IStepContext
{
	T Get<T>(string key);
	T Set<T>(string key, T state);
}
public interface IStepExecutor
{
	Task ExecuteAsync(IStepContext context, CancellationToken cancellationToken = default);
}
public delegate Task StepHandler(IStepContext context, CancellationToken cancellationToken = default);
#endregion

#region Implementation
public abstract class Step<TStep> : IStep where TStep : IStep
{
	public virtual int Order { get; }

	public virtual string Name
	{
		get
		{
			var index = 0;
			var nameOf = nameof(TStep);
			var name = string.Empty;

			for (var i = 0; i < nameOf.Length; i++)
			{
				if (i > 0 && char.IsAsciiLetterUpper(nameOf[i]))
				{
					name = name + " " + nameOf.Substring(index, i - index);
					index = i;
				}
			}

			return name;
		}
	}
	public abstract Task ExecuteAsync(IStepContext context, StepHandler next, CancellationToken cancellationToken = default);
}
public sealed class StepExecutor : IStepExecutor, IEnumerator<IStep>
{
	private readonly IEnumerator<IStep> steps;
	private readonly ILogger logger;


	public StepExecutor(IEnumerable<IStep> steps, ILogger<StepExecutor> logger)
	{
		this.steps = steps.OrderBy(p => p.Order).GetEnumerator();
		this.logger = logger;
	}

	public IStep Current => steps.Current;
	object IEnumerator.Current => this.Current;

	public Task ExecuteAsync(IStepContext context, CancellationToken cancellationToken = default)
	{
		using (logger.BeginScope("Starting.."))
		{
			return OnHandleAsync(context, cancellationToken);
		}
	}

	private Task OnHandleAsync(IStepContext context, CancellationToken cancellationToken = default)
	{
		if (Current is not null)
		{
			//ForegroundColor = ConsoleColor.Blue;
			//WriteLine("		Completed");
			//WriteLine();
		}
		if (MoveNext())
		{
			//logger.LogInformation($"	Step {Current!.Order.ToString().PadLeft(2, '0')}: {Current!.Name}");

			using (logger.BeginScope($"	Step {Current!.Order.ToString().PadLeft(2, '0')}: {Current!.Name}"))
			{
				return Current.ExecuteAsync(context, new StepHandler(OnHandleAsync), cancellationToken);
			}
			//ForegroundColor = ConsoleColor.Blue;
			//WriteLine($"	Step {Current!.Order.ToString().PadLeft(2, '0')}: {Current!.Name}");
			//ForegroundColor = ConsoleColor.Magenta;


		}

		return Task.CompletedTask;
	}


	public bool MoveNext()
	{
		return steps.MoveNext();
	}

	public void Reset()
	{
		steps.Reset();
	}

	public void Dispose()
	{
		steps.Dispose();
	}
}
public sealed class StepContex : IStepContext
{
	private readonly Dictionary<string, object> state;

	public StepContex()
	{
		state = new(StringComparer.OrdinalIgnoreCase);
	}

	public StepContex(IEnumerable<KeyValuePair<string, object>> values)
	{
		state = new(values, StringComparer.OrdinalIgnoreCase);
	}

	public T Get<T>(string key)
	{
		return (T)state[key];
	}

	public T Set<T>(string key, T state)
	{
		return (T)(this.state[key] = state!);
	}
}
public sealed class StepStateKey
{
	public const string ClientName = "client-name";
	public const string ClientPrefix = "client-prefix";
	public const string ClientUiApp = "client-ui-app";
	public const string ClientApiApp = "client-api-app";
}
public sealed class StepExecutorBuilder
{
	private readonly IServiceCollection services;

	public StepExecutorBuilder(IServiceCollection services)
	{
		this.services = services;
	}




	//public IStepExecutor Build()
	//{
	//	
	//}
}
public sealed class StepLoggerProvider : ILoggerProvider
{
	public ILogger CreateLogger(string categoryName)
	{
		throw new NotImplementedException();
	}

	public void Dispose()
	{
		throw new NotImplementedException();
	}

	partial class StepLogger : ILogger
	{
		public StepLogger()
		{

		}
		public IDisposable? BeginScope<TState>(TState state) where TState : notnull
		{
			throw new NotImplementedException();
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return true;
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
		{
			throw new NotImplementedException();
		}
	}
}
#endregion



public class Step1 : Step<Step1>
{



	public override Task ExecuteAsync(IStepContext context, StepHandler next, CancellationToken cancellationToken = default)
	{
		return next.Invoke(context, cancellationToken);
	}
}

public class Step2 : Step<Step1>
{
	public override Task ExecuteAsync(IStepContext context, StepHandler next, CancellationToken cancellationToken = default)
	{
		return next.Invoke(context, cancellationToken);
	}
}


public class AzureGroupUpcertStep : IStep
{
	private readonly GraphServiceClient client;
	private readonly ILogger logger;

	public AzureGroupUpcertStep(GraphServiceClient client, ILogger<StepExecutor> logger)
	{
		this.client = client;
		this.logger = logger;
	}

	public int Order => 3;

	public string Name => "Azure Entra Group Upcert";

	public static string[] Groups =
	[
		"@Client MSI App Configuration",
		"@Client MSI Cosmos",
		"@Client MSI Directory Readers",
		"@Client MSI Event Grid",
		"@Client MSI Key Vault",
		"@Client MSI Notification Hub",
		"@Client MSI Service Bus",
		"@Client MSI SQL Server",
		"@Client MSI Storage Account",
		"V3 Technology Admins",
		"V3 Technology AppDev",
		"V3 Technology DevOps",
		"V3 Technology SQL Admins"

	];

	private string ConvertToPascalCase(string value)
	{
		return value.Substring(0, 1).ToUpper() + value.Substring(1, value.Length - 1);
	}

	public async Task ExecuteAsync(IStepContext context, StepHandler next, CancellationToken cancellationToken = default)
	{
		try
		{
			var groups = new List<Group>();

			var clientName = context.Get<string>(StepStateKey.ClientName);
			var names = Groups.Select(group =>
			{
				return group.Replace("@Client", ConvertToPascalCase(clientName));
			});

			logger.LogInformation("Task 1: Quering Groups");
			var groupCollection = await client.Groups.GetAsync(options =>
			{
				options.QueryParameters.Top = Groups.Length;
				options.QueryParameters.Filter = $"DisplayName in ({string.Join(',', names.Select(p => "'" + p + "'"))})"; ;
			});

			logger.LogInformation("(Complete)");
			logger.LogInformation($"Groups Found: {groupCollection?.Value?.Count ?? 0}");

			logger.LogInformation("		Task 2: Cross-References existing groups");
			foreach (var name in names)
			{
				var group = groupCollection?.Value?.FirstOrDefault(p => !string.IsNullOrEmpty(p.DisplayName) && p.DisplayName.Equals(name, StringComparison.OrdinalIgnoreCase));

				if (group is null)
				{
					logger.LogInformation($"			- Group: {name}");

					logger.LogInformation("			  (Created)");
				}
				else
				{
					WriteLine($"		- Group: {name}");
				}
			}
		}
		catch (Exception exception)
		{

		}

	}
}