<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
</Query>

#load "patterns\repository.pattern.abstractions"


void Main()
{
	
}



#region Abstrations

public interface IRepositoryFactoryBuilder
{
	IRepositoryFactoryBuilder Use(IRepository repository);
	IRepositoryFactoryBuilder Use(Func<IServiceProvider, IRepository> factory);
	IRepositoryFactory Build();
}
public interface IRepositoryFactory
{
	IRepository<T> Create<T>() where T : class, new();
	IBatchRepository<T> CreateBatchRepository<T>() where T : class, new();
	IQueryableRepository<T> CreateQueryableRepository<T>() where T : class, new();
	IBulkOperationRepository<T> CreateBulkOperationRepository<T>() where T : class, new();
}
#endregion

#region Implementation

internal class RepositoryFactory : IRepositoryFactory
{
	public RepositoryFactory()
	{
		
	}
	
	public IRepository<T> Create<T>() where T : class, new()
	{
		throw new NotImplementedException();
	}
	public IBatchRepository<T> CreateBatchRepository<T>() where T : class, new()
	{
		throw new NotImplementedException();
	}
	public IBulkOperationRepository<T> CreateBulkOperationRepository<T>() where T : class, new()
	{
		throw new NotImplementedException();
	}
	public IQueryableRepository<T> CreateQueryableRepository<T>() where T : class, new()
	{
		throw new NotImplementedException();
	}
}

internal class RepositoryFactoryBuilder : IRepositoryFactoryBuilder
{
	private readonly IServiceProvider serviceProvider;
	
	public RepositoryFactoryBuilder(IServiceProvider serviceProvider)
	{
		this.serviceProvider = serviceProvider;
	}
	
	public IRepositoryFactory Build()
	{
		throw new NotImplementedException();
	}

	public IRepositoryFactoryBuilder Use(IRepository repository)
	{
		throw new NotImplementedException();
	}

	public IRepositoryFactoryBuilder Use(Func<IServiceProvider, IRepository> factory)
	{
		throw new NotImplementedException();
	}
}

#endregion

#region Extensions
public static class RepositoryServiceCollectionExtensions
{
	public static IServiceCollection AddRepository(this IServiceCollection services, Action<IRepositoryFactoryBuilder> configure)
	{
		if (configure is null)
		{
			throw new ArgumentNullException(nameof(configure));
		}
		return services.AddSingleton<IRepositoryFactory>(serviceProvider =>
		{
			var builder = new RepositoryFactoryBuilder(serviceProvider);
			
			configure.Invoke(builder);
			
			return builder.Build();
		});
	}
}
#endregion
