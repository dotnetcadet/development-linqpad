<Query Kind="Program">
  <Namespace>static Zio.FileSystemExceptionHelper</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.ComponentModel</Namespace>
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <Namespace>System.IO.Compression</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
</Query>

using Zio;

void Main()
{
	var fileSystem = new MemoryFileSystem();
	
	var fs = new SubFileSystem(fileSystem,"");
	
	fileSystem.CreateDirectory("test");
	
	var handle = File.OpenHandle("");

}

#region Zio(netstandard2.0)
namespace Zio
{
	#region \
	public class DirectoryEntry : FileSystemEntry
	{
	    public DirectoryEntry(IFileSystem fileSystem, UPath path) : base(fileSystem, path)
	    {
	    }
	    public void Create()
	    {
	        FileSystem.CreateDirectory(Path);
	    }
	    public DirectoryEntry CreateSubdirectory(UPath path)
	    {
	        if (!path.IsRelative)
	        {
	            throw new ArgumentException("The path must be relative", nameof(path));
	        }
	        // Check that path is not null and relative
	        var subPath = new DirectoryEntry(FileSystem, Path / path);
	        subPath.Create();
	        return subPath;
	    }
	    public void Delete(bool recursive)
	    {
	        FileSystem.DeleteDirectory(Path, recursive);
	    }
	    public IEnumerable<DirectoryEntry> EnumerateDirectories(string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
	    {
	        return FileSystem.EnumerateDirectoryEntries(Path, searchPattern, searchOption);
	    }
	    public IEnumerable<FileEntry> EnumerateFiles(string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
	    {
	        return FileSystem.EnumerateFileEntries(Path, searchPattern, searchOption);
	    }
	    public IEnumerable<FileSystemEntry> EnumerateEntries(string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly, SearchTarget searchTarget = SearchTarget.Both)
	    {
	        return FileSystem.EnumerateFileSystemEntries(Path, searchPattern, searchOption, searchTarget);
	    }
	    public IEnumerable<FileSystemItem> EnumerateItems(SearchOption searchOption = SearchOption.TopDirectoryOnly, SearchPredicate? searchPredicate = null)
	    {
	        return FileSystem.EnumerateItems(Path, searchOption, searchPredicate);
	    }
	    public void MoveTo(UPath destDirName)
	    {
	        FileSystem.MoveDirectory(Path, destDirName);
	    }
	    public override bool Exists => FileSystem.DirectoryExists(Path);
	    public override void Delete()
	    {
	        Delete(true);
	    }
	}
	public class FileChangedEventArgs : EventArgs
	{
	    public WatcherChangeTypes ChangeType { get; }
	    public IFileSystem FileSystem { get; }
	    public UPath FullPath { get; }
	    public string Name { get; }
	    public FileChangedEventArgs(IFileSystem fileSystem, WatcherChangeTypes changeType, UPath fullPath)
	    {
	        if (fileSystem is null) throw new ArgumentNullException(nameof(fileSystem));
	        fullPath.AssertNotNull(nameof(fullPath));
	        fullPath.AssertAbsolute(nameof(fullPath));
	        FileSystem = fileSystem;
	        ChangeType = changeType;
	        FullPath = fullPath;
	        Name = fullPath.GetName();
	    }
	}
	public class FileEntry : FileSystemEntry
	{
	    public FileEntry(IFileSystem fileSystem, UPath path) : base(fileSystem, path)
	    {
	    }
	    public DirectoryEntry Directory => Parent!;
	    public bool IsReadOnly => (FileSystem.GetAttributes(Path) & FileAttributes.ReadOnly) != 0;
	    public long Length => FileSystem.GetFileLength(Path);
	    public FileEntry CopyTo(UPath destFileName, bool overwrite)
	    {
	        FileSystem.CopyFile(Path, destFileName, overwrite);
	        return new FileEntry(FileSystem, destFileName);
	    }
	    public FileEntry CopyTo(FileEntry destFile, bool overwrite)
	    {
	        if (destFile is null) throw new ArgumentNullException(nameof(destFile));
	        FileSystem.CopyFileCross(Path, destFile.FileSystem, destFile.Path, overwrite);
	        return destFile;
	    }
	    public Stream Create()
	    {
	        return FileSystem.CreateFile(Path);
	    }
	    public void MoveTo(UPath destFileName)
	    {
	        FileSystem.MoveFile(Path, destFileName);
	    }
	    public Stream Open(FileMode mode, FileAccess access, FileShare share = FileShare.None)
	    {
	        return FileSystem.OpenFile(Path, mode, access, share);
	    }
	    public void ReplaceTo(UPath destPath, UPath destBackupPath, bool ignoreMetadataErrors)
	    {
	        FileSystem.ReplaceFile(Path, destPath, destBackupPath, ignoreMetadataErrors);
	    }
	    public string ReadAllText()
	    {
	        return FileSystem.ReadAllText(Path);
	    }
	    public string ReadAllText(Encoding encoding)
	    {
	        return FileSystem.ReadAllText(Path, encoding);
	    }
	    public void WriteAllText(string content)
	    {
	        FileSystem.WriteAllText(Path, content);
	    }
	    public void WriteAllText(string content, Encoding encoding)
	    {
	        FileSystem.WriteAllText(Path, content, encoding);
	    }
	    public void AppendAllText(string content)
	    {
	        FileSystem.AppendAllText(Path, content);
	    }
	    public void AppendAllText(string content, Encoding encoding)
	    {
	        FileSystem.AppendAllText(Path, content, encoding);
	    }
	    public string[] ReadAllLines()
	    {
	        return FileSystem.ReadAllLines(Path);
	    }
	    public string[] ReadAllLines(Encoding encoding)
	    {
	        return FileSystem.ReadAllLines(Path, encoding);
	    }
	    public byte[] ReadAllBytes()
	    {
	        return FileSystem.ReadAllBytes(Path);
	    }
	    public void WriteAllBytes(byte[] content)
	    {
	        FileSystem.WriteAllBytes(Path, content);
	    }
	    public override bool Exists => FileSystem.FileExists(Path);
	    public override void Delete()
	    {
	        FileSystem.DeleteFile(Path);
	    }
	}
	public class FileRenamedEventArgs : FileChangedEventArgs
	{
	    public UPath OldFullPath { get; }
	    public string OldName { get; }
	    public FileRenamedEventArgs(IFileSystem fileSystem, WatcherChangeTypes changeType, UPath fullPath, UPath oldFullPath)
	        : base(fileSystem, changeType, fullPath)
	    {
	        fullPath.AssertNotNull(nameof(oldFullPath));
	        fullPath.AssertAbsolute(nameof(oldFullPath));
	        OldFullPath = oldFullPath;
	        OldName = oldFullPath.GetName();
	    }
	}
	public abstract class FileSystemEntry : IEquatable<FileSystemEntry>
	{
	    protected FileSystemEntry(IFileSystem fileSystem, UPath path)
	    {
	        FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
	        path.AssertAbsolute();
	        Path = path;
	    }
	    public UPath Path { get; }
	    public IFileSystem FileSystem { get; }
	    public string FullName => Path.FullName;
	    public string Name => Path.GetName();
	    public string NameWithoutExtension => Path.GetNameWithoutExtension()!;
	    public string? ExtensionWithDot => Path.GetExtensionWithDot();
	    public FileAttributes Attributes
	    {
	        get => FileSystem.GetAttributes(Path);
	        set => FileSystem.SetAttributes(Path, value);
	    }
	    public abstract bool Exists { get; }
	    public DateTime CreationTime
	    {
	        get => FileSystem.GetCreationTime(Path);
	        set => FileSystem.SetCreationTime(Path, value);
	    }
	    public DateTime LastAccessTime
	    {
	        get => FileSystem.GetLastAccessTime(Path);
	        set => FileSystem.SetLastAccessTime(Path, value);
	    }
	    public DateTime LastWriteTime
	    {
	        get => FileSystem.GetLastWriteTime(Path);
	        set => FileSystem.SetLastWriteTime(Path, value);
	    }
	    public DirectoryEntry? Parent => Path == UPath.Root ? null : new DirectoryEntry(FileSystem, Path / "..");
	    public abstract void Delete();
	    public override string ToString()
	    {
	        return Path.FullName;
	    }
	    public bool Equals(FileSystemEntry? other)
	    {
	        if (other is null) return false;
	        if (ReferenceEquals(this, other)) return true;
	        return Path.Equals(other.Path) && FileSystem.Equals(other.FileSystem);
	    }
	    public override bool Equals(object? obj)
	    {
	        if (obj is null) return false;
	        if (ReferenceEquals(this, obj)) return true;
	        if (obj.GetType() != this.GetType()) return false;
	        return Equals((FileSystemEntry) obj);
	    }
	    public override int GetHashCode()
	    {
	        unchecked
	        {
	            return (Path.GetHashCode() * 397) ^ FileSystem.GetHashCode();
	        }
	    }
	    public static bool operator ==(FileSystemEntry left, FileSystemEntry right)
	    {
	        return Equals(left, right);
	    }
	    public static bool operator !=(FileSystemEntry left, FileSystemEntry right)
	    {
	        return !Equals(left, right);
	    }
	}
	public class FileSystemErrorEventArgs : EventArgs
	{
	    public Exception Exception { get; }
	    public FileSystemErrorEventArgs(Exception exception)
	    {
	        if (exception is null)
	        {
	            throw new ArgumentNullException(nameof(exception));
	        }
	        Exception = exception;
	    }
	}
	internal static class FileSystemExceptionHelper
	{
	    public static FileNotFoundException NewFileNotFoundException(UPath path)
	    {
	        return new FileNotFoundException($"Could not find file `{path}`.");
	    }
	    public static DirectoryNotFoundException NewDirectoryNotFoundException(UPath path)
	    {
	        return new DirectoryNotFoundException($"Could not find a part of the path `{path}`.");
	    }
	    public static IOException NewDestinationDirectoryExistException(UPath path)
	    {
	        return new IOException($"The destination path `{path}` is an existing directory");
	    }
	    public static IOException NewDestinationFileExistException(UPath path)
	    {
	        return new IOException($"The destination path `{path}` is an existing file");
	    }
	}
	public static class FileSystemExtensions
	{
	    public static SubFileSystem GetOrCreateSubFileSystem(this IFileSystem fs, UPath subFolder)
	    {
	        if (!fs.DirectoryExists(subFolder))
	        {
	            fs.CreateDirectory(subFolder);
	        }
	        return new SubFileSystem(fs, subFolder);
	    }
	    public static void CopyTo(this IFileSystem fs, IFileSystem destFileSystem, UPath dstFolder, bool overwrite)
	    {
	        CopyTo(fs, destFileSystem, dstFolder, overwrite, true);
	    }
	    public static void CopyTo(this IFileSystem fs, IFileSystem destFileSystem, UPath dstFolder, bool overwrite, bool copyAttributes)
	    {
	        if (destFileSystem is null) throw new ArgumentNullException(nameof(destFileSystem));
	        CopyDirectory(fs, UPath.Root, destFileSystem, dstFolder, overwrite, copyAttributes);
	    }
	    public static void CopyDirectory(this IFileSystem fs, UPath srcFolder, IFileSystem destFileSystem, UPath dstFolder, bool overwrite)
	    {
	        CopyDirectory(fs, srcFolder, destFileSystem, dstFolder, overwrite, true);
	    }
	    public static void CopyDirectory(this IFileSystem fs, UPath srcFolder, IFileSystem destFileSystem, UPath dstFolder, bool overwrite, bool copyAttributes)
	    {
	        if (destFileSystem is null) throw new ArgumentNullException(nameof(destFileSystem));
	        if (!fs.DirectoryExists(srcFolder)) throw new DirectoryNotFoundException($"{srcFolder} folder not found from source file system.");
	        if (dstFolder != UPath.Root)
	        {
	            destFileSystem.CreateDirectory(dstFolder);
	        }
	        var srcFolderFullName = srcFolder.FullName;
	        int deltaSubString = srcFolder == UPath.Root ? 0 : 1;
	        // Copy the files for the folder
	        foreach (var file in fs.EnumerateFiles(srcFolder))
	        {
	            var relativeFile = file.FullName.Substring(srcFolderFullName.Length + deltaSubString);
	            var destFile = UPath.Combine(dstFolder, relativeFile);
	            fs.CopyFileCross(file, destFileSystem, destFile, overwrite, copyAttributes);
	        }
	        // Then copy the folder structure recursively
	        foreach (var srcSubFolder in fs.EnumerateDirectories(srcFolder))
	        {
	            var relativeDestFolder = srcSubFolder.FullName.Substring(srcFolderFullName.Length + deltaSubString);
	            var destSubFolder = UPath.Combine(dstFolder, relativeDestFolder);
	            CopyDirectory(fs, srcSubFolder, destFileSystem, destSubFolder, overwrite, copyAttributes);
	        }
	    }
	    public static void CopyFileCross(this IFileSystem fs, UPath srcPath, IFileSystem destFileSystem, UPath destPath, bool overwrite)
	    {
	        CopyFileCross(fs, srcPath, destFileSystem, destPath, overwrite, true);
	    }
	    public static void CopyFileCross(this IFileSystem fs, UPath srcPath, IFileSystem destFileSystem, UPath destPath, bool overwrite, bool copyAttributes)
	    {
	        if (destFileSystem is null) throw new ArgumentNullException(nameof(destFileSystem));
	        (fs, srcPath) = fs.ResolvePath(srcPath);
	        (destFileSystem, destPath) = destFileSystem.ResolvePath(destPath);
	        // If this is the same filesystem, use the file system directly to perform the action
	        if (fs == destFileSystem)
	        {
	            fs.CopyFile(srcPath, destPath, overwrite);
	            return;
	        }
	        srcPath.AssertAbsolute(nameof(srcPath));
	        if (!fs.FileExists(srcPath))
	        {
	            throw NewFileNotFoundException(srcPath);
	        }
	        destPath.AssertAbsolute(nameof(destPath));
	        var destDirectory = destPath.GetDirectory();
	        if (!destFileSystem.DirectoryExists(destDirectory))
	        {
	            throw NewDirectoryNotFoundException(destDirectory);
	        }
	        if (destFileSystem.FileExists(destPath) && !overwrite)
	        {
	            throw new IOException($"The destination file path `{destPath}` already exist and overwrite is false");
	        }
	        using (var sourceStream = fs.OpenFile(srcPath, FileMode.Open, FileAccess.Read, FileShare.Read))
	        {
	            var copied = false;
	            try
	            {
	                using (var destStream = destFileSystem.OpenFile(destPath, FileMode.Create, FileAccess.Write, FileShare.Read))
	                {
	                    sourceStream.CopyTo(destStream);
	                }
	                if (copyAttributes)
	                {
	                    // NOTE: For some reasons, we can sometimes get an Unauthorized access if we try to set the LastWriteTime after the SetAttributes
	                    // So we setup it here.
	                    destFileSystem.SetLastWriteTime(destPath, fs.GetLastWriteTime(srcPath));
	                    // Preserve attributes and LastWriteTime as a regular File.Copy
	                    destFileSystem.SetAttributes(destPath, fs.GetAttributes(srcPath));
	                }
	                copied = true;
	            }
	            finally
	            {
	                if (!copied)
	                {
	                    try
	                    {
	                        destFileSystem.DeleteFile(destPath);
	                    }
	                    catch
	                    {
	                        // ignored
	                    }
	                }
	            }
	        }
	    }
	    public static void MoveFileCross(this IFileSystem fs, UPath srcPath, IFileSystem destFileSystem, UPath destPath)
	    {
	        if (destFileSystem is null) throw new ArgumentNullException(nameof(destFileSystem));
	        (fs, srcPath) = fs.ResolvePath(srcPath);
	        (destFileSystem, destPath) = destFileSystem.ResolvePath(destPath);
	        // If this is the same filesystem, use the file system directly to perform the action
	        if (fs == destFileSystem)
	        {
	            fs.MoveFile(srcPath, destPath);
	            return;
	        }
	        // Check source
	        srcPath.AssertAbsolute(nameof(srcPath));
	        if (!fs.FileExists(srcPath))
	        {
	            throw NewFileNotFoundException(srcPath);
	        }
	        // Check destination
	        destPath.AssertAbsolute(nameof(destPath));
	        var destDirectory = destPath.GetDirectory();
	        if (!destFileSystem.DirectoryExists(destDirectory))
	        {
	            throw NewDirectoryNotFoundException(destPath);
	        }
	        if (destFileSystem.DirectoryExists(destPath))
	        {
	            throw NewDestinationDirectoryExistException(destPath);
	        }
	        if (destFileSystem.FileExists(destPath))
	        {
	            throw NewDestinationFileExistException(destPath);
	        }
	        using (var sourceStream = fs.OpenFile(srcPath, FileMode.Open, FileAccess.Read, FileShare.Read))
	        {
	            var copied = false;
	            try
	            {
	                using (var destStream = destFileSystem.OpenFile(destPath, FileMode.Create, FileAccess.Write, FileShare.Read))
	                {
	                    sourceStream.CopyTo(destStream);
	                }
	                // Preserve all attributes and times
	                destFileSystem.SetAttributes(destPath, fs.GetAttributes(srcPath));
	                destFileSystem.SetCreationTime(destPath, fs.GetCreationTime(srcPath));
	                destFileSystem.SetLastAccessTime(destPath, fs.GetLastAccessTime(srcPath));
	                destFileSystem.SetLastWriteTime(destPath, fs.GetLastWriteTime(srcPath));
	                copied = true;
	            }
	            finally
	            {
	                if (!copied)
	                {
	                    try
	                    {
	                        destFileSystem.DeleteFile(destPath);
	                    }
	                    catch
	                    {
	                        // ignored
	                    }
	                }
	            }
	        }
	        var deleted = false;
	        try
	        {
	            fs.DeleteFile(srcPath);
	            deleted = true;
	        }
	        finally
	        {
	            if (!deleted)
	            {
	                try
	                {
	                    destFileSystem.DeleteFile(destPath);
	                }
	                catch
	                {
	                    // ignored
	                }
	            }
	        }
	    }
	    public static byte[] ReadAllBytes(this IFileSystem fs, UPath path)
	    {
	        var memstream = new MemoryStream();
	        using (var stream = fs.OpenFile(path, FileMode.Open, FileAccess.Read, FileShare.Read))
	        {
	            stream.CopyTo(memstream);
	        }
	        return memstream.ToArray();
	    }
	    public static string ReadAllText(this IFileSystem fs, UPath path)
	    {
	        var stream = fs.OpenFile(path, FileMode.Open, FileAccess.Read, FileShare.Read);
	        using (var reader = new StreamReader(stream))
	        {
	            return reader.ReadToEnd();
	        }
	    }
	    public static string ReadAllText(this IFileSystem fs, UPath path, Encoding encoding)
	    {
	        if (encoding is null) throw new ArgumentNullException(nameof(encoding));
	        var stream = fs.OpenFile(path, FileMode.Open, FileAccess.Read, FileShare.Read);
	        using (var reader = new StreamReader(stream, encoding))
	        {
	            return reader.ReadToEnd();
	        }
	    }
	    public static void WriteAllBytes(this IFileSystem fs, UPath path, byte[] content)
	    {
	        if (content is null) throw new ArgumentNullException(nameof(content));
	        using (var stream = fs.OpenFile(path, FileMode.Create, FileAccess.Write, FileShare.Read))
	        {
	            stream.Write(content, 0, content.Length);
	        }
	    }
	    public static string[] ReadAllLines(this IFileSystem fs, UPath path)
	    {
	        var stream = fs.OpenFile(path, FileMode.Open, FileAccess.Read, FileShare.Read);
	        {
	            using (var reader = new StreamReader(stream))
	            {
	                var lines = new List<string>();
	                string? line;
	                while ((line = reader.ReadLine()) != null)
	                {
	                    lines.Add(line);
	                }
	                return lines.ToArray();
	            }
	        }
	    }
	    public static string[] ReadAllLines(this IFileSystem fs, UPath path, Encoding encoding)
	    {
	        if (encoding is null) throw new ArgumentNullException(nameof(encoding));
	        var stream = fs.OpenFile(path, FileMode.Open, FileAccess.Read, FileShare.Read);
	        {
	            using (var reader = new StreamReader(stream, encoding))
	            {
	                var lines = new List<string>();
	                string? line;
	                while ((line = reader.ReadLine()) != null)
	                {
	                    lines.Add(line);
	                }
	                return lines.ToArray();
	            }
	        }
	    }
	    public static void WriteAllText(this IFileSystem fs, UPath path, string content)
	    {
	        if (content is null) throw new ArgumentNullException(nameof(content));
	        var stream = fs.OpenFile(path, FileMode.Create, FileAccess.Write, FileShare.Read);
	        {
	            using (var writer = new StreamWriter(stream))
	            {
	                writer.Write(content);
	                writer.Flush();
	            }
	        }
	    }
	    public static void WriteAllText(this IFileSystem fs, UPath path, string content, Encoding encoding)
	    {
	        if (content is null) throw new ArgumentNullException(nameof(content));
	        if (encoding is null) throw new ArgumentNullException(nameof(encoding));
	        var stream = fs.OpenFile(path, FileMode.Create, FileAccess.Write, FileShare.Read);
	        {
	            using (var writer = new StreamWriter(stream, encoding))
	            {
	                writer.Write(content);
	                writer.Flush();
	            }
	        }
	    }
	    public static void AppendAllText(this IFileSystem fs, UPath path, string content)
	    {
	        if (content is null) throw new ArgumentNullException(nameof(content));
	        var stream = fs.OpenFile(path, FileMode.Append, FileAccess.Write, FileShare.Read);
	        {
	            using (var writer = new StreamWriter(stream))
	            {
	                writer.Write(content);
	                writer.Flush();
	            }
	        }
	    }
	    public static void AppendAllText(this IFileSystem fs, UPath path, string content, Encoding encoding)
	    {
	        if (content is null) throw new ArgumentNullException(nameof(content));
	        if (encoding is null) throw new ArgumentNullException(nameof(encoding));
	        var stream = fs.OpenFile(path, FileMode.Append, FileAccess.Write, FileShare.Read);
	        {
	            using (var writer = new StreamWriter(stream, encoding))
	            {
	                writer.Write(content);
	                writer.Flush();
	            }
	        }
	    }
	    public static Stream CreateFile(this IFileSystem fileSystem, UPath path)
	    {
	        path.AssertAbsolute();
	        return fileSystem.OpenFile(path, FileMode.Create, FileAccess.ReadWrite);
	    }
	    public static IEnumerable<UPath> EnumerateDirectories(this IFileSystem fileSystem, UPath path)
	    {
	        return EnumerateDirectories(fileSystem, path, "*");
	    }
	    public static IEnumerable<UPath> EnumerateDirectories(this IFileSystem fileSystem, UPath path, string searchPattern)
	    {
	        if (searchPattern is null) throw new ArgumentNullException(nameof(searchPattern));
	        return EnumerateDirectories(fileSystem, path, searchPattern, SearchOption.TopDirectoryOnly);
	    }
	    public static IEnumerable<UPath> EnumerateDirectories(this IFileSystem fileSystem, UPath path, string searchPattern, SearchOption searchOption)
	    {
	        if (searchPattern is null) throw new ArgumentNullException(nameof(searchPattern));
	        return fileSystem.EnumeratePaths(path, searchPattern, searchOption, SearchTarget.Directory);
	    }
	    public static IEnumerable<UPath> EnumerateFiles(this IFileSystem fileSystem, UPath path)
	    {
	        return EnumerateFiles(fileSystem, path, "*");
	    }
	    public static IEnumerable<UPath> EnumerateFiles(this IFileSystem fileSystem, UPath path, string searchPattern)
	    {
	        if (searchPattern is null) throw new ArgumentNullException(nameof(searchPattern));
	        return EnumerateFiles(fileSystem, path, searchPattern, SearchOption.TopDirectoryOnly);
	    }
	    public static IEnumerable<UPath> EnumerateFiles(this IFileSystem fileSystem, UPath path, string searchPattern, SearchOption searchOption)
	    {
	        if (searchPattern is null) throw new ArgumentNullException(nameof(searchPattern));
	        return fileSystem.EnumeratePaths(path, searchPattern, searchOption, SearchTarget.File);
	    }
	    public static IEnumerable<UPath> EnumeratePaths(this IFileSystem fileSystem, UPath path)
	    {
	        return EnumeratePaths(fileSystem, path, "*");
	    }
	    public static IEnumerable<UPath> EnumeratePaths(this IFileSystem fileSystem, UPath path, string searchPattern)
	    {
	        if (searchPattern is null) throw new ArgumentNullException(nameof(searchPattern));
	        return EnumeratePaths(fileSystem, path, searchPattern, SearchOption.TopDirectoryOnly);
	    }
	    public static IEnumerable<UPath> EnumeratePaths(this IFileSystem fileSystem, UPath path, string searchPattern, SearchOption searchOption)
	    {
	        if (searchPattern is null) throw new ArgumentNullException(nameof(searchPattern));
	        return fileSystem.EnumeratePaths(path, searchPattern, searchOption, SearchTarget.Both);
	    }
	    public static IEnumerable<FileEntry> EnumerateFileEntries(this IFileSystem fileSystem, UPath path)
	    {
	        return EnumerateFileEntries(fileSystem, path, "*");
	    }
	    public static IEnumerable<FileEntry> EnumerateFileEntries(this IFileSystem fileSystem, UPath path, string searchPattern)
	    {
	        if (searchPattern is null) throw new ArgumentNullException(nameof(searchPattern));
	        return EnumerateFileEntries(fileSystem, path, searchPattern, SearchOption.TopDirectoryOnly);
	    }
	    public static IEnumerable<FileEntry> EnumerateFileEntries(this IFileSystem fileSystem, UPath path, string searchPattern, SearchOption searchOption)
	    {
	        if (searchPattern is null) throw new ArgumentNullException(nameof(searchPattern));
	        foreach (var subPath in EnumerateFiles(fileSystem, path, searchPattern, searchOption))
	        {
	            yield return new FileEntry(fileSystem, subPath);
	        }
	    }
	    public static IEnumerable<DirectoryEntry> EnumerateDirectoryEntries(this IFileSystem fileSystem, UPath path)
	    {
	        return EnumerateDirectoryEntries(fileSystem, path, "*");
	    }
	    public static IEnumerable<DirectoryEntry> EnumerateDirectoryEntries(this IFileSystem fileSystem, UPath path, string searchPattern)
	    {
	        if (searchPattern is null) throw new ArgumentNullException(nameof(searchPattern));
	        return EnumerateDirectoryEntries(fileSystem, path, searchPattern, SearchOption.TopDirectoryOnly);
	    }
	    public static IEnumerable<DirectoryEntry> EnumerateDirectoryEntries(this IFileSystem fileSystem, UPath path, string searchPattern, SearchOption searchOption)
	    {
	        if (searchPattern is null) throw new ArgumentNullException(nameof(searchPattern));
	        foreach (var subPath in EnumerateDirectories(fileSystem, path, searchPattern, searchOption))
	        {
	            yield return new DirectoryEntry(fileSystem, subPath);
	        }
	    }
	    public static IEnumerable<FileSystemEntry> EnumerateFileSystemEntries(this IFileSystem fileSystem, UPath path)
	    {
	        return EnumerateFileSystemEntries(fileSystem, path, "*");
	    }
	    public static IEnumerable<FileSystemEntry> EnumerateFileSystemEntries(this IFileSystem fileSystem, UPath path, string searchPattern)
	    {
	        if (searchPattern is null) throw new ArgumentNullException(nameof(searchPattern));
	        return EnumerateFileSystemEntries(fileSystem, path, searchPattern, SearchOption.TopDirectoryOnly);
	    }
	    public static IEnumerable<FileSystemEntry> EnumerateFileSystemEntries(this IFileSystem fileSystem, UPath path, string searchPattern, SearchOption searchOption, SearchTarget searchTarget = SearchTarget.Both)
	    {
	        if (searchPattern is null) throw new ArgumentNullException(nameof(searchPattern));
	        foreach (var subPath in fileSystem.EnumeratePaths(path, searchPattern, searchOption, searchTarget))
	        {
	            yield return fileSystem.DirectoryExists(subPath) ? (FileSystemEntry) new DirectoryEntry(fileSystem, subPath) : new FileEntry(fileSystem, subPath);
	        }
	    }
	    public static FileSystemEntry GetFileSystemEntry(this IFileSystem fileSystem, UPath path)
	    {
	        var fileExists = fileSystem.FileExists(path);
	        if (fileExists)
	        {
	            return new FileEntry(fileSystem, path);
	        }
	        var directoryExists = fileSystem.DirectoryExists(path);
	        if (directoryExists)
	        {
	            return new DirectoryEntry(fileSystem, path);
	        }
	        throw NewFileNotFoundException(path);
	    }
	    public static FileSystemEntry? TryGetFileSystemEntry(this IFileSystem fileSystem, UPath path)
	    {
	        var fileExists = fileSystem.FileExists(path);
	        if (fileExists)
	        {
	            return new FileEntry(fileSystem, path);
	        }
	        var directoryExists = fileSystem.DirectoryExists(path);
	        if (directoryExists)
	        {
	            return new DirectoryEntry(fileSystem, path);
	        }
	        return null;
	    }
	    public static FileEntry GetFileEntry(this IFileSystem fileSystem, UPath filePath)
	    {
	        if (!fileSystem.FileExists(filePath))
	        {
	            throw NewFileNotFoundException(filePath);
	        }
	        return new FileEntry(fileSystem, filePath);
	    }
	    public static DirectoryEntry GetDirectoryEntry(this IFileSystem fileSystem, UPath directoryPath)
	    {
	        if (!fileSystem.DirectoryExists(directoryPath))
	        {
	            throw NewDirectoryNotFoundException(directoryPath);
	        }
	        return new DirectoryEntry(fileSystem, directoryPath);
	    }
	    public static IFileSystemWatcher? TryWatch(this IFileSystem fileSystem, UPath path)
	    {
	        if (!fileSystem.CanWatch(path))
	        {
	            return null;
	        }
	        return fileSystem.Watch(path);
	    }
	}
	public struct FileSystemItem
	{
	    public FileSystemItem(IFileSystem fileSystem, UPath path, bool directory) : this()
	    {
	        FileSystem = fileSystem;
	        AbsolutePath = path;
	        Path = path;
	        Attributes = directory ? FileAttributes.Directory : FileAttributes.Normal;
	    }
	    public readonly bool IsEmpty => FileSystem == null;
	    public IFileSystem? FileSystem;
	    public UPath AbsolutePath { get; set; }
	    public readonly string FullName => Path.FullName;
	    public readonly string GetName() => Path.GetName();
	    public UPath Path;
	    public DateTimeOffset CreationTime;
	    public DateTimeOffset LastAccessTime;
	    public DateTimeOffset LastWriteTime;
	    public FileAttributes Attributes;
	    public long Length;
	    public readonly bool IsDirectory => (Attributes & FileAttributes.Directory) != 0;
	    public readonly bool IsHidden => (Attributes & FileAttributes.Hidden) != 0;
	    public readonly Stream Open(FileMode mode, FileAccess access, FileShare share = FileShare.None)
	    {
	        if (FileSystem == null) throw NewThrowNotInitialized();
	        return FileSystem.OpenFile(AbsolutePath, mode, access, share);
	    }
	    public readonly bool Exists() => FileSystem != null && (IsDirectory ? FileSystem.DirectoryExists(AbsolutePath) : FileSystem.FileExists(AbsolutePath));
	    public readonly string ReadAllText()
	    {
	        if (FileSystem == null) throw NewThrowNotInitialized();
	        return FileSystem.ReadAllText(AbsolutePath);
	    }
	    private readonly InvalidOperationException NewThrowNotInitialized()
	    {
	        throw new InvalidOperationException("This instance is not initialized");
	    }
	    public override string ToString()
	    {
	        return AbsolutePath.FullName;
	    }
	}
	public readonly struct FilterPattern
	{
	    private static readonly char[] SpecialChars = {'.', '*', '?'};
	    private readonly string? _exactMatch;
	    private readonly Regex? _regexMatch;
	    public static FilterPattern Parse(string filter)
	    {
	        return new FilterPattern(filter);
	    }
	    public bool Match(UPath path)
	    {
	        path.AssertNotNull();
	        var name = path.GetName();
	        // if _execMatch is null and _regexMatch is null, we have a * match
	        return _exactMatch != null ? _exactMatch == name : _regexMatch is null || _regexMatch.IsMatch(name);
	    }
	    public bool Match(string fileName)
	    {
	        if (fileName is null) throw new ArgumentNullException(nameof(fileName));
	        // if _execMatch is null and _regexMatch is null, we have a * match
	        return _exactMatch != null ? _exactMatch == fileName : _regexMatch is null || _regexMatch.IsMatch(fileName);
	    }
	    public FilterPattern(string filter)
	    {
	        if (filter is null)
	        {
	            throw new ArgumentNullException(nameof(filter));
	        }
	        if (filter.IndexOf(UPath.DirectorySeparator) >= 0)
	        {
	            throw new ArgumentException("Filter cannot contain directory parts.", nameof(filter));
	        }
	        _exactMatch = null;
	        _regexMatch = null;
	        // Optimized path, most common cases
	        if (filter is "" or "*" or "*.*")
	        {
	            return;
	        }
	        bool appendSpecialCaseForWildcardExt = false;
	        var startIndex = 0;
	        StringBuilder? builder = null;
	        try
	        {
	            int nextIndex;
	            while ((nextIndex = filter.IndexOfAny(SpecialChars, startIndex)) >= 0)
	            {
	                if (builder is null)
	                {
	                    builder = UPath.GetSharedStringBuilder();
	                    builder.Append("^");
	                }
	                var lengthToEscape = nextIndex - startIndex;
	                if (lengthToEscape > 0)
	                {
	                    var toEscape = Regex.Escape(filter.Substring(startIndex, lengthToEscape));
	                    builder.Append(toEscape);
	                }
	                var c = filter[nextIndex];
	                // special case for wildcard file extension to allow blank extensions as well
	                if (c == '.' && nextIndex == filter.Length - 2 && filter[nextIndex + 1] == '*')
	                {
	                    appendSpecialCaseForWildcardExt = true;
	                    break;
	                }
	                var regexPatternPart =
	                    c == '.' ? "\\." : c == '*' ? ".*?" : ".";
	                builder.Append(regexPatternPart);
	                startIndex = nextIndex + 1;
	            }
	            if (builder is null)
	            {
	                _exactMatch = filter;
	            }
	            else
	            {
	                if (appendSpecialCaseForWildcardExt)
	                {
	                    builder.Append("(\\.[^.]*)?");
	                }
	                else
	                {
	                    var length = filter.Length - startIndex;
	                    if (length > 0)
	                    {
	                        var toEscape = Regex.Escape(filter.Substring(startIndex, length));
	                        builder.Append(toEscape);
	                    }
	                }
	                builder.Append("$");
	                var regexPattern = builder.ToString();
	                _regexMatch = new Regex(regexPattern);
	            }
	        }
	        finally
	        {
	            if (builder != null)
	            {
	                builder.Length = 0;
	            }
	        }
	    }
	}
	public interface IFileSystem : IDisposable
	{
	    // ----------------------------------------------
	    // Directory API
	    // ----------------------------------------------
	    void CreateDirectory(UPath path);
	    bool DirectoryExists(UPath path);
	    void MoveDirectory(UPath srcPath, UPath destPath);
	    void DeleteDirectory(UPath path, bool isRecursive);
	    // ----------------------------------------------
	    // File API
	    // ----------------------------------------------
	    void CopyFile(UPath srcPath, UPath destPath, bool overwrite);
	    void ReplaceFile(UPath srcPath, UPath destPath, UPath destBackupPath, bool ignoreMetadataErrors);
	    long GetFileLength(UPath path);
	    bool FileExists(UPath path);
	    void MoveFile(UPath srcPath, UPath destPath);
	    void DeleteFile(UPath path);
	    Stream OpenFile(UPath path, FileMode mode, FileAccess access, FileShare share = FileShare.None);
	    // ----------------------------------------------
	    // Metadata API
	    // ----------------------------------------------
	    FileAttributes GetAttributes(UPath path);
	    void SetAttributes(UPath path, FileAttributes attributes);
	    DateTime GetCreationTime(UPath path);
	    void SetCreationTime(UPath path, DateTime time);
	    DateTime GetLastAccessTime(UPath path);
	    void SetLastAccessTime(UPath path, DateTime time);
	    DateTime GetLastWriteTime(UPath path);
	    void SetLastWriteTime(UPath path, DateTime time);
	    void CreateSymbolicLink(UPath path, UPath pathToTarget);
	    bool TryResolveLinkTarget(UPath linkPath, out UPath resolvedPath);
	    // ----------------------------------------------
	    // Search API
	    // ----------------------------------------------
	    IEnumerable<UPath> EnumeratePaths(UPath path, string searchPattern, SearchOption searchOption, SearchTarget searchTarget);
	    IEnumerable<FileSystemItem> EnumerateItems(UPath path, SearchOption searchOption, SearchPredicate? searchPredicate = null);
	    // ----------------------------------------------
	    // Watch API
	    // ----------------------------------------------
	    bool CanWatch(UPath path);
	    IFileSystemWatcher Watch(UPath path);
	    // ----------------------------------------------
	    // Path API
	    // ----------------------------------------------
	    string ConvertPathToInternal(UPath path);
	    UPath ConvertPathFromInternal(string systemPath);
	    (IFileSystem FileSystem, UPath Path) ResolvePath(UPath path);
	}
	public delegate bool SearchPredicate(ref FileSystemItem item);
	public interface IFileSystemWatcher : IDisposable
	{
	    event EventHandler<FileChangedEventArgs>? Changed;
	    event EventHandler<FileChangedEventArgs>? Created;
	    event EventHandler<FileChangedEventArgs>? Deleted;
	    event EventHandler<FileSystemErrorEventArgs>? Error;
	    event EventHandler<FileRenamedEventArgs>? Renamed;
	    IFileSystem FileSystem { get; }
	    UPath Path { get; }
	    int InternalBufferSize { get; set; }
	    NotifyFilters NotifyFilter { get; set; }
	    bool EnableRaisingEvents { get; set; }
	    string Filter { get; set; }
	    bool IncludeSubdirectories { get; set; }
	}
	internal static class Interop
	{
	    public static class Windows
	    {
	        private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
	        private const uint FILE_READ_EA = 0x0008;
	        private const uint FILE_FLAG_BACKUP_SEMANTICS = 0x2000000;
	        [DllImport("kernel32.dll", SetLastError = true)]
	        public static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, SymbolicLink dwFlags);
	        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
	        public static extern uint GetFinalPathNameByHandle(IntPtr hFile, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszFilePath, uint cchFilePath, uint dwFlags);
	        [DllImport("kernel32.dll", SetLastError = true)]
	        [return: MarshalAs(UnmanagedType.Bool)]
	        public static extern bool CloseHandle(IntPtr hObject);
	        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	        public static extern IntPtr CreateFile(
	            [MarshalAs(UnmanagedType.LPTStr)] string filename,
	            [MarshalAs(UnmanagedType.U4)] uint access,
	            [MarshalAs(UnmanagedType.U4)] FileShare share,
	            IntPtr securityAttributes,
	            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
	            [MarshalAs(UnmanagedType.U4)] uint flagsAndAttributes,
	            IntPtr templateFile);
	        public static string GetFinalPathName(string path)
	        {
	            var h = CreateFile(path,
	                FILE_READ_EA,
	                FileShare.ReadWrite | FileShare.Delete,
	                IntPtr.Zero,
	                FileMode.Open,
	                FILE_FLAG_BACKUP_SEMANTICS,
	                IntPtr.Zero);
	            if (h == INVALID_HANDLE_VALUE)
	            {
	                throw new Win32Exception();
	            }
	            try
	            {
	                var sb = new StringBuilder(1024);
	                var res = GetFinalPathNameByHandle(h, sb, 1024, 0);
	                if (res == 0)
	                {
	                    throw new Win32Exception();
	                }
	                // Trim '\\?\'
	                if (sb.Length >= 4 && sb[0] == '\\' && sb[1] == '\\' && sb[2] == '?' && sb[3] == '\\')
	                {
	                    sb.Remove(0, 4);
	                    // Trim 'UNC\'
	                    if (sb.Length >= 4 && sb[0] == 'U' && sb[1] == 'N' && sb[2] == 'C' && sb[3] == '\\')
	                    {
	                        sb.Remove(0, 4);
	                        // Add the default UNC prefix
	                        sb.Insert(0, @"\\");
	                    }
	                }
	                return sb.ToString();
	            }
	            finally
	            {
	                CloseHandle(h);
	            }
	        }
	        public enum SymbolicLink
	        {
	            File = 0,
	            Directory = 1
	        }
	    }
	    public static class Unix
	    {
	        [DllImport("libc", SetLastError = true)]
	        public static extern int symlink(string target, string linkpath);
	        [DllImport ("libc")]
	        private static extern int readlink (string path, byte[] buffer, int buflen);
	        public static string? readlink(string path)
	        {
	            var buf = new byte[1024];
	            var ret = readlink(path, buf, buf.Length);
	            return ret == -1 ? null : Encoding.Default.GetString(buf, 0, ret);
	        }
	    }
	}
	[Flags]
	public enum NotifyFilters
	{
	    FileName = 1,
	    DirectoryName = 2,
	    Attributes = 4,
	    Size = 8,
	    LastWrite = 16,
	    LastAccess = 32,
	    CreationTime = 64,
	    Security = 256,
	    Default = FileName | DirectoryName | LastWrite
	}
	public struct SearchPattern
	{
	    private static readonly char[] SpecialChars = {'?', '*'};
	    private readonly string? _exactMatch;
	    private readonly Regex? _regexMatch;
	    public bool Match(UPath path)
	    {
	        path.AssertNotNull();
	        var name = path.GetName();
	        // if _execMatch is null and _regexMatch is null, we have a * match
	        return _exactMatch != null ? _exactMatch == name : _regexMatch is null || _regexMatch.IsMatch(name);
	    }
	    public bool Match(string name)
	    {
	        if (name is null) throw new ArgumentNullException(nameof(name));
	        // if _execMatch is null and _regexMatch is null, we have a * match
	        return _exactMatch != null ? _exactMatch == name : _regexMatch is null || _regexMatch.IsMatch(name);
	    }
	    public static SearchPattern Parse(ref UPath path, ref string searchPattern)
	    {
	        return new SearchPattern(ref path, ref searchPattern);
	    }
	    public static void Normalize(ref UPath path, ref string searchPattern)
	    {
	        Parse(ref path, ref searchPattern);
	    }
	    private SearchPattern(ref UPath path, ref string searchPattern)
	    {
	        path.AssertAbsolute();
	        if (searchPattern is null) throw new ArgumentNullException(nameof(searchPattern));
	        _exactMatch = null;
	        _regexMatch = null;
	        // Optimized path, most common case
	        if (searchPattern is "*")
	        {
	            return;
	        }
	        if (searchPattern.StartsWith("/", StringComparison.Ordinal))
	        {
	            throw new ArgumentException($"The search pattern `{searchPattern}` cannot start by an absolute path `/`");
	        }
	        searchPattern = searchPattern.Replace('\\', '/');
	        // If the path contains any directory, we need to concatenate the directory part with the input path
	        if (searchPattern.IndexOf('/') > 0)
	        {
	            var pathPattern = new UPath(searchPattern);
	            var directory = pathPattern.GetDirectory();
	            if (!directory.IsNull && !directory.IsEmpty)
	            {
	                path = path / directory;
	            }
	            searchPattern = pathPattern.GetName();
	            // If the search pattern is again a plain any, optimized path
	            if (searchPattern is "*")
	            {
	                return;
	            }
	        }
	        int startIndex = 0;
	        int nextIndex;
	        StringBuilder? builder = null;
	        try
	        {
	            while ((nextIndex = searchPattern.IndexOfAny(SpecialChars, startIndex)) >= 0)
	            {
	                if (builder is null)
	                {
	                    builder = UPath.GetSharedStringBuilder();
	                    builder.Append("^");
	                }
	                var lengthToEscape = nextIndex - startIndex;
	                if (lengthToEscape > 0)
	                {
	                    var toEscape = Regex.Escape(searchPattern.Substring(startIndex, lengthToEscape));
	                    builder.Append(toEscape);
	                }
	                var c = searchPattern[nextIndex];
	                var regexPatternPart = c == '*' ? "[^/]*" : "[^/]";
	                builder.Append(regexPatternPart);
	                startIndex = nextIndex + 1;
	            }
	            if (builder is null)
	            {
	                _exactMatch = searchPattern;
	            }
	            else
	            {
	                var length = searchPattern.Length - startIndex;
	                if (length > 0)
	                {
	                    var toEscape = Regex.Escape(searchPattern.Substring(startIndex, length));
	                    builder.Append(toEscape);
	                }
	                builder.Append("$");
	                var regexPattern = builder.ToString();
	                _regexMatch = new Regex(regexPattern);
	            }
	        }
	        finally
	        {
	            if (builder != null)
	            {
	                builder.Length = 0;
	            }
	        }
	    }
	}
	public enum SearchTarget
	{
	    Both,
	    File,
	    Directory
	}
	public readonly struct UPath : IEquatable<UPath>, IComparable<UPath>
	{
	    [ThreadStatic] private static InternalHelper? _internalHelperTls;
	    public static readonly UPath Empty = new UPath(string.Empty, true);
	    public static readonly UPath Root = new UPath("/", true);
	    internal static readonly UPath Null = new UPath(null!);
	    public const char DirectorySeparator = '/';
	    private static InternalHelper InternalHelperTls => _internalHelperTls ??= new InternalHelper();
	    public static readonly IComparer<UPath> DefaultComparer = UPathComparer.Ordinal;
	    public static readonly IComparer<UPath> DefaultComparerIgnoreCase = UPathComparer.OrdinalIgnoreCase;
	    public UPath(string path) : this(path, false)
	    {
	    }
	    internal UPath(string path, bool safe)
	    {
	        if (safe)
	        {
	            FullName = path;
	        }
	        else
	        {
	            string? errorMessage;
	            FullName = ValidateAndNormalize(path, out errorMessage)!;
	            if (errorMessage != null)
	                throw new ArgumentException(errorMessage, nameof(path));
	        }
	    }
	    public string FullName { get; }
	    public bool IsNull => FullName is null;
	    public bool IsEmpty => this.FullName?.Length == 0;
	    public bool IsAbsolute => FullName is not null && FullName.Length > 0 && FullName[0] == '/';
	    public bool IsRelative => !IsAbsolute;
	    public static implicit operator UPath(string path)
	    {
	        return new UPath(path);
	    }
	    public static explicit operator string(UPath path)
	    {
	        return path.FullName;
	    }
	    public static UPath Combine(UPath path1, UPath path2)
	    {
	        if (TryGetAbsolute(path1, path2, out var result))
	        {
	            return result;
	        }
	        try
	        {
	#if NET7_0_OR_GREATER
	            return string.Create(path1.FullName.Length + path2.FullName.Length + 1, new KeyValuePair<UPath, UPath>(path1, path2), (span, state) =>
	            {
	                var (left, right) = state;
	                left.FullName.AsSpan().CopyTo(span);
	                span[left.FullName.Length] = '/';
	                right.FullName.AsSpan().CopyTo(span.Slice(left.FullName.Length + 1));
	            });
	#else
	            return new UPath($"{path1.FullName}/{path2.FullName}");
	#endif
	        }
	        catch (ArgumentException ex)
	        {
	            throw new ArgumentException($"Unable to combine path `{path1}` with `{path2}`", ex);
	        }
	    }
	    private static bool TryGetAbsolute(UPath path1, UPath path2, out UPath result)
	    {
	        if (path1.FullName is null)
	            throw new ArgumentNullException(nameof(path1));
	        if (path2.FullName is null)
	            throw new ArgumentNullException(nameof(path2));
	        // If the right path is absolute, it takes priority over path1
	        if (path1.IsEmpty || path2.IsAbsolute)
	        {
	            result = path2;
	            return true;
	        }
	        result = default;
	        return false;
	    }
	    public static UPath Combine(UPath path1, UPath path2, UPath path3)
	    {
	        if (TryGetAbsolute(path1, path2, out var result))
	        {
	            return Combine(result, path3);
	        }
	        if (TryGetAbsolute(path2, path3, out result))
	        {
	            return Combine(path1, result);
	        }
	#if NET7_0_OR_GREATER
	        return string.Create(path1.FullName.Length + path2.FullName.Length + path3.FullName.Length + 2, (path1, path2, path3), (span, state) =>
	        {
	            var (p1, p2, p3) = state;
	            var remaining = span;
	            p1.FullName.AsSpan().CopyTo(remaining);
	            remaining[p1.FullName.Length] = '/';
	            remaining = remaining.Slice(p1.FullName.Length + 1);
	            p2.FullName.AsSpan().CopyTo(remaining);
	            remaining[p2.FullName.Length] = '/';
	            remaining = remaining.Slice(p2.FullName.Length + 1);
	            p3.FullName.AsSpan().CopyTo(remaining);
	        });
	#else
	        return UPath.Combine(UPath.Combine(path1, path2), path3);
	#endif
	    }
	    public static UPath Combine(UPath path1, UPath path2, UPath path3, UPath path4)
	    {
	        if (TryGetAbsolute(path1, path2, out var result))
	        {
	            return Combine(result, path3, path4);
	        }
	        if (TryGetAbsolute(path2, path3, out result))
	        {
	            return Combine(path1, result, path4);
	        }
	        if (TryGetAbsolute(path3, path4, out result))
	        {
	            return Combine(path1, path2, result);
	        }
	#if NET7_0_OR_GREATER
	        return string.Create(path1.FullName.Length + path2.FullName.Length + path3.FullName.Length + path4.FullName.Length + 3, (path1, path2, path3, path4), (span, state) =>
	        {
	            var (p1, p2, p3, p4) = state;
	            var remaining = span;
	            p1.FullName.AsSpan().CopyTo(remaining);
	            remaining[p1.FullName.Length] = '/';
	            remaining = remaining.Slice(p1.FullName.Length + 1);
	            p2.FullName.AsSpan().CopyTo(remaining);
	            remaining[p2.FullName.Length] = '/';
	            remaining = remaining.Slice(p2.FullName.Length + 1);
	            p3.FullName.AsSpan().CopyTo(remaining);
	            remaining[p3.FullName.Length] = '/';
	            remaining = remaining.Slice(p3.FullName.Length + 1);
	            p4.FullName.AsSpan().CopyTo(remaining);
	        });
	#else
	        return UPath.Combine(UPath.Combine(path1, path2), UPath.Combine(path3, path4));
	#endif
	    }
	    public static UPath Combine(params UPath[] paths)
	    {
	        var path = paths[0];
	        for (var i = 1; i < paths.Length; i++)
	            path = Combine(path, paths[i]);
	        return path;
	    }
	    public static UPath operator /(UPath path1, UPath path2)
	    {
	        return Combine(path1, path2);
	    }
	    public bool Equals(UPath other)
	    {
	        return string.Equals(FullName, other.FullName);
	    }
	    public override bool Equals(object? obj)
	    {
	        return obj is UPath path && Equals(path);
	    }
	    public override int GetHashCode()
	    {
	        return FullName?.GetHashCode() ?? 0;
	    }
	    public static bool operator ==(UPath left, UPath right)
	    {
	        return left.Equals(right);
	    }
	    public static bool operator !=(UPath left, UPath right)
	    {
	        return !left.Equals(right);
	    }
	    public override string ToString()
	    {
	        return FullName;
	    }
	    public static bool TryParse(string path, out UPath pathInfo)
	    {
	        string? errorMessage;
	        path = ValidateAndNormalize(path, out errorMessage)!;
	        pathInfo = errorMessage is null ? new UPath(path!, true) : new UPath();
	        return errorMessage is null;
	    }
	    internal static StringBuilder GetSharedStringBuilder()
	    {
	        var builder = InternalHelperTls.Builder;
	        builder.Length = 0;
	        return builder;
	    }
	    private static string? ValidateAndNormalize(string path, out string? errorMessage)
	    {
	        errorMessage = null;
	        // Early exit
	        switch (path)
	        {
	            case null:
	                return null;
	            case "/":
	            case "..":
	            case ".":
	                return path;
	            case "\\":
	                return "/";
	        }
	        // Optimized path
	        var internalHelper = InternalHelperTls;
	        var parts = internalHelper.Slices;
	        parts.Clear();
	        var lastIndex = 0;
	        var i = 0;
	        var processParts = false;
	        var dotCount = 0;
	        for (; i < path.Length; i++)
	        {
	            var c = path[i];
	            // We don't disallow characters, as we let the IFileSystem implementations decided for them
	            // depending on the platform
	            //if (c < ' ' || c == ':' || c == '<' || c == '>' || c == '"' || c == '|')
	            //{
	            //    throw new InvalidUPathException($"The path `{path}` contains invalid characters `{c}`");
	            //}
	            if (c == '.')
	            {
	                dotCount++;
	            }
	            else if (c == DirectorySeparator || c == '\\')
	            {
	                // optimization: If we don't expect to process the path
	                // and we only have a trailing / or \\, then just perform
	                // a substring on the path
	                if (!processParts && i + 1 == path.Length)
	                    return path.Substring(0, path.Length - 1);
	                if (c == '\\')
	                    processParts = true;
	                var endIndex = i - 1;
	                for (i++; i < path.Length; i++)
	                {
	                    c = path[i];
	                    if (c == DirectorySeparator || c == '\\')
	                    {
	                        // If we have consecutive / or \\, we need to process parts
	                        processParts = true;
	                        continue;
	                    }
	                    break;
	                }
	                if (endIndex >= lastIndex || endIndex == -1)
	                {
	                    var part = new TextSlice(lastIndex, endIndex);
	                    parts.Add(part);
	                    if (dotCount > 0 &&                     // has dots
	                        dotCount == part.Length &&          // only dots
	                        (dotCount != 2 || parts.Count > 1)) // Skip ".." if it's the first part
	                        processParts = true;
	                }
	                dotCount = c == '.' ? 1 : 0;
	                lastIndex = i;
	            }
	        }
	        if (lastIndex < path.Length)
	        {
	            var part = new TextSlice(lastIndex, path.Length - 1);
	            parts.Add(part);
	            // If the previous part had only dots, we need to process it
	            if (part.Length == dotCount)
	                processParts = true;
	        }
	        // Optimized path if we don't need to compact the path
	        if (!processParts)
	            return path;
	        // Slow path, we need to process the parts
	        for (i = 0; i < parts.Count; i++)
	        {
	            var part = parts[i];
	            var partLength = part.Length;
	            if (partLength < 1)
	                continue;
	            if (path[part.Start] != '.')
	                continue;
	            if (partLength == 1)
	            {
	                // We have a '.'
	                if (parts.Count > 1)
	                    parts.RemoveAt(i--);
	            }
	            else
	            {
	                if (path[part.Start + 1] != '.')
	                    continue;
	                // Throws an exception if our slice parth contains only `.`  and is longer than 2 characters
	                if (partLength > 2)
	                {
	                    var isValid = false;
	                    for (var j = part.Start + 2; j <= part.End; j++)
	                    {
	                        if (path[j] != '.')
	                        {
	                            isValid = true;
	                            break;
	                        }
	                    }
	                    if (!isValid)
	                    {
	                        errorMessage = $"The path `{path}` contains invalid dots `{path.Substring(part.Start, part.Length)}` while only `.` or `..` are supported";
	                        return string.Empty;
	                    }
	                    // Otherwise, it is a valid path part
	                    continue;
	                }
	                if (i - 1 >= 0)
	                {
	                    var previousSlice = parts[i - 1];
	                    if (!IsDotDot(previousSlice, path))
	                    {
	                        if (previousSlice.Length == 0)
	                        {
	                            errorMessage = $"The path `{path}` cannot go to the parent (..) of a root path /";
	                            return string.Empty;
	                        }
	                        parts.RemoveAt(i--);
	                        parts.RemoveAt(i--);
	                    }
	                }
	            }
	        }
	        // If we have a single part and it is empty, it is a root
	        if (parts.Count == 1 && parts[0].Start == 0 && parts[0].End < 0)
	        {
	            return "/";
	        }
	        var builder = internalHelper.Builder;
	        builder.Length = 0;
	        for (i = 0; i < parts.Count; i++)
	        {
	            var slice = parts[i];
	            if (slice.Length > 0)
	                builder.Append(path, slice.Start, slice.Length);
	            if (i + 1 < parts.Count)
	                builder.Append('/');
	        }
	        return builder.ToString();
	    }
	    private static bool IsDotDot(TextSlice slice, string path)
	    {
	        if (slice.Length != 2)
	            return false;
	        return path[slice.Start] == '.' && path[slice.End] == '.';
	    }
	    private class InternalHelper
	    {
	        public readonly StringBuilder Builder;
	        public readonly List<TextSlice> Slices;
	        public InternalHelper()
	        {
	            Builder = new StringBuilder();
	            Slices = new List<TextSlice>();
	        }
	    }
	    private readonly struct TextSlice
	    {
	        public TextSlice(int start, int end)
	        {
	            Start = start;
	            End = end;
	        }
	        public readonly int Start;
	        public readonly int End;
	        public int Length => End - Start + 1;
	    }
	    public int CompareTo(UPath other)
	    {
	        return string.Compare(FullName, other.FullName, StringComparison.Ordinal);
	    }
	}
	public class UPathComparer : IComparer<UPath>, IEqualityComparer<UPath>
	{
	    public static readonly UPathComparer Ordinal = new(StringComparer.Ordinal);
	    public static readonly UPathComparer OrdinalIgnoreCase = new(StringComparer.OrdinalIgnoreCase);
	    public static readonly UPathComparer CurrentCulture = new(StringComparer.CurrentCulture);
	    public static readonly UPathComparer CurrentCultureIgnoreCase = new(StringComparer.CurrentCultureIgnoreCase);
	    private readonly StringComparer _comparer;
	    private UPathComparer(StringComparer comparer)
	    {
	        _comparer = comparer;
	    }
	    public int Compare(UPath x, UPath y)
	    {
	        return _comparer.Compare(x.FullName, y.FullName);
	    }
	    public bool Equals(UPath x, UPath y)
	    {
	        return _comparer.Equals(x.FullName, y.FullName);
	    }
	    public int GetHashCode(UPath obj)
	    {
	        return _comparer.GetHashCode(obj.FullName);
	    }
	}
	public static class UPathExtensions
	{
	    public static UPath ToRelative(this UPath path)
	    {
	        path.AssertNotNull();
	        if (path.IsRelative)
	        {
	            return path;
	        }
	        return path.FullName is "/" ? UPath.Empty : new UPath(path.FullName.Substring(1), true);
	    }
	    public static UPath ToAbsolute(this UPath path)
	    {
	        path.AssertNotNull();
	        if (path.IsAbsolute)
	        {
	            return path;
	        }
	        return path.IsEmpty ? UPath.Root : UPath.Root / path;
	    }
	    public static UPath GetDirectory(this UPath path)
	    {
	        path.AssertNotNull();
	        var fullname = path.FullName;
	        if (fullname is "/")
	        {
	            return new UPath();
	        }
	        var lastIndex = fullname.LastIndexOf(UPath.DirectorySeparator);
	        if (lastIndex > 0)
	        {
	            return new UPath(fullname.Substring(0, lastIndex), true);
	        }
	        return lastIndex == 0 ? UPath.Root : UPath.Empty;
	    }
	    public static string GetFirstDirectory(this UPath path, out UPath remainingPath)
	    {
	        path.AssertNotNull();
	        remainingPath = UPath.Empty;
	        string firstDirectory;
	        var fullname = path.FullName;
	        var offset = path.IsRelative ? 0 : 1;
	        var index = fullname.IndexOf(UPath.DirectorySeparator, offset);
	        if (index < 0)
	        {
	            firstDirectory = fullname.Substring(offset, fullname.Length - offset);
	        }
	        else
	        {
	            firstDirectory = fullname.Substring(offset, index - offset);
	            if (index + 1 < fullname.Length)
	            {
	                remainingPath = fullname.Substring(index + 1);
	            }
	        }
	        return firstDirectory;
	    }
	    public static List<string> Split(this UPath path)
	    {
	        path.AssertNotNull();
	        var fullname = path.FullName;
	        if (fullname == string.Empty)
	        {
	            return new List<string>();
	        }
	        var paths = new List<string>();
	        int previousIndex = path.IsAbsolute ? 1 : 0;
	        int nextIndex;
	        while ((nextIndex = fullname.IndexOf(UPath.DirectorySeparator, previousIndex)) >= 0)
	        {
	            if (nextIndex != 0)
	            {
	                paths.Add(fullname.Substring(previousIndex, nextIndex - previousIndex));
	            }
	            previousIndex = nextIndex + 1;
	        }
	        if (previousIndex < fullname.Length)
	        {
	            paths.Add(fullname.Substring(previousIndex, fullname.Length - previousIndex));
	        }
	        return paths;
	    }
	    public static string GetName(this UPath path)
	    {
	        return path.IsNull ? null! : Path.GetFileName(path.FullName);
	    }
	    public static string? GetNameWithoutExtension(this UPath path)
	    {
	        return path.IsNull ? null : Path.GetFileNameWithoutExtension(path.FullName);
	    }
	    public static string? GetExtensionWithDot(this UPath path)
	    {
	        return path.IsNull ? null : Path.GetExtension(path.FullName);
	    }
	    public static UPath ChangeExtension(this UPath path, string extension)
	    {
	        return new UPath(Path.ChangeExtension(path.FullName, extension));
	    }
	    public static bool IsInDirectory(this UPath path, UPath directory, bool recursive)
	    {
	        path.AssertNotNull();
	        directory.AssertNotNull(nameof(directory));
	        if (path.IsAbsolute != directory.IsAbsolute)
	        {
	            throw new ArgumentException("Cannot mix absolute and relative paths", nameof(directory));
	        }
	        var target = path.FullName;
	        var dir = directory.FullName;
	        if (target.Length < dir.Length || !target.StartsWith(dir, StringComparison.Ordinal))
	        {
	            return false;
	        }
	        if (target.Length == dir.Length)
	        {
	            // exact match due to the StartsWith above
	            // the directory parameter is interpreted as a directory so trailing separator isn't important
	            return true;
	        }
	        var dirHasTrailingSeparator = dir[dir.Length - 1] == UPath.DirectorySeparator;
	        if (!recursive)
	        {
	            // need to check if the directory part terminates 
	            var lastSeparatorInTarget = target.LastIndexOf(UPath.DirectorySeparator);
	            var expectedLastSeparator = dir.Length - (dirHasTrailingSeparator ? 1 : 0);
	            if (lastSeparatorInTarget != expectedLastSeparator)
	            {
	                return false;
	            }
	        }
	        if (!dirHasTrailingSeparator)
	        {
	            // directory is missing ending slash, check that target has it
	            return target.Length > dir.Length && target[dir.Length] == UPath.DirectorySeparator;
	        }
	        return true;
	    }
	    public static UPath AssertNotNull(this UPath path, string name = "path")
	    {
	        if (path.IsNull)
	            Throw(name);
	        return path;
	        static void Throw(string name) => throw new ArgumentNullException(name);
	    }
	    public static UPath AssertAbsolute(this UPath path, string name = "path")
	    {
	        if (!path.IsAbsolute)
	            Throw(path, name);
	        return path;
	        static void Throw(UPath path, string name)
	        {
	            // Assert not null first, as a not absolute path could also be null, and if so, an ArgumentNullException shall be thrown
	            path.AssertNotNull(name);
	            throw new ArgumentException($"Path `{path}` must be absolute", name);
	        }
	    }
	}
	[Flags]
	public enum WatcherChangeTypes
	{
	    Created = 1,
	    Deleted = 2,
	    Changed = 4,
	    Renamed = 8,
	    All = Created | Deleted | Changed | Renamed
	}
	#endregion
	#region \FileSystems
	[DebuggerDisplay("{" + nameof(DebuggerDisplay) + "(),nq} Count={_fileSystems.Count}")]
	[DebuggerTypeProxy(typeof(DebuggerProxy))]
	public class AggregateFileSystem : ReadOnlyFileSystem
	{
	    private readonly List<IFileSystem> _fileSystems;
	    private readonly List<Watcher> _watchers;
	    public AggregateFileSystem(bool owned = true) : this(null, owned)
	    {
	    }
	    public AggregateFileSystem(IFileSystem? fileSystem, bool owned = true) : base(fileSystem, owned)
	    {
	        _fileSystems = new List<IFileSystem>();
	        _watchers = new List<Watcher>();
	    }
	    protected override void Dispose(bool disposing)
	    {
	        base.Dispose(disposing);
	        if (!disposing)
	        {
	            return;
	        }
	        if (Owned)
	        {
	            foreach (var fs in _fileSystems)
	            {
	                fs.Dispose();
	            }
	        }
	        _fileSystems.Clear();
	        foreach (var watcher in _watchers)
	        {
	            watcher.Dispose();
	        }
	        _watchers.Clear();
	    }
	    public List<IFileSystem> GetFileSystems()
	    {
	        return new List<IFileSystem>(_fileSystems);
	    }
	    public void ClearFileSystems()
	    {
	        _fileSystems.Clear();
	        foreach (var watcher in _watchers)
	        {
	            watcher.Clear(Fallback);
	        }
	    }
	    public void SetFileSystems(IEnumerable<IFileSystem> fileSystems)
	    {
	        if (fileSystems is null) throw new ArgumentNullException(nameof(fileSystems));
	        _fileSystems.Clear();
	        foreach (var watcher in _watchers)
	        {
	            watcher.Clear(Fallback);
	        }
	        foreach (var fileSystem in fileSystems)
	        {
	            if (fileSystem is null) throw new ArgumentException("A null filesystem is invalid");
	            if (fileSystem == this) throw new ArgumentException("Cannot add this instance as an aggregate delegate of itself");
	            _fileSystems.Add(fileSystem);
	            foreach (var watcher in _watchers)
	            {
	                if (fileSystem.CanWatch(watcher.Path))
	                {
	                    var newWatcher = fileSystem.Watch(watcher.Path);
	                    watcher.Add(newWatcher);
	                }
	            }
	        }
	    }
	    public virtual void AddFileSystem(IFileSystem fs)
	    {
	        if (fs is null) throw new ArgumentNullException(nameof(fs));
	        if (fs == this) throw new ArgumentException("Cannot add this instance as an aggregate delegate of itself");
	        if (!_fileSystems.Contains(fs))
	        {
	            _fileSystems.Add(fs);
	            foreach (var watcher in _watchers)
	            {
	                if (fs.CanWatch(watcher.Path))
	                {
	                    var newWatcher = fs.Watch(watcher.Path);
	                    watcher.Add(newWatcher);
	                }
	            }
	        }
	        else
	        {
	            throw new ArgumentException("The filesystem is already added");
	        }
	    }
	    public virtual void RemoveFileSystem(IFileSystem fs)
	    {
	        if (fs is null) throw new ArgumentNullException(nameof(fs));
	        if (_fileSystems.Contains(fs))
	        {
	            _fileSystems.Remove(fs);
	            foreach (var watcher in _watchers)
	            {
	                watcher.RemoveFrom(fs);
	            }
	        }
	        else
	        {
	            throw new ArgumentException("FileSystem was not found", nameof(fs));
	        }
	    }
	    public FileSystemEntry? FindFirstFileSystemEntry(UPath path)
	    {
	        path.AssertAbsolute();
	        var entry  = TryGetPath(path);
	        if (!entry.HasValue) return null;
	        var pathItem = entry.Value;
	        return pathItem.IsFile ? (FileSystemEntry) new FileEntry(pathItem.FileSystem, pathItem.Path) : new DirectoryEntry(pathItem.FileSystem, pathItem.Path);
	    }
	    public List<FileSystemEntry> FindFileSystemEntries(UPath path)
	    {
	        path.AssertAbsolute();
	        var paths = new List<FileSystemPath>();
	        FindPaths(path, SearchTarget.Both, paths);
	        var result = new List<FileSystemEntry>(paths.Count);
	        if (paths.Count == 0)
	        {
	            return result;
	        }
	        var isFile = paths[0].IsFile;
	        foreach (var pathItem in paths)
	        {
	            if (pathItem.IsFile == isFile)
	            {
	                if (isFile)
	                {
	                    result.Add(new FileEntry(pathItem.FileSystem, pathItem.Path));
	                }
	                else
	                {
	                    result.Add(new DirectoryEntry(pathItem.FileSystem, pathItem.Path));
	                }
	            }
	        }
	        return result;
	    }
	    // ----------------------------------------------
	    // Directory API
	    // ----------------------------------------------
	    protected override bool DirectoryExistsImpl(UPath path)
	    {
	        var directory = TryGetDirectory(path);
	        return directory.HasValue;
	    }
	    // ----------------------------------------------
	    // File API
	    // ----------------------------------------------
	    protected override long GetFileLengthImpl(UPath path)
	    {
	        var entry = GetFile(path);
	        return entry.FileSystem.GetFileLength(path);
	    }
	    protected override bool FileExistsImpl(UPath path)
	    {
	        var entry = TryGetFile(path);
	        return entry.HasValue;
	    }
	    protected override Stream OpenFileImpl(UPath path, FileMode mode, FileAccess access, FileShare share = FileShare.None)
	    {
	        if (mode != FileMode.Open)
	        {
	            throw new IOException(FileSystemIsReadOnly);
	        }
	        if ((access & FileAccess.Write) != 0)
	        {
	            throw new IOException(FileSystemIsReadOnly);
	        }
	        var entry = GetFile(path);
	        return entry.FileSystem.OpenFile(path, mode, access, share);
	    }
	    // ----------------------------------------------
	    // Metadata API
	    // ----------------------------------------------
	    protected override FileAttributes GetAttributesImpl(UPath path)
	    {
	        var entry = GetPath(path);
	        var attributes = entry.FileSystem.GetAttributes(path);
	        return attributes == FileAttributes.Normal
	            ? FileAttributes.ReadOnly
	            : attributes | FileAttributes.ReadOnly;
	    }
	    protected override DateTime GetCreationTimeImpl(UPath path)
	    {
	        var entry = TryGetPath(path);
	        return entry.HasValue ? entry.Value.FileSystem.GetCreationTime(path) : DefaultFileTime;
	    }
	    protected override DateTime GetLastAccessTimeImpl(UPath path)
	    {
	        var entry = TryGetPath(path);
	        return entry.HasValue ? entry.Value.FileSystem.GetLastWriteTime(path) : DefaultFileTime;
	    }
	    protected override DateTime GetLastWriteTimeImpl(UPath path)
	    {
	        var entry = TryGetPath(path);
	        return entry.HasValue ? entry.Value.FileSystem.GetLastWriteTime(path) : DefaultFileTime;
	    }
	    // ----------------------------------------------
	    // Search API
	    // ----------------------------------------------
	    protected override IEnumerable<UPath> EnumeratePathsImpl(UPath path, string searchPattern, SearchOption searchOption, SearchTarget searchTarget)
	    {
	        SearchPattern.Parse( ref path, ref searchPattern );
	        var entries = new SortedSet<UPath>();
	        var fileSystems = new List<IFileSystem>();
	        if (Fallback != null)
	        {
	            fileSystems.Add(Fallback);
	        }
	        // Query all filesystems just once
	        fileSystems.AddRange(_fileSystems);
	        for (var i = fileSystems.Count - 1; i >= 0; i--)
	        {
	            var fileSystem = fileSystems[i];
	            if (!fileSystem.DirectoryExists( path ))
	                continue;
	            foreach (var item in fileSystem.EnumeratePaths( path, searchPattern, searchOption, searchTarget ) )
	            {
	                if (entries.Contains( item )) continue;
	                entries.Add(item);
	            }
	        }
	        // Return entries
	        foreach (var entry in entries)
	        {
	            yield return entry;
	        }
	    }
	    protected override IEnumerable<FileSystemItem> EnumerateItemsImpl(UPath path, SearchOption searchOption, SearchPredicate? searchPredicate)
	    {
	        var entries = new HashSet<UPath>();
	        for (var i = _fileSystems.Count - 1; i >= 0; i--)
	        {
	            var fileSystem = _fileSystems[i];
	            foreach (var item in fileSystem.EnumerateItems(path, searchOption, searchPredicate))
	            {
	                if (entries.Add(item.Path))
	                {
	                    yield return item;
	                }
	            }
	        }
	        var fallback = Fallback;
	        if (fallback != null)
	        {
	            foreach (var item in fallback.EnumerateItems(path, searchOption, searchPredicate))
	            {
	                if (entries.Add(item.Path))
	                {
	                    yield return item;
	                }
	            }
	        }
	    }
	    protected override UPath ConvertPathToDelegate(UPath path)
	    {
	        return path;
	    }
	    protected override UPath ConvertPathFromDelegate(UPath path)
	    {
	        return path;
	    }
	    // ----------------------------------------------
	    // Watch API
	    // ----------------------------------------------
	    protected override bool CanWatchImpl(UPath path)
	    {
	        // Always allow watching because a future filesystem can be added that matches this path.
	        return true;
	    }
	    protected override IFileSystemWatcher WatchImpl(UPath path)
	    {
	        var watcher = new Watcher(this, path);
	        if (Fallback != null && Fallback.CanWatch(path) && Fallback.DirectoryExists(path))
	        {
	            watcher.Add(Fallback.Watch(path));
	        }
	        foreach (var fs in _fileSystems)
	        {
	            if (fs.CanWatch(path) && fs.DirectoryExists(path))
	            {
	                watcher.Add(fs.Watch(path));
	            }
	        }
	        _watchers.Add(watcher);
	        return watcher;
	    }
	    private sealed class Watcher : AggregateFileSystemWatcher
	    {
	        private readonly AggregateFileSystem _fileSystem;
	        public Watcher(AggregateFileSystem fileSystem, UPath path)
	            : base(fileSystem, path)
	        {
	            _fileSystem = fileSystem;
	        }
	        protected override void Dispose(bool disposing)
	        {
	            base.Dispose(disposing);
	            if (disposing && !_fileSystem.IsDisposing)
	            {
	                _fileSystem._watchers.Remove(this);
	            }
	        }
	    }
	    // ----------------------------------------------
	    // Internals API
	    // Used to retrieve the correct paths
	    // from the list of registered filesystem.
	    // ----------------------------------------------
	    private FileSystemPath GetFile(UPath path)
	    {
	        var entry = TryGetFile(path);
	        if (!entry.HasValue)
	        {
	            throw NewFileNotFoundException(path);
	        }
	        return entry.Value;
	    }
	    private FileSystemPath? TryGetFile(UPath path)
	    {
	        for (var i = _fileSystems.Count - 1; i >= -1; i--)
	        {
	            var fileSystem = i < 0 ? Fallback : _fileSystems[i];
	            // Go through aggregates
	            if (fileSystem is AggregateFileSystem aggregate)
	            {
	                var result = aggregate.TryGetFile(path);
	                if (result is not null)
	                {
	                    return result;
	                }
	            }
	            else if (fileSystem != null)
	            {
	                if (fileSystem.FileExists(path))
	                {
	                    return new FileSystemPath(fileSystem, path, true);
	                }
	            }
	            else
	            {
	                break;
	            }
	        }
	        return null;
	    }
	    private FileSystemPath? TryGetDirectory(UPath path)
	    {
	        for (var i = _fileSystems.Count - 1; i >= -1; i--)
	        {
	            var fileSystem = i < 0 ? Fallback : _fileSystems[i];
	            // Go through aggregates
	            if (fileSystem is AggregateFileSystem aggregate)
	            {
	                var result = aggregate.TryGetDirectory(path);
	                if (result is not null)
	                {
	                    return result;
	                }
	            }
	            else if (fileSystem != null)
	            {
	                if (fileSystem.DirectoryExists(path))
	                {
	                    return new FileSystemPath(fileSystem, path, false);
	                }
	            }
	            else
	            {
	                break;
	            }
	        }
	        return null;
	    }
	    private FileSystemPath GetPath(UPath path)
	    {
	        var entry = TryGetPath(path);
	        if (!entry.HasValue)
	        {
	            throw NewFileNotFoundException(path);
	        }
	        return entry.Value;
	    }
	    private FileSystemPath? TryGetPath(UPath path, SearchTarget searchTarget = SearchTarget.Both)
	    {
	        if (searchTarget == SearchTarget.File)
	        {
	            return TryGetFile(path);
	        }
	        else if (searchTarget == SearchTarget.Directory)
	        {
	            return TryGetDirectory(path);
	        }
	        else
	        {
	            for (var i = _fileSystems.Count - 1; i >= -1; i--)
	            {
	                var fileSystem = i < 0 ? Fallback : _fileSystems[i];
	                if (fileSystem is null)
	                {
	                    break;
	                }
	                // Go through aggregates
	                if (fileSystem is AggregateFileSystem aggregate)
	                {
	                    var result = aggregate.TryGetPath(path, searchTarget);
	                    if (result is not null)
	                    {
	                        return result;
	                    }
	                }
	                else if (fileSystem.DirectoryExists(path))
	                {
	                    return new FileSystemPath(fileSystem, path, false);
	                }
	                else if (fileSystem.FileExists(path))
	                {
	                    return new FileSystemPath(fileSystem, path, true);
	                }
	            }
	        }
	        return null;
	    }
	    private void FindPaths(UPath path, SearchTarget searchTarget, List<FileSystemPath> paths)
	    {
	        bool queryDirectory = searchTarget == SearchTarget.Both || searchTarget == SearchTarget.Directory;
	        bool queryFile = searchTarget == SearchTarget.Both || searchTarget == SearchTarget.File;
	        var fileSystems = _fileSystems;
	        for (var i = fileSystems.Count - 1; i >= -1; i--)
	        {
	            var fileSystem = i < 0 ? Fallback : fileSystems[i];
	            if (fileSystem is null)
	            {
	                break;
	            }
	            // Go through aggregates
	            if (fileSystem is AggregateFileSystem aggregate)
	            {
	                aggregate.FindPaths(path, searchTarget, paths);
	            }
	            else
	            {
	                bool isFile = false;
	                if ((queryDirectory && fileSystem.DirectoryExists(path)) || (queryFile && (isFile = fileSystem.FileExists(path))))
	                {
	                    paths.Add(new FileSystemPath(fileSystem, path, isFile));
	                }
	            }
	        }
	    }
	    private readonly struct FileSystemPath
	    {
	        public FileSystemPath(IFileSystem fileSystem, UPath path, bool isFile)
	        {
	            FileSystem = fileSystem;
	            Path = path;
	            IsFile = isFile;
	        }
	        public readonly IFileSystem FileSystem;
	        public readonly UPath Path;
	        public readonly bool IsFile;
	    }
	    private sealed class DebuggerProxy
	    {
	        private readonly AggregateFileSystem _fs;
	        public DebuggerProxy(AggregateFileSystem fs)
	        {
	            _fs = fs;
	        }
	        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
	        public IFileSystem[] FileSystems => _fs._fileSystems.ToArray();
	        public IFileSystem? Fallback => _fs.Fallback;
	    }
	}
	public class AggregateFileSystemWatcher : FileSystemWatcher
	{
	    private readonly List<IFileSystemWatcher> _children;
	    private int _internalBufferSize;
	    private NotifyFilters _notifyFilter;
	    private bool _enableRaisingEvents;
	    private bool _includeSubdirectories;
	    private string _filter;
	    public AggregateFileSystemWatcher(IFileSystem fileSystem, UPath path)
	        : base(fileSystem, path)
	    {
	        _children = new List<IFileSystemWatcher>();
	        _internalBufferSize = 0;
	        _notifyFilter = NotifyFilters.Default;
	        _enableRaisingEvents = false;
	        _includeSubdirectories = false;
	        _filter = "*.*";
	    }
	    public void Add(IFileSystemWatcher watcher)
	    {
	        if (watcher is null)
	        {
	            throw new ArgumentNullException(nameof(watcher));
	        }
	        if (_children.Contains(watcher))
	        {
	            throw new ArgumentException("The filesystem watcher is already added", nameof(watcher));
	        }
	        watcher.InternalBufferSize = InternalBufferSize;
	        watcher.NotifyFilter = NotifyFilter;
	        watcher.EnableRaisingEvents = EnableRaisingEvents;
	        watcher.IncludeSubdirectories = IncludeSubdirectories;
	        watcher.Filter = Filter;
	        RegisterEvents(watcher);
	        _children.Add(watcher);
	    }
	    public void RemoveFrom(IFileSystem fileSystem)
	    {
	        if (fileSystem is null)
	        {
	            throw new ArgumentNullException(nameof(fileSystem));
	        }
	        lock (_children)
	        {
	            for (var i = _children.Count - 1; i >= 0; i--)
	            {
	                var watcher = _children[i];
	                if (watcher.FileSystem != fileSystem)
	                {
	                    continue;
	                }
	                UnregisterEvents(watcher);
	                _children.RemoveAt(i);
	                watcher.Dispose();
	            }
	        }
	    }
	    public void Clear(IFileSystem? excludeFileSystem = null)
	    {
	        for (var i = _children.Count - 1; i >= 0; i--)
	        {
	            var watcher = _children[i];
	            if (watcher.FileSystem == excludeFileSystem)
	            {
	                continue;
	            }
	            UnregisterEvents(watcher);
	            _children.RemoveAt(i);
	            watcher.Dispose();
	        }
	    }
	    protected override void Dispose(bool disposing)
	    {
	        if (disposing)
	        {
	            Clear();
	        }
	    }
	    public override int InternalBufferSize
	    {
	        get => _internalBufferSize;
	        set
	        {
	            if (value == _internalBufferSize)
	            {
	                return;
	            }
	            foreach (var watcher in _children)
	            {
	                watcher.InternalBufferSize = value;
	            }
	            _internalBufferSize = value;
	        }
	    }
	    public override NotifyFilters NotifyFilter
	    {
	        get => _notifyFilter;
	        set
	        {
	            if (value == _notifyFilter)
	            {
	                return;
	            }
	            foreach (var watcher in _children)
	            {
	                watcher.NotifyFilter = value;
	            }
	            _notifyFilter = value;
	        }
	    }
	    public override bool EnableRaisingEvents
	    {
	        get => _enableRaisingEvents;
	        set
	        {
	            if (value == _enableRaisingEvents)
	            {
	                return;
	            }
	            foreach (var watcher in _children)
	            {
	                watcher.EnableRaisingEvents = value;
	            }
	            _enableRaisingEvents = value;
	        }
	    }
	    public override bool IncludeSubdirectories
	    {
	        get => _includeSubdirectories;
	        set
	        {
	            if (value == _includeSubdirectories)
	            {
	                return;
	            }
	            foreach (var watcher in _children)
	            {
	                watcher.IncludeSubdirectories = value;
	            }
	            _includeSubdirectories = value;
	        }
	    }
	    public override string Filter
	    {
	        get => _filter;
	        set
	        {
	            if (value == _filter)
	            {
	                return;
	            }
	            foreach (var watcher in _children)
	            {
	                watcher.Filter = value;
	            }
	            _filter = value;
	        }
	    }
	}
	public abstract class ComposeFileSystem : FileSystem
	{
	    protected bool Owned { get; }
	    protected ComposeFileSystem(IFileSystem? fileSystem, bool owned = true)
	    {
	        Fallback = fileSystem;
	        Owned = owned;
	    }
	    protected override void Dispose(bool disposing)
	    {
	        if (disposing && Owned)
	        {
	            Fallback?.Dispose();
	        }
	    }
	    protected IFileSystem? Fallback { get; }
	    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
	    protected IFileSystem FallbackSafe
	    {
	        get
	        {
	            if (Fallback is null)
	            {
	                throw new InvalidOperationException("The delegate filesystem for this instance is null");
	            }
	            return Fallback;
	        }
	    }
	    protected override string DebuggerDisplay()
	    {
	        return $"{base.DebuggerDisplay()} (Fallback: {(Fallback is FileSystem fs ? fs.DebuggerKindName() : Fallback?.GetType().Name.Replace("FileSystem", "fs").ToLowerInvariant())})";
	    }
	    // ----------------------------------------------
	    // Directory API
	    // ----------------------------------------------
	    protected override void CreateDirectoryImpl(UPath path)
	    {
	        FallbackSafe.CreateDirectory(ConvertPathToDelegate(path));
	    }
	    protected override bool DirectoryExistsImpl(UPath path)
	    {
	        return FallbackSafe.DirectoryExists(ConvertPathToDelegate(path));
	    }
	    protected override void MoveDirectoryImpl(UPath srcPath, UPath destPath)
	    {
	        FallbackSafe.MoveDirectory(ConvertPathToDelegate(srcPath), ConvertPathToDelegate(destPath));
	    }
	    protected override void DeleteDirectoryImpl(UPath path, bool isRecursive)
	    {
	        FallbackSafe.DeleteDirectory(ConvertPathToDelegate(path), isRecursive);
	    }
	    // ----------------------------------------------
	    // File API
	    // ----------------------------------------------
	    protected override void CopyFileImpl(UPath srcPath, UPath destPath, bool overwrite)
	    {
	        FallbackSafe.CopyFile(ConvertPathToDelegate(srcPath), ConvertPathToDelegate(destPath), overwrite);
	    }
	    protected override void ReplaceFileImpl(UPath srcPath, UPath destPath, UPath destBackupPath,
	        bool ignoreMetadataErrors)
	    {
	        FallbackSafe.ReplaceFile(ConvertPathToDelegate(srcPath), ConvertPathToDelegate(destPath), destBackupPath.IsNull ? destBackupPath : ConvertPathToDelegate(destBackupPath), ignoreMetadataErrors);
	    }
	    protected override long GetFileLengthImpl(UPath path)
	    {
	        return FallbackSafe.GetFileLength(ConvertPathToDelegate(path));
	    }
	    protected override bool FileExistsImpl(UPath path)
	    {
	        return FallbackSafe.FileExists(ConvertPathToDelegate(path));
	    }
	    protected override void MoveFileImpl(UPath srcPath, UPath destPath)
	    {
	        FallbackSafe.MoveFile(ConvertPathToDelegate(srcPath), ConvertPathToDelegate(destPath));
	    }
	    protected override void DeleteFileImpl(UPath path)
	    {
	        FallbackSafe.DeleteFile(ConvertPathToDelegate(path));
	    }
	    protected override Stream OpenFileImpl(UPath path, FileMode mode, FileAccess access, FileShare share = FileShare.None)
	    {
	        return FallbackSafe.OpenFile(ConvertPathToDelegate(path), mode, access, share);
	    }
	    // ----------------------------------------------
	    // Metadata API
	    // ----------------------------------------------
	    protected override FileAttributes GetAttributesImpl(UPath path)
	    {
	        return FallbackSafe.GetAttributes(ConvertPathToDelegate(path));
	    }
	    protected override void SetAttributesImpl(UPath path, FileAttributes attributes)
	    {
	        FallbackSafe.SetAttributes(ConvertPathToDelegate(path), attributes);
	    }
	    protected override DateTime GetCreationTimeImpl(UPath path)
	    {
	        return FallbackSafe.GetCreationTime(ConvertPathToDelegate(path));
	    }
	    protected override void SetCreationTimeImpl(UPath path, DateTime time)
	    {
	        FallbackSafe.SetCreationTime(ConvertPathToDelegate(path), time);
	    }
	    protected override DateTime GetLastAccessTimeImpl(UPath path)
	    {
	        return FallbackSafe.GetLastAccessTime(ConvertPathToDelegate(path));
	    }
	    protected override void SetLastAccessTimeImpl(UPath path, DateTime time)
	    {
	        FallbackSafe.SetLastAccessTime(ConvertPathToDelegate(path), time);
	    }
	    protected override DateTime GetLastWriteTimeImpl(UPath path)
	    {
	        return FallbackSafe.GetLastWriteTime(ConvertPathToDelegate(path));
	    }
	    protected override void SetLastWriteTimeImpl(UPath path, DateTime time)
	    {
	        FallbackSafe.SetLastWriteTime(ConvertPathToDelegate(path), time);
	    }
	    protected override void CreateSymbolicLinkImpl(UPath path, UPath pathToTarget)
	    {
	        FallbackSafe.CreateSymbolicLink(ConvertPathToDelegate(path), ConvertPathToDelegate(pathToTarget));
	    }
	    protected override bool TryResolveLinkTargetImpl(UPath linkPath, out UPath resolvedPath)
	    {
	        if (!FallbackSafe.TryResolveLinkTarget(ConvertPathToDelegate(linkPath), out var resolvedPathDelegate))
	        {
	            resolvedPath = default;
	            return false;
	        }
	        resolvedPath = ConvertPathFromDelegate(resolvedPathDelegate);
	        return true;
	    }
	    // ----------------------------------------------
	    // Search API
	    // ----------------------------------------------
	    protected override IEnumerable<UPath> EnumeratePathsImpl(UPath path, string searchPattern, SearchOption searchOption, SearchTarget searchTarget)
	    {
	        foreach (var subPath in FallbackSafe.EnumeratePaths(ConvertPathToDelegate(path), searchPattern, searchOption, searchTarget))
	        {
	            yield return ConvertPathFromDelegate(subPath);
	        }
	    }
	    protected override IEnumerable<FileSystemItem> EnumerateItemsImpl(UPath path, SearchOption searchOption, SearchPredicate? searchPredicate)
	    {
	        foreach (var subItem in FallbackSafe.EnumerateItems(ConvertPathToDelegate(path), searchOption, searchPredicate))
	        {
	            var localItem = subItem;
	            localItem.Path = ConvertPathFromDelegate(localItem.Path);
	            yield return localItem;
	        }
	    }
	    // ----------------------------------------------
	    // Watch API
	    // ----------------------------------------------
	    protected override bool CanWatchImpl(UPath path)
	    {
	        return FallbackSafe.CanWatch(ConvertPathToDelegate(path));
	    }
	    protected override IFileSystemWatcher WatchImpl(UPath path)
	    {
	        return FallbackSafe.Watch(ConvertPathToDelegate(path));
	    }
	    // ----------------------------------------------
	    // Path API
	    // ----------------------------------------------
	    protected override string ConvertPathToInternalImpl(UPath path)
	    {
	        return FallbackSafe.ConvertPathToInternal(ConvertPathToDelegate(path));
	    }
	    protected override UPath ConvertPathFromInternalImpl(string innerPath)
	    {
	        return ConvertPathFromDelegate(FallbackSafe.ConvertPathFromInternal(innerPath));
	    }
	    protected abstract UPath ConvertPathToDelegate(UPath path);
	    protected abstract UPath ConvertPathFromDelegate(UPath path);
	    protected override (IFileSystem FileSystem, UPath Path) ResolvePathImpl(UPath path)
	        => Fallback?.ResolvePath(ConvertPathToDelegate(path)) ?? base.ResolvePathImpl(path);
	}
	public abstract class FileSystem : IFileSystem
	{
	    public static readonly DateTime DefaultFileTime = new DateTime(1601, 01, 01, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();
	    ~FileSystem()
	    {
	        DisposeInternal(false);
	    }
	    public void Dispose()
	    {
	        DisposeInternal(true);
	        GC.SuppressFinalize(this);
	    }
	    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
	    protected bool IsDisposing { get; private set; }
	    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
	    protected bool IsDisposed { get; private set; }
	    public string? Name { get; set; }
	    // ----------------------------------------------
	    // Directory API
	    // ----------------------------------------------
	    public void CreateDirectory(UPath path)
	    {
	        AssertNotDisposed();
	        if (path == UPath.Root) return; // nop
	        CreateDirectoryImpl(ValidatePath(path));
	    }
	    protected abstract void CreateDirectoryImpl(UPath path);
	    public bool DirectoryExists(UPath path)
	    {
	        AssertNotDisposed();
	        // With FileExists, case where a null path is allowed
	        if (path.IsNull)
	        {
	            return false;
	        }
	        return DirectoryExistsImpl(ValidatePath(path));
	    }
	    protected abstract bool DirectoryExistsImpl(UPath path);
	    public void MoveDirectory(UPath srcPath, UPath destPath)
	    {
	        AssertNotDisposed();
	        if (srcPath == UPath.Root)
	        {
	            throw new UnauthorizedAccessException("Cannot move from the source root directory `/`");
	        }
	        if (destPath == UPath.Root)
	        {
	            throw new UnauthorizedAccessException("Cannot move to the root directory `/`");
	        }
	        if (srcPath == destPath)
	        {
	            throw new IOException($"The source and destination path are the same `{srcPath}`");
	        }
	        MoveDirectoryImpl(ValidatePath(srcPath, nameof(srcPath)), ValidatePath(destPath, nameof(destPath)));
	    }
	    protected abstract void MoveDirectoryImpl(UPath srcPath, UPath destPath);
	    public void DeleteDirectory(UPath path, bool isRecursive)
	    {
	        AssertNotDisposed();
	        if (path == UPath.Root)
	        {
	            throw new UnauthorizedAccessException("Cannot delete root directory `/`");
	        }
	        DeleteDirectoryImpl(ValidatePath(path), isRecursive);
	    }
	    protected abstract void DeleteDirectoryImpl(UPath path, bool isRecursive);
	    internal string DebuggerDisplayInternal()
	    {
	        return DebuggerDisplay();
	    }
	    internal string DebuggerKindName()
	    {
	        var typeName = this.GetType().Name.Replace("FileSystem", "fs").ToLowerInvariant();
	        return Name != null ? $"{typeName}-{Name}" : typeName;
	    }
	    protected virtual string DebuggerDisplay() => DebuggerKindName();
	    // ----------------------------------------------
	    // File API
	    // ----------------------------------------------
	    public void CopyFile(UPath srcPath, UPath destPath, bool overwrite)
	    {
	        AssertNotDisposed();
	        CopyFileImpl(ValidatePath(srcPath, nameof(srcPath)), ValidatePath(destPath, nameof(destPath)), overwrite);
	    }
	    protected abstract void CopyFileImpl(UPath srcPath, UPath destPath, bool overwrite);
	    public void ReplaceFile(UPath srcPath, UPath destPath, UPath destBackupPath, bool ignoreMetadataErrors)
	    {
	        AssertNotDisposed();
	        srcPath = ValidatePath(srcPath, nameof(srcPath));
	        destPath = ValidatePath(destPath, nameof(destPath));
	        destBackupPath = ValidatePath(destBackupPath, nameof(destBackupPath), true);
	        if (!FileExistsImpl(srcPath))
	        {
	            throw NewFileNotFoundException(srcPath);
	        }
	        if (!FileExistsImpl(destPath))
	        {
	            throw NewFileNotFoundException(srcPath);
	        }
	        if (destBackupPath == srcPath)
	        {
	            throw new IOException($"The source and backup cannot have the same path `{srcPath}`");
	        }
	        ReplaceFileImpl(srcPath, destPath, destBackupPath, ignoreMetadataErrors);
	    }
	    protected abstract void ReplaceFileImpl(UPath srcPath, UPath destPath, UPath destBackupPath, bool ignoreMetadataErrors);
	    public long GetFileLength(UPath path)
	    {
	        AssertNotDisposed();
	        return GetFileLengthImpl(ValidatePath(path));
	    }
	    protected abstract long GetFileLengthImpl(UPath path);
	    public bool FileExists(UPath path)
	    {
	        AssertNotDisposed();
	        // Only case where a null path is allowed
	        if (path.IsNull)
	        {
	            return false;
	        }
	        return FileExistsImpl(ValidatePath(path));
	    }
	    protected abstract bool FileExistsImpl(UPath path);
	    public void MoveFile(UPath srcPath, UPath destPath)
	    {
	        AssertNotDisposed();
	        MoveFileImpl(ValidatePath(srcPath, nameof(srcPath)), ValidatePath(destPath, nameof(destPath)));
	    }
	    protected abstract void MoveFileImpl(UPath srcPath, UPath destPath);
	    public void DeleteFile(UPath path)
	    {
	        AssertNotDisposed();
	        DeleteFileImpl(ValidatePath(path));
	    }
	    protected abstract void DeleteFileImpl(UPath path);
	    public Stream OpenFile(UPath path, FileMode mode, FileAccess access, FileShare share = FileShare.None)
	    {
	        AssertNotDisposed();
	        return OpenFileImpl(ValidatePath(path), mode, access, share);
	    }
	    protected abstract Stream OpenFileImpl(UPath path, FileMode mode, FileAccess access, FileShare share);
	    // ----------------------------------------------
	    // Metadata API
	    // ----------------------------------------------
	    public FileAttributes GetAttributes(UPath path)
	    {
	        AssertNotDisposed();
	        return GetAttributesImpl(ValidatePath(path));
	    }
	    protected abstract FileAttributes GetAttributesImpl(UPath path);
	    public void SetAttributes(UPath path, FileAttributes attributes)
	    {
	        AssertNotDisposed();
	        SetAttributesImpl(ValidatePath(path), attributes);
	    }
	    protected abstract void SetAttributesImpl(UPath path, FileAttributes attributes);
	    public DateTime GetCreationTime(UPath path)
	    {
	        AssertNotDisposed();
	        return GetCreationTimeImpl(ValidatePath(path));
	    }
	    protected abstract DateTime GetCreationTimeImpl(UPath path);
	    public void SetCreationTime(UPath path, DateTime time)
	    {
	        AssertNotDisposed();
	        SetCreationTimeImpl(ValidatePath(path), time);
	    }
	    protected abstract void SetCreationTimeImpl(UPath path, DateTime time);
	    public DateTime GetLastAccessTime(UPath path)
	    {
	        AssertNotDisposed();
	        return GetLastAccessTimeImpl(ValidatePath(path));
	    }
	    protected abstract DateTime GetLastAccessTimeImpl(UPath path);
	    public void SetLastAccessTime(UPath path, DateTime time)
	    {
	        AssertNotDisposed();
	        SetLastAccessTimeImpl(ValidatePath(path), time);
	    }
	    protected abstract void SetLastAccessTimeImpl(UPath path, DateTime time);
	    public DateTime GetLastWriteTime(UPath path)
	    {
	        AssertNotDisposed();
	        return GetLastWriteTimeImpl(ValidatePath(path));
	    }
	    protected abstract DateTime GetLastWriteTimeImpl(UPath path);
	    public void SetLastWriteTime(UPath path, DateTime time)
	    {
	        AssertNotDisposed();
	        SetLastWriteTimeImpl(ValidatePath(path), time);
	    }
	    protected abstract void SetLastWriteTimeImpl(UPath path, DateTime time);
	    public void CreateSymbolicLink(UPath path, UPath pathToTarget)
	    {
	        AssertNotDisposed();
	        CreateSymbolicLinkImpl(ValidatePath(path), ValidatePath(pathToTarget));
	    }
	    protected abstract void CreateSymbolicLinkImpl(UPath path, UPath pathToTarget);
	    public bool TryResolveLinkTarget(UPath linkPath, out UPath resolvedPath)
	    {
	        AssertNotDisposed();
	        return TryResolveLinkTargetImpl(ValidatePath(linkPath), out resolvedPath);
	    }
	    protected abstract bool TryResolveLinkTargetImpl(UPath linkPath, out UPath resolvedPath);
	    // ----------------------------------------------
	    // Search API
	    // ----------------------------------------------
	    public IEnumerable<UPath> EnumeratePaths(UPath path, string searchPattern, SearchOption searchOption, SearchTarget searchTarget)
	    {
	        AssertNotDisposed();
	        if (searchPattern is null) throw new ArgumentNullException(nameof(searchPattern));
	        return EnumeratePathsImpl(ValidatePath(path), searchPattern, searchOption, searchTarget);
	    }
	    protected abstract IEnumerable<UPath> EnumeratePathsImpl(UPath path, string searchPattern, SearchOption searchOption, SearchTarget searchTarget);
	    public IEnumerable<FileSystemItem> EnumerateItems(UPath path, SearchOption searchOption, SearchPredicate? searchPredicate = null)
	    {
	        AssertNotDisposed();
	        return EnumerateItemsImpl(ValidatePath(path), searchOption, searchPredicate);
	    }
	    protected abstract IEnumerable<FileSystemItem> EnumerateItemsImpl(UPath path, SearchOption searchOption, SearchPredicate? searchPredicate);
	    // ----------------------------------------------
	    // Watch API
	    // ----------------------------------------------
	    public bool CanWatch(UPath path)
	    {
	        AssertNotDisposed();
	        return CanWatchImpl(ValidatePath(path));
	    }
	    protected virtual bool CanWatchImpl(UPath path)
	    {
	        return true;
	    }
	    public IFileSystemWatcher Watch(UPath path)
	    {
	        AssertNotDisposed();
	        var validatedPath = ValidatePath(path);
	        if (!CanWatchImpl(validatedPath))
	        {
	            throw new NotSupportedException($"The file system or path `{validatedPath}` does not support watching");
	        }
	        return WatchImpl(validatedPath);
	    }
	    protected abstract IFileSystemWatcher WatchImpl(UPath path);
	    // ----------------------------------------------
	    // Path API
	    // ----------------------------------------------
	    public string ConvertPathToInternal(UPath path)
	    {
	        AssertNotDisposed();
	        return ConvertPathToInternalImpl(ValidatePath(path));
	    }
	    protected abstract string ConvertPathToInternalImpl(UPath path);
	    public UPath ConvertPathFromInternal(string systemPath)
	    {
	        AssertNotDisposed();
	        if (systemPath is null) throw new ArgumentNullException(nameof(systemPath));
	        return ValidatePath(ConvertPathFromInternalImpl(systemPath));
	    }
	    protected abstract UPath ConvertPathFromInternalImpl(string innerPath);
	    public (IFileSystem FileSystem, UPath Path) ResolvePath(UPath path)
	    {
	        AssertNotDisposed();
	        return ResolvePathImpl(ValidatePath(path));
	    }
	    protected virtual (IFileSystem FileSystem, UPath Path) ResolvePathImpl(UPath path) => (this, path);
	    protected virtual UPath ValidatePathImpl(UPath path, string name = "path")
	    {
	        if (path.FullName.IndexOf(':') >= 0)
	        {
	            throw new NotSupportedException($"The path `{path}` cannot contain the `:` character");
	        }
	        return path;
	    }
	    protected UPath ValidatePath(UPath path, string name = "path", bool allowNull = false)
	    {
	        if (allowNull && path.IsNull)
	        {
	            return path;
	        }
	        // Make sure that we don't have any control characters in the path.
	        var fullPath = path.FullName;
	        for (var i = 0; i < fullPath.Length; i++)
	        {
	            var c = fullPath[i];
	            if (char.IsControl(c))
	            {
	                throw new ArgumentException($"Invalid character found \\u{(int)c:X4} at index {i}", nameof(path));
	            }
	        }
	        path.AssertAbsolute(name);
	        return ValidatePathImpl(path, name);
	    }
	    protected virtual void Dispose(bool disposing)
	    {
	    }
	    private void AssertNotDisposed()
	    {
	        if (IsDisposing || IsDisposed)
	        {
	            Throw(this.GetType());
	        }
	        static void Throw(Type type)
	        {
	            throw new ObjectDisposedException($"This instance `{type}` is already disposed.");
	        }
	    }
	    private void DisposeInternal(bool disposing)
	    {
	        if (!IsDisposed)
	        {
	            AssertNotDisposed();
	            IsDisposing = true;
	            Dispose(disposing);
	            IsDisposed = true;
	        }
	    }
	}
	public class FileSystemEventDispatcher<T> : IDisposable
	    where T : FileSystemWatcher
	{
	    private readonly Thread _dispatchThread;
	    private readonly BlockingCollection<Action> _dispatchQueue;
	    private readonly CancellationTokenSource _dispatchCts;
	    private readonly List<T> _watchers;
	    public FileSystemEventDispatcher(IFileSystem fileSystem)
	    {
	        FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
	        _dispatchThread = new Thread(DispatchWorker)
	        {
	            Name = "FileSystem Event Dispatch",
	            IsBackground = true
	        };
	        _dispatchQueue = new BlockingCollection<Action>(16);
	        _dispatchCts = new CancellationTokenSource();
	        _watchers = new List<T>();
	        _dispatchThread.Start();
	    }
	    public IFileSystem FileSystem { get; }
	    ~FileSystemEventDispatcher()
	    {
	        Dispose(false);
	    }
	    public void Dispose()
	    {
	        Dispose(true);
	        GC.SuppressFinalize(this);
	    }
	    protected virtual void Dispose(bool disposing)
	    {
	        _dispatchCts?.Cancel();
	        _dispatchThread?.Join();
	        if (!disposing)
	        {
	            return;
	        }
	        _dispatchQueue.CompleteAdding();
	        lock (_watchers)
	        {
	            foreach (var watcher in _watchers)
	            {
	                watcher.Dispose();
	            }
	            _watchers.Clear();
	        }
	        _dispatchQueue.Dispose();
	    }
	    public void Add(T watcher)
	    {
	        lock (_watchers)
	        {
	            _watchers.Add(watcher);
	        }
	    }
	    public void Remove(T watcher)
	    {
	        lock (_watchers)
	        {
	            _watchers.Remove(watcher);
	        }
	    }
	    public void RaiseChange(UPath path)
	    {
	        var args = new FileChangedEventArgs(FileSystem, WatcherChangeTypes.Changed, path);
	        Dispatch(args, (w, a) => w.RaiseChanged(a));
	    }
	    public void RaiseCreated(UPath path)
	    {
	        var args = new FileChangedEventArgs(FileSystem, WatcherChangeTypes.Created, path);
	        Dispatch(args, (w, a) => w.RaiseCreated(a));
	    }
	    public void RaiseDeleted(UPath path)
	    {
	        var args = new FileChangedEventArgs(FileSystem, WatcherChangeTypes.Deleted, path);
	        Dispatch(args, (w, a) => w.RaiseDeleted(a));
	    }
	    public void RaiseRenamed(UPath newPath, UPath oldPath)
	    {
	        var args = new FileRenamedEventArgs(FileSystem, WatcherChangeTypes.Renamed, newPath, oldPath);
	        Dispatch(args, (w, a) => w.RaiseRenamed(a));
	    }
	    public void RaiseError(Exception exception)
	    {
	        var args = new FileSystemErrorEventArgs(exception);
	        Dispatch(args, (w, a) => w.RaiseError(a), false);
	    }
	    private void Dispatch<TArgs>(TArgs eventArgs, Action<T, TArgs> handler, bool captureError = true)
	        where TArgs : EventArgs
	    {
	        List<T> watchersSnapshot;
	        lock (_watchers)
	        {
	            if (_watchers.Count == 0)
	            {
	                return;
	            }
	            watchersSnapshot = _watchers.ToList(); // TODO: reduce allocations
	        }
	        // The events should be called on a separate thread because the filesystem code
	        // could be holding locks that must be released.
	        _dispatchQueue.Add(() =>
	        {
	            foreach (var watcher in watchersSnapshot)
	            {
	                try
	                {
	                    handler(watcher, eventArgs);
	                }
	                catch (Exception e) when (captureError)
	                {
	                    RaiseError(e);
	                }
	            }
	        });
	    }
	    // Worker runs on dedicated thread to call events
	    private void DispatchWorker()
	    {
	        var ct = _dispatchCts.Token;
	        try
	        {
	            foreach (var action in _dispatchQueue.GetConsumingEnumerable(ct))
	            {
	                action();
	            }
	        }
	        catch (OperationCanceledException) { }
	        catch (ObjectDisposedException) { }
	    }
	}
	public class FileSystemWatcher : IFileSystemWatcher
	{
	    private string _filter;
	    private FilterPattern _filterPattern;
	    public event EventHandler<FileChangedEventArgs>? Changed;
	    public event EventHandler<FileChangedEventArgs>? Created;
	    public event EventHandler<FileChangedEventArgs>? Deleted;
	    public event EventHandler<FileSystemErrorEventArgs>? Error;
	    public event EventHandler<FileRenamedEventArgs>? Renamed;
	    public IFileSystem FileSystem { get; }
	    public UPath Path { get; }
	    public virtual int InternalBufferSize
	    {
	        get => 0;
	        set { }
	    }
	    public virtual NotifyFilters NotifyFilter { get; set; } = NotifyFilters.Default;
	    public virtual bool EnableRaisingEvents { get; set; }
	    public virtual string Filter
	    {
	        get => _filter;
	        set
	        {
	            if (string.IsNullOrEmpty(value))
	            {
	                value = "*";
	            }
	            if (value == _filter)
	            {
	                return;
	            }
	            _filterPattern = FilterPattern.Parse(value);
	            _filter = value;
	        }
	    }
	    public virtual bool IncludeSubdirectories { get; set; }
	    public FileSystemWatcher(IFileSystem fileSystem, UPath path)
	    {
	        path.AssertAbsolute();
	        FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
	        Path = path;
	        _filter = "*.*";
	    }
	    ~FileSystemWatcher()
	    {
	        Dispose(false);
	    }
	    public void Dispose()
	    {
	        Dispose(true);
	        GC.SuppressFinalize(this);
	    }
	    protected virtual void Dispose(bool disposing)
	    {
	    }
	    public void RaiseChanged(FileChangedEventArgs args)
	    {
	        if (!ShouldRaiseEvent(args))
	        {
	            return;
	        }
	        Changed?.Invoke(this, args);
	    }
	    public void RaiseCreated(FileChangedEventArgs args)
	    {
	        if (!ShouldRaiseEvent(args))
	        {
	            return;
	        }
	        Created?.Invoke(this, args);
	    }
	    public void RaiseDeleted(FileChangedEventArgs args)
	    {
	        if (!ShouldRaiseEvent(args))
	        {
	            return;
	        }
	        Deleted?.Invoke(this, args);
	    }
	    public void RaiseError(FileSystemErrorEventArgs args)
	    {
	        if (!EnableRaisingEvents)
	        {
	            return;
	        }
	        Error?.Invoke(this, args);
	    }
	    public void RaiseRenamed(FileRenamedEventArgs args)
	    {
	        if (!ShouldRaiseEvent(args))
	        {
	            return;
	        }
	        Renamed?.Invoke(this, args);
	    }
	    private bool ShouldRaiseEvent(FileChangedEventArgs args)
	    {
	        return EnableRaisingEvents &&
	               _filterPattern.Match(args.Name) &&
	               ShouldRaiseEventImpl(args);
	    }
	    protected virtual bool ShouldRaiseEventImpl(FileChangedEventArgs args)
	    {
	        return args.FullPath.IsInDirectory(Path, IncludeSubdirectories);
	    }
	    protected void RegisterEvents(IFileSystemWatcher watcher)
	    {
	        if (watcher is null)
	        {
	            throw new ArgumentNullException(nameof(watcher));
	        }
	        watcher.Changed += OnChanged;
	        watcher.Created += OnCreated;
	        watcher.Deleted += OnDeleted;
	        watcher.Error += OnError;
	        watcher.Renamed += OnRenamed;
	    }
	    protected void UnregisterEvents(IFileSystemWatcher watcher)
	    {
	        if (watcher is null)
	        {
	            throw new ArgumentNullException(nameof(watcher));
	        }
	        watcher.Changed -= OnChanged;
	        watcher.Created -= OnCreated;
	        watcher.Deleted -= OnDeleted;
	        watcher.Error -= OnError;
	        watcher.Renamed -= OnRenamed;
	    }
	    protected virtual UPath? TryConvertPath(UPath pathFromEvent)
	    {
	        return pathFromEvent;
	    }
	    private void OnChanged(object? sender, FileChangedEventArgs args)
	    {
	        var newPath = TryConvertPath(args.FullPath);
	        if (!newPath.HasValue)
	        {
	            return;
	        }
	        var newArgs = new FileChangedEventArgs(FileSystem, args.ChangeType, newPath.Value);
	        RaiseChanged(newArgs);
	    }
	    private void OnCreated(object? sender, FileChangedEventArgs args)
	    {
	        var newPath = TryConvertPath(args.FullPath);
	        if (!newPath.HasValue)
	        {
	            return;
	        }
	        var newArgs = new FileChangedEventArgs(FileSystem, args.ChangeType, newPath.Value);
	        RaiseCreated(newArgs);
	    }
	    private void OnDeleted(object? sender, FileChangedEventArgs args)
	    {
	        var newPath = TryConvertPath(args.FullPath);
	        if (!newPath.HasValue)
	        {
	            return;
	        }
	        var newArgs = new FileChangedEventArgs(FileSystem, args.ChangeType, newPath.Value);
	        RaiseDeleted(newArgs);
	    }
	    private void OnError(object? sender, FileSystemErrorEventArgs args)
	    {
	        RaiseError(args);
	    }
	    private void OnRenamed(object? sender, FileRenamedEventArgs args)
	    {
	        var newPath = TryConvertPath(args.FullPath);
	        if (!newPath.HasValue)
	        {
	            return;
	        }
	        var newOldPath = TryConvertPath(args.OldFullPath);
	        if (!newOldPath.HasValue)
	        {
	            return;
	        }
	        var newArgs = new FileRenamedEventArgs(FileSystem, args.ChangeType, newPath.Value, newOldPath.Value);
	        RaiseRenamed(newArgs);
	    }
	}
	[DebuggerDisplay("{" + nameof(DebuggerDisplay) + "(),nq}")]
	[DebuggerTypeProxy(typeof(DebuggerProxy))]
	public class MemoryFileSystem : FileSystem
	{
	    // The locking strategy is based on https://www.kernel.org/doc/Documentation/filesystems/directory-locking
	    private readonly DirectoryNode _rootDirectory;
	    private readonly FileSystemNodeReadWriteLock _globalLock;
	    private readonly object _dispatcherLock;
	    private FileSystemEventDispatcher<Watcher>? _dispatcher;
	    public MemoryFileSystem()
	    {
	        _rootDirectory = new DirectoryNode(this);
	        _globalLock = new FileSystemNodeReadWriteLock();
	        _dispatcherLock = new object();
	    }
	    protected MemoryFileSystem(MemoryFileSystem copyFrom)
	    {
	        if (copyFrom is null) throw new ArgumentNullException(nameof(copyFrom));
	        Debug.Assert(copyFrom._globalLock.IsLocked);
	        _rootDirectory = (DirectoryNode)copyFrom._rootDirectory.Clone(null, null);
	        _globalLock = new FileSystemNodeReadWriteLock();
	        _dispatcherLock = new object();
	    }
	    protected override void Dispose(bool disposing)
	    {
	        base.Dispose(disposing);
	        if (disposing)
	        {
	            TryGetDispatcher()?.Dispose();
	        }
	    }
	    public MemoryFileSystem Clone()
	    {
	        EnterFileSystemExclusive();
	        try
	        {
	            return CloneImpl();
	        }
	        finally
	        {
	            ExitFileSystemExclusive();
	        }
	    }
	    protected virtual MemoryFileSystem CloneImpl()
	    {
	        return new MemoryFileSystem(this);
	    }
	    protected override string DebuggerDisplay()
	    {
	        return $"{base.DebuggerDisplay()} {_rootDirectory.DebuggerDisplay()}";
	    }
	    // ----------------------------------------------
	    // Directory API
	    // ----------------------------------------------
	    protected override void CreateDirectoryImpl(UPath path)
	    {
	        EnterFileSystemShared();
	        try
	        {
	            CreateDirectoryNode(path);
	            TryGetDispatcher()?.RaiseCreated(path);
	        }
	        finally
	        {
	            ExitFileSystemShared();
	        }
	    }
	    protected override bool DirectoryExistsImpl(UPath path)
	    {
	        if (path == UPath.Root)
	        {
	            return true;
	        }
	        EnterFileSystemShared();
	        try
	        {
	            // NodeCheck doesn't take a lock, on the return node
	            // but allows us to check if it is a directory or a file
	            var result = EnterFindNode(path, FindNodeFlags.NodeCheck);
	            try
	            {
	                return result.Node is DirectoryNode;
	            }
	            finally
	            {
	                ExitFindNode(result);
	            }
	        }
	        finally
	        {
	            ExitFileSystemShared();
	        }
	    }
	    protected override void MoveDirectoryImpl(UPath srcPath, UPath destPath)
	    {
	        MoveFileOrDirectory(srcPath, destPath, true);
	    }
	    protected override void DeleteDirectoryImpl(UPath path, bool isRecursive)
	    {
	        EnterFileSystemShared();
	        try
	        {
	            var result = EnterFindNode(path, FindNodeFlags.KeepParentNodeExclusive | FindNodeFlags.NodeExclusive);
	            bool deleteRootDirectory = false;
	            try
	            {
	                ValidateDirectory(result.Node, path);
	                if (result.Node.IsReadOnly)
	                {
	                    throw new IOException($"Access to the path `{path}` is denied");
	                }
	                using (var locks = new ListFileSystemNodes(this))
	                {
	                    TryLockExclusive(result.Node, locks, isRecursive, path);
	                    // Check that files are not readonly
	                    foreach (var lockFile in locks)
	                    {
	                        var node = lockFile.Value;
	                        if (node.IsReadOnly)
	                        {
	                            throw new UnauthorizedAccessException($"Access to path `{path}` is denied.");
	                        }
	                    }
	                    // We remove all elements
	                    for (var i = locks.Count - 1; i >= 0; i--)
	                    {
	                        var lockFile = locks[i];
	                        locks.RemoveAt(i);
	                        lockFile.Value.DetachFromParent();
	                        lockFile.Value.Dispose();
	                        ExitExclusive(lockFile.Value);
	                    }
	                }
	                deleteRootDirectory = true;
	            }
	            finally
	            {
	                if (deleteRootDirectory && result.Node != null)
	                {
	                    result.Node.DetachFromParent();
	                    result.Node.Dispose();
	                    TryGetDispatcher()?.RaiseDeleted(path);
	                }
	                ExitFindNode(result);
	            }
	        }
	        finally
	        {
	            ExitFileSystemShared();
	        }
	    }
	    // ----------------------------------------------
	    // File API
	    // ----------------------------------------------
	    protected override void CopyFileImpl(UPath srcPath, UPath destPath, bool overwrite)
	    {
	        EnterFileSystemShared();
	        try
	        {
	            var srcResult = EnterFindNode(srcPath, FindNodeFlags.NodeShared);
	            try
	            {
	                // The source file must exist
	                var srcNode = srcResult.Node;
	                if (srcNode is DirectoryNode)
	                {
	                    throw new UnauthorizedAccessException($"Cannot copy file. The path `{srcPath}` is a directory");
	                }
	                if (srcNode is null)
	                {
	                    throw NewFileNotFoundException(srcPath);
	                }
	                var destResult = EnterFindNode(destPath, FindNodeFlags.KeepParentNodeExclusive | FindNodeFlags.NodeExclusive);
	                var destFileName = destResult.Name;
	                var destDirectory = destResult.Directory;
	                var destNode = destResult.Node;
	                try
	                {
	                    // The dest file may exist
	                    if (destDirectory is null)
	                    {
	                        throw NewDirectoryNotFoundException(destPath);
	                    }
	                    if (destNode is DirectoryNode)
	                    {
	                        throw new IOException($"The target file `{destPath}` is a directory, not a file.");
	                    }
	                    // If the destination is empty, we need to create it
	                    if (destNode is null)
	                    {
	                        // Constructor copies and attaches to directory for us
	                        var newFileNode = new FileNode(this, destDirectory, destFileName, (FileNode)srcNode);
	                        TryGetDispatcher()?.RaiseCreated(destPath);
	                        TryGetDispatcher()?.RaiseChange(destPath);
	                    }
	                    else if (overwrite)
	                    {
	                        if (destNode.IsReadOnly)
	                        {
	                            throw new UnauthorizedAccessException($"Access to path `{destPath}` is denied.");
	                        }
	                        var destFileNode = (FileNode)destNode;
	                        destFileNode.Content.CopyFrom(((FileNode)srcNode).Content);
	                        TryGetDispatcher()?.RaiseChange(destPath);
	                    }
	                    else
	                    {
	                        throw new IOException($"The destination file path `{destPath}` already exist and overwrite is false");
	                    }
	                }
	                finally
	                {
	                    if (destNode != null)
	                    {
	                        ExitExclusive(destNode);
	                    }
	                    if (destDirectory != null)
	                    {
	                        ExitExclusive(destDirectory);
	                    }
	                }
	            }
	            finally
	            {
	                ExitFindNode(srcResult);
	            }
	        }
	        finally
	        {
	            ExitFileSystemShared();
	        }
	    }
	    protected override void ReplaceFileImpl(UPath srcPath, UPath destPath, UPath destBackupPath, bool ignoreMetadataErrors)
	    {
	        // Get the directories of src/dest/backup
	        var parentSrcPath = srcPath.GetDirectory();
	        var parentDestPath = destPath.GetDirectory();
	        var parentDestBackupPath = destBackupPath.IsNull ? new UPath() : destBackupPath.GetDirectory();
	        // Simple case: src/dest/backup in the same folder
	        var isSameFolder = parentSrcPath == parentDestPath && (destBackupPath.IsNull || (parentDestBackupPath == parentSrcPath));
	        // Else at least one folder is different. This is a rename semantic (as per the locking guidelines)
	        var paths = new List<KeyValuePair<UPath, int>>
	        {
	            new KeyValuePair<UPath, int>(srcPath, 0),
	            new KeyValuePair<UPath, int>(destPath, 1)
	        };
	        if (!destBackupPath.IsNull)
	        {
	            paths.Add(new KeyValuePair<UPath, int>(destBackupPath, 2));
	        }
	        paths.Sort((p1, p2) => string.Compare(p1.Key.FullName, p2.Key.FullName, StringComparison.Ordinal));
	        // We need to take the lock on the folders in the correct order to avoid deadlocks
	        // So we sort the srcPath and destPath in alphabetical order
	        // (if srcPath is a subFolder of destPath, we will lock first destPath parent Folder, and then srcFolder)
	        if (isSameFolder)
	        {
	            EnterFileSystemShared();
	        }
	        else
	        {
	            EnterFileSystemExclusive();
	        }
	        try
	        {
	            var results = new NodeResult[destBackupPath.IsNull ? 2 : 3];
	            try
	            {
	                for (int i = 0; i < paths.Count; i++)
	                {
	                    var pathPair = paths[i];
	                    var flags = FindNodeFlags.KeepParentNodeExclusive;
	                    if (pathPair.Value != 2)
	                    {
	                        flags |= FindNodeFlags.NodeExclusive;
	                    }
	                    else
	                    {
	                        flags |= FindNodeFlags.NodeShared;
	                    }
	                    results[pathPair.Value] = EnterFindNode(pathPair.Key, flags, results);
	                }
	                var srcResult = results[0];
	                var destResult = results[1];
	                ValidateFile(srcResult.Node, srcPath);
	                ValidateFile(destResult.Node, destPath);
	                if (!destBackupPath.IsNull)
	                {
	                    var backupResult = results[2];
	                    ValidateDirectory(backupResult.Directory, destPath);
	                    if (backupResult.Node != null)
	                    {
	                        ValidateFile(backupResult.Node, destBackupPath);
	                        backupResult.Node.DetachFromParent();
	                        backupResult.Node.Dispose();
	                        TryGetDispatcher()?.RaiseDeleted(destBackupPath);
	                    }
	                    destResult.Node.DetachFromParent();
	                    destResult.Node.AttachToParent(backupResult.Directory!, backupResult.Name!);
	                    TryGetDispatcher()?.RaiseRenamed(destBackupPath, destPath);
	                }
	                else
	                {
	                    destResult.Node.DetachFromParent();
	                    destResult.Node.Dispose();
	                    TryGetDispatcher()?.RaiseDeleted(destPath);
	                }
	                srcResult.Node.DetachFromParent();
	                srcResult.Node.AttachToParent(destResult.Directory!, destResult.Name!);
	                TryGetDispatcher()?.RaiseRenamed(destPath, srcPath);
	            }
	            finally
	            {
	                for (int i = results.Length - 1; i >= 0; i--)
	                {
	                    ExitFindNode(results[i]);
	                }
	            }
	        }
	        finally
	        {
	            if (isSameFolder)
	            {
	                ExitFileSystemShared();
	            }
	            else
	            {
	                ExitFileSystemExclusive();
	            }
	        }
	    }
	    protected override long GetFileLengthImpl(UPath path)
	    {
	        EnterFileSystemShared();
	        try
	        {
	            return ((FileNode)FindNodeSafe(path, true)).Content.Length;
	        }
	        finally
	        {
	            ExitFileSystemShared();
	        }
	    }
	    protected override bool FileExistsImpl(UPath path)
	    {
	        EnterFileSystemShared();
	        try
	        {
	            // NodeCheck doesn't take a lock, on the return node
	            // but allows us to check if it is a directory or a file
	            var result = EnterFindNode(path, FindNodeFlags.NodeCheck);
	            ExitFindNode(result);
	            return result.Node is FileNode;
	        }
	        finally
	        {
	            ExitFileSystemShared();
	        }
	    }
	    protected override void MoveFileImpl(UPath srcPath, UPath destPath)
	    {
	        MoveFileOrDirectory(srcPath, destPath, false);
	    }
	    protected override void DeleteFileImpl(UPath path)
	    {
	        EnterFileSystemShared();
	        try
	        {
	            var result = EnterFindNode(path, FindNodeFlags.KeepParentNodeExclusive | FindNodeFlags.NodeExclusive);
	            try
	            {
	                var srcNode = result.Node;
	                if (srcNode is null)
	                {
	                    // If the file to be deleted does not exist, no exception is thrown.
	                    return;
	                }
	                if (srcNode is DirectoryNode || srcNode.IsReadOnly)
	                {
	                    throw new UnauthorizedAccessException($"Access to path `{path}` is denied.");
	                }
	                srcNode.DetachFromParent();
	                srcNode.Dispose();
	                TryGetDispatcher()?.RaiseDeleted(path);
	            }
	            finally
	            {
	                ExitFindNode(result);
	            }
	        }
	        finally
	        {
	            ExitFileSystemShared();
	        }
	    }
	    protected override Stream OpenFileImpl(UPath path, FileMode mode, FileAccess access, FileShare share)
	    {
	        if (mode == FileMode.Append && (access & FileAccess.Read) != 0)
	        {
	            throw new ArgumentException("Combining FileMode: Append with FileAccess: Read is invalid.", nameof(access));
	        }
	        var isReading = (access & FileAccess.Read) != 0;
	        var isWriting = (access & FileAccess.Write) != 0;
	        var isExclusive = share == FileShare.None;
	        EnterFileSystemShared();
	        DirectoryNode? parentDirectory = null;
	        FileNode? fileNodeToRelease = null;
	        try
	        {
	            var result = EnterFindNode(path, (isExclusive ? FindNodeFlags.NodeExclusive : FindNodeFlags.NodeShared) | FindNodeFlags.KeepParentNodeExclusive, share);
	            if (result.Directory is null)
	            {
	                ExitFindNode(result);
	                throw NewDirectoryNotFoundException(path);
	            }
	            if (result.Node is DirectoryNode || (isWriting && result.Node != null && result.Node.IsReadOnly))
	            {
	                ExitFindNode(result);
	                throw new UnauthorizedAccessException($"Access to the path `{path}` is denied.");
	            }
	            var filename = result.Name;
	            parentDirectory = result.Directory;
	            var srcNode = result.Node;
	            var fileNode = (FileNode)srcNode!;
	            // Append: Opens the file if it exists and seeks to the end of the file, or creates a new file. 
	            //         This requires FileIOPermissionAccess.Append permission. FileMode.Append can be used only in 
	            //         conjunction with FileAccess.Write. Trying to seek to a position before the end of the file 
	            //         throws an IOException exception, and any attempt to read fails and throws a 
	            //         NotSupportedException exception.
	            //
	            //
	            // CreateNew: Specifies that the operating system should create a new file.This requires 
	            //            FileIOPermissionAccess.Write permission. If the file already exists, an IOException 
	            //            exception is thrown.
	            //
	            // Open: Specifies that the operating system should open an existing file. The ability to open 
	            //       the file is dependent on the value specified by the FileAccess enumeration. 
	            //       A System.IO.FileNotFoundException exception is thrown if the file does not exist.
	            //
	            // OpenOrCreate: Specifies that the operating system should open a file if it exists; 
	            //               otherwise, a new file should be created. If the file is opened with 
	            //               FileAccess.Read, FileIOPermissionAccess.Read permission is required. 
	            //               If the file access is FileAccess.Write, FileIOPermissionAccess.Write permission 
	            //               is required. If the file is opened with FileAccess.ReadWrite, both 
	            //               FileIOPermissionAccess.Read and FileIOPermissionAccess.Write permissions 
	            //               are required. 
	            //
	            // Truncate: Specifies that the operating system should open an existing file. 
	            //           When the file is opened, it should be truncated so that its size is zero bytes. 
	            //           This requires FileIOPermissionAccess.Write permission. Attempts to read from a file 
	            //           opened with FileMode.Truncate cause an ArgumentException exception.
	            // Create: Specifies that the operating system should create a new file.If the file already exists, 
	            //         it will be overwritten.This requires FileIOPermissionAccess.Write permission. 
	            //         FileMode.Create is equivalent to requesting that if the file does not exist, use CreateNew; 
	            //         otherwise, use Truncate. If the file already exists but is a hidden file, 
	            //         an UnauthorizedAccessException exception is thrown.
	            bool shouldTruncate = false;
	            bool shouldAppend = false;
	            if (mode == FileMode.Create)
	            {
	                if (fileNode != null)
	                {
	                    mode = FileMode.Open;
	                    shouldTruncate = true;
	                }
	                else
	                {
	                    mode = FileMode.CreateNew;
	                }
	            }
	            if (mode == FileMode.OpenOrCreate)
	            {
	                mode = fileNode != null ? FileMode.Open : FileMode.CreateNew;
	            }
	            if (mode == FileMode.Append)
	            {
	                if (fileNode != null)
	                {
	                    mode = FileMode.Open;
	                    shouldAppend = true;
	                }
	                else
	                {
	                    mode = FileMode.CreateNew;
	                }
	            }
	            if (mode == FileMode.Truncate)
	            {
	                if (fileNode != null)
	                {
	                    mode = FileMode.Open;
	                    shouldTruncate = true;
	                }
	                else
	                {
	                    throw NewFileNotFoundException(path);
	                }
	            }
	            // Here we should only have Open or CreateNew
	            Debug.Assert(mode == FileMode.Open || mode == FileMode.CreateNew);
	            if (mode == FileMode.CreateNew)
	            {
	                // This is not completely accurate to throw an exception (as we have been called with an option to OpenOrCreate)
	                // But we assume that between the beginning of the method and here, the filesystem is not changing, and 
	                // if it is, it is an unfortunate conrurrency
	                if (fileNode != null)
	                {
	                    fileNodeToRelease = fileNode;
	                    throw NewDestinationFileExistException(path);
	                }
	                fileNode = new FileNode(this, parentDirectory, filename, null);
	                TryGetDispatcher()?.RaiseCreated(path);
	                if (isExclusive)
	                {
	                    EnterExclusive(fileNode, path);
	                }
	                else
	                {
	                    EnterShared(fileNode, path, share);
	                }
	            }
	            else
	            {
	                if (fileNode is null)
	                {
	                    throw NewFileNotFoundException(path);
	                }
	                ExitExclusive(parentDirectory);
	                parentDirectory = null;
	            }
	            // TODO: Add checks between mode and access
	            // Create a memory file stream
	            var stream = new MemoryFileStream(this, fileNode, isReading, isWriting, isExclusive);
	            if (shouldAppend)
	            {
	                stream.Position = stream.Length;
	            }
	            else if (shouldTruncate)
	            {
	                stream.SetLength(0);
	            }
	            return stream;
	        }
	        finally
	        {
	            if (fileNodeToRelease != null)
	            {
	                if (isExclusive)
	                {
	                    ExitExclusive(fileNodeToRelease);
	                }
	                else
	                {
	                    ExitShared(fileNodeToRelease);
	                }
	            }
	            if (parentDirectory != null)
	            {
	                ExitExclusive(parentDirectory);
	            }
	            ExitFileSystemShared();
	        }
	    }
	    // ----------------------------------------------
	    // Metadata API
	    // ----------------------------------------------
	    protected override FileAttributes GetAttributesImpl(UPath path)
	    {
	        var node = FindNodeSafe(path, false);
	        var attributes = node.Attributes;
	        if (node is DirectoryNode)
	        {
	            attributes |= FileAttributes.Directory;
	        }
	        else if (attributes == 0)
	        {
	            // If this is a file and there is no attributes, return Normal
	            attributes = FileAttributes.Normal;
	        }
	        return attributes;
	    }
	    protected override void SetAttributesImpl(UPath path, FileAttributes attributes)
	    {
	        // We don't store the attributes Normal or directory
	        // As they are returned by GetAttributes and we don't want
	        // to duplicate the information with the type inheritance (FileNode or DirectoryNode)
	        attributes &= ~FileAttributes.Normal;
	        attributes &= ~FileAttributes.Directory;
	        var node = FindNodeSafe(path, false);
	        node.Attributes = attributes;
	        TryGetDispatcher()?.RaiseChange(path);
	    }
	    protected override DateTime GetCreationTimeImpl(UPath path)
	    {
	        return TryFindNodeSafe(path)?.CreationTime ?? DefaultFileTime;
	    }
	    protected override void SetCreationTimeImpl(UPath path, DateTime time)
	    {
	        FindNodeSafe(path, false).CreationTime = time;
	        TryGetDispatcher()?.RaiseChange(path);
	    }
	    protected override DateTime GetLastAccessTimeImpl(UPath path)
	    {
	        return TryFindNodeSafe(path)?.LastAccessTime ?? DefaultFileTime;
	    }
	    protected override void SetLastAccessTimeImpl(UPath path, DateTime time)
	    {
	        FindNodeSafe(path, false).LastAccessTime = time;
	        TryGetDispatcher()?.RaiseChange(path);
	    }
	    protected override DateTime GetLastWriteTimeImpl(UPath path)
	    {
	        return TryFindNodeSafe(path)?.LastWriteTime ?? DefaultFileTime;
	    }
	    protected override void SetLastWriteTimeImpl(UPath path, DateTime time)
	    {
	        FindNodeSafe(path, false).LastWriteTime = time;
	        TryGetDispatcher()?.RaiseChange(path);
	    }
	    protected override void CreateSymbolicLinkImpl(UPath path, UPath pathToTarget)
	    {
	        throw new NotSupportedException("Symbolic links are not supported by MemoryFileSystem");
	    }
	    protected override bool TryResolveLinkTargetImpl(UPath linkPath, out UPath resolvedPath)
	    {
	        resolvedPath = default;
	        return false;
	    }
	    // ----------------------------------------------
	    // Search API
	    // ----------------------------------------------
	    protected override IEnumerable<UPath> EnumeratePathsImpl(UPath path, string searchPattern, SearchOption searchOption, SearchTarget searchTarget)
	    {
	        var search = SearchPattern.Parse(ref path, ref searchPattern);
	        var foldersToProcess = new List<UPath>();
	        foldersToProcess.Add(path);
	        var entries = new SortedSet<UPath>(UPath.DefaultComparerIgnoreCase);
	        while (foldersToProcess.Count > 0)
	        {
	            var directoryPath = foldersToProcess[0];
	            foldersToProcess.RemoveAt(0);
	            int dirIndex = 0;
	            entries.Clear();
	            // This is important that here we don't lock the FileSystemShared
	            // or the visited folder while returning a yield otherwise the finally
	            // may never be executed if the caller of this method decide to not
	            // Dispose the IEnumerable (because the generated IEnumerable
	            // doesn't have a finalizer calling Dispose)
	            // This is why the yield is performed outside this block
	            EnterFileSystemShared();
	            try
	            {
	                var result = EnterFindNode(directoryPath, FindNodeFlags.NodeShared);
	                try
	                {
	                    if (directoryPath == path)
	                    {
	                        // The first folder must be a directory, if it is not, throw an error
	                        ValidateDirectory(result.Node, directoryPath);
	                    }
	                    else
	                    {
	                        // Might happen during the time a DirectoryNode is enqueued into foldersToProcess
	                        // and the time we are going to actually visit it, it might have been
	                        // removed in the meantime, so we make sure here that we have a folder
	                        // and we don't throw an error if it is not
	                        if (result.Node is not DirectoryNode)
	                        {
	                            continue;
	                        }
	                    }
	                    var directory = (DirectoryNode)result.Node;
	                    foreach (var nodePair in directory.Children)
	                    {
	                        if (nodePair.Value is FileNode && searchTarget == SearchTarget.Directory)
	                        {
	                            continue;
	                        }
	                        var isEntryMatching = search.Match(nodePair.Key);
	                        var canFollowFolder = searchOption == SearchOption.AllDirectories && nodePair.Value is DirectoryNode;
	                        var addEntry = (nodePair.Value is FileNode && searchTarget != SearchTarget.Directory && isEntryMatching)
	                                       || (nodePair.Value is DirectoryNode && searchTarget != SearchTarget.File && isEntryMatching);
	                        var fullPath = directoryPath / nodePair.Key;
	                        if (canFollowFolder)
	                        {
	                            foldersToProcess.Insert(dirIndex++, fullPath);
	                        }
	                        if (addEntry)
	                        {
	                            entries.Add(fullPath);
	                        }
	                    }
	                }
	                finally
	                {
	                    ExitFindNode(result);
	                }
	            }
	            finally
	            {
	                ExitFileSystemShared();
	            }
	            // We return all the elements of visited directory in one shot, outside the previous lock block
	            foreach (var entry in entries)
	            {
	                yield return entry;
	            }
	        }
	    }
	    protected override IEnumerable<FileSystemItem> EnumerateItemsImpl(UPath path, SearchOption searchOption, SearchPredicate? searchPredicate)
	    {
	        var foldersToProcess = new List<UPath>();
	        foldersToProcess.Add(path);
	        var entries = new List<FileSystemItem>();
	        while (foldersToProcess.Count > 0)
	        {
	            var directoryPath = foldersToProcess[0];
	            foldersToProcess.RemoveAt(0);
	            int dirIndex = 0;
	            entries.Clear();
	            // This is important that here we don't lock the FileSystemShared
	            // or the visited folder while returning a yield otherwise the finally
	            // may never be executed if the caller of this method decide to not
	            // Dispose the IEnumerable (because the generated IEnumerable
	            // doesn't have a finalizer calling Dispose)
	            // This is why the yield is performed outside this block
	            EnterFileSystemShared();
	            try
	            {
	                var result = EnterFindNode(directoryPath, FindNodeFlags.NodeShared);
	                try
	                {
	                    if (directoryPath == path)
	                    {
	                        // The first folder must be a directory, if it is not, throw an error
	                        ValidateDirectory(result.Node, directoryPath);
	                    }
	                    else
	                    {
	                        // Might happen during the time a DirectoryNode is enqueued into foldersToProcess
	                        // and the time we are going to actually visit it, it might have been
	                        // removed in the meantime, so we make sure here that we have a folder
	                        // and we don't throw an error if it is not
	                        if (result.Node is not DirectoryNode)
	                        {
	                            continue;
	                        }
	                    }
	                    var directory = (DirectoryNode)result.Node;
	                    foreach (var nodePair in directory.Children)
	                    {
	                        var node = nodePair.Value;
	                        var canFollowFolder = searchOption == SearchOption.AllDirectories && nodePair.Value is DirectoryNode;
	                        var fullPath = directoryPath / nodePair.Key;
	                        if (canFollowFolder)
	                        {
	                            foldersToProcess.Insert(dirIndex++, fullPath);
	                        }
	                        var item = new FileSystemItem
	                        {
	                            FileSystem = this,
	                            AbsolutePath = fullPath,
	                            Path = fullPath,
	                            Attributes = node.Attributes,
	                            CreationTime = node.CreationTime,
	                            LastWriteTime = node.LastWriteTime,
	                            LastAccessTime = node.LastAccessTime,
	                            Length = node is FileNode fileNode ? fileNode.Content.Length : 0,
	                        };
	                        if (searchPredicate == null || searchPredicate(ref item))
	                        {
	                            entries.Add(item);
	                        }
	                    }
	                }
	                finally
	                {
	                    ExitFindNode(result);
	                }
	            }
	            finally
	            {
	                ExitFileSystemShared();
	            }
	            // We return all the elements of visited directory in one shot, outside the previous lock block
	            foreach (var entry in entries)
	            {
	                yield return entry;
	            }
	        }
	    }
	    // ----------------------------------------------
	    // Watch API
	    // ----------------------------------------------
	    protected override IFileSystemWatcher WatchImpl(UPath path)
	    {
	        var watcher = new Watcher(this, path);
	        GetOrCreateDispatcher().Add(watcher);
	        return watcher;
	    }
	    private class Watcher : FileSystemWatcher
	    {
	        private readonly MemoryFileSystem _fileSystem;
	        public Watcher(MemoryFileSystem fileSystem, UPath path)
	            : base(fileSystem, path)
	        {
	            _fileSystem = fileSystem;
	        }
	        protected override void Dispose(bool disposing)
	        {
	            if (disposing && !_fileSystem.IsDisposing)
	            {
	                _fileSystem.TryGetDispatcher()?.Remove(this);
	            }
	        }
	    }
	    // ----------------------------------------------
	    // Path API
	    // ----------------------------------------------
	    protected override string ConvertPathToInternalImpl(UPath path)
	    {
	        return path.FullName;
	    }
	    protected override UPath ConvertPathFromInternalImpl(string innerPath)
	    {
	        return new UPath(innerPath);
	    }
	    // ----------------------------------------------
	    // Internals
	    // ----------------------------------------------
	    private void MoveFileOrDirectory(UPath srcPath, UPath destPath, bool expectDirectory)
	    {
	        var parentSrcPath = srcPath.GetDirectory();
	        var parentDestPath = destPath.GetDirectory();
	        void AssertNoDestination(FileSystemNode? node)
	        {
	            if (expectDirectory)
	            {
	                if (node is FileNode || node != null)
	                {
	                    throw NewDestinationFileExistException(destPath);
	                }
	            }
	            else
	            {
	                if (node is DirectoryNode || node != null)
	                {
	                    throw NewDestinationDirectoryExistException(destPath);
	                }
	            }
	        }
	        // Same directory move
	        bool isSamefolder = parentSrcPath == parentDestPath;
	        // Check that Destination folder is not a subfolder of source directory
	        if (!isSamefolder && expectDirectory)
	        {
	            var checkParentDestDirectory = destPath.GetDirectory();
	            while (!checkParentDestDirectory.IsNull)
	            {
	                if (checkParentDestDirectory == srcPath)
	                {
	                    throw new IOException($"Cannot move the source directory `{srcPath}` to a a sub-folder of itself `{destPath}`");
	                }
	                checkParentDestDirectory = checkParentDestDirectory.GetDirectory();
	            }
	        }
	        // We need to take the lock on the folders in the correct order to avoid deadlocks
	        // So we sort the srcPath and destPath in alphabetical order
	        // (if srcPath is a subFolder of destPath, we will lock first destPath parent Folder, and then srcFolder)
	        bool isLockInverted = !isSamefolder && string.Compare(srcPath.FullName, destPath.FullName, StringComparison.Ordinal) > 0;
	        if (isSamefolder)
	        {
	            EnterFileSystemShared();
	        }
	        else
	        {
	            EnterFileSystemExclusive();
	        }
	        try
	        {
	            var srcResult = new NodeResult();
	            var destResult = new NodeResult();
	            try
	            {
	                if (isLockInverted)
	                {
	                    destResult = EnterFindNode(destPath, FindNodeFlags.KeepParentNodeExclusive | FindNodeFlags.NodeShared);
	                    srcResult = EnterFindNode(srcPath, FindNodeFlags.KeepParentNodeExclusive | FindNodeFlags.NodeExclusive, destResult);
	                }
	                else
	                {
	                    srcResult = EnterFindNode(srcPath, FindNodeFlags.KeepParentNodeExclusive | FindNodeFlags.NodeExclusive);
	                    destResult = EnterFindNode(destPath, FindNodeFlags.KeepParentNodeExclusive | FindNodeFlags.NodeShared, srcResult);
	                }
	                if (expectDirectory)
	                {
	                    ValidateDirectory(srcResult.Node, srcPath);
	                }
	                else
	                {
	                    ValidateFile(srcResult.Node, srcPath);
	                }
	                ValidateDirectory(destResult.Directory, destPath);
	                AssertNoDestination(destResult.Node);
	                srcResult.Node.DetachFromParent();
	                srcResult.Node.AttachToParent(destResult.Directory, destResult.Name!);
	                TryGetDispatcher()?.RaiseDeleted(srcPath);
	                TryGetDispatcher()?.RaiseCreated(destPath);
	            }
	            finally
	            {
	                if (isLockInverted)
	                {
	                    ExitFindNode(srcResult);
	                    ExitFindNode(destResult);
	                }
	                else
	                {
	                    ExitFindNode(destResult);
	                    ExitFindNode(srcResult);
	                }
	            }
	        }
	        finally
	        {
	            if (isSamefolder)
	            {
	                ExitFileSystemShared();
	            }
	            else
	            {
	                ExitFileSystemExclusive();
	            }
	        }
	    }
	    private static void ValidateDirectory([NotNull] FileSystemNode? node, UPath srcPath)
	    {
	        if (node is FileNode)
	        {
	            throw new IOException($"The source directory `{srcPath}` is a file");
	        }
	        if (node is null)
	        {
	            throw NewDirectoryNotFoundException(srcPath);
	        }
	    }
	    private static void ValidateFile([NotNull] FileSystemNode? node, UPath srcPath)
	    {
	        if (node is null)
	        {
	            throw NewFileNotFoundException(srcPath);
	        }
	    }
	    private FileSystemNode? TryFindNodeSafe(UPath path)
	    {
	        EnterFileSystemShared();
	        try
	        {
	            var result = EnterFindNode(path, FindNodeFlags.NodeShared);
	            try
	            {
	                var node = result.Node;
	                return node;
	            }
	            finally
	            {
	                ExitFindNode(result);
	            }
	        }
	        finally
	        {
	            ExitFileSystemShared();
	        }
	    }
	    private FileSystemNode FindNodeSafe(UPath path, bool expectFileOnly)
	    {
	        var node = TryFindNodeSafe(path);
	        if (node is null)
	        {
	            if (expectFileOnly)
	            {
	                throw NewFileNotFoundException(path);
	            }
	            throw new IOException($"The file or directory `{path}` was not found");
	        }
	        if (node is DirectoryNode)
	        {
	            if (expectFileOnly)
	            {
	                throw NewFileNotFoundException(path);
	            }
	        }
	        return node;
	    }
	    private void CreateDirectoryNode(UPath path)
	    {
	        ExitFindNode(EnterFindNode(path, FindNodeFlags.CreatePathIfNotExist | FindNodeFlags.NodeShared));
	    }
	    private readonly struct NodeResult
	    {
	        public NodeResult(DirectoryNode? directory, FileSystemNode? node, string? name, FindNodeFlags flags)
	        {
	            Directory = directory;
	            Node = node;
	            Name = name;
	            Flags = flags;
	        }
	        public readonly DirectoryNode? Directory;
	        public readonly FileSystemNode? Node;
	        public readonly string? Name;
	        public readonly FindNodeFlags Flags;
	    }
	    [Flags]
	    private enum FindNodeFlags
	    {
	        CreatePathIfNotExist = 1 << 1,
	        NodeCheck = 1 << 2,
	        NodeShared = 1 << 3,
	        NodeExclusive = 1 << 4,
	        KeepParentNodeExclusive = 1 << 5,
	        KeepParentNodeShared = 1 << 6,
	    }
	    private void ExitFindNode(in NodeResult nodeResult)
	    {
	        var flags = nodeResult.Flags;
	        // Unlock first the node
	        if (nodeResult.Node != null)
	        {
	            if ((flags & FindNodeFlags.NodeExclusive) != 0)
	            {
	                ExitExclusive(nodeResult.Node);
	            }
	            else if ((flags & FindNodeFlags.NodeShared) != 0)
	            {
	                ExitShared(nodeResult.Node);
	            }
	        }
	        if (nodeResult.Directory is null)
	        {
	            return;
	        }
	        // Unlock the parent directory if necessary
	        if ((flags & FindNodeFlags.KeepParentNodeExclusive) != 0)
	        {
	            ExitExclusive(nodeResult.Directory);
	        }
	        else if ((flags & FindNodeFlags.KeepParentNodeShared) != 0)
	        {
	            ExitShared(nodeResult.Directory);
	        }
	    }
	    private NodeResult EnterFindNode(UPath path, FindNodeFlags flags, params NodeResult[] existingNodes)
	    {
	        return EnterFindNode(path, flags, null, existingNodes);
	    }
	    private NodeResult EnterFindNode(UPath path, FindNodeFlags flags, FileShare? share, params NodeResult[] existingNodes)
	    {
	        // TODO: Split the flags between parent and node to make the code more clear
	        var result = new NodeResult();
	        // This method should be always called with at least one of these
	        Debug.Assert((flags & (FindNodeFlags.NodeExclusive|FindNodeFlags.NodeShared|FindNodeFlags.NodeCheck)) != 0);
	        var sharePath = share ?? FileShare.Read;
	        bool isLockOnRootAlreadyTaken = IsNodeAlreadyLocked(_rootDirectory, existingNodes);
	        // Even if it is not valid, the EnterFindNode may be called with a root directory
	        // So we handle it as a special case here
	        if (path == UPath.Root)
	        {
	            if (!isLockOnRootAlreadyTaken)
	            {
	                if ((flags & FindNodeFlags.NodeExclusive) != 0)
	                {
	                    EnterExclusive(_rootDirectory, path);
	                }
	                else if ((flags & FindNodeFlags.NodeShared) != 0)
	                {
	                    EnterShared(_rootDirectory, path, sharePath);
	                }
	            }
	            else
	            {
	                // If the lock was already taken, we make sure that NodeResult
	                // will not try to release it
	                flags &= ~(FindNodeFlags.NodeExclusive | FindNodeFlags.NodeShared);
	            }
	            result = new NodeResult(null, _rootDirectory, null, flags);
	            return result;
	        }
	        var isRequiringExclusiveLockForParent = (flags & (FindNodeFlags.CreatePathIfNotExist | FindNodeFlags.KeepParentNodeExclusive)) != 0;
	        var parentNode = _rootDirectory;
	        var names = path.Split();
	        // Walking down the nodes in locking order:
	        // /a/b/c.txt
	        //
	        // Lock /
	        // Lock /a
	        // Unlock /
	        // Lock /a/b
	        // Unlock /a
	        // Lock /a/b/c.txt
	        // Start by locking the parent directory (only if it is not already locked)
	        bool isParentLockTaken = false;
	        if (!isLockOnRootAlreadyTaken)
	        {
	            EnterExclusiveOrSharedDirectoryOrBlock(_rootDirectory, path, isRequiringExclusiveLockForParent);
	            isParentLockTaken = true;
	        }
	        for (var i = 0; i < names.Count && parentNode != null; i++)
	        {
	            var name = names[i];
	            bool isLast = i + 1 == names.Count;
	            DirectoryNode? nextParent = null;
	            bool isNextParentLockTaken = false;
	            try
	            {
	                FileSystemNode? subNode;
	                if (!parentNode.Children.TryGetValue(name, out subNode))
	                {
	                    if ((flags & FindNodeFlags.CreatePathIfNotExist) != 0)
	                    {
	                        subNode = new DirectoryNode(this, parentNode, name);
	                    }
	                }
	                else
	                {
	                    // If we are trying to create a directory and one of the node on the way is a file
	                    // this is an error
	                    if ((flags & FindNodeFlags.CreatePathIfNotExist) != 0 && subNode is FileNode)
	                    {
	                        throw new IOException($"Cannot create directory `{path}` on an existing file");
	                    }
	                }
	                // Special case of the last entry
	                if (isLast)
	                {
	                    // If the lock was not taken by the parent, modify the flags 
	                    // so that Exit(NodeResult) will not try to release the lock on the parent
	                    if (!isParentLockTaken)
	                    {
	                        flags &= ~(FindNodeFlags.KeepParentNodeExclusive | FindNodeFlags.KeepParentNodeShared);
	                    }
	                    result = new NodeResult(parentNode, subNode, name, flags);
	                    // The last subnode may be null but we still want to return a valid parent
	                    // otherwise, lock the final node if necessary
	                    if (subNode != null)
	                    {
	                        if ((flags & FindNodeFlags.NodeExclusive) != 0)
	                        {
	                            EnterExclusive(subNode, path);
	                        }
	                        else if ((flags & FindNodeFlags.NodeShared) != 0)
	                        {
	                            EnterShared(subNode, path, sharePath);
	                        }
	                    }
	                    // After we have taken the lock, and we need to keep a lock on the parent, make sure
	                    // that the finally {} below will not unlock the parent
	                    // This is important to perform this here, as the previous EnterExclusive/EnterShared
	                    // could have failed (e.g trying to lock exclusive on a file already locked)
	                    // and thus, we would have to release the lock of the parent in finally
	                    if ((flags & (FindNodeFlags.KeepParentNodeExclusive | FindNodeFlags.KeepParentNodeShared)) != 0)
	                    {
	                        parentNode = null;
	                        break;
	                    }
	                }
	                else
	                {
	                    // Going down the directory, 
	                    nextParent = subNode as DirectoryNode;
	                    if (nextParent != null && !IsNodeAlreadyLocked(nextParent, existingNodes))
	                    {
	                        EnterExclusiveOrSharedDirectoryOrBlock(nextParent, path, isRequiringExclusiveLockForParent);
	                        isNextParentLockTaken = true;
	                    }
	                }
	            }
	            finally
	            {
	                // We unlock the parent only if it was taken
	                if (isParentLockTaken && parentNode != null)
	                {
	                    ExitExclusiveOrShared(parentNode, isRequiringExclusiveLockForParent);
	                }
	            }
	            parentNode = nextParent;
	            isParentLockTaken = isNextParentLockTaken;
	        }
	        return result;
	    }
	    private static bool IsNodeAlreadyLocked(DirectoryNode directoryNode, NodeResult[] existingNodes)
	    {
	        foreach (var existingNode in existingNodes)
	        {
	            if (existingNode.Directory == directoryNode || existingNode.Node == directoryNode)
	            {
	                return true;
	            }
	        }
	        return false;
	    }
	    private FileSystemEventDispatcher<Watcher> GetOrCreateDispatcher()
	    {
	        lock (_dispatcherLock)
	        {
	            _dispatcher ??= new FileSystemEventDispatcher<Watcher>(this);
	            return _dispatcher;
	        }
	    }
	    private FileSystemEventDispatcher<Watcher>? TryGetDispatcher()
	    {
	        lock (_dispatcherLock)
	        {
	            return _dispatcher;
	        }
	    }
	    // ----------------------------------------------
	    // Locks internals
	    // ----------------------------------------------
	    private void EnterFileSystemShared()
	    {
	        _globalLock.EnterShared(UPath.Root);
	    }
	    private void ExitFileSystemShared()
	    {
	        _globalLock.ExitShared();
	    }
	    private void EnterFileSystemExclusive()
	    {
	        _globalLock.EnterExclusive();
	    }
	    private void ExitFileSystemExclusive()
	    {
	        _globalLock.ExitExclusive();
	    }
	    private void EnterSharedDirectoryOrBlock(DirectoryNode node, UPath context)
	    {
	        EnterShared(node, context, true, FileShare.Read);
	    }
	    private void EnterExclusiveOrSharedDirectoryOrBlock(DirectoryNode node, UPath context, bool isExclusive)
	    {
	        if (isExclusive)
	        {
	            EnterExclusiveDirectoryOrBlock(node, context);
	        }
	        else
	        {
	            EnterSharedDirectoryOrBlock(node, context);
	        }
	    }
	    private void EnterExclusiveDirectoryOrBlock(DirectoryNode node, UPath context)
	    {
	        EnterExclusive(node, context, true);
	    }
	    private void EnterExclusive(FileSystemNode node, UPath context)
	    {
	        EnterExclusive(node, context, node is DirectoryNode);
	    }
	    private void EnterShared(FileSystemNode node, UPath context, FileShare share)
	    {
	        EnterShared(node, context, node is DirectoryNode, share);
	    }
	    private void EnterShared(FileSystemNode node, UPath context, bool block, FileShare share)
	    {
	        if (block)
	        {
	            node.EnterShared(share, context);
	        }
	        else if (!node.TryEnterShared(share))
	        {
	            var pathType = node is FileNode ? "file" : "directory";
	            throw new IOException($"The {pathType} `{context}` is already used for writing by another thread.");
	        }
	    }
	    private void ExitShared(FileSystemNode node)
	    {
	        node.ExitShared();
	    }
	    private void EnterExclusive(FileSystemNode node, UPath context, bool block)
	    {
	        if (block)
	        {
	            node.EnterExclusive();
	        }
	        else if(!node.TryEnterExclusive())
	        {
	            var pathType = node is FileNode ? "file" : "directory";
	            throw new IOException($"The {pathType} `{context}` is already locked.");
	        }
	    }
	    private void ExitExclusiveOrShared(FileSystemNode node, bool isExclusive)
	    {
	        if (isExclusive)
	        {
	            node.ExitExclusive();
	        }
	        else
	        {
	            node.ExitShared();
	        }
	    }
	    private void ExitExclusive(FileSystemNode node)
	    {
	        node.ExitExclusive();
	    }
	    private void TryLockExclusive(FileSystemNode node, ListFileSystemNodes locks, bool recursive, UPath context)
	    {
	        if (locks is null) throw new ArgumentNullException(nameof(locks));
	        if (node is DirectoryNode directory)
	        {
	            if (recursive)
	            {
	                foreach (var child in directory.Children)
	                {
	                    EnterExclusive(child.Value, context);
	                    var path = context / child.Key;
	                    locks.Add(child);
	                    TryLockExclusive(child.Value, locks, true, path);
	                }
	            }
	            else
	            {
	                if (directory.Children.Count > 0)
	                {
	                    throw new IOException($"The directory `{context}` is not empty");
	                }
	            }
	        }
	    }
	    private abstract class FileSystemNode : FileSystemNodeReadWriteLock
	    {
	        protected readonly MemoryFileSystem FileSystem;
	        protected FileSystemNode(MemoryFileSystem fileSystem, DirectoryNode? parentNode, string? name, FileSystemNode? copyNode)
	        {
	            Debug.Assert((parentNode is null) == string.IsNullOrEmpty(name));
	            FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
	            if (parentNode != null && name is { Length: > 0 })
	            {
	                Debug.Assert(parentNode.IsLocked);
	                parentNode.Children.Add(name, this);
	                Parent = parentNode;
	                Name = name;
	            }
	            if (copyNode != null && copyNode.Attributes != 0)
	            {
	                Attributes = copyNode.Attributes;
	            }
	            CreationTime = DateTime.Now;
	            LastWriteTime = copyNode?.LastWriteTime ?? CreationTime;
	            LastAccessTime = copyNode?.LastAccessTime ?? CreationTime;
	        }
	        public DirectoryNode? Parent { get; private set; }
	        public string? Name { get; private set; }
	        public FileAttributes Attributes { get; set; }
	        public DateTime CreationTime { get; set; }
	        public DateTime LastWriteTime { get; set; }
	        public DateTime LastAccessTime { get; set; }
	        public bool IsDisposed { get; set; }
	        public bool IsReadOnly => (Attributes & FileAttributes.ReadOnly) != 0;
	        public void DetachFromParent()
	        {
	            Debug.Assert(IsLocked);
	            var parent = Parent!;
	            Debug.Assert(parent.IsLocked);
	            parent.Children.Remove(Name!);
	            Parent = null!;
	            Name = null!;
	        }
	        public void AttachToParent(DirectoryNode parentNode, string name)
	        {
	            if (parentNode is null) 
	                throw new ArgumentNullException(nameof(parentNode));
	            if (string.IsNullOrEmpty(name)) 
	                throw new ArgumentNullException(nameof(name));
	            Debug.Assert(parentNode.IsLocked);
	            Debug.Assert(IsLocked);
	            Debug.Assert(Parent is null);
	            Parent = parentNode;
	            Parent.Children.Add(name, this);
	            Name = name;
	        }
	        public void Dispose()
	        {
	            Debug.Assert(IsLocked);
	            // In order to issue a Dispose, we need to have control on this node
	            IsDisposed = true;
	        }
	        public virtual FileSystemNode Clone(DirectoryNode? newParent, string? newName)
	        {
	            Debug.Assert((newParent is null) == string.IsNullOrEmpty(newName));
	            var clone = (FileSystemNode)Clone();
	            clone.Parent = newParent;
	            clone.Name = newName;
	            return clone;
	        }
	    }
	    private class ListFileSystemNodes : List<KeyValuePair<string, FileSystemNode>>, IDisposable
	    {
	        private readonly MemoryFileSystem _fs;
	        public ListFileSystemNodes(MemoryFileSystem fs)
	        {
	            _fs = fs ?? throw new ArgumentNullException(nameof(fs));
	        }
	        public void Dispose()
	        {
	            for (var i = this.Count - 1; i >= 0; i--)
	            {
	                var entry = this[i];
	                _fs.ExitExclusive(entry.Value);
	            }
	            Clear();
	        }
	    }
	    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + "(),nq}")]
	    [DebuggerTypeProxy(typeof(DebuggerProxyInternal))]
	    private class DirectoryNode : FileSystemNode
	    {
	        internal Dictionary<string, FileSystemNode> _children;
	        public DirectoryNode(MemoryFileSystem fileSystem) : base(fileSystem, null, null, null)
	        {
	            _children = new Dictionary<string, FileSystemNode>();
	            Attributes = FileAttributes.Directory;
	        }
	        public DirectoryNode(MemoryFileSystem fileSystem, DirectoryNode parent, string name) : base(fileSystem, parent, name, null)
	        {
	            Debug.Assert(parent != null);
	            _children = new Dictionary<string, FileSystemNode>();
	            Attributes = FileAttributes.Directory;
	        }
	        public Dictionary<string, FileSystemNode> Children
	        {
	            get
	            {
	                Debug.Assert(IsLocked);
	                return _children;
	            }
	        }
	        public override FileSystemNode Clone(DirectoryNode? newParent, string? newName)
	        {
	            var dir = (DirectoryNode)base.Clone(newParent, newName);
	            dir._children = new Dictionary<string, FileSystemNode>();
	            foreach (var name in _children.Keys)
	            {
	                dir._children[name] = _children[name].Clone(dir, name);
	            }
	            return dir;
	        }
	        public override string DebuggerDisplay()
	        {
	            return Name is null ? $"Count = {_children.Count}{base.DebuggerDisplay()}"  : $"Folder: {Name}, Count = {_children.Count}{base.DebuggerDisplay()}";
	        }
	        private sealed class DebuggerProxyInternal
	        {
	            private readonly DirectoryNode _directoryNode;
	            public DebuggerProxyInternal(DirectoryNode directoryNode)
	            {
	                _directoryNode = directoryNode;
	            }
	            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
	            public FileSystemNode[] Items => _directoryNode._children.Values.ToArray();
	        }
	    }
	    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + "(),nq}")]
	    private sealed class FileNode : FileSystemNode
	    {
	        public FileNode(MemoryFileSystem fileSystem, DirectoryNode parentNode, string? name, FileNode? copyNode)
	            : base(fileSystem, parentNode, name, copyNode)
	        {
	            if (copyNode != null)
	            {
	                Content = new FileContent(this, copyNode.Content);
	            }
	            else
	            {
	                // Mimic OS-specific attributes.
	                Attributes = PhysicalFileSystem.IsOnWindows ? FileAttributes.Archive : FileAttributes.Normal;
	                Content = new FileContent(this);
	            }
	        }
	        public FileContent Content { get; private set; }
	        public override FileSystemNode Clone(DirectoryNode? newParent, string? newName)
	        {
	            var copy = (FileNode)base.Clone(newParent, newName);
	            copy.Content = new FileContent(copy, Content);
	            return copy;
	        }
	        public override string DebuggerDisplay()
	        {
	            return $"File: {Name}, {Content.DebuggerDisplay()}{base.DebuggerDisplay()}";
	        }
	        public void ContentChanged()
	        {
	            var dispatcher = FileSystem.TryGetDispatcher();
	            if (dispatcher != null)
	            {
	                // TODO: cache this
	                var path = GeneratePath();
	                dispatcher.RaiseChange(path);
	            }
	        }
	        private UPath GeneratePath()
	        {
	            var builder = UPath.GetSharedStringBuilder();
	            FileSystemNode node = this;
	            var parent = Parent;
	            while (parent != null)
	            {
	                builder.Insert(0, node.Name);
	                builder.Insert(0, UPath.DirectorySeparator);
	                node = parent;
	                parent = parent.Parent;
	            }
	            return builder.ToString();
	        }
	    }
	    private sealed class FileContent
	    {
	        private readonly FileNode _fileNode;
	        private readonly MemoryStream _stream;
	        public FileContent(FileNode fileNode)
	        {
	            _fileNode = fileNode ?? throw new ArgumentNullException(nameof(fileNode));
	            _stream = new MemoryStream();
	        }
	        public FileContent(FileNode fileNode, FileContent copy)
	        {
	            _fileNode = fileNode ?? throw new ArgumentNullException(nameof(fileNode));
	            var length = copy.Length;
	            _stream = new MemoryStream(length <= int.MaxValue ? (int)length : int.MaxValue);
	            CopyFrom(copy);
	        }
	        public byte[] ToArray()
	        {
	            lock (this)
	            {
	                return _stream.ToArray();
	            }
	        }
	        public void CopyFrom(FileContent copy)
	        {
	            lock (this)
	            {
	                var length = copy.Length;
	                var buffer = copy.ToArray();
	                _stream.Position = 0;
	                _stream.Write(buffer, 0, buffer.Length);
	                _stream.Position = 0;
	                _stream.SetLength(length);
	            }
	        }
	        public int Read(long position, byte[] buffer, int offset, int count)
	        {
	            lock (this)
	            {
	                _stream.Position = position;
	                return _stream.Read(buffer, offset, count);
	            }
	        }
	        public void Write(long position, byte[] buffer, int offset, int count)
	        {
	            lock (this)
	            {
	                _stream.Position = position;
	                _stream.Write(buffer, offset, count);
	            }
	            _fileNode.ContentChanged();
	        }
	        public void SetPosition(long position)
	        {
	            lock (this)
	            {
	                _stream.Position = position;
	            }
	        }
	        public long Length
	        {
	            get
	            {
	                lock (this)
	                {
	                    return _stream.Length;
	                }
	            }
	            set
	            {
	                lock (this)
	                {
	                    _stream.SetLength(value);
	                }
	                _fileNode.ContentChanged();
	            }
	        }
	        public string DebuggerDisplay() => $"Size = {_stream.Length}";
	    }
	    private sealed class MemoryFileStream : Stream
	    {
	        private readonly MemoryFileSystem _fs;
	        private readonly FileNode _fileNode;
	        private readonly bool _canRead;
	        private readonly bool _canWrite;
	        private readonly bool _isExclusive;
	        private int _isDisposed;
	        private long _position;
	        public MemoryFileStream(MemoryFileSystem fs, FileNode fileNode, bool canRead, bool canWrite, bool isExclusive)
	        {
	            _fs = fs ?? throw new ArgumentNullException(nameof(fs));
	            _fileNode = fileNode ?? throw new ArgumentNullException(nameof(fs));
	            _canWrite = canWrite;
	            _canRead = canRead;
	            _isExclusive = isExclusive;
	            _position = 0;
	            Debug.Assert(fileNode.IsLocked);
	        }
	        public override bool CanRead => _isDisposed == 0 && _canRead;
	        public override bool CanSeek => _isDisposed == 0;
	        public override bool CanWrite => _isDisposed == 0 && _canWrite;
	        public override long Length
	        {
	            get
	            {
	                CheckNotDisposed();
	                return _fileNode.Content.Length;
	            }
	        }
	        public override long Position
	        {
	            get
	            {
	                CheckNotDisposed();
	                return _position;
	            }
	            set
	            {
	                CheckNotDisposed();
	                if (value < 0)
	                {
	                    throw new ArgumentOutOfRangeException("The position cannot be negative");
	                }
	                _position = value;
	                _fileNode.Content.SetPosition(_position);
	            }
	        }
	        ~MemoryFileStream()
	        {
	            Dispose(false);
	        }
	        protected override void Dispose(bool disposing)
	        {
	            if (Interlocked.Exchange(ref _isDisposed, 1) == 1)
	            {
	                return;
	            }
	            if (_isExclusive)
	            {
	                _fs.ExitExclusive(_fileNode);
	            }
	            else
	            {
	                _fs.ExitShared(_fileNode);
	            }
	            base.Dispose(disposing);
	        }
	        public override void Flush()
	        {
	            CheckNotDisposed();
	        }
	        public override int Read(byte[] buffer, int offset, int count)
	        {
	            CheckNotDisposed();
	            int readCount = _fileNode.Content.Read(_position, buffer, offset, count);
	            _position += readCount;
	            _fileNode.LastAccessTime = DateTime.Now;
	            return readCount;
	        }
	        public override long Seek(long offset, SeekOrigin origin)
	        {
	            CheckNotDisposed();
	            var newPosition = offset;
	            switch (origin)
	            {
	                case SeekOrigin.Current:
	                    newPosition += _position;
	                    break;
	                case SeekOrigin.End:
	                    newPosition += _fileNode.Content.Length;
	                    break;
	            }
	            if (newPosition < 0)
	            {
	                throw new IOException("An attempt was made to move the file pointer before the beginning of the file");
	            }
	            return _position = newPosition;
	        }
	        public override void SetLength(long value)
	        {
	            CheckNotDisposed();
	            _fileNode.Content.Length = value;
	            var time = DateTime.Now;
	            _fileNode.LastAccessTime = time;
	            _fileNode.LastWriteTime = time;
	        }
	        public override void Write(byte[] buffer, int offset, int count)
	        {
	            CheckNotDisposed();
	            _fileNode.Content.Write(_position, buffer, offset, count);
	            _position += count;
	            var time = DateTime.Now;
	            _fileNode.LastAccessTime = time;
	            _fileNode.LastWriteTime = time;
	        }
	        private void CheckNotDisposed()
	        {
	            if (_isDisposed > 0)
	            {
	                throw new ObjectDisposedException("Cannot access a closed file.");
	            }
	        }
	    }
	    private class FileSystemNodeReadWriteLock
	    {
	        // _sharedCount  < 0 => This is an exclusive lock (_sharedCount == -1)
	        // _sharedCount == 0 => No lock
	        // _sharedCount  > 0 => This is a shared lock
	        private int _sharedCount;
	        private FileShare? _shared;
	        internal bool IsLocked => _sharedCount != 0;
	        public void EnterShared(UPath context)
	        {
	            EnterShared(FileShare.Read, context);
	        }
	        protected FileSystemNodeReadWriteLock Clone()
	        {
	            var locker = (FileSystemNodeReadWriteLock)MemberwiseClone();
	            // Erase any locks
	            locker._sharedCount = 0;
	            locker._shared = null;
	            return locker;
	        }
	        public void EnterShared(FileShare share, UPath context)
	        {
	            Monitor.Enter(this);
	            try
	            {
	                while (_sharedCount < 0)
	                {
	                    Monitor.Wait(this);
	                }
	                if (_shared.HasValue)
	                {
	                    var currentShare = _shared.Value;
	                    // The previous share must be a superset of the shared being asked
	                    if ((share & currentShare) != share)
	                    {
	                        throw new UnauthorizedAccessException($"Cannot access shared resource path `{context}` with shared access`{share}` while current is `{currentShare}`");
	                    }
	                }
	                else
	                {
	                    _shared = share;
	                }
	                _sharedCount++;
	                Monitor.PulseAll(this);
	            }
	            finally
	            {
	                Monitor.Exit(this);
	            }
	        }
	        public void ExitShared()
	        {
	            Monitor.Enter(this);
	            try
	            {
	                Debug.Assert(_sharedCount > 0);
	                _sharedCount--;
	                if (_sharedCount == 0)
	                {
	                    _shared = null;
	                }
	                Monitor.PulseAll(this);
	            }
	            finally
	            {
	                Monitor.Exit(this);
	            }
	        }
	        public void EnterExclusive()
	        {
	            Monitor.Enter(this);
	            try
	            {
	                while (_sharedCount != 0)
	                {
	                    Monitor.Wait(this);
	                }
	                _sharedCount  = -1;
	                Monitor.PulseAll(this);
	            }
	            finally
	            {
	                Monitor.Exit(this);
	            }
	        }
	        public bool TryEnterShared(FileShare share)
	        {
	            Monitor.Enter(this);
	            try
	            {
	                if (_sharedCount < 0)
	                {
	                    return false;
	                }
	                if (_shared.HasValue)
	                {
	                    var currentShare = _shared.Value;
	                    if ((share & currentShare) != share)
	                    {
	                        return false;
	                    }
	                }
	                else
	                {
	                    _shared = share;
	                }
	                _sharedCount++;
	                Monitor.PulseAll(this);
	            }
	            finally
	            {
	                Monitor.Exit(this);
	            }
	            return true;
	        }
	        public bool TryEnterExclusive()
	        {
	            Monitor.Enter(this);
	            try
	            {
	                if (_sharedCount != 0)
	                {
	                    return false;
	                }
	                _sharedCount = -1;
	                Monitor.PulseAll(this);
	            }
	            finally
	            {
	                Monitor.Exit(this);
	            }
	            return true;
	        }
	        public void ExitExclusive()
	        {
	            Monitor.Enter(this);
	            try
	            {
	                Debug.Assert(_sharedCount < 0);
	                _sharedCount = 0;
	                Monitor.PulseAll(this);
	            }
	            finally
	            {
	                Monitor.Exit(this);
	            }
	        }
	        public virtual string DebuggerDisplay()
	        {
	            return _sharedCount < 0 ? " (exclusive lock)" : _sharedCount > 0 ? $" (shared lock: {_sharedCount})" : string.Empty;
	        }
	    }
	    private sealed class DebuggerProxy
	    {
	        private readonly MemoryFileSystem _fs;
	        public DebuggerProxy(MemoryFileSystem fs)
	        {
	            _fs = fs;
	        }
	        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
	        public FileSystemNode[] Items => _fs._rootDirectory._children.Select(x => x.Value).ToArray();
	    }
	}
	[DebuggerDisplay("{" + nameof(DebuggerDisplay) + "(),nq} Count={_mounts.Count}")]
	[DebuggerTypeProxy(typeof(DebuggerProxy))]
	public class MountFileSystem : ComposeFileSystem
	{
	    private readonly SortedList<UPath, IFileSystem> _mounts;
	    private readonly List<AggregateWatcher> _watchers;
	    public MountFileSystem(bool owned = true) : this(null, owned)
	    {
	    }
	    public MountFileSystem(IFileSystem? defaultBackupFileSystem, bool owned = true) : base(defaultBackupFileSystem, owned)
	    {
	        _mounts = new SortedList<UPath, IFileSystem>(new UPathLengthComparer());
	        _watchers = new List<AggregateWatcher>();
	    }
	    protected override void Dispose(bool disposing)
	    {
	        base.Dispose(disposing);
	        if (!disposing)
	        {
	            return;
	        }
	        foreach (var watcher in _watchers)
	        {
	            watcher.Dispose();
	        }
	        _watchers.Clear();
	        if (Owned)
	        {
	            foreach (var kvp in _mounts)
	            {
	                kvp.Value.Dispose();
	            }
	        }
	        _mounts.Clear();
	    }
	    public void Mount(UPath name, IFileSystem fileSystem)
	    {
	        if (fileSystem is null) throw new ArgumentNullException(nameof(fileSystem));
	        if (fileSystem == this)
	        {
	            throw new ArgumentException("Cannot recursively mount the filesystem to self", nameof(fileSystem));
	        }
	        ValidateMountName(name);
	        if (_mounts.ContainsKey(name))
	        {
	            throw new ArgumentException($"There is already a mount with the same name: `{name}`", nameof(name));
	        }
	        _mounts.Add(name, fileSystem);
	        foreach (var watcher in _watchers)
	        {
	            if (!IsMountIncludedInWatch(name, watcher.Path, out var remainingPath))
	            {
	                continue;
	            }
	            if (fileSystem.CanWatch(remainingPath))
	            {
	                var internalWatcher = fileSystem.Watch(remainingPath);
	                watcher.Add(new WrapWatcher(fileSystem, name, remainingPath, internalWatcher));
	            }
	        }
	    }
	    public bool IsMounted(UPath name)
	    {
	        ValidateMountName(name);
	        return _mounts.ContainsKey(name);
	    }
	    public Dictionary<UPath, IFileSystem> GetMounts()
	    {
	        var dict = new Dictionary<UPath, IFileSystem>();
	        foreach (var mount in _mounts)
	        {
	            dict.Add(mount.Key, mount.Value);
	        }
	        return dict;
	    }
	    public IFileSystem Unmount(UPath name)
	    {
	        ValidateMountName(name);
	        IFileSystem? mountFileSystem;
	        if (!_mounts.TryGetValue(name, out mountFileSystem))
	        {
	            throw new ArgumentException($"The mount with the name `{name}` was not found");
	        }
	        foreach (var watcher in _watchers)
	        {
	            watcher.RemoveFrom(mountFileSystem);
	        }
	        _mounts.Remove(name);
	        return mountFileSystem;
	    }
	    public bool TryGetMount(UPath path, out UPath name, out IFileSystem? fileSystem, out UPath? fileSystemPath)
	    {
	        path.AssertNotNull();
	        path.AssertAbsolute();
	        var fs = TryGetMountOrNext(ref path, out name);
	        if (fs is null || name.IsNull)
	        {
	            name = UPath.Null;
	            fileSystem = null;
	            fileSystemPath = null;
	            return false;
	        }
	        fileSystem = fs;
	        fileSystemPath = path;
	        return true;
	    }
	    public bool TryGetMountName(IFileSystem fileSystem, out UPath name)
	    {
	        if (fileSystem is null)
	            throw new ArgumentNullException(nameof(fileSystem));
	        foreach (var mount in _mounts)
	        {
	            if (mount.Value != fileSystem)
	                continue;
	            name = mount.Key;
	            return true;
	        }
	        name = UPath.Null;
	        return false;
	    }
	    protected override void CreateDirectoryImpl(UPath path)
	    {
	        var originalSrcPath = path;
	        var fs = TryGetMountOrNext(ref path);
	        if (fs != null && path != UPath.Root)
	        {
	            fs.CreateDirectory(path);
	        }
	        else
	        {
	            throw new UnauthorizedAccessException($"The access to path `{originalSrcPath}` is denied");
	        }
	    }
	    protected override bool DirectoryExistsImpl(UPath path)
	    {
	        if (path == UPath.Root)
	        {
	            return true;
	        }
	        var fs = TryGetMountOrNext(ref path);
	        if (fs != null)
	        {
	            return path == UPath.Root || fs.DirectoryExists(path);
	        }
	        // Check if the path is part of a mount name
	        foreach (var kvp in _mounts)
	        {
	            var remainingPath = GetRemaining(path, kvp.Key);
	            if (!remainingPath.IsNull)
	            {
	                return true;
	            }
	        }
	        return false;
	    }
	    protected override void MoveDirectoryImpl(UPath srcPath, UPath destPath)
	    {
	        var originalSrcPath = srcPath;
	        var originalDestPath = destPath;
	        var srcfs = TryGetMountOrNext(ref srcPath);
	        var destfs = TryGetMountOrNext(ref destPath);
	        if (srcfs != null && srcPath == UPath.Root)
	        {
	            throw new UnauthorizedAccessException($"Cannot move a mount directory `{originalSrcPath}`");
	        }
	        if (destfs != null && destPath == UPath.Root)
	        {
	            throw new UnauthorizedAccessException($"Cannot move a mount directory `{originalDestPath}`");
	        }
	        if (srcfs != null && srcfs == destfs)
	        {
	            srcfs.MoveDirectory(srcPath, destPath);
	        }
	        else
	        {
	            // TODO: Add support for Copy + Delete ?
	            throw new NotSupportedException($"Cannot move directory between mount `{originalSrcPath}` and `{originalDestPath}`");
	        }
	    }
	    protected override void DeleteDirectoryImpl(UPath path, bool isRecursive)
	    {
	        var originalSrcPath = path;
	        var mountfs = TryGetMountOrNext(ref path);
	        if (mountfs != null && path == UPath.Root)
	        {
	            throw new UnauthorizedAccessException($"Cannot delete mount directory `{originalSrcPath}`. Use Unmount() instead");
	        }
	        if (mountfs != null)
	        {
	            mountfs.DeleteDirectory(path, isRecursive);
	        }
	        else
	        {
	            throw NewDirectoryNotFoundException(originalSrcPath);
	        }
	    }
	    protected override void CopyFileImpl(UPath srcPath, UPath destPath, bool overwrite)
	    {
	        var originalSrcPath = srcPath;
	        var originalDestPath = destPath;
	        var srcfs = TryGetMountOrNext(ref srcPath);
	        var destfs = TryGetMountOrNext(ref destPath);
	        if (srcfs != null && destfs != null)
	        {
	            if (srcfs == destfs)
	            {
	                srcfs.CopyFile(srcPath, destPath, overwrite);
	            }
	            else
	            {
	                // Otherwise, perform a copy between filesystem
	                srcfs.CopyFileCross(srcPath, destfs, destPath, overwrite);
	            }
	        }
	        else
	        {
	            if (srcfs is null)
	            {
	                throw NewFileNotFoundException(originalSrcPath);
	            }
	            throw NewDirectoryNotFoundException(originalDestPath);
	        }
	    }
	    protected override void ReplaceFileImpl(UPath srcPath, UPath destPath, UPath destBackupPath, bool ignoreMetadataErrors)
	    {
	        var originalSrcPath = srcPath;
	        var originalDestPath = destPath;
	        var originalDestBackupPath = destBackupPath;
	        if (!FileExistsImpl(srcPath))
	        {
	            throw NewFileNotFoundException(srcPath);
	        }
	        if (!FileExistsImpl(destPath))
	        {
	            throw NewFileNotFoundException(destPath);
	        }
	        var srcfs = TryGetMountOrNext(ref srcPath);
	        var destfs = TryGetMountOrNext(ref destPath);
	        var backupfs = TryGetMountOrNext(ref destBackupPath);
	        if (srcfs != null && srcfs == destfs && (destBackupPath.IsNull || srcfs == backupfs))
	        {
	            srcfs.ReplaceFile(srcPath, destPath, destBackupPath, ignoreMetadataErrors);
	        }
	        else
	        {
	            // TODO: Add support for moving file between filesystems (Copy+Delete) ?
	            throw new NotSupportedException($"Cannot replace file between mount `{originalSrcPath}`, `{originalDestPath}` and `{originalDestBackupPath}`");
	        }
	    }
	    protected override long GetFileLengthImpl(UPath path)
	    {
	        var originalSrcPath = path;
	        var mountfs = TryGetMountOrNext(ref path);
	        if (mountfs != null)
	        {
	            return mountfs.GetFileLength(path);
	        }
	        throw NewFileNotFoundException(originalSrcPath);
	    }
	    protected override bool FileExistsImpl(UPath path)
	    {
	        var mountfs = TryGetMountOrNext(ref path);
	        return mountfs?.FileExists(path) ?? false;
	    }
	    protected override void MoveFileImpl(UPath srcPath, UPath destPath)
	    {
	        var originalSrcPath = srcPath;
	        var originalDestPath = destPath;
	        if (!FileExistsImpl(srcPath))
	        {
	            throw NewFileNotFoundException(srcPath);
	        }
	        var destDirectory = destPath.GetDirectory();
	        if (!DirectoryExistsImpl(destDirectory))
	        {
	            throw NewDirectoryNotFoundException(destDirectory);
	        }
	        if (FileExistsImpl(destPath))
	        {
	            throw new IOException($"The destination path `{destPath}` already exists");
	        }
	        var srcfs = TryGetMountOrNext(ref srcPath);
	        var destfs = TryGetMountOrNext(ref destPath);
	        if (srcfs != null && srcfs == destfs)
	        {
	            srcfs.MoveFile(srcPath, destPath);
	        }
	        else if (srcfs != null && destfs != null)
	        {
	            srcfs.MoveFileCross(srcPath, destfs, destPath);
	        }
	        else
	        {
	            if (srcfs is null)
	            {
	                throw NewFileNotFoundException(originalSrcPath);
	            }
	            throw NewDirectoryNotFoundException(originalDestPath);
	        }
	    }
	    protected override void DeleteFileImpl(UPath path)
	    {
	        var mountfs = TryGetMountOrNext(ref path);
	        mountfs?.DeleteFile(path);
	    }
	    protected override Stream OpenFileImpl(UPath path, FileMode mode, FileAccess access, FileShare share = FileShare.None)
	    {
	        var originalSrcPath = path;
	        var mountfs = TryGetMountOrNext(ref path);
	        if (mountfs != null)
	        {
	            return mountfs.OpenFile(path, mode, access, share);
	        }
	        if (mode == FileMode.Open || mode == FileMode.Truncate)
	        {
	            throw NewFileNotFoundException(originalSrcPath);
	        }
	        throw new UnauthorizedAccessException($"The access to path `{originalSrcPath}` is denied");
	    }
	    protected override FileAttributes GetAttributesImpl(UPath path)
	    {
	        var originalSrcPath = path;
	        var mountfs = TryGetMountOrNext(ref path);
	        if (mountfs != null)
	        {
	            return mountfs.GetAttributes(path);
	        }
	        throw NewFileNotFoundException(originalSrcPath);
	    }
	    protected override void SetAttributesImpl(UPath path, FileAttributes attributes)
	    {
	        var originalSrcPath = path;
	        var mountfs = TryGetMountOrNext(ref path);
	        if (mountfs != null)
	        {
	            mountfs.SetAttributes(path, attributes);
	        }
	        else
	        {
	            throw NewFileNotFoundException(originalSrcPath);
	        }
	    }
	    protected override DateTime GetCreationTimeImpl(UPath path)
	    {
	        return TryGetMountOrNext(ref path)?.GetCreationTime(path) ?? DefaultFileTime;
	    }
	    protected override void SetCreationTimeImpl(UPath path, DateTime time)
	    {
	        var originalSrcPath = path;
	        var mountfs = TryGetMountOrNext(ref path);
	        if (mountfs != null)
	        {
	            mountfs.SetCreationTime(path, time);
	        }
	        else
	        {
	            throw NewFileNotFoundException(originalSrcPath);
	        }
	    }
	    protected override DateTime GetLastAccessTimeImpl(UPath path)
	    {
	        return TryGetMountOrNext(ref path)?.GetLastAccessTime(path) ?? DefaultFileTime;
	    }
	    protected override void SetLastAccessTimeImpl(UPath path, DateTime time)
	    {
	        var originalSrcPath = path;
	        var mountfs = TryGetMountOrNext(ref path);
	        if (mountfs != null)
	        {
	            mountfs.SetLastAccessTime(path, time);
	        }
	        else
	        {
	            throw NewFileNotFoundException(originalSrcPath);
	        }
	    }
	    protected override DateTime GetLastWriteTimeImpl(UPath path)
	    {
	        return TryGetMountOrNext(ref path)?.GetLastWriteTime(path) ?? DefaultFileTime;
	    }
	    protected override void SetLastWriteTimeImpl(UPath path, DateTime time)
	    {
	        var originalSrcPath = path;
	        var mountfs = TryGetMountOrNext(ref path);
	        if (mountfs != null)
	        {
	            mountfs.SetLastWriteTime(path, time);
	        }
	        else
	        {
	            throw NewFileNotFoundException(originalSrcPath);
	        }
	    }
	    protected override void CreateSymbolicLinkImpl(UPath path, UPath pathToTarget)
	    {
	        var originalSrcPath = path;
	        var mountfs = TryGetMountOrNext(ref path);
	        var mountTargetfs = TryGetMountOrNext(ref pathToTarget);
	        if (mountfs != mountTargetfs)
	        {
	            throw new InvalidOperationException("Cannot create a symbolic link between two different filesystems");
	        }
	        if (mountfs != null)
	        {
	            mountfs.CreateSymbolicLink(path, pathToTarget);
	        }
	        else
	        {
	            throw NewFileNotFoundException(originalSrcPath);
	        }
	    }
	    protected override bool TryResolveLinkTargetImpl(UPath linkPath, out UPath resolvedPath)
	    {
	        var mountfs = TryGetMountOrNext(ref linkPath, out var mountPath);
	        if (mountfs is null)
	        {
	            resolvedPath = default;
	            return false;
	        }
	        if (!mountfs.TryResolveLinkTarget(linkPath, out var resolved))
	        {
	            resolvedPath = default;
	            return false;
	        }
	        resolvedPath = CombinePrefix(mountPath, resolved);
	        return true;
	    }
	    protected override (IFileSystem FileSystem, UPath Path) ResolvePathImpl(UPath path)
	    {
	        var mountfs = TryGetMountOrNext(ref path);
	        if (mountfs is null)
	        {
	            return base.ResolvePathImpl(path);
	        }
	        return mountfs.ResolvePath(path);
	    }
	    protected override IEnumerable<UPath> EnumeratePathsImpl(UPath path, string searchPattern, SearchOption searchOption, SearchTarget searchTarget)
	    {
	        // Use the search pattern to normalize the path/search pattern
	        var search = SearchPattern.Parse(ref path, ref searchPattern);
	        // Internal method used to retrieve the list of search locations
	        List<SearchLocation> GetSearchLocations(UPath basePath)
	        {
	            var locations = new List<SearchLocation>();
	            var matchedMount = false;
	            foreach (var kvp in _mounts)
	            {
	                // Check if path partially matches a mount name
	                var remainingPath = GetRemaining(basePath, kvp.Key);
	                if (!remainingPath.IsNull && remainingPath != UPath.Root)
	                {
	                    locations.Add(new SearchLocation(this, basePath, remainingPath));
	                    continue;
	                }
	                if (!matchedMount)
	                {
	                    // Check if path fully matches a mount name
	                    remainingPath = GetRemaining(kvp.Key, basePath);
	                    if (!remainingPath.IsNull)
	                    {
	                        matchedMount = true; // don't check other mounts, we don't want to merge them together
	                        if (kvp.Value.DirectoryExists(remainingPath))
	                        {
	                            locations.Add(new SearchLocation(kvp.Value, kvp.Key, remainingPath));
	                        }
	                    }
	                }
	            }
	            if (!matchedMount && Fallback != null && Fallback.DirectoryExists(basePath))
	            {
	                locations.Add(new SearchLocation(Fallback, UPath.Null, basePath));
	            }
	            return locations;
	        }
	        var directoryToVisit = new List<UPath>();
	        directoryToVisit.Add(path);
	        var entries = new SortedSet<UPath>();
	        var sortedDirectories = new SortedSet<UPath>();
	        var first = true;
	        while (directoryToVisit.Count > 0)
	        {
	            var pathToVisit = directoryToVisit[0];
	            directoryToVisit.RemoveAt(0);
	            var dirIndex = 0;
	            entries.Clear();
	            sortedDirectories.Clear();
	            var locations = GetSearchLocations(pathToVisit);
	            // Only need to search within one filesystem, no need to sort or do other work
	            if (locations.Count == 1 && locations[0].FileSystem != this && (!first || searchOption == SearchOption.AllDirectories))
	            {
	                var last = locations[0];
	                foreach (var item in last.FileSystem.EnumeratePaths(last.Path, searchPattern, searchOption, searchTarget))
	                {
	                    yield return CombinePrefix(last.Prefix, item);
	                }
	            }
	            else
	            {
	                for (var i = locations.Count - 1; i >= 0; i--)
	                {
	                    var location = locations[i];
	                    var fileSystem = location.FileSystem;
	                    var searchPath = location.Path;
	                    if (fileSystem == this)
	                    {
	                        // List a single part of a mount name, queue it to be visited if needed
	                        var mountPart = new UPath(searchPath.GetFirstDirectory(out _)).ToRelative();
	                        var mountPath = location.Prefix / mountPart;
	                        var isMatching = search.Match(mountPath);
	                        if (isMatching && searchTarget != SearchTarget.File)
	                        {
	                            entries.Add(mountPath);
	                        }
	                        if (searchOption == SearchOption.AllDirectories)
	                        {
	                            sortedDirectories.Add(mountPath);
	                        }
	                    }
	                    else
	                    {
	                        // List files in the mounted filesystems, merged and sorted into one list
	                        foreach (var item in fileSystem.EnumeratePaths(searchPath, "*", SearchOption.TopDirectoryOnly, SearchTarget.Both))
	                        {
	                            var publicName = CombinePrefix(location.Prefix, item);
	                            if (entries.Contains(publicName))
	                            {
	                                continue;
	                            }
	                            var isFile = fileSystem.FileExists(item);
	                            var isDirectory = fileSystem.DirectoryExists(item);
	                            var isMatching = search.Match(publicName);
	                            if (isMatching && ((isFile && searchTarget != SearchTarget.Directory) || (isDirectory && searchTarget != SearchTarget.File)))
	                            {
	                                entries.Add(publicName);
	                            }
	                            if (searchOption == SearchOption.AllDirectories && isDirectory)
	                            {
	                                sortedDirectories.Add(publicName);
	                            }
	                        }
	                    }
	                }
	            }
	            if (first)
	            {
	                if (locations.Count == 0 && path != UPath.Root)
	                    throw NewDirectoryNotFoundException(path);
	                first = false;
	            }
	            // Enqueue directories and respect order
	            foreach (var nextDir in sortedDirectories)
	            {
	                directoryToVisit.Insert(dirIndex++, nextDir);
	            }
	            // Return entries
	            foreach (var entry in entries)
	            {
	                yield return entry;
	            }
	        }
	    }
	    protected override IEnumerable<FileSystemItem> EnumerateItemsImpl(UPath path, SearchOption searchOption, SearchPredicate? searchPredicate)
	    {
	        // Internal method used to retrieve the list of search locations
	        List<SearchLocation> GetSearchLocations(UPath basePath)
	        {
	            var locations = new List<SearchLocation>();
	            var matchedMount = false;
	            foreach (var kvp in _mounts)
	            {
	                // Check if path partially matches a mount name
	                var remainingPath = GetRemaining(basePath, kvp.Key);
	                if (!remainingPath.IsNull && remainingPath != UPath.Root)
	                {
	                    locations.Add(new SearchLocation(this, basePath, remainingPath));
	                    continue;
	                }
	                if (!matchedMount)
	                {
	                    // Check if path fully matches a mount name
	                    remainingPath = GetRemaining(kvp.Key, basePath);
	                    if (!remainingPath.IsNull)
	                    {
	                        matchedMount = true; // don't check other mounts, we don't want to merge them together
	                        if (kvp.Value.DirectoryExists(remainingPath))
	                        {
	                            locations.Add(new SearchLocation(kvp.Value, kvp.Key, remainingPath));
	                        }
	                    }
	                }
	            }
	            if (!matchedMount && Fallback != null && Fallback.DirectoryExists(basePath))
	            {
	                locations.Add(new SearchLocation(Fallback, UPath.Null, basePath));
	            }
	            return locations;
	        }
	        var directoryToVisit = new List<UPath> {path};
	        var entries = new HashSet<UPath>();
	        var sortedDirectories = new SortedSet<UPath>();
	        var first = true;
	        while (directoryToVisit.Count > 0)
	        {
	            var pathToVisit = directoryToVisit[0];
	            directoryToVisit.RemoveAt(0);
	            var dirIndex = 0;
	            entries.Clear();
	            sortedDirectories.Clear();
	            var locations = GetSearchLocations(pathToVisit);
	            // Only need to search within one filesystem, no need to sort or do other work
	            if (locations.Count == 1 && locations[0].FileSystem != this && (!first || searchOption == SearchOption.AllDirectories))
	            {
	                var last = locations[0];
	                foreach (var item in last.FileSystem.EnumerateItems(last.Path, searchOption, searchPredicate))
	                {
	                    var localItem = item;
	                    localItem.Path = CombinePrefix(last.Prefix, item.Path);
	                    if (entries.Add(localItem.Path))
	                    {
	                        yield return localItem;
	                    }
	                }
	            }
	            else
	            {
	                for (var i = locations.Count - 1; i >= 0; i--)
	                {
	                    var location = locations[i];
	                    var fileSystem = location.FileSystem;
	                    var searchPath = location.Path;
	                    if (fileSystem == this)
	                    {
	                        // List a single part of a mount name, queue it to be visited if needed
	                        var mountPart = new UPath(searchPath.GetFirstDirectory(out _)).ToRelative();
	                        var mountPath = location.Prefix / mountPart;
	                        var item = new FileSystemItem(this, mountPath, true);
	                        if (searchPredicate == null || searchPredicate(ref item))
	                        {
	                            if (entries.Add(item.Path))
	                            {
	                                yield return item;
	                            }
	                        }
	                        if (searchOption == SearchOption.AllDirectories)
	                        {
	                            sortedDirectories.Add(mountPath);
	                        }
	                    }
	                    else
	                    {
	                        // List files in the mounted filesystems, merged and sorted into one list
	                        foreach (var item in fileSystem.EnumerateItems(searchPath, SearchOption.TopDirectoryOnly, searchPredicate))
	                        {
	                            var publicName = CombinePrefix(location.Prefix, item.Path);
	                            if (entries.Add(publicName))
	                            {
	                                var localItem = item;
	                                localItem.Path = publicName;
	                                yield return localItem;
	                                if (searchOption == SearchOption.AllDirectories && item.IsDirectory)
	                                {
	                                    sortedDirectories.Add(publicName);
	                                }
	                            }
	                        }
	                    }
	                }
	            }
	            if (first)
	            {
	                if (locations.Count == 0 && path != UPath.Root)
	                    throw NewDirectoryNotFoundException(path);
	                first = false;
	            }
	            // Enqueue directories and respect order
	            foreach (var nextDir in sortedDirectories)
	            {
	                directoryToVisit.Insert(dirIndex++, nextDir);
	            }
	        }
	    }
	    protected override bool CanWatchImpl(UPath path)
	    {
	        // Always allow watching because a future filesystem can be added that matches this path.
	        return true;
	    }
	    protected override IFileSystemWatcher WatchImpl(UPath path)
	    {
	        // TODO: create/delete events when mounts are added/removed
	        var watcher = new AggregateWatcher(this, path);
	        foreach (var kvp in _mounts)
	        {
	            if (!IsMountIncludedInWatch(kvp.Key, path, out var remainingPath))
	            {
	                continue;
	            }
	            if (kvp.Value.CanWatch(remainingPath))
	            {
	                var internalWatcher = kvp.Value.Watch(remainingPath);
	                watcher.Add(new WrapWatcher(kvp.Value, kvp.Key, remainingPath, internalWatcher));
	            }
	        }
	        if (Fallback != null && Fallback.CanWatch(path))
	        {
	            var internalWatcher = Fallback.Watch(path);
	            watcher.Add(new WrapWatcher(Fallback, UPath.Null, path, internalWatcher));
	        }
	        _watchers.Add(watcher);
	        return watcher;
	    }
	    private class AggregateWatcher : AggregateFileSystemWatcher
	    {
	        private readonly MountFileSystem _fileSystem;
	        public AggregateWatcher(MountFileSystem fileSystem, UPath path)
	            : base(fileSystem, path)
	        {
	            _fileSystem = fileSystem;
	        }
	        protected override void Dispose(bool disposing)
	        {
	            if (disposing && !_fileSystem.IsDisposing)
	            {
	                _fileSystem._watchers.Remove(this);
	            }
	        }
	    }
	    private class WrapWatcher : WrapFileSystemWatcher
	    {
	        private readonly UPath _mountPath;
	        public WrapWatcher(IFileSystem fileSystem, UPath mountPath, UPath path, IFileSystemWatcher watcher)
	            : base(fileSystem, path, watcher)
	        {
	            _mountPath = mountPath;
	        }
	        protected override UPath? TryConvertPath(UPath pathFromEvent)
	        {
	            if (!_mountPath.IsNull)
	            {
	                return _mountPath / pathFromEvent.ToRelative();
	            }
	            else
	            {
	                return pathFromEvent;
	            }
	        }
	    }
	    protected override UPath ConvertPathToDelegate(UPath path)
	    {
	        return path;
	    }
	    protected override UPath ConvertPathFromDelegate(UPath path)
	    {
	        return path;
	    }
	    private IFileSystem? TryGetMountOrNext(ref UPath path)
	    {
	        return TryGetMountOrNext(ref path, out var _);
	    }
	    private IFileSystem? TryGetMountOrNext(ref UPath path, out UPath mountPath)
	    {
	        mountPath = UPath.Null;
	        if (path.IsNull)
	        {
	            return null;
	        }
	        IFileSystem? mountfs = null;
	        foreach (var kvp in _mounts)
	        {
	            var remainingPath = GetRemaining(kvp.Key, path);
	            if (remainingPath.IsNull)
	            {
	                continue;
	            }
	            mountPath = kvp.Key;
	            mountfs = kvp.Value;
	            path = remainingPath;
	            break;
	        }
	        if (mountfs != null)
	        {
	            return mountfs;
	        }
	        mountPath = UPath.Null;
	        return Fallback;
	    }
	    private static bool IsMountIncludedInWatch(UPath mountPrefix, UPath watchPath, out UPath remainingPath)
	    {
	        if (watchPath == UPath.Root)
	        {
	            remainingPath = UPath.Root;
	            return true;
	        }
	        remainingPath = GetRemaining(mountPrefix, watchPath);
	        return !remainingPath.IsNull;
	    }
	    private static UPath GetRemaining(UPath prefix, UPath path)
	    {
	        if (!path.IsInDirectory(prefix, true))
	        {
	            return null!;
	        }
	        var remaining = path.FullName.Substring(prefix.FullName.Length);
	        var remainingPath = new UPath(remaining).ToAbsolute();
	        return remainingPath;
	    }
	    private static UPath CombinePrefix(UPath prefix, UPath remaining)
	    {
	        return prefix.IsNull ? remaining
	            : prefix / remaining.ToRelative();
	    }
	    private static void ValidateMountName(UPath name)
	    {
	        name.AssertAbsolute(nameof(name));
	        if (name == UPath.Root)
	        {
	            throw new ArgumentException("The mount name cannot be a `/` root filesystem", nameof(name));
	        }
	    }
	    private class UPathLengthComparer : IComparer<UPath>
	    {
	        public int Compare(UPath x, UPath y)
	        {
	            // longest UPath first
	            var lengthCompare = y.FullName.Length.CompareTo(x.FullName.Length);
	            if (lengthCompare != 0)
	            {
	                return lengthCompare;
	            }
	            // then compare name if equal length (otherwise we get exceptions about duplicates)
	            return string.CompareOrdinal(x.FullName, y.FullName);
	        }
	    }
	    private readonly struct SearchLocation
	    {
	        public IFileSystem FileSystem { get; }
	        public UPath Prefix { get; }
	        public UPath Path { get; }
	        public SearchLocation(IFileSystem fileSystem, UPath prefix, UPath path)
	        {
	            FileSystem = fileSystem;
	            Prefix = prefix;
	            Path = path;
	        }
	    }
	    private sealed class DebuggerProxy
	    {
	        private readonly MountFileSystem _fs;
	        public DebuggerProxy(MountFileSystem fs)
	        {
	            _fs = fs;
	        }
	        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
	        public KeyValuePair<string, IFileSystem>[] Mounts => _fs._mounts.Select(x => new KeyValuePair<string, IFileSystem>(x.Key.ToString(), x.Value)).ToArray();
	        public IFileSystem? Fallback => _fs.Fallback;
	    }
	}
	[DebuggerDisplay("{" + nameof(DebuggerDisplay) + "(),nq}")]
	public class PhysicalFileSystem : FileSystem
	{
	    private const string DrivePrefixOnWindows = "/mnt/";
	    private static readonly UPath PathDrivePrefixOnWindows = new UPath(DrivePrefixOnWindows);
	#if NETSTANDARD
	    internal static readonly bool IsOnWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
	#else
	    internal static readonly bool IsOnWindows = CheckIsOnWindows();
	    private static bool CheckIsOnWindows()
	    {
	        switch (Environment.OSVersion.Platform)
	        {
	            case PlatformID.Xbox:
	            case PlatformID.Win32NT:
	            case PlatformID.Win32S:
	            case PlatformID.Win32Windows:
	            case PlatformID.WinCE:
	                return true;
	        }
	        return false;
	    }
	#endif
	    // ----------------------------------------------
	    // Directory API
	    // ----------------------------------------------
	    protected override void CreateDirectoryImpl(UPath path)
	    {
	        if (IsWithinSpecialDirectory(path))
	        {
	            throw new UnauthorizedAccessException($"Cannot create a directory in the path `{path}`");
	        }
	        Directory.CreateDirectory(ConvertPathToInternal(path));
	    }
	    protected override bool DirectoryExistsImpl(UPath path)
	    {
	        return IsWithinSpecialDirectory(path) ? SpecialDirectoryExists(path) : Directory.Exists(ConvertPathToInternal(path));
	    }
	    protected override void MoveDirectoryImpl(UPath srcPath, UPath destPath)
	    {
	        if (IsOnWindows)
	        {
	            if (IsWithinSpecialDirectory(srcPath))
	            {
	                if (!SpecialDirectoryExists(srcPath))
	                {
	                    throw NewDirectoryNotFoundException(srcPath);
	                }
	                throw new UnauthorizedAccessException($"Cannot move the special directory `{srcPath}`");
	            }
	            if (IsWithinSpecialDirectory(destPath))
	            {
	                if (!SpecialDirectoryExists(destPath))
	                {
	                    throw NewDirectoryNotFoundException(destPath);
	                }
	                throw new UnauthorizedAccessException($"Cannot move to the special directory `{destPath}`");
	            }
	        }
	        var systemSrcPath = ConvertPathToInternal(srcPath);
	        var systemDestPath = ConvertPathToInternal(destPath);
	        // If the souce path is a file
	        var fileInfo = new FileInfo(systemSrcPath);
	        if (fileInfo.Exists)
	        {
	            throw new IOException($"The source `{srcPath}` is not a directory");
	        }
	        Directory.Move(systemSrcPath, systemDestPath);
	    }
	    protected override void DeleteDirectoryImpl(UPath path, bool isRecursive)
	    {
	        if (IsWithinSpecialDirectory(path))
	        {
	            if (!SpecialDirectoryExists(path))
	            {
	                throw NewDirectoryNotFoundException(path);
	            }
	            throw new UnauthorizedAccessException($"Cannot delete directory `{path}`");
	        }
	        Directory.Delete(ConvertPathToInternal(path), isRecursive);
	    }
	    // ----------------------------------------------
	    // File API
	    // ----------------------------------------------
	    protected override void CopyFileImpl(UPath srcPath, UPath destPath, bool overwrite)
	    {
	        if (IsWithinSpecialDirectory(srcPath))
	        {
	            throw new UnauthorizedAccessException($"The access to `{srcPath}` is denied");
	        }
	        if (IsWithinSpecialDirectory(destPath))
	        {
	            throw new UnauthorizedAccessException($"The access to `{destPath}` is denied");
	        }
	        File.Copy(ConvertPathToInternal(srcPath), ConvertPathToInternal(destPath), overwrite);
	    }
	    protected override void ReplaceFileImpl(UPath srcPath, UPath destPath, UPath destBackupPath, bool ignoreMetadataErrors)
	    {
	        if (IsWithinSpecialDirectory(srcPath))
	        {
	            throw new UnauthorizedAccessException($"The access to `{srcPath}` is denied");
	        }
	        if (IsWithinSpecialDirectory(destPath))
	        {
	            throw new UnauthorizedAccessException($"The access to `{destPath}` is denied");
	        }
	        if (!destBackupPath.IsNull && IsWithinSpecialDirectory(destBackupPath))
	        {
	            throw new UnauthorizedAccessException($"The access to `{destBackupPath}` is denied");
	        }
	        if (!destBackupPath.IsNull)
	        {
	            CopyFileImpl(destPath, destBackupPath, true);
	        }
	        CopyFileImpl(srcPath, destPath, true);
	        DeleteFileImpl(srcPath);
	        // TODO: Add atomic version using File.Replace coming with .NET Standard 2.0
	    }
	    protected override long GetFileLengthImpl(UPath path)
	    {
	        if (IsWithinSpecialDirectory(path))
	        {
	            throw new UnauthorizedAccessException($"The access to `{path}` is denied");
	        }
	        return new FileInfo(ConvertPathToInternal(path)).Length;
	    }
	    protected override bool FileExistsImpl(UPath path)
	    {
	        return !IsWithinSpecialDirectory(path) && File.Exists(ConvertPathToInternal(path));
	    }
	    protected override void MoveFileImpl(UPath srcPath, UPath destPath)
	    {
	        if (IsWithinSpecialDirectory(srcPath))
	        {
	            throw new UnauthorizedAccessException($"The access to `{srcPath}` is denied");
	        }
	        if (IsWithinSpecialDirectory(destPath))
	        {
	            throw new UnauthorizedAccessException($"The access to `{destPath}` is denied");
	        }
	        File.Move(ConvertPathToInternal(srcPath), ConvertPathToInternal(destPath));
	    }
	    protected override void DeleteFileImpl(UPath path)
	    {
	        if (IsWithinSpecialDirectory(path))
	        {
	            throw new UnauthorizedAccessException($"The access to `{path}` is denied");
	        }
	        File.Delete(ConvertPathToInternal(path));
	    }
	    protected override Stream OpenFileImpl(UPath path, FileMode mode, FileAccess access,
	        FileShare share = FileShare.None)
	    {
	        if (IsWithinSpecialDirectory(path))
	        {
	            throw new UnauthorizedAccessException($"The access to `{path}` is denied");
	        }
	        return File.Open(ConvertPathToInternal(path), mode, access, share);
	    }
	    protected override FileAttributes GetAttributesImpl(UPath path)
	    {
	        // Handle special folders to return valid FileAttributes
	        if (IsWithinSpecialDirectory(path))
	        {
	            if (!SpecialDirectoryExists(path))
	            {
	                throw NewDirectoryNotFoundException(path);
	            }
	            // The path / and /drive are readonly
	            if (path == PathDrivePrefixOnWindows || path == UPath.Root)
	            {
	                return FileAttributes.Directory | FileAttributes.System | FileAttributes.ReadOnly;
	            }
	            // Otherwise let the File.GetAttributes returns the proper attributes for root drive (e.g /drive/c)
	        }
	        return File.GetAttributes(ConvertPathToInternal(path));
	    }
	    // ----------------------------------------------
	    // Metadata API
	    // ----------------------------------------------
	    protected override void SetAttributesImpl(UPath path, FileAttributes attributes)
	    {
	        // Handle special folders
	        if (IsWithinSpecialDirectory(path))
	        {
	            if (!SpecialDirectoryExists(path))
	            {
	                throw NewDirectoryNotFoundException(path);
	            }
	            throw new UnauthorizedAccessException($"Cannot set attributes on system directory `{path}`");
	        }
	        File.SetAttributes(ConvertPathToInternal(path), attributes);
	    }
	    protected override DateTime GetCreationTimeImpl(UPath path)
	    {
	        // Handle special folders
	        if (IsWithinSpecialDirectory(path))
	        {
	            if (!SpecialDirectoryExists(path))
	            {
	                throw NewDirectoryNotFoundException(path);
	            }
	            // For /drive and /, get the oldest CreationTime of all folders (approx)
	            if (path == PathDrivePrefixOnWindows || path == UPath.Root)
	            {
	                var creationTime = DateTime.MaxValue;
	                foreach (var drive in DriveInfo.GetDrives())
	                {
	                    if (!drive.IsReady)
	                        continue;
	                    var newCreationTime = drive.RootDirectory.CreationTime;
	                    if (newCreationTime < creationTime)
	                    {
	                        creationTime = newCreationTime;
	                    }
	                }
	                return creationTime;
	            }
	        }
	        return File.GetCreationTime(ConvertPathToInternal(path));
	    }
	    protected override void SetCreationTimeImpl(UPath path, DateTime time)
	    {
	        // Handle special folders
	        if (IsWithinSpecialDirectory(path))
	        {
	            if (!SpecialDirectoryExists(path))
	            {
	                throw NewDirectoryNotFoundException(path);
	            }
	            throw new UnauthorizedAccessException($"Cannot set creation time on system directory `{path}`");
	        }
	        var internalPath = ConvertPathToInternal(path);
	        var attributes = File.GetAttributes(internalPath);
	        if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
	        {
	            Directory.SetCreationTime(internalPath, time);
	        }
	        else
	        {
	            File.SetCreationTime(internalPath, time);
	        }
	    }
	    protected override DateTime GetLastAccessTimeImpl(UPath path)
	    {
	        // Handle special folders to return valid LastAccessTime
	        if (IsWithinSpecialDirectory(path))
	        {
	            if (!SpecialDirectoryExists(path))
	            {
	                throw NewDirectoryNotFoundException(path);
	            }
	            // For /drive and /, get the oldest CreationTime of all folders (approx)
	            if (path == PathDrivePrefixOnWindows || path == UPath.Root)
	            {
	                var lastAccessTime = DateTime.MaxValue;
	                foreach (var drive in DriveInfo.GetDrives())
	                {
	                    if (!drive.IsReady)
	                        continue;
	                    var time = drive.RootDirectory.LastAccessTime;
	                    if (time < lastAccessTime)
	                    {
	                        lastAccessTime = time;
	                    }
	                }
	                return lastAccessTime;
	            }
	            // otherwise let the regular function running
	        }
	        return File.GetLastAccessTime(ConvertPathToInternal(path));
	    }
	    protected override void SetLastAccessTimeImpl(UPath path, DateTime time)
	    {
	        // Handle special folders
	        if (IsWithinSpecialDirectory(path))
	        {
	            if (!SpecialDirectoryExists(path))
	            {
	                throw NewDirectoryNotFoundException(path);
	            }
	            throw new UnauthorizedAccessException($"Cannot set last access time on system directory `{path}`");
	        }
	        var internalPath = ConvertPathToInternal(path);
	        var attributes = File.GetAttributes(internalPath);
	        if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
	        {
	            Directory.SetLastAccessTime(internalPath, time);
	        }
	        else
	        {
	            File.SetLastAccessTime(internalPath, time);
	        }
	    }
	    protected override DateTime GetLastWriteTimeImpl(UPath path)
	    {
	        // Handle special folders to return valid LastAccessTime
	        if (IsWithinSpecialDirectory(path))
	        {
	            if (!SpecialDirectoryExists(path))
	            {
	                throw NewDirectoryNotFoundException(path);
	            }
	            // For /drive and /, get the oldest CreationTime of all folders (approx)
	            if (path == PathDrivePrefixOnWindows || path == UPath.Root)
	            {
	                var lastWriteTime = DateTime.MaxValue;
	                foreach (var drive in DriveInfo.GetDrives())
	                {
	                    if (!drive.IsReady)
	                        continue;
	                    var time = drive.RootDirectory.LastWriteTime;
	                    if (time < lastWriteTime)
	                    {
	                        lastWriteTime = time;
	                    }
	                }
	                return lastWriteTime;
	            }
	            // otherwise let the regular function running
	        }
	        return File.GetLastWriteTime(ConvertPathToInternal(path));
	    }
	    protected override void SetLastWriteTimeImpl(UPath path, DateTime time)
	    {
	        // Handle special folders
	        if (IsWithinSpecialDirectory(path))
	        {
	            if (!SpecialDirectoryExists(path))
	            {
	                throw NewDirectoryNotFoundException(path);
	            }
	            throw new UnauthorizedAccessException($"Cannot set last write time on system directory `{path}`");
	        }
	        var internalPath = ConvertPathToInternal(path);
	        var attributes = File.GetAttributes(internalPath);
	        if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
	        {
	            Directory.SetLastWriteTime(internalPath, time);
	        }
	        else
	        {
	            File.SetLastWriteTime(internalPath, time);
	        }
	    }
	    protected override void CreateSymbolicLinkImpl(UPath path, UPath pathToTarget)
	    {
	        if (IsWithinSpecialDirectory(path))
	        {
	            throw new UnauthorizedAccessException($"The access to `{path}` is denied");
	        }
	        if (IsWithinSpecialDirectory(pathToTarget))
	        {
	            throw new UnauthorizedAccessException($"The access to `{pathToTarget}` is denied");
	        }
	        var systemPath = ConvertPathToInternal(path);
	        if (File.Exists(systemPath))
	        {
	            throw NewDestinationFileExistException(path);
	        }
	        if (Directory.Exists(systemPath))
	        {
	            throw NewDestinationDirectoryExistException(path);
	        }
	        var systemPathToTarget = ConvertPathToInternal(pathToTarget);
	        bool isDirectory;
	        if (File.Exists(systemPathToTarget))
	        {
	            isDirectory = false;
	        }
	        else if (Directory.Exists(systemPathToTarget))
	        {
	            isDirectory = true;
	        }
	        else
	        {
	            throw NewDirectoryNotFoundException(path);
	        }
	#if NET7_0_OR_GREATER
	        if (isDirectory)
	        {
	            Directory.CreateSymbolicLink(systemPath, systemPathToTarget);
	        }
	        else
	        {
	            File.CreateSymbolicLink(systemPath, systemPathToTarget);
	        }
	#else
	        bool success;
	        if (IsOnWindows)
	        {
	            var type = isDirectory ? Interop.Windows.SymbolicLink.Directory : Interop.Windows.SymbolicLink.File;
	            success = Interop.Windows.CreateSymbolicLink(systemPath, systemPathToTarget, type);
	            if (!success && Marshal.GetLastWin32Error() == 1314)
	            {
	                throw new UnauthorizedAccessException($"Could not create symbolic link `{path}` to `{pathToTarget}` due to insufficient privileges");
	            }
	        }
	        else
	        {
	            success = Interop.Unix.symlink(systemPathToTarget, systemPath) == 0;
	        }
	        if (!success)
	        {
	            throw new IOException($"Could not create symbolic link `{path}` to `{pathToTarget}`");
	        }
	#endif
	    }
	    protected override bool TryResolveLinkTargetImpl(UPath linkPath, out UPath resolvedPath)
	    {
	        if (IsWithinSpecialDirectory(linkPath))
	        {
	            throw new UnauthorizedAccessException($"The access to `{linkPath}` is denied");
	        }
	        var systemPath = ConvertPathToInternal(linkPath);
	        bool isDirectory;
	        if (File.Exists(systemPath))
	        {
	            isDirectory = false;
	        }
	        else if (Directory.Exists(systemPath))
	        {
	            isDirectory = true;
	        }
	        else
	        {
	            resolvedPath = default;
	            return false;
	        }
	#if NET7_0_OR_GREATER
	        var systemResult = isDirectory ? Directory.ResolveLinkTarget(systemPath, true)?.FullName : File.ResolveLinkTarget(systemPath, true)?.FullName;
	#else
	        var systemResult = IsOnWindows ? Interop.Windows.GetFinalPathName(systemPath) : Interop.Unix.readlink(systemPath);
	#endif
	        if (systemResult == null)
	        {
	            resolvedPath = default;
	            return false;
	        }
	        resolvedPath = ConvertPathFromInternal(systemResult);
	        return true;
	    }
	    // ----------------------------------------------
	    // Search API
	    // ----------------------------------------------
	    protected override IEnumerable<UPath> EnumeratePathsImpl(UPath path, string searchPattern, SearchOption searchOption, SearchTarget searchTarget)
	    {
	        // Special case for Windows as we need to provide list for:
	        // - the root folder / (which should just return the /drive folder)
	        // - the drive folders /drive/c, drive/e...etc.
	        var search = SearchPattern.Parse(ref path, ref searchPattern);
	        if (IsOnWindows)
	        {
	            if (IsWithinSpecialDirectory(path))
	            {
	                if (!SpecialDirectoryExists(path))
	                {
	                    throw NewDirectoryNotFoundException(path);
	                }
	                var searchForDirectory = searchTarget == SearchTarget.Both || searchTarget == SearchTarget.Directory;
	                // Only sub folder "/drive/" on root folder /
	                if (path == UPath.Root)
	                {
	                    if (searchForDirectory)
	                    {
	                        yield return PathDrivePrefixOnWindows;
	                        if (searchOption == SearchOption.AllDirectories)
	                        {
	                            foreach (var subPath in EnumeratePathsImpl(PathDrivePrefixOnWindows, searchPattern, searchOption, searchTarget))
	                            {
	                                yield return subPath;
	                            }
	                        }
	                    }
	                    yield break;
	                }
	                // When listing for /drive, return the list of drives available
	                if (path == PathDrivePrefixOnWindows)
	                {
	                    var pathDrives = new List<UPath>();
	                    foreach (var drive in DriveInfo.GetDrives())
	                    {
	                        if (drive.Name.Length < 2 || drive.Name[1] != ':')
	                        {
	                            continue;
	                        }
	                        var pathDrive = PathDrivePrefixOnWindows / char.ToLowerInvariant(drive.Name[0]).ToString();
	                        if (search.Match(pathDrive))
	                        {
	                            pathDrives.Add(pathDrive);
	                            if (searchForDirectory)
	                            {
	                                yield return pathDrive;
	                            }
	                        }
	                    }
	                    if (searchOption == SearchOption.AllDirectories)
	                    {
	                        foreach (var pathDrive in pathDrives)
	                        {
	                            foreach (var subPath in EnumeratePathsImpl(pathDrive, searchPattern, searchOption, searchTarget))
	                            {
	                                yield return subPath;
	                            }
	                        }
	                    }
	                    yield break;
	                }
	            }
	        }
	        IEnumerable<string> results;
	        switch (searchTarget)
	        {
	            case SearchTarget.File:
	                results = Directory.EnumerateFiles(ConvertPathToInternal(path), searchPattern, searchOption);
	                break;
	            case SearchTarget.Directory:
	                results = Directory.EnumerateDirectories(ConvertPathToInternal(path), searchPattern, searchOption);
	                break;
	            case SearchTarget.Both:
	                results = Directory.EnumerateFileSystemEntries(ConvertPathToInternal(path), searchPattern, searchOption);
	                break;
	            default:
	                yield break;
	        }
	        foreach (var subPath in results)
	        {
	            // Windows will truncate the search pattern's extension to three characters if the filesystem
	            // has 8.3 paths enabled. This means searching for *.docx will list *.doc as well which is
	            // not what we want. Check against the search pattern again to filter out those false results.
	            if (!IsOnWindows || search.Match(Path.GetFileName(subPath)))
	            {
	                yield return ConvertPathFromInternal(subPath);
	            }
	        }
	    }
	    protected override IEnumerable<FileSystemItem> EnumerateItemsImpl(UPath path, SearchOption searchOption, SearchPredicate? searchPredicate)
	    {
	        if (IsOnWindows)
	        {
	            if (IsWithinSpecialDirectory(path))
	            {
	                if (!SpecialDirectoryExists(path))
	                {
	                    throw NewDirectoryNotFoundException(path);
	                }
	                // Only sub folder "/drive/" on root folder /
	                if (path == UPath.Root)
	                {
	                    var item = new FileSystemItem(this, PathDrivePrefixOnWindows, true);
	                    if (searchPredicate == null || searchPredicate(ref item))
	                    {
	                        yield return item;
	                    }
	                    if (searchOption == SearchOption.AllDirectories)
	                    {
	                        foreach (var subItem in EnumerateItemsImpl(PathDrivePrefixOnWindows, searchOption, searchPredicate))
	                        {
	                            yield return subItem;
	                        }
	                    }
	                    yield break;
	                }
	                // When listing for /drive, return the list of drives available
	                if (path == PathDrivePrefixOnWindows)
	                {
	                    var pathDrives = new List<UPath>();
	                    foreach (var drive in DriveInfo.GetDrives())
	                    {
	                        if (drive.Name.Length < 2 || drive.Name[1] != ':')
	                        {
	                            continue;
	                        }
	                        var pathDrive = PathDrivePrefixOnWindows / char.ToLowerInvariant(drive.Name[0]).ToString();
	                        pathDrives.Add(pathDrive);
	                        var item = new FileSystemItem(this, pathDrive, true);
	                        if (searchPredicate == null || searchPredicate(ref item))
	                        {
	                            yield return item;
	                        }
	                    }
	                    if (searchOption == SearchOption.AllDirectories)
	                    {
	                        foreach (var pathDrive in pathDrives)
	                        {
	                            foreach (var subItem in EnumerateItemsImpl(pathDrive, searchOption, searchPredicate))
	                            {
	                                yield return subItem;
	                            }
	                        }
	                    }
	                    yield break;
	                }
	            }
	        }
	        var pathOnDisk = ConvertPathToInternal(path);
	        if (!Directory.Exists(pathOnDisk)) yield break;
	#if NETSTANDARD2_1
	        var enumerable = new FileSystemEnumerable<FileSystemItem>(pathOnDisk, TransformToFileSystemItem, searchOption == SearchOption.AllDirectories ? CompatibleRecursive : Compatible);
	        foreach (var item in enumerable)
	        {
	            var localItem = item;
	            if (searchPredicate == null || searchPredicate(ref localItem))
	            {
	                yield return localItem;
	            }
	        }
	#else
	        var results = Directory.EnumerateFileSystemEntries(pathOnDisk, "*", searchOption);
	        foreach (var subPath in results)
	        {
	            var fileInfo = new FileInfo(subPath);
	            var fullPath = ConvertPathFromInternal(subPath);
	            var item = new FileSystemItem
	            {
	                FileSystem = this,
	                AbsolutePath = fullPath,
	                Path = fullPath,
	                Attributes = fileInfo.Attributes,
	                CreationTime = fileInfo.CreationTimeUtc.ToLocalTime(),
	                LastAccessTime = fileInfo.LastAccessTimeUtc.ToLocalTime(),
	                LastWriteTime = fileInfo.LastWriteTimeUtc.ToLocalTime(),
	                Length = (fileInfo.Attributes & FileAttributes.Directory) > 0 ? 0 : fileInfo.Length
	            };
	            if (searchPredicate == null || searchPredicate(ref item))
	            {
	                yield return item;
	            }
	        }
	#endif
	    }
	#if NETSTANDARD2_1
	    internal static EnumerationOptions Compatible { get; } = new EnumerationOptions()
	    {
	        MatchType = MatchType.Win32,
	        AttributesToSkip = (FileAttributes)0,
	        IgnoreInaccessible = false
	    };
	    private static EnumerationOptions CompatibleRecursive { get; } = new EnumerationOptions()
	    {
	        RecurseSubdirectories = true,
	        MatchType = MatchType.Win32,
	        AttributesToSkip = (FileAttributes)0,
	        IgnoreInaccessible = false
	    };
	    private FileSystemItem TransformToFileSystemItem(ref System.IO.Enumeration.FileSystemEntry entry)
	    {
	        var fullPath = ConvertPathFromInternal(entry.ToFullPath());
	        return new FileSystemItem
	        {
	            FileSystem = this,
	            AbsolutePath = fullPath,
	            Path = fullPath,
	            Attributes = entry.Attributes,
	            CreationTime = entry.CreationTimeUtc.ToLocalTime(),
	            LastAccessTime = entry.LastAccessTimeUtc.ToLocalTime(),
	            LastWriteTime = entry.LastWriteTimeUtc.ToLocalTime(),
	            Length = entry.Length
	        };
	    }
	#endif
	    // ----------------------------------------------
	    // Watch API
	    // ----------------------------------------------
	    protected override bool CanWatchImpl(UPath path)
	    {
	        if (IsWithinSpecialDirectory(path))
	        {
	            return SpecialDirectoryExists(path);
	        }
	        return Directory.Exists(ConvertPathToInternal(path));
	    }
	    protected override IFileSystemWatcher WatchImpl(UPath path)
	    {
	        if (IsWithinSpecialDirectory(path))
	        {
	            throw new UnauthorizedAccessException($"The access to `{path}` is denied");
	        }
	        return new Watcher(this, path);
	    }
	    private sealed class Watcher : IFileSystemWatcher
	    {
	        private readonly PhysicalFileSystem _fileSystem;
	        private readonly System.IO.FileSystemWatcher _watcher;
	        public event EventHandler<FileChangedEventArgs>? Changed;
	        public event EventHandler<FileChangedEventArgs>? Created;
	        public event EventHandler<FileChangedEventArgs>? Deleted;
	        public event EventHandler<FileSystemErrorEventArgs>? Error;
	        public event EventHandler<FileRenamedEventArgs>? Renamed;
	        public IFileSystem FileSystem => _fileSystem;
	        public UPath Path { get; }
	        public int InternalBufferSize
	        {
	            get => _watcher.InternalBufferSize;
	            set => _watcher.InternalBufferSize = value;
	        }
	        public NotifyFilters NotifyFilter
	        {
	            get => (NotifyFilters)_watcher.NotifyFilter;
	            set => _watcher.NotifyFilter = (System.IO.NotifyFilters)value;
	        }
	        public bool EnableRaisingEvents
	        {
	            get => _watcher.EnableRaisingEvents;
	            set => _watcher.EnableRaisingEvents = value;
	        }
	        public string Filter
	        {
	            get => _watcher.Filter;
	            set => _watcher.Filter = value;
	        }
	        public bool IncludeSubdirectories
	        {
	            get => _watcher.IncludeSubdirectories;
	            set => _watcher.IncludeSubdirectories = value;
	        }
	        public Watcher(PhysicalFileSystem fileSystem, UPath path)
	        {
	            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
	            _watcher = new System.IO.FileSystemWatcher(_fileSystem.ConvertPathToInternal(path))
	            {
	                Filter = "*"
	            };
	            Path = path;
	            _watcher.Changed += (sender, args) => Changed?.Invoke(this, Remap(args));
	            _watcher.Created += (sender, args) => Created?.Invoke(this, Remap(args));
	            _watcher.Deleted += (sender, args) => Deleted?.Invoke(this, Remap(args));
	            _watcher.Error += (sender, args) => Error?.Invoke(this, Remap(args));
	            _watcher.Renamed += (sender, args) => Renamed?.Invoke(this, Remap(args));
	        }
	        ~Watcher()
	        {
	            Dispose(false);
	        }
	        public void Dispose()
	        {
	            Dispose(true);
	            GC.SuppressFinalize(this);
	        }
	        private void Dispose(bool disposing)
	        {
	            if (disposing)
	            {
	                _watcher.Dispose();
	            }
	        }
	        private FileChangedEventArgs Remap(FileSystemEventArgs args)
	        {
	            var newChangeType = (WatcherChangeTypes)args.ChangeType;
	            var newPath = _fileSystem.ConvertPathFromInternal(args.FullPath);
	            return new FileChangedEventArgs(FileSystem, newChangeType, newPath);
	        }
	        private FileSystemErrorEventArgs Remap(ErrorEventArgs args)
	        {
	            return new FileSystemErrorEventArgs(args.GetException());
	        }
	        private FileRenamedEventArgs Remap(RenamedEventArgs args)
	        {
	            var newChangeType = (WatcherChangeTypes)args.ChangeType;
	            var newPath = _fileSystem.ConvertPathFromInternal(args.FullPath);
	            var newOldPath = _fileSystem.ConvertPathFromInternal(args.OldFullPath);
	            return new FileRenamedEventArgs(FileSystem, newChangeType, newPath, newOldPath);
	        }
	    }
	    // ----------------------------------------------
	    // Path API
	    // ----------------------------------------------
	    protected override string ConvertPathToInternalImpl(UPath path)
	    {
	        var absolutePath = path.FullName;
	        if (IsOnWindows)
	        {
	            if (!absolutePath.StartsWith(DrivePrefixOnWindows, StringComparison.Ordinal) ||
	                absolutePath.Length == DrivePrefixOnWindows.Length ||
	                !IsDriveLetter(absolutePath[DrivePrefixOnWindows.Length]))
	                throw new ArgumentException($"A path on Windows must start by `{DrivePrefixOnWindows}` followed by the drive letter");
	            var driveLetter = char.ToUpper(absolutePath[DrivePrefixOnWindows.Length]);
	            if (absolutePath.Length != DrivePrefixOnWindows.Length + 1 &&
	                absolutePath[DrivePrefixOnWindows.Length + 1] !=
	                UPath.DirectorySeparator)
	                throw new ArgumentException($"The driver letter `/{DrivePrefixOnWindows}{absolutePath[DrivePrefixOnWindows.Length]}` must be followed by a `/` or nothing in the path -> `{absolutePath}`");
	            var builder = UPath.GetSharedStringBuilder();
	            builder.Append(driveLetter).Append(":\\");
	            if (absolutePath.Length > DrivePrefixOnWindows.Length + 1)
	                builder.Append(absolutePath.Replace(UPath.DirectorySeparator, '\\').Substring(DrivePrefixOnWindows.Length + 2));
	            return builder.ToString();
	        }
	        return absolutePath;
	    }
	    protected override UPath ConvertPathFromInternalImpl(string innerPath)
	    {
	        if (IsOnWindows)
	        {
	            // We currently don't support special Windows files (\\.\ \??\  DosDevices...etc.)
	            if (innerPath.StartsWith(@"\\", StringComparison.Ordinal) || innerPath.StartsWith(@"\?", StringComparison.Ordinal))
	                throw new NotSupportedException($"Path starting with `\\\\` or `\\?` are not supported -> `{innerPath}` ");
	            // We want to avoid using Path.GetFullPath unless absolutely necessary,
	            // because it can change the case of already rooted paths that contain a ~
	            var absolutePath = HasWindowsVolumeLabel(innerPath) ? innerPath : Path.GetFullPath(innerPath);
	            // Assert that Path.GetFullPath returned the format we expect
	            if (!HasWindowsVolumeLabel(absolutePath))
	                throw new ArgumentException($"Expecting a drive for the path `{absolutePath}`");
	            var builder = UPath.GetSharedStringBuilder();
	            builder.Append(DrivePrefixOnWindows).Append(char.ToLowerInvariant(absolutePath[0])).Append('/');
	            if (absolutePath.Length > 2)
	                builder.Append(absolutePath.Substring(2));
	            return new UPath(builder.ToString());
	        }
	        return innerPath;
	    }
	    private static bool IsWithinSpecialDirectory(UPath path)
	    {
	        if (!IsOnWindows)
	        {
	            return false;
	        }
	        var parentDirectory = path.GetDirectory();
	        return path == PathDrivePrefixOnWindows ||
	               path == UPath.Root ||
	               parentDirectory == PathDrivePrefixOnWindows ||
	               parentDirectory == UPath.Root;
	    }
	    private static bool SpecialDirectoryExists(UPath path)
	    {
	        // /drive or / can be read
	        if (path == PathDrivePrefixOnWindows || path == UPath.Root)
	        {
	            return true;
	        }
	        // If /xxx, invalid (parent folder is /)
	        var parentDirectory = path.GetDirectory();
	        if (parentDirectory == UPath.Root)
	        {
	            return false;
	        }
	        var dirName = path.GetName();
	        // Else check that we have a valid drive path (e.g /drive/c)
	        return parentDirectory == PathDrivePrefixOnWindows && 
	               dirName.Length == 1 && 
	               DriveInfo.GetDrives().Any(p => char.ToLowerInvariant(p.Name[0]) == dirName[0]);
	    }
	    private static bool IsDriveLetter(char c)
	    {
	        return c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z';
	    }
	    private static bool HasWindowsVolumeLabel( string path )
	    {
	        if ( !IsOnWindows )
	            throw new NotSupportedException( $"{nameof( HasWindowsVolumeLabel )} is only supported on Windows platforms." );
	        return path.Length >= 3 && path[1] == ':' && path[2] is '\\' or '/';
	    }
	}
	[DebuggerDisplay("{" + nameof(DebuggerDisplay) + "(),nq}")]
	public class ReadOnlyFileSystem : ComposeFileSystem
	{
	    protected const string FileSystemIsReadOnly = "This filesystem is read-only";
	    public ReadOnlyFileSystem(IFileSystem? fileSystem, bool owned = true) : base(fileSystem, owned)
	    {
	    }
	    // ----------------------------------------------
	    // Directory API
	    // ----------------------------------------------
	    protected override void CreateDirectoryImpl(UPath path)
	    {
	        throw new IOException(FileSystemIsReadOnly);
	    }
	    protected override void MoveDirectoryImpl(UPath srcPath, UPath destPath)
	    {
	        throw new IOException(FileSystemIsReadOnly);
	    }
	    protected override void DeleteDirectoryImpl(UPath path, bool isRecursive)
	    {
	        throw new IOException(FileSystemIsReadOnly);
	    }
	    // ----------------------------------------------
	    // File API
	    // ----------------------------------------------
	    protected override void CopyFileImpl(UPath srcPath, UPath destPath, bool overwrite)
	    {
	        throw new IOException(FileSystemIsReadOnly);
	    }
	    protected override void ReplaceFileImpl(UPath srcPath, UPath destPath, UPath destBackupPath, bool ignoreMetadataErrors)
	    {
	        throw new IOException(FileSystemIsReadOnly);
	    }
	    protected override void MoveFileImpl(UPath srcPath, UPath destPath)
	    {
	        throw new IOException(FileSystemIsReadOnly);
	    }
	    protected override void DeleteFileImpl(UPath path)
	    {
	        throw new IOException(FileSystemIsReadOnly);
	    }
	    protected override Stream OpenFileImpl(UPath path, FileMode mode, FileAccess access, FileShare share = FileShare.None)
	    {
	        if (mode != FileMode.Open)
	        {
	            throw new IOException(FileSystemIsReadOnly);
	        }
	        if ((access & FileAccess.Write) != 0)
	        {
	            throw new IOException(FileSystemIsReadOnly);
	        }
	        return base.OpenFileImpl(path, mode, access, share);
	    }
	    // ----------------------------------------------
	    // Metadata API
	    // ----------------------------------------------
	    protected override FileAttributes GetAttributesImpl(UPath path)
	    {
	        // All paths are readonly
	        var attributes = base.GetAttributesImpl(path);
	        return attributes == FileAttributes.Normal
	            ? FileAttributes.ReadOnly
	            : attributes | FileAttributes.ReadOnly;
	    }
	    protected override void SetAttributesImpl(UPath path, FileAttributes attributes)
	    {
	        throw new IOException(FileSystemIsReadOnly);
	    }
	    protected override void SetCreationTimeImpl(UPath path, DateTime time)
	    {
	        throw new IOException(FileSystemIsReadOnly);
	    }
	    protected override void SetLastAccessTimeImpl(UPath path, DateTime time)
	    {
	        throw new IOException(FileSystemIsReadOnly);
	    }
	    protected override void SetLastWriteTimeImpl(UPath path, DateTime time)
	    {
	        throw new IOException(FileSystemIsReadOnly);
	    }
	    protected override void CreateSymbolicLinkImpl(UPath path, UPath pathToTarget)
	    {
	        throw new IOException(FileSystemIsReadOnly);
	    }
	    // ----------------------------------------------
	    // Path
	    // ----------------------------------------------
	    protected override UPath ConvertPathToDelegate(UPath path)
	    {
	        // A readonly filesystem doesn't change the path to the delegated filesystem
	        return path;
	    }
	    protected override UPath ConvertPathFromDelegate(UPath path)
	    {
	        // A readonly filesystem doesn't change the path from the delegated filesystem
	        return path;
	    }
	}
	[DebuggerDisplay("{" + nameof(DebuggerDisplay) + "(),nq}")]
	public class SubFileSystem : ComposeFileSystem
	{
	    public SubFileSystem(IFileSystem fileSystem, UPath subPath, bool owned = true) : base(fileSystem, owned)
	    {
	        SubPath = subPath.AssertAbsolute(nameof(subPath));
	        if (!fileSystem.DirectoryExists(SubPath))
	        {
	            throw NewDirectoryNotFoundException(SubPath);
	        }
	    }
	    public UPath SubPath { get; }
	    protected override string DebuggerDisplay()
	    {
	        return $"{base.DebuggerDisplay()} Path: {SubPath}";
	    }
	    protected override IFileSystemWatcher WatchImpl(UPath path)
	    {
	        var delegateWatcher = base.WatchImpl(path);
	        return new Watcher(this, path, delegateWatcher);
	    }
	    private class Watcher : WrapFileSystemWatcher
	    {
	        private readonly SubFileSystem _fileSystem;
	        public Watcher(SubFileSystem fileSystem, UPath path, IFileSystemWatcher watcher)
	            : base(fileSystem, path, watcher)
	        {
	            _fileSystem = fileSystem;
	        }
	        protected override UPath? TryConvertPath(UPath pathFromEvent)
	        {
	            if (!pathFromEvent.IsInDirectory(_fileSystem.SubPath, true))
	            {
	                return null;
	            }
	            return _fileSystem.ConvertPathFromDelegate(pathFromEvent);
	        }
	    }
	    protected override UPath ConvertPathToDelegate(UPath path)
	    {
	        var safePath = path.ToRelative();
	        return SubPath / safePath;
	    }
	    protected override UPath ConvertPathFromDelegate(UPath path)
	    {
	        var fullPath = path.FullName;
	        if (!fullPath.StartsWith(SubPath.FullName, StringComparison.Ordinal) || (fullPath.Length > SubPath.FullName.Length && fullPath[SubPath.FullName.Length] != UPath.DirectorySeparator))
	        {
	            // More a safe guard, as it should never happen, but if a delegate filesystem doesn't respect its root path
	            // we are throwing an exception here
	            throw new InvalidOperationException($"The path `{path}` returned by the delegate filesystem is not rooted to the subpath `{SubPath}`");
	        }
	        var subPath = fullPath.Substring(SubPath.FullName.Length);
	        return subPath == string.Empty ? UPath.Root : new UPath(subPath, true);
	    }
	}
	public class WrapFileSystemWatcher : FileSystemWatcher
	{
	    private readonly IFileSystemWatcher _watcher;
	    public WrapFileSystemWatcher(IFileSystem fileSystem, UPath path, IFileSystemWatcher watcher)
	        : base(fileSystem, path)
	    {
	        if (watcher is null)
	        {
	            throw new ArgumentNullException(nameof(watcher));
	        }
	        _watcher = watcher;
	        RegisterEvents(_watcher);
	    }
	    public override int InternalBufferSize
	    {
	        get => _watcher.InternalBufferSize;
	        set => _watcher.InternalBufferSize = value;
	    }
	    public override NotifyFilters NotifyFilter
	    {
	        get => _watcher.NotifyFilter;
	        set => _watcher.NotifyFilter = value;
	    }
	    public override bool EnableRaisingEvents
	    {
	        get => _watcher.EnableRaisingEvents;
	        set => _watcher.EnableRaisingEvents = value;
	    }
	    public override string Filter
	    {
	        get => _watcher.Filter;
	        set => _watcher.Filter = value;
	    }
	    public override bool IncludeSubdirectories
	    {
	        get => _watcher.IncludeSubdirectories;
	        set => _watcher.IncludeSubdirectories = value;
	    }
	    protected override void Dispose(bool disposing)
	    {
	        if (disposing)
	        {
	            UnregisterEvents(_watcher);
	            _watcher.Dispose();
	        }
	    }
	}
	public class ZipArchiveFileSystem : FileSystem
	{
	    private readonly bool _isCaseSensitive;
	    private ZipArchive _archive;
	    private Dictionary<UPath, InternalZipEntry> _entries;
	    private readonly string? _path;
	    private readonly Stream? _stream;
	    private readonly bool _disposeStream;
	    private readonly CompressionLevel _compressionLevel;
	    private readonly ReaderWriterLockSlim _entriesLock = new();
	    private FileSystemEventDispatcher<FileSystemWatcher>? _dispatcher;
	    private readonly object _dispatcherLock = new();
	    private readonly DateTime _creationTime;
	    private readonly Dictionary<ZipArchiveEntry, EntryState> _openStreams;
	    private readonly object _openStreamsLock = new();
	    private const char DirectorySeparator = '/';
	    public ZipArchiveFileSystem(ZipArchive archive, bool isCaseSensitive = false, CompressionLevel compressionLevel = CompressionLevel.NoCompression)
	    {
	        _archive = archive;
	        _isCaseSensitive = isCaseSensitive;
	        _creationTime = DateTime.Now;
	        _compressionLevel = compressionLevel;
	        if (archive == null)
	        {
	            throw new ArgumentNullException(nameof(archive));
	        }
	        _openStreams = new Dictionary<ZipArchiveEntry, EntryState>();
	        _entries = null!; // Loaded below
	        LoadEntries();
	    }
	    public ZipArchiveFileSystem(Stream stream, ZipArchiveMode mode = ZipArchiveMode.Update, bool leaveOpen = false, bool isCaseSensitive = false, CompressionLevel compressionLevel = CompressionLevel.NoCompression)
	        : this(new ZipArchive(stream, mode, leaveOpen: true), isCaseSensitive, compressionLevel)
	    {
	        _disposeStream = !leaveOpen;
	        _stream = stream;
	    }
	    public ZipArchiveFileSystem(string path, ZipArchiveMode mode = ZipArchiveMode.Update, bool leaveOpen = false, bool isCaseSensitive = false, CompressionLevel compressionLevel = CompressionLevel.NoCompression)
	        : this(new ZipArchive(File.Open(path, FileMode.OpenOrCreate), mode, leaveOpen), isCaseSensitive, compressionLevel)
	    {
	        _path = path;
	    }
	    public ZipArchiveFileSystem(ZipArchiveMode mode = ZipArchiveMode.Update, bool leaveOpen = false, bool isCaseSensitive = false, CompressionLevel compressionLevel = CompressionLevel.NoCompression)
	        : this(new MemoryStream(), mode, leaveOpen, isCaseSensitive, compressionLevel)
	    {
	    }
	    public void Save()
	    {
	        var mode = _archive.Mode;
	        if (_path != null)
	        {
	            _archive.Dispose();
	            _archive = new ZipArchive(File.Open(_path, FileMode.OpenOrCreate), mode);
	        }
	        else if (_stream != null)
	        {
	            if (!_stream.CanSeek)
	            {
	                throw new InvalidOperationException("Cannot save archive to a stream that doesn't support seeking");
	            }
	            _archive.Dispose();
	            _stream.Seek(0, SeekOrigin.Begin);
	            _archive = new ZipArchive(_stream, mode, leaveOpen: true);
	        }
	        else
	        {
	            throw new InvalidOperationException("Cannot save archive without a path or stream");
	        }
	        LoadEntries();
	    }
	    private void LoadEntries()
	    {
	        var comparer = _isCaseSensitive ? UPathComparer.Ordinal : UPathComparer.OrdinalIgnoreCase;
	        _entries = _archive.Entries.ToDictionary(
	            e => new UPath(e.FullName).ToAbsolute(),
	            static e =>
	            {
	                var lastChar = e.FullName[e.FullName.Length - 1];
	                return new InternalZipEntry(e, lastChar is '/' or '\\');
	            },
	            comparer);
	    }
	    private ZipArchiveEntry? GetEntry(UPath path, out bool isDirectory)
	    {
	        _entriesLock.EnterReadLock();
	        try
	        {
	            if (_entries.TryGetValue(path, out var foundEntry))
	            {
	                isDirectory = foundEntry.IsDirectory;
	                return foundEntry.Entry;
	            }
	        }
	        finally
	        {
	            _entriesLock.ExitReadLock();
	        }
	        isDirectory = false;
	        return null;
	    }
	    private ZipArchiveEntry? GetEntry(UPath path) => GetEntry(path, out _);
	    protected override UPath ConvertPathFromInternalImpl(string innerPath)
	    {
	        return new UPath(innerPath);
	    }
	    protected override string ConvertPathToInternalImpl(UPath path)
	    {
	        return path.FullName;
	    }
	    protected override void CopyFileImpl(UPath srcPath, UPath destPath, bool overwrite)
	    {
	        if (srcPath == destPath)
	        {
	            throw new IOException("Source and destination path must be different.");
	        }
	        var srcEntry = GetEntry(srcPath, out var isDirectory);
	        if (isDirectory)
	        {
	            throw new UnauthorizedAccessException(nameof(srcPath) + " is a directory.");
	        }
	        if (srcEntry == null)
	        {
	            if (!DirectoryExistsImpl(srcPath.GetDirectory()))
	            {
	                throw new DirectoryNotFoundException(srcPath.GetDirectory().FullName);
	            }
	            throw FileSystemExceptionHelper.NewFileNotFoundException(srcPath);
	        }
	        var parentDirectory = destPath.GetDirectory();
	        if (!DirectoryExistsImpl(parentDirectory))
	        {
	            throw FileSystemExceptionHelper.NewDirectoryNotFoundException(parentDirectory);
	        }
	        if (DirectoryExistsImpl(destPath))
	        {
	            if (!FileExistsImpl(destPath))
	            {
	                throw new IOException("Destination path is a directory");
	            }
	        }
	        var destEntry = GetEntry(destPath);
	        if (destEntry != null)
	        {
	#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
	            if ((destEntry.ExternalAttributes & (int)FileAttributes.ReadOnly) == (int)FileAttributes.ReadOnly)
	            {
	                throw new UnauthorizedAccessException("Destination file is read only");
	            }
	#endif
	            if (!overwrite)
	            {
	                throw FileSystemExceptionHelper.NewDestinationFileExistException(srcPath);
	            }
	            RemoveEntry(destEntry);
	            TryGetDispatcher()?.RaiseDeleted(destPath);
	        }
	        destEntry = CreateEntry(destPath.FullName);
	        using (var destStream = destEntry.Open())
	        {
	            using var srcStream = srcEntry.Open();
	            srcStream.CopyTo(destStream);
	        }
	#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
	        destEntry.ExternalAttributes = srcEntry.ExternalAttributes | (int)FileAttributes.Archive;
	#endif
	        TryGetDispatcher()?.RaiseCreated(destPath);
	    }
	    protected override void CreateDirectoryImpl(UPath path)
	    {
	        if (FileExistsImpl(path))
	        {
	            throw FileSystemExceptionHelper.NewDestinationFileExistException(path);
	        }
	        if (DirectoryExistsImpl(path))
	        {
	            throw FileSystemExceptionHelper.NewDestinationDirectoryExistException(path);
	        }
	        var parentPath = new UPath(GetParent(path.FullName));
	        if (parentPath != "")
	        {
	            if (!DirectoryExistsImpl(parentPath))
	            {
	                CreateDirectoryImpl(parentPath);
	            }
	        }
	        CreateEntry(path, isDirectory: true);
	        TryGetDispatcher()?.RaiseCreated(path);
	    }
	    protected override void DeleteDirectoryImpl(UPath path, bool isRecursive)
	    {
	        if (FileExistsImpl(path))
	        {
	            throw new IOException(nameof(path) + " is a file.");
	        }
	        var entries = new List<ZipArchiveEntry>();
	        if (!isRecursive)
	        {
	            // folder name ends with slash so StartWith check is enough
	            _entriesLock.EnterReadLock();
	            try
	            {
	                entries = _entries
	                    .Where(x => x.Key.FullName.StartsWith(path.FullName, _isCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
	                    .Take(2)
	                    .Select(x => x.Value.Entry)
	                    .ToList();
	            }
	            finally
	            {
	                _entriesLock.ExitReadLock();
	            }
	            if (entries.Count == 0)
	            {
	                throw FileSystemExceptionHelper.NewDirectoryNotFoundException(path);
	            }
	            if (entries.Count == 1)
	            {
	                RemoveEntry(entries[0]);
	            }
	            if (entries.Count == 2)
	            {
	                throw new IOException("Directory is not empty");
	            }
	            TryGetDispatcher()?.RaiseDeleted(path);
	            return;
	        }
	        _entriesLock.EnterReadLock();
	        try
	        {
	            entries = _entries
	                .Where(x => x.Key.FullName.StartsWith(path.FullName, _isCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
	                .Select(x => x.Value.Entry)
	                .ToList();
	            if (entries.Count == 0)
	            {
	                throw FileSystemExceptionHelper.NewDirectoryNotFoundException(path);
	            }
	            // check if there are no open file in directory
	            foreach (var entry in entries)
	            {
	                lock (_openStreamsLock)
	                {
	                    if (_openStreams.ContainsKey(entry))
	                    {
	                        throw new IOException($"There is an open file {entry.FullName} in directory");
	                    }
	                }
	            }
	#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
	            // check if there are none readonly entries
	            foreach (var entry in entries)
	            {
	                if ((entry.ExternalAttributes & (int)FileAttributes.ReadOnly) == (int)FileAttributes.ReadOnly)
	                {
	                    throw entry.FullName.Length == path.FullName.Length + 1
	                        ? new IOException("Directory is read only")
	                        : new UnauthorizedAccessException($"Cannot delete directory that contains readonly entry {entry.FullName}");
	                }
	            }
	#endif
	        }
	        finally
	        {
	            _entriesLock.ExitReadLock();
	        }
	        _entriesLock.EnterWriteLock();
	        try
	        {
	            foreach (var entry in entries)
	            {
	                _entries.Remove(new UPath(entry.FullName).ToAbsolute());
	                entry.Delete();
	            }
	        }
	        finally
	        {
	            _entriesLock.ExitWriteLock();
	        }
	        TryGetDispatcher()?.RaiseDeleted(path);
	    }
	    protected override void DeleteFileImpl(UPath path)
	    {
	        if (DirectoryExistsImpl(path))
	        {
	            throw new IOException("Cannot delete a directory");
	        }
	        var entry = GetEntry(path);
	        if (entry == null)
	        {
	            return;
	        }
	#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
	        if ((entry.ExternalAttributes & (int)FileAttributes.ReadOnly) == (int)FileAttributes.ReadOnly)
	        {
	            throw new UnauthorizedAccessException("Cannot delete file with readonly attribute");
	        }
	#endif
	        TryGetDispatcher()?.RaiseDeleted(path);
	        RemoveEntry(entry);
	    }
	    protected override bool DirectoryExistsImpl(UPath path)
	    {
	        if (path.FullName is "/" or "\\" or "")
	        {
	            return true;
	        }
	        _entriesLock.EnterReadLock();
	        try
	        {
	            return _entries.TryGetValue(path, out var entry) && entry.IsDirectory;
	        }
	        finally
	        {
	            _entriesLock.ExitReadLock();
	        }
	    }
	    protected override void Dispose(bool disposing)
	    {
	        _archive.Dispose();
	        if (_stream != null && _disposeStream)
	        {
	            _stream.Dispose();
	        }
	        if (disposing)
	        {
	            TryGetDispatcher()?.Dispose();
	        }
	    }
	    protected override IEnumerable<FileSystemItem> EnumerateItemsImpl(UPath path, SearchOption searchOption, SearchPredicate? searchPredicate)
	    {
	        return EnumeratePathsStr(path, "*", searchOption, SearchTarget.Both).Select(p => new FileSystemItem(this, p, p[p.Length - 1] == DirectorySeparator));
	    }
	    protected override IEnumerable<UPath> EnumeratePathsImpl(UPath path, string searchPattern, SearchOption searchOption, SearchTarget searchTarget)
	    {
	        return EnumeratePathsStr(path, searchPattern, searchOption, searchTarget).Select(x => new UPath(x));
	    }
	    private IEnumerable<string> EnumeratePathsStr(UPath path, string searchPattern, SearchOption searchOption, SearchTarget searchTarget)
	    {
	        var search = SearchPattern.Parse(ref path, ref searchPattern);
	        _entriesLock.EnterReadLock();
	        var entriesList = new List<ZipArchiveEntry>();
	        try
	        {
	            var internEntries = path == UPath.Root
	                ? _entries
	                : _entries.Where(kv => kv.Key.FullName.StartsWith(path.FullName, _isCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase) && kv.Key.FullName.Length > path.FullName.Length);
	            if (searchOption == SearchOption.TopDirectoryOnly)
	            {
	                internEntries = internEntries.Where(kv => kv.Key.IsInDirectory(path, false));
	            }
	            entriesList = internEntries.Select(kv => kv.Value.Entry).ToList();
	        }
	        finally
	        {
	            _entriesLock.ExitReadLock();
	        }
	        if (entriesList.Count == 0)
	        {
	            return Enumerable.Empty<string>();
	        }
	        var entries = (IEnumerable<ZipArchiveEntry>)entriesList;
	        if (searchTarget == SearchTarget.File)
	        {
	            entries = entries.Where(e => e.FullName[e.FullName.Length - 1] != DirectorySeparator);
	        }
	        else if (searchTarget == SearchTarget.Directory)
	        {
	            entries = entries.Where(e => e.FullName[e.FullName.Length - 1] == DirectorySeparator);
	        }
	        if (!string.IsNullOrEmpty(searchPattern))
	        {
	            entries = entries.Where(e => search.Match(GetName(e)));
	        }
	        return entries.Select(e => '/' + e.FullName);
	    }
	    protected override bool FileExistsImpl(UPath path)
	    {
	        _entriesLock.EnterReadLock();
	        try
	        {
	            return _entries.TryGetValue(path, out var entry) && !entry.IsDirectory;
	        }
	        finally
	        {
	            _entriesLock.ExitReadLock();
	        }
	    }
	    protected override FileAttributes GetAttributesImpl(UPath path)
	    {
	        var entry = GetEntry(path);
	        if (entry is null)
	        {
	            throw FileSystemExceptionHelper.NewFileNotFoundException(path);
	        }
	        var attributes = entry.FullName[entry.FullName.Length - 1] == DirectorySeparator ? FileAttributes.Directory : 0;
	#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
	        const FileAttributes validValues = (FileAttributes)0x7FFF /* Up to FileAttributes.Encrypted */ | FileAttributes.IntegrityStream | FileAttributes.NoScrubData;
	        var externalAttributes = (FileAttributes)entry.ExternalAttributes & validValues;
	        if (externalAttributes == 0 && attributes == 0)
	        {
	            attributes |= FileAttributes.Normal;
	        }
	        return externalAttributes | attributes;
	#else
	        // return standard attributes if it's not NetStandard2.1
	        return attributes == FileAttributes.Directory ? FileAttributes.Directory : entry.LastWriteTime >= _creationTime ? FileAttributes.Archive : FileAttributes.Normal;
	#endif
	    }
	    protected override long GetFileLengthImpl(UPath path)
	    {
	        var entry = GetEntry(path, out var isDirectory);
	        if (entry == null || isDirectory)
	        {
	            throw FileSystemExceptionHelper.NewFileNotFoundException(path);
	        }
	        try
	        {
	            return entry.Length;
	        }
	        catch (Exception ex) // for some reason entry.Length doesn't work with MemoryStream used in tests
	        {
	            Debug.WriteLine(ex.Message);
	            using var stream = OpenFileImpl(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
	            return stream.Length;
	        }
	    }
	    protected override DateTime GetCreationTimeImpl(UPath path)
	    {
	        return GetLastWriteTimeImpl(path);
	    }
	    protected override DateTime GetLastAccessTimeImpl(UPath path)
	    {
	        return GetLastWriteTimeImpl(path);
	    }
	    protected override DateTime GetLastWriteTimeImpl(UPath path)
	    {
	        var entry = GetEntry(path);
	        if (entry == null)
	        {
	            return DefaultFileTime;
	        }
	        return entry.LastWriteTime.DateTime;
	    }
	    protected override void MoveDirectoryImpl(UPath srcPath, UPath destPath)
	    {
	        if (destPath.IsInDirectory(srcPath, true))
	        {
	            throw new IOException("Cannot move directory to itself or a subdirectory.");
	        }
	        if (FileExistsImpl(srcPath))
	        {
	            throw new IOException(nameof(srcPath) + " is a file.");
	        }
	        var srcDir = srcPath.FullName;
	        _entriesLock.EnterReadLock();
	        var entries = Array.Empty<ZipArchiveEntry>();
	        try
	        {
	            entries = _archive.Entries.Where(e => e.FullName.StartsWith(srcDir, _isCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase)).ToArray();
	        }
	        finally
	        {
	            _entriesLock.ExitReadLock();
	        }
	        if (entries.Length == 0)
	        {
	            throw FileSystemExceptionHelper.NewDirectoryNotFoundException(srcPath);
	        }
	        CreateDirectoryImpl(destPath);
	        foreach (var entry in entries)
	        {
	            if (entry.FullName.Length == srcDir.Length)
	            {
	                RemoveEntry(entry);
	                continue;
	            }
	            using (var entryStream = entry.Open())
	            {
	                var entryName = entry.FullName.Substring(srcDir.Length);
	                var destEntry = CreateEntry(destPath + entryName, isDirectory: true);
	                using (var destEntryStream = destEntry.Open())
	                {
	                    entryStream.CopyTo(destEntryStream);
	                }
	            }
	            TryGetDispatcher()?.RaiseCreated(destPath);
	            RemoveEntry(entry);
	            TryGetDispatcher()?.RaiseDeleted(srcPath);
	        }
	    }
	    protected override void MoveFileImpl(UPath srcPath, UPath destPath)
	    {
	        var srcEntry = GetEntry(srcPath) ?? throw FileSystemExceptionHelper.NewFileNotFoundException(srcPath);
	        if (!DirectoryExistsImpl(destPath.GetDirectory()))
	        {
	            throw FileSystemExceptionHelper.NewDirectoryNotFoundException(destPath.GetDirectory());
	        }
	        var destEntry = GetEntry(destPath);
	        if (destEntry != null)
	        {
	            throw new IOException("Cannot overwrite existing file.");
	        }        
	        destEntry = CreateEntry(destPath.FullName);
	        TryGetDispatcher()?.RaiseCreated(destPath);
	        using (var destStream = destEntry.Open())
	        {
	            using var srcStream = srcEntry.Open();
	            srcStream.CopyTo(destStream);
	        }
	        RemoveEntry(srcEntry);
	        TryGetDispatcher()?.RaiseDeleted(srcPath);
	    }
	    protected override Stream OpenFileImpl(UPath path, FileMode mode, FileAccess access, FileShare share)
	    {
	        if (_archive.Mode == ZipArchiveMode.Read && access == FileAccess.Write)
	        {
	            throw new UnauthorizedAccessException("Cannot open a file for writing in a read-only archive.");
	        }
	        if (access == FileAccess.Read && (mode == FileMode.CreateNew || mode == FileMode.Create || mode == FileMode.Truncate || mode == FileMode.Append))
	        {
	            throw new ArgumentException("Cannot write in a read-only access.");
	        }
	        var entry = GetEntry(path, out var isDirectory);
	        if (isDirectory)
	        {
	            throw new UnauthorizedAccessException(nameof(path) + " is a directory.");
	        }
	        if (entry == null)
	        {
	            if (mode is FileMode.Create or FileMode.CreateNew or FileMode.OpenOrCreate or FileMode.Append)
	            {
	                entry = CreateEntry(path.FullName);
	#if NETSTANDARD2_1
	                entry.ExternalAttributes = (int)FileAttributes.Archive;
	#endif
	                TryGetDispatcher()?.RaiseCreated(path);
	            }
	            else
	            {
	                if (!DirectoryExistsImpl(path.GetDirectory()))
	                {
	                    throw FileSystemExceptionHelper.NewDirectoryNotFoundException(path.GetDirectory());
	                }
	                throw FileSystemExceptionHelper.NewFileNotFoundException(path);
	            }
	        }
	        else if (mode == FileMode.CreateNew)
	        {
	            throw new IOException("Cannot create a file in CreateNew mode if it already exists.");
	        }
	        else if (mode == FileMode.Create)
	        {
	            RemoveEntry(entry);
	            entry = CreateEntry(path.FullName);
	        }
	#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
	        if ((access == FileAccess.Write || access == FileAccess.ReadWrite) && (entry.ExternalAttributes & (int)FileAttributes.ReadOnly) == (int)FileAttributes.ReadOnly)
	        {
	            throw new UnauthorizedAccessException("Cannot open a file for writing in a file with readonly attribute.");
	        }
	#endif
	        var stream = new ZipEntryStream(share, this, entry);
	#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
	        if (access is FileAccess.Write or FileAccess.ReadWrite)
	        {
	            entry.ExternalAttributes |= (int)FileAttributes.Archive;
	        }
	#endif
	        if (mode == FileMode.Append)
	        {
	            stream.Seek(0, SeekOrigin.End);
	        }
	        else if (mode == FileMode.Truncate)
	        {
	            stream.SetLength(0);
	        }
	        return stream;
	    }
	    protected override void ReplaceFileImpl(UPath srcPath, UPath destPath, UPath destBackupPath, bool ignoreMetadataErrors)
	    {
	        var sourceEntry = GetEntry(srcPath);
	        if (sourceEntry is null)
	        {
	            throw FileSystemExceptionHelper.NewFileNotFoundException(srcPath);
	        }
	        var destEntry = GetEntry(destPath);
	        if (destEntry == sourceEntry)
	        {
	            throw new IOException("Cannot replace the file with itself.");
	        }
	        if (destEntry != null)
	        {
	            // create a backup at destBackupPath if its not null
	            if (!destBackupPath.IsEmpty)
	            {
	                var destBackupEntry = CreateEntry(destBackupPath.FullName);
	                using var destBackupStream = destBackupEntry.Open();
	                using var destStream = destEntry.Open();
	                destStream.CopyTo(destBackupStream);
	            }
	            RemoveEntry(destEntry);
	        }
	        var newEntry = CreateEntry(destPath.FullName);
	        using (var newStream = newEntry.Open())
	        {
	            using (var sourceStream = sourceEntry.Open())
	            {
	                sourceStream.CopyTo(newStream);
	            }
	        }
	        RemoveEntry(sourceEntry);
	        TryGetDispatcher()?.RaiseDeleted(srcPath);
	        TryGetDispatcher()?.RaiseCreated(destPath);
	    }
	    protected override void SetAttributesImpl(UPath path, FileAttributes attributes)
	    {
	#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
	        var entry = GetEntry(path);
	        if (entry == null)
	        {
	            throw FileSystemExceptionHelper.NewFileNotFoundException(path);
	        }
	        entry.ExternalAttributes = (int)attributes;
	        TryGetDispatcher()?.RaiseChange(path);
	#else
	        Debug.WriteLine("SetAttributes don't work in NetStandard2.0 or older.");
	#endif
	    }
	    protected override void SetCreationTimeImpl(UPath path, DateTime time)
	    {
	    }
	    protected override void SetLastAccessTimeImpl(UPath path, DateTime time)
	    {
	    }
	    protected override void SetLastWriteTimeImpl(UPath path, DateTime time)
	    {
	        var entry = GetEntry(path);
	        if (entry is null)
	        {
	            throw FileSystemExceptionHelper.NewFileNotFoundException(path);
	        }
	        TryGetDispatcher()?.RaiseChange(path);
	        entry.LastWriteTime = time;
	    }
	    protected override void CreateSymbolicLinkImpl(UPath path, UPath pathToTarget)
	    {
	        throw new NotSupportedException("Symbolic links are not supported by ZipArchiveFileSystem");
	    }
	    protected override bool TryResolveLinkTargetImpl(UPath linkPath, out UPath resolvedPath)
	    {
	        resolvedPath = UPath.Empty;
	        return false;
	    }
	    protected override IFileSystemWatcher WatchImpl(UPath path)
	    {
	        var watcher = new FileSystemWatcher(this, path);
	        lock (_dispatcherLock)
	        {
	            _dispatcher ??= new FileSystemEventDispatcher<FileSystemWatcher>(this);
	            _dispatcher.Add(watcher);
	        }
	        return watcher;
	    }
	    private void RemoveEntry(ZipArchiveEntry entry)
	    {
	        _entriesLock.EnterWriteLock();
	        try
	        {
	            entry.Delete();
	            _entries.Remove(new UPath(entry.FullName).ToAbsolute());
	        }
	        finally
	        {
	            _entriesLock.ExitWriteLock();
	        }
	    }
	    private ZipArchiveEntry CreateEntry(UPath path, bool isDirectory = false)
	    {
	        _entriesLock.EnterWriteLock();
	        try
	        {
	            var internalPath = path.FullName;
	            if (isDirectory)
	            {
	                internalPath += DirectorySeparator;
	            }
	            var entry = _archive.CreateEntry(internalPath, _compressionLevel);
	            _entries[path] = new InternalZipEntry(entry, isDirectory);
	            return entry;
	        }
	        finally
	        {
	            _entriesLock.ExitWriteLock();
	        }
	    }
	    private static readonly char[] s_slashChars = { '/', '\\' };
	    private static string GetName(ZipArchiveEntry entry)
	    {
	        var name = entry.FullName.TrimEnd(s_slashChars);
	        var index = name.LastIndexOfAny(s_slashChars);
	        return name.Substring(index + 1);
	    }
	    private static string GetParent(string path)
	    {
	        path = path.TrimEnd(s_slashChars);
	        var lastIndex = path.LastIndexOfAny(s_slashChars);
	        return lastIndex == -1 ? "" : path.Substring(0, lastIndex);
	    }
	    private FileSystemEventDispatcher<FileSystemWatcher>? TryGetDispatcher()
	    {
	        lock (_dispatcherLock)
	        {
	            return _dispatcher;
	        }
	    }
	    private sealed class ZipEntryStream : Stream
	    {
	        private readonly ZipArchiveEntry _entry;
	        private readonly ZipArchiveFileSystem _fileSystem;
	        private readonly Stream _streamImplementation;
	        private bool _isDisposed;
	        public ZipEntryStream(FileShare share, ZipArchiveFileSystem system, ZipArchiveEntry entry)
	        {
	            _entry = entry;
	            _fileSystem = system;
	            lock (_fileSystem._openStreamsLock)
	            {
	                var fileShare = _fileSystem._openStreams.TryGetValue(entry, out var fileData) ? fileData.Share : FileShare.ReadWrite;
	                if (fileData != null)
	                {
	                    // we only check for read share, because ZipArchive doesn't support write share
	                    if (share is not FileShare.Read and not FileShare.ReadWrite)
	                    {
	                        throw new IOException("File is already opened for reading");
	                    }
	                    if (fileShare is not FileShare.Read and not FileShare.ReadWrite)
	                    {
	                        throw new IOException("File is already opened for reading by another stream with non compatible share");
	                    }
	                    fileData.Count++;
	                }
	                else
	                {
	                    _fileSystem._openStreams.Add(_entry, new EntryState(share));
	                }
	                _streamImplementation = entry.Open();
	            }
	            Share = share;
	        }
	        private FileShare Share { get; }
	        public override bool CanRead => _streamImplementation.CanRead;
	        public override bool CanSeek => _streamImplementation.CanSeek;
	        public override bool CanWrite => _streamImplementation.CanWrite;
	        public override long Length => _streamImplementation.Length;
	        public override long Position
	        {
	            get => _streamImplementation.Position;
	            set => _streamImplementation.Position = value;
	        }
	        public override void Flush()
	        {
	            _streamImplementation.Flush();
	        }
	        public override int Read(byte[] buffer, int offset, int count)
	        {
	            return _streamImplementation.Read(buffer, offset, count);
	        }
	        public override long Seek(long offset, SeekOrigin origin)
	        {
	            return _streamImplementation.Seek(offset, origin);
	        }
	        public override void SetLength(long value)
	        {
	            _streamImplementation.SetLength(value);
	        }
	        public override void Write(byte[] buffer, int offset, int count)
	        {
	            _streamImplementation.Write(buffer, offset, count);
	        }
	        public override void Close()
	        {
	            if (_isDisposed)
	            {
	                return;
	            }
	            _streamImplementation.Close();
	            _isDisposed = true;
	            lock (_fileSystem._openStreamsLock)
	            {
	                if (!_fileSystem._openStreams.TryGetValue(_entry, out var fileData))
	                {
	                    return;
	                }
	                fileData.Count--;
	                if (fileData.Count == 0)
	                {
	                    _fileSystem._openStreams.Remove(_entry);
	                }
	            }
	        }
	    }
	    private sealed class EntryState
	    {
	        public EntryState(FileShare share)
	        {
	            Share = share;
	            Count = 1;
	        }
	        public FileShare Share { get; }
	        public int Count;
	    }
	    private readonly struct InternalZipEntry
	    {
	        public InternalZipEntry(ZipArchiveEntry entry, bool isDirectory)
	        {
	            Entry = entry;
	            IsDirectory = isDirectory;
	        }
	        public readonly ZipArchiveEntry Entry;
	        public readonly bool IsDirectory;
	    }
	}
	#endregion
	#region \Polyfills
	    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false)]
	    internal sealed class NotNullAttribute : Attribute
	    {
	    }
	    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
	    internal sealed class NotNullWhenAttribute : Attribute
	    {
	        public NotNullWhenAttribute(bool returnValue)
	        {
	            ReturnValue = returnValue;
	        }
	        public bool ReturnValue { get; }
	    }
	#endregion
}
#endregion
