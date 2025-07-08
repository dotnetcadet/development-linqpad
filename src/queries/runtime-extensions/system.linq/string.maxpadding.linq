<Query Kind="Program" />

void Main()
{
	IEnumerable<string> items = [
		"ab",
		"abc",
		"abcd",
		"test 1",
		"test 34",
		"test 1"
	];
	
	
	
	foreach (var item in items.MaxPadLeft(5, '.'))
	{
		item.Dump();
	}
}


public static class StringExtensions
{
	/// <summary>
	/// 
	/// </summary>
	public static IEnumerable<string> MaxPadRight(this IEnumerable<string> source, int width)
	{
		return source.MaxPadRight(width, ' ');
	}
	
	/// <summary>
	/// 
	/// </summary>
	public static IEnumerable<string> MaxPadRight(this IEnumerable<string> source, int width, char paddingChar)
	{
		var count = source.GetCount();
		var max = source.Max()!;
		var length = max.Length + count;
		
		foreach (var item in source)
		{
			yield return item.PadRight(length, paddingChar);
		}
	}


	public static IEnumerable<string> MaxPadLeft(this IEnumerable<string> source, int width)
	{
		return source.MaxPadLeft(width, ' ');
	}


	public static IEnumerable<string> MaxPadLeft(this IEnumerable<string> source, int width, char paddingChar)
	{
		var count = source.GetCount();
		var max = source.Max()!;
		var length = max.Length + count;

		foreach (var item in source)
		{
			yield return item.PadLeft(length, paddingChar);
		}
	}
	



	private static int GetCount<T>(this IEnumerable<T> source)
	{
		return source.TryGetNonEnumeratedCount(out var count) ? 
			count :
			source.GetCount();
	}
}


