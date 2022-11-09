<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
</Query>

#load "patterns\repository.pattern.abstractions"


void Main()
{
	
}


public static class RepositoryServiceCollectionExtensions
{ 
	
	public static IServiceCollection AddRepository(this IServiceCollection services, Action<IRepositoryFactoryBuilder> builder)
	{
		
		
		
		return services;
	}

	public static IServiceCollection AddRepository(this IServiceCollection services, Action<IServiceProvider, IRepositoryFactoryBuilder> builder)
	{
		
		return services.AddSingleton(



		return services;
	}
	
	public static IServiceCollection AddRepository<TRepository>(this IServiceCollection services) where TRepository : IRepository, new()
	{
		return service.AddRepository(ServiceProvider =>
		{

		})
	}

	public static IServiceCollection AddRepository(this IServiceCollection services, Func<IServiceProvider, IRepository> factory)
	{

	}
}
