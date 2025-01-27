<Query Kind="Program">
  <Namespace>Internal</Namespace>
  <Namespace>Globbing</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Runtime.Versioning</Namespace>
  <Namespace>System.Security</Namespace>
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <Namespace>Assimalign.Cohesion.FileSystem.Physical</Namespace>
</Query>

#load ".\assimalign.cohesion.core"
#load ".\assimalign.cohesion.filesystem"

void Main()
{
	var fileSystem = new PhysicalFileSystem("*");
	
	foreach (var item in fileSystem)
	{
		item.Path.Dump();
	}

}

#region Assimalign.Cohesion.FileSystem.Physical(net8.0)
namespace Assimalign.Cohesion.FileSystem.Physical
{
	#region \
	[DebuggerDisplay("{Name} - {Size}")]
	public sealed class PhysicalFileSystem : IFileSystem
	{
	    private readonly DriveInfo driveInfo;
	    private static readonly char[] separators =
	    [
	        System.IO.Path.DirectorySeparatorChar,
	        System.IO.Path.AltDirectorySeparatorChar
	    ];
	    public PhysicalFileSystem(string drive)
	    {
	        this.driveInfo = new DriveInfo(drive);
	    }
	    public string Name => driveInfo.Name;
	    public Size Size => driveInfo.TotalSize;
	    public Size Space => driveInfo.TotalFreeSpace;
	    public Size SpaceUsed => (driveInfo.TotalSize - driveInfo.TotalFreeSpace);
	    public IFileSystemDirectory RootDirectory => new PhysicalFileSystemDirectory(driveInfo.RootDirectory);
	    public bool Exist(Path path)
	    {
	#if NET7_0_OR_GREATER
	        return System.IO.Path.Exists(path);
	#else
	        var fullPath = System.IO.Path.GetFullPath(path);
	        if (File.Exists(fullPath) || Directory.Exists(fullPath))
	        {
	            return true;
	        }
	        return false;
	#endif
	    }
	    public IFileSystemDirectory CreateDirectory(Path path)
	    {
	        var fullPath = GetFullPath(path);
	        var directoryInfo = Directory.CreateDirectory(fullPath);
	        return new PhysicalFileSystemDirectory(directoryInfo);
	    }
	    public IFileSystemFile CreateFile(Path path)
	    {
	        var fullPath = GetFullPath(path);
	        var stream = File.Create(fullPath);
	        throw new NotImplementedException();
	    }
	    public void DeleteDirectory(Path path)
	    {
	        var fullPath = GetFullPath(path);
	        CheckFileOrDirectoryExist(fullPath);
	        Directory.Delete(fullPath);
	    }
	    public void DeleteFile(Path path)
	    {
	        var fullPath = GetFullPath(path);
	        CheckFileOrDirectoryExist(fullPath);
	        File.Delete(fullPath);
	    }
	    public IFileSystemDirectory GetDirectory(Path path)
	    {
	        CheckFileOrDirectoryExist(path);
	        throw new NotImplementedException();
	    }
	    public IFileSystemFile GetFile(Path path)
	    {
	        CheckFileOrDirectoryExist(path);
	        throw new NotImplementedException();
	    }
	    public void CopyFile(Path source, Path destination)
	    {
	        throw new NotImplementedException();
	    }
	    public IFileSystemChangeToken Watch(string filter)
	    {
	        if (filter is null)
	        {
	            throw new ArgumentNullException(nameof(filter));
	        }
	        if (PathUtilities.HasInvalidFilterChars(filter))
	        {
	            throw new ArgumentException("The provider filter has an invalid character.");
	        }
	        // Relative paths starting with leading slashes are okay
	        filter = filter.TrimStart(separators);
	        throw new NotImplementedException();
	    }
	    public IEnumerable<IFileSystemDirectory> GetDirectories()
	    {
	        return this.OfType<IFileSystemDirectory>();
	    }
	    public IEnumerable<IFileSystemFile> GetFiles()
	    {
	        return this.OfType<IFileSystemFile>();
	    }
	    public IEnumerator<IFileSystemInfo> GetEnumerator()
	    {
	        return driveInfo.RootDirectory.EnumerateFileSystemInfos("*", new EnumerationOptions()
	        {
	            IgnoreInaccessible = true,
	            RecurseSubdirectories = true
	        })
	            .Select<FileSystemInfo, IFileSystemInfo>(item => item switch
	            {
	                FileInfo info => new PhysicalFileSystemFile(info),
	                DirectoryInfo info => new PhysicalFileSystemDirectory(info),
	                _ => throw new Exception("Invalid object in physical file system.")
	            }).GetEnumerator();
	    }
	    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	    public void Dispose()
	    {
	        throw new NotImplementedException();
	    }
	    private Path GetFullPath(Path path)
	    {
	        return System.IO.Path.GetFullPath(path);
	    }
	    private void CheckFileOrDirectoryExist(Path path)
	    {
	        if (!Exist(path))
	        {
	            throw new FileSystemException($"The given path: {path} does not exist.");
	        }
	    }
	    //private IEnumerable<TFileSystemInfo> Enumerate<TFileSystemInfo>(IEnumerable<IFileSystemInfo> enumerable)
	    //    where TFileSystemInfo : IFileSystemInfo
	    //{
	    //    foreach (var item in enumerable)
	    //    {
	    //        if (item is TFileSystemInfo fsInfo)
	    //        {
	    //            yield return fsInfo;
	    //            if (fsInfo is IEnumerable<IFileSystemInfo> children)
	    //            {
	    //                foreach (var child in Enumerate<TFileSystemInfo>(children))
	    //                {
	    //                    yield return child;
	    //                }
	    //            }
	    //        }
	    //    }
	    //}
	}
	[DebuggerDisplay("{Path}")]
	public class PhysicalFileSystemDirectory : IFileSystemDirectory
	{
	    private readonly string directory;
	    private readonly DirectoryInfo directoryInfo;
	    private readonly ExclusionFilterType directoryInfoFilters;
	    private IEnumerable<IFileSystemInfo> files;
	    public PhysicalFileSystemDirectory(string directory)
	        : this(directory, ExclusionFilterType.Sensitive) { }
	    public PhysicalFileSystemDirectory(string directory, ExclusionFilterType filters)
	        : this(new DirectoryInfo(directory), filters)
	    {
	        this.directory = directory;
	    }
	    public PhysicalFileSystemDirectory(DirectoryInfo directoryInfo)
	        : this(directoryInfo, ExclusionFilterType.Sensitive) { }
	    internal PhysicalFileSystemDirectory(DirectoryInfo directoryInfo, ExclusionFilterType filters)
	    {
	        this.directoryInfo = directoryInfo;
	    }
	    private void EnsureInitialized()
	    {
	        try
	        {
	            files = directoryInfo
	                .EnumerateFileSystemInfos()
	                .Where(info => !FileSystemInfoHelper.IsExcluded(info, directoryInfoFilters))
	                .Select<FileSystemInfo, IFileSystemInfo>(info =>
	                {
	                    return info switch
	                    {
	                        FileInfo file => new PhysicalFileSystemFile(file),
	                        DirectoryInfo directory => new PhysicalFileSystemDirectory(directory),
	                    };
	                });
	        }
	        catch (Exception ex) when (ex is DirectoryNotFoundException || ex is IOException)
	        {
	            files = Enumerable.Empty<IFileSystemInfo>();
	        }
	    }
	    public IFileSystemDirectory GetDirectory(FileName name)
	    {
	        throw new NotImplementedException();
	    }
	    public bool TryGetDirectory(FileName name, out IFileSystemDirectory? directory)
	    {
	        throw new NotImplementedException();
	    }
	    public IFileSystemFile GetFile(FileName name)
	    {
	        throw new NotImplementedException();
	    }
	    public bool TryGetFile(FileName name, out IFileSystemFile? file)
	    {
	        throw new NotImplementedException();
	    }
	    public IEnumerable<IFileSystemDirectory> GetDirectories()
	    {
	        return this.OfType<IFileSystemDirectory>();
	    }
	    public IEnumerable<IFileSystemFile> GetFiles()
	    {
	        return this.OfType<IFileSystemFile>();
	    }
	    public IEnumerator<IFileSystemInfo> GetEnumerator()
	    {
	        return directoryInfo.EnumerateFileSystemInfos("*", new EnumerationOptions()
	        {
	            IgnoreInaccessible = true,
	            RecurseSubdirectories = true
	        })
	            .Select<FileSystemInfo, IFileSystemInfo>(item => item switch
	            {
	                FileInfo info => new PhysicalFileSystemFile(info),
	                DirectoryInfo info => new PhysicalFileSystemDirectory(info),
	                _ => throw new Exception("Invalid object in physical file system.")
	            }).GetEnumerator();
	    }
	    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	    public bool Exist(Path path)
	    {
	        throw new NotImplementedException();
	    }
	    public IFileSystemDirectory GetDirectory(Path path)
	    {
	        throw new NotImplementedException();
	    }
	    public bool TryGetDirectory(Path path, out IFileSystemDirectory? directory)
	    {
	        throw new NotImplementedException();
	    }
	    public IFileSystemFile GetFile(Path path)
	    {
	        throw new NotImplementedException();
	    }
	    public bool TryGetFile(Path path, out IFileSystemFile? file)
	    {
	        throw new NotImplementedException();
	    }

		IEnumerable<IFileSystemInfo> IFileSystemDirectory.GetFiles()
		{
			throw new NotImplementedException();
		}

		public bool Exists => directoryInfo.Exists;
	    public long Length => -1;
	    public string PhysicalPath => directoryInfo.FullName;
	    public string Name => directoryInfo.Name;
	    public DateTimeOffset LastModified => directoryInfo.LastWriteTimeUtc;
	    public IFileSystemDirectory Parent => throw new NotImplementedException();
	    public DateTimeOffset UpdatedDateTime => throw new NotImplementedException();
	    public DateTimeOffset CreatedDateTime => throw new NotImplementedException();
	    Path IFileSystemInfo.Path => throw new NotImplementedException();

		public DateTimeOffset UpdatedOn => throw new NotImplementedException();

		public DateTimeOffset CreatedOn => throw new NotImplementedException();

		public DateTimeOffset AccessedOn => throw new NotImplementedException();
	}
	internal class PhysicalFileSystemFile : IFileSystemFile
	{
	    private readonly FileInfo fileInfo;
	    private FileStream? stream;
	    public PhysicalFileSystemFile(FileInfo fileInfo)
	    {
	        this.fileInfo = fileInfo;
	    }
	    public PhysicalFileSystemFile(Path path)
	    {
	        this.fileInfo = new FileInfo(path);
	    }
	    public string Name => fileInfo.Name;
	    public Path Path => fileInfo.FullName;
	    public Size Size => fileInfo.Length;
	    public DateTimeOffset UpdatedOn => fileInfo.LastWriteTimeUtc;
	    public DateTimeOffset CreatedOn => fileInfo.CreationTimeUtc;
	    public DateTimeOffset AccessedOn => fileInfo.LastAccessTimeUtc;
	    public IFileSystemDirectory Directory => new PhysicalFileSystemDirectory(fileInfo.Directory!);
	    //private FileStream TryOpen()
	    //{
	    //    if (isOpen) return stream!;
	    //    // We are setting buffer size to 1 to prevent FileStream from allocating it's internal buffer
	    //    // 0 causes constructor to throw
	    //    int bufferSize = 1;
	    //    stream ??= new FileStream(
	    //        Path,
	    //        FileMode.Open,
	    //        FileAccess.ReadWrite,
	    //        FileShare.ReadWrite,
	    //        bufferSize,
	    //        FileOptions.Asynchronous | FileOptions.SequentialScan);
	    //    isOpen = true;
	    //    return stream;
	    //}
	    public Stream GetStream()
	    {
	        throw new NotImplementedException();
	    }
	    public void Dispose()
	    {
	        throw new NotImplementedException();
	    }
	    public IFileSystemChangeToken Watch()
	    {
	        throw new NotImplementedException();
	    }
	}
	#endregion
	#region \Internal
	internal class PhysicalFileSystemChangeToken : IFileSystemChangeToken
	{
	    public bool HasChanged => throw new NotImplementedException();
	    public bool ActiveChangeCallbacks => throw new NotImplementedException();
	    public void OnChange(Action<IFileSystemInfo> callback)
	    {
	        throw new NotImplementedException();
	    }
	    public IDisposable OnChange(Action<object> callback, object state)
	    {
	        throw new NotImplementedException();
	    }
	    public void OnDelete(Action<IFileSystemInfo> callback)
	    {
	        throw new NotImplementedException();
	    }
	    public void OnRename(Action<IFileSystemInfo> callback)
	    {
	        throw new NotImplementedException();
	    }
	}
	#endregion
	#region \Internal\Utilities
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
	internal static class PathUtilities
	{
	    private static readonly char[] _invalidFileNameChars = System.IO.Path.GetInvalidFileNameChars()
	        .Where(c => c != System.IO.Path.DirectorySeparatorChar && c != System.IO.Path.AltDirectorySeparatorChar).ToArray();
	    private static readonly char[] _invalidFilterChars = _invalidFileNameChars
	        .Where(c => c != '*' && c != '|' && c != '?').ToArray();
	    private static readonly char[] _pathSeparators = new[]
	        {System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar};
	    internal static bool HasInvalidPathChars(string path)
	    {
	        return path.IndexOfAny(_invalidFileNameChars) != -1;
	    }
	    internal static bool HasInvalidFilterChars(string path)
	    {
	        return path.IndexOfAny(_invalidFilterChars) != -1;
	    }
	    internal static string EnsureTrailingSlash(string path)
	    {
	        if (!string.IsNullOrEmpty(path) &&
	            path[path.Length - 1] != System.IO.Path.DirectorySeparatorChar)
	        {
	            return path + System.IO.Path.DirectorySeparatorChar;
	        }
	        return path;
	    }
	    internal static bool PathNavigatesAboveRoot(string path)
	    {
	        int depth = 0;
	        foreach (var segment in GetSegments(path))
	        {
	            if (segment.Equals(".") || segment.Equals(""))
	            {
	                continue;
	            }
	            else if (segment.Equals(".."))
	            {
	                depth--;
	                if (depth == -1)
	                {
	                    return true;
	                }
	            }
	            else
	            {
	                depth++;
	            }
	        }
	        return false;
	    }
	    private static IEnumerable<string> GetSegments(string path)
	    {
	        int index = 0;
	        for (int i = 0; i < path.Length; i++)
	        {
	            if (_pathSeparators.Contains(path[i]))
	            {
	                yield return path.Substring(index, i - 1);
	                index = i + 1;
	            }
	            if ((i + 1) == path.Length)
	            {
	                yield return path.Substring(index, i + 1);
	            }
	        }
	    }
	}
	internal static class ThrowHelper
	{
	    [DoesNotReturn]
	    public static void ThrowPlatformNotSupportedException()
	    {
	        throw new PlatformNotSupportedException();
	    }
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
}
#endregion
