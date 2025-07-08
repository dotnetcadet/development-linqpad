<Query Kind="Program" />



void Main()
{
	var strs = new string[]
	{
		"employees",
		"employeeeJobs",
		"employeesTasks",
		"employeeWorkItems",
		"test1",
		"test2",
		"test3",
		"test4",
		"test5"
	};
	
	Statistic.Permutations(strs.Length, strs.Length, true).Dump();
	Statistic.Permutations(strs.Length, strs.Length, false).Dump();

	
	strs.Permutate(strs.Length, true).Count().Dump();
	
	
	//foreach (var permutation in strs.Permutate(4))
	//{
	//	
	//	//var choice = new string[permutation.Length];
	//	//
	//	//for (int i = 0; i < permutation.Length; i++)
	//	//{
	//	//	choice[i] = strs[permutation[i]];
	//	//}
	//	//
	//	string.Join('/', permutation).Dump();
	//	i++;
	//}

	

	
	//Test();
	
	// repeatable: 10 * 10 * 10 * 10
	// not repeatable: 10 * 9 * 8 * 7
	//Statistic.Purmutations(10, 4).Dump();
	//
	//Statistic.Combinations(10, 4, false).Dump();

	/*
	
		
	*/
	//Generate(strs).Count().Dump();
	//foreach (var path in Generate(strs))
	//{
	//	path.Dump();
	//}
	
	//CalculatePermutation(6).Dump();
	//var combinations = Calculate(strs.Length).Dump();
	//
	//for (int i = 0; i < combinations; i++) 
	//{
	//	
	//}
}


public static class Statistic
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="count">The total number of items to select from.</param>
	/// <param name="choices">How many </param>
	/// <param name="repeatable">Flags whether or not an item can be reused in each choice.</param>
	public static double Permutations(double count, double choices, bool repeatable = true)
	{
		if (repeatable) 
		{
			return Math.Pow(count, choices);
		}
		
		return Factorial(count, (count - choices));
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="count">The total number of items to select from.</param>
	/// <param name="choices">How many </param>
	/// <param name="repeatable">Flags whether or not an item can be reused in each choice.</param>
	public static double Combinations(double count, double choices, bool repeatable = true)
	{
		if (repeatable)
		{
			return Factorial((choices + count) - 1) / (Factorial(choices) * Factorial(count - 1));
		}
		
		return Factorial(count) / (Factorial(choices) * Factorial((count - choices))); 
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="value">The total number of items to select from.</param>
	/// <param name="boundary">How many </param>
	public static double Factorial(double value, double boundary = 0)
	{
		var a = value;
		
		for (double i = a - 1; i > boundary; i--)
		{
			a = a * i;
		}
		
		return a;
	}
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="choices">The number of possible choices to select from the items.</param>
	/// <param name="repeatable">Specifies whether an item can be selected multiple times.</param>
	public static IEnumerable<T[]> Permutate<T>(this IEnumerable<T> items, int choices, bool repeatable = true)
	{
		if (choices == 1)
		{
			return items.Select(t => new T[] { t });
		}
		
		if (repeatable)
		{
			return items.Permutate(choices - 1, repeatable)
				.SelectMany(
					_ => items,
					(t1, t2) => t1.Concat(new T[] { t2 }).ToArray());
		}
		else
		{
			return items.Permutate(choices - 1, repeatable)
				.SelectMany(
					(t) => items.Where(e => !t.Contains(e)),
					(t1, t2) => t1.Concat(new T[] { t2 }).ToArray());
		}
	}
}




private static void Swap(ref char a, ref char b)
{
	if (a == b) return;

	var temp = a;
	a = b;
	b = temp;
}

public static void GetPer(char[] list)
{
	int x = list.Length - 1;
	GetPer(list, 0, x);
}

private static void GetPer(char[] list, int k, int m)
{
	if (k == m)
	{
		Console.Write(list);
	}
	else
		for (int i = k; i <= m; i++)
		{
			Swap(ref list[k], ref list[i]);
			GetPer(list, k + 1, m);
			Swap(ref list[k], ref list[i]);
		}
}
