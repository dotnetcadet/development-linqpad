<Query Kind="Program">
<Namespace>System</Namespace>
<Namespace>System.Threading</Namespace>
<Namespace>System.Threading.Tasks</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>System.Collections</Namespace>
<Namespace>System.Diagnostics</Namespace>
<Namespace>System.Reflection</Namespace>
<Namespace>System.IO</Namespace>
</Query>
#load ".\assimalign.cohesion.core"

void Main()
{

}

#region Assimalign.Cohesion.Configuration(net8.0)
namespace Assimalign.Cohesion.Configuration
{
	#region \
	public abstract class ChangeToken : IChangeToken
	{
	    public bool HasChanged => throw new NotImplementedException();
	    public bool ActiveChangeCallbacks => throw new NotImplementedException();
	    public static IDisposable OnChange(Func<IChangeToken> changeTokenProducer, Action changeTokenConsumer)
	    {
	        if (changeTokenProducer == null)
	        {
	            throw new ArgumentNullException(nameof(changeTokenProducer));
	        }
	        if (changeTokenConsumer == null)
	        {
	            throw new ArgumentNullException(nameof(changeTokenConsumer));
	        }
	        return new ChangeTokenRegistration<Action>(changeTokenProducer, callback => callback(), changeTokenConsumer);
	    }
	    public static IDisposable OnChange<TState>(Func<IChangeToken> changeTokenProducer, Action<TState> changeTokenConsumer, TState state)
	    {
	        if (changeTokenProducer == null)
	        {
	            throw new ArgumentNullException(nameof(changeTokenProducer));
	        }
	        if (changeTokenConsumer == null)
	        {
	            throw new ArgumentNullException(nameof(changeTokenConsumer));
	        }
	        return new ChangeTokenRegistration<TState>(changeTokenProducer, changeTokenConsumer, state);
	    }
	    public IDisposable OnChange(Action<object> callback, object state)
	    {
	        throw new NotImplementedException();
	    }
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
	            IDisposable registraton = token.RegisterChangeCallback(s => ((ChangeTokenRegistration<TState>)s).OnChangeTokenFired(), this);
	            SetDisposable(registraton);
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
	public class ConfigurationBuilder : IConfigurationBuilder
	{
	    private readonly IList<IConfigurationSource> sources;
	    public ConfigurationBuilder()
	    {
	        this.sources = new List<IConfigurationSource>();
	    }
	    public IEnumerable<IConfigurationSource> Sources => this.sources;
	    public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();
	    public IConfigurationBuilder Add(IConfigurationSource source)
	    {
	        if (source == null)
	        {
	            throw new ArgumentNullException(nameof(source));
	        }
	        sources.Add(source);
	        return this;
	    }
	    public IConfigurationRoot Build()
	    {
	        var providers = new List<IConfigurationProvider>();
	        foreach (IConfigurationSource source in Sources)
	        {
	            IConfigurationProvider provider = source.Build(this);
	            providers.Add(provider);
	        }
	        return new ConfigurationRoot(providers);
	    }
	}
	public class ConfigurationKeyComparer : IComparer<string>
	{
	    private static readonly string[] _keyDelimiterArray = new[] { ConfigurationPath.KeyDelimiter };
	    public static ConfigurationKeyComparer Instance { get; } = new ConfigurationKeyComparer();
	    internal static Comparison<string> Comparison { get; } = Instance.Compare;
	    public int Compare(string? x, string? y)
	    {
	        string[] xParts = x?.Split(_keyDelimiterArray, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
	        string[] yParts = y?.Split(_keyDelimiterArray, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
	        // Compare each part until we get two parts that are not equal
	        for (int i = 0; i < Math.Min(xParts.Length, yParts.Length); i++)
	        {
	            x = xParts[i];
	            y = yParts[i];
	            int value1 = 0;
	            int value2 = 0;
	            bool xIsInt = x != null && int.TryParse(x, out value1);
	            bool yIsInt = y != null && int.TryParse(y, out value2);
	            int result;
	            if (!xIsInt && !yIsInt)
	            {
	                // Both are strings
	                result = string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
	            }
	            else if (xIsInt && yIsInt)
	            {
	                // Both are int
	                result = value1 - value2;
	            }
	            else
	            {
	                // Only one of them is int
	                result = xIsInt ? -1 : 1;
	            }
	            if (result != 0)
	            {
	                // One of them is different
	                return result;
	            }
	        }
	        // If we get here, the common parts are equal.
	        // If they are of the same length, then they are totally identical
	        return xParts.Length - yParts.Length;
	    }
	}
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class ConfigurationKeyNameAttribute : Attribute
	{
	    public ConfigurationKeyNameAttribute(string name) => Name = name;
	    public string Name { get; }
	}
	public sealed class ConfigurationManager : IConfigurationBuilder, IConfigurationRoot, IDisposable
	{
	    private readonly ConfigurationSources sources;
	    private readonly ConfigurationBuilderProperties properties;
	    private readonly object providerLock = new();
	    private readonly IList<IConfigurationProvider> providers;
	    private readonly IList<IDisposable> changeTokenRegistrations;
	    private ConfigurationReloadToken changeToken;
	    public ConfigurationManager()
	    {
	        this.sources = new ConfigurationSources(this);
	        this.properties = new ConfigurationBuilderProperties(this);
	        this.providers = new List<IConfigurationProvider>();
	        this.changeTokenRegistrations = new List<IDisposable>();
	        this.changeToken = new();
	        // Make sure there's some default storage since there are no default providers.
	        this.AddInMemoryCollection();
	        AddSource(sources[0]);
	    }
	    public string this[string key]
	    {
	        get
	        {
	            lock (providerLock)
	            {
	                return ConfigurationRoot.GetConfiguration(providers, key);
	            }
	        }
	        set
	        {
	            lock (providerLock)
	            {
	                ConfigurationRoot.SetConfiguration(providers, key, value);
	            }
	        }
	    }
	    public IConfigurationSection GetSection(string key) => new ConfigurationSection(this, key);
	    public IEnumerable<IConfigurationSection> GetChildren()
	    {
	        lock (providerLock)
	        {
	            // ToList() to eagerly evaluate inside lock.
	            return this.GetChildrenImplementation(null).ToList();
	        }
	    }
	    IDictionary<string, object> IConfigurationBuilder.Properties => properties;
	    IEnumerable<IConfigurationSource> IConfigurationBuilder.Sources => sources;
	    IEnumerable<IConfigurationProvider> IConfigurationRoot.Providers
	    {
	        get
	        {
	            lock (providerLock)
	            {
	                return new List<IConfigurationProvider>(providers);
	            }
	        }
	    }
	    public void Dispose()
	    {
	        lock (providerLock)
	        {
	            DisposeRegistrationsAndProvidersUnsynchronized();
	        }
	    }
	    IConfigurationBuilder IConfigurationBuilder.Add(IConfigurationSource source)
	    {
	        sources.Add(source ?? throw new ArgumentNullException(nameof(source)));
	        return this;
	    }
	    IConfigurationRoot IConfigurationBuilder.Build() => this;
	    IChangeToken IConfiguration.GetReloadToken() => changeToken;
	    void IConfigurationRoot.Reload()
	    {
	        lock (providerLock)
	        {
	            foreach (var provider in providers)
	            {
	                provider.Load();
	            }
	        }
	        RaiseChanged();
	    }
	    private void RaiseChanged()
	    {
	        var previousToken = Interlocked.Exchange(ref changeToken, new ConfigurationReloadToken());
	        previousToken.OnReload();
	    }
	    // Don't rebuild and reload all providers in the common case when a source is simply added to the IList.
	    private void AddSource(IConfigurationSource source)
	    {
	        lock (providerLock)
	        {
	            var provider = source.Build(this);
	            providers.Add(provider);
	            provider.Load();
	            changeTokenRegistrations.Add(ChangeToken.OnChange(() => provider.GetReloadToken(), () => RaiseChanged()));
	        }
	        RaiseChanged();
	    }
	    // Something other than Add was called on IConfigurationBuilder.Sources or IConfigurationBuilder.Properties has changed.
	    private void ReloadSources()
	    {
	        lock (providerLock)
	        {
	            DisposeRegistrationsAndProvidersUnsynchronized();
	            changeTokenRegistrations.Clear();
	            providers.Clear();
	            foreach (var source in sources)
	            {
	                providers.Add(source.Build(this));
	            }
	            foreach (var p in providers)
	            {
	                p.Load();
	                changeTokenRegistrations.Add(ChangeToken.OnChange(() => p.GetReloadToken(), () => RaiseChanged()));
	            }
	        }
	        RaiseChanged();
	    }
	    private void DisposeRegistrationsAndProvidersUnsynchronized()
	    {
	        // dispose change token registrations
	        foreach (var registration in changeTokenRegistrations)
	        {
	            registration.Dispose();
	        }
	        // dispose providers
	        foreach (var provider in providers)
	        {
	            (provider as IDisposable)?.Dispose();
	        }
	    }
	    private class ConfigurationSources : IList<IConfigurationSource>
	    {
	        private readonly List<IConfigurationSource> _sources = new();
	        private readonly ConfigurationManager _config;
	        public ConfigurationSources(ConfigurationManager config)
	        {
	            _config = config;
	        }
	        public IConfigurationSource this[int index]
	        {
	            get => _sources[index];
	            set
	            {
	                _sources[index] = value;
	                _config.ReloadSources();
	            }
	        }
	        public int Count => _sources.Count;
	        public bool IsReadOnly => false;
	        public void Add(IConfigurationSource source)
	        {
	            _sources.Add(source);
	            _config.AddSource(source);
	        }
	        public void Clear()
	        {
	            _sources.Clear();
	            _config.ReloadSources();
	        }
	        public bool Contains(IConfigurationSource source)
	        {
	            return _sources.Contains(source);
	        }
	        public void CopyTo(IConfigurationSource[] array, int arrayIndex)
	        {
	            _sources.CopyTo(array, arrayIndex);
	        }
	        public IEnumerator<IConfigurationSource> GetEnumerator()
	        {
	            return _sources.GetEnumerator();
	        }
	        public int IndexOf(IConfigurationSource source)
	        {
	            return _sources.IndexOf(source);
	        }
	        public void Insert(int index, IConfigurationSource source)
	        {
	            _sources.Insert(index, source);
	            _config.ReloadSources();
	        }
	        public bool Remove(IConfigurationSource source)
	        {
	            var removed = _sources.Remove(source);
	            _config.ReloadSources();
	            return removed;
	        }
	        public void RemoveAt(int index)
	        {
	            _sources.RemoveAt(index);
	            _config.ReloadSources();
	        }
	        IEnumerator IEnumerable.GetEnumerator()
	        {
	            return GetEnumerator();
	        }
	    }
	    private class ConfigurationBuilderProperties : IDictionary<string, object>
	    {
	        private readonly Dictionary<string, object> _properties = new();
	        private readonly ConfigurationManager _config;
	        public ConfigurationBuilderProperties(ConfigurationManager config)
	        {
	            _config = config;
	        }
	        public object this[string key]
	        {
	            get => _properties[key];
	            set
	            {
	                _properties[key] = value;
	                _config.ReloadSources();
	            }
	        }
	        public ICollection<string> Keys => _properties.Keys;
	        public ICollection<object> Values => _properties.Values;
	        public int Count => _properties.Count;
	        public bool IsReadOnly => false;
	        public void Add(string key, object value)
	        {
	            _properties.Add(key, value);
	            _config.ReloadSources();
	        }
	        public void Add(KeyValuePair<string, object> item)
	        {
	            ((IDictionary<string, object>)_properties).Add(item);
	            _config.ReloadSources();
	        }
	        public void Clear()
	        {
	            _properties.Clear();
	            _config.ReloadSources();
	        }
	        public bool Contains(KeyValuePair<string, object> item)
	        {
	            return _properties.Contains(item);
	        }
	        public bool ContainsKey(string key)
	        {
	            return _properties.ContainsKey(key);
	        }
	        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
	        {
	            ((IDictionary<string, object>)_properties).CopyTo(array, arrayIndex);
	        }
	        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
	        {
	            return _properties.GetEnumerator();
	        }
	        public bool Remove(string key)
	        {
	            var wasRemoved = _properties.Remove(key);
	            _config.ReloadSources();
	            return wasRemoved;
	        }
	        public bool Remove(KeyValuePair<string, object> item)
	        {
	            var wasRemoved = ((IDictionary<string, object>)_properties).Remove(item);
	            _config.ReloadSources();
	            return wasRemoved;
	        }
	        public bool TryGetValue(string key, out object value)
	        {
	            return _properties.TryGetValue(key, out value);
	        }
	        IEnumerator IEnumerable.GetEnumerator()
	        {
	            return _properties.GetEnumerator();
	        }
	    }
	}
	public static class ConfigurationPath
	{
	    public const string KeyDelimiter = ":";
	    public static string Combine(params string[] pathSegments)
	    {
	        if (pathSegments == null)
	        {
	            throw new ArgumentNullException(nameof(pathSegments));
	        }
	        return string.Join(KeyDelimiter, pathSegments);
	    }
	    public static string Combine(IEnumerable<string> pathSegments)
	    {
	        if (pathSegments == null)
	        {
	            throw new ArgumentNullException(nameof(pathSegments));
	        }
	        return string.Join(KeyDelimiter, pathSegments);
	    }
	    public static string GetSectionKey(string path)
	    {
	        if (string.IsNullOrEmpty(path))
	        {
	            return path;
	        }
	        int lastDelimiterIndex = path.LastIndexOf(KeyDelimiter, StringComparison.OrdinalIgnoreCase);
	        return lastDelimiterIndex == -1 ? path : path.Substring(lastDelimiterIndex + 1);
	    }
	    public static string GetParentPath(string path)
	    {
	        if (string.IsNullOrEmpty(path))
	        {
	            return null;
	        }
	        int lastDelimiterIndex = path.LastIndexOf(KeyDelimiter, StringComparison.OrdinalIgnoreCase);
	        return lastDelimiterIndex == -1 ? null : path.Substring(0, lastDelimiterIndex);
	    }
	}
	public abstract class ConfigurationProvider : IConfigurationProvider
	{
	    private ConfigurationReloadToken _reloadToken = new ConfigurationReloadToken();
	    protected ConfigurationProvider()
	    {
	        Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
	    }
	    public virtual string? Name { get; }
	    protected IDictionary<string, string> Data { get; set; }
	    public virtual bool TryGet(string key, out string value) => Data.TryGetValue(key, out value!);
	    public virtual void Set(string key, string value) => Data[key] = value;
	    public virtual string Get(string key) => Data[key];
	    public virtual void Load() { }
	    public virtual IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
	    {
	        var results = new List<string>();
	        if (parentPath is null)
	        {
	            foreach (KeyValuePair<string, string> kv in Data)
	            {
	                results.Add(Segment(kv.Key, 0));
	            }
	        }
	        else
	        {
	            Debug.Assert(ConfigurationPath.KeyDelimiter == ":");
	            foreach (KeyValuePair<string, string> kv in Data)
	            {
	                if (kv.Key.Length > parentPath.Length &&
	                    kv.Key.StartsWith(parentPath, StringComparison.OrdinalIgnoreCase) &&
	                    kv.Key[parentPath.Length] == ':')
	                {
	                    results.Add(Segment(kv.Key, parentPath.Length + 1));
	                }
	            }
	        }
	        results.AddRange(earlierKeys);
	        results.Sort(ConfigurationKeyComparer.Comparison);
	        return results;
	    }
	    private static string Segment(string key, int prefixLength)
	    {
	        int indexOf = key.IndexOf(ConfigurationPath.KeyDelimiter, prefixLength, StringComparison.OrdinalIgnoreCase);
	        return indexOf < 0 ? key.Substring(prefixLength) : key.Substring(prefixLength, indexOf - prefixLength);
	    }
	    public IChangeToken GetReloadToken()
	    {
	        return _reloadToken;
	    }
	    protected void OnReload()
	    {
	        ConfigurationReloadToken previousToken = Interlocked.Exchange(ref _reloadToken, new ConfigurationReloadToken());
	        previousToken.OnReload();
	    }
	    public override string ToString() => $"{GetType().Name}";
	}
	public class ConfigurationReloadToken : IChangeToken
	{
	    private CancellationTokenSource cts = new CancellationTokenSource();
	    public bool ActiveChangeCallbacks => true;
	    public bool HasChanged => cts.IsCancellationRequested;
	    public IDisposable RegisterChangeCallback(Action<object> callback, object state) => cts.Token.Register(callback, state);
	    public void OnReload() => cts.Cancel();
	}
	public class ConfigurationRoot : IConfigurationRoot, IDisposable
	{
	    private readonly IList<IConfigurationProvider> providers;
	    private readonly IList<IDisposable> changeTokenRegistrations;
	    private ConfigurationReloadToken changeToken = new ConfigurationReloadToken();
	    public ConfigurationRoot(IList<IConfigurationProvider> providers)
	    {
	        if (providers == null)
	        {
	            throw new ArgumentNullException(nameof(providers));
	        }
	        this.providers = providers;
	        this.changeTokenRegistrations = new List<IDisposable>(providers.Count);
	        foreach (IConfigurationProvider provider in providers)
	        {
	            provider.Load();
	            changeTokenRegistrations.Add(ChangeToken.OnChange(() => provider.GetReloadToken(), () => RaiseChanged()));
	        }
	    }
	    public IEnumerable<IConfigurationProvider> Providers => providers;
	    public string this[string key]
	    {
	        get => GetConfiguration(providers, key);
	        set => SetConfiguration(providers, key, value);
	    }
	    public IEnumerable<IConfigurationSection> GetChildren() => this.GetChildrenImplementation(null);
	    public IChangeToken GetReloadToken() => changeToken;
	    public IConfigurationSection GetSection(string key)
	        => new ConfigurationSection(this, key);
	    public void Reload()
	    {
	        foreach (IConfigurationProvider provider in providers)
	        {
	            provider.Load();
	        }
	        RaiseChanged();
	    }
	    private void RaiseChanged()
	    {
	        ConfigurationReloadToken previousToken = Interlocked.Exchange(ref changeToken, new ConfigurationReloadToken());
	        previousToken.OnReload();
	    }
	    public void Dispose()
	    {
	        // dispose change token registrations
	        foreach (IDisposable registration in changeTokenRegistrations)
	        {
	            registration.Dispose();
	        }
	        // dispose providers
	        foreach (IConfigurationProvider provider in providers)
	        {
	            (provider as IDisposable)?.Dispose();
	        }
	    }
	    internal static string GetConfiguration(IList<IConfigurationProvider> providers, string key)
	    {
	        for (int i = providers.Count - 1; i >= 0; i--)
	        {
	            IConfigurationProvider provider = providers[i];
	            if (provider.TryGet(key, out string value))
	            {
	                return value;
	            }
	        }
	        return null;
	    }
	    internal static void SetConfiguration(IList<IConfigurationProvider> providers, string key, string value)
	    {
	        if (providers.Count == 0)
	        {
	            throw new InvalidOperationException();// SR.Error_NoSources);
	        }
	        foreach (IConfigurationProvider provider in providers)
	        {
	            provider.Set(key, value);
	        }
	    }
	}
	public class ConfigurationSection : IConfigurationSection
	{
	    private readonly IConfigurationRoot root;
	    private readonly string path;
	    private string key;
	    public ConfigurationSection(IConfigurationRoot root, string path)
	    {
	        if (root == null)
	        {
	            throw new ArgumentNullException(nameof(root));
	        }
	        if (path == null)
	        {
	            throw new ArgumentNullException(nameof(path));
	        }
	        this.root = root;
	        this.path = path;
	    }
	    public string Path => path;
	    public string Key
	    {
	        get
	        {
	            if (key == null)
	            {
	                // Key is calculated lazily as last portion of Path
	                key = ConfigurationPath.GetSectionKey(path);
	            }
	            return key;
	        }
	    }
	    public string Value
	    {
	        get
	        {
	            return root[Path];
	        }
	        set
	        {
	            root[Path] = value;
	        }
	    }
	    public string this[string key]
	    {
	        get
	        {
	            return root[ConfigurationPath.Combine(Path, key)];
	        }
	        set
	        {
	            root[ConfigurationPath.Combine(Path, key)] = value;
	        }
	    }
	    public IConfigurationSection GetSection(string key) => root.GetSection(ConfigurationPath.Combine(Path, key));
	    public IEnumerable<IConfigurationSection> GetChildren() => root.GetChildrenImplementation(Path);
	    public IChangeToken GetReloadToken() => root.GetReloadToken();
	}
	#endregion
	#region \Abstractions
	public interface IConfiguration
	{
	    string this[string key] { get; set; }
	    IConfigurationSection GetSection(string key);
	    IEnumerable<IConfigurationSection> GetChildren();
	    IChangeToken GetReloadToken();
	}
	public interface IConfigurationBuilder
	{
	    IDictionary<string, object> Properties { get; }
	    //IEnumerable<IConfigurationSource> Sources { get; }
	    IConfigurationBuilder Add(IConfigurationSource source);
	    IConfigurationRoot Build();
	}
	public interface IConfigurationContext
	{
	    IEnumerable<IConfigurationProvider> Providers { get; }
	}
	public interface IConfigurationProvider
	{
	    bool TryGet(string key, out string value);
	    void Set(string key, string value);
	    string Get(string key);
	    void Load();
	    IChangeToken? GetReloadToken();
	    IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath);
	}
	public interface IConfigurationRoot : IConfiguration
	{
	    void Reload();
	    IEnumerable<IConfigurationProvider> Providers { get; }
	}
	public interface IConfigurationSection : IConfiguration
	{
	    string Key { get; }
	    string Path { get; }
	    string Value { get; set; }
	}
	public interface IConfigurationSource
	{
	    IConfigurationProvider Build(IConfigurationBuilder builder);
	}
	#endregion
	#region \Extensions
	public static partial class ConfigurationBuilderExtensions
	{
	    #region Chaining Provider
	    public static IConfigurationBuilder AddConfiguration(this IConfigurationBuilder configurationBuilder, IConfiguration config)
	        => AddConfiguration(configurationBuilder, config, shouldDisposeConfiguration: false);
	    public static IConfigurationBuilder AddConfiguration(this IConfigurationBuilder configurationBuilder, IConfiguration config, bool shouldDisposeConfiguration)
	    {
	        if (configurationBuilder == null)
	        {
	            throw new ArgumentNullException(nameof(configurationBuilder));
	        }
	        if (config == null)
	        {
	            throw new ArgumentNullException(nameof(config));
	        }
	        configurationBuilder.Add(new ChainedConfigurationSource()
	        {
	            Configuration = config,
	            ShouldDisposeConfiguration = shouldDisposeConfiguration,
	        });
	        return configurationBuilder;
	    }
	    #endregion
	    #region Memory Provider
	    public static IConfigurationBuilder AddInMemoryCollection(this IConfigurationBuilder configurationBuilder)
	    {
	        if (configurationBuilder == null)
	        {
	            throw new ArgumentNullException(nameof(configurationBuilder));
	        }
	        configurationBuilder.Add(new MemoryConfigurationSource());
	        return configurationBuilder;
	    }
	    public static IConfigurationBuilder AddInMemoryCollection(
	        this IConfigurationBuilder configurationBuilder,
	        IEnumerable<KeyValuePair<string, string>> initialData)
	    {
	        if (configurationBuilder == null)
	        {
	            throw new ArgumentNullException(nameof(configurationBuilder));
	        }
	        configurationBuilder.Add(new MemoryConfigurationSource { InitialData = initialData });
	        return configurationBuilder;
	    }
	    #endregion
	}
	public static class ConfigurationExtensions
	{
	    public static IConfigurationBuilder Add<TSource>(this IConfigurationBuilder builder, Action<TSource> configureSource) where TSource : IConfigurationSource, new()
	    {
	        var source = new TSource();
	        configureSource?.Invoke(source);
	        return builder.Add(source);
	    }
	    public static string GetConnectionString(this IConfiguration configuration, string name)
	    {
	        return configuration?.GetSection("ConnectionStrings")?[name];
	    }
	    public static IEnumerable<KeyValuePair<string, string>> AsEnumerable(this IConfiguration configuration) => configuration.AsEnumerable(makePathsRelative: false);
	    public static IEnumerable<KeyValuePair<string, string>> AsEnumerable(this IConfiguration configuration, bool makePathsRelative)
	    {
	        var stack = new Stack<IConfiguration>();
	        stack.Push(configuration);
	        var rootSection = configuration as IConfigurationSection;
	        int prefixLength = (makePathsRelative && rootSection != null) ? rootSection.Path.Length + 1 : 0;
	        while (stack.Count > 0)
	        {
	            IConfiguration config = stack.Pop();
	            // Don't include the sections value if we are removing paths, since it will be an empty key
	            if (config is IConfigurationSection section && (!makePathsRelative || config != configuration))
	            {
	                yield return new KeyValuePair<string, string>(section.Path.Substring(prefixLength), section.Value);
	            }
	            foreach (IConfigurationSection child in config.GetChildren())
	            {
	                stack.Push(child);
	            }
	        }
	    }
	    public static bool Exists(this IConfigurationSection section)
	    {
	        if (section == null)
	        {
	            return false;
	        }
	        return section.Value != null || section.GetChildren().Any();
	    }
	    public static IConfigurationSection GetRequiredSection(this IConfiguration configuration, string key)
	    {
	        if (configuration == null)
	        {
	            throw new ArgumentNullException(nameof(configuration));
	        }
	        IConfigurationSection section = configuration.GetSection(key);
	        if (section.Exists())
	        {
	            return section;
	        }
	        throw new InvalidOperationException($"The section does not exist in {key}");
	    }
	}
	public static class ConfigurationRootExtensions
	{
	    internal static IEnumerable<IConfigurationSection> GetChildrenImplementation(this IConfigurationRoot root, string path)
	    {
	        return root.Providers
	            .Aggregate(Enumerable.Empty<string>(),
	                (seed, source) => source.GetChildKeys(seed, path))
	            .Distinct(StringComparer.OrdinalIgnoreCase)
	            .Select(key => root.GetSection(path == null ? key : ConfigurationPath.Combine(path, key)));
	    }
	    public static string GetDebugView(this IConfigurationRoot root)
	    {
	        void RecurseChildren(
	            StringBuilder stringBuilder,
	            IEnumerable<IConfigurationSection> children,
	            string indent)
	        {
	            foreach (IConfigurationSection child in children)
	            {
	                (string Value, IConfigurationProvider Provider) valueAndProvider = GetValueAndProvider(root, child.Path);
	                if (valueAndProvider.Provider != null)
	                {
	                    stringBuilder
	                        .Append(indent)
	                        .Append(child.Key)
	                        .Append('=')
	                        .Append(valueAndProvider.Value)
	                        .Append(" (")
	                        .Append(valueAndProvider.Provider)
	                        .AppendLine(")");
	                }
	                else
	                {
	                    stringBuilder
	                        .Append(indent)
	                        .Append(child.Key)
	                        .AppendLine(":");
	                }
	                RecurseChildren(stringBuilder, child.GetChildren(), indent + "  ");
	            }
	        }
	        var builder = new StringBuilder();
	        RecurseChildren(builder, root.GetChildren(), "");
	        return builder.ToString();
	    }
	    private static (string Value, IConfigurationProvider Provider) GetValueAndProvider(
	        IConfigurationRoot root,
	        string key)
	    {
	        foreach (IConfigurationProvider provider in root.Providers.Reverse())
	        {
	            if (provider.TryGet(key, out string value))
	            {
	                return (value, provider);
	            }
	        }
	        return (null, null);
	    }
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
	#region \Providers
	public class ChainedConfigurationProvider : IConfigurationProvider, IDisposable
	{
	    private readonly IConfiguration configuration;
	    private readonly bool disposeConfiguration;
	    public ChainedConfigurationProvider(ChainedConfigurationSource source)
	    {
	        if (source == null)
	        {
	            throw new ArgumentNullException(nameof(source));
	        }
	        if (source.Configuration == null)
	        {
	            throw new ArgumentException();// SR.Format(SR.InvalidNullArgument, "source.Configuration"), nameof(source));
	        }
	        configuration = source.Configuration;
	        disposeConfiguration = source.ShouldDisposeConfiguration;
	    }
	    public bool TryGet(string key, out string value)
	    {
	        value = configuration[key];
	        return !string.IsNullOrEmpty(value);
	    }
	    public void Set(string key, string value) => configuration[key] = value;
	    public IChangeToken GetReloadToken() => configuration.GetReloadToken();
	    public void Load() { }
	    public IEnumerable<string> GetChildKeys(
	        IEnumerable<string> earlierKeys,
	        string parentPath)
	    {
	        IConfiguration section = parentPath == null ? configuration : configuration.GetSection(parentPath);
	        var keys = new List<string>();
	        foreach (IConfigurationSection child in section.GetChildren())
	        {
	            keys.Add(child.Key);
	        }
	        keys.AddRange(earlierKeys);
	        keys.Sort(ConfigurationKeyComparer.Comparison);
	        return keys;
	    }
	    public void Dispose()
	    {
	        if (disposeConfiguration)
	        {
	            (configuration as IDisposable)?.Dispose();
	        }
	    }
	    public string Get(string key)
	    {
	        throw new NotImplementedException();
	    }
	}
	public class ChainedConfigurationSource : IConfigurationSource
	{
	    public IConfiguration Configuration { get; set; }
	    public bool ShouldDisposeConfiguration { get; set; }
	    public IConfigurationProvider Build(IConfigurationBuilder builder)
	        => new ChainedConfigurationProvider(this);
	}
	public class MemoryConfigurationProvider : ConfigurationProvider, IEnumerable<KeyValuePair<string, string>>
	{
	    private readonly MemoryConfigurationSource _source;
	    public MemoryConfigurationProvider(MemoryConfigurationSource source)
	    {
	        if (source == null)
	        {
	            throw new ArgumentNullException(nameof(source));
	        }
	        _source = source;
	        if (_source.InitialData != null)
	        {
	            foreach (KeyValuePair<string, string> pair in _source.InitialData)
	            {
	                Data.Add(pair.Key, pair.Value);
	            }
	        }
	    }
	    public void Add(string key, string value)
	    {
	        Data.Add(key, value);
	    }
	    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
	    {
	        return Data.GetEnumerator();
	    }
	    IEnumerator IEnumerable.GetEnumerator()
	    {
	        return GetEnumerator();
	    }
	}
	public class MemoryConfigurationSource : IConfigurationSource
	{
	    public IEnumerable<KeyValuePair<string, string>> InitialData { get; set; }
	    public IConfigurationProvider Build(IConfigurationBuilder builder)
	    {
	        return new MemoryConfigurationProvider(this);
	    }
	}
	public abstract class StreamConfigurationProvider : ConfigurationProvider
	{
	    private bool isLoaded;
	    public StreamConfigurationProvider(StreamConfigurationSource source)
	    {
	        Source = source ?? throw new ArgumentNullException(nameof(source));
	    }
	    public StreamConfigurationSource Source { get; }
	    public abstract void Load(Stream stream);
	    public override void Load()
	    {
	        if (isLoaded)
	        {
	            throw new InvalidOperationException("The current Configuration Stream Provider instance is already loaded.");
	        }
	        Load(Source.Stream);
	        isLoaded = true;
	    }
	}
	public abstract class StreamConfigurationSource : IConfigurationSource
	{
	    public Stream Stream { get; set; }
	    public abstract IConfigurationProvider Build(IConfigurationBuilder builder);
	}
	#endregion
}
#endregion
