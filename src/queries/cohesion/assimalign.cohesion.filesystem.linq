<Query Kind="Program">
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load ".\assimalign.cohesion.core"

void Main()
{

}

#region Assimalign.Cohesion.FileSystem(net8.0)
namespace Assimalign.Cohesion.FileSystem
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
	#endregion
	#region \Abstractions
	public interface IFileSystem : IEnumerable<IFileSystemInfo>, IDisposable
	{
	    string Name { get; }
	    Size Size { get; }
	    Size Space { get; }
	    Size SpaceUsed { get; }
	    IFileSystemDirectory RootDirectory { get; }
	    bool Exist(Path path);
	    IFileSystemChangeToken Watch(string filter);
	    IEnumerable<IFileSystemDirectory> GetDirectories();
	    IEnumerable<IFileSystemFile> GetFiles();
	    IFileSystemDirectory GetDirectory(Path path);
	    IFileSystemFile GetFile(Path path);
	    IFileSystemDirectory CreateDirectory(Path path);
	    IFileSystemFile CreateFile(Path path);
	    void DeleteDirectory(Path path);
	    void DeleteFile(Path path);
	    void CopyFile(Path source, Path destination);
	}
	public interface IReadOnlyFileSystem : IEnumerable<IFileSystemInfo>, IAsyncDisposable
	{
	    string Name { get; }
	    Size Size { get; }
	    Size Space { get; }
	    Size SpaceUsed { get; }
	    IFileSystemDirectory RootDirectory { get; }
	    bool Exist(Path path);
	    IEnumerable<IFileSystemDirectory> GetDirectories();
	    IEnumerable<IFileSystemFile> GetFiles();
	    IFileSystemDirectory GetDirectory(Path path);
	    IFileSystemFile GetFile(Path path);
	}
	public interface IFileSystemChangeToken : IChangeToken
	{
	    void OnChange(Action<IFileSystemInfo> callback);
	    void OnDelete(Action<IFileSystemInfo> callback);
	    void OnRename(Action<IFileSystemInfo> callbcack);
	}
	public interface IFileSystemDirectory : IFileSystemInfo, IEnumerable<IFileSystemInfo>
	{
	    //long Count { get; }
	    IFileSystemDirectory? Parent { get; }
	    bool Exist(Path path);
	    IEnumerable<IFileSystemDirectory> GetDirectories();
	    IFileSystemDirectory GetDirectory(Path path);
	    IEnumerable<IFileSystemInfo> GetFiles();
	    IFileSystemFile GetFile(Path path);
	}
	public interface IFileSystemFactory
	{
	    IFileSystem Create(string name);
	    IFileSystem Create<TFileSystem>() where TFileSystem : IFileSystem;
	}
	public interface IFileSystemFile : IFileSystemInfo, IDisposable
	{
	    Size Size { get; }
	    IFileSystemDirectory Directory { get; }
	    Stream GetStream();
	    IFileSystemChangeToken Watch();
	    //int Read(Span<byte> buffer, long offset);
	    //ValueTask<int> ReadAsync(Span<byte> buffer, long offset);
	    //void Write(Span<byte> buffer, long offset);
	    //ValueTask WriteAsync(Span<byte> buffer, long offset);
	}
	public interface IFileSystemInfo
	{
	    string Name { get; }
	    Path Path { get; }
	    DateTimeOffset UpdatedOn { get; }
	    DateTimeOffset CreatedOn { get; }
	    DateTimeOffset AccessedOn { get; }
	}
	#endregion
	#region \Exceptions
	public class FileSystemException : CoreException
	{
	    public FileSystemException(string message) : base(message)
	    {
	    }
	}
	#endregion
	#region \Extensions
	public static class FileSystemExtensions
	{
	    //public static Size GetUsedSpace(this IFileSystem fileSystem)
	    //{
	    //    var size = fileSystem.Size;
	    //    var space = fileSystem.Space;
	    //    return (size - space);
	    //}
	}
	public static class FileSystemFileExtensions
	{
	    public static void WriteObjectAsJson<T>(this IFileSystemFile file, T value, JsonSerializerOptions? options = null)
	    {
	        if (file is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(file));
	        }
	        JsonSerializer.Serialize<T>(file.GetStream(), value, options);
	    }
	}
	#endregion
	#region \Internal
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
	#region \obj\Debug\net8.0
	#endregion
}
#endregion
