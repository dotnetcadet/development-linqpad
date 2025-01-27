<Query Kind="Program" />

void Main()
{
	
}


public record class Email : IEquatable<Email>
{
	public Email(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			throw new ArgumentNullException(nameof(value));
		}
		
		Value = value;
	}
	
	public string Value { get; }
	
	
	
	
	
	
	
	
	
	
}