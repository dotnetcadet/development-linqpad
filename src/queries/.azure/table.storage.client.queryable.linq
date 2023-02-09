<Query Kind="Program">
  <NuGetReference>Azure.Data.Tables</NuGetReference>
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <Namespace>Azure.Data.Tables</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Azure</Namespace>
</Query>



void Main()
{
	
	var test = new TableStorageQueryable<Person>();
	
	test.Where(x=>x.FirstName == "Chase").ToArray();
}

public class Person 
{
	public string FirstName { get; set; }
}



internal class TableStorageEntity<TEntity> : ITableEntity
{
	public string PartitionKey { get; set; }
	public string RowKey { get; set; }
	public TEntity Entity { get; set; }
	public DateTimeOffset? Timestamp { get; set; }
	public ETag ETag { get; set; }
}

internal class TableStorageQueryable<T> : IQueryable<T>, IQueryProvider
{
	private readonly Expression expression;
	private readonly TableClient client;
	
	private string filter;
	
	
	public TableStorageQueryable()
	{
		this.expression = System.Linq.Expressions.Expression.Constant(this);
	}
	
	
	
	public IQueryable CreateQuery(Expression expression)
	{
		switch (expression)
		{
			case MethodCallExpression methodCall when (methodCall.Method.Name == "Where"):
				{
					Expression<Func<TableStorageEntity<T>, T>> member = e => e.Entity;
					
					var filter = ((methodCall.Arguments
						?.First(x => x is UnaryExpression) as UnaryExpression)
						?.Operand as LambdaExpression) as Expression<Func<T, bool>>;

					if (filter is null)
					{
						throw new NotSupportedException();
					}
					
					

					var invocation = Expression.Invoke(filter, member.Body);
					var predicate = Expression.Lambda<Func<TableStorageEntity<T>, bool>>(invocation, member.Parameters);
					
					var builder = new ExpressionBuilder<TableStorageEntity<T>>();
					
					

					this.filter = TableClient.CreateQueryFilter<TableStorageEntity<T>>(test);
					break;
				}
			case UnaryExpression unary:
				{
					CreateQuery(unary.Operand);
					break;
				}
			case LambdaExpression lambda:
				{
					if (lambda is Expression<Func<T, bool>> filter)
					{
						this.filter = TableClient.CreateQueryFilter<TableStorageEntity<T>>(x => filter.Compile().Invoke(x.Entity));
					}
					break;
				}
			case ConstantExpression constant: 
				{
					
					break;
				}
			case MethodCallExpression methodCall when (methodCall.Method.Name == "Select"):
				{
					break;
				}
			default: 
			{
				throw new NotSupportedException();	
			}
		}
		
		return this;
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

	public IEnumerator<T> GetEnumerator()
	{
		throw new NotImplementedException();
	}

	IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}


internal class TableStorageQueryableExpressionVisitor : ExpressionVisitor
{
	
}

