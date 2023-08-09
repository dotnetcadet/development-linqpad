<Query Kind="Program">
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>


private static int index;

async Task Main()
{
	var str = "abcdefghijklmnopqrstuvwxzyABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
	var tasks1 = new List<Task<string>>();
	var tasks2 = new List<Task<string>>();

	var random = new Random();
	
	for (int i = 0; i < 25; i++)
	{
		tasks1.Add(Task.Run(() =>
		{
			index++;
			index.Dump();
			var next = random.Next(100, 1000);
			var chars = new char[next + 4];
			
			chars[0] = index.ToString()[0];
			chars[1] = ' ';
			chars[2] = '-';
			chars[3] = ' ';
			
			for (int a = 0; a < next; a++)
			{
				var chr = random.Next(0, str.Length);
				
				chars[a + 4] = str[chr];
			}
			
			return new string(chars);
		}));
	}

	await foreach (var result in ReadAsync(tasks1))
	{
		result.Dump();
	}

}

public async IAsyncEnumerable<string> ReadAsync(IList<Task<string>> tasks)
{
	while (tasks.Any())
	{
		var result = await Task.WhenAny(tasks);

		tasks.Remove(result);

		yield return result.Result;
	}
}


public async IAsyncEnumerable<string> WriteAsync(IList<Task<string>> tasks)
{
	while (tasks.Any())
	{
		var result = await Task.WhenAny(tasks);

		tasks.Remove(result);

		yield return result.Result;
	}
}