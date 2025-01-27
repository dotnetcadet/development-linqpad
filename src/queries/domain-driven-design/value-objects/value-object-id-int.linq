<Query Kind="Program">
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <RuntimeVersion>6.0</RuntimeVersion>
</Query>

void Main()
{
	IntId? i = 200;
	IntId? i1 = 1;
	
	
	if (i == i1) {
		
	}
	

}


public readonly struct IntId : IEquatable<IntId>, IComparable<IntId>
{
	public IntId(int value)
	{
		Value = value;
	}

	public int Value { get; }

	#region Overloads

	public override bool Equals(object instance)
	{
		if (instance is IntId id)
		{
			
		}
		
		return false;
	}

	public override string ToString()
	{
		return base.ToString();
	}

	public override int GetHashCode()
	{
		return Value;
	}


	#endregion

	
	int IComparable<IntId>.CompareTo(IntId other)
	{
		return Value.CompareTo(other);
	}
	bool IEquatable<IntId>.Equals(IntId other)
	{
		return Value == other.Value;
	}
	
	#region Operators
	
	public static bool operator ==(IntId left, IntId right)
	{
		return left.Equals(right);
	}
	public static bool operator !=(IntId left, IntId right)
	{
		return !left.Equals(right);
	}
	public static implicit operator int(IntId id) 
	{
		return id.Value;
	}
	public static implicit operator IntId(int value)
	{
		return new IntId(value);
	}
	
	#endregion
}
