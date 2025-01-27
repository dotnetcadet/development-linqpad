<Query Kind="Program">
<Namespace>Assimalign.Cohesion.DependencyInjection.Properties</Namespace>
<Namespace>System</Namespace>
<Namespace>System.Collections</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>System.Diagnostics</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>System.Threading.Tasks</Namespace>
<Namespace>System.Collections.Concurrent</Namespace>
<Namespace>System.Diagnostics.CodeAnalysis</Namespace>
<Namespace>System.Runtime.CompilerServices</Namespace>
<Namespace>Assimalign.Cohesion.DependencyInjection.Internal</Namespace>
<Namespace>System.Diagnostics.Tracing</Namespace>
<Namespace>System.Reflection</Namespace>
<Namespace>System.Linq.Expressions</Namespace>
<Namespace>System.Threading</Namespace>
<Namespace>System.Reflection.Emit</Namespace>
<Namespace>System.Runtime.ExceptionServices</Namespace>
<Namespace>Assimalign.Cohesion.DependencyInjection.Utilities</Namespace>
</Query>

void Main()
{

}

#region Assimalign.Cohesion.DependencyInjection(net8.0)
namespace Assimalign.Cohesion.DependencyInjection
{
	#region \
	public sealed class ServiceCollection : IServiceCollection
	{
	    private bool isReadOnly;
	    private readonly List<ServiceDescriptor> descriptors = new List<ServiceDescriptor>();
	    public ServiceCollection()
	    {
	    }
	    public ServiceCollection(IEnumerable<ServiceDescriptor> descriptors)
	    {
	        this.descriptors.AddRange(descriptors);
	    }
	    public int Count => descriptors.Count;
	    public bool IsReadOnly => isReadOnly;
	    public ServiceDescriptor this[int index]
	    {
	        get
	        {
	            return descriptors[index];
	        }
	        set
	        {
	            CheckReadOnly();
	            descriptors[index] = value;
	        }
	    }
	    public void Clear()
	    {
	        CheckReadOnly();
	        descriptors.Clear();
	    }
	    public bool Contains(ServiceDescriptor item)
	    {
	        return descriptors.Contains(item);
	    }
	    public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
	    {
	        descriptors.CopyTo(array, arrayIndex);
	    }
	    public bool Remove(ServiceDescriptor item)
	    {
	        CheckReadOnly();
	        return descriptors.Remove(item);
	    }
	    public IEnumerator<ServiceDescriptor> GetEnumerator()
	    {
	        return descriptors.GetEnumerator();
	    }
	    void ICollection<ServiceDescriptor>.Add(ServiceDescriptor item)
	    {
	        CheckReadOnly();
	        descriptors.Add(item);
	    }
	    IEnumerator IEnumerable.GetEnumerator()
	    {
	        return GetEnumerator();
	    }
	    public int IndexOf(ServiceDescriptor item)
	    {
	        return descriptors.IndexOf(item);
	    }
	    public void Insert(int index, ServiceDescriptor item)
	    {
	        CheckReadOnly();
	        descriptors.Insert(index, item);
	    }
	    public void RemoveAt(int index)
	    {
	        CheckReadOnly();
	        descriptors.RemoveAt(index);
	    }
	    public void MakeReadOnly()
	    {
	        isReadOnly = true;
	    }
	    private void CheckReadOnly()
	    {
	        if (isReadOnly)
	        {
	            ThrowReadOnlyException();
	        }
	    }
	    private static void ThrowReadOnlyException() =>
	        throw new InvalidOperationException(Resources.ServiceCollectionReadOnly);
	}
	[DebuggerDisplay("Lifetime = {Lifetime}, ServiceType = {ServiceType}, ImplementationType = {ImplementationType}")]
	public sealed class ServiceDescriptor
	{
	    public ServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime lifetime)
	        : this(serviceType, lifetime)
	    {
	        if (serviceType == null)
	        {
	            throw new ArgumentNullException(nameof(serviceType));
	        }
	        if (implementationType == null)
	        {
	            throw new ArgumentNullException(nameof(implementationType));
	        }
	        //if (!serviceType.IsAssignableFrom(implementationType))
	        //{
	        //    throw new ArgumentException($"The type '{implementationType}' is not assignable to '{serviceType}'.");
	        //}
	        ImplementationType = implementationType;
	    }
	    public ServiceDescriptor(Type serviceType, object implementationInstance) 
	        : this(serviceType, ServiceLifetime.Singleton)
	    {
	        if (serviceType == null)
	        {
	            throw new ArgumentNullException(nameof(serviceType));
	        }
	        if (implementationInstance == null)
	        {
	            throw new ArgumentNullException(nameof(implementationInstance));
	        }
	        //if (!serviceType.IsAssignableFrom(implementationInstance.GetType()))
	        //{
	        //    throw new ArgumentException($"The type '{implementationInstance.GetType()}' is not assignable to '{serviceType}'.");
	        //}
	        ImplementationInstance = implementationInstance;
	    }
	    public ServiceDescriptor(Type serviceType, Func<IServiceProvider, object> factory, ServiceLifetime lifetime)
	        : this(serviceType, lifetime)
	    {
	        if (serviceType == null)
	        {
	            throw new ArgumentNullException(nameof(serviceType));
	        }
	        if (factory == null)
	        {
	            throw new ArgumentNullException(nameof(factory));
	        }
	        var returnType = factory.GetType().GenericTypeArguments[1];
	        //if (!serviceType.IsAssignableFrom(returnType))
	        //{
	        //    throw new ArgumentException($"The type '{returnType}' is not assignable to '{serviceType}'.");
	        //}
	        ImplementationFactory = factory;
	    }
	    private ServiceDescriptor(Type serviceType, ServiceLifetime lifetime)
	    {
	        Lifetime = lifetime;
	        ServiceType = serviceType;
	    }
	    public ServiceLifetime Lifetime { get; }
	    public Type ServiceType { get; }
	    public Type? ImplementationType { get; }
	    public object? ImplementationInstance { get; }
	    public Func<IServiceProvider, object>? ImplementationFactory { get; }
	    public override string ToString()
	    {
	        var lifetime = $"{nameof(ServiceType)}: {ServiceType} {nameof(Lifetime)}: {Lifetime} ";
	        if (ImplementationType != null)
	        {
	            return lifetime + $"{nameof(ImplementationType)}: {ImplementationType}";
	        }
	        if (ImplementationFactory != null)
	        {
	            return lifetime + $"{nameof(ImplementationFactory)}: {ImplementationFactory.Method}";
	        }
	        return lifetime + $"{nameof(ImplementationInstance)}: {ImplementationInstance}";
	    }
	    internal Type GetImplementationType()
	    {
	        if (ImplementationType != null)
	        {
	            return ImplementationType;
	        }
	        if (ImplementationInstance != null)
	        {
	            return ImplementationInstance.GetType();
	        }
	        if (ImplementationFactory != null)
	        {
	            var typeArguments = ImplementationFactory.GetType().GenericTypeArguments;
	            Debug.Assert(typeArguments.Length == 2);
	            return typeArguments[1];
	        }
	        Debug.Assert(false, "ImplementationType, ImplementationInstance or ImplementationFactory must be non null");
	        return null;
	    }
	    #region Static Methods
	    public static ServiceDescriptor Transient<TService, TImplementation>()
	        where TService : class
	        where TImplementation : class, TService
	    {
	        return Describe<TService, TImplementation>(ServiceLifetime.Transient);
	    }
	    public static ServiceDescriptor Transient(Type service,Type implementationType)
	    {
	        if (service == null)
	        {
	            throw new ArgumentNullException(nameof(service));
	        }
	        if (implementationType == null)
	        {
	            throw new ArgumentNullException(nameof(implementationType));
	        }
	        return Describe(service, implementationType, ServiceLifetime.Transient);
	    }
	    public static ServiceDescriptor Transient<TService, TImplementation>(
	        Func<IServiceProvider, TImplementation> implementationFactory)
	        where TService : class
	        where TImplementation : class, TService
	    {
	        if (implementationFactory == null)
	        {
	            throw new ArgumentNullException(nameof(implementationFactory));
	        }
	        return Describe(typeof(TService), implementationFactory, ServiceLifetime.Transient);
	    }
	    public static ServiceDescriptor Transient<TService>(Func<IServiceProvider, TService> implementationFactory)
	        where TService : class
	    {
	        if (implementationFactory == null)
	        {
	            throw new ArgumentNullException(nameof(implementationFactory));
	        }
	        return Describe(typeof(TService), implementationFactory, ServiceLifetime.Transient);
	    }
	    public static ServiceDescriptor Transient(Type service, Func<IServiceProvider, object> implementationFactory)
	    {
	        if (service == null)
	        {
	            throw new ArgumentNullException(nameof(service));
	        }
	        if (implementationFactory == null)
	        {
	            throw new ArgumentNullException(nameof(implementationFactory));
	        }
	        return Describe(service, implementationFactory, ServiceLifetime.Transient);
	    }
	    public static ServiceDescriptor Scoped<TService, TImplementation>()
	        where TService : class
	        where TImplementation : class, TService
	    {
	        return Describe<TService, TImplementation>(ServiceLifetime.Scoped);
	    }
	    public static ServiceDescriptor Scoped(Type service, Type implementationType)
	    {
	        return Describe(service, implementationType, ServiceLifetime.Scoped);
	    }
	    public static ServiceDescriptor Scoped<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)
	        where TService : class
	        where TImplementation : class, TService
	    {
	        if (implementationFactory == null)
	        {
	            throw new ArgumentNullException(nameof(implementationFactory));
	        }
	        return Describe(typeof(TService), implementationFactory, ServiceLifetime.Scoped);
	    }
	    public static ServiceDescriptor Scoped<TService>(Func<IServiceProvider, TService> implementationFactory)
	        where TService : class
	    {
	        if (implementationFactory == null)
	        {
	            throw new ArgumentNullException(nameof(implementationFactory));
	        }
	        return Describe(typeof(TService), implementationFactory, ServiceLifetime.Scoped);
	    }
	    public static ServiceDescriptor Scoped(Type service, Func<IServiceProvider, object> implementationFactory)
	    {
	        if (service == null)
	        {
	            throw new ArgumentNullException(nameof(service));
	        }
	        if (implementationFactory == null)
	        {
	            throw new ArgumentNullException(nameof(implementationFactory));
	        }
	        return Describe(service, implementationFactory, ServiceLifetime.Scoped);
	    }
	    public static ServiceDescriptor Singleton<TService, TImplementation>()
	        where TService : class
	        where TImplementation : class, TService
	    {
	        return Describe<TService, TImplementation>(ServiceLifetime.Singleton);
	    }
	    public static ServiceDescriptor Singleton(Type service, Type implementationType)
	    {
	        if (service == null)
	        {
	            throw new ArgumentNullException(nameof(service));
	        }
	        if (implementationType == null)
	        {
	            throw new ArgumentNullException(nameof(implementationType));
	        }
	        return Describe(service, implementationType, ServiceLifetime.Singleton);
	    }
	    public static ServiceDescriptor Singleton<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)
	        where TService : class
	        where TImplementation : class, TService
	    {
	        if (implementationFactory == null)
	        {
	            throw new ArgumentNullException(nameof(implementationFactory));
	        }
	        return Describe(typeof(TService), implementationFactory, ServiceLifetime.Singleton);
	    }
	    public static ServiceDescriptor Singleton<TService>(Func<IServiceProvider, TService> implementationFactory)
	        where TService : class
	    {
	        if (implementationFactory == null)
	        {
	            throw new ArgumentNullException(nameof(implementationFactory));
	        }
	        return Describe(typeof(TService), implementationFactory, ServiceLifetime.Singleton);
	    }
	    public static ServiceDescriptor Singleton(Type serviceType, Func<IServiceProvider, object> implementationFactory)
	    {
	        if (serviceType == null)
	        {
	            throw new ArgumentNullException(nameof(serviceType));
	        }
	        if (implementationFactory == null)
	        {
	            throw new ArgumentNullException(nameof(implementationFactory));
	        }
	        return Describe(serviceType, implementationFactory, ServiceLifetime.Singleton);
	    }
	    public static ServiceDescriptor Singleton<TService>(TService implementationInstance)
	        where TService : class
	    {
	        if (implementationInstance == null)
	        {
	            throw new ArgumentNullException(nameof(implementationInstance));
	        }
	        return Singleton(typeof(TService), implementationInstance);
	    }
	    public static ServiceDescriptor Singleton(Type serviceType, object implementationInstance)
	    {
	        if (serviceType == null)
	        {
	            throw new ArgumentNullException(nameof(serviceType));
	        }
	        if (implementationInstance == null)
	        {
	            throw new ArgumentNullException(nameof(implementationInstance));
	        }
	        return new ServiceDescriptor(serviceType, implementationInstance);
	    }    
	    public static ServiceDescriptor Describe(Type serviceType, Type implementationType, ServiceLifetime lifetime)
	    {
	        return new ServiceDescriptor(serviceType, implementationType, lifetime);
	    }
	    public static ServiceDescriptor Describe(Type serviceType, Func<IServiceProvider, object> implementationFactory, ServiceLifetime lifetime)
	    {
	        return new ServiceDescriptor(serviceType, implementationFactory, lifetime);
	    }
	    private static ServiceDescriptor Describe<TService, TImplementation>(ServiceLifetime lifetime)
	        where TService : class
	        where TImplementation : class, TService
	    {
	        return Describe(
	            typeof(TService),
	            typeof(TImplementation),
	            lifetime: lifetime);
	    }
	    #endregion
	}
	public delegate T ServiceFactory<T>();
	public enum ServiceLifetime
	{
	    Singleton,
	    Scoped,
	    Transient
	}
	public sealed class ServiceProvider : IServiceProvider, IDisposable, IAsyncDisposable
	{
	    private readonly CallSiteValidatorVisitor? callSiteValidator;
	    private readonly Func<Type, Func<ServiceProviderEngineScope, object?>> createServiceAccessor;
	    // Internal for testing
	    internal ServiceProviderEngine engine;
	    internal bool IsDisposed;
	    private ConcurrentDictionary<Type, Func<ServiceProviderEngineScope, object?>> realizedServices;
	    internal CallSiteFactory CallSiteFactory { get; }
	    internal ServiceProviderEngineScope Root { get; }
	    internal static bool VerifyOpenGenericServiceTrimmability { get; } = AppContext.TryGetSwitch(
	        "Assimalign.Cohesion.DependencyInjection.VerifyOpenGenericServiceTrimmability",
	        out bool verifyOpenGenerics) ? verifyOpenGenerics : false;
	    internal ServiceProvider(ICollection<ServiceDescriptor> serviceDescriptors, ServiceProviderOptions options)
	    {
	        // note that Root needs to be set before calling GetEngine(), because the engine may need to access Root
	        Root = new ServiceProviderEngineScope(this, isRootScope: true);
	        engine = GetEngine();
	        createServiceAccessor = CreateServiceAccessor;
	        realizedServices = new ConcurrentDictionary<Type, Func<ServiceProviderEngineScope, object?>>();
	        CallSiteFactory = new CallSiteFactory(serviceDescriptors);
	        CallSiteFactory.Add(typeof(IServiceProvider), new ServiceProviderCallSite()); // The list of built in services that aren't part of the list of service descriptors. keep this in sync with CallSiteFactory.IsService
	        CallSiteFactory.Add(typeof(IServiceScopeFactory), new ConstantCallSite(typeof(IServiceScopeFactory), Root));
	        CallSiteFactory.Add(typeof(IServiceLookup), new ConstantCallSite(typeof(IServiceLookup), CallSiteFactory));
	        if (options.ValidateScopes)
	        {
	            callSiteValidator = new CallSiteValidatorVisitor();
	        }
	        if (options.ValidateOnBuild)
	        {
	            List<Exception>? exceptions = null;
	            foreach (ServiceDescriptor serviceDescriptor in serviceDescriptors)
	            {
	                try
	                {
	                    ValidateService(serviceDescriptor);
	                }
	                catch (Exception exception)
	                {
	                    exceptions ??= new List<Exception>();
	                    exceptions.Add(exception);
	                }
	            }
	            if (exceptions is not null)
	            {
	                throw new AggregateException("Some services are not able to be constructed", exceptions.ToArray());
	            }
	        }
	        ServiceEventSource.Log.ServiceProviderBuilt(this);
	    }
	    public object GetService(Type serviceType) => GetService(serviceType, Root);
	    public void Dispose()
	    {
	        DisposeCore();
	        Root.Dispose();
	    }
	    public ValueTask DisposeAsync()
	    {
	        DisposeCore();
	        return Root.DisposeAsync();
	    }
	    private void DisposeCore()
	    {
	        IsDisposed = true;
	        ServiceEventSource.Log.ServiceProviderDisposed(this);
	    }
	    private void OnCreate(CallSiteService callSite)
	    {
	        callSiteValidator?.ValidateCallSite(callSite);
	    }
	    private void OnResolve(Type serviceType, IServiceScope scope)
	    {
	        callSiteValidator?.ValidateResolution(serviceType, scope, Root);
	    }
	    internal object GetService(Type serviceType, ServiceProviderEngineScope serviceProviderEngineScope)
	    {
	        if (IsDisposed)
	        {
	            ThrowHelper.ThrowObjectDisposedException();
	        }
	        var realizedService = realizedServices.GetOrAdd(serviceType, this.createServiceAccessor);
	        OnResolve(serviceType, serviceProviderEngineScope);
	        ServiceEventSource.Log.ServiceResolved(this, serviceType);
	        var result = realizedService.Invoke(serviceProviderEngineScope);
	        System.Diagnostics.Debug.Assert(result is null || CallSiteFactory.IsService(serviceType));
	        return result;
	    }
	    private void ValidateService(ServiceDescriptor descriptor)
	    {
	        if (descriptor.ServiceType.IsGenericType && !descriptor.ServiceType.IsConstructedGenericType)
	        {
	            return;
	        }
	        try
	        {
	            var callSite = CallSiteFactory.GetCallSite(descriptor, new CallSiteChain());
	            if (callSite != null)
	            {
	                OnCreate(callSite);
	            }
	        }
	        catch (Exception exception)
	        {
	            throw new InvalidOperationException($"Error while validating the service descriptor '{descriptor}': {exception.Message}", exception);
	        }
	    }
	    private Func<ServiceProviderEngineScope, object> CreateServiceAccessor(Type serviceType)
	    {
	        var callSite = CallSiteFactory.GetCallSite(serviceType, new CallSiteChain());
	        if (callSite != null)
	        {
	            ServiceEventSource.Log.CallSiteBuilt(this, serviceType, callSite);
	            OnCreate(callSite);
	            // Optimize singleton case
	            if (callSite.Cache.Location == CallSiteResultCacheLocation.Root)
	            {
	                object value = CallSiteRuntimeResolverVisitor.Instance.Resolve(callSite, Root);
	                return scope => value;
	            }
	            return engine.RealizeService(callSite);
	        }
	        return _ => null;
	    }
	    internal void ReplaceServiceAccessor(CallSiteService callSite, Func<ServiceProviderEngineScope, object> accessor)
	    {
	        realizedServices[callSite.ServiceType] = accessor;
	    }
	    internal IServiceScope CreateScope()
	    {
	        if (IsDisposed)
	        {
	            ThrowHelper.ThrowObjectDisposedException();
	        }
	        return new ServiceProviderEngineScope(this, isRootScope: false);
	    }
	    private ServiceProviderEngine GetEngine()
	    {
	        ServiceProviderEngine engine;
	        engine = RuntimeFeature.IsDynamicCodeCompiled ?
	             CreateDynamicEngine() :           
	            RuntimeServiceProviderEngine.Instance;  // Don't try to compile Expressions/IL if they are going to get interpreted
	        return engine;
	        [UnconditionalSuppressMessage("AotAnalysis", "IL3050:RequiresDynamicCode",
	                Justification = "CreateDynamicEngine won't be called when using NativeAOT.")] // see also https://github.com/dotnet/linker/issues/2715
	        ServiceProviderEngine CreateDynamicEngine() => new DynamicServiceProviderEngine(this);
	    }
	}
	public sealed class ServiceProviderBuilder : IServiceProviderBuilder, IDisposable
	{
	    private static readonly ConcurrentDictionary<int, IServiceCollection> services = new();
	    private static readonly ConcurrentDictionary<int, IServiceProvider> providers = new();
	    private readonly ServiceProviderOptions options;
	    public ServiceProviderBuilder() => this.options = ServiceProviderOptions.Default;
	    public ServiceProviderBuilder(ServiceProviderOptions options) => this.options = options == null ? throw new ArgumentNullException(nameof(options)) : options;
	    public IServiceCollection Services => services.GetOrAdd(this.GetHashCode(), new ServiceCollection());    
	    public IServiceProviderBuilder Add(ServiceDescriptor serviceDescriptor)
	    {
	        if (serviceDescriptor == null)
	        {
	            throw new ArgumentNullException(nameof(serviceDescriptor));
	        }
	        Services.Add(serviceDescriptor);
	        return this;
	    }
	    IServiceProvider IServiceProviderBuilder.Build()
	    {
	        return new ServiceProvider(
	            Services, 
	            options);
	    }
	    public void Dispose()
	    {
	        throw new NotImplementedException();
	    }
	}
	public sealed class ServiceProviderFactory
	{
	    private static string defaultKey = Guid.NewGuid().ToString("N");
	    private static Factory factory = new();
	    private static ConcurrentDictionary<string, Func<IServiceProvider>> providers = new(StringComparer.CurrentCultureIgnoreCase);
	    public ServiceProviderFactory Register(Action<IServiceProviderBuilder> configure)
	    {
	        IServiceProviderBuilder builder = new ServiceProviderBuilder();
	        configure.Invoke(builder);
	        var descriptor = ServiceDescriptor.Singleton<IServiceProviderFactory>(serviceProvider =>
	        {
	            return factory;
	        });
	        builder.Add(descriptor);
	        providers[defaultKey] = () => builder.Build();
	        return this;
	    }
	    public ServiceProviderFactory Register(string serviceProviderName, IServiceCollection services)
	    {
	        var descriptor = ServiceDescriptor.Singleton<IServiceProviderFactory>(factory);
	        services.Add(descriptor);
	        providers.TryAdd(serviceProviderName, () =>
	        {
	            return new ServiceProvider(services, ServiceProviderOptions.Default);
	        });
	        return this;
	    }
	    public ServiceProviderFactory Register(string serviceProviderName, Action<IServiceProviderBuilder> configure)
	    {
	        IServiceProviderBuilder builder = new ServiceProviderBuilder();
	        configure.Invoke(builder);
	        var descriptor = ServiceDescriptor.Singleton<IServiceProviderFactory>(serviceProvider =>
	        {
	            return factory;
	        });
	        builder.Add(descriptor);
	        providers[serviceProviderName] = () => builder.Build();
	        return this;
	    }
	    public ServiceProviderFactory Register(string serviceProviderName, ServiceProviderOptions options, Action<IServiceProviderBuilder> configure)
	    {
	        IServiceProviderBuilder builder = new ServiceProviderBuilder(options);
	        configure.Invoke(builder);
	        var descriptor = ServiceDescriptor.Singleton<IServiceProviderFactory>(serviceProvider =>
	        {
	            return factory;
	        });
	        builder.Add(descriptor);
	        providers[serviceProviderName] = () => builder.Build();
	        return this;
	    }
	    public IServiceProviderFactory Build() => factory;
	    private partial class Factory : IServiceProviderFactory
	    {
	        IServiceProvider IServiceProviderFactory.Create()
	        {
	            if (!providers.Any())
	            {
	                throw new InvalidOperationException("No IServiceProvider's have been registered.");
	            }
	            if (providers.TryGetValue(defaultKey, out var provider))
	            {
	                return provider.Invoke();
	            }
	            throw new Exception("Provider does not exist");
	        }
	        IServiceProvider IServiceProviderFactory.Create(string serviceProviderName)
	        {
	            if (!providers.Any())
	            {
	                throw new InvalidOperationException("No IServiceProvider's have been registered.");
	            }
	            if (string.IsNullOrEmpty(serviceProviderName))
	            {
	                throw new ArgumentNullException(nameof(serviceProviderName));
	            }
	            if (providers.TryGetValue(serviceProviderName, out var provider))
	            {
	                return provider.Invoke();
	            }
	            throw new Exception("Provider does not exist");
	        }
	    }
	}
	public class ServiceProviderOptions
	{
	    // Avoid allocating objects in the default case
	    internal static readonly ServiceProviderOptions Default = new();
	    public bool ValidateScopes { get; set; }
	    public bool ValidateOnBuild { get; set; }
	}
	#endregion
	#region \Abstractions
	public interface IServiceCollection : IList<ServiceDescriptor>
	{
	}
	public interface IServiceFactory<out TService>
	{
	    TService Create(object serviceKey);
	}
	public interface IServiceLookup
	{
	    bool IsService(Type serviceType);
	}
	public interface IServiceProviderBuilder
	{
	    IServiceCollection Services { get; }
	    IServiceProviderBuilder Add(ServiceDescriptor serviceDescriptor);
	    IServiceProvider Build();
	}
	public interface IServiceProviderFactory
	{
	    IServiceProvider Create();
	    IServiceProvider Create(string serviceProviderName);
	}
	public interface IServiceScope : IDisposable
	{
	    IServiceProvider ServiceProvider { get; }
	}
	public interface IServiceScopeFactory
	{
	    IServiceScope CreateScope();
	}
	public interface ISupportRequiredService
	{
	    object GetRequiredService(Type serviceType);
	}
	#endregion
	#region \Extensions
	public static partial class ServiceProviderBuilderExtensions
	{
	    public static IServiceProviderBuilder AddTransient(this IServiceProviderBuilder builder, Type serviceType, Type implementationType)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (serviceType == null)
	        {
	            throw new ArgumentNullException(nameof(serviceType));
	        }
	        if (implementationType == null)
	        {
	            throw new ArgumentNullException(nameof(implementationType));
	        }
	        return Add(builder, serviceType, implementationType, ServiceLifetime.Transient);
	    }
	    public static IServiceProviderBuilder AddTransient(this IServiceProviderBuilder builder, Type serviceType, Func<IServiceProvider, object> implementationFactory)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (serviceType == null)
	        {
	            throw new ArgumentNullException(nameof(serviceType));
	        }
	        if (implementationFactory == null)
	        {
	            throw new ArgumentNullException(nameof(implementationFactory));
	        }
	        return Add(builder, serviceType, implementationFactory, ServiceLifetime.Transient);
	    }
	    public static IServiceProviderBuilder AddTransient<TService, TImplementation>(this IServiceProviderBuilder builder)
	        where TService : class
	        where TImplementation : class, TService
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        return builder.AddTransient(typeof(TService), typeof(TImplementation));
	    }
	    public static IServiceProviderBuilder AddTransient(this IServiceProviderBuilder builder, Type serviceType)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (serviceType == null)
	        {
	            throw new ArgumentNullException(nameof(serviceType));
	        }
	        return builder.AddTransient(serviceType, serviceType);
	    }
	    public static IServiceProviderBuilder AddTransient<TService>(this IServiceProviderBuilder builder)
	        where TService : class
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        return builder.AddTransient(typeof(TService));
	    }
	    public static IServiceProviderBuilder AddTransient<TService>(this IServiceProviderBuilder builder, Func<IServiceProvider, TService> implementationFactory)
	        where TService : class
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (implementationFactory == null)
	        {
	            throw new ArgumentNullException(nameof(implementationFactory));
	        }
	        return builder.AddTransient(typeof(TService), implementationFactory);
	    }
	    public static IServiceProviderBuilder AddTransient<TService, TImplementation>(this IServiceProviderBuilder builder, Func<IServiceProvider, TImplementation> implementationFactory)
	        where TService : class
	        where TImplementation : class, TService
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (implementationFactory == null)
	        {
	            throw new ArgumentNullException(nameof(implementationFactory));
	        }
	        return builder.AddTransient(typeof(TService), implementationFactory);
	    }
	    public static IServiceProviderBuilder AddScoped(this IServiceProviderBuilder builder, Type serviceType, Type implementationType)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (serviceType == null)
	        {
	            throw new ArgumentNullException(nameof(serviceType));
	        }
	        if (implementationType == null)
	        {
	            throw new ArgumentNullException(nameof(implementationType));
	        }
	        return Add(builder, serviceType, implementationType, ServiceLifetime.Scoped);
	    }
	    public static IServiceProviderBuilder AddScoped(this IServiceProviderBuilder builder, Type serviceType, Func<IServiceProvider, object> implementationFactory)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (serviceType == null)
	        {
	            throw new ArgumentNullException(nameof(serviceType));
	        }
	        if (implementationFactory == null)
	        {
	            throw new ArgumentNullException(nameof(implementationFactory));
	        }
	        return Add(builder, serviceType, implementationFactory, ServiceLifetime.Scoped);
	    }
	    public static IServiceProviderBuilder AddScoped<TService, TImplementation>(this IServiceProviderBuilder builder)
	        where TService : class
	        where TImplementation : class, TService
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        return builder.AddScoped(typeof(TService), typeof(TImplementation));
	    }
	    public static IServiceProviderBuilder AddScoped(this IServiceProviderBuilder builder, Type serviceType)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (serviceType == null)
	        {
	            throw new ArgumentNullException(nameof(serviceType));
	        }
	        return builder.AddScoped(serviceType, serviceType);
	    }
	    public static IServiceProviderBuilder AddScoped<TService>(this IServiceProviderBuilder builder)
	        where TService : class
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        return builder.AddScoped(typeof(TService));
	    }
	    public static IServiceProviderBuilder AddScoped<TService>(this IServiceProviderBuilder builder, Func<IServiceProvider, TService> implementationFactory)
	        where TService : class
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (implementationFactory == null)
	        {
	            throw new ArgumentNullException(nameof(implementationFactory));
	        }
	        return builder.AddScoped(typeof(TService), implementationFactory);
	    }
	    public static IServiceProviderBuilder AddScoped<TService, TImplementation>(this IServiceProviderBuilder builder, Func<IServiceProvider, TImplementation> implementationFactory)
	        where TService : class
	        where TImplementation : class, TService
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (implementationFactory == null)
	        {
	            throw new ArgumentNullException(nameof(implementationFactory));
	        }
	        return builder.AddScoped(typeof(TService), implementationFactory);
	    }
	    public static IServiceProviderBuilder AddSingleton(this IServiceProviderBuilder builder, Type serviceType, Type implementationType)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (serviceType == null)
	        {
	            throw new ArgumentNullException(nameof(serviceType));
	        }
	        if (implementationType == null)
	        {
	            throw new ArgumentNullException(nameof(implementationType));
	        }
	        return Add(builder, serviceType, implementationType, ServiceLifetime.Singleton);
	    }
	    public static IServiceProviderBuilder AddSingleton(this IServiceProviderBuilder builder, Type serviceType, Func<IServiceProvider, object> implementationFactory)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (serviceType == null)
	        {
	            throw new ArgumentNullException(nameof(serviceType));
	        }
	        if (implementationFactory == null)
	        {
	            throw new ArgumentNullException(nameof(implementationFactory));
	        }
	        return Add(builder, serviceType, implementationFactory, ServiceLifetime.Singleton);
	    }
	    public static IServiceProviderBuilder AddSingleton<TService, TImplementation>(this IServiceProviderBuilder builder)
	        where TService : class
	        where TImplementation : class, TService
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        return builder.AddSingleton(typeof(TService), typeof(TImplementation));
	    }
	    public static IServiceProviderBuilder AddSingleton(this IServiceProviderBuilder builder, Type serviceType)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (serviceType == null)
	        {
	            throw new ArgumentNullException(nameof(serviceType));
	        }
	        return builder.AddSingleton(serviceType, serviceType);
	    }
	    public static IServiceProviderBuilder AddSingleton<TService>(this IServiceProviderBuilder builder)
	        where TService : class
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        return builder.AddSingleton(typeof(TService));
	    }
	    public static IServiceProviderBuilder AddSingleton<TService>(this IServiceProviderBuilder builder, Func<IServiceProvider, TService> implementationFactory)
	        where TService : class
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (implementationFactory == null)
	        {
	            throw new ArgumentNullException(nameof(implementationFactory));
	        }
	        return builder.AddSingleton(typeof(TService), implementationFactory);
	    }
	    public static IServiceProviderBuilder AddSingleton<TService, TImplementation>(this IServiceProviderBuilder builder, Func<IServiceProvider, TImplementation> implementationFactory)
	        where TService : class
	        where TImplementation : class, TService
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (implementationFactory == null)
	        {
	            throw new ArgumentNullException(nameof(implementationFactory));
	        }
	        return builder.AddSingleton(typeof(TService), implementationFactory);
	    }
	    public static IServiceProviderBuilder AddSingleton(this IServiceProviderBuilder builder, Type serviceType, object implementationInstance)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (serviceType == null)
	        {
	            throw new ArgumentNullException(nameof(serviceType));
	        }
	        if (implementationInstance == null)
	        {
	            throw new ArgumentNullException(nameof(implementationInstance));
	        }
	        var serviceDescriptor = new ServiceDescriptor(serviceType, implementationInstance);
	        builder.Add(serviceDescriptor);
	        return builder;
	    }
	    public static IServiceProviderBuilder AddSingleton<TService>(this IServiceProviderBuilder builder, TService implementationInstance)
	        where TService : class
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (implementationInstance == null)
	        {
	            throw new ArgumentNullException(nameof(implementationInstance));
	        }
	        return builder.AddSingleton(typeof(TService), implementationInstance);
	    }
	    private static IServiceProviderBuilder Add(IServiceProviderBuilder builder, Type serviceType, Type implementationType, ServiceLifetime lifetime)
	    {
	        var descriptor = new ServiceDescriptor(
	            serviceType,
	            implementationType,
	            lifetime);
	        builder.Add(descriptor);
	        return builder;
	    }
	    private static IServiceProviderBuilder Add(IServiceProviderBuilder builder, Type serviceType, Func<IServiceProvider, object> implementationFactory, ServiceLifetime lifetime)
	    {
	        var descriptor = new ServiceDescriptor(
	            serviceType,
	            implementationFactory,
	            lifetime);
	        builder.Add(descriptor);
	        return builder;
	    }
	}
	public static partial class ServiceProviderBuilderExtensions
	{
	    public static bool TryAddTransient(this IServiceProviderBuilder builder, Type service)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (service == null)
	        {
	            throw new ArgumentNullException(nameof(service));
	        }
	        var descriptor = ServiceDescriptor.Transient(service, service);
	        return TryAdd(builder, descriptor);
	    }
	    public static bool TryAddTransient(this IServiceProviderBuilder builder, Type service, Type implementationType)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (service == null)
	        {
	            throw new ArgumentNullException(nameof(service));
	        }
	        if (implementationType == null)
	        {
	            throw new ArgumentNullException("implementationType");
	        }
	        var descriptor = ServiceDescriptor.Transient(service, implementationType);
	        return TryAdd(builder, descriptor);
	    }
	    public static bool TryAddTransient(this IServiceProviderBuilder builder, Type service, Func<IServiceProvider, object> implementationFactory)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (service == null)
	        {
	            throw new ArgumentNullException(nameof(service));
	        }
	        if (implementationFactory == null)
	        {
	            throw new ArgumentNullException(nameof(implementationFactory));
	        }
	        var descriptor = ServiceDescriptor.Transient(service, implementationFactory);
	        return TryAdd(builder, descriptor);
	    }
	    public static bool TryAddTransient<TService>(this IServiceProviderBuilder builder) where TService : class
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        return TryAddTransient(builder, typeof(TService), typeof(TService));
	    }
	    public static bool TryAddTransient<TService, TImplementation>(this IServiceProviderBuilder builder) where TService : class where TImplementation : class, TService
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        return TryAddTransient(builder, typeof(TService), typeof(TImplementation));
	    }
	    public static bool TryAddTransient<TService>(this IServiceProviderBuilder builder, Func<IServiceProvider, TService> implementationFactory) where TService : class
	    {
	        return TryAdd(builder, ServiceDescriptor.Transient(implementationFactory));
	    }
	    public static bool TryAddScoped(this IServiceProviderBuilder builder, Type service)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (service == null)
	        {
	            throw new ArgumentNullException(nameof(service));
	        }
	        var descriptor = ServiceDescriptor.Scoped(service, service);
	        return TryAdd(builder, descriptor);
	    }
	    public static bool TryAddScoped(this IServiceProviderBuilder builder, Type service, Type implementationType)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (service == null)
	        {
	            throw new ArgumentNullException(nameof(service));
	        }
	        if (implementationType == null)
	        {
	            throw new ArgumentNullException(nameof(implementationType));
	        }
	        var descriptor = ServiceDescriptor.Scoped(service, implementationType);
	        return TryAdd(builder, descriptor);
	    }
	    public static bool TryAddScoped(this IServiceProviderBuilder builder, Type service, Func<IServiceProvider, object> implementationFactory)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (service == null)
	        {
	            throw new ArgumentNullException(nameof(service));
	        }
	        if (implementationFactory == null)
	        {
	            throw new ArgumentNullException(nameof(implementationFactory));
	        }
	        var descriptor = ServiceDescriptor.Scoped(service, implementationFactory);
	        return TryAdd(builder, descriptor);
	    }
	    public static bool TryAddScoped<TService>(this IServiceProviderBuilder builder) where TService : class
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        return TryAddScoped(builder, typeof(TService), typeof(TService));
	    }
	    public static bool TryAddScoped<TService, TImplementation>(this IServiceProviderBuilder builder) where TService : class where TImplementation : class, TService
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        return TryAddScoped(builder, typeof(TService), typeof(TImplementation));
	    }
	    public static bool TryAddScoped<TService>(this IServiceProviderBuilder builder, Func<IServiceProvider, TService> implementationFactory) where TService : class
	    {
	        return TryAdd(builder, ServiceDescriptor.Scoped(implementationFactory));
	    }
	    public static bool TryAddSingleton(this IServiceProviderBuilder builder, Type service)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (service == null)
	        {
	            throw new ArgumentNullException(nameof(service));
	        }
	        var descriptor = ServiceDescriptor.Singleton(service, service);
	        return TryAdd(builder, descriptor);
	    }
	    public static bool TryAddSingleton(this IServiceProviderBuilder builder, Type service, Type implementationType)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (service == null)
	        {
	            throw new ArgumentNullException(nameof(service));
	        }
	        if (implementationType == null)
	        {
	            throw new ArgumentNullException(nameof(implementationType));
	        }
	        var descriptor = ServiceDescriptor.Singleton(service, implementationType);
	        return TryAdd(builder, descriptor);
	    }
	    public static bool TryAddSingleton(this IServiceProviderBuilder builder, Type service, Func<IServiceProvider, object> implementationFactory)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (service == null)
	        {
	            throw new ArgumentNullException(nameof(service));
	        }
	        if (implementationFactory == null)
	        {
	            throw new ArgumentNullException(nameof(implementationFactory));
	        }
	        var descriptor = ServiceDescriptor.Singleton(service, implementationFactory);
	        return TryAdd(builder, descriptor);
	    }
	    public static bool TryAddSingleton<TService>(this IServiceProviderBuilder builder) where TService : class
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        return TryAddSingleton(builder, typeof(TService), typeof(TService));
	    }
	    public static bool TryAddSingleton<TService, TImplementation>(this IServiceProviderBuilder builder) where TService : class where TImplementation : class, TService
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        return TryAddSingleton(builder, typeof(TService), typeof(TImplementation));
	    }
	    public static bool TryAddSingleton<TService>(this IServiceProviderBuilder builder, TService instance) where TService : class
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (instance == null)
	        {
	            throw new ArgumentNullException(nameof(instance));
	        }
	        var descriptor = ServiceDescriptor.Singleton(typeof(TService), instance);
	        return TryAdd(builder, descriptor);
	    }
	    public static bool TryAddSingleton<TService>(this IServiceProviderBuilder services, Func<IServiceProvider, TService> implementationFactory) where TService : class
	    {
	        return TryAdd(services, ServiceDescriptor.Singleton(implementationFactory));
	    }
	    public static bool TryAddEnumerable(this IServiceProviderBuilder builder, ServiceDescriptor descriptor)
	    {
	        var descriptor2 = descriptor;
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (descriptor2 == null)
	        {
	            throw new ArgumentNullException(nameof(descriptor));
	        }
	        var implementationType = descriptor2.GetImplementationType();
	        if (implementationType == typeof(object) || implementationType == descriptor2.ServiceType)
	        {
	            throw new ArgumentException(Resources.GetTryAddIndistinguishableTypeToEnumerableExceptionMessage(implementationType, descriptor2.ServiceType), "descriptor");
	        }
	        if (!builder.Services.Any((ServiceDescriptor d) => d.ServiceType == descriptor2.ServiceType && d.GetImplementationType() == implementationType))
	        {
	            builder.Services.Add(descriptor2);
	            return true;
	        }
	        return false;
	    }
	    public static void TryAddEnumerable(this IServiceProviderBuilder services, IEnumerable<ServiceDescriptor> descriptors)
	    {
	        if (services == null)
	        {
	            throw new ArgumentNullException("builder");
	        }
	        if (descriptors == null)
	        {
	            throw new ArgumentNullException("descriptors");
	        }
	        foreach (ServiceDescriptor descriptor in descriptors)
	        {
	            TryAddEnumerable(services, descriptor);
	        }
	    }
	    public static bool TryAdd(this IServiceProviderBuilder builder, ServiceDescriptor descriptor)
	    {
	        if (builder is null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (descriptor is null)
	        {
	            throw new ArgumentNullException(nameof(descriptor));
	        }
	        if (!builder.Services.Any((ServiceDescriptor serviceDescriptor) => serviceDescriptor.ServiceType == descriptor.ServiceType))
	        {
	            builder.Add(descriptor);
	            return true;
	        }
	        return false;
	    }
	    public static IServiceProviderBuilder Replace(this IServiceProviderBuilder builder, ServiceDescriptor descriptor)
	    {
	        var descriptor2 = descriptor;
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (descriptor2 == null)
	        {
	            throw new ArgumentNullException(nameof(descriptor));
	        }
	        var serviceDescriptor = builder.Services.FirstOrDefault((ServiceDescriptor s) => s.ServiceType == descriptor2.ServiceType);
	        if (serviceDescriptor != null)
	        {
	            builder.Services.Remove(serviceDescriptor);
	        }
	        builder.Add(descriptor2);
	        return builder;
	    }
	    public static IServiceProviderBuilder RemoveAll<T>(this IServiceProviderBuilder builder)
	    {
	        return RemoveAll(builder, typeof(T));
	    }
	    public static IServiceProviderBuilder RemoveAll(this IServiceProviderBuilder builder, Type serviceType)
	    {
	        if (serviceType == null)
	        {
	            throw new ArgumentNullException(nameof(serviceType));
	        }
	        for (int num = builder.Services.Count - 1; num >= 0; num--)
	        {
	            var serviceDescriptor = builder.Services[num];
	            if (serviceDescriptor.ServiceType == serviceType)
	            {
	                builder.Services.RemoveAt(num);
	            }
	        }
	        return builder;
	    }
	}
	public static class ServiceProviderExtensions
	{
	    public static T? GetService<T>(this IServiceProvider provider)
	    {
	        if (provider == null)
	        {
	            throw new ArgumentNullException(nameof(provider));
	        }
	        return (T?)provider.GetService(typeof(T));
	    }
	    public static object GetRequiredService(this IServiceProvider provider, Type serviceType)
	    {
	        if (provider == null)
	        {
	            throw new ArgumentNullException(nameof(provider));
	        }
	        if (serviceType == null)
	        {
	            throw new ArgumentNullException(nameof(serviceType));
	        }
	        if (provider is ISupportRequiredService requiredServiceSupportingProvider)
	        {
	            return requiredServiceSupportingProvider.GetRequiredService(serviceType);
	        }
	        var service = provider.GetService(serviceType);
	        if (service == null)
	        {
	            throw new InvalidOperationException(Resources.GetNoServiceRegisteredExceptionMessage(serviceType));
	        }
	        return service;
	    }
	    public static T GetRequiredService<T>(this IServiceProvider provider) where T : notnull
	    {
	        if (provider == null)
	        {
	            throw new ArgumentNullException(nameof(provider));
	        }
	        return (T)provider.GetRequiredService(typeof(T));
	    }
	    public static IEnumerable<T> GetServices<T>(this IServiceProvider provider)
	    {
	        if (provider == null)
	        {
	            throw new ArgumentNullException(nameof(provider));
	        }
	        return provider.GetRequiredService<IEnumerable<T>>();
	    }
	    public static IEnumerable<object?> GetServices(this IServiceProvider provider, Type serviceType)
	    {
	        if (provider == null)
	        {
	            throw new ArgumentNullException(nameof(provider));
	        }
	        if (serviceType == null)
	        {
	            throw new ArgumentNullException(nameof(serviceType));
	        }
	        Type? genericEnumerable = typeof(IEnumerable<>).MakeGenericType(serviceType);
	        return (IEnumerable<object>)provider.GetRequiredService(genericEnumerable);
	    }
	    public static IServiceScope CreateScope(this IServiceProvider provider)
	    {
	        return provider.GetRequiredService<IServiceScopeFactory>().CreateScope();
	    }
	    public static AsyncServiceScope CreateAsyncScope(this IServiceProvider provider)
	    {
	        return new AsyncServiceScope(provider.CreateScope());
	    }
	    public static AsyncServiceScope CreateAsyncScope(this IServiceScopeFactory serviceScopeFactory)
	    {
	        return new AsyncServiceScope(serviceScopeFactory.CreateScope());
	    }
	}
	#endregion
	#region \Internal
	[EventSource(Name = "Assimalign-Extensions-DependencyInjection")]
	internal sealed class ServiceEventSource : EventSource
	{
	    public static readonly ServiceEventSource Log = new ServiceEventSource();
	    public static class Keywords
	    {
	        public const EventKeywords ServiceProviderInitialized = (EventKeywords)0x1;
	    }
	    // Event source doesn't support large payloads so we chunk large payloads like formatted call site tree and descriptors
	    private const int MaxChunkSize = 10 * 1024;
	    private readonly List<WeakReference<ServiceProvider>> providers = new();
	    private ServiceEventSource() : base(EventSourceSettings.EtwSelfDescribingEventFormat)
	    {
	    }
	    // NOTE
	    // - The 'Start' and 'Stop' suffixes on the following event names have special meaning in EventSource. They
	    //   enable creating 'activities'.
	    //   For more information, take a look at the following blog post:
	    //   https://blogs.msdn.microsoft.com/vancem/2015/09/14/exploring-eventsource-activity-correlation-and-causation-features/
	    // - A stop event's event id must be next one after its start event.
	    // - Avoid renaming methods or parameters marked with EventAttribute. EventSource uses these to form the event object.
	    [UnconditionalSuppressMessage(
	        category: "ReflectionAnalysis", 
	        checkId: "IL2026:RequiresUnreferencedCode",
	        Justification = "Parameters to this method are primitive and are trimmer safe.")]
	    [Event(1, Level = EventLevel.Verbose)]
	    private void CallSiteBuilt(string serviceType, string callSite, int chunkIndex, int chunkCount, int serviceProviderHashCode)
	    {
	        WriteEvent(1, serviceType, callSite, chunkIndex, chunkCount, serviceProviderHashCode);
	    }
	    [Event(2, Level = EventLevel.Verbose)]
	    public void ServiceResolved(string serviceType, int serviceProviderHashCode)
	    {
	        WriteEvent(2, serviceType, serviceProviderHashCode);
	    }
	    [Event(3, Level = EventLevel.Verbose)]
	    public void ExpressionTreeGenerated(string serviceType, int nodeCount, int serviceProviderHashCode)
	    {
	        WriteEvent(3, serviceType, nodeCount, serviceProviderHashCode);
	    }
	    [Event(4, Level = EventLevel.Verbose)]
	    public void DynamicMethodBuilt(string serviceType, int methodSize, int serviceProviderHashCode)
	    {
	        WriteEvent(4, serviceType, methodSize, serviceProviderHashCode);
	    }
	    [Event(5, Level = EventLevel.Verbose)]
	    public void ScopeDisposed(int serviceProviderHashCode, int scopedServicesResolved, int disposableServices)
	    {
	        WriteEvent(5, serviceProviderHashCode, scopedServicesResolved, disposableServices);
	    }
	    [Event(6, Level = EventLevel.Error)]
	    public void ServiceRealizationFailed(string? exceptionMessage, int serviceProviderHashCode)
	    {
	        WriteEvent(6, exceptionMessage, serviceProviderHashCode);
	    }
	    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode",
	        Justification = "Parameters to this method are primitive and are trimmer safe.")]
	    [Event(7, Level = EventLevel.Informational, Keywords = Keywords.ServiceProviderInitialized)]
	    private void ServiceProviderBuilt(int serviceProviderHashCode, int singletonServices, int scopedServices, int transientServices, int closedGenericsServices, int openGenericsServices)
	    {
	        WriteEvent(7, serviceProviderHashCode, singletonServices, scopedServices, transientServices, closedGenericsServices, openGenericsServices);
	    }
	    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode",
	        Justification = "Parameters to this method are primitive and are trimmer safe.")]
	    [Event(8, Level = EventLevel.Informational, Keywords = Keywords.ServiceProviderInitialized)]
	    private void ServiceProviderDescriptors(int serviceProviderHashCode, string descriptors, int chunkIndex, int chunkCount)
	    {
	        WriteEvent(8, serviceProviderHashCode, descriptors, chunkIndex, chunkCount);
	    }
	    [NonEvent]
	    public void ServiceResolved(ServiceProvider provider, Type serviceType)
	    {
	        if (IsEnabled(EventLevel.Verbose, EventKeywords.All))
	        {
	            ServiceResolved(serviceType.ToString(), provider.GetHashCode());
	        }
	    }
	    [NonEvent]
	    public void CallSiteBuilt(ServiceProvider provider, Type serviceType, CallSiteService callSite)
	    {
	        if (IsEnabled(EventLevel.Verbose, EventKeywords.All))
	        {
	            var format = CallSiteJsonFormatterVisitor.Instance.Format(callSite);
	            var chunkCount = format.Length / MaxChunkSize + (format.Length % MaxChunkSize > 0 ? 1 : 0);
	            var providerHashCode = provider.GetHashCode();
	            for (int i = 0; i < chunkCount; i++)
	            {
	                CallSiteBuilt(
	                    serviceType.ToString(),
	                    format.Substring(i * MaxChunkSize, Math.Min(MaxChunkSize, format.Length - i * MaxChunkSize)), i, chunkCount,
	                    providerHashCode);
	            }
	        }
	    }
	    [NonEvent]
	    public void DynamicMethodBuilt(ServiceProvider provider, Type serviceType, int methodSize)
	    {
	        if (IsEnabled(EventLevel.Verbose, EventKeywords.All))
	        {
	            DynamicMethodBuilt(serviceType.ToString(), methodSize, provider.GetHashCode());
	        }
	    }
	    [NonEvent]
	    public void ServiceRealizationFailed(Exception exception, int serviceProviderHashCode)
	    {
	        if (IsEnabled(EventLevel.Error, EventKeywords.All))
	        {
	            ServiceRealizationFailed(exception.ToString(), serviceProviderHashCode);
	        }
	    }
	    [NonEvent]
	    public void ServiceProviderBuilt(ServiceProvider provider)
	    {
	        lock (providers)
	        {
	            providers.Add(new WeakReference<ServiceProvider>(provider));
	        }
	        WriteServiceProviderBuilt(provider);
	    }
	    [NonEvent]
	    public void ServiceProviderDisposed(ServiceProvider provider)
	    {
	        lock (providers)
	        {
	            for (int i = providers.Count - 1; i >= 0; i--)
	            {
	                // remove the provider, along with any stale references
	                WeakReference<ServiceProvider> reference = providers[i];
	                if (!reference.TryGetTarget(out ServiceProvider target) || target == provider)
	                {
	                    providers.RemoveAt(i);
	                }
	            }
	        }
	    }
	    [NonEvent]
	    private void WriteServiceProviderBuilt(ServiceProvider provider)
	    {
	        if (IsEnabled(EventLevel.Informational, Keywords.ServiceProviderInitialized))
	        {
	            int singletonServices = 0;
	            int scopedServices = 0;
	            int transientServices = 0;
	            int closedGenericsServices = 0;
	            int openGenericsServices = 0;
	            StringBuilder descriptorBuilder = new StringBuilder("{ \"descriptors\":[ ");
	            bool firstDescriptor = true;
	            foreach (ServiceDescriptor descriptor in provider.CallSiteFactory.Descriptors)
	            {
	                if (firstDescriptor)
	                {
	                    firstDescriptor = false;
	                }
	                else
	                {
	                    descriptorBuilder.Append(", ");
	                }
	                AppendServiceDescriptor(descriptorBuilder, descriptor);
	                switch (descriptor.Lifetime)
	                {
	                    case ServiceLifetime.Singleton:
	                        singletonServices++;
	                        break;
	                    case ServiceLifetime.Scoped:
	                        scopedServices++;
	                        break;
	                    case ServiceLifetime.Transient:
	                        transientServices++;
	                        break;
	                }
	                if (descriptor.ServiceType.IsGenericType)
	                {
	                    if (descriptor.ServiceType.IsConstructedGenericType)
	                    {
	                        closedGenericsServices++;
	                    }
	                    else
	                    {
	                        openGenericsServices++;
	                    }
	                }
	            }
	            descriptorBuilder.Append(" ] }");
	            int providerHashCode = provider.GetHashCode();
	            ServiceProviderBuilt(providerHashCode, singletonServices, scopedServices, transientServices, closedGenericsServices, openGenericsServices);
	            string descriptorString = descriptorBuilder.ToString();
	            int chunkCount = descriptorString.Length / MaxChunkSize + (descriptorString.Length % MaxChunkSize > 0 ? 1 : 0);
	            for (int i = 0; i < chunkCount; i++)
	            {
	                ServiceProviderDescriptors(
	                    providerHashCode,
	                    descriptorString.Substring(i * MaxChunkSize, Math.Min(MaxChunkSize, descriptorString.Length - i * MaxChunkSize)), i, chunkCount);
	            }
	        }
	    }
	    [NonEvent]
	    private static void AppendServiceDescriptor(StringBuilder builder, ServiceDescriptor descriptor)
	    {
	        builder.Append("{ \"serviceType\": \"");
	        builder.Append(descriptor.ServiceType);
	        builder.Append("\", \"lifetime\": \"");
	        builder.Append(descriptor.Lifetime);
	        builder.Append("\", ");
	        if (descriptor.ImplementationType is not null)
	        {
	            builder.Append("\"implementationType\": \"");
	            builder.Append(descriptor.ImplementationType);
	        }
	        else if (descriptor.ImplementationFactory is not null)
	        {
	            builder.Append("\"implementationFactory\": \"");
	            builder.Append(descriptor.ImplementationFactory.Method);
	        }
	        else if (descriptor.ImplementationInstance is not null)
	        {
	            builder.Append("\"implementationInstance\": \"");
	            builder.Append(descriptor.ImplementationInstance.GetType());
	            builder.Append(" (instance)");
	        }
	        else
	        {
	            builder.Append("\"unknown\": \"");
	        }
	        builder.Append("\" }");
	    }
	    protected override void OnEventCommand(EventCommandEventArgs command)
	    {
	        if (command.Command == EventCommand.Enable)
	        {
	            // When this EventSource becomes enabled, write out the existing ServiceProvider information
	            // because building the ServiceProvider happens early in the process. This way a listener
	            // can get this information, even if they attach while the process is running.
	            lock (providers)
	            {
	                foreach (WeakReference<ServiceProvider> reference in providers)
	                {
	                    if (reference.TryGetTarget(out ServiceProvider provider))
	                    {
	                        WriteServiceProviderBuilt(provider);
	                    }
	                }
	            }
	        }
	    }
	}
	#endregion
	#region \Internal\Extensions
	internal static class ReflectionExtensions
	{
	    internal static bool CheckHasDefaultValue(this ParameterInfo parameter, out bool tryToGetDefaultValue)
	    {
	        tryToGetDefaultValue = true;
	        return parameter.HasDefaultValue;
	    }
	    internal static bool TryGetDefaultValue(this ParameterInfo parameter, out object? defaultValue)
	    {
	        bool hasDefaultValue = parameter.CheckHasDefaultValue( out bool tryToGetDefaultValue);
	        defaultValue = null;
	        if (hasDefaultValue)
	        {
	            if (tryToGetDefaultValue)
	            {
	                defaultValue = parameter.DefaultValue;
	            }
	            bool isNullableParameterType = parameter.ParameterType.IsGenericType &&
	                parameter.ParameterType.GetGenericTypeDefinition() == typeof(Nullable<>);
	            // Workaround for https://github.com/dotnet/runtime/issues/18599
	            if (defaultValue == null && parameter.ParameterType.IsValueType && !isNullableParameterType) // Nullable types should be left null
	            {
	                defaultValue = CreateValueType(parameter.ParameterType);
	            }
	            [UnconditionalSuppressMessage(
	                "ReflectionAnalysis",
	                "IL2067:UnrecognizedReflectionPattern",
	                Justification = "CreateValueType is only called on a ValueType. You can always create an instance of a ValueType.")]
	            static object? CreateValueType(Type t) => RuntimeHelpers.GetUninitializedObject(t);
	            // Handle nullable enums
	            if (defaultValue != null && isNullableParameterType)
	            {
	                Type? underlyingType = Nullable.GetUnderlyingType(parameter.ParameterType);
	                if (underlyingType != null && underlyingType.IsEnum)
	                {
	                    defaultValue = Enum.ToObject(underlyingType, defaultValue);
	                }
	            }
	        }
	        return hasDefaultValue;
	    }
	}
	internal static class ServiceEventSourceExtensions
	{
	    // This is an extension method because this assembly is trimmed at a "type granular" level in Blazor,
	    // and the whole DependencyInjectionEventSource type can't be trimmed. So extracting this to a separate
	    // type allows for the System.Linq.Expressions usage to be trimmed by the ILLinker.
	    public static void ExpressionTreeGenerated(this ServiceEventSource source, ServiceProvider provider, Type serviceType, Expression expression)
	    {
	        if (source.IsEnabled(EventLevel.Verbose, EventKeywords.All))
	        {
	            var visitor = new NodeCountingVisitor();
	            visitor.Visit(expression);
	            source.ExpressionTreeGenerated(serviceType.ToString(), visitor.NodeCount, provider.GetHashCode());
	        }
	    }
	    private sealed class NodeCountingVisitor : ExpressionVisitor
	    {
	        public int NodeCount { get; private set; }
	        public override Expression Visit(Expression e)
	        {
	            base.Visit(e);
	            NodeCount++;
	            return e;
	        }
	    }
	}
	#endregion
	#region \Internal\Helpers
	internal static class ServiceLookupHelpers
	{
	    private const BindingFlags LookupFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
	    private static readonly MethodInfo? ArrayEmptyMethodInfo = typeof(Array).GetMethod(nameof(Array.Empty));
	    internal static readonly MethodInfo? InvokeFactoryMethodInfo = typeof(Func<IServiceProvider, object>)
	        .GetMethod(nameof(Func<IServiceProvider, object>.Invoke), LookupFlags);
	    internal static readonly MethodInfo? CaptureDisposableMethodInfo = typeof(ServiceProviderEngineScope)
	        .GetMethod(nameof(ServiceProviderEngineScope.CaptureDisposable), LookupFlags);
	    internal static readonly MethodInfo? TryGetValueMethodInfo = typeof(IDictionary<CallSiteServiceCacheKey, object>)
	        .GetMethod(nameof(IDictionary<CallSiteServiceCacheKey, object>.TryGetValue), LookupFlags);
	    internal static readonly MethodInfo? ResolveCallSiteAndScopeMethodInfo = typeof(CallSiteRuntimeResolverVisitor)
	        .GetMethod(nameof(CallSiteRuntimeResolverVisitor.Resolve), LookupFlags);
	    internal static readonly MethodInfo? AddMethodInfo = typeof(IDictionary<CallSiteServiceCacheKey, object>)
	        .GetMethod(nameof(IDictionary<CallSiteServiceCacheKey, object>.Add), LookupFlags);
	    internal static readonly MethodInfo? MonitorEnterMethodInfo = typeof(Monitor)
	        .GetMethod(nameof(Monitor.Enter), BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(object), typeof(bool).MakeByRefType() }, null);
	    internal static readonly MethodInfo? MonitorExitMethodInfo = typeof(Monitor)
	        .GetMethod(nameof(Monitor.Exit), BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(object) }, null);
	    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2060:MakeGenericMethod",
	        Justification = "Calling Array.Empty<T>() is safe since the T doesn't have trimming annotations.")]
	    internal static MethodInfo GetArrayEmptyMethodInfo(Type itemType) =>
	        ArrayEmptyMethodInfo.MakeGenericMethod(itemType);
	}
	internal static class ThrowHelper
	{
	    [DoesNotReturn]
	    [MethodImpl(MethodImplOptions.NoInlining)]
	    internal static void ThrowObjectDisposedException()
	    {
	        throw new ObjectDisposedException(nameof(IServiceProvider));
	    }
	    [DoesNotReturn]
	    internal static void ThrowArgumentExceptionIfNull(string paramName)
	    {
	        throw new ArgumentNullException(paramName);
	    } 
	}
	internal static class TypeNameHelper
	{
	    private const char DefaultNestedTypeDelimiter = '+';
	    private static readonly Dictionary<Type, string> _builtInTypeNames = new Dictionary<Type, string>
	    {
	        { typeof(void), "void" },
	        { typeof(bool), "bool" },
	        { typeof(byte), "byte" },
	        { typeof(char), "char" },
	        { typeof(decimal), "decimal" },
	        { typeof(double), "double" },
	        { typeof(float), "float" },
	        { typeof(int), "int" },
	        { typeof(long), "long" },
	        { typeof(object), "object" },
	        { typeof(sbyte), "sbyte" },
	        { typeof(short), "short" },
	        { typeof(string), "string" },
	        { typeof(uint), "uint" },
	        { typeof(ulong), "ulong" },
	        { typeof(ushort), "ushort" }
	    };
	    [return: NotNullIfNotNull("item")]
	    public static string? GetTypeDisplayName(object? item, bool fullName = true)
	    {
	        return item == null ? null : GetTypeDisplayName(item.GetType(), fullName);
	    }
	    public static string GetTypeDisplayName(Type type, bool fullName = true, bool includeGenericParameterNames = false, bool includeGenericParameters = true, char nestedTypeDelimiter = DefaultNestedTypeDelimiter)
	    {
	        var builder = new StringBuilder();
	        ProcessType(builder, type, new DisplayNameOptions(fullName, includeGenericParameterNames, includeGenericParameters, nestedTypeDelimiter));
	        return builder.ToString();
	    }
	    private static void ProcessType(StringBuilder builder, Type type, in DisplayNameOptions options)
	    {
	        if (type.IsGenericType)
	        {
	            Type[] genericArguments = type.GetGenericArguments();
	            ProcessGenericType(builder, type, genericArguments, genericArguments.Length, options);
	        }
	        else if (type.IsArray)
	        {
	            ProcessArrayType(builder, type, options);
	        }
	        else if (_builtInTypeNames.TryGetValue(type, out string? builtInName))
	        {
	            builder.Append(builtInName);
	        }
	        else if (type.IsGenericParameter)
	        {
	            if (options.IncludeGenericParameterNames)
	            {
	                builder.Append(type.Name);
	            }
	        }
	        else
	        {
	            string name = options.FullName ? type.FullName! : type.Name;
	            builder.Append(name);
	            if (options.NestedTypeDelimiter != DefaultNestedTypeDelimiter)
	            {
	                builder.Replace(DefaultNestedTypeDelimiter, options.NestedTypeDelimiter, builder.Length - name.Length, name.Length);
	            }
	        }
	    }
	    private static void ProcessArrayType(StringBuilder builder, Type type, in DisplayNameOptions options)
	    {
	        Type innerType = type;
	        while (innerType.IsArray)
	        {
	            innerType = innerType.GetElementType()!;
	        }
	        ProcessType(builder, innerType, options);
	        while (type.IsArray)
	        {
	            builder.Append('[');
	            builder.Append(',', type.GetArrayRank() - 1);
	            builder.Append(']');
	            type = type.GetElementType()!;
	        }
	    }
	    private static void ProcessGenericType(StringBuilder builder, Type type, Type[] genericArguments, int length, in DisplayNameOptions options)
	    {
	        int offset = 0;
	        if (type.IsNested)
	        {
	            offset = type.DeclaringType!.GetGenericArguments().Length;
	        }
	        if (options.FullName)
	        {
	            if (type.IsNested)
	            {
	                ProcessGenericType(builder, type.DeclaringType!, genericArguments, offset, options);
	                builder.Append(options.NestedTypeDelimiter);
	            }
	            else if (!string.IsNullOrEmpty(type.Namespace))
	            {
	                builder.Append(type.Namespace);
	                builder.Append('.');
	            }
	        }
	        int genericPartIndex = type.Name.IndexOf('`');
	        if (genericPartIndex <= 0)
	        {
	            builder.Append(type.Name);
	            return;
	        }
	        builder.Append(type.Name, 0, genericPartIndex);
	        if (options.IncludeGenericParameters)
	        {
	            builder.Append('<');
	            for (int i = offset; i < length; i++)
	            {
	                ProcessType(builder, genericArguments[i], options);
	                if (i + 1 == length)
	                {
	                    continue;
	                }
	                builder.Append(',');
	                if (options.IncludeGenericParameterNames || !genericArguments[i + 1].IsGenericParameter)
	                {
	                    builder.Append(' ');
	                }
	            }
	            builder.Append('>');
	        }
	    }
	    private readonly struct DisplayNameOptions
	    {
	        public DisplayNameOptions(bool fullName, bool includeGenericParameterNames, bool includeGenericParameters, char nestedTypeDelimiter)
	        {
	            FullName = fullName;
	            IncludeGenericParameters = includeGenericParameters;
	            IncludeGenericParameterNames = includeGenericParameterNames;
	            NestedTypeDelimiter = nestedTypeDelimiter;
	        }
	        public bool FullName { get; }
	        public bool IncludeGenericParameters { get; }
	        public bool IncludeGenericParameterNames { get; }
	        public char NestedTypeDelimiter { get; }
	    }
	}
	#endregion
	#region \Internal\ServiceLookup
	internal sealed class CallSiteChain
	{
	    private readonly Dictionary<Type, ChainItemInfo> callSiteChain;
	    public CallSiteChain()
	    {
	        callSiteChain = new Dictionary<Type, ChainItemInfo>();
	    }
	    public void CheckCircularDependency(Type serviceType)
	    {
	        if (callSiteChain.ContainsKey(serviceType))
	        {
	            throw new InvalidOperationException(CreateCircularDependencyExceptionMessage(serviceType));
	        }
	    }
	    public void Remove(Type serviceType) => callSiteChain.Remove(serviceType);
	    public void Add(Type serviceType, Type implementationType = null) => callSiteChain[serviceType] = new ChainItemInfo(callSiteChain.Count, implementationType);
	    private string CreateCircularDependencyExceptionMessage(Type type)
	    {
	        var messageBuilder = new StringBuilder()
	            .Append(Resources.GetCircularDependencyExceptionMessage(TypeNameHelper.GetTypeDisplayName(type)))
	            .AppendLine();
	        AppendResolutionPath(messageBuilder, type);
	        return messageBuilder.ToString();
	    }
	    private void AppendResolutionPath(StringBuilder builder, Type currentlyResolving)
	    {
	        var ordered = new List<KeyValuePair<Type, ChainItemInfo>>(callSiteChain);
	        ordered.Sort((a, b) => a.Value.Order.CompareTo(b.Value.Order));
	        foreach (KeyValuePair<Type, ChainItemInfo> pair in ordered)
	        {
	            Type serviceType = pair.Key;
	            Type implementationType = pair.Value.ImplementationType;
	            if (implementationType == null || serviceType == implementationType)
	            {
	                builder.Append(TypeNameHelper.GetTypeDisplayName(serviceType));
	            }
	            else
	            {
	                builder.AppendFormat("{0}({1})",
	                    TypeNameHelper.GetTypeDisplayName(serviceType),
	                    TypeNameHelper.GetTypeDisplayName(implementationType));
	            }
	            builder.Append(" -> ");
	        }
	        builder.Append(TypeNameHelper.GetTypeDisplayName(currentlyResolving));
	    }
	    private readonly struct ChainItemInfo
	    {
	        public int Order { get; }
	        public Type ImplementationType { get; }
	        public ChainItemInfo(int order, Type implementationType)
	        {
	            Order = order;
	            ImplementationType = implementationType;
	        }
	    }
	}
	internal sealed class CallSiteFactory : IServiceLookup
	{
	    private const int DefaultSlot = 0;
	    private readonly ServiceDescriptor[]                                            descriptors;
	    private readonly Dictionary<Type, ServiceDescriptorCacheItem>                   descriptorLookup   = new();
	    private readonly ConcurrentDictionary<CallSiteServiceCacheKey, CallSiteService> callSiteCache      = new();
	    private readonly ConcurrentDictionary<Type, object>                             callSiteLocks      = new();
	    private readonly CallSiteStackGuard                                             callSiteStackGuard;
	    public CallSiteFactory(ICollection<ServiceDescriptor> descriptors)
	    {
	        this.callSiteStackGuard = new();
	        this.descriptors = new ServiceDescriptor[descriptors.Count];
	        descriptors.CopyTo(this.descriptors, 0);
	        Populate();
	    }
	    internal ServiceDescriptor[] Descriptors => descriptors;
	    private void Populate()
	    {
	        foreach (ServiceDescriptor descriptor in descriptors)
	        {
	            var serviceType = descriptor.ServiceType;
	            if (serviceType.IsGenericTypeDefinition)
	            {
	                var implementationType = descriptor.ImplementationType;
	                if (implementationType == null || !implementationType.IsGenericTypeDefinition)
	                {
	                    throw new ArgumentException(
	                        Resources.GetOpenGenericServiceRequiresOpenGenericImplementationExceptionMessage(serviceType),
	                        "descriptors");
	                }
	                if (implementationType.IsAbstract || implementationType.IsInterface)
	                {
	                    throw new ArgumentException(
	                        Resources.GetTypeCannotBeActivatedExceptionMessage(implementationType, serviceType));
	                }
	                Type[] serviceTypeGenericArguments = serviceType.GetGenericArguments();
	                Type[] implementationTypeGenericArguments = implementationType.GetGenericArguments();
	                if (serviceTypeGenericArguments.Length != implementationTypeGenericArguments.Length)
	                {
	                    throw new ArgumentException(
	                        Resources.GetArityOfOpenGenericServiceNotEqualArityOfOpenGenericImplementationExceptionMessage(serviceType, implementationType), "descriptors");
	                }
	                if (ServiceProvider.VerifyOpenGenericServiceTrimmability)
	                {
	                    ValidateTrimmingAnnotations(serviceType, serviceTypeGenericArguments, implementationType, implementationTypeGenericArguments);
	                }
	            }
	            else if (descriptor.ImplementationInstance == null && descriptor.ImplementationFactory == null)
	            {
	                Debug.Assert(descriptor.ImplementationType != null);
	                Type implementationType = descriptor.ImplementationType;
	                if (implementationType.IsGenericTypeDefinition ||
	                    implementationType.IsAbstract ||
	                    implementationType.IsInterface)
	                {
	                    throw new ArgumentException(
	                         Resources.GetTypeCannotBeActivatedExceptionMessage(implementationType, serviceType));
	                }
	            }
	            Type cacheKey = serviceType;
	            descriptorLookup.TryGetValue(cacheKey, out ServiceDescriptorCacheItem cacheItem);
	            descriptorLookup[cacheKey] = cacheItem.Add(descriptor);
	        }
	    }
	    private static void ValidateTrimmingAnnotations(
	        Type serviceType,
	        Type[] serviceTypeGenericArguments,
	        Type implementationType,
	        Type[] implementationTypeGenericArguments)
	    {
	        Debug.Assert(serviceTypeGenericArguments.Length == implementationTypeGenericArguments.Length);
	        for (int i = 0; i < serviceTypeGenericArguments.Length; i++)
	        {
	            Type serviceGenericType = serviceTypeGenericArguments[i];
	            Type implementationGenericType = implementationTypeGenericArguments[i];
	            DynamicallyAccessedMemberTypes serviceDynamicallyAccessedMembers = GetDynamicallyAccessedMemberTypes(serviceGenericType);
	            DynamicallyAccessedMemberTypes implementationDynamicallyAccessedMembers = GetDynamicallyAccessedMemberTypes(implementationGenericType);
	            if (!AreCompatible(serviceDynamicallyAccessedMembers, implementationDynamicallyAccessedMembers))
	            {
	                throw new ArgumentException(
	                    Resources.GetTrimmingAnnotationsDoNotMatchExceptionMessage(implementationType, serviceType));
	            }
	            bool serviceHasNewConstraint = serviceGenericType.GenericParameterAttributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint);
	            bool implementationHasNewConstraint = implementationGenericType.GenericParameterAttributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint);
	            if (implementationHasNewConstraint && !serviceHasNewConstraint)
	            {
	                throw new ArgumentException(
	                    Resources.GetTrimmingAnnotationsDoNotMatch_NewConstraintExceptionMessage(implementationType, serviceType));
	            }
	        }
	    }
	    private static DynamicallyAccessedMemberTypes GetDynamicallyAccessedMemberTypes(Type serviceGenericType)
	    {
	        foreach (CustomAttributeData attributeData in serviceGenericType.GetCustomAttributesData())
	        {
	            if (attributeData.AttributeType.FullName == "System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute" &&
	                attributeData.ConstructorArguments.Count == 1 &&
	                attributeData.ConstructorArguments[0].ArgumentType.FullName == "System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes")
	            {
	                return (DynamicallyAccessedMemberTypes)(int)attributeData.ConstructorArguments[0].Value;
	            }
	        }
	        return DynamicallyAccessedMemberTypes.None;
	    }
	    private static bool AreCompatible(
	        DynamicallyAccessedMemberTypes serviceDynamicallyAccessedMembers, 
	        DynamicallyAccessedMemberTypes implementationDynamicallyAccessedMembers)
	    {
	        // The DynamicallyAccessedMemberTypes don't need to exactly match.
	        // The service type needs to preserve a superset of the members required by the implementation type.
	        return serviceDynamicallyAccessedMembers.HasFlag(implementationDynamicallyAccessedMembers);
	    }
	    // For unit testing
	    internal int? GetSlot(ServiceDescriptor serviceDescriptor)
	    {
	        if (descriptorLookup.TryGetValue(serviceDescriptor.ServiceType, out ServiceDescriptorCacheItem item))
	        {
	            return item.GetSlot(serviceDescriptor);
	        }
	        return null;
	    }
	    internal CallSiteService GetCallSite(Type serviceType, CallSiteChain callSiteChain) =>
	            callSiteCache.TryGetValue(new CallSiteServiceCacheKey(serviceType, DefaultSlot), out CallSiteService? site) ? site :
	        CreateCallSite(serviceType, callSiteChain);
	    internal CallSiteService GetCallSite(ServiceDescriptor serviceDescriptor, CallSiteChain callSiteChain)
	    {
	        if (descriptorLookup.TryGetValue(serviceDescriptor.ServiceType, out ServiceDescriptorCacheItem descriptor))
	        {
	            return TryCreateExact(serviceDescriptor, serviceDescriptor.ServiceType, callSiteChain, descriptor.GetSlot(serviceDescriptor));
	        }
	        Debug.Fail("descriptorLookup didn't contain requested serviceDescriptor");
	        return null;
	    }
	    private CallSiteService CreateCallSite(Type serviceType, CallSiteChain callSiteChain)
	    {
	        if (!callSiteStackGuard.TryEnterOnCurrentStack())
	        {
	            return callSiteStackGuard.RunOnEmptyStack((type, chain) => CreateCallSite(type, chain), serviceType, callSiteChain);
	        }
	        // We need to lock the resolution process for a single service type at a time:
	        // Consider the following:
	        // C -> D -> A
	        // E -> D -> A
	        // Resolving C and E in parallel means that they will be modifying the callsite cache concurrently
	        // to add the entry for C and E, but the resolution of D and A is synchronized
	        // to make sure C and E both reference the same instance of the callsite.
	        // This is to make sure we can safely store singleton values on the callsites themselves
	        var callsiteLock = callSiteLocks.GetOrAdd(serviceType, static _ => new object());
	        lock (callsiteLock)
	        {
	            callSiteChain.CheckCircularDependency(serviceType);
	            CallSiteService callSite = TryCreateExact(serviceType, callSiteChain) ??
	                                       TryCreateOpenGeneric(serviceType, callSiteChain) ??
	                                       TryCreateEnumerable(serviceType, callSiteChain);
	            return callSite;
	        }
	    }
	    private CallSiteService TryCreateExact(Type serviceType, CallSiteChain callSiteChain)
	    {
	        if (descriptorLookup.TryGetValue(serviceType, out var descriptor))
	        {
	            return TryCreateExact(descriptor.Last, serviceType, callSiteChain, DefaultSlot);
	        }
	        return null;
	    }
	    private CallSiteService TryCreateOpenGeneric(Type serviceType, CallSiteChain callSiteChain)
	    {
	        if (serviceType.IsConstructedGenericType
	            && descriptorLookup.TryGetValue(serviceType.GetGenericTypeDefinition(), out ServiceDescriptorCacheItem descriptor))
	        {
	            return TryCreateOpenGeneric(descriptor.Last, serviceType, callSiteChain, DefaultSlot, true);
	        }
	        return null;
	    }
	    private CallSiteService TryCreateEnumerable(Type serviceType, CallSiteChain callSiteChain)
	    {
	        CallSiteServiceCacheKey callSiteKey = new CallSiteServiceCacheKey(serviceType, DefaultSlot);
	        if (callSiteCache.TryGetValue(callSiteKey, out CallSiteService serviceCallSite))
	        {
	            return serviceCallSite;
	        }
	        try
	        {
	            callSiteChain.Add(serviceType);
	            if (serviceType.IsConstructedGenericType &&
	                serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
	            {
	                Type itemType = serviceType.GenericTypeArguments[0];
	                CallSiteResultCacheLocation cacheLocation = CallSiteResultCacheLocation.Root;
	                var callSites = new List<CallSiteService>();
	                // If item type is not generic we can safely use descriptor cache
	                if (!itemType.IsConstructedGenericType &&
	                    descriptorLookup.TryGetValue(itemType, out ServiceDescriptorCacheItem descriptors))
	                {
	                    for (int i = 0; i < descriptors.Count; i++)
	                    {
	                        ServiceDescriptor descriptor = descriptors[i];
	                        // Last service should get slot 0
	                        int slot = descriptors.Count - i - 1;
	                        // There may not be any open generics here
	                        CallSiteService callSite = TryCreateExact(descriptor, itemType, callSiteChain, slot);
	                        Debug.Assert(callSite != null);
	                        cacheLocation = GetCommonCacheLocation(cacheLocation, callSite.Cache.Location);
	                        callSites.Add(callSite);
	                    }
	                }
	                else
	                {
	                    int slot = 0;
	                    // We are going in reverse so the last service in descriptor list gets slot 0
	                    for (int i = this.descriptors.Length - 1; i >= 0; i--)
	                    {
	                        ServiceDescriptor descriptor = this.descriptors[i];
	                        CallSiteService callSite = TryCreateExact(descriptor, itemType, callSiteChain, slot) ??
	                                       TryCreateOpenGeneric(descriptor, itemType, callSiteChain, slot, false);
	                        if (callSite != null)
	                        {
	                            slot++;
	                            cacheLocation = GetCommonCacheLocation(cacheLocation, callSite.Cache.Location);
	                            callSites.Add(callSite);
	                        }
	                    }
	                    callSites.Reverse();
	                }
	                CallSiteResultCache resultCache = CallSiteResultCache.None;
	                if (cacheLocation == CallSiteResultCacheLocation.Scope || cacheLocation == CallSiteResultCacheLocation.Root)
	                {
	                    resultCache = new CallSiteResultCache(cacheLocation, callSiteKey);
	                }
	                return callSiteCache[callSiteKey] = new EnumerableCallSite(resultCache, itemType, callSites.ToArray());
	            }
	            return null;
	        }
	        finally
	        {
	            callSiteChain.Remove(serviceType);
	        }
	    }
	    private CallSiteResultCacheLocation GetCommonCacheLocation(CallSiteResultCacheLocation locationA, CallSiteResultCacheLocation locationB)
	    {
	        return (CallSiteResultCacheLocation)Math.Max((int)locationA, (int)locationB);
	    }
	    private CallSiteService TryCreateExact(ServiceDescriptor descriptor, Type serviceType, CallSiteChain callSiteChain, int slot)
	    {
	        if (serviceType == descriptor.ServiceType)
	        {
	            CallSiteServiceCacheKey callSiteKey = new CallSiteServiceCacheKey(serviceType, slot);
	            if (callSiteCache.TryGetValue(callSiteKey, out CallSiteService serviceCallSite))
	            {
	                return serviceCallSite;
	            }
	            CallSiteService callSite;
	            var lifetime = new CallSiteResultCache(descriptor.Lifetime, serviceType, slot);
	            if (descriptor.ImplementationInstance != null)
	            {
	                callSite = new ConstantCallSite(descriptor.ServiceType, descriptor.ImplementationInstance);
	            }
	            else if (descriptor.ImplementationFactory != null)
	            {
	                callSite = new FactoryCallSite(lifetime, descriptor.ServiceType, descriptor.ImplementationFactory);
	            }
	            else if (descriptor.ImplementationType != null)
	            {
	                callSite = CreateConstructorCallSite(lifetime, descriptor.ServiceType, descriptor.ImplementationType, callSiteChain);
	            }
	            else
	            {
	                throw new InvalidOperationException(Resources.InvalidServiceDescriptor);
	            }
	            return callSiteCache[callSiteKey] = callSite;
	        }
	        return null;
	    }
	    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2055:MakeGenericType",
	        Justification = "MakeGenericType here is used to create a closed generic implementation type given the closed service type. " +
	        "Trimming annotations on the generic types are verified when 'Assimalign.Cohesion.DependencyInjection.VerifyOpenGenericServiceTrimmability' is set, which is set by default when PublishTrimmed=true. " +
	        "That check informs developers when these generic types don't have compatible trimming annotations.")]
	    private CallSiteService TryCreateOpenGeneric(ServiceDescriptor descriptor, Type serviceType, CallSiteChain callSiteChain, int slot, bool throwOnConstraintViolation)
	    {
	        if (serviceType.IsConstructedGenericType &&
	            serviceType.GetGenericTypeDefinition() == descriptor.ServiceType)
	        {
	            CallSiteServiceCacheKey callSiteKey = new CallSiteServiceCacheKey(serviceType, slot);
	            if (callSiteCache.TryGetValue(callSiteKey, out CallSiteService serviceCallSite))
	            {
	                return serviceCallSite;
	            }
	            Debug.Assert(descriptor.ImplementationType != null, "descriptor.ImplementationType != null");
	            var lifetime = new CallSiteResultCache(descriptor.Lifetime, serviceType, slot);
	            Type closedType;
	            try
	            {
	                closedType = descriptor.ImplementationType.MakeGenericType(serviceType.GenericTypeArguments);
	            }
	            catch (ArgumentException)
	            {
	                if (throwOnConstraintViolation)
	                {
	                    throw;
	                }
	                return null;
	            }
	            return callSiteCache[callSiteKey] = CreateConstructorCallSite(lifetime, serviceType, closedType, callSiteChain);
	        }
	        return null;
	    }
	    private CallSiteService CreateConstructorCallSite(
	        CallSiteResultCache lifetime,
	        Type serviceType,
	        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType,
	        CallSiteChain callSiteChain)
	    {
	        try
	        {
	            callSiteChain.Add(serviceType, implementationType);
	            ConstructorInfo[] constructors = implementationType.GetConstructors();
	            CallSiteService[] parameterCallSites = null;
	            if (constructors.Length == 0)
	            {
	                throw new InvalidOperationException(Resources.GetNoConstructorMatchExceptionMessage(implementationType));
	            }
	            else if (constructors.Length == 1)
	            {
	                ConstructorInfo constructor = constructors[0];
	                ParameterInfo[] parameters = constructor.GetParameters();
	                if (parameters.Length == 0)
	                {
	                    return new ConstructorCallSite(lifetime, serviceType, constructor);
	                }
	                parameterCallSites = CreateArgumentCallSites(
	                    implementationType,
	                    callSiteChain,
	                    parameters,
	                    throwIfCallSiteNotFound: true);
	                return new ConstructorCallSite(lifetime, serviceType, constructor, parameterCallSites);
	            }
	            Array.Sort(constructors,
	                (a, b) => b.GetParameters().Length.CompareTo(a.GetParameters().Length));
	            ConstructorInfo bestConstructor = null;
	            HashSet<Type> bestConstructorParameterTypes = null;
	            for (int i = 0; i < constructors.Length; i++)
	            {
	                ParameterInfo[] parameters = constructors[i].GetParameters();
	                CallSiteService[] currentParameterCallSites = CreateArgumentCallSites(
	                    implementationType,
	                    callSiteChain,
	                    parameters,
	                    throwIfCallSiteNotFound: false);
	                if (currentParameterCallSites != null)
	                {
	                    if (bestConstructor == null)
	                    {
	                        bestConstructor = constructors[i];
	                        parameterCallSites = currentParameterCallSites;
	                    }
	                    else
	                    {
	                        // Since we're visiting constructors in decreasing order of number of parameters,
	                        // we'll only see ambiguities or supersets once we've seen a 'bestConstructor'.
	                        if (bestConstructorParameterTypes == null)
	                        {
	                            bestConstructorParameterTypes = new HashSet<Type>();
	                            foreach (ParameterInfo p in bestConstructor.GetParameters())
	                            {
	                                bestConstructorParameterTypes.Add(p.ParameterType);
	                            }
	                        }
	                        foreach (ParameterInfo p in parameters)
	                        {
	                            if (!bestConstructorParameterTypes.Contains(p.ParameterType))
	                            {
	                                // Ambiguous match exception
	                                throw new InvalidOperationException(string.Join(
	                                    Environment.NewLine,
	                                    Resources.GetAmbiguousConstructorExceptionMessage(implementationType),
	                                    bestConstructor,
	                                    constructors[i]));
	                            }
	                        }
	                    }
	                }
	            }
	            if (bestConstructor == null)
	            {
	                throw new InvalidOperationException(
	                    Resources.GetUnableToActivateTypeExceptionMessage(implementationType));
	            }
	            else
	            {
	                Debug.Assert(parameterCallSites != null);
	                return new ConstructorCallSite(lifetime, serviceType, bestConstructor, parameterCallSites);
	            }
	        }
	        finally
	        {
	            callSiteChain.Remove(serviceType);
	        }
	    }
	    private CallSiteService[] CreateArgumentCallSites(
	        Type implementationType,
	        CallSiteChain callSiteChain,
	        ParameterInfo[] parameters,
	        bool throwIfCallSiteNotFound)
	    {
	        var parameterCallSites = new CallSiteService[parameters.Length];
	        for (int index = 0; index < parameters.Length; index++)
	        {
	            Type parameterType = parameters[index].ParameterType;
	            CallSiteService callSite = GetCallSite(parameterType, callSiteChain);
	            if (callSite == null && parameters[index].TryGetDefaultValue(out object defaultValue))
	            {
	                callSite = new ConstantCallSite(parameterType, defaultValue);
	            }
	            if (callSite == null)
	            {
	                if (throwIfCallSiteNotFound)
	                {
	                    throw new InvalidOperationException(Resources.GetCannotResolveServiceExceptionMessage(
	                        parameterType,
	                        implementationType));
	                }
	                return null;
	            }
	            parameterCallSites[index] = callSite;
	        }
	        return parameterCallSites;
	    }
	    public void Add(Type type, CallSiteService serviceCallSite)
	    {
	        callSiteCache[new CallSiteServiceCacheKey(type, DefaultSlot)] = serviceCallSite;
	    }
	    public bool IsService(Type serviceType)
	    {
	        if (serviceType is null)
	        {
	            throw new ArgumentNullException(nameof(serviceType));
	        }
	        // Querying for an open generic should return false (they aren't resolvable)
	        if (serviceType.IsGenericTypeDefinition)
	        {
	            return false;
	        }
	        if (descriptorLookup.ContainsKey(serviceType))
	        {
	            return true;
	        }
	        if (serviceType.IsConstructedGenericType && serviceType.GetGenericTypeDefinition() is Type genericDefinition)
	        {
	            // We special case Enumerable since it isn't explicitly registered in the container
	            // yet we can manifest instances of it when requested.
	            return genericDefinition == typeof(IEnumerable<>) || descriptorLookup.ContainsKey(genericDefinition);
	        }
	        // These are the built in service types that aren't part of the list of service descriptors
	        // If you update these make sure to also update the code in ServiceProvider.ctor
	        return serviceType == typeof(IServiceProvider) ||
	               serviceType == typeof(IServiceScopeFactory) ||
	               serviceType == typeof(IServiceLookup);
	    }
	    private struct ServiceDescriptorCacheItem
	    {
	        private ServiceDescriptor _item;
	        private List<ServiceDescriptor> _items;
	        public ServiceDescriptor Last
	        {
	            get
	            {
	                if (_items != null && _items.Count > 0)
	                {
	                    return _items[_items.Count - 1];
	                }
	                Debug.Assert(_item != null);
	                return _item;
	            }
	        }
	        public int Count
	        {
	            get
	            {
	                if (_item == null)
	                {
	                    Debug.Assert(_items == null);
	                    return 0;
	                }
	                return 1 + (_items?.Count ?? 0);
	            }
	        }
	        public ServiceDescriptor this[int index]
	        {
	            get
	            {
	                if (index >= Count)
	                {
	                    throw new ArgumentOutOfRangeException(nameof(index));
	                }
	                if (index == 0)
	                {
	                    return _item;
	                }
	                return _items[index - 1];
	            }
	        }
	        public int GetSlot(ServiceDescriptor descriptor)
	        {
	            if (descriptor == _item)
	            {
	                return Count - 1;
	            }
	            if (_items != null)
	            {
	                int index = _items.IndexOf(descriptor);
	                if (index != -1)
	                {
	                    return _items.Count - (index + 1);
	                }
	            }
	            throw new InvalidOperationException(Resources.ServiceDescriptorNotExist);
	        }
	        public ServiceDescriptorCacheItem Add(ServiceDescriptor descriptor)
	        {
	            var newCacheItem = default(ServiceDescriptorCacheItem);
	            if (_item == null)
	            {
	                Debug.Assert(_items == null);
	                newCacheItem._item = descriptor;
	            }
	            else
	            {
	                newCacheItem._item = _item;
	                newCacheItem._items = _items ?? new List<ServiceDescriptor>();
	                newCacheItem._items.Add(descriptor);
	            }
	            return newCacheItem;
	        }
	    }
	}
	internal enum CallSiteKind
	{
	    Factory,
	    Constructor,
	    Constant,
	    Enumerable,
	    ServiceProvider,
	    Scope,
	    Transient,
	    Singleton
	}
	internal struct CallSiteResultCache
	{
	    public static CallSiteResultCache None { get; } = new CallSiteResultCache(CallSiteResultCacheLocation.None, CallSiteServiceCacheKey.Empty);
	    internal CallSiteResultCache(CallSiteResultCacheLocation lifetime, CallSiteServiceCacheKey cacheKey)
	    {
	        Location = lifetime;
	        Key = cacheKey;
	    }
	    public CallSiteResultCache(ServiceLifetime lifetime, Type type, int slot)
	    {
	        Debug.Assert(lifetime == ServiceLifetime.Transient || type != null);
	        switch (lifetime)
	        {
	            case ServiceLifetime.Singleton:
	                Location = CallSiteResultCacheLocation.Root;
	                break;
	            case ServiceLifetime.Scoped:
	                Location = CallSiteResultCacheLocation.Scope;
	                break;
	            case ServiceLifetime.Transient:
	                Location = CallSiteResultCacheLocation.Dispose;
	                break;
	            default:
	                Location = CallSiteResultCacheLocation.None;
	                break;
	        }
	        Key = new CallSiteServiceCacheKey(type, slot);
	    }
	    public CallSiteResultCacheLocation Location { get; set; }
	    public CallSiteServiceCacheKey Key { get; set; }
	}
	internal enum CallSiteResultCacheLocation
	{
	    Root,
	    Scope,
	    Dispose,
	    None
	}
	internal abstract class CallSiteService
	{
	    protected CallSiteService(CallSiteResultCache cache)
	    {
	        Cache = cache;
	    }
	    public abstract Type ServiceType { get; }
	    public abstract Type? ImplementationType { get; }
	    public abstract CallSiteKind Kind { get; }
	    public CallSiteResultCache Cache { get; }
	    public object Value { get; set; }
	    public bool CaptureDisposable =>
	        ImplementationType == null ||
	        typeof(IDisposable).IsAssignableFrom(ImplementationType) ||
	        typeof(IAsyncDisposable).IsAssignableFrom(ImplementationType);
	}
	internal readonly struct CallSiteServiceCacheKey : IEquatable<CallSiteServiceCacheKey>
	{
	    public static CallSiteServiceCacheKey Empty { get; } = new CallSiteServiceCacheKey(null, 0);
	    public Type Type { get; }
	    public int Slot { get; }
	    public CallSiteServiceCacheKey(Type type, int slot)
	    {
	        Type = type;
	        Slot = slot;
	    }
	    public bool Equals(CallSiteServiceCacheKey other)
	    {
	        return Type == other.Type && Slot == other.Slot;
	    }
	    public override int GetHashCode()
	    {
	        unchecked
	        {
	            return ((Type?.GetHashCode() ?? 23) * 397) ^ Slot;
	        }
	    }
	}
	internal sealed class CallSiteStackGuard
	{
	    private const int MaxExecutionStackCount = 1024;
	    private int executionStackCount;
	    public bool TryEnterOnCurrentStack()
	    {
	        if (RuntimeHelpers.TryEnsureSufficientExecutionStack())
	        {
	            return true;
	        }
	        if (executionStackCount < MaxExecutionStackCount)
	        {
	            return false;
	        }
	        throw new InsufficientExecutionStackException();
	    }
	    public TR RunOnEmptyStack<T1, T2, TR>(Func<T1, T2, TR> action, T1 arg1, T2 arg2)
	    {
	        // Prefer ValueTuple when available to reduce dependencies on Tuple
	        return RunOnEmptyStackCore(static s =>
	        {
	            var t = ((Func<T1, T2, TR>, T1, T2))s;
	            return t.Item1(t.Item2, t.Item3);
	        }, (action, arg1, arg2));
	    }
	    private R RunOnEmptyStackCore<R>(Func<object, R> action, object state)
	    {
	        executionStackCount++;
	        try
	        {
	            // Using default scheduler rather than picking up the current scheduler.
	            Task<R> task = Task.Factory.StartNew(action, state, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
	            // Avoid AsyncWaitHandle lazy allocation of ManualResetEvent in the rare case we finish quickly.
	            if (!task.IsCompleted)
	            {
	                // Task.Wait has the potential of inlining the task's execution on the current thread; avoid this.
	                ((IAsyncResult)task).AsyncWaitHandle.WaitOne();
	            }
	            // Using awaiter here to propagate original exception
	            return task.GetAwaiter().GetResult();
	        }
	        finally
	        {
	            executionStackCount--;
	        }
	    }
	}
	#endregion
	#region \Internal\ServiceLookup\Engines
	internal abstract class CompiledServiceProviderEngine : ServiceProviderEngine
	{
	#if IL_EMIT
	    public ILEmitResolverBuilderVisitor ResolverBuilder { get; }
	#else
	    public CallSiteExpressionResolverBuilderVisitor ResolverBuilder { get; }
	#endif
	    public CompiledServiceProviderEngine(ServiceProvider provider)
	    {
	        ResolverBuilder = new(provider);
	    }
	    public override Func<ServiceProviderEngineScope, object> RealizeService(CallSiteService callSite) => ResolverBuilder.Build(callSite);
	}
	internal sealed class DynamicServiceProviderEngine : CompiledServiceProviderEngine
	{
	    private readonly ServiceProvider serviceProvider;
	    public DynamicServiceProviderEngine(ServiceProvider serviceProvider) : base(serviceProvider)
	    {
	        this.serviceProvider = serviceProvider;
	    }
	    public override Func<ServiceProviderEngineScope, object> RealizeService(CallSiteService callSite)
	    {
	        int callCount = 0;
	        return scope =>
	        {
	            // Resolve the result before we increment the call count, this ensures that singletons
	            // won't cause any side effects during the compilation of the resolve function.
	            var result = CallSiteRuntimeResolverVisitor.Instance.Resolve(callSite, scope);
	            if (Interlocked.Increment(ref callCount) == 2)
	            {
	                // Don't capture the ExecutionContext when forking to build the compiled version of the
	                // resolve function
	                _ = ThreadPool.UnsafeQueueUserWorkItem(_ =>
	                {
	                    try
	                    {
	                        serviceProvider.ReplaceServiceAccessor(callSite, base.RealizeService(callSite));
	                    }
	                    catch (Exception ex)
	                    {
	                        ServiceEventSource.Log.ServiceRealizationFailed(ex, serviceProvider.GetHashCode());
	                        Debug.Fail($"We should never get exceptions from the background compilation.{Environment.NewLine}{ex}");
	                    }
	                },
	                null);
	            }
	            return result;
	        };
	    }
	}
	internal class ExpressionsServiceProviderEngine : ServiceProviderEngine
	{
	    private readonly CallSiteExpressionResolverBuilderVisitor visitor;
	    public ExpressionsServiceProviderEngine(ServiceProvider serviceProvider)
	    {
	        visitor = new CallSiteExpressionResolverBuilderVisitor(serviceProvider);
	    }
	    public override Func<ServiceProviderEngineScope, object> RealizeService(CallSiteService callSite)
	    {
	        return visitor.Build(callSite);
	    }
	}
	internal sealed class RuntimeServiceProviderEngine : ServiceProviderEngine
	{
	    private RuntimeServiceProviderEngine() { }
	    public override Func<ServiceProviderEngineScope, object> RealizeService(CallSiteService callSite)
	    {
	        return scope =>
	        {
	            return CallSiteRuntimeResolverVisitor.Instance.Resolve(callSite, scope);
	        };
	    }
	    public static RuntimeServiceProviderEngine Instance { get; } = new RuntimeServiceProviderEngine();
	}
	internal abstract class ServiceProviderEngine
	{
	    public abstract Func<ServiceProviderEngineScope, object> RealizeService(CallSiteService callSite);
	}
	internal sealed class ServiceProviderEngineScope : IServiceScope, IServiceProvider, IServiceScopeFactory, IAsyncDisposable
	{
	    // For testing only
	    internal IList<object> Disposables => this.disposables ?? (IList<object>)Array.Empty<object>();
	    private bool disposed;
	    private List<object> disposables;
	    public ServiceProviderEngineScope(ServiceProvider provider, bool isRootScope)
	    {
	        ResolvedServices = new();
	        RootProvider = provider;
	        IsRootScope = isRootScope;
	    }
	    internal Dictionary<CallSiteServiceCacheKey, object?> ResolvedServices { get; }
	    // This lock protects state on the scope, in particular, for the root scope, it protects
	    // the list of disposable entries only, since ResolvedServices are cached on CallSites
	    // For other scopes, it protects ResolvedServices and the list of disposables
	    internal object Sync => ResolvedServices;
	    public bool IsRootScope { get; }
	    internal ServiceProvider RootProvider { get; }
	    public object GetService(Type serviceType)
	    {
	        if (disposed)
	        {
	            ThrowHelper.ThrowObjectDisposedException();
	        }
	        return RootProvider.GetService(serviceType, this);
	    }
	    public IServiceProvider ServiceProvider => this;
	    public IServiceScope CreateScope() => RootProvider.CreateScope();
	    internal object CaptureDisposable(object service)
	    {
	        if (ReferenceEquals(this, service) || !(service is IDisposable || service is IAsyncDisposable))
	        {
	            return service;
	        }
	        var disposed = false;
	        lock (Sync)
	        {
	            if (this.disposed)
	            {
	                disposed = true;
	            }
	            else
	            {
	                this.disposables ??= new List<object>();
	                this.disposables.Add(service);
	            }
	        }
	        // Don't run customer code under the lock
	        if (disposed)
	        {
	            if (service is IDisposable disposable)
	            {
	                disposable.Dispose();
	            }
	            else
	            {
	                // sync over async, for the rare case that an object only implements IAsyncDisposable and may end up starving the thread pool.
	                Task.Run(() => ((IAsyncDisposable)service).DisposeAsync().AsTask()).GetAwaiter().GetResult();
	            }
	            ThrowHelper.ThrowObjectDisposedException();
	        }
	        return service;
	    }
	    public void Dispose()
	    {
	        List<object> toDispose = BeginDispose();
	        if (toDispose != null)
	        {
	            for (int i = toDispose.Count - 1; i >= 0; i--)
	            {
	                if (toDispose[i] is IDisposable disposable)
	                {
	                    disposable.Dispose();
	                }
	                else
	                {
	                    throw new InvalidOperationException(
	                        Resources.GetAsyncDisposableServiceDisposeExceptionMessage(
	                            TypeNameHelper.GetTypeDisplayName(toDispose[i])));
	                }
	            }
	        }
	    }
	    public ValueTask DisposeAsync()
	    {
	        List<object> toDispose = BeginDispose();
	        if (toDispose != null)
	        {
	            try
	            {
	                for (int i = toDispose.Count - 1; i >= 0; i--)
	                {
	                    object disposable = toDispose[i];
	                    if (disposable is IAsyncDisposable asyncDisposable)
	                    {
	                        ValueTask vt = asyncDisposable.DisposeAsync();
	                        if (!vt.IsCompletedSuccessfully)
	                        {
	                            return Await(i, vt, toDispose);
	                        }
	                        // If its a IValueTaskSource backed ValueTask,
	                        // inform it its result has been read so it can reset
	                        vt.GetAwaiter().GetResult();
	                    }
	                    else
	                    {
	                        ((IDisposable)disposable).Dispose();
	                    }
	                }
	            }
	            catch (Exception ex)
	            {
	                return new ValueTask(Task.FromException(ex));
	            }
	        }
	        return default;
	        static async ValueTask Await(int i, ValueTask vt, List<object> toDispose)
	        {
	            await vt.ConfigureAwait(false);
	            // vt is acting on the disposable at index i,
	            // decrement it and move to the next iteration
	            i--;
	            for (; i >= 0; i--)
	            {
	                object disposable = toDispose[i];
	                if (disposable is IAsyncDisposable asyncDisposable)
	                {
	                    await asyncDisposable.DisposeAsync().ConfigureAwait(false);
	                }
	                else
	                {
	                    ((IDisposable)disposable).Dispose();
	                }
	            }
	        }
	    }
	    private List<object> BeginDispose()
	    {
	        lock (Sync)
	        {
	            if (disposed)
	            {
	                return null;
	            }
	            // Track statistics about the scope (number of disposable objects and number of disposed services)
	            ServiceEventSource.Log.ScopeDisposed(RootProvider.GetHashCode(), ResolvedServices.Count, disposables?.Count ?? 0);
	            // We've transitioned to the disposed state, so future calls to
	            // CaptureDisposable will immediately dispose the object.
	            // No further changes to _state.Disposables, are allowed.
	            disposed = true;
	            // ResolvedServices is never cleared for singletons because there might be a compilation running in background
	            // trying to get a cached singleton service. If it doesn't find it
	            // it will try to create a new one which will result in an ObjectDisposedException.
	        }
	        if (IsRootScope && !RootProvider.IsDisposed)
	        {
	            // If this ServiceProviderEngineScope instance is a root scope, disposing this instance will need to dispose the RootProvider too.
	            // Otherwise the RootProvider will never get disposed and will leak.
	            // Note, if the RootProvider get disposed first, it will automatically dispose all attached ServiceProviderEngineScope objects.
	            RootProvider.Dispose();
	        }
	        return disposables;
	    }
	}
	#endregion
	#region \Internal\ServiceLookup\ILEmit
	internal sealed class ILEmitResolverBuilderContext
	{
	    public ILGenerator Generator { get; set; }
	    public List<object> Constants { get; set; }
	    public List<Func<IServiceProvider, object>> Factories { get; set; }
	}
	internal sealed class ILEmitResolverBuilderVisitor : CallSiteVisitor<ILEmitResolverBuilderContext, object>
	{
	    private static readonly MethodInfo ResolvedServicesGetter = typeof(ServiceProviderEngineScope).GetProperty(
	        nameof(ServiceProviderEngineScope.ResolvedServices), BindingFlags.Instance | BindingFlags.NonPublic).GetMethod;
	    private static readonly MethodInfo ScopeLockGetter = typeof(ServiceProviderEngineScope).GetProperty(
	        nameof(ServiceProviderEngineScope.Sync), BindingFlags.Instance | BindingFlags.NonPublic).GetMethod;
	    private static readonly MethodInfo ScopeIsRootScope = typeof(ServiceProviderEngineScope).GetProperty(
	        nameof(ServiceProviderEngineScope.IsRootScope), BindingFlags.Instance | BindingFlags.Public).GetMethod;
	    private static readonly MethodInfo CallSiteRuntimeResolverResolveMethod = typeof(CallSiteRuntimeResolverVisitor).GetMethod(
	        nameof(CallSiteRuntimeResolverVisitor.Resolve), BindingFlags.Public | BindingFlags.Instance);
	    private static readonly MethodInfo CallSiteRuntimeResolverInstanceField = typeof(CallSiteRuntimeResolverVisitor).GetProperty(
	        nameof(CallSiteRuntimeResolverVisitor.Instance), BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetMethod;
	    private static readonly FieldInfo FactoriesField = typeof(ILEmitResolverBuilderRuntimeContext).GetField(nameof(ILEmitResolverBuilderRuntimeContext.Factories));
	    private static readonly FieldInfo ConstantsField = typeof(ILEmitResolverBuilderRuntimeContext).GetField(nameof(ILEmitResolverBuilderRuntimeContext.Constants));
	    private static readonly MethodInfo GetTypeFromHandleMethod = typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle));
	    private static readonly ConstructorInfo CacheKeyCtor = typeof(CallSiteServiceCacheKey).GetConstructors()[0];
	    private sealed class ILEmitResolverBuilderRuntimeContext
	    {
	        public object?[]? Constants;
	        public Func<IServiceProvider, object>[]? Factories;
	    }
	    private struct GeneratedMethod
	    {
	        public Func<ServiceProviderEngineScope, object?> Lambda;
	        public ILEmitResolverBuilderRuntimeContext Context;
	        public DynamicMethod DynamicMethod;
	    }
	    private readonly ServiceProviderEngineScope _rootScope;
	    private readonly ConcurrentDictionary<CallSiteServiceCacheKey, GeneratedMethod> _scopeResolverCache;
	    private readonly Func<CallSiteServiceCacheKey, CallSiteService, GeneratedMethod> _buildTypeDelegate;
	    public ILEmitResolverBuilderVisitor(ServiceProvider serviceProvider)
	    {
	        _rootScope = serviceProvider.Root;
	        _scopeResolverCache = new ConcurrentDictionary<CallSiteServiceCacheKey, GeneratedMethod>();
	        _buildTypeDelegate = (key, cs) => BuildTypeNoCache(cs);
	    }
	    public Func<ServiceProviderEngineScope, object?> Build(CallSiteService callSite)
	    {
	        return BuildType(callSite).Lambda;
	    }
	    private GeneratedMethod BuildType(CallSiteService callSite)
	    {
	        // Only scope methods are cached
	        if (callSite.Cache.Location == CallSiteResultCacheLocation.Scope)
	        {
	#if NETFRAMEWORK || NETSTANDARD2_0
	                return _scopeResolverCache.GetOrAdd(callSite.Cache.Key, key => _buildTypeDelegate(key, callSite));
	#else
	            return _scopeResolverCache.GetOrAdd(callSite.Cache.Key, _buildTypeDelegate, callSite);
	#endif
	        }
	        return BuildTypeNoCache(callSite);
	    }
	    private GeneratedMethod BuildTypeNoCache(CallSiteService callSite)
	    {
	        // We need to skip visibility checks because services/constructors might be private
	        var dynamicMethod = new DynamicMethod("ResolveService",
	            attributes: MethodAttributes.Public | MethodAttributes.Static,
	            callingConvention: CallingConventions.Standard,
	            returnType: typeof(object),
	            parameterTypes: new[] { typeof(ILEmitResolverBuilderRuntimeContext), typeof(ServiceProviderEngineScope) },
	            owner: GetType(),
	            skipVisibility: true);
	        // In traces we've seen methods range from 100B - 4K sized methods since we've
	        // stop trying to inline everything into scoped methods. We'll pay for a couple of resizes
	        // so there'll be allocations but we could potentially change ILGenerator to use the array pool
	        ILGenerator ilGenerator = dynamicMethod.GetILGenerator(512);
	        ILEmitResolverBuilderRuntimeContext runtimeContext = GenerateMethodBody(callSite, ilGenerator);
	#if SAVE_ASSEMBLIES
	            var assemblyName = "Test" + DateTime.Now.Ticks;
	            var fileName = assemblyName + ".dll";
	            var assembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.RunAndSave);
	            var module = assembly.DefineDynamicModule(assemblyName, fileName);
	            var type = module.DefineType(callSite.ServiceType.Name + "Resolver");
	            var method = type.DefineMethod(
	                "ResolveService", MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, typeof(object),
	                new[] { typeof(ILEmitResolverBuilderRuntimeContext), typeof(ServiceProviderEngineScope) });
	            GenerateMethodBody(callSite, method.GetILGenerator());
	            type.CreateTypeInfo();
	            // Assembly.Save is only available in .NET Framework (https://github.com/dotnet/runtime/issues/15704)
	            assembly.Save(fileName);
	#endif
	        ServiceEventSource.Log.DynamicMethodBuilt(_rootScope.RootProvider, callSite.ServiceType, ilGenerator.ILOffset);
	        return new GeneratedMethod()
	        {
	            Lambda = (Func<ServiceProviderEngineScope, object?>)dynamicMethod.CreateDelegate(typeof(Func<ServiceProviderEngineScope, object?>), runtimeContext),
	            Context = runtimeContext,
	            DynamicMethod = dynamicMethod
	        };
	    }
	    protected override object? VisitDisposeCache(CallSiteService transientCallSite, ILEmitResolverBuilderContext argument)
	    {
	        if (transientCallSite.CaptureDisposable)
	        {
	            BeginCaptureDisposable(argument);
	            VisitCallSiteMain(transientCallSite, argument);
	            EndCaptureDisposable(argument);
	        }
	        else
	        {
	            VisitCallSiteMain(transientCallSite, argument);
	        }
	        return null;
	    }
	    protected override object? VisitConstructor(ConstructorCallSite constructorCallSite, ILEmitResolverBuilderContext argument)
	    {
	        // new T([create arguments])
	        foreach (CallSiteService parameterCallSite in constructorCallSite.ParameterCallSites)
	        {
	            VisitCallSite(parameterCallSite, argument);
	            if (parameterCallSite.ServiceType.IsValueType)
	            {
	                argument.Generator.Emit(OpCodes.Unbox_Any, parameterCallSite.ServiceType);
	            }
	        }
	        argument.Generator.Emit(OpCodes.Newobj, constructorCallSite.ConstructorInfo);
	        if (constructorCallSite.ImplementationType!.IsValueType)
	        {
	            argument.Generator.Emit(OpCodes.Box, constructorCallSite.ImplementationType);
	        }
	        return null;
	    }
	    protected override object? VisitRootCache(CallSiteService callSite, ILEmitResolverBuilderContext argument)
	    {
	        AddConstant(argument, CallSiteRuntimeResolverVisitor.Instance.Resolve(callSite, _rootScope));
	        return null;
	    }
	    protected override object? VisitScopeCache(CallSiteService scopedCallSite, ILEmitResolverBuilderContext argument)
	    {
	        GeneratedMethod generatedMethod = BuildType(scopedCallSite);
	        // Type builder doesn't support invoking dynamic methods, replace them with delegate.Invoke calls
	#if SAVE_ASSEMBLIES
	            AddConstant(argument, generatedMethod.Lambda);
	            // ProviderScope
	            argument.Generator.Emit(OpCodes.Ldarg_1);
	            argument.Generator.Emit(OpCodes.Call, generatedMethod.Lambda.GetType().GetMethod("Invoke"));
	#else
	        AddConstant(argument, generatedMethod.Context);
	        // ProviderScope
	        argument.Generator.Emit(OpCodes.Ldarg_1);
	        argument.Generator.Emit(OpCodes.Call, generatedMethod.DynamicMethod);
	#endif
	        return null;
	    }
	    protected override object? VisitConstant(ConstantCallSite constantCallSite, ILEmitResolverBuilderContext argument)
	    {
	        AddConstant(argument, constantCallSite.DefaultValue);
	        return null;
	    }
	    protected override object? VisitServiceProvider(ServiceProviderCallSite serviceProviderCallSite, ILEmitResolverBuilderContext argument)
	    {
	        // [return] ProviderScope
	        argument.Generator.Emit(OpCodes.Ldarg_1);
	        return null;
	    }
	    protected override object? VisitEnumerable(EnumerableCallSite enumerableCallSite, ILEmitResolverBuilderContext argument)
	    {
	        if (enumerableCallSite.ServiceCallSites.Length == 0)
	        {
	            argument.Generator.Emit(OpCodes.Call, ServiceLookupHelpers.GetArrayEmptyMethodInfo(enumerableCallSite.ItemType));
	        }
	        else
	        {
	            // var array = new ItemType[];
	            // array[0] = [Create argument0];
	            // array[1] = [Create argument1];
	            // ...
	            argument.Generator.Emit(OpCodes.Ldc_I4, enumerableCallSite.ServiceCallSites.Length);
	            argument.Generator.Emit(OpCodes.Newarr, enumerableCallSite.ItemType);
	            for (int i = 0; i < enumerableCallSite.ServiceCallSites.Length; i++)
	            {
	                // duplicate array
	                argument.Generator.Emit(OpCodes.Dup);
	                // push index
	                argument.Generator.Emit(OpCodes.Ldc_I4, i);
	                // create parameter
	                CallSiteService parameterCallSite = enumerableCallSite.ServiceCallSites[i];
	                VisitCallSite(parameterCallSite, argument);
	                if (parameterCallSite.ServiceType.IsValueType)
	                {
	                    argument.Generator.Emit(OpCodes.Unbox_Any, parameterCallSite.ServiceType);
	                }
	                // store
	                argument.Generator.Emit(OpCodes.Stelem, enumerableCallSite.ItemType);
	            }
	        }
	        return null;
	    }
	    protected override object? VisitFactory(FactoryCallSite factoryCallSite, ILEmitResolverBuilderContext argument)
	    {
	        argument.Factories ??= new List<Func<IServiceProvider, object>>();
	        // this.Factories[i](ProviderScope)
	        argument.Generator.Emit(OpCodes.Ldarg_0);
	        argument.Generator.Emit(OpCodes.Ldfld, FactoriesField);
	        argument.Generator.Emit(OpCodes.Ldc_I4, argument.Factories.Count);
	        argument.Generator.Emit(OpCodes.Ldelem, typeof(Func<IServiceProvider, object>));
	        argument.Generator.Emit(OpCodes.Ldarg_1);
	        argument.Generator.Emit(OpCodes.Call, ServiceLookupHelpers.InvokeFactoryMethodInfo);
	        argument.Factories.Add(factoryCallSite.Factory);
	        return null;
	    }
	    private static void AddConstant(ILEmitResolverBuilderContext argument, object? value)
	    {
	        argument.Constants ??= new List<object?>();
	        // this.Constants[i]
	        argument.Generator.Emit(OpCodes.Ldarg_0);
	        argument.Generator.Emit(OpCodes.Ldfld, ConstantsField);
	        argument.Generator.Emit(OpCodes.Ldc_I4, argument.Constants.Count);
	        argument.Generator.Emit(OpCodes.Ldelem, typeof(object));
	        argument.Constants.Add(value);
	    }
	    private static void AddCacheKey(ILEmitResolverBuilderContext argument, CallSiteServiceCacheKey key)
	    {
	        Debug.Assert(key.Type != null);
	        // new ServiceCacheKey(typeof(key.Type), key.Slot)
	        argument.Generator.Emit(OpCodes.Ldtoken, key.Type);
	        argument.Generator.Emit(OpCodes.Call, GetTypeFromHandleMethod);
	        argument.Generator.Emit(OpCodes.Ldc_I4, key.Slot);
	        argument.Generator.Emit(OpCodes.Newobj, CacheKeyCtor);
	    }
	    private ILEmitResolverBuilderRuntimeContext GenerateMethodBody(CallSiteService callSite, ILGenerator generator)
	    {
	        var context = new ILEmitResolverBuilderContext
	        {
	            Generator = generator,
	            Constants = null,
	            Factories = null
	        };
	        // if (scope.IsRootScope)
	        // {
	        //    return CallSiteRuntimeResolver.Instance.Resolve(callSite, scope);
	        // }
	        // var cacheKey = scopedCallSite.CacheKey;
	        // object sync;
	        // bool lockTaken;
	        // object result;
	        // try
	        // {
	        //    var resolvedServices = scope.ResolvedServices;
	        //    sync = scope.Sync;
	        //    Monitor.Enter(sync, ref lockTaken);
	        //    if (!resolvedServices.TryGetValue(cacheKey, out result)
	        //    {
	        //       result = [createvalue];
	        //       CaptureDisposable(result);
	        //       resolvedServices.Add(cacheKey, result);
	        //    }
	        // }
	        // finally
	        // {
	        //   if (lockTaken)
	        //   {
	        //      Monitor.Exit(sync);
	        //   }
	        // }
	        // return result;
	        if (callSite.Cache.Location == CallSiteResultCacheLocation.Scope)
	        {
	            LocalBuilder cacheKeyLocal = context.Generator.DeclareLocal(typeof(CallSiteServiceCacheKey));
	            LocalBuilder resolvedServicesLocal = context.Generator.DeclareLocal(typeof(IDictionary<CallSiteServiceCacheKey, object>));
	            LocalBuilder syncLocal = context.Generator.DeclareLocal(typeof(object));
	            LocalBuilder lockTakenLocal = context.Generator.DeclareLocal(typeof(bool));
	            LocalBuilder resultLocal = context.Generator.DeclareLocal(typeof(object));
	            Label skipCreationLabel = context.Generator.DefineLabel();
	            Label returnLabel = context.Generator.DefineLabel();
	            Label defaultLabel = context.Generator.DefineLabel();
	            // Check if scope IsRootScope
	            context.Generator.Emit(OpCodes.Ldarg_1);
	            context.Generator.Emit(OpCodes.Callvirt, ScopeIsRootScope);
	            context.Generator.Emit(OpCodes.Brfalse_S, defaultLabel);
	            context.Generator.Emit(OpCodes.Call, CallSiteRuntimeResolverInstanceField);
	            AddConstant(context, callSite);
	            context.Generator.Emit(OpCodes.Ldarg_1);
	            context.Generator.Emit(OpCodes.Callvirt, CallSiteRuntimeResolverResolveMethod);
	            context.Generator.Emit(OpCodes.Ret);
	            // Generate cache key
	            context.Generator.MarkLabel(defaultLabel);
	            AddCacheKey(context, callSite.Cache.Key);
	            // and store to local
	            context.Generator.Emit(OpCodes.Stloc, cacheKeyLocal);
	            context.Generator.BeginExceptionBlock();
	            // scope
	            context.Generator.Emit(OpCodes.Ldarg_1);
	            // .ResolvedServices
	            context.Generator.Emit(OpCodes.Callvirt, ResolvedServicesGetter);
	            // Store resolved services
	            context.Generator.Emit(OpCodes.Stloc, resolvedServicesLocal);
	            // scope
	            context.Generator.Emit(OpCodes.Ldarg_1);
	            // .Sync
	            context.Generator.Emit(OpCodes.Callvirt, ScopeLockGetter);
	            // Store syncLocal
	            context.Generator.Emit(OpCodes.Stloc, syncLocal);
	            // Load syncLocal
	            context.Generator.Emit(OpCodes.Ldloc, syncLocal);
	            // Load address of lockTaken
	            context.Generator.Emit(OpCodes.Ldloca, lockTakenLocal);
	            // Monitor.Enter
	            context.Generator.Emit(OpCodes.Call, ServiceLookupHelpers.MonitorEnterMethodInfo);
	            // Load resolved services
	            context.Generator.Emit(OpCodes.Ldloc, resolvedServicesLocal);
	            // Load cache key
	            context.Generator.Emit(OpCodes.Ldloc, cacheKeyLocal);
	            // Load address of result local
	            context.Generator.Emit(OpCodes.Ldloca, resultLocal);
	            // .TryGetValue
	            context.Generator.Emit(OpCodes.Callvirt, ServiceLookupHelpers.TryGetValueMethodInfo);
	            // Jump to the end if already in cache
	            context.Generator.Emit(OpCodes.Brtrue, skipCreationLabel);
	            // Create value
	            VisitCallSiteMain(callSite, context);
	            context.Generator.Emit(OpCodes.Stloc, resultLocal);
	            if (callSite.CaptureDisposable)
	            {
	                BeginCaptureDisposable(context);
	                context.Generator.Emit(OpCodes.Ldloc, resultLocal);
	                EndCaptureDisposable(context);
	                // Pop value returned by CaptureDisposable off the stack
	                generator.Emit(OpCodes.Pop);
	            }
	            // load resolvedServices
	            context.Generator.Emit(OpCodes.Ldloc, resolvedServicesLocal);
	            // load cache key
	            context.Generator.Emit(OpCodes.Ldloc, cacheKeyLocal);
	            // load value
	            context.Generator.Emit(OpCodes.Ldloc, resultLocal);
	            // .Add
	            context.Generator.Emit(OpCodes.Callvirt, ServiceLookupHelpers.AddMethodInfo);
	            context.Generator.MarkLabel(skipCreationLabel);
	            context.Generator.BeginFinallyBlock();
	            // load lockTaken
	            context.Generator.Emit(OpCodes.Ldloc, lockTakenLocal);
	            // return if not
	            context.Generator.Emit(OpCodes.Brfalse, returnLabel);
	            // Load syncLocal
	            context.Generator.Emit(OpCodes.Ldloc, syncLocal);
	            // Monitor.Exit
	            context.Generator.Emit(OpCodes.Call, ServiceLookupHelpers.MonitorExitMethodInfo);
	            context.Generator.MarkLabel(returnLabel);
	            context.Generator.EndExceptionBlock();
	            // load value
	            context.Generator.Emit(OpCodes.Ldloc, resultLocal);
	            // return
	            context.Generator.Emit(OpCodes.Ret);
	        }
	        else
	        {
	            VisitCallSite(callSite, context);
	            // return
	            context.Generator.Emit(OpCodes.Ret);
	        }
	        return new ILEmitResolverBuilderRuntimeContext
	        {
	            Constants = context.Constants?.ToArray(),
	            Factories = context.Factories?.ToArray()
	        };
	    }
	    private static void BeginCaptureDisposable(ILEmitResolverBuilderContext argument)
	    {
	        argument.Generator.Emit(OpCodes.Ldarg_1);
	    }
	    private static void EndCaptureDisposable(ILEmitResolverBuilderContext argument)
	    {
	        // When calling CaptureDisposable we expect callee and arguments to be on the stackcontext.Generator.BeginExceptionBlock
	        argument.Generator.Emit(OpCodes.Callvirt, ServiceLookupHelpers.CaptureDisposableMethodInfo);
	    }
	}
	internal sealed class ILEmitServiceProviderEngine : ServiceProviderEngine
	{
	    private readonly ILEmitResolverBuilderVisitor _expressionResolverBuilder;
	    public ILEmitServiceProviderEngine(ServiceProvider serviceProvider)
	    {
	        _expressionResolverBuilder = new ILEmitResolverBuilderVisitor(serviceProvider);
	    }
	    public override Func<ServiceProviderEngineScope, object> RealizeService(CallSiteService callSite)
	    {
	        return _expressionResolverBuilder.Build(callSite);
	    }
	}
	#endregion
	#region \Internal\ServiceLookup\Kind
	internal sealed class ConstantCallSite : CallSiteService
	{
	    private readonly Type serviceType;
	    internal object DefaultValue => Value;
	    public ConstantCallSite(Type serviceType, object defaultValue) : base(CallSiteResultCache.None)
	    {
	        this.serviceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
	        if (defaultValue != null && !serviceType.IsInstanceOfType(defaultValue))
	        {
	            throw new ArgumentException(Resources.GetConstantCantBeConvertedToServiceType(defaultValue.GetType(), serviceType));
	        }
	        Value = defaultValue;
	    }
	    public override Type ServiceType => serviceType;
	    public override Type ImplementationType => DefaultValue?.GetType() ?? serviceType;
	    public override CallSiteKind Kind { get; } = CallSiteKind.Constant;
	}
	internal sealed class ConstructorCallSite : CallSiteService
	{
	    internal ConstructorInfo ConstructorInfo { get; }
	    internal CallSiteService[] ParameterCallSites { get; }
	    public ConstructorCallSite(CallSiteResultCache cache, Type serviceType, ConstructorInfo constructorInfo) 
	        : this(cache, serviceType, constructorInfo, Array.Empty<CallSiteService>())
	    {
	    }
	    public ConstructorCallSite(CallSiteResultCache cache, Type serviceType, ConstructorInfo constructorInfo, CallSiteService[] parameterCallSites) 
	        : base(cache)
	    {
	        if (!serviceType.IsAssignableFrom(constructorInfo.DeclaringType))
	        {
	            throw new ArgumentException(Resources.GetImplementationTypeCantBeConvertedToServiceType(constructorInfo.DeclaringType, serviceType));
	        }
	        ServiceType = serviceType;
	        ConstructorInfo = constructorInfo;
	        ParameterCallSites = parameterCallSites;
	    }
	    public override Type ServiceType { get; }
	    public override Type ImplementationType => ConstructorInfo.DeclaringType ?? throw new NullReferenceException();
	    public override CallSiteKind Kind { get; } = CallSiteKind.Constructor;
	}
	internal sealed class EnumerableCallSite : CallSiteService
	{
	    internal Type ItemType { get; }
	    internal CallSiteService[] ServiceCallSites { get; }
	    public EnumerableCallSite(CallSiteResultCache cache, Type itemType, CallSiteService[] serviceCallSites) : base(cache)
	    {
	        ItemType = itemType;
	        ServiceCallSites = serviceCallSites;
	    }
	    public override Type ServiceType => typeof(IEnumerable<>).MakeGenericType(ItemType);
	    public override Type ImplementationType => ItemType.MakeArrayType();
	    public override CallSiteKind Kind { get; } = CallSiteKind.Enumerable;
	}
	internal sealed class FactoryCallSite : CallSiteService
	{
	    public FactoryCallSite(CallSiteResultCache cache, Type serviceType, Func<IServiceProvider, object> factory) : base(cache)
	    {
	        Factory = factory;
	        ServiceType = serviceType;
	    }
	    public Func<IServiceProvider, object> Factory { get; }
	    public override Type ServiceType { get; }
	    public override Type ImplementationType => null;
	    public override CallSiteKind Kind { get; } = CallSiteKind.Factory;
	}
	internal sealed class ServiceProviderCallSite : CallSiteService
	{
	    public ServiceProviderCallSite() : base(CallSiteResultCache.None)
	    {
	    }
	    public override Type ServiceType { get; } = typeof(IServiceProvider);
	    public override Type ImplementationType { get; } = typeof(ServiceProvider);
	    public override CallSiteKind Kind { get; } = CallSiteKind.ServiceProvider;
	}
	#endregion
	#region \Internal\ServiceLookup\Visitors
	internal abstract class CallSiteVisitor<TArgument, TResult>
	{
	    private readonly CallSiteStackGuard stackGuard;
	    protected CallSiteVisitor() => stackGuard = new CallSiteStackGuard();
	    protected virtual TResult VisitCallSite(CallSiteService callSite, TArgument argument)
	    {
	        if (!stackGuard.TryEnterOnCurrentStack())
	        {
	            return stackGuard.RunOnEmptyStack((c, a) => VisitCallSite(c, a), callSite, argument);
	        }
	        return callSite.Cache.Location switch
	        {
	            CallSiteResultCacheLocation.Root => VisitRootCache(callSite, argument),
	            CallSiteResultCacheLocation.Scope => VisitScopeCache(callSite, argument),
	            CallSiteResultCacheLocation.Dispose => VisitDisposeCache(callSite, argument),
	            CallSiteResultCacheLocation.None => VisitNoCache(callSite, argument),
	            _ => throw new ArgumentOutOfRangeException()
	        };
	    }
	    protected virtual TResult VisitCallSiteMain(CallSiteService callSite, TArgument argument)
	    {
	        return callSite.Kind switch
	        {
	            CallSiteKind.Factory => VisitFactory((FactoryCallSite)callSite, argument),
	            CallSiteKind.Enumerable => VisitEnumerable((EnumerableCallSite)callSite, argument),
	            CallSiteKind.Constructor => VisitConstructor((ConstructorCallSite)callSite, argument),
	            CallSiteKind.Constant => VisitConstant((ConstantCallSite)callSite, argument),
	            CallSiteKind.ServiceProvider => VisitServiceProvider((ServiceProviderCallSite)callSite, argument),
	            _ => throw new NotSupportedException(Resources.GetCallSiteTypeNotSupportedExceptionMessage(callSite.GetType()))
	        };
	    }
	    protected virtual TResult VisitNoCache(CallSiteService callSite, TArgument argument) => VisitCallSiteMain(callSite, argument);
	    protected virtual TResult VisitDisposeCache(CallSiteService callSite, TArgument argument) => VisitCallSiteMain(callSite, argument);
	    protected virtual TResult VisitRootCache(CallSiteService callSite, TArgument argument) => VisitCallSiteMain(callSite, argument);
	    protected virtual TResult VisitScopeCache(CallSiteService callSite, TArgument argument) => VisitCallSiteMain(callSite, argument);
	    protected abstract TResult VisitConstructor(ConstructorCallSite constructorCallSite, TArgument argument);
	    protected abstract TResult VisitConstant(ConstantCallSite constantCallSite, TArgument argument);
	    protected abstract TResult VisitServiceProvider(ServiceProviderCallSite serviceProviderCallSite, TArgument argument);
	    protected abstract TResult VisitEnumerable(EnumerableCallSite enumerableCallSite, TArgument argument);
	    protected abstract TResult VisitFactory(FactoryCallSite factoryCallSite, TArgument argument);
	}
	internal sealed class CallSiteExpressionResolverBuilderVisitor : CallSiteVisitor<object?, Expression>
	{
	    private static readonly ParameterExpression ScopeParameter = Expression.Parameter(typeof(ServiceProviderEngineScope));
	    private static readonly ParameterExpression ResolvedServices = Expression.Variable(typeof(IDictionary<CallSiteServiceCacheKey, object>), ScopeParameter.Name + "resolvedServices");
	    private static readonly ParameterExpression Sync = Expression.Variable(typeof(object), ScopeParameter.Name + "sync");
	    private static readonly BinaryExpression ResolvedServicesVariableAssignment =
	        Expression.Assign(ResolvedServices,
	            Expression.Property(
	                ScopeParameter,
	                typeof(ServiceProviderEngineScope).GetProperty(nameof(ServiceProviderEngineScope.ResolvedServices), BindingFlags.Instance | BindingFlags.NonPublic)!));
	    private static readonly BinaryExpression SyncVariableAssignment =
	        Expression.Assign(Sync,
	            Expression.Property(
	                ScopeParameter,
	                typeof(ServiceProviderEngineScope).GetProperty(nameof(ServiceProviderEngineScope.Sync), BindingFlags.Instance | BindingFlags.NonPublic)!));
	    private static readonly ParameterExpression CaptureDisposableParameter = Expression.Parameter(typeof(object));
	    private static readonly LambdaExpression CaptureDisposable = Expression.Lambda(
	                Expression.Call(ScopeParameter, ServiceLookupHelpers.CaptureDisposableMethodInfo, CaptureDisposableParameter),
	                CaptureDisposableParameter);
	    private static readonly ConstantExpression CallSiteRuntimeResolverInstanceExpression = Expression.Constant(
	        CallSiteRuntimeResolverVisitor.Instance,
	        typeof(CallSiteRuntimeResolverVisitor));
	    private readonly ServiceProviderEngineScope _rootScope;
	    private readonly ConcurrentDictionary<CallSiteServiceCacheKey, Func<ServiceProviderEngineScope, object>> _scopeResolverCache;
	    private readonly Func<CallSiteServiceCacheKey, CallSiteService, Func<ServiceProviderEngineScope, object>> _buildTypeDelegate;
	    public CallSiteExpressionResolverBuilderVisitor(ServiceProvider serviceProvider)
	    {
	        _rootScope = serviceProvider.Root;
	        _scopeResolverCache = new ConcurrentDictionary<CallSiteServiceCacheKey, Func<ServiceProviderEngineScope, object>>();
	        _buildTypeDelegate = (key, cs) => BuildNoCache(cs);
	    }
	    public Func<ServiceProviderEngineScope, object> Build(CallSiteService callSite)
	    {
	        // Only scope methods are cached
	        if (callSite.Cache.Location == CallSiteResultCacheLocation.Scope)
	        {
	            return _scopeResolverCache.GetOrAdd(callSite.Cache.Key, _buildTypeDelegate, callSite);
	        }
	        return BuildNoCache(callSite);
	    }
	    public Func<ServiceProviderEngineScope, object> BuildNoCache(CallSiteService callSite)
	    {
	        Expression<Func<ServiceProviderEngineScope, object>> expression = BuildExpression(callSite);
	        ServiceEventSource.Log.ExpressionTreeGenerated(_rootScope.RootProvider, callSite.ServiceType, expression);
	        return expression.Compile();
	    }
	    private Expression<Func<ServiceProviderEngineScope, object>> BuildExpression(CallSiteService callSite)
	    {
	        if (callSite.Cache.Location == CallSiteResultCacheLocation.Scope)
	        {
	            return Expression.Lambda<Func<ServiceProviderEngineScope, object>>(
	                Expression.Block(
	                    new[] { ResolvedServices, Sync },
	                    ResolvedServicesVariableAssignment,
	                    SyncVariableAssignment,
	                    BuildScopedExpression(callSite)),
	                ScopeParameter);
	        }
	        return Expression.Lambda<Func<ServiceProviderEngineScope, object>>(
	            Convert(VisitCallSite(callSite, null), typeof(object), forceValueTypeConversion: true),
	            ScopeParameter);
	    }
	    protected override Expression VisitRootCache(CallSiteService singletonCallSite, object? context)
	    {
	        return Expression.Constant(CallSiteRuntimeResolverVisitor.Instance.Resolve(singletonCallSite, _rootScope));
	    }
	    protected override Expression VisitConstant(ConstantCallSite constantCallSite, object? context)
	    {
	        return Expression.Constant(constantCallSite.DefaultValue);
	    }
	    protected override Expression VisitServiceProvider(ServiceProviderCallSite serviceProviderCallSite, object? context)
	    {
	        return ScopeParameter;
	    }
	    protected override Expression VisitFactory(FactoryCallSite factoryCallSite, object? context)
	    {
	        return Expression.Invoke(Expression.Constant(factoryCallSite.Factory), ScopeParameter);
	    }
	    protected override Expression VisitEnumerable(EnumerableCallSite callSite, object? context)
	    {
	        [UnconditionalSuppressMessage("AotAnalysis", "IL3050:RequiresDynamicCode",
	            Justification = "VerifyAotCompatibility ensures elementType is not a ValueType")]
	        static MethodInfo GetArrayEmptyMethodInfo(Type elementType)
	        {
	            Debug.Assert(!RuntimeFeature.IsDynamicCodeSupported || !elementType.IsValueType, "VerifyAotCompatibility=true will throw during building the IEnumerableCallSite if elementType is a ValueType.");
	            return ServiceLookupHelpers.GetArrayEmptyMethodInfo(elementType);
	        }
	        if (callSite.ServiceCallSites.Length == 0)
	        {
	            return Expression.Constant(
	                GetArrayEmptyMethodInfo(callSite.ItemType)
	                .Invoke(obj: null, parameters: Array.Empty<object>()));
	        }
	        return Expression.NewArrayInit(
	            callSite.ItemType,
	            callSite.ServiceCallSites.Select(cs =>
	                Convert(
	                    VisitCallSite(cs, context),
	                    callSite.ItemType)));
	    }
	    protected override Expression VisitDisposeCache(CallSiteService callSite, object? context)
	    {
	        // Elide calls to GetCaptureDisposable if the implementation type isn't disposable
	        return TryCaptureDisposable(
	            callSite,
	            ScopeParameter,
	            VisitCallSiteMain(callSite, context));
	    }
	    private static Expression TryCaptureDisposable(CallSiteService callSite, ParameterExpression scope, Expression service)
	    {
	        if (!callSite.CaptureDisposable)
	        {
	            return service;
	        }
	        return Expression.Invoke(GetCaptureDisposable(scope), service);
	    }
	    protected override Expression VisitConstructor(ConstructorCallSite callSite, object? context)
	    {
	        ParameterInfo[] parameters = callSite.ConstructorInfo.GetParameters();
	        Expression[] parameterExpressions;
	        if (callSite.ParameterCallSites.Length == 0)
	        {
	            parameterExpressions = Array.Empty<Expression>();
	        }
	        else
	        {
	            parameterExpressions = new Expression[callSite.ParameterCallSites.Length];
	            for (int i = 0; i < parameterExpressions.Length; i++)
	            {
	                parameterExpressions[i] = Convert(VisitCallSite(callSite.ParameterCallSites[i], context), parameters[i].ParameterType);
	            }
	        }
	        Expression expression = Expression.New(callSite.ConstructorInfo, parameterExpressions);
	        if (callSite.ImplementationType!.IsValueType)
	        {
	            expression = Expression.Convert(expression, typeof(object));
	        }
	        return expression;
	    }
	    private static Expression Convert(Expression expression, Type type, bool forceValueTypeConversion = false)
	    {
	        // Don't convert if the expression is already assignable
	        if (type.IsAssignableFrom(expression.Type)
	            && (!expression.Type.IsValueType || !forceValueTypeConversion))
	        {
	            return expression;
	        }
	        return Expression.Convert(expression, type);
	    }
	    protected override Expression VisitScopeCache(CallSiteService callSite, object? context)
	    {
	        Func<ServiceProviderEngineScope, object> lambda = Build(callSite);
	        return Expression.Invoke(Expression.Constant(lambda), ScopeParameter);
	    }
	    // Move off the main stack
	    private ConditionalExpression BuildScopedExpression(CallSiteService callSite)
	    {
	        ConstantExpression callSiteExpression = Expression.Constant(
	            callSite,
	            typeof(CallSiteService));
	        // We want to directly use the callsite value if it's set and the scope is the root scope.
	        // We've already called into the RuntimeResolver and pre-computed any singletons or root scope
	        // Avoid the compilation for singletons (or promoted singletons)
	        MethodCallExpression resolveRootScopeExpression = Expression.Call(
	            CallSiteRuntimeResolverInstanceExpression,
	            ServiceLookupHelpers.ResolveCallSiteAndScopeMethodInfo,
	            callSiteExpression,
	            ScopeParameter);
	        ConstantExpression keyExpression = Expression.Constant(
	            callSite.Cache.Key,
	            typeof(CallSiteServiceCacheKey));
	        ParameterExpression resolvedVariable = Expression.Variable(typeof(object), "resolved");
	        ParameterExpression resolvedServices = ResolvedServices;
	        MethodCallExpression tryGetValueExpression = Expression.Call(
	            resolvedServices,
	            ServiceLookupHelpers.TryGetValueMethodInfo,
	            keyExpression,
	            resolvedVariable);
	        Expression captureDisposible = TryCaptureDisposable(callSite, ScopeParameter, VisitCallSiteMain(callSite, null));
	        BinaryExpression assignExpression = Expression.Assign(
	            resolvedVariable,
	            captureDisposible);
	        MethodCallExpression addValueExpression = Expression.Call(
	            resolvedServices,
	            ServiceLookupHelpers.AddMethodInfo,
	            keyExpression,
	            resolvedVariable);
	        BlockExpression blockExpression = Expression.Block(
	            typeof(object),
	            new[]
	            {
	                    resolvedVariable
	            },
	            Expression.IfThen(
	                Expression.Not(tryGetValueExpression),
	                Expression.Block(
	                    assignExpression,
	                    addValueExpression)),
	            resolvedVariable);
	        // The C# compiler would copy the lock object to guard against mutation.
	        // We don't, since we know the lock object is readonly.
	        ParameterExpression lockWasTaken = Expression.Variable(typeof(bool), "lockWasTaken");
	        ParameterExpression sync = Sync;
	        MethodCallExpression monitorEnter = Expression.Call(ServiceLookupHelpers.MonitorEnterMethodInfo, sync, lockWasTaken);
	        MethodCallExpression monitorExit = Expression.Call(ServiceLookupHelpers.MonitorExitMethodInfo, sync);
	        BlockExpression tryBody = Expression.Block(monitorEnter, blockExpression);
	        ConditionalExpression finallyBody = Expression.IfThen(lockWasTaken, monitorExit);
	        return Expression.Condition(
	                Expression.Property(
	                    ScopeParameter,
	                    typeof(ServiceProviderEngineScope)
	                        .GetProperty(nameof(ServiceProviderEngineScope.IsRootScope), BindingFlags.Instance | BindingFlags.Public)!),
	                resolveRootScopeExpression,
	                Expression.Block(
	                    typeof(object),
	                    new[] { lockWasTaken },
	                    Expression.TryFinally(tryBody, finallyBody))
	            );
	    }
	    public static Expression GetCaptureDisposable(ParameterExpression scope)
	    {
	        if (scope != ScopeParameter)
	        {
	            throw new NotSupportedException(Resources.GetCaptureDisposableNotSupported);
	        }
	        return CaptureDisposable;
	    }
	}
	internal sealed class CallSiteJsonFormatterVisitor : CallSiteVisitor<CallSiteJsonFormatterVisitor.CallSiteFormatterContext, object>
	{
	    internal static CallSiteJsonFormatterVisitor Instance = new CallSiteJsonFormatterVisitor();
	    private CallSiteJsonFormatterVisitor()
	    {
	    }
	    public string Format(CallSiteService callSite)
	    {
	        var stringBuilder = new StringBuilder();
	        var context = new CallSiteFormatterContext(stringBuilder, 0, new HashSet<CallSiteService>());
	        VisitCallSite(callSite, context);
	        return stringBuilder.ToString();
	    }
	    protected override object VisitConstructor(ConstructorCallSite constructorCallSite, CallSiteFormatterContext argument)
	    {
	        argument.WriteProperty("implementationType", constructorCallSite.ImplementationType);
	        if (constructorCallSite.ParameterCallSites.Length > 0)
	        {
	            argument.StartProperty("arguments");
	            CallSiteFormatterContext childContext = argument.StartArray();
	            foreach (CallSiteService parameter in constructorCallSite.ParameterCallSites)
	            {
	                childContext.StartArrayItem();
	                VisitCallSite(parameter, childContext);
	            }
	            argument.EndArray();
	        }
	        return null;
	    }
	    protected override object VisitCallSiteMain(CallSiteService callSite, CallSiteFormatterContext argument)
	    {
	        if (argument.ShouldFormat(callSite))
	        {
	            CallSiteFormatterContext childContext = argument.StartObject();
	            childContext.WriteProperty("serviceType", callSite.ServiceType);
	            childContext.WriteProperty("kind", callSite.Kind);
	            childContext.WriteProperty("cache", callSite.Cache.Location);
	            base.VisitCallSiteMain(callSite, childContext);
	            argument.EndObject();
	        }
	        else
	        {
	            CallSiteFormatterContext childContext = argument.StartObject();
	            childContext.WriteProperty("ref", callSite.ServiceType);
	            argument.EndObject();
	        }
	        return null;
	    }
	    protected override object VisitConstant(ConstantCallSite constantCallSite, CallSiteFormatterContext argument)
	    {
	        argument.WriteProperty("value", constantCallSite.DefaultValue ?? "");
	        return null;
	    }
	    protected override object VisitServiceProvider(ServiceProviderCallSite serviceProviderCallSite, CallSiteFormatterContext argument)
	    {
	        return null;
	    }
	    protected override object VisitEnumerable(EnumerableCallSite enumerableCallSite, CallSiteFormatterContext argument)
	    {
	        argument.WriteProperty("itemType", enumerableCallSite.ItemType);
	        argument.WriteProperty("size", enumerableCallSite.ServiceCallSites.Length);
	        if (enumerableCallSite.ServiceCallSites.Length > 0)
	        {
	            argument.StartProperty("items");
	            CallSiteFormatterContext childContext = argument.StartArray();
	            foreach (CallSiteService item in enumerableCallSite.ServiceCallSites)
	            {
	                childContext.StartArrayItem();
	                VisitCallSite(item, childContext);
	            }
	            argument.EndArray();
	        }
	        return null;
	    }
	    protected override object VisitFactory(FactoryCallSite factoryCallSite, CallSiteFormatterContext argument)
	    {
	        argument.WriteProperty("method", factoryCallSite.Factory.Method);
	        return null;
	    }
	    internal struct CallSiteFormatterContext
	    {
	        private readonly HashSet<CallSiteService> _processedCallSites;
	        public CallSiteFormatterContext(StringBuilder builder, int offset, HashSet<CallSiteService> processedCallSites)
	        {
	            Builder = builder;
	            Offset = offset;
	            _processedCallSites = processedCallSites;
	            _firstItem = true;
	        }
	        private bool _firstItem;
	        public int Offset { get; }
	        public StringBuilder Builder { get; }
	        public bool ShouldFormat(CallSiteService serviceCallSite)
	        {
	            return _processedCallSites.Add(serviceCallSite);
	        }
	        public CallSiteFormatterContext IncrementOffset()
	        {
	            return new CallSiteFormatterContext(Builder, Offset + 4, _processedCallSites)
	            {
	                _firstItem = true
	            };
	        }
	        public CallSiteFormatterContext StartObject()
	        {
	            Builder.Append('{');
	            return IncrementOffset();
	        }
	        public void EndObject()
	        {
	            Builder.Append('}');
	        }
	        public void StartProperty(string name)
	        {
	            if (!_firstItem)
	            {
	                Builder.Append(',');
	            }
	            else
	            {
	                _firstItem = false;
	            }
	            Builder.AppendFormat("\"{0}\":", name);
	        }
	        public void StartArrayItem()
	        {
	            if (!_firstItem)
	            {
	                Builder.Append(',');
	            }
	            else
	            {
	                _firstItem = false;
	            }
	        }
	        public void WriteProperty(string name, object value)
	        {
	            StartProperty(name);
	            if (value != null)
	            {
	                Builder.AppendFormat(" \"{0}\"", value);
	            }
	            else
	            {
	                Builder.Append("null");
	            }
	        }
	        public CallSiteFormatterContext StartArray()
	        {
	            Builder.Append('[');
	            return IncrementOffset();
	        }
	        public void EndArray()
	        {
	            Builder.Append(']');
	        }
	    }
	}
	internal sealed class CallSiteRuntimeResolverVisitor : CallSiteVisitor<CallSiteRuntimeResolverVisitor.CallSiteRuntimeResolverContext, object>
	{
	    public static CallSiteRuntimeResolverVisitor Instance { get; } = new();
	    private CallSiteRuntimeResolverVisitor()
	    {
	    }
	    public object Resolve(CallSiteService callSite, ServiceProviderEngineScope scope)
	    {
	        // Fast path to avoid virtual calls if we already have the cached value in the root scope
	        if (scope.IsRootScope && callSite.Value is object cached)
	        {
	            return cached;
	        }
	        return VisitCallSite(callSite, new CallSiteRuntimeResolverContext
	        {
	            Scope = scope
	        });
	    }
	    private object VisitCache(CallSiteService callSite, CallSiteRuntimeResolverContext context, ServiceProviderEngineScope serviceProviderEngine, CallSiteRuntimeResolverLock lockType)
	    {
	        bool lockTaken = false;
	        object sync = serviceProviderEngine.Sync;
	        Dictionary<CallSiteServiceCacheKey, object> resolvedServices = serviceProviderEngine.ResolvedServices;
	        // Taking locks only once allows us to fork resolution process
	        // on another thread without causing the deadlock because we
	        // always know that we are going to wait the other thread to finish before
	        // releasing the lock
	        if ((context.AcquiredLocks & lockType) == 0)
	        {
	            Monitor.Enter(sync, ref lockTaken);
	        }
	        try
	        {
	            // Note: This method has already taken lock by the caller for resolution and access synchronization.
	            // For scoped: takes a dictionary as both a resolution lock and a dictionary access lock.
	            if (resolvedServices.TryGetValue(callSite.Cache.Key, out object resolved))
	            {
	                return resolved;
	            }
	            resolved = VisitCallSiteMain(callSite, new CallSiteRuntimeResolverContext
	            {
	                Scope = serviceProviderEngine,
	                AcquiredLocks = context.AcquiredLocks | lockType
	            });
	            serviceProviderEngine.CaptureDisposable(resolved);
	            resolvedServices.Add(callSite.Cache.Key, resolved);
	            return resolved;
	        }
	        finally
	        {
	            if (lockTaken)
	            {
	                Monitor.Exit(sync);
	            }
	        }
	    }
	    #region Visitor Base Overrides
	    protected override object VisitDisposeCache(CallSiteService transientCallSite, CallSiteRuntimeResolverContext context)
	    {
	        return context.Scope.CaptureDisposable(VisitCallSiteMain(transientCallSite, context));
	    }
	    protected override object VisitConstructor(ConstructorCallSite constructorCallSite, CallSiteRuntimeResolverContext context)
	    {
	        object[] parameterValues;
	        if (constructorCallSite.ParameterCallSites.Length == 0)
	        {
	            parameterValues = Array.Empty<object>();
	        }
	        else
	        {
	            parameterValues = new object[constructorCallSite.ParameterCallSites.Length];
	            for (int index = 0; index < parameterValues.Length; index++)
	            {
	                parameterValues[index] = VisitCallSite(constructorCallSite.ParameterCallSites[index], context);
	            }
	        }
	        return constructorCallSite.ConstructorInfo.Invoke(BindingFlags.DoNotWrapExceptions, binder: null, parameters: parameterValues, culture: null);
	    }
	    protected override object VisitRootCache(CallSiteService callSite, CallSiteRuntimeResolverContext context)
	    {
	        if (callSite.Value is object value)
	        {
	            // Value already calculated, return it directly
	            return value;
	        }
	        var lockType = CallSiteRuntimeResolverLock.Root;
	        ServiceProviderEngineScope serviceProviderEngine = context.Scope.RootProvider.Root;
	        lock (callSite)
	        {
	            // Lock the callsite and check if another thread already cached the value
	            if (callSite.Value is object resolved)
	            {
	                return resolved;
	            }
	            resolved = VisitCallSiteMain(callSite, new CallSiteRuntimeResolverContext
	            {
	                Scope = serviceProviderEngine,
	                AcquiredLocks = context.AcquiredLocks | lockType
	            });
	            serviceProviderEngine.CaptureDisposable(resolved);
	            callSite.Value = resolved;
	            return resolved;
	        }
	    }
	    protected override object VisitScopeCache(CallSiteService callSite, CallSiteRuntimeResolverContext context)
	    {
	        // Check if we are in the situation where scoped service was promoted to singleton
	        // and we need to lock the root
	        return context.Scope.IsRootScope ?
	            VisitRootCache(callSite, context) :
	            VisitCache(callSite, context, context.Scope, CallSiteRuntimeResolverLock.Scope);
	    }
	    protected override object VisitConstant(ConstantCallSite constantCallSite, CallSiteRuntimeResolverContext context)
	    {
	        return constantCallSite.DefaultValue;
	    }
	    protected override object VisitServiceProvider(ServiceProviderCallSite serviceProviderCallSite, CallSiteRuntimeResolverContext context)
	    {
	        return context.Scope;
	    }
	    protected override object VisitEnumerable(EnumerableCallSite enumerableCallSite, CallSiteRuntimeResolverContext context)
	    {
	        var array = Array.CreateInstance(
	            enumerableCallSite.ItemType,
	            enumerableCallSite.ServiceCallSites.Length);
	        for (int index = 0; index < enumerableCallSite.ServiceCallSites.Length; index++)
	        {
	            object value = VisitCallSite(enumerableCallSite.ServiceCallSites[index], context);
	            array.SetValue(value, index);
	        }
	        return array;
	    }
	    protected override object VisitFactory(FactoryCallSite factoryCallSite, CallSiteRuntimeResolverContext context)
	    {
	        return factoryCallSite.Factory(context.Scope);
	    }
	    #endregion
	    [Flags]
	    internal enum CallSiteRuntimeResolverLock
	    {
	        Scope = 1,
	        Root = 2
	    }
	    internal struct CallSiteRuntimeResolverContext
	    {
	        public ServiceProviderEngineScope Scope { get; set; }
	        public CallSiteRuntimeResolverLock AcquiredLocks { get; set; }
	    }
	}
	internal sealed class CallSiteValidatorVisitor : CallSiteVisitor<CallSiteValidatorVisitor.CallSiteValidatorState, Type?>
	{
	    // Keys are services being resolved via GetService, values - first scoped service in their call site tree
	    private readonly ConcurrentDictionary<Type, Type> scopedServices = new();
	    public void ValidateCallSite(CallSiteService callSite)
	    {
	        var scoped = VisitCallSite(callSite, default);
	        if (scoped != null)
	        {
	            scopedServices[callSite.ServiceType] = scoped;
	        }
	    }
	    public void ValidateResolution(Type serviceType, IServiceScope scope, IServiceScope rootScope)
	    {
	        if (ReferenceEquals(scope, rootScope) && scopedServices.TryGetValue(serviceType, out Type? scopedService))
	        {
	            if (serviceType == scopedService)
	            {
	                throw new InvalidOperationException(
	                    Resources.GetDirectScopedResolvedFromRootExceptionMessage( 
	                        serviceType,
	                        nameof(ServiceLifetime.Scoped).ToLowerInvariant()));
	            }
	            throw new InvalidOperationException(
	                Resources.GetScopedResolvedFromRootExceptionMessage(
	                    serviceType,
	                    scopedService,
	                    nameof(ServiceLifetime.Scoped).ToLowerInvariant()));
	        }
	    }
	    protected override Type? VisitConstructor(ConstructorCallSite constructorCallSite, CallSiteValidatorState state)
	    {
	        Type result = null;
	        foreach (CallSiteService parameterCallSite in constructorCallSite.ParameterCallSites)
	        {
	            Type scoped = VisitCallSite(parameterCallSite, state);
	            if (result == null)
	            {
	                result = scoped;
	            }
	        }
	        return result;
	    }
	    protected override Type VisitEnumerable(EnumerableCallSite enumerableCallSite, CallSiteValidatorState state)
	    {
	        Type result = null;
	        foreach (CallSiteService serviceCallSite in enumerableCallSite.ServiceCallSites)
	        {
	            Type scoped = VisitCallSite(serviceCallSite, state);
	            if (result == null)
	            {
	                result = scoped;
	            }
	        }
	        return result;
	    }
	    protected override Type VisitRootCache(CallSiteService singletonCallSite, CallSiteValidatorState state)
	    {
	        state.Singleton = singletonCallSite;
	        return VisitCallSiteMain(singletonCallSite, state);
	    }
	    protected override Type VisitScopeCache(CallSiteService scopedCallSite, CallSiteValidatorState state)
	    {
	        // We are fine with having ServiceScopeService requested by singletons
	        if (scopedCallSite.ServiceType == typeof(IServiceScopeFactory))
	        {
	            return null;
	        }
	        if (state.Singleton != null)
	        {
	            throw new InvalidOperationException(Resources.GetScopedInSingletonExceptionMessage(
	                scopedCallSite.ServiceType,
	                state.Singleton.ServiceType,
	                nameof(ServiceLifetime.Scoped).ToLowerInvariant(),
	                nameof(ServiceLifetime.Singleton).ToLowerInvariant()
	                ));
	        }
	        VisitCallSiteMain(scopedCallSite, state);
	        return scopedCallSite.ServiceType;
	    }
	    protected override Type VisitConstant(ConstantCallSite constantCallSite, CallSiteValidatorState state) => null;
	    protected override Type VisitServiceProvider(ServiceProviderCallSite serviceProviderCallSite, CallSiteValidatorState state) => null;
	    protected override Type VisitFactory(FactoryCallSite factoryCallSite, CallSiteValidatorState state) => null;
	    internal struct CallSiteValidatorState
	    {
	        public CallSiteService Singleton { get; set; }
	    }
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
	#region \Properties
	    internal partial class Resources
	    {
	        internal static string GetTryAddIndistinguishableTypeToEnumerableExceptionMessage(Type p1, Type p2)
	        {
	            return Format(TryAddIndistinguishableTypeToEnumerable, p1, p2);
	        }
	        internal static string GetCallSiteTypeNotSupportedExceptionMessage(Type p1)
	        {
	            return Format(CallSiteTypeNotSupported, p1);
	        }
	        internal static string GetNoServiceRegisteredExceptionMessage(Type p1)
	        {
	            return Format(NoServiceRegistered, p1);
	        }
	        internal static string GetAsyncDisposableServiceDisposeExceptionMessage(string p1)
	        {
	            return Format(AsyncDisposableServiceDispose, p1);
	        }
	        internal static string GetDirectScopedResolvedFromRootExceptionMessage(Type t1, string t2)
	        {
	            return Format(DirectScopedResolvedFromRootException, t1, t2);
	        }
	        internal static string GetScopedResolvedFromRootExceptionMessage(Type t1, Type t2, string t3)
	        {
	            return Format(ScopedResolvedFromRootException, t1, t2, t3);
	        }
	        internal static string GetScopedInSingletonExceptionMessage(Type t1, Type t2, string t3, string t4)
	        {
	            return Format(ScopedInSingletonException, t1, t2, t3, t4);
	        }
	        internal static string GetCannotResolveServiceExceptionMessage(Type parameterType, Type implementationType)
	        {
	            return Format(CannotResolveService, parameterType, implementationType);
	        }
	        internal static string GetUnableToActivateTypeExceptionMessage(Type implementationType)
	        {
	            return Format(UnableToActivateTypeException, implementationType);
	        }
	        internal static string GetAmbiguousConstructorExceptionMessage(Type implementationType)
	        {
	            return Format(AmbiguousConstructorException, implementationType);
	        }
	        internal static string GetNoConstructorMatchExceptionMessage(Type implementationType)
	        {
	            return Format(NoConstructorMatch, implementationType);
	        }
	        internal static string GetTrimmingAnnotationsDoNotMatch_NewConstraintExceptionMessage(Type implementationType, Type serviceType)
	        {
	            return Format(TrimmingAnnotationsDoNotMatch_NewConstraint, implementationType.FullName, serviceType.FullName);
	        }
	        internal static string GetTrimmingAnnotationsDoNotMatchExceptionMessage(Type implementationType, Type serviceType)
	        {
	            return Format(TrimmingAnnotationsDoNotMatch, implementationType.FullName, serviceType.FullName);
	        }
	        internal static string GetArityOfOpenGenericServiceNotEqualArityOfOpenGenericImplementationExceptionMessage(Type implementationType, Type serviceType)
	        {
	            return Format(ArityOfOpenGenericServiceNotEqualArityOfOpenGenericImplementation, implementationType, serviceType);
	        }
	        internal static string GetOpenGenericServiceRequiresOpenGenericImplementationExceptionMessage(Type serviceType)
	        {
	            return Format(OpenGenericServiceRequiresOpenGenericImplementation, serviceType);
	        }
	        internal static string GetTypeCannotBeActivatedExceptionMessage(Type implementationType, Type serviceType)
	        {
	            return Format(TypeCannotBeActivated, implementationType, serviceType);
	        }
	        internal static string GetConstantCantBeConvertedToServiceType(Type defaultType, Type serviceType)
	        {
	            return Format(ConstantCantBeConvertedToServiceType, defaultType, serviceType);
	        }
	        internal static string GetImplementationTypeCantBeConvertedToServiceType(Type implementationType, Type serviceType)
	        {
	            return Format(ImplementationTypeCantBeConvertedToServiceType, implementationType, serviceType);
	        }
	        internal static string GetCircularDependencyExceptionMessage(string typeName)
	        {
	            return Format(CircularDependencyException, typeName);
	        }
	    }
	    // This class was auto-generated by the StronglyTypedResourceBuilder
	    // class via a tool like ResGen or Visual Studio.
	    // To add or remove a member, edit your .ResX file then rerun ResGen
	    // with the /str option, or rebuild your VS project.
	    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
	    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
	    internal partial class Resources {
	        private static global::System.Resources.ResourceManager resourceMan;
	        private static global::System.Globalization.CultureInfo resourceCulture;
	        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
	        internal Resources() {
	        }
	        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
	        internal static global::System.Resources.ResourceManager ResourceManager {
	            get {
	                if (object.ReferenceEquals(resourceMan, null)) {
	                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Assimalign.Cohesion.DependencyInjection.Properties.Resources", typeof(Resources).Assembly);
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
	        internal static string AmbiguousConstructorException {
	            get {
	                return ResourceManager.GetString("AmbiguousConstructorException", resourceCulture);
	            }
	        }
	        internal static string AmbiguousConstructorMatch {
	            get {
	                return ResourceManager.GetString("AmbiguousConstructorMatch", resourceCulture);
	            }
	        }
	        internal static string ArityOfOpenGenericServiceNotEqualArityOfOpenGenericImplementation {
	            get {
	                return ResourceManager.GetString("ArityOfOpenGenericServiceNotEqualArityOfOpenGenericImplementation", resourceCulture);
	            }
	        }
	        internal static string AsyncDisposableServiceDispose {
	            get {
	                return ResourceManager.GetString("AsyncDisposableServiceDispose", resourceCulture);
	            }
	        }
	        internal static string CallSiteTypeNotSupported {
	            get {
	                return ResourceManager.GetString("CallSiteTypeNotSupported", resourceCulture);
	            }
	        }
	        internal static string CannotCreateAbstractClasses {
	            get {
	                return ResourceManager.GetString("CannotCreateAbstractClasses", resourceCulture);
	            }
	        }
	        internal static string CannotResolveService {
	            get {
	                return ResourceManager.GetString("CannotResolveService", resourceCulture);
	            }
	        }
	        internal static string CannotResolveService1 {
	            get {
	                return ResourceManager.GetString("CannotResolveService1", resourceCulture);
	            }
	        }
	        internal static string CircularDependencyException {
	            get {
	                return ResourceManager.GetString("CircularDependencyException", resourceCulture);
	            }
	        }
	        internal static string ConstantCantBeConvertedToServiceType {
	            get {
	                return ResourceManager.GetString("ConstantCantBeConvertedToServiceType", resourceCulture);
	            }
	        }
	        internal static string CtorNotLocated {
	            get {
	                return ResourceManager.GetString("CtorNotLocated", resourceCulture);
	            }
	        }
	        internal static string DirectScopedResolvedFromRootException {
	            get {
	                return ResourceManager.GetString("DirectScopedResolvedFromRootException", resourceCulture);
	            }
	        }
	        internal static string GetCaptureDisposableNotSupported {
	            get {
	                return ResourceManager.GetString("GetCaptureDisposableNotSupported", resourceCulture);
	            }
	        }
	        internal static string ImplementationTypeCantBeConvertedToServiceType {
	            get {
	                return ResourceManager.GetString("ImplementationTypeCantBeConvertedToServiceType", resourceCulture);
	            }
	        }
	        internal static string InvalidServiceDescriptor {
	            get {
	                return ResourceManager.GetString("InvalidServiceDescriptor", resourceCulture);
	            }
	        }
	        internal static string MarkedCtorMissingArgumentTypes {
	            get {
	                return ResourceManager.GetString("MarkedCtorMissingArgumentTypes", resourceCulture);
	            }
	        }
	        internal static string MultipleCtorsFound {
	            get {
	                return ResourceManager.GetString("MultipleCtorsFound", resourceCulture);
	            }
	        }
	        internal static string MultipleCtorsFoundWithBestLength {
	            get {
	                return ResourceManager.GetString("MultipleCtorsFoundWithBestLength", resourceCulture);
	            }
	        }
	        internal static string MultipleCtorsMarkedWithAttribute {
	            get {
	                return ResourceManager.GetString("MultipleCtorsMarkedWithAttribute", resourceCulture);
	            }
	        }
	        internal static string NoConstructorMatch {
	            get {
	                return ResourceManager.GetString("NoConstructorMatch", resourceCulture);
	            }
	        }
	        internal static string NoConstructorMatch1 {
	            get {
	                return ResourceManager.GetString("NoConstructorMatch1", resourceCulture);
	            }
	        }
	        internal static string NoServiceRegistered {
	            get {
	                return ResourceManager.GetString("NoServiceRegistered", resourceCulture);
	            }
	        }
	        internal static string OpenGenericServiceRequiresOpenGenericImplementation {
	            get {
	                return ResourceManager.GetString("OpenGenericServiceRequiresOpenGenericImplementation", resourceCulture);
	            }
	        }
	        internal static string ScopedInSingletonException {
	            get {
	                return ResourceManager.GetString("ScopedInSingletonException", resourceCulture);
	            }
	        }
	        internal static string ScopedResolvedFromRootException {
	            get {
	                return ResourceManager.GetString("ScopedResolvedFromRootException", resourceCulture);
	            }
	        }
	        internal static string ServiceCollectionReadOnly {
	            get {
	                return ResourceManager.GetString("ServiceCollectionReadOnly", resourceCulture);
	            }
	        }
	        internal static string ServiceDescriptorNotExist {
	            get {
	                return ResourceManager.GetString("ServiceDescriptorNotExist", resourceCulture);
	            }
	        }
	        internal static string TrimmingAnnotationsDoNotMatch {
	            get {
	                return ResourceManager.GetString("TrimmingAnnotationsDoNotMatch", resourceCulture);
	            }
	        }
	        internal static string TrimmingAnnotationsDoNotMatch_NewConstraint {
	            get {
	                return ResourceManager.GetString("TrimmingAnnotationsDoNotMatch_NewConstraint", resourceCulture);
	            }
	        }
	        internal static string TryAddIndistinguishableTypeToEnumerable {
	            get {
	                return ResourceManager.GetString("TryAddIndistinguishableTypeToEnumerable", resourceCulture);
	            }
	        }
	        internal static string TypeCannotBeActivated {
	            get {
	                return ResourceManager.GetString("TypeCannotBeActivated", resourceCulture);
	            }
	        }
	        internal static string UnableToActivateTypeException {
	            get {
	                return ResourceManager.GetString("UnableToActivateTypeException", resourceCulture);
	            }
	        }
	        internal static string UnableToResolveService {
	            get {
	                return ResourceManager.GetString("UnableToResolveService", resourceCulture);
	            }
	        }
	    }
	    internal partial class Resources
	    {
	        private static readonly bool IsUsingResourceKeys = AppContext.TryGetSwitch("System.Resources.UseSystemResourceKeys", out bool isEnabled) ? isEnabled : false;
	        // This method is used to decide if we need to append the exception message parameters to the message when calling SR.Format.
	        // by default it returns the value of System.Resources.UseSystemResourceKeys AppContext switch or false if not specified.
	        // Native code generators can replace the value this returns based on user input at the time of native code generation.
	        // The Linker is also capable of replacing the value of this method when the application is being trimmed.
	        //private static bool UsingResourceKeys() => s_usingResourceKeys;
	        internal static string GetResourceString(string resourceKey)
	        {
	            if (IsUsingResourceKeys)
	            {
	                return resourceKey;
	            }
	            string? resourceString = null;
	            try
	            {
	                resourceString =
	#if SYSTEM_PRIVATE_CORELIB || NATIVEAOT
	                    InternalGetResourceString(resourceKey);
	#else
	                    ResourceManager.GetString(resourceKey);
	#endif
	            }
	            catch { }
	            return resourceString!; // only null if missing resources
	        }
	        internal static string GetResourceString(string resourceKey, string defaultString)
	        {
	            string resourceString = GetResourceString(resourceKey);
	            return resourceKey == resourceString || resourceString == null ? defaultString : resourceString;
	        }
	        internal static string Format(string resourceFormat, params object?[]? args)
	        {
	            if (args == null)
	            {
	                return resourceFormat;
	            }
	            if (args.Length <= 3)
	            {
	                return IsUsingResourceKeys ?
	                    string.Join(", ", resourceFormat, args) :
	                    string.Format(resourceFormat, args);
	            }
	            else
	            {
	                return IsUsingResourceKeys ?
	                    resourceFormat + ", " + string.Join(", ", args) :
	                    string.Format(resourceFormat, args);
	            }
	        }
	        //internal static string Format(IFormatProvider? provider, string resourceFormat, object? p1)
	        //{
	        //    if (UsingResourceKeys())
	        //    {
	        //        return string.Join(", ", resourceFormat, p1);
	        //    }
	        //    return string.Format(provider, resourceFormat, p1);
	        //}
	        //internal static string Format(IFormatProvider? provider, string resourceFormat, object? p1, object? p2)
	        //{
	        //    if (UsingResourceKeys())
	        //    {
	        //        return string.Join(", ", resourceFormat, p1, p2);
	        //    }
	        //    return string.Format(provider, resourceFormat, p1, p2);
	        //}
	        //internal static string Format(IFormatProvider? provider, string resourceFormat, object? p1, object? p2, object? p3)
	        //{
	        //    if (UsingResourceKeys())
	        //    {
	        //        return string.Join(", ", resourceFormat, p1, p2, p3);
	        //    }
	        //    return string.Format(provider, resourceFormat, p1, p2, p3);
	        //}
	        //internal static string Format(IFormatProvider? provider, string resourceFormat, params object?[]? args)
	        //{
	        //    if (args != null)
	        //    {
	        //        if (UsingResourceKeys())
	        //        {
	        //            return resourceFormat + ", " + string.Join(", ", args);
	        //        }
	        //        return string.Format(provider, resourceFormat, args);
	        //    }
	        //    return resourceFormat;
	        //}
	    }
	#endregion
	#region \Scopes
	public readonly struct AsyncServiceScope : IServiceScope, IAsyncDisposable
	{
	    private readonly IServiceScope serviceScope;
	    public AsyncServiceScope(IServiceScope serviceScope) => this.serviceScope = serviceScope ?? throw new ArgumentNullException(nameof(serviceScope));
	    public IServiceProvider ServiceProvider => this.serviceScope.ServiceProvider;
	    public void Dispose() => this.serviceScope.Dispose();
	    public ValueTask DisposeAsync()
	    {
	        if (this.serviceScope is IAsyncDisposable ad)
	        {
	            return ad.DisposeAsync();
	        }
	        this.serviceScope.Dispose();
	        // ValueTask.CompletedTask is only available in net5.0 and later.
	        return ValueTask.CompletedTask;
	    }
	}
	#endregion
	#region \Utilities
	    public static class ActivatorUtilities
	    {
	        private static readonly MethodInfo GetServiceInfo =
	            GetMethodInfo<Func<IServiceProvider, Type, Type, bool, object?>>((sp, t, r, c) => GetService(sp, t, r, c));
	        public static ObjectFactory CreateFactory([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type instanceType, Type[] argumentTypes)
	        {
	            CreateFactoryInternal(
	                instanceType,
	                argumentTypes,
	                out ParameterExpression provider,
	                out ParameterExpression argumentArray,
	                out Expression factoryExpressionBody);
	            var factoryLambda = Expression.Lambda<Func<IServiceProvider, object?[]?, object>>(
	                factoryExpressionBody,
	                provider,
	                argumentArray);
	            Func<IServiceProvider, object?[]?, object>? result = factoryLambda.Compile();
	            return result.Invoke;
	        }
	        public static ObjectFactory<T> CreateFactory<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(Type[] argumentTypes)
	        {
	            CreateFactoryInternal(
	                typeof(T),
	                argumentTypes,
	                out ParameterExpression provider,
	                out ParameterExpression argumentArray,
	                out Expression factoryExpressionBody);
	            var factoryLambda = Expression.Lambda<Func<IServiceProvider, object?[]?, T>>(
	                factoryExpressionBody,
	                provider,
	                argumentArray);
	            Func<IServiceProvider, object?[]?, T>? result = factoryLambda.Compile();
	            return result.Invoke;
	        }
	        private static void CreateFactoryInternal([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type instanceType, Type[] argumentTypes, out ParameterExpression provider, out ParameterExpression argumentArray, out Expression factoryExpressionBody)
	        {
	            FindApplicableConstructor(
	                instanceType,
	                argumentTypes,
	                out ConstructorInfo constructor,
	                out int?[] parameterMap);
	            provider = Expression.Parameter(typeof(IServiceProvider), "provider");
	            argumentArray = Expression.Parameter(typeof(object[]), "argumentArray");
	            factoryExpressionBody = BuildFactoryExpression(constructor, parameterMap, provider, argumentArray);
	        }
	        public static object CreateInstance(IServiceProvider provider, Type instanceType, params object[] parameters)
	        {
	            int bestLength = -1;
	            bool seenPreferred = false;
	            ConstructorMatcher bestMatcher = default;
	            if (!instanceType.IsAbstract)
	            {
	                foreach (ConstructorInfo? constructor in instanceType.GetConstructors())
	                {
	                    var matcher = new ConstructorMatcher(constructor);
	                    var isPreferred = constructor.IsDefined(typeof(ActivatorUtilitiesConstructorAttribute), false);
	                    var length = matcher.Match(parameters);
	                    if (isPreferred)
	                    {
	                        if (seenPreferred)
	                        {
	                            ThrowMultipleCtorsMarkedWithAttributeException();
	                        }
	                        if (length == -1)
	                        {
	                            ThrowMarkedCtorDoesNotTakeAllProvidedArguments();
	                        }
	                    }
	                    if (isPreferred || bestLength < length)
	                    {
	                        bestLength = length;
	                        bestMatcher = matcher;
	                    }
	                    seenPreferred |= isPreferred;
	                }
	            }
	            if (bestLength == -1)
	            {
	                string? message = $"A suitable constructor for type '{instanceType}' could not be located. Ensure the type is concrete and all parameters of a public constructor are either registered as services or passed as arguments. Also ensure no extraneous arguments are provided.";
	                throw new InvalidOperationException(message);
	            }
	            return bestMatcher.CreateInstance(provider);
	        }
	        public static T CreateInstance<T>(IServiceProvider provider, params object[] parameters)
	        {
	            return (T)CreateInstance(provider, typeof(T), parameters);
	        }
	        public static T GetServiceOrCreateInstance<T>(IServiceProvider provider)
	        {
	            return (T)GetServiceOrCreateInstance(provider, typeof(T));
	        }
	        public static object GetServiceOrCreateInstance(IServiceProvider provider, Type type)
	        {
	            return provider.GetService(type) ?? CreateInstance(provider, type);
	        }
	        private static MethodInfo GetMethodInfo<T>(Expression<T> expr)
	        {
	            var mc = (MethodCallExpression)expr.Body;
	            return mc.Method;
	        }
	        private static object? GetService(IServiceProvider sp, Type type, Type requiredBy, bool isDefaultParameterRequired)
	        {
	            object? service = sp.GetService(type);
	            if (service == null && !isDefaultParameterRequired)
	            {
	                string? message = $"Unable to resolve service for type '{type}' while attempting to activate '{requiredBy}'.";
	                throw new InvalidOperationException(message);
	            }
	            return service;
	        }
	        private static Expression BuildFactoryExpression(
	            ConstructorInfo constructor,
	            int?[] parameterMap,
	            Expression serviceProvider,
	            Expression factoryArgumentArray)
	        {
	            var constructorParameters = constructor.GetParameters();
	            var constructorArguments = new Expression[constructorParameters.Length];
	            for (int i = 0; i < constructorParameters.Length; i++)
	            {
	                var constructorParameter = constructorParameters[i];
	                var parameterType = constructorParameter.ParameterType;
	                var hasDefaultValue = constructorParameter.TryGetDefaultValue(out object? defaultValue);
	                constructorArguments[i] = parameterMap[i] != null ?
	                    Expression.ArrayAccess(factoryArgumentArray, Expression.Constant(parameterMap[i])) :
	                    Expression.Call(GetServiceInfo, new Expression[]
	                    {
	                        serviceProvider,
	                        Expression.Constant(parameterType, typeof(Type)),
	                        Expression.Constant(constructor.DeclaringType, typeof(Type)),
	                        Expression.Constant(hasDefaultValue)
	                    });
	                // Support optional constructor arguments by passing in the default value
	                // when the argument would otherwise be null.
	                if (hasDefaultValue)
	                {
	                    constructorArguments[i] = Expression.Coalesce(
	                        constructorArguments[i], 
	                        Expression.Constant(defaultValue));
	                }
	                constructorArguments[i] = Expression.Convert(constructorArguments[i], parameterType);
	            }
	            return Expression.New(constructor, constructorArguments);
	        }
	        private static void FindApplicableConstructor(
	            Type instanceType,
	            Type[] argumentTypes,
	            out ConstructorInfo matchingConstructor,
	            out int?[] matchingParameterMap)
	        {
	            ConstructorInfo? constructorInfo = null;
	            int?[]? parameterMap = null;
	            if (!TryFindPreferredConstructor(instanceType, argumentTypes, ref constructorInfo, ref parameterMap) &&
	                !TryFindMatchingConstructor(instanceType, argumentTypes, ref constructorInfo, ref parameterMap))
	            {
	                string? message = $"A suitable constructor for type '{instanceType}' could not be located. Ensure the type is concrete and all parameters of a public constructor are either registered as services or passed as arguments. Also ensure no extraneous arguments are provided.";
	                throw new InvalidOperationException(message);
	            }
	            matchingConstructor = constructorInfo;
	            matchingParameterMap = parameterMap;
	        }
	        // Tries to find constructor based on provided argument types
	        private static bool TryFindMatchingConstructor(
	            Type instanceType,
	            Type[] argumentTypes,
	            [NotNullWhen(true)] ref ConstructorInfo? matchingConstructor,
	            [NotNullWhen(true)] ref int?[]? parameterMap)
	        {
	            foreach (ConstructorInfo? constructor in instanceType.GetConstructors())
	            {
	                if (TryCreateParameterMap(constructor.GetParameters(), argumentTypes, out int?[] tempParameterMap))
	                {
	                    if (matchingConstructor != null)
	                    {
	                        throw new InvalidOperationException($"Multiple constructors accepting all given argument types have been found in type '{instanceType}'. There should only be one applicable constructor.");
	                    }
	                    matchingConstructor = constructor;
	                    parameterMap = tempParameterMap;
	                }
	            }
	            if (matchingConstructor != null)
	            {
	                Debug.Assert(parameterMap != null);
	                return true;
	            }
	            return false;
	        }
	        // Tries to find constructor marked with ActivatorUtilitiesConstructorAttribute
	        private static bool TryFindPreferredConstructor(
	            Type instanceType,
	            Type[] argumentTypes,
	            [NotNullWhen(true)] ref ConstructorInfo? matchingConstructor,
	            [NotNullWhen(true)] ref int?[]? parameterMap)
	        {
	            bool seenPreferred = false;
	            foreach (ConstructorInfo? constructor in instanceType.GetConstructors())
	            {
	                if (constructor.IsDefined(typeof(ActivatorUtilitiesConstructorAttribute), false))
	                {
	                    if (seenPreferred)
	                    {
	                        ThrowMultipleCtorsMarkedWithAttributeException();
	                    }
	                    if (!TryCreateParameterMap(constructor.GetParameters(), argumentTypes, out int?[] tempParameterMap))
	                    {
	                        ThrowMarkedCtorDoesNotTakeAllProvidedArguments();
	                    }
	                    matchingConstructor = constructor;
	                    parameterMap = tempParameterMap;
	                    seenPreferred = true;
	                }
	            }
	            if (matchingConstructor != null)
	            {
	                Debug.Assert(parameterMap != null);
	                return true;
	            }
	            return false;
	        }
	        // Creates an injective parameterMap from givenParameterTypes to assignable constructorParameters.
	        // Returns true if each given parameter type is assignable to a unique; otherwise, false.
	        private static bool TryCreateParameterMap(ParameterInfo[] constructorParameters, Type[] argumentTypes, out int?[] parameterMap)
	        {
	            parameterMap = new int?[constructorParameters.Length];
	            for (int i = 0; i < argumentTypes.Length; i++)
	            {
	                bool foundMatch = false;
	                Type? givenParameter = argumentTypes[i];
	                for (int j = 0; j < constructorParameters.Length; j++)
	                {
	                    if (parameterMap[j] != null)
	                    {
	                        // This ctor parameter has already been matched
	                        continue;
	                    }
	                    if (constructorParameters[j].ParameterType.IsAssignableFrom(givenParameter))
	                    {
	                        foundMatch = true;
	                        parameterMap[j] = i;
	                        break;
	                    }
	                }
	                if (!foundMatch)
	                {
	                    return false;
	                }
	            }
	            return true;
	        }
	        private struct ConstructorMatcher
	        {
	            private readonly ConstructorInfo _constructor;
	            private readonly ParameterInfo[] _parameters;
	            private readonly object?[] _parameterValues;
	            public ConstructorMatcher(ConstructorInfo constructor)
	            {
	                _constructor = constructor;
	                _parameters = _constructor.GetParameters();
	                _parameterValues = new object?[_parameters.Length];
	            }
	            public int Match(object[] givenParameters)
	            {
	                int applyIndexStart = 0;
	                int applyExactLength = 0;
	                for (int givenIndex = 0; givenIndex != givenParameters.Length; givenIndex++)
	                {
	                    Type? givenType = givenParameters[givenIndex]?.GetType();
	                    bool givenMatched = false;
	                    for (int applyIndex = applyIndexStart; givenMatched == false && applyIndex != _parameters.Length; ++applyIndex)
	                    {
	                        if (_parameterValues[applyIndex] == null &&
	                            _parameters[applyIndex].ParameterType.IsAssignableFrom(givenType))
	                        {
	                            givenMatched = true;
	                            _parameterValues[applyIndex] = givenParameters[givenIndex];
	                            if (applyIndexStart == applyIndex)
	                            {
	                                applyIndexStart++;
	                                if (applyIndex == givenIndex)
	                                {
	                                    applyExactLength = applyIndex;
	                                }
	                            }
	                        }
	                    }
	                    if (givenMatched == false)
	                    {
	                        return -1;
	                    }
	                }
	                return applyExactLength;
	            }
	            public object CreateInstance(IServiceProvider provider)
	            {
	                for (int index = 0; index != _parameters.Length; index++)
	                {
	                    if (_parameterValues[index] == null)
	                    {
	                        object? value = provider.GetService(_parameters[index].ParameterType);
	                        if (value == null)
	                        {
	                            if (!_parameters[index].TryGetDefaultValue(out object? defaultValue))
	                            {
	                                throw new InvalidOperationException($"Unable to resolve service for type '{_parameters[index].ParameterType}' while attempting to activate '{_constructor.DeclaringType}'.");
	                            }
	                            else
	                            {
	                                _parameterValues[index] = defaultValue;
	                            }
	                        }
	                        else
	                        {
	                            _parameterValues[index] = value;
	                        }
	                    }
	                }
	                return _constructor.Invoke(BindingFlags.DoNotWrapExceptions, binder: null, parameters: _parameterValues, culture: null);
	            }
	        }
	        private static void ThrowMultipleCtorsMarkedWithAttributeException()
	        {
	            throw new InvalidOperationException($"Multiple constructors were marked with {nameof(ActivatorUtilitiesConstructorAttribute)}.");
	        }
	        private static void ThrowMarkedCtorDoesNotTakeAllProvidedArguments()
	        {
	            throw new InvalidOperationException($"Constructor marked with {nameof(ActivatorUtilitiesConstructorAttribute)} does not accept all given argument types.");
	        }
	    }
	    [AttributeUsage(AttributeTargets.All)]
	    public class ActivatorUtilitiesConstructorAttribute : Attribute
	    {
	    }
	public delegate object ObjectFactory(IServiceProvider serviceProvider, object?[]? arguments);
	public delegate T ObjectFactory<T>(IServiceProvider serviceProvider, object?[]? arguments);
	#endregion
}
#endregion
