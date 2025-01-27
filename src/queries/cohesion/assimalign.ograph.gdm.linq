<Query Kind="Program">
<Namespace>System</Namespace>
<Namespace>System.Globalization</Namespace>
<Namespace>System.ComponentModel</Namespace>
<Namespace>System.Diagnostics</Namespace>
<Namespace>System.Diagnostics.CodeAnalysis</Namespace>
<Namespace>System.Text.RegularExpressions</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>Gdm</Namespace>
<Namespace>Gdm.Internal</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>System.Threading.Tasks</Namespace>
<Namespace>Assimalign.OGraph.Gdm.Internal</Namespace>
<Namespace>System.Linq.Expressions</Namespace>
<Namespace>System.Xml.Serialization</Namespace>
<Namespace>System.Xml</Namespace>
<Namespace>System.Text.Json</Namespace>
<Namespace>System.Collections</Namespace>
<Namespace>System.Runtime.CompilerServices</Namespace>
<Namespace>System.Text.Json.Serialization</Namespace>
<Namespace>System.IO</Namespace>
<Namespace>System.Xml.Schema</Namespace>
<Namespace>System.Reflection</Namespace>
<Namespace>Internal</Namespace>
<Namespace>Properties</Namespace>
<Namespace>System.Buffers</Namespace>
</Query>

void Main()
{

}

#region Assimalign.OGraph.Gdm(net8.0)
namespace Assimalign.OGraph.Gdm
{
	#region \
	public enum GdmBindingKind
	{
	    Query,
	    Command,
	    Event,
	    Resolver
	}
	public enum GdmBuilderStrategy
	{
	    Implicit,
	    Explicit
	}
	public enum GdmCardinality
	{
	    OneToOne,
	    OneToMany,
	}
	public enum GdmElementKind
	{
	    Model,
	    Graph,
	    Vertex,
	    Edge,
	    Type,
	    Property,
	    Key
	}
	public readonly struct GdmEnumValue
	{
	    GdmEnumValue(string name, object value)
	    {
	        Name = name;
	        Value = value;
	    }
	    public string Name { get; }
	    public object Value { get; }
	    /* 
	     * 
	     */
	    public static GdmEnumValue Create(string name, byte value) => new(name, value);
	    public static GdmEnumValue Create(string name, sbyte value) => new(name, value);
	    public static GdmEnumValue Create(string name, short value) => new(name, value);
	    public static GdmEnumValue Create(string name, ushort value) => new(name, value);
	    public static GdmEnumValue Create(string name, int value) => new(name, value);
	    public static GdmEnumValue Create(string name, uint value) => new(name, value);
	    public static GdmEnumValue Create(string name, long value) => new(name, value);
	    public static GdmEnumValue Create(string name, ulong value) => new(name, value);
	}
	[Flags]
	public enum GdmPropertyFlags
	{
	    None = 0,
	    ReadOnly,
	    Nullable,
	    Computed
	}
	public enum GdmTypeKind
	{
	    None,
	    Entity,
	    Collection,
	    Complex,
	    Scalar,
	    Enum
	}
	[DebuggerDisplay("{Value}")]
	[TypeConverter(typeof(LabelTypeConverter))]
	public readonly struct Label : 
	    IEquatable<Label>, 
	    IEqualityComparer<Label>,
	    IComparable<Label>
	{
	    // Allowed characters for name
	    private const string pattern = "^[a-zA-Z0-9_@-]+$";
	    public Label(string value)
	    {
	        if (string.IsNullOrEmpty(value))
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(value));
	        }
	        if (!Regex.IsMatch(value, pattern))
	        {
	            ThrowHelper.ThrowInvalidLabel(value);
	        }
	        Value = value;
	    }
	    public readonly string Value { get; }
	    public Label ToPascalCase()
	    {
	        return AsPascalCase(Value);
	    }
	    public Label ToCamalCase()
	    {
	        return AsCamalCase(Value);
	    }
	    public static Label AsCamalCase(string value)
	    {
	        return string.Create(value.Length, value, (chars, name) =>
	        {
	            name.CopyTo(chars);
	            for (int i = 0; i < chars.Length && (i != 1 || char.IsUpper(chars[i])); i++)
	            {
	                bool flag = i + 1 < chars.Length;
	                if (i > 0 && flag && !char.IsUpper(chars[i + 1]))
	                {
	                    if (chars[i + 1] == ' ')
	                    {
	                        chars[i] = char.ToLowerInvariant(chars[i]);
	                    }
	                    break;
	                }
	                chars[i] = char.ToLowerInvariant(chars[i]);
	            }
	        });
	    }
	    public static Label AsPascalCase(string value)
	    {
	        return string.Create(value.Length, value, (chars, name) =>
	        {
	            name.CopyTo(chars);
	            for (int i = 0; i < chars.Length && (i != 1 || char.IsUpper(chars[i])); i++)
	            {
	                bool flag = i + 1 < chars.Length;
	                if (i > 0 && flag && !char.IsUpper(chars[i + 1]))
	                {
	                    if (chars[i + 1] == ' ')
	                    {
	                        chars[i] = char.ToUpperInvariant(chars[i]);
	                    }
	                    break;
	                }
	                chars[i] = char.ToUpperInvariant(chars[i]);
	            }
	        });
	    }
	    public override string ToString()
	    {
	        return Value;
	    }
	    public override bool Equals(object? obj)
	    {
	        if (ReferenceEquals(null, obj))
	        {
	            return false;
	        }
	        return obj is Label other && Equals(other);
	    }
	    public override int GetHashCode()
	    {
	        // TODO: Need to revisit. Not sure if I want the HashCode for the name to be the same as the instance of the string.
	        return Value.GetHashCode();
	    }
	    public bool Equals(Label left, Label right)
	    {
	        return left.Equals(right);
	    }
	    public int GetHashCode([DisallowNull] Label name)
	    {
	        return name.GetHashCode();
	    }
	    public bool Equals(Label other)
	    {
	        return (Value, other.Value) switch
	        {
	            (null, null) => true,
	            (null, _) => false,
	            (_, null) => false,
	            (_, _) => Value.Equals(other.Value, StringComparison.Ordinal),
	        };
	    }
	    public int CompareTo(Label other)
	    {
	        return (Value, other.Value) switch
	        {
	            (null, null) => 0,
	            (null, _) => -1,
	            (_, null) => 1,
	            (_, _) => string.CompareOrdinal(Value, other.Value),
	        };
	    }
	    public static implicit operator Label(string value) => new Label(value);
	    public static implicit operator string(Label name) => name.Value;
	    public static bool operator ==(Label left, Label right) => left.Equals(right);
	    public static bool operator !=(Label left, Label right) => !left.Equals(right);
	    public static bool operator <(Label left, Label right) => left.CompareTo(right) < 0;
	    public static bool operator >(Label left, Label right) => left.CompareTo(right) > 0;
	    public static bool operator <=(Label left, Label right) => left.CompareTo(right) <= 0;
	    public static bool operator >=(Label left, Label right) => left.CompareTo(right) >= 0;
	    public static bool IsValid(string name) => Regex.IsMatch(name, pattern);
	    internal partial class LabelTypeConverter : TypeConverter
	    {
	        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
	        {
	            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
	        }
	        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
	        {
	            if (value is string stringValue)
	            {
	                return new Label(stringValue);
	            }
	            return base.ConvertFrom(context, culture, value);
	        }
	        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? sourceType)
	        {
	            return sourceType == typeof(string) || base.CanConvertTo(context, sourceType);
	        }
	        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
	        {
	            if (value is Label idValue)
	            {
	                if (destinationType == typeof(string))
	                {
	                    return idValue.Value;
	                }
	            }
	            return base.ConvertTo(context, culture, value, destinationType);
	        }
	    }
	}
	public readonly struct MetaKey
	{
	    public MetaKey(string value)
	    {
	        Value = value;
	    }
	    public MetaKey(string value, string decorator) : this(value)
	    {
	        Decorator = decorator;
	    }
	    public string Value { get; }
	    public string? Decorator { get; }
	}
	public sealed class OGraphGdmBuilder : IOGraphGdmBuilder
	{
	    private readonly Gdm model;
	    private readonly GdmValidator validator;
	    private readonly IList<Action<Gdm>> beforeBuild;
	    private readonly IList<Action<Gdm>> onBuild;
	    private readonly IList<Action<Gdm>> afterBuild;
	    OGraphGdmBuilder(Label label)
	    {
	        this.beforeBuild = new List<Action<Gdm>>();
	        this.onBuild = new List<Action<Gdm>>();
	        this.afterBuild = new List<Action<Gdm>>();
	        this.validator = new();
	        this.model = new()
	        {
	            Label = label
	        };
	        afterBuild.Add(TryResolveTypeReferences);
	    }
	    IOGraphGdmBuilder IOGraphGdmBuilder.BeforeBuild(Action<IOGraphGdm> configure)
	    {
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        beforeBuild.Add(configure);
	        return this;
	    }
	    IOGraphGdmBuilder IOGraphGdmBuilder.AfterBuild(Action<IOGraphGdm> configure)
	    {
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        afterBuild.Add(configure);
	        return this;
	    }
	    IOGraphGdmBuilder IOGraphGdmBuilder.AddType<TGdmType>()
	    {
	        return (this as IOGraphGdmBuilder).AddType(new TGdmType());
	    }
	    IOGraphGdmBuilder IOGraphGdmBuilder.AddType(IOGraphGdmType type)
	    {
	        if (type is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(type));
	        }
	        var elements = model.Elements;
	        var hasType = elements.OfType<IOGraphGdmType>().Any(p => p.Label == type.Label);
	        if (!hasType)
	        {
	            model.Elements.Add(type);
	            //switch (type)
	            //{
	            //    case IOGraphGdmComplexType complex:
	            //        {
	            //            break;
	            //        }
	            //    case IOGraphGdmCollectionType collection:
	            //        {
	            //            break;
	            //        }
	            //}
	            if (type is IOGraphGdmComplexType complex) // This accounts for GDM Entity Types as well
	            {
	                var props = new GdmPropertyCollection();
	                var properties = complex.Properties;
	                foreach (var property in properties)
	                {
	                    var prop = GdmProperty.Wrap(property);
	                    var propertyType = prop?.Type?.Definition;
	                    if (propertyType is null)
	                    {
	                        props.Add(prop);
	                        // If the property type is null this could be a result of a shared type that is configured post build.
	                        continue;
	                    }
	                    var propType = elements.OfType<IOGraphGdmType>().SingleOrDefault(p => p.Label == propertyType.Label);
	                    if (propType is null)
	                    {
	                        (this as IOGraphGdmBuilder).AddType(propertyType);
	                    }
	                    else
	                    {
	                        prop.Type = new GdmTypeReference()
	                        {
	                            Definition = propType
	                        };
	                    }
	                    props.Add(prop);
	                }
	                complex.Properties.Clear();
	                foreach (var item in props)
	                {
	                    complex.Properties.Add(item);
	                    model.Elements.Add(item);
	                }
	            }
	            if (type is IOGraphGdmCollectionType collection)
	            {
	            }
	        }
	        return this;
	    }
	    IOGraphGdmBuilder IOGraphGdmBuilder.AddVertex<TVertex>()
	    {
	        return (this as IOGraphGdmBuilder).AddVertex(new TVertex());
	    }
	    IOGraphGdmBuilder IOGraphGdmBuilder.AddVertex(IOGraphGdmVertex vertex)
	    {
	        if (vertex is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(vertex));
	        }
	        var hasVertex = model.Elements.OfType<IOGraphGdmVertex>().Any(p=>p.Label == vertex.Label);
	        if (hasVertex)
	        {
	            throw new Exception(); // Duplicate vertex
	        }
	        var type = vertex.Type.Definition;
	        if (type is null)
	        {
	            throw new Exception();
	        }
	        (this as IOGraphGdmBuilder).AddType(type);
	        model.Elements.Add(vertex);
	        return this;
	    }
	    IOGraphGdmBuilder IOGraphGdmBuilder.AddEdge<TSource, TTarget>(Label label)
	    {
	        throw new NotImplementedException();
	    }
	    IOGraphGdm IOGraphGdmBuilder.Build()
	    {
	        OnBuild(beforeBuild);
	        OnBuild(afterBuild);
	        var result = validator.Validate(model);
	        if (!result.IsValid)
	        {
	            throw result.ToException();
	        }
	        //(model.Elements as GdmElementCollection)!.IsReadOnly = true;
	        return model;
	    }
	    private void OnBuild(IList<Action<Gdm>> actions)
	    {
	        foreach (var action in actions)
	        {
	            action.Invoke(model);
	        }
	    }
	    #region Static Memebers
	    public static IOGraphGdm Create(Label label, Action<IOGraphGdmBuilder> configure)
	    {
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        IOGraphGdmBuilder builder = new OGraphGdmBuilder(label);
	        configure.Invoke(builder);
	        return builder.Build();
	    }
	    public static IOGraphGdmBuilder Create(Label label)
	    {
	        return new OGraphGdmBuilder(label);
	    }
	    #endregion
	    private static void TryResolveTypeReferences(Gdm model)
	    {
	        foreach (var property in model.GetGdmProperties().Where(p => p is GdmProperty).Cast<GdmProperty>())
	        {
	            // 1. Check if type has been set
	            if (property.Type is null)
	            {
	                // 2. Find the first assignable to runtime property, if any.
	                var gdmType = model.GetGdmTypes().FirstOrDefault(p =>
	                    p.RuntimeType!.IsAssignableTo(property.PropertyInfo.PropertyType));
	                if (gdmType is not null)
	                {
	                    property.Type = new GdmTypeReference()
	                    {
	                        Definition = gdmType
	                    };
	                }
	                else
	                {
	                }
	            }
	        }
	    }
	}
	public sealed class OGraphGdmBuilderOptions
	{
	    public bool GenerateEntityCollectionTypes { get; set; } = true;
	    public bool ConvertAllLabelsToCamalCase { get; set; }
	}
	public sealed class OGraphGdmFactoryBuilder : IOGraphGdmFactoryBuilder
	{
	    private GdmBuilderStrategy strategy = GdmBuilderStrategy.Explicit;
	    private readonly IList<IOGraphGdm> models;
	    private readonly IDictionary<Label, IList<Action<IOGraphGdmBuilder>>> actions;
	    public OGraphGdmFactoryBuilder()
    {
        models = new List<IOGraphGdm>();
        actions = new Dictionary<Label, IList<Action<IOGraphGdmBuilder>>>();
    }
	    public IOGraphGdmFactoryBuilder UseStrategy(GdmBuilderStrategy strategy)
	    {
	        this.strategy = strategy;
	        return this;
	    }
	    public IOGraphGdmFactoryBuilder Configure(Label label, Action<IOGraphGdmBuilder> configure)
	    {
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        if (actions.TryGetValue(label, out var builds))
	        {
	            builds.Add(configure);
	        }
	        else
	        {
	            actions[label] = new List<Action<IOGraphGdmBuilder>>() { configure };
	        }
	        return this;
	    }
	    public IOGraphGdmFactory Build()
	    {
	        foreach (var (key, value) in actions)
	        {
	            var builder = OGraphGdmBuilder.Create(key);
	            foreach (var action in value)
	            {
	                action.Invoke(builder);
	            }
	            models.Add(builder.Build());
	        }
	        return new GdmFactory(models);
	    }
	}
	[TypeConverter(typeof(PropertyNameTypeConverter))]
	public readonly struct PropertyName : 
	    IComparable<PropertyName>, 
	    IEquatable<PropertyName>
	{
	    public PropertyName(string value)
	    {
	        if (string.IsNullOrEmpty(value))
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(value));
	        }
	        Value = value;
	    }
	    public string Value { get; }
	    #region Overloads
	    public override bool Equals(object? obj)
	    {
	        if (ReferenceEquals(null, obj))
	        {
	            return false;
	        }
	        return obj is PropertyName other && Equals(other);
	    }
	    public override int GetHashCode()
	    {
	        return Value.GetHashCode();
	    }
	    public override string ToString()
	    {
	        return Value;
	    }
	    #endregion
	    #region Operators
	    public static bool operator ==(PropertyName a, PropertyName b) => a.Equals(b);
	    public static bool operator !=(PropertyName a, PropertyName b) => !(a == b);
	    public static bool operator >(PropertyName a, PropertyName b) => a.CompareTo(b) > 0;
	    public static bool operator <(PropertyName a, PropertyName b) => a.CompareTo(b) < 0;
	    public static bool operator >=(PropertyName a, PropertyName b) => a.CompareTo(b) >= 0;
	    public static bool operator <=(PropertyName a, PropertyName b) => a.CompareTo(b) <= 0;
	    public static implicit operator PropertyName(string value) => new PropertyName(value);
	    #endregion
	    public bool Equals(PropertyName other)
	    {
	        return (Value, other.Value) switch
	        {
	            (null, null) => true,
	            (null, _) => false,
	            (_, null) => false,
	            (_, _) => Value.Equals(other.Value, StringComparison.Ordinal),
	        };
	    }
	    public int CompareTo(PropertyName other)
	    {
	        return (Value, other.Value) switch
	        {
	            (null, null) => 0,
	            (null, _) => -1,
	            (_, null) => 1,
	            (_, _) => string.CompareOrdinal(Value, other.Value),
	        };
	    }
	    internal partial class PropertyNameTypeConverter : TypeConverter
	    {
	        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
	        {
	            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
	        }
	        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
	        {
	            if (value is string stringValue)
	            {
	                return new PropertyName(stringValue);
	            }
	            return base.ConvertFrom(context, culture, value);
	        }
	        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? sourceType)
	        {
	            return sourceType == typeof(string) || base.CanConvertTo(context, sourceType);
	        }
	        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value,Type destinationType)
	        {
	            if (value is PropertyName idValue)
	            {
	                if (destinationType == typeof(string))
	                {
	                    return idValue.Value;
	                }
	            }
	            return base.ConvertTo(context, culture, value, destinationType);
	        }
	    }
	}
	#endregion
	#region \Abstractions
	public interface IOGraphGdm : IOGraphGdmBindingElement
	{
	    IOGraphGdmElementCollection Elements { get; }
	}
	public interface IOGraphGdmCommandBinding : IOGraphGdmBinding
	{
	    IOGraphGdmTypeReference ReturnType { get; }
	    IEnumerable<IOGraphGdmBindingParameter> Parameters { get; }
	}
	public interface IOGraphGdmBinding
	{
	    Label Label { get; }
	    GdmBindingKind Kind { get; }
	    IOGraphGdmMetadata Metadata { get; }
	}
	public interface IOGraphGdmQueryBinding : IOGraphGdmBinding
	{
	    IOGraphGdmTypeReference ReturnType { get; }
	    IEnumerable<IOGraphGdmBindingParameter> Parameters { get; }
	}
	public interface IOGraphGdmBindingElement : IOGraphGdmLabeledElement
	{
	    IEnumerable<IOGraphGdmBinding> Bindings { get; }
	    void Bind(IOGraphGdmBinding binding);
	    void Unbind(IOGraphGdmBinding binding);
	}
	public interface IOGraphGdmBindingParameter
	{
	    string Name { get; }    
	    IOGraphGdmTypeReference Type { get; }
	}
	public interface IOGraphGdmBuilder
	{
	    IOGraphGdmBuilder BeforeBuild(Action<IOGraphGdm> configure);
	    IOGraphGdmBuilder AfterBuild(Action<IOGraphGdm> configure);
	    IOGraphGdmBuilder AddType<TGdmType>() where TGdmType : class, IOGraphGdmType, new();
	    IOGraphGdmBuilder AddType(IOGraphGdmType type);
	    IOGraphGdmBuilder AddVertex<TGdmVertex>() where TGdmVertex : IOGraphGdmVertex, new();
	    IOGraphGdmBuilder AddVertex(IOGraphGdmVertex vertex);
	    IOGraphGdmBuilder AddEdge<TSource, TTarget>(Label label);
	    IOGraphGdm Build();
	}
	public interface IOGraphGdmEdge : IOGraphGdmBindingElement
	{
	    //Label - ! The Edge Label must match a literal segment of on operation on the target vertex. Operation Methods must not be mismatched
	    IOGraphGdmVertexReference Source { get; }
	    IOGraphGdmVertexReference Target { get; }
	    IOGraphGdmMetadata Metadata { get; }
	}
	public interface IOGraphGdmEdgeDescriptor
	{
	}
	public interface IOGraphGdmEdgeDescriptor<TSource, TTarget> 
	    where TSource : class, new()
	    where TTarget : class, new()
	{
	    IOGraphGdmEdgeDescriptor<TSource, TTarget> HasLabel(Label label);
	    IOGraphGdmEdgeDescriptor<TSource, TTarget> WithOne();
	    IOGraphGdmEdgeDescriptor<TSource, TTarget> WithMany();
	}
	public interface IOGraphGdmEdgeKeyDescriptor<T>
	{
	    void HasReferenceKey<TMember>(Expression<Func<T, TMember>> expression);
	}
	public interface IOGraphGdmEdgeReference
	{
	    IOGraphGdmEdge Definition { get; }
	}
	public interface IOGraphGdmEdgeReferenceCollection
	    : ICollection<IOGraphGdmEdgeReference>
	{
	}
	public interface IOGraphGdmElement
	{
	    GdmElementKind ElementKind { get; }
	}
	public interface IOGraphGdmElementCollection : ICollection<IOGraphGdmElement>
	{
	    TGdmElement Get<TGdmElement>(Label label) where TGdmElement : IOGraphGdmLabeledElement;
	}
	public interface IOGraphGdmEntityKey : IOGraphGdmElement
	{
	    IOGraphGdmPropertyReference Property { get; }
	}
	public interface IOGraphGdmFactory
	{
	    IOGraphGdm Create(Label label); 
	}
	public interface IOGraphGdmFactoryBuilder
	{
	    IOGraphGdmFactoryBuilder Configure(Label label, Action<IOGraphGdmBuilder> configure);
	    IOGraphGdmFactory Build();
	}
	public interface IOGraphGdmGraph : IOGraphGdmBindingElement
	{
	    IEnumerable<IOGraphGdmEdge> Edges { get; }
	    IEnumerable<IOGraphGdmVertex> Vertices { get; }
	    IOGraphGdmEdge GetEdge(Label label);
	    IOGraphGdmVertex GetVertex(Label label);
	}
	public interface IOGraphGdmLabeledElement : IOGraphGdmElement
	{
	    Label Label { get; }
	}
	public interface IOGraphGdmMetadata : IDictionary<Label, object>
	{
	}
	public interface IOGraphGdmMetadataValue : IXmlSerializable
	{
	}
	public interface IOGraphGdmProperty : IOGraphGdmBindingElement
	{
	    IOGraphGdmTypeReference Type { get; }
	    IOGraphGdmTypeReference DeclaringType { get; }
	    IOGraphGdmMetadata Metadata { get; }
	    GdmPropertyGetter Getter { get; }
	    GdmPropertySetter Setter { get; }
	    bool IsReadOnly { get; }
	    bool IsComputed { get; }
	    bool IsNullable { get; }
	}
	public interface IOGraphGdmPropertyCollection : ICollection<IOGraphGdmProperty>
	{
	    IOGraphGdmProperty this[Label name] { get; }
	}
	public interface IOGraphGdmPropertyDescriptor
	{
	    IOGraphGdmPropertyDescriptor UsePropertyName(Label label);
	    IOGraphGdmPropertyDescriptor UseType<TGdmType>() where TGdmType : IOGraphGdmType, new();
	    IOGraphGdmPropertyDescriptor UseType(IOGraphGdmType type);
	    //IOGraphGdmPropertyDescriptor UseTypeReference(Label label);
	    IOGraphGdmPropertyDescriptor UseMetadata(Label key, object value);
	    IOGraphGdmPropertyDescriptor IsComputed();
	    IOGraphGdmPropertyDescriptor IsRequired();
	    IOGraphGdmPropertyDescriptor UseGetter(GdmPropertyGetter getter);
	    IOGraphGdmPropertyDescriptor UseSetter(GdmPropertySetter setter);
	}
	public interface IOGraphGdmPropertyDescriptor<T>
	{
	    IOGraphGdmPropertyDescriptor<T> UsePropertyName(Label label);
	    IOGraphGdmPropertyDescriptor<T> UseType<TGdmType>() where TGdmType : IOGraphGdmType, new();
	    IOGraphGdmPropertyDescriptor<T> UseType(IOGraphGdmType type);
	    //IOGraphGdmPropertyDescriptor<T> UseTypeReference(Label label);
	    IOGraphGdmPropertyDescriptor<T> UseMetadata(Label key, object value);
	    IOGraphGdmPropertyDescriptor<T> IsComputed();
	    IOGraphGdmPropertyDescriptor<T> IsRequired();
	    IOGraphGdmPropertyDescriptor<T> UseGetter(GdmPropertyGetter getter);
	    IOGraphGdmPropertyDescriptor<T> UseSetter(GdmPropertySetter setter);
	}
	public interface IOGraphGdmPropertyReference
	{
	    IOGraphGdmProperty Definition { get; }
	}
	public interface IOGraphGdmCollectionType : IOGraphGdmType
	{
	    IOGraphGdmType ItemType { get; }
	}
	public interface IOGraphGdmComplexType : IOGraphGdmType
	{
	    IOGraphGdmPropertyCollection Properties { get; }
	}
	public interface IOGraphGdmType : IOGraphGdmLabeledElement
	{
	    GdmTypeKind Kind { get; }
	    Type RuntimeType { get; }
	    void Write(Utf8JsonWriter writer, object value);
	    void Write(XmlWriter writer, object value);
	    object Read(ref Utf8JsonReader reader);
	    object Read(XmlReader reader);
	}
	public interface IOGraphGdmEntityType : IOGraphGdmComplexType
	{
	    IOGraphGdmEntityKey Key { get; }
	}
	public interface IOGraphGdmEnumType : IOGraphGdmType
	{
	    public GdmEnumValue[] Values { get; }
	}
	public interface IOGraphGdmScalarType : IOGraphGdmType
	{
	    string?[]? Formats { get; }
	    object Parse(object? value);
	    bool TryParse(object? value, out object? result);
	}
	public interface IOGraphGdmComplexTypeDescriptor
	{
	    IOGraphGdmComplexTypeDescriptor HasLabel(Label label);
	    IOGraphGdmPropertyDescriptor HasProperty(Label name);
	}
	public interface IOGraphGdmComplexTypeDescriptor<T> 
	    where T : class, new()
	{
	    IOGraphGdmComplexTypeDescriptor<T> HasLabel(Label label);
	    IOGraphGdmPropertyDescriptor HasProperty(Label name);
	    IOGraphGdmPropertyDescriptor<TProperty> HasProperty<TProperty>(Expression<Func<T, TProperty>> expression);
	}
	public interface IOGraphGdmEntityTypeDescriptor
	{
	    IOGraphGdmEntityTypeDescriptor HasLabel(Label label);
	    IOGraphGdmEntityTypeDescriptor HasKey(Label label);
	    IOGraphGdmPropertyDescriptor HasProperty(Label label);
	}
	public interface IOGraphGdmEntityTypeDescriptor<T> 
	    where T : class, new()
	{
	    IOGraphGdmEntityTypeDescriptor<T> HasLabel(Label label);
	    IOGraphGdmEntityTypeDescriptor<T> HasKey(Label label);
	    IOGraphGdmEntityTypeDescriptor<T> HasKey<TKey>(Expression<Func<T, TKey>> expression) where TKey : struct;
	    IOGraphGdmEntityTypeDescriptor<T> HasKey<TKey>(Expression<Func<T, TKey?>> expression) where TKey : struct;
	    IOGraphGdmPropertyDescriptor HasProperty(Label label);
	    IOGraphGdmPropertyDescriptor<TProperty?> HasProperty<TProperty>(Expression<Func<T, TProperty?>> expression);
	}
	public interface IOGraphGdmTypeReference
	{
	    IOGraphGdmType Definition { get; }
	}
	public interface IOGraphGdmVertex : IOGraphGdmBindingElement
	{
	    IOGraphGdmTypeReference Type { get; } // TODO: Revisit whether IOGraphEntityType should be specified explicitly
	    IOGraphGdmEdgeReferenceCollection Edges { get; }
	    IOGraphGdmMetadata Metadata { get; }
	    //IOGraphGdmRoot Child { get; }
	}
	public interface IOGraphGdmVertexDescriptor
	{
	    IOGraphGdmVertexDescriptor HasLabel(Label label);
	    IOGraphGdmVertexDescriptor HasType(IOGraphGdmEntityType type);
	    IOGraphGdmVertexDescriptor HasType<TGdmType>() where TGdmType : IOGraphGdmEntityType, new();
	}
	public interface IOGraphGdmVertexDescriptor<T> 
	    where T : class, new()
	{
	    IOGraphGdmVertexDescriptor<T> HasLabel(Label label);
	    IOGraphGdmVertexDescriptor<T> HasType<TGdmType>() where TGdmType : IOGraphGdmEntityType, new();
	    IOGraphGdmVertexDescriptor<T> HasType(IOGraphGdmEntityType type);
	    IOGraphGdmVertexDescriptor<T> HasEdge<TTarget>(Label label) where TTarget : class, IOGraphGdmVertex, new();
	}
	public interface IOGraphGdmVertexReference
	{
	    IOGraphGdmVertex Definition { get; }
	}
	#endregion
	#region \Delegates
	public delegate object? GdmPropertyGetter(object instance);
	public delegate void GdmPropertySetter(object instance, object value);
	#endregion
	#region \Elements\Types
	[DebuggerDisplay("Type = {Label}")]
	public abstract class GdmType<T> : IOGraphGdmType
	{
	    internal Label label;
	    public GdmType()
    {
        var typeName = RuntimeType.Name;
        // Let's only override the label if it has valid characters
        if (Label.IsValid(typeName))
	        {
	            label = typeName;
	        }
    }
	    public Type RuntimeType => typeof(T);
	    public GdmElementKind ElementKind => GdmElementKind.Type;
	    public virtual Label Label => label;
	    public abstract GdmTypeKind Kind { get; }
	    public abstract T Read(ref Utf8JsonReader reader);
	    public abstract T Read(XmlReader reader);
	    public abstract void Write(Utf8JsonWriter writer, T value);
	    public abstract void Write(XmlWriter writer, T value);
	    object IOGraphGdmType.Read(ref Utf8JsonReader reader) => Read(ref reader)!;
	    object IOGraphGdmType.Read(XmlReader reader) => Read(reader)!;
	    void IOGraphGdmType.Write(Utf8JsonWriter writer, object value) => Write(writer, AssertType(value));
	    void IOGraphGdmType.Write(XmlWriter writer, object value) => Write(writer, AssertType(value));
	    private T AssertType(object value)
	    {
	        if (value is not T)
	        {
	            ThrowHelper.ThrowInvalidTypeSerializationException(typeof(T), value.GetType());
	        }
	        return (T)value;
	    }
	}
	#endregion
	#region \Elements\Types\Collections
	public class GdmArrayType<T> : GdmCollectionType<T[], T>
	{
	    public GdmArrayType(IOGraphGdmType itemType)
    {
        if (itemType is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(itemType));
	        }
	        if (!ItemType!.RuntimeType!.IsAssignableTo(typeof(T)))
	        {
	            ThrowHelper.ThrowArgumentException("");
	        }
	        ItemType = itemType;
	        if (Label.IsValid(ItemType!.RuntimeType!.Name))
	        {
	            label = ItemType!.RuntimeType!.Name + "Array";
	        }
    }
	    public override IOGraphGdmType ItemType { get; }
	    public override T[] Read(ref Utf8JsonReader reader)
	    {
	        var items = new T[4]; // use 4 as buffer
	        var count = 0;
	        Array.Resize<T>(ref items, count);
	        return items;
	    }
	    public override T[] Read(XmlReader reader)
	    {
	        throw new NotImplementedException();
	    }
	    public override void Write(Utf8JsonWriter writer, T[] value)
	    {
	        throw new NotImplementedException();
	    }
	    public override void Write(XmlWriter writer, T[] value)
	    {
	        throw new NotImplementedException();
	    }
	}
	public abstract class GdmCollectionType<TCollection, T> : GdmType<TCollection>, 
	    IOGraphGdmCollectionType
	    where TCollection : IEnumerable<T>
	{
	    public abstract IOGraphGdmType ItemType { get; }
	    public override GdmTypeKind Kind => GdmTypeKind.Collection;
	}
	public sealed class GdmDictionaryType<TKey, TValue> : GdmCollectionType<Dictionary<TKey, TValue>, KeyValuePair<TKey, TValue>>
	    where TKey : notnull
	{
	    public GdmDictionaryType(GdmType<TKey> key, GdmType<TValue> value)
    {
	        ItemType = new GdmKeyValueType<TKey, TValue>(key, value);
	    }
	    public override IOGraphGdmType ItemType { get; }
	    public override Dictionary<TKey, TValue> Read(ref Utf8JsonReader reader)
	    {
	        IDictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
	        var value = ((GdmKeyValueType<TKey, TValue>)ItemType).Read(ref reader);
	        dictionary.Add(value);
	        throw new NotImplementedException();
	    }
	    public override Dictionary<TKey, TValue> Read(XmlReader reader)
	    {
	        throw new NotImplementedException();
	    }
	    public override void Write(Utf8JsonWriter writer, Dictionary<TKey, TValue> value)
	    {
	        throw new NotImplementedException();
	    }
	    public override void Write(XmlWriter writer, Dictionary<TKey, TValue> value)
	    {
	        throw new NotImplementedException();
	    }
	}
	public class GdmHashSetType<T> : GdmCollectionType<HashSet<T>, T> 
	{
	    public override IOGraphGdmType ItemType => throw new NotImplementedException();
	    public override HashSet<T> Read(ref Utf8JsonReader reader)
	    {
	        throw new NotImplementedException();
	    }
	    public override HashSet<T> Read(XmlReader reader)
	    {
	        throw new NotImplementedException();
	    }
	    public override void Write(Utf8JsonWriter writer, HashSet<T> value)
	    {
	        throw new NotImplementedException();
	    }
	    public override void Write(XmlWriter writer, HashSet<T> value)
	    {
	        throw new NotImplementedException();
	    }
	}
	public class GdmListType<T> : GdmCollectionType<List<T>, T>
	{
	    public GdmListType(IOGraphGdmType itemType)
	    {
	        if (itemType is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(itemType));
	        }
	        if (!itemType!.RuntimeType!.IsAssignableTo(typeof(T)))
	        {
	            ThrowHelper.ThrowArgumentException("");
	        }
	        ItemType = itemType;
	        if (Label.IsValid(ItemType!.RuntimeType!.Name))
	        {
	            label = ItemType!.RuntimeType!.Name + "List";
	        }
	    }
	    #region Overloads
	    public override IOGraphGdmType ItemType { get; }
	    public override List<T> Read(ref Utf8JsonReader reader)
	    {
	        var list = new List<T>();
	        var startDepth = reader.CurrentDepth;
	        while (reader.Read() && reader.CurrentDepth > startDepth)
	        {
	            if (ItemType.Read(ref reader) is not T item) 
	            {
	                throw new Exception();
	            }
	            list.Add(item);
	        }
	        return list;
	    }
	    public override List<T> Read(XmlReader reader)
	    {
	        if (reader.NodeType != XmlNodeType.Element)
	        {
	            // TODO: throw invalid exception
	        }
	        if (reader.LocalName != Label)
	        {
	            // TODO: throw invalid exception
	        }
	        var list = new List<T>();
	        var startDepth = reader.Depth;
	        while (reader.Read() && reader.Depth > startDepth)
	        {
	            if (ItemType.Read(reader) is not T item)
	            {
	                throw new Exception();
	            }
	            list.Add(item);
	            if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == Label)
	            {
	                break;
	            }
	        }
	        return list;
	    }
	    public override void Write(Utf8JsonWriter writer, List<T> value)
	    {
	        writer.WriteStartArray();
	        foreach (var item in value)
	        {
	            ItemType.Write(writer, item!);
	        }
	        writer.WriteEndArray();
	    }
	    public override void Write(XmlWriter writer, List<T> value)
	    {
	        writer.WriteStartElement(Label);
	        foreach (var item in value)
	        {
	            ItemType.Write(writer, item!);
	        }
	        writer.WriteEndElement();
	    }
	    #endregion
	    #region Static Members
	    public static GdmListType<T> Create<TGdmType>() where TGdmType : GdmScalarType<T>, new()
	    {
	        return new GdmListType<T>(new TGdmType());
	    }
	    public static GdmListType<T> Create(IOGraphGdmType type)
	    {
	        return new(type);
	    }
	    #endregion
	}
	#endregion
	#region \Elements\Types\Complex
	public class GdmComplexType : IOGraphGdmComplexType
	{
	    private readonly Action<IOGraphGdmComplexTypeDescriptor> configure;
	    [DynamicallyAccessedMembers(
	        DynamicallyAccessedMemberTypes.PublicParameterlessConstructor |
	        DynamicallyAccessedMemberTypes.PublicProperties)]
	    internal Type runtimeType;
	    internal Label label;
	    public GdmComplexType([DynamicallyAccessedMembers(
	        DynamicallyAccessedMemberTypes.PublicParameterlessConstructor |
	        DynamicallyAccessedMemberTypes.PublicProperties)] Type type) : this(type, descriptor => { }) { }
	    GdmComplexType([DynamicallyAccessedMembers(
	        DynamicallyAccessedMemberTypes.PublicParameterlessConstructor |
	        DynamicallyAccessedMemberTypes.PublicProperties)] Type type, Action<IOGraphGdmComplexTypeDescriptor> configure)
	    {
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        if (type is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(type));
	        }
	        this.configure = configure;
	        this.runtimeType = type;
	        this.Configure(new GdmComplexTypeDescriptor(this));
	    }
	    protected virtual void Configure(IOGraphGdmComplexTypeDescriptor descriptor)
	    {
	        configure.Invoke(descriptor);
	    }
	    public Label Label => label;
	    public Type RuntimeType => runtimeType;
	    public GdmTypeKind Kind => GdmTypeKind.Complex;
	    public GdmElementKind ElementKind => GdmElementKind.Type;
	    public IOGraphGdmPropertyCollection Properties { get; } = new GdmPropertyCollection();
	    public virtual object Read(ref Utf8JsonReader reader)
	    {
	        if (reader.TokenType != JsonTokenType.StartObject)
	        {
	            // TODO: throw invalid operation
	        }
	        // capture the depth
	        var startDepth = reader.CurrentDepth;
	        var instance = Activator.CreateInstance(runtimeType)!;
	        while (reader.Read() || reader.TokenType == JsonTokenType.EndObject || startDepth <= reader.CurrentDepth)
	        {
	            if (reader.TokenType != JsonTokenType.PropertyName)
	            {
	                // TODO: throw invalid exception
	            }
	            var name = reader.GetString()!;
	            if (!reader.Read())
	            {
	                // TODO: throw invalid operation exception
	            }
	            if (this.TryGetProperty(name, out var property))
	            {
	                // TODO: throw invalid operation exception
	            }
	            if (property!.IsComputed)
	            {
	                // TODO: throw invalid operation. Cannot set computed value
	            }
	            if (!property.IsNullable && reader.TokenType == JsonTokenType.Null)
	            {
	                // TODO:throw invalid operation. Property is required.
	            }
	            var type = property!.Type.Definition;
	            var value = type.Read(ref reader);
	            var setter = property!.Setter;
	            setter.Invoke(instance, value);
	        }
	        return instance;
	    }
	    public virtual object Read(XmlReader reader)
	    {
	        if (reader.NodeType != XmlNodeType.Element)
	        {
	            // TODO: Throw invalid operation exception
	        }
	        if (reader.LocalName != Label)
	        {
	            // TODO: Throw invalid operation exception
	        }
	        var instance = Activator.CreateInstance(runtimeType)!;
	        var startDepth = reader.Depth;
	        while (reader.Read() || startDepth <= reader.Depth)
	        {
	            if (reader.NodeType != XmlNodeType.Element)
	            {
	                // TODO: Throw invalid operation exception
	            }
	            var propertyName = reader.LocalName;
	            if (!reader.Read())
	            {
	                // TODO: throw invalid operation exception
	            }
	            if (!this.TryGetProperty(propertyName, out var property))
	            {
	                // TODO: throw invalid operation exception
	            }
	            if (property!.IsComputed)
	            {
	                // TODO: throw invalid operation. Cannot set computed value
	            }
	            if (!property.IsNullable && reader.NodeType == XmlNodeType.Text)
	            {
	                // TODO:throw invalid operation. Property is required.
	            }
	            var type = property!.Type.Definition;
	            var value = type.Read(reader);
	            var setter = property!.Setter;
	            setter.Invoke(instance, value);
	        }
	        return instance;
	    }
	    public virtual void Write(Utf8JsonWriter writer, object value)
	    {
	        if (value is null)
	        {
	            writer.WriteNullValue();
	        }
	        else
	        {
	            var type = value.GetType();
	            if (!type.IsAssignableTo(RuntimeType))
	            {
	                throw new InvalidOperationException("");
	            }
	            writer.WriteStartObject();
	            foreach (var property in (this as IOGraphGdmComplexType).Properties)
	            {
	                var propertyName = property.Label;
	                var propertyType = property.Type.Definition;
	                var propertyValue = property.Getter.Invoke(value)!;
	                writer.WritePropertyName(propertyName);
	                propertyType.Write(writer, propertyValue);
	            }
	            writer.WriteEndObject();
	        }
	    }
	    public virtual void Write(XmlWriter writer, object value)
	    {
	        var type = value.GetType();
	        if (!type.IsAssignableTo(RuntimeType))
	        {
	        }
	        throw new NotImplementedException();
	    }
	    #region Overloads
	    public override string ToString()
	    {
	        return Label;
	    }
	    public override int GetHashCode()
	    {
	        return HashCode.Combine(Label, typeof(IOGraphGdmComplexType));
	    }
	    public override bool Equals(object? instance)
	    {
	        if (instance is not null)
	        {
	            return GetHashCode() == instance.GetHashCode();
	        }
	        return false;
	    }
	    #endregion
	    #region Static Members
	    public static GdmComplexType Create([DynamicallyAccessedMembers(
	        DynamicallyAccessedMemberTypes.PublicParameterlessConstructor |
	        DynamicallyAccessedMemberTypes.PublicProperties)]Type type, Action<IOGraphGdmComplexTypeDescriptor> configure)
	    {
	        return new GdmComplexType(type, configure);
	    }
	    #endregion
	}
	public class GdmComplexType<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T> : GdmType<T>,
	    IOGraphGdmComplexType
	    where T : class, new()
	{
	    private readonly Action<IOGraphGdmComplexTypeDescriptor<T>> configure;
	    public GdmComplexType() : this(descriptor => { }) { }
	    GdmComplexType(Action<IOGraphGdmComplexTypeDescriptor<T>> configure)
	    {
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        this.configure = configure;
	        Configure(new GdmComplexTypeDescriptor<T>(this));
	    }
	    public override GdmTypeKind Kind => GdmTypeKind.Complex;
	    public IOGraphGdmPropertyCollection Properties { get; } = new GdmPropertyCollection();
	    protected virtual void Configure(IOGraphGdmComplexTypeDescriptor<T> descriptor)
	    {
	        configure?.Invoke(descriptor);
	    }
	    public override T Read(ref Utf8JsonReader reader)
	    {
	        if (reader.TokenType != JsonTokenType.StartObject)
	        {
	            // TODO: throw invalid operation
	        }
	        // capture the depth
	        var startDepth = reader.CurrentDepth;
	        var instance = Activator.CreateInstance<T>();
	        while (reader.Read() || reader.TokenType == JsonTokenType.EndObject || startDepth <= reader.CurrentDepth)
	        {
	            if (reader.TokenType != JsonTokenType.PropertyName)
	            {
	                // TODO: throw invalid exception
	            }
	            var name = reader.GetString()!;
	            if (!reader.Read())
	            {
	                // TODO: throw invalid operation exception
	            }
	            if (this.TryGetProperty(name, out var property))
	            {
	                // TODO: throw invalid operation exception
	            }
	            if (property!.IsComputed)
	            {
	                // TODO: throw invalid operation. Cannot set computed value
	            }
	            if (!property.IsNullable && reader.TokenType == JsonTokenType.Null)
	            {
	                // TODO:throw invalid operation. Property is required.
	            }
	            var type = property!.Type.Definition;
	            var value = type.Read(ref reader);
	            var setter = property!.Setter;
	            setter.Invoke(instance, value);
	        }
	        return instance;
	    }
	    public override T Read(XmlReader reader)
	    {
	        if (reader.NodeType != XmlNodeType.Element)
	        {
	            // TODO: Throw invalid operation exception
	        }
	        if (reader.LocalName != Label)
	        {
	            // TODO: Throw invalid operation exception
	        }
	        var instance = Activator.CreateInstance<T>();
	        var startDepth = reader.Depth;
	        while (reader.Read() || startDepth <= reader.Depth)
	        {
	            if (reader.NodeType != XmlNodeType.Element)
	            {
	                // TODO: Throw invalid operation exception
	            }
	            var propertyName = reader.LocalName;
	            if (!reader.Read())
	            {
	                // TODO: throw invalid operation exception
	            }
	            if (!this.TryGetProperty(propertyName, out var property))
	            {
	                // TODO: throw invalid operation exception
	            }
	            if (property!.IsComputed)
	            {
	                // TODO: throw invalid operation. Cannot set computed value
	            }
	            if (!property.IsNullable && reader.NodeType == XmlNodeType.Text)
	            {
	                // TODO:throw invalid operation. Property is required.
	            }
	            var type = property!.Type.Definition;
	            var value = type.Read(reader);
	            var setter = property!.Setter;
	            setter.Invoke(instance, value);
	        }
	        return instance;
	    }
	    public override void Write(Utf8JsonWriter writer, T value)
	    {
	        if (value is null)
	        {
	            return;
	        }
	        writer.WriteStartObject();
	        foreach (var property in Properties)
	        {
	            var label = property.Label;
	            var type = property!.Type.Definition;
	            var getter = property!.Getter;
	            var propertyValue = getter.Invoke(value);
	            writer.WritePropertyName(Label);
	            if (!property.IsNullable && propertyValue is null)
	            {
	                // TODO: throw invalid operation exception
	            }
	            else if (propertyValue is null)
	            {
	                writer.WriteNullValue();
	            }
	            else
	            {
	                type.Write(writer, propertyValue);
	            }
	        }
	        writer.WriteEndObject();
	    }
	    public override void Write(XmlWriter writer, T value)
	    {
	        if (value is null)
	        {
	            return;
	        }
	        writer.WriteStartElement(Label);
	        foreach (var property in Properties)
	        {
	            var label = property.Label;
	            var type = property!.Type.Definition;
	            var getter = property!.Getter;
	            var propertyValue = getter.Invoke(value);
	            writer.WriteStartElement(label);
	            if (!property.IsNullable && propertyValue is null)
	            {
	                // TODO: throw invalid operation exception
	            }
	            else if (propertyValue is not null)
	            { 
	                type.Write(writer, propertyValue);
	            }
	            writer.WriteEndElement();
	        }
	        writer.WriteEndElement();
	    }
	    #region Overloads
	    public override string ToString()
	    {
	        return Label;
	    }
	    public override int GetHashCode()
	    {
	        return HashCode.Combine(Label, typeof(IOGraphGdmComplexType));
	    }
	    public override bool Equals(object? instance)
	    {
	        if (instance is not null)
	        {
	            return GetHashCode() == instance.GetHashCode();
	        }
	        return false;
	    }
	    #endregion
	    public static GdmComplexType<T> Create(Action<IOGraphGdmComplexTypeDescriptor<T>> configure)
	    {
	        return new GdmComplexType<T>(configure);
	    }
	}
	public sealed class GdmKeyValueType<TKey, TValue> : GdmComplexType<GdmKeyValuePair<TKey, TValue>>
	    where TKey : notnull
	{
	    public GdmKeyValueType(IOGraphGdmType keyType, IOGraphGdmType valueType) 
	    {
	        foreach (var property in Properties.Cast<GdmProperty>())
	        {
	            if (property.Label == "key")
	            {
	                property.Type = new GdmTypeReference()
	                {
	                    Definition = keyType
	                };
	            }
	            if (property.Label == "value")
	            {
	                property.Type = new GdmTypeReference()
	                {
	                    Definition = valueType
	                };
	            }
	        }
	    }
	    protected override void Configure(IOGraphGdmComplexTypeDescriptor<GdmKeyValuePair<TKey, TValue>> descriptor)
	    {
	        descriptor.HasProperty(p => p.Key).UsePropertyName("key");
	        descriptor.HasProperty(p => p.Value).UsePropertyName("value");
	    }
	    public override GdmKeyValuePair<TKey, TValue> Read(ref Utf8JsonReader reader)
	    {
	        var key = (TKey)Properties["key"].Type.Definition.Read(ref reader);
	        var value = (TValue)Properties["value"].Type.Definition.Read(ref reader);
	        return new GdmKeyValuePair<TKey, TValue> 
	        { 
	            Key = key, 
	            Value = value 
	        };
	    }
	    public override GdmKeyValuePair<TKey, TValue> Read(XmlReader reader)
	    {
	        return base.Read(reader);
	    }
	    public override void Write(Utf8JsonWriter writer, GdmKeyValuePair<TKey, TValue> value)
	    {
	        base.Write(writer, value);
	    }
	    public override void Write(XmlWriter writer, GdmKeyValuePair<TKey, TValue> value)
	    {
	        base.Write(writer, value);
	    }
	}
	public class GdmKeyValuePair<TKey, TValue>
	{
	    public TKey? Key { get; set; }
	    public TValue? Value { get; set; }
	    public static implicit operator KeyValuePair<TKey, TValue>(GdmKeyValuePair<TKey, TValue> valueType)
	    {
	        return new KeyValuePair<TKey, TValue>(valueType.Key!, valueType.Value!);
	    }
	}
	#endregion
	#region \Elements\Types\Entity
	public class GdmEntityType : IOGraphGdmEntityType
	{
	    private readonly Action<IOGraphGdmEntityTypeDescriptor> configure;
	    [DynamicallyAccessedMembers(
	        DynamicallyAccessedMemberTypes.PublicParameterlessConstructor |
	        DynamicallyAccessedMemberTypes.PublicProperties)]
	    internal Type runtimeType;
	    internal Label label;
	    internal IOGraphGdmEntityKey key;
	    public GdmEntityType([DynamicallyAccessedMembers(
	        DynamicallyAccessedMemberTypes.PublicParameterlessConstructor |
	        DynamicallyAccessedMemberTypes.PublicProperties)]Type type) 
	        : this(type, descriptor => { }) { }
	    GdmEntityType([DynamicallyAccessedMembers(
	        DynamicallyAccessedMemberTypes.PublicParameterlessConstructor |
	        DynamicallyAccessedMemberTypes.PublicProperties)]Type type, Action<IOGraphGdmEntityTypeDescriptor> configure)
	    {
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        if (type is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(type));
	        }
	        this.configure = configure;
	        this.runtimeType = type;
	        this.key = default!;
	        this.Configure(new GdmEntityTypeDescriptor(this));
	    }
	    public Label Label => label;
	    public Type RuntimeType => runtimeType;
	    public GdmTypeKind Kind => GdmTypeKind.Complex;
	    public GdmElementKind ElementKind => GdmElementKind.Type;
	    public IOGraphGdmPropertyCollection Properties { get; } = new GdmPropertyCollection();
	    public IOGraphGdmEntityKey Key => key;
	    protected virtual void Configure(IOGraphGdmEntityTypeDescriptor descriptor)
	    {
	        configure.Invoke(descriptor);
	    }
	    public object Read(ref Utf8JsonReader reader)
	    {
	        if (reader.TokenType != JsonTokenType.StartObject)
	        {
	            // TODO: throw invalid operation
	        }
	        // capture the depth
	        var startDepth = reader.CurrentDepth;
	        var instance = Activator.CreateInstance(runtimeType)!;
	        while (reader.Read() || reader.TokenType == JsonTokenType.EndObject || startDepth <= reader.CurrentDepth)
	        {
	            if (reader.TokenType != JsonTokenType.PropertyName)
	            {
	                // TODO: throw invalid exception
	            }
	            var name = reader.GetString()!;
	            if (!reader.Read())
	            {
	                // TODO: throw invalid operation exception
	            }
	            if (this.TryGetProperty(name, out var property))
	            {
	                // TODO: throw invalid operation exception
	            }
	            if (property!.IsComputed)
	            {
	                // TODO: throw invalid operation. Cannot set computed value
	            }
	            if (!property.IsNullable && reader.TokenType == JsonTokenType.Null)
	            {
	                // TODO:throw invalid operation. Property is required.
	            }
	            var type = property!.Type.Definition;
	            var value = type.Read(ref reader);
	            var setter = property!.Setter;
	            setter.Invoke(instance, value);
	        }
	        return instance;
	    }
	    public object Read(XmlReader reader)
	    {
	        if (reader.NodeType != XmlNodeType.Element)
	        {
	            // TODO: Throw invalid operation exception
	        }
	        if (reader.LocalName != Label)
	        {
	            // TODO: Throw invalid operation exception
	        }
	        var instance = Activator.CreateInstance(runtimeType)!;
	        var startDepth = reader.Depth;
	        while (reader.Read() || startDepth <= reader.Depth)
	        {
	            if (reader.NodeType != XmlNodeType.Element)
	            {
	                // TODO: Throw invalid operation exception
	            }
	            var propertyName = reader.LocalName;
	            if (!reader.Read())
	            {
	                // TODO: throw invalid operation exception
	            }
	            if (!this.TryGetProperty(propertyName, out var property))
	            {
	                // TODO: throw invalid operation exception
	            }
	            if (property!.IsComputed)
	            {
	                // TODO: throw invalid operation. Cannot set computed value
	            }
	            if (!property.IsNullable && reader.NodeType == XmlNodeType.Text)
	            {
	                // TODO:throw invalid operation. Property is required.
	            }
	            var type = property!.Type.Definition;
	            var value = type.Read(reader);
	            var setter = property!.Setter;
	            setter.Invoke(instance, value);
	        }
	        return instance;
	    }
	    public void Write(Utf8JsonWriter writer, object value)
	    {
	        throw new NotImplementedException();
	    }
	    public void Write(XmlWriter writer, object value)
	    {
	        throw new NotImplementedException();
	    }
	    public static GdmEntityType Create([DynamicallyAccessedMembers(
	        DynamicallyAccessedMemberTypes.PublicParameterlessConstructor |
	        DynamicallyAccessedMemberTypes.PublicProperties)] Type type, Action<IOGraphGdmEntityTypeDescriptor> descriptor)
	    {
	        return new GdmEntityType(type, descriptor);
	    }
	}
	public class GdmEntityType<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicProperties)] T> : GdmType<T>, IOGraphGdmEntityType
	    where T : class, new()
	{
	    internal IOGraphGdmEntityKey key = default!;
	    private readonly Action<IOGraphGdmEntityTypeDescriptor<T>> configure;
	    public GdmEntityType() : this(descriptor => { }) { }
	    GdmEntityType(Action<IOGraphGdmEntityTypeDescriptor<T>> configure)
	    {
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        this.configure = configure;
	        Configure(new GdmEntityTypeDescriptor<T>(this));
	    }
	    protected virtual void Configure(IOGraphGdmEntityTypeDescriptor<T> descriptor)
	    {
	        configure.Invoke(descriptor);
	    }
	    public override GdmTypeKind Kind => GdmTypeKind.Entity;
	    public IOGraphGdmPropertyCollection Properties { get; } = new GdmPropertyCollection();
	    public IOGraphGdmEntityKey Key => key;
	    #region Overloads
	    public override string ToString()
	    {
	        return Label;
	    }
	    public override int GetHashCode()
	    {
	        return HashCode.Combine(Label, typeof(IOGraphGdmEntityType));
	    }
	    public override bool Equals(object? instance)
	    {
	        if (instance is not null)
	        {
	            return GetHashCode() == instance.GetHashCode();
	        }
	        return false;
	    }
	    public override T Read(ref Utf8JsonReader reader)
	    {
	        if (reader.TokenType != JsonTokenType.StartObject)
	        {
	            // TODO: throw invalid operation
	        }
	        // capture the depth
	        var startDepth = reader.CurrentDepth;
	        var instance = Activator.CreateInstance<T>();
	        while (reader.Read() || reader.TokenType == JsonTokenType.EndObject || startDepth <= reader.CurrentDepth)
	        {
	            if (reader.TokenType != JsonTokenType.PropertyName)
	            {
	                // TODO: throw invalid exception
	            }
	            var name = reader.GetString()!;
	            if (!reader.Read())
	            {
	                // TODO: throw invalid operation exception
	            }
	            if (this.TryGetProperty(name, out var property))
	            {
	                // TODO: throw invalid operation exception
	            }
	            if (property!.IsComputed)
	            {
	                // TODO: throw invalid operation. Cannot set computed value
	            }
	            if (!property.IsNullable && reader.TokenType == JsonTokenType.Null)
	            {
	                // TODO:throw invalid operation. Property is required.
	            }
	            var type = property!.Type.Definition;
	            var value = type.Read(ref reader);
	            var setter = property!.Setter;
	            setter.Invoke(instance, value);
	        }
	        return instance;
	    }
	    public override T Read(XmlReader reader)
	    {
	        if (reader.NodeType != XmlNodeType.Element)
	        {
	            // TODO: Throw invalid operation exception
	        }
	        if (reader.LocalName != Label)
	        {
	            // TODO: Throw invalid operation exception
	        }
	        var instance = Activator.CreateInstance<T>();
	        var startDepth = reader.Depth;
	        while (reader.Read() || startDepth <= reader.Depth)
	        {
	            if (reader.NodeType != XmlNodeType.Element)
	            {
	                // TODO: Throw invalid operation exception
	            }
	            var propertyName = reader.LocalName;
	            if (!reader.Read())
	            {
	                // TODO: throw invalid operation exception
	            }
	            if (!this.TryGetProperty(propertyName, out var property))
	            {
	                // TODO: throw invalid operation exception
	            }
	            if (property!.IsComputed)
	            {
	                // TODO: throw invalid operation. Cannot set computed value
	            }
	            if (!property.IsNullable && reader.NodeType == XmlNodeType.Text)
	            {
	                // TODO:throw invalid operation. Property is required.
	            }
	            var type = property!.Type.Definition;
	            var value = type.Read(reader);
	            var setter = property!.Setter;
	            setter.Invoke(instance, value);
	        }
	        return instance;
	    }
	    public override void Write(Utf8JsonWriter writer, T value)
	    {
	        if (value is null)
	        {
	            return;
	        }
	        writer.WriteStartObject();
	        foreach (var property in Properties)
	        {
	            var label = property.Label;
	            var type = property!.Type.Definition;
	            var getter = property!.Getter;
	            var propertyValue = getter.Invoke(value);
	            writer.WritePropertyName(Label);
	            if (!property.IsNullable && propertyValue is null)
	            {
	                // TODO: throw invalid operation exception
	            }
	            else if (propertyValue is null)
	            {
	                writer.WriteNullValue();
	            }
	            else
	            {
	                type.Write(writer, propertyValue);
	            }
	        }
	        writer.WriteEndObject();
	    }
	    public override void Write(XmlWriter writer, T value)
	    {
	        if (value is null)
	        {
	            return;
	        }
	        writer.WriteStartElement(Label);
	        foreach (var property in Properties)
	        {
	            var label = property.Label;
	            var type = property!.Type.Definition;
	            var getter = property!.Getter;
	            var propertyValue = getter.Invoke(value);
	            writer.WriteStartElement(label);
	            if (!property.IsNullable && propertyValue is null)
	            {
	                // TODO: throw invalid operation exception
	            }
	            else if (propertyValue is not null)
	            {
	                type.Write(writer, propertyValue);
	            }
	            writer.WriteEndElement();
	        }
	        writer.WriteEndElement();
	    }
	    #endregion
	    #region Static Members/Methods
	    public static GdmEntityType<T> Create(Action<IOGraphGdmEntityTypeDescriptor<T>> configure) 
	    {
	        return new GdmEntityType<T>(configure);
	    }
	    #endregion
	}
	#endregion
	#region \Elements\Types\Enums
	public class GdmEnumType<TEnum> : GdmType<TEnum>
	    where TEnum : struct, Enum
	{
	    public GdmEnumType()
	    {
	        Values = GetEnumValues();
	    }
	    private GdmEnumValue[] GetEnumValues()
	    {
	        var enumValues = Enum.GetValues<TEnum>();
	        var enumConverter = GetConverter();
	        if (enumValues is null || enumValues.Length == 0)
	        {
	            throw new Exception();
	        }
	        var values = new GdmEnumValue[enumValues.Length];
	        for (int i = 0; i < enumValues.Length; i++)
	        {
	            values[i] = enumConverter(
	                Enum.GetName<TEnum>(enumValues[i])!,
	                enumValues[i]);
	        }
	        return values;
	    }
	    private Func<string, object, GdmEnumValue> GetConverter() => Enum.GetUnderlyingType(typeof(TEnum)).Name switch
	    {
	        nameof(Byte) => (string name, object value) => GdmEnumValue.Create(name, Convert.ToByte(value)),
	        nameof(SByte) => (string name, object value) => GdmEnumValue.Create(name, Convert.ToSByte(value)),
	        nameof(Int16) => (string name, object value) => GdmEnumValue.Create(name, Convert.ToInt16(value)),
	        nameof(UInt16) => (string name, object value) => GdmEnumValue.Create(name, Convert.ToUInt16(value)),
	        nameof(Int32) => (string name, object value) => GdmEnumValue.Create(name, Convert.ToInt32(value)),
	        nameof(UInt32) => (string name, object value) => GdmEnumValue.Create(name, Convert.ToUInt32(value)),
	        nameof(Int64) => (string name, object value) => GdmEnumValue.Create(name, Convert.ToInt64(value)),
	        nameof(UInt64) => (string name, object value) => GdmEnumValue.Create(name, Convert.ToUInt64(value)),
	        _ => throw new Exception()
	    };
	    public GdmEnumValue[] Values { get; }
	    public override GdmTypeKind Kind => GdmTypeKind.Enum;
	    public override unsafe TEnum Read(ref Utf8JsonReader reader)
	    {
	        if (reader.TokenType == JsonTokenType.String)
	        {
	            return Enum.Parse<TEnum>(reader.GetString()!);
	        }
	        if (reader.TokenType == JsonTokenType.Number)
	        {
	            switch (Type.GetTypeCode(RuntimeType))
	            {
	                case TypeCode.Int32:
	                    {
	                        if (reader.TryGetInt32(out var value8))
	                        {
	                            return Unsafe.As<int, TEnum>(ref value8);
	                        }
	                        break;
	                    }
	                case TypeCode.UInt32:
	                    {
	                        if (reader.TryGetUInt32(out var value4))
	                        {
	                            return Unsafe.As<uint, TEnum>(ref value4);
	                        }
	                        break;
	                    }
	                case TypeCode.UInt64:
	                    {
	                        if (reader.TryGetUInt64(out var value6))
	                        {
	                            return Unsafe.As<ulong, TEnum>(ref value6);
	                        }
	                        break;
	                    }
	                case TypeCode.Int64:
	                    {
	                        if (reader.TryGetInt64(out var value2))
	                        {
	                            return Unsafe.As<long, TEnum>(ref value2);
	                        }
	                        break;
	                    }
	                case TypeCode.SByte:
	                    {
	                        if (reader.TryGetSByte(out var value7))
	                        {
	                            return Unsafe.As<sbyte, TEnum>(ref value7);
	                        }
	                        break;
	                    }
	                case TypeCode.Byte:
	                    {
	                        if (reader.TryGetByte(out var value5))
	                        {
	                            return Unsafe.As<byte, TEnum>(ref value5);
	                        }
	                        break;
	                    }
	                case TypeCode.Int16:
	                    {
	                        if (reader.TryGetInt16(out var value3))
	                        {
	                            return Unsafe.As<short, TEnum>(ref value3);
	                        }
	                        break;
	                    }
	                case TypeCode.UInt16:
	                    {
	                        if (reader.TryGetUInt16(out var value))
	                        {
	                            return Unsafe.As<ushort, TEnum>(ref value);
	                        }
	                        break;
	                    }
	            }
	        }
	        throw new Exception();
	    }
	    public override TEnum Read(XmlReader reader)
	    {
	        throw new NotImplementedException();
	    }
	    public override void Write(Utf8JsonWriter writer, TEnum value)
	    {
	        throw new NotImplementedException();
	    }
	    public override void Write(XmlWriter writer, TEnum value)
	    {
	        throw new NotImplementedException();
	    }
	    private TEnum AssertType(object value)
	    {
	        if (value is not TEnum type)
	        {
	            throw new InvalidOperationException($"Could not write type {value.GetType().Name}. Expected {typeof(TEnum).Name}");
	        }
	        return type;
	    }
	}
	#endregion
	#region \Elements\Types\Scalar
	public sealed class GdmBooleanType : GdmScalarType<bool>
	{
	    public override bool Read(ref Utf8JsonReader reader)
	    {
	        return reader.GetBoolean();
	    }
	    public override bool Read(XmlReader reader)
	    {
	        return reader.ReadContentAsBoolean();
	    }
	    public override void Write(Utf8JsonWriter writer, bool value)
	    {
	        writer.WriteBooleanValue(value);
	    }
	    public override void Write(XmlWriter writer, bool value)
	    {
	        writer.WriteValue(value);
	    }
	    public override bool Parse(object? value)
	    {
	        if (value is not string str)
	        {
	            throw new ArgumentException("");
	        }
	        return bool.Parse(str);
	    }
	    public override bool TryParse(object? value, out bool result)
	    {
	        result = false;
	        if (value is not string str)
	        {
	            return false;
	        }
	        return bool.TryParse(str, out result);
	    }
	}
	public sealed class GdmByteType : GdmScalarType<byte>
	{
	    public override byte Read(ref Utf8JsonReader reader)
	    {
	        throw new System.NotImplementedException();
	    }
	    public override byte Read(XmlReader reader)
	    {
	        throw new System.NotImplementedException();
	    }
	    public override void Write(Utf8JsonWriter writer, byte value)
	    {
	        throw new System.NotImplementedException();
	    }
	    public override void Write(XmlWriter writer, byte value)
	    {
	        throw new System.NotImplementedException();
	    }
	}
	public sealed class GdmCharType : GdmScalarType<char>
	{
	    public override char Read(ref Utf8JsonReader reader)
	    {
	        var value = reader.GetString();
	        if (value is null || value.Length == 0 || value.Length > 1)
	        {
	            throw new JsonException("");
	        }
	        return value[0];
	    }
	    public override char Read(XmlReader reader)
	    {
	        return reader.ReadElementContentAsString()[0];
	    }
	    public override void Write(Utf8JsonWriter writer, char value)
	    {
	        //writer.WriteBooleanValue(value);
	    }
	    public override void Write(XmlWriter writer, char value)
	    {
	        //writer.WriteValue(value);
	    }
	}
	public sealed class GdmDateTimeOffsetType : GdmScalarType<DateTimeOffset>
	{
	    public override DateTimeOffset Read(ref Utf8JsonReader reader)
	    {
	        return reader.GetDateTimeOffset();
	    }
	    public override DateTimeOffset Read(XmlReader reader)
	    {
	        return reader.ReadContentAsDateTimeOffset();
	    }
	    public override void Write(Utf8JsonWriter writer, DateTimeOffset value)
	    {
	        writer.WriteStringValue(value);
	    }
	    public override void Write(XmlWriter writer, DateTimeOffset value)
	    {
	        writer.WriteValue(value);
	    }
	}
	public sealed class GdmDateTimeType : GdmScalarType<DateTime>
	{
	    //public override string[]? Formats => new[]
	    //{
	    //    "yyyy-MM-dd",
	    //    "yyyyMMdd",
	    //    "yyyy-MM-ddTHH:mm:ss",
	    //    "yyyy-MM-ddTHH:mm:ss.fffffffK",
	    //    "yyyy-MM-ddTHH:mm:ss.fffffffZ",
	    //    "yyyy-MM-ddTHH:mm:ss.fffffffzzz"
	    //};
	    public override DateTime Read(ref Utf8JsonReader reader)
	    {
	        return reader.GetDateTime();
	    }
	    public override DateTime Read(XmlReader reader)
	    {
	        return reader.ReadContentAsDateTime();
	    }
	    public override void Write(Utf8JsonWriter writer, DateTime value)
	    {
	        writer.WriteStringValue(value);
	    }
	    public override void Write(XmlWriter writer, DateTime value)
	    {
	        writer.WriteValue(value);
	    }
	}
	public sealed class GdmDateType : GdmScalarType<DateOnly>
	{
	    public GdmDateType() { }
	    public override Label Label => "Date";
	    public override DateOnly Read(ref Utf8JsonReader reader)
	    {
	        return DateOnly.Parse(reader.GetString()!);
	    }
	    public override DateOnly Read(XmlReader reader)
	    {
	        return DateOnly.Parse(reader.ReadContentAsString());
	    }
	    public override void Write(Utf8JsonWriter writer, DateOnly value)
	    {
	        writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
	    }
	    public override void Write(XmlWriter writer, DateOnly value)
	    {
	        writer.WriteValue(value.ToString("yyyy-MM-dd"));
	    }
	}
	public sealed class GdmDecimalType : GdmScalarType<Decimal>
	{
	    public override decimal Read(ref Utf8JsonReader reader)
	    {
	        return reader.GetDecimal();
	    }
	    public override decimal Read(XmlReader reader)
	    {
	        return reader.ReadContentAsDecimal();
	    }
	    public override void Write(Utf8JsonWriter writer, decimal value)
	    {
	        writer.WriteNumberValue(value);
	    }
	    public override void Write(XmlWriter writer, decimal value)
	    {
	        writer.WriteValue(value);
	    }
	}
	public sealed class GdmDoubleType : GdmScalarType<Double>
	{
	    public override double Read(ref Utf8JsonReader reader)
	    {
	        return reader.GetDouble();
	    }
	    public override double Read(XmlReader reader)
	    {
	        return reader.ReadContentAsDouble();
	    }
	    public override void Write(Utf8JsonWriter writer, double value)
	    {
	        writer.WriteNumberValue(value);
	    }
	    public override void Write(XmlWriter writer, double value)
	    {
	        writer.WriteValue(value);
	    }
	}
	public sealed class GdmFloatType : GdmScalarType<Single>
	{
	    public override float Read(ref Utf8JsonReader reader)
	    {
	        return reader.GetSingle();
	    }
	    public override float Read(XmlReader reader)
	    {
	        return reader.ReadContentAsFloat();
	    }
	    public override void Write(Utf8JsonWriter writer, float value)
	    {
	        writer.WriteNumberValue(value);
	    }
	    public override void Write(XmlWriter writer, float value)
	    {
	        writer.WriteValue(value);
	    }
	}
	public class GdmGuidType : GdmScalarType<Guid>
	{
	    public override string[]? Formats => Regex;
	    public override Guid Read(ref Utf8JsonReader reader)
	    {
	        return reader.GetGuid();
	    }
	    public override Guid Read(XmlReader reader)
	    {
	        return Guid.Parse(reader.ReadContentAsString());
	    }
	    public override void Write(Utf8JsonWriter writer, Guid value)
	    {
	        writer.WriteStringValue(value);
	    }
	    public override void Write(XmlWriter writer, Guid value)
	    {
	        writer.WriteValue(value.ToString());
	    }
	    public static string[] Regex => ["^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[1-5][0-9a-fA-F]{3}-[89abAB][0-9a-fA-F]{3}-[0-9a-fA-F]{12}$"];
	}
	public sealed class GdmHalfType : GdmScalarType<Half>
	{
	    public override Half Read(ref Utf8JsonReader reader)
	    {
	        throw new NotImplementedException();
	    }
	    public override Half Read(XmlReader reader)
	    {
	        throw new NotImplementedException();
	    }
	    public override void Write(Utf8JsonWriter writer, Half value)
	    {
	        throw new NotImplementedException();
	    }
	    public override void Write(XmlWriter writer, Half value)
	    {
	        throw new NotImplementedException();
	    }
	}
	public sealed class GdmInt128Type : GdmScalarType<Int128>
	{
	    public override Int128 Read(ref Utf8JsonReader reader)
	    {
	        return Int128.Parse(reader.ValueSpan,
	            NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
	            NumberFormatInfo.InvariantInfo);
	    }
	    public override Int128 Read(XmlReader reader)
	    {
	        return Int128.Parse(reader.ReadContentAsString(),
	            NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
	            NumberFormatInfo.InvariantInfo);
	    }
	    public override void Write(Utf8JsonWriter writer, Int128 value)
	    {
	        writer.WriteRawValue(ConvertToBytes(value), true);
	    }
	    public override void Write(XmlWriter writer, Int128 value)
	    {
	        writer.WriteRaw(value.ToString());
	    }
	    private unsafe byte[] ConvertToBytes(Int128 value)
	    {
	        var array = new byte[16];
	        fixed (byte* pointer = array)
	        {
	            *(Int128*)pointer = value;
	        }
	        return array;
	    }
	}
	public sealed class GdmInt16Type : GdmScalarType<Int16>
	{
	    public override short Read(ref Utf8JsonReader reader)
	    {
	        return reader.GetInt16();
	    }
	    public override short Read(XmlReader reader)
	    {
	        return short.Parse(
	            reader.ReadContentAsString(),
	            NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
	            NumberFormatInfo.InvariantInfo);
	    }
	    public override void Write(Utf8JsonWriter writer, short value)
	    {
	        writer.WriteNumberValue(value);
	    }
	    public override void Write(XmlWriter writer, short value)
	    {
	        writer.WriteValue(value);
	    }
	}
	public sealed class GdmInt32Type : GdmScalarType<Int32>
	{
	    public override int Read(ref Utf8JsonReader reader)
	    {
	        return reader.GetInt32();
	    }
	    public override int Read(XmlReader reader)
	    {
	        return reader.ReadContentAsInt();
	    }
	    public override void Write(Utf8JsonWriter writer, int value)
	    {
	        writer.WriteNumberValue(value);
	    }
	    public override void Write(XmlWriter writer, int value)
	    {
	        writer.WriteValue(value);
	    }
	}
	public sealed class GdmInt64Type : GdmScalarType<Int64>
	{
	    public override long Read(ref Utf8JsonReader reader)
	    {
	        return reader.GetInt64();
	    }
	    public override long Read(XmlReader reader)
	    {
	        return reader.ReadContentAsLong();
	    }
	    public override void Write(Utf8JsonWriter writer, long value)
	    {
	        writer.WriteNumberValue(value);
	    }
	    public override void Write(XmlWriter writer, long value)
	    {
	        writer.WriteValue(value);
	    }
	}
	public abstract class GdmScalarType<T> : GdmType<T>,
	    IOGraphGdmScalarType
	{
	    public virtual string[]? Formats => [];
	    public virtual T Parse(object? value)
	    {
	        throw new NotImplementedException();
	    }
	    public virtual bool TryParse(object? value, out T result)
	    {
	        throw new NotImplementedException();
	    }
	    #region Overloads
	    public override GdmTypeKind Kind => GdmTypeKind.Scalar;
	    public override string ToString()
	    {
	        return Label;
	    }
	    public override bool Equals(object? obj)
	    {
	        if (obj is not null)
	        {
	            return GetHashCode() == obj.GetHashCode();
	        }
	        return false;
	    }
	    public override int GetHashCode()
	    {
	        return Label.GetHashCode();
	    }
	    object IOGraphGdmScalarType.Parse(object? value)
	    {
	        return Parse(value)!;
	    }
	    bool IOGraphGdmScalarType.TryParse(object? value, out object? result)
	    {
	        result = default;
	        if (TryParse(value, out var r))
	        {
	            result = r;
	            return true;
	        }
	        return false;
	    }
	    #endregion
	}
	public sealed class GdmStringType : GdmScalarType<string>
	{
	    public override string Read(ref Utf8JsonReader reader)
	    {
	        return reader.GetString()!;
	    }
	    public override string Read(XmlReader reader)
	    {
	        return reader.ReadContentAsString();
	    }
	    public override void Write(Utf8JsonWriter writer, string value)
	    {
	        writer.WriteStringValue(value);
	    }
	    public override void Write(XmlWriter writer, string value)
	    {
	        writer.WriteValue(value);
	    }
	}
	public sealed class GdmTimeSpanType : GdmScalarType<TimeSpan>
	{
	    public override TimeSpan Read(ref Utf8JsonReader reader)
	    {
	        return TimeSpan.Parse(reader.GetString()!);
	    }
	    public override TimeSpan Read(XmlReader reader)
	    {
	        return TimeSpan.Parse(reader.ReadContentAsString());
	    }
	    public override void Write(Utf8JsonWriter writer, TimeSpan value)
	    {
	        writer.WriteStringValue(value.ToString());
	    }
	    public override void Write(XmlWriter writer, TimeSpan value)
	    {
	        writer.WriteValue(value.ToString());
	    }
	}
	public sealed class GdmTimeType : GdmScalarType<TimeOnly>
	{
	    public GdmTimeType() { }
	    public override Label Label => "Time";
	    public override TimeOnly Read(ref Utf8JsonReader reader)
	    {
	        return TimeOnly.Parse(reader.GetString()!);
	    }
	    public override TimeOnly Read(XmlReader reader)
	    {
	        return TimeOnly.Parse(reader.ReadContentAsString());
	    }
	    public override void Write(Utf8JsonWriter writer, TimeOnly value)
	    {
	        writer.WriteStringValue(value.ToString());
	    }
	    public override void Write(XmlWriter writer, TimeOnly value)
	    {
	        writer.WriteValue(value.ToString());
	    }
	}
	public sealed class GdmUInt128Type : GdmScalarType<UInt128>
	{
	    public override UInt128 Read(ref Utf8JsonReader reader)
	    {
	        return UInt128.Parse(reader.ValueSpan,
	            NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
	            NumberFormatInfo.InvariantInfo);
	    }
	    public override UInt128 Read(XmlReader reader)
	    {
	        return UInt128.Parse(reader.ReadContentAsString(),
	            NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
	            NumberFormatInfo.InvariantInfo);
	    }
	    public override void Write(Utf8JsonWriter writer, UInt128 value)
	    {
	        writer.WriteRawValue(ConvertToBytes(value), true);
	    }
	    public override void Write(XmlWriter writer, UInt128 value)
	    {
	        writer.WriteRaw(value.ToString());
	    }
	    private unsafe byte[] ConvertToBytes(UInt128 value)
	    {
	        var array = new byte[16];
	        fixed (byte* pointer = array)
	        {
	            *(UInt128*)pointer = value;
	        }
	        return array;
	    }
	}
	public sealed class GdmUInt16Type : GdmScalarType<UInt16>
	{
	    public override ushort Read(ref Utf8JsonReader reader)
	    {
	        return reader.GetUInt16();
	    }
	    public override ushort Read(XmlReader reader)
	    {
	        return ushort.Parse(
	            reader.ReadContentAsString(),
	            NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
	            NumberFormatInfo.InvariantInfo);
	    }
	    public override void Write(Utf8JsonWriter writer, ushort value)
	    {
	        writer.WriteNumberValue(value);
	    }
	    public override void Write(XmlWriter writer, ushort value)
	    {
	        writer.WriteValue(value.ToString());
	    }
	}
	public sealed class GdmUInt32Type : GdmScalarType<UInt32>
	{
	    public override uint Read(ref Utf8JsonReader reader)
	    {
	        return reader.GetUInt32();
	    }
	    public override uint Read(XmlReader reader)
	    {
	        return uint.Parse(
	            reader.ReadContentAsString(),
	            NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
	            NumberFormatInfo.InvariantInfo);
	    }
	    public override void Write(Utf8JsonWriter writer, uint value)
	    {
	        writer.WriteNumberValue(value);
	    }
	    public override void Write(XmlWriter writer, uint value)
	    {
	        writer.WriteValue(value.ToString());
	    }
	}
	public sealed class GdmUInt64Type : GdmScalarType<UInt64>
	{
	    public override ulong Read(ref Utf8JsonReader reader)
	    {
	        return reader.GetUInt64();
	    }
	    public override ulong Read(XmlReader reader)
	    {
	        return ulong.Parse(
	            reader.ReadContentAsString(), 
	            NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, 
	            NumberFormatInfo.InvariantInfo);
	    }
	    public override void Write(Utf8JsonWriter writer, ulong value)
	    {
	        writer.WriteNumberValue(value);
	    }
	    public override void Write(XmlWriter writer, ulong value)
	    {
	        writer.WriteValue(value.ToString());
	    }
	}
	public sealed class GdmUriType : GdmScalarType<Uri>
	{
	    public override Uri Read(ref Utf8JsonReader reader)
	    {
	        return new Uri(reader.GetString()!);
	    }
	    public override Uri Read(XmlReader reader)
	    {
	        return new Uri(reader.ReadContentAsString());
	    }
	    public override void Write(Utf8JsonWriter writer, Uri value)
	    {
	        writer.WriteStringValue(value.ToString());
	    }
	    public override void Write(XmlWriter writer, Uri value)
	    {
	        writer.WriteValue(value.ToString());
	    }
	}
	#endregion
	#region \Elements\Vertex
	[DebuggerDisplay("Vertex = {Label}")]
	public class GdmVertex : IOGraphGdmVertex
	{
	    private readonly Action<IOGraphGdmVertexDescriptor> configure;
	    private readonly IList<IOGraphGdmBinding> bindings = new List<IOGraphGdmBinding>();
	    internal Label label;
	    internal IOGraphGdmTypeReference? type;
	    public GdmVertex() : this(descriptor => { }) { }
	    GdmVertex(Action<IOGraphGdmVertexDescriptor> configure)
	    {
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        this.configure = configure;
	        this.Configure(new GdmVertexDescriptor(this));
	    }
	    protected virtual void Configure(IOGraphGdmVertexDescriptor descriptor)
	    {
	        configure.Invoke(descriptor);
	    }
	    public Label Label => label;
	    public IOGraphGdmTypeReference Type => type!;
	    public IOGraphGdmEdgeReferenceCollection Edges { get; } = new GdmEdgeReferenceCollection();
	    public IOGraphGdmMetadata Metadata { get; } = new GdmMetadata();
	    public GdmElementKind ElementKind => GdmElementKind.Vertex;
	    #region Explicit Implementations
	    IEnumerable<IOGraphGdmBinding> IOGraphGdmBindingElement.Bindings => bindings;
	    void IOGraphGdmBindingElement.Bind(IOGraphGdmBinding binding)
	    {
	        if (binding is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(binding));
	        }
	        if (this.HasBinding(binding.Label))
	        {
	            ThrowHelper.ThrowInvalidOperationException($"The element already contains a binding with the label: {binding.Label}");
	        }
	        bindings.Add(binding);
	    }
	    void IOGraphGdmBindingElement.Unbind(IOGraphGdmBinding binding)
	    {
	        if (binding is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(binding));
	        }
	        if (!bindings.Remove(binding))
	        {
	            // TODO: Throw error
	        }
	    }
	    #endregion
	    #region Static Members
	    public static GdmVertex Create(Action<IOGraphGdmVertexDescriptor> configure)
	    {
	        return new GdmVertex(configure);
	    }
	    #endregion
	    #region Overload Members
	    public override string ToString()
	    {
	        return Label;
	    }
	    public override int GetHashCode()
	    {
	        return HashCode.Combine(Label, typeof(IOGraphGdmVertex));
	    }
	    public override bool Equals(object? instance)
	    {
	        if (instance is not null)
	        {
	            return GetHashCode() == instance.GetHashCode();
	        }
	        return false;
	    }
	    #endregion
	}
	[DebuggerDisplay("Vertex = {Label}")]
	public class GdmVertex<T> : IOGraphGdmVertex
	    where T : class, new()
	{
	    private readonly Action<IOGraphGdmVertexDescriptor<T>> configure;
	    private readonly IList<IOGraphGdmBinding> bindings = new List<IOGraphGdmBinding>();
	    internal Label label;
	    internal IOGraphGdmTypeReference? type;
	    public GdmVertex() : this(descriptor => { }) { }
	    GdmVertex(Action<IOGraphGdmVertexDescriptor<T>> configure)
	    {
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        this.configure = configure;
	        this.Configure(new GdmVertexDescriptor<T>(this));
	    }
	    protected virtual void Configure(IOGraphGdmVertexDescriptor<T> descriptor)
	    {
	        configure?.Invoke(descriptor);
	    }
	    public Label Label => label;
	    public IOGraphGdmTypeReference Type => type!;
	    public IOGraphGdmEdgeReferenceCollection Edges { get; } = new GdmEdgeReferenceCollection();
	    public IOGraphGdmMetadata Metadata { get; } = new GdmMetadata();
	    public GdmElementKind ElementKind => GdmElementKind.Vertex;
	    #region Explicit Implementations
	    IEnumerable<IOGraphGdmBinding> IOGraphGdmBindingElement.Bindings => bindings;
	    void IOGraphGdmBindingElement.Bind(IOGraphGdmBinding binding)
	    {
	        if (binding is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(binding));
	        }
	        if (this.HasBinding(binding.Label))
	        {
	            ThrowHelper.ThrowInvalidOperationException($"The element already contains a binding with the label: {binding.Label}");
	        }
	        bindings.Add(binding);
	    }
	    void IOGraphGdmBindingElement.Unbind(IOGraphGdmBinding binding)
	    {
	        if (binding is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(binding));
	        }
	        if (!bindings.Remove(binding))
	        {
	            // TODO: Throw error
	        }
	    }
	    #endregion
	    public static GdmVertex<T> Create(Action<IOGraphGdmVertexDescriptor<T>> configure) 
	    {
	        return new GdmVertex<T>(configure);
	    }
	    #region Overload Members
	    public override string ToString()
	    {
	        return Label;
	    }
	    public override int GetHashCode()
	    {
	        return HashCode.Combine(Label, typeof(IOGraphGdmVertex));
	    }
	    public override bool Equals(object? instance)
	    {
	        if (instance is not null)
	        {
	            return GetHashCode() == instance.GetHashCode();
	        }
	        return false;
	    }
	    #endregion
	}
	#endregion
	#region \Exceptions
	public enum GdmErrorCode
	{
	    GDM0000,
	    #region Type Errors - 0###
	    //      Primitive Type Errors   - 01##,
	    GDM0101,
	    //      Enum Type Errors        - 02##,
	    GDM0201,
	    //      Complex Type Errors     - 03##
	    GDM0301,
	    //      Entity Type Errors      - 04##,
	    GDM0401,
	    //      Collection Type Errors  - 05##,
	    GDM0501,
	    #endregion
	    #region Vertex Errors - 1###
	    GDM1001,
	    GDM1002,
	    #endregion
	    #region Edge Errors - 2###
	    #endregion
	    #region Serialization Errors - 3###
	    GDM3001,
	    #endregion
	    /* GDM5000 is a custom error to be used for 
	     */
	    GDM5000,
	    GDM5001,
	}
	[DebuggerDisplay("{ErrorCode} - {Message}. Error occurred at or on: {Source}")]
	public abstract class GdmException : Exception
	{
	    protected GdmException(string? message) 
	        : base(message)
	    {
	    }
	    protected GdmException(string? message, Exception? innerException) 
	        : base(message, innerException)
	    {
	    }
	    public virtual GdmErrorCode ErrorCode { get; }
	}
	#endregion
	#region \Extensions
	public static class OGraphGdmBindingElementExtensions
	{
	    public static bool HasBinding<T>(this IOGraphGdmBindingElement element, out T? binding) 
	        where T : IOGraphGdmBinding
	    {
	        binding = default;
	        if (element is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(element));
	        }
	        if (element.Bindings is not null)
	        {
	            foreach (var b in element.Bindings)
	            {
	                if (b is T match)
	                {
	                    binding = match;
	                    return true;
	                }
	            }
	        }
	        return false;
	    }
	}
	public static class OGraphGdmBuilderExtensions
	{
	    public static IOGraphGdmBuilder ConfigureOptions(this IOGraphGdmBuilder builder , Action<OGraphGdmBuilderOptions> configure)
	    {
	        var options = new OGraphGdmBuilderOptions();
	        configure.Invoke(options);
	        return default;
	    }
	    public static IOGraphGdmBuilder SetAllPropertiesToCamalCase(this IOGraphGdmBuilder builder)
	    {
	        return builder.AfterBuild(model =>
	        {
	            foreach (var complexType in model.GetGdmComplexTypes())
	            {
	                // Copy the current properties to a new collection
	                var properties = new List<IOGraphGdmProperty>(complexType.Properties);
	                // Clear out the existing properties
	                complexType.Properties.Clear();
	                foreach (var property in complexType.Properties)
	                {
	                    var prop = GdmProperty.Wrap(property);
	                    prop.Label = prop.Label.ToCamalCase();
	                    complexType.Properties.Add(prop);
	                }
	            }
	        });
	    }
	    public static IOGraphGdmBuilder AddVertex<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicProperties)] T>(this IOGraphGdmBuilder builder, Label label, Action<IOGraphGdmEntityTypeDescriptor<T>> configure)
	        where T : class, new()
	    {
	        if (builder is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(builder));
	        }
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        Action<IOGraphGdmVertexDescriptor<T>> action = vertex =>
	        {
	            vertex.HasLabel(label);
	            vertex.HasType(GdmEntityType<T>.Create(configure));
	        };
	        return builder.AddVertex<T>(action);
	    }
	    public static IOGraphGdmBuilder AddVertex<T>(this IOGraphGdmBuilder builder, Action<IOGraphGdmVertexDescriptor<T>> configure) 
	        where T : class, new()
	    {
	        if (builder is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(builder));
	        }
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        return builder.AddVertex(GdmVertex<T>.Create(configure));
	    }
	    public static IOGraphGdmBuilder AddComplexType<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicProperties)] T>(this IOGraphGdmBuilder builder, Action<IOGraphGdmComplexTypeDescriptor<T>> configure) where T : class, new()
	    {
	        if (builder is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(builder));
	        }
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        return builder.AddType(GdmComplexType<T>.Create(configure));
	    }
	    public static IOGraphGdmBuilder AddEntityType<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicProperties)] T>(this IOGraphGdmBuilder builder, Action<IOGraphGdmEntityTypeDescriptor<T>> configure) where T : class, new()
	    {
	        if (builder is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(builder));
	        }
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        return builder.AddType(GdmEntityType<T>.Create(configure));
	    }
	}
	public static class OGraphGdmExtensions
	{
	    public static TGdmBinding? GetBinding<TGdmBinding>(this IOGraphGdmBindingElement element, Label label)
	        where TGdmBinding : IOGraphGdmBinding
	    {
	        if (element is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(element));
	        }
	        return element.Bindings
	            .OfType<TGdmBinding>()
	            .FirstOrDefault(p => p.Label.Equals(label));
	    }
	    public static bool HasBinding(this IOGraphGdmBindingElement element, Label label)
	    {
	        if (element is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(element));
	        }
	        return element.Bindings.Any(p=>p.Label.Equals(label));
	    }
	    public static IEnumerable<IOGraphGdmVertex> GetGdmVertices(this IOGraphGdm model)
	    {
	        if (model is null)
	        {
	            throw new ArgumentNullException(nameof(model));
	        }
	        return model.Elements.OfType<IOGraphGdmVertex>();
	    }
	    public static IEnumerable<IOGraphGdmEdge> GetGdmEdges(this IOGraphGdm model)
	    {
	        if (model is null)
	        {
	            throw new ArgumentNullException(nameof(model));
	        }
	        return model.Elements.OfType<IOGraphGdmEdge>();
	    }
	    public static IEnumerable<IOGraphGdmType> GetGdmTypes(this IOGraphGdm model)
	    {
	        if (model is null)
	        {
	            throw new ArgumentNullException(nameof(model));
	        }
	        return model.Elements.OfType<IOGraphGdmType>();
	    }
	    public static IEnumerable<IOGraphGdmScalarType> GetGdmPrimitiveTypes(this IOGraphGdm model)
	    {
	        if (model is null)
	        {
	            throw new ArgumentNullException(nameof(model));
	        }
	        return model.Elements.OfType<IOGraphGdmScalarType>();
	    }
	    public static IEnumerable<IOGraphGdmEnumType> GetGdmEnumTypes(this IOGraphGdm model)
	    {
	        if (model is null)
	        {
	            throw new ArgumentNullException(nameof(model));
	        }
	        return model.Elements.OfType<IOGraphGdmEnumType>();
	    }
	    public static IEnumerable<IOGraphGdmEntityType> GetGdmEntityTypes(this IOGraphGdm model)
	    {
	        if (model is null)
	        {
	            throw new ArgumentNullException(nameof(model));
	        }
	        return model.Elements.OfType<IOGraphGdmEntityType>();
	    }
	    public static IEnumerable<IOGraphGdmComplexType> GetGdmComplexTypes(this IOGraphGdm model)
	    {
	        if (model is null)
	        {
	            throw new ArgumentNullException(nameof(model));
	        }
	        return model.Elements.OfType<IOGraphGdmComplexType>();
	    }
	    public static IEnumerable<IOGraphGdmCollectionType> GetGdmCollectionTypes(this IOGraphGdm model)
	    {
	        if (model is null)
	        {
	            throw new ArgumentNullException(nameof(model));
	        }
	        return model.Elements.OfType<IOGraphGdmCollectionType>();
	    }
	    public static IEnumerable<IOGraphGdmProperty> GetGdmProperties(this IOGraphGdm model)
	    {
	        if (model is null)
	        {
	            throw new ArgumentNullException(nameof(model));
	        }
	        return model.Elements.OfType<IOGraphGdmProperty>();
	    }
	    private static IEnumerable<string> GetPaths(this IOGraphGdm model)
	    {
	        var root = model.Label;
	        yield return root;
	        foreach (var vertex in model.GetGdmVertices())
	        {
	            foreach (var path in Paths(root, vertex))
	            {
	                yield return path;
	            }
	        }
	        IEnumerable<string> Paths(string root, IOGraphGdmVertex vertex)
	        {
	            foreach (var edge in vertex.Edges)
	            {
	                var key = (vertex.Type as IOGraphGdmEntityType)!.Key.Property.Definition.Label;
	                var label = string.Join('/', root, $"{key}", edge.Definition.Label);
	                yield return string.Join('/', root, $"{key}", label);
	                foreach (var child in Paths(label, edge.Definition.Target.Definition))
	                {
	                    yield return child;
	                }
	            }
	        }
	    }
	}
	public static class OGraphGdmEdgeExtensions
	{
	}
	public static class OGraphGdmPropertyExtensions
	{
	}
	public static class OGraphGdmPropertyCollectionExtensions
	{
	}
	public static class OGraphGdmPropertyDescriptorExtensions
	{
	    public static IOGraphGdmPropertyDescriptor<T?> UseType<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(
	        this IOGraphGdmPropertyDescriptor<T?> descriptor,
	        Action<IOGraphGdmComplexTypeDescriptor<T>> configure) where T : class, new()
	    {
	        if (descriptor is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(descriptor));
	        }
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        return descriptor.UseType(
	            GdmComplexType<T>.Create(
	                configure));
	    }
	    public static IOGraphGdmPropertyDescriptor<IEnumerable<T>?> UseType<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(
	        this IOGraphGdmPropertyDescriptor<IEnumerable<T>?> descriptor,
	        Action<IOGraphGdmComplexTypeDescriptor<T>> configure) where T : class, new()
	    {
	        if (descriptor is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(descriptor));
	        }
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        return descriptor.UseType(
	            new GdmListType<T>(
	                GdmComplexType<T>.Create(
	                    configure)));
	    }
	    public static IOGraphGdmPropertyDescriptor<IEnumerable<T>?> UseType<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(
	        this IOGraphGdmPropertyDescriptor<IEnumerable<T>?> descriptor,
	        Label label,
	        Action<IOGraphGdmComplexTypeDescriptor<T>> configure) where T : class, new()
	    {
	        if (descriptor is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(descriptor));
	        }
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        return descriptor.UseType(
	            new GdmListType<T>(
	                GdmComplexType<T>.Create(configure))
	                {
	                    label = label
	                });
	    }
	    public static IOGraphGdmPropertyDescriptor<T[]?> UseType<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(
	        this IOGraphGdmPropertyDescriptor<T[]?> descriptor,
	        Action<IOGraphGdmComplexTypeDescriptor<T>> configure) 
	        where T : class, new()
	    {
	        if (descriptor is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(descriptor));
	        }
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        return descriptor.UseType(
	            new GdmArrayType<T>(
	                GdmComplexType<T>.Create(
	                    configure)));
	    }
	}
	public static class OGraphGdmTypeExtensions
	{
	    public static IOGraphGdmCollectionType AsCollection<
	        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicProperties)] T
	        >(this GdmType<T> gdmType)
	    {
	        if (gdmType is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(gdmType));
	        }
	        return new GdmListType<T>(gdmType);
	    }
	    public static bool TryGetProperty(this IOGraphGdmComplexType complexType, Label label, out IOGraphGdmProperty? property)
	    {
	        property = default;
	        foreach (var item in complexType.Properties)
	        {
	            if (item.Label == label)
	            {
	                property = item;
	                return true;
	            }
	        }
	        return false;
	    }
	    //public static IOGraphGdmPropertyDescriptor<T>
	}
	public static class OGraphGdmTypeDescriptorExtensions
	{
	}
	public static class OGraphGdmVertexExtensions
	{
	    public static bool TryGetProperty(this IOGraphGdmVertex vertex, Label label, out IOGraphGdmProperty? property)
	    {
	        property = null;
	        if (vertex is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(vertex));
	        }
	        if (vertex.Type is null || vertex.Type.Definition is null)
	        {
	            ThrowHelper.ThrowVertexInvalidTypeReferenceIsNull(vertex.Label);
	        }
	        if (vertex.Type.Definition is not IOGraphGdmEntityType entity)
	        {
	            ThrowHelper.ThrowVertexInvalidTypeReferenceIsNotEntityType(vertex.Label);
	        }
	        else
	        {
	            property = entity.Properties.FirstOrDefault(p => p.Label == label);
	        }
	        return property is not null;
	    }
	    public static IEnumerable<IOGraphGdmProperty> GetProperties(this IOGraphGdmVertex vertex)
	    {
	        if (vertex is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(vertex));
	        }
	        if (vertex.Type is null || vertex.Type.Definition is null)
	        {
	            ThrowHelper.ThrowVertexInvalidTypeReferenceIsNull(vertex.Label);
	        }
	        if (vertex.Type.Definition is not IOGraphGdmEntityType entity)
	        {
	            ThrowHelper.ThrowVertexInvalidTypeReferenceIsNotEntityType(vertex.Label);
	        }
	        else 
	        {
	            foreach (var property in entity.Properties)
	            {
	                yield return property;
	            }
	        }
	    }
	    public static IOGraphGdmEntityType GetGdmEntityType(this IOGraphGdmVertex vertex)
	    {
	        IOGraphGdmEntityType type = null!;
	        if (vertex is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(vertex));
	        }
	        else if (vertex.Type is null || vertex.Type.Definition is null)
	        {
	            ThrowHelper.ThrowVertexInvalidTypeReferenceIsNull(vertex.Label);
	        }
	        else if (vertex.Type.Definition is not IOGraphGdmEntityType entity)
	        {
	            ThrowHelper.ThrowVertexInvalidTypeReferenceIsNotEntityType(vertex.Label);
	        }
	        else
	        {
	            type = entity;
	        }
	        return type;
	    }
	    public static bool IsRuntimeTypeMatch(this IOGraphGdmVertex vertex, Type? type)
	    {
	        if (type is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(type));
	        }
	        var entityType = vertex.GetGdmEntityType();
	        return entityType.RuntimeType!.IsAssignableFrom(type);
	    }
	}
	public static class OGraphGdmVertexDescriptorExtensions
	{
	    public static IOGraphGdmVertexDescriptor<T> HasType<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicProperties)] T>(
	        this IOGraphGdmVertexDescriptor<T> descriptor,
	        Action<IOGraphGdmEntityTypeDescriptor<T>> configure)
	        where T : class, new()
	    {
	        if (descriptor is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(descriptor));
	        }
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        return descriptor.HasType(
	            GdmEntityType<T>.Create(
	                configure));
	    }
	    public static IOGraphGdmVertexDescriptor HasType<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicProperties)] T>(
	        this IOGraphGdmVertexDescriptor descriptor, 
	        Action<IOGraphGdmEntityTypeDescriptor<T>> configure) where T : class, new()
	    {
	        if (descriptor is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(descriptor));
	        }
	        if (configure is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(configure));
	        }
	        return descriptor.HasType(
	            GdmEntityType<T>.Create(
	                configure));
	    }
	}
	#endregion
	#region \Internal
	[DebuggerDisplay("Gdm = {Label}")]
	internal class Gdm : IOGraphGdm
	{
	    public Gdm()
	    {
	        Elements.Add(this);
	    }
	    public Label Label { get; internal set; }
	    public IOGraphGdmElementCollection Elements { get; } = new GdmElementCollection();
	    public IEnumerable<IOGraphGdmBinding> Bindings { get; } = new List<IOGraphGdmBinding>();
	    public GdmElementKind ElementKind => GdmElementKind.Graph;
	    public void Bind(IOGraphGdmBinding binding)
	    {
	        if (binding is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(binding));
	        }
	        (Bindings as List<IOGraphGdmBinding>)!.Add(binding);
	    }
	    void IOGraphGdmBindingElement.Unbind(IOGraphGdmBinding binding)
	    {
	        if (binding is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(binding));
	        }
	        if (!(Bindings as List<IOGraphGdmBinding>)!.Remove(binding))
	        {
	            // TODO: Throw error
	        }
	    }
	}
	internal class GdmEdge : IOGraphGdmEdge
	{
	    private readonly IList<IOGraphGdmBinding> bindings = new List<IOGraphGdmBinding>();
	    public GdmEdge()
	    {
	        Metadata = new GdmMetadata();
	    }
	    public Label Label { get; set; } = default!;
	    public IOGraphGdmVertexReference Source { get; set; } = default!;
	    public IOGraphGdmVertexReference Target { get; set; } = default!;
	    public IOGraphGdmMetadata Metadata { get; }
	    public GdmElementKind ElementKind => GdmElementKind.Edge;
	    IEnumerable<IOGraphGdmBinding> IOGraphGdmBindingElement.Bindings => bindings;
	    void IOGraphGdmBindingElement.Bind(IOGraphGdmBinding binding)
	    {
	        if (binding is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(binding));
	        }
	        bindings.Add(binding);
	    }
	    void IOGraphGdmBindingElement.Unbind(IOGraphGdmBinding binding)
	    {
	        if (binding is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(binding));
	        }
	        if (!bindings.Remove(binding))
	        {
	            // TODO: Throw error
	        }
	    }
	}
	internal class GdmEdgeReference : IOGraphGdmEdgeReference
	{
	    public IOGraphGdmEdge Definition { get; init; } = default!;
	}
	[DebuggerDisplay("Count = {Count}")]
	internal class GdmEdgeReferenceCollection : List<IOGraphGdmEdgeReference>,
	    IOGraphGdmEdgeReferenceCollection
	{
	}
	internal class GdmElementCollection : List<IOGraphGdmElement>,
	    IOGraphGdmElementCollection
	{
	    public TGdmElement Get<TGdmElement>(Label label) where TGdmElement : IOGraphGdmLabeledElement
	    {
	        foreach (var item in this)
	        {
	            if (item is TGdmElement element && element.Label == label)
	            {
	                return element;
	            }
	        }
	        throw new InvalidOperationException();
	    }
	}
	[DebuggerDisplay("Count = {Count}")]
	internal class GdmElementCollectionT : IOGraphGdmElementCollection
	{
	    private readonly HashSet<IOGraphGdmLabeledElement> elements;
	    public GdmElementCollectionT()
	    {
	        elements = new HashSet<IOGraphGdmLabeledElement>();
	    }
	    public int Count => elements.Count;
	    public bool IsReadOnly { get; internal set; }
	    public void Add(IOGraphGdmLabeledElement item)
	    {
	        AssertReadOnly();
	        AssertNotNull(item);
	        switch (item)
	        {
	            case IOGraphGdmProperty property:
	                {
	                    Add(property);
	                    break;
	                }
	            case IOGraphGdmType type:
	                {
	                    Add(type);
	                    break;
	                }
	            case IOGraphGdmVertex vertex:
	                {
	                    Add(vertex);
	                    break;
	                }
	        }
	    }
	    private void Add(IOGraphGdmProperty property)
	    {
	        elements.Add(property);
	        if (property.Type is not null)
	        {
	            Add(property.Type.Definition);
	        }
	    }
	    private void Add(IOGraphGdmVertex vertex)
	    {
	        elements.Add(vertex);
	        Add(vertex.Type.Definition);
	    }
	    private void Add(IOGraphGdmType type)
	    {
	        switch (type)
	        {
	            case IOGraphGdmScalarType pt:
	                {
	                    Add(pt);
	                    break;
	                }
	            case IOGraphGdmEnumType et:
	                {
	                    Add(et);
	                    break;
	                }
	            case IOGraphGdmComplexType cpt:
	                {
	                    Add(cpt);
	                    break;
	                }
	            case IOGraphGdmCollectionType ct:
	                {
	                    Add(ct);
	                    break;
	                }
	        }
	    }
	    private void Add(IOGraphGdmComplexType type)
	    {
	        elements.Add(type);
	        foreach (var property in type.Properties)
	        {
	            Add(property);
	        }
	    }
	    private void Add(IOGraphGdmCollectionType type)
	    {
	        elements.Add(type);
	        Add(type.ItemType);
	    }
	    private void Add(IOGraphGdmEnumType type)
	    {
	        elements.Add(type);
	    }
	    private void Add(IOGraphGdmScalarType type)
	    {
	        elements.Add(type);
	    }
	    public bool Remove(IOGraphGdmLabeledElement item)
	    {
	        AssertReadOnly();
	        AssertNotNull(item);
	        throw new NotImplementedException();
	    }
	    public void Clear()
	    {
	        AssertReadOnly();
	        elements.Clear();
	    }
	    public bool Contains(IOGraphGdmLabeledElement item)
	    {
	        AssertNotNull(item);
	        throw new NotImplementedException();
	    }
	    public void CopyTo(IOGraphGdmLabeledElement[] array, int arrayIndex)
	    {
	        if (array is null) throw new ArgumentNullException("array");
	        elements.CopyTo(array, arrayIndex);
	    }
	    public IEnumerator<IOGraphGdmLabeledElement> GetEnumerator()
	    {
	        return elements.GetEnumerator();
	    }
	    IEnumerator IEnumerable.GetEnumerator()
	    {
	        return GetEnumerator();
	    }
	    private void AssertReadOnly()
	    {
	        if (IsReadOnly)
	        {
	            throw new InvalidOperationException("The collection is readonly.");
	        }
	    }
	    private void AssertNotNull(object item)
	    {
	        if (item is null)
	        {
	            throw new ArgumentNullException(nameof(item));
	        }
	    }
	    public TGdmElement Get<TGdmElement>(Label label) where TGdmElement : IOGraphGdmLabeledElement
	    {
	        throw new NotImplementedException();
	    }
	    public void Add(IOGraphGdmElement item)
	    {
	        throw new NotImplementedException();
	    }
	    public bool Contains(IOGraphGdmElement item)
	    {
	        throw new NotImplementedException();
	    }
	    public void CopyTo(IOGraphGdmElement[] array, int arrayIndex)
	    {
	        throw new NotImplementedException();
	    }
	    public bool Remove(IOGraphGdmElement item)
	    {
	        throw new NotImplementedException();
	    }
	    IEnumerator<IOGraphGdmElement> IEnumerable<IOGraphGdmElement>.GetEnumerator()
	    {
	        throw new NotImplementedException();
	    }
	}
	internal class GdmFactory : IOGraphGdmFactory
	{
	    private readonly IEnumerable<IOGraphGdm> models;
	    public GdmFactory(IEnumerable<IOGraphGdm> models)
    {
        this.models = models;
	    }
	    public IOGraphGdm Create(Label label)
	    {
	        var model = models.FirstOrDefault(p => p.Label == label);
	        if (model is null)
	        {
	            ThrowHelper.ThrowArgumentException("");
	        }
	        return model!;
	    }
	}
	internal class GdmGraph : IOGraphGdmGraph
	{
	    private readonly Dictionary<Label, IOGraphGdmVertex> vertices;
	    private readonly Dictionary<Label, IOGraphGdmEdge> edges;
	    public GdmGraph(Label label)
	    {
	        Label = label;
	        this.vertices = new();
	        this.edges = new();
	    }
	    public Label Label { get; }
	    public IEnumerable<IOGraphGdmVertex> Vertices => vertices.Values;
	    public IEnumerable<IOGraphGdmEdge> Edges => edges.Values;
	    public IEnumerable<IOGraphGdmBinding> Bindings => throw new NotImplementedException();
	    public GdmElementKind ElementKind => GdmElementKind.Graph;
	    public void Bind(IOGraphGdmBinding binding)
	    {
	        throw new NotImplementedException();
	    }
	    public IOGraphGdmEdge GetEdge(Label label) => edges[label];
	    public IOGraphGdmVertex GetVertex(Label label) => vertices[label];
	    public void Unbind(IOGraphGdmBinding binding)
	    {
	        throw new NotImplementedException();
	    }
	}
	internal class GdmMetadata : Dictionary<Label, object>,
	    IOGraphGdmMetadata { }
	[DebuggerDisplay("Gdm = {Label} Property")]
	internal class GdmProperty : IOGraphGdmProperty
	{
	    public GdmProperty()
	    {
	        Metadata = new GdmMetadata();
	    }
	    public Label Label { get; set; }
	    public PropertyInfo PropertyInfo { get; set; } = default!;
	    public IOGraphGdmTypeReference Type { get; set; } = default!;
	    public IOGraphGdmTypeReference DeclaringType { get; set; } = default!;
	    public IOGraphGdmMetadata Metadata { get; init; }
	    public bool IsComputed { get; set; }
	    public bool IsNullable { get; set; } = true;
	    public bool IsReadOnly { get; set; }
	    public GdmPropertyGetter Getter { get; set; } = default!;
	    public GdmPropertySetter Setter { get; set; } = default!;
	    public GdmElementKind ElementKind => GdmElementKind.Property;
	    public IEnumerable<IOGraphGdmBinding> Bindings { get; } = new List<IOGraphGdmBinding>();
	    public void Bind(IOGraphGdmBinding binding)
	    {
	        if (binding is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(binding));
	        }
	        if (this.HasBinding(binding.Label))
	        {
	            ThrowHelper.ThrowInvalidOperationException($"The element already contains a binding with the label: {binding.Label}");
	        }
	        (Bindings as List<IOGraphGdmBinding>)!.Add(binding);
	    }
	    public override string ToString()
	    {
	        return Label;
	    }
	    public override int GetHashCode()
	    {
	        return PropertyInfo.GetHashCode();
	    }
	    public override bool Equals(object? obj)
	    {
	        if (obj is GdmProperty property)
	        {
	            return PropertyInfo.Equals(property.PropertyInfo);
	        }
	        return false;
	    }
	    public static GdmProperty Wrap(IOGraphGdmProperty property)
	    {
	        if (property is GdmProperty prop)
	        {
	            return prop;
	        }
	        return new GdmProperty()
	        {
	            IsComputed = property.IsComputed,
	            Getter = property.Getter,
	            Setter = property.Setter,
	            IsNullable = property.IsNullable,
	            Metadata = property.Metadata,
	            Label = property.Label,
	            Type = property.Type
	        };
	    }
	    public void Unbind(IOGraphGdmBinding binding)
	    {
	        (Bindings as List<IOGraphGdmBinding>)!.Remove(binding);
	    }
	}
	[DebuggerDisplay("Count = {Count}")]
	internal class GdmPropertyCollection : IOGraphGdmPropertyCollection
	{
	    private readonly HashSet<IOGraphGdmProperty> properties;
	    public GdmPropertyCollection()
	    {
	        properties = new(new GdmPropertyComparer());
	    }
	    public IOGraphGdmProperty this[Label name] => properties.First(p=>p.Label == name);
	    public int Count => properties.Count;
	    public bool IsReadOnly { get; set; }
	    public void Add(IOGraphGdmProperty item)
	    {
	        AssertReadOnly();
	        AssertNotNull(item);
	        properties.Add(GdmProperty.Wrap(item));
	    }
	    public void Clear()
	    {
	        AssertReadOnly();
	        properties.Clear();
	    }
	    public bool Contains(IOGraphGdmProperty item)
	    {
	        AssertNotNull(item);
	        return properties.Contains(item);
	    }
	    public void CopyTo(IOGraphGdmProperty[] array, int arrayIndex)
	    {
	        properties.ToArray().CopyTo(array, arrayIndex);
	    }
	    public bool Remove(IOGraphGdmProperty item)
	    {
	        AssertReadOnly();
	        AssertNotNull(item);
	        return properties.Remove(item);
	    }
	    public IEnumerator<IOGraphGdmProperty> GetEnumerator()
	    {
	        return properties.GetEnumerator();
	    }
	    IEnumerator IEnumerable.GetEnumerator()
	    {
	        return GetEnumerator();
	    }
	    private void AssertReadOnly()
	    {
	        if (IsReadOnly)
	        {
	            throw new InvalidOperationException("The collection is readonly.");
	        }
	    }
	    private void AssertNotNull(object item)
	    {
	        if (item is null)
	        {
	            throw new ArgumentNullException(nameof(item));
	        }
	    }
	    private class GdmPropertyComparer : IEqualityComparer<IOGraphGdmProperty>
	    {
	        public bool Equals(IOGraphGdmProperty? left, IOGraphGdmProperty? right)
	        {
	            return left!.Label == right!.Label;
	        }
	        public int GetHashCode(IOGraphGdmProperty obj)
	        {
	            if (obj is GdmProperty ip)
	            {
	                return ip.GetHashCode();
	            }
	            return obj.Label.GetHashCode();
	        }
	    }
	}
	internal class GdmPropertyDescriptor : IOGraphGdmPropertyDescriptor
	{
	    private readonly GdmProperty property;
	    public GdmPropertyDescriptor(GdmProperty property)
	    {
	        this.property = property;
	    }
	    public IOGraphGdmPropertyDescriptor IsComputed()
	    {
	        property.IsComputed = true;
	        return this;
	    }
	    public IOGraphGdmPropertyDescriptor IsRequired()
	    {
	        property.IsNullable = false;
	        return this;
	    }
	    public IOGraphGdmPropertyDescriptor UseGetter(GdmPropertyGetter getter)
	    {
	        if (getter is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(getter));
	        }
	        property.Getter = getter;
	        return this;
	    }
	    public IOGraphGdmPropertyDescriptor UseSetter(GdmPropertySetter setter)
	    {
	        if (setter is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(setter));
	        }
	        property.Setter = setter;
	        return this;
	    }
	    public IOGraphGdmPropertyDescriptor UseMetadata(Label key, object value)
	    {
	        property.Metadata.Add(key, value);
	        return this;
	    }
	    public IOGraphGdmPropertyDescriptor UsePropertyName(Label label)
	    {
	        property.Label = label;
	        return this;
	    }
	    public IOGraphGdmPropertyDescriptor UseType<TType>() where TType : IOGraphGdmType, new()
	    {
	        return UseType(new TType());
	    }
	    public IOGraphGdmPropertyDescriptor UseType(IOGraphGdmType type)
	    {
	        if (type is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(type));
	        }
	        property.Type = new GdmTypeReference()
	        {
	            Definition = type
	        };
	        return this;
	    }
	}
	internal class GdmPropertyDescriptor<T> : IOGraphGdmPropertyDescriptor<T>
	{
	    private readonly GdmProperty property;
	    public GdmPropertyDescriptor(GdmProperty property)
	    {
	        this.property = property;
	    }
	    public IOGraphGdmPropertyDescriptor<T> IsComputed()
	    {
	        property.IsComputed = true;
	        return this;
	    }
	    public IOGraphGdmPropertyDescriptor<T> IsRequired()
	    {
	        property.IsNullable = false;
	        return this;
	    }
	    public IOGraphGdmPropertyDescriptor<T> UseGetter(GdmPropertyGetter getter)
	    {
	        if (getter is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(getter));
	        }
	        property.Getter = getter;
	        return this;
	    }
	    public IOGraphGdmPropertyDescriptor<T> UseSetter(GdmPropertySetter setter)
	    {
	        if (setter is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(setter));
	        }
	        property.Setter = setter;
	        return this;
	    }
	    public IOGraphGdmPropertyDescriptor<T> UseMetadata(Label key, object value)
	    {
	        property.Metadata.Add(key, value);
	        return this;
	    }
	    public IOGraphGdmPropertyDescriptor<T> UsePropertyName(Label label)
	    {
	        property.Label = label;
	        return this;
	    }
	    public IOGraphGdmPropertyDescriptor<T> UseType<TGdmType>() where TGdmType : IOGraphGdmType, new()
	    {
	        return UseType(new TGdmType());
	    }
	    public IOGraphGdmPropertyDescriptor<T> UseType(IOGraphGdmType type)
	    {
	        if (type is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(type));
	        }
	        property.Type = new GdmTypeReference()
	        {
	            Definition = type
	        };
	        return this;
	    }
	}
	internal class GdmComplexTypeDescriptor : IOGraphGdmComplexTypeDescriptor
	{
	    private readonly GdmComplexType complexType;
	    public GdmComplexTypeDescriptor(GdmComplexType complexType)
	    {
	        this.complexType = complexType;
	    }
	    public IOGraphGdmComplexTypeDescriptor HasLabel(Label label)
	    {
	        complexType.label = label;
	        return this;
	    }
	    public IOGraphGdmPropertyDescriptor HasProperty(Label label)
	    {
	        var propertyInfo = complexType.runtimeType!.GetProperty(label);
	        if (propertyInfo is null)
	        {
	            throw new InvalidOperationException($"The property '{label}' does not exist on type {complexType.runtimeType!.Name}");
	        }
	        var property = complexType.GetProperty(propertyInfo);
	        property.Getter ??= propertyInfo.GetValue;
	        property.Setter ??= propertyInfo.SetValue;
	        property.DeclaringType = new GdmTypeReference()
	        {
	            Definition = complexType
	        };
	        return new GdmPropertyDescriptor(property);
	    }
	}
	internal class GdmComplexTypeDescriptor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T> : IOGraphGdmComplexTypeDescriptor<T> 
	    where T : class, new()
	{
	    private readonly GdmComplexType<T> complexType;
	    public GdmComplexTypeDescriptor(GdmComplexType<T> complexType)
	    {
	        this.complexType = complexType;
	    }
	    public IOGraphGdmComplexTypeDescriptor<T> HasLabel(Label label)
	    {
	        complexType.label = label;
	        return this;
	    }
	    public IOGraphGdmPropertyDescriptor HasProperty(Label label)
	    {
	        var propertyInfo = typeof(T).GetProperty(label);
	        if (propertyInfo is null)
	        {
	            throw new Exception();
	        }
	        var property = complexType.GetProperty(propertyInfo);
	        property.Getter ??= propertyInfo.GetValue;
	        property.Setter ??= propertyInfo.SetValue;
	        property.DeclaringType = new GdmTypeReference()
	        {
	            Definition = complexType
	        };
	        return new GdmPropertyDescriptor(property);
	    }
	    public IOGraphGdmPropertyDescriptor<TMember> HasProperty<TMember>(Expression<Func<T, TMember>> expression)
	    {
	        var propertyInfo = AssertExpression(expression)!;
	        var property = complexType.GetProperty(propertyInfo);
	        var method = expression.Compile();
	        property.Getter ??= (instance) => method.Invoke((T)instance);
	        property.Setter ??= propertyInfo.SetValue;
	        property.DeclaringType = new GdmTypeReference()
	        {
	            Definition = complexType
	        };
	        return new GdmPropertyDescriptor<TMember>(property);
	    }
	    private PropertyInfo AssertExpression<TMember>(Expression<Func<T, TMember>> expression)
	    {
	        // 1. Null reference check
	        if (expression is null)
	        {
	            throw new ArgumentNullException(nameof(expression));
	        }
	        if (expression.Body is not MemberExpression memberExpression)
	        {
	            throw new ArgumentException("");
	        }
	        //if (!memberExpression.Member.DeclaringType.IsAssignableTo(typeof(T)))
	        //{
	        //    throw new Exception();
	        //}
	        if (memberExpression.Member is not PropertyInfo propertyInfo)
	        {
	            throw new Exception();
	        }
	        return propertyInfo;
	    }
	}
	internal class GdmEntityTypeDescriptor : IOGraphGdmEntityTypeDescriptor
	{
	    private readonly GdmEntityType entityType;
	    public GdmEntityTypeDescriptor(GdmEntityType entityType)
    {
        this.entityType = entityType;
    }
	    public IOGraphGdmEntityTypeDescriptor HasLabel(Label label)
	    {
	        entityType.label = label;
	        return this;
	    }
	    public IOGraphGdmEntityTypeDescriptor HasKey(Label label)
	    {
	        var runtimeType = entityType.runtimeType;
	        var propertyInfo = runtimeType!.GetProperty(label);
	        if (propertyInfo is null)
	        {
	            throw new InvalidOperationException($"The property '{label}' does not exist on type {entityType.runtimeType!.Name}");
	        }
	        var property = entityType.GetProperty(propertyInfo);
	        property.Getter ??= propertyInfo.GetValue;
	        property.Setter ??= propertyInfo.SetValue;
	        return this;
	    }
	    public IOGraphGdmPropertyDescriptor HasProperty(Label label)
	    {
	        var propertyInfo = entityType.runtimeType!.GetProperty(label);
	        if (propertyInfo is null)
	        {
	            throw new InvalidOperationException($"The property '{label}' does not exist on type {entityType.runtimeType!.Name}");
	        }
	        var property = entityType.GetProperty(propertyInfo);
	        property.Getter ??= propertyInfo.GetValue;
	        property.Setter ??= propertyInfo.SetValue;
	        property.DeclaringType = new GdmTypeReference()
	        {
	            Definition = entityType,
	        };
	        return new GdmPropertyDescriptor(property);
	    }
	}
	internal class GdmEntityTypeDescriptor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] T> : IOGraphGdmEntityTypeDescriptor<T>
	    where T : class, new()
	{
	    private readonly GdmEntityType<T> entityType;
	    public GdmEntityTypeDescriptor(GdmEntityType<T> entityType)
	    {
	        this.entityType = entityType;
	    }
	    public IOGraphGdmEntityTypeDescriptor<T> HasLabel(Label label)
	    {
	        entityType.label = label;
	        return this;
	    }
	    public IOGraphGdmEntityTypeDescriptor<T> HasKey(Label label)
	    {
	        var propertyInfo = typeof(T).GetProperty(label);
	        if (propertyInfo is null)
	        {
	            throw new InvalidOperationException($"The property '{label}' does not exist on type {typeof(T).Name}");
	        }
	        var property = entityType.GetProperty(propertyInfo);
	        property.Getter ??= propertyInfo.GetValue;
	        property.Setter ??= propertyInfo.SetValue;
	        return this;
	    }
	    public IOGraphGdmEntityTypeDescriptor<T> HasKey<TMember>(Expression<Func<T, TMember>> expression) where TMember : struct
	    {
	        var propertyInfo = AssertExpression(expression);
	        var property = entityType.GetProperty(propertyInfo);
	        var method = expression.Compile();
	        property.Getter ??= (instance) => method.Invoke((T)instance);
	        property.Setter ??= propertyInfo.SetValue;
	        return this;
	    }
	    public IOGraphGdmEntityTypeDescriptor<T> HasKey<TMember>(Expression<Func<T, TMember?>> expression) where TMember : struct
	    {
	        var propertyInfo = AssertExpression(expression);
	        var property = entityType.GetProperty(propertyInfo);
	        var method = expression.Compile();
	        property.Getter ??= (instance) => method.Invoke((T)instance);
	        property.Setter ??= propertyInfo.SetValue;
	        return this;
	    }
	    public IOGraphGdmPropertyDescriptor HasProperty(Label label)
	    {
	        var propertyInfo = typeof(T).GetProperty(label);
	        if (propertyInfo is null)
	        {
	            throw new Exception();
	        }
	        var property = entityType.GetProperty(propertyInfo);
	        property.Getter ??= propertyInfo.GetValue;
	        property.Setter ??= propertyInfo.SetValue;
	        property.DeclaringType = new GdmTypeReference()
	        {
	            Definition = entityType
	        };
	        return new GdmPropertyDescriptor(property);
	    }
	    public IOGraphGdmPropertyDescriptor<TMember?> HasProperty<TMember>(Expression<Func<T, TMember?>> expression)
	    {
	        var propertyInfo = AssertExpression(expression)!;
	        var property = entityType.GetProperty(propertyInfo);
	        var method = expression.Compile();
	        property.Getter ??= (instance) => method.Invoke((T)instance);
	        property.Setter ??= propertyInfo.SetValue;
	        property.DeclaringType = new GdmTypeReference()
	        {
	            Definition = entityType
	        };
	        return new GdmPropertyDescriptor<TMember?>(property);
	    }
	    private PropertyInfo AssertExpression<TMember>(Expression<Func<T, TMember>> expression)
	    {
	        if (expression is null)
	        {
	            throw new ArgumentNullException(nameof(expression));
	        }
	        if (expression.Body is not MemberExpression memberExpression)
	        {
	            throw new ArgumentException("");
	        }
	        //if (!memberExpression.Member.DeclaringType.IsAssignableTo(typeof(T)))
	        //{
	        //    throw new Exception();
	        //}
	        if (memberExpression.Member is not PropertyInfo propertyInfo)
	        {
	            throw new Exception();
	        }
	        return propertyInfo;
	    }
	}
	[DebuggerDisplay("Gdm Type Reference: {Definition?.Label}")]
	internal class GdmTypeReference : IOGraphGdmTypeReference
	{
	    public IOGraphGdmType Definition { get; init; } = default!;
	}
	internal class GdmVertexDescriptor : IOGraphGdmVertexDescriptor
	{
	    private readonly GdmVertex vertex;
	    public GdmVertexDescriptor(GdmVertex vertex)
	    {
	        this.vertex = vertex;
	    }
	    public IOGraphGdmVertexDescriptor HasLabel(Label label)
	    {
	        vertex.label = label;
	        return this;
	    }
	    public IOGraphGdmVertexDescriptor HasType(IOGraphGdmEntityType type)
	    {
	        if (type is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(type));
	        }
	        vertex.type = new GdmTypeReference()
	        {
	            Definition = type
	        };
	        return this;
	    }
	    public IOGraphGdmVertexDescriptor HasType<TGdmType>() where TGdmType : IOGraphGdmEntityType, new()
	    {
	        return HasType(new TGdmType());
	    }
	}
	internal class GdmVertexDescriptor<T> : IOGraphGdmVertexDescriptor<T>
	    where T : class, new()
	{
	    private readonly GdmVertex<T> vertex;
	    public GdmVertexDescriptor(GdmVertex<T> vertex)
	    {
	        this.vertex = vertex;
	    }
	    public IOGraphGdmVertexDescriptor<T> HasEdge<TTarget>(Action<IOGraphGdmEdgeDescriptor<T, TTarget>> configure) where TTarget : class, new()
	    {
	        throw new NotImplementedException();
	    }
	    public IOGraphGdmVertexDescriptor<T> HasLabel(Label label)
	    {
	        vertex.label = label;
	        return this;
	    }
	    public IOGraphGdmVertexDescriptor<T> HasType(IOGraphGdmEntityType type)
	    {
	        if (type is null)
	        {
	            throw new ArgumentNullException(nameof(type));
	        }
	        if (type.RuntimeType is null)
	        {
	            throw new ArgumentException("The provided GDM Type has not runtime type");
	        }
	        if (!type.RuntimeType.IsAssignableTo((typeof(T))))
	        {
	            throw new InvalidOperationException($"The underlying runtime type: {type.RuntimeType.Name} is not assignable to {typeof(T).Name}.");
	        }
	        vertex.type = new GdmTypeReference()
	        {
	            Definition = type
	        };
	        return this;
	    }
	    public IOGraphGdmVertexDescriptor<T> HasType<TGdmType>() where TGdmType : IOGraphGdmEntityType, new()
	    {
	        return HasType(new TGdmType());
	    }
	    IOGraphGdmVertexDescriptor<T> IOGraphGdmVertexDescriptor<T>.HasEdge<TTarget>(Label label)
	    {
	        return this;
	    }
	}
	#endregion
	#region \Internal\Exceptions
	internal class GdmModelException : GdmException
	{
	    public GdmModelException(GdmValidatorError error) : base(error.Message)
	    {
	        ErrorCode = error.Code;
	        Source = error.Source;
	    }
	    public GdmModelException(GdmErrorCode errorCode, string? message) 
	        : base(message)
	    {
	        ErrorCode = errorCode;
	    }
	    public GdmModelException(GdmErrorCode errorCode, string source, string? message) 
	        : base(message)
	    {
	        ErrorCode = errorCode;
	        Source = source;
	    }
	    public override string? Source { get; set; }
	    public override GdmErrorCode ErrorCode { get;  }
	}
	internal class GdmSerializationException : GdmException
	{
	    public GdmSerializationException(GdmErrorCode errorCode, string? message) 
	        : base(message)
	    {
	        ErrorCode = errorCode;
	    }
	    public GdmSerializationException(GdmErrorCode errorCode, string source, string? message) 
	        : base(message)
	    {
	        ErrorCode = errorCode;
	        Source = source;
	    }
	    public override string? Source { get; set; }
	    public override GdmErrorCode ErrorCode { get; }
	}
	internal class GdmUnknownException : GdmException
	{
	    public GdmUnknownException() : base(Resources.GDM0000)
	    {
	        ErrorCode = GdmErrorCode.GDM0000;
	    }
	    public override GdmErrorCode ErrorCode { get; }
	}
	#endregion
	#region \Internal\Extensions
	internal static class GdmTypeExtensions
	{
	    public static GdmProperty GetProperty(this IOGraphGdmComplexType complexType, PropertyInfo propertyInfo)
	    {
	        var property = complexType.Properties
	            .Select(p => p is GdmProperty property ? property : complexType.WrapProperty(p, propertyInfo))
	            .FirstOrDefault(p => p.PropertyInfo.Name == propertyInfo.Name) ?? new GdmProperty()
	            {
	                PropertyInfo = propertyInfo,
	                Label = propertyInfo.Name,
	            };
	        complexType.Properties.Add(property);
	        return property;
	    }
	    public static GdmProperty WrapProperty(this IOGraphGdmComplexType complexType, IOGraphGdmProperty property, PropertyInfo propertyInfo)
	    {
	        complexType.Properties.Remove(property);
	        var wrapped = new GdmProperty()
	        {
	            IsComputed = property.IsComputed,
	            Getter = property.Getter,
	            Setter = property.Setter,
	            PropertyInfo = propertyInfo,
	            IsNullable = property.IsNullable,
	            Metadata = property.Metadata,
	            Label = property.Label,
	            Type = property.Type
	        };
	        return wrapped;
	    }
	//    public static IOGraphGdmType GetGdmType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] this Type runtimeType)
	//    {
	//        if (!runtimeType.TryGetGdmType(out var gdmType))
	//        {
	//            GdmThrowHelper.ThrowArgumentException("Runtime type could not be mapped to GDM Type.");
	//        }
	//        return gdmType!;
	//    }
	//    public static bool TryGetGdmType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] this Type runtimeType, out IOGraphGdmType? gdmType)
	//    {
	//        gdmType = null;
	//        if (runtimeType.TryGetGdmPrimitiveType(out var gdmPrimitiveType))
	//        {
	//            gdmType = gdmPrimitiveType;
	//            return true;
	//        }
	//        if (runtimeType.TryGetGdmEnumType(out var gdmEnumType))
	//        {
	//            gdmType = gdmEnumType;
	//            return true;
	//        }
	//        if (runtimeType.TryGetGdmCollectionType(out var gdmCollectionType))
	//        {
	//            gdmType = gdmCollectionType;
	//            return true;
	//        }
	//        if (runtimeType.TryGetGdmComplexType(out var gdmComplexType))
	//        {
	//            gdmType = gdmComplexType;
	//            return true;
	//        }
	//        return false;
	//    }
	//    public static IOGraphGdmEnumType GetGdmEnumType(this Type runtimeType)
	//    {
	//        if (runtimeType.TryGetGdmEnumType(out var gdmType))
	//        {
	//            GdmThrowHelper.ThrowArgumentException("Runtime type could not be mapped to GDM Type.");
	//        }
	//        return gdmType!;
	//    }
	//    public static bool TryGetGdmEnumType(this Type runtimeType, out IOGraphGdmEnumType? gdmType)
	//    {
	//        gdmType = null;
	//        if (runtimeType.Name == "Nullable`1" && runtimeType.GenericTypeArguments[0].IsEnum)
	//        {
	//            gdmType = new GdmEnumType(runtimeType.GenericTypeArguments[0]);
	//        }
	//        if (runtimeType.IsEnum)
	//        {
	//        }
	//        if (runtimeType.IsValueType && 
	//            !runtimeType.IsEnum && 
	//            runtimeType.GenericTypeArguments.Length == 1 && runtimeType.Ge)
	//        runtimeType.GenericTypeArguments
	//        //if (type.IsEnum)
	//        //{
	//        //    var enumType = typeof(GdmEnumType<>).MakeGenericType(type);
	//        //    gdmEnumType = (Activator.CreateInstance(enumType) as IOGraphGdmEnumType)!;
	//        //    return true;
	//        //}
	//        //var typeArgs = type.GetGenericArguments();
	//        //if (typeArgs.Length == 1 &&
	//        //    typeArgs[0].IsEnum &&
	//        //    typeof(Nullable<>).MakeGenericType(typeArgs[0]).IsAssignableTo(type))
	//        //{
	//        //    var enumType = typeof(GdmNullEnumType<>).MakeGenericType(typeArgs[0]);
	//        //    gdmEnumType = (Activator.CreateInstance(enumType) as IOGraphGdmEnumType)!;
	//        //    return true;
	//        //}
	//        return false;
	//    }
	//    /// <summary>
	//    /// 
	//    /// </summary>
	//    /// <param name="type"></param>
	//    /// <returns></returns>
	//    public static IOGraphGdmCollectionType GetGdmCollectionType(this Type type)
	//    {
	//        if (type.TryGetGdmCollectionType(out var gdmCollectionType))
	//        {
	//            return gdmCollectionType!;
	//        }
	//        throw new Exception();
	//    }
	//    public static bool TryGetGdmCollectionType(this Type runtimeType, out IOGraphGdmCollectionType? gdmType)
	//    {
	//        gdmType = null;
	//        if (runtimeType.IsGenericType)
	//        {
	//        }
	//        if (runtimeType.IsEnumerableType(out var enumerableType))
	//        {
	//            if (enumerableType.TryGetGdmType(out var gdmType))
	//            {
	//                var collectionType = typeof(GdmListType<>).MakeGenericType(gdmType!.GetType());
	//                gdmType = (Activator.CreateInstance(collectionType) as IOGraphGdmCollectionType)!;
	//                return true;
	//            }
	//        }
	//        return false;
	//    }
	//    public static IOGraphGdmComplexType GetGdmComplexType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] this Type type)
	//    {
	//        if (type.TryGetGdmComplexType(out var gdmComplexType))
	//        {
	//            return gdmComplexType!;
	//        }
	//        throw new Exception("");
	//    }
	//    public static bool TryGetGdmComplexType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] this Type runtimeType, out IOGraphGdmComplexType? gdmType)
	//    {
	//        gdmType = null;
	//        // 1. Check that Type is not null
	//        if (runtimeType is null)
	//        {
	//            return false;
	//        }
	//        // 2. Check that the type is a reference type
	//        if (!runtimeType.IsClass)
	//        {
	//            return false;
	//        }
	//        // 3. Ensure class is NOT abstract
	//        if (runtimeType.IsAbstract)
	//        {
	//            return false;
	//        }
	//        // 4. Since a delegate are actually compiled into a class. Let's check that 
	//        if (runtimeType.IsSubclassOf(typeof(Delegate)))
	//        {
	//            return false;
	//        }
	//        // 5. Check if type has default constructor
	//        if (runtimeType.GetConstructor(Type.EmptyTypes) is null)
	//        {
	//            return false;
	//        }
	//        // 6. Ensure 
	//        if (runtimeType.IsAssignableTo(typeof(string)))
	//        {
	//            return false;
	//        }
	//        gdmType = GdmComplexType.Create(descriptor =>
	//        {
	//            descriptor.HasRuntimeType(runtimeType);
	//        });
	//        //var complexType = typeof(GdmComplexType<>).MakeGenericType(type);
	//        //gdmComplexType = (Activator.CreateInstance(complexType) as IOGraphGdmComplexType)!;
	//        return true;
	//    }
	//    public static IEnumerable<GdmProperty> GetGdmComplexTypeProperties(this Type type)
	//    {
	//        // 1. Check that Type is not null
	//        if (type is null)
	//        {
	//            throw new ArgumentNullException(nameof(type));
	//        }
	//        // 2. Check that the type is a reference type
	//        if (!type.IsClass)
	//        {
	//            throw new ArgumentException("Invalid type. Complex types must be a class.");
	//        }
	//        // 3. Ensure class is NOT abstract
	//        if (type.IsAbstract)
	//        {
	//            throw new ArgumentException("Complex types cannot be abstract.");
	//        }
	//        // 4. Since a delegate are actually compiled into a class. Let's check that 
	//        if (type.IsSubclassOf(typeof(Delegate)))
	//        {
	//            throw new ArgumentException("Delegates are not allowed as complex types.");
	//        }
	//        // 5. Check if type has default constructor
	//        if (type.GetConstructor(Type.EmptyTypes) is null)
	//        {
	//            throw new ArgumentException($"The type {type.Name} does not have a default constructor. {type.Name}.ctor()");
	//        }
	//        var properties = type
	//            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
	//            .Where(p => p.CanWrite && p.CanRead && p.GetIndexParameters().Length == 0);
	//        foreach (var propertyInfo in properties)
	//        {
	//            var property = new GdmProperty()
	//            {
	//                Label = propertyInfo.Name,
	//                PropertyInfo = propertyInfo,
	//                Getter = (instance) =>
	//                {
	//                    return propertyInfo.GetValue(instance, null);
	//                },
	//                Setter = (instance, value) =>
	//                {
	//                    propertyInfo.SetValue(instance, value, null);
	//                }
	//            };
	//            if (propertyInfo.PropertyType.TryGetGdmType(out var gdmType))
	//            {
	//                property.Type = new GdmTypeReference()
	//                {
	//                    Definition = gdmType!
	//                };
	//            }
	//            yield return property;
	//        }
	//    }
	//    public static IOGraphGdmPrimitiveType GetGdmPrimitiveType(this Type type)
	//    {
	//        if (type.TryGetGdmPrimitiveType(out var gdmPrimitiveType))
	//        {
	//            return gdmPrimitiveType!;
	//        }
	//        throw new Exception("Couldn't identify type");
	//    }
	//    public static bool TryGetGdmPrimitiveType(this Type runtimeType, out IOGraphGdmPrimitiveType? gdmType)
	//    {
	//        gdmType = null;
	//        static Dictionary<Type, IOGraphGdmType> types = new();
	//        if (runtimeType == typeof(Uri))
	//        {
	//            gdmType = new GdmUriType();
	//            return true;
	//        }
	//        if (runtimeType == typeof(Byte))
	//        {
	//            gdmType = new GdmByteType();
	//            return true;
	//        }
	//        if (runtimeType == typeof(Char))
	//        {
	//            gdmType = new GdmCharType();
	//            return true;
	//        }
	//        if (runtimeType == typeof(Char?))
	//        {
	//            gdmType = new GdmNullCharType();
	//            return true;
	//        }
	//        if (runtimeType == typeof(DateTimeOffset))
	//        {
	//            gdmType = new GdmDateTimeOffsetType();
	//            return true;
	//        }
	//        if (runtimeType == typeof(DateTimeOffset?))
	//        {
	//            gdmType = new GdmNullDateTimeOffsetType();
	//            return true;
	//        }
	//        if (runtimeType == typeof(DateTime))
	//        {
	//            gdmType = new GdmDateTimeType();
	//            return true;
	//        }
	//        if (runtimeType == typeof(DateTime?))
	//        {
	//            gdmType = new GdmNullDateTimeType();
	//            return true;
	//        }
	//        if (runtimeType == typeof(DateOnly))
	//        {
	//            gdmType = new GdmDateType();
	//            return true;
	//        }
	//        if (runtimeType == typeof(DateOnly?))
	//        {
	//            gdmType = new GdmNullDateType();
	//            return true;
	//        }
	//        if (runtimeType == typeof(Decimal))
	//        {
//            gdmType = new GdmDecimalType();
//            return true;
//        }
//        if (runtimeType == typeof(Decimal?))
//        {
//            gdmType = new GdmNullDecimalType();
//            return true;
//        }
//        if (runtimeType == typeof(Double))
//        {
//            gdmType = new GdmDoubleType();
//            return true;
//        }
//        if (runtimeType == typeof(Double?))
//        {
//            gdmType = new GdmNullDoubleType();
//            return true;
//        }
//        if (runtimeType == typeof(float))
//        {
//            gdmType = new GdmFloatType();
//            return true;
//        }
//        if (runtimeType == typeof(float?))
//        {
//            gdmType = new GdmNullFloatType();
//            return true;
//        }
//        if (runtimeType == typeof(Guid))
//        {
//            gdmType = new GdmGuidType();
//            return true;
//        }
//        if (runtimeType == typeof(Guid?))
//        {
//            gdmType = new GdmNullGuidType();
//            return true;
//        }
//        if (runtimeType == typeof(Half))
//        {
//            gdmType = new GdmHalfType();
//            return true;
//        }
//        if (runtimeType == typeof(Half?))
//        {
//            gdmType = new GdmNullHalfType();
//            return true;
//        }
//#if NET7_0_OR_GREATER
//        if (runtimeType == typeof(Int128))
//        {
//            gdmType = new GdmInt128Type();
//            return true;
//        }
//        if (runtimeType == typeof(Int128?))
//        {
//            gdmType = new GdmNullInt128Type();
//            return true;

//        }
//#endif
//        if (runtimeType == typeof(Int16))
//        {
//            gdmType = new GdmInt16Type();
//            return true;
//        }
//        if (runtimeType == typeof(Int16?))
//        {
//            gdmType = new GdmNullInt16Type();
//            return true;
//        }
//        if (runtimeType == typeof(Int32))
//        {
//            gdmType = new GdmInt32Type();
//            return true;
//        }
//        if (runtimeType == typeof(Int32?))
//        {
//            gdmType = new GdmNullInt32Type();
//            return true;
//        }
//        if (runtimeType == typeof(Int64))
//        {
//            gdmType = new GdmInt64Type();
//            return true;
//        }
//        if (runtimeType == typeof(Int64?))
//        {
//            gdmType = new GdmNullInt64Type();
//            return true;
//        }
//        if (runtimeType == typeof(string))
//        {
//            gdmType = new GdmStringType();
//            return true;
//        }
//        if (runtimeType == typeof(TimeSpan))
//        {
//            gdmType = new GdmTimeSpanType();
//            return true;
//        }
//        if (runtimeType == typeof(TimeSpan?))
//        {
//            gdmType = new GdmNullTimeSpanType();
//            return true;
//        }
//        if (runtimeType == typeof(TimeOnly))
//        {
//            gdmType = new GdmTimeType();
//            return true;
//        }
//        if (runtimeType == typeof(TimeOnly?))
//        {
//            gdmType = new GdmNullTimeType();
//            return true;
//        }
//        if (runtimeType == typeof(UInt16))
//        {
//            gdmType = new GdmUInt16Type();
//            return true;
//        }
//        if (runtimeType == typeof(UInt16?))
//        {
//            gdmType = new GdmNullUInt16Type();
//            return true;
//        }
//        if (runtimeType == typeof(UInt32))
//        {
//            gdmType = new GdmUInt32Type();
//            return true;
//        }
//        if (runtimeType == typeof(UInt32?))
//        {
//            gdmType = new GdmNullUInt32Type();
//            return true;
//        }
//        if (runtimeType == typeof(UInt64))
//        {
//            gdmType = new GdmUInt64Type();
//            return true;
//        }
//        if (runtimeType == typeof(UInt64?))
//        {
//            gdmType = new GdmNullUInt64Type();
//            return true;
//        }

//        return false;
//    }





}
	internal static class StringExtensions
	{
		public static string ConvertToCamalCase(this string value, StringConversionStrategy strategy = StringConversionStrategy.None)
	    {
	        var chars = strategy.Equals(StringConversionStrategy.OnlyAlphaNumeric) ?
	            value.Where(c => char.IsLetter(c) || char.IsDigit(c)).ToArray() :
	            value.ToCharArray();
	        return string.Create(chars.Length, value, (span, value) =>
	        {
	            value.CopyTo(span);
	            for (int i = 0; i < span.Length && (i != 1 || char.IsUpper(span[i])); i++)
	            {
	                var flag = i + 1 < span.Length;
	                if (i > 0 && flag && !char.IsUpper(span[i + 1]))
	                {
	                    if (span[i + 1] == ' ')
	                    {
	                        span[i] = char.ToLowerInvariant(span[i]);
	                    }
	                    break;
	                }
	                span[i] = char.ToLowerInvariant(span[i]);
	            }
	        });
	    }
	    public static string ConvertToPascalCase(this string value, StringConversionStrategy strategy = StringConversionStrategy.None)
	    {
	        var chars = strategy.Equals(StringConversionStrategy.OnlyAlphaNumeric) ?
	            value.Where(c => char.IsLetter(c) || char.IsDigit(c)).ToArray() :
	            value.ToCharArray();
	        return string.Create(chars.Length, chars, (span, value) =>
	        {
	            value.CopyTo(span);
	            for (int i = 0; i < span.Length && (i != 1 || char.IsLower(span[i])); i++)
	            {
	                bool flag = i + 1 < span.Length;
	                if (i > 0 && flag && !char.IsLower(span[i + 1]))
	                {
	                    if (span[i + 1] == ' ')
	                    {
	                        span[i] = char.ToUpperInvariant(span[i]);
	                    }
	                    break;
	                }
	                span[i] = char.ToUpperInvariant(span[i]);
	            }
	        });
	    }
	    public static string ConvertToKebabCase(this string value, bool lowercase = true)
	    {
	        return ConvertStringValue('-', lowercase, value.AsSpan());
	    }
	    public static string ConvertToSnakeCase(this string value, bool lowercase = true)
	    {
	        return ConvertStringValue('_', lowercase, value.AsSpan());
	    }
	    private enum SeparatorState
	    {
	        NotStarted,
	        UppercaseLetter,
	        LowercaseLetterOrDigit,
	        SpaceSeparator
	    }
	    private static string ConvertStringValue(char separator, bool lowercase, ReadOnlySpan<char> chars)
	    {
	        char[] rentedBuffer = null;
	        int num = (int)(1.2 * (double)chars.Length);
	        Span<char> span = ((num > 128) ? ((Span<char>)(rentedBuffer = ArrayPool<char>.Shared.Rent(num))) : stackalloc char[128]);
	        Span<char> destination2 = span;
	        SeparatorState separatorState = SeparatorState.NotStarted;
	        int charsWritten = 0;
	        for (int i = 0; i < chars.Length; i++)
	        {
	            char c = chars[i];
	            UnicodeCategory unicodeCategory = char.GetUnicodeCategory(c);
	            switch (unicodeCategory)
	            {
	                case UnicodeCategory.UppercaseLetter:
	                    switch (separatorState)
	                    {
	                        case SeparatorState.LowercaseLetterOrDigit:
	                        case SeparatorState.SpaceSeparator:
	                            WriteChar(separator, ref destination2);
	                            break;
	                        case SeparatorState.UppercaseLetter:
	                            if (i + 1 < chars.Length && char.IsLower(chars[i + 1]))
	                            {
	                                WriteChar(separator, ref destination2);
	                            }
	                            break;
	                    }
	                    if (lowercase)
	                    {
	                        c = char.ToLowerInvariant(c);
	                    }
	                    WriteChar(c, ref destination2);
	                    separatorState = SeparatorState.UppercaseLetter;
	                    break;
	                case UnicodeCategory.LowercaseLetter:
	                case UnicodeCategory.DecimalDigitNumber:
	                    if (separatorState == SeparatorState.SpaceSeparator)
	                    {
	                        WriteChar(separator, ref destination2);
	                    }
	                    if (!lowercase && unicodeCategory == UnicodeCategory.LowercaseLetter)
	                    {
	                        c = char.ToUpperInvariant(c);
	                    }
	                    WriteChar(c, ref destination2);
	                    separatorState = SeparatorState.LowercaseLetterOrDigit;
	                    break;
	                case UnicodeCategory.SpaceSeparator:
	                    if (separatorState != 0)
	                    {
	                        separatorState = SeparatorState.SpaceSeparator;
	                    }
	                    break;
	                default:
	                    WriteChar(c, ref destination2);
	                    separatorState = SeparatorState.NotStarted;
	                    break;
	            }
	        }
	        string result = destination2.Slice(0, charsWritten).ToString();
	        if (rentedBuffer != null)
	        {
	            destination2.Slice(0, charsWritten).Clear();
	            ArrayPool<char>.Shared.Return(rentedBuffer);
	        }
	        return result;
	        void ExpandBuffer(ref Span<char> destination)
	        {
	            int minimumLength = checked(destination.Length * 2);
	            char[] array = ArrayPool<char>.Shared.Rent(minimumLength);
	            destination.CopyTo(array);
	            if (rentedBuffer != null)
	            {
	                destination.Slice(0, charsWritten).Clear();
	                ArrayPool<char>.Shared.Return(rentedBuffer);
	            }
	            rentedBuffer = array;
	            destination = rentedBuffer;
	        }
	        [MethodImpl(MethodImplOptions.AggressiveInlining)]
	        void WriteChar(char value, ref Span<char> destination)
	        {
	            if (charsWritten == destination.Length)
	            {
	                ExpandBuffer(ref destination);
	            }
	            destination[charsWritten++] = value;
	        }
	    }
	}
	internal enum StringConversionStrategy
	{
	    None,
	    OnlyAlphaNumeric,
	}
	#endregion
	#region \Internal\Serialization
	internal class JsonKey
	{
	}
	internal class XmlGdmElementSerializer
	{
	}
	internal class XmlKey
	{
	    public const string PrimitiveType = "primitiveType";
	    public const string ComplexType = "complexType";
	    public const string EntityType = "entityType";
	}
	#endregion
	#region \Internal\Utilities
	internal static class GdmTypeHelper
	{
	    private static readonly IDictionary<Type, IOGraphGdmType> cache;
	    static GdmTypeHelper()
	    {
	        cache = new Dictionary<Type, IOGraphGdmType>();
	    }
	    public static IOGraphGdmType GetType<TType>() where TType : IOGraphGdmType, new()
	    {
	        IOGraphGdmType obj;
	        var type = typeof(TType);
	        if (cache.TryGetValue(type, out obj!))
	        {
	            return obj;
	        }
	        obj = new TType();
	        cache[type] = obj;
	        return obj;
	    }
	}
	internal static class GdmTypeResolver
	{
	    static GdmTypeResolver()
	    {
	        //Func<GdmProperty, bool> func = TryResolveBoolean;
	    }
	    //private static bool TryResolveBoolean(GdmProperty property)
	    //{
	    //    if (property.PropertyInfo.PropertyType == typeof(bool))
	    //    {
	    //        property.Type;
	    //    }
	    //}
	}
	internal static class ThrowHelper
	{
	    #region Common Exceptions
	    [DoesNotReturn]
	    public static void ThrowArgumentNullException(string paramName) =>
	        throw new ArgumentNullException(paramName);
	    [DoesNotReturn]
	    public static void ThrowArgumentException(string message) => 
	        throw new ArgumentException(message);
	    [DoesNotReturn]
	    public static void ThrowInvalidOperationException(string message) =>
	        throw new InvalidOperationException(message);
	    #endregion
	    [DoesNotReturn]
	    public static void ThrowUnknownError() =>
	        throw new GdmUnknownException();
	    #region Validation/Model Exceptions
	    [DoesNotReturn]
	    public static void ThrowComplexTypeKeyDisallowed() =>
	        throw new GdmModelException(GdmErrorCode.GDM0301, Resources.GDM0301);
	    [DoesNotReturn]
	    public static void ThrowComplexTypeKeyDisallowed(string source) =>
	        throw new GdmModelException(GdmErrorCode.GDM0301, source, Resources.GDM0301);
	    [DoesNotReturn]
	    public static void ThrowVertexInvalidTypeReferenceIsNotEntityType() =>
	        throw new GdmModelException(GdmErrorCode.GDM1001, Resources.GDM1001);
	    [DoesNotReturn]
	    public static void ThrowVertexInvalidTypeReferenceIsNotEntityType(string source) =>
	        throw new GdmModelException(GdmErrorCode.GDM1001, source, Resources.GDM1001);
	    [DoesNotReturn]
	    public static void ThrowVertexInvalidTypeReferenceIsNull() =>
	        throw new GdmModelException(GdmErrorCode.GDM1002, Resources.GDM1002);
	    [DoesNotReturn]
	    public static void ThrowVertexInvalidTypeReferenceIsNull(string source) =>
	        throw new GdmModelException(GdmErrorCode.GDM1002, source, Resources.GDM1002);
	    [DoesNotReturn]
	    public static void ThrowInvalidLabel(string source) =>
	        throw new GdmModelException(GdmErrorCode.GDM5001, source, Resources.GDM5001);
	    #endregion
	    #region Serialization Exceptions
	    [DoesNotReturn]
	    public static void ThrowInvalidContentException(string source) =>
	        throw new GdmSerializationException(GdmErrorCode.GDM3001, source, Resources.GDM3001);
	    [DoesNotReturn]
	    public static void ThrowInvalidTypeSerializationException(Type expected, Type received) =>
	        throw new InvalidOperationException($"Invalid type serialization. Expected type {expected.Name}. Received type {received.Name}");
	    #endregion
	}
	#endregion
	#region \Internal\Validation
	internal class GdmValidator
	{
	    private static readonly IList<GdmValidatorRule> rules = new List<GdmValidatorRule>();
	    static GdmValidator()
	    {
	        AddRule<GdmComplexTypeCheckRule>();
	        AddRule<GdmComplexTypeKeyDisallowedValidatorRule>();
	        AddRule<GdmEntityTypeMissingKeyValidatorRule>();
	        AddRule<GdmVertexInvalidTypeReferenceIsNullValidatorRule>();
	        AddRule<GdmVertexInvalidTypeReferenceNotEntityTypeValidatorRule>();
	    }
	    private static void AddRule<TRule>() where TRule : GdmValidatorRule, new()
	    {
	        rules.Add(new TRule());
	    }
	    public GdmValidatorResult Validate(IOGraphGdm model)
	    {
	        var result = new GdmValidatorResult();
	        var context = new GdmValidatorContext() 
	        { 
	            Model = model,
	            Errors = result.Errors
	        };
	        foreach (var rule in rules)
	        {
	            rule.OnValidate(context);
	        }
	        return result;
	    }
	}
	internal class GdmValidatorContext
	{
	    public IOGraphGdm Model { get; init; } = default!;
	    public IList<GdmValidatorError> Errors { get; init; } = default!;
	    public void AddFailure(GdmValidatorError error)
	    {
	        if (error is null)
	        {
	            throw new ArgumentNullException(nameof(error));
	        }
	        Errors.Add(error);
	    }
	    public void AddFailure(Action<GdmValidatorError> configure)
	    {
	        var error = new GdmValidatorError();
	        configure.Invoke(error);
	        AddFailure(error);
	    }
	}
	internal class GdmValidatorError
	{
	    public GdmErrorCode Code { get; set; }
	    public string? Message { get; set; }
	    public string? Source { get; set; }
	}
	internal class GdmValidatorResult
	{
	    public GdmValidatorResult()
	    {
	        Errors = new List<GdmValidatorError>();
	    }
	    public bool IsValid => !Errors.Any();
	    public IList<GdmValidatorError> Errors { get; }
	    public AggregateException ToException()
	    {
	        return new AggregateException(Errors.Select(p => new GdmModelException(p)));
	    }
	}
	internal abstract class GdmValidatorRule
	{
	    public abstract void OnValidate(GdmValidatorContext context);
	}
	#endregion
	#region \Internal\Validation\Rules
	internal class GdmComplexTypeCheckRule : GdmValidatorRule
	{
	    /*
	        Rules:
	            1. No Complex Type property should be a key 
	     */
	    public override void OnValidate(GdmValidatorContext context)
	    {
	        var complexTypes = context.Model.Elements.OfType<IOGraphGdmComplexType>();
	        foreach (var complexType in complexTypes)
	        {
	            var runtimeType = complexType.RuntimeType;
	            if (runtimeType is null)
	            {
	                context.AddFailure(error =>
	                {
	                    error.Message = "";
	                    error.Source = "";
	                });
	            }
	            else if (!runtimeType.IsClass)
	            {
	                context.AddFailure(error =>
	                {
	                    error.Message = "";
	                    error.Source = "";
	                });
	            }
	            else if (runtimeType.IsAbstract)
	            {
	                context.AddFailure(error =>
	                {
	                    error.Message = "";
	                    error.Source = "";
	                });
	            }
	            // Since a delegate are actually compiled into a class. Let's check that 
	            else if (runtimeType.IsSubclassOf(typeof(Delegate)))
	            {
	                context.AddFailure(error =>
	                {
	                    error.Message = "Delegates are not allowed as complex types.";
	                    error.Source = complexType.Label;
	                });
	            }
	            // 3. Check if type has default constructor
	            else if (runtimeType.GetConstructor(Type.EmptyTypes) is null)
	            {
	                context.AddFailure(error =>
	                {
	                    error.Message = $"The type {runtimeType.Name} does not have a default constructor. {runtimeType.Name}.ctor()";
	                    error.Source = complexType.Label;
	                });
	            }
	            else if (runtimeType.IsAssignableTo(typeof(string)))
	            {
	                context.AddFailure(error =>
	                {
	                    error.Message = "System.String is not allowed as a complex type.";
	                    error.Source = complexType.Label;
	                });
	            }
	        }
	    }
	}
	internal class GdmComplexTypeKeyDisallowedValidatorRule : GdmValidatorRule
	{
	    public override void OnValidate(GdmValidatorContext context)
	    {
	        //var complexTypes = context.Model.GetGdmComplexTypes()
	        //    .Where(p => p is not IOGraphGdmEntityType);
	        //foreach (var complexType in complexTypes)
	        //{
	        //    var property = complexType.Properties.FirstOrDefault(p => p.IsKey);
	        //    if (property is not null)
	        //    {
	        //        context.AddFailure(error =>
	        //        {
	        //            error.Code = GdmErrorCode.GDM0301;
	        //            error.Message = Resources.GDM0301;
	        //            error.Source = $"{complexType.Label}.{property.Label}";
	        //        });
	        //    }
	        //}
	    }
	}
	internal class GdmEntityTypeMissingKeyValidatorRule : GdmValidatorRule
	{
	    public override void OnValidate(GdmValidatorContext context)
	    {
	        //var entityTypes = context.Model.GetGdmEntityTypes();
	        //foreach (var entityType in entityTypes)
	        //{
	        //    var property = entityType.Properties.FirstOrDefault(p => p.IsKey);
	        //    if (property is null)
	        //    {
	        //        context.AddFailure(error =>
	        //        {
	        //            error.Code = GdmErrorCode.GDM0401;
	        //            error.Message = Resources.GDM0401;
	        //            error.Source = $"{entityType.Label}";
	        //        });
	        //    }
	        //}
	    }
	}
	internal class GdmVertexInvalidTypeReferenceIsNullValidatorRule : GdmValidatorRule
	{
	    public override void OnValidate(GdmValidatorContext context)
	    {
	        var vertices = context.Model.GetGdmVertices();
	        foreach (var vertex in vertices)
	        {
	            if (vertex.Type is null || vertex.Type.Definition is null)
	            {
	                context.AddFailure(error =>
	                {
	                    error.Code = GdmErrorCode.GDM1002;
	                    error.Message = Resources.GDM1002;
	                    error.Source = vertex.Label;
	                });
	            }
	        }
	    }
	}
	internal class GdmVertexInvalidTypeReferenceNotEntityTypeValidatorRule : GdmValidatorRule
	{
	    public override void OnValidate(GdmValidatorContext context)
	    {
	        var vertices = context.Model.GetGdmVertices();
	        foreach (var vertex in vertices)
	        {
	            if (vertex.Type is not null && vertex.Type.Definition is not null && vertex.Type.Definition is not IOGraphGdmEntityType)
	            {
	                context.AddFailure(error =>
	                {
	                    error.Code = GdmErrorCode.GDM1001;
	                    error.Message = Resources.GDM1001;
	                    error.Source = vertex.Label;
	                });
	            }
	        }
	    }
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
	#region \Properties
	    // This class was auto-generated by the StronglyTypedResourceBuilder
	    // class via a tool like ResGen or Visual Studio.
	    // To add or remove a member, edit your .ResX file then rerun ResGen
	    // with the /str option, or rebuild your VS project.
	    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
	    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
	    internal class Resources {
	        private static global::System.Resources.ResourceManager resourceMan;
	        private static global::System.Globalization.CultureInfo resourceCulture;
	        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
	        internal Resources() {
	        }
	        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
	        internal static global::System.Resources.ResourceManager ResourceManager {
	            get {
	                if (object.ReferenceEquals(resourceMan, null)) {
	                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Assimalign.OGraph.Gdm.Properties.Resources", typeof(Resources).Assembly);
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
	        internal static string GDM0000 {
	            get {
	                return ResourceManager.GetString("GDM0000", resourceCulture);
	            }
	        }
	        internal static string GDM0301 {
	            get {
	                return ResourceManager.GetString("GDM0301", resourceCulture);
	            }
	        }
	        internal static string GDM0401 {
	            get {
	                return ResourceManager.GetString("GDM0401", resourceCulture);
	            }
	        }
	        internal static string GDM1001 {
	            get {
	                return ResourceManager.GetString("GDM1001", resourceCulture);
	            }
	        }
	        internal static string GDM1002 {
	            get {
	                return ResourceManager.GetString("GDM1002", resourceCulture);
	            }
	        }
	        internal static string GDM3001 {
	            get {
	                return ResourceManager.GetString("GDM3001", resourceCulture);
	            }
	        }
	        internal static string GDM5001 {
	            get {
	                return ResourceManager.GetString("GDM5001", resourceCulture);
	            }
	        }
	    }
	#endregion
}
#endregion
