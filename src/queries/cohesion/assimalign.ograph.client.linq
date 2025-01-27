<Query Kind="Program">
<Namespace>System</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>System.Threading</Namespace>
<Namespace>System.Threading.Tasks</Namespace>
<Namespace>System.Collections</Namespace>
<Namespace>System.Linq.Expressions</Namespace>
<Namespace>System.Reflection</Namespace>
</Query>
#load ".\assimalign.ograph.core"
#load ".\assimalign.ograph.gdm"
#load ".\assimalign.ograph.syntax"

void Main()
{

}

#region Assimalign.OGraph.Client(net8.0)
namespace Assimalign.OGraph.Client
{
	#region \Abstractions
	public interface IOGraphClient
	{
	    Task<IOGraphClientResponse> GetAsync(IOGraphClientRequest request, CancellationToken cancellationToken = default);
	    Task<IOGraphClientResponse> PutAsync(IOGraphClientRequest request, CancellationToken cancellationToken = default);
	    Task<IOGraphClientResponse> PostAsync(IOGraphClientRequest request, CancellationToken cancellationToken = default);
	    Task<IOGraphClientResponse> PatchAsync(IOGraphClientRequest request, CancellationToken cancellationToken = default);
	    Task<IOGraphClientResponse> DeleteAsync(IOGraphClientRequest request, CancellationToken cancellationToken = default);
	}
	public interface IOGraphClientFactory
	{
	    IOGraphClient Create(Label label);
	}
	public interface IOGraphClientHeaderCollection
	{
	}
	public interface IOGraphClientRequest
	{
	}
	public interface IOGraphClientResponse
	{
	}
	#endregion
	#region \Linq\Internal
	internal class OGraphQueryable<T> : IQueryable<T>
	{
	    public Type ElementType => throw new NotImplementedException();
	    public Expression Expression => throw new NotImplementedException();
	    public IQueryProvider Provider => throw new NotImplementedException();
	    public IEnumerator<T> GetEnumerator()
	    {
	        throw new NotImplementedException();
	    }
	    IEnumerator IEnumerable.GetEnumerator()
	    {
	        throw new NotImplementedException();
	    }
	}
	internal class OGraphQueryProvider : IQueryProvider
	{
	    public IQueryable CreateQuery(Expression expression)
	    {
	        throw new NotImplementedException();
	    }
	    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
	    {
	        throw new NotImplementedException();
	    }
	    public object? Execute(Expression expression)
	    {
	        throw new NotImplementedException();
	    }
	    public TResult Execute<TResult>(Expression expression)
	    {
	        throw new NotImplementedException();
	    }
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
}
#endregion
