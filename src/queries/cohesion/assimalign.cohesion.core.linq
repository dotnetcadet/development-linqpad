<Query Kind="Program">
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.IO.Enumeration</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>System.Reflection.Metadata.Ecma335</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

using static Assimalign.Cohesion.PathHelper;

void Main()
{

}

#region Assimalign.Cohesion.Core
namespace Assimalign.Cohesion
{
	#region \
	public static partial class AppEnvironment
	{
	    public static string? GetEnvironmentName()
	    {
	        return Environment.GetEnvironmentVariable(Keys.EnvironmentKey, EnvironmentVariableTarget.Process);
	    }
	}
	public static partial class AppEnvironment
	{
	    internal static partial class Keys
	    {
	        internal static readonly string EnvironmentKey = "COHESION_ENVIRONMENT";
	    }
	}
	#endregion
	#region \Exceptions
	public abstract class CohesionException : Exception
	{
	    protected CohesionException() { }
	    protected CohesionException(string message) 
	        : base(message) { }
	    protected CohesionException(string message, Exception? innerException) 
	        : base(message, innerException) { }
	}
	public abstract class NetworkException  : CohesionException
	{
	    public NetworkException(string message)
	        : base(message) { }
	    public NetworkException(string message, Exception innerException)
	        : base(message, innerException) { }
	    public abstract NetworkOsiLayer Layer { get; }
	}
	public enum NetworkOsiLayer
	{
	    Physical = 1,
	    DataLink = 2,
	    Network = 3,
	    Transport = 4,
	    Session = 5,
	    Presentation = 6,
	    Application = 7,
	}
	#endregion
	#region \Internal
	internal static class HashHelpers
	{
	    public static int Combine(int h1, int h2)
	    {
	        // RyuJIT optimizes this to use the ROL instruction
	        // Related GitHub pull request: https://github.com/dotnet/coreclr/pull/1830
	        uint rol5 = ((uint)h1 << 5) | ((uint)h1 >> 27);
	        return ((int)rol5 + h1) ^ h2;
	    }
	}
	// A convenience API for interacting with System.Threading.Timer in a way
	// that doesn't capture the ExecutionContext. We should be using this (or equivalent)
	// everywhere we use timers to avoid rooting any values stored in asynclocals.
	internal static class NonCapturingTimer
	{
	    public static Timer Create(TimerCallback callback, object state, TimeSpan dueTime, TimeSpan period)
	    {
	        if (callback is null)
	        {
	            throw new ArgumentNullException(nameof(callback));
	        }
	        // Don't capture the current ExecutionContext and its AsyncLocals onto the timer
	        bool restoreFlow = false;
	        try
	        {
	            if (!ExecutionContext.IsFlowSuppressed())
	            {
	                ExecutionContext.SuppressFlow();
	                restoreFlow = true;
	            }
	            return new Timer(callback, state, dueTime, period);
	        }
	        finally
	        {
	            // Restore the current ExecutionContext
	            if (restoreFlow)
	            {
	                ExecutionContext.RestoreFlow();
	            }
	        }
	    }
	}
	internal static class PathHelper
	{
	    private const char dot = '.';
	    private static readonly char[] _separators = ['/', '\\'];
	    private static readonly char[] _invalidFileChars = [
	        '\"', '<', '>', '|', '\0',
	            (char)1, (char)2, (char)3, (char)4, (char)5, (char)6, (char)7, (char)8, (char)9, (char)10,
	            (char)11, (char)12, (char)13, (char)14, (char)15, (char)16, (char)17, (char)18, (char)19, (char)20,
	            (char)21, (char)22, (char)23, (char)24, (char)25, (char)26, (char)27, (char)28, (char)29, (char)30,
	            (char)31, ':', '*', '?', '\\', '/'
	        ];
	    private static readonly char[] _invalidPathChars = [  // DO NOT use Path.GetInvalidPathChars() - the results differ per platform.
	        '|', '\0',
	        (char)1, (char)2, (char)3, (char)4, (char)5, (char)6, (char)7, (char)8, (char)9, (char)10,
	        (char)11, (char)12, (char)13, (char)14, (char)15, (char)16, (char)17, (char)18, (char)19, (char)20,
	        (char)21, (char)22, (char)23, (char)24, (char)25, (char)26, (char)27, (char)28, (char)29, (char)30,
	        (char)31, 
	        '<', '>', '?' 
	        ];
	    internal static bool IsDot(char value)
	    {
	        return value == dot;
	    }
	    internal static bool IsPathSeparator(char value)
	    {
	        return value == _separators[0] || value == _separators[1];
	    }
	    internal static bool IsValidDriveChar(char value)
	    {
	        return (uint)((value | 0x20) - 97) <= 25u;
	    }
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    internal static bool IsDirectorySeparator(char c)
	    {
	        if (c != '\\')
	        {
	            return c == '/';
	        }
	        return true;
	    }
	    internal static bool IsEffectivelyEmpty(ReadOnlySpan<char> path)
	    {
	        if (path.IsEmpty)
	        {
	            return true;
	        }
	        ReadOnlySpan<char> readOnlySpan = path;
	        for (int i = 0; i < readOnlySpan.Length; i++)
	        {
	            if (readOnlySpan[i] != ' ')
	            {
	                return false;
	            }
	        }
	        return true;
	    }
	    internal static string GetPathRoot(ReadOnlySpan<char> path)
	    {
	        if (IsEffectivelyEmpty(path))
	        {
	            return string.Empty;
	        }
	        int rootLength = GetRootLength(path);
	        if (rootLength > 0)
	        {
	            return path.Slice(0, rootLength).ToString();
	        }
	        return  string.Empty;
	    }
	    internal static bool IsValidPathChar(char value)
	    {
	        for (int i = 0; i < _invalidPathChars.Length; i++)
	        {
	            if (_invalidPathChars[i] == value)
	            {
	                return false;
	            }
	        }
	        return true;
	    }
	    internal static bool IsValidNameChar(char value)
	    {
	        for (int i = 0; i < _invalidFileChars.Length; i++)
	        {
	            if (_invalidFileChars[i] == value)
	            {
	                return false;
	            }
	        }
	        return true;
	    }
	    internal static bool HasDriveLetter(string value)
	    {
	        if (value.Length >= 2)
	        {
	            if (IsValidDriveChar(value[0]) && value[1] == ':')
	            {
	                return true;
	            }
	        }
	        return false;
	    }
	    internal static int GetRootLength(ReadOnlySpan<char> path)
	    {
	        int length = path.Length;
	        int i = 0;
	        bool flag = IsDevice(path);
	        bool flag2 = flag && IsDeviceUNC(path);
	        if ((!flag || flag2) && length > 0 && IsDirectorySeparator(path[0]))
	        {
	            if (flag2 || (length > 1 && IsDirectorySeparator(path[1])))
	            {
	                i = (flag2 ? 8 : 2);
	                int num = 2;
	                for (; i < length; i++)
	                {
	                    if (IsDirectorySeparator(path[i]) && --num <= 0)
	                    {
	                        break;
	                    }
	                }
	            }
	            else
	            {
	                i = 1;
	            }
	        }
	        else if (flag)
	        {
	            for (i = 4; i < length && !IsDirectorySeparator(path[i]); i++)
	            {
	            }
	            if (i < length && i > 4 && IsDirectorySeparator(path[i]))
	            {
	                i++;
	            }
	        }
	        else if (length >= 2 && path[1] == ':' && IsValidDriveChar(path[0]))
	        {
	            i = 2;
	            if (length > 2 && IsDirectorySeparator(path[2]))
	            {
	                i++;
	            }
	        }
	        return i;
	    }
	    internal static bool IsDeviceUNC(ReadOnlySpan<char> path)
	    {
	        if (path.Length >= 8 && IsDevice(path) && IsDirectorySeparator(path[7]) && path[4] == 'U' && path[5] == 'N')
	        {
	            return path[6] == 'C';
	        }
	        return false;
	    }
	    internal static bool IsExtended(ReadOnlySpan<char> path)
	    {
	        if (path.Length >= 4 && path[0] == '\\' && (path[1] == '\\' || path[1] == '?') && path[2] == '?')
	        {
	            return path[3] == '\\';
	        }
	        return false;
	    }
	    internal static bool IsDevice(ReadOnlySpan<char> path)
	    {
	        if (!IsExtended(path))
	        {
	            if (path.Length >= 4 && IsDirectorySeparator(path[0]) && IsDirectorySeparator(path[1]) && (path[2] == '.' || path[2] == '?'))
	            {
	                return IsDirectorySeparator(path[3]);
	            }
	            return false;
	        }
	        return true;
	    }
	    internal static void CalculateTrimRange(string value, ref int start, ref int end)
	    {
	        // Calculate start of string
	        for (; start < value.Length; start++)
	        {
	            int index = 0;
	            char c = value[start];
	            while (index < _separators.Length && _separators[index] != c)
	            {
	                index++;
	            }
	            if (index == _separators.Length) break;
	        }
	        // Calculate end of string
	        for (; end >= start; end--)
	        {
	            int index = 0;
	            char c = value[end];
	            while (index < _separators.Length && _separators[index] != c)
	            {
	                index++;
	            }
	            if (index == _separators.Length) break;
	        }
	    }
	    internal static void CalculateTrimStart(string value, ref int start)
	    {
	        // Calculate start of string
	        for (; start < value.Length; start++)
	        {
	            int index = 0;
	            char c = value[start];
	            while (index < _separators.Length && _separators[index] != c)
	            {
	                index++;
	            }
	            if (index == _separators.Length) break;
	        }
	    }
	    //    private static readonly char[] _invalidFileNameChars = Path.GetInvalidFileNameChars()
	    //        .Where(c => c != Path.DirectorySeparatorChar && c != Path.AltDirectorySeparatorChar).ToArray();
	    //    private static readonly char[] _invalidFilterChars = _invalidFileNameChars
	    //        .Where(c => c != '*' && c != '|' && c != '?').ToArray();
	    //    private static readonly char[] _pathSeparators = new[]
	    //        {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar};
	    //    internal static bool HasInvalidPathChars(string path)
	    //    {
	    //        return path.IndexOfAny(_invalidFileNameChars) != -1;
	    //    }
	    //    internal static bool HasInvalidFilterChars(string path)
	    //    {
	    //        return path.IndexOfAny(_invalidFilterChars) != -1;
	    //    }
	    //    internal static string EnsureTrailingSlash(string path)
	    //    {
	    //        if (!string.IsNullOrEmpty(path) &&
	    //            path[path.Length - 1] != Path.DirectorySeparatorChar)
	    //        {
	    //            return path + Path.DirectorySeparatorChar;
	    //        }
	    //        return path;
	    //    }
	    //    internal static bool PathNavigatesAboveRoot(string path)
	    //    {
	    //        var tokenizer = new StringTokenizer(path, _pathSeparators);
	    //        int depth = 0;
	    //        foreach (StringSegment segment in tokenizer)
	    //        {
	    //            if (segment.Equals(".") || segment.Equals(""))
	    //            {
	    //                continue;
	    //            }
	    //            else if (segment.Equals(".."))
	    //            {
	    //                depth--;
	    //                if (depth == -1)
	    //                {
	    //                    return true;
	    //                }
	    //            }
	    //            else
	    //            {
	    //                depth++;
	    //            }
	    //        }
	    //        return false;
	    //    
	}
	#endregion
	#region \Internal\Shared
	internal static partial class ThrowHelper
	{
	    #region Arguments
	    internal static T ThrowIfNull<T>(
	        [NotNull]T? argument,
	        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	    {
	        if (argument is null)
	        {
	            ThrowArgumentNullException(paramName);
	        }
	        return argument;
	    }
	    internal static string ThrowIfNullOrEmpty(
	        [NotNull] string? argument,
	        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	    {
	        if (string.IsNullOrEmpty(argument))
	        {
	            ThrowArgumentNullException(paramName);
	        }
	        return argument;
	    }
	    internal static T ThrowIfNullOrNone<T>(
	        [NotNull] T argument,
	        [CallerArgumentExpression(nameof(argument))] string? paramName = null) where T : IEnumerable
	    {
	        switch (argument)
	        {
	            case null:
	            case ICollection collection when collection.Count == 0:
	            case Array array when array.Length == 0:
	                ThrowArgumentNullException(paramName);
	                break;
	        }
	        return argument;
	    }
	    [DoesNotReturn]
	    internal static void ThrowArgumentNullException(
	        string? paramName)
	    {
	        throw new ArgumentNullException(paramName);
	    }
	    [DoesNotReturn]
	    internal static void ThrowArgumentNullException(
	        string paramName, 
	        string message)
	    {
	        throw new ArgumentNullException(paramName, message);
	    }
	    [DoesNotReturn]
	    internal static void ThrowArgumentException(string message)
	    {
	        throw new ArgumentException(message);
	    }
	    [DoesNotReturn]
	    internal static void ThrowArgumentException(string message, string paramName)
	    {
	        throw new ArgumentException(message, paramName);
	    }
	    [DoesNotReturn]
	    internal static void ThrowInvalidOperationException(string message)
	    {
	        throw new InvalidOperationException(message);
	    }
	    #endregion
	    #region Threading
	    [DoesNotReturn]
	    internal static void ThrowObjectDisposedException(string objectName)
	    {
	        throw new ObjectDisposedException(objectName);
	    }
	    [DoesNotReturn]
	    internal static void ThrowObjectDisposedException(string objectName, string message)
	    {
	        throw new ObjectDisposedException(objectName, message);
	    }
	    #endregion
	    #region IO
	    [DoesNotReturn]
	    internal static void ThrowEndOfStreamException(string message)
	    {
	        throw new EndOfStreamException(message);
	    }
	    #endregion
	    #region Json Serialization
	    [DoesNotReturn]
	    internal static void ThrowJsonException(string message)
	    {
	        throw new JsonException(message);
	    }
	    #endregion
	}
	#endregion
	#region \obj\Debug\net9.0
	#endregion
	#region \Properties
	    // This class was auto-generated by the StronglyTypedResourceBuilder
	    // class via a tool like ResGen or Visual Studio.
	    // To add or remove a member, edit your .ResX file then rerun ResGen
	    // with the /str option, or rebuild your VS project.
	    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
	    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
	    internal class Resources {
	        private static global::System.Resources.ResourceManager resourceMan;
	        private static global::System.Globalization.CultureInfo resourceCulture;
	        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
	        internal Resources() {
	        }
	        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
	        internal static global::System.Resources.ResourceManager ResourceManager {
	            get {
	                if (object.ReferenceEquals(resourceMan, null)) {
	                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Assimalign.Cohesion.Properties.Resources", typeof(Resources).Assembly);
	                    resourceMan = temp;
	                }
	                return resourceMan;
	            }
	        }
	        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
	        internal static global::System.Globalization.CultureInfo Culture {
	            get {
	                return resourceCulture;
	            }
	            set {
	                resourceCulture = value;
	            }
	        }
	        internal static string FileSystePathError {
	            get {
	                return ResourceManager.GetString("FileSystePathError", resourceCulture);
	            }
	        }
	    }
	#endregion
	#region \System
	    internal readonly struct Coordinate
	    {
	    }
	public static class RangeExtensions
	{
	    public static (int start, int length) GetStartLength(this Range range)
	    {
	        var start = range.Start.Value;
	        var length =  range.End.Value - start;
	        return (start, length);
	    }
	}
	//[JsonConverter(typeof(SizeJsonConverter))]
	[DebuggerDisplay("Length: {Gigabytes}")]
	public readonly struct Size : IEquatable<Size>, IComparable<Size>, IEqualityComparer<Size>
	#if NET7_0_OR_GREATER
	    , IEqualityOperators<Size, Size, bool>
	    , IAdditionOperators<Size, Size, Size>
	    , ISubtractionOperators<Size, Size, Size>
	#endif
	{
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public Size(long length)
	    {
	        if (length < -1)
	        {
	            ThrowHelper.ThrowArgumentException($"The '{nameof(length)}' must be greater than -1.");
	        }
	        Length = length;
	    }
	    #region Implementation
	    public static Size Empty => new Size(-1);
	    public long Bits => Length * 8;
	    public long Length { get; }
	    /* Decimal Prefix */
	    public double Kilobytes => Calculate(1000, 1);
	    public double Megabytes => Calculate(1000, 2);
	    public double Gigabytes => Calculate(1000, 3);
	    public double Terabytes => Calculate(1000, 4);
	    public double Petabytes => Calculate(1000, 5);
	    /* Binary Prefix */
	    public double Kibibytes => Calculate(1024, 1);
	    public double Mebibytes => Calculate(1024, 2);
	    public double Gibibytes => Calculate(1024, 3);
	    public double Tebibytes => Calculate(1024, 4);
	    public double Pebibytes => Calculate(1024, 5);
	    private double Calculate(int value, int unit)
	    {
	        return Length == -1 ?
	            0 :
	            (double)Length / Math.Pow(value, unit);
	    }
	    #endregion
	    #region Overloads
	    public override bool Equals(object? obj)
	    {
	        return obj is Size size ? Equals(size) : false;
	    }
	    public override string ToString()
	    {
	        return ToString("b");
	    }
	    public override int GetHashCode()
	    {
	        return Length.GetHashCode();
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
	        return Length.GetHashCode();
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
	            "ki" => Kibibytes.ToString(),
	            "mi" => Mebibytes.ToString(),
	            "gi" => Gibibytes.ToString(),
	            "ti" => Tebibytes.ToString(),
	            "pi" => Pebibytes.ToString(),
	            _ => Length.ToString()
	        };
	    }
	    #endregion
	    #region Operators
	    public static implicit operator long(Size fileSize)
	    {
	        return fileSize.Length;
	    }
	    public static implicit operator Size(long length)
	    {
	        return new Size(length);
	    }
	    public static implicit operator Size(int length)
	    {
	        return new Size(length);
	    }
	    public static bool operator ==(Size left, Size right)
	    {
	        return left.Equals(right);
	    }
	    public static bool operator !=(Size left, Size right)
	    {
	        return !left.Equals(right);
	    }
	    public static bool operator >(Size left, Size right)
	    {
	        return left.CompareTo(right) > 0;
	    }
	    public static bool operator <(Size left, Size right)
	    {
	        return left.CompareTo(right) < 0;
	    }
	    public static bool operator >=(Size left, Size right)
	    {
	        return left.CompareTo(right) >= 0;
	    }
	    public static bool operator <=(Size left, Size right)
	    {
	        return left.CompareTo(right) <= 0;
	    }
	    public static Size operator +(Size left, Size right)
	    {
	        return left.Length + right.Length;
	    }
	    public static Size operator -(Size left, Size right)
	    {
	        var value = left.Length - right.Length;
	        if (value < -1)
	        {
	            ThrowHelper.ThrowInvalidOperationException("The calculated must exceed -1.");
	        }
	        return value;
	    }
	    #endregion
	    #region Helpers
	    public static Size FromKilobytes(double size)
	    {
	        return new Size((long)(size * Math.Pow(1000, 1)));
	    }
	    public static Size FromKibibytes(double size)
	    {
	        return new Size((long)(size * Math.Pow(1024, 1)));
	    }
	    public static Size FromMegabytes(double size)
	    {
	        return new Size((long)(size * Math.Pow(1000, 2)));
	    }
	    public static Size FromMebibytes(double size)
	    {
	        return new Size((long)(size * Math.Pow(1024, 2)));
	    }
	    public static Size FromGigabytes(double size)
	    {
	        return new Size((long)(size * Math.Pow(1000, 3)));
	    }
	    public static Size FromGibibytes(double size)
	    {
	        return new Size((long)(size * Math.Pow(1024, 3)));
	    }
	    public static Size FromTerabytes(double size)
	    {
	        return new Size((long)(size * Math.Pow(1000, 4)));
	    }
	    public static Size FromTebibytes(double size)
	    {
	        return new Size((long)(size * Math.Pow(1024, 4)));
	    }
	    public static Size FromPetabytes(double size)
	    {
	        return new Size((long)(size * Math.Pow(1000, 5)));
	    }
	    public static Size FromPebibytes(double size)
	    {
	        return new Size((long)(size * Math.Pow(1000, 5)));
	    }
	    #endregion
	}
	public static class SpanExtensions
	{
	    public static int SplitAny(this ReadOnlySpan<char> source, Span<Range> destination, ReadOnlySpan<char> separators)
	    {
	#if NET7_0_OR_GREATER
	    return MemoryExtensions.SplitAny(source, destination, separators);
	#else
	        var count = 0;
	        var start = 0;
	        for (int i = 0; i < source.Length; i++)
	        {
	            if ((i + 1) == source.Length)
	            {
	                destination[count] = new Range(start, i);
	                count++;
	                break;
	            }
	            for (int a = 0; a < separators.Length; a++)
	            {
	                if (source[i] == separators[a])
	                {
	                    destination[count] = new Range(start, i);
	                    start = (i + 1);
	                    count++;
	                }
	            }
	        }
	        return count;
	#endif
	    }
	}
	#endregion
	#region \System\IO
	[DebuggerDisplay("{_value}")]
	public readonly struct DirectoryName : IEquatable<DirectoryName>, IComparable<DirectoryName>
	#if NET7_0_OR_GREATER
	    ,IEqualityOperators<DirectoryName, DirectoryName, bool>
	#endif
	{
	    private readonly string _value;
	    public DirectoryName(string value)
	    {
	        ThrowHelper.ThrowIfNullOrEmpty(value, nameof(value));
	        if (value.Length == 1 && IsPathSeparator(value[0]))
	        {
	            _value = "/";
	            return;
	        }
	        string error = null!;
	        int start = 0;
	        int end = value.Length - 1;
	        CalculateTrimRange(value, ref start, ref end);
	        _value = string.Create((end + 2) - start, value, (span, value) =>
	        {
	            for (int i = start; i < (end + 1); i++)
	            {
	                var current = value[i];
	                if (!IsValidNameChar(current))
	                {
	                    error = $"The directory name has an invalid character `{current}`.";
	                    break;
	                }
	                span[i - start] = current;
	            }
	            // ending slash's indicate directory so lets concat each name with an ending slash
	            span[span.Length - 1] = '/';
	        });
	        if (_value.Length > MaxLength)
	        {
	            ThrowHelper.ThrowArgumentException($"The file name is too long. Max Length allowed is {MaxLength}");
	        }
	        if (error is not null)
	        {
	            ThrowHelper.ThrowArgumentException(error);
	        }
	    }
	    public const int MaxLength = 255;
	    public bool IsRoot => _value[0] == '/';
	    public static DirectoryName Root { get; } = "/";
	    #region Methods
	    public bool Equals(DirectoryName other)
	    {
	        return Equals(other, CultureInfo.InvariantCulture);
	    }
	    public bool Equals(DirectoryName other, CultureInfo cultureInfo)
	    {
	        return StringComparer.Create(cultureInfo, true).Equals(_value, other._value);
	    }
	    public int CompareTo(DirectoryName other)
	    {
	        return CompareTo(other, CultureInfo.InvariantCulture);
	    }
	    public int CompareTo(DirectoryName other, CultureInfo cultureInfo)
	    {
	        return StringComparer.Create(cultureInfo, true).Compare(_value, other._value);
	    }
	    public int GetHashCode(CultureInfo cultureInfo)
	    {
	        return StringComparer.Create(cultureInfo, true).GetHashCode(_value);
	    }
	    #region Overloads
	    public override bool Equals([NotNullWhen(true)] object? obj)
	    {
	        if (obj is null)
	        {
	            return false;
	        }
	        if (obj is not DirectoryName name)
	        {
	            return false;
	        }
	        return Equals(name);
	    }
	    public override string ToString()
	    {
	        return _value;
	    }
	    public override int GetHashCode()
	    {
	        return GetHashCode(CultureInfo.InvariantCulture);
	    }
	    #endregion
	    #endregion
	    #region Operators
	    public static implicit operator DirectoryName(string value)
	    {
	        return new DirectoryName(value);
	    }
	    public static implicit operator string(DirectoryName name)
	    {
	        return name._value;
	    }
	    public static implicit operator FileSystemPath(DirectoryName name)
	    {
	        return name._value;
	    }
	    public static bool operator ==(DirectoryName left, DirectoryName right)
	    {
	        return left.Equals(right);
	    }
	    public static bool operator !=(DirectoryName left, DirectoryName right)
	    {
	        return !left.Equals(right);
	    }
	    #endregion
	}
	[DebuggerDisplay("{_value}")]
	public readonly struct FileName : IEquatable<FileName>, IComparable<FileName>
	#if NET7_0_OR_GREATER
	    ,IEqualityOperators<FileName, FileName, bool>
	#endif
	{
	    private readonly string _value;
	    public FileName(string name)
	    {
	        ThrowHelper.ThrowIfNullOrEmpty(name);
	        int start = 0;
	        CalculateTrimStart(name, ref start);
	        string error = null!;
	        _value = string.Create(name.Length - start, name, (span, value) =>
	        {
	            for (int i = start; i < name.Length; i++)
	            {
	                var current = value[i];
	                if (!IsValidNameChar(current))
	                {
	                    error = $"The file name has an invalid character `{current}`.";
	                    break;
	                }
	                span[i - start] = current;
	            }
	        });
	        if (_value.Length > MaxLength)
	        {
	            ThrowHelper.ThrowArgumentException($"The file name is too long. Max Length allowed is {MaxLength}");
	        }
	        if (error is not null)
	        {
	            ThrowHelper.ThrowArgumentException(error);
	        }
	    }
	    public const int MaxLength = 255;
	    #region Methods
	    public bool HasExtension(out string extension)
	    {
	        return (extension = Path.GetExtension(_value)!) is not null;
	    }
	    public bool Equals(FileName other)
	    {
	        return Equals(other, CultureInfo.InvariantCulture);
	    }
	    public bool Equals(FileName other, CultureInfo cultureInfo)
	    {
	        return StringComparer.Create(cultureInfo, true).Equals(_value, other._value);
	    }
	    public int CompareTo(FileName other)
	    {
	        return CompareTo(other, CultureInfo.InvariantCulture);
	    }
	    public int CompareTo(FileName other, CultureInfo cultureInfo)
	    {
	        return StringComparer.Create(cultureInfo, true).Compare(_value, other._value);
	    }
	    public int GetHashCode(CultureInfo cultureInfo)
	    {
	        return StringComparer.Create(cultureInfo, true).GetHashCode(_value);
	    }
	    #region Overloads
	    public override bool Equals([NotNullWhen(true)] object? obj)
	    {
	        if (obj is null)
	        {
	            return false;
	        }
	        if (obj is not FileName name)
	        {
	            return false;
	        }
	        return Equals(name);
	    }
	    // <inheritdoc />
	    public override string ToString()
	    {
	        return _value;
	    }
	    // <inheritdoc />
	    public override int GetHashCode()
	    {
	        return GetHashCode(CultureInfo.InvariantCulture);
	    }
	    #endregion
	    #endregion
	    #region Operators
	    public static implicit operator FileName(string value)
	    {
	        return new FileName(value);
	    }
	    public static implicit operator string(FileName name)
	    {
	        return name._value;
	    }
	    public static implicit operator FileSystemPath(FileName name)
	    {
	        return name._value;
	    }
	    public static bool operator ==(FileName left, FileName right)
	    {
	        return left.Equals(right);
	    }
	    public static bool operator !=(FileName left, FileName right)
	    {
	        return !left.Equals(right);
	    }
	    #endregion
	}
	[DebuggerDisplay("{_value}")]
	[JsonConverter(typeof(PathJsonConverter))]
	public readonly struct FileSystemPath : IEquatable<FileSystemPath>, IComparable<FileSystemPath>
	#if NET7_0_OR_GREATER
	    , IEqualityOperators<FileSystemPath, FileSystemPath, bool>
	    , IAdditionOperators<FileSystemPath, FileSystemPath, FileSystemPath>
	#endif
	{
	    private readonly string _value;
	    private FileSystemPath(string value)
	    {
	        _value = value;
	    }
	    public const int MaxLength = 4096;
	    public const char Separator = '/';
	    public int Length => _value.Length;
	    public char this[int index] => _value[index];
	    public bool IsEmpty => _value.Length == 0;
	    public static FileSystemPath Empty { get; } = new FileSystemPath("");
	    #region Methods
	    public ReadOnlySpan<char> AsSpan()
	    {
	        return _value.AsSpan();
	    }
	    public string[] GetSegments()
	    {
	        //if (HasRoot(out string root))
	        //{
	        //    return _value.Substring(root.Length).Split(Separator, StringSplitOptions.RemoveEmptyEntries);
	        //}
	        //return _value.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
	        ReadOnlySpan<char> span = _value.AsSpan();
	        if (HasRoot(out string root))
	        {
	            span = span.Slice(root.Length);
	        }
	        var segments = new List<string>();
	        int start = 0;
	        for (int i = 0; i < span.Length; i++)
	        {
	            if (span[i] == Separator)
	            {
	                if (i > start)
	                {
	                    segments.Add(span.Slice(start, i - start).ToString());
	                }
	                start = i + 1;
	            }
	        }
	        if (start < span.Length)
	        {
	            segments.Add(span.Slice(start).ToString());
	        }
	        return segments.ToArray();
	    }
	    public bool HasRoot()
	    {
	        if (IsEmpty)
	        {
	            return false;
	        }
	        ReadOnlySpan<char> path = _value.AsSpan();
	        int length = path.Length;
	        if (length < 1 || !IsDirectorySeparator(path[0]))
	        {
	            if (length >= 2 && IsValidDriveChar(path[0]))
	            {
	                return path[1] == ':';
	            }
	            return false;
	        }
	        return true;
	    }
	    public bool HasRoot(out string root)
	    {
	        root = default!;
	        if (IsEmpty)
	        {
	            return false;
	        }
	        var value = GetPathRoot(_value)!;
	        if (!string.IsNullOrEmpty(value))
	        {
	            root = string.Create(value.Length, value, (span, item) =>
	            {
	                for (int i = 0; i < item.Length; i++)
	                {
	                    if (item[i] == '\\')
	                    {
	                        span[i] = '/';
	                    }
	                    else
	                    {
	                        span[i] = item[i];
	                    }
	                }
	            });
	            return true;
	        }
	        return false;
	    }
	    public bool HasDrive(out char drive)
	    {
	        drive = '\0'; // set it too null char
	        if (HasDriveLetter(_value))
	        {
	            drive = _value[0];
	            return true;
	        }
	        return false;
	    }
	    public bool HasShare(out string share)
	    {
	        share = null!;
	        if (HasRoot(out var root) && root.Length >= 5 && IsPathSeparator(root[0]) && IsPathSeparator(root[1]))
	        {
	            share = root;
	            return true;
	        }
	        return false;
	    }
	    public FileSystemPath Join(FileSystemPath path)
	    {
	        return Join(this, path);
	    }
	    public static FileSystemPath Join(FileSystemPath left, FileSystemPath right)
	    {
	        if (right.IsEmpty)
	        {
	            return left;
	        }
	        if (left.IsEmpty)
	        {
	            return right;
	        }
	        if (right.HasRoot(out var root) && root.Length != 1 && root[0] != Separator)
	        {
	            ThrowHelper.ThrowArgumentException("The right most path must not be rooted in either '[Drive]:/' or '//[Server]/[share]'.");
	        }
	        return string.Join(Separator, left._value, right._value.Trim(Separator));
	    }
	    public FileSystemPath Combine(FileSystemPath path)
	    {
	        return Combine(this, path);
	    }
	    public static FileSystemPath Combine(FileSystemPath left, FileSystemPath right)
	    {
	        if (right.StartsWith(left))
	        {
	            return right;
	        }
	        return Join(right, left);
	    }
	    public bool EndsWith(FileSystemPath path)
	    {
	        return EndsWith(path, CultureInfo.InvariantCulture);
	    }
	    public bool EndsWith(FileSystemPath path, CultureInfo cultureInfo)
	    {
	        return _value.EndsWith(path._value, true, cultureInfo);
	    }
	    public bool StartsWith(FileSystemPath path)
	    {
	        return StartsWith(path, CultureInfo.InvariantCulture);
	    }
	    public bool StartsWith(FileSystemPath path, CultureInfo cultureInfo)
	    {
	        return _value.StartsWith(path._value, true, cultureInfo);
	    }
	    public int GetHashCode(CultureInfo cultureInfo)
	    {
	        int code = StringComparer.Create(cultureInfo, true).GetHashCode(_value);
	        return (int)((uint)code | ((uint)code << 16));
	    }
	    public bool Equals(FileSystemPath other)
	    {
	        return Equals(other, CultureInfo.InvariantCulture);
	    }
	    public bool Equals(FileSystemPath other, CultureInfo cultureInfo)
	    {
	        return StringComparer.Create(cultureInfo, true).Equals(_value, other._value);
	    }
	    public int CompareTo(FileSystemPath other)
	    {
	        return CompareTo(other, CultureInfo.InvariantCulture);
	    }
	    public int CompareTo(FileSystemPath other, CultureInfo cultureInfo)
	    {
	        return StringComparer.Create(cultureInfo, true).Compare(_value, other._value);
	    }
	    public static FileSystemPath Parse(string value)
	    {
	        ThrowHelper.ThrowIfNullOrEmpty(value, nameof(value));
	        // Check if only root was passed
	        if (value.Length == 1)
	        {
	            if (IsPathSeparator(value[0]))
	            {
	                return new FileSystemPath("/");
	            }
	            if (IsDot(value[0])) // "." is current directory
	            {
	                return Empty;
	            }
	        }
	        int start = 0;
	        int end = value.Length - 1;
	        int shift = 0;
	        // Check for current directory syntax "./" and skip over
	        if (value.Length >= 2 && IsDot(value[0]) && IsPathSeparator(value[1]))
	        {
	            start =+ 2;
	        }
	        // Check if path has valid drive, if so disregard shift
	        if (HasDriveLetter(value))
	        {
	            shift = 0;
	        }
	        // Check for leading slash root  '//' or '\\', or if your a weirdo '/\' '\/'
	        else if (value.Length >= 2 && IsPathSeparator(value[0]) && IsPathSeparator(value[1]))
	        {
	            shift =+ 2;
	        }
	        // Maintain directory root '/'
	        else if (value.Length >= 2 && IsPathSeparator(value[0]))
	        {
	            shift = 1;
	        }
	        CalculateTrimRange(value, ref start, ref end);
	        int reduce = 0;
	        int length = ((end + 1) - start) + shift;
	        var span = new Span<char>(new char[length]);
	        for (int i = 0; i < shift; i++)
	        {
	            span[i] = Separator;
	        }
	        char previous = default;
	        // Let's convert all backward slashes to forward slashes
	        for (int i = start; i < (end + 1); i++)
	        {
	            var current = value[i];
	            // Convert back slash to forward slash
	            if (current == '\\')
	            {
	                current = Separator;
	            }
	            // Check for excessive slashes
	            if (IsPathSeparator(previous) && IsPathSeparator(current))
	            {
	                reduce++;
	                continue;
	            }
	            // Check for parent directory globing ".."
	            if (IsDot(previous) && IsDot(current))
	            {
	                // scenario 1: ".." was only passed
	                // scenario 2: "{directory}/../{directory}"
	                // scenario 3: "../{directory}"
	                // scenario 4: "/{directory}/.."
	                var s = i - 2;
	                var e = i + 1;
	                var hasStart = (s > 0 && IsPathSeparator(value[s])) || s < 0;
	                var hasEnd = (e < end && IsPathSeparator(value[e])) || e > end;
	                if ((s < 0 && e > end) || (hasStart && hasEnd))
	                {
	                    ThrowHelper.ThrowArgumentException("Parent directory globing is not allowed - \"..\". The value must be an absolute or relative path.");
	                }
	            }
	            if (!IsValidPathChar(current))
	            {
	                ThrowHelper.ThrowArgumentException($"Path contains illegal character '{current}' at index {i}.");
	            }
	            previous = current;
	            span[(i + shift) - start - reduce] = current;
	        }
	       // span[span.Length - reduce]
	        if (reduce > 0)
	        {
	            span = span.Slice(0, span.Length - reduce);
	        }
	        if (span.Length > MaxLength)
	        {
	            ThrowHelper.ThrowArgumentException("The path is too long");
	        }
	        return new FileSystemPath(span.ToString());
	    }
	    #region Overloads
	    public override bool Equals(object? obj)
	    {
	        if (obj is FileSystemPath path)
	        {
	            return Equals(path);
	        }
	        return false;
	    }
	    public override string ToString()
	    {
	        return _value;
	    }
	    public override int GetHashCode()
	    {
	        return GetHashCode(CultureInfo.InvariantCulture);
	    }
	    #endregion
	    #endregion
	    #region Operators
	    public static implicit operator FileSystemPath(string path)
	    {
	        return Parse(path);
	    }
	    public static implicit operator string(FileSystemPath path)
	    {
	        return path._value;
	    }
	    public static bool operator ==(FileSystemPath left, FileSystemPath right)
	    {
	        return left.Equals(right);
	    }
	    public static bool operator !=(FileSystemPath left, FileSystemPath right)
	    {
	        return !left.Equals(right);
	    }
	    public static FileSystemPath operator +(FileSystemPath left, FileSystemPath right)
	    {
	        return left.Combine(right);
	    }
	    #endregion
	    #region Partials
	    partial class PathJsonConverter : JsonConverter<FileSystemPath>
	    {
	        public override FileSystemPath Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
	        {
	            if (reader.TokenType != JsonTokenType.String)
	            {
	                throw new JsonException("");
	            }
	            var str = reader.GetString();
	            if (string.IsNullOrEmpty(str))
	            {
	                return Empty;
	            }
	            return new FileSystemPath(str);
	        }
	        public override void Write(Utf8JsonWriter writer, FileSystemPath value, JsonSerializerOptions options)
	        {
	            var str = value.ToString();
	            writer.WriteStringValue(str);
	        }
	    }
	    #endregion
	}
	public sealed class FileSystemPathComparer : IEqualityComparer<FileSystemPath>, IComparer<FileSystemPath>
	{
	    private readonly CultureInfo _cultureInfo;
	    private FileSystemPathComparer(CultureInfo cultureInfo)
	    {
	        _cultureInfo = cultureInfo;
	    }
	    public int Compare(FileSystemPath left, FileSystemPath right)
	    {
	        return left.CompareTo(right, _cultureInfo);
	    }
	    public bool Equals(FileSystemPath left, FileSystemPath right)
	    {
	        return left.Equals(right, _cultureInfo);
	    }
	    public int GetHashCode([DisallowNull] FileSystemPath path)
	    {
	        return path.GetHashCode(_cultureInfo);
	    }
	    public static FileSystemPathComparer Create(CultureInfo cultureInfo)
	    {
	        ThrowHelper.ThrowIfNull(cultureInfo, nameof(cultureInfo));
	        return new FileSystemPathComparer(cultureInfo);
	    }
	    public static FileSystemPathComparer CurrentCulture { get; } = new FileSystemPathComparer(CultureInfo.InvariantCulture);
	    public static FileSystemPathComparer InvariantCulture { get; } = new FileSystemPathComparer(CultureInfo.InvariantCulture);
	}
	[DebuggerDisplay("{ToString()}")]
	public sealed partial class Glob 
	{
	    private static readonly Parser _parser = new Parser();
	    private readonly TokenBase[] _tokens;
	    internal Glob(TokenBase[] tokens)
	    {
	        _tokens = ThrowHelper.ThrowIfNullOrNone(tokens);
	    }
	    public int Count => Tokens.Length;
	    public Token[] Tokens => _tokens;
	    #region Methods
	    public bool IsMatch(FileSystemPath path)
	    {
	        return IsMatch(path, CultureInfo.InvariantCulture);
	    }
	    public bool IsMatch(FileSystemPath path, bool caseInSensitive)
	    {
	        return IsMatch(path, CultureInfo.InvariantCulture, caseInSensitive);
	    }
	    public bool IsMatch(FileSystemPath path, CultureInfo cultureInfo)
	    {
	        return IsMatch(path, cultureInfo, false);
	    }
	    public bool IsMatch(FileSystemPath path, CultureInfo cultureInfo, bool caseInSensitive)
	    {
	        bool consumesVariableLength = _tokens.Length == 0;
	        int consumesMinLength = 0;
	        int position = 0;
	        for (int i = 0; i < _tokens.Length; i++)
	        {
	            consumesMinLength = consumesMinLength + _tokens[i].ConsumesMinLength;
	            if (!consumesVariableLength)
	            {
	                if (_tokens[i].ConsumesVariableLength)
	                {
	                    consumesVariableLength = true;
	                }
	            }
	        }
	        if (!consumesVariableLength)
	        {
	            if ((path.Length - position) != consumesMinLength)
	            {
	                // can't possibly match as tokens require a fixed length and the string length is different.
	                return false;
	            }
	        }
	        else if ((path.Length - position) < consumesMinLength)
	        {
	            // can't possibly match as tokens require a minimum length and the string is too short.
	            return false;
	        }
	        var span = path.AsSpan();
	        for (int i = 0; i < _tokens.Length; i++)
	        {
	            if (!_tokens[i].Test(span, cultureInfo, caseInSensitive, position, out position))
	            {
	                return false;
	            }
	        }
	        // if all tokens matched but still more text then fail!
	        if (position < path.Length - 1)
	        {
	            return false;
	        }
	        // Success.
	        return true;
	    }
	    public static Glob Parse(string pattern)
	    {
	        ThrowHelper.ThrowIfNullOrEmpty(pattern);
	        var tokens = _parser.Tokenize(pattern);
	        return new Glob(tokens);
	    }
	    #region Overloads
	    public override string ToString()
	    {
	        var builder = new StringBuilder();
	        for(int i = 0; i < _tokens.Length; i++)
	        {
	            Format(builder, _tokens[i]);
	        }
	        return builder.ToString();
	    }
	    private static StringBuilder Format(StringBuilder builder, TokenBase token)
	    {
	        if (token is CompositeGlobToken composite)
	        {
	            builder.Append(token.Value);
	            for (int i = 0; i < composite.Tokens.Length; i++)
	            {
	                Format(builder, composite.Tokens[i]);
	            }
	            return builder;
	        }
	        return builder.Append(token.Value);
	    }
	    public override int GetHashCode()
	    {
	        return ToString().GetHashCode();
	    }
	    #endregion
	    #endregion
	    #region Operators
	    public static implicit operator Glob(string pattern)
	    {
	        return Parse(pattern);
	    }
	    public static implicit operator string(Glob glob)
	    {
	        return glob.ToString();
	    }
	    #endregion
	    #region Partials
	    public enum TokenKind
	    {
	        Literal,
	        CharacterSet,
	        Any,
	        Wildcard,
	        WildcardDirectory,
	        Range,
	        PathSeparator,
	        // TODO - Support Brace Grouping
	        //BraceGrouping
	    }
	    public abstract class Token
	    {
	        internal Token() { }
	        public abstract string Value { get; }
	        public abstract TokenKind Kind { get; }
	    }
	    #endregion
	}
	public sealed partial class Glob
	{
	    internal partial class Lexer : StringReader
	    {
	        private readonly string _text;
	        private int _currentIndex;
	        public const int FailedRead = -1;
	        public const char NullChar = (char)0;
	        public const char ExclamationMarkChar = '!';
	        public const char StarChar = '*';
	        public const char OpenBracketChar = '[';
	        public const char CloseBracketChar = ']';
	        public const char DashChar = '-';
	        public const char QuestionMarkChar = '?';
	        public static char[] BeginningOfTokenCharacters = { StarChar, OpenBracketChar, QuestionMarkChar };
	        public static char[] AllowedNonAlphaNumericChars = { '.', ' ', '!', '#', '-', ';', '=', '@', '~', '_', ':' };
	        private static readonly char[] PathSeparators = { '/', '\\' };
	        public Lexer(string text) : base(text)
	        {
	            _text = text;
	            _currentIndex = -1;
	        }
	        #region Properties
	        public int CurrentIndex
	        {
	            get { return _currentIndex; }
	            private set
	            {
	                _currentIndex = value;
	                LastChar = _text[_currentIndex - 1];
	                CurrentChar = _text[_currentIndex];
	            }
	        }
	        public char LastChar { get; private set; }
	        public char CurrentChar { get; private set; }
	        public bool HasReachedEnd => base.Peek() == -1;
	        public bool IsWhiteSpace => char.IsWhiteSpace(CurrentChar);
	        public bool IsBeginningOfRangeOrList => CurrentChar == OpenBracketChar;
	        public bool IsEndOfRangeOrList => CurrentChar == CloseBracketChar;
	        public bool IsSingleCharacterMatch => CurrentChar == QuestionMarkChar;
	        public bool IsWildcardCharacterMatch => CurrentChar == StarChar && Peek() != StarChar;
	        public bool IsBeginningOfDirectoryWildcard => CurrentChar == StarChar && Peek() == StarChar;
	        #endregion
	        public bool TryRead()
	        {
	            return Read() != FailedRead;
	        }
	        public override int Read()
	        {
	            var result = base.Read();
	            if (result != FailedRead)
	            {
	                _currentIndex++;
	                LastChar = CurrentChar;
	                CurrentChar = (char)result;
	                return result;
	            }
	            return result;
	        }
	        public override int Read(char[] buffer, int index, int count)
	        {
	            var read = base.Read(buffer, index, count);
	            CurrentIndex += read;
	            CurrentChar = _text[CurrentIndex];
	            return read;
	        }
	        public override int ReadBlock(char[] buffer, int index, int count)
	        {
	            var read = base.ReadBlock(buffer, index, count);
	            CurrentIndex += read;
	            return read;
	        }
	        public override string ReadLine()
	        {
	            var readLine = base.ReadLine();
	            if (readLine != null)
	                CurrentIndex += readLine.Length;
	            return readLine!;
	        }
	        public string ReadPathSegment()
	        {
	            var segmentBuilder = new StringBuilder();
	            while (TryRead())
	            {
	                if (!IsPathSeparator(CurrentChar))
	                {
	                    segmentBuilder.Append(CurrentChar);
	                }
	                else
	                {
	                    break;
	                }
	            }
	            return segmentBuilder.ToString();
	        }
	        public override string ReadToEnd()
	        {
	            CurrentIndex = _text.Length - 1;
	            return base.ReadToEnd();
	        }
	        public new char Peek()
	        {
	            if (HasReachedEnd)
	            {
	                return NullChar;
	            }
	            return (char)base.Peek();
	        }
	        public bool IsPathSeparator()
	        {
	            return IsPathSeparator(CurrentChar);
	        }
	        public static bool IsPathSeparator(char character)
	        {
	            var isCurrentCharacterStartOfDelimiter = character == PathSeparators[0] ||
	                                                     character == PathSeparators[1];
	            return isCurrentCharacterStartOfDelimiter;
	        }
	        public static bool IsNotStartOfToken(char character)
	        {
	            return !BeginningOfTokenCharacters.Contains(character);
	        }
	    }
	    internal partial class Parser
	    {
	        private readonly StringBuilder _buffer;
	        public Parser()
	        {
	            _buffer = new StringBuilder();
	        }
	        public TokenBase[] Tokenize(string pattern)
	        {
	            var tokens = new List<TokenBase>();
	            using (var reader = new Lexer(pattern))
	            {
	                while (reader.TryRead())
	                {
	                    if (reader.IsBeginningOfRangeOrList)
	                    {
	                        tokens.Add(ParseRangeOrCharacterSet(reader));
	                    }
	                    else if (reader.IsSingleCharacterMatch)
	                    {
	                        tokens.Add(ParseSingleCharacterMatch());
	                    }
	                    else if (reader.IsWildcardCharacterMatch)
	                    {
	                        tokens.Add(ParseWildcard(reader));
	                    }
	                    else if (reader.IsPathSeparator())
	                    {
	                        var sepToken = ParsePathSeparator(reader);
	                        tokens.Add(sepToken);
	                    }
	                    else if (reader.IsBeginningOfDirectoryWildcard)
	                    {
	                        if (tokens.Count > 0)
	                        {
	                            if (tokens[tokens.Count - 1] is PathSeparatorToken lastToken)
	                            {
	                                tokens.Remove(lastToken);
	                                tokens.Add(ParseDirectoryWildcard(reader, lastToken));
	                                continue;
	                            }
	                        }
	                        tokens.Add(ParseDirectoryWildcard(reader, null!));
	                    }
	                    else
	                    {
	                        tokens.Add(ParseLiteral(reader));
	                    }
	                }
	            }
	            _buffer.Clear();
	            return tokens.ToArray();
	        }
	        private TokenBase[] ParseComposite(Lexer reader)
	        {
	            var tokens = new List<TokenBase>();
	            while (reader.TryRead())
	            {
	                if (reader.IsBeginningOfRangeOrList)
	                {
	                    tokens.Add(ParseRangeOrCharacterSet(reader));
	                }
	                else if (reader.IsSingleCharacterMatch)
	                {
	                    tokens.Add(ParseSingleCharacterMatch());
	                }
	                else if (reader.IsWildcardCharacterMatch)
	                {
	                    tokens.Add(ParseWildcard(reader));
	                }
	                else if (reader.IsPathSeparator())
	                {
	                    var sepToken = ParsePathSeparator(reader);
	                    tokens.Add(sepToken);
	                }
	                else if (reader.IsBeginningOfDirectoryWildcard)
	                {
	                    if (tokens.Count > 0)
	                    {
	                        if (tokens[tokens.Count - 1] is PathSeparatorToken lastToken)
	                        {
	                            tokens.Remove(lastToken);
	                            tokens.Add(ParseDirectoryWildcard(reader, lastToken));
	                            continue;
	                        }
	                    }
	                    tokens.Add(ParseDirectoryWildcard(reader, null!));
	                }
	                else
	                {
	                    tokens.Add(ParseLiteral(reader));
	                }
	            }
	            return tokens.ToArray();
	        }
	        private TokenBase ParseDirectoryWildcard(Lexer reader, PathSeparatorToken leadingPathSeparatorToken)
	        {
	            reader.TryRead();
	            if (Lexer.IsPathSeparator(reader.Peek()))
	            {
	                reader.TryRead();
	                var trailingSeparator = ParsePathSeparator(reader);
	                return new WildcardDirectoryToken(
	                    leadingPathSeparatorToken,
	                    (PathSeparatorToken)trailingSeparator,
	                    ParseComposite(reader));
	            }
	            return new WildcardDirectoryToken(
	                leadingPathSeparatorToken,
	                null!,
	                ParseComposite(reader)); // this shouldn't happen unless a pattern ends with ** which is weird. **sometext is not legal.
	        }
	        private TokenBase ParseLiteral(Lexer reader)
	        {
	            AcceptCurrentChar(reader);
	            while (!reader.HasReachedEnd)
	            {
	                var peek = reader.Peek();
	                var isValid = Lexer.IsNotStartOfToken(peek) && !Lexer.IsPathSeparator(peek);
	                if (isValid)
	                {
	                    if (reader.TryRead())
	                    {
	                        AcceptCurrentChar(reader);
	                    }
	                    else
	                    {
	                        // potentially hit end of string.
	                        break;
	                    }
	                }
	                else
	                {
	                    // we have hit a character that may not be a valid literal (could be unsupported, or start of a token for instance).
	                    break;
	                }
	            }
	            return new LiteralToken(GetBufferAndReset());
	        }
	        private TokenBase ParseRangeOrCharacterSet(Lexer reader) // Parses a token for a range or list globbing expression.
	        {
	            bool isNegated = false;
	            bool isNumberRange = false;
	            bool isLetterRange = false;
	            bool isCharList = false;
	            if (reader.Peek() == Lexer.ExclamationMarkChar)
	            {
	                isNegated = true;
	                reader.Read();
	            }
	            var nextChar = reader.Peek();
	            if (Char.IsLetterOrDigit(nextChar))
	            {
	                reader.Read();
	                nextChar = reader.Peek();
	                if (nextChar == Lexer.DashChar)
	                {
	                    if (Char.IsLetter(reader.CurrentChar))
	                    {
	                        isLetterRange = true;
	                    }
	                    else
	                    {
	                        isNumberRange = true;
	                    }
	                }
	                else
	                {
	                    isCharList = true;
	                }
	                AcceptCurrentChar(reader);
	            }
	            else
	            {
	                isCharList = true;
	                reader.Read();
	                AcceptCurrentChar(reader);
	            }
	            if (isLetterRange || isNumberRange)
	            {
	                // skip over the dash char
	                reader.TryRead();
	            }
	            while (reader.TryRead())
	            {
	                if (reader.IsEndOfRangeOrList)
	                {
	                    var peekChar = reader.Peek();
	                    // Close brackets within brackets are escaped with another
	                    // Close bracket. e.g. [a]] matches a[
	                    if (peekChar == Lexer.CloseBracketChar)
	                    {
	                        AcceptCurrentChar(reader);
	                    }
	                    else
	                    {
	                        break;
	                    }
	                }
	                else
	                {
	                    AcceptCurrentChar(reader);
	                }
	            }
	            // construct token
	            TokenBase result = null!;
	            var value = GetBufferAndReset();
	            if (isCharList)
	            {
	                result = new CharacterSetToken(value.ToCharArray(), isNegated);
	            }
	            else if (isLetterRange)
	            {
	                var start = value[0];
	                var end = value[1];
	                result = new RangeToken(start, end, isNegated);
	            }
	            else if (isNumberRange)
	            {
	                var start = value[0]; // int.Parse(value[0].ToString());
	                var end = value[1]; // int.Parse(value[1].ToString());
	                result = new RangeToken(start, end, isNegated);
	            }
	            return result;
	        }
	        private TokenBase ParsePathSeparator(Lexer reader)
	        {
	            return new PathSeparatorToken();
	        }
	        private TokenBase ParseWildcard(Lexer reader)
	        {
	            var children = ParseComposite(reader);
	            return new WildcardToken(children);
	        }
	        private TokenBase ParseSingleCharacterMatch()
	        {
	            return new AnyCharacterToken();
	        }
	        private void AcceptCurrentChar(Lexer reader)
	        {
	            if (reader.CurrentChar == '\\')
	            {
	                _buffer.Append('/'); // Normalize any backslashes to forward slashes
	            }
	            else
	            {
	                _buffer.Append(reader.CurrentChar);
	            }
	        }
	        private string GetBufferAndReset()
	        {
	            var text = _buffer.ToString();
	            _buffer.Clear();
	            return text;
	        }
	        //private void AcceptChar(char character)
	        //{
	        //    _buffer.Append(character);
	        //}
	    }
	}
	public sealed partial class Glob
	{
	    [DebuggerDisplay("{Kind} - {ToString()}")]
	    internal abstract class TokenBase : Token
	    {
	        public abstract int ConsumesMinLength { get; }
	        public virtual bool ConsumesVariableLength { get; }
	        public abstract bool Test(
	            ReadOnlySpan<char> path,
	            CultureInfo cultureInfo,
	            bool ignoreCase,
	            int position,
	            out int next);
	        public override string ToString()
	        {
	            var builder = new StringBuilder();
	            Format(builder, this);
	            return builder.ToString();
	        }
	    }
	    internal abstract class NegatableGlobToken : TokenBase
	    {
	        public abstract bool IsNegated { get; }
	    }
	    internal abstract class CompositeGlobToken : TokenBase
	    {
	        protected CompositeGlobToken(TokenBase[] tokens)
	        {
	            Tokens = tokens;
	            for (int i = 0; i < tokens.Length; i++)
	            {
	                ConsumesAnyMinLength = ConsumesAnyMinLength + tokens[i].ConsumesMinLength;
	                if (!ConsumesAnyVariableLength)
	                {
	                    if (tokens[i].ConsumesVariableLength)
	                    {
	                        ConsumesAnyVariableLength = true;
	                    }
	                }
	            }
	        }
	        public TokenBase[] Tokens { get; }
	        public bool ConsumesAnyVariableLength { get;  }
	        public int ConsumesAnyMinLength { get; } = 0;
	        public bool TestAll(
	            ReadOnlySpan<char> path,
	            CultureInfo cultureInfo,
	            bool ignoreCase,
	            int position, out
	            int next)
	        {
	            next = position;
	            if (!ConsumesAnyVariableLength)
	            {
	                if ((path.Length - position) != ConsumesAnyMinLength)
	                {
	                    // can't possibly match as tokens require a fixed length and the string length is different.
	                    return false;
	                }
	            }
	            else if ((path.Length - position) < ConsumesAnyMinLength)
	            {
	                // can't possibly match as tokens require a minimum length and the string is too short.
	                return false;
	            }
	            foreach (var token in Tokens)
	            {
	                if (!token.Test(path, cultureInfo, ignoreCase, next, out next))
	                {
	                    return false;
	                }
	            }
	            // if all tokens matched but still more text then fail!
	            if (next < path.Length - 1)
	            {
	                return false;
	            }
	            // Success.
	            return true;
	        }
	    }
	    internal class PathSeparatorToken : TokenBase
	    {
	        public override string Value { get; } = "/";
	        public override TokenKind Kind { get; } = TokenKind.PathSeparator;
	        public override int ConsumesMinLength => 1;
	        public override bool Test(
	            ReadOnlySpan<char> path,
	            CultureInfo cultureInfo,
	            bool ignoreCase, int position, out int next)
	        {
	            var currentChar = path[position];
	            next = position + 1;
	            return Lexer.IsPathSeparator(currentChar);
	        }
	    }
	    internal class AnyCharacterToken : TokenBase
	    {
	        public override string Value { get; } = "?";
	        public override TokenKind Kind { get; } = TokenKind.Any;
	        public override int ConsumesMinLength => 1;
	        public override bool Test(
	            ReadOnlySpan<char> path,
	            CultureInfo cultureInfo,
	            bool ignoreCase,
	            int position, out
	            int next)
	        {
	            next = position + 1;
	            var currentChar = path[position];
	            if (Lexer.IsPathSeparator(currentChar))
	            {
	                return false;
	            }
	            return true;
	        }
	    }
	    internal class CharacterSetToken : NegatableGlobToken
	    {
	        public CharacterSetToken(char[] characters, bool isNegated)
	        {
	            Characters = characters;
	            IsNegated = isNegated;
	            Value = string.Create(characters.Length + (isNegated ? 3 : 2), characters, (span, array) =>
	            {
	                int start = isNegated ? 2 : 1;
	                span[0] = '[';
	                if (isNegated)
	                {
	                    span[1] = '!';
	                }
	                for (int i = start; i < span.Length - 1; i++)
	                {
	                    span[i] = array[i - start];
	                }
	                span[span.Length - 1] = ']';
	            });
	        }
	        public char[] Characters { get; }
	        public override string Value { get; }
	        public override bool IsNegated { get; }
	        public override int ConsumesMinLength => 1;
	        public override TokenKind Kind { get; } = TokenKind.CharacterSet;
	        public override bool Test(
	            ReadOnlySpan<char> path,
	            CultureInfo cultureInfo,
	            bool ignoreCase,
	            int position, out
	            int next)
	        {
	            var text = cultureInfo.TextInfo;
	            var value = path[position];
	            next = position + 1;
	            bool contains = false;
	            if (ignoreCase)
	            {
	                for (int i = 0; i < Characters.Length; i++)
	                {
	                    if (text.ToUpper(Characters[i]).Equals(text.ToUpper(value)))
	                    {
	                        contains = true;
	                    }
	                }
	            }
	            else
	            {
	                for (int i = 0; i < Characters.Length; i++)
	                {
	                    if (Characters[i].Equals(value))
	                    {
	                        contains = true;
	                    }
	                }
	            }
	            if (IsNegated)
	            {
	                return !contains;
	            }
	            else
	            {
	                return contains;
	            }
	        }
	    }
	    internal class RangeToken : NegatableGlobToken
	    {
	        public RangeToken(char start, char end, bool isNegated)
	        {
	            Start = start;
	            End = end;
	            IsNegated = isNegated;
	            Value = string.Create(5 + (isNegated ? 1 : 0), (Start, End), (span, tuple) =>
	            {
	                span[0] = '[';
	                if (isNegated)
	                {
	                    span[1] = '!';
	                    span[2] = tuple.Start;
	                    span[3] = '-';
	                    span[4] = tuple.End;
	                    span[5] = ']';
	                }
	                else
	                {
	                    span[1] = tuple.Start;
	                    span[2] = '-';
	                    span[3] = tuple.End;
	                    span[4] = ']';
	                }
	            });
	        }
	        public override string Value { get; }
	        public char Start { get; }
	        public char End { get; }
	        public override bool IsNegated { get; }
	        public override int ConsumesMinLength => 1;
	        public override TokenKind Kind { get; } = TokenKind.Range;
	        public override bool Test(
	            ReadOnlySpan<char> path,
	            CultureInfo cultureInfo,
	            bool ignoreCase,
	            int position, out
	            int next)
	        {
	            if (char.IsDigit(Start) && char.IsDigit(End))
	            {
	                return TestNumberRange(path, cultureInfo, ignoreCase, position, out next);
	            }
	            else if (ignoreCase)
	            {
	                return TestCaseInSensitiveLetterRange(path, cultureInfo, position, out next);
	            }
	            else
	            {
	                return TestCaseSensitiveLetterRange(path, cultureInfo, position, out next);
	            }
	        }
	        private bool TestNumberRange(
	            ReadOnlySpan<char> path,
	            CultureInfo cultureInfo,
	            bool ignoreCase,
	            int position,
	            out int next)
	        {
	            var currentChar = path[position];
	            next = position + 1;
	            if (currentChar >= Start && currentChar <= End)
	            {
	                if (IsNegated)
	                {
	                    return false;
	                }
	            }
	            else if (!IsNegated)
	            {
	                return false;
	            }
	            return true;
	        }
	        private bool TestCaseSensitiveLetterRange(
	            ReadOnlySpan<char> path,
	            CultureInfo cultureInfo,
	            int position,
	            out int next)
	        {
	            next = position + 1;
	            char currentChar;
	            currentChar = path[position];
	            bool isMatch = currentChar >= Start && currentChar <= End;
	            if (IsNegated)
	            {
	                return !isMatch;
	            }
	            else
	            {
	                return isMatch;
	            }
	        }
	        private bool TestCaseInSensitiveLetterRange(
	            ReadOnlySpan<char> path,
	            CultureInfo cultureInfo,
	            int position,
	            out int next)
	        {
	            next = position + 1;
	            TextInfo text = cultureInfo.TextInfo;
	            char currentChar;
	            currentChar = path[position];
	            var lowerStart = text.ToLower(Start);
	            var lowerEnd = text.ToLower(End);
	            var upperStart = text.ToUpper(Start);
	            var upperEnd = text.ToUpper(End);
	            bool isMatch = (currentChar >= lowerStart && currentChar <= lowerEnd)
	                || (currentChar >= upperStart && currentChar <= upperEnd);
	            if (IsNegated)
	            {
	                return !isMatch;
	            }
	            else
	            {
	                return isMatch;
	            }
	        }
	    }
	    internal class WildcardToken : CompositeGlobToken
	    {
	        public WildcardToken(TokenBase[] tokens) : base(tokens)
	        {
	        }
	        public override string Value { get; } = "*";
	        public override TokenKind Kind { get; } = TokenKind.Wildcard;
	        public override int ConsumesMinLength => ConsumesAnyMinLength;
	        public override bool ConsumesVariableLength { get; } = true;
	        public override bool Test(
	            ReadOnlySpan<char> path,
	            CultureInfo cultureInfo,
	            bool ignoreCase,
	            int position,
	            out int next)
	        {
	            next = position;
	            if (Tokens.Length == 0) // We are the last token in the pattern
	            {
	                // If we have reached the end of the string, then we match.
	                if (position >= path.Length)
	                {
	                    return true;
	                }
	                // We don't match if the remaining string has separators.
	                for (int i = position; i <= path.Length - 1; i++)
	                {
	                    var currentChar = path[i];
	                    if (currentChar == '/' || currentChar == '\\')
	                    {
	                        return false;
	                    }
	                }
	                // we have matched up to the new position.
	                next = position + path.Length;
	                return true;
	            }
	            // We are not the last token in the pattern, and so the _subEvaluator representing the remaining pattern tokens must also match.
	            // Does the sub pattern match a fixed length string, or variable length string?
	            if (!ConsumesAnyVariableLength)
	            {
	                // The remaining tokens match against a fixed length string, so we can infer that this wildcard **must** match
	                // a fixed amount of characters in order for the subevaluator to match its fixed amount of characters from the remaining portion
	                // of the string. 
	                // So we must match up-to that position. We can't match separators. 
	                var requiredMatchPosition = path.Length - ConsumesAnyMinLength;
	                //if (requiredMatchPosition < currentPosition)
	                //{
	                //    return false;
	                //}
	                for (int i = position; i < requiredMatchPosition; i++)
	                {
	                    var currentChar = path[i];
	                    if (currentChar == '/' || currentChar == '\\')
	                    {
	                        return false;
	                    }
	                }
	                var isMatch = TestAll(path, cultureInfo, ignoreCase, requiredMatchPosition, out next);
	                return isMatch;
	            }
	            // We can match a variable amount of characters but,
	            // We can't match more characters than the amount that will take us past the min required length required by the sub evaluator tokens,
	            // and as we are not a directory wildcard, we can't match past a path separator.
	            var maxPos = path.Length - 1;
	            if (ConsumesMinLength > 0)
	            {
	                maxPos = maxPos - ConsumesMinLength + 1;
	            }
	            // var maxPos = (allChars.Length - _subEvaluator.ConsumesMinLength);
	            for (int i = position; i <= maxPos; i++)
	            {
	                var isMatch = TestAll(path, cultureInfo, ignoreCase, i, out next);
	                if (isMatch)
	                {
	                    return true;
	                }
	                var currentChar = path[i];
	                if (currentChar == '/' || currentChar == '\\')
	                {
	                    return false;
	                }
	            }
	            // If subevakuators are optional match then match
	            if (ConsumesMinLength == 0)
	            {
	                return true;
	            }
	            return false;
	        }
	    }
	    internal class WildcardDirectoryToken : CompositeGlobToken
	    {
	        public WildcardDirectoryToken(
	            PathSeparatorToken leading,
	            PathSeparatorToken trailing,
	            TokenBase[] tokens) : base(tokens)
	        {
	            Leading = leading;
	            Trailing = trailing;
	        }
	        public override string Value
	        {
	            get
	            {
	                if (Leading is not null && Trailing is not null)
	                {
	                    return "/**/";
	                }
	                if (Leading is not null)
	                {
	                    return "/**";
	                }
	                if (Trailing is not null)
	                {
	                    return "**/";
	                }
	                else
	                {
	                    return "**";
	                }
	            }
	        }
	        public PathSeparatorToken Trailing { get; }
	        public PathSeparatorToken Leading { get; }
	        public override TokenKind Kind { get; } = TokenKind.WildcardDirectory;
	        public override int ConsumesMinLength => ConsumesAnyMinLength;
	        public override bool ConsumesVariableLength { get; } = true;
	        public override bool Test(
	            ReadOnlySpan<char> path,
	            CultureInfo cultureInfo,
	            bool ignoreCase,
	            int position, out
	            int next)
	        {
	            // We shortcut to success for a ** in some special cases:-
	            //  1. The remaining tokens don't need to consume a minimum number of chracters in order to match.
	            // We shortcut to failure for a ** in some special cases:-
	            // A) The token was parsed with a leading path separator (i.e '/**' and the current charater we are matching from isn't a path separator.
	            next = position;
	            //  bool matchedLeadingSeperator = false;
	            // A) If leading seperater then current character needs to be that seperator.
	            if (path.Length <= position || position < 0)
	            {
	                return false;
	            }
	            char currentChar = path[position];
	            if (Leading is not null)
	            {
	                if (!Lexer.IsPathSeparator(currentChar))
	                {
	                    // expected separator.
	                    return false;
	                }
	                //else
	                //{
	                // advance current position to match the leading separator.
	                //  matchedLeadingSeperator = true;
	                position = position + 1;
	                //}
	            }
	            else
	            {
	                // no leading seperator, in which case match an optional leading seperator in string.
	                // means ** or possibly **/ used as pattern, not /**             
	                //   Input string doesn't need to start with a / or \ but if it does, it will be matched.
	                // i.e **/foo/bar will match foo/bar or /foo/bar.
	                //     where as /**/foo/bar will not match foo/bar it will only match /foo/bar.
	                // currentChar = allChars[currentPosition];
	                if (Lexer.IsPathSeparator(currentChar))
	                {
	                    // advance current position to match the leading separator.
	                    // matchedLeadingSeperator = true;
	                    position = position + 1;
	                }
	            }
	            // 1. if no more tokens require matching we match.         
	            if (ConsumesMinLength == 0)
	            {
	                next = path.Length;
	                return true;
	            }
	            // Because we know we have more tokens in the pattern (subevaluators) - those will require a minimum amount of characters to match (could be 0 too).
	            // We can therefore calculate a "max" character position that we can match to, as if we exceed that position the remaining tokens cant possibly match.
	            int maxPos = (path.Length - ConsumesMinLength);
	            // Is there enough remaining characters to provide a match, if not exit early.
	            if (position > maxPos)
	            {
	                return false;
	            }
	            // If all of the remaining tokens have a precise length, we can calculate the exact character that we need to macth to in the string.
	            // Otherwise we have to test at multiple character positions until we find a match (less efficient)
	            if (!ConsumesAnyVariableLength)
	            {
	                // Fixed length.
	                // As we can only match full segments, make sure character before chacracter at max pos is a separator, 
	                if (maxPos > 0)
	                {
	                    char mustMatchUntilChar = path[maxPos - 1];
	                    if (!Lexer.IsPathSeparator(mustMatchUntilChar))
	                    {
	                        // can only match full segments.
	                        return false;
	                    }
	                }
	                // Advance position to max pos.
	                position = maxPos;
	                return TestAll(path, cultureInfo, ignoreCase, position, out next);
	            }
	            else
	            {
	                // Remaining tokens match a variable length of the test string.
	                // We iterate each position (within acceptable range) and test at each position.
	                bool isMatch;
	                currentChar = path[position];
	                bool matchedSeperator = false;
	                // If the ** token was parsed with a trailing slash - i.e "**/" then we need to match past it before we test remainijng tokens.
	                // if input string is /foo we make sure we match the /
	                // special exception if **/ is at start of pattern,  then the input string need not have any path separators.
	                if (Trailing != null)
	                {
	                    if (Lexer.IsPathSeparator(currentChar))
	                    {
	                        // match the separator.
	                        position = position + 1;
	                    }
	                }
	                // We may already be at max pos, if so sub evaluators need to match here in the string otherwise we fail.    
	                if (position == maxPos)
	                {
	                    isMatch = TestAll(path, cultureInfo, ignoreCase, position, out next);
	                    return isMatch;
	                }
	                while (position <= maxPos)
	                {
	                    // Test at current position which is either following a seperator, or at max pos.
	                    if (position == maxPos)
	                    {
	                        // We must have encountered a seperator as we can only match full segments.
	                        if (!matchedSeperator)
	                        {
	                            return false;
	                        }
	                    }
	                    isMatch = TestAll(path, cultureInfo, ignoreCase, position, out next);
	                    if (isMatch)
	                    {
	                        return true;
	                    }
	                    if (position == maxPos) // didn't match, and can't go any further.
	                    {
	                        return false;
	                    }
	                    // Iterate until we hit the next separator or maxPos.
	                    matchedSeperator = false;
	                    while (position < maxPos)
	                    {
	                        position = position + 1;
	                        currentChar = path[position];
	                        if (Lexer.IsPathSeparator(currentChar))
	                        {
	                            // match the separator.
	                            matchedSeperator = true;
	                            position = position + 1;
	                            break;
	                        }
	                    }
	                }
	            }
	            return false;
	        }
	    }
	    internal class LiteralToken : TokenBase
	    {
	        public LiteralToken(string value)
	        {
	            Value = value;
	        }
	        public override string Value { get; }
	        public override TokenKind Kind { get; } = TokenKind.Literal;
	        public override int ConsumesMinLength => Value.Length;
	        public override bool ConsumesVariableLength => false;
	        public override bool Test(
	            ReadOnlySpan<char> path,
	            CultureInfo cultureInfo,
	            bool ignoreCase,
	            int position, out
	            int next)
	        {
	            var text = cultureInfo.TextInfo;
	            if (ignoreCase)
	            {
	                return TestIgnoreCase(path, text, position, out next);
	            }
	            else
	            {
	                return Test(path, position, out next);
	            }
	        }
	        private bool Test(ReadOnlySpan<char> path, int position, out int next)
	        {
	            var counter = 0;
	            next = position;
	            while (next < path.Length && counter < Value.Length)
	            {
	                var a = path[next];
	                var b = Value[counter];
	                if (a != b)
	                {
	                    return false;
	                }
	                next = next + 1;
	                counter = counter + 1;
	            }
	            if (counter < Value.Length)
	            {
	                return false;
	            }
	            return true;
	        }
	        private bool TestIgnoreCase(ReadOnlySpan<char> path, TextInfo text, int position, out int next)
	        {
	            var counter = 0;
	            next = position;
	            while (next < path.Length && counter < Value.Length)
	            {
	                var a = text.ToUpper(path[next]);
	                var b = text.ToUpper(Value[counter]);
	                if (a != b)
	                {
	                    return false;
	                }
	                next = next + 1;
	                counter = counter + 1;
	            }
	            if (counter < Value.Length)
	            {
	                return false;
	            }
	            return true;
	        }
	    }
	}
	// length: 30 - offset 15, limit 25
	[DebuggerDisplay("length: {_stream.Length} [Range {Offset} .. {Offset + Length}]")]
	public class OffsetStream : Stream
	{
	    private readonly Stream _stream;
	    private readonly bool _leaveOpen;
	    private readonly bool _isReadOnly;
	    private bool _isDisposed;
	    private long _length;
	    private long _offset;
	    private long _position = 0;
	    public OffsetStream(Stream stream, long offset = 0, long length = 0, bool isReadOnly = false, bool leaveOpen = false)
	    {
	        _stream = stream;
	        _leaveOpen = leaveOpen;
	        _offset = offset;
	        _length = length;
	        _isReadOnly = isReadOnly;
	    }
	    #region Properties
	    public override bool CanRead => _stream.CanRead;
	    public override bool CanSeek => _stream.CanSeek;
	    public override bool CanWrite => _stream.CanWrite && !IsReadOnly;
	    public override bool CanTimeout => _stream.CanTimeout;
	    public override long Length => _length;
	    public override long Position
	    {
	        get => _position;
	        set
	        {
	            if (!_stream.CanSeek)
	            {
	                ThrowHelper.ThrowInvalidOperationException("Seeking is not allowed.");
	            }
	            // Check if exceeding boundary of offset
	            if (value < 0 || value > _length)
	            {
	                ThrowHelper.ThrowInvalidOperationException($"The position {value} cannot exceed boundary of {_offset + _length}");
	            }
	            _stream.Position = (_offset + value);
	            _position = value;
	        }
	    }
	    public long Offset => _offset;
	    public long Remaining => _length - (_stream.Position - Offset);
	    public bool IsReadOnly { get; }
	    #endregion
	    public override void Flush()
	    {
	        AssertReadOnly();
	        _stream.Flush();
	    }
	    public override Task FlushAsync(CancellationToken cancellationToken)
	    {
	        AssertReadOnly();
	        return _stream.FlushAsync(cancellationToken);
	    }
	    public override int Read(byte[] buffer, int offset, int count)
	    {
	        CheckOrAdjustPosition();
	        if (count < 1)
	        {
	            return 0;
	        }
	        if (count > Remaining)
	        {
	            ThrowHelper.ThrowInvalidOperationException("The count exceeds the remaining readable bytes.");
	        }
	        int bytesRead = _stream.Read(buffer, offset, count);
	        _position =+ bytesRead;
	        return bytesRead;
	    }
	    public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	    {
	        CheckOrAdjustPosition();
	        if (count < 1)
	        {
	            return 0;
	        }
	        if (count > Remaining)
	        {
	            ThrowHelper.ThrowInvalidOperationException("The count exceeds the remaining readable bytes.");
	        }
	        Memory<byte> memory = buffer.AsMemory(offset, count);
	        int bytesRead = await _stream.ReadAsync(memory, cancellationToken);
	        _position =+ bytesRead;
	        return bytesRead;
	    }
	    public override long Seek(long offset, SeekOrigin origin)
	    {
	        CheckOrAdjustPosition();
	        if (!_stream.CanSeek)
	        {
	            ThrowHelper.ThrowInvalidOperationException("Stream is not seekable.");
	        }
	        var boundary = origin switch
	        {
	            SeekOrigin.Begin => _offset + offset,
	            SeekOrigin.Current => _stream.Position + offset,
	            SeekOrigin.End => (_offset + _length) + offset,
	            _ => throw new ArgumentException("Invalid Seek Origin")
	        };
	        if (boundary < _offset || boundary > (_offset + _length))
	        {
	            ThrowHelper.ThrowInvalidOperationException("The offset exceeds the boundary of the stream.");
	        }
	        _stream.Position = boundary;
	        _position =+ (boundary - _offset);
	        return boundary;
	    }
	    public void SetOffset(long value)
	    {
	        AssertReadOnly();
	        throw new NotImplementedException();
	    }
	    public override void SetLength(long value)
	    {
	        AssertReadOnly();
	        // If greater than the underlying length of the stream we 
	        // need to adjust the underlying stream length and the 
	        if (value > Length)
	        {
	            _stream.SetLength(value);
	        }
	        _length = value;
	    }
	    public override void Write(byte[] buffer, int offset, int count)
	    {
	        AssertReadOnly();
	        //
	        //		if (count < 1)
	        //			return;
	        //
	        //		if (_stream.CanSeek)
	        //			_stream.Position = _offset + _position;
	        //
	        //		_stream.Write(buffer, offset, count);
	        //		_position += count;
	        //
	        //		if (_position > _length)
	        //			_length = _position;
	    }
	    public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	    {
	        AssertReadOnly();
	        if (count < 1)
	            return;
	        //		if (_stream.CanSeek)
	        //			_stream.Position = _offset + _position;
	        //
	        //		await _stream.WriteAsync(buffer.AsMemory(offset, count), cancellationToken);
	        //		_position += count;
	        //
	        //		if (_position > _length)
	        //			_length = _position;
	    }
	    public override async ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
	    {
	        AssertReadOnly();
	        if (buffer.Length < 1)
	            return;
	        //
	        //		if (_stream.CanSeek)
	        //			_stream.Position = _offset + _position;
	        //
	        //		await _stream.WriteAsync(buffer, cancellationToken);
	        //		_position += buffer.Length;
	        //
	        //		if (_position > _length)
	        //			_length = _position;
	    }
	    protected override void Dispose(bool disposing)
	    {
	        if (_isDisposed)
	        {
	            ThrowHelper.ThrowObjectDisposedException(nameof(OffsetStream));
	        }
	        if (disposing && !_leaveOpen && _stream is not null)
	        {
	            _stream?.Dispose();
	        }
	        _isDisposed = true;
	        base.Dispose(disposing);
	    }
	    private void AssertReadOnly()
	    {
	        if (IsReadOnly)
	        {
	            ThrowHelper.ThrowInvalidOperationException("The stream is ReadOnly.");
	        }
	    }
	    private void CheckOrAdjustPosition()
	    {
	        if (_stream.Position < _offset)
	        {
	            _stream.Position = _offset;
	        }
	    }
	}
	#endregion
	#region \System\Linq
	public static class EnumerableExtensions
	{
	    public static bool ContainsAny<T>(this IEnumerable<T> enumerable, IEnumerable<T> values)
	    {
	        foreach (var value in values)
	        {
	            if (enumerable.Contains(value))
	            {
	                return true;
	            }
	        }
	        return false;
	    }
	    public static bool ContainsAny<T>(this IEnumerable<T> enumerable, IEnumerable<T> values, out T? found)
	    {
	        found = default;
	        foreach (var value in values)
	        {
	            if (enumerable.Contains(value))
	            {
	                found = value;
	                return true;
	            }
	        }
	        return false;
	    }
	}
	#endregion
	#region \System\Net
	readonly struct CidrRange
	{
	    private readonly ushort _a;
	}
	public class FileHandleEndPoint : EndPoint
	{
	    public FileHandleEndPoint(ulong fileHandle, FileHandleType fileHandleType)
	    {
	        FileHandle = fileHandle;
	        FileHandleType = fileHandleType;
	        switch (fileHandleType)
	        {
	            case FileHandleType.Auto:
	            case FileHandleType.Tcp:
	            case FileHandleType.Pipe:
	                break;
	            default:
	                throw new NotSupportedException();
	        }
	    }
	    public ulong FileHandle { get; }
	    public FileHandleType FileHandleType { get; }
	}
	public enum FileHandleType
	{
	    Auto,
	    Tcp,
	    Pipe
	}
	#endregion
	#region \System\SumTypes
	public abstract record class Either
	{
	    internal Either() { }
	    protected virtual Type? Type { get; }
	    protected virtual int TypeIndex { get; }
	    protected virtual object? Value { get; }
	}
	public record Either<T1, T2> : Either
	{
	    #region Implicit Conversion From Value
	    public static implicit operator Either<T1, T2>(T1 value) => new Either<T1, T2>(value);
	    public static implicit operator Either<T1, T2>(T2 value) => new Either<T1, T2>(value);
	    #endregion Implicit Conversion From Value
	    #region Implicit Conversion By Type Swap
	    public static implicit operator Either<T1, T2>(Either<T2, T1> other)
	    {
	        int[] map = new[] { 2, 1 };
	        return new Either<T1, T2>(map[other._typeIndex - 1], other._value!);
	    }
	    #endregion Implicit Conversion By Type Swap
	    #region Implicit Widening Conversions
	    #endregion Implicit Widening Conversions
	    #region Constructors
	    public Either(T1 value) { _value = value; _typeIndex = 1; }
	    public Either(T2 value) { _value = value; _typeIndex = 2; }
	    #endregion Constructors
	    #region Or methods
	    public Either<T1, T2, T3> Or<T3>() => this
	        .Match((T1 v1) => new Either<T1, T2, T3>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3>(v2));
	    public Either<T1, T2, T3, T4> Or<T3, T4>() => this
	        .Match((T1 v1) => new Either<T1, T2, T3, T4>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4>(v2));
	    public Either<T1, T2, T3, T4, T5> Or<T3, T4, T5>() => this
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2));
	    #endregion Or methods
	    #region IEither Implementation
	    int _typeIndex;
	    object? _value;
	    protected override int TypeIndex => _typeIndex;
	    protected override Type Type => _typeIndex switch
	    {
	        1 => typeof(T1),
	        2 => typeof(T2),
	        _ => throw new InvalidOperationException()
	    };
	    protected override object? Value => _value;
	    Either(int typeIndex, object value) => (_typeIndex, _value) = (typeIndex, value);
	    #endregion IEither Implementation
	    #region Value Casts
	    T1 AsT1 => (T1)_value!;
	    T2 AsT2 => (T2)_value!;
	    #endregion Value Casts
	    #region Explicit Casts
	    public static explicit operator T1(Either<T1, T2> either) => either.AsT1;
	    public static explicit operator T2(Either<T1, T2> either) => either.AsT2;
	    #endregion Explicit Casts
	    #region Switch method
	    public void Switch(Action<T1> ifT1, Action<T2> ifT2)
	    {
	        switch (_typeIndex)
	        {
	            case 1: ifT1(AsT1); break;
	            case 2: ifT2(AsT2); break;
	            default: throw new InvalidOperationException();
	        }
	    }
	    #endregion Switch method
	    #region Nonreductive Match
	    public Either<TResult1, T2> Match<TResult1>(Func<T1, TResult1> ifT1) => _typeIndex switch
	    {
	        1 => ifT1(AsT1),
	        2 => AsT2,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, TResult1> Match<TResult1>
	        (Func<T1, TResult1> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, TResult2> Match<TResult2>(Func<T2, TResult2> ifT2) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => ifT2(AsT2),
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, TResult2> Match<TResult2>
	        (Func<T2, TResult2> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Nonreductive Match
	    #region Nonreductive Match - Compositional
	    public Either<TResult1, T2> Match<TResult1>(Func<T1, Either<TResult1, T2>> ifT1) => _typeIndex switch
	    {
	        1 => ifT1(AsT1),
	        2 => AsT2,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, TResult1> Match<TResult1>
	        (Func<T1, Either<T1, T2, TResult1>> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, TResult2> Match<TResult2>(Func<T2, Either<T1, TResult2>> ifT2) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => ifT2(AsT2),
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, TResult2> Match<TResult2>
	        (Func<T2, Either<T1, T2, TResult2>> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Nonreductive Match - Compositional
	    #region Reductive Match
	    public T2 Match
	        (Func<T1, T2> ifT1) => _typeIndex switch
	        {
	            1 => ifT1(AsT1),
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2> Match
	        (Func<T1, T2> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public T1 Match
	        (Func<T2, T1> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => ifT2(AsT2),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2> Match
	        (Func<T2, T1> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Reductive Match
	    #region Throw Methods
	    public T2 ThrowIf
	        (Func<T1, Exception> ifT1) => _typeIndex switch
	        {
	            1 => throw ifT1(AsT1),
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2> ThrowIf
	        (Func<T1, Exception> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => throw ifT1(AsT1),
	            2 => AsT2,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public T1 ThrowIf
	        (Func<T2, Exception> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => throw ifT2(AsT2),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2> ThrowIf
	        (Func<T2, Exception> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => throw ifT2(AsT2),
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Throw Methods
	    #region If (methods)
	    public bool If(out T1 @if) => If(out @if, out _);
	    public bool If(out T1 @if, out T2 @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = AsT1;
	                @else = default!;
	                return true;
	            case 2:
	                @if = default!;
	                @else = AsT2;
	                return false;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    public bool If(out T2 @if) => If(out @if, out _);
	    public bool If(out T2 @if, out T1 @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = default!;
	                @else = AsT1;
	                return false;
	            case 2:
	                @if = AsT2;
	                @else = default!;
	                return true;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    #endregion If (methods)
	    #region ToString
	    public override string ToString() => $"{Type.Name}:{_value}";
	    #endregion ToString
	}
	public record Either<T1, T2, T3> : Either
	{
	    #region Implicit Conversion From Value
	    public static implicit operator Either<T1, T2, T3>(T1 value) => new Either<T1, T2, T3>(value);
	    public static implicit operator Either<T1, T2, T3>(T2 value) => new Either<T1, T2, T3>(value);
	    public static implicit operator Either<T1, T2, T3>(T3 value) => new Either<T1, T2, T3>(value);
	    #endregion Implicit Conversion From Value
	    #region Implicit Conversion By Type Swap
	    public static implicit operator Either<T1, T2, T3>(Either<T1, T3, T2> other)
	    {
	        int[] map = new[] { 1, 3, 2 };
	        return new Either<T1, T2, T3>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3>(Either<T2, T1, T3> other)
	    {
	        int[] map = new[] { 2, 1, 3 };
	        return new Either<T1, T2, T3>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3>(Either<T2, T3, T1> other)
	    {
	        int[] map = new[] { 2, 3, 1 };
	        return new Either<T1, T2, T3>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3>(Either<T3, T1, T2> other)
	    {
	        int[] map = new[] { 3, 1, 2 };
	        return new Either<T1, T2, T3>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3>(Either<T3, T2, T1> other)
	    {
	        int[] map = new[] { 3, 2, 1 };
	        return new Either<T1, T2, T3>(map[other._typeIndex - 1], other._value);
	    }
	    #endregion Implicit Conversion By Type Swap
	    #region Implicit Widening Conversions
	    public static implicit operator Either<T1, T2, T3>(Either<T1, T2> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3>(v2));
	    public static implicit operator Either<T1, T2, T3>(Either<T1, T3> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3>(v1))
	        .Match((T3 v3) => new Either<T1, T2, T3>(v3));
	    public static implicit operator Either<T1, T2, T3>(Either<T2, T3> other) => other
	        .Match((T2 v2) => new Either<T1, T2, T3>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3>(v3));
	    #endregion Implicit Widening Conversions
	    #region Constructors
	    public Either(T1 value) { _value = value!; _typeIndex = 1; }
	    public Either(T2 value) { _value = value!; _typeIndex = 2; }
	    public Either(T3 value) { _value = value!; _typeIndex = 3; }
	    #endregion Constructors
	    #region Or methods
	    public Either<T1, T2, T3, T4> Or<T4>() => this
	        .Match((T1 v1) => new Either<T1, T2, T3, T4>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4>(v3));
	    public Either<T1, T2, T3, T4, T5> Or<T4, T5>() => this
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3));
	    #endregion Or methods
	    #region IEither Implementation
	    int _typeIndex;
	    object _value;
	    protected override int TypeIndex => _typeIndex;
	    protected override Type Type => _typeIndex switch
	    {
	        1 => typeof(T1),
	        2 => typeof(T2),
	        3 => typeof(T3),
	        _ => throw new InvalidOperationException()
	    };
	    protected override object Value => _value;
	    Either(int typeIndex, object value) => (_typeIndex, _value) = (typeIndex, value);
	    #endregion IEither Implementation
	    #region Value Casts
	    T1 AsT1 => (T1)_value;
	    T2 AsT2 => (T2)_value;
	    T3 AsT3 => (T3)_value;
	    #endregion Value Casts
	    #region Explicit Casts
	    public static explicit operator T1(Either<T1, T2, T3> either) => either.AsT1;
	    public static explicit operator T2(Either<T1, T2, T3> either) => either.AsT2;
	    public static explicit operator T3(Either<T1, T2, T3> either) => either.AsT3;
	    #endregion Explicit Casts
	    #region Switch method
	    public void Switch(Action<T1> ifT1, Action<T2> ifT2, Action<T3> ifT3)
	    {
	        switch (_typeIndex)
	        {
	            case 1: ifT1(AsT1); break;
	            case 2: ifT2(AsT2); break;
	            case 3: ifT3(AsT3); break;
	            default: throw new InvalidOperationException();
	        }
	    }
	    #endregion Switch method
	    #region Nonreductive Match
	    public Either<TResult1, T2, T3> Match<TResult1>(Func<T1, TResult1> ifT1) => _typeIndex switch
	    {
	        1 => ifT1(AsT1),
	        2 => AsT2,
	        3 => AsT3,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, TResult1> Match<TResult1>
	        (Func<T1, TResult1> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, TResult2, T3> Match<TResult2>(Func<T2, TResult2> ifT2) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => ifT2(AsT2),
	        3 => AsT3,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, TResult2> Match<TResult2>
	        (Func<T2, TResult2> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, TResult3> Match<TResult3>(Func<T3, TResult3> ifT3) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => ifT3(AsT3),
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, TResult3> Match<TResult3>
	        (Func<T3, TResult3> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Nonreductive Match
	    #region Nonreductive Match - Compositional
	    public Either<TResult1, T2, T3> Match<TResult1>(Func<T1, Either<TResult1, T2, T3>> ifT1) => _typeIndex switch
	    {
	        1 => ifT1(AsT1),
	        2 => AsT2,
	        3 => AsT3,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, TResult1> Match<TResult1>
	        (Func<T1, Either<T1, T2, T3, TResult1>> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, TResult2, T3> Match<TResult2>(Func<T2, Either<T1, TResult2, T3>> ifT2) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => ifT2(AsT2),
	        3 => AsT3,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, TResult2> Match<TResult2>
	        (Func<T2, Either<T1, T2, T3, TResult2>> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, TResult3> Match<TResult3>(Func<T3, Either<T1, T2, TResult3>> ifT3) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => ifT3(AsT3),
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, TResult3> Match<TResult3>
	        (Func<T3, Either<T1, T2, T3, TResult3>> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Nonreductive Match - Compositional
	    #region Reductive Match
	    public Either<T2, T3> Match
	        (Func<T1, T2> ifT1) => _typeIndex switch
	        {
	            1 => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> Match
	        (Func<T1, T2> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3> Match
	        (Func<T2, T1> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => ifT2(AsT2),
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> Match
	        (Func<T2, T1> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T2, T3> Match
	        (Func<T1, T3> ifT1) => _typeIndex switch
	        {
	            1 => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> Match
	        (Func<T1, T3> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2> Match
	        (Func<T3, T1> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => ifT3(AsT3),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> Match
	        (Func<T3, T1> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3> Match
	        (Func<T2, T3> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => ifT2(AsT2),
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> Match
	        (Func<T2, T3> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2> Match
	        (Func<T3, T2> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => ifT3(AsT3),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> Match
	        (Func<T3, T2> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Reductive Match
	    #region Throw Methods
	    public Either<T2, T3> ThrowIf
	        (Func<T1, Exception> ifT1) => _typeIndex switch
	        {
	            1 => throw ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> ThrowIf
	        (Func<T1, Exception> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => throw ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3> ThrowIf
	        (Func<T2, Exception> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => throw ifT2(AsT2),
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> ThrowIf
	        (Func<T2, Exception> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => throw ifT2(AsT2),
	            3 => AsT3,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2> ThrowIf
	        (Func<T3, Exception> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => throw ifT3(AsT3),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> ThrowIf
	        (Func<T3, Exception> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => throw ifT3(AsT3),
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Throw Methods
	    #region If (methods)
	    public bool If(out T1 @if) => If(out @if, out _);
	    public bool If(out T1 @if, out Either<T2, T3> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = AsT1;
	                @else = default!;
	                return true;
	            case 2:
	                @if = default!;
	                @else = AsT2;
	                return false;
	            case 3:
	                @if = default!;
	                @else = AsT3;
	                return false;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    public bool If(out T2 @if) => If(out @if, out _);
	    public bool If(out T2 @if, out Either<T1, T3> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = default!;
	                @else = AsT1;
	                return false;
	            case 2:
	                @if = AsT2;
	                @else = default!;
	                return true;
	            case 3:
	                @if = default!;
	                @else = AsT3;
	                return false;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    public bool If(out T3 @if) => If(out @if, out _);
	    public bool If(out T3 @if, out Either<T1, T2> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = default!;
	                @else = AsT1;
	                return false;
	            case 2:
	                @if = default!;
	                @else = AsT2;
	                return false;
	            case 3:
	                @if = AsT3;
	                @else = default!;
	                return true;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    #endregion If (methods)
	    #region ToString
	    public override string ToString() => $"{Type.Name}:{_value}";
	    #endregion ToString
	}
	public record Either<T1, T2, T3, T4> : Either
	{
	    #region Implicit Conversion From Value
	    public static implicit operator Either<T1, T2, T3, T4>(T1 value) => new Either<T1, T2, T3, T4>(value);
	    public static implicit operator Either<T1, T2, T3, T4>(T2 value) => new Either<T1, T2, T3, T4>(value);
	    public static implicit operator Either<T1, T2, T3, T4>(T3 value) => new Either<T1, T2, T3, T4>(value);
	    public static implicit operator Either<T1, T2, T3, T4>(T4 value) => new Either<T1, T2, T3, T4>(value);
	    #endregion Implicit Conversion From Value
	    #region Implicit Conversion By Type Swap
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T1, T2, T4, T3> other)
	    {
	        int[] map = new[] { 1, 2, 4, 3 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T1, T3, T2, T4> other)
	    {
	        int[] map = new[] { 1, 3, 2, 4 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T1, T3, T4, T2> other)
	    {
	        int[] map = new[] { 1, 3, 4, 2 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T1, T4, T2, T3> other)
	    {
	        int[] map = new[] { 1, 4, 2, 3 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T1, T4, T3, T2> other)
	    {
	        int[] map = new[] { 1, 4, 3, 2 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T2, T1, T3, T4> other)
	    {
	        int[] map = new[] { 2, 1, 3, 4 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T2, T1, T4, T3> other)
	    {
	        int[] map = new[] { 2, 1, 4, 3 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T2, T3, T1, T4> other)
	    {
	        int[] map = new[] { 2, 3, 1, 4 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T2, T3, T4, T1> other)
	    {
	        int[] map = new[] { 2, 3, 4, 1 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T2, T4, T1, T3> other)
	    {
	        int[] map = new[] { 2, 4, 1, 3 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T2, T4, T3, T1> other)
	    {
	        int[] map = new[] { 2, 4, 3, 1 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T3, T1, T2, T4> other)
	    {
	        int[] map = new[] { 3, 1, 2, 4 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T3, T1, T4, T2> other)
	    {
	        int[] map = new[] { 3, 1, 4, 2 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T3, T2, T1, T4> other)
	    {
	        int[] map = new[] { 3, 2, 1, 4 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T3, T2, T4, T1> other)
	    {
	        int[] map = new[] { 3, 2, 4, 1 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T3, T4, T1, T2> other)
	    {
	        int[] map = new[] { 3, 4, 1, 2 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T3, T4, T2, T1> other)
	    {
	        int[] map = new[] { 3, 4, 2, 1 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T4, T1, T2, T3> other)
	    {
	        int[] map = new[] { 4, 1, 2, 3 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T4, T1, T3, T2> other)
	    {
	        int[] map = new[] { 4, 1, 3, 2 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T4, T2, T1, T3> other)
	    {
	        int[] map = new[] { 4, 2, 1, 3 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T4, T2, T3, T1> other)
	    {
	        int[] map = new[] { 4, 2, 3, 1 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T4, T3, T1, T2> other)
	    {
	        int[] map = new[] { 4, 3, 1, 2 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T4, T3, T2, T1> other)
	    {
	        int[] map = new[] { 4, 3, 2, 1 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    #endregion Implicit Conversion By Type Swap
	    #region Implicit Widening Conversions
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T1, T2> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4>(v2));
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T1, T3> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4>(v1))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4>(v3));
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T1, T4> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4>(v1))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4>(v4));
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T2, T3> other) => other
	        .Match((T2 v2) => new Either<T1, T2, T3, T4>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4>(v3));
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T2, T4> other) => other
	        .Match((T2 v2) => new Either<T1, T2, T3, T4>(v2))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4>(v4));
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T3, T4> other) => other
	        .Match((T3 v3) => new Either<T1, T2, T3, T4>(v3))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4>(v4));
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T1, T2, T3> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4>(v3));
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T1, T2, T4> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4>(v2))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4>(v4));
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T1, T3, T4> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4>(v1))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4>(v3))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4>(v4));
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T2, T3, T4> other) => other
	        .Match((T2 v2) => new Either<T1, T2, T3, T4>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4>(v3))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4>(v4));
	    #endregion Implicit Widening Conversions
	    #region Constructors
	    public Either(T1 value) { _value = value!; _typeIndex = 1; }
	    public Either(T2 value) { _value = value!; _typeIndex = 2; }
	    public Either(T3 value) { _value = value!; _typeIndex = 3; }
	    public Either(T4 value) { _value = value!; _typeIndex = 4; }
	    #endregion Constructors
	    #region Or methods
	    public Either<T1, T2, T3, T4, T5> Or<T5>() => this
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4));
	    #endregion Or methods
	    #region IEither Implementation
	    int _typeIndex;
	    object _value;
	    protected override int TypeIndex => _typeIndex;
	    protected override Type Type => _typeIndex switch
	    {
	        1 => typeof(T1),
	        2 => typeof(T2),
	        3 => typeof(T3),
	        4 => typeof(T4),
	        _ => throw new InvalidOperationException()
	    };
	    protected override object Value => _value;
	    Either(int typeIndex, object value) => (_typeIndex, _value) = (typeIndex, value);
	    #endregion IEither Implementation
	    #region Value Casts
	    T1 AsT1 => (T1)_value;
	    T2 AsT2 => (T2)_value;
	    T3 AsT3 => (T3)_value;
	    T4 AsT4 => (T4)_value;
	    #endregion Value Casts
	    #region Explicit Casts
	    public static explicit operator T1(Either<T1, T2, T3, T4> either) => either.AsT1;
	    public static explicit operator T2(Either<T1, T2, T3, T4> either) => either.AsT2;
	    public static explicit operator T3(Either<T1, T2, T3, T4> either) => either.AsT3;
	    public static explicit operator T4(Either<T1, T2, T3, T4> either) => either.AsT4;
	    #endregion Explicit Casts
	    #region Switch method
	    public void Switch(Action<T1> ifT1, Action<T2> ifT2, Action<T3> ifT3, Action<T4> ifT4)
	    {
	        switch (_typeIndex)
	        {
	            case 1: ifT1(AsT1); break;
	            case 2: ifT2(AsT2); break;
	            case 3: ifT3(AsT3); break;
	            case 4: ifT4(AsT4); break;
	            default: throw new InvalidOperationException();
	        }
	    }
	    #endregion Switch method
	    #region Nonreductive Match
	    public Either<TResult1, T2, T3, T4> Match<TResult1>(Func<T1, TResult1> ifT1) => _typeIndex switch
	    {
	        1 => ifT1(AsT1),
	        2 => AsT2,
	        3 => AsT3,
	        4 => AsT4,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, T4, TResult1> Match<TResult1>
	        (Func<T1, TResult1> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, TResult2, T3, T4> Match<TResult2>(Func<T2, TResult2> ifT2) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => ifT2(AsT2),
	        3 => AsT3,
	        4 => AsT4,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, T4, TResult2> Match<TResult2>
	        (Func<T2, TResult2> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, TResult3, T4> Match<TResult3>(Func<T3, TResult3> ifT3) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => ifT3(AsT3),
	        4 => AsT4,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, T4, TResult3> Match<TResult3>
	        (Func<T3, TResult3> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            4 => AsT4,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, TResult4> Match<TResult4>(Func<T4, TResult4> ifT4) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => AsT3,
	        4 => ifT4(AsT4),
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, T4, TResult4> Match<TResult4>
	        (Func<T4, TResult4> ifT4, Func<T4, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 when (when(AsT4)) => ifT4(AsT4),
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Nonreductive Match
	    #region Nonreductive Match - Compositional
	    public Either<TResult1, T2, T3, T4> Match<TResult1>(Func<T1, Either<TResult1, T2, T3, T4>> ifT1) => _typeIndex switch
	    {
	        1 => ifT1(AsT1),
	        2 => AsT2,
	        3 => AsT3,
	        4 => AsT4,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, T4, TResult1> Match<TResult1>
	        (Func<T1, Either<T1, T2, T3, T4, TResult1>> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, TResult2, T3, T4> Match<TResult2>(Func<T2, Either<T1, TResult2, T3, T4>> ifT2) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => ifT2(AsT2),
	        3 => AsT3,
	        4 => AsT4,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, T4, TResult2> Match<TResult2>
	        (Func<T2, Either<T1, T2, T3, T4, TResult2>> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, TResult3, T4> Match<TResult3>(Func<T3, Either<T1, T2, TResult3, T4>> ifT3) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => ifT3(AsT3),
	        4 => AsT4,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, T4, TResult3> Match<TResult3>
	        (Func<T3, Either<T1, T2, T3, T4, TResult3>> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            4 => AsT4,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, TResult4> Match<TResult4>(Func<T4, Either<T1, T2, T3, TResult4>> ifT4) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => AsT3,
	        4 => ifT4(AsT4),
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, T4, TResult4> Match<TResult4>
	        (Func<T4, Either<T1, T2, T3, T4, TResult4>> ifT4, Func<T4, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 when (when(AsT4)) => ifT4(AsT4),
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Nonreductive Match - Compositional
	    #region Reductive Match
	    public Either<T2, T3, T4> Match
	        (Func<T1, T2> ifT1) => _typeIndex switch
	        {
	            1 => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T1, T2> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3, T4> Match
	        (Func<T2, T1> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T2, T1> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T2, T3, T4> Match
	        (Func<T1, T3> ifT1) => _typeIndex switch
	        {
	            1 => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T1, T3> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T4> Match
	        (Func<T3, T1> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => ifT3(AsT3),
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T3, T1> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            4 => AsT4,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T2, T3, T4> Match
	        (Func<T1, T4> ifT1) => _typeIndex switch
	        {
	            1 => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T1, T4> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> Match
	        (Func<T4, T1> ifT4) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => ifT4(AsT4),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T4, T1> ifT4, Func<T4, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 when (when(AsT4)) => ifT4(AsT4),
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3, T4> Match
	        (Func<T2, T3> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T2, T3> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T4> Match
	        (Func<T3, T2> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => ifT3(AsT3),
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T3, T2> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            4 => AsT4,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3, T4> Match
	        (Func<T2, T4> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T2, T4> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> Match
	        (Func<T4, T2> ifT4) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => ifT4(AsT4),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T4, T2> ifT4, Func<T4, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 when (when(AsT4)) => ifT4(AsT4),
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T4> Match
	        (Func<T3, T4> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => ifT3(AsT3),
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T3, T4> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            4 => AsT4,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> Match
	        (Func<T4, T3> ifT4) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => ifT4(AsT4),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T4, T3> ifT4, Func<T4, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 when (when(AsT4)) => ifT4(AsT4),
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Reductive Match
	    #region Throw Methods
	    public Either<T2, T3, T4> ThrowIf
	        (Func<T1, Exception> ifT1) => _typeIndex switch
	        {
	            1 => throw ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> ThrowIf
	        (Func<T1, Exception> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => throw ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3, T4> ThrowIf
	        (Func<T2, Exception> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => throw ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> ThrowIf
	        (Func<T2, Exception> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => throw ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T4> ThrowIf
	        (Func<T3, Exception> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => throw ifT3(AsT3),
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> ThrowIf
	        (Func<T3, Exception> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => throw ifT3(AsT3),
	            4 => AsT4,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> ThrowIf
	        (Func<T4, Exception> ifT4) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => throw ifT4(AsT4),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> ThrowIf
	        (Func<T4, Exception> ifT4, Func<T4, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 when (when(AsT4)) => throw ifT4(AsT4),
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Throw Methods
	    #region If (methods)
	    public bool If(out T1 @if) => If(out @if, out _);
	    public bool If(out T1 @if, out Either<T2, T3, T4> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = AsT1;
	                @else = default!;
	                return true;
	            case 2:
	                @if = default!;
	                @else = AsT2;
	                return false;
	            case 3:
	                @if = default!;
	                @else = AsT3;
	                return false;
	            case 4:
	                @if = default!;
	                @else = AsT4;
	                return false;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    public bool If(out T2 @if) => If(out @if, out _);
	    public bool If(out T2 @if, out Either<T1, T3, T4> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = default!;
	                @else = AsT1;
	                return false;
	            case 2:
	                @if = AsT2;
	                @else = default!;
	                return true;
	            case 3:
	                @if = default!;
	                @else = AsT3;
	                return false;
	            case 4:
	                @if = default!;
	                @else = AsT4;
	                return false;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    public bool If(out T3 @if) => If(out @if, out _);
	    public bool If(out T3 @if, out Either<T1, T2, T4> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = default!;
	                @else = AsT1;
	                return false;
	            case 2:
	                @if = default!;
	                @else = AsT2;
	                return false;
	            case 3:
	                @if = AsT3;
	                @else = default!;
	                return true;
	            case 4:
	                @if = default!;
	                @else = AsT4;
	                return false;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    public bool If(out T4 @if) => If(out @if, out _);
	    public bool If(out T4 @if, out Either<T1, T2, T3> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = default!;
	                @else = AsT1;
	                return false;
	            case 2:
	                @if = default!;
	                @else = AsT2;
	                return false;
	            case 3:
	                @if = default!;
	                @else = AsT3;
	                return false;
	            case 4:
	                @if = AsT4;
	                @else = default!;
	                return true;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    #endregion If (methods)
	    #region ToString
	    public override string ToString() => $"{Type.Name}:{_value}";
	    #endregion ToString
	}
	public record Either<T1, T2, T3, T4, T5> : Either
	{
	    #region Implicit Conversion From Value
	    public static implicit operator Either<T1, T2, T3, T4, T5>(T1 value) => new Either<T1, T2, T3, T4, T5>(value);
	    public static implicit operator Either<T1, T2, T3, T4, T5>(T2 value) => new Either<T1, T2, T3, T4, T5>(value);
	    public static implicit operator Either<T1, T2, T3, T4, T5>(T3 value) => new Either<T1, T2, T3, T4, T5>(value);
	    public static implicit operator Either<T1, T2, T3, T4, T5>(T4 value) => new Either<T1, T2, T3, T4, T5>(value);
	    public static implicit operator Either<T1, T2, T3, T4, T5>(T5 value) => new Either<T1, T2, T3, T4, T5>(value);
	    #endregion Implicit Conversion From Value
	    #region Implicit Conversion By Type Swap
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2, T3, T5, T4> other)
	    {
	        int[] map = new[] { 1, 2, 3, 5, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2, T4, T3, T5> other)
	    {
	        int[] map = new[] { 1, 2, 4, 3, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2, T4, T5, T3> other)
	    {
	        int[] map = new[] { 1, 2, 4, 5, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2, T5, T3, T4> other)
	    {
	        int[] map = new[] { 1, 2, 5, 3, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2, T5, T4, T3> other)
	    {
	        int[] map = new[] { 1, 2, 5, 4, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T3, T2, T4, T5> other)
	    {
	        int[] map = new[] { 1, 3, 2, 4, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T3, T2, T5, T4> other)
	    {
	        int[] map = new[] { 1, 3, 2, 5, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T3, T4, T2, T5> other)
	    {
	        int[] map = new[] { 1, 3, 4, 2, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T3, T4, T5, T2> other)
	    {
	        int[] map = new[] { 1, 3, 4, 5, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T3, T5, T2, T4> other)
	    {
	        int[] map = new[] { 1, 3, 5, 2, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T3, T5, T4, T2> other)
	    {
	        int[] map = new[] { 1, 3, 5, 4, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T4, T2, T3, T5> other)
	    {
	        int[] map = new[] { 1, 4, 2, 3, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T4, T2, T5, T3> other)
	    {
	        int[] map = new[] { 1, 4, 2, 5, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T4, T3, T2, T5> other)
	    {
	        int[] map = new[] { 1, 4, 3, 2, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T4, T3, T5, T2> other)
	    {
	        int[] map = new[] { 1, 4, 3, 5, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T4, T5, T2, T3> other)
	    {
	        int[] map = new[] { 1, 4, 5, 2, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T4, T5, T3, T2> other)
	    {
	        int[] map = new[] { 1, 4, 5, 3, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T5, T2, T3, T4> other)
	    {
	        int[] map = new[] { 1, 5, 2, 3, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T5, T2, T4, T3> other)
	    {
	        int[] map = new[] { 1, 5, 2, 4, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T5, T3, T2, T4> other)
	    {
	        int[] map = new[] { 1, 5, 3, 2, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T5, T3, T4, T2> other)
	    {
	        int[] map = new[] { 1, 5, 3, 4, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T5, T4, T2, T3> other)
	    {
	        int[] map = new[] { 1, 5, 4, 2, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T5, T4, T3, T2> other)
	    {
	        int[] map = new[] { 1, 5, 4, 3, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T1, T3, T4, T5> other)
	    {
	        int[] map = new[] { 2, 1, 3, 4, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T1, T3, T5, T4> other)
	    {
	        int[] map = new[] { 2, 1, 3, 5, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T1, T4, T3, T5> other)
	    {
	        int[] map = new[] { 2, 1, 4, 3, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T1, T4, T5, T3> other)
	    {
	        int[] map = new[] { 2, 1, 4, 5, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T1, T5, T3, T4> other)
	    {
	        int[] map = new[] { 2, 1, 5, 3, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T1, T5, T4, T3> other)
	    {
	        int[] map = new[] { 2, 1, 5, 4, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T3, T1, T4, T5> other)
	    {
	        int[] map = new[] { 2, 3, 1, 4, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T3, T1, T5, T4> other)
	    {
	        int[] map = new[] { 2, 3, 1, 5, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T3, T4, T1, T5> other)
	    {
	        int[] map = new[] { 2, 3, 4, 1, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T3, T4, T5, T1> other)
	    {
	        int[] map = new[] { 2, 3, 4, 5, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T3, T5, T1, T4> other)
	    {
	        int[] map = new[] { 2, 3, 5, 1, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T3, T5, T4, T1> other)
	    {
	        int[] map = new[] { 2, 3, 5, 4, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T4, T1, T3, T5> other)
	    {
	        int[] map = new[] { 2, 4, 1, 3, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T4, T1, T5, T3> other)
	    {
	        int[] map = new[] { 2, 4, 1, 5, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T4, T3, T1, T5> other)
	    {
	        int[] map = new[] { 2, 4, 3, 1, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T4, T3, T5, T1> other)
	    {
	        int[] map = new[] { 2, 4, 3, 5, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T4, T5, T1, T3> other)
	    {
	        int[] map = new[] { 2, 4, 5, 1, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T4, T5, T3, T1> other)
	    {
	        int[] map = new[] { 2, 4, 5, 3, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T5, T1, T3, T4> other)
	    {
	        int[] map = new[] { 2, 5, 1, 3, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T5, T1, T4, T3> other)
	    {
	        int[] map = new[] { 2, 5, 1, 4, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T5, T3, T1, T4> other)
	    {
	        int[] map = new[] { 2, 5, 3, 1, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T5, T3, T4, T1> other)
	    {
	        int[] map = new[] { 2, 5, 3, 4, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T5, T4, T1, T3> other)
	    {
	        int[] map = new[] { 2, 5, 4, 1, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T5, T4, T3, T1> other)
	    {
	        int[] map = new[] { 2, 5, 4, 3, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T1, T2, T4, T5> other)
	    {
	        int[] map = new[] { 3, 1, 2, 4, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T1, T2, T5, T4> other)
	    {
	        int[] map = new[] { 3, 1, 2, 5, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T1, T4, T2, T5> other)
	    {
	        int[] map = new[] { 3, 1, 4, 2, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T1, T4, T5, T2> other)
	    {
	        int[] map = new[] { 3, 1, 4, 5, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T1, T5, T2, T4> other)
	    {
	        int[] map = new[] { 3, 1, 5, 2, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T1, T5, T4, T2> other)
	    {
	        int[] map = new[] { 3, 1, 5, 4, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T2, T1, T4, T5> other)
	    {
	        int[] map = new[] { 3, 2, 1, 4, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T2, T1, T5, T4> other)
	    {
	        int[] map = new[] { 3, 2, 1, 5, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T2, T4, T1, T5> other)
	    {
	        int[] map = new[] { 3, 2, 4, 1, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T2, T4, T5, T1> other)
	    {
	        int[] map = new[] { 3, 2, 4, 5, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T2, T5, T1, T4> other)
	    {
	        int[] map = new[] { 3, 2, 5, 1, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T2, T5, T4, T1> other)
	    {
	        int[] map = new[] { 3, 2, 5, 4, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T4, T1, T2, T5> other)
	    {
	        int[] map = new[] { 3, 4, 1, 2, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T4, T1, T5, T2> other)
	    {
	        int[] map = new[] { 3, 4, 1, 5, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T4, T2, T1, T5> other)
	    {
	        int[] map = new[] { 3, 4, 2, 1, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T4, T2, T5, T1> other)
	    {
	        int[] map = new[] { 3, 4, 2, 5, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T4, T5, T1, T2> other)
	    {
	        int[] map = new[] { 3, 4, 5, 1, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T4, T5, T2, T1> other)
	    {
	        int[] map = new[] { 3, 4, 5, 2, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T5, T1, T2, T4> other)
	    {
	        int[] map = new[] { 3, 5, 1, 2, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T5, T1, T4, T2> other)
	    {
	        int[] map = new[] { 3, 5, 1, 4, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T5, T2, T1, T4> other)
	    {
	        int[] map = new[] { 3, 5, 2, 1, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T5, T2, T4, T1> other)
	    {
	        int[] map = new[] { 3, 5, 2, 4, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T5, T4, T1, T2> other)
	    {
	        int[] map = new[] { 3, 5, 4, 1, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T5, T4, T2, T1> other)
	    {
	        int[] map = new[] { 3, 5, 4, 2, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T1, T2, T3, T5> other)
	    {
	        int[] map = new[] { 4, 1, 2, 3, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T1, T2, T5, T3> other)
	    {
	        int[] map = new[] { 4, 1, 2, 5, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T1, T3, T2, T5> other)
	    {
	        int[] map = new[] { 4, 1, 3, 2, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T1, T3, T5, T2> other)
	    {
	        int[] map = new[] { 4, 1, 3, 5, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T1, T5, T2, T3> other)
	    {
	        int[] map = new[] { 4, 1, 5, 2, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T1, T5, T3, T2> other)
	    {
	        int[] map = new[] { 4, 1, 5, 3, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T2, T1, T3, T5> other)
	    {
	        int[] map = new[] { 4, 2, 1, 3, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T2, T1, T5, T3> other)
	    {
	        int[] map = new[] { 4, 2, 1, 5, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T2, T3, T1, T5> other)
	    {
	        int[] map = new[] { 4, 2, 3, 1, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T2, T3, T5, T1> other)
	    {
	        int[] map = new[] { 4, 2, 3, 5, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T2, T5, T1, T3> other)
	    {
	        int[] map = new[] { 4, 2, 5, 1, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T2, T5, T3, T1> other)
	    {
	        int[] map = new[] { 4, 2, 5, 3, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T3, T1, T2, T5> other)
	    {
	        int[] map = new[] { 4, 3, 1, 2, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T3, T1, T5, T2> other)
	    {
	        int[] map = new[] { 4, 3, 1, 5, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T3, T2, T1, T5> other)
	    {
	        int[] map = new[] { 4, 3, 2, 1, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T3, T2, T5, T1> other)
	    {
	        int[] map = new[] { 4, 3, 2, 5, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T3, T5, T1, T2> other)
	    {
	        int[] map = new[] { 4, 3, 5, 1, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T3, T5, T2, T1> other)
	    {
	        int[] map = new[] { 4, 3, 5, 2, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T5, T1, T2, T3> other)
	    {
	        int[] map = new[] { 4, 5, 1, 2, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T5, T1, T3, T2> other)
	    {
	        int[] map = new[] { 4, 5, 1, 3, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T5, T2, T1, T3> other)
	    {
	        int[] map = new[] { 4, 5, 2, 1, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T5, T2, T3, T1> other)
	    {
	        int[] map = new[] { 4, 5, 2, 3, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T5, T3, T1, T2> other)
	    {
	        int[] map = new[] { 4, 5, 3, 1, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T5, T3, T2, T1> other)
	    {
	        int[] map = new[] { 4, 5, 3, 2, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T1, T2, T3, T4> other)
	    {
	        int[] map = new[] { 5, 1, 2, 3, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T1, T2, T4, T3> other)
	    {
	        int[] map = new[] { 5, 1, 2, 4, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T1, T3, T2, T4> other)
	    {
	        int[] map = new[] { 5, 1, 3, 2, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T1, T3, T4, T2> other)
	    {
	        int[] map = new[] { 5, 1, 3, 4, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T1, T4, T2, T3> other)
	    {
	        int[] map = new[] { 5, 1, 4, 2, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T1, T4, T3, T2> other)
	    {
	        int[] map = new[] { 5, 1, 4, 3, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T2, T1, T3, T4> other)
	    {
	        int[] map = new[] { 5, 2, 1, 3, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T2, T1, T4, T3> other)
	    {
	        int[] map = new[] { 5, 2, 1, 4, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T2, T3, T1, T4> other)
	    {
	        int[] map = new[] { 5, 2, 3, 1, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T2, T3, T4, T1> other)
	    {
	        int[] map = new[] { 5, 2, 3, 4, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T2, T4, T1, T3> other)
	    {
	        int[] map = new[] { 5, 2, 4, 1, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T2, T4, T3, T1> other)
	    {
	        int[] map = new[] { 5, 2, 4, 3, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T3, T1, T2, T4> other)
	    {
	        int[] map = new[] { 5, 3, 1, 2, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T3, T1, T4, T2> other)
	    {
	        int[] map = new[] { 5, 3, 1, 4, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T3, T2, T1, T4> other)
	    {
	        int[] map = new[] { 5, 3, 2, 1, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T3, T2, T4, T1> other)
	    {
	        int[] map = new[] { 5, 3, 2, 4, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T3, T4, T1, T2> other)
	    {
	        int[] map = new[] { 5, 3, 4, 1, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T3, T4, T2, T1> other)
	    {
	        int[] map = new[] { 5, 3, 4, 2, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T4, T1, T2, T3> other)
	    {
	        int[] map = new[] { 5, 4, 1, 2, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T4, T1, T3, T2> other)
	    {
	        int[] map = new[] { 5, 4, 1, 3, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T4, T2, T1, T3> other)
	    {
	        int[] map = new[] { 5, 4, 2, 1, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T4, T2, T3, T1> other)
	    {
	        int[] map = new[] { 5, 4, 2, 3, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T4, T3, T1, T2> other)
	    {
	        int[] map = new[] { 5, 4, 3, 1, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T4, T3, T2, T1> other)
	    {
	        int[] map = new[] { 5, 4, 3, 2, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    #endregion Implicit Conversion By Type Swap
	    #region Implicit Widening Conversions
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T3> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T4> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T5> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T3> other) => other
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T4> other) => other
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T5> other) => other
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T4> other) => other
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T5> other) => other
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T5> other) => other
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2, T3> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2, T4> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2, T5> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T3, T4> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T3, T5> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T4, T5> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T3, T4> other) => other
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T3, T5> other) => other
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T4, T5> other) => other
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T4, T5> other) => other
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2, T3, T4> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2, T3, T5> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2, T4, T5> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T3, T4, T5> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T3, T4, T5> other) => other
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    #endregion Implicit Widening Conversions
	    #region Constructors
	    public Either(T1 value) { _value = value!; _typeIndex = 1; }
	    public Either(T2 value) { _value = value!; _typeIndex = 2; }
	    public Either(T3 value) { _value = value!; _typeIndex = 3; }
	    public Either(T4 value) { _value = value!; _typeIndex = 4; }
	    public Either(T5 value) { _value = value!; _typeIndex = 5; }
	    #endregion Constructors
	    #region Or methods
	    #endregion Or methods
	    #region IEither Implementation
	    int _typeIndex;
	    object _value;
	    protected override int TypeIndex => _typeIndex;
	    protected override Type Type => _typeIndex switch
	    {
	        1 => typeof(T1),
	        2 => typeof(T2),
	        3 => typeof(T3),
	        4 => typeof(T4),
	        5 => typeof(T5),
	        _ => throw new InvalidOperationException()
	    };
	    protected override object Value => _value;
	    Either(int typeIndex, object value) => (_typeIndex, _value) = (typeIndex, value);
	    #endregion IEither Implementation
	    #region Value Casts
	    T1 AsT1 => (T1)_value;
	    T2 AsT2 => (T2)_value;
	    T3 AsT3 => (T3)_value;
	    T4 AsT4 => (T4)_value;
	    T5 AsT5 => (T5)_value;
	    #endregion Value Casts
	    #region Explicit Casts
	    public static explicit operator T1(Either<T1, T2, T3, T4, T5> either) => either.AsT1;
	    public static explicit operator T2(Either<T1, T2, T3, T4, T5> either) => either.AsT2;
	    public static explicit operator T3(Either<T1, T2, T3, T4, T5> either) => either.AsT3;
	    public static explicit operator T4(Either<T1, T2, T3, T4, T5> either) => either.AsT4;
	    public static explicit operator T5(Either<T1, T2, T3, T4, T5> either) => either.AsT5;
	    #endregion Explicit Casts
	    #region Switch method
	    public void Switch(Action<T1> ifT1, Action<T2> ifT2, Action<T3> ifT3, Action<T4> ifT4, Action<T5> ifT5)
	    {
	        switch (_typeIndex)
	        {
	            case 1: ifT1(AsT1); break;
	            case 2: ifT2(AsT2); break;
	            case 3: ifT3(AsT3); break;
	            case 4: ifT4(AsT4); break;
	            case 5: ifT5(AsT5); break;
	            default: throw new InvalidOperationException();
	        }
	    }
	    #endregion Switch method
	    #region Nonreductive Match
	    public Either<TResult1, T2, T3, T4, T5> Match<TResult1>(Func<T1, TResult1> ifT1) => _typeIndex switch
	    {
	        1 => ifT1(AsT1),
	        2 => AsT2,
	        3 => AsT3,
	        4 => AsT4,
	        5 => AsT5,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, TResult2, T3, T4, T5> Match<TResult2>(Func<T2, TResult2> ifT2) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => ifT2(AsT2),
	        3 => AsT3,
	        4 => AsT4,
	        5 => AsT5,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, TResult3, T4, T5> Match<TResult3>(Func<T3, TResult3> ifT3) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => ifT3(AsT3),
	        4 => AsT4,
	        5 => AsT5,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, TResult4, T5> Match<TResult4>(Func<T4, TResult4> ifT4) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => AsT3,
	        4 => ifT4(AsT4),
	        5 => AsT5,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, T4, TResult5> Match<TResult5>(Func<T5, TResult5> ifT5) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => AsT3,
	        4 => AsT4,
	        5 => ifT5(AsT5),
	        _ => throw new InvalidOperationException()
	    };
	    #endregion Nonreductive Match
	    #region Nonreductive Match - Compositional
	    public Either<TResult1, T2, T3, T4, T5> Match<TResult1>(Func<T1, Either<TResult1, T2, T3, T4, T5>> ifT1) => _typeIndex switch
	    {
	        1 => ifT1(AsT1),
	        2 => AsT2,
	        3 => AsT3,
	        4 => AsT4,
	        5 => AsT5,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, TResult2, T3, T4, T5> Match<TResult2>(Func<T2, Either<T1, TResult2, T3, T4, T5>> ifT2) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => ifT2(AsT2),
	        3 => AsT3,
	        4 => AsT4,
	        5 => AsT5,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, TResult3, T4, T5> Match<TResult3>(Func<T3, Either<T1, T2, TResult3, T4, T5>> ifT3) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => ifT3(AsT3),
	        4 => AsT4,
	        5 => AsT5,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, TResult4, T5> Match<TResult4>(Func<T4, Either<T1, T2, T3, TResult4, T5>> ifT4) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => AsT3,
	        4 => ifT4(AsT4),
	        5 => AsT5,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, T4, TResult5> Match<TResult5>(Func<T5, Either<T1, T2, T3, T4, TResult5>> ifT5) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => AsT3,
	        4 => AsT4,
	        5 => ifT5(AsT5),
	        _ => throw new InvalidOperationException()
	    };
	    #endregion Nonreductive Match - Compositional
	    #region Reductive Match
	    public Either<T2, T3, T4, T5> Match
	        (Func<T1, T2> ifT1) => _typeIndex switch
	        {
	            1 => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T1, T2> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3, T4, T5> Match
	        (Func<T2, T1> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T2, T1> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T2, T3, T4, T5> Match
	        (Func<T1, T3> ifT1) => _typeIndex switch
	        {
	            1 => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T1, T3> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T4, T5> Match
	        (Func<T3, T1> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => ifT3(AsT3),
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T3, T1> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            4 => AsT4,
	            5 => AsT5,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T2, T3, T4, T5> Match
	        (Func<T1, T4> ifT1) => _typeIndex switch
	        {
	            1 => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T1, T4> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T5> Match
	        (Func<T4, T1> ifT4) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => ifT4(AsT4),
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T4, T1> ifT4, Func<T4, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 when (when(AsT4)) => ifT4(AsT4),
	            5 => AsT5,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T2, T3, T4, T5> Match
	        (Func<T1, T5> ifT1) => _typeIndex switch
	        {
	            1 => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T1, T5> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T5, T1> ifT5) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => ifT5(AsT5),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T5, T1> ifT5, Func<T5, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 when (when(AsT5)) => ifT5(AsT5),
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3, T4, T5> Match
	        (Func<T2, T3> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T2, T3> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T4, T5> Match
	        (Func<T3, T2> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => ifT3(AsT3),
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T3, T2> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            4 => AsT4,
	            5 => AsT5,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3, T4, T5> Match
	        (Func<T2, T4> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T2, T4> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T5> Match
	        (Func<T4, T2> ifT4) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => ifT4(AsT4),
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T4, T2> ifT4, Func<T4, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 when (when(AsT4)) => ifT4(AsT4),
	            5 => AsT5,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3, T4, T5> Match
	        (Func<T2, T5> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T2, T5> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T5, T2> ifT5) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => ifT5(AsT5),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T5, T2> ifT5, Func<T5, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 when (when(AsT5)) => ifT5(AsT5),
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T4, T5> Match
	        (Func<T3, T4> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => ifT3(AsT3),
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T3, T4> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            4 => AsT4,
	            5 => AsT5,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T5> Match
	        (Func<T4, T3> ifT4) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => ifT4(AsT4),
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T4, T3> ifT4, Func<T4, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 when (when(AsT4)) => ifT4(AsT4),
	            5 => AsT5,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T4, T5> Match
	        (Func<T3, T5> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => ifT3(AsT3),
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T3, T5> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            4 => AsT4,
	            5 => AsT5,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T5, T3> ifT5) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => ifT5(AsT5),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T5, T3> ifT5, Func<T5, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 when (when(AsT5)) => ifT5(AsT5),
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T5> Match
	        (Func<T4, T5> ifT4) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => ifT4(AsT4),
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T4, T5> ifT4, Func<T4, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 when (when(AsT4)) => ifT4(AsT4),
	            5 => AsT5,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T5, T4> ifT5) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => ifT5(AsT5),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T5, T4> ifT5, Func<T5, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 when (when(AsT5)) => ifT5(AsT5),
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Reductive Match
	    #region Throw Methods
	    public Either<T2, T3, T4, T5> ThrowIf
	        (Func<T1, Exception> ifT1) => _typeIndex switch
	        {
	            1 => throw ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> ThrowIf
	        (Func<T1, Exception> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => throw ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3, T4, T5> ThrowIf
	        (Func<T2, Exception> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => throw ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> ThrowIf
	        (Func<T2, Exception> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => throw ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T4, T5> ThrowIf
	        (Func<T3, Exception> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => throw ifT3(AsT3),
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> ThrowIf
	        (Func<T3, Exception> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => throw ifT3(AsT3),
	            4 => AsT4,
	            5 => AsT5,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T5> ThrowIf
	        (Func<T4, Exception> ifT4) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => throw ifT4(AsT4),
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> ThrowIf
	        (Func<T4, Exception> ifT4, Func<T4, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 when (when(AsT4)) => throw ifT4(AsT4),
	            5 => AsT5,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> ThrowIf
	        (Func<T5, Exception> ifT5) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => throw ifT5(AsT5),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> ThrowIf
	        (Func<T5, Exception> ifT5, Func<T5, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 when (when(AsT5)) => throw ifT5(AsT5),
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Throw Methods
	    #region If (methods)
	    public bool If(out T1 @if) => If(out @if, out _);
	    public bool If(out T1 @if, out Either<T2, T3, T4, T5> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = AsT1;
	                @else = default!;
	                return true;
	            case 2:
	                @if = default!;
	                @else = AsT2;
	                return false;
	            case 3:
	                @if = default!;
	                @else = AsT3;
	                return false;
	            case 4:
	                @if = default!;
	                @else = AsT4;
	                return false;
	            case 5:
	                @if = default!;
	                @else = AsT5;
	                return false;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    public bool If(out T2 @if) => If(out @if, out _);
	    public bool If(out T2 @if, out Either<T1, T3, T4, T5> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = default!;
	                @else = AsT1;
	                return false;
	            case 2:
	                @if = AsT2;
	                @else = default!;
	                return true;
	            case 3:
	                @if = default!;
	                @else = AsT3;
	                return false;
	            case 4:
	                @if = default!;
	                @else = AsT4;
	                return false;
	            case 5:
	                @if = default!;
	                @else = AsT5;
	                return false;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    public bool If(out T3 @if) => If(out @if, out _);
	    public bool If(out T3 @if, out Either<T1, T2, T4, T5> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = default!;
	                @else = AsT1;
	                return false;
	            case 2:
	                @if = default!;
	                @else = AsT2;
	                return false;
	            case 3:
	                @if = AsT3;
	                @else = default!;
	                return true;
	            case 4:
	                @if = default!;
	                @else = AsT4;
	                return false;
	            case 5:
	                @if = default!;
	                @else = AsT5;
	                return false;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    public bool If(out T4 @if) => If(out @if, out _);
	    public bool If(out T4 @if, out Either<T1, T2, T3, T5> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = default!;
	                @else = AsT1;
	                return false;
	            case 2:
	                @if = default!;
	                @else = AsT2;
	                return false;
	            case 3:
	                @if = default!;
	                @else = AsT3;
	                return false;
	            case 4:
	                @if = AsT4;
	                @else = default!;
	                return true;
	            case 5:
	                @if = default!;
	                @else = AsT5;
	                return false;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    public bool If(out T5 @if) => If(out @if, out _);
	    public bool If(out T5 @if, out Either<T1, T2, T3, T4> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = default!;
	                @else = AsT1;
	                return false;
	            case 2:
	                @if = default!;
	                @else = AsT2;
	                return false;
	            case 3:
	                @if = default!;
	                @else = AsT3;
	                return false;
	            case 4:
	                @if = default!;
	                @else = AsT4;
	                return false;
	            case 5:
	                @if = AsT5;
	                @else = default!;
	                return true;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    #endregion If (methods)
	    #region ToString
	    public override string ToString() => $"{Type.Name}:{_value}";
	    #endregion ToString
	}
	#endregion
	#region \System\Threading
	public abstract class ActiveChangeToken 
	{
	    protected ActiveChangeToken()
	    {
	        var token = default(ChangeToken<string>);
	        using var tracker = token.OnChange(state =>
	        {
	        });
	    }
	}
	public abstract class ChangeToken<T> : IChangeToken<T>
	{
	    private readonly List<IDisposable> subscribers;
	    protected ChangeToken()
	    {
	        subscribers = new List<IDisposable>();
	    }
	    public abstract IDisposable OnChange(Action<T> callback);
	    IDisposable IChangeToken.OnChange(Action<object> callback)
	    {
	        return OnChange((callback as Action<T>)!);
	    }
	}
	public abstract class PollingChangeToken<T> : IChangeToken<T>, IDisposable
	{
	    private readonly Timer timer;
	    private readonly List<IDisposable> subscribers;
	    protected PollingChangeToken(TimeSpan startAfter, TimeSpan interval)
	        : this(startAfter, interval, null)
	    {
	    }
	    protected PollingChangeToken(TimeSpan startAfter, TimeSpan interval, object? state)
	    {
	        subscribers = new List<IDisposable>();
	        timer = new Timer((state =>
	        {
	            if (HasChanged(state, out var data))
	            {
	            }
	        }),
	        state,
	        startAfter,
	        interval);
	    }
	    public abstract bool HasChanged(out T? state);
	    public abstract IDisposable OnChange(Action<T> callback);
	    public virtual bool HasChanged(object? data, out T? state)
	    {
	        return HasChanged(out state);
	    }
	    public virtual IDisposable OnChange(Action<object> callback)
	    {
	        if (callback is not Action<T>)
	        {
	            ThrowHelper.ThrowInvalidOperationException("");
	        }
	        return OnChange((callback as Action<T>)!);
	    }
	    public void Dispose()
	    {
	        timer.Dispose();
	    }
	}
	public class TestPollingChangeToken : PollingChangeToken<string>
	{
	    public TestPollingChangeToken(TimeSpan startAfter, TimeSpan interval) : base(startAfter, interval)
	    {
	    }
	    public override bool HasChanged(out string? state)
	    {
	        state = null;
	        return true;
	    }
	    public override IDisposable OnChange(Action<string> callback)
	    {
	        throw new NotImplementedException();
	    }
	}
	public interface IChangeToken
	{
	    IDisposable OnChange(Action<object> callback);
	}
	public interface IChangeToken<T> : IChangeToken
	{
	    IDisposable OnChange(Action<T> callback);
	}
	#endregion
	#region \System\Threading\Tasks
	public static class AsyncExtensions
	{
	    public static async IAsyncEnumerable<T> EnumerateAsync<T>(this IEnumerable<Task<T>> tasks)
	    {
	        var items = tasks is ICollection<Task<T>> collection ? collection : tasks.ToList();
	        while (items.Any())
	        {
	            var finished = await Task.WhenAny(items);
	            items.Remove(finished);
	            yield return finished.Result;
	        }
	    }
	}
	public static class TaskCancellationExtensions
	{
	    //public static Task OnCancel<T>(this Task<T> task, Action onCancel)
	    //{
	    //    var taskCompletionSource = new TaskCompletionSource()
	    //    return task.ContinueWith<T>((_,t) =>
	    //    {
	    //    });
	    //}
	    //    public static Task<Either<None, Cancelled>> CaptureCancellation(this Task task) =>
	    //    CaptureCancellation(task.ToTaskOfT<None>());
	    //    public static Task<Either<T, Cancelled>> CaptureCancellation<T>(this Task<T> task) =>
	    //        CaptureCancellation(task,
	    //            new TaskCompletionSource<Either<T, Cancelled>>(),
	    //            tcs => tcs.TrySetResult(new Cancelled()),
	    //            tcs => tcs.TrySetResult(task.Result));
	    //    public static Task<T> CaptureCancellation<T>(this Task<T> task, onCancel)
	    //    public static Task<T> CaptureCancellation<T>(Task<T> task,
	    //        TaskCompletionSource<T> taskCompletionSource,
	    //        Action<TaskCompletionSource<T>> onCancel,
	    //        Action<TaskCompletionSource<T?>> onSuccess)
	    //    {
	    //        task.ContinueWith(_ =>
	    //        {
	    //            if (task.IsCanceled || task.IsFaulted && task.Exception.InnerException is OperationCanceledException)
	    //            {
	    //                onCancel.Invoke(taskCompletionSource);
	    //            }
	    //            else
	    //            {
	    //                onSuccess.Invoke(taskCompletionSource);
	    //            }
	    //        });
	    //        return taskCompletionSource.Task;
	    //    }
	    //    private static Task<T> ToTaskOfT<T>(this Task task, T? value = default)
	    //    {
	    //        if (task is Task<T> t)
	    //        {
	    //            return t;
	    //        }
	    //        var taskCompletionSource = new TaskCompletionSource<T>();
	    //        task.ContinueWith(ant =>
	    //        {
	    //            if (ant.IsCanceled) taskCompletionSource.SetCanceled();
	    //            else if (ant.IsFaulted) taskCompletionSource.SetException(ant.Exception.InnerException);
	    //            else taskCompletionSource.SetResult(value);
	    //        });
	    //        return taskCompletionSource.Task;
	    //    }
	}
	#endregion
	#region \Utilities
	public static class Memoise<TIn, TOut>
	    where TIn : notnull
	{
	    private static IDictionary<TIn, TOut> cache;
	    static Memoise()
	    {
	        cache ??= new ConcurrentDictionary<TIn, TOut>();
	    }
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public static Func<TIn, TOut> Invoke(Func<TIn, TOut> method)
	    {
	        return input => cache.TryGetValue(input, out var results) ?
	            results :
	            cache[input] = method.Invoke(input);
	    }
	}
	#endregion
}
#endregion
