<Query Kind="Program">
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{

}

#region Assimalign.Cohesion.Core(net8.0)
namespace Assimalign.Cohesion
{
	#region \
	public static class AppEnvironment
	{
	    private static readonly string EnvironmentKey = "COHESION_ENVIRONMENT";
	    public static string? GetEnvironmentName()
	    {
	        return Environment.GetEnvironmentVariable(EnvironmentKey, EnvironmentVariableTarget.Process);
	    }
	}
	#endregion
	#region \ComponentModel
	public abstract class ChangeToken : IChangeToken
	{
	    public abstract bool HasChanged { get; }
	    public abstract bool ActiveChangeCallbacks { get; }
	    public abstract IDisposable OnChange(Action<object> callback, object state);
	    private sealed class ChangeTokenRegistration<TState> : IDisposable
	    {
	        private readonly Func<IChangeToken> changeTokenProducer;
	        private readonly Action<TState> changeTokenConsumer;
	        private readonly TState state;
	        private IDisposable disposable;
	        private static readonly NoopDisposable disposedSentinel = new NoopDisposable();
	        public ChangeTokenRegistration(Func<IChangeToken> changeTokenProducer, Action<TState> changeTokenConsumer, TState state)
	        {
	            this.changeTokenProducer = changeTokenProducer;
	            this.changeTokenConsumer = changeTokenConsumer;
	            this.state = state;
	            var token = changeTokenProducer();
	            RegisterChangeTokenCallback(token);
	        }
	        private void OnChangeTokenFired()
	        {
	            // The order here is important. We need to take the token and then apply our changes BEFORE
	            // registering. This prevents us from possible having two change updates to process concurrently.
	            //
	            // If the token changes after we take the token, then we'll process the update immediately upon
	            // registering the callback.
	            IChangeToken token = changeTokenProducer();
	            try
	            {
	                changeTokenConsumer(state);
	            }
	            finally
	            {
	                // We always want to ensure the callback is registered
	                RegisterChangeTokenCallback(token);
	            }
	        }
	        private void RegisterChangeTokenCallback(IChangeToken token)
	        {
	            if (token is null)
	            {
	                return;
	            }
	            IDisposable registration = token.OnChange(s => ((ChangeTokenRegistration<TState>)s).OnChangeTokenFired(), this);
	            SetDisposable(registration);
	        }
	        private void SetDisposable(IDisposable disposable)
	        {
	            // We don't want to transition from _disposedSentinel => anything since it's terminal
	            // but we want to allow going from previously assigned disposable, to another
	            // disposable.
	            var current = Volatile.Read(ref this.disposable);
	            // If Dispose was called, then immediately dispose the disposable
	            if (current == disposedSentinel)
	            {
	                disposable.Dispose();
	                return;
	            }
	            // Otherwise, try to update the disposable
	            var previous = Interlocked.CompareExchange(ref this.disposable, disposable, current);
	            if (previous == disposedSentinel)
	            {
	                // The subscription was disposed so we dispose immediately and return
	                disposable.Dispose();
	            }
	            else if (previous == current)
	            {
	                // We successfuly assigned the _disposable field to disposable
	            }
	            else
	            {
	                // Sets can never overlap with other SetDisposable calls so we should never get into this situation
	                throw new InvalidOperationException("Somebody else set the _disposable field");
	            }
	        }
	        public void Dispose()
	        {
	            // If the previous value is disposable then dispose it, otherwise,
	            // now we've set the disposed sentinel
	            Interlocked.Exchange(ref disposable, disposedSentinel).Dispose();
	        }
	        private sealed class NoopDisposable : IDisposable
	        {
	            public void Dispose()
	            {
	            }
	        }
	    }
	}
	public static class ChangeTokenExtensions
	{
	    public static IDisposable OnChange<TState>(this IChangeToken token, Action<TState> callback, TState state)
	    {
	        return token.OnChange(callback, state);
	    }
	}
	public interface IChangeToken
	{
	    bool HasChanged { get; }
	    bool ActiveChangeCallbacks { get; }
	    IDisposable OnChange(Action<object> callback, object state);
	}
	#endregion
	#region \Exceptions
	public class CoreException : Exception
	{
	    public CoreException(string message) 
	        : base(message) { }
	    public CoreException(string message, Exception? innerException) 
	        : base(message, innerException) { }
	}
	#endregion
	#region \Internal\Utilities
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
	#endregion
	#region \IO
	public readonly struct FileName
	{
	}
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
	        // Trim white space, lead and trailing forward/backward slashes
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
	    public readonly string Value { get; }
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
	    public Path Combine(Path path)
	    {
	        return System.IO.Path.Combine(this, path);
	    }
	    public static Path Combine(params Path[] paths)
	    {
	        return System.IO.Path.Combine(paths.Select(p=>p.Value).ToArray());
	    }
	    public static Path Empty => "\\";
	    #region Overloads
	    public override bool Equals(object? obj)
	    {
	        return obj is Path path ? Equals(path) : false;
	    }
	    public override string ToString()
	    {
	        return Value;
	    }
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
	[DebuggerDisplay("Length: {Length} | {Gigabytes} GB")]
	public readonly struct Size : IEquatable<Size>, IComparable<Size>, IEqualityComparer<Size>
	{
	    private const long kilobyte = 1000;
	    private const long megabyte = kilobyte * 1000;
	    private const long gigabyte = megabyte * 1000;
	    private const long terabyte = gigabyte * 1000;
	    private const long petabyte = terabyte * 1000;
	    public Size(long length)
	    {
	        if (Length < -1)
	        {
	            ThrowHelper.ThrowArgumentException("File size must be greater than -1.");
	        }
	        Length = length;
	    }
	    public static Size Empty => new Size(-1);
	    public long Length { get; }
	    public double Kilobytes
	    {
	        get
	        {
	            if (Length == -1)
	            {
	                return 0;
	            }
	            return ((double)Length) / kilobyte;
	        }
	    }
	    public double Megabytes
	    {
	        get
	        {
	            if (Length == -1)
	            {
	                return 0;
	            }
	            return ((double)Length) / megabyte;
	        }
	    }
	    public double Gigabytes
	    {
	        get
	        {
	            if (Length == -1)
	            {
	                return 0;
	            }
	            return ((double)Length) / gigabyte;
	        }
	    }
	    public double Terabytes
	    {
	        get
	        {
	            if (Length == -1)
	            {
	                return 0;
	            }
	            return ((double)Length) / terabyte;
	        }
	    }
	    public double Petabytes
	    {
	        get
	        {
	            if (Length == -1)
	            {
	                return 0;
	            }
	            return ((double)Length) / petabyte;
	        }
	    }
	    #region Overloads
	    public override bool Equals([NotNullWhen(true)] object? obj)
	    {
	        return obj is Size size ? Equals(size) : false;
	    }
	    public override string ToString()
	    {
	        return ToString("b");
	    }
	    public override int GetHashCode()
	    {
	        return HashCode.Combine(typeof(Size), Length);
	    }
	    #endregion
	    #region Interfaces
	    public bool Equals(Size other)
	    {
	        return other.Length == Length;
	    }
	    public bool Equals(Size left, Size right)
	    {
	        return left.Equals(right);
	    }
	    public int GetHashCode([DisallowNull] Size obj)
	    {
	        return obj.GetHashCode();
	    }
	    public int CompareTo(Size other)
	    {
	        return Length.CompareTo(other.Length);
	    }
	    public string ToString(string? format)
	    {
	        format ??= "b";
	        return format switch
	        {
	            "b" => Length.ToString(),
	            "kb" => Kilobytes.ToString(),
	            "mb" => Megabytes.ToString(),
	            "gb" => Gigabytes.ToString(),
	            "tb" => Terabytes.ToString(),
	            "pb" => Petabytes.ToString(),
	            _ => Length.ToString()
	        };
	    }
	    #endregion
	    #region Operators
	    public static implicit operator long(Size fileSize) => fileSize.Length;
	    public static implicit operator Size(long length) => new Size(length);
	    public static bool operator ==(Size left, Size right) => left.Equals(right);
	    public static bool operator !=(Size left, Size right) => !left.Equals(right);
	    public static bool operator >(Size left, Size right) => left.CompareTo(right) > 0;
	    public static bool operator <(Size left, Size right) => left.CompareTo(right) < 0;
	    public static bool operator >=(Size left, Size right) => left.CompareTo(right) >= 0;
	    public static bool operator <=(Size left, Size right) => left.CompareTo(right) <= 0;
	    #endregion
	    #region Helpers
	    public static Size FromKilobytes(double size)
	    {
	        return new Size((long)(size * kilobyte));
	    }
	    public static Size FromMegabytes(double size)
	    {
	        return new Size((long)(size * megabyte));
	    }
	    public static Size FromGigabytes(double size)
	    {
	        return new Size((long)(size * gigabyte));
	    }
	    public static Size FromTerabytes(double size)
	    {
	        return new Size((long)(size * terabyte));
	    }
	    public static Size FromPetabytes(double size)
	    {
	        return new Size((long)(size * terabyte));
	    }
	    #endregion
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
	#region \Utilities
	public static class Cacher<TIn, TOut>
	    where TIn : notnull
	{
	    private static readonly ConcurrentDictionary<TIn, TOut> cache;
	    static Cacher()
	    {
	        cache = new();
	    }
	    public static Func<TIn, TOut> Memoise(Func<TIn, TOut> method)
	    {
	        return input => cache.TryGetValue(input, out var result) ?
	            result :
	            method(input);
	    }
	}
	#endregion
}
#endregion
