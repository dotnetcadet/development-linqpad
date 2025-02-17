﻿<Query Kind="Program">
<Namespace>Assimalign.Extensions.Configuration.Properties</Namespace>
<Namespace>System</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>System.ComponentModel</Namespace>
<Namespace>System.Diagnostics.CodeAnalysis</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Reflection</Namespace>
</Query>
#load ".\assimalign.cohesion.configuration"
#load ".\assimalign.cohesion.core"

void Main()
{

}

#region Assimalign.Cohesion.Configuration.Binder(net8.0)
namespace Assimalign.Cohesion.Configuration
{
	#region \
	public static class ConfigurationBinder
	{
	    private const BindingFlags DeclaredOnlyLookup = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
	    private const string TrimmingWarningMessage = "In case the type is non-primitive, the trimmer cannot statically analyze the object's type so its members may be trimmed.";
	    private const string InstanceGetTypeTrimmingWarningMessage = "Cannot statically analyze the type of instance so its members may be trimmed";
	    private const string PropertyTrimmingWarningMessage = "Cannot statically analyze property.PropertyType so its members may be trimmed.";
	    [RequiresUnreferencedCode(TrimmingWarningMessage)]
	    public static T Get<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(this IConfiguration configuration)
	        => configuration.Get<T>(_ => { });
	    [RequiresUnreferencedCode(TrimmingWarningMessage)]
	    public static T Get<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(this IConfiguration configuration, Action<ConfigurationBinderOptions> configureOptions)
	    {
	        if (configuration == null)
	        {
	            throw new ArgumentNullException(nameof(configuration));
	        }
	        object result = configuration.Get(typeof(T), configureOptions);
	        if (result == null)
	        {
	            return default(T);
	        }
	        return (T)result;
	    }
	    [RequiresUnreferencedCode(TrimmingWarningMessage)]
	    public static object Get(this IConfiguration configuration, Type type)
	        => configuration.Get(type, _ => { });
	    [RequiresUnreferencedCode(TrimmingWarningMessage)]
	    public static object Get(
	        this IConfiguration configuration,
	        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
	        Type type,
	        Action<ConfigurationBinderOptions> configureOptions)
	    {
	        if (configuration == null)
	        {
	            throw new ArgumentNullException(nameof(configuration));
	        }
	        var options = new ConfigurationBinderOptions();
	        configureOptions?.Invoke(options);
	        return BindInstance(type, instance: null, config: configuration, options: options);
	    }
	    [RequiresUnreferencedCode(InstanceGetTypeTrimmingWarningMessage)]
	    public static void Bind(this IConfiguration configuration, string key, object instance)
	        => configuration.GetSection(key).Bind(instance);
	    [RequiresUnreferencedCode(InstanceGetTypeTrimmingWarningMessage)]
	    public static void Bind(this IConfiguration configuration, object instance)
	        => configuration.Bind(instance, o => { });
	    [RequiresUnreferencedCode(InstanceGetTypeTrimmingWarningMessage)]
	    public static void Bind(this IConfiguration configuration, object instance, Action<ConfigurationBinderOptions> configureOptions)
	    {
	        if (configuration == null)
	        {
	            throw new ArgumentNullException(nameof(configuration));
	        }
	        if (instance != null)
	        {
	            var options = new ConfigurationBinderOptions();
	            configureOptions?.Invoke(options);
	            BindInstance(instance.GetType(), instance, configuration, options);
	        }
	    }
	    [RequiresUnreferencedCode(TrimmingWarningMessage)]
	    public static T GetValue<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(this IConfiguration configuration, string key)
	    {
	        return GetValue(configuration, key, default(T));
	    }
	    [RequiresUnreferencedCode(TrimmingWarningMessage)]
	    public static T GetValue<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(this IConfiguration configuration, string key, T defaultValue)
	    {
	        return (T)GetValue(configuration, typeof(T), key, defaultValue);
	    }
	    [RequiresUnreferencedCode(TrimmingWarningMessage)]
	    public static object GetValue(
	        this IConfiguration configuration,
	        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
	        Type type,
	        string key)
	    {
	        return GetValue(configuration, type, key, defaultValue: null);
	    }
	    [RequiresUnreferencedCode(TrimmingWarningMessage)]
	    public static object GetValue(
	        this IConfiguration configuration,
	        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
	        Type type, string key,
	        object defaultValue)
	    {
	        IConfigurationSection section = configuration.GetSection(key);
	        string value = section.Value;
	        if (value != null)
	        {
	            return ConvertValue(type, value, section.Path);
	        }
	        return defaultValue;
	    }
	    [RequiresUnreferencedCode(PropertyTrimmingWarningMessage)]
	    private static void BindNonScalar(this IConfiguration configuration, object instance, ConfigurationBinderOptions options)
	    {
	        if (instance != null)
	        {
	            List<PropertyInfo> modelProperties = GetAllProperties(instance.GetType());
	            if (options.ErrorOnUnknownConfiguration)
	            {
	                HashSet<string> propertyNames = new(modelProperties.Select(mp => mp.Name),
	                    StringComparer.OrdinalIgnoreCase);
	                IEnumerable<IConfigurationSection> configurationSections = configuration.GetChildren();
	                List<string> missingPropertyNames = configurationSections
	                    .Where(cs => !propertyNames.Contains(cs.Key))
	                    .Select(mp => $"'{mp.Key}'")
	                    .ToList();
	                if (missingPropertyNames.Count > 0)
	                {
	                    throw new InvalidOperationException();//SR.Format(SR.Error_MissingConfig,
	                        //nameof(options.ErrorOnUnknownConfiguration), nameof(ConfigurationBinderOptions), instance.GetType(),
	                        //string.Join(", ", missingPropertyNames)));
	                }
	            }
	            foreach (PropertyInfo property in modelProperties)
	            {
	                BindProperty(property, instance, configuration, options);
	            }
	        }
	    }
	    [RequiresUnreferencedCode(PropertyTrimmingWarningMessage)]
	    private static void BindProperty(PropertyInfo property, object instance, IConfiguration config, ConfigurationBinderOptions options)
	    {
	        // We don't support set only, non public, or indexer properties
	        if (property.GetMethod == null ||
	            (!options.BindNonPublicProperties && !property.GetMethod.IsPublic) ||
	            property.GetMethod.GetParameters().Length > 0)
	        {
	            return;
	        }
	        object propertyValue = property.GetValue(instance);
	        bool hasSetter = property.SetMethod != null && (property.SetMethod.IsPublic || options.BindNonPublicProperties);
	        if (propertyValue == null && !hasSetter)
	        {
	            // Property doesn't have a value and we cannot set it so there is no
	            // point in going further down the graph
	            return;
	        }
	        propertyValue = GetPropertyValue(property, instance, config, options);
	        if (propertyValue != null && hasSetter)
	        {
	            property.SetValue(instance, propertyValue);
	        }
	    }
	    [RequiresUnreferencedCode("Cannot statically analyze what the element type is of the object collection in type so its members may be trimmed.")]
	    private static object BindToCollection(Type type, IConfiguration config, ConfigurationBinderOptions options)
	    {
	        Type genericType = typeof(List<>).MakeGenericType(type.GenericTypeArguments[0]);
	        object instance = Activator.CreateInstance(genericType);
	        BindCollection(instance, genericType, config, options);
	        return instance;
	    }
	    // Try to create an array/dictionary instance to back various collection interfaces
	    [RequiresUnreferencedCode("In case type is a Dictionary, cannot statically analyze what the element type is of the value objects in the dictionary so its members may be trimmed.")]
	    private static object AttemptBindToCollectionInterfaces(
	        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
	        Type type,
	        IConfiguration config, ConfigurationBinderOptions options)
	    {
	        if (!type.IsInterface)
	        {
	            return null;
	        }
	        Type collectionInterface = FindOpenGenericInterface(typeof(IReadOnlyList<>), type);
	        if (collectionInterface != null)
	        {
	            // IEnumerable<T> is guaranteed to have exactly one parameter
	            return BindToCollection(type, config, options);
	        }
	        collectionInterface = FindOpenGenericInterface(typeof(IReadOnlyDictionary<,>), type);
	        if (collectionInterface != null)
	        {
	            Type dictionaryType = typeof(Dictionary<,>).MakeGenericType(type.GenericTypeArguments[0], type.GenericTypeArguments[1]);
	            object instance = Activator.CreateInstance(dictionaryType);
	            BindDictionary(instance, dictionaryType, config, options);
	            return instance;
	        }
	        collectionInterface = FindOpenGenericInterface(typeof(IDictionary<,>), type);
	        if (collectionInterface != null)
	        {
	            object instance = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(type.GenericTypeArguments[0], type.GenericTypeArguments[1]));
	            BindDictionary(instance, collectionInterface, config, options);
	            return instance;
	        }
	        collectionInterface = FindOpenGenericInterface(typeof(IReadOnlyCollection<>), type);
	        if (collectionInterface != null)
	        {
	            // IReadOnlyCollection<T> is guaranteed to have exactly one parameter
	            return BindToCollection(type, config, options);
	        }
	        collectionInterface = FindOpenGenericInterface(typeof(ICollection<>), type);
	        if (collectionInterface != null)
	        {
	            // ICollection<T> is guaranteed to have exactly one parameter
	            return BindToCollection(type, config, options);
	        }
	        collectionInterface = FindOpenGenericInterface(typeof(IEnumerable<>), type);
	        if (collectionInterface != null)
	        {
	            // IEnumerable<T> is guaranteed to have exactly one parameter
	            return BindToCollection(type, config, options);
	        }
	        return null;
	    }
	    [RequiresUnreferencedCode(TrimmingWarningMessage)]
	    private static object BindInstance(
	        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
	        Type type,
	        object instance, IConfiguration config, ConfigurationBinderOptions options)
	    {
	        // if binding IConfigurationSection, break early
	        if (type == typeof(IConfigurationSection))
	        {
	            return config;
	        }
	        var section = config as IConfigurationSection;
	        string configValue = section?.Value;
	        object convertedValue;
	        Exception error;
	        if (configValue != null && TryConvertValue(type, configValue, section.Path, out convertedValue, out error))
	        {
	            if (error != null)
	            {
	                throw error;
	            }
	            // Leaf nodes are always reinitialized
	            return convertedValue;
	        }
	        if (config != null && config.GetChildren().Any())
	        {
	            // If we don't have an instance, try to create one
	            if (instance == null)
	            {
	                // We are already done if binding to a new collection instance worked
	                instance = AttemptBindToCollectionInterfaces(type, config, options);
	                if (instance != null)
	                {
	                    return instance;
	                }
	                instance = CreateInstance(type);
	            }
	            // See if its a Dictionary
	            Type collectionInterface = FindOpenGenericInterface(typeof(IDictionary<,>), type);
	            if (collectionInterface != null)
	            {
	                BindDictionary(instance, collectionInterface, config, options);
	            }
	            else if (type.IsArray)
	            {
	                instance = BindArray((Array)instance, config, options);
	            }
	            else
	            {
	                // See if its an ICollection
	                collectionInterface = FindOpenGenericInterface(typeof(ICollection<>), type);
	                if (collectionInterface != null)
	                {
	                    BindCollection(instance, collectionInterface, config, options);
	                }
	                // Something else
	                else
	                {
	                    BindNonScalar(config, instance, options);
	                }
	            }
	        }
	        return instance;
	    }
	    private static object CreateInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] Type type)
	    {
	        if (type.IsInterface || type.IsAbstract)
	        {
	            throw new InvalidOperationException();// SR.Format(SR.Error_CannotActivateAbstractOrInterface, type));
	        }
	        if (type.IsArray)
	        {
	            if (type.GetArrayRank() > 1)
	            {
	                throw new InvalidOperationException();// SR.Format(SR.Error_UnsupportedMultidimensionalArray, type));
	            }
	            return Array.CreateInstance(type.GetElementType(), 0);
	        }
	        if (!type.IsValueType)
	        {
	            bool hasDefaultConstructor = type.GetConstructors(DeclaredOnlyLookup).Any(ctor => ctor.IsPublic && ctor.GetParameters().Length == 0);
	            if (!hasDefaultConstructor)
	            {
	                throw new InvalidOperationException();// SR.Format(SR.Error_MissingParameterlessConstructor, type));
	            }
	        }
	        try
	        {
	            return Activator.CreateInstance(type);
	        }
	        catch (Exception ex)
	        {
	            throw new InvalidOperationException(Resources.ExceptionMessageFailedToActivate);// SR.Format(SR.Error_FailedToActivate, type), ex);
	        }
	    }
	    [RequiresUnreferencedCode("Cannot statically analyze what the element type is of the value objects in the dictionary so its members may be trimmed.")]
	    private static void BindDictionary(
	        object dictionary,
	        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)]
	        Type dictionaryType,
	        IConfiguration config, ConfigurationBinderOptions options)
	    {
	        // IDictionary<K,V> is guaranteed to have exactly two parameters
	        Type keyType = dictionaryType.GenericTypeArguments[0];
	        Type valueType = dictionaryType.GenericTypeArguments[1];
	        bool keyTypeIsEnum = keyType.IsEnum;
	        if (keyType != typeof(string) && !keyTypeIsEnum)
	        {
	            // We only support string and enum keys
	            return;
	        }
	        PropertyInfo setter = dictionaryType.GetProperty("Item", DeclaredOnlyLookup);
	        foreach (IConfigurationSection child in config.GetChildren())
	        {
	            object item = BindInstance(
	                type: valueType,
	                instance: null,
	                config: child,
	                options: options);
	            if (item != null)
	            {
	                if (keyType == typeof(string))
	                {
	                    string key = child.Key;
	                    setter.SetValue(dictionary, item, new object[] { key });
	                }
	                else if (keyTypeIsEnum)
	                {
	                    object key = Enum.Parse(keyType, child.Key);
	                    setter.SetValue(dictionary, item, new object[] { key });
	                }
	            }
	        }
	    }
	    [RequiresUnreferencedCode("Cannot statically analyze what the element type is of the object collection so its members may be trimmed.")]
	    private static void BindCollection(
	        object collection,
	        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
	        Type collectionType,
	        IConfiguration config, ConfigurationBinderOptions options)
	    {
	        // ICollection<T> is guaranteed to have exactly one parameter
	        Type itemType = collectionType.GenericTypeArguments[0];
	        MethodInfo addMethod = collectionType.GetMethod("Add", DeclaredOnlyLookup);
	        foreach (IConfigurationSection section in config.GetChildren())
	        {
	            try
	            {
	                object item = BindInstance(
	                    type: itemType,
	                    instance: null,
	                    config: section,
	                    options: options);
	                if (item != null)
	                {
	                    addMethod.Invoke(collection, new[] { item });
	                }
	            }
	            catch
	            {
	            }
	        }
	    }
	    [RequiresUnreferencedCode("Cannot statically analyze what the element type is of the Array so its members may be trimmed.")]
	    private static Array BindArray(Array source, IConfiguration config, ConfigurationBinderOptions options)
	    {
	        IConfigurationSection[] children = config.GetChildren().ToArray();
	        int arrayLength = source.Length;
	        Type elementType = source.GetType().GetElementType();
	        var newArray = Array.CreateInstance(elementType, arrayLength + children.Length);
	        // binding to array has to preserve already initialized arrays with values
	        if (arrayLength > 0)
	        {
	            Array.Copy(source, newArray, arrayLength);
	        }
	        for (int i = 0; i < children.Length; i++)
	        {
	            try
	            {
	                object item = BindInstance(
	                    type: elementType,
	                    instance: null,
	                    config: children[i],
	                    options: options);
	                if (item != null)
	                {
	                    newArray.SetValue(item, arrayLength + i);
	                }
	            }
	            catch
	            {
	            }
	        }
	        return newArray;
	    }
	    [RequiresUnreferencedCode(TrimmingWarningMessage)]
	    private static bool TryConvertValue(
	        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
	        Type type,
	        string value, string path, out object result, out Exception error)
	    {
	        error = null;
	        result = null;
	        if (type == typeof(object))
	        {
	            result = value;
	            return true;
	        }
	        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
	        {
	            if (string.IsNullOrEmpty(value))
	            {
	                return true;
	            }
	            return TryConvertValue(Nullable.GetUnderlyingType(type), value, path, out result, out error);
	        }
	        TypeConverter converter = TypeDescriptor.GetConverter(type);
	        if (converter.CanConvertFrom(typeof(string)))
	        {
	            try
	            {
	                result = converter.ConvertFromInvariantString(value);
	            }
	            catch (Exception ex)
	            {
	                error = new InvalidOperationException();// SR.Format(SR.Error_FailedBinding, path, type), ex);
	            }
	            return true;
	        }
	        if (type == typeof(byte[]))
	        {
	            try
	            {
	                result = Convert.FromBase64String(value);
	            }
	            catch (FormatException ex)
	            {
	                error = new InvalidOperationException();// SR.Format(SR.Error_FailedBinding, path, type), ex);
	            }
	            return true;
	        }
	        return false;
	    }
	    [RequiresUnreferencedCode(TrimmingWarningMessage)]
	    private static object ConvertValue(
	        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
	        Type type,
	        string value, string path)
	    {
	        object result;
	        Exception error;
	        TryConvertValue(type, value, path, out result, out error);
	        if (error != null)
	        {
	            throw error;
	        }
	        return result;
	    }
	    private static Type FindOpenGenericInterface(
	        Type expected,
	        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
	        Type actual)
	    {
	        if (actual.IsGenericType &&
	            actual.GetGenericTypeDefinition() == expected)
	        {
	            return actual;
	        }
	        Type[] interfaces = actual.GetInterfaces();
	        foreach (Type interfaceType in interfaces)
	        {
	            if (interfaceType.IsGenericType &&
	                interfaceType.GetGenericTypeDefinition() == expected)
	            {
	                return interfaceType;
	            }
	        }
	        return null;
	    }
	    private static List<PropertyInfo> GetAllProperties(
	        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
	        Type type)
	    {
	        var allProperties = new List<PropertyInfo>();
	        do
	        {
	            allProperties.AddRange(type.GetProperties(DeclaredOnlyLookup));
	            type = type.BaseType;
	        }
	        while (type != typeof(object));
	        return allProperties;
	    }
	    [RequiresUnreferencedCode(PropertyTrimmingWarningMessage)]
	    private static object GetPropertyValue(PropertyInfo property, object instance, IConfiguration config, ConfigurationBinderOptions options)
	    {
	        string propertyName = GetPropertyName(property);
	        return BindInstance(
	            property.PropertyType,
	            property.GetValue(instance),
	            config.GetSection(propertyName),
	            options);
	    }
	    private static string GetPropertyName(MemberInfo property)
	    {
	        if (property == null)
	        {
	            throw new ArgumentNullException(nameof(property));
	        }
	        // Check for a custom property name used for configuration key binding
	        foreach (var attributeData in property.GetCustomAttributesData())
	        {
	            if (attributeData.AttributeType != typeof(ConfigurationKeyNameAttribute))
	            {
	                continue;
	            }
	            // Ensure ConfigurationKeyName constructor signature matches expectations
	            if (attributeData.ConstructorArguments.Count != 1)
	            {
	                break;
	            }
	            // Assumes ConfigurationKeyName constructor first arg is the string key name
	            string name = attributeData
	                .ConstructorArguments[0]
	                .Value?
	                .ToString();
	            return !string.IsNullOrWhiteSpace(name) ? name : property.Name;
	        }
	        return property.Name;
	    }
	}
	public class ConfigurationBinderOptions
	{
	    public bool BindNonPublicProperties { get; set; }
	    public bool ErrorOnUnknownConfiguration { get; set; }
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
}
#endregion
