<Query Kind="Program">
  <Namespace>System.Collections.Concurrent</Namespace>
</Query>

void Main()
{
	var user = new User() { FirstName = "Chase" };
	Func<User, string> GetFirstName = x =>
	{
		return x.FirstName;
	};

	var func = Memoize(GetFirstName);
	var string1 = func.Invoke(user); // Will Call the Function 
	var string2 = func.Invoke(user); // Will Use cached results instead of function
	
}

public class User
{
	public string FirstName { get; set; }
}


public static Func<TIn, TOut> Memoize<TIn, TOut>(Func<TIn, TOut> method)
{
	var cache = new Dictionary<TIn, TOut>();
	return input => cache.TryGetValue(input, out var results) ?
		results :
		cache[input] = method(input);
}

public static class FuncCache<TIn, TOut> where TIn : notnull
{
	private static readonly ConcurrentDictionary<TIn, TOut> cache;
	
	static FuncCache()
	{
		cache = new();
	}

	public static Func<TIn, TOut> Memoize(Func<TIn, TOut> method)
	{
		return input => cache.TryGetValue(input, out var results) ?
			results :
			cache[input] = method(input);
	}
	
	public static void Clear() 
	{
		cache.Clear();
	}
}