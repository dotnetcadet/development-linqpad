<Query Kind="Program" />

void Main()
{
	var facotry = EventMediatorEventGridEventFactory.Create(factory =>
	{


		factory
			// Deal Root Events Create
			.AddMessage<DealEventContext<Deal>>(DealEventMediator.ReadModelMediator, DealEvent.DealCreate, context =>
			{
				var egEventType = DealEvent.DealCreate;
				var egRecordId = context.StateChanges.DealId;
				var egSubject = DealRoute.GetDealById.FormatRoute(egRecordId);
				var egTopicName = egOptions.GetTopicName("DealEvents");
				//var egBody = GetBinaryData(egEventType, egRecordId);

				return new EventGridEvent(egSubject, egEventType, "v1.0", new
				{
					Domain = "deal",
					Event = egEventType,
					RecordId = egRecordId,
					Meta = new Dictionary<string, object>()
					{
						{ "PropertyChanges", context.GetPropertyChanges() }
					}
				})
				{
					Topic = egTopicName
				};
			});
);
}


public interface IMessageFactory<out TMessage>
{
	// TContext is data that is used to create the message
	IEnumerable<TMessage> CreateEvents<TContext>(string mediatorId, string eventId, TContext context);
}

internal abstract class EventMediatorMessageFactoryBase<TMessage> : IMessageFactory<TMessage>
{
	private readonly IList<
		Tuple<
			Func<string, string, TContext, bool>, // The rist tuple item 
			Func<TContext, TMessage>>> messages;

	public EventMediatorMessageFactoryBase()
	{
		this.messages = new List<
			Tuple<
				Func<string, string, IEventContext, bool>,
				Func<IEventContext, TMessage>>>();
	}

	protected virtual void AddMessage<TContext>(string mediatorId, string eventId, Func<TContext, TMessage> method) where TContext : IEventContext
	{
		messages.Add(new Tuple<Func<string, string, IEventContext, bool>, Func<IEventContext, TMessage>>(
			(m, e, c) => mediatorId == m && eventId == e && c is TContext,
			(context) => method.Invoke((TContext)context)));
	}

	public IEnumerable<TMessage> CreateEvents(string mediatorId, string eventId, IEventContext context)
	{
		foreach (var message in messages)
		{
			if (message.Item1.Invoke(mediatorId, eventId, context))
			{
				yield return message.Item2.Invoke(context);
			}
		}
	}
}

internal sealed class EventMediatorEventGridEventFactory : EventMediatorMessageFactoryBase<EventGridEvent>
{
	public EventMediatorEventGridEventFactory AddMessage<TContext>(string mediatorId, string eventId, Func<TContext, EventGridEvent> method)
		where TContext : IEventContext
	{
		base.AddMessage(mediatorId, eventId, method);
		return this;
	}

	public static EventMediatorEventGridEventFactory Create(Action<EventMediatorEventGridEventFactory> configure)
	{
		var factory = new EventMediatorEventGridEventFactory();

		configure.Invoke(factory);

		return factory;
	}
}

