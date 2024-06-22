<Query Kind="Program">
  <NuGetReference Version="8.0.5">Microsoft.EntityFrameworkCore</NuGetReference>
  <NuGetReference Version="8.0.5">Microsoft.EntityFrameworkCore.SqlServer</NuGetReference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
</Query>

#load "patterns\repository.pattern.abstractions"
#load "patterns\repository.pattern.dependency.injection"

void Main()
{
	var services = new ServiceCollection();

	services.AddRepository(builder =>
	{
		builder.UseSqlServer<MySqlContext>(options =>
		{
			options.UseContextFactory = true;
			
			options.Add<User>();
		});
	});
}

public class MySqlContext : DbContext
{

}


public sealed class SqlServerRepositoryOptions<TContext> where TContext : DbContext
{
	private readonly IRepositoryFactoryBuilder builder;

	internal SqlServerRepositoryOptions(IRepositoryFactoryBuilder builder)
	{
		this.builder = builder;
	}

	public bool UseContextFactory { get; set; }


	public void Add<T>() where T : class, new()
	{
		builder.Use(serviceProvider =>
		{
			TContext dbContext;

			if (UseContextFactory)
			{
				dbContext = serviceProvider.GetRequiredService<IDbContextFactory<TContext>>().CreateDbContext();
			}
			else
			{
				dbContext = serviceProvider.GetRequiredService<TContext>();
			}
			
			return new SqlServerRepository<T>(dbContext);
		});
	}
}
internal sealed class SqlServerRepository<T> : Repository<T> where T : class, new()
{
	private readonly DbSet<T> dbSet;
	
	public SqlServerRepository(DbContext dbContext)
	{
		this.dbSet = dbContext.Set<T>();
	}

	public override Type ElementType => (dbSet as IQueryable<T>).ElementType;
	public override Expression Expression => (dbSet as IQueryable<T>).Expression;
	public override IQueryProvider Provider => (dbSet as IQueryable<T>).Provider;
	public override IEnumerator<T> GetEnumerator() 
	{
		return (dbSet as IQueryable<T>).GetEnumerator();
	}

	public override Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public override IBatchRepositoryContext<T> CreateBatchContext()
	{
		throw new NotImplementedException();
	}

	public override Task<int> DeleteAsync(Expression<Func<T, bool>> predicate)
	{
		throw new NotImplementedException();
	}

	public override Task<T> DeleteAsync(object[] keys, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public override Task<int> ExecuteAsync(string operationName, object[]? arguments = null, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public override Task<T> GetAsync(object[] keys, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public override Task<int> UpdateAsync(Expression<Func<T, bool>> predicate)
	{
		throw new NotImplementedException();
	}
	public override Task<T> UpdateAsync(object[] keys, Action<T> configure, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
}

public static class SqlServerRepositoryFactoryBuilderExtensions
{
	public static IRepositoryFactoryBuilder UseSqlServer<TContext>(this IRepositoryFactoryBuilder builder, Action<SqlServerRepositoryOptions<TContext>> configure)
		where TContext : DbContext
	{
		if (configure is null)
		{
			throw new ArgumentNullException(nameof(configure));
		}

		var options = new SqlServerRepositoryOptions<TContext>(builder);

		configure.Invoke(options);

		return builder;
	}
}