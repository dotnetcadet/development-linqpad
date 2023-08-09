<Query Kind="Program" />

void Main()
{
	var bytes = File.ReadAllBytes(@"C:\source\repos\assimalign\github\ograph-dotnet\.designing\query.ograph");

	var buffer = RemoveWhiteSpace(bytes
		.Where(x => (char)x != '\n' && (char)x != '\r' && (char)x != '\t')
		.ToArray());
	Encoding.UTF8.GetString(buffer).Dump();
	buffer.Length.Dump();



}

public byte[] RemoveWhiteSpace(byte[] bytes)
{

	int current = 0;
	char[] output = new char[bytes.Length];
	bool skipped = false;

	foreach (char c in bytes.Select(x=> (char)x))
	{
		if (char.IsWhiteSpace(c))
		{
			if (!skipped)
			{
				if (current > 0)
					output[current++] = ' ';

				skipped = true;
			}
		}
		else
		{
			skipped = false;
			output[current++] = c;
		}
	}

	return output.Select(x=> (byte)x).Take(current).ToArray();
}


public class User
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
}

