<Query Kind="Program">
  <NuGetReference>Microsoft.Azure.Cosmos</NuGetReference>
  <Namespace>Microsoft.Azure.Cosmos</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

#load "patterns\repository.pattern.abstractions"

using Microsoft.Azure.Cosmos.Linq;

void Main()
{
	var options = new CosmosRepositoryOptions();

	options.ConfigureClient(client =>
	{
		client.SerializerOptions = new CosmosSerializationOptions()
		{
			IgnoreNullValues = true,
			PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
		}
	});
}

public sealed class CosmosRepositoryOptions
{
	public string Connection { get; set; }
	public string Database { get; set; }
	public string Container { get; set; }

	internal IList<Action> OnFailure
	internal CosmosClientOptions ClientOptions { get; } = new();

	public void ConfigureClient(Action<CosmosClientOptions> configure)
	{
		if (configure is null)
		{
			throw new ArgumentNullException(nameof(configure));
		}

		configure.Invoke(ClientOptions);
	}
}





public abstract class CosmosRepository<T> : IRepository<T>, IDisposable
{
	// Using protected to allow inherited class access
	protected readonly CosmosClient client;
	protected readonly Database database;
	protected readonly Container container;

	public CosmosRepository(CosmosRepositoryOptions options)
	{
		this.client = new CosmosClient(options.Connection, options.ClientOptions);
		this.database = client.GetDatabase(options.Database);
		this.container = database.GetContainer(options.Container);
	}

	protected virtual Func<T, PartitionKey> Partition { get; }

	async Task<IRepositoryResult> IRepository.GetAsync(object[] keys, CancellationToken cancellationToken) => await GetAsync(keys, cancellationToken);
	public virtual Task<IRepositoryResult<T>> GetAsync(object[] keys, CancellationToken cancellationToken = default)
	{
		EnsureKeyLength(keys);

		return AsyncInvocationWrapper(async () =>
		{
			var itemResponse = await container.ReadItemAsync<T>(
				keys[0].ToString(),
				GetPartitionKey(keys[1]),
				default,
				cancellationToken);


			return new RepositoryResult<T>(itemResponse)
			{
				IsSuccess = true
			};
		});
	}

	async Task<IRepositoryResult> IRepository.DeleteAsync(object[] keys, CancellationToken cancellationToken) => await DeleteAsync(keys, cancellationToken)
	public Task<IRepositoryResult<T>> DeleteAsync(object[] keys, CancellationToken cancellationToken = default)
	{
		EnsureKeyLength(keys);

		return AsyncInvocationWrapper(async () =>
		{
			var itemResponse = await container.DeleteItemAsync<T>(
				keys[0].ToString(),
				GetPartitionKey(keys[1]),
				default,
				cancellationToken);

			return new RepositoryResult<T>(itemResponse)
			{
				IsSuccess = true
			};
		});
	}

	public Task<IRepositoryResult> UpdateAsync(RepositoryContext context, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
	public Task<IRepositoryResult<T>> UpdateAsync(RepositoryContext<T> context, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task<IRepositoryResult> UpsertAsync(RepositoryContext context, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
	public Task<IRepositoryResult<T>> UpsertAsync(RepositoryContext<T> context, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	async Task<IRepositoryResult> IRepository.CreateAsync(RepositoryContext context, CancellationToken cancellationToken = default) => await CreateAsync(((RepositoryContext<T>)context), cancellationToken);
	public virtual Task<IRepositoryResult<T>> CreateAsync(RepositoryContext<T> context, CancellationToken cancellationToken = default)
	{
		return AsyncInvocationWrapper(async () =>
		{
			var itemResponse = await container.CreateItemAsync(
				context.State,
				default,
				default,
				cancellationToken);

			return new RepositoryResult<T>(itemResponse)
			{
				IsSuccess = true
			};
		});
	}



	// This wrapper is to cut down on the same try catch block
	private async Task<IRepositoryResult<T>> AsyncInvocationWrapper(Func<Task<IRepositoryResult<T>>> method)
	{
		try
		{
			return await method.Invoke();
		}
		catch (CosmosException exception) when (exception.StatusCode == HttpStatusCode.Conflict)
		{
			throw RepositoryException.Conflict(
				message: $"Unable to find Object with ID '{id}'.");
		}
		catch (CosmosException exception) when (exception.StatusCode == HttpStatusCode.NotFound)
		{
			throw RepositoryException.NotFound(
				message: $"Unable to find Object with ID '{id}'.");
		}
		catch (CosmosException exception) when (exception.StatusCode == HttpStatusCode.PreconditionFailed)
		{
			throw RepositoryException.PreconditionFailure(
				message: $"Unable to find Object with ID '{id}'.");
		}
		catch (CosmosException exception) when (exception.StatusCode == HttpStatusCode.TooManyRequests)
		{
			throw RepositoryException.NotFound(
				message: $"Unable to find Object with ID '{id}'.");
		}
		catch (Exception exception)
		{
			return new RepositoryResult<T>(default(T))
			{
				IsSuccess = false,
				Error = exception
			};
		}
	}
	private PartitionKey GetPartitionKey(object partitionKey)
	{
		if (bool.TryParse(partitionKey.ToString(), out var boolPk))
		{
			return new PartitionKey(boolPk);
		}


		throw new Exception();
	}
	private void EnsureKeyLength(object[] keys)
	{
		if (keys.Length != 2)
		{
			throw new ArgumentOutOfRangeException("The given repository");
		}
	}


	public IRepositoryCollectionResult<T> QueryAsync(IQueryable<T> queryable, CancellationToken cancellationToken = default)
	{
		try
		{
			var orderedQueryable = (IOrderedQueryable<T>)container
				.GetItemLinqQueryable<T>()
				.Provider // Fo whatever reason Microsoft didn't override the return with implicit interface implementation, so have to explixitly cast IOrderedQueryable<T>
				.CreateQuery(queryable.Expression);

			var iterator = orderedQueryable.ToFeedIterator();

			return new CosmosRepositoryCollectionResult<T>(iterator);
		}
		catch (CosmosException exception)
		{

		}
		throw new NotImplementedException();
	}


	void IDisposable.Dispose()
	{
		client.Dispose();
	}





	

	public IRepositoryBulkOperation GetBulkOperations(IEnumerable<RepositoryContext<T>> contexts)
	{
		throw new NotImplementedException();
	}




	

	

	public IRepositoryBulkOperation GetBulkOperations(IEnumerable<RepositoryContext> contexts)
	{
		throw new NotImplementedException();
	}


	protected partial class CosmosRepositoryCollectionResult<T> : IRepositoryCollectionResult<T>, IDisposable
	{
		private readonly FeedIterator<T> iterator;

		public CosmosRepositoryCollectionResult(FeedIterator<T> iterator)
		{
			this.iterator = iterator;
		}

		public bool IsSuccess { get; set; }

		public Exception Error { get; set; }

		public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
		{
			while (iterator.HasMoreResults)
			{
				var items = await iterator.ReadNextAsync(cancellationToken);

				foreach (var item in items)
				{
					yield return item;
				}
			}
		}

		public void Dispose()
		{
			iterator.Dispose();
		}
	}
}




