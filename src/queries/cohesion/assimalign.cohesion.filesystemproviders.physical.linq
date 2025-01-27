<Query Kind="Program">
<Namespace>System</Namespace>
<Namespace>System.IO</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>Assimalign.Cohesion.FileSystemGlobbing</Namespace>
<Namespace>Assimalign.Cohesion.FileProviders.Internal</Namespace>
<Namespace>System.Diagnostics</Namespace>
<Namespace>System.Threading</Namespace>
<Namespace>Assimalign.Cohesion.FileProviders.Internal.Utilities</Namespace>
<Namespace>System.Collections.Concurrent</Namespace>
<Namespace>System.Runtime.Versioning</Namespace>
<Namespace>System.Security</Namespace>
<Namespace>System.Threading.Tasks</Namespace>
<Namespace>Assimalign.Cohesion.Primitives</Namespace>
<Namespace>Assimalign.Cohesion.FileProviders</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Security.Cryptography</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>System.Reflection</Namespace>
</Query>
#load ".\assimalign.cohesion.filesystemproviders"
#load ".\assimalign.cohesion.core"

void Main()
{

}

#region Assimalign.Cohesion.FileSystemProviders.Physical(net8.0)
namespace Assimalign.Cohesion.FileSystemProviders.Physical
{
	#region \
	public class PhysicalDirectoryInfo : IFileSystemDirectoryInfo
	{
	    private readonly string directory;
	    private readonly DirectoryInfo directoryInfo;
	    private readonly ExclusionFilterType directoryInfoFilters;
	    private IEnumerable<IFileSystemInfo> files;
	    public PhysicalDirectoryInfo(string directory)
	        : this(directory, ExclusionFilterType.Sensitive) { }
	    public PhysicalDirectoryInfo(string directory, ExclusionFilterType filters)
	        : this(new DirectoryInfo(directory), filters)
	    {
	        this.directory = directory;
	    }
	    public PhysicalDirectoryInfo(DirectoryInfo directoryInfo)
	        : this(directoryInfo, ExclusionFilterType.Sensitive) { }
	    internal PhysicalDirectoryInfo(DirectoryInfo directoryInfo, ExclusionFilterType filters)
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
	                        FileInfo file => new PhysicalFileInfo(file),
	                        DirectoryInfo directory => new PhysicalDirectoryInfo(directory),
	                    };
	                });
	        }
	        catch (Exception ex) when (ex is DirectoryNotFoundException || ex is IOException)
	        {
	            files = Enumerable.Empty<IFileSystemInfo>();
	        }
	    }
	    public bool Exists => directoryInfo.Exists;
	    public long Length => -1;
	    public string PhysicalPath => directoryInfo.FullName;
	    public string Name => directoryInfo.Name;
	    public DateTimeOffset LastModified => directoryInfo.LastWriteTimeUtc;
	    public bool IsDirectory => true;
	    public string FullName => directoryInfo.FullName;
	    public IFileSystemDirectoryInfo? ParentDirectory => directoryInfo.Parent is null ? null : new PhysicalDirectoryInfo(directoryInfo.Parent);
	    Stream IFileSystemInfo.CreateReadStream() => throw new InvalidOperationException();
	    public IEnumerable<IFileSystemInfo> EnumerateFileSystem()
	    {
	        EnsureInitialized();
	        return files;
	    }
	    public IFileSystemDirectoryInfo? GetDirectory(string path)
	    {
	        var fullPath = Path.Combine(directoryInfo.FullName, path);
	        return new PhysicalDirectoryInfo(fullPath);
	    }
	    public IFileSystemInfo? GetFile(string path)
	    {
	        var fullPath = Path.Combine(directoryInfo.FullName, path);
	        return new PhysicalFileInfo(fullPath);
	    }
	}
	public class PhysicalFileProvider : IFileSystemProvider, IDisposable
	{
	    private const string PollingEnvironmentKey = "DOTNET_USE_POLLING_FILE_WATCHER";
	    private static readonly char[] separators = new[] {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar};
	    private readonly ExclusionFilterType _filters;
	    private readonly Func<PhysicalFilesWatcher> fileWatcherFactory;
	    private PhysicalFilesWatcher fileWatcher;
	    private bool fileWatcherInitialized;
	    private object fileWatcherLock = new object();
	    private bool? usePollingFileWatcher;
	    private bool? useActivePolling;
	    private bool isDisposed;
	    public PhysicalFileProvider(string root) : this(root, ExclusionFilterType.Sensitive) { }
	    public PhysicalFileProvider(string root, ExclusionFilterType filters)
	    {
	        if (!Path.IsPathRooted(root))
	        {
	            throw new ArgumentException("The path must be absolute.", nameof(root));
	        }
	        string fullRoot = Path.GetFullPath(root);
	        // When we do matches in GetFullPath, we want to only match full directory names.
	        Root = PathUtilities.EnsureTrailingSlash(fullRoot);
	        if (!Directory.Exists(Root))
	        {
	            throw new DirectoryNotFoundException(Root);
	        }
	        _filters = filters;
	        fileWatcherFactory = () => CreateFileWatcher();
	    }
	    public bool UsePollingFileWatcher
	    {
	        get
	        {
	            if (fileWatcher != null)
	            {
	                return false;
	            }
	            if (usePollingFileWatcher == null)
	            {
	                ReadPollingEnvironmentVariables();
	            }
	            return usePollingFileWatcher ?? false;
	        }
	        set
	        {
	            if (fileWatcher != null)
	            {
	                throw new InvalidOperationException();// SR.Format(SR.CannotModifyWhenFileWatcherInitialized, nameof(UsePollingFileWatcher)));
	            }
	            usePollingFileWatcher = value;
	        }
	    }
	    public bool UseActivePolling
	    {
	        get
	        {
	            if (useActivePolling == null)
	            {
	                ReadPollingEnvironmentVariables();
	            }
	            return useActivePolling.Value;
	        }
	        set => useActivePolling = value;
	    }
	    public string Root { get; }
	    internal PhysicalFilesWatcher FileWatcher
	    {
	        get
	        {
	            return LazyInitializer.EnsureInitialized(
	                ref fileWatcher,
	                ref fileWatcherInitialized,
	                ref fileWatcherLock,
	                fileWatcherFactory);
	        }
	        set
	        {
	            Debug.Assert(!fileWatcherInitialized);
	            fileWatcherInitialized = true;
	            fileWatcher = value;
	        }
	    }
	    internal PhysicalFilesWatcher CreateFileWatcher()
	    {
	        string root = PathUtilities.EnsureTrailingSlash(Path.GetFullPath(Root));
	        FileSystemWatcher watcher;
	        //  For browser/iOS/tvOS we will proactively fallback to polling since FileSystemWatcher is not supported.
	        if (OperatingSystem.IsBrowser() || (OperatingSystem.IsIOS() && !OperatingSystem.IsMacCatalyst()) || OperatingSystem.IsTvOS())
	        {
	            UsePollingFileWatcher = true;
	            UseActivePolling = true;
	            watcher = null;
	        }
	        else
	        {
	            // When UsePollingFileWatcher & UseActivePolling are set, we won't use a FileSystemWatcher.
	            watcher = UsePollingFileWatcher && UseActivePolling ? null : new FileSystemWatcher(root);
	        }
	        return new PhysicalFilesWatcher(root, watcher, UsePollingFileWatcher, _filters)
	        {
	            UseActivePolling = UseActivePolling,
	        };
	    }
	    private void ReadPollingEnvironmentVariables()
	    {
	        string environmentValue = Environment.GetEnvironmentVariable(PollingEnvironmentKey);
	        bool pollForChanges = string.Equals(environmentValue, "1", StringComparison.Ordinal) ||
	            string.Equals(environmentValue, "true", StringComparison.OrdinalIgnoreCase);
	        usePollingFileWatcher = pollForChanges;
	        useActivePolling = pollForChanges;
	    }
	    public void Dispose()
	    {
	        Dispose(true);
	        GC.SuppressFinalize(this);
	    }
	    protected virtual void Dispose(bool disposing)
	    {
	        if (!isDisposed)
	        {
	            if (disposing)
	            {
	                fileWatcher?.Dispose();
	            }
	            isDisposed = true;
	        }
	    }
	    private string GetFullPath(string path)
	    {
	        if (PathUtilities.PathNavigatesAboveRoot(path))
	        {
	            return null;
	        }
	        string fullPath;
	        try
	        {
	            fullPath = Path.GetFullPath(Path.Combine(Root, path));
	        }
	        catch
	        {
	            return null;
	        }
	        if (!IsUnderneathRoot(fullPath))
	        {
	            return null;
	        }
	        return fullPath;
	    }
	    private bool IsUnderneathRoot(string fullPath)
	    {
	        return fullPath.StartsWith(Root, StringComparison.OrdinalIgnoreCase);
	    }
	    public IFileSystemInfo GetFile(string subpath)
	    {
	        if (string.IsNullOrEmpty(subpath) || PathUtilities.HasInvalidPathChars(subpath))
	        {
	            return null;
	        }
	        // Relative paths starting with leading slashes are okay
	        subpath = subpath.TrimStart(separators);
	        // Absolute paths not permitted.
	        if (Path.IsPathRooted(subpath))
	        {
	            return null;
	        }
	        string fullPath = GetFullPath(subpath);
	        if (fullPath == null)
	        {
	            return null;
	        }
	        var fileInfo = new FileInfo(fullPath);
	        if (FileSystemInfoHelper.IsExcluded(fileInfo, _filters))
	        {
	            return null;
	        }
	        return new PhysicalFileInfo(fileInfo);
	    }
	    public IFileSystemDirectoryInfo GetDirectory(string subpath)
	    {
	        try
	        {
	            if (subpath == null || PathUtilities.HasInvalidPathChars(subpath))
	            {
	                return null;
	            }
	            // Relative paths starting with leading slashes are okay
	            subpath = subpath.TrimStart(separators);
	            // Absolute paths not permitted.
	            if (Path.IsPathRooted(subpath))
	            {
	                return null;
	            }
	            string fullPath = GetFullPath(subpath);
	            if (fullPath == null || !Directory.Exists(fullPath))
	            {
	                return null;
	            }
	            return new PhysicalDirectoryInfo(fullPath, _filters);
	        }
	        catch (DirectoryNotFoundException)
	        {
	        }
	        catch (IOException)
	        {
	        }
	        return null;
	    }
	    public IChangeToken Watch(string filter)
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
	        return FileWatcher.CreateFileChangeToken(filter);
	    }
	}
	public abstract class PhysicalFileSystemInfo : IFileSystemInfo
	{
	    private readonly FileInfo fileInfo;
	    public PhysicalFileSystemInfo(FileInfo fileInfo)
	    {
	        this.fileInfo = fileInfo;
	    }
	    public PhysicalFileSystemInfo(string path)
	    {
	        this.fileInfo = new FileInfo(path);
	    }
	    public FileName Name => throw new NotImplementedException();
	    public FilePath Path => throw new NotImplementedException();
	    public DateTimeOffset UpdatedDateTime => throw new NotImplementedException();
	    public DateTimeOffset CreatedDateTime => throw new NotImplementedException();
	    public abstract bool IsDirectory { get; }
	    public abstract bool IsFile { get; }
	    public Stream CreateReadStream()
	    {
	        // We are setting buffer size to 1 to prevent FileStream from allocating it's internal buffer
	        // 0 causes constructor to throw
	        int bufferSize = 1;
	        return new FileStream(
	            FullName,
	            FileMode.Open,
	            FileAccess.Read,
	            FileShare.ReadWrite,
	            bufferSize,
	            FileOptions.Asynchronous | FileOptions.SequentialScan);
	    }
	}
	public class PhysicalFilesWatcher : IDisposable
	{
	    private static readonly Action<object> _cancelTokenSource = state => ((CancellationTokenSource)state).Cancel();
	    internal static TimeSpan DefaultPollingInterval = TimeSpan.FromSeconds(4);
	    private readonly ConcurrentDictionary<string, ChangeTokenInfo> _filePathTokenLookup = new();
	    private readonly ConcurrentDictionary<string, ChangeTokenInfo> _wildcardTokenLookup = new(StringComparer.OrdinalIgnoreCase);
	    private readonly FileSystemWatcher _fileWatcher;
	    private readonly object _fileWatcherLock = new object();
	    private readonly string _root;
	    private readonly ExclusionFilterType _filters;
	    private Timer _timer;
	    private bool _timerInitialzed;
	    private object _timerLock = new object();
	    private Func<Timer> _timerFactory;
	    private bool _disposed;
	    public PhysicalFilesWatcher(string root, FileSystemWatcher fileSystemWatcher, bool pollForChanges)
	        : this(root, fileSystemWatcher, pollForChanges, ExclusionFilterType.Sensitive) { }
	    public PhysicalFilesWatcher(
	        string root,
	        FileSystemWatcher fileSystemWatcher,
	        bool pollForChanges,
	        ExclusionFilterType filters)
	    {
	        if (fileSystemWatcher == null && !pollForChanges)
	        {
	            throw new ArgumentNullException(); // nameof(fileSystemWatcher), SR.Error_FileSystemWatcherRequiredWithoutPolling);
	        }
	        _root = root;
	        if (fileSystemWatcher != null)
	        {
	            if (OperatingSystem.IsBrowser() || (OperatingSystem.IsIOS() && !OperatingSystem.IsMacCatalyst()) || OperatingSystem.IsTvOS())
	            {
	                throw new PlatformNotSupportedException();// SR.Format(SR.FileSystemWatcher_PlatformNotSupported, typeof(FileSystemWatcher)));
	            }
	            _fileWatcher = fileSystemWatcher;
	            _fileWatcher.IncludeSubdirectories = true;
	            _fileWatcher.Created += OnChanged;
	            _fileWatcher.Changed += OnChanged;
	            _fileWatcher.Renamed += OnRenamed;
	            _fileWatcher.Deleted += OnChanged;
	            _fileWatcher.Error += OnError;
	        }
	        PollForChanges = pollForChanges;
	        _filters = filters;
	        PollingChangeTokens = new ConcurrentDictionary<IPollingChangeToken, IPollingChangeToken>();
	        _timerFactory = () => NonCapturingTimer.Create(RaiseChangeEvents, state: PollingChangeTokens, dueTime: TimeSpan.Zero, period: DefaultPollingInterval);
	    }
	    internal bool PollForChanges { get; }
	    internal bool UseActivePolling { get; set; }
	    internal ConcurrentDictionary<IPollingChangeToken, IPollingChangeToken> PollingChangeTokens { get; }
	    public IChangeToken CreateFileChangeToken(string filter)
	    {
	        if (filter == null)
	        {
	            throw new ArgumentNullException(nameof(filter));
	        }
	        filter = NormalizePath(filter);
	        // Absolute paths and paths traversing above root not permitted.
	        if (Path.IsPathRooted(filter) || PathUtilities.PathNavigatesAboveRoot(filter))
	        {
	            return NullChangeToken.Singleton;
	        }
	        IChangeToken changeToken = GetOrAddChangeToken(filter);
	        // We made sure that browser/iOS/tvOS never uses FileSystemWatcher.
	#pragma warning disable CA1416 // Validate platform compatibility
	        TryEnableFileSystemWatcher();
	#pragma warning restore CA1416 // Validate platform compatibility
	        return changeToken;
	    }
	    private IChangeToken GetOrAddChangeToken(string pattern)
	    {
	        if (UseActivePolling)
	        {
	            LazyInitializer.EnsureInitialized(ref _timer, ref _timerInitialzed, ref _timerLock, _timerFactory);
	        }
	        IChangeToken changeToken;
	        bool isWildCard = pattern.IndexOf('*') != -1;
	        if (isWildCard || IsDirectoryPath(pattern))
	        {
	            changeToken = GetOrAddWildcardChangeToken(pattern);
	        }
	        else
	        {
	            changeToken = GetOrAddFilePathChangeToken(pattern);
	        }
	        return changeToken;
	    }
	    internal IChangeToken GetOrAddFilePathChangeToken(string filePath)
	    {
	        if (!_filePathTokenLookup.TryGetValue(filePath, out ChangeTokenInfo tokenInfo))
	        {
	            var cancellationTokenSource = new CancellationTokenSource();
	            var cancellationChangeToken = new CancellationChangeToken(cancellationTokenSource.Token);
	            tokenInfo = new ChangeTokenInfo(cancellationTokenSource, cancellationChangeToken);
	            tokenInfo = _filePathTokenLookup.GetOrAdd(filePath, tokenInfo);
	        }
	        IChangeToken changeToken = tokenInfo.ChangeToken;
	        if (PollForChanges)
	        {
	            // The expiry of CancellationChangeToken is controlled by this type and consequently we can cache it.
	            // PollingFileChangeToken on the other hand manages its own lifetime and consequently we cannot cache it.
	            var pollingChangeToken = new PollingFileChangeToken(new System.IO.FileInfo(Path.Combine(_root, filePath)));
	            if (UseActivePolling)
	            {
	                pollingChangeToken.ActiveChangeCallbacks = true;
	                pollingChangeToken.CancellationTokenSource = new CancellationTokenSource();
	                PollingChangeTokens.TryAdd(pollingChangeToken, pollingChangeToken);
	            }
	            changeToken = new CompositeChangeToken(
	                new IChangeToken[]
	                {
	                    changeToken,
	                    pollingChangeToken,
	                });
	        }
	        return changeToken;
	    }
	    internal IChangeToken GetOrAddWildcardChangeToken(string pattern)
	    {
	        if (!_wildcardTokenLookup.TryGetValue(pattern, out ChangeTokenInfo tokenInfo))
	        {
	            var cancellationTokenSource = new CancellationTokenSource();
	            var cancellationChangeToken = new CancellationChangeToken(cancellationTokenSource.Token);
	            var matcher = new FilePatternMatcher(StringComparison.OrdinalIgnoreCase);
	            matcher.AddInclude(pattern);
	            tokenInfo = new ChangeTokenInfo(cancellationTokenSource, cancellationChangeToken, matcher);
	            tokenInfo = _wildcardTokenLookup.GetOrAdd(pattern, tokenInfo);
	        }
	        IChangeToken changeToken = tokenInfo.ChangeToken;
	        if (PollForChanges)
	        {
	            // The expiry of CancellationChangeToken is controlled by this type and consequently we can cache it.
	            // PollingFileChangeToken on the other hand manages its own lifetime and consequently we cannot cache it.
	            var pollingChangeToken = new PollingWildCardChangeToken(_root, pattern);
	            if (UseActivePolling)
	            {
	                pollingChangeToken.ActiveChangeCallbacks = true;
	                pollingChangeToken.CancellationTokenSource = new CancellationTokenSource();
	                PollingChangeTokens.TryAdd(pollingChangeToken, pollingChangeToken);
	            }
	            changeToken = new CompositeChangeToken(
	                new[]
	                {
	                    changeToken,
	                    pollingChangeToken,
	                });
	        }
	        return changeToken;
	    }
	    public void Dispose()
	    {
	        Dispose(true);
	        GC.SuppressFinalize(this);
	    }
	    protected virtual void Dispose(bool disposing)
	    {
	        if (!_disposed)
	        {
	            if (disposing)
	            {
	                _fileWatcher?.Dispose();
	                _timer?.Dispose();
	            }
	            _disposed = true;
	        }
	    }
	    [UnsupportedOSPlatform("browser")]
	    [UnsupportedOSPlatform("ios")]
	    [UnsupportedOSPlatform("tvos")]
	    [SupportedOSPlatform("maccatalyst")]
	    private void OnRenamed(object sender, RenamedEventArgs e)
	    {
	        // For a file name change or a directory's name change notify registered tokens.
	        OnFileSystemEntryChange(e.OldFullPath);
	        OnFileSystemEntryChange(e.FullPath);
	        if (Directory.Exists(e.FullPath))
	        {
	            try
	            {
	                // If the renamed entity is a directory then notify tokens for every sub item.
	                foreach (
	                    string newLocation in
	                    Directory.EnumerateFileSystemEntries(e.FullPath, "*", SearchOption.AllDirectories))
	                {
	                    // Calculated previous path of this moved item.
	                    string oldLocation = Path.Combine(e.OldFullPath, newLocation.Substring(e.FullPath.Length + 1));
	                    OnFileSystemEntryChange(oldLocation);
	                    OnFileSystemEntryChange(newLocation);
	                }
	            }
	            catch (Exception ex) when (
	                ex is IOException ||
	                ex is SecurityException ||
	                ex is DirectoryNotFoundException ||
	                ex is UnauthorizedAccessException)
	            {
	                // Swallow the exception.
	            }
	        }
	    }
	    [UnsupportedOSPlatform("browser")]
	    [UnsupportedOSPlatform("ios")]
	    [UnsupportedOSPlatform("tvos")]
	    [SupportedOSPlatform("maccatalyst")]
	    private void OnChanged(object sender, FileSystemEventArgs e)
	    {
	        OnFileSystemEntryChange(e.FullPath);
	    }
	    [UnsupportedOSPlatform("browser")]
	    [UnsupportedOSPlatform("ios")]
	    [UnsupportedOSPlatform("tvos")]
	    [SupportedOSPlatform("maccatalyst")]
	    private void OnError(object sender, ErrorEventArgs e)
	    {
	        // Notify all cache entries on error.
	        foreach (string path in _filePathTokenLookup.Keys)
	        {
	            ReportChangeForMatchedEntries(path);
	        }
	    }
	    [UnsupportedOSPlatform("browser")]
	    [UnsupportedOSPlatform("ios")]
	    [UnsupportedOSPlatform("tvos")]
	    [SupportedOSPlatform("maccatalyst")]
	    private void OnFileSystemEntryChange(string fullPath)
	    {
	        try
	        {
	            var fileSystemInfo = new System.IO.FileInfo(fullPath);
	            if (FileSystemInfoHelper.IsExcluded(fileSystemInfo, _filters))
	            {
	                return;
	            }
	            string relativePath = fullPath.Substring(_root.Length);
	            ReportChangeForMatchedEntries(relativePath);
	        }
	        catch (Exception ex) when (
	            ex is IOException ||
	            ex is SecurityException ||
	            ex is UnauthorizedAccessException)
	        {
	            // Swallow the exception.
	        }
	    }
	    [UnsupportedOSPlatform("browser")]
	    [UnsupportedOSPlatform("ios")]
	    [UnsupportedOSPlatform("tvos")]
	    [SupportedOSPlatform("maccatalyst")]
	    private void ReportChangeForMatchedEntries(string path)
	    {
	        if (string.IsNullOrEmpty(path))
	        {
	            // System.IO.FileSystemWatcher may trigger events that are missing the file name,
	            // which makes it appear as if the root directory is renamed or deleted. Moving the root directory
	            // of the file watcher is not supported, so this type of event is ignored.
	            return;
	        }
	        path = NormalizePath(path);
	        bool matched = false;
	        if (_filePathTokenLookup.TryRemove(path, out ChangeTokenInfo matchInfo))
	        {
	            CancelToken(matchInfo);
	            matched = true;
	        }
	        foreach (System.Collections.Generic.KeyValuePair<string, ChangeTokenInfo> wildCardEntry in _wildcardTokenLookup)
	        {
	            FilePatternMatchingResult matchResult = wildCardEntry.Value.Matcher.Match(path);
	            if (matchResult.HasMatches &&
	                _wildcardTokenLookup.TryRemove(wildCardEntry.Key, out matchInfo))
	            {
	                CancelToken(matchInfo);
	                matched = true;
	            }
	        }
	        if (matched)
	        {
	            TryDisableFileSystemWatcher();
	        }
	    }
	    [UnsupportedOSPlatform("browser")]
	    [UnsupportedOSPlatform("ios")]
	    [UnsupportedOSPlatform("tvos")]
	    [SupportedOSPlatform("maccatalyst")]
	    private void TryDisableFileSystemWatcher()
	    {
	        if (_fileWatcher != null)
	        {
	            lock (_fileWatcherLock)
	            {
	                if (_filePathTokenLookup.IsEmpty &&
	                    _wildcardTokenLookup.IsEmpty &&
	                    _fileWatcher.EnableRaisingEvents)
	                {
	                    // Perf: Turn off the file monitoring if no files to monitor.
	                    _fileWatcher.EnableRaisingEvents = false;
	                }
	            }
	        }
	    }
	    [UnsupportedOSPlatform("browser")]
	    [UnsupportedOSPlatform("ios")]
	    [UnsupportedOSPlatform("tvos")]
	    [SupportedOSPlatform("maccatalyst")]
	    private void TryEnableFileSystemWatcher()
	    {
	        if (_fileWatcher != null)
	        {
	            lock (_fileWatcherLock)
	            {
	                if ((!_filePathTokenLookup.IsEmpty || !_wildcardTokenLookup.IsEmpty) &&
	                    !_fileWatcher.EnableRaisingEvents)
	                {
	                    // Perf: Turn off the file monitoring if no files to monitor.
	                    _fileWatcher.EnableRaisingEvents = true;
	                }
	            }
	        }
	    }
	    private static string NormalizePath(string filter) => filter = filter.Replace('\\', '/');
	    private static bool IsDirectoryPath(string path)
	    {
	        return path.Length > 0 &&
	            (path[path.Length - 1] == Path.DirectorySeparatorChar ||
	            path[path.Length - 1] == Path.AltDirectorySeparatorChar);
	    }
	    private static void CancelToken(ChangeTokenInfo matchInfo)
	    {
	        if (matchInfo.TokenSource.IsCancellationRequested)
	        {
	            return;
	        }
	        Task.Factory.StartNew(
	            _cancelTokenSource,
	            matchInfo.TokenSource,
	            CancellationToken.None,
	            TaskCreationOptions.DenyChildAttach,
	            TaskScheduler.Default);
	    }
	    internal static void RaiseChangeEvents(object state)
	    {
	        // Iterating over a concurrent bag gives us a point in time snapshot making it safe
	        // to remove items from it.
	        var changeTokens = (ConcurrentDictionary<IPollingChangeToken, IPollingChangeToken>)state;
	        foreach (System.Collections.Generic.KeyValuePair<IPollingChangeToken, IPollingChangeToken> item in changeTokens)
	        {
	            IPollingChangeToken token = item.Key;
	            if (!token.HasChanged)
	            {
	                continue;
	            }
	            if (!changeTokens.TryRemove(token, out _))
	            {
	                // Move on if we couldn't remove the item.
	                continue;
	            }
	            // We're already on a background thread, don't need to spawn a background Task to cancel the CTS
	            try
	            {
	                token.CancellationTokenSource.Cancel();
	            }
	            catch
	            {
	            }
	        }
	    }
	    private readonly struct ChangeTokenInfo
	    {
	        public ChangeTokenInfo(
	            CancellationTokenSource tokenSource,
	            CancellationChangeToken changeToken)
	            : this(tokenSource, changeToken, matcher: null)
	        {
	        }
	        public ChangeTokenInfo(
	            CancellationTokenSource tokenSource,
	            CancellationChangeToken changeToken,
	            FilePatternMatcher matcher)
	        {
	            TokenSource = tokenSource;
	            ChangeToken = changeToken;
	            Matcher = matcher;
	        }
	        public CancellationTokenSource TokenSource { get; }
	        public CancellationChangeToken ChangeToken { get; }
	        public FilePatternMatcher Matcher { get; }
	    }
	}
	#endregion
	#region \Internal
	public class PollingFileChangeToken : IPollingChangeToken
	{
	    private readonly FileInfo _fileInfo;
	    private DateTime _previousWriteTimeUtc;
	    private DateTime _lastCheckedTimeUtc;
	    private bool _hasChanged;
	    private CancellationTokenSource _tokenSource;
	    private CancellationChangeToken _changeToken;
	    public PollingFileChangeToken(FileInfo fileInfo)
	    {
	        _fileInfo = fileInfo;
	        _previousWriteTimeUtc = GetLastWriteTimeUtc();
	    }
	    // Internal for unit testing
	    internal static TimeSpan PollingInterval { get; set; } = PhysicalFilesWatcher.DefaultPollingInterval;
	    private DateTime GetLastWriteTimeUtc()
	    {
	        _fileInfo.Refresh();
	        if (!_fileInfo.Exists)
	        {
	            return DateTime.MinValue;
	        }
	        return FileSystemInfoHelper.GetFileLinkTargetLastWriteTimeUtc(_fileInfo) ?? _fileInfo.LastWriteTimeUtc;
	    }
	    public bool ActiveChangeCallbacks { get; internal set; }
	    internal CancellationTokenSource CancellationTokenSource
	    {
	        get => _tokenSource;
	        set
	        {
	            Debug.Assert(_tokenSource == null, "We expect CancellationTokenSource to be initialized exactly once.");
	            _tokenSource = value;
	            _changeToken = new CancellationChangeToken(_tokenSource.Token);
	        }
	    }
	    CancellationTokenSource IPollingChangeToken.CancellationTokenSource => CancellationTokenSource;
	    public bool HasChanged
	    {
	        get
	        {
	            if (_hasChanged)
	            {
	                return _hasChanged;
	            }
	            DateTime currentTime = DateTime.UtcNow;
	            if (currentTime - _lastCheckedTimeUtc < PollingInterval)
	            {
	                return _hasChanged;
	            }
	            DateTime lastWriteTimeUtc = GetLastWriteTimeUtc();
	            if (_previousWriteTimeUtc != lastWriteTimeUtc)
	            {
	                _previousWriteTimeUtc = lastWriteTimeUtc;
	                _hasChanged = true;
	            }
	            _lastCheckedTimeUtc = currentTime;
	            return _hasChanged;
	        }
	    }
	    public IDisposable RegisterChangeCallback(Action<object> callback, object state)
	    {
	        if (!ActiveChangeCallbacks)
	        {
	            return EmptyDisposable.Instance;
	        }
	        return _changeToken.RegisterChangeCallback(callback, state);
	    }
	}
	public class PollingWildCardChangeToken : IPollingChangeToken
	{
	    private static readonly byte[] Separator = Encoding.Unicode.GetBytes("|");
	    private readonly object _enumerationLock = new object();
	    private readonly FileDirectoryInfo _directoryInfo;
	    private readonly FilePatternMatcher _matcher;
	    private bool _changed;
	    private DateTime? _lastScanTimeUtc;
	    private byte[] _byteBuffer;
	    private byte[] _previousHash;
	    private CancellationTokenSource _tokenSource;
	    private CancellationChangeToken _changeToken;
	    public PollingWildCardChangeToken(
	        string root,
	        string pattern)
	        : this(
	            new FileDirectoryInfo(new DirectoryInfo(root)),
	            pattern,
	            Internal.Clock.Instance)
	    {
	    }
	    // Internal for unit testing.
	    internal PollingWildCardChangeToken(
	        FileDirectoryInfo directoryInfo,
	        string pattern,
	        IClock clock)
	    {
	        _directoryInfo = directoryInfo;
	        Clock = clock;
	        _matcher = new FilePatternMatcher(StringComparison.OrdinalIgnoreCase);
	        _matcher.AddInclude(pattern);
	        CalculateChanges();
	    }
	    public bool ActiveChangeCallbacks { get; internal set; }
	    // Internal for unit testing.
	    internal TimeSpan PollingInterval { get; set; } = PhysicalFilesWatcher.DefaultPollingInterval;
	    internal CancellationTokenSource CancellationTokenSource
	    {
	        get => _tokenSource;
	        set
	        {
	            Debug.Assert(_tokenSource == null, "We expect CancellationTokenSource to be initialized exactly once.");
	            _tokenSource = value;
	            _changeToken = new CancellationChangeToken(_tokenSource.Token);
	        }
	    }
	    CancellationTokenSource IPollingChangeToken.CancellationTokenSource => CancellationTokenSource;
	    private IClock Clock { get; }
	    public bool HasChanged
	    {
	        get
	        {
	            if (_changed)
	            {
	                return _changed;
	            }
	            if (Clock.UtcNow - _lastScanTimeUtc >= PollingInterval)
	            {
	                lock (_enumerationLock)
	                {
	                    _changed = CalculateChanges();
	                }
	            }
	            return _changed;
	        }
	    }
	    private bool CalculateChanges()
	    {
	        FilePatternMatchingResult result = _matcher.Execute(_directoryInfo);
	        IOrderedEnumerable<FilePatternMatch> files = result.Files.OrderBy(f => f.Path, StringComparer.Ordinal);
	        using (var sha256 = IncrementalHash.CreateHash(HashAlgorithmName.SHA256))
	        {
	            foreach (FilePatternMatch file in files)
	            {
	                DateTime lastWriteTimeUtc = GetLastWriteUtc(file.Path);
	                if (_lastScanTimeUtc != null && _lastScanTimeUtc < lastWriteTimeUtc)
	                {
	                    // _lastScanTimeUtc is the greatest timestamp that any last writes could have been.
	                    // If a file has a newer timestamp than this value, it must've changed.
	                    return true;
	                }
	                ComputeHash(sha256, file.Path, lastWriteTimeUtc);
	            }
	            byte[] currentHash = sha256.GetHashAndReset();
	            if (!ArrayEquals(_previousHash, currentHash))
	            {
	                return true;
	            }
	            _previousHash = currentHash;
	            _lastScanTimeUtc = Clock.UtcNow;
	        }
	        return false;
	    }
	    protected virtual DateTime GetLastWriteUtc(string path)
	    {
	        string filePath = Path.Combine(_directoryInfo.FullName, path);
	        return FileSystemInfoHelper.GetFileLinkTargetLastWriteTimeUtc(filePath) ?? File.GetLastWriteTimeUtc(filePath);
	    }
	    private static bool ArrayEquals(byte[] previousHash, byte[] currentHash)
	    {
	        if (previousHash == null)
	        {
	            // First run
	            return true;
	        }
	        Debug.Assert(previousHash.Length == currentHash.Length);
	        for (int i = 0; i < previousHash.Length; i++)
	        {
	            if (previousHash[i] != currentHash[i])
	            {
	                return false;
	            }
	        }
	        return true;
	    }
	    private void ComputeHash(IncrementalHash sha256, string path, DateTime lastChangedUtc)
	    {
	        int byteCount = Encoding.Unicode.GetByteCount(path);
	        if (_byteBuffer == null || byteCount > _byteBuffer.Length)
	        {
	            _byteBuffer = new byte[Math.Max(byteCount, 256)];
	        }
	        int length = Encoding.Unicode.GetBytes(path, 0, path.Length, _byteBuffer, 0);
	        sha256.AppendData(_byteBuffer, 0, length);
	        sha256.AppendData(Separator, 0, Separator.Length);
	        Debug.Assert(_byteBuffer.Length > sizeof(long));
	        unsafe
	        {
	            fixed (byte* b = _byteBuffer)
	            {
	                *(long*)b = lastChangedUtc.Ticks;
	            }
	        }
	        sha256.AppendData(_byteBuffer, 0, sizeof(long));
	        sha256.AppendData(Separator, 0, Separator.Length);
	    }
	    IDisposable IChangeToken.RegisterChangeCallback(Action<object> callback, object state)
	    {
	        if (!ActiveChangeCallbacks)
	        {
	            return EmptyDisposable.Instance;
	        }
	        return _changeToken.RegisterChangeCallback(callback, state);
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
	    private static readonly char[] _invalidFileNameChars = Path.GetInvalidFileNameChars()
	        .Where(c => c != Path.DirectorySeparatorChar && c != Path.AltDirectorySeparatorChar).ToArray();
	    private static readonly char[] _invalidFilterChars = _invalidFileNameChars
	        .Where(c => c != '*' && c != '|' && c != '?').ToArray();
	    private static readonly char[] _pathSeparators = new[]
	        {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar};
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
	            path[path.Length - 1] != Path.DirectorySeparatorChar)
	        {
	            return path + Path.DirectorySeparatorChar;
	        }
	        return path;
	    }
	    internal static bool PathNavigatesAboveRoot(string path)
	    {
	        var tokenizer = new StringTokenizer(path, _pathSeparators);
	        int depth = 0;
	        foreach (StringSegment segment in tokenizer)
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
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
}
#endregion
