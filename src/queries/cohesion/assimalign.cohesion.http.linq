<Query Kind="Program">
  <Namespace>static System.Text.Encoding</Namespace>
  <Namespace>System.Buffers</Namespace>
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
  <Namespace>System.Security.Claims</Namespace>
  <Namespace>System.Text.Encodings.Web</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Web</Namespace>
</Query>

#load ".\assimalign.cohesion.core"

using Assimalign.Cohesion.Http;

void Main()
{
	
}


public void Test()
{
	HttpMethod method = "Get";
}



#region Assimalign.Cohesion.Http
namespace Assimalign.Cohesion.Http
{
	#region \
	public class HttpCookie
	{
	    public HttpCookie()
	    {
	    }
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
	        this.Value = ThrowHelper.ThrowIfNullOrEmpty(value);
	    }
	    public string Value { get; }
	    public bool IsEmpty => string.IsNullOrEmpty(Value);
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
	        ReadOnlySpan<char> span = ThrowHelper.ThrowIfNullOrEmpty(value);
	        Value = span.ToString();
	    }
	    public string Value { get; }
	    //public int? Port { get; }
	    public bool Equals(HttpHost other)
	    {
	        return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
	    }
	    #region Overloads
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
	    #endregion
	    #region Operators
	    public static implicit operator HttpHost(string value)
	    {
	        return new HttpHost(value);
	    }
	    public static implicit operator string(HttpHost host)
	    {
	        return host.Value;
	    }
	    #endregion
	}
	[DebuggerDisplay("{Value}")]
	public readonly struct HttpMethod : IEquatable<HttpMethod>
	{
		const int _length = 16;
		
	    #region Constructors
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public HttpMethod(string value)
		{
			if (value.Length > _length)
			{
				ThrowHelper.ThrowArgumentException($"The method is too long. Must be under {_length} characters.");
			}

			ReadOnlySpan<char> source = ThrowHelper.ThrowIfNullOrEmpty(value);
			Span<char> destination = stackalloc char[source.Length];

			for (int i = 0; i < source.Length; i++)
			{
				var c = source[i];

				if (!char.IsLetterOrDigit(c))
				{
					ThrowHelper.InvalidHttpMethod(value);
				}
				if (char.IsLower(c))
				{
					c = char.ToUpper(c);
				}
				destination[i] = c;
			}

			Value = destination.ToString();
		}
		#endregion
	    #region Properties
	    public string Value { get; }
	    public static readonly HttpMethod Connect = "CONNECT";
	    public static readonly HttpMethod Delete = "DELETE";
	    public static readonly HttpMethod Get = "GET";
	    public static readonly HttpMethod Head = "HEAD";
	    public static readonly HttpMethod Options = "OPTIONS";
	    public static readonly HttpMethod Patch = "PATCH";
	    public static readonly HttpMethod Post = "POST";
	    public static readonly HttpMethod Put = "PUT";
	    public static readonly HttpMethod Trace = "TRACE";
	    #endregion
	    #region Methods
	    public bool Equals(HttpMethod other)
	    {
	        return Equals(this, other);
	    }
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
	    public static HttpMethod GetCanonicalizedValue(string method) => method switch
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
	    #endregion
	    #region Overloads
	    public override string ToString()
	    {
	        return Value;
	    }
	    public override bool Equals([NotNullWhen(true)] object? obj)
	    {
	        if (obj is HttpMethod method)
	        {
	            return Equals(method);
	        }
	        return false;
	    }
	    public override int GetHashCode()
	    {
	        return Value.GetHashCode();
	    }
	    #endregion
	    #region Operators
	    public static implicit operator HttpMethod(string method)
	    {
	        return new HttpMethod(method);
	    }
	    public static implicit operator string(HttpMethod method)
	    {
	        return method.Value;
	    }
	    public static bool operator ==(HttpMethod left, HttpMethod right)
	    {
	        return Equals(left, right);
	    }
	    public static bool operator !=(HttpMethod left, HttpMethod right)
	    {
	        return !Equals(left, right);
	    }
	    #endregion
	}
	[DebuggerDisplay("{Value}")]
	public readonly struct HttpPath : IEquatable<HttpPath>
	{
		const int StackAllocationLimit = 128;
	#if NET8_0_OR_GREATER
		// The allowed characters in an HTTP Path.
		private static readonly SearchValues<char> characters = SearchValues.Create("!$&'()*+,-./0123456789:;=@ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz~");
	#else
	#endif
	    // HttpPath is only set internally on requestion creation.
	    public HttpPath(string value)
		{
	        ReadOnlySpan<char> span = value;
	        if (span.ContainsAny(characters))
	        {
	        }
			//if (value.Any(c => !characters.Contains(c)))
			//{
			//	ThrowUtility.InvalidHttpPath($"The following path contains an in invalid character: '{value}'.");
			//}
			this.Value = value;
		}
		public string Value { get; }
	    #region Methods
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
		public bool StartsWith(HttpPath other)
		{
			return false;
		}
	    public static HttpPath FromUriComponent(string uriComponent)
	    {
	        int num = uriComponent.IndexOf('%');
	        if (num == -1)
	        {
	            return new HttpPath(uriComponent);
	        }
	        Span<char> span = ((uriComponent.Length > 128) ? ((Span<char>)new char[uriComponent.Length]) : stackalloc char[128]);
	        Span<char> destination = span;
	        uriComponent.CopyTo(destination);
	        int num2 = UrlDecoder.DecodeInPlace(destination.Slice(num, uriComponent.Length - num));
	        destination = destination.Slice(0, num + num2);
	        return new HttpPath(destination.ToString());
	    }
	    #endregion
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
	    #region Operators
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
	    //private static ref HttpQueryValue GetValueRefOrNullRef(IHttpCookieCollection collection, HttpQueryKey key) 
	    //{
	    //    return ref dictionary.FindValue(key);
	    //}
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
	            ThrowHelper.ThrowArgumentNullException(nameof(value));
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
	    private static ReadOnlySpan<KeyValuePair<int, string>> _statusCodes => new KeyValuePair<int, string>[]
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
	            ThrowHelper.ThrowArgumentException($"The provided status code is invalid: '{statusCode}'");
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
	        for (int i = 0; i < _statusCodes.Length; i++)
	        {
	            var statusCode = _statusCodes[i];
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
	        for (int i = 0; i < _statusCodes.Length; i++)
	        {
	            if (_statusCodes[i].Key == statusCode)
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
	    Http20 = 2,
	    Http30 = 3
	}
	#endregion
	#region \Abstractions
	public interface IHttpConnection : IDisposable
	{
	    IAsyncEnumerable<IHttpContext> ReceiveAsync(CancellationToken cancellationToken = default);
	    IAsyncEnumerable<IHttpContext> SendAsync(IHttpContext context, CancellationToken cancellationToken = default);
	}
	public interface IHttpConnectionInfo
	{
	    int RemotePort { get; }
	    IPAddress RemoteIp { get; }
	    int LocalPort { get; }
	    IPAddress LocalIp { get; }
	}
	public interface IHttpContext : IAsyncDisposable
	{
	    HttpVersion Version { get; }
	    IHttpSession Session { get; }
	    IHttpRequest Request { get; }
	    IHttpResponse Response { get; }
	    IHttpConnectionInfo ConnectionInfo { get; }
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
	#region \Internal
	internal sealed class UrlDecoder
	{
	    private static ReadOnlySpan<sbyte> CharToHexLookup => new sbyte[256]
	    {
	        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
	        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
	        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
	        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
	        -1, -1, -1, -1, -1, -1, -1, -1, 0, 1,
	        2, 3, 4, 5, 6, 7, 8, 9, -1, -1,
	        -1, -1, -1, -1, -1, 10, 11, 12, 13, 14,
	        15, -1, -1, -1, -1, -1, -1, -1, -1, -1,
	        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
	        -1, -1, -1, -1, -1, -1, -1, 10, 11, 12,
	        13, 14, 15, -1, -1, -1, -1, -1, -1, -1,
	        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
	        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
	        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
	        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
	        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
	        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
	        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
	        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
	        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
	        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
	        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
	        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
	        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
	        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
	        -1, -1, -1, -1, -1, -1
	    };
	    public static int DecodeRequestLine(ReadOnlySpan<byte> source, Span<byte> destination, bool isFormEncoding)
	    {
	        if (destination.Length < source.Length)
	        {
	            throw new ArgumentException("Length of the destination byte span is less than the source.", "destination");
	        }
	        source.CopyTo(destination);
	        return DecodeInPlace(destination.Slice(0, source.Length), isFormEncoding);
	    }
	    public static int DecodeInPlace(Span<byte> buffer, bool isFormEncoding)
	    {
	        int num = 0;
	        int destinationIndex = 0;
	        while (num != buffer.Length)
	        {
	            if (buffer[num] == 43 && isFormEncoding)
	            {
	                buffer[num] = 32;
	            }
	            else if (buffer[num] == 37)
	            {
	                int sourceIndex = num;
	                if (!DecodeCore(ref sourceIndex, ref destinationIndex, buffer, isFormEncoding))
	                {
	                    Copy(num, sourceIndex, ref destinationIndex, buffer);
	                }
	                num = sourceIndex;
	            }
	            else
	            {
	                buffer[destinationIndex++] = buffer[num++];
	            }
	        }
	        return destinationIndex;
	    }
	    private static bool DecodeCore(ref int sourceIndex, ref int destinationIndex, Span<byte> buffer, bool isFormEncoding)
	    {
	        int num = UnescapePercentEncoding(ref sourceIndex, buffer, isFormEncoding);
	        if (num == -1)
	        {
	            return false;
	        }
	        if (num == 0)
	        {
	            throw new InvalidOperationException("The path contains null characters.");
	        }
	        if (num <= 127)
	        {
	            buffer[destinationIndex++] = (byte)num;
	            return true;
	        }
	        int num2 = 0;
	        int num3 = 0;
	        int num4 = 0;
	        int num5;
	        int num6;
	        int num7;
	        if ((num & 0xE0) == 192)
	        {
	            num5 = num & 0x1F;
	            num6 = 2;
	            num7 = 128;
	        }
	        else if ((num & 0xF0) == 224)
	        {
	            num5 = num & 0xF;
	            num6 = 3;
	            num7 = 2048;
	        }
	        else
	        {
	            if ((num & 0xF8) != 240)
	            {
	                return false;
	            }
	            num5 = num & 7;
	            num6 = 4;
	            num7 = 65536;
	        }
	        int num8 = num6 - 1;
	        while (num8 > 0)
	        {
	            if (sourceIndex == buffer.Length)
	            {
	                return false;
	            }
	            int scan = sourceIndex;
	            int num9 = UnescapePercentEncoding(ref scan, buffer, isFormEncoding);
	            if (num9 == -1)
	            {
	                return false;
	            }
	            if ((num9 & 0xC0) != 128)
	            {
	                return false;
	            }
	            num5 = (num5 << 6) | (num9 & 0x3F);
	            num8--;
	            if (num8 == 1 && num5 >= 864 && num5 <= 895)
	            {
	                return false;
	            }
	            if (num8 == 2 && num5 >= 272)
	            {
	                return false;
	            }
	            sourceIndex = scan;
	            if (num6 - num8 == 2)
	            {
	                num2 = num9;
	            }
	            else if (num6 - num8 == 3)
	            {
	                num3 = num9;
	            }
	            else if (num6 - num8 == 4)
	            {
	                num4 = num9;
	            }
	        }
	        if (num5 < num7)
	        {
	            return false;
	        }
	        if (num6 > 0)
	        {
	            buffer[destinationIndex++] = (byte)num;
	        }
	        if (num6 > 1)
	        {
	            buffer[destinationIndex++] = (byte)num2;
	        }
	        if (num6 > 2)
	        {
	            buffer[destinationIndex++] = (byte)num3;
	        }
	        if (num6 > 3)
	        {
	            buffer[destinationIndex++] = (byte)num4;
	        }
	        return true;
	    }
	    private static void Copy<T>(int begin, int end, ref int writer, Span<T> buffer)
	    {
	        while (begin != end)
	        {
	            buffer[writer++] = buffer[begin++];
	        }
	    }
	    private static int UnescapePercentEncoding(ref int scan, Span<byte> buffer, bool isFormEncoding)
	    {
	        if (buffer[scan++] != 37)
	        {
	            return -1;
	        }
	        int scan2 = scan;
	        int num = ReadHex(ref scan2, buffer);
	        if (num == -1)
	        {
	            return -1;
	        }
	        int num2 = ReadHex(ref scan2, buffer);
	        if (num2 == -1)
	        {
	            return -1;
	        }
	        if (SkipUnescape(num, num2, isFormEncoding))
	        {
	            return -1;
	        }
	        scan = scan2;
	        return (num << 4) + num2;
	    }
	    private static int ReadHex(ref int scan, Span<byte> buffer)
	    {
	        if (scan == buffer.Length)
	        {
	            return -1;
	        }
	        byte b = buffer[scan++];
	        if ((b < 48 || b > 57) && (b < 65 || b > 70) && (b < 97 || b > 102))
	        {
	            return -1;
	        }
	        if (b <= 57)
	        {
	            return b - 48;
	        }
	        if (b <= 70)
	        {
	            return b - 65 + 10;
	        }
	        return b - 97 + 10;
	    }
	    private static bool SkipUnescape(int value1, int value2, bool isFormEncoding)
	    {
	        if (isFormEncoding)
	        {
	            return false;
	        }
	        if (value1 == 2 && value2 == 15)
	        {
	            return true;
	        }
	        return false;
	    }
	    public static int DecodeRequestLine(ReadOnlySpan<char> source, Span<char> destination)
	    {
	        source.CopyTo(destination);
	        return DecodeInPlace(destination.Slice(0, source.Length));
	    }
	    public static int DecodeInPlace(Span<char> buffer)
	    {
	        int num = buffer.IndexOf('%');
	        if (num == -1)
	        {
	            return buffer.Length;
	        }
	        int num2 = num;
	        int destinationIndex = num;
	        while (num2 != buffer.Length)
	        {
	            if (buffer[num2] == '%')
	            {
	                int sourceIndex = num2;
	                if (!DecodeCore(ref sourceIndex, ref destinationIndex, buffer))
	                {
	                    Copy(num2, sourceIndex, ref destinationIndex, buffer);
	                }
	                num2 = sourceIndex;
	            }
	            else
	            {
	                buffer[destinationIndex++] = buffer[num2++];
	            }
	        }
	        return destinationIndex;
	    }
	    private static bool DecodeCore(ref int sourceIndex, ref int destinationIndex, Span<char> buffer)
	    {
	        int num = UnescapePercentEncoding(ref sourceIndex, buffer);
	        if (num == -1)
	        {
	            return false;
	        }
	        if (num == 0)
	        {
	            throw new InvalidOperationException("The path contains null characters.");
	        }
	        if (num <= 127)
	        {
	            buffer[destinationIndex++] = (char)num;
	            return true;
	        }
	        int num2;
	        int num3;
	        int num4;
	        if ((num & 0xE0) == 192)
	        {
	            num2 = num & 0x1F;
	            num3 = 2;
	            num4 = 128;
	        }
	        else if ((num & 0xF0) == 224)
	        {
	            num2 = num & 0xF;
	            num3 = 3;
	            num4 = 2048;
	        }
	        else
	        {
	            if ((num & 0xF8) != 240)
	            {
	                return false;
	            }
	            num2 = num & 7;
	            num3 = 4;
	            num4 = 65536;
	        }
	        int num5 = num3 - 1;
	        while (num5 > 0)
	        {
	            if (sourceIndex == buffer.Length)
	            {
	                return false;
	            }
	            int scan = sourceIndex;
	            int num6 = UnescapePercentEncoding(ref scan, buffer);
	            if (num6 == -1)
	            {
	                return false;
	            }
	            if ((num6 & 0xC0) != 128)
	            {
	                return false;
	            }
	            num2 = (num2 << 6) | (num6 & 0x3F);
	            num5--;
	            sourceIndex = scan;
	        }
	        if (num2 < num4)
	        {
	            return false;
	        }
	        if (!Rune.TryCreate(num2, out var result) || !result.TryEncodeToUtf16(buffer.Slice(destinationIndex), out var charsWritten))
	        {
	            return false;
	        }
	        destinationIndex += charsWritten;
	        return true;
	    }
	    private static int UnescapePercentEncoding(ref int scan, ReadOnlySpan<char> buffer)
	    {
	        if (buffer[scan++] != '%')
	        {
	            return -1;
	        }
	        int scan2 = scan;
	        int num = ReadHex(ref scan2, buffer);
	        int num2 = ReadHex(ref scan2, buffer);
	        int num3 = (num << 4) | num2;
	        if (num3 < 0 || num3 == 47)
	        {
	            return -1;
	        }
	        scan = scan2;
	        return num3;
	    }
	    private static int ReadHex(ref int scan, ReadOnlySpan<char> buffer)
	    {
	        int num = scan++;
	        if ((uint)num >= (uint)buffer.Length)
	        {
	            return -1;
	        }
	        return FromChar(buffer[num]);
	    }
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    private static int FromChar(int c)
	    {
	        if ((uint)c < (uint)CharToHexLookup.Length)
	        {
	            return CharToHexLookup[c];
	        }
	        return -1;
	    }
	}
	#endregion
	#region \Internal\Exceptions
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
	#region \Internal\Shared
	internal static partial class ThrowHelper
	{
	    public static void InvalidHttpPath(string message) => 
	        throw new HttpInvalidPathException(message);
	    internal static void InvalidHttpMethod(string method) => 
	        throw new HttpInvalidMethodException($"The provided method is invalid: '{method}'. A method can only contain alphanumeric characters.");
	}
	#endregion
	#region \obj\Debug\net9.0
	#endregion
	#region \Properties
	#endregion
	#region C:\Source\repos\assimalign\cohesion\libraries\Core\Assimalign.Cohesion.Core\src\Internal\Shared
	internal static partial class ThrowHelper
	{
	    #region Arguments
	    internal static T ThrowIfNull<T>(
	        [NotNull]T? argument,
	        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	    {
	        if (argument is null)
	        {
	            ThrowArgumentNullException(paramName);
	        }
	        return argument;
	    }
	    internal static string ThrowIfNullOrEmpty(
	        [NotNull] string? argument,
	        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	    {
	        if (string.IsNullOrEmpty(argument))
	        {
	            ThrowArgumentNullException(paramName);
	        }
	        return argument;
	    }
	    internal static T ThrowIfNullOrNone<T>(
	        [NotNull] T argument,
	        [CallerArgumentExpression(nameof(argument))] string? paramName = null) where T : IEnumerable
	    {
	        switch (argument)
	        {
	            case null:
	            case ICollection collection when collection.Count == 0:
	            case Array array when array.Length == 0:
	                ThrowArgumentNullException(paramName);
	                break;
	        }
	        return argument;
	    }
	    [DoesNotReturn]
	    internal static void ThrowArgumentNullException(
	        string? paramName)
	    {
	        throw new ArgumentNullException(paramName);
	    }
	    [DoesNotReturn]
	    internal static void ThrowArgumentNullException(
	        string paramName, 
	        string message)
	    {
	        throw new ArgumentNullException(paramName, message);
	    }
	    [DoesNotReturn]
	    internal static void ThrowArgumentException(string message)
	    {
	        throw new ArgumentException(message);
	    }
	    [DoesNotReturn]
	    internal static void ThrowArgumentException(string message, string paramName)
	    {
	        throw new ArgumentException(message, paramName);
	    }
	    [DoesNotReturn]
	    internal static void ThrowInvalidOperationException(string message)
	    {
	        throw new InvalidOperationException(message);
	    }
	    #endregion
	    #region Threading
	    [DoesNotReturn]
	    internal static void ThrowObjectDisposedException(string objectName)
	    {
	        throw new ObjectDisposedException(objectName);
	    }
	    [DoesNotReturn]
	    internal static void ThrowObjectDisposedException(string objectName, string message)
	    {
	        throw new ObjectDisposedException(objectName, message);
	    }
	    #endregion
	    #region IO
	    [DoesNotReturn]
	    internal static void ThrowEndOfStreamException(string message)
	    {
	        throw new EndOfStreamException(message);
	    }
	    #endregion
	    #region Json Serialization
	    [DoesNotReturn]
	    internal static void ThrowJsonException(string message)
	    {
	        throw new JsonException(message);
	    }
	    #endregion
	}
	#endregion
}
#endregion
