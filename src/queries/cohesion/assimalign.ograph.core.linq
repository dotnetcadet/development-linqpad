<Query Kind="Program">
<Namespace>System.Collections.Generic</Namespace>
<Namespace>System</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>System.Threading.Tasks</Namespace>
<Namespace>System.Reflection</Namespace>
<Namespace>System.Collections.Concurrent</Namespace>
<Namespace>System.Collections.Immutable</Namespace>
<Namespace>System.ComponentModel</Namespace>
<Namespace>System.Linq.Expressions</Namespace>
<Namespace>System.Runtime.CompilerServices</Namespace>
</Query>
#load ".\assimalign.ograph.gdm"
#load ".\assimalign.ograph.syntax"

void Main()
{

}

#region Assimalign.OGraph.Core(net8.0)
namespace Assimalign.OGraph
{
	#region \
	public sealed class OGraphError : IOGraphError
	{
	    public OGraphError()
	    {
	        Details = new();
	    }
	    public OGraphError(string code, string message) : this()
	    {
	        Code = code;
	        Message = message;
	    }
	    public string? Code { get; set; }
	    public string? Message { get; set; }
	    public OGraphErrorDetailsCollection Details { get; }
	    IOGraphErrorDetailsCollection IOGraphError.Details => Details;
	}
	public sealed class OGraphErrorDetails : IOGraphErrorDetails
	{
	    public string? Title { get; set; }
	    public string? Message { get; set; }
	}
	public sealed class OGraphErrorDetailsCollection : List<IOGraphErrorDetails>, 
	    IOGraphErrorDetailsCollection
	{
	}
	#endregion
	#region \Abstractions
	public interface IOGraphError
	{
	    string? Code { get; }
	    string? Message { get; }
	    IOGraphErrorDetailsCollection? Details { get; }
	}
	public interface IOGraphErrorDetails
	{
	    public string? Title { get; }
	    public string? Message { get; }
	}
	public interface IOGraphErrorDetailsCollection : ICollection<IOGraphErrorDetails>
	{
	}
	#endregion
	#region \Exceptions
	    internal class OGraphErrorCode
	    {
	    }
	public abstract class OGraphException : Exception
	{
	    public OGraphException() { }
	    public OGraphException(string message) : base(message) { }
	    public OGraphException(string message, Exception innerException): base(message, innerException) { }
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
	#region \SourceGeneration
	[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
	public sealed class EntityKeyAttribute : Attribute
	{
	    public EntityKeyAttribute(EntityKeyType keyType)
	    {
	        KeyType = keyType;
	    }
	    public EntityKeyType KeyType { get; }
	}
	public enum EntityKeyType
	{
	    String,
	    Int,
	    Long,
	    Guid
	}
	public sealed class ValueObjectAttribute : Attribute
	{
	    public ValueObjectAttribute()
	    {
	    }
	}
	#endregion
	#region \Utilities\Either
	public record Either<T1, T2> : IEither
	{
	    #region Implicit Conversion From Value
	    public static implicit operator Either<T1, T2>(T1 value) => new Either<T1, T2>(value);
	    public static implicit operator Either<T1, T2>(T2 value) => new Either<T1, T2>(value);
	    #endregion Implicit Conversion From Value
	    #region Implicit Conversion By Type Swap
	    public static implicit operator Either<T1, T2>(Either<T2, T1> other)
	    {
	        int[] map = new[] { 2, 1 };
	        return new Either<T1, T2>(map[other._typeIndex - 1], other._value);
	    }
	    #endregion Implicit Conversion By Type Swap
	    #region Implicit Widening Conversions
	    #endregion Implicit Widening Conversions
	    #region Constructors
	    public Either(T1 value) { _value = value; _typeIndex = 1; }
	    public Either(T2 value) { _value = value; _typeIndex = 2; }
	    #endregion Constructors
	    #region Or methods
	    public Either<T1, T2, T3> Or<T3>() => this
	        .Match((T1 v1) => new Either<T1, T2, T3>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3>(v2));
	    public Either<T1, T2, T3, T4> Or<T3, T4>() => this
	        .Match((T1 v1) => new Either<T1, T2, T3, T4>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4>(v2));
	    public Either<T1, T2, T3, T4, T5> Or<T3, T4, T5>() => this
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2));
	    #endregion Or methods
	    #region IEither Implementation
	    int _typeIndex;
	    object _value;
	    int IEither.TypeIndex => _typeIndex;
	    Type IEither.Type => _typeIndex switch
	    {
	        1 => typeof(T1),
	        2 => typeof(T2),
	        _ => throw new InvalidOperationException()
	    };
	    object IEither.Value => _value;
	    Either(int typeIndex, object value) => (_typeIndex, _value) = (typeIndex, value);
	    #endregion IEither Implementation
	    #region Value Casts
	    T1 AsT1 => (T1)_value;
	    T2 AsT2 => (T2)_value;
	    #endregion Value Casts
	    #region Explicit Casts
	    public static explicit operator T1(Either<T1, T2> either) => either.AsT1;
	    public static explicit operator T2(Either<T1, T2> either) => either.AsT2;
	    #endregion Explicit Casts
	    #region Switch method
	    public void Switch(Action<T1> ifT1, Action<T2> ifT2)
	    {
	        switch (_typeIndex)
	        {
	            case 1: ifT1(AsT1); break;
	            case 2: ifT2(AsT2); break;
	            default: throw new InvalidOperationException();
	        }
	    }
	    #endregion Switch method
	    #region Nonreductive Match
	    public Either<TResult1, T2> Match<TResult1>(Func<T1, TResult1> ifT1) => _typeIndex switch
	    {
	        1 => ifT1(AsT1),
	        2 => AsT2,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, TResult1> Match<TResult1>
	        (Func<T1, TResult1> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, TResult2> Match<TResult2>(Func<T2, TResult2> ifT2) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => ifT2(AsT2),
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, TResult2> Match<TResult2>
	        (Func<T2, TResult2> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Nonreductive Match
	    #region Nonreductive Match - Compositional
	    public Either<TResult1, T2> Match<TResult1>(Func<T1, Either<TResult1, T2>> ifT1) => _typeIndex switch
	    {
	        1 => ifT1(AsT1),
	        2 => AsT2,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, TResult1> Match<TResult1>
	        (Func<T1, Either<T1, T2, TResult1>> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, TResult2> Match<TResult2>(Func<T2, Either<T1, TResult2>> ifT2) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => ifT2(AsT2),
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, TResult2> Match<TResult2>
	        (Func<T2, Either<T1, T2, TResult2>> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Nonreductive Match - Compositional
	    #region Reductive Match
	    public T2 Match
	        (Func<T1, T2> ifT1) => _typeIndex switch
	        {
	            1 => ifT1(AsT1),
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2> Match
	        (Func<T1, T2> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public T1 Match
	        (Func<T2, T1> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => ifT2(AsT2),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2> Match
	        (Func<T2, T1> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Reductive Match
	    #region Throw Methods
	    public T2 ThrowIf
	        (Func<T1, Exception> ifT1) => _typeIndex switch
	        {
	            1 => throw ifT1(AsT1),
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2> ThrowIf
	        (Func<T1, Exception> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => throw ifT1(AsT1),
	            2 => AsT2,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public T1 ThrowIf
	        (Func<T2, Exception> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => throw ifT2(AsT2),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2> ThrowIf
	        (Func<T2, Exception> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => throw ifT2(AsT2),
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Throw Methods
	    #region If (methods)
	    public bool If(out T1 @if) => If(out @if, out _);
	    public bool If(out T1 @if, out T2 @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = AsT1;
	                @else = default;
	                return true;
	            case 2:
	                @if = default;
	                @else = AsT2;
	                return false;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    public bool If(out T2 @if) => If(out @if, out _);
	    public bool If(out T2 @if, out T1 @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = default;
	                @else = AsT1;
	                return false;
	            case 2:
	                @if = AsT2;
	                @else = default;
	                return true;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    #endregion If (methods)
	    #region ToString
	    public override string ToString() => $"{((IEither)this).Type.Name}:{_value}";
	    // For LINQPad:
	    object ToDump() => new { Type = ((IEither)this).Type, Value = ((IEither)this).Value };
	    #endregion ToString
	}
	public record Either<T1, T2, T3> : IEither
	{
	    #region Implicit Conversion From Value
	    public static implicit operator Either<T1, T2, T3>(T1 value) => new Either<T1, T2, T3>(value);
	    public static implicit operator Either<T1, T2, T3>(T2 value) => new Either<T1, T2, T3>(value);
	    public static implicit operator Either<T1, T2, T3>(T3 value) => new Either<T1, T2, T3>(value);
	    #endregion Implicit Conversion From Value
	    #region Implicit Conversion By Type Swap
	    public static implicit operator Either<T1, T2, T3>(Either<T1, T3, T2> other)
	    {
	        int[] map = new[] { 1, 3, 2 };
	        return new Either<T1, T2, T3>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3>(Either<T2, T1, T3> other)
	    {
	        int[] map = new[] { 2, 1, 3 };
	        return new Either<T1, T2, T3>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3>(Either<T2, T3, T1> other)
	    {
	        int[] map = new[] { 2, 3, 1 };
	        return new Either<T1, T2, T3>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3>(Either<T3, T1, T2> other)
	    {
	        int[] map = new[] { 3, 1, 2 };
	        return new Either<T1, T2, T3>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3>(Either<T3, T2, T1> other)
	    {
	        int[] map = new[] { 3, 2, 1 };
	        return new Either<T1, T2, T3>(map[other._typeIndex - 1], other._value);
	    }
	    #endregion Implicit Conversion By Type Swap
	    #region Implicit Widening Conversions
	    public static implicit operator Either<T1, T2, T3>(Either<T1, T2> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3>(v2));
	    public static implicit operator Either<T1, T2, T3>(Either<T1, T3> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3>(v1))
	        .Match((T3 v3) => new Either<T1, T2, T3>(v3));
	    public static implicit operator Either<T1, T2, T3>(Either<T2, T3> other) => other
	        .Match((T2 v2) => new Either<T1, T2, T3>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3>(v3));
	    #endregion Implicit Widening Conversions
	    #region Constructors
	    public Either(T1 value) { _value = value; _typeIndex = 1; }
	    public Either(T2 value) { _value = value; _typeIndex = 2; }
	    public Either(T3 value) { _value = value; _typeIndex = 3; }
	    #endregion Constructors
	    #region Or methods
	    public Either<T1, T2, T3, T4> Or<T4>() => this
	        .Match((T1 v1) => new Either<T1, T2, T3, T4>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4>(v3));
	    public Either<T1, T2, T3, T4, T5> Or<T4, T5>() => this
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3));
	    #endregion Or methods
	    #region IEither Implementation
	    int _typeIndex;
	    object _value;
	    int IEither.TypeIndex => _typeIndex;
	    Type IEither.Type => _typeIndex switch
	    {
	        1 => typeof(T1),
	        2 => typeof(T2),
	        3 => typeof(T3),
	        _ => throw new InvalidOperationException()
	    };
	    object IEither.Value => _value;
	    Either(int typeIndex, object value) => (_typeIndex, _value) = (typeIndex, value);
	    #endregion IEither Implementation
	    #region Value Casts
	    T1 AsT1 => (T1)_value;
	    T2 AsT2 => (T2)_value;
	    T3 AsT3 => (T3)_value;
	    #endregion Value Casts
	    #region Explicit Casts
	    public static explicit operator T1(Either<T1, T2, T3> either) => either.AsT1;
	    public static explicit operator T2(Either<T1, T2, T3> either) => either.AsT2;
	    public static explicit operator T3(Either<T1, T2, T3> either) => either.AsT3;
	    #endregion Explicit Casts
	    #region Switch method
	    public void Switch(Action<T1> ifT1, Action<T2> ifT2, Action<T3> ifT3)
	    {
	        switch (_typeIndex)
	        {
	            case 1: ifT1(AsT1); break;
	            case 2: ifT2(AsT2); break;
	            case 3: ifT3(AsT3); break;
	            default: throw new InvalidOperationException();
	        }
	    }
	    #endregion Switch method
	    #region Nonreductive Match
	    public Either<TResult1, T2, T3> Match<TResult1>(Func<T1, TResult1> ifT1) => _typeIndex switch
	    {
	        1 => ifT1(AsT1),
	        2 => AsT2,
	        3 => AsT3,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, TResult1> Match<TResult1>
	        (Func<T1, TResult1> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, TResult2, T3> Match<TResult2>(Func<T2, TResult2> ifT2) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => ifT2(AsT2),
	        3 => AsT3,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, TResult2> Match<TResult2>
	        (Func<T2, TResult2> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, TResult3> Match<TResult3>(Func<T3, TResult3> ifT3) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => ifT3(AsT3),
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, TResult3> Match<TResult3>
	        (Func<T3, TResult3> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Nonreductive Match
	    #region Nonreductive Match - Compositional
	    public Either<TResult1, T2, T3> Match<TResult1>(Func<T1, Either<TResult1, T2, T3>> ifT1) => _typeIndex switch
	    {
	        1 => ifT1(AsT1),
	        2 => AsT2,
	        3 => AsT3,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, TResult1> Match<TResult1>
	        (Func<T1, Either<T1, T2, T3, TResult1>> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, TResult2, T3> Match<TResult2>(Func<T2, Either<T1, TResult2, T3>> ifT2) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => ifT2(AsT2),
	        3 => AsT3,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, TResult2> Match<TResult2>
	        (Func<T2, Either<T1, T2, T3, TResult2>> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, TResult3> Match<TResult3>(Func<T3, Either<T1, T2, TResult3>> ifT3) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => ifT3(AsT3),
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, TResult3> Match<TResult3>
	        (Func<T3, Either<T1, T2, T3, TResult3>> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Nonreductive Match - Compositional
	    #region Reductive Match
	    public Either<T2, T3> Match
	        (Func<T1, T2> ifT1) => _typeIndex switch
	        {
	            1 => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> Match
	        (Func<T1, T2> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3> Match
	        (Func<T2, T1> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => ifT2(AsT2),
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> Match
	        (Func<T2, T1> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T2, T3> Match
	        (Func<T1, T3> ifT1) => _typeIndex switch
	        {
	            1 => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> Match
	        (Func<T1, T3> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2> Match
	        (Func<T3, T1> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => ifT3(AsT3),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> Match
	        (Func<T3, T1> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3> Match
	        (Func<T2, T3> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => ifT2(AsT2),
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> Match
	        (Func<T2, T3> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2> Match
	        (Func<T3, T2> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => ifT3(AsT3),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> Match
	        (Func<T3, T2> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Reductive Match
	    #region Throw Methods
	    public Either<T2, T3> ThrowIf
	        (Func<T1, Exception> ifT1) => _typeIndex switch
	        {
	            1 => throw ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> ThrowIf
	        (Func<T1, Exception> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => throw ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3> ThrowIf
	        (Func<T2, Exception> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => throw ifT2(AsT2),
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> ThrowIf
	        (Func<T2, Exception> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => throw ifT2(AsT2),
	            3 => AsT3,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2> ThrowIf
	        (Func<T3, Exception> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => throw ifT3(AsT3),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> ThrowIf
	        (Func<T3, Exception> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => throw ifT3(AsT3),
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Throw Methods
	    #region If (methods)
	    public bool If(out T1 @if) => If(out @if, out _);
	    public bool If(out T1 @if, out Either<T2, T3> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = AsT1;
	                @else = default;
	                return true;
	            case 2:
	                @if = default;
	                @else = AsT2;
	                return false;
	            case 3:
	                @if = default;
	                @else = AsT3;
	                return false;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    public bool If(out T2 @if) => If(out @if, out _);
	    public bool If(out T2 @if, out Either<T1, T3> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = default;
	                @else = AsT1;
	                return false;
	            case 2:
	                @if = AsT2;
	                @else = default;
	                return true;
	            case 3:
	                @if = default;
	                @else = AsT3;
	                return false;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    public bool If(out T3 @if) => If(out @if, out _);
	    public bool If(out T3 @if, out Either<T1, T2> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = default;
	                @else = AsT1;
	                return false;
	            case 2:
	                @if = default;
	                @else = AsT2;
	                return false;
	            case 3:
	                @if = AsT3;
	                @else = default;
	                return true;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    #endregion If (methods)
	    #region ToString
	    public override string ToString() => $"{((IEither)this).Type.Name}:{_value}";
	    // For LINQPad:
	    object ToDump() => new { Type = ((IEither)this).Type, Value = ((IEither)this).Value };
	    #endregion ToString
	}
	public record Either<T1, T2, T3, T4> : IEither
	{
	    #region Implicit Conversion From Value
	    public static implicit operator Either<T1, T2, T3, T4>(T1 value) => new Either<T1, T2, T3, T4>(value);
	    public static implicit operator Either<T1, T2, T3, T4>(T2 value) => new Either<T1, T2, T3, T4>(value);
	    public static implicit operator Either<T1, T2, T3, T4>(T3 value) => new Either<T1, T2, T3, T4>(value);
	    public static implicit operator Either<T1, T2, T3, T4>(T4 value) => new Either<T1, T2, T3, T4>(value);
	    #endregion Implicit Conversion From Value
	    #region Implicit Conversion By Type Swap
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T1, T2, T4, T3> other)
	    {
	        int[] map = new[] { 1, 2, 4, 3 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T1, T3, T2, T4> other)
	    {
	        int[] map = new[] { 1, 3, 2, 4 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T1, T3, T4, T2> other)
	    {
	        int[] map = new[] { 1, 3, 4, 2 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T1, T4, T2, T3> other)
	    {
	        int[] map = new[] { 1, 4, 2, 3 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T1, T4, T3, T2> other)
	    {
	        int[] map = new[] { 1, 4, 3, 2 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T2, T1, T3, T4> other)
	    {
	        int[] map = new[] { 2, 1, 3, 4 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T2, T1, T4, T3> other)
	    {
	        int[] map = new[] { 2, 1, 4, 3 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T2, T3, T1, T4> other)
	    {
	        int[] map = new[] { 2, 3, 1, 4 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T2, T3, T4, T1> other)
	    {
	        int[] map = new[] { 2, 3, 4, 1 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T2, T4, T1, T3> other)
	    {
	        int[] map = new[] { 2, 4, 1, 3 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T2, T4, T3, T1> other)
	    {
	        int[] map = new[] { 2, 4, 3, 1 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T3, T1, T2, T4> other)
	    {
	        int[] map = new[] { 3, 1, 2, 4 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T3, T1, T4, T2> other)
	    {
	        int[] map = new[] { 3, 1, 4, 2 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T3, T2, T1, T4> other)
	    {
	        int[] map = new[] { 3, 2, 1, 4 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T3, T2, T4, T1> other)
	    {
	        int[] map = new[] { 3, 2, 4, 1 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T3, T4, T1, T2> other)
	    {
	        int[] map = new[] { 3, 4, 1, 2 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T3, T4, T2, T1> other)
	    {
	        int[] map = new[] { 3, 4, 2, 1 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T4, T1, T2, T3> other)
	    {
	        int[] map = new[] { 4, 1, 2, 3 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T4, T1, T3, T2> other)
	    {
	        int[] map = new[] { 4, 1, 3, 2 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T4, T2, T1, T3> other)
	    {
	        int[] map = new[] { 4, 2, 1, 3 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T4, T2, T3, T1> other)
	    {
	        int[] map = new[] { 4, 2, 3, 1 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T4, T3, T1, T2> other)
	    {
	        int[] map = new[] { 4, 3, 1, 2 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T4, T3, T2, T1> other)
	    {
	        int[] map = new[] { 4, 3, 2, 1 };
	        return new Either<T1, T2, T3, T4>(map[other._typeIndex - 1], other._value);
	    }
	    #endregion Implicit Conversion By Type Swap
	    #region Implicit Widening Conversions
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T1, T2> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4>(v2));
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T1, T3> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4>(v1))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4>(v3));
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T1, T4> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4>(v1))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4>(v4));
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T2, T3> other) => other
	        .Match((T2 v2) => new Either<T1, T2, T3, T4>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4>(v3));
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T2, T4> other) => other
	        .Match((T2 v2) => new Either<T1, T2, T3, T4>(v2))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4>(v4));
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T3, T4> other) => other
	        .Match((T3 v3) => new Either<T1, T2, T3, T4>(v3))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4>(v4));
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T1, T2, T3> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4>(v3));
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T1, T2, T4> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4>(v2))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4>(v4));
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T1, T3, T4> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4>(v1))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4>(v3))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4>(v4));
	    public static implicit operator Either<T1, T2, T3, T4>(Either<T2, T3, T4> other) => other
	        .Match((T2 v2) => new Either<T1, T2, T3, T4>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4>(v3))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4>(v4));
	    #endregion Implicit Widening Conversions
	    #region Constructors
	    public Either(T1 value) { _value = value; _typeIndex = 1; }
	    public Either(T2 value) { _value = value; _typeIndex = 2; }
	    public Either(T3 value) { _value = value; _typeIndex = 3; }
	    public Either(T4 value) { _value = value; _typeIndex = 4; }
	    #endregion Constructors
	    #region Or methods
	    public Either<T1, T2, T3, T4, T5> Or<T5>() => this
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4));
	    #endregion Or methods
	    #region IEither Implementation
	    int _typeIndex;
	    object _value;
	    int IEither.TypeIndex => _typeIndex;
	    Type IEither.Type => _typeIndex switch
	    {
	        1 => typeof(T1),
	        2 => typeof(T2),
	        3 => typeof(T3),
	        4 => typeof(T4),
	        _ => throw new InvalidOperationException()
	    };
	    object IEither.Value => _value;
	    Either(int typeIndex, object value) => (_typeIndex, _value) = (typeIndex, value);
	    #endregion IEither Implementation
	    #region Value Casts
	    T1 AsT1 => (T1)_value;
	    T2 AsT2 => (T2)_value;
	    T3 AsT3 => (T3)_value;
	    T4 AsT4 => (T4)_value;
	    #endregion Value Casts
	    #region Explicit Casts
	    public static explicit operator T1(Either<T1, T2, T3, T4> either) => either.AsT1;
	    public static explicit operator T2(Either<T1, T2, T3, T4> either) => either.AsT2;
	    public static explicit operator T3(Either<T1, T2, T3, T4> either) => either.AsT3;
	    public static explicit operator T4(Either<T1, T2, T3, T4> either) => either.AsT4;
	    #endregion Explicit Casts
	    #region Switch method
	    public void Switch(Action<T1> ifT1, Action<T2> ifT2, Action<T3> ifT3, Action<T4> ifT4)
	    {
	        switch (_typeIndex)
	        {
	            case 1: ifT1(AsT1); break;
	            case 2: ifT2(AsT2); break;
	            case 3: ifT3(AsT3); break;
	            case 4: ifT4(AsT4); break;
	            default: throw new InvalidOperationException();
	        }
	    }
	    #endregion Switch method
	    #region Nonreductive Match
	    public Either<TResult1, T2, T3, T4> Match<TResult1>(Func<T1, TResult1> ifT1) => _typeIndex switch
	    {
	        1 => ifT1(AsT1),
	        2 => AsT2,
	        3 => AsT3,
	        4 => AsT4,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, T4, TResult1> Match<TResult1>
	        (Func<T1, TResult1> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, TResult2, T3, T4> Match<TResult2>(Func<T2, TResult2> ifT2) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => ifT2(AsT2),
	        3 => AsT3,
	        4 => AsT4,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, T4, TResult2> Match<TResult2>
	        (Func<T2, TResult2> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, TResult3, T4> Match<TResult3>(Func<T3, TResult3> ifT3) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => ifT3(AsT3),
	        4 => AsT4,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, T4, TResult3> Match<TResult3>
	        (Func<T3, TResult3> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            4 => AsT4,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, TResult4> Match<TResult4>(Func<T4, TResult4> ifT4) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => AsT3,
	        4 => ifT4(AsT4),
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, T4, TResult4> Match<TResult4>
	        (Func<T4, TResult4> ifT4, Func<T4, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 when (when(AsT4)) => ifT4(AsT4),
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Nonreductive Match
	    #region Nonreductive Match - Compositional
	    public Either<TResult1, T2, T3, T4> Match<TResult1>(Func<T1, Either<TResult1, T2, T3, T4>> ifT1) => _typeIndex switch
	    {
	        1 => ifT1(AsT1),
	        2 => AsT2,
	        3 => AsT3,
	        4 => AsT4,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, T4, TResult1> Match<TResult1>
	        (Func<T1, Either<T1, T2, T3, T4, TResult1>> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, TResult2, T3, T4> Match<TResult2>(Func<T2, Either<T1, TResult2, T3, T4>> ifT2) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => ifT2(AsT2),
	        3 => AsT3,
	        4 => AsT4,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, T4, TResult2> Match<TResult2>
	        (Func<T2, Either<T1, T2, T3, T4, TResult2>> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, TResult3, T4> Match<TResult3>(Func<T3, Either<T1, T2, TResult3, T4>> ifT3) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => ifT3(AsT3),
	        4 => AsT4,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, T4, TResult3> Match<TResult3>
	        (Func<T3, Either<T1, T2, T3, T4, TResult3>> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            4 => AsT4,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, TResult4> Match<TResult4>(Func<T4, Either<T1, T2, T3, TResult4>> ifT4) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => AsT3,
	        4 => ifT4(AsT4),
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, T4, TResult4> Match<TResult4>
	        (Func<T4, Either<T1, T2, T3, T4, TResult4>> ifT4, Func<T4, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 when (when(AsT4)) => ifT4(AsT4),
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Nonreductive Match - Compositional
	    #region Reductive Match
	    public Either<T2, T3, T4> Match
	        (Func<T1, T2> ifT1) => _typeIndex switch
	        {
	            1 => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T1, T2> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3, T4> Match
	        (Func<T2, T1> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T2, T1> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T2, T3, T4> Match
	        (Func<T1, T3> ifT1) => _typeIndex switch
	        {
	            1 => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T1, T3> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T4> Match
	        (Func<T3, T1> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => ifT3(AsT3),
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T3, T1> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            4 => AsT4,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T2, T3, T4> Match
	        (Func<T1, T4> ifT1) => _typeIndex switch
	        {
	            1 => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T1, T4> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> Match
	        (Func<T4, T1> ifT4) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => ifT4(AsT4),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T4, T1> ifT4, Func<T4, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 when (when(AsT4)) => ifT4(AsT4),
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3, T4> Match
	        (Func<T2, T3> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T2, T3> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T4> Match
	        (Func<T3, T2> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => ifT3(AsT3),
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T3, T2> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            4 => AsT4,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3, T4> Match
	        (Func<T2, T4> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T2, T4> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> Match
	        (Func<T4, T2> ifT4) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => ifT4(AsT4),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T4, T2> ifT4, Func<T4, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 when (when(AsT4)) => ifT4(AsT4),
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T4> Match
	        (Func<T3, T4> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => ifT3(AsT3),
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T3, T4> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            4 => AsT4,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> Match
	        (Func<T4, T3> ifT4) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => ifT4(AsT4),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T4, T3> ifT4, Func<T4, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 when (when(AsT4)) => ifT4(AsT4),
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Reductive Match
	    #region Throw Methods
	    public Either<T2, T3, T4> ThrowIf
	        (Func<T1, Exception> ifT1) => _typeIndex switch
	        {
	            1 => throw ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> ThrowIf
	        (Func<T1, Exception> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => throw ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3, T4> ThrowIf
	        (Func<T2, Exception> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => throw ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> ThrowIf
	        (Func<T2, Exception> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => throw ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T4> ThrowIf
	        (Func<T3, Exception> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => throw ifT3(AsT3),
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> ThrowIf
	        (Func<T3, Exception> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => throw ifT3(AsT3),
	            4 => AsT4,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3> ThrowIf
	        (Func<T4, Exception> ifT4) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => throw ifT4(AsT4),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> ThrowIf
	        (Func<T4, Exception> ifT4, Func<T4, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 when (when(AsT4)) => throw ifT4(AsT4),
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Throw Methods
	    #region If (methods)
	    public bool If(out T1 @if) => If(out @if, out _);
	    public bool If(out T1 @if, out Either<T2, T3, T4> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = AsT1;
	                @else = default;
	                return true;
	            case 2:
	                @if = default;
	                @else = AsT2;
	                return false;
	            case 3:
	                @if = default;
	                @else = AsT3;
	                return false;
	            case 4:
	                @if = default;
	                @else = AsT4;
	                return false;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    public bool If(out T2 @if) => If(out @if, out _);
	    public bool If(out T2 @if, out Either<T1, T3, T4> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = default;
	                @else = AsT1;
	                return false;
	            case 2:
	                @if = AsT2;
	                @else = default;
	                return true;
	            case 3:
	                @if = default;
	                @else = AsT3;
	                return false;
	            case 4:
	                @if = default;
	                @else = AsT4;
	                return false;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    public bool If(out T3 @if) => If(out @if, out _);
	    public bool If(out T3 @if, out Either<T1, T2, T4> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = default;
	                @else = AsT1;
	                return false;
	            case 2:
	                @if = default;
	                @else = AsT2;
	                return false;
	            case 3:
	                @if = AsT3;
	                @else = default;
	                return true;
	            case 4:
	                @if = default;
	                @else = AsT4;
	                return false;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    public bool If(out T4 @if) => If(out @if, out _);
	    public bool If(out T4 @if, out Either<T1, T2, T3> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = default;
	                @else = AsT1;
	                return false;
	            case 2:
	                @if = default;
	                @else = AsT2;
	                return false;
	            case 3:
	                @if = default;
	                @else = AsT3;
	                return false;
	            case 4:
	                @if = AsT4;
	                @else = default;
	                return true;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    #endregion If (methods)
	    #region ToString
	    public override string ToString() => $"{((IEither)this).Type.Name}:{_value}";
	    // For LINQPad:
	    object ToDump() => new { Type = ((IEither)this).Type, Value = ((IEither)this).Value };
	    #endregion ToString
	}
	public record Either<T1, T2, T3, T4, T5> : IEither
	{
	    #region Implicit Conversion From Value
	    public static implicit operator Either<T1, T2, T3, T4, T5>(T1 value) => new Either<T1, T2, T3, T4, T5>(value);
	    public static implicit operator Either<T1, T2, T3, T4, T5>(T2 value) => new Either<T1, T2, T3, T4, T5>(value);
	    public static implicit operator Either<T1, T2, T3, T4, T5>(T3 value) => new Either<T1, T2, T3, T4, T5>(value);
	    public static implicit operator Either<T1, T2, T3, T4, T5>(T4 value) => new Either<T1, T2, T3, T4, T5>(value);
	    public static implicit operator Either<T1, T2, T3, T4, T5>(T5 value) => new Either<T1, T2, T3, T4, T5>(value);
	    #endregion Implicit Conversion From Value
	    #region Implicit Conversion By Type Swap
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2, T3, T5, T4> other)
	    {
	        int[] map = new[] { 1, 2, 3, 5, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2, T4, T3, T5> other)
	    {
	        int[] map = new[] { 1, 2, 4, 3, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2, T4, T5, T3> other)
	    {
	        int[] map = new[] { 1, 2, 4, 5, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2, T5, T3, T4> other)
	    {
	        int[] map = new[] { 1, 2, 5, 3, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2, T5, T4, T3> other)
	    {
	        int[] map = new[] { 1, 2, 5, 4, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T3, T2, T4, T5> other)
	    {
	        int[] map = new[] { 1, 3, 2, 4, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T3, T2, T5, T4> other)
	    {
	        int[] map = new[] { 1, 3, 2, 5, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T3, T4, T2, T5> other)
	    {
	        int[] map = new[] { 1, 3, 4, 2, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T3, T4, T5, T2> other)
	    {
	        int[] map = new[] { 1, 3, 4, 5, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T3, T5, T2, T4> other)
	    {
	        int[] map = new[] { 1, 3, 5, 2, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T3, T5, T4, T2> other)
	    {
	        int[] map = new[] { 1, 3, 5, 4, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T4, T2, T3, T5> other)
	    {
	        int[] map = new[] { 1, 4, 2, 3, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T4, T2, T5, T3> other)
	    {
	        int[] map = new[] { 1, 4, 2, 5, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T4, T3, T2, T5> other)
	    {
	        int[] map = new[] { 1, 4, 3, 2, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T4, T3, T5, T2> other)
	    {
	        int[] map = new[] { 1, 4, 3, 5, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T4, T5, T2, T3> other)
	    {
	        int[] map = new[] { 1, 4, 5, 2, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T4, T5, T3, T2> other)
	    {
	        int[] map = new[] { 1, 4, 5, 3, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T5, T2, T3, T4> other)
	    {
	        int[] map = new[] { 1, 5, 2, 3, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T5, T2, T4, T3> other)
	    {
	        int[] map = new[] { 1, 5, 2, 4, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T5, T3, T2, T4> other)
	    {
	        int[] map = new[] { 1, 5, 3, 2, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T5, T3, T4, T2> other)
	    {
	        int[] map = new[] { 1, 5, 3, 4, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T5, T4, T2, T3> other)
	    {
	        int[] map = new[] { 1, 5, 4, 2, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T5, T4, T3, T2> other)
	    {
	        int[] map = new[] { 1, 5, 4, 3, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T1, T3, T4, T5> other)
	    {
	        int[] map = new[] { 2, 1, 3, 4, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T1, T3, T5, T4> other)
	    {
	        int[] map = new[] { 2, 1, 3, 5, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T1, T4, T3, T5> other)
	    {
	        int[] map = new[] { 2, 1, 4, 3, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T1, T4, T5, T3> other)
	    {
	        int[] map = new[] { 2, 1, 4, 5, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T1, T5, T3, T4> other)
	    {
	        int[] map = new[] { 2, 1, 5, 3, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T1, T5, T4, T3> other)
	    {
	        int[] map = new[] { 2, 1, 5, 4, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T3, T1, T4, T5> other)
	    {
	        int[] map = new[] { 2, 3, 1, 4, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T3, T1, T5, T4> other)
	    {
	        int[] map = new[] { 2, 3, 1, 5, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T3, T4, T1, T5> other)
	    {
	        int[] map = new[] { 2, 3, 4, 1, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T3, T4, T5, T1> other)
	    {
	        int[] map = new[] { 2, 3, 4, 5, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T3, T5, T1, T4> other)
	    {
	        int[] map = new[] { 2, 3, 5, 1, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T3, T5, T4, T1> other)
	    {
	        int[] map = new[] { 2, 3, 5, 4, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T4, T1, T3, T5> other)
	    {
	        int[] map = new[] { 2, 4, 1, 3, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T4, T1, T5, T3> other)
	    {
	        int[] map = new[] { 2, 4, 1, 5, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T4, T3, T1, T5> other)
	    {
	        int[] map = new[] { 2, 4, 3, 1, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T4, T3, T5, T1> other)
	    {
	        int[] map = new[] { 2, 4, 3, 5, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T4, T5, T1, T3> other)
	    {
	        int[] map = new[] { 2, 4, 5, 1, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T4, T5, T3, T1> other)
	    {
	        int[] map = new[] { 2, 4, 5, 3, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T5, T1, T3, T4> other)
	    {
	        int[] map = new[] { 2, 5, 1, 3, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T5, T1, T4, T3> other)
	    {
	        int[] map = new[] { 2, 5, 1, 4, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T5, T3, T1, T4> other)
	    {
	        int[] map = new[] { 2, 5, 3, 1, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T5, T3, T4, T1> other)
	    {
	        int[] map = new[] { 2, 5, 3, 4, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T5, T4, T1, T3> other)
	    {
	        int[] map = new[] { 2, 5, 4, 1, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T5, T4, T3, T1> other)
	    {
	        int[] map = new[] { 2, 5, 4, 3, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T1, T2, T4, T5> other)
	    {
	        int[] map = new[] { 3, 1, 2, 4, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T1, T2, T5, T4> other)
	    {
	        int[] map = new[] { 3, 1, 2, 5, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T1, T4, T2, T5> other)
	    {
	        int[] map = new[] { 3, 1, 4, 2, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T1, T4, T5, T2> other)
	    {
	        int[] map = new[] { 3, 1, 4, 5, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T1, T5, T2, T4> other)
	    {
	        int[] map = new[] { 3, 1, 5, 2, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T1, T5, T4, T2> other)
	    {
	        int[] map = new[] { 3, 1, 5, 4, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T2, T1, T4, T5> other)
	    {
	        int[] map = new[] { 3, 2, 1, 4, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T2, T1, T5, T4> other)
	    {
	        int[] map = new[] { 3, 2, 1, 5, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T2, T4, T1, T5> other)
	    {
	        int[] map = new[] { 3, 2, 4, 1, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T2, T4, T5, T1> other)
	    {
	        int[] map = new[] { 3, 2, 4, 5, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T2, T5, T1, T4> other)
	    {
	        int[] map = new[] { 3, 2, 5, 1, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T2, T5, T4, T1> other)
	    {
	        int[] map = new[] { 3, 2, 5, 4, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T4, T1, T2, T5> other)
	    {
	        int[] map = new[] { 3, 4, 1, 2, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T4, T1, T5, T2> other)
	    {
	        int[] map = new[] { 3, 4, 1, 5, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T4, T2, T1, T5> other)
	    {
	        int[] map = new[] { 3, 4, 2, 1, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T4, T2, T5, T1> other)
	    {
	        int[] map = new[] { 3, 4, 2, 5, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T4, T5, T1, T2> other)
	    {
	        int[] map = new[] { 3, 4, 5, 1, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T4, T5, T2, T1> other)
	    {
	        int[] map = new[] { 3, 4, 5, 2, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T5, T1, T2, T4> other)
	    {
	        int[] map = new[] { 3, 5, 1, 2, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T5, T1, T4, T2> other)
	    {
	        int[] map = new[] { 3, 5, 1, 4, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T5, T2, T1, T4> other)
	    {
	        int[] map = new[] { 3, 5, 2, 1, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T5, T2, T4, T1> other)
	    {
	        int[] map = new[] { 3, 5, 2, 4, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T5, T4, T1, T2> other)
	    {
	        int[] map = new[] { 3, 5, 4, 1, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T5, T4, T2, T1> other)
	    {
	        int[] map = new[] { 3, 5, 4, 2, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T1, T2, T3, T5> other)
	    {
	        int[] map = new[] { 4, 1, 2, 3, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T1, T2, T5, T3> other)
	    {
	        int[] map = new[] { 4, 1, 2, 5, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T1, T3, T2, T5> other)
	    {
	        int[] map = new[] { 4, 1, 3, 2, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T1, T3, T5, T2> other)
	    {
	        int[] map = new[] { 4, 1, 3, 5, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T1, T5, T2, T3> other)
	    {
	        int[] map = new[] { 4, 1, 5, 2, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T1, T5, T3, T2> other)
	    {
	        int[] map = new[] { 4, 1, 5, 3, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T2, T1, T3, T5> other)
	    {
	        int[] map = new[] { 4, 2, 1, 3, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T2, T1, T5, T3> other)
	    {
	        int[] map = new[] { 4, 2, 1, 5, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T2, T3, T1, T5> other)
	    {
	        int[] map = new[] { 4, 2, 3, 1, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T2, T3, T5, T1> other)
	    {
	        int[] map = new[] { 4, 2, 3, 5, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T2, T5, T1, T3> other)
	    {
	        int[] map = new[] { 4, 2, 5, 1, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T2, T5, T3, T1> other)
	    {
	        int[] map = new[] { 4, 2, 5, 3, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T3, T1, T2, T5> other)
	    {
	        int[] map = new[] { 4, 3, 1, 2, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T3, T1, T5, T2> other)
	    {
	        int[] map = new[] { 4, 3, 1, 5, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T3, T2, T1, T5> other)
	    {
	        int[] map = new[] { 4, 3, 2, 1, 5 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T3, T2, T5, T1> other)
	    {
	        int[] map = new[] { 4, 3, 2, 5, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T3, T5, T1, T2> other)
	    {
	        int[] map = new[] { 4, 3, 5, 1, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T3, T5, T2, T1> other)
	    {
	        int[] map = new[] { 4, 3, 5, 2, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T5, T1, T2, T3> other)
	    {
	        int[] map = new[] { 4, 5, 1, 2, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T5, T1, T3, T2> other)
	    {
	        int[] map = new[] { 4, 5, 1, 3, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T5, T2, T1, T3> other)
	    {
	        int[] map = new[] { 4, 5, 2, 1, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T5, T2, T3, T1> other)
	    {
	        int[] map = new[] { 4, 5, 2, 3, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T5, T3, T1, T2> other)
	    {
	        int[] map = new[] { 4, 5, 3, 1, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T5, T3, T2, T1> other)
	    {
	        int[] map = new[] { 4, 5, 3, 2, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T1, T2, T3, T4> other)
	    {
	        int[] map = new[] { 5, 1, 2, 3, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T1, T2, T4, T3> other)
	    {
	        int[] map = new[] { 5, 1, 2, 4, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T1, T3, T2, T4> other)
	    {
	        int[] map = new[] { 5, 1, 3, 2, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T1, T3, T4, T2> other)
	    {
	        int[] map = new[] { 5, 1, 3, 4, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T1, T4, T2, T3> other)
	    {
	        int[] map = new[] { 5, 1, 4, 2, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T1, T4, T3, T2> other)
	    {
	        int[] map = new[] { 5, 1, 4, 3, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T2, T1, T3, T4> other)
	    {
	        int[] map = new[] { 5, 2, 1, 3, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T2, T1, T4, T3> other)
	    {
	        int[] map = new[] { 5, 2, 1, 4, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T2, T3, T1, T4> other)
	    {
	        int[] map = new[] { 5, 2, 3, 1, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T2, T3, T4, T1> other)
	    {
	        int[] map = new[] { 5, 2, 3, 4, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T2, T4, T1, T3> other)
	    {
	        int[] map = new[] { 5, 2, 4, 1, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T2, T4, T3, T1> other)
	    {
	        int[] map = new[] { 5, 2, 4, 3, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T3, T1, T2, T4> other)
	    {
	        int[] map = new[] { 5, 3, 1, 2, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T3, T1, T4, T2> other)
	    {
	        int[] map = new[] { 5, 3, 1, 4, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T3, T2, T1, T4> other)
	    {
	        int[] map = new[] { 5, 3, 2, 1, 4 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T3, T2, T4, T1> other)
	    {
	        int[] map = new[] { 5, 3, 2, 4, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T3, T4, T1, T2> other)
	    {
	        int[] map = new[] { 5, 3, 4, 1, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T3, T4, T2, T1> other)
	    {
	        int[] map = new[] { 5, 3, 4, 2, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T4, T1, T2, T3> other)
	    {
	        int[] map = new[] { 5, 4, 1, 2, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T4, T1, T3, T2> other)
	    {
	        int[] map = new[] { 5, 4, 1, 3, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T4, T2, T1, T3> other)
	    {
	        int[] map = new[] { 5, 4, 2, 1, 3 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T4, T2, T3, T1> other)
	    {
	        int[] map = new[] { 5, 4, 2, 3, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T4, T3, T1, T2> other)
	    {
	        int[] map = new[] { 5, 4, 3, 1, 2 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T5, T4, T3, T2, T1> other)
	    {
	        int[] map = new[] { 5, 4, 3, 2, 1 };
	        return new Either<T1, T2, T3, T4, T5>(map[other._typeIndex - 1], other._value);
	    }
	    #endregion Implicit Conversion By Type Swap
	    #region Implicit Widening Conversions
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T3> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T4> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T5> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T3> other) => other
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T4> other) => other
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T5> other) => other
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T4> other) => other
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T5> other) => other
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T4, T5> other) => other
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2, T3> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2, T4> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2, T5> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T3, T4> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T3, T5> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T4, T5> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T3, T4> other) => other
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T3, T5> other) => other
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T4, T5> other) => other
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T3, T4, T5> other) => other
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2, T3, T4> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2, T3, T5> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T2, T4, T5> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T1, T3, T4, T5> other) => other
	        .Match((T1 v1) => new Either<T1, T2, T3, T4, T5>(v1))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    public static implicit operator Either<T1, T2, T3, T4, T5>(Either<T2, T3, T4, T5> other) => other
	        .Match((T2 v2) => new Either<T1, T2, T3, T4, T5>(v2))
	        .Match((T3 v3) => new Either<T1, T2, T3, T4, T5>(v3))
	        .Match((T4 v4) => new Either<T1, T2, T3, T4, T5>(v4))
	        .Match((T5 v5) => new Either<T1, T2, T3, T4, T5>(v5));
	    #endregion Implicit Widening Conversions
	    #region Constructors
	    public Either(T1 value) { _value = value; _typeIndex = 1; }
	    public Either(T2 value) { _value = value; _typeIndex = 2; }
	    public Either(T3 value) { _value = value; _typeIndex = 3; }
	    public Either(T4 value) { _value = value; _typeIndex = 4; }
	    public Either(T5 value) { _value = value; _typeIndex = 5; }
	    #endregion Constructors
	    #region Or methods
	    #endregion Or methods
	    #region IEither Implementation
	    int _typeIndex;
	    object _value;
	    int IEither.TypeIndex => _typeIndex;
	    Type IEither.Type => _typeIndex switch
	    {
	        1 => typeof(T1),
	        2 => typeof(T2),
	        3 => typeof(T3),
	        4 => typeof(T4),
	        5 => typeof(T5),
	        _ => throw new InvalidOperationException()
	    };
	    object IEither.Value => _value;
	    Either(int typeIndex, object value) => (_typeIndex, _value) = (typeIndex, value);
	    #endregion IEither Implementation
	    #region Value Casts
	    T1 AsT1 => (T1)_value;
	    T2 AsT2 => (T2)_value;
	    T3 AsT3 => (T3)_value;
	    T4 AsT4 => (T4)_value;
	    T5 AsT5 => (T5)_value;
	    #endregion Value Casts
	    #region Explicit Casts
	    public static explicit operator T1(Either<T1, T2, T3, T4, T5> either) => either.AsT1;
	    public static explicit operator T2(Either<T1, T2, T3, T4, T5> either) => either.AsT2;
	    public static explicit operator T3(Either<T1, T2, T3, T4, T5> either) => either.AsT3;
	    public static explicit operator T4(Either<T1, T2, T3, T4, T5> either) => either.AsT4;
	    public static explicit operator T5(Either<T1, T2, T3, T4, T5> either) => either.AsT5;
	    #endregion Explicit Casts
	    #region Switch method
	    public void Switch(Action<T1> ifT1, Action<T2> ifT2, Action<T3> ifT3, Action<T4> ifT4, Action<T5> ifT5)
	    {
	        switch (_typeIndex)
	        {
	            case 1: ifT1(AsT1); break;
	            case 2: ifT2(AsT2); break;
	            case 3: ifT3(AsT3); break;
	            case 4: ifT4(AsT4); break;
	            case 5: ifT5(AsT5); break;
	            default: throw new InvalidOperationException();
	        }
	    }
	    #endregion Switch method
	    #region Nonreductive Match
	    public Either<TResult1, T2, T3, T4, T5> Match<TResult1>(Func<T1, TResult1> ifT1) => _typeIndex switch
	    {
	        1 => ifT1(AsT1),
	        2 => AsT2,
	        3 => AsT3,
	        4 => AsT4,
	        5 => AsT5,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, TResult2, T3, T4, T5> Match<TResult2>(Func<T2, TResult2> ifT2) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => ifT2(AsT2),
	        3 => AsT3,
	        4 => AsT4,
	        5 => AsT5,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, TResult3, T4, T5> Match<TResult3>(Func<T3, TResult3> ifT3) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => ifT3(AsT3),
	        4 => AsT4,
	        5 => AsT5,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, TResult4, T5> Match<TResult4>(Func<T4, TResult4> ifT4) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => AsT3,
	        4 => ifT4(AsT4),
	        5 => AsT5,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, T4, TResult5> Match<TResult5>(Func<T5, TResult5> ifT5) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => AsT3,
	        4 => AsT4,
	        5 => ifT5(AsT5),
	        _ => throw new InvalidOperationException()
	    };
	    #endregion Nonreductive Match
	    #region Nonreductive Match - Compositional
	    public Either<TResult1, T2, T3, T4, T5> Match<TResult1>(Func<T1, Either<TResult1, T2, T3, T4, T5>> ifT1) => _typeIndex switch
	    {
	        1 => ifT1(AsT1),
	        2 => AsT2,
	        3 => AsT3,
	        4 => AsT4,
	        5 => AsT5,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, TResult2, T3, T4, T5> Match<TResult2>(Func<T2, Either<T1, TResult2, T3, T4, T5>> ifT2) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => ifT2(AsT2),
	        3 => AsT3,
	        4 => AsT4,
	        5 => AsT5,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, TResult3, T4, T5> Match<TResult3>(Func<T3, Either<T1, T2, TResult3, T4, T5>> ifT3) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => ifT3(AsT3),
	        4 => AsT4,
	        5 => AsT5,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, TResult4, T5> Match<TResult4>(Func<T4, Either<T1, T2, T3, TResult4, T5>> ifT4) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => AsT3,
	        4 => ifT4(AsT4),
	        5 => AsT5,
	        _ => throw new InvalidOperationException()
	    };
	    public Either<T1, T2, T3, T4, TResult5> Match<TResult5>(Func<T5, Either<T1, T2, T3, T4, TResult5>> ifT5) => _typeIndex switch
	    {
	        1 => AsT1,
	        2 => AsT2,
	        3 => AsT3,
	        4 => AsT4,
	        5 => ifT5(AsT5),
	        _ => throw new InvalidOperationException()
	    };
	    #endregion Nonreductive Match - Compositional
	    #region Reductive Match
	    public Either<T2, T3, T4, T5> Match
	        (Func<T1, T2> ifT1) => _typeIndex switch
	        {
	            1 => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T1, T2> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3, T4, T5> Match
	        (Func<T2, T1> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T2, T1> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T2, T3, T4, T5> Match
	        (Func<T1, T3> ifT1) => _typeIndex switch
	        {
	            1 => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T1, T3> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T4, T5> Match
	        (Func<T3, T1> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => ifT3(AsT3),
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T3, T1> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            4 => AsT4,
	            5 => AsT5,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T2, T3, T4, T5> Match
	        (Func<T1, T4> ifT1) => _typeIndex switch
	        {
	            1 => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T1, T4> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T5> Match
	        (Func<T4, T1> ifT4) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => ifT4(AsT4),
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T4, T1> ifT4, Func<T4, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 when (when(AsT4)) => ifT4(AsT4),
	            5 => AsT5,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T2, T3, T4, T5> Match
	        (Func<T1, T5> ifT1) => _typeIndex switch
	        {
	            1 => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T1, T5> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T5, T1> ifT5) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => ifT5(AsT5),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T5, T1> ifT5, Func<T5, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 when (when(AsT5)) => ifT5(AsT5),
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3, T4, T5> Match
	        (Func<T2, T3> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T2, T3> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T4, T5> Match
	        (Func<T3, T2> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => ifT3(AsT3),
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T3, T2> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            4 => AsT4,
	            5 => AsT5,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3, T4, T5> Match
	        (Func<T2, T4> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T2, T4> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T5> Match
	        (Func<T4, T2> ifT4) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => ifT4(AsT4),
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T4, T2> ifT4, Func<T4, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 when (when(AsT4)) => ifT4(AsT4),
	            5 => AsT5,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3, T4, T5> Match
	        (Func<T2, T5> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T2, T5> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T5, T2> ifT5) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => ifT5(AsT5),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T5, T2> ifT5, Func<T5, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 when (when(AsT5)) => ifT5(AsT5),
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T4, T5> Match
	        (Func<T3, T4> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => ifT3(AsT3),
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T3, T4> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            4 => AsT4,
	            5 => AsT5,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T5> Match
	        (Func<T4, T3> ifT4) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => ifT4(AsT4),
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T4, T3> ifT4, Func<T4, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 when (when(AsT4)) => ifT4(AsT4),
	            5 => AsT5,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T4, T5> Match
	        (Func<T3, T5> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => ifT3(AsT3),
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T3, T5> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => ifT3(AsT3),
	            4 => AsT4,
	            5 => AsT5,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T5, T3> ifT5) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => ifT5(AsT5),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T5, T3> ifT5, Func<T5, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 when (when(AsT5)) => ifT5(AsT5),
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T5> Match
	        (Func<T4, T5> ifT4) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => ifT4(AsT4),
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T4, T5> ifT4, Func<T4, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 when (when(AsT4)) => ifT4(AsT4),
	            5 => AsT5,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> Match
	        (Func<T5, T4> ifT5) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => ifT5(AsT5),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> Match
	        (Func<T5, T4> ifT5, Func<T5, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 when (when(AsT5)) => ifT5(AsT5),
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Reductive Match
	    #region Throw Methods
	    public Either<T2, T3, T4, T5> ThrowIf
	        (Func<T1, Exception> ifT1) => _typeIndex switch
	        {
	            1 => throw ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> ThrowIf
	        (Func<T1, Exception> ifT1, Func<T1, bool> when) => _typeIndex switch
	        {
	            1 when (when(AsT1)) => throw ifT1(AsT1),
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            1 => AsT1,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T3, T4, T5> ThrowIf
	        (Func<T2, Exception> ifT2) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => throw ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> ThrowIf
	        (Func<T2, Exception> ifT2, Func<T2, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 when (when(AsT2)) => throw ifT2(AsT2),
	            3 => AsT3,
	            4 => AsT4,
	            5 => AsT5,
	            2 => AsT2,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T4, T5> ThrowIf
	        (Func<T3, Exception> ifT3) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => throw ifT3(AsT3),
	            4 => AsT4,
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> ThrowIf
	        (Func<T3, Exception> ifT3, Func<T3, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 when (when(AsT3)) => throw ifT3(AsT3),
	            4 => AsT4,
	            5 => AsT5,
	            3 => AsT3,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T5> ThrowIf
	        (Func<T4, Exception> ifT4) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => throw ifT4(AsT4),
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> ThrowIf
	        (Func<T4, Exception> ifT4, Func<T4, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 when (when(AsT4)) => throw ifT4(AsT4),
	            5 => AsT5,
	            4 => AsT4,
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4> ThrowIf
	        (Func<T5, Exception> ifT5) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 => throw ifT5(AsT5),
	            _ => throw new InvalidOperationException()
	        };
	    public Either<T1, T2, T3, T4, T5> ThrowIf
	        (Func<T5, Exception> ifT5, Func<T5, bool> when) => _typeIndex switch
	        {
	            1 => AsT1,
	            2 => AsT2,
	            3 => AsT3,
	            4 => AsT4,
	            5 when (when(AsT5)) => throw ifT5(AsT5),
	            5 => AsT5,
	            _ => throw new InvalidOperationException()
	        };
	    #endregion Throw Methods
	    #region If (methods)
	    public bool If(out T1 @if) => If(out @if, out _);
	    public bool If(out T1 @if, out Either<T2, T3, T4, T5> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = AsT1;
	                @else = default;
	                return true;
	            case 2:
	                @if = default;
	                @else = AsT2;
	                return false;
	            case 3:
	                @if = default;
	                @else = AsT3;
	                return false;
	            case 4:
	                @if = default;
	                @else = AsT4;
	                return false;
	            case 5:
	                @if = default;
	                @else = AsT5;
	                return false;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    public bool If(out T2 @if) => If(out @if, out _);
	    public bool If(out T2 @if, out Either<T1, T3, T4, T5> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = default;
	                @else = AsT1;
	                return false;
	            case 2:
	                @if = AsT2;
	                @else = default;
	                return true;
	            case 3:
	                @if = default;
	                @else = AsT3;
	                return false;
	            case 4:
	                @if = default;
	                @else = AsT4;
	                return false;
	            case 5:
	                @if = default;
	                @else = AsT5;
	                return false;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    public bool If(out T3 @if) => If(out @if, out _);
	    public bool If(out T3 @if, out Either<T1, T2, T4, T5> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = default;
	                @else = AsT1;
	                return false;
	            case 2:
	                @if = default;
	                @else = AsT2;
	                return false;
	            case 3:
	                @if = AsT3;
	                @else = default;
	                return true;
	            case 4:
	                @if = default;
	                @else = AsT4;
	                return false;
	            case 5:
	                @if = default;
	                @else = AsT5;
	                return false;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    public bool If(out T4 @if) => If(out @if, out _);
	    public bool If(out T4 @if, out Either<T1, T2, T3, T5> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = default;
	                @else = AsT1;
	                return false;
	            case 2:
	                @if = default;
	                @else = AsT2;
	                return false;
	            case 3:
	                @if = default;
	                @else = AsT3;
	                return false;
	            case 4:
	                @if = AsT4;
	                @else = default;
	                return true;
	            case 5:
	                @if = default;
	                @else = AsT5;
	                return false;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    public bool If(out T5 @if) => If(out @if, out _);
	    public bool If(out T5 @if, out Either<T1, T2, T3, T4> @else)
	    {
	        switch (_typeIndex)
	        {
	            case 1:
	                @if = default;
	                @else = AsT1;
	                return false;
	            case 2:
	                @if = default;
	                @else = AsT2;
	                return false;
	            case 3:
	                @if = default;
	                @else = AsT3;
	                return false;
	            case 4:
	                @if = default;
	                @else = AsT4;
	                return false;
	            case 5:
	                @if = AsT5;
	                @else = default;
	                return true;
	            default:
	                throw new InvalidOperationException();
	        }
	    }
	    #endregion If (methods)
	    #region ToString
	    public override string ToString() => $"{((IEither)this).Type.Name}:{_value}";
	    // For LINQPad:
	    object ToDump() => new { Type = ((IEither)this).Type, Value = ((IEither)this).Value };
	    #endregion ToString
	}
	#endregion
	#region \Utilities\Either\Abstractions
	public interface IEither
	{
	    int TypeIndex { get; }
	    Type Type { get; }
	    object Value { get; }
	}
	#endregion
	#region \Utilities\Entity
	public abstract partial class Entity<T>
	{
	    // This will specify whether to start tracking changes.
	    private bool isTracking;
	    // Create a concurrent cache to maintain all the changes
	    // Want to prevent duplicates so rather than maintain a collection we need  to merge 
	    // the changes.
	    private readonly ConcurrentDictionary<string, EntityChange> changes = new(StringComparer.InvariantCultureIgnoreCase);
	    private readonly static ConcurrentDictionary<Type, PropertyInfo[]> reflectionCache = new();
	    public void BeginTracking()
	    {
	        changes.Clear();
	        isTracking = true;
	    }
	    public void EndTracking()
	    {
	        isTracking = false;
	    }
	    public IEnumerable<EntityChange> GetChanges()
	    {
	        // Let's only returned changes that have changed
	        return changes.Values.Where(change => change.HasChanged).ToImmutableArray();
	    }
	    public bool HasChanged(string propertyName, out EntityChange? change)
	    {
	        change = null;
	        if (changes.TryGetValue(propertyName, out var value) && value.HasChanged)
	        {
	            change = value;
	            return true;
	        }
	        return false;
	    }
	    public bool HasChanged<TValue>(Expression<Func<T, TValue>> expression, out EntityChange? change)
	    {
	        change = null;
	        if (expression.Body is MemberExpression member)
	        {
	            var propertyName = string.Join('.', member.ToString().Split('.').Skip(1));
	            if (changes.TryGetValue(propertyName, out var value) && value.HasChanged)
	            {
	                change = value;
	                return true;
	            }
	            return false;
	        }
	        else
	        {
	            throw new ArgumentException("The provided expression must be a Member Expression");
	        }
	    }
	    protected void BeginPropertyChange([CallerMemberName] string propertyName = "")
	    {
	        propertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
	        if (!isTracking) return;
	        var propertyInfo = reflectionCache
	            .GetOrAdd(typeof(T), type => type.GetProperties())
	            .First(info => info.Name.Equals(propertyName));
	        changes[propertyName] = new EntityChange()
	        {
	            PropertyName = propertyName,
	            Original = propertyInfo.GetValue(this)
	        };
	    }
	    protected void EndPropertyChange([CallerMemberName] string propertyName = "")
	    {
	        propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	        if (!isTracking) return;
	        var propertyInfo = reflectionCache
	            .GetOrAdd(typeof(T), type => type.GetProperties())
	            .First(info => info.Name.Equals(propertyName));
	        var propertyType = propertyInfo.PropertyType;
	        var propertyTypeArgs = propertyType.GetGenericArguments();
	        if (!changes.TryGetValue(propertyName, out var entityChange))
	        {
	            throw new InvalidOperationException($"No Entity Change was found for property: {propertyName}. Make sure to call BeginPropertyChange before updating the value.");
	        }
	        entityChange.Current = propertyInfo.GetValue(this);
	        var original = entityChange.Original;
	        var current = entityChange.Current;
	        // Nullable<> types
	        if (propertyType.IsValueType && propertyTypeArgs.Length == 1 && propertyType.IsAssignableTo(typeof(Nullable<>).MakeGenericType(propertyTypeArgs[0])))
	        {
	            // No change
	            if (original is null && current is null)
	            {
	                return;
	            }
	            // Added
	            else if (original is null && current is not null)
	            {
	                entityChange.ChangeType = EntityChangeType.Added;
	            }
	            // Removed
	            else if (original is not null && current is null)
	            {
	                entityChange.ChangeType = EntityChangeType.Removed;
	            }
	            // Updated
	            else if (!Nullable.Equals(original, current))
	            {
	                entityChange.ChangeType = EntityChangeType.Updated;
	            }
	            // No change
	            else
	            {
	                return;
	            }
	        }
	        else if (propertyType.IsValueType)
	        {
	            // No change has occurred
	            if (original.Equals(current))
	            {
	                return;
	            }
	            // Value Types always have a value so 
	            // there should only ever be an update
	            else if (!original.Equals(current))
	            {
	                entityChange.ChangeType = EntityChangeType.Updated;
	            }
	            else
	            {
	                return;
	            }
	        }
	        // Default to reference equality
	        else
	        {
	            // No change
	            if (original is null && current is null)
	            {
	                return;
	            }
	            // Added
	            else if (original is null && current is not null)
	            {
	                entityChange.ChangeType = EntityChangeType.Added;
	            }
	            // Removed
	            else if (original is not null && current is null)
	            {
	                entityChange.ChangeType = EntityChangeType.Removed;
	            }
	            // Updated
	            else if (!original.Equals(current))
	            {
	                entityChange.ChangeType = EntityChangeType.Updated;
	            }
	            // No change
	            else
	            {
	                return;
	            }
	        }
	        entityChange.HasChanged = true;
	    }
	}
	public abstract partial class Entity<T> : INotifyPropertyChanging, INotifyPropertyChanged
	{
	    private event PropertyChangingEventHandler? propertyChanging;
	    private event PropertyChangedEventHandler? propertyChanged;
	    event PropertyChangingEventHandler? INotifyPropertyChanging.PropertyChanging
	    {
	        add => this.propertyChanging += value;
	        remove => this.propertyChanging -= value;
	    }
	    event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
	    {
	        add => this.propertyChanged += value;
	        remove => this.propertyChanged -= value;
	    }
	}
	public abstract partial class Entity<T> 
	    where T : Entity<T>
	{
	    public virtual object? Id { get; set; }
	}
	public abstract partial class Entity<T>
	{
	    // TODO: Revisit implementation
	    //public void Update(T entity)
	    //{
	    //    var properties = reflectionCache
	    //        .GetOrAdd(typeof(T), type => type.GetProperties());
	    //    foreach (var property in  properties)
	    //    {
	    //    }
	    //}
	}
	public sealed class EntityChange
	{
	    public string? PropertyName { get; set; }
	    public EntityChangeType ChangeType { get; set; }
	    public object? Current { get; set; }
	    public object? Original { get; set; }
	    internal bool HasChanged { get; set; }
	}
	public enum EntityChangeType
	{
	    None = 0,
	    Added,
	    Updated,
	    Removed
	}
	public sealed class IgnoreOnChangeTrackingAttribute : Attribute
	{
	}
	#endregion
}
#endregion
