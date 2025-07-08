<Query Kind="Program">
  <Namespace>System.ComponentModel</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "xunit"

void Main()
{
	//RunTests();  // Call RunTests() or press Alt+Shift+T to initiate testing.

	var user = new User()
	{
		Id = 10001,
		Info = new UserInfo("chase", "crawford", null, "ccrawford@assimalign.com")
	};
	
	var role = new UserRole();


	user.BeginTracking();
	user.Info = new UserInfo("Chase", "Crawford", null, "ccrawford@assimalign.com");

	user.EndTracking();
	user.GetChanges().Dump();



	if (user.HasChanged(p => p.Info!.Password!.Value, out var change))
	{
		change.Dump();
	}

}


#region Entity::TestObject

public class User : Entity<User, int>
{
	private UserInfo? info;

	public UserInfo? Info
	{
		get => info;
		set
		{
			BeginPropertyChange();
			info = value;
			EndPropertyChange();
		}
	}
	public UserAuditEntry? Created { get; set; }
	public UserAuditEntry? Updated { get; set; }
}
public class UserRole : Entity<UserRole, int>
{
	
}
public record UserInfo(string? FirstName, string? LastName, string? MiddleName, string? Email)
{
	public DateOnly Birthdate { get; set; }
}
public record UserAuditEntry(DateTime TimeStamp, string UserId);



#endregion

#region Entity::Abstractions


public enum ChangeKind
{
	None = 0,
	Added,
	Updated,
	Removed
}

public class EntityChange
{
	/// <summary>
	/// The name of the property that changes.
	/// </summary>
	public string? Property { get; set; }
	/// <summary>
	/// Specifies the type of change that occurred on the property.
	/// </summary>
	public ChangeKind Kind { get; set; }
	/// <summary>
	/// Represents the current state of the
	/// </summary>
	public virtual object? Current { get; set; }
	/// <summary>
	/// Represents the original value before state change.
	/// </summary>
	public virtual object? Original { get; set; }
	/// <summary>
	/// Specifies that the property change went from changing to changed.
	/// </summary>
	internal bool HasChanged { get; set; }
}
public class EntityChange<T> : EntityChange
{
	public new T? Current { get; set; }
	public new T? Original { get; set; }
}

public abstract partial class Entity<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T, TKey> : Entity<T> where T : class where TKey : struct
{
	public virtual TKey Id { get; set; }
}
public abstract partial class Entity<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T> where T : class
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
	public void BeginTracking()
	{
		changes.Clear();
		isTracking = true;
	}

	/// <summary>
	/// Ends change tracking of entity.
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
	public bool HasChanged<TValue>(Expression<Func<T, TValue?>> expression, out EntityChange<TValue>? change)
	{
		change = null;
		if (expression.Body is MemberExpression member)
		{
			var propertyName = string.Join('.', member.ToString().Split('.').Skip(1));
			if (changes.TryGetValue(propertyName, out var value) && value.HasChanged)
			{
				change = new EntityChange<TValue>()
				{
					Original = (TValue)value.Original!,
					Current = (TValue)value.Current!,
					Kind = value.Kind,
					Property = value.Property
				};
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
	/// Begins tracing all property changes
	/// </summary>
	protected void BeginPropertyChange([CallerMemberName] string propertyName = "")
	{
		propertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));

		if (!isTracking) return;

		var propertyInfo = reflectionCache
			.GetOrAdd(typeof(T), type => type.GetProperties().Where(p => p.CanWrite && p.CanRead).ToArray())
			.First(info => info.Name.Equals(propertyName));

		var propertyType = propertyInfo.PropertyType;
		var propertyValue = propertyInfo.GetValue(this);

		if (propertyType.IsClass &&
			!propertyType.IsAbstract &&
			!propertyType.IsAssignableTo(typeof(string)) &&
			!propertyType.IsAssignableTo(typeof(IEnumerable)))
		{
			BeginChildPropertyChange(propertyInfo, propertyName, propertyValue);
		}

		changes[propertyName] = new EntityChange()
		{
			Property = propertyName,
			Original = propertyValue
		};
	}

	/// <summary>
	/// Ends the property change.
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
		var propertyValue = propertyInfo.GetValue(this);

		if (!changes.TryGetValue(propertyName, out var entityChange))
		{
			throw new InvalidOperationException($"No Entity Change was found for property: {propertyName}. Make sure to call BeginPropertyChange before updating the value.");
		}

		if (propertyType.IsClass &&
			!propertyType.IsAbstract &&
			!propertyType.IsAssignableTo(typeof(string)) &&
			!propertyType.IsAssignableTo(typeof(IEnumerable)))
		{
			EndChildPropertyChange(propertyInfo, propertyName, propertyValue);
		}

		entityChange.Current = propertyValue;

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
				entityChange.Kind = ChangeKind.Added;
			}
			// Removed
			else if (original is not null && current is null)
			{
				entityChange.Kind = ChangeKind.Removed;
			}
			// Updated
			else if (!Nullable.Equals(original, current))
			{
				entityChange.Kind = ChangeKind.Updated;
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
			if (original!.Equals(current))
			{
				return;
			}
			// Value Types always have a value so 
			// there should only ever be an update
			else if (!original.Equals(current))
			{
				entityChange.Kind = ChangeKind.Updated;
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
				entityChange.Kind = ChangeKind.Added;
			}
			// Removed
			else if (original is not null && current is null)
			{
				entityChange.Kind = ChangeKind.Removed;
			}
			// Updated
			else if (!original.Equals(current))
			{
				entityChange.Kind = ChangeKind.Updated;
			}
			// No change
			else
			{
				return;
			}
		}

		entityChange.HasChanged = true;
	}


	private void BeginChildPropertyChange(PropertyInfo propertyInfo, string propertyName, object? propertyValue)
	{
		var children = reflectionCache
			.GetOrAdd(propertyInfo.PropertyType, type => type.GetProperties()
				.Where(p => p.CanWrite && p.CanRead).ToArray());

		foreach (var child in children)
		{
			object? childValue = null;

			if (propertyValue is not null)
			{
				childValue = child.GetValue(propertyValue);
			}

			var childName = string.Join(".", propertyName, child.Name);
			var childType = child.PropertyType;

			if (childType.IsClass &&
				!childType.IsAbstract &&
				!childType.IsAssignableTo(typeof(string)) &&
				!childType.IsAssignableTo(typeof(IEnumerable)))
			{
				BeginChildPropertyChange(child, childName, childValue);
			}

			changes[childName] = new EntityChange()
			{
				Property = childName,
				Original = childValue
			};
		}
	}
	private void EndChildPropertyChange(PropertyInfo propertyInfo, string propertyName, object? propertyValue)
	{
		var children = reflectionCache
			.GetOrAdd(propertyInfo.PropertyType, type => type.GetProperties()
				.Where(p => p.CanWrite && p.CanRead).ToArray());

		foreach (var child in children)
		{
			object? childValue = null;

			if (propertyValue is not null)
			{
				childValue = child.GetValue(propertyValue);
			}

			var childName = string.Join(".", propertyName, child.Name);
			var childType = child.PropertyType;
			var childTypeArgs = childType.GetGenericArguments();

			if (!changes.TryGetValue(childName, out var entityChange))
			{
				throw new InvalidOperationException($"No Entity Change was found for property: {propertyName}. Make sure to call BeginPropertyChange before updating the value.");
			}

			if (childType.IsClass &&
				!childType.IsAbstract &&
				!childType.IsAssignableTo(typeof(string)) &&
				!childType.IsAssignableTo(typeof(IEnumerable)))
			{
				EndChildPropertyChange(child, childName, childValue);
			}

			entityChange.Current = childValue;

			var original = entityChange.Original;
			var current = entityChange.Current;

			// Nullable<> types
			if (childType.IsValueType && childTypeArgs.Length == 1 && childType.IsAssignableTo(typeof(Nullable<>).MakeGenericType(childTypeArgs[0])))
			{
				// No change
				if (original is null && current is null)
				{
					continue;
				}
				// Added
				else if (original is null && current is not null)
				{
					entityChange.Kind = ChangeKind.Added;
				}
				// Removed
				else if (original is not null && current is null)
				{
					entityChange.Kind = ChangeKind.Removed;
				}
				// Updated
				else if (!Nullable.Equals(original, current))
				{
					entityChange.Kind = ChangeKind.Updated;
				}
				// No change
				else
				{
					continue;
				}
			}
			else if (childType.IsValueType)
			{
				// No change has occurred
				if (original!.Equals(current))
				{
					continue;
				}
				// Value Types always have a value so 
				// there should only ever be an update
				else if (!original.Equals(current))
				{
					entityChange.Kind = ChangeKind.Updated;
				}
				else
				{
					continue;
				}
			}
			// Default to reference equality
			else
			{
				// No change
				if (original is null && current is null)
				{
					continue;
				}
				// Added
				else if (original is null && current is not null)
				{
					entityChange.Kind = ChangeKind.Added;
				}
				// Removed
				else if (original is not null && current is null)
				{
					entityChange.Kind = ChangeKind.Removed;
				}
				// Updated
				else if (!original.Equals(current))
				{
					entityChange.Kind = ChangeKind.Updated;
				}
				// No change
				else
				{
					continue;
				}
			}

			entityChange.HasChanged = true;
		}
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

#endregion

#region Entity::Tests

[Fact] void Test_Xunit() => Assert.True (1 + 1 == 2);

#endregion