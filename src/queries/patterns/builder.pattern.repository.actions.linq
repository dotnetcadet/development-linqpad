<Query Kind="Program" />

void Main()
{
	
	var builder = default(IRepositoryActionProfileBuilder<User>);
	
	builder.ConfigureOnCreate(descriptor =>
	{
		descriptor.
	});

}

public class User
{
	public string FirstName { get; set; }
}

public class RepositoryActionContext<T>
{

	public RepositoryActionContext(T entity)
	{
		this.Entity = entity;
	}

	public T Entity { get; }

	public static implicit operator RepositoryActionContext<T>(T entity) => new RepositoryActionContext<T>(entity);
}
public interface IRepositoryAction<T>
{
	void Invoke(RepositoryActionContext<T> context);
}
public interface IRepositoryActionDescriptor<T>
{
	IRepositoryActionDescriptor<T> AddAction(IRepositoryAction<T> action);
	IRepositoryActionDescriptor<T> AddMemberSetterAction(IRepositoryAction<T> action);
}
public interface IRepositoryActionProfile<T>
{
	void OnCreate(RepositoryActionContext<T> context);
	void OnCreated(RepositoryActionContext<T> context);
	void OnUpdate(RepositoryActionContext<T> context);
	void OnUpdated(RepositoryActionContext<T> context);
	void OnDelete(RepositoryActionContext<T> context);
	void OnDeleted(RepositoryActionContext<T> context);
}
public interface IRepositoryActionProfileBuilder<T>
{
	IRepositoryActionProfileBuilder<T> ConfigureOnCreate(Action<IRepositoryActionDescriptor<T>> configure);
	IRepositoryActionProfileBuilder<T> ConfigureOnCreated(Action<IRepositoryActionDescriptor<T>> configure);
	IRepositoryActionProfileBuilder<T> ConfigureOnUpdate(Action<IRepositoryActionDescriptor<T>> configure);
	IRepositoryActionProfileBuilder<T> ConfigureOnUpdated(Action<IRepositoryActionDescriptor<T>> configure);
	IRepositoryActionProfileBuilder<T> ConfigureOnDelete(Action<IRepositoryActionDescriptor<T>> configure);
	IRepositoryActionProfileBuilder<T> ConfigureOnDeleted(Action<IRepositoryActionDescriptor<T>> configure);
	IRepositoryActionProfile<T> Build();
}

internal sealed class RepositoryActionDescriptor<T> : IRepositoryActionDescriptor<T>
{
	public RepositoryActionDescriptor()
	{
		this.Actions = new List<IRepositoryAction<T>>();
	}
	public IList<IRepositoryAction<T>> Actions { get; init; }

	public IRepositoryActionDescriptor<T> AddAction(IRepositoryAction<T> action)
	{
		if (action is null)
		{
			throw new ArgumentNullException(nameof(action));
		}

		Actions.Add(action);

		return this;
	}

	public IRepositoryActionDescriptor<T> AddMemberSetterAction(IRepositoryAction<T> action)
	{
		throw new NotImplementedException();
	}
}
internal sealed class RepositoryActionProfile<T> : IRepositoryActionProfile<T>
{
	public RepositoryActionProfile()
	{
		this.OnCreateActions = new List<IRepositoryAction<T>>();
		this.OnCreatedActions = new List<IRepositoryAction<T>>();
		this.OnUpdateActions = new List<IRepositoryAction<T>>();
		this.OnUpdatedActions = new List<IRepositoryAction<T>>();
		this.OnDeleteActions = new List<IRepositoryAction<T>>();
		this.OnDeletedActions = new List<IRepositoryAction<T>>();
	}

	public IList<IRepositoryAction<T>> OnCreateActions { get; }
	public IList<IRepositoryAction<T>> OnCreatedActions { get; }
	public IList<IRepositoryAction<T>> OnUpdateActions { get; }
	public IList<IRepositoryAction<T>> OnUpdatedActions { get; }
	public IList<IRepositoryAction<T>> OnDeleteActions { get; }
	public IList<IRepositoryAction<T>> OnDeletedActions { get; }

	public void OnCreate(RepositoryActionContext<T> context)
	{
		foreach (var action in OnCreateActions)
		{
			action.Invoke(context);
		}
	}
	public void OnCreated(RepositoryActionContext<T> context)
	{
		foreach (var action in OnCreatedActions)
		{
			action.Invoke(context);
		}
	}
	public void OnDelete(RepositoryActionContext<T> context)
	{
		foreach (var action in OnDeleteActions)
		{
			action.Invoke(context);
		}
	}
	public void OnDeleted(RepositoryActionContext<T> context)
	{
		foreach (var action in OnDeletedActions)
		{
			action.Invoke(context);
		}
	}
	public void OnUpdate(RepositoryActionContext<T> context)
	{
		foreach (var action in OnUpdateActions)
		{
			action.Invoke(context);
		}
	}
	public void OnUpdated(RepositoryActionContext<T> context)
	{
		foreach (var action in OnUpdatedActions)
		{
			action.Invoke(context);
		}
	}
}
internal sealed class RepositoryActionProfileBuilder<T> : IRepositoryActionProfileBuilder<T>
{
	private readonly RepositoryActionProfile<T> profile;

	public RepositoryActionProfileBuilder()
	{
		this.profile = new RepositoryActionProfile<T>();
	}

	private void EnsureDescriptorNotNull(Action<IRepositoryActionDescriptor<T>> configure)
	{
		if (configure is null)
		{
			throw new ArgumentNullException(nameof(configure));
		}
	}

	public IRepositoryActionProfileBuilder<T> ConfigureOnCreate(Action<IRepositoryActionDescriptor<T>> configure)
	{
		EnsureDescriptorNotNull(configure);

		var descriptor = new RepositoryActionDescriptor<T>()
		{
			Actions = profile.OnCreateActions
		};

		configure.Invoke(descriptor);

		return this;
	}
	public IRepositoryActionProfileBuilder<T> ConfigureOnCreated(Action<IRepositoryActionDescriptor<T>> configure)
	{
		EnsureDescriptorNotNull(configure);

		var descriptor = new RepositoryActionDescriptor<T>()
		{
			Actions = profile.OnCreatedActions
		};

		configure.Invoke(descriptor);

		return this;
	}
	public IRepositoryActionProfileBuilder<T> ConfigureOnDelete(Action<IRepositoryActionDescriptor<T>> configure)
	{
		EnsureDescriptorNotNull(configure);

		var descriptor = new RepositoryActionDescriptor<T>()
		{
			Actions = profile.OnDeleteActions
		};

		configure.Invoke(descriptor);

		return this;
	}
	public IRepositoryActionProfileBuilder<T> ConfigureOnDeleted(Action<IRepositoryActionDescriptor<T>> configure)
	{
		EnsureDescriptorNotNull(configure);

		var descriptor = new RepositoryActionDescriptor<T>()
		{
			Actions = profile.OnDeletedActions
		};

		configure.Invoke(descriptor);

		return this;
	}
	public IRepositoryActionProfileBuilder<T> ConfigureOnUpdate(Action<IRepositoryActionDescriptor<T>> configure)
	{
		EnsureDescriptorNotNull(configure);

		var descriptor = new RepositoryActionDescriptor<T>()
		{
			Actions = profile.OnUpdateActions
		};

		configure.Invoke(descriptor);

		return this;
	}
	public IRepositoryActionProfileBuilder<T> ConfigureOnUpdated(Action<IRepositoryActionDescriptor<T>> configure)
	{
		EnsureDescriptorNotNull(configure);

		var descriptor = new RepositoryActionDescriptor<T>()
		{
			Actions = profile.OnUpdatedActions
		};

		configure.Invoke(descriptor);

		return this;
	}
	public IRepositoryActionProfile<T> Build() => profile;
}