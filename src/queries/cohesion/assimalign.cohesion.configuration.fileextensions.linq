<Query Kind="Program">
<Namespace>System</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>System.IO</Namespace>
<Namespace>System.Runtime.ExceptionServices</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>System.Threading</Namespace>
<Namespace>Assimalign.Extensions.Primitives</Namespace>
<Namespace>Assimalign.Extensions.FileProviders</Namespace>
<Namespace>Assimalign.Extensions.FileSystemGlobbing</Namespace>
<Namespace>Assimalign.Extensions.Configuration</Namespace>
<Namespace>Assimalign.Extensions.FileProviders.Physical</Namespace>
<Namespace>Assimalign.Extensions.Configuration.Providers</Namespace>
<Namespace>System.Reflection</Namespace>
</Query>
#load ".\assimalign.cohesion.configuration"
#load ".\assimalign.cohesion.core"

void Main()
{

}

#region Assimalign.Cohesion.Configuration.FileExtensions(net8.0)
namespace Assimalign.Cohesion.Configuration.Providers
{
	#region \
	public class ConfigurationFileLoadExceptionContext
	{
	    public ConfigurationFileProvider Provider { get; set; }
	    public Exception Exception { get; set; }
	    public bool Ignore { get; set; }
	}
	public abstract class ConfigurationFileProvider : ConfigurationProvider, IDisposable
	{
	    private readonly IDisposable _changeTokenRegistration;
	    public ConfigurationFileProvider(ConfigurationFileSource source)
	    {
	        Source = source ?? throw new ArgumentNullException(nameof(source));
	        if (Source.ReloadOnChange && Source.FileProvider != null)
	        {
	            _changeTokenRegistration = ChangeToken.OnChange(
	                () => Source.FileProvider.Watch(Source.Path),
	                () =>
	                {
	                    Thread.Sleep(Source.ReloadDelay);
	                    Load(reload: true);
	                });
	        }
	    }
	    public ConfigurationFileSource Source { get; }
	    public override string ToString()
	        => $"{GetType().Name} for '{Source.Path}' ({(Source.Optional ? "Optional" : "Required")})";
	    private void Load(bool reload)
	    {
	        var file = Source.FileProvider?.GetFile(Source.Path);
	        if (file == null || !file.Exists)
	        {
	            if (Source.Optional || reload) // Always optional on reload
	            {
	                Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
	            }
	            else
	            {
	                var error = new StringBuilder("");// SR.Format(SR.Error_FileNotFound, Source.Path));
	                if (!string.IsNullOrEmpty(file?.FullName))
	                {
	                    error.Append("not currently in the works");// SR.Format(SR.Error_ExpectedPhysicalPath, file.PhysicalPath));
	                }
	                HandleException(ExceptionDispatchInfo.Capture(new FileNotFoundException(error.ToString())));
	            }
	        }
	        else
	        {
	            static Stream OpenRead(IFileSystemInfo fileInfo)
	            {
	                if (fileInfo.FullName != null)
	                {
	                    // The default physical file info assumes asynchronous IO which results in unnecessary overhead
	                    // especially since the configuration system is synchronous. This uses the same settings
	                    // and disables async IO.
	                    return new FileStream(
	                        fileInfo.FullName,
	                        FileMode.Open,
	                        FileAccess.Read,
	                        FileShare.ReadWrite,
	                        bufferSize: 1,
	                        FileOptions.SequentialScan);
	                }
	                return fileInfo.CreateReadStream();
	            }
	            using (Stream stream = OpenRead(file))
	            {
	                try
	                {
	                    Load(stream);
	                }
	                catch (Exception ex)
	                {
	                    if (reload)
	                    {
	                        Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
	                    }
	                    var exception = new InvalidDataException();// SR.Format(SR.Error_FailedToLoad, file.PhysicalPath), ex);
	                    HandleException(ExceptionDispatchInfo.Capture(exception));
	                }
	            }
	        }
	        // REVIEW: Should we raise this in the base as well / instead?
	        OnReload();
	    }
	    public override void Load()
	    {
	        Load(reload: false);
	    }
	    public abstract void Load(Stream stream);
	    private void HandleException(ExceptionDispatchInfo info)
	    {
	        bool ignoreException = false;
	        if (Source.OnLoadException != null)
	        {
	            var exceptionContext = new ConfigurationFileLoadExceptionContext
	            {
	                Provider = this,
	                Exception = info.SourceException
	            };
	            Source.OnLoadException.Invoke(exceptionContext);
	            ignoreException = exceptionContext.Ignore;
	        }
	        if (!ignoreException)
	        {
	            info.Throw();
	        }
	    }
	    public void Dispose() => Dispose(true);
	    protected virtual void Dispose(bool disposing)
	    {
	        _changeTokenRegistration?.Dispose();
	    }
	}
	public abstract class ConfigurationFileSource : IConfigurationSource
	{
	    public IFileProvider FileProvider { get; set; }
	    public string Path { get; set; }
	    public bool Optional { get; set; }
	    public bool ReloadOnChange { get; set; }
	    public int ReloadDelay { get; set; } = 250;
	    public Action<ConfigurationFileLoadExceptionContext> OnLoadException { get; set; }
	    public abstract IConfigurationProvider Build(IConfigurationBuilder builder);
	    public void EnsureDefaults(IConfigurationBuilder builder)
	    {
	        FileProvider = FileProvider ?? builder.GetFileProvider();
	        OnLoadException = OnLoadException ?? builder.GetFileLoadExceptionHandler();
	    }
	    public void ResolveFileProvider()
	    {
	        if (FileProvider == null &&
	            !string.IsNullOrEmpty(Path) &&
	            System.IO.Path.IsPathRooted(Path))
	        {
	            string directory = System.IO.Path.GetDirectoryName(Path);
	            string pathToFile = System.IO.Path.GetFileName(Path);
	            while (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
	            {
	                pathToFile = System.IO.Path.Combine(System.IO.Path.GetFileName(directory), pathToFile);
	                directory = System.IO.Path.GetDirectoryName(directory);
	            }
	            if (Directory.Exists(directory))
	            {
	                FileProvider = new PhysicalFileProvider(directory);
	                Path = pathToFile;
	            }
	        }
	    }
	}
	#endregion
	#region \Extensions
	public static partial class ConfigurationBuilderExtensions
	{
	    #region File Provider
	    private static string FileProviderKey = "FileProvider";
	    private static string FileLoadExceptionHandlerKey = "FileLoadExceptionHandler";
	    public static IConfigurationBuilder SetFileProvider(this IConfigurationBuilder builder, IFileProvider fileProvider)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        builder.Properties[FileProviderKey] = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
	        return builder;
	    }
	    public static IFileProvider GetFileProvider(this IConfigurationBuilder builder)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (builder.Properties.TryGetValue(FileProviderKey, out object provider))
	        {
	            return provider as IFileProvider;
	        }
	        return new PhysicalFileProvider(AppContext.BaseDirectory ?? string.Empty);
	    }
	    public static IConfigurationBuilder SetBasePath(this IConfigurationBuilder builder, string basePath)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (basePath == null)
	        {
	            throw new ArgumentNullException(nameof(basePath));
	        }
	        return builder.SetFileProvider(new PhysicalFileProvider(basePath));
	    }
	    public static IConfigurationBuilder SetFileLoadExceptionHandler(this IConfigurationBuilder builder, Action<ConfigurationFileLoadExceptionContext> handler)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        builder.Properties[FileLoadExceptionHandlerKey] = handler;
	        return builder;
	    }
	    public static Action<ConfigurationFileLoadExceptionContext> GetFileLoadExceptionHandler(this IConfigurationBuilder builder)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (builder.Properties.TryGetValue(FileLoadExceptionHandlerKey, out object handler))
	        {
	            return handler as Action<ConfigurationFileLoadExceptionContext>;
	        }
	        return null;
	    }
	    #endregion
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
}
#endregion
