<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	var incrementer = new Incrementer();
	
	int i;
	await ThreadTestBuilder<Incrementer>
		.Create(value =>
		{
			var end = DateTime.UtcNow.Ticks + TimeSpan.FromSeconds(30).Ticks;
			try
			{
				while (true)
				{
					if (DateTime.UtcNow.Ticks >= end)
					{
						break;
					}
					incrementer.Value++;
				}
			}
			catch (Exception exception)
			{
				
			}
		})
		.UseThreadCount(100)
		.RunAsync(incrementer);
		
		incrementer.Value.Dump();
}


public class Incrementer 
{
	public long _value;
	
	
	public long Value {
		get => _value;
		set => _value = value;
	}
	
	public Incrementer()
	{
		_value = 0;
	}
}



public class ThreadTestBuilder<T>
{
	private readonly Action<T> _test;
	
	private int _count;
	private TimeSpan _timeout;
	
	ThreadTestBuilder(Action<T> test)
	{
		_test = test;
		_count = 2;
		_timeout = TimeSpan.FromMinutes(2);
	}
	
	
	public ThreadTestBuilder<T> UseTimeout(TimeSpan timeout)
	{
		_timeout = timeout;
		return this;
	}
	public ThreadTestBuilder<T> UseThreadCount(int count)
	{
		if (count < 2)
		{
			throw new ArgumentException("Count must be greater than or equal to 2.");
		}
		_count = count;
		return this;
	}
	
	
	public Task RunAsync(T state)
	{
		var threads = new List<Thread>();
		
		for (int i = 0 ; i < _count; i++)
		{
			var thread = new Thread(item =>
			{
				_test.Invoke(((T)item));
			});
			
			thread.Start();
			threads.Add(thread);
			thread.Join();
		}

		return Task.Run(() =>
		{
			long end = DateTime.UtcNow.Ticks + _timeout.Ticks;
			
			while (threads.Any(p => p.IsAlive))
			{
				if (DateTime.UtcNow.Ticks >= end)
				{
					break;
				}
			}
			
			
		});
	}
	
	
	public static ThreadTestBuilder<T> Create(Action<T> test)
	{
		return new ThreadTestBuilder<T>(test);
	}
}