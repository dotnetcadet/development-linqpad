<Query Kind="Program">
  <Namespace>Internal</Namespace>
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <Namespace>Assimalign.Cohesion.FileSystemProviders</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load ".\assimalign.cohesion.core"

void Main()
{

}

#region Assimalign.Cohesion.FileSystemProviders(net8.0)
namespace Assimalign.Cohesion.FileSystemProviders
{
	#region \
	[Flags]
	public enum ExclusionFilterType
	{
	    Sensitive = DotPrefixed | Hidden | System,
	    DotPrefixed = 0x0001,
	    Hidden = 0x0002,
	    System = 0x0004,
	    None = 0
	}
	[DebuggerDisplay("{Name}")]
	public record FileName
	{
	    public FileName(string name)
	    {
	        if (string.IsNullOrWhiteSpace(name))
	        {
	            ThrowHelper.ThrowArgumentException($"{nameof(name)} cannot be null or empty");
	        }
	        else if (System.IO.Path.GetInvalidFileNameChars().Intersect(name).Any())
	        {
	            ThrowHelper.ThrowArgumentException($"{nameof(name)} contains illegal characters");
	        }
	        else
	        {
	            Name = System.IO.Path.GetFullPath(name.Trim());
	        }
	    }
	    public string Name { get; set; }
	    public virtual bool Equals(FileName? other) => Name.Equals(other?.Name, StringComparison.InvariantCultureIgnoreCase);
	    public static implicit operator FileName(string name) => new(name);
	    public static implicit operator string(FileName name) => name.Name;
	    public override string ToString() => Name;
	}	
	[DebuggerDisplay("{Path}")]
	public class FilePath : IEquatable<FilePath>, IEqualityComparer<FilePath>, IComparable<FilePath>
	{
	    public FilePath(string path)
	    {
	        if (string.IsNullOrWhiteSpace(path))
	        {
	            ThrowHelper.ThrowArgumentException($"{nameof(path)} cannot be null or empty");
	        }
	        else if (System.IO.Path.GetInvalidPathChars().Intersect(path).Any())
	        {
	            ThrowHelper.ThrowArgumentException($"{nameof(path)} contains illegal characters");
	        }
	        else
	        {
	            Path = System.IO.Path.GetFullPath(path.Trim());
	        }
	    }
	    public string Path { get; }
		#region Overloads
	    public override bool Equals(object? obj)
	    {
	        return obj is FilePath path ? Equals(path) : false;
	    }
	    public override string ToString()
	    {
	        return Path;
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		#endregion
		public virtual bool Equals(FilePath? other) => Path.Equals(other?.Path, StringComparison.InvariantCultureIgnoreCase);
	    #region Operators
	    public static implicit operator FilePath(string name) => new(name);
	    public static implicit operator string(FilePath path) => path.Path;
	    public static bool operator ==(FilePath left, FilePath right) => left.Equals(right);
	    public static bool operator !=(FilePath left, FilePath right) => !left.Equals(right);
	    #endregion
	    public FilePath Combine(params string[] paths) => System.IO.Path.Combine(paths.Prepend(Path).ToArray());    
	    public bool Equals(FilePath? right, FilePath? left)
	    {
	        throw new NotImplementedException();
	    }
	    public int GetHashCode([DisallowNull] FilePath obj)
	    {
	        throw new NotImplementedException();
	    }
	    public int CompareTo(FilePath? other)
	    {
	        throw new NotImplementedException();
	    }
	}
	[DebuggerDisplay("Length: {Length}")]
	public readonly struct FileSize : IEquatable<FileSize>, IComparable<FileSize>, IEqualityComparer<FileSize>, IFormattable
	{
	    private const long kilobyte = 1000;
	    private const long megabyte = kilobyte * 1000;
	    private const long gigabyte = megabyte * 1000;
	    private const long terabyte = gigabyte * 1000;
	    private const long petabyte = terabyte * 1000;
	    public FileSize(long length)
	    {
	        if (Length < -1)
	        {
	            ThrowHelper.ThrowArgumentException("File size must be greater than ");
	        }
	        Length = length;
	    }
	    public static FileSize Default => new FileSize(-1);
	    public long Length { get; }
	    public decimal Kilobytes => (decimal)Length / kilobyte;
	    public decimal Megabytes => (decimal)Length / megabyte;
	    public decimal Gigabytes => (decimal)Length / gigabyte;
	    public decimal Terabytes => (decimal)Length / terabyte;
	    public decimal Petabytes => (decimal)Length / petabyte;
	    #region Overloads
	    public override bool Equals([NotNullWhen(true)] object? obj)
	    {
	        return obj is FileSize size ? Equals(size) : false;
	    }
	    public override string ToString()
	    {
	        return ToString();
	    }
	    public override int GetHashCode()
	    {
	        return HashCode.Combine(typeof(FileSize), Length);
	    }
	    #endregion
	    #region Interfaces
	    public bool Equals(FileSize other)
	    {
	        return other.Length == Length;
	    }
	    public bool Equals(FileSize left, FileSize right)
	    {
	        return left.Equals(right);
	    }
	    public int GetHashCode([DisallowNull] FileSize obj)
	    {
	        return obj.GetHashCode();
	    }
	    public int CompareTo(FileSize other)
	    {
	        return Length.CompareTo(other.Length);
	    }
	    public string ToString(string? format, IFormatProvider? formatProvider)
	    {
	        //formatProvider.
	        throw new NotImplementedException();
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
	    public static FileSize FromKilobytes(long size)
	    {
	        return new FileSize(size * kilobyte);
	    }
	    public static FileSize FromMegabytes(long size)
	    {
	        return new FileSize(size * megabyte);
	    }
	    public static FileSize FromGigabytes(long size)
	    {
	        return new FileSize(size * gigabyte);
	    }
	    public static FileSize FromTerabytes(long size)
	    {
	        return new FileSize(size * terabyte);
	    }
	    #endregion
	}
	#endregion
	#region \Abstractions
	public interface IFileSystemDirectory : IFileSystemInfo
	{
	    IEnumerable<IFileSystemInfo> GetFiles();
	    IFileSystemDirectory GetDirectory(string path);
		IFileSystemFile GetFile(string path);
	    bool TryGetDirectory(string path, out IFileSystemDirectory? directory);
	    bool TryGetFile(string path, out IFileSystemFile? file);
	}
	public interface IFileSystemFile : IFileSystemInfo
	{
	    FileSize Length { get; }
	    IFileSystemDirectory ParentDirectory { get; }
	    Stream GetStream();
	}
	public interface IFileSystemInfo
	{
	    FileName Name { get; }
	    FilePath Path { get; }
	    DateTimeOffset UpdatedDateTime { get; }
	    DateTimeOffset CreatedDateTime { get; }
	    bool IsDirectory { get; }
	    bool IsFile { get; }
	}
	public interface IFileSystemProvider
	{
	    IFileSystemInfo GetFile(string subpath);
	    IFileSystemDirectory GetDirectory(string subpath);
	    IChangeToken Watch(string filter);
	}
	#endregion
	#region \Internal
	    internal sealed class Clock : IClock
	    {
	        public static readonly Clock Instance = new Clock();
	        private Clock()
	        {
	        }
	        public DateTime UtcNow => DateTime.UtcNow;
	    }
	    internal sealed class EmptyDisposable : IDisposable
	    {
	        public static EmptyDisposable Instance { get; } = new EmptyDisposable();
	        private EmptyDisposable()
	        {
	        }
	        public void Dispose()
	        {
	        }
	    }
	internal static class FileSystemInfoHelper
	{
	    public static bool IsExcluded(FileSystemInfo fileSystemInfo, ExclusionFilterType filters)
	    {
	        if (filters == ExclusionFilterType.None)
	        {
	            return false;
	        }
	        else if (fileSystemInfo.Name.StartsWith(".", StringComparison.Ordinal) && (filters & ExclusionFilterType.DotPrefixed) != 0)
	        {
	            return true;
	        }
	        else if (fileSystemInfo.Exists &&
	            (((fileSystemInfo.Attributes & FileAttributes.Hidden) != 0 && (filters & ExclusionFilterType.Hidden) != 0) ||
	             ((fileSystemInfo.Attributes & FileAttributes.System) != 0 && (filters & ExclusionFilterType.System) != 0)))
	        {
	            return true;
	        }
	        return false;
	    }
	    public static DateTime? GetFileLinkTargetLastWriteTimeUtc(string filePath)
	    {
	        var fileInfo = new FileInfo(filePath);
	        if (fileInfo.Exists)
	        {
	            return GetFileLinkTargetLastWriteTimeUtc(fileInfo);
	        }
	        return null;
	    }
	    // If file is a link and link target exists, return target's LastWriteTimeUtc.
	    // If file is a link, and link target does not exists, return DateTime.MinValue
	    //   since the link's LastWriteTimeUtc doesn't convey anything for this scenario.
	    // If file is not a link, return null to inform the caller that file is not a link.
	    public static DateTime? GetFileLinkTargetLastWriteTimeUtc(FileInfo fileInfo)
	    {
	        Debug.Assert(fileInfo.Exists);
	        if (fileInfo.LinkTarget != null)
	        {
	            try
	            {
	                FileSystemInfo targetInfo = fileInfo.ResolveLinkTarget(returnFinalTarget: true);
	                if (targetInfo != null && targetInfo.Exists)
	                {
	                    return targetInfo.LastWriteTimeUtc;
	                }
	            }
	            catch (FileNotFoundException)
	            {
	                // The file ceased to exist between LinkTarget and ResolveLinkTarget.
	            }
	            return DateTime.MinValue;
	        }
	        return null;
	    }
	}
	    // A convenience API for interacting with System.Threading.Timer in a way
	    // that doesn't capture the ExecutionContext. We should be using this (or equivalent)
	    // everywhere we use timers to avoid rooting any values stored in asynclocals.
	    internal static class NonCapturingTimer
	    {
	        public static Timer Create(TimerCallback callback, object state, TimeSpan dueTime, TimeSpan period)
	        {
	            if (callback == null)
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
	#endregion
	#region \Internal\Abstractions
	internal interface IClock
	{
	    DateTime UtcNow { get; }
	}
	internal interface IPollingChangeToken : IChangeToken
	{
	    CancellationTokenSource CancellationTokenSource { get; }
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
}
#endregion
