<Query Kind="Program">
  <Namespace>System.Data.Common</Namespace>
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "xunit"

void Main()
{
	//RunTests();  // Call RunTests() or press Alt+Shift+T to initiate testing.
	
	Route route = "/users/{userId}\\test";
	
	route.Segments.Dump();
}

#region private::Tests

[Fact]
void TestRoute()
{
	Route route1 = "/users";    // With leading slash
	Route route2 = "users";     // Without leading slash
	Route route3 = "/users/{userId}\test";

	Path path1 = "/users/12345";

}
#endregion


[DebuggerDisplay("Route: /{ToString()}")]
public readonly struct Route :
	IEquatable<Route>, 
	IEqualityComparer<Route>
{
	private readonly string route;
	private readonly RouteSegment[] segments;

	public Route(string route)
	{
		if (string.IsNullOrEmpty(route))
		{
			throw new ArgumentNullException(nameof(route));
		}
		this.route = route;
		this.segments = GetSegments(route);
	}

	private RouteSegment[] GetSegments(string route)
	{
		var index = 0;
		var segments = new RouteSegment[10];
		var segment = string.Empty;

		for (int i = 0; i < route.Length; i++)
		{
			var character = route[i];

			// Check if we reached the end of the current segment
			if (character == '/' || character == '\\')
			{
				// Let's skip leading slashes
				if (i == 0) continue;

				segments[index] = new RouteSegment(segment);
				index++;
				segment = string.Empty;

				// Lets see if we reached the buffer in the array. Resize if reached.
				if (index == segments.Length)
				{
					Array.Resize(ref segments, 5);
				}
			}
			else
			{
				segment = segment + character;
			}
		}

		// Resizes segments to actual length
		Array.Resize(ref segments, index);

		return segments;
	}

	/// <summary>
	/// Gets the raw route value.
	/// </summary>
	public string Value => route;
	/// <summary>
	/// Gets a copy of the route segment.
	/// </summary>
	public RouteSegment[] Segments 
	{
		get 
		{
			// Let's only return copy
			var copy = new RouteSegment[segments.Length];
			segments.CopyTo(copy, 0);
			return copy;
		}
	}
	/// <summary>
	/// Returns a formatted route value.
	/// </summary>
	public override string ToString()
	{
		return "/" + string.Join('/', Segments.Select(x => x.Value));
	}
	/// <summary>
	/// 
	/// </summary>
	public override int GetHashCode()
	{
		var hashCode = new HashCode();
		foreach (var segment in Segments)
		{
			hashCode.Add(segment);
		}
		return hashCode.ToHashCode();
	}
	/// <summary>
	/// 
	/// </summary>
	public override bool Equals([NotNullWhen(true)] object? instance)
	{
		if (instance is Route route)
		{
			return Equals(route);
		}
		return false;
	}
	/// <summary>
	/// 
	/// </summary>
	public bool Equals(Route route)
	{
		var left = Segments;
		var right = route.Segments;

		if (left.Length != right.Length)
		{
			return false;
		}
		for (int i = 0; i < left.Length; i++)
		{
			if (!left[i].Equals(right[i]))
			{
				return false;
			}
		}
		return true;
	}
	/// <summary>
	/// 
	/// </summary>
	public bool Equals(Route left, Route right)
	{
		return left.Equals(right);
	}
	/// <summary>
	/// 
	/// </summary>
	public int GetHashCode([DisallowNull] Route instance)
	{
		return instance.GetHashCode();
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="path"></param>
	public bool IsMatch(Path path)
	{
		return IsMatch(path);
	}
	/// <summary>
	/// Matches the Path to the Route
	/// </summary>
	/// <param name="path"></param>
	/// <param name="prefix"></param>
	/// <returns></returns>
	public bool IsMatch(Path path, string prefix = null)
	{
		var pathSegment = path.Segments;
		
		//path.Segments
//		path = path.ToString().Replace(prefix, "");
//
//		if (path.Segments.Length != Segments.Length)
//		{
//			return false;
//		}
//		for (int i = 0; i < Segments.Length; i++)
//		{
//			var routeSegment = Segments[i];
//			var pathSegment = path.Segments[i];
//
//			if (routeSegment.IsParameter)
//			{
//				if (!routeSegment.IsValid(pathSegment))
//				{
//					return false;
//				}
//				continue;
//			}
//			if (!routeSegment.Value.Equals(pathSegment.Value, StringComparison.CurrentCultureIgnoreCase))
//			{
//				return false;
//			}
//		}
		return true;
	}


	public static implicit operator Route(string route) => new Route(route);
	public static implicit operator string(Route route) => route.ToString();
}

[DebuggerDisplay("Segment: {Value}")]
public readonly struct RouteSegment : 
	IEquatable<RouteSegment>, 
	IEqualityComparer<RouteSegment>
{
	internal RouteSegment(string value)
	{
		if (value[0] == '{' && value[value.Length - 1] == '}')
		{
			var item = value.Substring(1, value.Length - 2);
			
			this.Value = value;
			this.SegmentType = RouteSegmentType.Parameter;
		}
		else
		{
			this.Value = value;
			this.SegmentType = RouteSegmentType.Literal;
		}
	}
	
	/// <summary>
	/// The raw segment value.
	/// </summary>
	public string Value { get; }
	/// <summary>
	/// A 
	/// </summary>
	public RouteSegmentType SegmentType { get; }
	/// <summary>
	/// The index of the route
	/// </summary>
	public int Index { get; }
	/// <summary>
	/// 
	/// </summary>
	public bool Equals(RouteSegment other)
	{
		return Value.Equals(other.Value, StringComparison.InvariantCultureIgnoreCase);
	}
	/// <summary>
	/// 
	/// </summary>
	public bool Equals(RouteSegment right, RouteSegment left)
	{
		return right.Equals(left);
	}
	/// <summary>
	/// 
	/// </summary>
	public int GetHashCode(RouteSegment instance)
	{
		return instance.GetHashCode();
	}
	/// <summary>
	/// 
	/// </summary>
	public override bool Equals([NotNullWhen(true)] object instance)
	{
		if (instance is RouteSegment segment)
		{
			return Equals(segment);
		}
		return false;
	}
	/// <summary>
	/// 
	/// </summary>
	public override int GetHashCode()
	{
		return HashCode.Combine(this, Value);
	}
	/// <summary>
	/// 
	/// </summary>
	public override string ToString() => Value;

	
	internal bool IsValid(PathSegment segment)
	{
		//segment.Value.Se
		

		return true;
	}
}

public enum RouteSegmentType
{
	Parameter,
	Literal
}

[DebuggerDisplay("Path: /{ToString()}")]
public readonly struct Path : IEquatable<Path>, IEqualityComparer<Path>
{
	private const string invalidCharacters = @"<>*%&:\?";
	
	private readonly PathSegment[] segments;

	public Path(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			throw new ArgumentNullException(nameof(path));
		}
		//if (path.Any(c => invalidCharacters.Contains(c)))
		//{
		//	throw new ArgumentException($"The following path: '{path}' contains an invalid character. Disallowed Characters '{invalidCharacters}'.");
		//}
		segments = GetSegments(path);
	}

	private PathSegment[] GetSegments(string path)
	{
		var index = 0;
		var segments = new PathSegment[10];
		var segment = string.Empty;

		for (int i = 0; i < path.Length; i++)
		{
			var character = path[i];

			// Check if we reached the end of the current segment
			if (character == '/' || character == '\\')
			{
				// Let's skip leading slashes
				if (i == 0) continue;

				segments[index] = new PathSegment(segment);
				index++;
				segment = string.Empty;

				// Lets see if we reached the buffer in the array. Resize if reached.
				if (index == segments.Length)
				{
					Array.Resize(ref segments, 5);
				}
			}
			else
			{
				segment = segment + character;
			}
		}

		// Resizes segments to actual length
		Array.Resize(ref segments, index);

		return segments;
	}

	/// <summary>
	/// A collection of path segments.
	/// </summary>
	public PathSegment[] Segments
	{
		get 
		{
			var copy = new PathSegment[segments.Length];
			segments.CopyTo(copy, 0);
			return copy;
		}
	}

	/// <inheritdoc />
	public override string ToString()
	{
		return string.Join("/", Segments.Select(s => s.Value));
	}

	/// <inheritdoc />
	public override int GetHashCode()
	{
		var hashCode = new HashCode();
		foreach (var segment in Segments)
		{
			hashCode.Add(segment);
		}
		return hashCode.ToHashCode();
	}

	/// <inheritdoc />
	public override bool Equals([NotNullWhen(true)] object? instance)
	{
		if (instance is Path path)
		{
			return Equals(path);
		}

		return false;
	}

	/// <inheritdoc />
	public bool Equals(Path path)
	{
		if (path.Segments.Length != Segments.Length)
		{
			return false;
		}
		for (int i = 0; i < Segments.Length; i++)
		{
			var incoming = path.Segments[i];
			var current = Segments[i];

			if (incoming.Value != current.Value)
			{
				return false;
			}
		}

		return true;
	}

	/// <inheritdoc />
	public bool Equals(Path left, Path right) => left.Equals(right);
	
	/// <inheritdoc />
	public int GetHashCode([DisallowNull] Path path)
	{
		return path.GetHashCode();
	}

	public static implicit operator Path(string path) => new Path(path);
	public static implicit operator string(Path path) => path.ToString();
}

[DebuggerDisplay("Path Segment: {Value}")]
public readonly struct PathSegment
{

	internal PathSegment(string value)
	{
		if (value is null)
		{
			throw new ArgumentNullException(nameof(value));
		}

		this.Value = value;
	}
	/// <summary>
	/// The raw segment value.
	/// </summary>
	public string Value { get; }
}

