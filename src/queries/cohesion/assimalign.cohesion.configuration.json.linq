<Query Kind="Program">
<Namespace>System</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>System.Diagnostics</Namespace>
<Namespace>System.IO</Namespace>
<Namespace>System.Text.Json</Namespace>
<Namespace>Assimalign.Cohesion.Configuration</Namespace>
<Namespace>Assimalign.Cohesion.Configuration.Providers</Namespace>
<Namespace>System.Reflection</Namespace>
</Query>
#load ".\assimalign.cohesion.configuration"
#load ".\assimalign.cohesion.core"

void Main()
{

}

#region Assimalign.Cohesion.Configuration.Json(net8.0)
namespace Assimalign.Cohesion.Configuration.Providers
{
	#region \
	public class ConfigurationJsonProvider : ConfigurationFileProvider
	{
	    public ConfigurationJsonProvider(ConfigurationJsonSource source) : base(source) { }
	    public override void Load(Stream stream)
	    {
	        try
	        {
	            Data = JsonConfigurationFileParser.Parse(stream);
	        }
	        catch (JsonException e)
	        {
	            throw new FormatException();// SR.Error_JSONParseError, e);
	        }
	    }
	    internal sealed class JsonConfigurationFileParser
	    {
	        private JsonConfigurationFileParser() { }
	        private readonly Dictionary<string, string> _data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
	        private readonly Stack<string> _paths = new Stack<string>();
	        public static IDictionary<string, string> Parse(Stream input)
	            => new JsonConfigurationFileParser().ParseStream(input);
	        private IDictionary<string, string> ParseStream(Stream input)
	        {
	            var jsonDocumentOptions = new JsonDocumentOptions
	            {
	                CommentHandling = JsonCommentHandling.Skip,
	                AllowTrailingCommas = true,
	            };
	            using (var reader = new StreamReader(input))
	            using (JsonDocument doc = JsonDocument.Parse(reader.ReadToEnd(), jsonDocumentOptions))
	            {
	                if (doc.RootElement.ValueKind != JsonValueKind.Object)
	                {
	                    throw new FormatException();// SR.Format(SR.Error_InvalidTopLevelJSONElement, doc.RootElement.ValueKind));
	                }
	                VisitElement(doc.RootElement);
	            }
	            return _data;
	        }
	        private void VisitElement(JsonElement element)
	        {
	            var isEmpty = true;
	            foreach (JsonProperty property in element.EnumerateObject())
	            {
	                isEmpty = false;
	                EnterContext(property.Name);
	                VisitValue(property.Value);
	                ExitContext();
	            }
	            if (isEmpty && _paths.Count > 0)
	            {
	                _data[_paths.Peek()] = null;
	            }
	        }
	        private void VisitValue(JsonElement value)
	        {
	            Debug.Assert(_paths.Count > 0);
	            switch (value.ValueKind)
	            {
	                case JsonValueKind.Object:
	                    VisitElement(value);
	                    break;
	                case JsonValueKind.Array:
	                    int index = 0;
	                    foreach (JsonElement arrayElement in value.EnumerateArray())
	                    {
	                        EnterContext(index.ToString());
	                        VisitValue(arrayElement);
	                        ExitContext();
	                        index++;
	                    }
	                    break;
	                case JsonValueKind.Number:
	                case JsonValueKind.String:
	                case JsonValueKind.True:
	                case JsonValueKind.False:
	                case JsonValueKind.Null:
	                    string key = _paths.Peek();
	                    if (_data.ContainsKey(key))
	                    {
	                        throw new FormatException();// SR.Format(SR.Error_KeyIsDuplicated, key));
	                    }
	                    _data[key] = value.ToString();
	                    break;
	                default:
	                    throw new FormatException();// SR.Format(SR.Error_UnsupportedJSONToken, value.ValueKind));
	            }
	        }
	        private void EnterContext(string context) =>
	            _paths.Push(_paths.Count > 0 ?
	                _paths.Peek() + ConfigurationPath.KeyDelimiter + context :
	                context);
	        private void ExitContext() => _paths.Pop();
	    }
	}
	    public class ConfigurationJsonSource : ConfigurationFileSource
	    {
	        public override IConfigurationProvider Build(IConfigurationBuilder builder)
	        {
	            EnsureDefaults(builder);
	            return new ConfigurationJsonProvider(this);
	        }
	    }
	public class ConfigurationJsonStreamProvider : StreamConfigurationProvider
	{
	    public ConfigurationJsonStreamProvider(ConfigurationJsonStreamSource source) : base(source) { }
	    public override void Load(Stream stream)
	    {
	        Data = ConfigurationJsonProvider.JsonConfigurationFileParser.Parse(stream);
	    }
	}
	    public class ConfigurationJsonStreamSource : StreamConfigurationSource
	    {
	        public override IConfigurationProvider Build(IConfigurationBuilder builder)
	            => new ConfigurationJsonStreamProvider(this);
	    }
	#endregion
	#region \Extensions
	public static partial class ConfigurationBuilderExtensions
	{
	    #region Json Provider 
	    public static IConfigurationBuilder AddJsonFile(this IConfigurationBuilder builder, string path)
	    {
	        return AddJsonFile(builder, provider: null, path: path, optional: false, reloadOnChange: false);
	    }
	    public static IConfigurationBuilder AddJsonFile(this IConfigurationBuilder builder, string path, bool optional)
	    {
	        return AddJsonFile(builder, provider: null, path: path, optional: optional, reloadOnChange: false);
	    }
	    public static IConfigurationBuilder AddJsonFile(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange)
	    {
	        return AddJsonFile(builder, provider: null, path: path, optional: optional, reloadOnChange: reloadOnChange);
	    }
	    public static IConfigurationBuilder AddJsonFile(this IConfigurationBuilder builder, IFileProvider provider, string path, bool optional, bool reloadOnChange)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        if (string.IsNullOrEmpty(path))
	        {
	            throw new ArgumentException();// SR.Error_InvalidFilePath, nameof(path));
	        }
	        return builder.AddJsonFile(s =>
	        {
	            s.FileProvider = provider;
	            s.Path = path;
	            s.Optional = optional;
	            s.ReloadOnChange = reloadOnChange;
	            s.ResolveFileProvider();
	        });
	    }
	    public static IConfigurationBuilder AddJsonFile(this IConfigurationBuilder builder, Action<ConfigurationJsonSource> configureSource)
	        => builder.Add(configureSource);
	    public static IConfigurationBuilder AddJsonStream(this IConfigurationBuilder builder, Stream stream)
	    {
	        if (builder == null)
	        {
	            throw new ArgumentNullException(nameof(builder));
	        }
	        return builder.Add<ConfigurationJsonStreamSource>(s => s.Stream = stream);
	    }
	    #endregion
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
}
#endregion
