<Query Kind="Program">
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "xunit"


void Main()
{
	
	//RunTest("TestRootPropertyChange");
	//RunTests();  // Call RunTests() or press Alt+Shift+T to initiate testing.

	var person = new Person()
	{
		Id = Guid.NewGuid(),
		Name = new Name()
		{
			FirstName = "Marta",
			LastName = "Elizondo"
		}
	};

	person.Id = Guid.NewGuid();
	person.Name = null;
	person.Name = new Name()
	{
		FirstName = "Chase",
		LastName = "Crawford",
		Audit = new AudiField()
		{
			Timestamp = DateTimeOffset.Now
		}
	};

	person.EndTracking();

	//	Traverse(person.GetProperties());
}


#region Entity Implementation

public abstract partial class Entity;
public abstract partial class Entity<T> : Entity where T : Entity<T>
{
	private readonly Type _type;

	private ConcurrentBag<EntityChange>? _changes;
	private bool _isTracking;

	public Entity()
	{
		_type = typeof(T);
	}

	/// <summary>
	/// Begins tracking property entries. Will clear any existing changes begin tracked.
	/// </summary>
	public void BeginTracking()
	{
		_changes = _type.CaptureEntityProperties(() => this);

		foreach (var change in _changes)
		{
			change.SetOriginal();
		}

		_isTracking = true;

		//return new PollingChangeHandler<T>(this);
	}

	/// <summary>
	/// Ends property entry tracking.
	/// </summary>
	public void EndTracking()
	{
		if (!_isTracking)
		{
			throw new InvalidOperationException("BeginTracking was not called.");
		}

		foreach (var change in _changes!)
		{
			change.SetCurrent();
		}

		_isTracking = false;
	}

	/// <summary>
	/// 
	/// </summary>
	public bool HasChanged<TProperty>(Expression<Func<T, TProperty>> expression, out EntityChange? change)
	{
		change = null;

		if (_changes is null || _changes.IsEmpty)
		{
			return false;
		}

		if (expression.Body is MemberExpression member)
		{
			var path = string.Join('.', member.ToString().Split('.').Skip(1));

			if ((change = _changes.FirstOrDefault(p => p.PropertyPath == path && p.Kind != EntityChangeKind.None)) is not null)
			{
				return true;
			}

			return false;
		}

		throw new ArgumentException("The provided expression must be a Member Expression");
	}

	/// <summary>
	/// Returns a collection of changes that occurred on the entity.
	/// </summary>
	public IEnumerable<EntityChange> GetChanges()
	{
		return (_changes ?? []).Where(p => p.Kind != EntityChangeKind.None);
	}


//	private partial class PollingChangeHandler<TEntity> : IDisposable
//		where TEntity : Entity<TEntity>
//	{
//		private readonly Entity<TEntity> _entity;
		
//		public PollingChangeHandler(Entity<TEntity> entity)
//		{
//			_entity = entity;
//		}
		
//		public void Dispose()
//		{
//			throw new NotImplementedException();
//		}
//	}
}


[DebuggerDisplay("{ToString()}")]
public class EntityChange
{
	private static readonly (
		Func<object?, object?, bool> Predicate,
		EntityChangeKind State
	)[] _states = [
		((o, c) => o is null && c is null, EntityChangeKind.None),
		((o, c) => o is null && c is not null, EntityChangeKind.Added),
		((o, c) => o is not null && c is null, EntityChangeKind.Removed),
		((o, c) => o is IComparable comp && c is IComparable && comp.CompareTo(c) != 0, EntityChangeKind.Modified),
		((o, c) => !o!.Equals(c), EntityChangeKind.Modified)
	];

	private readonly Type _propertyType;
	private readonly Func<object?> _propertyParentGetter;
	private readonly Func<object?, object?> _propertyGetter;
	private readonly string _propertyName;
	private readonly string _propertyPath;
	private readonly IReadOnlyList<EntityChange> _childChanges = [];

	private object? _original;
	private object? _current;
	private EntityChangeKind _kind;

	internal EntityChange(
		string propertyName,
		string propertyPath,
		Func<object?> propertyParentGetter, // Should return the parent/declaring type of the property
		Func<object?, object?> propertyGetter,
		Type propertyType)
	{
		_propertyType = propertyType;
		_propertyName = propertyName;
		_propertyPath = propertyPath;
		_propertyGetter = propertyGetter;
		_propertyParentGetter = propertyParentGetter;
	}

	internal EntityChange(
		string propertyName,
		string propertyPath,
		Func<object?> propertyParentGetter, // Should return the parent/declaring type of the property
		Func<object?, object?> propertyGetter,
		Type propertyType,
		IReadOnlyList<EntityChange> nested)
		: this(propertyName, propertyPath, propertyParentGetter, propertyGetter, propertyType)
	{
		_childChanges = nested;
	}

	/// <summary>
	/// The name of the property that changed.
	/// </summary>
	public string PropertyName => _propertyName;

	/// <summary>
	/// The property path, if any.
	/// </summary>
	public string PropertyPath => _propertyPath;

	/// <summary>
	/// The original value of the property at the beginning of change tracking.
	/// </summary>
	public object? Original => _original;

	/// <summary>
	/// The new value, if any, of the current property after end tracking.
	/// </summary>
	public object? Current => _current;

	/// <summary>
	/// The state of the change, if any.
	/// </summary>
	public EntityChangeKind Kind => _kind;

	/// <summary>
	/// Gets the child changes, if any.
	/// </summary>
	public IReadOnlyList<EntityChange> ChildChanges => _childChanges.Where(p => p.Kind != EntityChangeKind.None).ToImmutableList();


	public override string ToString()
	{
		if (_childChanges.Count > 0)
		{
			return $"[{Kind}] {_propertyName}";
		}

		return $"[{Kind}] {_propertyName} = {_original} -> {_current}";
	}

	internal virtual void SetOriginal()
	{
		object? parent = _propertyParentGetter.Invoke();

		if (parent is not null)
		{
			_original = _propertyGetter.Invoke(parent);
		}

		for (int i = 0; i < _childChanges.Count; i++)
		{
			_childChanges[i].SetOriginal();
		}
	}

	internal virtual void SetCurrent()
	{
		object? parent = _propertyParentGetter.Invoke();

		if (parent is not null)
		{
			_current = _propertyGetter.Invoke(parent);
		}

		for (int i = 0; i < _states.Length; i++)
		{
			var check = _states[i];

			if (check.Predicate.Invoke(_original, _current))
			{
				_kind = check.State;
				break;
			}
		}

		for (int i = 0; i < _childChanges.Count; i++)
		{
			_childChanges[i].SetCurrent();
		}
	}
}

public enum EntityChangeKind
{
	None = 0,
	Added,
	Modified,
	Removed
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class IgnoreOnChangeTrackingAttribute : Attribute
{
}


internal static class EntityPropertyEntryExtensions
{
	private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _cache;

	static EntityPropertyEntryExtensions()
	{
		_cache = new ConcurrentDictionary<Type, PropertyInfo[]>();
	}

	extension(Type type)
	{
		internal ConcurrentBag<EntityChange> CaptureEntityProperties(
			Func<object?> state,
			string? path = null)
		{
			const BindingFlags flags = BindingFlags.Public | BindingFlags.Public | BindingFlags.Instance;

			ConcurrentBag<EntityChange> changes = new();

			// Get readable and writable properties
			PropertyInfo[] properties = _cache.GetOrAdd(type, type =>
			{
				return type.GetProperties(flags)
					.Where(p =>
					{
						if (p.CanRead && p.CanWrite)
						{
							var attribute = Attribute.GetCustomAttribute(p, typeof(IgnoreOnChangeTrackingAttribute));

							if (attribute is not null)
							{
								return false; // Ignore this property
							}

							return true;
						}

						return false;
					})
					.ToArray();
			});

			for (int i = 0; i < properties.Length; i++)
			{
				PropertyInfo property = properties[i];

				Type propertyType = property.PropertyType;
				string propertyName = property.Name;
				string propertyPath = path is null ? propertyName : string.Join('.', path, propertyName);
				Func<object?, object?> propertyGetter = property.GetValue!;
				
				if (propertyType.IsValueType || propertyType.IsEnum || propertyType == typeof(string))
				{
					changes.Add(new EntityChange(
						propertyName,
						propertyPath,
						state,
						propertyGetter,
						propertyType));
				}

				else if (propertyType.IsAssignableTo(typeof(IEnumerable)))
				{
					
				}

				else if (propertyType.IsClass)
				{
					changes.Add(new EntityChange(
						propertyName,
						propertyPath,
						state,
						propertyGetter,
						propertyType,
						propertyType.CaptureEntityProperties(
							state: () =>
							{
								var parent = state.Invoke();

								if (parent is not null)
								{
									return propertyGetter.Invoke(parent);
								}

								return null;
							},
							path: propertyPath).ToImmutableList()));
				}
			}

			return changes;
		}
	}
}

#endregion

#region Entity Test Objects

public class Person : Entity<Person>
{
	public Guid? Id { get; set; }
	public Name? Name { get; set; }
}

public class Name
{
	public string? FirstName { get; set; }
	public string? MiddleName { get; set; }
	public string? LastName { get; set; }
	public AudiField? Audit { get; set; }
}

public class AudiField
{
	public DateTimeOffset? Timestamp { get; set; }
}

#endregion

#region Entity Tests

[Fact(DisplayName = "Change Tracking - Property Modified Success")]
public void TestRootPropertyChange()
{
	var person = new Person()
	{
		Id = Guid.NewGuid()
	};

	person.BeginTracking();
	person.Id = Guid.NewGuid();
	person.EndTracking();

	EntityChange change = Assert.Single(person.GetChanges());
	Assert.Equal(nameof(person.Id), change.PropertyName);

}

#endregion