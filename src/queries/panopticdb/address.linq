<Query Kind="Program" />

void Main()
{
	long.MaxValue.Dump();
	var lg = (long)120393992003485;
	BitConverter.GetBytes(lg).Length.Dump();
	using var stream = new MemoryStream();
	
	int.MaxValue.Dump();
	short.MaxValue.Dump();
	var sh = (short)15;
	
	BitConverter.GetBytes(sh).Length.Dump();
	
}

/*
| Segment 1 (Root):			8 bytes
|
|---> Segment 1.1	- + 
|
|---> Segment 1.2

*/

public enum AddressType {
	Segment,
	Unit
}

/// <summary>
/// 
/// </summary>
/// <remarks>
/// root + depth offset
/// </remarks>
public readonly struct Address 
{
	private readonly byte[] buffer;
	
	private Address(byte[] buffer)
	{
		this.buffer = buffer;
	}

	/// <summary>
	/// A Segment or Unit Address should . In summary, no data should be nested more than 10 segments.
	/// </summary>
	public const ushort MaxDepth = 10;
	
	/// <summary>
	/// Represents the address depth.
	/// </summary>
	/// <remarks>
	/// Address Depth must match Segment or Unit Depth.
	/// </remarks>
	public ushort Depth { get; }
	
	
	
	//public Tuple<int, 



	//public static Address Empty() {
	//	
	//}
	
	
	
	
	
	// implicit operators
	
	
	
}