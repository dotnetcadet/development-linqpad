<Query Kind="Program">
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>System.ComponentModel</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "xunit"

void Main()
{
	RunTests();  // Call RunTests() or press Alt+Shift+T to initiate testing.


}

public enum TestEnum
{
	None,
	Test1,
	Test2
}
public record class TestObject : Entity<TestObject>
{
	private int i32;
	private int? ni32;
	private string? name;
	private TestEnum? enum1;

	public int Int32
	{
		get => i32;
		set
		{
			BeginPropertyChange();
			i32 = value;
			EndPropertyChange();
		}
	}
	public int? NullableInt32
	{
		get => ni32;
		set
		{
			BeginPropertyChange();
			ni32 = value;
			EndPropertyChange();
		}
	}
	public string Name
	{
		get => this.name;
		set
		{
			BeginPropertyChange();
			name = value;
			EndPropertyChange();
		}
	}
	
	public TestEnum? Enum1
	{
		get => this.enum1;
		set
		{
			BeginPropertyChange();
			enum1 = value;
			EndPropertyChange();
		}
	}
	
}


#region private::Tests

[Fact(DisplayName = "String Changed - Added")]
void StringChangeAddedTest()
{
	var obj = new TestObject();
	obj.BeginTraking();
	obj.Name = "test";
	obj.EndTracking();
	
	Assert.True(obj.HasChanged(p=>p.Name, out var change));
	Assert.Equal(EntityChangeType.Added, change.ChangeType);
}

[Fact(DisplayName = "String Changed - Removed")]
void StringChangeRemovedTest()
{
	var obj = new TestObject()
	{
		Name = "test"
	};
	obj.BeginTraking();
	obj.Name = null;
	obj.EndTracking();

	Assert.True(obj.HasChanged(p => p.Name, out var change));
	Assert.Equal(EntityChangeType.Removed, change.ChangeType);
}

[Fact(DisplayName = "String Changed - Updated")]
void StringChangeUpdatedTest()
{
	var obj = new TestObject()
	{
		Name = "old value"
	};
	obj.BeginTraking();
	obj.Name = "new value";
	obj.EndTracking();

	Assert.True(obj.HasChanged(p => p.Name, out var change));
	Assert.Equal(EntityChangeType.Updated, change.ChangeType);
}

[Fact(DisplayName = "Int32 Changed - Updated")]
void Int32ChangeUpdatedTest()
{
	var obj = new TestObject()
	{
		Int32 = 2
	};
	obj.BeginTraking();
	obj.Int32 = 3;
	obj.EndTracking();

	Assert.True(obj.HasChanged(p => p.Int32, out var change));
	Assert.Equal(EntityChangeType.Updated, change.ChangeType);
}

[Fact(DisplayName = "Nullable Int32 Changed - Added")]
void NullableInt32ChangeAddedTest()
{
	var obj = new TestObject();
	obj.BeginTraking();
	obj.NullableInt32 = 3;
	obj.EndTracking();

	Assert.True(obj.HasChanged(p => p.NullableInt32, out var change));
	Assert.Equal(EntityChangeType.Added, change.ChangeType);
}

[Fact(DisplayName = "Nullable Int32 Changed - Removed")]
void NullableInt32ChangeRemovedTest()
{
	var obj = new TestObject()
	{
		NullableInt32 = 3
	};
	obj.BeginTraking();
	obj.NullableInt32 = null;
	obj.EndTracking();

	Assert.True(obj.HasChanged(p => p.NullableInt32, out var change));
	Assert.Equal(EntityChangeType.Removed, change.ChangeType);
}

[Fact(DisplayName = "Nullable Int32 Changed - Updated")]
void NullableInt32ChangeUpdatedTest()
{
	var obj = new TestObject()
	{
		NullableInt32 = 2
	};
	obj.BeginTraking();
	obj.NullableInt32 = 3;
	obj.EndTracking();

	Assert.True(obj.HasChanged(p => p.NullableInt32, out var change));
	Assert.Equal(EntityChangeType.Updated, change.ChangeType);
}


[Fact(DisplayName = "Nullable Enum Changed - Added")]
void NullableEnumChangeAddedTest()
{
	var obj = new TestObject();
	obj.BeginTraking();
	obj.Enum1 = TestEnum.Test1;
	obj.EndTracking();

	Assert.True(obj.HasChanged(p => p.Enum1, out var change));
	Assert.Equal(EntityChangeType.Added, change.ChangeType);
}

[Fact(DisplayName = "Nullable Enum Changed - Removed")]
void NullableEnumChangeRemovedTest()
{
	var obj = new TestObject()
	{
		Enum1 = TestEnum.Test1
	};
	obj.BeginTraking();
	obj.Enum1 = null;
	obj.EndTracking();

	Assert.True(obj.HasChanged(p => p.Enum1, out var change));
	Assert.Equal(EntityChangeType.Removed, change.ChangeType);
}

[Fact(DisplayName = "Nullable Enum Changed - Updated")]
void NullableEnumChangeUpdatedTest()
{
	var obj = new TestObject()
	{
		Enum1 = TestEnum.Test1
	};
	obj.BeginTraking();
	obj.Enum1 = TestEnum.Test2;
	obj.EndTracking();

	Assert.True(obj.HasChanged(p => p.Enum1, out var change));
	Assert.Equal(EntityChangeType.Updated, change.ChangeType);
}
#endregion


#region Entity 
// Default Base Entity Properties
public abstract partial record class Entity<T> where T : Entity<T>
{
	/// <summary>
	/// The unique identifier for the Entity.
	/// </summary>
	public virtual string? Id { get; set; }
	/// <summary>
	/// The descriminator of the entity.
	/// </summary>
	public EntityType EntityType { get; set; }
	/// <summary>
	/// Created audit entry.
	/// </summary>
	public EntityAuditField? Created { get; set; }
	/// <summary>
	/// Updated audit entry.
	/// </summary>
	public EntityAuditField? Updated { get; set; }
	/// <summary>
	/// This includes meta data regarding the entity.
	/// </summary>
	public IDictionary<string, string> Meta { get; set; } = new Dictionary<string, string>();
}
//
public abstract partial record class Entity<T>
{
	// This will specify whether to start tracking changes.
	private bool isTracking;

	// Create a concurrent cache to maintain all the changes
	// Want to prevent duplicates so rather than maintain a collection we need  to merge 
	// the changes.
	private readonly ConcurrentDictionary<string, EntityChange> changes = new(StringComparer.InvariantCultureIgnoreCase);

	
	private readonly static ConcurrentDictionary<Type, PropertyInfo[]> reflectionCache = new();

	/// <summary>
	/// Starts change tracking of the entity. Clears out any existing changes.
	/// </summary>
	public void BeginTraking()
	{
		changes.Clear();
		isTracking = true;
	}

	/// <summary>
	/// Ends change tracking of 
	/// </summary>
	public void EndTracking()
	{
		isTracking = false;
	}

	/// <summary>
	/// Returns all the changes that have been tracked.
	/// </summary>
	/// <returns></returns>
	public IEnumerable<EntityChange> GetChanges()
	{
		// Let's only returned changes that have changed
		return changes.Values.Where(change => change.HasChanged).ToImmutableArray();
	}

	/// <summary>
	/// Checks if a particular property had changed and returns the event arguments
	/// </summary>
	/// <param name="propertyName"></param>
	/// <param name="change"></param>
	/// <returns></returns>
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

	/// <summary>
	/// Uses a member expression tp check of the member had changed.
	/// </summary>
	/// <typeparam name="TValue"></typeparam>
	/// <param name="expression"></param>
	/// <param name="change"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
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

	/// <summary>
	/// 
	/// </summary>
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

	/// <summary>
	/// 
	/// </summary>
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
// Runtime Interfaces implementation
public abstract partial record class Entity<T> : INotifyPropertyChanging, INotifyPropertyChanged
{
	private event PropertyChangingEventHandler propertyChanging;
	private event PropertyChangedEventHandler propertyChanged;


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
public sealed record class EntityAuditField
{
	/// <summary>
	/// The Azure Entra ID of the user in which invoked a transaction.
	/// </summary>
	public string? ObjectId { get; set; }
	/// <summary>
	/// The date and time in which the transaction occurred.
	/// </summary>
	public DateTime? Timestamp { get; set; }
	/// <summary>
	/// Specifies whether the transactions was invoked by a system.
	/// </summary>
	public bool? IsSystem { get; set; }
}
public sealed record class EntityChange
{
	/// <summary>
	/// The name of the property that changes.
	/// </summary>
	public string? PropertyName { get; set; }
	/// <summary>
	/// Specifies the type of change that occurred on the property.
	/// </summary>
	public EntityChangeType ChangeType { get; set; }
	/// <summary>
	/// Represents the current state of the
	/// </summary>
	public object? Current { get; set; }
	/// <summary>
	/// Represents the original value before state change.
	/// </summary>
	public object? Original { get; set; }
	/// <summary>
	/// Specifies that the property change went from changing to changed.
	/// </summary>
	internal bool HasChanged { get; set; }
}

public enum EntityChangeType
{
	None = 0,
	Added,
	Updated,
	Removed
}
public enum EntityType
{

}

#endregion

