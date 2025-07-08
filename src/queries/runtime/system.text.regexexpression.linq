<Query Kind="Program" />

private const string AlphanumericPattern = "^[a-zA-Z0-9]+$";


void Main()
{
	Regex.IsMatch("Employee", AlphanumericPattern).Dump();
	
}



// You can define other methods, fields, classes and namespaces here