<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "xunit"

void Main()
{
	//RunTests();  // Call RunTests() or press Alt+Shift+T to initiate testing.
}

#region Abstractions

public interface IRepository
{

}

/// <summary>
/// A simple repository for the data layer.
/// </summary>
public interface IRepository<T> : IRepository where T : class, new()
{
	/// <summary>
	/// 
	/// </summary>
	Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
	/// <summary>
	/// 
	/// </summary>
	Task<T> GetAsync(object[] keys, CancellationToken cancellationToken = default);
	/// <summary>
	/// 
	/// </summary>
	Task<T> DeleteAsync(object[] keys, CancellationToken cancellationToken = default);
	/// <summary>
	/// 
	/// </summary>
	Task<T> UpdateAsync(object[] keys, Action<T> configure, CancellationToken cancellationToken = default);
}
/// <summary>
/// A repository for batching operations to the data layer.
/// </summary>
public interface IBatchRepository<T> : IRepository<T> where T : class, new()
{
	IBatchRepositoryContext<T> CreateBatchContext();
}
/// <summary>
/// 
/// </summary>
/// <remarks>
/// On 
/// </remarks>
public interface IBatchRepositoryContext<T> : IAsyncDisposable where T : class, new()
{
	Task CreateAsync(T entity);
	Task DeleteAsync(object[] keys);
	Task UpdateAsync(object[] keys, Action<T> entity);
	Task<IBatchResult<T>> ExecuteAsync();
}
public interface IBatchResult<T> where T : class, new()
{

}
public interface IBatchResultChange<T>
{
	Exception Error { get; }
	StateChange Change { get; }
	T State { get; }
}
public enum StateChange
{
	Unknown,
	Failure,
	Update,
	Delete,
	Create
}
/// <summary>
/// 
/// </summary>
public interface IQueryableRepository<T> : IRepository<T>, IQueryable<T> where T : class, new()
{

}
public interface IQueryableResult<T> where T : class, new()
{
	long Count { get; }
}
public interface IBulkOperationRepository<T> : IRepository<T> where T : class, new()
{
	Task<int> DeleteAsync(Expression<Func<T, bool>> predicate);
	Task<int> UpdateAsync(Expression<Func<T, bool>> predicate);
	Task<int> ExecuteAsync(string operationName, object[]? arguments = default, CancellationToken cancellationToken = default);
}
public abstract class Repository<T> : IBulkOperationRepository<T>, IQueryableRepository<T>, IBatchRepository<T> where T : class, new()
{
	public abstract Type ElementType { get; }
	public abstract Expression Expression { get; }
	public abstract IQueryProvider Provider { get; }

	public abstract Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
	public abstract IBatchRepositoryContext<T> CreateBatchContext();
	public abstract Task<int> DeleteAsync(Expression<Func<T, bool>> predicate);
	public abstract Task<T> DeleteAsync(object[] keys, CancellationToken cancellationToken = default);
	public abstract Task<int> ExecuteAsync(string operationName, object[]? arguments = null, CancellationToken cancellationToken = default);
	public abstract Task<T> GetAsync(object[] keys, CancellationToken cancellationToken = default);
	public abstract IEnumerator<T> GetEnumerator();
	public abstract Task<int> UpdateAsync(Expression<Func<T, bool>> predicate);
	public abstract Task<T> UpdateAsync(object[] keys, Action<T> configure, CancellationToken cancellationToken = default);

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
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