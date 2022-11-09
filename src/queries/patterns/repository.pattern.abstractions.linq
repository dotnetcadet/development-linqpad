<Query Kind="Program">
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>Xunit</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "xunit"

void Main()
{
	//RunTests();  // Call RunTests() or press Alt+Shift+T to initiate testing.

	//var builder = RepositoryFactoryBuilder.Create();
}



#region Abstractions

// Let's use generic Http Status Codes as Repository Error Code for easy conversion into HTTP Responses
// We can create an adapter pattern to format the HTTP Response of Repository Error
public enum RepositoryErrorCode
{
	Unknown = 0,
	NotFound = 404,
	Conflict = 409,
	PreconditionFailure = 412,
	ToManyRequests = 429
}
public abstract class RepositoryException : Exception
{
	public abstract RepositoryErrorCode ErrorCode { get; }
	public static RepositoryException NotFound(string message)=> new RepositoryExceptionDefault(RepositoryErrorCode.NotFound, message);
	public static RepositoryException Conflict(string message) => new RepositoryExceptionDefault(RepositoryErrorCode.Conflict, message);
	public static RepositoryException ToManyRequests(string message) => new RepositoryExceptionDefault(RepositoryErrorCode.ToManyRequests, message);
	public static RepositoryException PreconditionFailure(string message) => new RepositoryExceptionDefault(RepositoryErrorCode.PreconditionFailure, message);
}
internal sealed class RepositoryExceptionDefault : RepositoryException
{
	public RepositoryExceptionDefault(RepositoryErrorCode errorCode)
	{
		this.ErrorCode = errorCode;
	}
	public RepositoryExceptionDefault(RepositoryErrorCode errorCode, string message) 
	{
		this.ErrorCode = errorCode;
		this.Message = message;
	}

	public override RepositoryErrorCode ErrorCode { get; }
	public override string Message { get; }
}

public interface IRepositoryFactoryBuilder
{
	IRepositoryFactoryBuilder Register(IRepository repository);
	IRepositoryFactoryBuilder Register(IRepository repository, string repositoryName);
	IRepositoryFactoryBuilder Register<TRepository>() where TRepository : IRepository, new();
	IRepositoryFactoryBuilder Register<TRepository>(string repositoryName) where TRepository : IRepository, new();
	IRepositoryFactory Build();
}
public interface IRepositoryFactory
{
	IRepository<T> Create<T>();
	IRepository Create(string repositoryName);
	IRepository<T> Create<T>(string repositoryName);
}
public interface IRepository
{
	Task<IRepositoryResult> GetAsync(object[] keys, CancellationToken cancellationToken = default);
	Task<IRepositoryResult> DeleteAsync(object[] keys, CancellationToken cancellationToken = default);
	Task<IRepositoryResult> CreateAsync(RepositoryContext context, CancellationToken cancellationToken = default);
	Task<IRepositoryResult> UpdateAsync(RepositoryContext context, CancellationToken cancellationToken = default);
	Task<IRepositoryResult> UpsertAsync(RepositoryContext context, CancellationToken cancellationToken = default);
	IRepositoryBulkOperation GetBulkOperations(IEnumerable<RepositoryContext> contexts);
}
public interface IRepository<T> : IRepository
{
	Task<IRepositoryResult<T>> GetAsync(object[] keys, CancellationToken cancellationToken = default);	
	Task<IRepositoryResult<T>> DeleteAsync(object[] keys, CancellationToken cancellationToken = default);	
	Task<IRepositoryResult<T>> CreateAsync(RepositoryContext<T> context, CancellationToken cancellationToken = default);
	Task<IRepositoryResult<T>> UpdateAsync(RepositoryContext<T> context, CancellationToken cancellationToken = default);
	Task<IRepositoryResult<T>> UpsertAsync(RepositoryContext<T> context, CancellationToken cancellationToken = default);
	IRepositoryCollectionResult<T> QueryAsync(IQueryable<T> queryable, CancellationToken cancellationToken = default);
	
	IRepositoryBulkOperation GetBulkOperations(IEnumerable<RepositoryContext<T>> contexts);
}
public interface IRepositoryResult
{
	object State { get; }
	bool IsSuccess { get; }
	Exception? Error { get; }
}
public interface IRepositoryResult<T> : IRepositoryResult
{
	T State { get; }
}
public interface IRepositoryCollectionResult<T> : IAsyncEnumerable<T>
{
	bool IsSuccess { get; }
	Exception? Error { get; }
}
public interface IRepositoryBulkOperation
{
	Task<IRepositoryResult> ExecuteAsync();
}


public abstract class RepositoryContext
{
	public abstract object State { get; }
}
public abstract class RepositoryContext<T>
{
	public abstract T State { get; }
	
	public static implicit operator RepositoryContext<T>(T instance) => new RepositoryContextDefault<T>(instance);
	public static implicit operator RepositoryContext<T>(RepositoryContext context)
	{
		if (context.State is T instance)
		{
			return new RepositoryContextDefault<T>(instance);
		}

		throw new InvalidCastException($"Unable to case type '{context.State.GetType().Name}' to '{nameof(T)}'.");
	}
}
internal sealed class RepositoryContextDefault<T> : RepositoryContext<T>
{
	public RepositoryContextDefault(T state)
	{
		this.State = state;
	}

	public override T State { get; }
}


#endregion

#region Extensions 

public static class RepositoryExtensions
{
	public static Task<IRepositoryResult> GetAsync(this IRepository repository, params object[] keys) => repository.GetAsync(keys, default);
	public static Task<IRepositoryResult<T>> GetAsync<T>(this IRepository<T> repository, params object[] keys) => repository.GetAsync(keys, default);
	public static Task<IRepositoryResult> DeleteAsync(this IRepository repository, params object[] keys) => repository.DeleteAsync(keys, default);
	public static Task<IRepositoryResult<T>> DeleteAsync<T>(this IRepository<T> repository, params object[] keys) => repository.DeleteAsync(keys, default);
}


#endregion

#region implementation

public sealed class RepositoryFactoryBuilder : IRepositoryFactoryBuilder
{
	// The Key for this dictionary will be the hashcode of the nameof() 
	private readonly IDictionary<string, IRepository> repositories;

	public RepositoryFactoryBuilder()
	{
		this.repositories = new ConcurrentDictionary<string, IRepository>(StringComparer.InvariantCultureIgnoreCase);
	}


	public IRepositoryFactoryBuilder Register(IRepository repository)
	{
		var repositoryName = repository.GetType().Name;

		repositories.Add(repositoryName, repository);

		return this;
	}
	public IRepositoryFactoryBuilder Register(IRepository repository, string repositoryName)
	{
		repositories.Add(repositoryName, repository);

		return this;
	}
	public IRepositoryFactoryBuilder Register<TRepository>() where TRepository : IRepository, new()
	{
		var repositoryName = typeof(TRepository).Name;

		//repositories.AddOrUpdate(repositoryName.GetHashCode(), 

		return this;
	}

	public IRepositoryFactoryBuilder Register<TRepository>(string repositoryName) where TRepository : IRepository, new()
	{
		return this;
	}

	IRepositoryFactory IRepositoryFactoryBuilder.Build()
	{
		return new RepositoryFactory(repositories);
	}

	public static IRepositoryFactoryBuilder Create() => new RepositoryFactoryBuilder();
}



public sealed class RepositoryFactory : IRepositoryFactory
{
	private readonly IDictionary<string, IRepository> repositories;

	internal RepositoryFactory(IDictionary<string, IRepository> repositories)
	{
		this.repositories = repositories;
	}

	public IRepository<T> Create<T>()
	{
		throw new NotImplementedException();
	}
	public IRepository Create(string repositoryName)
	{
		throw new NotImplementedException();
	}

	public IRepository<T> Create<T>(string repositoryName)
	{
		var repository = repositories[repositoryName];

		if (repository is IRepository<T> instance)
		{
			return instance;
		}

		throw new ArgumentException();
	}
}


public sealed class RepositoryResult : IRepositoryResult
{
	public RepositoryResult() { }
	public RepositoryResult(object state)
	{
		this.State = state;
	}
	public object State { get; init; }
	public bool IsSuccess { get; init; }
	public Exception Error { get; init; }
}
public sealed class RepositoryResult<T> : IRepositoryResult<T>
{
	
	public RepositoryResult() { }
	public RepositoryResult(T state)
	{
		this.State = state;
	}
	public T State { get; init; }
	public bool IsSuccess { get; init; }
	public Exception Error { get; init; }
	object IRepositoryResult.State => this.State;
	
	
	//public static implicit operator RepositoryResult<T>(T state) =>  new RepositoryResult<T>(state);
}
public class RepositoryCollectionResult<T> : IRepositoryCollectionResult<T>
{
	public bool IsSuccess => throw new NotImplementedException();

	public Exception Error => throw new NotImplementedException();

	public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
}

#endregion

#region private::Tests

[Fact] void Test_Xunit() => Assert.True (1 + 1 == 2);

#endregion