<Query Kind="Program">
  <Namespace>System.Collections.ObjectModel</Namespace>
</Query>

void Main()
{


}


public interface IState {
	
}
public interface IStateStore
{
	void AddStateWatcher();
}

public interface IStateManager
{

	Tuple<T, StateSetter<T>> UseState<T>(T state = default);
	void UseEffect(Action action);
	void UseEffect(Action action, params object[] dependencies);
}

public delegate void StateSetter<T>(T state);


internal sealed class StateStore
{

}

public partial class StateManager : IStateManager
{
	private readonly IStateStore store;

	public StateManager(IStateStore store)
	{
		this.store = store;
	}


	public void UseEffect(Action action)
	{

	}


	public void UseEffect(Action action, params object[] dependencies)
	{

	}

	public Tuple<T, StateSetter<T>> UseState<T>(T state = default)
	{
		return new Tuple<T, StateSetter<T>>(state, change =>
		{



		});
		throw new NotImplementedException();
	}
}





