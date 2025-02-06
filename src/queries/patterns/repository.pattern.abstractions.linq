<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "xunit"

void Main()
{
	
	//RunTests();  // Call RunTests() or press Alt+Shift+T to initiate testing.
}

public interface IEntity<T> where T : IEntity<T>, new()
{
	
}
#region Abstractions

public interface IRepository
{	
	string Name { get; }
	Task<object> GetAsync(object[] keys, CancellationToken cancellationToken = default);
	Task<object> DeleteAsync(object[] keys, CancellationToken cancellationToken = default);
	Task<object> UpdateAsync(object[] keys, Action<object> configure, CancellationToken cancellationToken = default);
	Task<object> CreateAsync(object entity, CancellationToken cancellationToken = default);
}
public interface IRepository<T> : IRepository where T : IEntity<T>, new()
{
	new Task<T> GetAsync(object[] keys, CancellationToken cancellationToken = default);
	new Task<T> DeleteAsync(object[] keys, CancellationToken cancellationToken = default);
	Task<T> UpdateAsync(object[] keys, Action<T> configure, CancellationToken cancellationToken = default);
	Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
	IRepositoryBatchContext<T> CreateBatchContext();
}
public interface IQueryableRepository<T> : IRepository<T>, IQueryable<T> where T : IEntity<T>, new()
{
	
}
public interface IIncludableRepository<T> : IQueryable<T>, IRepository where T : class, new()
{
	IIncludableRepository<T, TProperty> Include<TProperty>(
		Expression<Func<T, IEnumerable<TProperty>>> navigation)
		where TProperty : class, new();
}
public interface IIncludableRepository<T, TProperty> : IIncludableRepository<T> where TProperty : class, new() where T : class, new()
{
	IIncludableRepository<TProperty, TNested> ThenInclude<TNested>(
		Expression<Func<TProperty, IEnumerable<TNested>>> navigation)
		where TNested : class, new();
}
public interface IBulkOperationRepository<T> : IRepository<T> where T : IEntity<T>, new()
{
	Task<int> DeleteAsync(Expression<Func<T, bool>> predicate);
	Task<int> UpdateAsync(Expression<Func<T, bool>> predicate);
	Task<int> ExecuteAsync(string operationName, object[]? arguments = default, CancellationToken cancellationToken = default);
}
public interface IBulkOperation
{
	string Name { get; }
	Task<int> InvokeAsync(object[]? args = default, CancellationToken cancellationToken = default);
}
public interface IRepositoryFactory 
{
	IRepository Create(string name);
	
	TRepository Create<TRepository>() 
		where TRepository : IRepository;
		
	TRepository Create<TRepository>(string name) 
		where TRepository : IRepository;
}
public interface IRepositoryBatchContext<T> where T : IEntity<T>, new()
{
	Task CreateAsync(T entity);
	Task DeleteAsync(object[] keys);
	Task UpdateAsync(object[] keys, Action<T> entity);
	Task<IEnumerable<IBatchResult<T>>> ExecuteAsync();
}
public interface IBatchResult<T> where T : IEntity<T>, new()
{
	BatchResultState State { get; }
	T Entity { get; }
}
public enum BatchResultState
{
	Failed,
	Created,
	Updated,
	Deleted
}

#endregion

#region Internals 

internal class RepositoryContextDefault<T> : RepositoryContext<T> where T : IEntity<T>, new()
{
	public RepositoryContextDefault(T entity)
	{
		Entity = entity;
	}

	public override T Entity  { get; }
}

#endregion

#region Exceptions
/// <summary>
/// 
/// </summary>
public abstract class RepositoryException : Exception
{
	public RepositoryException(string message)
		: base(message) { }

	public RepositoryException(string message, Exception innerException)
		: base(message, innerException) { }


	public abstract ErrorCode Code { get; }

	public static RepositoryException NotFound(params object?[]? keys)
	{
		throw new DefaultRepositoryException($"", ErrorCode.NotFound);
	}
}

internal sealed class DefaultRepositoryException : RepositoryException
{
	public DefaultRepositoryException(string message, ErrorCode errorCode)
		: base(message)
	{
		Code = errorCode;
	}

	public DefaultRepositoryException(string message, ErrorCode errorCode, Exception innerException)
		: base(message, innerException)
	{
		Code = errorCode;
	}

	public override ErrorCode Code { get; }
}
/// <summary>
/// An error code which references an HTTP Status Code.
/// </summary>
public enum ErrorCode
{
	Unknown = 0,
	NotFound = 404,
	Conflict = 409,
	PreconditionFailure = 412,
	ToManyRequests = 429
}

#endregion

#region Extensions

public static class RepositoryExtensions
{
	public static bool IsQueryable<T>(this IRepository<T> repository, out IQueryableRepository<T>? queryable)
		where T : IEntity<T>, new()
	{
		queryable = null;
		
		if (repository is IQueryableRepository<T> type)
		{
			queryable = type;
			return true;
		}
		
		return false;
	}

}

#endregion

#region Utilities


#endregion

#region Test Objects
public class User
{
	public UserInfo? Info { get; set; }
}
public class UserInfo
{

}


#endregion

#region private::Tests

[Fact] void Test_Xunit() => Assert.True(1 + 1 == 2);

#endregion