<Query Kind="Program">
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
</Query>

void Main()
{
	var size = new FileSize(124583992);
	
	size.AsGigabytes().Dump();
	
	FileSystemInfo fsi;
	
	System.IO.Path.GetInvalidFileNameChars().Dump();
	
}




public readonly struct FileSize : IEquatable<FileSize>, IComparable<FileSize>, IEqualityComparer<FileSize>
{
	private const long kilobyte = 1000;
	private const long megabyte = kilobyte * 1000;
	private const long gigabyte = megabyte * 1000;
	private const long terabyte = gigabyte * 1000;

	public FileSize(long length)
	{
		Length = length;
	}

	/// <summary>
	/// The actual file length in bytes.
	/// </summary>
	public long Length { get; }
	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	public decimal AsKilobytes()
	{
		return (decimal)Length / kilobyte;
	}
	/// <summary>
	/// Returns the length as megabytes
	/// </summary>
	/// <returns></returns>
	public decimal AsMegabytes()
	{
		return (decimal)Length / megabyte;
	}
	/// <summary>
	/// Returns the length as megabytes
	/// </summary>
	/// <returns></returns>
	public decimal AsGigabytes()
	{
		return (decimal)Length / gigabyte;
	}
	
	#region Overloads

	/// <inheritdoc />
	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		return obj is FileSize size ? Equals(size) : false;
	}

	/// <inheritdoc />
	public override string ToString()
	{
		return base.ToString();
	}

	/// <inheritdoc />
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	#endregion

	#region Interfaces

	/// <inheritdoc />
	public bool Equals(FileSize other)
	{
		return other.Length == Length;
	}

	/// <inheritdoc />
	public bool Equals(FileSize left, FileSize right)
	{
		return left.Equals(right);
	}

	/// <inheritdoc />
	public int GetHashCode([DisallowNull] FileSize obj)
	{
		throw new NotImplementedException();
	}

	public int CompareTo(FileSize other)
	{
		return Length.CompareTo(other.Length);
	}
	#endregion

	#region Operators
	public static implicit operator long(FileSize fileSize) => fileSize.Length;
	public static bool operator ==(FileSize left, FileSize right) => left.Equals(right);
	public static bool operator !=(FileSize left, FileSize right) => !left.Equals(right);
	public static bool operator >(FileSize left, FileSize right) => left.CompareTo(right) > 0;
	public static bool operator <(FileSize left, FileSize right) => left.CompareTo(right) < 0;
	public static bool operator >=(FileSize left, FileSize right) => left.CompareTo(right) >= 0;
	public static bool operator <=(FileSize left, FileSize right) => left.CompareTo(right) <= 0;
	#endregion

	#region Helpers
	/// <summary>
	/// Returns a file size in kilobytes
	/// </summary>
	/// <param name="size"></param>
	/// <returns></returns>
	public static FileSize FromKilobytes(decimal size)
	{
		return new FileSize((long)(size * kilobyte));
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="size"></param>
	/// <returns></returns>
	public static FileSize FromMegabytes(long size)
	{
		return new FileSize(size * megabyte);
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="size"></param>
	/// <returns></returns>
	public static FileSize FromGigabytes(long size)
	{
		return new FileSize(size * gigabyte);
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="size"></param>
	/// <returns></returns>
	public static FileSize FromTerabytes(long size)
	{
		return new FileSize(size * terabyte);
	}
	#endregion
}