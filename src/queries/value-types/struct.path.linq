<Query Kind="Program">
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
</Query>

void Main()
{
	var directoryInfo = new DirectoryInfo(@"C:\Users\chase\");

	Enumerate(directoryInfo.EnumerateFileSystemInfos()).Select(p =>
	{
		Path path = p.FullName;
		return path;
	}).Dump();
}


private IEnumerable<FileSystemInfo> Enumerate(IEnumerable<FileSystemInfo> enumerable)
{
	foreach (var item in enumerable)
	{
		yield return item;

		if (item is DirectoryInfo children)
		{
			foreach (var child in Enumerate(children.EnumerateFileSystemInfos("*", new EnumerationOptions()
			{
				IgnoreInaccessible = true
			})))
			{
				yield return child;
			}
		}
	}
}


/// <summary>
/// A case-insensitive representation of a file path.
/// </summary>
/// <remarks>
/// Comparing file paths as strings can be dangerous as different OS's have 
/// case-sensitive file system such as linux. The following approach can be 
/// done with a class, or struct as well.
/// </remarks>
[DebuggerDisplay("{Value}")]
public readonly struct Path : IEquatable<Path>, IEqualityComparer<Path>, IComparable<Path>
{
	public Path(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			ThrowHelper.ThrowArgumentException($"{nameof(path)} cannot be null or empty.");
		}
		if (System.IO.Path.GetInvalidPathChars().Intersect(path).Any())
		{
			ThrowHelper.ThrowArgumentException($"{nameof(path)} contains illegal characters.");
		}

		var value = path.Trim(' ', '/', '\\');

		Value = string.Create(value.Length, value, (span, value) =>
		{
			value.CopyTo(span);

			// Let's convert all forward slashes to backward slashes
			for (int i = 0; i < span.Length; i++)
			{
				if (span[i] == '/')
				{
					span[i] = '\\';
				}
			}
		});
	}

	/// <summary>
	/// The raw path value.
	/// </summary>
	public readonly string Value { get; }
	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	public string? GetDirectoryOrFileName()
	{
		var index = Value.LastIndexOf('\\');

		if (index == -1)
		{
			return null;
		}

		var name = Value.Substring(index + 1, Value.Length - (index + 1));

		if (name == string.Empty || name.Contains("*"))
		{
			return null;
		}

		return name;
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public Path Combine(Path path)
	{
		return System.IO.Path.Combine(this, path);
	}
	/// <summary>
	/// Combines
	/// </summary>
	/// <param name="paths"></param>
	/// <returns></returns>
	public static Path Combine(params Path[] paths)
	{
		return System.IO.Path.Combine(paths.Select(p => p.Value).ToArray());
	}
	/// <summary>
	/// 
	/// </summary>
	public static Path Empty => "\\";

	#region Overloads
	/// <inheritdoc />
	public override bool Equals(object? obj)
	{
		return obj is Path path ? Equals(path) : false;
	}
	/// <inheritdoc />
	public override string ToString()
	{
		return Value;
	}
	/// <inheritdoc />
	public override int GetHashCode()
	{
		return HashCode.Combine(typeof(Path), Value);
	}
	#endregion

	#region Interfaces
	public bool Equals(Path other)
	{
		return string.Equals(Value, other.Value, StringComparison.InvariantCultureIgnoreCase);
	}
	public bool Equals(Path right, Path left)
	{
		return right!.Equals(left);
	}
	public int GetHashCode([DisallowNull] Path obj)
	{
		return obj.GetHashCode();
	}
	public int CompareTo(Path other)
	{
		return string.Compare(Value, other.Value, StringComparison.InvariantCultureIgnoreCase);
	}

	#endregion

	#region Operators
	public static implicit operator Path(string name)
	{
		return new(name);
	}
	public static implicit operator string(Path path)
	{
		return path.Value;
	}
	public static bool operator ==(Path left, Path right)
	{
		return left.Equals(right);
	}
	public static bool operator !=(Path left, Path right)
	{
		return !left.Equals(right);
	}
	public static bool operator >(Path left, Path right)
	{
		return left.CompareTo(right) > 0;
	}
	public static bool operator <(Path left, Path right)
	{
		return left.CompareTo(right) < 0;
	}
	public static bool operator >=(Path left, Path right)
	{
		return left.CompareTo(right) >= 0;
	}
	public static bool operator <=(Path left, Path right)
	{
		return left.CompareTo(right) <= 0;
	}
	public static Path operator +(Path left, Path right)
	{
		return left.Combine(right);
	}
	#endregion
}

internal static class ThrowHelper
{
	[DoesNotReturn]
	internal static void ThrowArgumentNullException(string paramName)
	{
		throw new ArgumentNullException(paramName);
	}

	[DoesNotReturn]
	internal static void ThrowArgumentException(string message)
	{
		throw new ArgumentException(message);
	}
}
