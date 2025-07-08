<Query Kind="Program">
  <NuGetReference>Azure.Identity</NuGetReference>
  <NuGetReference>Azure.Monitor.Query</NuGetReference>
  <NuGetReference>Microsoft.EntityFrameworkCore</NuGetReference>
  <NuGetReference>Microsoft.EntityFrameworkCore.Relational</NuGetReference>
  <Namespace>Azure.Identity</Namespace>
  <Namespace>Azure.Monitor.Query</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Infrastructure</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Metadata</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Update</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Diagnostics</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Storage.ValueConversion</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.ChangeTracking</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore.Storage.Json</Namespace>
  <Namespace>Azure.Core</Namespace>
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <Namespace>Azure.Monitor.Query.Models</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
</Query>


private const string TenantId = "";
private const string WorkspaceId = ""; // Workspace ID - Log Analytics Workspace ID
private const string MintBrowserErrors = """
    AppExceptions 
    | where ClientType == 'Browser'
    | project Id = IKey, Host = AppRoleName, Page = OperationName, Message = OuterMessage, Username = tostring(Properties.username), UserId = tostring(Properties.userId), Browser = ClientBrowser, Timestamp = TimeGenerated
    """;

void Main()
{
	var credentials = new DefaultAzureCredential(new DefaultAzureCredentialOptions()
	{
		TenantId = TenantId,
		ExcludeEnvironmentCredential = true,
		ExcludeWorkloadIdentityCredential = true,
	});

	var client = new LogsQueryClient(credentials);
	
	var queryable = client.ToQueryableWorkspace<BrowserErrorLogEntry>(
		WorkspaceId,
		MintBrowserErrors,
		QueryTimeRange.All);
		
	var results = queryable
		.Where(p => (p.Message != null && p.Message.Contains("Host")) && p.Browser!.StartsWith("edge"))
		.Take(50)
		.ToList();
}

#region Models
public abstract class Entity { }
public enum LogEntryType
{
	Error,
	Metrics
}
public abstract class LogEntry : Entity
{
	public virtual string? Id { get; set; }
	public abstract LogEntryType EntryType { get; }
}
public class BrowserErrorLogEntry : LogEntry
{
	public string? Host { get; set; }
	public string? Page { get; set; }
	public string? Message { get; set; }
	public string? UserId { get; set; }
	public string? Browser { get; set; }
	public DateTimeOffset? Timestamp { get; set; }
	public override LogEntryType EntryType => LogEntryType.Error;
}
#endregion

public static class LogQueryClientExtensions
{
	public static IQueryable<T> ToQueryableWorkspace<T>(
		this LogsQueryClient client,
		string workspaceId,
		string query,
		QueryTimeRange queryTimeRange) where T : LogEntry, new()
	{
		return new LogQueryClientQueryable<T>(
			client,
			new StringBuilder(query))
		{
			WorkspaceId = workspaceId
		};
	}
}


internal class LogQueryClientQueryable<T> : IQueryable<T>, IQueryProvider
		where T : new()
{
	private readonly LogsQueryClient client;
	private readonly StringBuilder query;

	public LogQueryClientQueryable(LogsQueryClient client, StringBuilder query)
	{
		this.client = client;
		this.query = query;
		this.Expression = Expression.Constant(this);
	}

	public required string WorkspaceId { get; init; }
	public Type ElementType => typeof(T);
	public Expression Expression { get; }
	public IQueryProvider Provider => this;
	public object? Execute(Expression expression)
	{
		throw new NotImplementedException();
	}
	public TResult Execute<TResult>(Expression expression)
	{
		return Execute(expression) is TResult result ?
			result :
			throw new Exception();
	}

	public IEnumerator<T> GetEnumerator()
	{
		var stringQuery = query.ToString();

		var response = client.QueryWorkspace(
			WorkspaceId,
			stringQuery,
			QueryTimeRange.All);

		return new LogQueryClientResultEnumerator<T>(response.Value.Table);
	}
	IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
	public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
	{
		if (this is not IQueryable<TElement> element)
		{
			throw new InvalidOperationException();
		}
		return (CreateQuery(expression) as IQueryable<TElement>)!;
	}
	public IQueryable CreateQuery(Expression expression)
	{
		return ParseRoot(expression);
	}

	public IQueryable ParseRoot(Expression expression)
	{
		var queryPart = expression switch
		{
			MethodCallExpression methodCall when methodCall.Method.Name == "Skip" => throw new InvalidOperationException(""),
			MethodCallExpression methodCall when methodCall.Method.Name == "Take" => ParseTake(methodCall),
			MethodCallExpression methodCall when methodCall.Method.Name == "Where" => ParseWhere(methodCall),
			MethodCallExpression methodCall when methodCall.Method.Name == "Select" => ParseSelect(methodCall),
			MethodCallExpression methodCall when methodCall.Method.Name == "OrderBy" => ParseOrderBy(methodCall),
			MethodCallExpression methodCall when methodCall.Method.Name == "OrderByDescending" => ParseOrderByDescending(methodCall),
			MethodCallExpression methodCall when methodCall.Method.Name == "ThenBy" => ParseThenBy(methodCall),
			MethodCallExpression methodCall when methodCall.Method.Name == "ThenByDescending" => ParseThenByDescending(methodCall),

			_ => throw new InvalidOperationException("Unsupported Operation")
		};

		query.AppendLine();
		query.Append($"| {queryPart}");

		return this;
	}

	private string ParseSelect(MethodCallExpression expression)
	{
		var projections = expression.Arguments[^1] switch
		{
			UnaryExpression unary => ParseUnary(unary),
			_ => throw new InvalidExpressionException()
		};

		return "project " + projections;
	}

	private string ParseTake(MethodCallExpression expression)
	{
		var argument = expression.Arguments[^1];

		if (argument is not ConstantExpression constant)
		{
			throw new InvalidExpressionException();
		}

		return "take " + ParseConstant(constant);
	}
	private string ParseOrderBy(MethodCallExpression expression)
	{
		var argument = expression.Arguments[^1] switch
		{
			UnaryExpression unary => ParseUnary(unary)
		};

		return "order by " + argument + " asc";
	}
	private string ParseOrderByDescending(MethodCallExpression expression)
	{
		var argument = expression.Arguments[^1] switch
		{
			UnaryExpression unary => ParseUnary(unary)
		};

		return "order by " + argument + " desc";
	}
	private string ParseThenByDescending(MethodCallExpression expression)
	{
		var argument = expression.Arguments[^1] switch
		{
			UnaryExpression unary => ParseUnary(unary)
		};

		var after = expression.Arguments[0] switch
		{
			MethodCallExpression methodCall when methodCall.Method.Name == "OrderBy" => ParseOrderBy(methodCall),
			MethodCallExpression methodCall when methodCall.Method.Name == "OrderByDescending" => ParseOrderByDescending(methodCall),
			MethodCallExpression methodCall when methodCall.Method.Name == "ThenBy" => ParseThenBy(methodCall),
			MethodCallExpression methodCall when methodCall.Method.Name == "ThenByDescending" => ParseThenByDescending(methodCall),
		};

		return after + ", " + argument + " desc";
	}
	private string ParseThenBy(MethodCallExpression expression)
	{
		var argument = expression.Arguments[^1] switch
		{
			UnaryExpression unary => ParseUnary(unary)
		};

		var after = expression.Arguments[0] switch
		{
			MethodCallExpression methodCall when methodCall.Method.Name == "OrderBy" => ParseOrderBy(methodCall),
			MethodCallExpression methodCall when methodCall.Method.Name == "OrderByDescending" => ParseOrderByDescending(methodCall),
			MethodCallExpression methodCall when methodCall.Method.Name == "ThenBy" => ParseThenBy(methodCall),
			MethodCallExpression methodCall when methodCall.Method.Name == "ThenByDescending" => ParseThenByDescending(methodCall),
		};

		return after + ", " + argument + " asc";
	}
	private string ParseWhere(MethodCallExpression expression)
	{
		var argument = expression.Arguments[^1];
		var binary = argument switch
		{
			UnaryExpression unaryExpression when unaryExpression.Operand is LambdaExpression lambda => ParseLambda(lambda),
			LambdaExpression lambdaExpression => ParseLambda(lambdaExpression),
			_ => throw new InvalidOperationException()
		};

		return "where " + binary;
	}
	private string ParseLambda(LambdaExpression expression)
	{
		if (expression.Body is BinaryExpression binaryExpression)
		{
			return ParseBinary(binaryExpression);
		}
		if (expression.Body is MemberExpression member)
		{
			return ParseMember(member);
		}
		if (expression.Body is MemberInitExpression memberInit)
		{
			return ParseMemberInit(memberInit);
		}

		throw new InvalidOperationException();
	}
	private string ParseMemberInit(MemberInitExpression expression)
	{
		return string.Join(',', expression.Bindings.Select(p => p.Member.Name));
	}
	private string ParseBinary(BinaryExpression expression)
	{
		var opr = expression.NodeType switch
		{
			ExpressionType.OrElse => "or",
			ExpressionType.AndAlso => "and",
			ExpressionType.Equal => "==",
			ExpressionType.NotEqual => "!=",
			ExpressionType.GreaterThan => ">",
			ExpressionType.GreaterThanOrEqual => ">=",
			ExpressionType.LessThan => "<",
			ExpressionType.LessThanOrEqual => "<=",
			_ => throw new InvalidOperationException($"Operator is not supported: {expression.NodeType} in expression {expression}")
		};
		var left = expression.Left switch
		{
			BinaryExpression binary => ParseBinary(binary),
			MemberExpression member => ParseMember(member),
			ConstantExpression constant => ParseConstant(constant),
			MethodCallExpression function => ParseFunction(function),
			UnaryExpression unary => ParseUnary(unary),

			_ => throw new InvalidOperationException()
		};
		var right = expression.Right switch
		{
			BinaryExpression binary => ParseBinary(binary),
			MemberExpression member => ParseMember(member),
			ConstantExpression constant => ParseConstant(constant),
			MethodCallExpression function => ParseFunction(function),
			UnaryExpression unary => ParseUnary(unary),

			_ => throw new InvalidOperationException()
		};

		// Kusto has no 'null' literals. Only functions for null checks
		if (right is null && opr == "!=")
		{
			return $"(isnotnull({left}) and isnotempty({left}))";
		}
		if (right is null && opr == "==")
		{
			return $"(isnull({left}) and isempty({left}))";
		}

		return "(" + string.Join($" {opr} ", left, right) + ")";
	}
	private string ParseUnary(UnaryExpression expression)
	{
		if (expression.NodeType == ExpressionType.Not)
		{
			return expression.Operand switch
			{
				MethodCallExpression function => "(" + ParseFunction(function) + ") == false",
				BinaryExpression binary => "(" + ParseBinary(binary) + ") == false"
			};
		}
		if (expression.NodeType == ExpressionType.Convert)
		{
			var delegateType = typeof(Func<>).MakeGenericType(expression.Type);
			var lambda = Expression.Lambda(delegateType, expression);
			var func = lambda.Compile();
			var value = func.DynamicInvoke();

			return FormatValue(value);
		}
		if (expression.NodeType == ExpressionType.Quote)
		{
			return expression.Operand switch
			{
				LambdaExpression lambda => ParseLambda(lambda)
			};
		}

		throw new InvalidExpressionException();
	}
	private string ParseMember(MemberExpression expression)
	{
		return expression.Member.Name;
	}
	private string ParseConstant(ConstantExpression expression)
	{
		var value = expression.Value;

		return FormatValue(value);
	}
	private string ParseFunction(MethodCallExpression expression)
	{
		// Two Arguments: Most likely extension method
		if (expression.Arguments.Count == 2)
		{
			var left = expression.Arguments[0] switch
			{
				MemberExpression member => ParseMember(member),
				ConstantExpression constant => ParseConstant(constant)
			};
			var right = expression.Arguments[1] switch
			{
				MemberExpression member => ParseMember(member),
				ConstantExpression constant => ParseConstant(constant),
				UnaryExpression => ParseUnary((UnaryExpression)expression.Arguments[0])
			};
			var method = expression.Method.Name switch
			{
				"Contains" => "contains",
				"StartsWith" => "startswith",
				"EndsWith" => "endswith",
				_ => throw new InvalidExpressionException()
			};
			if (left is IEnumerable && method == "contains")
			{
				return string.Join(" in ", right, left);
			}
		}
		//One Argument: Most likely function declared inside type
		if (expression.Arguments.Count == 1)
		{
			var identifier = expression.Object switch
			{
				MemberExpression member => ParseMember(member),

				_ => throw new InvalidExpressionException()
			};

			var method = expression.Method.Name switch
			{
				"Contains" => "contains",
				"StartsWith" => "startswith",
				"EndsWith" => "endswith",

				_ => throw new InvalidExpressionException()
			};

			var constant = expression.Arguments[0] switch
			{
				ConstantExpression => ParseConstant((ConstantExpression)expression.Arguments[0]),
				UnaryExpression => ParseUnary((UnaryExpression)expression.Arguments[0])
			};

			return string.Join(' ', identifier, method, constant);
		}
		throw new InvalidExpressionException();
	}

	private string FormatValue(object? value)
	{
		return value switch
		{
			string stringValue => ApplyQuotes(stringValue),
			DateOnly dateOnly => ApplyQuotes(dateOnly.ToString("yyyy-MM-dd")),
			DateTime dateTime => ApplyQuotes(dateTime.ToString("yyyy-MM-ddThh:mm:ss.zzz")),
			DateTimeOffset dateTimeOffset => ApplyQuotes(dateTimeOffset.ToString("yyyy-MM-ddThh:mm:ss.zzz")),
			TimeSpan timeSpan => ApplyQuotes(timeSpan.ToString("hh:mm:ss")),
			Guid guid => ApplyQuotes(guid.ToString()),
			int int32 => int32.ToString(),
			long int64 => int64.ToString(),
			short int16 => int16.ToString(),
			uint uint32 => uint32.ToString(),
			ulong uint64 => uint64.ToString(),
			ushort uint16 => uint16.ToString(),
			decimal dec => dec.ToString(),
			double db => db.ToString(),
			IEnumerable enumerable => ApplyBrackets(string.Join(",", FormatEnumerable(enumerable))),

			_ => TryFormat(value!.ToString()!)
		};

		IEnumerable<string> FormatEnumerable(IEnumerable enumerable)
		{
			foreach (var item in enumerable)
			{
				yield return FormatValue(item);
			}
		}
		string TryFormat(string value)
		{
			IEnumerable<Func<string, bool>> tests =
			[
				v => short.TryParse(v, out var a),
				v => int.TryParse(v, out var a),
				v => long.TryParse(v, out var a),
				v => ushort.TryParse(v, out var a),
				v => uint.TryParse(v, out var a),
				v => ulong.TryParse(v, out var a),
				v => decimal.TryParse(v, out var a),
				v => double.TryParse(v, out var a)
			];

			foreach (var test in tests)
			{
				if (test.Invoke(value))
				{
					return value;
				}
			}
			return ApplyQuotes(value);
		}
		string ApplyBrackets(string value)
		{
			return "(" + value + ")";
		}
		string ApplyQuotes(string value)
		{
			return "'" + value + "'";
		}
	}
}

internal class LogQueryClientResultEnumerator<T> : IEnumerator<T>
		where T : new()
{
	private int index = -1;
	private readonly LogsTable table;
	private readonly Type Type = typeof(T);
	private readonly string TypeName = typeof(T).Name;
	private static readonly ConcurrentDictionary<PropertyInfo, Func<object, object>> converters = new();
	private static readonly Dictionary<Type, Func<object, object?>> knownConverters = new()
	{
		{ typeof(string), value => value is null ? null : value.ToString() },
		{ typeof(short) , ParseShort },
		{ typeof(short?) , ParseShort },
		{ typeof(int) , ParseInt },
		{ typeof(int?) , ParseInt },
		{ typeof(long) , ParseLong },
		{ typeof(long?) , ParseLong },
		{ typeof(ushort) , ParseUShort },
		{ typeof(ushort?) , ParseUShort },
		{ typeof(uint) , ParseUInt },
		{ typeof(uint?) , ParseUInt },
		{ typeof(ulong) , ParseULong },
		{ typeof(ulong?) , ParseULong },
		{ typeof(DateOnly) , ParseDateOnly },
		{ typeof(DateOnly?) , ParseDateOnly },
		{ typeof(DateTime) , ParseDateTime },
		{ typeof(DateTime?) , ParseDateTime },
		{ typeof(DateTimeOffset) , ParseDateTimeOffset },
		{ typeof(DateTimeOffset?) , ParseDateTimeOffset },
		{ typeof(IDictionary<string, string>), value =>
		{
			if (value is BinaryData binaryData)
			{
				return binaryData.ToObjectFromJson<IDictionary<string, string>>();
			}
			throw new Exception();
		} }
	};

	public LogQueryClientResultEnumerator(LogsTable table)
	{
		this.table = table;
	}

	public T Current { get; set; } = default!;
	object IEnumerator.Current => Current!;
	public void Dispose() { }
	public bool MoveNext()
	{
		index++;

		if (index >= table.Rows.Count)
		{
			return false;
		}

		var row = table.Rows[index];
		// set 
		Current = new();

		foreach (var column in table.Columns)
		{
			var propertyInfo = Type.GetProperty(
				column.Name,
				BindingFlags.Public | BindingFlags.Instance);

			if (propertyInfo is null)
			{
				throw new Exception();
			}

			var converter = converters.GetOrAdd(propertyInfo, info =>
			{
				var propertyType = info.PropertyType;

				if (knownConverters.TryGetValue(propertyType, out var converter))
				{
					return converter!;
				}
				var genericArguments = propertyType.GetGenericArguments();
				var isNullableEnum = (genericArguments.Length == 1 && propertyType == typeof(Nullable<>).MakeGenericType(genericArguments[0]));

				if (propertyType.IsEnum || isNullableEnum)
				{
					return value =>
					{
						if (value is null) return Activator.CreateInstance(propertyType)!;
						if (value is string stringValue)
						{
							if (string.IsNullOrEmpty(stringValue))
							{
								return Activator.CreateInstance(propertyType)!;
							}

							return isNullableEnum ?
								Enum.Parse(genericArguments[0], stringValue) :
								Enum.Parse(propertyType, stringValue);

						}

						throw new Exception();
					};
				}

				var methodInfo = propertyType.GetMethods(BindingFlags.Public | BindingFlags.Static)
					.FirstOrDefault(method =>
					{
						var parameters = method.GetParameters();
						if (method.Name == "Parse" && parameters.Length == 1 && parameters[0].ParameterType == typeof(string))
						{
							return true;
						}
						return false;
					});

				if (methodInfo is not null && methodInfo.ReturnType.IsAssignableTo(propertyType))
				{
					return value =>
					{
						if (value is string stringValue && !string.IsNullOrEmpty(stringValue))
						{
							return methodInfo.Invoke(null, [stringValue])!;
						}
						if (methodInfo.ReturnType.IsValueType)
						{
							return Activator.CreateInstance(methodInfo.ReturnType);
						}

						throw new Exception();
					};
				}

				throw new Exception();
			});

			var value = converter.Invoke(row[column.Name]);

			propertyInfo.SetValue(Current, value);
		}

		return true;
	}
	public void Reset()
	{
		index = -1;
	}


	private static object? ParseShort(object value)
	{
		if (value is null) return value;
		if (value is short || value is int || value is long) return (short)value;
		if (value is string stringValue) return short.Parse(stringValue);
		return value;
	}
	private static object? ParseInt(object value)
	{
		if (value is null) return value;
		if (value is short || value is int || value is long) return (int)value;
		if (value is string stringValue) return int.Parse(stringValue);
		return value;
	}
	private static object? ParseLong(object value)
	{
		if (value is null) return value;
		if (value is short || value is int || value is long) return (long)value;
		if (value is string stringValue) return long.Parse(stringValue);
		return value;
	}
	private static object? ParseUShort(object value)
	{
		if (value is null) return value;
		if (value is short || value is int || value is long) return (ushort)value;
		if (value is string stringValue) return ushort.Parse(stringValue);
		return value;
	}
	private static object? ParseUInt(object value)
	{
		if (value is null) return value;
		if (value is short || value is int || value is long) return (uint)value;
		if (value is string stringValue) return uint.Parse(stringValue);
		return value;
	}
	private static object? ParseULong(object value)
	{
		if (value is null) return value;
		if (value is short || value is int || value is long) return (ulong)value;
		if (value is string stringValue) return ulong.Parse(stringValue);
		return value;
	}
	private static object? ParseDateOnly(object value)
	{
		if (value is null) return value;
		if (value is DateOnly dateOnly) return dateOnly;
		if (value is string stringValue) return DateOnly.Parse(stringValue);
		return value;
	}
	private static object? ParseDateTime(object value)
	{
		if (value is null) return value;
		if (value is DateTime dateTime) return dateTime;
		if (value is string stringValue) return DateTime.Parse(stringValue);
		return value;
	}
	private static object? ParseDateTimeOffset(object value)
	{
		if (value is null) return value;
		if (value is DateTimeOffset dateTimeOffset) return dateTimeOffset;
		if (value is string stringValue) return DateTimeOffset.Parse(stringValue);
		return value;

	}
}