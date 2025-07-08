<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	var builder = default(IOGraphModelBuilder);

	builder.AddQuery("GetUsers", descriptor =>
	{
		descriptor.UseMethod("GET")
			.UseRoute("/users")
			.UseFiltering()
			.UsePaging()
			.UseSorting()
			.UseAuthorization()
			.UseQueryableType<User>(type =>
			{
				type.UseName("UsersCollection");
				
				
			})
			.UseResolver(async context =>
			{

			});
	});
	builder.AddQuery("GetUserById", descriptor =>
	{
		descriptor.UseMethod("GET")
			.UseRoute("/users/{userId}")
			.UseFiltering()
			.UsePaging()
			.UseSorting()
			.UseAuthorization()
			.UseType<User>(type =>
			{
				
			})
			.UseResolver(async context =>
			{

			});
	});
	builder.AddQuery("GetUsersAppRoleAssignments", descriptor =>
	{
		descriptor.UseMethod("GET")
			.UseRoute("/users/{userId}/appRoleAssignments")
	});
	builder.AddCommand("CreateUsers", descriptor =>
	{
		descriptor.UseMethod("POST")
			.UseRoute("/users");
	});
		
		
		
		
}

public class User
{
	
}


#region Base Class Implementations


/*
	Design Patterns:
	
	- Iterator Pattern: Will use to implement Graphical Visualization

*/


#endregion

#region Abstractions

public readonly struct Name
{
	public string Value { get; }
}
public readonly struct Method
{
	public string Value { get; }
}
public readonly struct Route
{
	public string Value { get; }
}
public readonly struct RouteSegment
{
	
}

public delegate Task<T> OGraphResolver<T>(IOGraphResolverContext context);


#region Modeling

public interface IOGraphModel
{
	IOGraphQueryCollection Queries { get; }
	IOGraphCommandCollection Commands { get; }
}
public interface IOGraphQueryCollection : IList<IOGraphQuery>
{
	
}
public interface IOGraphCommandCollection : IList<IOGraphCommand>
{
	
}
public interface IOGraphQuery
{
	Name Name { get; } 		// The name of the query
	Route Route { get; }	
	Method Method { get; }
	IOGraphQueryType QueryType { get; }
}
public interface IOGraphQueryType
{
	Type Type { get; }
	Name TypeName { get; }
}
public interface IOGraphQueryType<T> : IOGraphQueryType
{

}
public interface IOGraphCommand
{
	Name Name { get; }      // The name of the command
	Route Route { get; }
	Method Method { get; }
	IOGraphCommandType CommandType { get; }
}
public interface IOGraphCommandType
{
	Type Type { get; }
	Name TypeName { get; }
}
public interface IOGraphCommandType<T> : IOGraphCommandType
{

}

#endregion

public interface IOGraphResolverContext
{
	T GetParent<T>();
	T GetService<T>();
}
public interface IOGraphContentFormatter
{
	
}

public interface IOGraphModelBuilder
{
	IOGraphModelBuilder AddFormatter(string format,IOGraphContentFormatter formatter);
	
	IOGraphModelBuilder AddQuery(IOGraphQuery query);
	IOGraphModelBuilder AddQuery(string name, Action<IOGraphQueryDescriptor> descriptor);
	IOGraphModelBuilder AddCommand(IOGraphCommand command);
	IOGraphModelBuilder AddCommand(string name, Action<IOGraphCommandDescriptor> descriptor);
	
	IOGraphModel Build();
}
public interface IOGraphQueryDescriptor
{
	IOGraphQueryDescriptor UseRoute(string route);
	IOGraphQueryDescriptor UseMethod(string method);
	IOGraphQueryDescriptor UseFormat(string format);
	IOGraphQueryDescriptor UseFiltering(); 	// Enables filtering
	IOGraphQueryDescriptor UseSorting();	// Enables sorting
	IOGraphQueryDescriptor UsePaging();		// Enables paging, UseQueryableType, or UsePagingType

	IOGraphQueryDescriptor UseType<T>();
	IOGraphQueryDescriptor UseType<T>(Action<IOGraphQueryTypeDescriptor<T>> descriptor);
	IOGraphQueryDescriptor UseType(Type type, Action<IOGraphQueryTypeDescriptor> descriptor);
	
	IOGraphQueryDescriptor UseEnumerableType<T>();
	IOGraphQueryDescriptor UseEnumerableType<T>(Action<IOGraphQueryTypeDescriptor<T>> descriptor);
	IOGraphQueryDescriptor UseEnumerableType(Type type, Action<IOGraphQueryTypeDescriptor> descriptor);
	
	IOGraphQueryDescriptor UseQueryableType<T>();
	IOGraphQueryDescriptor UseQueryableType<T>(Action<IOGraphQueryTypeDescriptor<T>> descriptor);
	IOGraphQueryDescriptor UseQueryableType(Type type, Action<IOGraphQueryTypeDescriptor> descriptor);
	
	IOGraphQueryDescriptor UseAuthorization();
	
	
	void UseResolver(OGraphResolver<object> resolver);
	void UseResolver<T>(OGraphResolver<T> ressolver);
}

public interface IOGraphQueryTypeDescriptor
{
	
	IOGraphQueryTypeDescriptor 
	
	IOGraphQueryTypeDescriptor UseName(string name);
	IOGraphQueryTypeDescriptor Ignore(string name);
	IOGraphQueryTypeDescriptor UseNavigation(string name, Action<IOGraphQueryNavigationDescriptor> configure);
}
public interface IOGraphQueryTypeDescriptor<T>
{
	IOGraphQueryTypeDescriptor<T> UseName(string name); // Override the name of the given type.	
	IOGraphQueryTypeDescriptor<T> Ignore<TMember>(Expression<Func<T, TMember>> expression); // Must be a Member Expression
	
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


public interface IOGraphCommandDescriptor
{
	IOGraphCommandDescriptor UseRoute(string route);
	IOGraphCommandDescriptor UseMethod(string method);
}

public interface IOGraphCommandContext
{

}
#endregion


