<Query Kind="Program">
<Namespace>System</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>Assimalign.OGraph.Gdm</Namespace>
<Namespace>Assimalign.OGraph.Internal</Namespace>
<Namespace>Assimalign.OGraph.Syntax</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>System.Threading.Tasks</Namespace>
<Namespace>System.Threading</Namespace>
<Namespace>System.Security.Claims</Namespace>
<Namespace>System.IO</Namespace>
<Namespace>System.Collections</Namespace>
<Namespace>Gdm</Namespace>
<Namespace>System.Text.Json</Namespace>
<Namespace>System.Xml</Namespace>
<Namespace>Syntax</Namespace>
<Namespace>System.Diagnostics.CodeAnalysis</Namespace>
<Namespace>System.Reflection</Namespace>
<Namespace>System.Diagnostics</Namespace>
<Namespace>System.Runtime.CompilerServices</Namespace>
<Namespace>System.Globalization</Namespace>
<Namespace>System.Text.RegularExpressions</Namespace>
<Namespace>System.Web</Namespace>
<Namespace>System.Buffers</Namespace>
<Namespace>System.Net</Namespace>
</Query>
#load ".\assimalign.ograph.core"
#load ".\assimalign.ograph.gdm"
#load ".\assimalign.ograph.syntax"

void Main()
{

}

#region Assimalign.OGraph.Server(net8.0)
namespace Assimalign.OGraph
{
	#region \
	public sealed class OGraphExecutorBuilder : IOGraphExecutorBuilder
	{
	    private readonly IList<IOGraphGdm> models;
	    private readonly OGraphExecutorOptions options;
	    private readonly IList<Action> onBuild;
	    public OGraphExecutorBuilder()
	    {
	        models = new List<IOGraphGdm>();
	        options = new OGraphExecutorOptions();
	        onBuild = new List<Action>();
	    }
	    public IOGraphExecutorBuilder ConfigureApplication(Action<IOGraphApplicationBuilder> configure)
	    {
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        onBuild.Add(() =>
	        {
	            var builder = new ApplicationBuilder(models)
	            {
	                Options = options
	            };
	            configure.Invoke(builder);
	        });
	        return this;
	    }
	    public IOGraphExecutorBuilder ConfigureModel(Label label, Action<IOGraphGdmBuilder> configure)
	    {
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        models.Add(OGraphGdmBuilder.Create(label, configure));
	        return this;
	    }
	    public IOGraphExecutorBuilder ConfigureOptions(Action<OGraphExecutorOptions> configure)
	    {
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        configure.Invoke(options);
	        return this;
	    }
	    public IOGraphExecutorBuilder ConfigureOptions(OGraphExecutorOptions options)
	    {
	        throw new NotImplementedException();
	    }
	    IOGraphExecutor IOGraphExecutorBuilder.Build()
	    {
	        foreach (var action in onBuild)
	        {
	            action();
	        }
	        return new Executor(models, options);
	    }
	}
	public sealed class OGraphExecutorOptions
	{
	    public string? RoutePrefix { get; set; }
	    public string DefaultMediaType { get; set; } = OGraphMediaType.Json;
	    public QueryParserOptions ParserOptions { get; set; } = new();
	    public IServiceProvider? ServiceProvider { get; set; }
	    public TimeSpan Timeout { get; set; } = TimeSpan.Zero;
	}
	public sealed class OGraphHeaderCollection : Dictionary<string, HeaderValue>
	{
	    public OGraphHeaderCollection() : base(StringComparer.CurrentCultureIgnoreCase) { }
	    public OGraphHeaderCollection(Dictionary<string, HeaderValue> headers) : base(headers, StringComparer.CurrentCultureIgnoreCase)
	    {
	    }
	    public HeaderValue? Host => TryGetValue("host", out var value) ? value : default;
	    public HeaderValue? Accept => TryGetValue("accept", out var value) ? value : default;
	    public HeaderValue? ContentType => TryGetValue("content-type", out var value) ? value : default;
	    public HeaderValue? ContentLength => TryGetValue("content-length", out var value) ? value : default;
	}
	public static class OGraphMediaType
	{
	    public const string Xml = "application/xml+ograph";
	    public const string Json = "application/json+ograph";
	}
	public abstract class OGraphQueryOptions
	{
	    public bool CanSort { get; set; } = true;
	    public bool CanFilter { get; set; } = true;
	    public bool CanPage { get; set; } = true;
	    public bool CanProject { get; set; } = true;
	    public int? MaxPageSize { get; set; } 
	    public int? DefaultPageSize { get; set; } = 100;
	    public bool DefaultProjectAll { get; set; } = true; // TODO: Need to evaluate this option. Sometimes for discovery it is nice to see what is returned in a query.
	    public static OGraphQueryOptions Default => new DefaultOptions();
	    private partial class DefaultOptions : OGraphQueryOptions { }
	}
	#endregion
	#region \Abstractions
	public interface IOGraphApplicationBuilder
	{
	    IOGraphApplicationBuilder Bind<T>(Action<IOGraphApplicationOperationDescriptor> configure) 
	        where T : class, new();
	}
	public interface IOGraphApplicationOperationDescriptor
	{
	    IOGraphOperationBindingDescriptor MapQuery(Label label);
	    IOGraphOperationBindingDescriptor MapCommand(Label label);
	    IOGraphOperationBindingDescriptor MapGet(Label operationName);
	    IOGraphOperationBindingDescriptor MapPut(Label operationName);
	    IOGraphOperationBindingDescriptor MapPost(Label operationName);
	    IOGraphOperationBindingDescriptor MapPatch(Label operationName);
	    IOGraphOperationBindingDescriptor MapDelete(Label operationName);
	}
	public interface IOGraphExecutor
	{
	    Task ExecuteAsync(IOGraphExecutorContext context, CancellationToken cancellationToken = default);
	}
	public interface IOGraphExecutorBuilder
	{
	    IOGraphExecutorBuilder ConfigureOptions(OGraphExecutorOptions options);
	    IOGraphExecutorBuilder ConfigureOptions(Action<OGraphExecutorOptions> configure);
	    IOGraphExecutorBuilder ConfigureModel(Label label, Action<IOGraphGdmBuilder> configure);
	    IOGraphExecutorBuilder ConfigureApplication(Action<IOGraphApplicationBuilder> configure);
	    IOGraphExecutor Build();
	}
	public interface IOGraphExecutorContext
	{
	    IOGraphExecutorRequest Request { get; }
	    IOGraphExecutorResponse Response { get; }
	    IServiceProvider? ServiceProvider { get; }
	    ClaimsPrincipal ClaimsPrincipal { get; }
	}
	public interface IOGraphExecutorHeaderCollection : IDictionary<HeaderKey, HeaderValue>
	{
	    HeaderValue ContentType { get; set; }
	    HeaderValue ContentLength { get; set; }
	    HeaderValue Accept { get; set; }
	    HeaderValue AcceptEncoding { get; set; }
	}
	public interface IOGraphExecutorQueryCollection : IDictionary<QueryKey, QueryValue>
	{
	    QueryValue Query { get; }
	}
	public interface IOGraphExecutorRequest
	{
	    Host Host { get; }
	    Path Path { get; }
	    Method Method { get; }
	    Stream Body { get; }
	    IOGraphExecutorQueryCollection Query { get; }
	    IOGraphExecutorHeaderCollection Headers { get; }
	}
	public interface IOGraphExecutorResponse
	{
	    StatusCode StatusCode { get; set; }
	    IOGraphExecutorHeaderCollection Headers { get; }
	    Stream Body { get; }
	}
	public interface IOGraphOperationBinding : IOGraphGdmBinding
	{
	    Label Label { get; }
	    Route Route { get; }
	    Method Method { get; }
	    OperationType OperationType { get; }
	    IOGraphGdmTypeReference RequestType { get; }
	    IOGraphGdmTypeReference ResponseType { get; }
	    IOGraphOperationBindingHeaders Headers { get; }
	    IOGraphOperationBindingQueryParams Query { get; }
	    IOGraphOperationBindingResolver Resolver { get; }
	    IOGraphOperationBindingMiddlewareQueue Middleware { get; }
	    OGraphQueryOptions QueryOptions { get; }
	    IOGraphQueryProvider QueryProvider { get; }
	    Task ExecuteAsync(IOGraphOperationBindingContext context, CancellationToken cancellationToken);
	}
	public interface IOGraphQueryOperationBinding
	{
	}
	public interface IOGraphOperationBindingContext : IOGraphGdmBindingContext
	{
	    IOGraphExecutorRequest Request { get; }
	    IOGraphExecutorResponse Response { get; }
	    IServiceProvider ServiceProvider { get; }
	    new IOGraphGdmVertex Element { get; }
	    T GetService<T>();
	    T GetRouteValue<T>(string paramName);
	    ClaimsPrincipal GetClaimsPrincipal();
	    QueryDocument? GetQueryDocument();
	    OGraphQueryOptions GetQueryOptions();
	    IOGraphQueryProvider GetQueryProvider();
	}
	public interface IOGraphCommandOperationBindingDescriptor
	{
	}
	public interface IOGraphOperationBindingDescriptor
	{
	    IOGraphOperationBindingDescriptor UseRoute(Route route);
	    IOGraphOperationBindingDescriptor UseMethod(Method method);
	    IOGraphOperationBindingDescriptor UseRequestType<TGdmType>() where TGdmType : IOGraphGdmType;
	    IOGraphOperationBindingDescriptor UseMiddleware<TMiddleware>() where TMiddleware : IOGraphOperationBindingMiddleware, new();
	    IOGraphOperationBindingDescriptor UseMiddleware(OGraphOperationBindingMiddleware middleware);
	    IOGraphOperationBindingDescriptor UseMiddleware(IOGraphOperationBindingMiddleware middleware);
	    IOGraphOperationBindingDescriptor UseResolver<TResolver>() where TResolver : IOGraphOperationBindingResolver, new();
	    IOGraphOperationBindingDescriptor UseResolver(OGraphOperationBindingResolver resolver);
	    IOGraphOperationBindingDescriptor UseResolver(IOGraphOperationBindingResolver resolver);
	}
	public interface IOGraphQueryOperationBindingDescriptor
	{
	}
	public interface IOGraphOperationBindingHeaders
	{
	}
	public interface IOGraphOperationBindingMiddleware
	{
	    Task<IOGraphResult> InvokeAsync(
	        IOGraphOperationBindingContext context,
	        CancellationToken cancellationToken,
	        OGraphOperationBindingMiddlewareHandler next);
	}
	public interface IOGraphOperationBindingMiddlewareQueue : IEnumerable<IOGraphOperationBindingMiddleware>
	{
	    int Count { get; }
	    bool IsReadOnly { get; }
	    void Enqueue(IOGraphOperationBindingMiddleware middleware);
	    void Dequeue(IOGraphOperationBindingMiddleware middleware);
	}
	public interface IOGraphOperationBindingQueryParams
	{
	}
	public interface IOGraphOperationBindingResolver
	{
	    Task<IOGraphResult> InvokeAsync(IOGraphOperationBindingContext context, CancellationToken cancellationToken);
	}
	public interface IOGraphPropertyBinding : IOGraphGdmBinding
	{
	    IOGraphPropertyBindingResolver Resolver { get; }
	    IOGraphPropertyBindingMiddlewareQueue Middleware { get; }
	    Task ExecuteAsync(IOGraphPropertyBindingContext context, CancellationToken cancellationToken);
	}
	public interface IOGraphPropertyBindingContext : IOGraphGdmBindingContext
	{
	    IOGraphExecutorRequest Request { get; }
	    IOGraphExecutorResponse Response { get; }
	    IServiceProvider ServiceProvider { get; }
	    new IOGraphGdmProperty Element { get; }
	    T GetService<T>();
	    ClaimsPrincipal? GetClaimsPrincipal();
	}
	public interface IOGraphPropertyBindingDescriptor
	{
	    IOGraphPropertyBindingDescriptor UseResolver<TResolver>() 
	        where TResolver : IOGraphPropertyBindingResolver, new();
	    IOGraphPropertyBindingDescriptor UseResolver(OGraphPropertyBindingResolver resolver);
	    IOGraphPropertyBindingDescriptor UseResolver(IOGraphPropertyBindingResolver resolver);
	    IOGraphPropertyBindingDescriptor UseMiddleware<TMiddleware>()
	        where TMiddleware : IOGraphPropertyBindingMiddleware, new();
	    IOGraphPropertyBindingDescriptor UseMiddleware(OGraphPropertyBindingMiddleware middleware);
	    IOGraphPropertyBindingDescriptor UseMiddleware(IOGraphPropertyBindingMiddleware middleware);
	}
	public interface IOGraphPropertyBindingMiddleware
	{
	    Task<IOGraphResult> InvokeAsync(
	        IOGraphPropertyBindingContext context,
	        CancellationToken cancellationToken,
	        OGraphPropertyBindingMiddlewareHandler next);
	}
	public interface IOGraphPropertyBindingMiddlewareQueue : IEnumerable<IOGraphPropertyBindingMiddleware>
	{
	    int Count { get; }
	    bool IsReadOnly { get; }
	    void Enqueue(IOGraphPropertyBindingMiddleware middleware);
	    void Dequeue(IOGraphPropertyBindingMiddleware middleware);
	}
	public interface IOGraphPropertyBindingResolver
	{
	    Task<IOGraphResult> InvokeAsync(IOGraphPropertyBindingContext context, CancellationToken cancellationToken);
	}
	public interface IOGraphQueryProvider
	{
	    Type ElementType { get; }
	    Task<IOGraphQueryResult> ExecuteAsync(IOGraphQueryProviderContext context, OGraphQueryOptions options, CancellationToken cancellationToken = default);
	}
	public interface IOGraphQueryProviderContext
	{
	    object QueryItem { get; }
	    QueryDocument QueryDocument { get; }
	    IOGraphGdmLabeledElement Element { get; }
	    IServiceProvider ServiceProvider { get; }
	}
	public interface IOGraphResult
	{
	    StatusCode StatusCode { get; }
	}
	public interface IOGraphErrorResult : IOGraphResult
	{
	    IOGraphError Error { get; }
	}
	public interface IOGraphObjectResult : IOGraphResult
	{
	    IOGraphError Error { get; }
	    object? Data { get; }
	}
	public interface IOGraphObjectResult<T> : IOGraphObjectResult 
	    where T : class
	{
	    new T Data { get; }
	}
	public interface IOGraphPropertyResult : IOGraphResult
	{
	    object? Value { get; }
	}
	public interface IOGraphPropertyResult<out T> : IOGraphPropertyResult
	{
	    new T Value { get; }
	}
	public interface IOGraphQueryResult : IOGraphResult, IEnumerable
	{
	    long Total { get; }
	    long Count { get; }
	}
	public interface IOGraphQueryResult<T> : IOGraphQueryResult, IEnumerable<T>
	{
	}
	#endregion
	#region \Delegates
	public delegate Task<IOGraphResult> OGraphOperationBindingMiddleware(
	    IOGraphOperationBindingContext context,
	    CancellationToken cancellationToken,
	    OGraphOperationBindingMiddlewareHandler next);
	public delegate Task<IOGraphResult> OGraphOperationBindingMiddlewareHandler(
	    IOGraphOperationBindingContext context, 
	    CancellationToken cancellationToken);
	public delegate Task<IOGraphResult> OGraphOperationBindingResolver(
	    IOGraphOperationBindingContext context, 
	    CancellationToken cancellationToken);
	public delegate ValueTask<IOGraphResult> OGraphPropertyBindingMiddleware(
	    IOGraphPropertyBindingContext context,
	    CancellationToken cancellationToken,
	    OGraphPropertyBindingMiddlewareHandler next);
	public delegate Task<IOGraphResult> OGraphPropertyBindingMiddlewareHandler(IOGraphPropertyBindingContext context, CancellationToken cancellationToken);
	public delegate Task<IOGraphResult> OGraphPropertyBindingResolver(
	    IOGraphPropertyBindingContext context, 
	    CancellationToken cancellationToken);
	#endregion
	#region \Exceptions
	public class OGraphServerException
	{
	}
	#endregion
	#region \Extensions
	public static class OGraphOperationBindingExtensions
	{
	    public static IOGraphOperationBindingDescriptor UseResolver<T>(
	        this IOGraphOperationBindingDescriptor descriptor, 
	        Func<IOGraphOperationBindingContext, Either<IQueryable<T>, IOGraphErrorResult>> resolver)
	    {
	        return descriptor
	            .UseResolver(async (context, cancellationToken) =>
	            {
	                var either = resolver.Invoke(context);
	                if (either.If(out IOGraphErrorResult error, out IQueryable<T> queryable))
	                {
	                    return error;
	                }
	                else
	                {
	                    var queryProvider = context.GetQueryProvider();
	                    var queryOptions = context.GetQueryOptions();
	                    return await queryProvider.ExecuteAsync(default, queryOptions, cancellationToken);
	                }
	            });
	    }
	}
	#endregion
	#region \Internal
	internal class ApplicationBuilder : IOGraphApplicationBuilder
	{
	    private readonly IEnumerable<IOGraphGdm> models;
	    public ApplicationBuilder(IEnumerable<IOGraphGdm> models)
    {
        this.models = models;
    }
	    public OGraphExecutorOptions Options { get; init; }
	    public IOGraphApplicationBuilder Bind<T>(Action<IOGraphApplicationOperationDescriptor<T>> configure) where T : class, new()
	    {
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        foreach (var model in models)
	        {
	            foreach (var vertex in model.GetGdmVertices())
	            {
	                if (vertex.IsRuntimeTypeMatch(typeof(T)))
	                {
	                    var descriptor = new ApplicationOperationDescriptor<T>(vertex)
	                    {
	                        Options = this.Options
	                    };
	                    configure.Invoke(descriptor);
	                    return this;
	                }
	            }
	        }
	        throw new Exception("Could not find vertex to bind to.");
	    }
	}
	internal class ApplicationOperationDescriptor<T> : IOGraphApplicationOperationDescriptor<T> where T : class, new()
	{
	    private readonly IOGraphGdmVertex vertex;
	    public ApplicationOperationDescriptor(IOGraphGdmVertex vertex)
    {
        this.vertex = vertex;
    }
	    public OGraphExecutorOptions Options { get; init; }
	    public IOGraphOperationBindingDescriptor MapDelete(Label operationName)
	    {
	        var binding = new OperationBinding()
	        {
	            Label = operationName,
	            Method = Method.Delete
	        };
	        vertex.Bind(binding);
	        return new OperationBindingDescriptor(binding)
	        {
	            Options = this.Options
	        };
	    }
	    public IOGraphOperationBindingDescriptor MapGet(Label operationName)
	    {
	        var binding = new OperationBinding()
	        {
	            Label = operationName,
	            Method = Method.Get
	        };
	        vertex.Bind(binding);
	        return new OperationBindingDescriptor(binding)
	        {
	            Options = this.Options
	        };
	    }
	    public IOGraphOperationBindingDescriptor MapPatch(Label operationName)
	    {
	        var binding = new OperationBinding()
	        {
	            Label = operationName,
	            Method = Method.Patch
	        };
	        vertex.Bind(binding);
	        return new OperationBindingDescriptor(binding)
	        {
	            Options = this.Options
	        };
	    }
	    public IOGraphOperationBindingDescriptor MapPost(Label operationName)
	    {
	        var binding = new OperationBinding()
	        {
	            Label = operationName,
	            Method = Method.Post
	        };
	        vertex.Bind(binding);
	        return new OperationBindingDescriptor(binding)
	        {
	            Options = this.Options
	        };
	    }
	    public IOGraphOperationBindingDescriptor MapPut(Label operationName)
	    {
	        var binding = new OperationBinding()
	        {
	            Label = operationName,
	            Method = Method.Put
	        };
	        vertex.Bind(binding);
	        return new OperationBindingDescriptor(binding)
	        {
	            Options = this.Options
	        };
	    }
	}
	internal class Executor : IOGraphExecutor
	{
	    private readonly OGraphExecutorOptions options;
	    private readonly IEnumerable<IOGraphGdm> models;
	    public Executor(IEnumerable<IOGraphGdm> models, OGraphExecutorOptions options)
    {
        this.options = options;
        this.models = models;
	    }
	    public Task ExecuteAsync(IOGraphExecutorContext context, CancellationToken cancellationToken)
	    {
	        try
	        {
	            using var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
	            // Check if timeout was set
	            if (options.Timeout > options.Timeout)
	            {
	                cancellationTokenSource.CancelAfter(options.Timeout);
	            }
	            // Throw an exception for cancellation
	            cancellationTokenSource.Token.ThrowIfCancellationRequested();
	            var response = context.Response;
	            var request = context.Request;
	            foreach (var model in models)
	            {
	                foreach (var vertex in model.GetGdmVertices())
	                {
	                    foreach (var binding in vertex.Bindings.OfType<IOGraphOperationBinding>())
	                    {
	                        // 1. Match method and route
	                        if (binding.Method.Equals(request.Method) && binding.Route.IsMatch(request.Path))
	                        {
	                            // 2. 415 Check - Let's check Content Length & Type header
	                            if (request.Headers.TryGetValue(HeaderKey.ContentLength, out var contentLength))
	                            {
	                                var length = long.Parse(contentLength!);
	                                if (length > 0 && request.Headers.TryGetValue(HeaderKey.ContentType, out var contentType))
	                                {
	                                    var collection = (contentType as ICollection<string>);
	                                    if (!collection.Contains(OGraphMediaType.Json) && collection.Contains(OGraphMediaType.Xml))
	                                    {
	                                    }
	                                }
	                                // Unsupported Media Type
	                                else
	                                {
	                                }
	                            }
	                            // 3. 406 Check - Check for accept header
	                            if (request.Headers.TryGetValue(HeaderKey.Accept, out var accept))
	                            {
	                                var collection = accept as ICollection<string>;
	                                // Accepts either any content-type or both OGraph content-type.
	                                if (collection.Contains("*/*") || (collection.Contains(OGraphMediaType.Xml) && collection.Contains(OGraphMediaType.Json)))
	                                {
	                                }
	                                else if (collection.Contains(OGraphMediaType.Xml))
	                                {
	                                }
	                                else if (collection.Contains(OGraphMediaType.Json))
	                                {
	                                }
	                                // The user requested an Unsupported media type. - 406 (Not Acceptable)
	                                else
	                                {
	                                   // return ProcessErrorResultAsJsonAsync(new OGraph)
	                                }
	                            }
	                            return binding.ExecuteAsync(new OperationBindingContext()
	                            {
	                                Element = vertex,
	                                Request = request,
	                                Response = response,
	                                ServiceProvider = options.ServiceProvider!
	                            }, cancellationTokenSource.Token);
	                        }
	                    }
	                }
	            }
	            return Task.CompletedTask;
	        }
	        catch (Exception exception)
	        {
	            throw;
	        }
	    }
	}
	internal class MiddlewareQueueBase<TMiddleware> : IEnumerable<TMiddleware>
	{
	    public int Count { get; set; }
	    public bool IsReadOnly { get; set; }
	    public void Enqueue(TMiddleware middleware)
	    {
	    }
	    public void Dequeue(TMiddleware middleware)
	    {
	    }
	    public IEnumerator<TMiddleware> GetEnumerator()
	    {
	        throw new NotImplementedException();
	    }
	    IEnumerator IEnumerable.GetEnumerator()
	    {
	        return GetEnumerator();
	    }
	}
	internal partial class OperationBinding : IOGraphOperationBinding
	{
	    public OperationBinding()
	    {
	        Middleware = new OperationBindingMiddlewareQueue();
	    }
	    public Label Label { get; set; }
	    public Route Route { get; set; }
	    public Method Method { get; set; }
	    public OGraphQueryOptions QueryOptions { get; set; } = OGraphQueryOptions.Default;
	    public IOGraphQueryProvider QueryProvider { get; set; }
	    public IOGraphOperationBindingResolver Resolver { get; set; } = default!;
	    public IOGraphOperationBindingMiddlewareQueue Middleware { get; }
	    public IOGraphGdmTypeReference RequestType { get; set; } = default!;
	    public IOGraphGdmTypeReference ResponseType { get; set; } = default!;
	    public IOGraphOperationBindingHeaders Headers { get; set; } = default!;
	    public IOGraphOperationBindingQueryParams Query { get; set; } = default!;
	    public OperationType OperationType { get; set; }
	    public async Task ExecuteAsync(IOGraphOperationBindingContext context, CancellationToken cancellationToken = default)
	    {
	        if (context is not OperationBindingContext ctx)
	        {
	            ThrowHelper.ThrowArgumentException("");
	        }
	        else
	        {
	            try
	            {
	                var request = ctx.Request;
	                var response = ctx.Response;
	                // 2. 415 Check - Let's check Content Length & Type header
	                if (request.Headers.TryGetValue(HeaderKey.ContentLength, out var contentLength))
	                {
	                    var length = long.Parse(contentLength!);
	                    if (length > 0 && request.Headers.TryGetValue(HeaderKey.ContentType, out var contentType))
	                    {
	                        var collection = (contentType as ICollection<string>);
	                        if (!collection.Contains(OGraphMediaType.Json) && collection.Contains(OGraphMediaType.Xml))
	                        {
	                        }
	                    }
	                    // Unsupported Media Type
	                    else
	                    {
	                    }
	                }
	                // 3. 406 Check - Check for accept header
	                if (request.Headers.TryGetValue(HeaderKey.Accept, out var accept))
	                {
	                    var collection = accept as ICollection<string>;
	                    // Accepts either any content-type or both OGraph content-type.
	                    if (collection.Contains("*/*") || (collection.Contains(OGraphMediaType.Xml) && collection.Contains(OGraphMediaType.Json)))
	                    {
	                    }
	                    else if (collection.Contains(OGraphMediaType.Xml))
	                    {
	                    }
	                    else if (collection.Contains(OGraphMediaType.Json))
	                    {
	                    }
	                    // The user requested an Unsupported media type. - 406 (Not Acceptable)
	                    else
	                    {
	                        return ProcessErrorResultAsJsonAsync(new OGraph)
	                    }
	                }
	                else
	                {
	                    var vertex = context.Element;
	                    var handler = GetChain();
	                    var result = await handler(ctx, cancellationToken);
	                    var task = result switch
	                    {
	                        IOGraphErrorResult error => ProcessErrorResultAsync(error, ctx),
	                        IOGraphQueryResult query => Task.CompletedTask,
	                        IOGraphObjectResult value => Task.CompletedTask
	                    };
	                    return task;
	                }
	            }
	            catch (Exception exception)
	            {
	            }
	        }
	    }
	    Task IOGraphGdmBinding.ExecuteAsync(IOGraphGdmBindingContext context, CancellationToken cancellationToken = default)
	    {
	        if (context is not IOGraphOperationBindingContext)
	        {
	            ThrowHelper.ThrowInvalidOperationException("");
	        }
	        return ExecuteAsync((IOGraphOperationBindingContext)context, cancellationToken);
	    }
	    private async Task ProcessQueryResultAsync(OperationBindingContext context, IOGraphQueryResult result, CancellationToken cancellationToken = default)
	    {
	        var writer = new Either<XmlWriter, Utf8JsonWriter>(new Utf8JsonWriter(context.Response.Body));
	        var element = context.Element;
	        var elementEntity = element.GetGdmEntityType();
	        var query = context.GetQueryDocument();
	        var queryOptions = context.GetQueryOptions();
	        var vertexNode = (VertexNode)query.Root;
	        var projectionNode = vertexNode.Nodes.OfType<ProjectNode>().FirstOrDefault();
	        var edgeNodes = vertexNode.Nodes.OfType<EdgeNode>();
	        writer.Switch(xml => xml.WriteStartElement(""), json => json.WriteStartObject());
	        if (projectionNode is null)
	        {
	            if (!queryOptions.DefaultProjectAll)
	            {
	                return;
	            }
	            writer.Switch(
	                xml =>
	                {
	                },
	                json =>
	                {
	                    json.WriteNumber("@ograph.status", result.StatusCode);
	                    json.WriteNumber("@ograph.total", result.Total);
	                    json.WriteNumber("@ograph.count", result.Count);
	                });
	            foreach (var item in result)
	            {
	                foreach (var property in elementEntity.Properties)
	                {
	                    // Check for property binding
	                    if (property!.HasBinding<IOGraphPropertyBinding>(out var binding))
	                    {
	                    }
	                }
	                //elementEntity.Write()
	            }
	        }
	        else
	        {
	            writer.Switch(
	                xml =>
	                {
	                },
	                json =>
	                {
	                    json.WritePropertyName("data");
	                    json.WriteStartArray();
	                });
	            var propertyBindingContext = new PropertyBindingContext()
	            {
	            };
	            foreach (var item in result)
	            {
	                writer.Switch(
	                    xml => xml.WriteStartElement(element.Label),
	                    json => json.WriteStartObject());
	                foreach (var propertyNode in projectionNode!.Properties)
	                {
	                    var propertyName = propertyNode.Name!;
	                    var tasks = new List<Task>();
	                    if (element.TryGetProperty(propertyName, out var property))
	                    {
	                        if (propertyNode.HasChildren)
	                        {
	                            if (property!.Type.Definition is IOGraphGdmComplexType complexType)
	                            {
	                            }
	                            else if (property!.Type.Definition is IOGraphGdmCollectionType collectionType &&
	                                collectionType.ItemType is IOGraphGdmComplexType complexType1)
	                            {
	                            }
	                        }
	                        // Check for property binding
	                        if (property!.HasBinding<IOGraphPropertyBinding>(out var binding))
	                        {
	                            tasks.Add(binding!.ExecuteAsync(propertyBindingContext, cancellationToken));
	                        }
	                        else
	                        {
	                        }
	                    }
	                    else
	                    {
	                        throw new InvalidOperationException("Invalid projection");
	                    }
	                }
	                foreach (var edgeNode in edgeNodes)
	                {
	                }
	                writer.Switch(
	                    xml => xml.WriteEndElement(),
	                    json => json.WriteEndObject());
	            }
	            writer.Switch(
	                xml =>
	                {
	                },
	                json =>
	                {
	                    json.WriteEndArray();
	                });
	        }
	        writer.Switch(xml => xml.WriteEndElement(), json => json.WriteEndObject());
	    }
	    private OGraphOperationBindingMiddlewareHandler GetChain()
	    {
	        var index = 0;
	        var root = new OGraphOperationBindingMiddlewareHandler(Resolver.InvokeAsync);
	        if (Middleware.Count == 0)
	        {
	            return root;
	        }
	        return Chain(root);
	        OGraphOperationBindingMiddlewareHandler Chain(OGraphOperationBindingMiddlewareHandler root)
	        {
	            var middleware = Middleware.Reverse().Skip(index).First();
	            var next = new OGraphOperationBindingMiddlewareHandler((context, cancellationToken) =>
	            {
	                return middleware.InvokeAsync(context, cancellationToken, root);
	            });
	            if (index < Middleware.Count - 1)
	            {
	                index++;
	                return Chain(next);
	            }
	            return next;
	        }
	    }
	}
	internal partial class OperationBinding
	{
	    private async Task ProcessErrorResultAsync(IOGraphErrorResult result, OperationBindingContext context)
	    {
	        var hasAcceptHeader = context.Request.Headers.TryGetValue("Accept", out var accept);
	        if (hasAcceptHeader)
	        {
	            if (accept.Equals(new[] { OGraphMediaType.Xml, OGraphMediaType.Json}))
	            {
	            }
	            if (accept.Equals(OGraphMediaType.Json))
	            {
	            }
	            if (accept.Equals(OGraphMediaType.Xml))
	            {
	            }
	        }
	    }
	    private Task ProcessErrorResultAsJsonAsync(IOGraphErrorResult result, OperationBindingContext context)
	    {
	        var writer = new Utf8JsonWriter(context.Response.Body);
	        writer.WriteStartObject();
	        writer.WriteNumber("@ograph.status", result.StatusCode);
	        writer.WritePropertyName("error");
	        writer.WriteStartObject();
	        writer.WriteString("code", result.Error.Code);
	        writer.WriteString("message", result.Error.Message);
	        if (result.Error.Details is not null && result.Error.Details.Any())
	        {
	            writer.WriteStartArray();
	            foreach (var detail in result.Error.Details)
	            {
	                writer.WriteStartObject();
	                writer.WriteString("title", detail.Title);
	                writer.WriteString("message", detail.Message);
	                writer.WriteEndObject();
	            }
	            writer.WriteEndArray();
	        }
	        writer.WriteEndObject();
	        writer.WriteEndObject();
	        return Task.CompletedTask;
	    }
	}
	internal class OperationBindingContext : IOGraphOperationBindingContext
	{
	    public IOGraphGdmVertex Element { get; init; } = default!;
	    public IOGraphExecutorRequest Request { get; init; } = default!;
	    public IOGraphExecutorResponse Response { get; init; } = default!;
	    public IOGraphGdmType RequestType { get; init; } = default!;
	    public IServiceProvider ServiceProvider { get; init; } = default!;
	    public QueryParser Parser { get; init; } = default!;
	    IOGraphGdmLabeledElement IOGraphGdmBindingContext.Element => Element;
	    public IOGraphOperationBinding Binding { get; init; } = default!;
	    public Either<XmlWriter, Utf8JsonWriter> GetWriter()
	    {
	        throw new Exception();
	    }
	    public ClaimsPrincipal GetClaimsPrincipal()
	    {
	        throw new NotImplementedException();
	    }
	    public QueryDocument GetQueryDocument()
	    {
	        throw new NotImplementedException();
	    }
	    public OGraphQueryOptions GetQueryOptions()
	    {
	        throw new NotImplementedException();
	    }
	    public T GetQueryParam<T>()
	    {
	        throw new NotImplementedException();
	    }
	    public IOGraphQueryProvider GetQueryProvider()
	    {
	        throw new NotImplementedException();
	    }
	    public T GetRequestBody<T>()
	    {
	        return Request.Headers.Accept.Value switch
	        {
	            OGraphMediaType.Xml => GetXmlRequestBody<T>(),
	            OGraphMediaType.Json => GetJsonRequestBody<T>()
	        };
	    }
	    private T GetXmlRequestBody<T>()
	    {
	        throw new NotImplementedException();
	    }
	    private T GetJsonRequestBody<T>()
	    {
	        var buffer = new byte[0];
	        var reader = new Utf8JsonReader();
	        if (RequestType.Read(ref reader) is T value)
	        {
	            return value;
	        }
	        throw new Exception();
	    }
	    public T GetService<T>()
	    {
	        if (ServiceProvider.GetService(typeof(T)) is T service)
	        {
	            return service;
	        }
	        throw new Exception();
	    }
	    public T GetRouteValue<T>(string paramName)
	    {
	        var routeSegments = Binding.Route.Segments;
	        var pathSegments = Request.Path.Segments;
	        for (int i = 0; i < pathSegments.Length; i++)
	        {
	            var rs = routeSegments[i];
	            var ps = pathSegments[i];
	            if (rs.SegmentType == RouteSegmentType.Parameter && r) 
	        }
	    }
	}
	internal class OperationBindingDescriptor : IOGraphOperationBindingDescriptor
	{
	    private readonly OperationBinding binding;
	    public OperationBindingDescriptor(OperationBinding binding)
    {
        this.binding = binding;
    }
	    public OGraphExecutorOptions Options { get; init; }
	    public IOGraphOperationBindingDescriptor UseMiddleware<TMiddleware>()
	        where TMiddleware : IOGraphOperationBindingMiddleware, new()
	    {
	        return UseMiddleware(new TMiddleware());
	    }
	    public IOGraphOperationBindingDescriptor UseMiddleware(OGraphOperationBindingMiddleware middleware)
	    {
	        if (middleware is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(middleware));
	        }
	        return UseMiddleware(new OperationBindingMiddlewareWrapper(middleware));
	    }
	    public IOGraphOperationBindingDescriptor UseMiddleware(IOGraphOperationBindingMiddleware middleware)
	    {
	        if (middleware is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(middleware));
	        }
	        binding.Middleware.Enqueue(middleware);
	        return this;
	    }
	    public IOGraphOperationBindingDescriptor UseRequestType<TGdmType>() where TGdmType : IOGraphGdmType
	    {
	        throw new NotImplementedException();
	    }
	    public IOGraphOperationBindingDescriptor UseResolver<TResolver>()
	        where TResolver : IOGraphOperationBindingResolver, new()
	    {
	        return UseResolver(new TResolver());
	    }
	    public IOGraphOperationBindingDescriptor UseResolver(OGraphOperationBindingResolver resolver)
	    {
	        if (resolver is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(resolver));
	        }
	        return UseResolver(new OperationBindingResolverWrapper(resolver));
	    }
	    public IOGraphOperationBindingDescriptor UseResolver(IOGraphOperationBindingResolver resolver)
	    {
	        if (resolver is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(resolver));
	        }
	        binding.Resolver = resolver;
	        return this;
	    }
	    public IOGraphOperationBindingDescriptor UseRoute(Route route)
	    {
	        // Check if a route prefix was set in the options
	        if (!string.IsNullOrEmpty(Options.RoutePrefix))
	        {
	            // Check if the prefix has already been provided
	            if (route.Segments[0].Value.Equals(Options.RoutePrefix, StringComparison.OrdinalIgnoreCase))
	            {
	                binding.Route = route;
	            }
	            else
	            {
	                binding.Route = Route.Combine(Options.RoutePrefix, route);
	            }
	        }
	        else
	        {
	            binding.Route = route;
	        }
	        return this;
	    }
	}
	internal class OperationBindingQueryParsingMiddleware : IOGraphOperationBindingMiddleware
	{
	    public Task<IOGraphResult> InvokeAsync(
	        IOGraphOperationBindingContext context, 
	        CancellationToken cancellationToken, 
	        OGraphOperationBindingMiddlewareHandler next)
	    {
	        try
	        {
	            if (context is not OperationBindingContext ctx)
	            {
	                throw new Exception();
	            }
	            var request = ctx.Request;
	            // 1. Check for query
	            if (request.Query.TryGetValue(QueryKey.Query, out var queryValue))
	            {
	                var queryParser = ctx.Parser;
	                var queryDocument = queryParser.Parse(queryValue);
	                // Check Query Validation
	                if (!queryDocument.IsValid)
	                {
	                }
	            }
	            return next.Invoke(context, cancellationToken);
	        }
	        catch (Exception exception)
	        {
	            throw;
	        }
	    }
	}
	internal class OperationBindingMiddlewareQueue : MiddlewareQueueBase<IOGraphOperationBindingMiddleware>,
	    IOGraphOperationBindingMiddlewareQueue
	{
	}
	internal class OperationBindingMiddlewareWrapper : IOGraphOperationBindingMiddleware
	{
	    private readonly OGraphOperationBindingMiddleware middleware;
	    public OperationBindingMiddlewareWrapper(OGraphOperationBindingMiddleware middleware)
    {
        this.middleware = middleware;
    }
	    public Task<IOGraphResult> InvokeAsync(IOGraphOperationBindingContext context, CancellationToken cancellationToken, OGraphOperationBindingMiddlewareHandler next)
	    {
	        return middleware.Invoke(context, cancellationToken, next);
	    }
	}
	internal class OperationBindingResolverWrapper : IOGraphOperationBindingResolver
	{
	    private readonly OGraphOperationBindingResolver resolver;
	    public OperationBindingResolverWrapper(OGraphOperationBindingResolver resolver)
	    {
	        this.resolver = resolver;
	    }
	    public Task<IOGraphResult> InvokeAsync(IOGraphOperationBindingContext context, CancellationToken cancellationToken = default)
	    {
	        return resolver.Invoke(context, cancellationToken);
	    }
	}
	internal abstract class OperationBindingResultStrategy
	{
	    internal abstract Type ResultType { get; }
	}
	internal class PropertyBinding : IOGraphPropertyBinding
	{
	    public PropertyBinding()
	    {
	    }
	    public IOGraphPropertyBindingResolver Resolver { get; set; } = default!;
	    public IOGraphPropertyBindingMiddlewareQueue Middleware { get; } = default!;
	    public async Task ExecuteAsync(IOGraphPropertyBindingContext context, CancellationToken cancellationToken = default)
	    {
	        if (context is not PropertyBindingContext propertyContext)
	        {
	            throw new InvalidOperationException();
	        }
	        var property = propertyContext.Element!;
	        var propertyType = property.Type.Definition;
	        var propertyParent = propertyContext.Parent;
	        var propertySetter = property.Setter!;
	        var propertyResult = await GetChain().Invoke(propertyContext, cancellationToken);
	        if (propertyResult is IOGraphError error)
	        {
	            propertyContext.Errors.Add(error);
	        }
	        else if (propertyResult is IOGraphPropertyResult success)
	        {
	            propertySetter.Invoke(
	                propertyParent, 
	                success.Value!);
	            if (propertyType is IOGraphGdmComplexType complexType)
	            {
	                foreach (var prop in complexType.Properties)
	                {
	                }
	            }
	            else if (propertyType is IOGraphGdmCollectionType collectionType)
	            {
	            }
	            if (property.HasBinding<IOGraphPropertyBinding>(out var binding))
	            {
	            }
	        }
	        else
	        {
	            throw new InvalidOperationException("Expected result is invalid");
	        }
	    }
	    Task IOGraphGdmBinding.ExecuteAsync(IOGraphGdmBindingContext context, CancellationToken cancellationToken)
	    {
	        if (context is not IOGraphPropertyBindingContext)
	        {
	            ThrowHelper.ThrowInvalidOperationException("");
	        }
	        return ExecuteAsync((IOGraphPropertyBindingContext)context, cancellationToken);
	    }
	    private OGraphPropertyBindingMiddlewareHandler GetChain()
	    {
	        var index = 0;
	        var root = new OGraphPropertyBindingMiddlewareHandler(Resolver.InvokeAsync);
	        if (Middleware.Count == 0)
	        {
	            return root;
	        }
	        return Chain(root);
	        OGraphPropertyBindingMiddlewareHandler Chain(OGraphPropertyBindingMiddlewareHandler root)
	        {
	            var middleware = Middleware.Reverse().Skip(index).First();
	            var next = new OGraphPropertyBindingMiddlewareHandler((context, cancellationToken) =>
	            {
	                return middleware.InvokeAsync(context, cancellationToken, root);
	            });
	            if (index < Middleware.Count - 1)
	            {
	                index++;
	                return Chain(next);
	            }
	            return next;
	        }
	    }
	}
	internal class PropertyBindingContext : IOGraphPropertyBindingContext
	{
	    internal volatile object Parent;
	    public IList<IOGraphError> Errors { get; init; } = new List<IOGraphError>();
	    public IOGraphGdmProperty Element { get; init; }
	    public IOGraphExecutorRequest Request { get; init; }
	    public IOGraphExecutorResponse Response { get; init; }
	    public IServiceProvider ServiceProvider { get; init; }
	    public PropertyNode Node { get; init; }
	    public T GetParent<T>()
	    {
	        if (Parent is not T parent)
	        {
	            throw new InvalidOperationException();
	        }
	        return parent;
	    }
	    public T GetService<T>()
	    {
	        if (ServiceProvider?.GetService(typeof(T)) is T service)
	        {
	            return service;
	        }
	        throw new Exception();
	    }
	    public ClaimsPrincipal GetClaimsPrincipal()
	    {
	        throw new NotImplementedException();
	    }
	    IOGraphGdmLabeledElement IOGraphGdmBindingContext.Element => Element;
	}
	internal class PropertyBindingMiddlewareQueue : MiddlewareQueueBase<IOGraphPropertyBindingMiddleware>, 
	    IOGraphPropertyBindingMiddlewareQueue
	{
	}
	internal class PropertyBindingResolver : IOGraphPropertyBindingResolver
	{
	    private readonly OGraphPropertyBindingResolver resolver;
	    public PropertyBindingResolver(OGraphPropertyBindingResolver resolver)
	    {
	        this.resolver = resolver;
	    }
	    public Task<IOGraphResult> InvokeAsync(IOGraphPropertyBindingContext context, CancellationToken cancellationToken)
	    {
	        return resolver.Invoke(context, cancellationToken);
	    }
	}
	#endregion
	#region \Internal\QueryProviders
	internal class QueryableQueryProvider : IOGraphQueryProvider
	{
	    private readonly IQueryable queryable;
	    public QueryableQueryProvider(IQueryable queryable)
	    {
	        this.queryable = queryable;
	    }
	    public Type ElementType => throw new NotImplementedException();
	    public async Task ExecuteAsync(IOGraphQueryProviderContext context, OGraphQueryOptions options, CancellationToken cancellationToken = default)
	    {
	        //var query = context.Query;
	        //var vertex = context.Vertex;
	        //if (query.Root is not VertexNode vertexNode)
	        //{
	        //    throw new Exception();
	        //}
	        //foreach (var edgeNode in vertexNode.Nodes.OfType<EdgeNode>())
	        //{
	        //    var label = edgeNode.Label.Name!;
	        //    var edge = vertex.Edges
	        //        .Where(p => p.Definition is IOGraphGdmEdge e && e.Label == label)
	        //        .Select(p => p.Definition)
	        //        .First();
	        //    var edgeBinding = edge.Bindings.OfType<IOGraphOperationBinding>().First();
	        //}
	        //var projectionNode = vertexNode.Nodes.OfType<ProjectionNode>().First();
	        //var projectionType = ApplyProjections(projectionNode, vertex);
	        //var writer = new Utf8JsonWriter(context.Stream, new()
	        //{
	        //    SkipValidation = true,
	        //});
	        //var items = new List<object>();
	        //var tasks = new List<Task>();
	        //writer.WriteStartObject();
	        //writer.WritePropertyName("data");
	        //writer.WriteStartArray();
	        //int index = 0;
	        //var propertyContext = new PropertyBindingContext()
	        //{
	        //};
	        //foreach (var item in queryable)
	        //{
	        //    propertyContext.Parent = item;
	        //    await foreach (var property in GetPropertiesAsync(propertyContext, options, out IOGraphError? error))
	        //    {
	        //        if (error is not null)
	        //        {
	        //        }
	        //    }
	        //}
	        //writer.WriteEndArray();
	    }
	    private IAsyncEnumerable<IOGraphGdmProperty> GetPropertiesAsync(
	        IOGraphPropertyBindingContext context, 
	        OGraphQueryOptions options,
	        out IOGraphError? error)
	    {
	        error = null;
	        //var query = context.Query;
	        //var vertex = context.Vertex;
	        //if (query.Root is not VertexNode vertexNode)
	        //{
	        //    throw new Exception();
	        //}
	        throw new NotImplementedException();
	    }
	    private IEnumerable<IOGraphGdmProperty> GetProperties(IOGraphGdmVertex vertex, VertexNode node)
	    {
	        foreach (var propertyNode in node.Nodes.OfType<ProjectNode>().First().Properties)
	        {
	            var propertyName = propertyNode.Name!;
	            if (!vertex.TryGetProperty(propertyName, out var property))
	            {
	                throw new Exception();
	            }
	            if (propertyNode.HasChildren)
	            {
	            }
	            else
	            {
	            }
	        }
	        // Let's asynchronously complete tasks as they finish
	        //while (tasks.Any())
	        //{
	        //    var task = await Task.WhenAny(tasks);
	        //    tasks.Remove(task);
	        //}
	        throw new NotImplementedException();
	    }
	    private IEnumerable<IOGraphGdmEdge> GetEdgeBindings(IOGraphGdmVertex vertex, VertexNode node)
	    {
	        foreach (var edgeNode in node.Nodes.OfType<EdgeNode>())
	        {
	            var label = edgeNode.Label.Name!;
	            yield return vertex.Edges
	                .Where(p => p.Definition is IOGraphGdmEdge e && e.Label == label)
	                .Select(p => p.Definition)
	                .First();
	        }
	    }
	    private IOGraphPropertyBinding GetBinding(IOGraphGdmProperty property)
	    {
	        throw new NotImplementedException();
	    }
	    protected void ApplyFiltering(OGraphQueryOptions options)
	    {
	        if (!options.CanFilter)
	        {
	            throw new Exception();
	        }
	    }
	    protected void ApplySorting(OGraphQueryOptions options)
	    {
	        if (!options.CanSort)
	        {
	            throw new Exception();
	        }
	    }
	    protected void ApplyPaging(OGraphQueryOptions options)
	    {
	        if (!options.CanPage)
	        {
	            throw new Exception();
	        }
	    }
	    private IOGraphGdmCollectionType ApplyProjections(ProjectNode node, IOGraphGdmVertex vertex)
	    {
	        var collectionType = new GdmListType<GdmComplexType>();
	        var complexType = new GdmComplexType();
	        foreach (var propertyNode in node.Properties)
	        {
	            if (vertex.TryGetProperty(propertyNode.Name!, out var property))
	            {
	               // var bindings = property!.GetBindings().OfType<IOGraphPropertyBinding>();
	                //var binding = bindings.First();
	            }
	        }
	        return collectionType;
	    }
	    Task<IOGraphQueryResult> IOGraphQueryProvider.ExecuteAsync(IOGraphQueryProviderContext context, OGraphQueryOptions options, CancellationToken cancellationToken)
	    {
	        throw new NotImplementedException();
	    }
	}
	internal class QueryContext : IOGraphQueryProviderContext
	{
	    public IOGraphGdmVertex Vertex { get; init; } = default!;
	    public QueryDocument Query { get; init; } = default!;
	    public IServiceProvider ServiceProvider { get; init; } = default!;
	    public Stream Stream { get; init; } = default!;
	    public QueryDocument QueryDocument => throw new NotImplementedException();
	    public IOGraphGdmLabeledElement Element => throw new NotImplementedException();
	}
	internal class QueryProvider : IOGraphQueryProvider
	{
	    private static readonly HashSet<QueryProviderStrategy> strategies = new();
	    static QueryProvider()
	    {
	    }
	    internal static void AddStrategy<TStrategy>() where TStrategy : QueryProviderStrategy, new()
	    {
	        strategies.Add(new TStrategy());
	    }
	    public Type ElementType { get; init; } = default!;
	    public Task ExecuteAsync(IOGraphQueryProviderContext context, OGraphQueryOptions options, CancellationToken cancellationToken = default)
	    {
	        throw new NotImplementedException();
	    }
	    Task<IOGraphQueryResult> IOGraphQueryProvider.ExecuteAsync(IOGraphQueryProviderContext context, OGraphQueryOptions options, CancellationToken cancellationToken)
	    {
	        throw new NotImplementedException();
	    }
	    /* Execution Plan #1
	       1.       Get Projections
	       1.1          Invoke Projections
	       1.2          
	     */
	}
	internal class QueryProvider<T> : QueryProvider 
	{
	    public QueryProvider()
	    {
	        ElementType = typeof(T);
	    }
	}
	internal abstract class QueryProviderStrategy
	{
	    public abstract Type ElementType { get; }
	    public abstract Task ExecuteAsync(
	        IOGraphQueryProviderContext context,
	        OGraphQueryOptions options,
	        CancellationToken cancellationToken = default);
	}
	internal class QueryableQueryProviderStrategy : QueryProviderStrategy
	{
	    public override Type ElementType => throw new NotImplementedException();
	    public override Task ExecuteAsync(IOGraphQueryProviderContext context, OGraphQueryOptions options, CancellationToken cancellationToken = default)
	    {
	        throw new NotImplementedException();
	    }
	}
	#endregion
	#region \Internal\Results
	internal class ErrorResult : OGraphResult, IOGraphErrorResult
	{
	    public ErrorResult()
    {
        
    }
	    public IOGraphError Error { get; }
	}
	    internal class QueryResult
	    {
	    }
	internal abstract class ResultProvider
	{
	    internal abstract Task HandleAsync(IOGraphResult result, OperationBindingContext context, CancellationToken cancellationToken);
	}
	internal class QueryResultProvider : ResultProvider
	{
	    internal override Task HandleAsync(IOGraphResult result, OperationBindingContext context, CancellationToken cancellationToken)
	    {
	        throw new NotImplementedException();
	    }
	}
	#endregion
	#region \Internal\Utilities
	internal static class ThrowHelper
	{
	    [DoesNotReturn]
	    internal static void ThrowArgumentException(string message) =>
	        throw new ArgumentException(message);
	    [DoesNotReturn]
	    internal static void ThrowInvalidOperationException(string message) => 
	        throw new InvalidOperationException(message);
	    [DoesNotReturn]
	    internal static void ThrowArgumentNullException(string paramName) =>
	        throw new ArgumentNullException(paramName);
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
	#region \Results
	public abstract class OGraphResult : IOGraphResult
	{
	    public OGraphResult() { }
	#if NET7_0_OR_GREATER
	    [SetsRequiredMembers]
#endif
    public OGraphResult(StatusCode statusCode)
    {
        StatusCode = statusCode;
    }
	    public virtual
	#if NET7_0_OR_GREATER
	    required
	#endif
	    StatusCode StatusCode { get; init; }
	    #region Error Results
	    public static IOGraphErrorResult Unauthorized()
	    {
	        return Unauthorized("The user is not authorized to access this resource.");
	    }
	    public static IOGraphErrorResult Unauthorized(string message)
	    {
	        return Unauthorized(error =>
	        {
	            error.Code = "Unauthorized";
	            error.Message = message;
	        });
	    }
	    public static IOGraphErrorResult Unauthorized(Action<OGraphError> configure) => throw new NotImplementedException();
	    public static IOGraphErrorResult BadRequest()
	    {
	        return BadRequest("The request is invalid.");
	    }
	    public static IOGraphErrorResult BadRequest(string message)
	    {
	        return BadRequest(error =>
	        {
	            error.Code = "BadRequest";
	            error.Message = message;
	        });
	    }
	    public static IOGraphErrorResult BadRequest(Action<OGraphError> configure) => throw new NotImplementedException();
	    #endregion
	}
	    public abstract class OGraphErrorResult : OGraphResult
	    {
	    }
	public class QueryResult : IOGraphQueryResult
	{
	    private readonly IEnumerable enumerable;
	    public QueryResult(IEnumerable enumerable, long total, long count)
    {
        this.enumerable = enumerable;
        this.Total = total;
        this.Count = count;
    }
	    public StatusCode StatusCode => 200;
	    public long Total { get; }
	    public long Count { get; }
	    public IEnumerator GetEnumerator() => enumerable.GetEnumerator();
	}
	public class QueryResult<T> : QueryResult, IOGraphQueryResult<T>
	{
	    public QueryResult()
    {
        
    }
	    public long Total => throw new NotImplementedException();
	    public long Count => throw new NotImplementedException();
	    public StatusCode StatusCode => throw new NotImplementedException();
	    public IEnumerator<T> GetEnumerator()
	    {
	        throw new NotImplementedException();
	    }
	    IEnumerator IEnumerable.GetEnumerator()
	    {
	        throw new NotImplementedException();
	    }
	}
	#endregion
	#region \Results\Errors
	    public sealed class OGraphBadRequestResult
	    {
	    }
	    internal class OGraphUnauthorizedResult
	    {
	    }
	#endregion
	#region \Utilities
	public static class RouteMatcher
	{
	}
	#endregion
	#region \ValueObjects
	[DebuggerDisplay("{Value}")]
	public readonly struct HeaderKey : 
	    IEquatable<HeaderKey>,
	    IEqualityComparer<HeaderKey>,
	    IComparable<HeaderKey>
	{
	    public HeaderKey(string value)
	    {
	        if (string.IsNullOrEmpty(value)) 
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(value));
	        }
	        Value = value;
	    }
	    public string Value { get; }
	    public override bool Equals(object? instance)
	    {
	        if (instance is HeaderKey headerKey) 
	        {
	            return Equals(headerKey);
	        }
	        return false;
	    }
	    public override int GetHashCode()
	    {
	        return HashCode.Combine(typeof(HeaderKey), Value.ToLower());
	    }
	    public override string ToString()
	    {
	        return Value;
	    }
	    public bool Equals(HeaderKey other)
	    {
	        return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
	    }
	    int IComparable<HeaderKey>.CompareTo(HeaderKey other)
	    {
	        var left = Value ?? string.Empty;
	        var right = other.Value ?? string.Empty;
	        var length = Math.Min(left.Length, right.Length); // Only need to compare up to the min length of either value
	        for (int i = 0; i < length; i++)
	        {
	            var a = char.ToLower(left[i]);
	            var b = char.ToLower(right[i]);
	            var c = a - b;
	            if (c != 0)
	            {
	                return c;
	            }
	        }
	        return 0;
	    }
	    bool IEqualityComparer<HeaderKey>.Equals(HeaderKey left, HeaderKey right)
	    {
	        return left.Equals(right);
	    }
	    int IEqualityComparer<HeaderKey>.GetHashCode(HeaderKey instance)
	    {
	        return instance.GetHashCode();
	    }
	    public static implicit operator string(HeaderKey key) => key.Value;
	    public static implicit operator HeaderKey(string value) => new HeaderKey(value);
	    public static HeaderKey ContentType => "Content-Type";
	    public static HeaderKey ContentLength => "Content-Length";
	    public static HeaderKey Accept => "Accept";
	}
	[DebuggerDisplay("{Value}")]
	public readonly partial struct HeaderValue :
	    IList<string?>,
	    IReadOnlyList<string?>,
	    IEquatable<HeaderValue>,
	    IEquatable<string?>,
	    IEquatable<string?[]?>
	{
	    public static readonly HeaderValue Empty = new HeaderValue(Array.Empty<string>());
	    private readonly object? values;
	    public HeaderValue(string? value)
	    {
	        values = value;
	    }
	    public HeaderValue(string?[]? values)
	    {
	        this.values = values;
	    }
	    public int Count
	    {
	        [MethodImpl(MethodImplOptions.AggressiveInlining)]
	        get
	        {
	            // Take local copy of values so type checks remain valid even if the StringValues is overwritten in memory
	            object? value = values;
	            if (value is null)
	            {
	                return 0;
	            }
	            if (value is string)
	            {
	                return 1;
	            }
	            else
	            {
	                // Not string, not null, can only be string[]
	                return Unsafe.As<string?[]>(value).Length;
	            }
	        }
	    }
	    public string? Value
	    {
	        get
	        {
	            if (values is string str)
	            {
	                return str;
	            }
	            if (values is string[] strArr)
	            {
	                return string.Join(';', strArr);
	            }
	            return null;
	        }
	    }
	    bool ICollection<string?>.IsReadOnly => true;
	    public static implicit operator HeaderValue(string? value)
	    {
	        return new HeaderValue(value);
	    }
	    public static implicit operator HeaderValue(string?[]? values)
	    {
	        return new HeaderValue(values);
	    }
	    public static implicit operator string?(HeaderValue values)
	    {
	        return values.GetStringValue();
	    }
	    public static implicit operator string?[]?(HeaderValue value)
	    {
	        return value.GetArrayValue();
	    }
	    string? IList<string?>.this[int index]
	    {
	        get => this[index];
	        set => throw new NotSupportedException();
	    }
	    public string? this[int index]
	    {
	        [MethodImpl(MethodImplOptions.AggressiveInlining)]
	        get
	        {
	            // Take local copy of values so type checks remain valid even if the StringValues is overwritten in memory
	            object? value = values;
	            if (value is string str)
	            {
	                if (index == 0)
	                {
	                    return str;
	                }
	            }
	            else if (value != null)
	            {
	                // Not string, not null, can only be string[]
	                return Unsafe.As<string?[]>(value)[index]; // may throw
	            }
	            return OutOfBounds(); // throws
	        }
	    }
	    [MethodImpl(MethodImplOptions.NoInlining)]
	    private static string OutOfBounds()
	    {
	        return Array.Empty<string>()[0]; // throws
	    }
	    public override string ToString()
	    {
	        return GetStringValue() ?? string.Empty;
	    }
	    private string? GetStringValue()
	    {
	        // Take local copy of values so type checks remain valid even if the StringValues is overwritten in memory
	        object? value = values;
	        if (value is string s)
	        {
	            return s;
	        }
	        else
	        {
	            return GetStringValueFromArray(value);
	        }
	        static string? GetStringValueFromArray(object? value)
	        {
	            if (value is null)
	            {
	                return null;
	            }
	            Debug.Assert(value is string[]);
	            // value is not null or string, array, can only be string[]
	            string?[] values = Unsafe.As<string?[]>(value);
	            return values.Length switch
	            {
	                0 => null,
	                1 => values[0],
	                _ => GetJoinedStringValueFromArray(values),
	            };
	        }
	        static string GetJoinedStringValueFromArray(string?[] values)
	        {
	            // Calculate final length
	            int length = 0;
	            for (int i = 0; i < values.Length; i++)
	            {
	                string? value = values[i];
	                // Skip null and empty values
	                if (value != null && value.Length > 0)
	                {
	                    if (length > 0)
	                    {
	                        // Add separator
	                        length++;
	                    }
	                    length += value.Length;
	                }
	            }
	            // Create the new string
	            return string.Create(length, values, (span, strings) => {
	                int offset = 0;
	                // Skip null and empty values
	                for (int i = 0; i < strings.Length; i++)
	                {
	                    string? value = strings[i];
	                    if (value != null && value.Length > 0)
	                    {
	                        if (offset > 0)
	                        {
	                            // Add separator
	                            span[offset] = ',';
	                            offset++;
	                        }
	                        value.AsSpan().CopyTo(span.Slice(offset));
	                        offset += value.Length;
	                    }
	                }
	            });
	        }
	    }
	    public string?[] ToArray()
	    {
	        return GetArrayValue() ?? Array.Empty<string>();
	    }
	    private string?[]? GetArrayValue()
	    {
	        // Take local copy of values so type checks remain valid even if the StringValues is overwritten in memory
	        object? value = this.values;
	        if (value is string[] values)
	        {
	            return values;
	        }
	        else if (value != null)
	        {
	            // value not array, can only be string
	            return new[] { Unsafe.As<string>(value) };
	        }
	        else
	        {
	            return null;
	        }
	    }
	    int IList<string?>.IndexOf(string? item)
	    {
	        return IndexOf(item);
	    }
	    private int IndexOf(string? item)
	    {
	        // Take local copy of values so type checks remain valid even if the StringValues is overwritten in memory
	        object? value = this.values;
	        if (value is string[] values)
	        {
	            for (int i = 0; i < values.Length; i++)
	            {
	                if (string.Equals(values[i], item, StringComparison.Ordinal))
	                {
	                    return i;
	                }
	            }
	            return -1;
	        }
	        if (value != null)
	        {
	            // value not array, can only be string
	            return string.Equals(Unsafe.As<string>(value), item, StringComparison.Ordinal) ? 0 : -1;
	        }
	        return -1;
	    }
	    bool ICollection<string?>.Contains(string? item)
	    {
	        return IndexOf(item) >= 0;
	    }
	    void ICollection<string?>.CopyTo(string?[] array, int arrayIndex)
	    {
	        CopyTo(array, arrayIndex);
	    }
	    private void CopyTo(string?[] array, int arrayIndex)
	    {
	        // Take local copy of values so type checks remain valid even if the StringValues is overwritten in memory
	        object? value = this.values;
	        if (value is string[] values)
	        {
	            Array.Copy(values, 0, array, arrayIndex, values.Length);
	            return;
	        }
	        if (value != null)
	        {
	            if (array == null)
	            {
	                throw new ArgumentNullException(nameof(array));
	            }
	            if (arrayIndex < 0)
	            {
	                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
	            }
	            if (array.Length - arrayIndex < 1)
	            {
	                throw new ArgumentException(
	                    $"'{nameof(array)}' is not long enough to copy all the items in the collection. Check '{nameof(arrayIndex)}' and '{nameof(array)}' length.");
	            }
	            // value not array, can only be string
	            array[arrayIndex] = Unsafe.As<string>(value);
	        }
	    }
	    void ICollection<string?>.Add(string? item) => throw new NotSupportedException();
	    void IList<string?>.Insert(int index, string? item) => throw new NotSupportedException();
	    bool ICollection<string?>.Remove(string? item) => throw new NotSupportedException();
	    void IList<string?>.RemoveAt(int index) => throw new NotSupportedException();
	    void ICollection<string?>.Clear() => throw new NotSupportedException();
	    public Enumerator GetEnumerator()
	    {
	        return new Enumerator(values);
	    }
	    IEnumerator<string?> IEnumerable<string?>.GetEnumerator()
	    {
	        return GetEnumerator();
	    }
	    IEnumerator IEnumerable.GetEnumerator()
	    {
	        return GetEnumerator();
	    }
	    public static bool IsNullOrEmpty(HeaderValue value)
	    {
	        object? data = value.values;
	        if (data is null)
	        {
	            return true;
	        }
	        if (data is string[] values)
	        {
	            return values.Length switch
	            {
	                0 => true,
	                1 => string.IsNullOrEmpty(values[0]),
	                _ => false,
	            };
	        }
	        else
	        {
	            // Not array, can only be string
	            return string.IsNullOrEmpty(Unsafe.As<string>(data));
	        }
	    }
	    public static HeaderValue Concat(HeaderValue values1, HeaderValue values2)
	    {
	        int count1 = values1.Count;
	        int count2 = values2.Count;
	        if (count1 == 0)
	        {
	            return values2;
	        }
	        if (count2 == 0)
	        {
	            return values1;
	        }
	        var combined = new string[count1 + count2];
	        values1.CopyTo(combined, 0);
	        values2.CopyTo(combined, count1);
	        return new HeaderValue(combined);
	    }
	    public static HeaderValue Concat(in HeaderValue values, string? value)
	    {
	        if (value == null)
	        {
	            return values;
	        }
	        int count = values.Count;
	        if (count == 0)
	        {
	            return new HeaderValue(value);
	        }
	        var combined = new string[count + 1];
	        values.CopyTo(combined, 0);
	        combined[count] = value;
	        return new HeaderValue(combined);
	    }
	    public static HeaderValue Concat(string? value, in HeaderValue values)
	    {
	        if (value == null)
	        {
	            return values;
	        }
	        int count = values.Count;
	        if (count == 0)
	        {
	            return new HeaderValue(value);
	        }
	        var combined = new string[count + 1];
	        combined[0] = value;
	        values.CopyTo(combined, 1);
	        return new HeaderValue(combined);
	    }
	    public static bool Equals(HeaderValue left, HeaderValue right)
	    {
	        int count = left.Count;
	        if (count != right.Count)
	        {
	            return false;
	        }
	        for (int i = 0; i < count; i++)
	        {
	            if (left[i] != right[i])
	            {
	                return false;
	            }
	        }
	        return true;
	    }
	    public static bool operator ==(HeaderValue left, HeaderValue right)
	    {
	        return Equals(left, right);
	    }
	    public static bool operator !=(HeaderValue left, HeaderValue right)
	    {
	        return !Equals(left, right);
	    }
	    public bool Equals(HeaderValue other) => Equals(this, other);
	    public static bool Equals(string? left, HeaderValue right) => Equals(new HeaderValue(left), right);
	    public static bool Equals(HeaderValue left, string? right) => Equals(left, new HeaderValue(right));
	    public bool Equals(string? other) => Equals(this, new HeaderValue(other));
	    public static bool Equals(string?[]? left, HeaderValue right) => Equals(new HeaderValue(left), right);
	    public static bool Equals(HeaderValue left, string?[]? right) => Equals(left, new HeaderValue(right));
	    public bool Equals(string?[]? other) => Equals(this, new HeaderValue(other));
	    public static bool operator ==(HeaderValue left, string? right) => Equals(left, new HeaderValue(right));
	    public static bool operator !=(HeaderValue left, string? right) => !Equals(left, new HeaderValue(right));
	    public static bool operator ==(string? left, HeaderValue right) => Equals(new HeaderValue(left), right);
	    public static bool operator !=(string left, HeaderValue right) => !Equals(new HeaderValue(left), right);
	    public static bool operator ==(HeaderValue left, string?[]? right) => Equals(left, new HeaderValue(right));
	    public static bool operator !=(HeaderValue left, string?[]? right) => !Equals(left, new HeaderValue(right));
	    public static bool operator ==(string?[]? left, HeaderValue right) => Equals(new HeaderValue(left), right);
	    public static bool operator !=(string?[]? left, HeaderValue right) => !Equals(new HeaderValue(left), right);
	    public static bool operator ==(HeaderValue left, object? right) => left.Equals(right);
	    public static bool operator !=(HeaderValue left, object? right) => !left.Equals(right);
	    public static bool operator ==(object? left, HeaderValue right) => right.Equals(left);
	    public static bool operator !=(object? left, HeaderValue right) => !right.Equals(left);
	    public override bool Equals(object? obj)
	    {
	        if (obj == null)
	        {
	            return Equals(this, HeaderValue.Empty);
	        }
	        if (obj is string str)
	        {
	            return Equals(this, str);
	        }
	        if (obj is string[] array)
	        {
	            return Equals(this, array);
	        }
	        if (obj is HeaderValue stringValues)
	        {
	            return Equals(this, stringValues);
	        }
	        return false;
	    }
	    public override int GetHashCode()
	    {
	        object? value = this.values;
	        if (value is string[] values)
	        {
	            if (Count == 1)
	            {
	                return Unsafe.As<string>(this[0])?.GetHashCode() ?? Count.GetHashCode();
	            }
	            int hashCode = 0;
	            for (int i = 0; i < values.Length; i++)
	            {
	                var rol5 = ((uint)hashCode << 5) | ((uint)hashCode >> 27);
	                hashCode = ((int)rol5 + hashCode) ^ values[i]?.GetHashCode() ?? 0;
	            }
	            return hashCode;
	        }
	        else
	        {
	            return Unsafe.As<string>(value)?.GetHashCode() ?? Count.GetHashCode();
	        }
	    }
	    public struct Enumerator : IEnumerator<string?>
	    {
	        private readonly string?[]? _values;
	        private int _index;
	        private string? _current;
	        internal Enumerator(object? value)
	        {
	            if (value is string str)
	            {
	                _values = null;
	                _current = str;
	            }
	            else
	            {
	                _current = null;
	                _values = Unsafe.As<string?[]>(value);
	            }
	            _index = 0;
	        }
	        public Enumerator(ref HeaderValue values) : this(values.values)
	        { }
	        public bool MoveNext()
	        {
	            int index = _index;
	            if (index < 0)
	            {
	                return false;
	            }
	            string?[]? values = _values;
	            if (values != null)
	            {
	                if ((uint)index < (uint)values.Length)
	                {
	                    _index = index + 1;
	                    _current = values[index];
	                    return true;
	                }
	                _index = -1;
	                return false;
	            }
	            _index = -1; // sentinel value
	            return _current != null;
	        }
	        public string? Current => _current;
	        object? IEnumerator.Current => _current;
	        void IEnumerator.Reset()
	        {
	            throw new NotSupportedException();
	        }
	        public void Dispose()
	        {
	        }
	    }
	}
	public readonly struct Host
	{
	    public Host(string value)
	    {
	        this.Value = value;
	    }
	    public Host(string host, int port)
	    {
	        if (host == null)
	        {
	            throw new ArgumentNullException(nameof(host));
	        }
	        if (port <= 0)
	        {
	            throw new ArgumentOutOfRangeException(nameof(port), "");
	        }
	        int index;
	        if (!host.Contains('[')
	            && (index = host.IndexOf(':')) >= 0
	            && index < host.Length - 1
	            && host.IndexOf(':', index + 1) >= 0)
	        {
	            // IPv6 without brackets ::1 is the only type of host with 2 or more colons
	            host = $"[{host}]";
	        }
	        this.Value = host + ":" + port.ToString(CultureInfo.InvariantCulture);
	        this.Port = port;
	    }
	    public string Value { get; }
	    public int? Port { get; }
	    public override string ToString()
	    {
	        return Value;
	    }
	    public static implicit operator string(Host host) => host.Value;
	    public static implicit operator Host(string value) => new Host(value);
	}
	public readonly struct Method : 
		IEquatable<Method>,
		IEqualityComparer<Method>
	{
	    private const string pattern = "^[a-zA-Z]+$"; // Alphabetic characters only
		public Method(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
	            throw new ArgumentNullException(nameof(value));
	        }
			if (!Regex.IsMatch(value, pattern))
			{
	            throw new Exception("Only Alphabetic characters are allowed as Method names");
	        }
			Value = value.ToUpperInvariant();
		}
		public string Value { get; }
		public override bool Equals([NotNullWhen(true)] object? instance)
		{
			if (instance is Method method)
			{
				return Equals(method);
			}
			return false;
		}
		public override int GetHashCode()
		{
			return HashCode.Combine(typeof(Method), Value);
		}
		public override string ToString()
		{
			return Value;
		}
		public bool Equals(Method other)
		{
			return Value.Equals(other.Value);
		}
		public bool Equals(Method left, Method right)
		{
			return left.Equals(right);
		}
		public int GetHashCode([DisallowNull] Method instance)
		{
			return instance.GetHashCode();
		}
		public static implicit operator Method(string value) => new Method(value);
		public static implicit operator string(Method method) => method.Value;
		public static Method Get => "GET";
		public static Method Post => "POST";
		public static Method Put => "PUT";
		public static Method Delete => "DELETE";
		public static Method Patch => "PATCH";
	}
	public enum OperationType
	{
	    Command,
	    Query
	}
	[DebuggerDisplay("Path: /{ToString()}")]
	public readonly struct Path : IEquatable<Path>, IEqualityComparer<Path>
	{
	    private readonly PathSegment[] segments;
	    public Path(string path)
	    {
	        if (string.IsNullOrEmpty(path))
	        {
	            throw new ArgumentNullException(nameof(path));
	        }
	        segments = GetSegments(HttpUtility.UrlDecode(path));
	    }
	    private PathSegment[] GetSegments(string path)
	    {
	        var index = 0;
	        var segments = new PathSegment[10];
	        var segment = string.Empty;
	        for (int i = 0; i < path.Length; i++)
	        {
	            var character = path[i];
	            // Check if we reached the end of the current segment
	            if (character == '/' || character == '\\')
	            {
	                // Let's skip leading slashes
	                if (i == 0) continue;
	                segments[index] = new PathSegment(segment, index);
	                index++;
	                segment = string.Empty;
	                // Lets see if we reached the buffer in the array. Resize if reached.
	                if (index == segments.Length)
	                {
	                    Array.Resize(ref segments, 5);
	                }
	            }
	            else if ((i + 1) >= path.Length)
	            {
	                segment = segment + character;
	                segments[index] = new PathSegment(segment, index);
	                index++;
	                segment = string.Empty;
	            }
	            else
	            {
	                segment = segment + character;
	            }
	        }
	        // Resizes segments to actual length
	        Array.Resize(ref segments, index);
	        return segments;
	    }
	    public PathSegment[] Segments
	    {
	        get
	        {
	            var copy = new PathSegment[segments.Length];
	            segments.CopyTo(copy, 0);
	            return copy;
	        }
	    }
	    public override string ToString()
	    {
	        return string.Join("/", Segments.Select(s => s.Value));
	    }
	    public override int GetHashCode()
	    {
	        var hashCode = new HashCode();
	        foreach (var segment in Segments)
	        {
	            hashCode.Add(segment);
	        }
	        return hashCode.ToHashCode();
	    }
	    public override bool Equals([NotNullWhen(true)] object? instance)
	    {
	        if (instance is Path path)
	        {
	            return Equals(path);
	        }
	        return false;
	    }
	    public bool Equals(Path path)
	    {
	        if (path.Segments.Length != Segments.Length)
	        {
	            return false;
	        }
	        for (int i = 0; i < Segments.Length; i++)
	        {
	            var incoming = path.Segments[i];
	            var current = Segments[i];
	            if (incoming.Value != current.Value)
	            {
	                return false;
	            }
	        }
	        return true;
	    }
	    public bool Equals(Path left, Path right) => left.Equals(right);
	    public int GetHashCode([DisallowNull] Path path)
	    {
	        return path.GetHashCode();
	    }
	    public static implicit operator Path(string path) => new Path(path);
	    public static implicit operator string(Path path) => path.ToString();
	}
	[DebuggerDisplay("Path Segment: {Value}")]
	public readonly struct PathSegment :
	    IEquatable<PathSegment>,
	    IEqualityComparer<PathSegment>
	{
	    internal PathSegment(string value, int ordinal)
	    {
	        if (value is null)
	        {
	            throw new ArgumentNullException(nameof(value));
	        }
	        this.Value = value;
	        this.Ordinal = ordinal;
	    }
	    public string Value { get; }
	    public int Ordinal { get; }
	    public override string ToString()
	    {
	        return Value;
	    }
	    public override bool Equals([NotNullWhen(true)] object? instance)
	    {
	        if (instance is PathSegment segment)
	        {
	            return Equals(segment);
	        }
	        return false;
	    }
	    public bool Equals(PathSegment other)
	    {
	        return Ordinal == other.Ordinal && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
	    }
	    public bool Equals(PathSegment left, PathSegment right)
	    {
	        return left.Equals(right);
	    }
	    public override int GetHashCode()
	    {
	        return HashCode.Combine(typeof(PathSegment), Ordinal, string.Create(Value.Length, Value, (chars, name) =>
	        {
	            name.CopyTo(chars);
	            for (int i = 0; i < chars.Length; i++)
	            {
	                var c = chars[i];
	                if (char.IsUpper(c))
	                {
	                    chars[i] = char.ToLower(c);
	                }
	            }
	        }));
	    }
	    public int GetHashCode([DisallowNull] PathSegment instance)
	    {
	        return instance.GetHashCode();
	    }
	}
	[DebuggerDisplay("{Value}")]
	public readonly struct QueryKey :
	    IEquatable<QueryKey>,
	    IEqualityComparer<QueryKey>,
	    IComparable<QueryKey>
	{
	    public QueryKey(string value)
	    {
	        if (string.IsNullOrEmpty(value)) 
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(value));
	        }
	        Value = value;
	    }
	    public string Value { get; }
	    public override bool Equals(object? instance)
	    {
	        if (instance is QueryKey queryKey)
	        {
	            return Equals(queryKey);
	        }
	        return false;
	    }
	    public override int GetHashCode()
	    {
	        return HashCode.Combine(typeof(QueryKey), Value.ToLower());
	    }
	    public override string ToString()
	    {
	        return Value;
	    }
	    public bool Equals(QueryKey other)
	    {
	        return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
	    }
	    int IComparable<QueryKey>.CompareTo(QueryKey other)
	    {
	        var left = Value ?? string.Empty;
	        var right = other.Value ?? string.Empty;
	        for (int i = 0; i < Math.Min(left.Length, right.Length); i++) // Only need to compare up to the min length of either value
	        {
	            var a = char.ToLower(left[i]);
	            var b = char.ToLower(right[i]);
	            var c = a - b;
	            if (c != 0) return c;
	        }
	        return 0;
	    }
	    bool IEqualityComparer<QueryKey>.Equals(QueryKey left, QueryKey right)
	    {
	        return left.Equals(right);
	    }
	    int IEqualityComparer<QueryKey>.GetHashCode(QueryKey instance)
	    {
	        return instance.GetHashCode();
	    }
	    public static implicit operator string(QueryKey key) => key.Value;
	    public static implicit operator QueryKey(string value) => new QueryKey(value);
	    public static QueryKey Query => "query";
	}
	[DebuggerDisplay("{Value}")]
	public readonly struct QueryValue : IEquatable<QueryValue>
	{
		public QueryValue(string value)
		{
			this.Value = value;
		}
		public string Value { get; }
		public ReadOnlySpan<byte> GetBytes(Encoding? encoding = null) => (encoding ?? Encoding.UTF8).GetBytes(Value);
	    public override string ToString()
	    {
			return Value;
	    }
	    public override bool Equals(object? instance)
	    {
	        if (instance is QueryValue queryValue)
			{
				return Equals(queryValue);
			}
			return false;
	    }
	    public override int GetHashCode()
	    {
			return HashCode.Combine(typeof(QueryValue), Value);
	    }
	    public bool Equals(QueryValue other)
	    {
			return Value.Equals(other.Value);
	    }
	    public static implicit operator QueryValue(string value) => new QueryValue(value);
		public static implicit operator string(QueryValue value) => value.Value;
	}
	[DebuggerDisplay("{Value}")]
	public readonly struct Route :
	    IEquatable<Route>,
	    IEqualityComparer<Route>,
	    IComparable<Route>
	{
	    private readonly RouteSegment[] segments;
	    private static string[] reserved => new string[]
	    {
	        "$query",
	        "$transactions",    // for sending multiple commands 
	        "$schema"           // GET /users/$schema?operation=CreateUser&format={json/xsd}
	    };
	    public Route(string route)
	    {
	        if (string.IsNullOrEmpty(route))
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(route));
	        }
	        this.segments = GetSegments(route);
	    }
	    private RouteSegment[] GetSegments(ReadOnlySpan<char> route)
	    {
	        var index = 0;
	        var segments = new RouteSegment[10];
	        var segment = string.Empty;
	        for (int i = 0; i < route.Length; i++)
	        {
	            var character = route[i];
	            // Check if we reached the end of the current segment
	            if (character == '/' || character == '\\')
	            {
	                // Let's skip leading slashes
	                if (i == 0) continue;
	                if (reserved.Contains(segment, StringComparer.OrdinalIgnoreCase))
	                {
	                    throw new Exception($"Invalid route. Using Reserved route: {segment} is not allowed.");
	                }
	                segments[index] = new RouteSegment(segment, index);
	                index++;
	                segment = string.Empty;
	                // Lets see if we reached the buffer in the array. Resize if reached.
	                if (index == segments.Length)
	                {
	                    Array.Resize(ref segments, 5);
	                }
	            }
	            else if ((i + 1) >= route.Length)
	            {
	                segment = segment + character;
	                segments[index] = new RouteSegment(segment, index);
	                index++;
	                segment = string.Empty;
	            }
	            else
	            {
	                segment = segment + character;
	            }
	        }
	        // Resizes segments to actual length
	        Array.Resize(ref segments, index);
	        return segments;
	    }
	    public string Value => ToString();
	    public RouteSegment[] Segments
	    {
	        get
	        {
	            // Let's only return copy
	            var copy = new RouteSegment[segments.Length];
	            segments.CopyTo(copy, 0);
	            return copy;
	        }
	    }
	    public override string ToString()
	    {
	        return string.Join('/', Segments.Select(x => x.Value));
	    }
	    public override int GetHashCode()
	    {
	        var hashCode = new HashCode();
	        foreach (var segment in Segments)
	        {
	            hashCode.Add(segment);
	        }
	        return hashCode.ToHashCode();
	    }
	    public override bool Equals(object? instance)
	    {
	        if (instance is Route route)
	        {
	            return Equals(route);
	        }
	        return false;
	    }
	    public bool IsMatch(Path path)
	    {
	        var pSegments = path.Segments;
	        var rSegments = Segments;
	        // Ensure same segment length
	        if (pSegments.Length != rSegments.Length)
	        {
	            return false;
	        }
	        for (int i = 0; i < pSegments.Length; i++)
	        {
	            var rseg = rSegments[i];
	            var pseg = pSegments[i];
	            if (!rseg.IsParameter() && rseg.Ordinal == pseg.Ordinal && !rseg.Equals(pseg.Value))
	            {
	                return false;
	            }
	        }
	        return true;
	    }
	    #region Implicit Interfaces
	    bool IEquatable<Route>.Equals(Route route)
	    {
	        var left = Segments;
	        var right = route.Segments;
	        if (left.Length != right.Length)
	        {
	            return false;
	        }
	        for (int i = 0; i < left.Length; i++)
	        {
	            if (!left[i].Equals(right[i]))
	            {
	                return false;
	            }
	        }
	        return true;
	    }
	    bool IEqualityComparer<Route>.Equals(Route left, Route right)
	    {
	        return left.Equals(right);
	    }
	    int IEqualityComparer<Route>.GetHashCode([DisallowNull] Route instance)
	    {
	        return instance.GetHashCode();
	    }
	    int IComparable<Route>.CompareTo(Route other)
	    {
	        return Value.ToLowerInvariant().CompareTo(other.Value.ToLowerInvariant());
	    }
	    #endregion
	    #region Operators
	    public static bool operator ==(Route left, Route right) => left.Equals(right);
	    public static bool operator !=(Route left, Route right) => !left.Equals(right);
	    public static implicit operator Route(string route) => new Route(route);
	    public static implicit operator string(Route route) => route.ToString();
	    #endregion
	    #region Helper Methods
	    public static Route Combine(Route left, Route right)
	    {
	        return Combine(new[] { left, right });
	    }
	    public static Route Combine(params Route[] routes)
	    {
	        return string.Join('/', routes.Select(p => p.ToString()));
	    }
	    #endregion
	}
	[DebuggerDisplay("{Value}")]
	public readonly partial struct RouteSegment :
	    IEquatable<RouteSegment>,
	    IEqualityComparer<RouteSegment>
	{
	    internal RouteSegment(string value, int ordinal)
	    {
	        Value = value;
	        Ordinal = ordinal;
	    }
	    public string Value { get; }
	    public int Ordinal { get; }
	    public bool Equals(RouteSegment other)
	    {
	        return Ordinal == other.Ordinal && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
	    }
	    public bool Equals(RouteSegment right, RouteSegment left)
	    {
	        return right.Equals(left);
	    }
	    public int GetHashCode(RouteSegment instance)
	    {
	        return instance.GetHashCode();
	    }
	    public override bool Equals(object? instance)
	    {
	        if (instance is RouteSegment segment)
	        {
	            return Equals(segment);
	        }
	        return false;
	    }
	    public override int GetHashCode()
	    {
	        return HashCode.Combine(typeof(RouteSegment), Ordinal, string.Create(Value.Length, Value, (chars, name) =>
	        {
	            // A string value's hash-code takes casing into account. Let's lowercase the characters since routes are insensitive
	            name.CopyTo(chars);
	            for (int i = 0; i < chars.Length;i++)
	            {
	                var c = chars[i];
	                if (char.IsUpper(c))
	                {
	                    chars[i] = char.ToLower(c);
	                }
	            }
	        }));
	    }
	    public override string ToString() => Value;
	    public bool IsParameter()
	    {
	        return Value[0] == '{' && Value[Value.Length - 1] == '}';
	    }
	    public string? GetParamName()
	    {
	        return IsParameter() ?
	            Value.Substring(1, Value.Length - 2).Split(':').First() : 
	            null;
	    }
	}
	public readonly partial struct RouteSegment
	{
	    //delegate bool Validator(object value, out string error);
	    //private readonly List<Validator> validators = new List<Validator>();
	    //public void Temp()
	    //{
	    //    var bytes = Encoding.UTF8.GetBytes(Value);
	    //    var sequence = new ReadOnlySequence<byte>(bytes);
	    //    var reader = new SequenceReader<byte>(sequence);
	    //    var delimiter = new byte[] { (byte)':' };
	    //    ReadOnlySequence<byte> current;
	    //    while (reader.TryRead(out var b))
	    //    {
	    //        if (b == (byte)'{') continue;   // Skip open bracket
	    //        if (b == (byte)'}') break;      // Stop parsing
	    //        if (reader.TryReadTo(out current, delimiter, false))
	    //        {
	    //            Parse(ref current);
	    //        }
	    //    }
	    //}
	    //public string? GetParamName()
	    //{
	    //}
	    //public bool IsValid(PathSegment segment, out string? error)
	    //{
	    //    error = null;
	    //    var value = segment.Value;
	    //    var bytes = Encoding.UTF8.GetBytes(Value);
	    //    var sequence = new ReadOnlySequence<byte>(bytes);
	    //    var reader = new SequenceReader<byte>(sequence);
	    //    var delimiter = new byte[] { (byte)':' };
	    //    ReadOnlySequence<byte> current;
	    //    while (reader.TryRead(out var b))
	    //    {
	    //        if (b == (byte)'{') continue;   // Skip open bracket
	    //        if (b == (byte)'}') break;      // Stop parsing
	    //        if (reader.TryReadTo(out current, delimiter, false))
	    //        {
	    //            Parse(ref current);
	    //        }
	    //    }
	    //    return false;
	    //}
	    //private static void Parse(ref ReadOnlySequence<byte> sequence)
	    //{
	    //}
	    //private static bool IsInt32(string value, out int number)
	    //{
	    //    return int.TryParse(value, out number);
	    //}
	}
	[DebuggerDisplay("{ToString()}")]
	public readonly struct StatusCode :
	    IEquatable<StatusCode>,
	    IEqualityComparer<StatusCode>,
	    IComparable<StatusCode>
	{
	    public static ReadOnlySpan<int> ValidStatusCodes => new int[]
	    {
	        200, // Ok
	        201, // Created
	        202, // Accepted
	        204, // NotContent
	        207, // MultiStatus
	        400, // BadRequest
	        401, // Unauthorized
	        403, // Forbidden
	        404, // NotFound
	        405, // MethodNotAllowed
	        406, // NotAcceptable
	        408, // RequestTimeout
	        409, // Conflict
	        412, // PreconditionFailed
	        414, // RequestUriTooLong
	        415, // UnsupportedMediaType
	        428, // PreconditionRequired
	        429, // TooManyRequests
	        500, // InternalServerError
	        501, // NotImplemented
	        502, // BadGateway
	        503, // ServiceUnavailable
	    };
	    public StatusCode(int code)
	    {
	        if (!ValidStatusCodes.Contains(code))
	        {
	            throw new ArgumentOutOfRangeException(nameof(code), "The status code is not valid.");
	        }
	        Code = code;
	    }
	    public int Code { get; }
	    bool IEquatable<StatusCode>.Equals(StatusCode statusCode)
	    {
	        return Code == statusCode.Code;
	    }
	    bool IEqualityComparer<StatusCode>.Equals(StatusCode left, StatusCode right)
	    {
	        return left.Equals(right);
	    }
	    int IEqualityComparer<StatusCode>.GetHashCode(StatusCode statusCode)
	    {
	        return statusCode.GetHashCode();
	    }
	    int IComparable<StatusCode>.CompareTo(StatusCode statusCode)
	    {
	        return statusCode.Code.CompareTo(this.Code);
	    }
	    #region Overloads
	    public override int GetHashCode()
	    {
	        return Code.GetHashCode();
	    }
	    public override string ToString()
	    {
	        return $"{Code} - {Enum.GetName(typeof(HttpStatusCode), (HttpStatusCode)Code)}";
	    }
	    public override bool Equals([NotNullWhen(true)] object? instance)
	    {
	        if (instance is StatusCode statusCode)
	        {
	            return Equals(statusCode);
	        }
	        return false;
	    }
	    #endregion
	    #region Operators
	    public static implicit operator StatusCode(int code) => new StatusCode(code);
	    public static implicit operator int(StatusCode status) => status.Code;
	    public static bool operator ==(StatusCode left, StatusCode right) => left.Equals(right);
	    public static bool operator !=(StatusCode left, StatusCode right) => !left.Equals(right);
	    public static bool operator >(StatusCode left, StatusCode right) => ((IComparable<StatusCode>)right).CompareTo(left) > 0;
	    public static bool operator <(StatusCode left, StatusCode right) => ((IComparable<StatusCode>)right).CompareTo(left) < 0;
	    public static bool operator >=(StatusCode left, StatusCode right) => ((IComparable<StatusCode>)right).CompareTo(left) >= 0;
	    public static bool operator <=(StatusCode left, StatusCode right) => ((IComparable<StatusCode>)right).CompareTo(left) <= 0;
	    #endregion
	    #region Success Status Codes
	    public static StatusCode Ok => new StatusCode(200);
	    public static StatusCode Created => new StatusCode(201);
	    public static StatusCode Accepted => new StatusCode(202);
	    public static StatusCode NotContent => new StatusCode(204);
	    public static StatusCode MultiStatus => new StatusCode(207);
	    #endregion
	    #region Bad Request Status Code
	    public static StatusCode BadRequest => new StatusCode(400);
	    public static StatusCode Unauthorized => new StatusCode(401);
	    public static StatusCode Forbidden => new StatusCode(403);
	    public static StatusCode NotFound => new StatusCode(404);
	    public static StatusCode MethodNotAllowed => new StatusCode(405);
	    public static StatusCode NotAcceptable => new StatusCode(406);
	    public static StatusCode RequestTimeout => new StatusCode(408);
	    public static StatusCode Conflict => new StatusCode(409);
	    public static StatusCode PreconditionFailed => new StatusCode(412);
	    public static StatusCode RequestUriTooLong => new StatusCode(414);
	    public static StatusCode UnsupportedMediaType => new StatusCode(415);
	    public static StatusCode PreconditionRequired => new StatusCode(428);
	    public static StatusCode TooManyRequests => new StatusCode(429);
	    #endregion
	    #region Server Error Status Codes
	    public static StatusCode InternalServerError => new StatusCode(500);
	    public static StatusCode NotImplemented => new StatusCode(501);
	    public static StatusCode BadGateway => new StatusCode(502);
	    public static StatusCode ServiceUnavailable => new StatusCode(503);
	    #endregion
	}
	#endregion
}
#endregion
