<Query Kind="Program">
  <NuGetReference>Azure.Data.Tables</NuGetReference>
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <Namespace>Azure.Data.Tables</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Azure</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
</Query>

void Main()
{

	
	var test = new TableStorageQueryable<Person>();
	
	test.Where(x=>x.FirstName == "Chase").Skip(0).Take(20).ToArray();
}

public class Person : TableStorageEntity<Person>
{
	public string FirstName { get; set; }
}



public class TableStorageEntity<TEntity> : ITableEntity
{
	public string PartitionKey { get; set; }
	public string RowKey { get; set; }
	public TEntity Entity { get; set; }
	public DateTimeOffset? Timestamp { get; set; }
	public ETag ETag { get; set; }
}

internal class TableStorageQueryable<T> : IQueryable<T>, IQueryProvider
	where T : class, ITableEntity
{
	private readonly Expression expression;
	private readonly TableClient client;

	public TableStorageQueryable()
	{
		this.expression = Expression.Constant(this);
	}
	
	
	private int skip;
	private int take = 50;
	private string filter;
	
	
	private IQueryable ParseSkip(MethodCallExpression expression)
	{
		if (expression.Arguments.Last() is ConstantExpression constant)
		{
			skip = (int)constant.Value;
		}
		return this;
	}
	private IQueryable ParseTake(MethodCallExpression expression)
	{
		if (expression.Arguments.Last() is ConstantExpression constant)
		{
			take = (int)constant.Value;
		}
		return this;
	}
	private IQueryable ParseWhere(MethodCallExpression expression)
	{
		if (expression.Arguments.Last() is UnaryExpression unary && unary.Operand is Expression<Func<T, bool>> lambda1)
		{
			filter = TableClient.CreateQueryFilter(lambda1);
		}
		else if (expression.Arguments.Last() is Expression<Func<T, bool>> lambda2)
		{
			filter = TableClient.CreateQueryFilter(lambda2);
		}
		return this;
	}

	public IQueryable CreateQuery(Expression expression)
	{
		return expression switch 
		{
			MethodCallExpression methodCall when methodCall.Method.Name == "Skip" => ParseSkip(methodCall),
			MethodCallExpression methodCall when methodCall.Method.Name == "Take" => ParseTake(methodCall),
			MethodCallExpression methodCall when methodCall.Method.Name == "Where" => ParseWhere(methodCall),
			MethodCallExpression methodCall when methodCall.Method.Name == "Select" => ParseWhere(methodCall),
			
			_ => throw new InvalidOperationException("Unsupported Operation")
		};
	}
	public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
	{
		if (this is not IQueryable<TElement> element)
		{
			throw new InvalidOperationException();
		}
		
		return CreateQuery(expression) as IQueryable<TElement>;
	}

	public object? Execute(Expression expression)
	{
		throw new NotImplementedException();
	}
	public TResult Execute<TResult>(Expression expression)
	{
		throw new NotImplementedException();
	}

	public Type ElementType => typeof(T);
	public Expression Expression => this.expression;
	public IQueryProvider Provider =>  this;


	private static ConcurrentDictionary<string, Pageable<T>> cache = new();
	
	
	public IEnumerator<T> GetEnumerator()
	{
		using (var sha256 = SHA256.Create())
		{
			var bytes = Encoding.UTF8.GetBytes(filter + $" take {take}");

			// ComputeHash - returns byte array  
			var hashArray = sha256.ComputeHash(bytes);

			// Convert byte array to a string   
			var builder = new StringBuilder();

			for (int i = 0; i < hashArray.Length; i++)
			{
				builder.Append(hashArray[i].ToString("x2"));
			}
			
			// Generate Query Hash
			var hash = builder.ToString();

			
			var pagable = cache.GetOrAdd(hash, key => 
			{
				return client.Query<T>(filter: filter, maxPerPage: take);
			});
			
			
			return new TableStoragePagableEnumerator<T>(pagable, skip, take);
		}
	}

	IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();


	partial class TableStoragePagableEnumerator<T> : IEnumerator<T>
		where T : class, ITableEntity
	{
		private IEnumerator<T> page;

		public TableStoragePagableEnumerator(Pageable<T> pageable, int skip, int take)
		{
			var pageIndex = 0;
			var pageNumber = (skip / take);

			// Check if the paging is starting on the first page
			if (pageNumber == 0)
			{
				page = pageable.AsPages().First().Values.GetEnumerator();
			}
			if (pageNumber > 0)
			{
				string? continuationToken = null;

				foreach (var result in pageable.AsPages(continuationToken))
				{
					continuationToken = result.ContinuationToken;

					pageIndex++;

					if (pageIndex == pageNumber)
					{
						break;
					}
				}
			}
		}

		public T Current => page.Current;

		object IEnumerator.Current => Current;

		public void Dispose()
		{
			page.Dispose();
		}

		public bool MoveNext()
		{
			return page.MoveNext();
		}

		public void Reset()
		{
			page.Reset();
		}
	}
}