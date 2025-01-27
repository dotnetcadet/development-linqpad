<Query Kind="Program">
<NuGetReference Version="8.0.0">System.IO.Pipelines</NuGetReference>
<Namespace>System</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>System.Threading.Tasks</Namespace>
<Namespace>System.Collections</Namespace>
<Namespace>System.Diagnostics.CodeAnalysis</Namespace>
<Namespace>System.Diagnostics</Namespace>
<Namespace>Assimalign.Cohesion.Net.Http.Internal</Namespace>
<Namespace>System.Runtime.CompilerServices</Namespace>
<Namespace>System.Net</Namespace>
<Namespace>System.Globalization</Namespace>
<Namespace>System.Buffers</Namespace>
<Namespace>System.Web</Namespace>
<Namespace>System.Text.Encodings.Web</Namespace>
<Namespace>System.Threading</Namespace>
<Namespace>System.IO</Namespace>
<Namespace>System.Security.Claims</Namespace>
<Namespace>System.Reflection</Namespace>
<Namespace>static System.Text.Encoding</Namespace>
<Namespace>Internal</Namespace>
<Namespace>Transports</Namespace>
<Namespace>Assimalign.Cohesion.Net.Transports</Namespace>
<Namespace>System.IO.Pipelines</Namespace>
<Namespace>System.Runtime.InteropServices</Namespace>
<Namespace>System.Collections.Concurrent</Namespace>
<Namespace>static Assimalign.Cohesion.Net.Http.Internal.HttpValues.Separators</Namespace>
<Namespace>static Assimalign.Cohesion.Net.Http.Internal.HttpValues</Namespace>
<Namespace>System.Buffers.Binary</Namespace>
<Namespace>static HttpValues</Namespace>
<Namespace>static HttpValues.Separators</Namespace>
<Namespace>static HttpValues.StatusCodes</Namespace>
<Namespace>System.Numerics</Namespace>
<Namespace>System.Runtime.Serialization</Namespace>
<Namespace>System.Runtime.Intrinsics</Namespace>
</Query>
#load ".\assimalign.cohesion.net.transports"
#load ".\assimalign.cohesion.core"

void Main()
{

}

#region Assimalign.Cohesion.Net.Http(net8.0)
namespace Assimalign.Cohesion.Net.Http
{
	#region \
	public class HttpCookie
	{
	}
	internal class HttpHeader
	{
	    // <summary>Gets the <c>Accept</c> HTTP header name.</summary>
	    public const string Accept = "Accept";
	    public const string AcceptCharset = "Accept-Charset";
	    public const string AcceptEncoding = "Accept-Encoding";
	    public const string AcceptLanguage = "Accept-Language";
	    public const string AcceptRanges = "Accept-Ranges";
	    public const string AccessControlAllowCredentials = "Access-Control-Allow-Credentials";
	    public const string AccessControlAllowHeaders = "Access-Control-Allow-Headers";
	    public const string AccessControlAllowMethods = "Access-Control-Allow-Methods";
	    public const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
	    public const string AccessControlExposeHeaders = "Access-Control-Expose-Headers";
	    public const string AccessControlMaxAge = "Access-Control-Max-Age";
	    public const string AccessControlRequestHeaders = "Access-Control-Request-Headers";
	    public const string AccessControlRequestMethod = "Access-Control-Request-Method";
	    public const string Age = "Age";
	    public const string Allow = "Allow";
	    public const string AltSvc = "Alt-Svc";
	    public const string Authority = ":authority";
	    public const string Authorization = "Authorization";
	    public const string Baggage = "baggage";
	    public const string CacheControl = "Cache-Control";
	    public const string Connection = "Connection";
	    public const string ContentDisposition = "Content-Disposition";
	    public const string ContentEncoding = "Content-Encoding";
	    public const string ContentLanguage = "Content-Language";
	    public const string ContentLength = "Content-Length";
	    public const string ContentLocation = "Content-Location";
	    public const string ContentMD5 = "Content-MD5";
	    public const string ContentRange = "Content-Range";
	    public const string ContentSecurityPolicy = "Content-Security-Policy";
	    public const string ContentSecurityPolicyReportOnly = "Content-Security-Policy-Report-Only";
	    public const string ContentType = "Content-Type";
	    public const string CorrelationContext = "Correlation-Context";
	    public const string Cookie = "Cookie";
	    public const string Date = "Date";
	    public const string DNT = "DNT";
	    public const string ETag = "ETag";
	    public const string Expires = "Expires";
	    public const string Expect = "Expect";
	    public const string From = "From";
	    public const string GrpcAcceptEncoding = "Grpc-Accept-Encoding";
	    public const string GrpcEncoding = "Grpc-Encoding";
	    public const string GrpcMessage = "Grpc-Message";
	    public const string GrpcStatus = "Grpc-Status";
	    public const string GrpcTimeout = "Grpc-Timeout";
	    public const string Host = "Host";
	    public const string KeepAlive = "Keep-Alive";
	    public const string IfMatch = "If-Match";
	    public const string IfModifiedSince = "If-Modified-Since";
	    public const string IfNoneMatch = "If-None-Match";
	    public const string IfRange = "If-Range";
	    public const string IfUnmodifiedSince = "If-Unmodified-Since";
	    public const string LastModified = "Last-Modified";
	    public const string Link = "Link";
	    public const string Location = "Location";
	    public const string MaxForwards = "Max-Forwards";
	    public const string Method = ":method";
	    public const string Origin = "Origin";
	    public const string Path = ":path";
	    public const string Pragma = "Pragma";
	    public const string Protocol = ":protocol";
	    public const string ProxyAuthenticate = "Proxy-Authenticate";
	    public const string ProxyAuthorization = "Proxy-Authorization";
	    public const string ProxyConnection = "Proxy-Connection";
	    public const string Range = "Range";
	    public const string Referer = "Referer";
	    public const string RetryAfter = "Retry-After";
	    public const string RequestId = "Request-Id";
	    public const string Scheme = ":scheme";
	    public const string SecWebSocketAccept = "Sec-WebSocket-Accept";
	    public const string SecWebSocketKey = "Sec-WebSocket-Key";
	    public const string SecWebSocketProtocol = "Sec-WebSocket-ProtocolType";
	    public const string SecWebSocketVersion = "Sec-WebSocket-Version";
	    public const string SecWebSocketExtensions = "Sec-WebSocket-Extensions";
	    public const string Server = "Server";
	    public const string SetCookie = "Set-Cookie";
	    public const string Status = ":status";
	    public const string StrictTransportSecurity = "Strict-Transports-Security";
	    public const string TE = "TE";
	    public const string Trailer = "Trailer";
	    public const string TransferEncoding = "Transfer-Encoding";
	    public const string Translate = "Translate";
	    public const string TraceParent = "traceparent";
	    public const string TraceState = "tracestate";
	    public const string Upgrade = "Upgrade";
	    public const string UpgradeInsecureRequests = "Upgrade-Insecure-Requests";
	    public const string UserAgent = "User-Agent";
	    public const string Vary = "Vary";
	    public const string Via = "Via";
	    public const string Warning = "Warning";
	    public const string WebSocketSubProtocols = "Sec-WebSocket-ProtocolType";
	    public const string WWWAuthenticate = "WWW-Authenticate";
	    public const string XContentTypeOptions = "X-Content-Type-Options";
	    public const string XFrameOptions = "X-Frame-Options";
	    public const string XPoweredBy = "X-Powered-By";
	    public const string XRequestedWith = "X-Requested-With";
	    public const string XUACompatible = "X-UA-Compatible";
	    public const string XXSSProtection = "X-XSS-Protection";
	}
	public sealed partial class HttpHeaderCollection : IHttpHeaderCollection
	{
	    private static readonly HttpHeaderKey[] EmptyKeys = Array.Empty<HttpHeaderKey>();
	    private static readonly HttpHeaderValue[] EmptyValues = Array.Empty<HttpHeaderValue>();
	    private static readonly IEnumerator<KeyValuePair<HttpHeaderKey, HttpHeaderValue>> EmptyIEnumeratorType = default(Enumerator);
	    private static readonly IEnumerator EmptyIEnumerator = default(Enumerator);
	    private Dictionary<HttpHeaderKey, HttpHeaderValue>? store;
	    public HttpHeaderCollection() { }
	    public HttpHeaderCollection(int capacity)
	    {
	        EnsureStore(capacity);
	    }
	    public HttpHeaderCollection(Dictionary<HttpHeaderKey, HttpHeaderValue>? store)
	    {
	        this.store = store;
	    }
	    public HttpHeaderValue this[HttpHeaderKey key]
	    {
	        get
	        {
	            if (store == null)
	            {
	                return HttpHeaderValue.Empty;
	            }
	            if (TryGetValue(key, out var value))
	            {
	                return value;
	            }
	            return HttpHeaderValue.Empty;
	        }
	        set
	        {
	            ThrowIfReadOnly();
	            if (value.Count == 0)
	            {
	                store?.Remove(key);
	                return;
	            }
	            EnsureStore(1);
	            store![key] = value;
	        }
	    }
	    public ICollection<HttpHeaderKey> Keys => store == null ? EmptyKeys : store!.Keys;
	    public ICollection<HttpHeaderValue> Values => store == null ? EmptyValues : store!.Values;
	    public int Count => store?.Count ?? 0;
	    public bool IsReadOnly { get; set; }
	    public void Add(HttpHeaderKey key, HttpHeaderValue value)
	    {
	        ThrowIfReadOnly();
	        EnsureStore(1);
	        store!.Add(key, value);
	    }
	    public void Add(KeyValuePair<HttpHeaderKey, HttpHeaderValue> item)
	    {
	        ThrowIfReadOnly();
	        EnsureStore(1);
	        store!.Add(item.Key, item.Value);
	    }
	    public void Clear()
	    {
	        ThrowIfReadOnly();
	        store?.Clear();
	    }
	    public bool Contains(KeyValuePair<HttpHeaderKey, HttpHeaderValue> item)
	    {
	        if (store == null || !store!.TryGetValue(item.Key, out var value) || !HttpHeaderValue.Equals(value, item.Value))
	        {
	            return false;
	        }
	        return true;
	    }
	    public bool ContainsKey(HttpHeaderKey key)
	    {
	        if (store == null)
	        {
	            return false;
	        }
	        return store!.ContainsKey(key);
	    }
	    public void CopyTo(KeyValuePair<HttpHeaderKey, HttpHeaderValue>[] array, int arrayIndex)
	    {
	        if (store == null)
	        {
	            return;
	        }
	        foreach (KeyValuePair<HttpHeaderKey, HttpHeaderValue> item in store!)
	        {
	            var keyValuePair = (array[arrayIndex] = item);
	            arrayIndex++;
	        }
	    }  
	    public bool Remove(HttpHeaderKey key)
	    {
	        ThrowIfReadOnly();
	        if (store == null)
	        {
	            return false;
	        }
	        return store!.Remove(key);
	    }
	    public bool Remove(KeyValuePair<HttpHeaderKey, HttpHeaderValue> item)
	    {
	        ThrowIfReadOnly();
	        if (store == null)
	        {
	            return false;
	        }
	        if (store!.TryGetValue(item.Key, out var value) && HttpHeaderValue.Equals(item.Value, value))
	        {
	            return store!.Remove(item.Key);
	        }
	        return false;
	    }
	    public bool TryGetValue(HttpHeaderKey key, [MaybeNullWhen(false)] out HttpHeaderValue value)
	    {
	        if (store == null)
	        {
	            value = default(HttpHeaderValue);
	            return false;
	        }
	        return store!.TryGetValue(key, out value);
	    }
	    public IEnumerator<KeyValuePair<HttpHeaderKey, HttpHeaderValue>> GetEnumerator()
	    {
	        if (store == null || store!.Count == 0)
	        {
	            return default(Enumerator);
	        }
	        return new Enumerator(store!.GetEnumerator());
	    }
	    IEnumerator IEnumerable.GetEnumerator()
	    {
	        if (store == null || store!.Count == 0)
	        {
	            return EmptyIEnumerator;
	        }
	        return store!.GetEnumerator();
	    }
	    [MemberNotNull("store")]
	    private void EnsureStore(int capacity)
	    {
	        if (store == null)
	        {
	            store = new Dictionary<HttpHeaderKey, HttpHeaderValue>(capacity);
	        }
	    }
	    private void ThrowIfReadOnly()
	    {
	        if (IsReadOnly)
	        {
	            throw new InvalidOperationException("The response headers cannot be modified because the response has already started.");
	        }
	    }
	    private struct Enumerator : IEnumerator<KeyValuePair<HttpHeaderKey, HttpHeaderValue>>, IEnumerator, IDisposable
	    {
	        private Dictionary<HttpHeaderKey, HttpHeaderValue>.Enumerator enumerator;
	        private readonly bool isNotEmpty;
	        public KeyValuePair<HttpHeaderKey, HttpHeaderValue> Current
	        {
	            get
	            {
	                if (isNotEmpty)
	                {
	                    return enumerator.Current;
	                }
	                return default(KeyValuePair<HttpHeaderKey, HttpHeaderValue>);
	            }
	        }
	        object IEnumerator.Current => Current;
	        internal Enumerator(Dictionary<HttpHeaderKey, HttpHeaderValue>.Enumerator dictionaryEnumerator)
	        {
	            enumerator = dictionaryEnumerator;
	            isNotEmpty = true;
	        }
	        public bool MoveNext() => isNotEmpty ? enumerator.MoveNext() : false;
	        public void Dispose() { }
	        void IEnumerator.Reset()
	        {
	            if (isNotEmpty)
	            {
	                ((IEnumerator)enumerator).Reset();
	            }
	        }
	    }
	    public HttpHeaderValue? Accepts => GetHeaderValue(HttpHeader.Accept);
	    public HttpHeaderValue? ContentType => GetHeaderValue(HttpHeader.ContentType);
	    public HttpHeaderValue? ContentLength => GetHeaderValue(HttpHeader.ContentLength);
	    public HttpHeaderValue? TransferEncoding => GetHeaderValue(HttpHeader.TransferEncoding);
	    public HttpHeaderValue? Connection => GetHeaderValue(HttpHeader.Connection);
	    public HttpHeaderValue? AcceptCharset => GetHeaderValue(HttpHeader.AcceptCharset);
	    public HttpHeaderValue? AcceptEncoding => GetHeaderValue(HttpHeader.AcceptEncoding);
	    public HttpHeaderValue? AcceptLanguage => GetHeaderValue(HttpHeader.AcceptLanguage);
	    public HttpHeaderValue? AcceptRanges => GetHeaderValue(HttpHeader.AcceptRanges);
	    public HttpHeaderValue? AccessControlAllowCredentials => GetHeaderValue(HttpHeader.AccessControlAllowCredentials);
	    public HttpHeaderValue? AccessControlAllowHeaders => GetHeaderValue(HttpHeader.AccessControlAllowHeaders);
	    public HttpHeaderValue? AccessControlAllowMethods => GetHeaderValue(HttpHeader.AccessControlAllowMethods);
	    public HttpHeaderValue? AccessControlAllowOrigin => GetHeaderValue(HttpHeader.AccessControlAllowOrigin);
	    public HttpHeaderValue? AccessControlExposeHeaders => GetHeaderValue(HttpHeader.AccessControlExposeHeaders);
	    public HttpHeaderValue? AccessControlMaxAge => GetHeaderValue(HttpHeader.AccessControlMaxAge);
	    public HttpHeaderValue? AccessControlRequestHeaders => GetHeaderValue(HttpHeader.AccessControlRequestHeaders);
	    public HttpHeaderValue? AccessControlRequestMethod => throw new NotImplementedException();
	    public HttpHeaderValue? Age => throw new NotImplementedException();
	    public HttpHeaderValue? Allow => throw new NotImplementedException();
	    public HttpHeaderValue? AltSvc => throw new NotImplementedException();
	    public HttpHeaderValue? Authorization => throw new NotImplementedException();
	    public HttpHeaderValue? Baggage => throw new NotImplementedException();
	    public HttpHeaderValue? CacheControl => throw new NotImplementedException();
	    public HttpHeaderValue? ContentDisposition => throw new NotImplementedException();
	    public HttpHeaderValue? ContentEncoding => throw new NotImplementedException();
	    public HttpHeaderValue? ContentLanguage => throw new NotImplementedException();
	    public HttpHeaderValue? ContentLocation => throw new NotImplementedException();
	    public HttpHeaderValue? ContentMD5 => throw new NotImplementedException();
	    public HttpHeaderValue? ContentRange => throw new NotImplementedException();
	    public HttpHeaderValue? ContentSecurityPolicy => throw new NotImplementedException();
	    public HttpHeaderValue? ContentSecurityPolicyReportOnly => throw new NotImplementedException();
	    public HttpHeaderValue? CorrelationContext => throw new NotImplementedException();
	    public HttpHeaderValue? Cookie => throw new NotImplementedException();
	    public HttpHeaderValue? Date => throw new NotImplementedException();
	    public HttpHeaderValue? ETag => throw new NotImplementedException();
	    public HttpHeaderValue? Expires => throw new NotImplementedException();
	    public HttpHeaderValue? Expect => throw new NotImplementedException();
	    public HttpHeaderValue? From => throw new NotImplementedException();
	    public HttpHeaderValue? GrpcAcceptEncoding => throw new NotImplementedException();
	    public HttpHeaderValue? GrpcEncoding => throw new NotImplementedException();
	    public HttpHeaderValue? GrpcMessage => throw new NotImplementedException();
	    public HttpHeaderValue? GrpcStatus => throw new NotImplementedException();
	    public HttpHeaderValue? GrpcTimeout => throw new NotImplementedException();
	    public HttpHeaderValue? Host => throw new NotImplementedException();
	    public HttpHeaderValue? KeepAlive => throw new NotImplementedException();
	    public HttpHeaderValue? IfMatch => throw new NotImplementedException();
	    public HttpHeaderValue? IfModifiedSince => throw new NotImplementedException();
	    public HttpHeaderValue? IfNoneMatch => throw new NotImplementedException();
	    public HttpHeaderValue? IfRange => throw new NotImplementedException();
	    public HttpHeaderValue? IfUnmodifiedSince => throw new NotImplementedException();
	    public HttpHeaderValue? LastModified => throw new NotImplementedException();
	    public HttpHeaderValue? Link => throw new NotImplementedException();
	    public HttpHeaderValue? Location => throw new NotImplementedException();
	    public HttpHeaderValue? MaxForwards => throw new NotImplementedException();
	    public HttpHeaderValue? Origin => throw new NotImplementedException();
	    public HttpHeaderValue? Pragma => throw new NotImplementedException();
	    public HttpHeaderValue? ProxyAuthenticate => throw new NotImplementedException();
	    public HttpHeaderValue? ProxyAuthorization => throw new NotImplementedException();
	    public HttpHeaderValue? ProxyConnection => throw new NotImplementedException();
	    public HttpHeaderValue? Range => throw new NotImplementedException();
	    public HttpHeaderValue? Referer => throw new NotImplementedException();
	    public HttpHeaderValue? RetryAfter => throw new NotImplementedException();
	    public HttpHeaderValue? RequestId => throw new NotImplementedException();
	    public HttpHeaderValue? SecWebSocketAccept => throw new NotImplementedException();
	    public HttpHeaderValue? SecWebSocketKey => throw new NotImplementedException();
	    public HttpHeaderValue? SecWebSocketProtocol => throw new NotImplementedException();
	    public HttpHeaderValue? SecWebSocketVersion => throw new NotImplementedException();
	    public HttpHeaderValue? SecWebSocketExtensions => throw new NotImplementedException();
	    public HttpHeaderValue? Server => throw new NotImplementedException();
	    public HttpHeaderValue? SetCookie => throw new NotImplementedException();
	    public HttpHeaderValue? StrictTransportSecurity => throw new NotImplementedException();
	    public HttpHeaderValue? TE => throw new NotImplementedException();
	    public HttpHeaderValue? Trailer => throw new NotImplementedException();
	    public HttpHeaderValue? Translate => throw new NotImplementedException();
	    public HttpHeaderValue? TraceParent => throw new NotImplementedException();
	    public HttpHeaderValue? TraceState => throw new NotImplementedException();
	    public HttpHeaderValue? Upgrade => throw new NotImplementedException();
	    public HttpHeaderValue? UpgradeInsecureRequests => throw new NotImplementedException();
	    public HttpHeaderValue? UserAgent => throw new NotImplementedException();
	    public HttpHeaderValue? Vary => throw new NotImplementedException();
	    public HttpHeaderValue? Via => throw new NotImplementedException();
	    public HttpHeaderValue? Warning => throw new NotImplementedException();
	    public HttpHeaderValue? WebSocketSubProtocols => throw new NotImplementedException();
	    public HttpHeaderValue? WWWAuthenticate => throw new NotImplementedException();
	    public HttpHeaderValue? XContentTypeOptions => throw new NotImplementedException();
	    public HttpHeaderValue? XFrameOptions => throw new NotImplementedException();
	    public HttpHeaderValue? XPoweredBy => throw new NotImplementedException();
	    public HttpHeaderValue? XRequestedWith => throw new NotImplementedException();
	    public HttpHeaderValue? XUACompatible => throw new NotImplementedException();
	    public HttpHeaderValue? XXSSProtection => throw new NotImplementedException();
	    private HttpHeaderValue? GetHeaderValue(string key)
	    {
	        var value = this[key];
	        if (value.IsEmpty)
	        {
	            return null;
	        }
	        return value;
	    }
	}
	[DebuggerDisplay("{Value}")]
	public readonly struct HttpHeaderKey :
	    IEquatable<HttpHeaderKey>,
	    IEqualityComparer<HttpHeaderKey>,
	    IComparable<HttpHeaderKey>
	{
	    private const StringComparison comparison = StringComparison.OrdinalIgnoreCase;
	    public HttpHeaderKey(string value)
	    {
	        if (string.IsNullOrEmpty(value))
	        {
	            ThrowUtility.ThrowArgumentNullException(nameof(value));
	        }
	        this.Value = value;
	    }
	    public string Value { get; }
	    #region Overloads
	    public override string ToString()
	    {
	        return Value;
	    }
	    public override int GetHashCode()
	    {
	        return string.GetHashCode(Value, comparison);
	    }
	    public override bool Equals(object? instance)
	    {
	        if (instance is HttpHeaderKey key)
	        {
	            return Equals(key);
	        }
	        return false;
	    }
	    #endregion
	    #region Explicit Implementations
	    bool IEquatable<HttpHeaderKey>.Equals(HttpHeaderKey other)
	    {
	        return Value.Equals(other.Value, comparison);
	    }
	    int IComparable<HttpHeaderKey>.CompareTo(HttpHeaderKey other)
	    {
	        return string.Compare(Value, other.Value, comparison);
	    }
	    bool IEqualityComparer<HttpHeaderKey>.Equals(HttpHeaderKey left, HttpHeaderKey right)
	    {
	        return left.Equals(right);
	    }
	    int IEqualityComparer<HttpHeaderKey>.GetHashCode(HttpHeaderKey obj)
	    {
	        return obj.GetHashCode();
	    }
	    #endregion
	    #region Operators
	    public static implicit operator HttpHeaderKey(string key) => new HttpHeaderKey(key);
	    public static implicit operator string(HttpHeaderKey key) => key.Value;
	    public static bool operator ==(HttpHeaderKey left, HttpHeaderKey right) => left.Equals(right);
	    public static bool operator !=(HttpHeaderKey left, HttpHeaderKey right) => !left.Equals(right);
	    #endregion
	}
	public readonly partial struct HttpHeaderValue :
	    IList<string?>,
	    IReadOnlyList<string?>,
	    IEquatable<HttpHeaderValue>,
	    IEquatable<string?>,
	    IEquatable<string?[]?>
	{
	    public static readonly HttpHeaderValue Empty = new HttpHeaderValue(Array.Empty<string>());
	    private readonly object? _values;
	    public HttpHeaderValue(string? value)
	    {
	        _values = value;
	    }
	    public HttpHeaderValue(string?[]? values)
	    {
	        _values = values;
	    }
	    public string Value
	    {
	        get
	        {
	            if (_values is string str)
	            {
	                return str;
	            }
	            if (_values is string[] strArr)
	            {
	                return string.Join(';', strArr);
	            }
	            return null;
	        }
	    }
	    public bool IsEmpty
	    {
	        get
	        {
	            if (_values is null)
	            {
	                return true;
	            }
	            if (_values is string str && string.IsNullOrEmpty(str))
	            {
	                return true;
	            }
	            if (_values is string[] strArr && strArr.Length == 0)
	            {
	                return true;
	            }
	            return false;
	        }
	    }
	    public static implicit operator HttpHeaderValue(string? value)
	    {
	        return new HttpHeaderValue(value);
	    }
	    public static implicit operator HttpHeaderValue(string?[]? values)
	    {
	        return new HttpHeaderValue(values);
	    }
	    public static implicit operator string?(HttpHeaderValue values)
	    {
	        return values.GetStringValue();
	    }
	    public static implicit operator string?[]?(HttpHeaderValue value)
	    {
	        return value.GetArrayValue();
	    }
	    public int Count
	    {
	        [MethodImpl(MethodImplOptions.AggressiveInlining)]
	        get
	        {
	            // Take local copy of _values so type checks remain valid even if the StringValues is overwritten in memory
	            object? value = _values;
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
	    bool ICollection<string?>.IsReadOnly => true;
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
	            // Take local copy of _values so type checks remain valid even if the StringValues is overwritten in memory
	            object? value = _values;
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
	        // Take local copy of _values so type checks remain valid even if the StringValues is overwritten in memory
	        object? value = _values;
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
	        // Take local copy of _values so type checks remain valid even if the StringValues is overwritten in memory
	        object? value = _values;
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
	        // Take local copy of _values so type checks remain valid even if the StringValues is overwritten in memory
	        object? value = _values;
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
	        // Take local copy of _values so type checks remain valid even if the StringValues is overwritten in memory
	        object? value = _values;
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
	        return new Enumerator(_values);
	    }
	    IEnumerator<string?> IEnumerable<string?>.GetEnumerator()
	    {
	        return GetEnumerator();
	    }
	    IEnumerator IEnumerable.GetEnumerator()
	    {
	        return GetEnumerator();
	    }
	    public static bool IsNullOrEmpty(HttpHeaderValue value)
	    {
	        object? data = value._values;
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
	    public static HttpHeaderValue Concat(HttpHeaderValue values1, HttpHeaderValue values2)
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
	        return new HttpHeaderValue(combined);
	    }
	    public static HttpHeaderValue Concat(in HttpHeaderValue values, string? value)
	    {
	        if (value == null)
	        {
	            return values;
	        }
	        int count = values.Count;
	        if (count == 0)
	        {
	            return new HttpHeaderValue(value);
	        }
	        var combined = new string[count + 1];
	        values.CopyTo(combined, 0);
	        combined[count] = value;
	        return new HttpHeaderValue(combined);
	    }
	    public static HttpHeaderValue Concat(string? value, in HttpHeaderValue values)
	    {
	        if (value == null)
	        {
	            return values;
	        }
	        int count = values.Count;
	        if (count == 0)
	        {
	            return new HttpHeaderValue(value);
	        }
	        var combined = new string[count + 1];
	        combined[0] = value;
	        values.CopyTo(combined, 1);
	        return new HttpHeaderValue(combined);
	    }
	    public static bool Equals(HttpHeaderValue left, HttpHeaderValue right)
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
	    public static bool operator ==(HttpHeaderValue left, HttpHeaderValue right)
	    {
	        return Equals(left, right);
	    }
	    public static bool operator !=(HttpHeaderValue left, HttpHeaderValue right)
	    {
	        return !Equals(left, right);
	    }
	    public bool Equals(HttpHeaderValue other) => Equals(this, other);
	    public static bool Equals(string? left, HttpHeaderValue right) => Equals(new HttpHeaderValue(left), right);
	    public static bool Equals(HttpHeaderValue left, string? right) => Equals(left, new HttpHeaderValue(right));
	    public bool Equals(string? other) => Equals(this, new HttpHeaderValue(other));
	    public static bool Equals(string?[]? left, HttpHeaderValue right) => Equals(new HttpHeaderValue(left), right);
	    public static bool Equals(HttpHeaderValue left, string?[]? right) => Equals(left, new HttpHeaderValue(right));
	    public bool Equals(string?[]? other) => Equals(this, new HttpHeaderValue(other));
	    public static bool operator ==(HttpHeaderValue left, string? right) => Equals(left, new HttpHeaderValue(right));
	    public static bool operator !=(HttpHeaderValue left, string? right) => !Equals(left, new HttpHeaderValue(right));
	    public static bool operator ==(string? left, HttpHeaderValue right) => Equals(new HttpHeaderValue(left), right);
	    public static bool operator !=(string left, HttpHeaderValue right) => !Equals(new HttpHeaderValue(left), right);
	    public static bool operator ==(HttpHeaderValue left, string?[]? right) => Equals(left, new HttpHeaderValue(right));
	    public static bool operator !=(HttpHeaderValue left, string?[]? right) => !Equals(left, new HttpHeaderValue(right));
	    public static bool operator ==(string?[]? left, HttpHeaderValue right) => Equals(new HttpHeaderValue(left), right);
	    public static bool operator !=(string?[]? left, HttpHeaderValue right) => !Equals(new HttpHeaderValue(left), right);
	    public static bool operator ==(HttpHeaderValue left, object? right) => left.Equals(right);
	    public static bool operator !=(HttpHeaderValue left, object? right) => !left.Equals(right);
	    public static bool operator ==(object? left, HttpHeaderValue right) => right.Equals(left);
	    public static bool operator !=(object? left, HttpHeaderValue right) => !right.Equals(left);
	    public override bool Equals(object? obj)
	    {
	        if (obj == null)
	        {
	            return Equals(this, HttpHeaderValue.Empty);
	        }
	        if (obj is string str)
	        {
	            return Equals(this, str);
	        }
	        if (obj is string[] array)
	        {
	            return Equals(this, array);
	        }
	        if (obj is HttpHeaderValue stringValues)
	        {
	            return Equals(this, stringValues);
	        }
	        return false;
	    }
	    public override int GetHashCode()
	    {
	        object? value = _values;
	        if (value is string[] values)
	        {
	            if (Count == 1)
	            {
	                return Unsafe.As<string>(this[0])?.GetHashCode() ?? Count.GetHashCode();
	            }
	            int hashCode = 0;
	            for (int i = 0; i < values.Length; i++)
	            {
	                // RyuJIT optimizes this to use the ROL instruction
	                // Related GitHub pull request: https://github.com/dotnet/coreclr/pull/1830
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
	        public Enumerator(ref HttpHeaderValue values) : this(values._values)
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
	public readonly partial struct HttpHeaderValue
	{
	}
	[DebuggerDisplay("{Value}")]
	public readonly struct HttpHost : IEquatable<HttpHost>
	{
	    public HttpHost(string value)
    {
	        if (string.IsNullOrEmpty(value))
	        {
	            throw new ArgumentNullException(nameof(value));
	        }
        Value = value;
    }
	    public string Value { get; }
	    //public int? Port { get; }
	    public bool Equals(HttpHost other)
	    {
	        return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
	    }
	    public override bool Equals(object? obj)
	    {
	        if (obj is  HttpHost other)
	        {
	            return Equals(other);
	        }
	        return false;
	    }
	    public override string ToString()
	    {
	        return Value;
	    }
	    public override int GetHashCode()
	    {
	        return string.GetHashCode(Value, StringComparison.OrdinalIgnoreCase);
	    }
	    public static implicit operator HttpHost(string value) => new HttpHost(value);
	    public static implicit operator string(HttpHost host) => host.Value;
	}
	public readonly struct HttpMethod
	{
	    public HttpMethod(string value)
	    {
	        if (string.IsNullOrWhiteSpace(value))
	        {
	            throw new ArgumentNullException(nameof(value));
	        }
	        if (value.Any(x=> !char.IsLetterOrDigit(x)))
	        {
	            ThrowUtility.InvalidHttpMethod(value);
	        }
	        this.Value = value.ToUpper();
	    }
	    public string Value { get; }
	    public override string ToString()
	    {
	        return $"Method: {Value}";
	    }
	    public static readonly string Connect = "CONNECT";
	    public static readonly string Delete = "DELETE";
	    public static readonly string Get = "GET";
	    public static readonly string Head = "HEAD";
	    public static readonly string Options = "OPTIONS";
	    public static readonly string Patch = "PATCH";
	    public static readonly string Post = "POST";
	    public static readonly string Put = "PUT";
	    public static readonly string Trace = "TRACE";
	    public static bool IsConnect(string method)
	    {
	        return Equals(Connect, method);
	    }
	    public static bool IsDelete(string method)
	    {
	        return Equals(Delete, method);
	    }
	    public static bool IsGet(string method)
	    {
	        return Equals(Get, method);
	    }
	    public static bool IsHead(string method)
	    {
	        return Equals(Head, method);
	    }
	    public static bool IsOptions(string method)
	    {
	        return Equals(Options, method);
	    }
	    public static bool IsPatch(string method)
	    {
	        return Equals(Patch, method);
	    }
	    public static bool IsPost(string method)
	    {
	        return Equals(Post, method);
	    }
	    public static bool IsPut(string method)
	    {
	        return Equals(Put, method);
	    }
	    public static bool IsTrace(string method)
	    {
	        return Equals(Trace, method);
	    }
	    public static string GetCanonicalizedValue(string method) => method switch
	    {
	        string _ when IsGet(method) => Get,
	        string _ when IsPost(method) => Post,
	        string _ when IsPut(method) => Put,
	        string _ when IsDelete(method) => Delete,
	        string _ when IsOptions(method) => Options,
	        string _ when IsHead(method) => Head,
	        string _ when IsPatch(method) => Patch,
	        string _ when IsTrace(method) => Trace,
	        string _ when IsConnect(method) => Connect,
	        string _ => method
	    };
	    private static bool Equals(string methodA, string methodB)
	    {
	        return object.ReferenceEquals(methodA, methodB) || StringComparer.OrdinalIgnoreCase.Equals(methodA, methodB);
	    }
	    public static implicit operator HttpMethod(string method) => new HttpMethod(method);
	    public static bool operator ==(HttpMethod left, HttpMethod right)
	    {
	        return Equals(left, right);
	    }
	    public static bool operator !=(HttpMethod left, HttpMethod right)
	    {
	        return !Equals(left, right);
	    }
	}
	[DebuggerDisplay("{Value}")]
	public readonly struct HttpPath : IEquatable<HttpPath>
	{
	#if NET8_0_OR_GREATER
		// The allowed characters in an HTTP Path.
		private static readonly SearchValues<char> characters = SearchValues.Create("!$&'()*+,-./0123456789:;=@ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz~");
	#else
	#endif
	    // HttpPath is only set internally on requestion creation.
	    public HttpPath(string value)
		{
			//if (value.Any(c => !characters.Contains(c)))
			//{
			//	ThrowUtility.InvalidHttpPath($"The following path contains an in invalid character: '{value}'.");
			//}
			this.Value = value;
			this.Segments = new string[0];
		}
		public string Value { get; }
		public string[] Segments { get; }
		public HttpPath Concat(HttpPath path)
		{
			throw new NotImplementedException();
		}
	    public bool Equals(HttpPath other)
	    {
			return Equals(other, StringComparison.OrdinalIgnoreCase);
	    }
		public bool Equals(HttpPath other, StringComparison comparison)
		{
			return string.Equals(Value, other.Value, comparison);
		}
	    #region Overloads
	    public override string ToString()
	    {
	        return base.ToString();
	    }
	    public override int GetHashCode()
	    {
	        return base.GetHashCode();
	    }
	    public override bool Equals(object? obj)
	    {
	        if (obj is HttpPath path)
	        {
	            return Equals(path);
	        }
	        return false;
	    }
	    #endregion
	    #region Operatos
	    public static implicit operator HttpPath(string value) => new HttpPath(value);
	    public static implicit operator string(HttpPath route) => route.Value;
		#endregion
	}
	public sealed partial class HttpQueryCollection : IHttpQueryCollection
	{
	    private static readonly HttpQueryKey[] EmptyKeys = Array.Empty<HttpQueryKey>();
	    private static readonly HttpQueryValue[] EmptyValues = Array.Empty<HttpQueryValue>();
	    private static readonly IEnumerator<KeyValuePair<HttpQueryKey, HttpQueryValue>> EmptyIEnumeratorType = default(Enumerator);
	    private static readonly IEnumerator EmptyIEnumerator = default(Enumerator);
	    private Dictionary<HttpQueryKey, HttpQueryValue>? store;
	    public HttpQueryCollection() { }
	    public HttpQueryCollection(int capacity)
	    {
	        EnsureStore(capacity);
	    }
	    public HttpQueryCollection(Dictionary<HttpQueryKey, HttpQueryValue>? store)
	    {
	        this.store = store;
	    }
	    public HttpQueryValue this[HttpQueryKey key]
	    {
	        get
	        {
	            if (store == null)
	            {
	                return HttpQueryValue.Empty;
	            }
	            if (TryGetValue(key, out var value))
	            {
	                return value;
	            }
	            return HttpQueryValue.Empty;
	        }
	        set
	        {
	            if (key == null)
	            {
	                throw new ArgumentNullException("key");
	            }
	            ThrowIfReadOnly();
	            //if (value.Count == 0)
	            //{
	            //    store?.Remove(key);
	            //    return;
	            //}
	            EnsureStore(1);
	            store![key] = value;
	        }
	    }
	    public ICollection<HttpQueryKey> Keys => store == null ? EmptyKeys : store!.Keys;
	    public ICollection<HttpQueryValue> Values => store == null ? EmptyValues : store!.Values;
	    public int Count => store?.Count ?? 0;
	    public bool IsReadOnly { get; set; }
	    public void Add(HttpQueryKey key, HttpQueryValue value)
	    {
	        ThrowIfReadOnly();
	        EnsureStore(1);
	        store!.Add(key, value);
	    }
	    public void Add(KeyValuePair<HttpQueryKey, HttpQueryValue> item)
	    {
	        ThrowIfReadOnly();
	        EnsureStore(1);
	        store!.Add(item.Key, item.Value);
	    }
	    public void Clear()
	    {
	        ThrowIfReadOnly();
	        store?.Clear();
	    }
	    public bool Contains(KeyValuePair<HttpQueryKey, HttpQueryValue> item)
	    {
	        if (store == null || !store!.TryGetValue(item.Key, out var value) || value != item.Value)
	        {
	            return false;
	        }
	        return true;
	    }
	    public bool ContainsKey(HttpQueryKey key)
	    {
	        if (store == null)
	        {
	            return false;
	        }
	        return store!.ContainsKey(key);
	    }
	    public void CopyTo(KeyValuePair<HttpQueryKey, HttpQueryValue>[] array, int arrayIndex)
	    {
	        if (store == null)
	        {
	            return;
	        }
	        foreach (KeyValuePair<HttpQueryKey, HttpQueryValue> item in store!)
	        {
	            KeyValuePair<HttpQueryKey, HttpQueryValue> keyValuePair = (array[arrayIndex] = item);
	            arrayIndex++;
	        }
	    }
	    public bool Remove(HttpQueryKey key)
	    {
	        ThrowIfReadOnly();
	        if (store == null)
	        {
	            return false;
	        }
	        return store!.Remove(key);
	    }
	    public bool Remove(KeyValuePair<HttpQueryKey, HttpQueryValue> item)
	    {
	        ThrowIfReadOnly();
	        if (store == null)
	        {
	            return false;
	        }
	        if (store!.TryGetValue(item.Key, out var value) && item.Value == value)
	        {
	            return store!.Remove(item.Key);
	        }
	        return false;
	    }
	    public bool TryGetValue(HttpQueryKey key, [MaybeNullWhen(false)] out HttpQueryValue value)
	    {
	        if (store == null)
	        {
	            value = default(HttpQueryValue);
	            return false;
	        }
	        return store!.TryGetValue(key, out value);
	    }
	    public IEnumerator<KeyValuePair<HttpQueryKey, HttpQueryValue>> GetEnumerator()
	    {
	        if (store == null || store!.Count == 0)
	        {
	            return default(Enumerator);
	        }
	        return new Enumerator(store!.GetEnumerator());
	    }
	    IEnumerator IEnumerable.GetEnumerator()
	    {
	        if (store == null || store!.Count == 0)
	        {
	            return EmptyIEnumerator;
	        }
	        return store!.GetEnumerator();
	    }
	    [MemberNotNull("store")]
	    private void EnsureStore(int capacity)
	    {
	        if (store == null)
	        {
	            store = new Dictionary<HttpQueryKey, HttpQueryValue>(capacity);
	        }
	    }
	    private void ThrowIfReadOnly()
	    {
	        if (IsReadOnly)
	        {
	            throw new InvalidOperationException("The response headers cannot be modified because the response has already started.");
	        }
	    }
	    private struct Enumerator : IEnumerator<KeyValuePair<HttpQueryKey, HttpQueryValue>>, IEnumerator, IDisposable
	    {
	        private Dictionary<HttpQueryKey, HttpQueryValue>.Enumerator enumerator;
	        private readonly bool isNotEmpty;
	        public KeyValuePair<HttpQueryKey, HttpQueryValue> Current
	        {
	            get
	            {
	                if (isNotEmpty)
	                {
	                    return enumerator.Current;
	                }
	                return default(KeyValuePair<HttpQueryKey, HttpQueryValue>);
	            }
	        }
	        object IEnumerator.Current => Current;
	        internal Enumerator(Dictionary<HttpQueryKey, HttpQueryValue>.Enumerator dictionaryEnumerator)
	        {
	            enumerator = dictionaryEnumerator;
	            isNotEmpty = true;
	        }
	        public bool MoveNext() => isNotEmpty ? enumerator.MoveNext() : false;
	        public void Dispose() { }
	        void IEnumerator.Reset()
	        {
	            if (isNotEmpty)
	            {
	                ((IEnumerator)enumerator).Reset();
	            }
	        }
	    }
	}
	[DebuggerDisplay("{Value}")]
	public readonly struct HttpQueryKey : 
	    IEquatable<HttpQueryKey>,
	    IEqualityComparer<HttpQueryKey>,
	    IComparable<HttpQueryKey>
	{
	    private const StringComparison comparison = StringComparison.OrdinalIgnoreCase;
	    public HttpQueryKey(string value)
    {
	        if (string.IsNullOrEmpty(value))
	        {
	            ThrowUtility.ThrowArgumentNullException(nameof(value));
	        }
        Value = value;
    }
	    public string Value { get; }
	    public bool Equals(HttpQueryKey other)
	    {
	       return string.Equals(Value, other.Value, comparison);
	    }
	    public bool Equals(HttpQueryKey left, HttpQueryKey right)
	    {
	        return left.Equals(right);
	    }
	    public int GetHashCode(HttpQueryKey obj)
	    {
	        return obj.GetHashCode();
	    }
	    public int CompareTo(HttpQueryKey other)
	    {
	        return string.Compare(Value, other.Value, comparison);
	    }
	    #region Overloads
	    public override bool Equals(object? obj)
	    {
	        if (obj is HttpQueryKey key)
	        {
	            return Equals(key);
	        }
	        return false;
	    }
	    public override int GetHashCode()
	    {
	        return string.GetHashCode(Value, comparison);
	    }
	    public override string ToString()
	    {
	        return Value;
	    }
	    #endregion
	    #region Operators
	    public static implicit operator HttpQueryKey(string value) => new HttpQueryKey(value);
	    public static implicit operator string(HttpQueryKey key) => key.Value;
	    public static bool operator ==(HttpQueryKey left, HttpQueryKey right) => left.Equals(right);
	    public static bool operator !=(HttpQueryKey left, HttpQueryKey right) => !left.Equals(right);
	    #endregion
	}
	[DebuggerDisplay("{Value}")]
	public readonly partial struct HttpQueryValue : 
	    IEquatable<HttpQueryValue>, 
	    IEqualityComparer<HttpQueryValue>
	{
	    public HttpQueryValue(string value)
	    {
	        this.Value = value;
	    }
	    public string Value { get; }
	    public short GetInt16() => short.Parse(this.Value);
	    public int GetInt32() => int.Parse(this.Value);
	    public long GetInt64() => long.Parse(this.Value);
	    public decimal GetDecimal() => decimal.Parse(this.Value);
	    public double GetDouble() => double.Parse(this.Value);
	    public float GetFloat() => float.Parse(this.Value);
	    public bool GetBoolean() => bool.Parse(this.Value);
	    public DateOnly GetDate() => DateOnly.Parse(this.Value);
	    public DateTime GetDateTime() => DateTime.Parse(this.Value);
	    public DateTimeOffset GetDateTimeOffset() => DateTimeOffset.Parse(this.Value);
	    public TimeOnly GetTime() => TimeOnly.Parse(this.Value);
	    public TimeSpan GetTimeSpan() => TimeSpan.Parse(this.Value);
	    public override string ToString()
	    {
	        return Value;
	    }
	    public bool TryGetInt16(out short value)
	    {
	        value = 0;
	        if (short.TryParse(this.Value, out short v))
	        {
	            value = v;
	            return true;
	        }
	        return false;
	    }
	    public override int GetHashCode()
	    {
	        return this.Value.ToLower().GetHashCode() ^ this.Value.ToLower().GetHashCode();
	    }
	    bool IEquatable<HttpQueryValue>.Equals(HttpQueryValue other) => this.Value.Equals(other.Value, StringComparison.InvariantCultureIgnoreCase);
	    bool IEqualityComparer<HttpQueryValue>.Equals(HttpQueryValue left, HttpQueryValue right) => left.Equals(right);
	    int IEqualityComparer<HttpQueryValue>.GetHashCode([DisallowNull] HttpQueryValue instance) => instance.GetHashCode();
	    //public override string ToString() => string.Format("{0}={1}", this.Key, this.Value);
	    public static implicit operator short(HttpQueryValue query) => query.GetInt16();
	    public static implicit operator int(HttpQueryValue query) => query.GetInt32();
	    public static implicit operator long(HttpQueryValue query) => query.GetInt64();
	    public static implicit operator bool(HttpQueryValue query) => query.GetBoolean();
	    public static implicit operator DateOnly(HttpQueryValue query) => query.GetDate();
	    public static implicit operator DateTime(HttpQueryValue query) => query.GetDateTime();
	    public static implicit operator DateTimeOffset(HttpQueryValue query) => query.GetDateTimeOffset();
	    public static implicit operator TimeOnly(HttpQueryValue query) => query.GetTime();
	    public static implicit operator TimeSpan(HttpQueryValue query) => query.GetTimeSpan();
	    public static implicit operator HttpQueryValue(string value) => new HttpQueryValue(value);
	    public static HttpQueryValue Empty => new HttpQueryValue("");
	    public static bool operator ==(HttpQueryValue left, HttpQueryValue right)
	    {
	        return Equals(left, right);
	    }
	    public static bool operator !=(HttpQueryValue left, HttpQueryValue right)
	    {
	        return !Equals(left, right);
	    }
	    public static bool Equals(HttpQueryValue left, HttpQueryValue right)
	    {
	        return left.Value == right.Value;
	    }
	}
	public enum HttpScheme
	{
	    None = 1,
	    Http,
	    Https
	}
	public abstract class HttpSession
	{
	    public virtual bool IsAvailable { get; }
	    public virtual string Id { get; }
	    public virtual IEnumerable<string> Keys { get; }
	    //public virtual Task LoadAsync(CancellationToken cancellationToken = default(CancellationToken))
	    //{
	    //}
	    //Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken));
	    //bool TryGetValue(string key, [NotNullWhen(true)] out byte[]? value);
	    //void Set(string key, byte[] value);
	    //void Remove(string key);
	    //void Clear();
	}
	[DebuggerDisplay("{ToString()}")]
	public readonly struct HttpStatusCode : IEquatable<HttpStatusCode>
	{
	    private static ReadOnlySpan<KeyValuePair<int, string>> statusCodes => new KeyValuePair<int, string>[]
	    {
	        // 1xx Information
	        new (100, "Continue"), 
	        new (101, "Switching Protocols"), 
	        new (102, "Processing"),
	        new (103, "Early Hints"),
	        // 2xx Success
	        new (200, "Ok"), 
	        new (201, "Created"), 
	        new (202, "Accepted"), 
	        new (203, "Non-Authoritative Information"), 
	        new (204, "No Content"),
	        new (205, "Reset Content"), 
	        new (206, "Partial Content"), 
	        new (207, "Multi-Status"), 
	        new (208, "Already Reported"),
	        // 3xx Redirection
	        new (301, "Multiple Choices"),
	        new (301, "Moved Permanently"),
	        new (302, "Found"),
	        new (303, "See Other"),
	        new (304, "Not Modified"),
	        new (305, "Use Proxy"),
	        new (306, "(Unused)"),
	        new (307, "Redirect Keep Verb"),
	        new (308, "Permanent Redirect"),
	        // 4xx Client Error
	        new (400, "Bad Request"),
	        new (401, "Unauthorized"),
	        new (402, "Payment Required"),
	        new (403, "Forbidden"),
	        new (404, "Not Found"),
	        new (405, "Method Not Allowed"),
	        new (406, "Not Acceptable"),
	        new (407, "Proxy Authentication Required"),
	        new (408, "Request Timeout"),
	        new (409, "Conflict"),
	        new (410, "Gone"),
	        new (411, "Length Required"),
	        new (412, "Precondition Failed"),
	        new (413, "Request Entity Too Large"),
	        new (414, "Request Uri Too Long"),
	        new (415, "Unsupported Media Type"),
	        new (416, "Requested Range Not Satisfiable"),
	        new (417, "Expectation Failed"),
	        new (421, "Misdirected Request"),
	        new (422, "Un-Processable Entity"),
	        new (423, "Locked"),
	        new (424, "Failed Dependency"),
	        new (425, "Too Early"),
	        new (426, "Upgrade Required"),
	        new (428, "Precondition Required"),
	        new (429, "Too Many Requests"),
	        new (431, "Request Header Fields Too Large"),
	        new (451, "Unavailable For Legal Reasons"),
	        // 5xx Server Error
	        new (500, "Internal Server Error"),
	        new (501, "Not Implemented"),
	        new (502, "Bad Gateway"),
	        new (503, "Service Unavailable"),
	        new (504, "Gateway Timeout"),
	        new (505, "Http Version Not Supported"),
	        new (506, "Variant Also Negotiates"),
	        new (507, "Insufficient Storage"),
	        new (508, "Loop Detected"),
	        new (510, "Not Extended"),
	        new (511, "Network Authentication Required"),
	    };
	    public HttpStatusCode(int statusCode)
	    {
	        if (!IsValid(statusCode))
	        {
	            ThrowUtility.ThrowArgumentException($"The provided status code is invalid: '{statusCode}'");
	        }
	        Value = statusCode;
	    }
	    public int Value { get; }
	    public bool Equals(HttpStatusCode other)
	    {
	        return other.Value == Value;
	    }
	    #region Overloads
	    public override string ToString()
	    {
	        var value = string.Empty;
	        for (int i = 0; i < statusCodes.Length; i++)
	        {
	            var statusCode = statusCodes[i];
	            if (statusCode.Key == Value)
	            {
	                value = statusCode.Key + " " + statusCode.Value;
	            }
	        }
	        return value;
	    }
	    public override bool Equals(object? obj)
	    {
	        if (obj is HttpStatusCode statusCode)
	        {
	            return Equals(statusCode);
	        }
	        return false;
	    }
	    public override int GetHashCode()
	    {
	        return HashCode.Combine(typeof(HttpStatusCode), Value);
	    }
	    #endregion
	    #region Operatos
	    public static implicit operator HttpStatusCode(int statusCode) => new HttpStatusCode(statusCode);
	    public static implicit operator int(HttpStatusCode statusCode) => statusCode.Value;
	    #endregion
	    #region Static Helpers
	    public static HttpStatusCode Continue => 100;
	    public static HttpStatusCode SwitchingProtocols => 101;
	    public static HttpStatusCode Processing => 102;
	    public static HttpStatusCode EarlyHints => 103;
	    public static HttpStatusCode Ok => 200;
	    public static HttpStatusCode Created => 201;
	    public static HttpStatusCode Accepted => 202;
	    public static HttpStatusCode NonAuthoritativeInformation => 203;
	    public static HttpStatusCode NoContent => 204;
	    public static HttpStatusCode ResetContent => 205;
	    public static HttpStatusCode PartialContent => 206;
	    public static HttpStatusCode MultiStatus => 207;
	    public static HttpStatusCode AlreadyReported => 208;
	    public static HttpStatusCode MovedPermanently => 301;
	    public static HttpStatusCode Found => 302;
	    public static HttpStatusCode SeeOther => 303;
	    public static HttpStatusCode NotModified => 304;
	    public static HttpStatusCode UseProxy => 305;
	    public static HttpStatusCode Unused => 306;
	    public static HttpStatusCode RedirectKeepVerb => 307;
	    public static HttpStatusCode PermanentRedirect => 308;
	    public static HttpStatusCode BadRequest => 400;
	    public static HttpStatusCode Unauthorized => 401;
	    public static HttpStatusCode PaymentRequired => 402;
	    public static HttpStatusCode Forbidden => 403;
	    public static HttpStatusCode NotFound => 404;
	    public static HttpStatusCode MethodNotAllowed => 405;
	    public static HttpStatusCode NotAcceptable => 406;
	    public static HttpStatusCode ProxyAuthenticationRequired => 407;
	    public static HttpStatusCode RequestTimeout => 408;
	    public static HttpStatusCode Conflict => 409;
	    public static HttpStatusCode Gone => 410;
	    public static HttpStatusCode LengthRequired => 411;
	    public static HttpStatusCode PreconditionFailed => 412;
	    public static HttpStatusCode RequestEntityTooLarge => 413;
	    public static HttpStatusCode RequestUriTooLong => 414;
	    public static HttpStatusCode UnsupportedMediaType => 415;
	    public static HttpStatusCode RequestedRangeNotSatisfiable => 416;
	    public static HttpStatusCode ExpectationFailed => 417;
	    public static HttpStatusCode MisdirectedRequest => 421;
	    public static HttpStatusCode UnProcessableEntity => 422;
	    public static HttpStatusCode Locked => 423;
	    public static HttpStatusCode FailedDependency => 424;
	    public static HttpStatusCode UpgradeRequired => 426;
	    public static HttpStatusCode PreconditionRequired => 428;
	    public static HttpStatusCode TooManyRequests => 429;
	    public static HttpStatusCode RequestHeaderFieldsTooLarge => 431;
	    public static HttpStatusCode UnavailableForLegalReasons => 451;
	    public static HttpStatusCode InternalServerError => 500;
	    public static HttpStatusCode NotImplemented => 501;
	    public static HttpStatusCode BadGateway => 502;
	    public static HttpStatusCode ServiceUnavailable => 503;
	    public static HttpStatusCode GatewayTimeout => 504;
	    public static HttpStatusCode HttpVersionNotSupported => 505;
	    public static HttpStatusCode VariantAlsoNegotiates => 506;
	    public static HttpStatusCode InsufficientStorage => 507;
	    public static HttpStatusCode LoopDetected => 508;
	    public static HttpStatusCode NotExtended => 510;
	    public static HttpStatusCode NetworkAuthenticationRequired => 511;
	    #endregion
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public static bool IsValid(int statusCode)
	    {
	        for (int i = 0; i < statusCodes.Length; i++)
	        {
	            if (statusCodes[i].Key == statusCode)
	            {
	                return true;
	            }
	        }
	        return false;
	    }
	}
	public abstract class HttpTraceHandler
	{
	}
	public enum HttpVersion
	{
	    Http11 = 1,
	    Http12 = 2,
	    Http13 = 3
	}
	#endregion
	#region \Abstractions
	public interface IHttpContext : IAsyncDisposable
	{
	    HttpVersion Version { get; }
	    IHttpSession Session { get; }
	    IHttpRequest Request { get; }
	    IHttpResponse Response { get; }
	}
	public interface IHttpContextExecutor
	{
	    Task ExecuteAsync(IHttpContext context, CancellationToken cancellationToken = default);
	}
	public interface IHttpCookieCollection : ICollection<HttpCookie>
	{
	}
	public interface IHttpFormCollection
	{
	    IHttpFormFileCollection Files { get; }
	}
	public interface IHttpFormFileCollection
	{
	}
	public interface IHttpFormFile
	{
	}
	public interface IHttpHeaderCollection : IDictionary<HttpHeaderKey, HttpHeaderValue>
	{
	    HttpHeaderValue? Accepts { get; }
	    HttpHeaderValue? ContentType { get; }
	    HttpHeaderValue? ContentLength { get; }
	    HttpHeaderValue? TransferEncoding { get; }
	    HttpHeaderValue? Connection { get; }
	    HttpHeaderValue? AcceptCharset { get; }
	    HttpHeaderValue? AcceptEncoding { get; }
	    HttpHeaderValue? AcceptLanguage { get; }
	    HttpHeaderValue? AcceptRanges { get; }
	    HttpHeaderValue? AccessControlAllowCredentials { get; }
	    HttpHeaderValue? AccessControlAllowHeaders { get; }
	    HttpHeaderValue? AccessControlAllowMethods { get; }
	    HttpHeaderValue? AccessControlAllowOrigin { get; }
	    HttpHeaderValue? AccessControlExposeHeaders { get; }
	    HttpHeaderValue? AccessControlMaxAge { get; }
	    HttpHeaderValue? AccessControlRequestHeaders { get; }
	    HttpHeaderValue? AccessControlRequestMethod { get; }
	    HttpHeaderValue? Age { get; }
	    HttpHeaderValue? Allow { get; }
	    HttpHeaderValue? AltSvc { get; }
	    HttpHeaderValue? Authorization { get; }
	    HttpHeaderValue? Baggage { get; }
	    HttpHeaderValue? CacheControl { get; }
	    HttpHeaderValue? ContentDisposition { get; }
	    HttpHeaderValue? ContentEncoding { get; }
	    HttpHeaderValue? ContentLanguage { get; }
	    HttpHeaderValue? ContentLocation { get; }
	    HttpHeaderValue? ContentMD5 { get; }
	    HttpHeaderValue? ContentRange { get; }
	    HttpHeaderValue? ContentSecurityPolicy { get; }
	    HttpHeaderValue? ContentSecurityPolicyReportOnly { get; }
	    HttpHeaderValue? CorrelationContext { get; }
	    HttpHeaderValue? Cookie { get; }
	    HttpHeaderValue? Date { get; }
	    HttpHeaderValue? ETag { get; }
	    HttpHeaderValue? Expires { get; }
	    HttpHeaderValue? Expect { get; }
	    HttpHeaderValue? From { get; }
	    HttpHeaderValue? GrpcAcceptEncoding { get; }
	    HttpHeaderValue? GrpcEncoding { get; }
	    HttpHeaderValue? GrpcMessage { get; }
	    HttpHeaderValue? GrpcStatus { get; }
	    HttpHeaderValue? GrpcTimeout { get; }
	    HttpHeaderValue? Host { get; }
	    HttpHeaderValue? KeepAlive { get; }
	    HttpHeaderValue? IfMatch { get; }
	    HttpHeaderValue? IfModifiedSince { get; }
	    HttpHeaderValue? IfNoneMatch { get; }
	    HttpHeaderValue? IfRange { get; }
	    HttpHeaderValue? IfUnmodifiedSince { get; }
	    HttpHeaderValue? LastModified { get; }
	    HttpHeaderValue? Link { get; }
	    HttpHeaderValue? Location { get; }
	    HttpHeaderValue? MaxForwards { get; }
	    HttpHeaderValue? Origin { get; }
	    HttpHeaderValue? Pragma { get; }
	    HttpHeaderValue? ProxyAuthenticate { get; }
	    HttpHeaderValue? ProxyAuthorization { get; }
	    HttpHeaderValue? ProxyConnection { get; }
	    HttpHeaderValue? Range { get; }
	    HttpHeaderValue? Referer { get; }
	    HttpHeaderValue? RetryAfter { get; }
	    HttpHeaderValue? RequestId { get; }
	    HttpHeaderValue? SecWebSocketAccept { get; }
	    HttpHeaderValue? SecWebSocketKey { get; }
	    HttpHeaderValue? SecWebSocketProtocol { get; }
	    HttpHeaderValue? SecWebSocketVersion { get; }
	    HttpHeaderValue? SecWebSocketExtensions { get; }
	    HttpHeaderValue? Server { get; }
	    HttpHeaderValue? SetCookie { get; }
	    HttpHeaderValue? StrictTransportSecurity { get; }
	    HttpHeaderValue? TE { get; }
	    HttpHeaderValue? Trailer { get; }
	    HttpHeaderValue? Translate { get; }
	    HttpHeaderValue? TraceParent { get; }
	    HttpHeaderValue? TraceState { get; }
	    HttpHeaderValue? Upgrade { get; }
	    HttpHeaderValue? UpgradeInsecureRequests { get; }
	    HttpHeaderValue? UserAgent { get; }
	    HttpHeaderValue? Vary { get; }
	    HttpHeaderValue? Via { get; }
	    HttpHeaderValue? Warning { get; }
	    HttpHeaderValue? WebSocketSubProtocols { get; }
	    HttpHeaderValue? WWWAuthenticate { get; }
	    HttpHeaderValue? XContentTypeOptions { get; }
	    HttpHeaderValue? XFrameOptions { get; }
	    HttpHeaderValue? XPoweredBy { get; }
	    HttpHeaderValue? XRequestedWith { get; }
	    HttpHeaderValue? XUACompatible { get; }
	    HttpHeaderValue? XXSSProtection { get; }
	}
	public interface IHttpQueryCollection : IDictionary<HttpQueryKey, HttpQueryValue>
	{
	}
	public interface IHttpRequest
	{
	    HttpHost Host { get; }
	    HttpPath Path { get; }
	    HttpMethod Method { get; }
	    HttpScheme Scheme { get; }
	    IHttpQueryCollection Query { get; }
	    IHttpHeaderCollection Headers { get; }
	    IHttpCookieCollection Cookies { get; }
	    IHttpFormCollection Form { get; }
	    Stream Body { get; }
	    ClaimsPrincipal ClaimsPrincipal { get; }
	}
	public interface IHttpResponse
	{
	    HttpStatusCode StatusCode { get; }
	    IHttpHeaderCollection Headers { get; }
	    IHttpCookieCollection Cookies { get; }
	    Stream Body { get; }
	}
	public interface IHttpSession
	{
	    string Id { get; }
	}
	#endregion
	#region \Delegates
	public delegate Task HttpContextHandler(IHttpContext context, HttpContextHandler next, CancellationToken cancellationToken);
	#endregion
	#region \Exceptions
	public abstract class HttpException : Exception
	{
	    public HttpException(string message) : base(message) { }
	    public HttpException(string message, Exception inner) : base(message, inner) { }
	    public HttpExceptionCode Code { get; init; }
	}
	public enum HttpExceptionCode
	{
	    Unknown = 0,
	    ReadingError,
	    WritingError,
	    ExecutionError,
	}
	#endregion
	#region \Internal\Utilities
	internal static class ThrowUtility
	{
	    #region General Exceptions
	    [DoesNotReturn]
	    public static void ThrowArgumentException() => throw new ArgumentException();
	    [DoesNotReturn]
	    public static void ThrowArgumentException(string message) => throw new ArgumentException(message);
	    [DoesNotReturn]
	    public static void ThrowArgumentException(string message, Exception innerException) => throw new ArgumentException(message, innerException);
	    [DoesNotReturn]
	    public static void ThrowArgumentNullException(string paramName) => throw new ArgumentNullException(paramName);
	    #endregion
	    public static void InvalidHttpPath(string message) => 
	        throw new HttpInvalidPathException(message);
	    internal static void InvalidHttpMethod(string method) => 
	        throw new HttpInvalidMethodException($"The provided method is invalid: '{method}'. A method can only contain alphanumeric characters.");
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
	#region \Properties
	#endregion
	#region \Server
	public partial class HttpServer
	{
	    private readonly IList<ITransport> transports;
	    private readonly IHttpContextExecutor executor;
	    private readonly HttpConnectionFactory factory;
	    private readonly HttpServerOptions options;
	    internal HttpServer(HttpServerOptionsInternal options)
	    {
	        if (options is null)
	        {
	            throw new ArgumentNullException(nameof(options));
	        }
	        this.options = options;
	        this.transports = options.Transports;
	        this.executor = options.Executor;
	        this.factory = HttpConnectionFactory.New();
	    }
	    public Task StartAsync(CancellationToken cancellationToken = default)
	    {
	        return ProcessAsync(cancellationToken);
	    }
	    private async Task ProcessAsync(CancellationToken cancellationToken = default)
	    {
	        using var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
	        while (true)
	        {
	            await foreach (var transportConnection in ProcessTransportConnectionsAsync().WithCancellation(cancellationTokenSource.Token))
	            {
	                try
	                {
	                    // Depending on the HTTP version the ability to taskQueue HTTP workloads will allow 
	                    // for the continuation of accepting further requests of other clients. Version such as
	                    // HTTP 1.1, HTTP/2 and HTTP/3 which allow for multiplex/pipelining connections (layman: receiving multiple HTTP request over one transportConnection.)
	                    // could technically stay open indefinitely. Although HTTP1.1 uses the same transportConnection, it does not implement true multiplexing since it does not have data frames that allow for responding
	                    // to multiple HTTP Requests asynchronously
	                    var queued = ThreadPool.UnsafeQueueUserWorkItem(async connection =>
	                    {
	                        try
	                        {
	                            var httpConnection = factory.Create(new()
	                            {
	                                Connection = connection,
	                                Executor = this.executor,
	                            });
	                            // Since transportConnection can remain open using IAsyncEnumerable will allow for 
	                            // async disposable
	                            await foreach (IAsyncDisposable disposable in httpConnection.ProcessAsync().WithCancellation(cancellationToken))
	                            {
	                                await disposable.DisposeAsync();
	                            }
	                        }
	                        catch (Exception)
	                        {
	                        }
	                    }, transportConnection, false);
	                }
	                catch (Exception)
	                {
	                    continue;
	                }
	            }
	        }
	        async IAsyncEnumerable<ITransportConnection> ProcessTransportConnectionsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
	        {
	            // Will use this as
	            var taskQueue = new Dictionary<Task<ITransportConnection>, int>();
	            while (true)
	            {
	                // Queue/Re-Queue
	                foreach (var transport in this.transports)
	                {
	                    var hashCode = transport.GetHashCode();
	                    if (!taskQueue.Values.Contains(hashCode))
	                    {
	                        // The underlying transports should handle exceptions and restart accepting 
	                        // connections which is why checking null is all that is needed.
	                        taskQueue.Add(transport.InitializeAsync(cancellationToken), hashCode);
	                    }
	                }
	                var tasks = taskQueue.Select(task => task.Key);
	                var taskCompleted = await Task.WhenAny(tasks);
	                taskQueue.Remove(taskCompleted);
	                var transportConnection = await taskCompleted;
	                // If null, most likely result of connection being aborted.
	                if (transportConnection is null)
	                {
	                    continue;
	                }
	                yield return transportConnection;
	            }
	        }
	    }
	    public Task StopAsync(CancellationToken cancellationToken = default)
	    {
	        foreach (var transport in this.transports)
	        {
	            transport.Dispose();
	        }
	        return Task.CompletedTask;
	    }
	    public void Dispose() => DisposeAsync().GetAwaiter().GetResult();
	    public async ValueTask DisposeAsync()
	    {
	        await StopAsync();
	    }
	}
	public sealed class HttpServerBuilder 
	{
	    private Func<IServiceProvider> serviceProviderAction;
	    private IList<Action<HttpServerOptions>> onBuild;
	    public HttpServerBuilder() //(IHostContext context)
	    {
	        this.onBuild = new List<Action<HttpServerOptions>>();
	        this.serviceProviderAction = () => default!;
	    }
	    public HttpServerBuilder ConfigureServer(Action<HttpServerOptions> configure)
	    {
	        if (configure is null)
	        {
	            ThrowUtility.ThrowArgumentNullException(nameof(configure));
	        }
	        onBuild.Add(configure);
	        return this;
	    }
	    public HttpServer Build()
	    {
	       // var options = new HttpServerOptionsInternal();
	        //var serviceProvider = serviceProviderAction.Invoke();
	        //foreach (var setting in settings)
	        //{
	        //    setting.Invoke(serviceProvider, options);
	        //}
	        return new HttpServer(default!);
	    }
	}
	public abstract class HttpServerOptions
	{
	    public string ServerName { get; set; } = "Cohesion .Net HTTP Server";
	    public TimeSpan ConnectionTimeout { get; set;  }
	    public abstract void UseHttp(HttpVersion version);
	    public abstract void UseHttps(HttpVersion version);
	    public abstract void UseTransport(ITransport transport);
	    public abstract void UseTransport(Func<ITransport> configure);
	    public abstract void UseTcpTransport(Action<TcpServerTransportOptions> configure);
	}
	#endregion
	#region \Server\Internal
	    internal class DefaultHttpContextExecutor : IHttpContextExecutor
	    {
	        private readonly HttpContextHandler handler;
	        public DefaultHttpContextExecutor(HttpContextHandler handler)
	        {
	            this.handler = handler;
	        }
	        public Task ExecuteAsync(IHttpContext context, CancellationToken cancellationToken = default)
	        {
	            return handler.Invoke(context, default);
	        }
	    }
	// This base class will be responsible for pumping 
	internal abstract class HttpConnection
	{
	    internal abstract IAsyncEnumerable<IHttpContext> ProcessAsync([EnumeratorCancellation] CancellationToken cancellationToken = default);
	    protected virtual IAsyncEnumerable<IHttpContext> ReceiveAsync(CancellationToken cancellationToken = default)
	    {
	        throw new NotImplementedException();
	    }
	    protected virtual IAsyncEnumerable<IHttpContext> SendAsync(IHttpContext context, CancellationToken cancellationToken = default)
	    {
	        throw new NotImplementedException();
	    }
	}
	internal class HttpConnectionContext
	{
	    internal TimeSpan ReceivingTimeout { get; init; }
	    public IHttpContextExecutor Executor { get; init; } = default!;
	    public ITransportConnection Connection { get; init; } = default!;
	}
	internal sealed class HttpConnectionFactory
	{
	    public static HttpConnectionFactory factory = default!;
	    private HttpConnectionFactory() { }
	    public HttpConnection Create(HttpConnectionContext context)
	    {
	        return new Http1Connection(context);
	    }
	    public static HttpConnectionFactory New() => factory ??= new HttpConnectionFactory();
	}
	internal enum HttpConnectionOption
	{
	    None = 0,
	    Close = 1,
	    KeepAlive = 2,
	    Upgrade = 4
	}
	internal abstract class HttpContext : IHttpContext
	{
	    public abstract HttpVersion Version { get; }
	    public abstract IHttpSession Session { get; }
	    public abstract IHttpRequest Request { get; }
	    public abstract IHttpResponse Response { get; }
	    public abstract ValueTask DisposeAsync();
	}
	internal static class HttpResponsePhrases
	{
	    private static readonly Dictionary<int, string> phrases = new()
	    {
	        { 100, "Continue" },
	        { 101, "Switching Protocols" },
	        { 102, "Processing" },
	        { 200, "OK" },
	        { 201, "Created" },
	        { 202, "Accepted" },
	        { 203, "Non-Authoritative Information" },
	        { 204, "No Content" },
	        { 205, "Reset Content" },
	        { 206, "Partial Content" },
	        { 207, "Multi-Status" },
	        { 208, "Already Reported" },
	        { 226, "IM Used" },
	        { 300, "Multiple Choices" },
	        { 301, "Moved Permanently" },
	        { 302, "Found" },
	        { 303, "See Other" },
	        { 304, "Not Modified" },
	        { 305, "Use Proxy" },
	        { 306, "Switch Proxy" },
	        { 307, "Temporary Redirect" },
	        { 308, "Permanent Redirect" },
	        { 400, "Bad Request" },
	        { 401, "Unauthorized" },
	        { 402, "Payment Required" },
	        { 403, "Forbidden" },
	        { 404, "Not Found" },
	        { 405, "Method Not Allowed" },
	        { 406, "Not Acceptable" },
	        { 407, "Proxy Authentication Required" },
	        { 408, "Request Timeout" },
	        { 409, "Conflict" },
	        { 410, "Gone" },
	        { 411, "Length Required" },
	        { 412, "Precondition Failed" },
	        { 413, "Payload Too Large" },
	        { 414, "URI Too Long" },
	        { 415, "Unsupported Media Type" },
	        { 416, "Range Not Satisfiable" },
	        { 417, "Expectation Failed" },
	        { 418, "I'm a teapot" },
	        { 419, "Authentication Timeout" },
	        { 421, "Misdirected Request" },
	        { 422, "Unprocessable Entity" },
	        { 423, "Locked" },
	        { 424, "Failed Dependency" },
	        { 426, "Upgrade Required" },
	        { 428, "Precondition Required" },
	        { 429, "Too Many Requests" },
	        { 431, "Request Header Fields Too Large" },
	        { 451, "Unavailable For Legal Reasons" },
	        { 500, "Internal Server Error" },
	        { 501, "Not Implemented" },
	        { 502, "Bad Gateway" },
	        { 503, "Service Unavailable" },
	        { 504, "Gateway Timeout" },
	        { 505, "HTTP Version Not Supported" },
	        { 506, "Variant Also Negotiates" },
	        { 507, "Insufficient Storage" },
	        { 508, "Loop Detected" },
	        { 510, "Not Extended" },
	        { 511, "Network Authentication Required" },
	    };
	    public static string GetStatusCodePhrase(int statucCode) => phrases[statucCode];
	}
	internal static class HttpRules
	{
	    // Controls
	    public const byte CarriageReturn = (byte)'\r';
	    public const byte LineFeed = (byte)'\n';
	    public const byte Space = (byte)' ';
	    public const byte Tab = (byte)'\t';
	    public const int MaxInt64Digits = 19;
	    public const int MaxInt32Digits = 10;
	    public static readonly string[] DateFormats = new string[]
	    {
	        // "r", // RFC 1123, required output format but too strict for input
	        "ddd, d MMM yyyy H:m:s 'GMT'",      // RFC 1123 (r, except it allows both 1 and 01 for date and time)
	        "ddd, d MMM yyyy H:m:s",            // RFC 1123, no zone - assume GMT
	        "d MMM yyyy H:m:s 'GMT'",           // RFC 1123, no day-of-week
	        "d MMM yyyy H:m:s",                 // RFC 1123, no day-of-week, no zone
	        "ddd, d MMM yy H:m:s 'GMT'",        // RFC 1123, short year
	        "ddd, d MMM yy H:m:s",              // RFC 1123, short year, no zone
	        "d MMM yy H:m:s 'GMT'",             // RFC 1123, no day-of-week, short year
	        "d MMM yy H:m:s",                   // RFC 1123, no day-of-week, short year, no zone
	        "dddd, d'-'MMM'-'yy H:m:s 'GMT'",   // RFC 850, short year
	        "dddd, d'-'MMM'-'yy H:m:s",         // RFC 850 no zone
	        "ddd, d'-'MMM'-'yyyy H:m:s 'GMT'",  // RFC 850, long year
	        "ddd MMM d H:m:s yyyy",             // ANSI C's asctime() format
	        "ddd, d MMM yyyy H:m:s zzz",        // RFC 5322
	        "ddd, d MMM yyyy H:m:s",            // RFC 5322 no zone
	        "d MMM yyyy H:m:s zzz",             // RFC 5322 no day-of-week
	        "d MMM yyyy H:m:s",                 // RFC 5322 no day-of-week, no zone
	    };
	}
	internal sealed class HttpServerOptionsInternal : HttpServerOptions
	{
	    public IServiceProvider? ServiceProvider { get; init; }
	    public IHttpContextExecutor Executor { get; init; }
	    public IList<ITransport> Transports { get; set; } = new List<ITransport>();
	    public override void UseTransport(ITransport transport)
	    {
	        ValidateTransport(transport);
	        Transports.Add(transport);
	    }
	    public override void UseTransport(Func<ITransport> configure)
	    {
	        var transport = configure.Invoke();
	        ValidateTransport(transport);
	        Transports.Add(transport);
	    }
	    public override void UseTcpTransport(Action<TcpServerTransportOptions> configure)
	    {
	        if (configure is null)
	        {
	            throw new ArgumentNullException(nameof(configure));
	        }
	        var options = new TcpServerTransportOptions();
	        configure.Invoke(options);
	        Transports.Add(new TcpServerTransport(options));
	    }
	    private void ValidateTransport(ITransport transport)
	    {
	        if (transport is null)
	        {
	            throw new ArgumentNullException(nameof(transport));
	        }
	        if (transport.TransportType == TransportType.Client)
	        {
	            throw new ArgumentException("Transport must be a server configure.", nameof(transport));
	        }
	        if (transport.ProtocolType != ProtocolType.Tcp && transport.ProtocolType != ProtocolType.Quic)
	        {
	            throw new ArgumentException("Transport must be a TCP or QUIC configure.", nameof(transport));
	        }
	    }
	    public override void UseExecutor(IHttpContextExecutor executor)
	    {
	        throw new NotImplementedException();
	    }
	}
	internal class HttpValues
	{
	    internal static ReadOnlySpan<byte> Version1 => new Span<byte>(new byte[] { (byte)'H', (byte)'T', (byte)'T', (byte)'P', (byte)'/', (byte)'1', (byte)'.', (byte)'1' });
	    internal class Separators
	    {
	        internal static ReadOnlySpan<byte> CarriageReturn => new ReadOnlySpan<byte>(new byte[] { (byte)'\r' });
	        internal static ReadOnlySpan<byte> LineFeed => new ReadOnlySpan<byte>(new byte[] { (byte)'\n' });
	        internal static ReadOnlySpan<byte> Space => new ReadOnlySpan<byte>(new byte[] { (byte)' ' });
	        internal static ReadOnlySpan<byte> Colon => new ReadOnlySpan<byte>(new byte[] { (byte)':' });
	        internal static ReadOnlySpan<byte> Tab => new ReadOnlySpan<byte>(new byte[] { (byte)'\t' });
	        internal static ReadOnlySpan<byte> QuestionMark => new ReadOnlySpan<byte>(new byte[] { (byte)'?' });
	        internal static ReadOnlySpan<byte> Percentage => new ReadOnlySpan<byte>(new byte[] { (byte)'%' });        
	        internal static ReadOnlySpan<byte> NewLine => new ReadOnlySpan<byte>(new byte[] { (byte)'\r', (byte)'\n' });
	    }
	    internal class StatusCodes
	    {
	        // 1xx
	        internal static ReadOnlySpan<byte> Continue => UTF8.GetBytes("100 Continue");
	        internal static ReadOnlySpan<byte> SwitchProtocols => UTF8.GetBytes("101 Switching Protocols");
	        // 2xx
	        internal static ReadOnlySpan<byte> Ok => UTF8.GetBytes("200 OK");
	        internal static ReadOnlySpan<byte> Created => UTF8.GetBytes("201 Created");
	        internal static ReadOnlySpan<byte> Accepted => UTF8.GetBytes("202 Accepted");
	        internal static ReadOnlySpan<byte> NonAuthoritativeInformation => UTF8.GetBytes("203 Non-Authoritative Information");
	        internal static ReadOnlySpan<byte> NoContent => UTF8.GetBytes("204 No Content");
	        internal static ReadOnlySpan<byte> ResetContent => UTF8.GetBytes("205 Reset Content");
	        internal static ReadOnlySpan<byte> PartialContent => UTF8.GetBytes("206 Partial Content");
	        // 3xx
	        internal static ReadOnlySpan<byte> MultipleChoices => UTF8.GetBytes("300 Multiple Choices");
	        internal static ReadOnlySpan<byte> MovedPermanently => UTF8.GetBytes("301 Moved Permanently");
	        internal static ReadOnlySpan<byte> Found => UTF8.GetBytes("302 Found");
	        internal static ReadOnlySpan<byte> SeeOther => UTF8.GetBytes("303 See Other");
	        internal static ReadOnlySpan<byte> NotModified => UTF8.GetBytes("304 Not Modified");
	        internal static ReadOnlySpan<byte> UseProxy => UTF8.GetBytes("305 Use Proxy");
	        internal static ReadOnlySpan<byte> TemporaryRedirect => UTF8.GetBytes("307 Temporary Redirect");
	        internal static ReadOnlySpan<byte> PermanentRedirect => UTF8.GetBytes("308 Permanent Redirect");
	        // 4xx
	        internal static ReadOnlySpan<byte> BadRequest => UTF8.GetBytes("400 Bad Request");
	        internal static ReadOnlySpan<byte> Unauthorized => UTF8.GetBytes("401 Unauthorized");
	        internal static ReadOnlySpan<byte> MethodNotAllowed => UTF8.GetBytes("403 Method Not Allowed");
	        internal static ReadOnlySpan<byte> NotFound => UTF8.GetBytes("404 Not Found");
	        internal static ReadOnlySpan<byte> NotAcceptable => UTF8.GetBytes("406 Not Acceptable");
	    }
	}
	#endregion
	#region \Server\Internal\Exceptions
	internal class HttpInvalidMethodException : HttpException
	{
	    public HttpInvalidMethodException(string message) : base(message)
	    {
	    }
	}
	internal class HttpInvalidPathException : HttpException
	{
	    public HttpInvalidPathException(string message) : base(message) { }
	}
	#endregion
	#region \Server\Internal\Extensions
	internal static class BufferExtensions
	{
	    private const int _maxULongByteLength = 20;
	    [ThreadStatic]
	    private static byte[]? _numericBytesScratch;
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public static ReadOnlySpan<byte> ToSpan(in this ReadOnlySequence<byte> buffer)
	    {
	        if (buffer.IsSingleSegment)
	        {
	            return buffer.FirstSpan;
	        }
	        return buffer.ToArray();
	    }
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public static void CopyTo(in this ReadOnlySequence<byte> buffer, PipeWriter pipeWriter)
	    {
	        if (buffer.IsSingleSegment)
	        {
	            pipeWriter.Write(buffer.FirstSpan);
	        }
	        else
	        {
	            CopyToMultiSegment(buffer, pipeWriter);
	        }
	    }
	    private static void CopyToMultiSegment(in ReadOnlySequence<byte> buffer, PipeWriter pipeWriter)
	    {
	        foreach (var item in buffer)
	        {
	            pipeWriter.Write(item.Span);
	        }
	    }
	    public static ArraySegment<byte> GetArray(this Memory<byte> buffer)
	    {
	        return ((ReadOnlyMemory<byte>)buffer).GetArray();
	    }
	    public static ArraySegment<byte> GetArray(this ReadOnlyMemory<byte> memory)
	    {
	        if (!MemoryMarshal.TryGetArray(memory, out var result))
	        {
	            throw new InvalidOperationException("Buffer backed by array was expected");
	        }
	        return result;
	    }
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public static SequencePosition? PositionOfAny<T>(in this ReadOnlySequence<T> source, T value0, T value1) where T : IEquatable<T>
	    {
	        if (source.IsSingleSegment)
	        {
	            int index = source.First.Span.IndexOfAny(value0, value1);
	            if (index != -1)
	            {
	                return source.GetPosition(index);
	            }
	            return null;
	        }
	        else
	        {
	            return PositionOfAnyMultiSegment(source, value0, value1);
	        }
	    }
	    private static SequencePosition? PositionOfAnyMultiSegment<T>(in ReadOnlySequence<T> source, T value0, T value1) where T : IEquatable<T>
	    {
	        SequencePosition position = source.Start;
	        SequencePosition result = position;
	        while (source.TryGet(ref position, out ReadOnlyMemory<T> memory))
	        {
	            int index = memory.Span.IndexOfAny(value0, value1);
	            if (index != -1)
	            {
	                return source.GetPosition(index, result);
	            }
	            else if (position.GetObject() == null)
	            {
	                break;
	            }
	            result = position;
	        }
	        return null;
	    }
	    internal static void WriteAscii(ref this BufferWriter<PipeWriter> buffer, string data)
	    {
	        if (string.IsNullOrEmpty(data))
	        {
	            return;
	        }
	        var dest = buffer.Span;
	        var sourceLength = data.Length;
	        // Fast path, try encoding to the available memory directly
	        if (sourceLength <= dest.Length)
	        {
	            Encoding.ASCII.GetBytes(data, dest);
	            buffer.Advance(sourceLength);
	        }
	        else
	        {
	            WriteEncodedMultiWrite(ref buffer, data, sourceLength, Encoding.ASCII);
	        }
	    }
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    internal static unsafe void WriteNumeric(ref this BufferWriter<PipeWriter> buffer, ulong number)
	    {
	        const byte AsciiDigitStart = (byte)'0';
	        var span = buffer.Span;
	        var bytesLeftInBlock = span.Length;
	        // Fast path, try copying to the available memory directly
	        var simpleWrite = true;
	        fixed (byte* output = span)
	        {
	            var start = output;
	            if (number < 10 && bytesLeftInBlock >= 1)
	            {
	                *(start) = (byte)(((uint)number) + AsciiDigitStart);
	                buffer.Advance(1);
	            }
	            else if (number < 100 && bytesLeftInBlock >= 2)
	            {
	                var val = (uint)number;
	                var tens = (byte)((val * 205u) >> 11); // div10, valid to 1028
	                *(start) = (byte)(tens + AsciiDigitStart);
	                *(start + 1) = (byte)(val - (tens * 10) + AsciiDigitStart);
	                buffer.Advance(2);
	            }
	            else if (number < 1000 && bytesLeftInBlock >= 3)
	            {
	                var val = (uint)number;
	                var digit0 = (byte)((val * 41u) >> 12); // div100, valid to 1098
	                var digits01 = (byte)((val * 205u) >> 11); // div10, valid to 1028
	                *(start) = (byte)(digit0 + AsciiDigitStart);
	                *(start + 1) = (byte)(digits01 - (digit0 * 10) + AsciiDigitStart);
	                *(start + 2) = (byte)(val - (digits01 * 10) + AsciiDigitStart);
	                buffer.Advance(3);
	            }
	            else
	            {
	                simpleWrite = false;
	            }
	        }
	        if (!simpleWrite)
	        {
	            WriteNumericMultiWrite(ref buffer, number);
	        }
	    }
	    [MethodImpl(MethodImplOptions.NoInlining)]
	    private static void WriteNumericMultiWrite(ref this BufferWriter<PipeWriter> buffer, ulong number)
	    {
	        const byte AsciiDigitStart = (byte)'0';
	        var value = number;
	        var position = _maxULongByteLength;
	        var byteBuffer = NumericBytesScratch;
	        do
	        {
	            // Consider using Math.DivRem() if available
	            var quotient = value / 10;
	            byteBuffer[--position] = (byte)(AsciiDigitStart + (value - quotient * 10)); // 0x30 = '0'
	            value = quotient;
	        }
	        while (value != 0);
	        var length = _maxULongByteLength - position;
	        buffer.Write(new ReadOnlySpan<byte>(byteBuffer, position, length));
	    }
	    internal static void WriteEncoded(ref this BufferWriter<PipeWriter> buffer, string data, Encoding encoding)
	    {
	        if (string.IsNullOrEmpty(data))
	        {
	            return;
	        }
	        var dest = buffer.Span;
	        var sourceLength = encoding.GetByteCount(data);
	        // Fast path, try encoding to the available memory directly
	        if (sourceLength <= dest.Length)
	        {
	            encoding.GetBytes(data, dest);
	            buffer.Advance(sourceLength);
	        }
	        else
	        {
	            WriteEncodedMultiWrite(ref buffer, data, sourceLength, encoding);
	        }
	    }
	    [MethodImpl(MethodImplOptions.NoInlining)]
	    private static void WriteEncodedMultiWrite(ref this BufferWriter<PipeWriter> buffer, string data, int encodedLength, Encoding encoding)
	    {
	        var source = data.AsSpan();
	        var totalBytesUsed = 0;
	        var encoder = encoding.GetEncoder();
	        var minBufferSize = encoding.GetMaxByteCount(1);
	        buffer.Ensure(minBufferSize);
	        var bytes = buffer.Span;
	        var completed = false;
	        // This may be a bug, but encoder.Convert returns completed = true for UTF7 too early.
	        // Therefore, we check encodedLength - totalBytesUsed too.
	        while (!completed || encodedLength - totalBytesUsed != 0)
	        {
	            // Zero length spans are possible, though unlikely.
	            // encoding.Convert and .Advance will both handle them so we won't special case for them.
	            encoder.Convert(source, bytes, flush: true, out var charsUsed, out var bytesUsed, out completed);
	            buffer.Advance(bytesUsed);
	            totalBytesUsed += bytesUsed;
	            if (totalBytesUsed >= encodedLength)
	            {
	                Debug.Assert(totalBytesUsed == encodedLength);
	                // Encoded everything
	                break;
	            }
	            source = source.Slice(charsUsed);
	            // Get new span, more to encode.
	            buffer.Ensure(minBufferSize);
	            bytes = buffer.Span;
	        }
	    }
	    private static byte[] NumericBytesScratch => _numericBytesScratch ?? CreateNumericBytesScratch();
	    [MethodImpl(MethodImplOptions.NoInlining)]
	    private static byte[] CreateNumericBytesScratch()
	    {
	        var bytes = new byte[_maxULongByteLength];
	        _numericBytesScratch = bytes;
	        return bytes;
	    }
	}
	internal static class ValueTaskExtensions
	{
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public static Task GetAsTask(this in ValueTask<FlushResult> valueTask)
	    {
	        // Try to avoid the allocation from AsTask
	        if (valueTask.IsCompletedSuccessfully)
	        {
	            // Signal consumption to the IValueTaskSource
	            valueTask.GetAwaiter().GetResult();
	            return Task.CompletedTask;
	        }
	        else
	        {
	            return valueTask.AsTask();
	        }
	    }
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public static ValueTask GetAsValueTask(this in ValueTask<FlushResult> valueTask)
	    {
	        // Try to avoid the allocation from AsTask
	        if (valueTask.IsCompletedSuccessfully)
	        {
	            // Signal consumption to the IValueTaskSource
	            valueTask.GetAwaiter().GetResult();
	            return default;
	        }
	        else
	        {
	            return new ValueTask(valueTask.AsTask());
	        }
	    }
	}
	#endregion
	#region \Server\Internal\Http1
	internal partial class Http1Connection : HttpConnection
	{
	    private readonly IHttpContextExecutor executor;
	    private readonly ITransportConnection connection;
	    public Http1Connection(HttpConnectionContext context)
	    {
	        this.connection = context.Connection;
	        this.executor = context.Executor;
	    }
	    internal override async IAsyncEnumerable<IHttpContext> ProcessAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
	    {
	        var reader = Http1RequestReader.Create();
	        var writer = Http1ResponseWriter.Create();
	        while (true)
	        {
	            var context = new Http1Context();
	            try
	            {
	                await reader.ReadAsync(context, connection);
	                await executor.ExecuteAsync(context);
	                await writer.WriteAsync(context, connection);
	                // Check if the underlying transport connection needs to be closed
	                //var connectionHeader = context.Request.Headers.Connection;
	                //if (connectionHeader.HasValue && connectionHeader.Value == "close")
	                //{
	                //    await connection.AbortAsync();
	                //    break;
	                //}
	            }
	            catch (Exception exception)
	            {
	                connection.Abort();
	                break;
	            }
	            yield return context;
	        }
	    }
	}
	internal sealed class Http1Context : IHttpContext
	{
	    internal volatile bool IsDisposed;
	    public HttpVersion Version => HttpVersion.Http11;
	    public Http1Request Request { get; set; } = new();
	    public Http1Response Response { get; set; } = new();
	    public Http1Session Session { get; set; } = new();
	    public IServiceProvider ServiceProvider { get; set; }
	    IHttpSession IHttpContext.Session => this.Session;
	    IHttpRequest IHttpContext.Request => this.Request;
	    IHttpResponse IHttpContext.Response => this.Response;
	    public ValueTask DisposeAsync()
	    {
	        IsDisposed = true;
	        return ValueTask.CompletedTask;
	    }
	}
	internal class Http1Request : IHttpRequest
	{
	    public HttpPath Path { get; set; }
	    public HttpMethod Method { get; set; }
	    public HttpScheme Scheme  { get; set; }
	    public HttpQueryCollection Query { get; } = new();
	    IHttpQueryCollection IHttpRequest.Query => this.Query;
	    public HttpHeaderCollection Headers { get; } = new();
	    IHttpHeaderCollection IHttpRequest.Headers => this.Headers;
	    public IHttpCookieCollection Cookies => throw new NotImplementedException();
	    public Stream Body { get; set; }
	    public HttpHost Host => throw new NotImplementedException();
	    public IHttpFormCollection Form => throw new NotImplementedException();
	    public ClaimsPrincipal ClaimsPrincipal => throw new NotImplementedException();
	}
	internal class Http1RequestStream : Stream
	{
	    private readonly Http1MessageBody body;
	    public Http1RequestStream(Http1MessageBody body)
	    {
	        this.body = body!;
	    }
	    public override bool CanRead => true;
	    public override bool CanSeek => false;
	    public override bool CanWrite => false;
	    public override long Length => throw new NotSupportedException();
	    public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
	    public override void Flush()
	    {
	        throw new NotImplementedException();
	    }
	    public override int Read(byte[] buffer, int offset, int count)
	    {
	        throw new NotImplementedException();
	    }
	    public override long Seek(long offset, SeekOrigin origin)
	    {
	        throw new NotImplementedException();
	    }
	    public override void SetLength(long value)
	    {
	        throw new NotImplementedException();
	    }
	    public override void Write(byte[] buffer, int offset, int count)
	    {
	        throw new NotImplementedException();
	    }
	}
	internal class Http1Response : IHttpResponse
	{
	    public HttpStatusCode StatusCode { get; set; } = 404;
	    public IHttpHeaderCollection Headers { get;  } = new HttpHeaderCollection();
	    public IHttpCookieCollection Cookies => throw new NotImplementedException();
	    public Stream Body { get; set; }
	}
	internal class Http1ResponseStream : Stream
	{
	    private readonly Http1MessageBody body;
	    public Http1ResponseStream(Http1MessageBody body)
	    {
	        this.body = body;
	    }
	    public override bool CanRead => throw new NotImplementedException();
	    public override bool CanSeek => throw new NotImplementedException();
	    public override bool CanWrite => throw new NotImplementedException();
	    public override long Length => throw new NotImplementedException();
	    public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	    public override void Flush()
	    {
	        throw new NotImplementedException();
	    }
	    public override int Read(byte[] buffer, int offset, int count)
	    {
	        throw new NotImplementedException();
	    }
	    public override long Seek(long offset, SeekOrigin origin)
	    {
	        throw new NotImplementedException();
	    }
	    public override void SetLength(long value)
	    {
	        throw new NotImplementedException();
	    }
	    public override void Write(byte[] buffer, int offset, int count)
	    {
	        throw new NotImplementedException();
	    }
	}
	internal class Http1Session : IHttpSession
	{
	}
	#endregion
	#region \Server\Internal\Http1\Exceptions
	internal class Http1Exception : HttpException
	{
	    public Http1Exception(string message) : base(message)
	    {
	    }
	    public Http1Exception(string message, Exception inner) : base(message, inner)
	    {
	    }
	    public static Http1Exception InvalidRequest(Exception inner = null!)
	    {
	        return new Http1Exception("The HTTP request could not be parsed", inner)
	        {
	            Code = HttpExceptionCode.ReadingError
	        };
	    }
	}
	internal class Http1InvalidRequestMessageException : HttpException
	{
	    public Http1InvalidRequestMessageException(string message) : base(message)
	    {
	    }
	}
	#endregion
	#region \Server\Internal\Http1\MessageBody
	internal class Http1ChuckedEncodingMessageBody : Http1MessageBody
	{
	    private enum Mode
	    {
	        Prefix,
	        Extension,
	        Data,
	        Suffix,
	        Trailer,
	        TrailerHeaders,
	        Complete
	    };
	    public Http1ChuckedEncodingMessageBody(PipeReader reader) : base(reader)
	    {
	    }
	    private Mode mode;
	    private static int GetChunkSize(int extraHexDigit, int currentParsedSize)
	    {
	        try
	        {
	            checked
	            {
	                if (extraHexDigit >= '0' && extraHexDigit <= '9')
	                {
	                    return currentParsedSize * 0x10 + (extraHexDigit - '0');
	                }
	                else if (extraHexDigit >= 'A' && extraHexDigit <= 'F')
	                {
	                    return currentParsedSize * 0x10 + (extraHexDigit - ('A' - 10));
	                }
	                else if (extraHexDigit >= 'a' && extraHexDigit <= 'f')
	                {
	                    return currentParsedSize * 0x10 + (extraHexDigit - ('a' - 10));
	                }
	            }
	        }
	        catch (OverflowException ex)
	        {
	            //throw new IOException(CoreStrings.BadRequest_BadChunkSizeData, ex);
	        }
	        //KestrelBadHttpRequestException.Throw(RequestRejectionReason.BadChunkSizeData);
	        return -1; // can't happen, but compiler complains
	    }
	}
	internal class Test : PipeReader
	{
	    public override void AdvanceTo(SequencePosition consumed)
	    {
	        throw new NotImplementedException();
	    }
	    public override void AdvanceTo(SequencePosition consumed, SequencePosition examined)
	    {
	        throw new NotImplementedException();
	    }
	    public override void CancelPendingRead()
	    {
	        throw new NotImplementedException();
	    }
	    public override void Complete(Exception? exception = null)
	    {
	        throw new NotImplementedException();
	    }
	    public override ValueTask<ReadResult> ReadAsync(CancellationToken cancellationToken = default)
	    {
	        throw new NotImplementedException();
	    }
	    public override bool TryRead(out ReadResult result)
	    {
	        throw new NotImplementedException();
	    }
	}
	internal class Http1ContentLengthMessageBody : Http1MessageBody
	{
	    public Http1ContentLengthMessageBody(PipeReader reader) : base(reader)
	    {
	    }
	}
	internal abstract class Http1MessageBody
	{
	    protected readonly PipeReader reader;
	    public Http1MessageBody(PipeReader reader)
	    {
	        this.reader = reader;
	    }
	    public static Http1MessageBody Create(Http1Context context)
	    {
	        var header = context.Request.Headers;
	        if (header.TryGetValue("Transfer-Encoding", out var transferEncoding))
	        {
	            if (transferEncoding == "chunked")
	            {
	               // return new Http1RequestChunkEncodingMessageBody();
	            }
	        }
	        else if (header.TryGetValue("Content-Length", out var contentLength))
	        {
	            if (int.TryParse(contentLength, out var length))
	            {
	               // return new Http1ContentLengthMessageBody(length);
	            }
	        }
	        else if (header.TryGetValue("Upgrade", out var upgrade))
	        {
	            //return new Http1UpgradeMessageBody();
	        }
	        return default;
	    }
	}
	internal class Http1UpgradeMessageBody : Http1MessageBody
	{
	    public Http1UpgradeMessageBody(PipeReader reader) : base(reader)
	    {
	    }
	}
	#endregion
	#region \Server\Internal\Http1\Reader
	internal abstract partial class Http1RequestReader
	{
	    public abstract Http1RequestReader Next { get; }
	    public abstract Task ReadAsync(Http1Context context, ITransportConnection connection);
	    protected bool TryReadLine(ReadResult result, out ReadOnlySequence<byte> line)
	    {
	        line = default;
	        var cr = result.Buffer.PositionOf((byte)'\r');
	        var lf = result.Buffer.PositionOf((byte)'\n');
	        if (cr is not null && lf is not null)
	        {
	            line = result.Buffer.Slice(0, cr.Value);
	            return true;
	        }
	        return false;
	    }
	    protected bool TryReadLine(ReadResult result, out ReadOnlySequence<byte> line, out SequencePosition lineEnding)
	    {
	        line = default;
	        lineEnding = default;
	        var cr = result.Buffer.PositionOf((byte)'\r');
	        var lf = result.Buffer.PositionOf((byte)'\n');
	        if (cr is not null && lf is not null)
	        {
	            line = result.Buffer.Slice(0, cr.Value);
	            lineEnding = result.Buffer.GetPosition(1, lf.Value);
	            return true;
	        }
	        return false;
	    }
	    public static Http1RequestReader Create() => new Http1RequestHttp2PrefaceReader();
	}
	internal class Http1RequestHttp2PrefaceReader : Http1RequestReader
	{
	    public Http1RequestHttp2PrefaceReader()
	    {
	        Next = new Http1RequestLineMethodReader();
	    }
	    public override Http1RequestReader Next { get; }
	    // In unsecure connection it's possible that the 
	    public override Task ReadAsync(Http1Context context, ITransportConnection connection)
	    {
	        return Next.ReadAsync(context, connection);
	    }
	}
	internal class Http1RequestLineMethodReader : Http1RequestReader
	{
	    public Http1RequestLineMethodReader()
	    {
	        Next = new Http1RequestLineTargetReader();
	    }
	    public override Http1RequestReader Next { get; } 
	    public override async Task ReadAsync(Http1Context context, ITransportConnection connection)
	    {
	        try
	        {
	            var input = connection.Pipe.Input;
	            var result = await input.ReadAsync();
	            // If the result is completed then there is something wrong with the underlying connection.
	            if (result.IsCompleted)
	            {
	            }
	            if (TryReadLine(result, out var line))
	            {
	                var remaining = Parse(context, line);
	                input.AdvanceTo(remaining);
	            }
	            await Next.ReadAsync(context, connection);
	        }
	        catch (Exception exception) when (exception is not Http1Exception)
	        {
	            throw Http1Exception.InvalidRequest(exception);
	        }
	    }
	    private SequencePosition Parse(Http1Context context, ReadOnlySequence<byte> line)
	    {
	        var reader = new SequenceReader<byte>(line);
	        if (reader.TryReadTo(out ReadOnlySequence<byte> method, Space))
	        {
	            context.Request.Method = Encoding.ASCII.GetString(method);
	            return reader.Position;
	        }
	        else
	        {
	            // TODO: Throw Invalid Request Line (There should be a space in-between '{Method} HTTP/1.1')
	            throw new Exception();
	        }
	    }
	}
	internal class Http1RequestLineTargetReader : Http1RequestReader
	{
	    public Http1RequestLineTargetReader()
	    {
	        Next = new Http1RequestLineQueryReader();
	    }
	    public override Http1RequestReader Next { get; }
	    public override async Task ReadAsync(Http1Context context, ITransportConnection connection)
	    {
	        var input = connection.Pipe.Input;
	        var result = await input.ReadAsync();
	        if (TryReadLine(result, out var line))
	        {
	            var remining = Parse(context, line);
	            input.AdvanceTo(remining);
	        }
	        await Next.ReadAsync(context, connection);
	    }
	    private SequencePosition Parse(Http1Context context, ReadOnlySequence<byte> line)
	    {
	        var temp = Encoding.ASCII.GetString(line);
	        var reader = new SequenceReader<byte>(line);
	        if (reader.TryPeek(out var value))
	        {
	            // Let's check if the path is in origin-form
	            // Origin form requires that the client send the requested path with a '/' as the beginning
	            // https://httpwg.org/specs/rfc9112.html#rfc.section.3.2.1
	            if (value == (byte)'/')
	            {
	                return ParseOriginForm(context, ref reader);
	            }
	            // Check for asterisk-form
	            // https://httpwg.org/specs/rfc9112.html#rfc.section.3.2.4
	            if (value == (byte)'*')
	            {
	                return ParseAstriskForm(context, ref reader);
	            }
	            // If the request method is CONNECT then we can assume the request target is in authority form
	            // https://httpwg.org/specs/rfc9112.html#rfc.section.3.2.3
	            if (context.Request.Method == HttpMethod.Connect)
	            {
	                return ParseAuthorityForm(context, ref reader);
	            }
	        }
	        // If unable to peek something is wrong
	        throw new Exception();
	    }
	    private SequencePosition ParseOriginForm(Http1Context context, ref SequenceReader<byte> reader)
	    {
	        if (reader.TryReadTo(out ReadOnlySequence<byte> path1, QuestionMark, true))
	        {
	            context.Request.Path = Encoding.ASCII.GetString(path1.FirstSpan);
	            return reader.Position;
	        }
	        if (reader.TryReadTo(out ReadOnlySequence<byte> path2, Space, true))
	        {
	            context.Request.Path = Encoding.ASCII.GetString(path2.FirstSpan);
	            return reader.Position;
	        }
	        throw new Exception();
	    }
	    private SequencePosition ParseAstriskForm(Http1Context context, ref SequenceReader<byte> reader)
	    {
	        if (reader.TryReadTo(out ReadOnlySequence<byte> path, QuestionMark, true))
	        {
	        }
	        throw new NotImplementedException();
	    }
	    private SequencePosition ParseAuthorityForm(Http1Context context, ref SequenceReader<byte> reader)
	    {
	        if (reader.TryReadTo(out ReadOnlySequence<byte> path, QuestionMark, true))
	        {
	        }
	        throw new NotImplementedException();
	    }
	}
	internal class Http1RequestLineQueryReader : Http1RequestReader
	{
	    public Http1RequestLineQueryReader()
	    {
	        Next = new Http1RequestLineVersionReader();
	    }
	    public override Http1RequestReader Next { get; }
	    public override async Task ReadAsync(Http1Context context, ITransportConnection connection)
	    {
	        var input = connection.Pipe.Input;
	        var result = await input.ReadAsync();
	        if (TryReadLine(result, out var line))
	        {
	            var remining = Parse(context, line);
	            input.AdvanceTo(remining);
	        }
	        await Next.ReadAsync(context, connection);
	    }
	    private SequencePosition Parse(Http1Context context, ReadOnlySequence<byte> line)
	    {
	        var temp = Encoding.ASCII.GetString(line);
	        var queryCollection = context.Request.Query;
	        var reader = new SequenceReader<byte>(line);
	        // If no Spce is found then just assume there is not query string to parse
	        if (reader.TryReadTo(out ReadOnlySequence<byte> query, Space))
	        {
	            if (!query.IsSingleSegment)
	            {
	                throw new Exception();
	            }
	            var segment = query.FirstSpan;
	            var key = new List<byte>();
	            for (int i = 0; i < segment.Length; i++)
	            {
	                // Check for query separator
	                if (segment[i] == (byte)'=')
	                {
	                    i++;
	                    var value = new List<byte>();
	                    for (; i < segment.Length; i++ )
	                    {
	                        if (segment[i] == (byte)'&')
	                        {
	                            break;
	                        }
	                        value.Add(segment[i]);
	                    }
	                    queryCollection.Add(
	                        Encoding.ASCII.GetString(key.ToArray()),
	                        Encoding.ASCII.GetString(value.ToArray()));
	                    key.Clear();
	                    value.Clear();
	                }
	                else
	                {
	                    key.Add(segment[i]);
	                }
	            }
	        }
	        return reader.Position;
	    }
	}
	internal class Http1RequestLineVersionReader : Http1RequestReader
	{
	    public Http1RequestLineVersionReader()
	    {
	        Next = new Http1RequestHeadersReader();
	    }
	    public override Http1RequestReader Next { get; }
	    public override async Task ReadAsync(Http1Context context, ITransportConnection connection)
	    {
	        var input = connection.Pipe.Input;
	        var result = await input.ReadAsync();
	        if (TryReadLine(result, out var line, out var position))
	        {
	            if (line.IsSingleSegment)
	            {
	                if (line.FirstSpan.SequenceEqual(Version1))
	                {
	                }
	                else
	                {
	                    // Invalid Http Version
	                    throw new Exception();
	                }
	            }
	            else
	            {
	                // TODO: Throw exception. This should never happen, but just in-case there
	                // are multiple segments that means the http request line is all jacked
	            }
	            input.AdvanceTo(position);
	        }
	        await Next.ReadAsync(context, connection);
	    }
	}
	internal class Http1RequestHeadersReader : Http1RequestReader
	{
	    public Http1RequestHeadersReader()
	    {
	        Next = new Http1RequestBodyReader();
	    }
	    public override Http1RequestReader Next { get; }
	    public override async Task ReadAsync(Http1Context context, ITransportConnection connection)
	    {
	        var input = connection.Pipe.Input;
	        while (true)
	        {
	            var result = await input.ReadAsync();
	            if (TryReadLine(result, out var line, out var position))
	            {
	                if (line.IsEmpty)
	                {
	                    input.AdvanceTo(position);
	                    break;
	                }
	                Read(context, line);
	                input.AdvanceTo(position);
	            }
	        }
	        await Next.ReadAsync(context, connection);
	    }
	    private void Read(Http1Context context, ReadOnlySequence<byte> sequence)
	    {
	        var reader = new SequenceReader<byte>(sequence);
	        if (reader.TryReadTo(out ReadOnlySequence<byte> headerKey, (byte)':'))
	        {
	            if (reader.TryPeek(out var next) && next == Space[0])
	            {
	                reader.Advance(1);
	            }
	            var key = Encoding.ASCII.GetString(headerKey);
	            var value = Encoding.ASCII.GetString(reader.UnreadSpan);
	            context.Request.Headers.Add(key, value);
	        }
	    }
	}
	internal class Http1RequestBodyReader : Http1RequestReader
	{
	    public override Http1RequestReader Next { get; } = default!;
	    public override Task ReadAsync(Http1Context context, ITransportConnection connection)
	    {
	        var input = connection.Pipe.Input;
	        var request = context.Request;
	        var headers = request.Headers;
	        //if (headers.TransferEncoding.HasValue && headers.TransferEncoding.Value == "chunked")
	        //{
	        //}
	        //if (headers.ContentLength.HasValue)
	        //{
	        //    request.Body = new Http1RequestStream(new Http1ContentLengthMessageBody(input));
	        //}
	        //else
	        //{
	        //    //await next.Invoke(context);
	        //}
	        //var index = (long)0;
	        //var length = default(int);
	        //var result = await Input.ReadAsync();
	        //if (TryReadLine(result, out var line, out var position))
	        //{
	        //    var t = Encoding.GetString(line);
	        //    length = int.Parse(t);
	        //    Input.AdvanceTo(position); 
	        //}
	        //while (true)
	        //{
	        //    var value = await Reader.ReadAsync();
	        //    index += value.Buffer.Length;
	        //    if (index >= length)
	        //    {
	        //        var temp = Encoding.ASCII.GetString(result.Buffer);
	        //        break;
	        //    }
	        //    Reader.AdvanceTo(result.Buffer.End);
	        //}
	        return Task.CompletedTask;
	    }
	}
	#endregion
	#region \Server\Internal\Http1\Utilities
	internal static class Http1ThrowUtility
	{
	    public static void ThrowIf<TException>(Func<bool> method) where TException : Exception, new()
	    {
	        if (method.Invoke())
	        {
	        }
	    }
	}
	#endregion
	#region \Server\Internal\Http1\Writer
	internal abstract class Http1ResponseWriter
	{
	    public Http1ResponseWriter Next { get; init; }
	    protected bool TryWriteLine(in ReadOnlyMemory<byte> memory)
	    {
	        return true;
	    }
	    public abstract Task WriteAsync(Http1Context context, ITransportConnection connection);
	    public static Http1ResponseWriter Create()
	    {
	        return new Http1ResponseLineVersionWriter();
	    }
	}
	internal class Http1ResponseLineVersionWriter : Http1ResponseWriter
	{
	    public Http1ResponseLineVersionWriter()
	    {
	        Next = new Http1ResponseLineStatusCodeWriter();
	    }
	    public override async Task WriteAsync(Http1Context context, ITransportConnection connection)
	    {
	        var writer = connection.Pipe.Output;
	        var memory = writer.GetMemory();
	        for (int i = 0; i < Version1.Length; i++)
	        {
	            memory.Span[i] = Version1[i];
	            if (i + 1 == Version1.Length)
	            {
	                memory.Span[i + 1] = (byte)' ';
	            }
	        }
	        writer.Advance(Version1.Length + 1);
	        await Next.WriteAsync(context, connection);
	    }
	    private static byte[] GetNotFoundBytes()
	    {
	        var value = "404 Not Found";
	        return Encoding.ASCII.GetBytes(value);
	    }
	}
	internal class Http1ResponseLineStatusCodeWriter : Http1ResponseWriter
	{
	    public Http1ResponseLineStatusCodeWriter()
	    {
	        Next = new Http1ResponseHeadersWriter();
	    }
	    public override Task WriteAsync(Http1Context context, ITransportConnection connection)
	    {
	        var writer = connection.Pipe.Output;
	        var content = context.Response.StatusCode.Value switch
	        {
	            200 => Ok,
	            201 => Created,
	            202 => Accepted,
	            203 => NonAuthoritativeInformation,
	            204 => NoContent,
	            205 => ResetContent,
	            206 => PartialContent,
	            400 => BadRequest,
	            401 => Unauthorized,
	            404 => NotFound,
	            _ => throw new Exception()
	        };
	        var memory = writer.GetMemory(content.Length + NewLine.Length);
	        int i = 0;
	        // Write Status Code
	        for (; i < content.Length; i++)
	        {
	            memory.Span[i] = content[i];
	        }
	        // Write New Line
	        for (; i < (content.Length + NewLine.Length); i++)
	        {
	            memory.Span[i] = NewLine[i - content.Length];
	        }
	        writer.Advance(content.Length + NewLine.Length);
	        return Next.WriteAsync(context, connection);
	    }
	}
	internal class Http1ResponseHeadersWriter : Http1ResponseWriter
	{
	    public Http1ResponseHeadersWriter()
	    {
	        Next = new Http1ResponseBodyWriter();
	    }
	    public override async Task WriteAsync(Http1Context context, ITransportConnection connection)
	    {
	        var writer = connection.Pipe.Output;
	        var headers = context.Response.Headers;
	        WriteHeaders(writer, headers);
	        await writer.FlushAsync();
	        //await Next?.WriteAsync(context, connection);
	    }
	    private void WriteHeaders(PipeWriter writer, IHttpHeaderCollection headers)
	    {
	        var i = 0;
	        var memory = writer.GetMemory();
	        foreach (var header in headers)
	        {
	            var key = ASCII.GetBytes(header.Key);
	            var value = ASCII.GetBytes(header.Value);
	            // Write Header Key
	            for (int a = 0; a < key.Length; a++)
	            {
	                memory.Span[i] = key[a];
	                if (i == memory.Length)
	                {
	                    Reset(ref i);
	                }
	                i++;
	            }
	            // Check if ': ' new two bytes equals or exceeds memory length
	            if ((i + 2) >= memory.Length)
	            {
	                Reset(ref i);
	            }
	            // Write Header Separator
	            memory.Span[i] = (byte)':';
	            i++;
	            memory.Span[i] = (byte)' ';
	            i++;
	            // Write Header Value
	            for (int b = 0; b < value.Length; b++)
	            {
	                memory.Span[i] = value[b];
	                if (i == memory.Length)
	                {
	                    Reset(ref i);
	                }
	                i++;
	            }
	            // Check if ' ' new two bytes equals or exceeds memory length
	            if ((i + 3) >= memory.Length) // 3 is to 
	            {
	                Reset(ref i);
	            }
	            memory.Span[i] = NewLine[0];
	            i++;
	            memory.Span[i] = NewLine[1];
	            i++;
	        }
	        if ((i + 2) >= memory.Length)
	        {
	            Reset(ref i);
	        }
	        memory.Span[i] = NewLine[0];
	        i++;
	        memory.Span[i] = NewLine[1];
	        i++;
	        writer.Advance(i);
	        void Reset(ref int index)
	        {
	            writer.Advance(index);
	            index = 0;
	            memory = writer.GetMemory();
	        }
	    }
	}
	internal class Http1ResponseBodyWriter : Http1ResponseWriter
	{
	    public Http1ResponseBodyWriter()
	    {
	    }
	    public override Task WriteAsync(Http1Context context, ITransportConnection connection)
	    {
	        return Task.CompletedTask;
	    }
	}
	#endregion
	#region \Server\Internal\Http2
	internal class Http2Connection : HttpConnection
	{
	    // This uses C# compiler's ability to refer to static data directly. For more information see https://vcsjones.dev/2019/02/01/csharp-readonly-span-bytes-static
	    // TODO: once C# 11 comes out want to switch to Utf8 Strings Literals 
	    private static ReadOnlySpan<byte> ClientPreface => new byte[24] { (byte)'P', (byte)'R', (byte)'I', (byte)' ', (byte)'*', (byte)' ', (byte)'H', (byte)'T', (byte)'T', (byte)'P', (byte)'/', (byte)'2', (byte)'.', (byte)'0', (byte)'\r', (byte)'\n', (byte)'\r', (byte)'\n', (byte)'S', (byte)'M', (byte)'\r', (byte)'\n', (byte)'\r', (byte)'\n' };
	    private static ReadOnlySpan<byte> Authority => new byte[10] { (byte)':', (byte)'a', (byte)'u', (byte)'t', (byte)'h', (byte)'o', (byte)'r', (byte)'i', (byte)'t', (byte)'y' };
	    private static ReadOnlySpan<byte> Method => new byte[7] { (byte)':', (byte)'m', (byte)'e', (byte)'t', (byte)'h', (byte)'o', (byte)'d' };
	    private static ReadOnlySpan<byte> Path => new byte[5] { (byte)':', (byte)'p', (byte)'a', (byte)'t', (byte)'h' };
	    private static ReadOnlySpan<byte> Scheme => new byte[7] { (byte)':', (byte)'s', (byte)'c', (byte)'h', (byte)'e', (byte)'m', (byte)'e' };
	    private static ReadOnlySpan<byte> Status => new byte[7] { (byte)':', (byte)'s', (byte)'t', (byte)'a', (byte)'t', (byte)'u', (byte)'s' };
	    private static ReadOnlySpan<byte> Connection => new byte[10] { (byte)'c', (byte)'o', (byte)'n', (byte)'n', (byte)'e', (byte)'c', (byte)'t', (byte)'i', (byte)'o', (byte)'n' };
	    private static ReadOnlySpan<byte> TeBytes => new byte[2] { (byte)'t', (byte)'e' };
	    private static ReadOnlySpan<byte> Trailers => new byte[8] { (byte)'t', (byte)'r', (byte)'a', (byte)'i', (byte)'l', (byte)'e', (byte)'r', (byte)'s' };
	    private static ReadOnlySpan<byte> Connect => new byte[7] { (byte)'C', (byte)'O', (byte)'N', (byte)'N', (byte)'E', (byte)'C', (byte)'T' };
	    private readonly ConcurrentDictionary<int, Tuple<Http2ConnectionParsingStatus,  Http2Stream>> streams;
	    internal enum Http2ConnectionParsingStatus
	    {
	    }
	    public Http2Connection() 
	    {
	        this.streams = new();
	    }
	    //protected override IAsyncEnumerable<IHttpContext> ReceiveAsync(CancellationToken cancellationToken = default)
	    //{
	    //    throw new NotImplementedException();
	    //}
	    //protected override Task<IHttpContext> SendAsync(IHttpContext context, CancellationToken cancellationToken = default)
	    //{
	    //    throw new NotImplementedException();
	    //}
	    internal override IAsyncEnumerable<IHttpContext> ProcessAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
	    {
	        throw new NotImplementedException();
	    }
	}
	internal class Http2Stream
	{
	}
	#endregion
	#region \Server\Internal\Http2\Exceptions
	internal class Http2ConnectionException : HttpException
	{
	    public Http2ConnectionException(string message) : base(message)
	    {
	    }
	}
	#endregion
	#region \Server\Internal\Http2\Frames
	internal enum Http2ErrorCode : uint
	{
	    NoError = 0x0,
	    ProtocolError = 0x1,
	    InternalError = 0x2,
	    FlowControlError = 0x3,
	    SettingsTimeout = 0x4,
	    StreamClosed = 0x5,
	    FrameSizeError = 0x6,
	    RefusedStream = 0x7,
	    Cancel = 0x8,
	    CompressionError = 0x9,
	    ConnectError = 0xa,
	    EnhanceYourCalm = 0xb,
	    InadequateSecurity = 0xc,
	    Http11Required = 0xd,
	}
	[Flags]
	internal enum Http2DataFrameFlags : byte
	{
	    None = 0x0,
	    EndStream = 0x1,
	    Padded = 0x8
	}
	[Flags]
	internal enum Http2HeadersFrameFlags : byte
	{
	    None = 0x0,
	    EndStrem = 0x1,
	    EndHeaders = 0x4,
	    Padded = 0x8,
	    Priority = 0x20
	}
	[Flags]
	internal enum Http2PingFrameFlags : byte
	{
	    None = 0x0,
	    Acknowledge = 0x1
	}
	[Flags]
	internal enum Http2SettingsFrameFlags : byte
	{
	    None = 0x0,
	    Acknowledge = 0x1,
	}
	/* https://tools.ietf.org/html/rfc7540#section-4.1
	    +-----------------------------------------------+
	    |                 Length (24)                   |
	    +---------------+---------------+---------------+
	    |   Type (8)    |   Flags (8)   |
	    +-+-------------+---------------+-------------------------------+
	    |R|                 Stream Identifier (31)                      |
	    +=+=============================================================+
	    |                   Frame Payload (0...)                      ...
	    +---------------------------------------------------------------+
	*/
	internal partial class Http2Frame
	{
	    public int PayloadLength { get; set; }
	    public Http2FrameType Type { get; set; }
	    public byte Flags { get; set; }
	    public int StreamId { get; set; }
	    internal object ShowFlags()
	    {
	        switch (Type)
	        {
	            case Http2FrameType.Continuation:
	               // return ContinuationFlags;
	            case Http2FrameType.Data:
	                return DataFlags;
	            case Http2FrameType.Headers:
	                return HeadersFlags;
	            case Http2FrameType.Settings:
	                return SettingsFlags;
	            case Http2FrameType.Ping:
	                return PingFlags;
	            // Not Implemented
	            case Http2FrameType.PushPromise:
	            // No flags defined
	            case Http2FrameType.Priority:
	            case Http2FrameType.RstStream:
	            case Http2FrameType.GoAway:
	            case Http2FrameType.WindowUpdate:
	            default:
	                return $"0x{Flags:x}";
	        }
	    }
	    public override string ToString()
	    {
	        return $"{Type} Stream: {StreamId} Length: {PayloadLength} Flags: {ShowFlags()}";
	    }
	}
	/*
	    +---------------+
	    |Pad Length? (8)|
	    +---------------+-----------------------------------------------+
	    |                            Data (*)                         ...
	    +---------------------------------------------------------------+
	    |                           Padding (*)                       ...
	    +---------------------------------------------------------------+
	*/
	internal partial class Http2Frame
	{
	    public Http2DataFrameFlags DataFlags
	    {
	        get => (Http2DataFrameFlags)Flags;
	        set => Flags = (byte)value;
	    }
	    public bool DataEndStream => (DataFlags & Http2DataFrameFlags.EndStream) == Http2DataFrameFlags.EndStream;
	    public bool DataHasPadding => (DataFlags & Http2DataFrameFlags.Padded) == Http2DataFrameFlags.Padded;
	    public byte DataPadLength { get; set; }
	    private int DataPayloadOffset => DataHasPadding ? 1 : 0;
	    public int DataPayloadLength => PayloadLength - DataPayloadOffset - DataPadLength;
	    public void PrepareData(int streamId, byte? padLength = null)
	    {
	        PayloadLength = 0;
	        Type = Http2FrameType.Data;
	        DataFlags = padLength.HasValue ? Http2DataFrameFlags.Padded : Http2DataFrameFlags.None;
	        StreamId = streamId;
	        DataPadLength = padLength ?? 0;
	    }
	}
	/* https://tools.ietf.org/html/rfc7540#section-6.8
	    +-+-------------------------------------------------------------+
	    |R|                  Last-Stream-ID (31)                        |
	    +-+-------------------------------------------------------------+
	    |                      Error Code (32)                          |
	    +---------------------------------------------------------------+
	    |                  Additional Debug Data (*)                    |
	    +---------------------------------------------------------------+
	*/
	internal partial class Http2Frame
	{
	    public int GoAwayLastStreamId { get; set; }
	    public Http2ErrorCode GoAwayErrorCode { get; set; }
	    public void PrepareGoAway(int lastStreamId, Http2ErrorCode errorCode)
	    {
	        PayloadLength = 8;
	        Type = Http2FrameType.GoAway;
	        Flags = 0;
	        StreamId = 0;
	        GoAwayLastStreamId = lastStreamId;
	        GoAwayErrorCode = errorCode;
	    }
	}
	/* https://tools.ietf.org/html/rfc7540#section-6.2
	    +---------------+
	    |Pad Length? (8)|
	    +-+-------------+-----------------------------------------------+
	    |E|                 Stream Dependency? (31)                     |
	    +-+-------------+-----------------------------------------------+
	    |  Weight? (8)  |
	    +-+-------------+-----------------------------------------------+
	    |                   Header Block Fragment (*)                 ...
	    +---------------------------------------------------------------+
	    |                           Padding (*)                       ...
	    +---------------------------------------------------------------+
	*/
	internal partial class Http2Frame
	{
	    public Http2HeadersFrameFlags HeadersFlags
	    {
	        get => (Http2HeadersFrameFlags)Flags;
	        set => Flags = (byte)value;
	    }
	    public bool HeadersEndHeaders => (HeadersFlags & Http2HeadersFrameFlags.EndHeaders) == Http2HeadersFrameFlags.EndHeaders;
	    public bool HeadersEndStream => (HeadersFlags & Http2HeadersFrameFlags.EndStrem) == Http2HeadersFrameFlags.EndStrem;
	    public bool HeadersHasPadding => (HeadersFlags & Http2HeadersFrameFlags.Padded) == Http2HeadersFrameFlags.Padded;
	    public bool HeadersHasPriority => (HeadersFlags & Http2HeadersFrameFlags.Priority) == Http2HeadersFrameFlags.Priority;
	    public byte HeadersPadLength { get; set; }
	    public int HeadersStreamDependency { get; set; }
	    public byte HeadersPriorityWeight { get; set; }
	    private int HeadersPayloadOffset => (HeadersHasPadding ? 1 : 0) + (HeadersHasPriority ? 5 : 0);
	    public int HeadersPayloadLength => PayloadLength - HeadersPayloadOffset - HeadersPadLength;
	    public void PrepareHeaders(Http2HeadersFrameFlags flags, int streamId)
	    {
	        PayloadLength = 0;
	        Type = Http2FrameType.Headers;
	        HeadersFlags = flags;
	        StreamId = streamId;
	    }
	}
	/* https://tools.ietf.org/html/rfc7540#section-6.7
	    +---------------------------------------------------------------+
	    |                                                               |
	    |                      Opaque Data (64)                         |
	    |                                                               |
	    +---------------------------------------------------------------+
	*/
	internal partial class Http2Frame
	{
	    public Http2PingFrameFlags PingFlags
	    {
	        get => (Http2PingFrameFlags)Flags;
	        set => Flags = (byte)value;
	    }
	    public bool PingAck => (PingFlags & Http2PingFrameFlags.Acknowledge) == Http2PingFrameFlags.Acknowledge;
	    public void PreparePing(Http2PingFrameFlags flags)
	    {
	        PayloadLength = 8;
	        Type = Http2FrameType.Ping;
	        PingFlags = flags;
	        StreamId = 0;
	    }
	}
	/* https://tools.ietf.org/html/rfc7540#section-6.3
	    +-+-------------------------------------------------------------+
	    |E|                  Stream Dependency (31)                     |
	    +-+-------------+-----------------------------------------------+
	    |   Weight (8)  |
	    +-+-------------+
	*/
	internal partial class Http2Frame
	{
	    public int PriorityStreamDependency { get; set; }
	    public bool PriorityIsExclusive { get; set; }
	    public byte PriorityWeight { get; set; }
	    public void PreparePriority(int streamId, int streamDependency, bool exclusive, byte weight)
	    {
	        PayloadLength = 5;
	        Type = Http2FrameType.Priority;
	        StreamId = streamId;
	        PriorityStreamDependency = streamDependency;
	        PriorityIsExclusive = exclusive;
	        PriorityWeight = weight;
	    }
	}
	/* https://tools.ietf.org/html/rfc7540#section-6.4
	    +---------------------------------------------------------------+
	    |                        Error Code (32)                        |
	    +---------------------------------------------------------------+
	*/
	internal partial class Http2Frame
	{
	    public Http2ErrorCode RstStreamErrorCode { get; set; }
	    public void PrepareRstStream(int streamId, Http2ErrorCode errorCode)
	    {
	        PayloadLength = 4;
	        Type = Http2FrameType.RstStream;
	        Flags = 0;
	        StreamId = streamId;
	        RstStreamErrorCode = errorCode;
	    }
	}
	/* https://tools.ietf.org/html/rfc7540#section-6.5.1
	    List of:
	    +-------------------------------+
	    |       Identifier (16)         |
	    +-------------------------------+-------------------------------+
	    |                        Value (32)                             |
	    +---------------------------------------------------------------+
	*/
	internal partial class Http2Frame
	{
	    public Http2SettingsFrameFlags SettingsFlags
	    {
	        get => (Http2SettingsFrameFlags)Flags;
	        set => Flags = (byte)value;
	    }
	    public bool SettingsAck => (SettingsFlags & Http2SettingsFrameFlags.Acknowledge) == Http2SettingsFrameFlags.Acknowledge;
	    public void PrepareSettings(Http2SettingsFrameFlags flags)
	    {
	        PayloadLength = 0;
	        Type = Http2FrameType.Settings;
	        SettingsFlags = flags;
	        StreamId = 0;
	    }
	}
	/* https://tools.ietf.org/html/rfc7540#section-6.9
	    +-+-------------------------------------------------------------+
	    |R|              Window Size Increment (31)                     |
	    +-+-------------------------------------------------------------+
	*/
	internal partial class Http2Frame
	{
	    public int WindowUpdateSizeIncrement { get; set; }
	    public void PrepareWindowUpdate(int streamId, int sizeIncrement)
	    {
	        PayloadLength = 4;
	        Type = Http2FrameType.WindowUpdate;
	        Flags = 0;
	        StreamId = streamId;
	        WindowUpdateSizeIncrement = sizeIncrement;
	    }
	}
	internal static class Http2FrameReader
	{
	    /* https://tools.ietf.org/html/rfc7540#section-4.1
	        +-----------------------------------------------+
	        |                 Length (24)                   |
	        +---------------+---------------+---------------+
	        |   Type (8)    |   Flags (8)   |
	        +-+-------------+---------------+-------------------------------+
	        |R|                 Stream Identifier (31)                      |
	        +=+=============================================================+
	        |                   Frame Payload (0...)                      ...
	        +---------------------------------------------------------------+
	    */
	    public const int HeaderLength = 9;
	    private const int TypeOffset = 3;
	    private const int FlagsOffset = 4;
	    private const int StreamIdOffset = 5;
	    public const int SettingSize = 6; // 2 bytes for the id, 4 bytes for the value.
	    public static bool TryReadFrame(ref ReadOnlySequence<byte> buffer, Http2Frame frame, uint maxFrameSize, out ReadOnlySequence<byte> framePayload)
	    {
	        framePayload = ReadOnlySequence<byte>.Empty;
	        if (buffer.Length < HeaderLength)
	        {
	            return false;
	        }
	        var headerSlice = buffer.Slice(0, HeaderLength);
	        var header = headerSlice.ToSpan();
	        var payloadLength = (int)Bitshifter.ReadUInt24BigEndian(header);
	        if (payloadLength > maxFrameSize)
	        {
	            throw new Exception();
	            //throw new Http2ConnectionException(SharedStrings.FormatHttp2ErrorFrameOverLimit(payloadLength, maxFrameSize), Http2ErrorCode.FrameSizeError);
	        }
	        // Make sure the whole frame is buffered
	        var frameLength = HeaderLength + payloadLength;
	        if (buffer.Length < frameLength)
	        {
	            return false;
	        }
	        frame.PayloadLength = payloadLength;
	        frame.Type = (Http2FrameType)header[TypeOffset];
	        frame.Flags = header[FlagsOffset];
	        frame.StreamId = (int)Bitshifter.ReadUInt31BigEndian(header.Slice(StreamIdOffset));
	        var extendedHeaderLength = ReadExtendedFields(frame, buffer);
	        // The remaining payload minus the extra fields
	        framePayload = buffer.Slice(HeaderLength + extendedHeaderLength, payloadLength - extendedHeaderLength);
	        buffer = buffer.Slice(framePayload.End);
	        return true;
	    }
	    private static int ReadExtendedFields(Http2Frame frame, in ReadOnlySequence<byte> readableBuffer)
	    {
	        // Copy in any extra fields for the given frame type
	        var extendedHeaderLength = GetPayloadFieldsLength(frame);
	        if (extendedHeaderLength > frame.PayloadLength)
	        {
	            throw new Exception();
	            //throw new Http2ConnectionException(
	            //    SharedStrings.FormatHttp2ErrorUnexpectedFrameLength(frame.Type, expectedLength: extendedHeaderLength), Http2ErrorCode.FrameSizeError);
	        }
	        var extendedHeaders = readableBuffer.Slice(HeaderLength, extendedHeaderLength).ToSpan();
	        // Parse frame type specific fields
	        switch (frame.Type)
	        {
	            /*
	                +---------------+
	                |Pad Length? (8)|
	                +---------------+-----------------------------------------------+
	                |                            Data (*)                         ...
	                +---------------------------------------------------------------+
	                |                           Padding (*)                       ...
	                +---------------------------------------------------------------+
	            */
	            case Http2FrameType.Data: // Variable 0 or 1
	                frame.DataPadLength = frame.DataHasPadding ? extendedHeaders[0] : (byte)0;
	                break;
	            /* https://tools.ietf.org/html/rfc7540#section-6.2
	                +---------------+
	                |Pad Length? (8)|
	                +-+-------------+-----------------------------------------------+
	                |E|                 Stream Dependency? (31)                     |
	                +-+-------------+-----------------------------------------------+
	                |  Weight? (8)  |
	                +-+-------------+-----------------------------------------------+
	                |                   Header Block Fragment (*)                 ...
	                +---------------------------------------------------------------+
	                |                           Padding (*)                       ...
	                +---------------------------------------------------------------+
	            */
	            case Http2FrameType.Headers:
	                if (frame.HeadersHasPadding)
	                {
	                    frame.HeadersPadLength = extendedHeaders[0];
	                    extendedHeaders = extendedHeaders.Slice(1);
	                }
	                else
	                {
	                    frame.HeadersPadLength = 0;
	                }
	                if (frame.HeadersHasPriority)
	                {
	                    frame.HeadersStreamDependency = (int)Bitshifter.ReadUInt31BigEndian(extendedHeaders);
	                    frame.HeadersPriorityWeight = extendedHeaders.Slice(4)[0];
	                }
	                else
	                {
	                    frame.HeadersStreamDependency = 0;
	                    frame.HeadersPriorityWeight = 0;
	                }
	                break;
	            /* https://tools.ietf.org/html/rfc7540#section-6.8
	                +-+-------------------------------------------------------------+
	                |R|                  Last-Stream-ID (31)                        |
	                +-+-------------------------------------------------------------+
	                |                      Error Code (32)                          |
	                +---------------------------------------------------------------+
	                |                  Additional Debug Data (*)                    |
	                +---------------------------------------------------------------+
	            */
	            case Http2FrameType.GoAway:
	                frame.GoAwayLastStreamId = (int)Bitshifter.ReadUInt31BigEndian(extendedHeaders);
	                frame.GoAwayErrorCode = (Http2ErrorCode)BinaryPrimitives.ReadUInt32BigEndian(extendedHeaders.Slice(4));
	                break;
	            /* https://tools.ietf.org/html/rfc7540#section-6.3
	                +-+-------------------------------------------------------------+
	                |E|                  Stream Dependency (31)                     |
	                +-+-------------+-----------------------------------------------+
	                |   Weight (8)  |
	                +-+-------------+
	            */
	            case Http2FrameType.Priority:
	                frame.PriorityStreamDependency = (int)Bitshifter.ReadUInt31BigEndian(extendedHeaders);
	                frame.PriorityWeight = extendedHeaders.Slice(4)[0];
	                break;
	            /* https://tools.ietf.org/html/rfc7540#section-6.4
	                +---------------------------------------------------------------+
	                |                        Error Code (32)                        |
	                +---------------------------------------------------------------+
	            */
	            case Http2FrameType.RstStream:
	                frame.RstStreamErrorCode = (Http2ErrorCode)BinaryPrimitives.ReadUInt32BigEndian(extendedHeaders);
	                break;
	            /* https://tools.ietf.org/html/rfc7540#section-6.9
	                +-+-------------------------------------------------------------+
	                |R|              Window Size Increment (31)                     |
	                +-+-------------------------------------------------------------+
	            */
	            case Http2FrameType.WindowUpdate:
	                frame.WindowUpdateSizeIncrement = (int)Bitshifter.ReadUInt31BigEndian(extendedHeaders);
	                break;
	            case Http2FrameType.Ping: // Opaque payload 8 bytes long
	            case Http2FrameType.Settings: // Settings are general payload
	            case Http2FrameType.Continuation: // None
	            case Http2FrameType.PushPromise: // Not implemented frames are ignored at this phase
	            default:
	                return 0;
	        }
	        return extendedHeaderLength;
	    }
	    // The length in bytes of additional fields stored in the payload section.
	    // This may be variable based on flags, but should be no more than 8 bytes.
	    public static int GetPayloadFieldsLength(Http2Frame frame)
	    {
	        switch (frame.Type)
	        {
	            // TODO: Extract constants
	            case Http2FrameType.Data: // Variable 0 or 1
	                return frame.DataHasPadding ? 1 : 0;
	            case Http2FrameType.Headers:
	                return (frame.HeadersHasPadding ? 1 : 0) + (frame.HeadersHasPriority ? 5 : 0); // Variable 0 to 6
	            case Http2FrameType.GoAway:
	                return 8; // Last stream id and error code.
	            case Http2FrameType.Priority:
	                return 5; // Stream dependency and weight
	            case Http2FrameType.RstStream:
	                return 4; // Error code
	            case Http2FrameType.WindowUpdate:
	                return 4; // Update size
	            case Http2FrameType.Ping: // 8 bytes of opaque data
	            case Http2FrameType.Settings: // Settings are general payload
	            case Http2FrameType.Continuation: // None
	            case Http2FrameType.PushPromise: // Not implemented frames are ignored at this phase
	            default:
	                return 0;
	        }
	    }
	    public static IList<Http2PeerSetting> ReadSettings(in ReadOnlySequence<byte> payload)
	    {
	        var data = payload.ToSpan();
	        Debug.Assert(data.Length % SettingSize == 0, "Invalid settings payload length");
	        var settingsCount = data.Length / SettingSize;
	        var settings = new Http2PeerSetting[settingsCount];
	        for (int i = 0; i < settings.Length; i++)
	        {
	            settings[i] = ReadSetting(data);
	            data = data.Slice(SettingSize);
	        }
	        return settings;
	    }
	    private static Http2PeerSetting ReadSetting(ReadOnlySpan<byte> payload)
	    {
	        var id = (Http2SettingsParameter)BinaryPrimitives.ReadUInt16BigEndian(payload);
	        var value = BinaryPrimitives.ReadUInt32BigEndian(payload.Slice(2));
	        return new Http2PeerSetting(id, value);
	    }
	}
	internal enum Http2FrameType : byte
	{
	    Data = 0x0,
	    Headers = 0x1,
	    Priority = 0x2,
	    RstStream = 0x3,
	    Settings = 0x4,
	    PushPromise = 0x5,
	    Ping = 0x6,
	    GoAway = 0x7,
	    WindowUpdate = 0x8,
	    Continuation = 0x9
	}
	internal class Http2FrameWriter
	{
	}
	internal readonly struct Http2PeerSetting
	{
	    public Http2PeerSetting(Http2SettingsParameter parameter, uint value)
	    {
	        Parameter = parameter;
	        Value = value;
	    }
	    public Http2SettingsParameter Parameter { get; }
	    public uint Value { get; }
	}
	// https://www.iana.org/assignments/http2-parameters/http2-parameters.xhtml#settings
	internal enum Http2SettingsParameter : ushort
	{
	    SETTINGS_HEADER_TABLE_SIZE = 0x1,
	    SETTINGS_ENABLE_PUSH = 0x2,
	    SETTINGS_MAX_CONCURRENT_STREAMS = 0x3,
	    SETTINGS_INITIAL_WINDOW_SIZE = 0x4,
	    SETTINGS_MAX_FRAME_SIZE = 0x5,
	    SETTINGS_MAX_HEADER_LIST_SIZE = 0x6,
	    SETTINGS_ENABLE_CONNECT_PROTOCOL = 0x8,
	}
	#endregion
	#region \Server\Internal\Http2\HPack
	// TODO: Should this be public?
	[Serializable]
	internal sealed class HPackDecodingException : Exception
	{
	    public HPackDecodingException()
	    {
	    }
	    public HPackDecodingException(string message) : base(message)
	    {
	    }
	    public HPackDecodingException(string message, Exception innerException) : base(message, innerException)
	    {
	    }
	    public HPackDecodingException(SerializationInfo info, StreamingContext context) : base(info, context)
	    {
	    }
	}
	internal sealed class HPackDynamicTable
	{
	    private HPackHeaderField[] buffer;
	    private int maxSize;
	    private int size;
	    private int count;
	    private int insertIndex;
	    private int removeIndex;
	    public HPackDynamicTable(int maxSize)
	    {
	        this.buffer = new HPackHeaderField[maxSize / HPackHeaderField.RfcOverhead];
	        this.maxSize = maxSize;
	    }
	    public int Count => count;
	    public int Size => size;
	    public int MaxSize => maxSize;
	    public ref readonly HPackHeaderField this[int index]
	    {
	        get
	        {
	            if (index >= count)
	            {
	                throw new IndexOutOfRangeException();
	            }
	            index = insertIndex - index - 1;
	            if (index < 0)
	            {
	                // _buffer is circular; wrap the index back around.
	                index += buffer.Length;
	            }
	            return ref buffer[index];
	        }
	    }
	    public void Insert(ReadOnlySpan<byte> name, ReadOnlySpan<byte> value)
	    {
	        Insert(staticTableIndex: null, name, value);
	    }
	    public void Insert(int? staticTableIndex, ReadOnlySpan<byte> name, ReadOnlySpan<byte> value)
	    {
	        int entryLength = HPackHeaderField.GetLength(name.Length, value.Length);
	        EnsureAvailable(entryLength);
	        if (entryLength > maxSize)
	        {
	            // http://httpwg.org/specs/rfc7541.html#rfc.section.4.4
	            // It is not an error to attempt to add an entry that is larger than the maximum size;
	            // an attempt to add an entry larger than the maximum size causes the table to be emptied
	            // of all existing entries and results in an empty table.
	            return;
	        }
	        var entry = new HPackHeaderField(staticTableIndex, name, value);
	        buffer[insertIndex] = entry;
	        insertIndex = (insertIndex + 1) % buffer.Length;
	        size += entry.Length;
	        count++;
	    }
	    public void Resize(int maxSize)
	    {
	        if (maxSize > this.maxSize)
	        {
	            var newBuffer = new HPackHeaderField[maxSize / HPackHeaderField.RfcOverhead];
	            int headCount = Math.Min(buffer.Length - removeIndex, count);
	            int tailCount = count - headCount;
	            Array.Copy(buffer, removeIndex, newBuffer, 0, headCount);
	            Array.Copy(buffer, 0, newBuffer, headCount, tailCount);
	            buffer = newBuffer;
	            removeIndex = 0;
	            insertIndex = count;
	            this.maxSize = maxSize;
	        }
	        else
	        {
	            this.maxSize = maxSize;
	            EnsureAvailable(0);
	        }
	    }
	    private void EnsureAvailable(int available)
	    {
	        while (count > 0 && maxSize - size < available)
	        {
	            ref HPackHeaderField field = ref buffer[removeIndex];
	            size -= field.Length;
	            field = default;
	            count--;
	            removeIndex = (removeIndex + 1) % buffer.Length;
	        }
	    }
	}
	    internal static partial class HPackEncoder
	    {
	        // Things we should add:
	        // * Huffman encoding
	        //
	        // Things we should consider adding:
	        // * Dynamic table encoding:
	        //   This would make the encoder stateful, which complicates things significantly.
	        //   Additionally, it's not clear exactly what strings we would add to the dynamic table
	        //   without some additional guidance from the user about this.
	        //   So for now, don't do dynamic encoding.
	        public static bool EncodeIndexedHeaderField(int index, Span<byte> destination, out int bytesWritten)
	        {
	            // From https://tools.ietf.org/html/rfc7541#section-6.1
	            // ----------------------------------------------------
	            //   0   1   2   3   4   5   6   7
	            // +---+---+---+---+---+---+---+---+
	            // | 1 |        Index (7+)         |
	            // +---+---------------------------+
	            if (destination.Length != 0)
	            {
	                destination[0] = 0x80;
	                return IntegerEncoder.Encode(index, 7, destination, out bytesWritten);
	            }
	            bytesWritten = 0;
	            return false;
	        }
	        public static bool EncodeStatusHeader(int statusCode, Span<byte> destination, out int bytesWritten)
	        {
	            // Bytes written depend on whether the status code value maps directly to an index
	            if (HPackStaticTable.TryGetStatusIndex(statusCode, out var index))
	            {
	                // Status codes which exist in the HTTP/2 StaticTable.
	                return EncodeIndexedHeaderField(index, destination, out bytesWritten);
	            }
	            else
	            {
	                // If the status code doesn't have a static index then we need to include the full value.
	                // Write a status index and then the number bytes as a string literal.
	                if (!EncodeLiteralHeaderFieldWithoutIndexing(HPackStaticTable.Status200, destination, out var nameLength))
	                {
	                    bytesWritten = 0;
	                    return false;
	                }
	                var statusBytes = default(Span<byte>);// statusCode.ToStatusBytes();
	                if (!EncodeStringLiteral(statusBytes, destination.Slice(nameLength), out var valueLength))
	                {
	                    bytesWritten = 0;
	                    return false;
	                }
	                bytesWritten = nameLength + valueLength;
	                return true;
	            }
	        }
	        public static bool EncodeLiteralHeaderFieldWithoutIndexing(int index, string value, Encoding? valueEncoding, Span<byte> destination, out int bytesWritten)
	        {
	            // From https://tools.ietf.org/html/rfc7541#section-6.2.2
	            // ------------------------------------------------------
	            //   0   1   2   3   4   5   6   7
	            // +---+---+---+---+---+---+---+---+
	            // | 0 | 0 | 0 | 0 |  Index (4+)   |
	            // +---+---+-----------------------+
	            // | H |     Value Length (7+)     |
	            // +---+---------------------------+
	            // | Value String (Length octets)  |
	            // +-------------------------------+
	            if ((uint)destination.Length >= 2)
	            {
	                destination[0] = 0;
	                if (IntegerEncoder.Encode(index, 4, destination, out int indexLength))
	                {
	                    Debug.Assert(indexLength >= 1);
	                    if (EncodeStringLiteral(value, valueEncoding, destination.Slice(indexLength), out int nameLength))
	                    {
	                        bytesWritten = indexLength + nameLength;
	                        return true;
	                    }
	                }
	            }
	            bytesWritten = 0;
	            return false;
	        }
	        public static bool EncodeLiteralHeaderFieldNeverIndexing(int index, string value, Encoding? valueEncoding, Span<byte> destination, out int bytesWritten)
	        {
	            // From https://tools.ietf.org/html/rfc7541#section-6.2.3
	            // ------------------------------------------------------
	            //   0   1   2   3   4   5   6   7
	            // +---+---+---+---+---+---+---+---+
	            // | 0 | 0 | 0 | 1 |  Index (4+)   |
	            // +---+---+-----------------------+
	            // | H |     Value Length (7+)     |
	            // +---+---------------------------+
	            // | Value String (Length octets)  |
	            // +-------------------------------+
	            if ((uint)destination.Length >= 2)
	            {
	                destination[0] = 0x10;
	                if (IntegerEncoder.Encode(index, 4, destination, out int indexLength))
	                {
	                    Debug.Assert(indexLength >= 1);
	                    if (EncodeStringLiteral(value, valueEncoding, destination.Slice(indexLength), out int nameLength))
	                    {
	                        bytesWritten = indexLength + nameLength;
	                        return true;
	                    }
	                }
	            }
	            bytesWritten = 0;
	            return false;
	        }
	        public static bool EncodeLiteralHeaderFieldIndexing(int index, string value, Encoding? valueEncoding, Span<byte> destination, out int bytesWritten)
	        {
	            // From https://tools.ietf.org/html/rfc7541#section-6.2.2
	            // ------------------------------------------------------
	            //   0   1   2   3   4   5   6   7
	            // +---+---+---+---+---+---+---+---+
	            // | 0 | 1 |      Index (6+)       |
	            // +---+---+-----------------------+
	            // | H |     Value Length (7+)     |
	            // +---+---------------------------+
	            // | Value String (Length octets)  |
	            // +-------------------------------+
	            if ((uint)destination.Length >= 2)
	            {
	                destination[0] = 0x40;
	                if (IntegerEncoder.Encode(index, 6, destination, out int indexLength))
	                {
	                    Debug.Assert(indexLength >= 1);
	                    if (EncodeStringLiteral(value, valueEncoding, destination.Slice(indexLength), out int nameLength))
	                    {
	                        bytesWritten = indexLength + nameLength;
	                        return true;
	                    }
	                }
	            }
	            bytesWritten = 0;
	            return false;
	        }
	        public static bool EncodeLiteralHeaderFieldWithoutIndexing(int index, Span<byte> destination, out int bytesWritten)
	        {
	            // From https://tools.ietf.org/html/rfc7541#section-6.2.2
	            // ------------------------------------------------------
	            //   0   1   2   3   4   5   6   7
	            // +---+---+---+---+---+---+---+---+
	            // | 0 | 0 | 0 | 0 |  Index (4+)   |
	            // +---+---+-----------------------+
	            //
	            // ... expected after this:
	            //
	            // | H |     Value Length (7+)     |
	            // +---+---------------------------+
	            // | Value String (Length octets)  |
	            // +-------------------------------+
	            if ((uint)destination.Length != 0)
	            {
	                destination[0] = 0;
	                if (IntegerEncoder.Encode(index, 4, destination, out int indexLength))
	                {
	                    Debug.Assert(indexLength >= 1);
	                    bytesWritten = indexLength;
	                    return true;
	                }
	            }
	            bytesWritten = 0;
	            return false;
	        }
	        public static bool EncodeLiteralHeaderFieldIndexingNewName(string name, string value, Encoding? valueEncoding, Span<byte> destination, out int bytesWritten)
	        {
	            // From https://tools.ietf.org/html/rfc7541#section-6.2.2
	            // ------------------------------------------------------
	            //   0   1   2   3   4   5   6   7
	            // +---+---+---+---+---+---+---+---+
	            // | 0 | 1 |           0           |
	            // +---+---+-----------------------+
	            // | H |     Name Length (7+)      |
	            // +---+---------------------------+
	            // |  Name String (Length octets)  |
	            // +---+---------------------------+
	            // | H |     Value Length (7+)     |
	            // +---+---------------------------+
	            // | Value String (Length octets)  |
	            // +-------------------------------+
	            return EncodeLiteralHeaderNewNameCore(0x40, name, value, valueEncoding, destination, out bytesWritten);
	        }
	        public static bool EncodeLiteralHeaderFieldWithoutIndexingNewName(string name, string value, Encoding? valueEncoding, Span<byte> destination, out int bytesWritten)
	        {
	            // From https://tools.ietf.org/html/rfc7541#section-6.2.2
	            // ------------------------------------------------------
	            //   0   1   2   3   4   5   6   7
	            // +---+---+---+---+---+---+---+---+
	            // | 0 | 0 | 0 | 0 |       0       |
	            // +---+---+-----------------------+
	            // | H |     Name Length (7+)      |
	            // +---+---------------------------+
	            // |  Name String (Length octets)  |
	            // +---+---------------------------+
	            // | H |     Value Length (7+)     |
	            // +---+---------------------------+
	            // | Value String (Length octets)  |
	            // +-------------------------------+
	            return EncodeLiteralHeaderNewNameCore(0, name, value, valueEncoding, destination, out bytesWritten);
	        }
	        public static bool EncodeLiteralHeaderFieldNeverIndexingNewName(string name, string value, Encoding? valueEncoding, Span<byte> destination, out int bytesWritten)
	        {
	            // From https://tools.ietf.org/html/rfc7541#section-6.2.3
	            // ------------------------------------------------------
	            //   0   1   2   3   4   5   6   7
	            // +---+---+---+---+---+---+---+---+
	            // | 0 | 0 | 0 | 1 |       0       |
	            // +---+---+-----------------------+
	            // | H |     Name Length (7+)      |
	            // +---+---------------------------+
	            // |  Name String (Length octets)  |
	            // +---+---------------------------+
	            // | H |     Value Length (7+)     |
	            // +---+---------------------------+
	            // | Value String (Length octets)  |
	            // +-------------------------------+
	            return EncodeLiteralHeaderNewNameCore(0x10, name, value, valueEncoding, destination, out bytesWritten);
	        }
	        private static bool EncodeLiteralHeaderNewNameCore(byte mask, string name, string value, Encoding? valueEncoding, Span<byte> destination, out int bytesWritten)
	        {
	            if ((uint)destination.Length >= 3)
	            {
	                destination[0] = mask;
	                if (EncodeLiteralHeaderName(name, destination.Slice(1), out int nameLength) &&
	                    EncodeStringLiteral(value, valueEncoding, destination.Slice(1 + nameLength), out int valueLength))
	                {
	                    bytesWritten = 1 + nameLength + valueLength;
	                    return true;
	                }
	            }
	            bytesWritten = 0;
	            return false;
	        }
	        public static bool EncodeLiteralHeaderFieldWithoutIndexingNewName(string name, ReadOnlySpan<string> values, string separator, Span<byte> destination, out int bytesWritten)
	        {
	            return EncodeLiteralHeaderFieldWithoutIndexingNewName(name, values, separator, valueEncoding: null, destination, out bytesWritten);
	        }
	        public static bool EncodeLiteralHeaderFieldWithoutIndexingNewName(string name, ReadOnlySpan<string> values, string separator, Encoding? valueEncoding, Span<byte> destination, out int bytesWritten)
	        {
	            // From https://tools.ietf.org/html/rfc7541#section-6.2.2
	            // ------------------------------------------------------
	            //   0   1   2   3   4   5   6   7
	            // +---+---+---+---+---+---+---+---+
	            // | 0 | 0 | 0 | 0 |       0       |
	            // +---+---+-----------------------+
	            // | H |     Name Length (7+)      |
	            // +---+---------------------------+
	            // |  Name String (Length octets)  |
	            // +---+---------------------------+
	            // | H |     Value Length (7+)     |
	            // +---+---------------------------+
	            // | Value String (Length octets)  |
	            // +-------------------------------+
	            if ((uint)destination.Length >= 3)
	            {
	                destination[0] = 0;
	                if (EncodeLiteralHeaderName(name, destination.Slice(1), out int nameLength) &&
	                    EncodeStringLiterals(values, separator, valueEncoding, destination.Slice(1 + nameLength), out int valueLength))
	                {
	                    bytesWritten = 1 + nameLength + valueLength;
	                    return true;
	                }
	            }
	            bytesWritten = 0;
	            return false;
	        }
	        public static bool EncodeLiteralHeaderFieldWithoutIndexingNewName(string name, Span<byte> destination, out int bytesWritten)
	        {
	            // From https://tools.ietf.org/html/rfc7541#section-6.2.2
	            // ------------------------------------------------------
	            //   0   1   2   3   4   5   6   7
	            // +---+---+---+---+---+---+---+---+
	            // | 0 | 0 | 0 | 0 |       0       |
	            // +---+---+-----------------------+
	            // | H |     Name Length (7+)      |
	            // +---+---------------------------+
	            // |  Name String (Length octets)  |
	            // +---+---------------------------+
	            //
	            // ... expected after this:
	            //
	            // | H |     Value Length (7+)     |
	            // +---+---------------------------+
	            // | Value String (Length octets)  |
	            // +-------------------------------+
	            if ((uint)destination.Length >= 2)
	            {
	                destination[0] = 0;
	                if (EncodeLiteralHeaderName(name, destination.Slice(1), out int nameLength))
	                {
	                    bytesWritten = 1 + nameLength;
	                    return true;
	                }
	            }
	            bytesWritten = 0;
	            return false;
	        }
	        private static bool EncodeLiteralHeaderName(string value, Span<byte> destination, out int bytesWritten)
	        {
	            // From https://tools.ietf.org/html/rfc7541#section-5.2
	            // ------------------------------------------------------
	            //   0   1   2   3   4   5   6   7
	            // +---+---+---+---+---+---+---+---+
	            // | H |    String Length (7+)     |
	            // +---+---------------------------+
	            // |  String Data (Length octets)  |
	            // +-------------------------------+
	            if (destination.Length != 0)
	            {
	                destination[0] = 0; // TODO: Use Huffman encoding
	                if (IntegerEncoder.Encode(value.Length, 7, destination, out int integerLength))
	                {
	                    Debug.Assert(integerLength >= 1);
	                    destination = destination.Slice(integerLength);
	                    if (value.Length <= destination.Length)
	                    {
	                        for (int i = 0; i < value.Length; i++)
	                        {
	                            char c = value[i];
	                            destination[i] = (byte)((uint)(c - 'A') <= ('Z' - 'A') ? c | 0x20 : c);
	                        }
	                        bytesWritten = integerLength + value.Length;
	                        return true;
	                    }
	                }
	            }
	            bytesWritten = 0;
	            return false;
	        }
	        private static void EncodeValueStringPart(string value, Span<byte> destination)
	        {
	            Debug.Assert(destination.Length >= value.Length);
	            for (int i = 0; i < value.Length; i++)
	            {
	                char c = value[i];
	                if ((c & 0xFF80) != 0)
	                {
	                    throw new Exception();
	                    // TODO: Need to add message to thrown exception
	                   // throw new HttpRequestException();
	                }
	                destination[i] = (byte)c;
	            }
	        }
	        public static bool EncodeStringLiteral(ReadOnlySpan<byte> value, Span<byte> destination, out int bytesWritten)
	        {
	            // From https://tools.ietf.org/html/rfc7541#section-5.2
	            // ------------------------------------------------------
	            //   0   1   2   3   4   5   6   7
	            // +---+---+---+---+---+---+---+---+
	            // | H |    String Length (7+)     |
	            // +---+---------------------------+
	            // |  String Data (Length octets)  |
	            // +-------------------------------+
	            if (destination.Length != 0)
	            {
	                destination[0] = 0; // TODO: Use Huffman encoding
	                if (IntegerEncoder.Encode(value.Length, 7, destination, out int integerLength))
	                {
	                    Debug.Assert(integerLength >= 1);
	                    destination = destination.Slice(integerLength);
	                    if (value.Length <= destination.Length)
	                    {
	                        // Note: No validation. Bytes should have already been validated.
	                        value.CopyTo(destination);
	                        bytesWritten = integerLength + value.Length;
	                        return true;
	                    }
	                }
	            }
	            bytesWritten = 0;
	            return false;
	        }
	        public static bool EncodeStringLiteral(string value, Span<byte> destination, out int bytesWritten)
	        {
	            return EncodeStringLiteral(value, valueEncoding: null, destination, out bytesWritten);
	        }
	        public static bool EncodeStringLiteral(string value, Encoding? valueEncoding, Span<byte> destination, out int bytesWritten)
	        {
	            // From https://tools.ietf.org/html/rfc7541#section-5.2
	            // ------------------------------------------------------
	            //   0   1   2   3   4   5   6   7
	            // +---+---+---+---+---+---+---+---+
	            // | H |    String Length (7+)     |
	            // +---+---------------------------+
	            // |  String Data (Length octets)  |
	            // +-------------------------------+
	            if (destination.Length != 0)
	            {
	                destination[0] = 0; // TODO: Use Huffman encoding
	                int encodedStringLength = valueEncoding is null || ReferenceEquals(valueEncoding, Encoding.Latin1)
	                    ? value.Length
	                    : valueEncoding.GetByteCount(value);
	                if (IntegerEncoder.Encode(encodedStringLength, 7, destination, out int integerLength))
	                {
	                    Debug.Assert(integerLength >= 1);
	                    destination = destination.Slice(integerLength);
	                    if (encodedStringLength <= destination.Length)
	                    {
	                        if (valueEncoding is null)
	                        {
	                            EncodeValueStringPart(value, destination);
	                        }
	                        else
	                        {
	                            int written = valueEncoding.GetBytes(value, destination);
	                            Debug.Assert(written == encodedStringLength);
	                        }
	                        bytesWritten = integerLength + encodedStringLength;
	                        return true;
	                    }
	                }
	            }
	            bytesWritten = 0;
	            return false;
	        }
	        public static bool EncodeDynamicTableSizeUpdate(int value, Span<byte> destination, out int bytesWritten)
	        {
	            // From https://tools.ietf.org/html/rfc7541#section-6.3
	            // ----------------------------------------------------
	            //   0   1   2   3   4   5   6   7
	            // +---+---+---+---+---+---+---+---+
	            // | 0 | 0 | 1 |   Max size (5+)   |
	            // +---+---------------------------+
	            if (destination.Length != 0)
	            {
	                destination[0] = 0x20;
	                return IntegerEncoder.Encode(value, 5, destination, out bytesWritten);
	            }
	            bytesWritten = 0;
	            return false;
	        }
	        public static bool EncodeStringLiterals(ReadOnlySpan<string> values, string? separator, Span<byte> destination, out int bytesWritten)
	        {
	            return EncodeStringLiterals(values, separator, valueEncoding: null, destination, out bytesWritten);
	        }
	        public static bool EncodeStringLiterals(ReadOnlySpan<string> values, string? separator, Encoding? valueEncoding, Span<byte> destination, out int bytesWritten)
	        {
	            bytesWritten = 0;
	            if (values.Length == 0)
	            {
	                return EncodeStringLiteral("", valueEncoding: null, destination, out bytesWritten);
	            }
	            else if (values.Length == 1)
	            {
	                return EncodeStringLiteral(values[0], valueEncoding, destination, out bytesWritten);
	            }
	            if (destination.Length != 0)
	            {
	                Debug.Assert(separator != null);
	                int valueLength;
	                // Calculate length of all parts and separators.
	                if (valueEncoding is null || ReferenceEquals(valueEncoding, Encoding.Latin1))
	                {
	                    valueLength = checked((int)(values.Length - 1) * separator.Length);
	                    foreach (string part in values)
	                    {
	                        valueLength = checked((int)(valueLength + part.Length));
	                    }
	                }
	                else
	                {
	                    valueLength = checked((int)(values.Length - 1) * valueEncoding.GetByteCount(separator));
	                    foreach (string part in values)
	                    {
	                        valueLength = checked((int)(valueLength + valueEncoding.GetByteCount(part)));
	                    }
	                }
	                destination[0] = 0;
	                if (IntegerEncoder.Encode(valueLength, 7, destination, out int integerLength))
	                {
	                    Debug.Assert(integerLength >= 1);
	                    destination = destination.Slice(integerLength);
	                    if (destination.Length >= valueLength)
	                    {
	                        if (valueEncoding is null)
	                        {
	                            string value = values[0];
	                            EncodeValueStringPart(value, destination);
	                            destination = destination.Slice(value.Length);
	                            for (int i = 1; i < values.Length; i++)
	                            {
	                                EncodeValueStringPart(separator, destination);
	                                destination = destination.Slice(separator.Length);
	                                value = values[i];
	                                EncodeValueStringPart(value, destination);
	                                destination = destination.Slice(value.Length);
	                            }
	                        }
	                        else
	                        {
	                            int written = valueEncoding.GetBytes(values[0], destination);
	                            destination = destination.Slice(written);
	                            for (int i = 1; i < values.Length; i++)
	                            {
	                                written = valueEncoding.GetBytes(separator, destination);
	                                destination = destination.Slice(written);
	                                written = valueEncoding.GetBytes(values[i], destination);
	                                destination = destination.Slice(written);
	                            }
	                        }
	                        bytesWritten = integerLength + valueLength;
	                        return true;
	                    }
	                }
	            }
	            return false;
	        }
	        public static byte[] EncodeLiteralHeaderFieldWithoutIndexingToAllocatedArray(int index)
	        {
	            Span<byte> span = stackalloc byte[256];
	            bool success = EncodeLiteralHeaderFieldWithoutIndexing(index, span, out int length);
	            Debug.Assert(success, $"Stack-allocated space was too small for index '{index}'.");
	            return span.Slice(0, length).ToArray();
	        }
	        public static byte[] EncodeLiteralHeaderFieldWithoutIndexingNewNameToAllocatedArray(string name)
	        {
	            Span<byte> span = stackalloc byte[256];
	            bool success = EncodeLiteralHeaderFieldWithoutIndexingNewName(name, span, out int length);
	            Debug.Assert(success, $"Stack-allocated space was too small for \"{name}\".");
	            return span.Slice(0, length).ToArray();
	        }
	        public static byte[] EncodeLiteralHeaderFieldWithoutIndexingToAllocatedArray(int index, string value)
	        {
	            Span<byte> span =
	#if DEBUG
	                stackalloc byte[4]; // to validate growth algorithm
	#else
	                stackalloc byte[512];
	#endif
	            while (true)
	            {
	                if (EncodeLiteralHeaderFieldWithoutIndexing(index, value, valueEncoding: null, span, out int length))
	                {
	                    return span.Slice(0, length).ToArray();
	                }
	                // This is a rare path, only used once per HTTP/2 connection and only
	                // for very long host names.  Just allocate rather than complicate
	                // the code with ArrayPool usage.  In practice we should never hit this,
	                // as hostnames should be <= 255 characters.
	                span = new byte[span.Length * 2];
	            }
	        }
	    }
	internal readonly struct HPackHeaderField
	{
	    // http://httpwg.org/specs/rfc7541.html#rfc.section.4.1
	    public const int RfcOverhead = 32;
	    public HPackHeaderField(int? staticTableIndex, ReadOnlySpan<byte> name, ReadOnlySpan<byte> value)
	    {
	        // Store the static table index (if there is one) for the header field.
	        // ASP.NET Core has a fast path that sets a header value using the static table index instead of the name.
	        StaticTableIndex = staticTableIndex;
	        Debug.Assert(name.Length > 0);
	        // TODO: We're allocating here on every new table entry.
	        // That means a poorly-behaved server could cause us to allocate repeatedly.
	        // We should revisit our allocation strategy here so we don't need to allocate per entry
	        // and we have a cap to how much allocation can happen per dynamic table
	        // (without limiting the number of table entries a server can provide within the table size limit).
	        Name = name.ToArray();
	        Value = value.ToArray();
	    }
	    public int? StaticTableIndex { get; }
	    public byte[] Name { get; }
	    public byte[] Value { get; }
	    public int Length => GetLength(Name.Length, Value.Length);
	    public static int GetLength(int nameLength, int valueLength) => nameLength + valueLength + RfcOverhead;
	    public override string ToString()
	    {
	        if (Name != null)
	        {
	            return Encoding.Latin1.GetString(Name) + ": " + Encoding.Latin1.GetString(Value);
	        }
	        else
	        {
	            return "<empty>";
	        }
	    }
	}
	// Ref: https://httpwg.org/specs/rfc7541.html#rfc.section.2.3.1
	// The static table consists of a predefined static list of header fields for HTTP/2
	internal static partial class HPackStaticTable
	{
	    // Values for encoding.
	    // Unused values are omitted.
	    public const int Authority = 1;
	    public const int MethodGet = 2;
	    public const int MethodPost = 3;
	    public const int PathSlash = 4;
	    public const int SchemeHttp = 6;
	    public const int SchemeHttps = 7;
	    public const int Status200 = 8;
	    public const int AcceptCharset = 15;
	    public const int AcceptEncoding = 16;
	    public const int AcceptLanguage = 17;
	    public const int AcceptRanges = 18;
	    public const int Accept = 19;
	    public const int AccessControlAllowOrigin = 20;
	    public const int Age = 21;
	    public const int Allow = 22;
	    public const int Authorization = 23;
	    public const int CacheControl = 24;
	    public const int ContentDisposition = 25;
	    public const int ContentEncoding = 26;
	    public const int ContentLanguage = 27;
	    public const int ContentLength = 28;
	    public const int ContentLocation = 29;
	    public const int ContentRange = 30;
	    public const int ContentType = 31;
	    public const int Cookie = 32;
	    public const int Date = 33;
	    public const int ETag = 34;
	    public const int Expect = 35;
	    public const int Expires = 36;
	    public const int From = 37;
	    public const int Host = 38;
	    public const int IfMatch = 39;
	    public const int IfModifiedSince = 40;
	    public const int IfNoneMatch = 41;
	    public const int IfRange = 42;
	    public const int IfUnmodifiedSince = 43;
	    public const int LastModified = 44;
	    public const int Link = 45;
	    public const int Location = 46;
	    public const int MaxForwards = 47;
	    public const int ProxyAuthenticate = 48;
	    public const int ProxyAuthorization = 49;
	    public const int Range = 50;
	    public const int Referer = 51;
	    public const int Refresh = 52;
	    public const int RetryAfter = 53;
	    public const int Server = 54;
	    public const int SetCookie = 55;
	    public const int StrictTransportSecurity = 56;
	    public const int TransferEncoding = 57;
	    public const int UserAgent = 58;
	    public const int Vary = 59;
	    public const int Via = 60;
	    public const int WwwAuthenticate = 61;
	    public static int Count => s_staticDecoderTable.Length;
	    public static ref readonly HPackHeaderField Get(int index) => ref s_staticDecoderTable[index];
	    public static bool TryGetStatusIndex(int status, out int index)
	    {
	        index = status switch
	        {
	            200 => 8,
	            204 => 9,
	            206 => 10,
	            304 => 11,
	            400 => 12,
	            404 => 13,
	            500 => 14,
	            _ => -1
	        };
	        return index != -1;
	    }
	    private static readonly HPackHeaderField[] s_staticDecoderTable = new HPackHeaderField[]
	    {
	            CreateHeaderField(1, ":authority", ""),
	            CreateHeaderField(2, ":method", "GET"),
	            CreateHeaderField(3, ":method", "POST"),
	            CreateHeaderField(4, ":path", "/"),
	            CreateHeaderField(5, ":path", "/index.html"),
	            CreateHeaderField(6, ":scheme", "http"),
	            CreateHeaderField(7, ":scheme", "https"),
	            CreateHeaderField(8, ":status", "200"),
	            CreateHeaderField(9, ":status", "204"),
	            CreateHeaderField(10, ":status", "206"),
	            CreateHeaderField(11, ":status", "304"),
	            CreateHeaderField(12, ":status", "400"),
	            CreateHeaderField(13, ":status", "404"),
	            CreateHeaderField(14, ":status", "500"),
	            CreateHeaderField(15, "accept-charset", ""),
	            CreateHeaderField(16, "accept-encoding", "gzip, deflate"),
	            CreateHeaderField(17, "accept-language", ""),
	            CreateHeaderField(18, "accept-ranges", ""),
	            CreateHeaderField(19, "accept", ""),
	            CreateHeaderField(20, "access-control-allow-origin", ""),
	            CreateHeaderField(21, "age", ""),
	            CreateHeaderField(22, "allow", ""),
	            CreateHeaderField(23, "authorization", ""),
	            CreateHeaderField(24, "cache-control", ""),
	            CreateHeaderField(25, "content-disposition", ""),
	            CreateHeaderField(26, "content-encoding", ""),
	            CreateHeaderField(27, "content-language", ""),
	            CreateHeaderField(28, "content-length", ""),
	            CreateHeaderField(29, "content-location", ""),
	            CreateHeaderField(30, "content-range", ""),
	            CreateHeaderField(31, "content-type", ""),
	            CreateHeaderField(32, "cookie", ""),
	            CreateHeaderField(33, "date", ""),
	            CreateHeaderField(34, "etag", ""),
	            CreateHeaderField(35, "expect", ""),
	            CreateHeaderField(36, "expires", ""),
	            CreateHeaderField(37, "from", ""),
	            CreateHeaderField(38, "host", ""),
	            CreateHeaderField(39, "if-match", ""),
	            CreateHeaderField(40, "if-modified-since", ""),
	            CreateHeaderField(41, "if-none-match", ""),
	            CreateHeaderField(42, "if-range", ""),
	            CreateHeaderField(43, "if-unmodified-since", ""),
	            CreateHeaderField(44, "last-modified", ""),
	            CreateHeaderField(45, "link", ""),
	            CreateHeaderField(46, "location", ""),
	            CreateHeaderField(47, "max-forwards", ""),
	            CreateHeaderField(48, "proxy-authenticate", ""),
	            CreateHeaderField(49, "proxy-authorization", ""),
	            CreateHeaderField(50, "range", ""),
	            CreateHeaderField(51, "referer", ""),
	            CreateHeaderField(52, "refresh", ""),
	            CreateHeaderField(53, "retry-after", ""),
	            CreateHeaderField(54, "server", ""),
	            CreateHeaderField(55, "set-cookie", ""),
	            CreateHeaderField(56, "strict-transports-security", ""),
	            CreateHeaderField(57, "transfer-encoding", ""),
	            CreateHeaderField(58, "user-agent", ""),
	            CreateHeaderField(59, "vary", ""),
	            CreateHeaderField(60, "via", ""),
	            CreateHeaderField(61, "www-authenticate", "")
	    };
	    // TODO: The HeaderField constructor will allocate and copy again. We should avoid this.
	    // Tackle as part of header table allocation strategy in general (see note in HeaderField constructor).
	    private static HPackHeaderField CreateHeaderField(int staticTableIndex, string name, string value) =>
	        new HPackHeaderField(
	            staticTableIndex,
	            Encoding.ASCII.GetBytes(name),
	            value.Length != 0 ? Encoding.ASCII.GetBytes(value) : Array.Empty<byte>());
	}
	#endregion
	#region \Server\Internal\Http3
	internal class Http3Connection : HttpConnection
	{
	    internal override IAsyncEnumerable<IHttpContext> ProcessAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
	    {
	        throw new NotImplementedException();
	    }
	    //protected override IAsyncEnumerable<IHttpContext> ReceiveAsync(CancellationToken cancellationToken = default)
	    //{
	    //    throw new NotImplementedException();
	    //}
	    //protected override Task<IHttpContext> SendAsync(IHttpContext context, CancellationToken cancellationToken = default)
	    //{
	    //    throw new NotImplementedException();
	    //}
	}
	internal class Http3Context : IHttpContext
	{
	    public HttpVersion Version => throw new NotImplementedException();
	    public IHttpSession? Session => throw new NotImplementedException();
	    public IHttpRequest? Request => throw new NotImplementedException();
	    public IHttpResponse? Response => throw new NotImplementedException();
	    public IServiceProvider? ServiceProvider => throw new NotImplementedException();
	    public ValueTask DisposeAsync()
	    {
	        throw new NotImplementedException();
	    }
	}
	    internal class Http3Request : IHttpRequest
	    {
	        public HttpPath Path => throw new NotImplementedException();
	        public HttpMethod Method => throw new NotImplementedException();
	        public HttpScheme Scheme => HttpScheme.Https;
	        public IHttpQueryCollection Query => throw new NotImplementedException();
	        public IHttpHeaderCollection Headers => throw new NotImplementedException();
	        public IHttpCookieCollection Cookies => throw new NotImplementedException();
	        public Stream Body => throw new NotImplementedException();
	        public HttpHost Host => throw new NotImplementedException();
	        public IHttpFormCollection Form => throw new NotImplementedException();
	        public ClaimsPrincipal ClaimsPrincipal => throw new NotImplementedException();
	    }
	#endregion
	#region \Server\Internal\Http3\Exceptions
	internal class Http3Exception : HttpException
	{
	    public Http3Exception(string message) : base(message)
	    {
	    }
	    public Http3Exception(string message, Exception inner) : base(message, inner)
	    {
	    }
	}
	#endregion
	#region \Server\Internal\Http3\Frames
	internal enum Http3ErrorCode : long
	{
	    NoError = 0x100,
	    ProtocolError = 0x101,
	    InternalError = 0x102,
	    StreamCreationError = 0x103,
	    ClosedCriticalStream = 0x104,
	    UnexpectedFrame = 0x105,
	    FrameError = 0x106,
	    ExcessiveLoad = 0x107,
	    IdError = 0x108,
	    SettingsError = 0x109,
	    MissingSettings = 0x10a,
	    RequestRejected = 0x10b,
	    RequestCancelled = 0x10c,
	    RequestIncomplete = 0x10d,
	    MessageError = 0x10e,
	    ConnectError = 0x10f,
	    VersionFallback = 0x110,
	}
	internal static class Http3Formatting
	{
	    public static string ToFormattedType(Http3FrameType type)
	    {
	        return type switch
	        {
	            Http3FrameType.Data => "DATA",
	            Http3FrameType.Headers => "HEADERS",
	            Http3FrameType.CancelPush => "CANCEL_PUSH",
	            Http3FrameType.Settings => "SETTINGS",
	            Http3FrameType.PushPromise => "PUSH_PROMISE",
	            Http3FrameType.GoAway => "GOAWAY",
	            Http3FrameType.MaxPushId => "MAX_PUSH_ID",
	            _ => type.ToString()
	        };
	    }
	    public static string ToFormattedErrorCode(Http3ErrorCode errorCode)
	    {
	        return errorCode switch
	        {
	            Http3ErrorCode.NoError => "H3_NO_ERROR",
	            Http3ErrorCode.ProtocolError => "H3_GENERAL_PROTOCOL_ERROR",
	            Http3ErrorCode.InternalError => "H3_INTERNAL_ERROR",
	            Http3ErrorCode.StreamCreationError => "H3_STREAM_CREATION_ERROR",
	            Http3ErrorCode.ClosedCriticalStream => "H3_CLOSED_CRITICAL_STREAM",
	            Http3ErrorCode.UnexpectedFrame => "H3_FRAME_UNEXPECTED",
	            Http3ErrorCode.FrameError => "H3_FRAME_ERROR",
	            Http3ErrorCode.ExcessiveLoad => "H3_EXCESSIVE_LOAD",
	            Http3ErrorCode.IdError => "H3_ID_ERROR",
	            Http3ErrorCode.SettingsError => "H3_SETTINGS_ERROR",
	            Http3ErrorCode.MissingSettings => "H3_MISSING_SETTINGS",
	            Http3ErrorCode.RequestRejected => "H3_REQUEST_REJECTED",
	            Http3ErrorCode.RequestCancelled => "H3_REQUEST_CANCELLED",
	            Http3ErrorCode.RequestIncomplete => "H3_REQUEST_INCOMPLETE",
	            Http3ErrorCode.ConnectError => "H3_CONNECT_ERROR",
	            Http3ErrorCode.VersionFallback => "H3_VERSION_FALLBACK",
	            _ => errorCode.ToString()
	        };
	    }
	}
	internal enum Http3FrameType : long
	{
	    Data = 0x0,
	    Headers = 0x1,
	    ReservedHttp2Priority = 0x2,
	    CancelPush = 0x3,
	    Settings = 0x4,
	    PushPromise = 0x5,
	    ReservedHttp2Ping = 0x6,
	    GoAway = 0x7,
	    ReservedHttp2WindowUpdate = 0x8,
	    ReservedHttp2Continuation = 0x9,
	    MaxPushId = 0xD
	}
	internal partial class Http3RawFrame
	{
	    public long Length { get; set; }
	    public Http3FrameType Type { get; internal set; }
	    public string FormattedType => Http3Formatting.ToFormattedType(Type);
	    public override string ToString()
	    {
	        return $"{FormattedType} Length: {Length}";
	    }
	}
	internal partial class Http3RawFrame
	{
	    public void PrepareData()
	    {
	        Length = 0;
	        Type = Http3FrameType.Data;
	    }
	}
	internal partial class Http3RawFrame
	{
	    public void PrepareGoAway()
	    {
	        Length = 0;
	        Type = Http3FrameType.GoAway;
	    }
	}
	internal partial class Http3RawFrame
	{
	    public void PrepareHeaders()
	    {
	        Length = 0;
	        Type = Http3FrameType.Headers;
	    }
	}
	internal partial class Http3RawFrame
	{
	    public void PrepareSettings()
	    {
	        Length = 0;
	        Type = Http3FrameType.Settings;
	    }
	}
	#endregion
	#region \Server\Internal\Http3\QPack
	internal static class QPackEncoder
	{
	    // https://tools.ietf.org/html/draft-ietf-quic-qpack-11#section-4.5.2
	    //   0   1   2   3   4   5   6   7
	    // +---+---+---+---+---+---+---+---+
	    // | 1 | T |      Index (6+)       |
	    // +---+---+-----------------------+
	    //
	    // Note for this method's implementation of above:
	    // - T is constant 1 here, indicating a static table reference.
	    public static bool EncodeStaticIndexedHeaderField(int index, Span<byte> destination, out int bytesWritten)
	    {
	        if (!destination.IsEmpty)
	        {
	            destination[0] = 0b11000000;
	            return IntegerEncoder.Encode(index, 6, destination, out bytesWritten);
	        }
	        else
	        {
	            bytesWritten = 0;
	            return false;
	        }
	    }
	    public static byte[] EncodeStaticIndexedHeaderFieldToArray(int index)
	    {
	        Span<byte> buffer = stackalloc byte[IntegerEncoder.MaxInt32EncodedLength];
	        bool res = EncodeStaticIndexedHeaderField(index, buffer, out int bytesWritten);
	        Debug.Assert(res);
	        return buffer.Slice(0, bytesWritten).ToArray();
	    }
	    // https://tools.ietf.org/html/draft-ietf-quic-qpack-11#section-4.5.4
	    //   0   1   2   3   4   5   6   7
	    // +---+---+---+---+---+---+---+---+
	    // | 0 | 1 | N | T |Name Index (4+)|
	    // +---+---+---+---+---------------+
	    // | H |     Value Length (7+)     |
	    // +---+---------------------------+
	    // |  Value String (Length bytes)  |
	    // +-------------------------------+
	    //
	    // Note for this method's implementation of above:
	    // - N is constant 0 here, indicating intermediates (proxies) can compress the header when fordwarding.
	    // - T is constant 1 here, indicating a static table reference.
	    // - H is constant 0 here, as we do not yet perform Huffman coding.
	    public static bool EncodeLiteralHeaderFieldWithStaticNameReference(int index, string value, Span<byte> destination, out int bytesWritten)
	    {
	        return EncodeLiteralHeaderFieldWithStaticNameReference(index, value, valueEncoding: null, destination, out bytesWritten);
	    }
	    public static bool EncodeLiteralHeaderFieldWithStaticNameReference(int index, string value, Encoding? valueEncoding, Span<byte> destination, out int bytesWritten)
	    {
	        // Requires at least two bytes (one for name reference header, one for value length)
	        if (destination.Length >= 2)
	        {
	            destination[0] = 0b01010000;
	            if (IntegerEncoder.Encode(index, 4, destination, out int headerBytesWritten))
	            {
	                destination = destination.Slice(headerBytesWritten);
	                if (EncodeValueString(value, valueEncoding, destination, out int valueBytesWritten))
	                {
	                    bytesWritten = headerBytesWritten + valueBytesWritten;
	                    return true;
	                }
	            }
	        }
	        bytesWritten = 0;
	        return false;
	    }
	    public static byte[] EncodeLiteralHeaderFieldWithStaticNameReferenceToArray(int index)
	    {
	        Span<byte> temp = stackalloc byte[IntegerEncoder.MaxInt32EncodedLength];
	        temp[0] = 0b01110000;
	        bool res = IntegerEncoder.Encode(index, 4, temp, out int headerBytesWritten);
	        Debug.Assert(res);
	        return temp.Slice(0, headerBytesWritten).ToArray();
	    }
	    public static byte[] EncodeLiteralHeaderFieldWithStaticNameReferenceToArray(int index, string value)
	    {
	        Span<byte> temp = value.Length < 256 ? stackalloc byte[256 + IntegerEncoder.MaxInt32EncodedLength * 2] : new byte[value.Length + IntegerEncoder.MaxInt32EncodedLength * 2];
	        bool res = EncodeLiteralHeaderFieldWithStaticNameReference(index, value, temp, out int bytesWritten);
	        Debug.Assert(res);
	        return temp.Slice(0, bytesWritten).ToArray();
	    }
	    // https://tools.ietf.org/html/draft-ietf-quic-qpack-11#section-4.5.6
	    //   0   1   2   3   4   5   6   7
	    // +---+---+---+---+---+---+---+---+
	    // | 0 | 0 | 1 | N | H |NameLen(3+)|
	    // +---+---+---+---+---+-----------+
	    // |  Name String (Length bytes)   |
	    // +---+---------------------------+
	    // | H |     Value Length (7+)     |
	    // +---+---------------------------+
	    // |  Value String (Length bytes)  |
	    // +-------------------------------+
	    //
	    // Note for this method's implementation of above:
	    // - N is constant 0 here, indicating intermediates (proxies) can compress the header when fordwarding.
	    // - H is constant 0 here, as we do not yet perform Huffman coding.
	    public static bool EncodeLiteralHeaderFieldWithoutNameReference(string name, string value, Span<byte> destination, out int bytesWritten)
	    {
	        return EncodeLiteralHeaderFieldWithoutNameReference(name, value, valueEncoding: null, destination, out bytesWritten);
	    }
	    public static bool EncodeLiteralHeaderFieldWithoutNameReference(string name, string value, Encoding? valueEncoding, Span<byte> destination, out int bytesWritten)
	    {
	        if (EncodeNameString(name, destination, out int nameLength) && EncodeValueString(value, valueEncoding, destination.Slice(nameLength), out int valueLength))
	        {
	            bytesWritten = nameLength + valueLength;
	            return true;
	        }
	        else
	        {
	            bytesWritten = 0;
	            return false;
	        }
	    }
	    public static bool EncodeLiteralHeaderFieldWithoutNameReference(string name, ReadOnlySpan<string> values, string valueSeparator, Span<byte> destination, out int bytesWritten)
	    {
	        return EncodeLiteralHeaderFieldWithoutNameReference(name, values, valueSeparator, valueEncoding: null, destination, out bytesWritten);
	    }
	    public static bool EncodeLiteralHeaderFieldWithoutNameReference(string name, ReadOnlySpan<string> values, string valueSeparator, Encoding? valueEncoding, Span<byte> destination, out int bytesWritten)
	    {
	        if (EncodeNameString(name, destination, out int nameLength) && EncodeValueString(values, valueSeparator, valueEncoding, destination.Slice(nameLength), out int valueLength))
	        {
	            bytesWritten = nameLength + valueLength;
	            return true;
	        }
	        bytesWritten = 0;
	        return false;
	    }
	    public static byte[] EncodeLiteralHeaderFieldWithoutNameReferenceToArray(string name)
	    {
	        Span<byte> temp = name.Length < 256 ? stackalloc byte[256 + IntegerEncoder.MaxInt32EncodedLength] : new byte[name.Length + IntegerEncoder.MaxInt32EncodedLength];
	        bool res = EncodeNameString(name, temp, out int nameLength);
	        Debug.Assert(res);
	        return temp.Slice(0, nameLength).ToArray();
	    }
	    public static byte[] EncodeLiteralHeaderFieldWithoutNameReferenceToArray(string name, string value)
	    {
	        Span<byte> temp = (name.Length + value.Length) < 256 ? stackalloc byte[256 + IntegerEncoder.MaxInt32EncodedLength * 2] : new byte[name.Length + value.Length + IntegerEncoder.MaxInt32EncodedLength * 2];
	        bool res = EncodeLiteralHeaderFieldWithoutNameReference(name, value, temp, out int bytesWritten);
	        Debug.Assert(res);
	        return temp.Slice(0, bytesWritten).ToArray();
	    }
	    private static bool EncodeValueString(string s, Encoding? valueEncoding, Span<byte> buffer, out int length)
	    {
	        if (buffer.Length != 0)
	        {
	            buffer[0] = 0;
	            int encodedStringLength = valueEncoding is null || ReferenceEquals(valueEncoding, Encoding.Latin1)
	                ? s.Length
	                : valueEncoding.GetByteCount(s);
	            if (IntegerEncoder.Encode(encodedStringLength, 7, buffer, out int nameLength))
	            {
	                buffer = buffer.Slice(nameLength);
	                if (buffer.Length >= encodedStringLength)
	                {
	                    if (valueEncoding is null)
	                    {
	                        EncodeValueStringPart(s, buffer);
	                    }
	                    else
	                    {
	                        int written = valueEncoding.GetBytes(s, buffer);
	                        Debug.Assert(written == encodedStringLength);
	                    }
	                    length = nameLength + encodedStringLength;
	                    return true;
	                }
	            }
	        }
	        length = 0;
	        return false;
	    }
	    public static bool EncodeValueString(ReadOnlySpan<string> values, string? separator, Span<byte> buffer, out int length)
	    {
	        return EncodeValueString(values, separator, valueEncoding: null, buffer, out length);
	    }
	    public static bool EncodeValueString(ReadOnlySpan<string> values, string? separator, Encoding? valueEncoding, Span<byte> buffer, out int length)
	    {
	        if (values.Length == 1)
	        {
	            return EncodeValueString(values[0], valueEncoding, buffer, out length);
	        }
	        if (values.Length == 0)
	        {
	            // TODO: this will be called with a string array from HttpHeaderCollection. Can we ever get a 0-length array from that? Assert if not.
	            return EncodeValueString(string.Empty, valueEncoding: null, buffer, out length);
	        }
	        if (buffer.Length > 0)
	        {
	            Debug.Assert(separator != null);
	            int valueLength;
	            if (valueEncoding is null || ReferenceEquals(valueEncoding, Encoding.Latin1))
	            {
	                valueLength = separator.Length * (values.Length - 1);
	                foreach (string part in values)
	                {
	                    valueLength += part.Length;
	                }
	            }
	            else
	            {
	                valueLength = valueEncoding.GetByteCount(separator) * (values.Length - 1);
	                foreach (string part in values)
	                {
	                    valueLength += valueEncoding.GetByteCount(part);
	                }
	            }
	            buffer[0] = 0;
	            if (IntegerEncoder.Encode(valueLength, 7, buffer, out int nameLength))
	            {
	                buffer = buffer.Slice(nameLength);
	                if (buffer.Length >= valueLength)
	                {
	                    if (valueEncoding is null)
	                    {
	                        string value = values[0];
	                        EncodeValueStringPart(value, buffer);
	                        buffer = buffer.Slice(value.Length);
	                        for (int i = 1; i < values.Length; i++)
	                        {
	                            EncodeValueStringPart(separator, buffer);
	                            buffer = buffer.Slice(separator.Length);
	                            value = values[i];
	                            EncodeValueStringPart(value, buffer);
	                            buffer = buffer.Slice(value.Length);
	                        }
	                    }
	                    else
	                    {
	                        int written = valueEncoding.GetBytes(values[0], buffer);
	                        buffer = buffer.Slice(written);
	                        for (int i = 1; i < values.Length; i++)
	                        {
	                            written = valueEncoding.GetBytes(separator, buffer);
	                            buffer = buffer.Slice(written);
	                            written = valueEncoding.GetBytes(values[i], buffer);
	                            buffer = buffer.Slice(written);
	                        }
	                    }
	                    length = nameLength + valueLength;
	                    return true;
	                }
	            }
	        }
	        length = 0;
	        return false;
	    }
	    private static void EncodeValueStringPart(string s, Span<byte> buffer)
	    {
	        Debug.Assert(buffer.Length >= s.Length);
	        for (int i = 0; i < s.Length; ++i)
	        {
	            char ch = s[i];
	            if (ch > 127)
	            {
	                throw new Exception("temp");
	                //throw new QPackEncodingException(SR.net_http_request_invalid_char_encoding);
	            }
	            buffer[i] = (byte)ch;
	        }
	    }
	    private static bool EncodeNameString(string s, Span<byte> buffer, out int length)
	    {
	        const int toLowerMask = 0x20;
	        if (buffer.Length != 0)
	        {
	            buffer[0] = 0x30;
	            if (IntegerEncoder.Encode(s.Length, 3, buffer, out int nameLength))
	            {
	                buffer = buffer.Slice(nameLength);
	                if (buffer.Length >= s.Length)
	                {
	                    for (int i = 0; i < s.Length; ++i)
	                    {
	                        int ch = s[i];
	                        Debug.Assert(ch <= 127, "HttpHeaders prevents adding non-ASCII header names.");
	                        if ((uint)(ch - 'A') <= 'Z' - 'A')
	                        {
	                            ch |= toLowerMask;
	                        }
	                        buffer[i] = (byte)ch;
	                    }
	                    length = nameLength + s.Length;
	                    return true;
	                }
	            }
	        }
	        length = 0;
	        return false;
	    }
	    /*
	     *     0   1   2   3   4   5   6   7
	           +---+---+---+---+---+---+---+---+
	           |   Required Insert Count (8+)  |
	           +---+---------------------------+
	           | S |      Delta Base (7+)      |
	           +---+---------------------------+
	           |      Compressed Headers     ...
	           +-------------------------------+
	     *
	     */
	    private static bool EncodeHeaderBlockPrefix(Span<byte> destination, out int bytesWritten)
	    {
	        int length;
	        bytesWritten = 0;
	        // Required insert count as first int
	        if (!IntegerEncoder.Encode(0, 8, destination, out length))
	        {
	            return false;
	        }
	        bytesWritten += length;
	        destination = destination.Slice(length);
	        // Delta base
	        if (destination.IsEmpty)
	        {
	            return false;
	        }
	        destination[0] = 0x00;
	        if (!IntegerEncoder.Encode(0, 7, destination, out length))
	        {
	            return false;
	        }
	        bytesWritten += length;
	        return true;
	    }
	}
	#endregion
	#region \Server\Internal\Transports
	internal class HttpTlsServerTransportMiddleware : TcpServerTransportMiddleware
	{
	    public override Task InvokeAsync(TcpServerTransportContext context, TransportMiddlewareHandler next)
	    {
	        return Task.CompletedTask;
	    }
	}
	#endregion
	#region \Server\Internal\Utilities
	// Mimics BinaryPrimitives with oddly sized units
	internal static class Bitshifter
	{
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public static uint ReadUInt24BigEndian(ReadOnlySpan<byte> source)
	    {
	        return (uint)((source[0] << 16) | (source[1] << 8) | source[2]);
	    }
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public static void WriteUInt24BigEndian(Span<byte> destination, uint value)
	    {
	        Debug.Assert(value <= 0xFF_FF_FF, value.ToString(CultureInfo.InvariantCulture));
	        destination[0] = (byte)((value & 0xFF_00_00) >> 16);
	        destination[1] = (byte)((value & 0x00_FF_00) >> 8);
	        destination[2] = (byte)(value & 0x00_00_FF);
	    }
	    // Drops the highest order bit
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public static uint ReadUInt31BigEndian(ReadOnlySpan<byte> source)
	    {
	        return BinaryPrimitives.ReadUInt32BigEndian(source) & 0x7F_FF_FF_FF;
	    }
	    // Does not overwrite the highest order bit
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public static void WriteUInt31BigEndian(Span<byte> destination, uint value)
	        => WriteUInt31BigEndian(destination, value, true);
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public static void WriteUInt31BigEndian(Span<byte> destination, uint value, bool preserveHighestBit)
	    {
	        Debug.Assert(value <= 0x7F_FF_FF_FF, value.ToString(CultureInfo.InvariantCulture));
	        if (preserveHighestBit)
	        {
	            // Keep the highest bit
	            value |= (destination[0] & 0x80u) << 24;
	        }
	        BinaryPrimitives.WriteUInt32BigEndian(destination, value);
	    }
	}
	internal ref struct BufferWriter<T> where T : IBufferWriter<byte>
	{
	    private readonly T _output;
	    private Span<byte> _span;
	    private int _buffered;
	    private long _bytesCommitted;
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public BufferWriter(T output)
	    {
	        _buffered = 0;
	        _bytesCommitted = 0;
	        _output = output;
	        _span = output.GetSpan();
	    }
	    public readonly Span<byte> Span => _span;
	    public readonly long BytesCommitted => _bytesCommitted;
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public void Commit()
	    {
	        var buffered = _buffered;
	        if (buffered > 0)
	        {
	            _bytesCommitted += buffered;
	            _buffered = 0;
	            _output.Advance(buffered);
	        }
	    }
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public void Advance(int count)
	    {
	        _buffered += count;
	        _span = _span.Slice(count);
	    }
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public void Write(ReadOnlySpan<byte> source)
	    {
	        if (_span.Length >= source.Length)
	        {
	            source.CopyTo(_span);
	            Advance(source.Length);
	        }
	        else
	        {
	            WriteMultiBuffer(source);
	        }
	    }
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public void Ensure(int count = 1)
	    {
	        if (_span.Length < count)
	        {
	            EnsureMore(count);
	        }
	    }
	    [MethodImpl(MethodImplOptions.NoInlining)]
	    private void EnsureMore(int count = 0)
	    {
	        if (_buffered > 0)
	        {
	            Commit();
	        }
	        _span = _output.GetSpan(count);
	    }
	    private void WriteMultiBuffer(ReadOnlySpan<byte> source)
	    {
	        while (source.Length > 0)
	        {
	            if (_span.Length == 0)
	            {
	                EnsureMore();
	            }
	            var writable = Math.Min(source.Length, _span.Length);
	            source.Slice(0, writable).CopyTo(_span);
	            source = source.Slice(writable);
	            Advance(writable);
	        }
	    }
	}
	internal static class ChunkWriter
	{
	    public static int BeginChunkBytes(int dataCount, Span<byte> span)
	    {
	        // Determine the most-significant non-zero nibble
	        int total, shift;
	        var count = dataCount;
	        total = (count > 0xffff) ? 0x10 : 0x00;
	        count >>= total;
	        shift = (count > 0x00ff) ? 0x08 : 0x00;
	        count >>= shift;
	        total |= shift;
	        total |= (count > 0x000f) ? 0x04 : 0x00;
	        count = (total >> 2) + 3;
	        // This must be explicitly typed as ReadOnlySpan<byte>
	        // It then becomes a non-allocating mapping to the data section of the assembly.
	        // For more information see https://vcsjones.dev/2019/02/01/csharp-readonly-span-bytes-static
	        ReadOnlySpan<byte> hex = "0123456789abcdef".Select(x => (byte)x).ToArray();
	        var offset = 0;
	        for (shift = total; shift >= 0; shift -= 4)
	        {
	            // Uses dotnet/runtime#1644 to elide the bounds check on hex as the & 0x0f definitely
	            // constrains it to the range 0x0 - 0xf, matching the bounds of the array.
	            span[offset] = hex[(dataCount >> shift) & 0x0f];
	            offset++;
	        }
	        span[count - 2] = (byte)'\r';
	        span[count - 1] = (byte)'\n';
	        return count;
	    }
	    internal static int GetPrefixBytesForChunk(int length, out bool sliceOneByte)
	    {
	        sliceOneByte = false;
	        // If GetMemory returns one of the following values, there is no way to set the prefix/body lengths
	        // such that we either wouldn't have an invalid chunk or would need to copy if the entire memory chunk is used.
	        // For example, if GetMemory returned 21, we would guess that the chunked prefix is 4 bytes initially
	        // and the suffix is 2 bytes, meaning there is 15 bytes remaining to write into. However, 15 bytes only need 3
	        // bytes for the chunked prefix, so we would have to copy once we call advance. Therefore, to avoid this scenario,
	        // we slice the memory by one byte.
	        // See https://gist.github.com/halter73/af2b9f78978f83813b19e187c4e5309e if you would like to tweak the algorithm at all.
	        if (length <= 65544)
	        {
	            if (length <= 262)
	            {
	                if (length <= 21)
	                {
	                    if (length == 21)
	                    {
	                        sliceOneByte = true;
	                    }
	                    return 3;
	                }
	                else
	                {
	                    if (length == 262)
	                    {
	                        sliceOneByte = true;
	                    }
	                    return 4;
	                }
	            }
	            else
	            {
	                if (length <= 4103)
	                {
	                    if (length == 4103)
	                    {
	                        sliceOneByte = true;
	                    }
	                    return 5;
	                }
	                else
	                {
	                    if (length == 65544)
	                    {
	                        sliceOneByte = true;
	                    }
	                    return 6;
	                }
	            }
	        }
	        else
	        {
	            if (length <= 16777226)
	            {
	                if (length <= 1048585)
	                {
	                    if (length == 1048585)
	                    {
	                        sliceOneByte = true;
	                    }
	                    return 7;
	                }
	                else
	                {
	                    if (length == 16777226)
	                    {
	                        sliceOneByte = true;
	                    }
	                    return 8;
	                }
	            }
	            else
	            {
	                if (length <= 268435467)
	                {
	                    if (length == 268435467)
	                    {
	                        sliceOneByte = true;
	                    }
	                    return 9;
	                }
	                else
	                {
	                    return 10;
	                }
	            }
	        }
	    }
	    internal static int WriteBeginChunkBytes(this ref BufferWriter<PipeWriter> start, int dataCount)
	    {
	        // 10 bytes is max length + \r\n
	        start.Ensure(10);
	        var count = BeginChunkBytes(dataCount, start.Span);
	        start.Advance(count);
	        return count;
	    }
	    internal static void WriteEndChunkBytes(this ref BufferWriter<PipeWriter> start)
	    {
	        start.Ensure(2);
	        var span = start.Span;
	        // CRLF done in reverse order so the 1st index will elide the bounds check for the 0th index
	        span[1] = (byte)'\n';
	        span[0] = (byte)'\r';
	        start.Advance(2);
	    }
	}
	internal static class IntegerEncoder
	{
	    public const int MaxInt32EncodedLength = 6;
	    public static bool Encode(int value, int numBits, Span<byte> destination, out int bytesWritten)
	    {
	        Debug.Assert(value >= 0);
	        Debug.Assert(numBits >= 1 && numBits <= 8);
	        if (destination.Length == 0)
	        {
	            bytesWritten = 0;
	            return false;
	        }
	        destination[0] &= MaskHigh(8 - numBits);
	        if (value < (1 << numBits) - 1)
	        {
	            destination[0] |= (byte)value;
	            bytesWritten = 1;
	            return true;
	        }
	        else
	        {
	            destination[0] |= (byte)((1 << numBits) - 1);
	            if (1 == destination.Length)
	            {
	                bytesWritten = 0;
	                return false;
	            }
	            value -= ((1 << numBits) - 1);
	            int i = 1;
	            while (value >= 128)
	            {
	                destination[i++] = (byte)(value % 128 + 128);
	                if (i >= destination.Length)
	                {
	                    bytesWritten = 0;
	                    return false;
	                }
	                value /= 128;
	            }
	            destination[i++] = (byte)value;
	            bytesWritten = i;
	            return true;
	        }
	    }
	    private static byte MaskHigh(int n) => (byte)(sbyte.MinValue >> (n - 1));
	}
	#endregion
	#region \Server\Options
	public abstract class HttpTransportOptions
	{
	    public abstract void UseHttp(HttpVersion version);
	    public abstract void UseHttps(HttpVersion version);
	    public abstract void UseTransport(ITransport transport);
	    public abstract void UseTransport(Func<ITransport> configure);
	    public abstract void UseTcpTransport(Action<TcpServerTransportOptions> configure);
	}
	#endregion
}
#endregion
