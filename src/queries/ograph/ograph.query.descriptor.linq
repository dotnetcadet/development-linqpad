<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	
}


#region Abstractions




public delegate Task<IEnumerable<T>> OGraphEnumerableResolver<T>(IOGraphQueryContext context);
public delegate Task<T> OGraphResolver<T>(IOGraphQueryContext context);


public interface IOGraphModel
{
	IEnumerable<IOGraphQuery> Queries { get; }
	IEnumerable<IOGraphCommand> Commands { get; }
}

/// <summary>
/// 
/// </summary>
/// <remarks>
/// - A Query can only have ONE defined IOGraphQueryType Defined
/// </remarks>
public interface IOGraphQuery
{
	void Configure(IOGraphQueryDescriptor descriptor);
}
public interface IOGraphQueryDescriptor
{
	IOGraphQueryDescriptor UseRoute(string route);	
	
	
	IOGraphQueryDescriptor UseType<TQueryType>();
	IOGraphQueryDescriptor UseType(Action<IOGraphQueryTypeDescriptor> configure);
	IOGraphQueryDescriptor UseType<T>(Action<IOGraphQueryTypeDescriptor<T>> configure);
	
	void UseResolver(OGraphResolver<object> resolver);
	void UseResolver<T>(OGraphResolver<T> ressolver);
}
public interface IOGraphQueryType
{
	void Configure(IOGraphQueryTypeDescriptor descriptor);
}
public interface IOGraphQueryType<T>
{
	void Configure(IOGraphQueryTypeDescriptor<T> descriptor);
}
public interface IOGraphQueryTypeDescriptor
{
	/// <summary>
	/// 
	/// </summary>
	IOGraphQueryTypeDescriptor UseName(string name);
	/// <summary>
	/// 
	/// </summary>
	IOGraphQueryTypeDescriptor UseFiltering();
	/// <summary>
	/// 
	/// </summary>
	IOGraphQueryTypeDescriptor UseSorting();
	/// <summary>
	/// 
	/// </summary>
	IOGraphQueryTypeDescriptor UsePaging();	
	/// <summary>
	/// 
	/// </summary>
	IOGraphQueryTypeDescriptor UseNavigation(string name, Action<IOGraphQueryNavigationDescriptor> configure);
}
public interface IOGraphQueryTypeDescriptor<T>
{
	/// <summary>
	/// Override the name of the given type.
	/// </summary>
	IOGraphQueryTypeDescriptor<T> UseName(string name);
	
	IOGraphQueryTypeDescriptor<T> AddField(string name);
	IOGraphQueryTypeDescriptor<T> Field(Expression<Func<T, object>> field);
	IOGraphQueryTypeDescriptor<T> Type<TOgraphType>();
	IOGraphQueryTypeDescriptor<T> Resolve();
	
	IOGraphQueryTypeDescriptor<T> AddNavigation(string name);
}
public interface IOGraphQueryNavigationDescriptor
{
	
}
public interface IOGraphQueryTypeFieldDescriptor
{
	
}
public interface IOGraphQueryContext
{

}

public interface IOGraphCommand
{
	
}
public interface IOGraphCommandType 
{
	
}
public interface IOGraphCommandContext
{

}
#endregion


#region Base Class Implementations

public class OGraphQuery : IOGraphQuery
{
	public void Configure(IOGraphQueryDescriptor descriptor)
	{
		descriptor
			.UseRoute("/users/{userId}")
			.UseType(type=> 
			{
				type.UseFiltering();
				type.UseSorting();
				type.UsePaging();
				type.UseNavigation("test", navigation =>
				{
					navigation.
				});
			})
			.UseResolver(async context =>
			{
				
				return "";
			});
	}
}


public class OGraphQueryType<T> : IOGraphQueryType<T>
{
	public void Configure(IOGraphQueryTypeDescriptor<T> descriptor)
	{
		descriptor.Field("name")
			
	}
}

#endregion