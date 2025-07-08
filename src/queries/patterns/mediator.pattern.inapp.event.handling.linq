<Query Kind="Program" />

void Main()
{
	
}



public interface IEventHandler
{
	/// <summary>
	/// This callback should be set from the mediator for 
	/// further downstream events.
	/// </summary>
	EventCallback Notify { get; set; }

	/// <summary>
	/// The invocation resulting from a the handler 
	/// being notified from the mediator.
	/// </summary>
	/// <param name="eventId"></param>
	/// <param name="context"></param>
	void Invoke(string eventId, IEventContext context);
}

public interface IEventMediator
{
	/// <summary>
	/// A collection handlers
	/// </summary>
	IEnumerable<IEventHandler> Handlers { get; }
	/// <summary>
	/// Subscribes and observer to the subject.
	/// </summary>
	/// <param name="handler"></param>
	/// <returns>return the current instance of <see cref="IEventMediator"/>.</returns>
	IEventMediator Attach(IEventHandler handler);
	/// <summary>
	/// 
	/// </summary>
	/// <param name="eventId"></param>
	/// <param name="context"></param>
	void Notify(string eventId, IEventContext context);
}

/// <summary>
/// 
/// </summary>
public interface IEventMediatorFactory
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="mediatorId"></param>
	/// <returns></returns>
	IEventMediator CreateMediator(string mediatorId);
	/// <summary>
	/// 
	/// </summary>
	/// <param name="mediatorId"></param>
	/// <param name="configure"></param>
	/// <returns></returns>
	IEventMediator CreateMediator(string mediatorId, Func<IEventMediator> configure);
	/// <summary>
	/// 
	/// </summary>
	/// <param name="mediatorId"></param>
	/// <param name="configure"></param>
	/// <returns></returns>
	IEventMediator CreateMediator(string mediatorId, Action<IEventMediator> configure);
}