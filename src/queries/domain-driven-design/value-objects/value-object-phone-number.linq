<Query Kind="Program">
  <Namespace>System.Xml.Schema</Namespace>
</Query>

private const string expression = "^-?(?:[1-9]\\d{0,8}|1\\d{9}|20\\d{8}|21[0-3]\\d{7}|214[0-6]\\d{6}|2147[0-3]\\d{5}|21474[0-7]\\d{4}|214748[0-2]\\d{3}|2147483[0-5]\\d{2}|21474836[0-3]\\d|214748364[0-7])$";

void Main()
{
	
	
	var schema = new System.Xml.Schema.XmlSchema();
	
	Regex.IsMatch(Int32.MaxValue.ToString(), expression).Dump();
	
}

public readonly struct PhoneNumber {
	private static ReadOnlySpan<char> validCharacters => new char[] { '(', ')', ' ' , '0'};
	
	public PhoneNumber(string value)
	{
		Value = value;
	}
	
	public string  Value { get; }
	
	
	
	public static implicit operator PhoneNumber(string value) => new PhoneNumber(value);
}
