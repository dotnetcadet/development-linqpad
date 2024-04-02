<Query Kind="Program">
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
</Query>

void Main()
{
	var left = Guid.NewGuid();
	var right = Guid.NewGuid();
	
	(left == right).Dump();
}


[DebuggerDisplay("{Value}")]
public readonly struct Uuid : IEquatable<Uuid>
{
	public Uuid(Guid value)
	{
		Value = value;
	}
	public Guid Value { get; }

	public bool Equals(Uuid other)
	{
		return Value.Equals(other.Value);
	}

	#region Overloads
	public override bool Equals([NotNullWhen(true)] object instance)
	{
		if (instance is Uuid id)
		{
			return Equals(id);
		}
		return false;
	}
	public override string ToString()
	{
		return Value.ToString();
	}
	public override int GetHashCode()
	{
		return HashCode.Combine(typeof(Uuid), Value);
	}
	#endregion

	#region Operators

	public static implicit operator Uuid(Guid value) => new Uuid(value);
	public static implicit operator Guid(Uuid id) => id.Value;


	public static bool operator ==(Uuid left, Uuid right) => left.Equals(right);
	public static bool operator !=(Uuid left, Uuid right) => !left.Equals(right);

	public static bool operator ==(Uuid? left, Uuid right) => left.Equals(right);
	public static bool operator !=(Uuid? left, Uuid right) => !left.Equals(right);
	public static bool operator ==(Uuid left, Uuid? right) => left.Equals(right);
	public static bool operator !=(Uuid left, Uuid? right) => !left.Equals(right);

	public static bool operator ==(Uuid? left, Uuid? right)
	{
		if (left.HasValue && right.HasValue)
		{
			return left.Value.Equals(right.Value);
		}
		return false;
	}
	public static bool operator !=(Uuid? left, Uuid? right)
	{
		if (left.HasValue && right.HasValue)
		{
			return !left.Value.Equals(right.Value);
		}
		return false;
	}
	#endregion
}