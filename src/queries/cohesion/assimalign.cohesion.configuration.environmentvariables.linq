<Query Kind="Program">
<Namespace>System</Namespace>
<Namespace>System.Collections</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>System.Threading.Tasks</Namespace>
<Namespace>Assimalign.Cohesion.Configuration</Namespace>
<Namespace>Assimalign.Cohesion.Configuration.Providers</Namespace>
<Namespace>System.Reflection</Namespace>
</Query>
#load ".\assimalign.cohesion.configuration"
#load ".\assimalign.cohesion.core"

void Main()
{

}

#region Assimalign.Cohesion.Configuration.EnvironmentVariables(net8.0)
namespace Assimalign.Cohesion.Configuration.Providers
{
	#region \
	    public class ConfigurationEnvironmentVariablesProvider : ConfigurationProvider
	    {
	        private const string MySqlServerPrefix = "MYSQLCONNSTR_";
	        private const string SqlAzureServerPrefix = "SQLAZURECONNSTR_";
	        private const string SqlServerPrefix = "SQLCONNSTR_";
	        private const string CustomPrefix = "CUSTOMCONNSTR_";
	        private readonly string _prefix;
	        public ConfigurationEnvironmentVariablesProvider() =>
	            _prefix = string.Empty;
	        public ConfigurationEnvironmentVariablesProvider(string prefix) =>
	            _prefix = prefix ?? string.Empty;
	        public override void Load() =>
	            Load(Environment.GetEnvironmentVariables());
	        public override string ToString()
	            => $"{GetType().Name} Prefix: '{_prefix}'";
	        internal void Load(IDictionary envVariables)
	        {
	            var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
	            IDictionaryEnumerator e = envVariables.GetEnumerator();
	            try
	            {
	                while (e.MoveNext())
	                {
	                    DictionaryEntry entry = e.Entry;
	                    string key = (string)entry.Key;
	                    string provider = null;
	                    string prefix;
	                    if (key.StartsWith(MySqlServerPrefix, StringComparison.OrdinalIgnoreCase))
	                    {
	                        prefix = MySqlServerPrefix;
	                        provider = "MySql.Data.MySqlClient";
	                    }
	                    else if (key.StartsWith(SqlAzureServerPrefix, StringComparison.OrdinalIgnoreCase))
	                    {
	                        prefix = SqlAzureServerPrefix;
	                        provider = "System.Data.SqlClient";
	                    }
	                    else if (key.StartsWith(SqlServerPrefix, StringComparison.OrdinalIgnoreCase))
	                    {
	                        prefix = SqlServerPrefix;
	                        provider = "System.Data.SqlClient";
	                    }
	                    else if (key.StartsWith(CustomPrefix, StringComparison.OrdinalIgnoreCase))
	                    {
	                        prefix = CustomPrefix;
	                    }
	                    else if (key.StartsWith(_prefix, StringComparison.OrdinalIgnoreCase))
	                    {
	                        // This prevents the prefix from being normalized.
	                        // We can also do a fast path branch, I guess? No point in reallocating if the prefix is empty.
	                        key = NormalizeKey(key.Substring(_prefix.Length));
	                        data[key] = entry.Value as string;
	                        continue;
	                    }
	                    else
	                    {
	                        continue;
	                    }
	                    // Add the key-value pair for connection string, and optionally provider name
	                    key = NormalizeKey(key.Substring(prefix.Length));
	                    AddIfPrefixed(data, $"ConnectionStrings:{key}", (string)entry.Value);
	                    if (provider != null)
	                    {
	                        AddIfPrefixed(data, $"ConnectionStrings:{key}_ProviderName", provider);
	                    }
	                }
	            }
	            finally
	            {
	                (e as IDisposable)?.Dispose();
	            }
	            Data = data;
	        }
	        private void AddIfPrefixed(Dictionary<string, string> data, string key, string value)
	        {
	            if (key.StartsWith(_prefix, StringComparison.OrdinalIgnoreCase))
	            {
	                key = key.Substring(_prefix.Length);
	                data[key] = value;
	            }
	        }
	        private static string NormalizeKey(string key) => key.Replace("__", ConfigurationPath.KeyDelimiter);
	    }
	    public class ConfigurationEnvironmentVariablesSource : IConfigurationSource
	    {
	        public string Prefix { get; set; }
	        public IConfigurationProvider Build(IConfigurationBuilder builder)
	        {
	            return new ConfigurationEnvironmentVariablesProvider(Prefix);
	        }
	    }
	#endregion
	#region \Extensions
	public static partial class ConfigurationBuilderExtensions
	{
	    #region Environment Variable Provider
	    // <summary>
	    public static IConfigurationBuilder AddEnvironmentVariables(this IConfigurationBuilder configurationBuilder)
	    {
	        configurationBuilder.Add(new ConfigurationEnvironmentVariablesSource());
	        return configurationBuilder;
	    }
	    public static IConfigurationBuilder AddEnvironmentVariables(
	        this IConfigurationBuilder configurationBuilder,
	        string prefix)
	    {
	        configurationBuilder.Add(new ConfigurationEnvironmentVariablesSource { Prefix = prefix });
	        return configurationBuilder;
	    }
	    public static IConfigurationBuilder AddEnvironmentVariables(this IConfigurationBuilder builder, Action<ConfigurationEnvironmentVariablesSource> configureSource)
	        => builder.Add(configureSource);
	    #endregion
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
}
#endregion
