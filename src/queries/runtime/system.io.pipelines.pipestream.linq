<Query Kind="Program">
  <NuGetReference>System.IO.Pipelines</NuGetReference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.IO.Pipelines</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
  <Namespace>System.Buffers</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
  <Namespace>System.Threading.Tasks.Sources</Namespace>
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

void Main()
{
	var connection = new PipelineConnectionFactory(new ()
	{
			
	})
		.Create(default);
		
	var pipeline = connection.Start();
	
	pipeline.ServerPipe.

}

// Converts a PipeReader and Pipe Writer into a stream 
public enum PipelineState
{
	Unknown = 0,
	Running = 1,
	Shutdown = 2
}

public sealed class PipelineConnectionPipe
{
	private volatile PipelineStream stream;

	public PipelineConnectionPipe(PipeReader reader, PipeWriter writer)
	{
		Input = reader;
		Output = writer;
	}

	public PipeReader Input { get; }
	public PipeWriter Output { get; }
	public Stream GetStream() => this.stream ??= new PipelineStream(this);
}
public sealed class Pipeline : IThreadPoolWorkItem
{
	private static readonly int MinAllocBufferSize = 4096 / 2;

	private readonly CancellationTokenSource _connectionClosedTokenSource = new CancellationTokenSource();
	private readonly TaskCompletionSource connectionClosingProcess = new TaskCompletionSource();

	private readonly PipelineConnectionPipe initialServerPipe;
	private readonly PipelineConnectionPipe initialClientPipe;

	private readonly Socket socket;
	private volatile bool isSocketDisposed;

	private PipelineConnectionReceiver receiver;
	private PipelineConnectionSender sender;
	private PipelineConnectionSenderPool senderPool;

	private Exception shutdownReason;


	private bool isConnectionClosed;
	private readonly object _shutdownLock = new object();

	public Pipeline(PipelineContext context)
	{
		this.senderPool = context.SenderPool;
		this.socket = context.Socket;
		this.receiver = new PipelineConnectionReceiver(context.Scheduler);

		var input = new Pipe(context.InputOptions);
		var output = new Pipe(context.OutputOptions);

		this.ServerPipe = initialServerPipe = new PipelineConnectionPipe(output.Reader, input.Writer);
		this.ClientPipe = initialClientPipe = new PipelineConnectionPipe(input.Reader, output.Writer);

		//		if (context.TlsOptions is not null)
		//		{
		//			var tlsStream = new SslStream(ServerPipe.GetStream());
		//
		//			tlsStream.AuthenticateAsServer(new SslServerAuthenticationOptions()
		//			{
		//				ServerCertificate = context.TlsOptions.ServerCertificate,
		//
		//			});
		//
		//			this.ServerPipe = new HttpSocketTransportConnectionPipe(
		//				reader: PipeReader.Create(tlsStream),
		//				writer: PipeWriter.Create(tlsStream));
		//		}
	}

	public PipelineState State { get; set; } = PipelineState.Running;
	public PipelineConnectionPipe ServerPipe { get; set; }
	public PipelineConnectionPipe ClientPipe { get; init; }

	public void Execute()
	{
		_ = SendAsync();
		_ = ReceiveAsync();
	}


	public async Task ReceiveAsync()
	{
		Exception? receivingError = null;

		try
		{
			while (true)
			{
				// Ensure we have some reasonable amount of buffer space
				var receiverBuffer = ClientPipe.Output.GetMemory(MinAllocBufferSize);
				var receiverResult = await receiver.ReceiveAsync(socket, receiverBuffer);

				if (receiverResult.BytesTransferred == 0)
				{
					// FIN
					//_trace.ConnectionReadFin(this);
					break;
				}

				ClientPipe.Output.Advance(receiverResult.BytesTransferred);

				var flushResultTask = ClientPipe.Output.FlushAsync();
				var flushResultTaskPaused = !flushResultTask.IsCompleted;

				if (flushResultTask.IsCompleted)
				{
					// TODO: Add 'Connection Paused' Trace
				}

				var flushResult = await flushResultTask;

				if (flushResultTaskPaused)
				{
					// TODO: Add 'Connection Resumed' Trace
				}
				if (flushResult.IsCompleted || flushResult.IsCanceled)
				{
					// Pipe consumer is shut down, do we stop writing
					break;
				}
			}
		}
		catch (SocketException exception) when (IsConnectionResetError(exception.SocketErrorCode))
		{
			// This could be ignored if _shutdownReason is already set.
			receivingError = exception;

			// There's still a small chance that both DoReceive() and DoSend() can log the same connection reset.
			// Both logs will have the same ConnectionId. I don't think it's worthwhile to lock just to avoid this.
			if (!isSocketDisposed)
			{
				// TODO: Add TRACE 'Connection Reset'
			}
		}
		catch (Exception exception)
		{
			// This exception should always be ignored because _shutdownReason should be set.
			receivingError = exception;

			if ((exception is SocketException socketException && IsConnectionAbortError(socketException.SocketErrorCode)) || exception is ObjectDisposedException)
			{
				if (!isSocketDisposed)
				{
					// This is unexpected if the socket hasn't been disposed yet.
					// TODO: Add 'Connection Error' Trace
				}
			}
			else
			{
				// TODO: Add 'Connection Error' Trace
			}
		}
		finally
		{
			// If Shutdown() has already bee called, assume that was the reason ProcessReceives() exited.
			ClientPipe.Output.Complete(shutdownReason ?? receivingError);

			// Guard against scheduling this multiple times
			if (!isConnectionClosed)
			{
				isConnectionClosed = true;

				ThreadPool.UnsafeQueueUserWorkItem(state =>
				{
					state.CancelConnectionClosedToken();
					state.connectionClosingProcess.TrySetResult();

				}, this, preferLocal: false);

				var task = connectionClosingProcess.Task;

				while (true)
				{
					if (task.IsCompletedSuccessfully || task.IsCompleted)
					{
						Shutdown();
						break;
					}
				}
			}
		}
	}
	public async Task SendAsync()
	{
		Exception? shutdownReason = null;
		Exception? unexpectedError = null;

		try
		{
			while (true)
			{
				var result = await ClientPipe.Input.ReadAsync();
				if (result.IsCanceled)
				{
					break;
				}
				var buffer = result.Buffer;

				if (!buffer.IsEmpty)
				{
					sender = senderPool.Rent();
					var transferResult = await sender.SendAsync(socket, buffer);

					if (transferResult.HasError)
					{
						if (IsConnectionResetError(transferResult.Error.SocketErrorCode))
						{
							var ex = transferResult.Error;

							break;
						}

						if (IsConnectionAbortError(transferResult.Error.SocketErrorCode))
						{
							shutdownReason = transferResult.Error;

							break;
						}

						unexpectedError = shutdownReason = transferResult.Error;
					}
					// We don't return to the pool if there was an exception, and
					// we keep the _sender assigned so that we can dispose it in StartAsync.
					senderPool.Return(sender);
					sender = null;
				}

				ClientPipe.Input.AdvanceTo(buffer.End);

				if (result.IsCompleted)
				{
					break;
				}
			}
		}
		catch (SocketException exception) when (IsConnectionResetError(exception.SocketErrorCode))
		{
			//shutdownReason = new ConnectionResetException(ex.Message, ex);
			//  _trace.ConnectionReset(this);
		}
		catch (Exception exception)
		when ((exception is SocketException socketEx && IsConnectionAbortError(socketEx.SocketErrorCode)) || exception is ObjectDisposedException)
		{
			// This should always be ignored since Shutdown() must have already been called by Abort().
			shutdownReason = exception;
		}
		catch (Exception ex)
		{
			shutdownReason = ex;
			unexpectedError = ex;
			// _trace.ConnectionError(this, unexpectedError);
		}
		finally
		{
			Shutdown();

			// Complete the output after disposing the socket
			ServerPipe.Input.Complete(unexpectedError);

			// Cancel any pending flushes so that the input loop is un-paused
			ClientPipe.Output.CancelPendingFlush();
		}
	}

	private void CancelConnectionClosedToken()
	{
		try
		{
			_connectionClosedTokenSource.Cancel();
		}
		catch (Exception ex)
		{
			//_trace.LogError(0, ex, $"Unexpected exception in {nameof(SocketConnection)}.{nameof(CancelConnectionClosedToken)}.");
		}
	}

	public void Shutdown()
	{
		lock (_shutdownLock)
		{
			if (isSocketDisposed)
			{
				return;
			}

			// Make sure to close the connection only after the _aborted flag is set.
			// Without this, the RequestsCanBeAbortedMidRead test will sometimes fail when
			// a BadHttpRequestException is thrown instead of a TaskCanceledException.
			isSocketDisposed = true;

			// shutdownReason should only be null if the output was completed gracefully, so no one should ever
			// ever observe the nondescript ConnectionAbortedException except for connection middleware attempting
			// to half close the connection which is currently unsupported.
			// _shutdownReason = shutdownReason ?? new ConnectionAbortedException("The Socket transport's send loop completed gracefully.");
			// _trace.ConnectionWriteFin(this, _shutdownReason.Message);

			try
			{
				// Try to gracefully close the socket even for aborts to match libuv behavior.
				socket.Shutdown(SocketShutdown.Both);
			}
			catch
			{
				// Ignore any errors from Socket.Shutdown() since we're tearing down the connection anyway.
			}

			socket.Dispose();

			State = PipelineState.Shutdown;
		}
	}

	private static bool IsConnectionResetError(SocketError errorCode)
	{
		return errorCode == SocketError.ConnectionReset ||
			   errorCode == SocketError.Shutdown ||
			   (errorCode == SocketError.ConnectionAborted && OperatingSystem.IsWindows());
	}
	private static bool IsConnectionAbortError(SocketError errorCode)
	{
		// Calling Dispose after ReceiveAsync can cause an "InvalidArgument" error on *nix.
		return errorCode == SocketError.OperationAborted ||
			   errorCode == SocketError.Interrupted ||
			   (errorCode == SocketError.InvalidArgument && !OperatingSystem.IsWindows());
	}
}
public sealed class PipelineOptions
{
	/// <summary>
	/// THe logger used for the underlying transport.
	/// </summary>
	//public ILogger? Logger { get; set; }
	/// <summary>
	/// The endpoint in which the socket should listen on.
	/// </summary>
	public EndPoint? Endpoint { get; set; }
	/// <summary>
	/// The number of I/O queues used to process requests. Set to 0 to directly schedule I/O to the ThreadPool.
	/// </summary>
	/// <remarks>
	/// Defaults to <see cref="Environment.ProcessorCount" /> rounded down and clamped between 1 and 16.
	/// </remarks>
	public int IOQueueCount { get; set; } = Math.Min(Environment.ProcessorCount, 16);

	/// <summary>
	/// Wait until there is data available to allocate a buffer. Setting this to false can increase throughput at the cost of increased memory usage.
	/// </summary>
	/// <remarks>
	/// Defaults to true.
	/// </remarks>
	public bool WaitForDataBeforeAllocatingBuffer { get; set; } = true;

	/// <summary>
	/// Set to false to enable Nagle's algorithm for all connections.
	/// </summary>
	/// <remarks>
	/// Defaults to true.
	/// </remarks>
	public bool NoDelay { get; set; } = true;

	/// <summary>
	/// The maximum length of the pending connection queue.
	/// </summary>
	/// <remarks>
	/// Defaults to 512.
	/// </remarks>
	public int Backlog { get; set; } = 512;

	/// <summary>
	/// Gets or sets the maximum unconsumed incoming bytes the transport will buffer.
	/// </summary>
	/// <remarks>
	/// Defaults to '1024 * 1024'.
	/// </remarks>
	public long? MaxReadBufferSize { get; set; } = 1024 * 1024;

	/// <summary>
	/// Gets or sets the maximum outgoing bytes the transport will buffer before applying write back-pressure.
	/// </summary>
	/// <remarks>
	/// Defaults to '64 * 1024'.
	/// </remarks>
	public long? MaxWriteBufferSize { get; set; } = 64 * 1024;

	/// <summary>
	/// In-line application and transport continuations instead of dispatching to the thread-pool.
	/// </summary>
	/// <remarks>
	/// This will run application code on the IO thread which is why this is unsafe.
	/// It is recommended to set the DOTNET_SYSTEM_NET_SOCKETS_INLINE_COMPLETIONS environment variable to '1' when using this setting to also in-line the completions
	/// at the runtime layer as well.
	/// This setting can make performance worse if there is expensive work that will end up holding onto the IO thread for longer than needed.
	/// Test to make sure this setting helps performance.
	/// </remarks>
	public bool UnsafePreferInlineScheduling { get; set; }

	/// <summary>
	/// Specifies whether or not when receiving data after a connection is initialized to 
	/// wait on either data receiving before 
	/// </summary>
	/// <remarks>
	/// Defaults to 'true'.
	/// </remarks>
	public bool WaitOnPacketInjestion { get; set; } = true;

	/// <summary>
	/// 
	/// </summary>
	//public TlsConnectionPipelineOptions? TlsOptions { get; set; }

	internal Func<MemoryPool<byte>> CreateMemoryPool { get; set; } = PipelineMemoryPool.Create;
}
public sealed class PipelineContext
{
	public Socket Socket { get; set; }
	public PipelineConnectionSenderPool SenderPool { get; set; }
	public PipeOptions InputOptions { get; init; } = default!;
	public PipeOptions OutputOptions { get; init; } = default!;
	public PipeScheduler Scheduler { get; init; } = default!;
	public MemoryPool<byte> MemmoryPool { get; set; }

	public bool WaitForDataBeforeAllocatingBuffer { get; set; }
}
public sealed class PipelineScheduler : PipeScheduler, IThreadPoolWorkItem
{
	private readonly ConcurrentQueue<Work> queue;
	private int active;

	public PipelineScheduler()
	{
		this.queue = new ConcurrentQueue<Work>();
	}

	public override void Schedule(Action<object?> action, object? state)
	{
		queue.Enqueue(new Work(action, state));

		// Set working if it wasn't (via atomic Interlocked).
		if (Interlocked.CompareExchange(ref active, 1, 0) == 0)
		{
			// Wasn't working, schedule.
			System.Threading.ThreadPool.UnsafeQueueUserWorkItem(this, preferLocal: false);
		}
	}

	void IThreadPoolWorkItem.Execute()
	{
		while (true)
		{
			while (queue.TryDequeue(out Work item))
			{
				item.Callback(item.State);
			}

			// All work done.

			// Set 'active' (0 == false) prior to checking IsEmpty to catch any missed work in interim.
			// This doesn't need to be volatile due to the following barrier (i.e. it is volatile).
			active = 0;

			// Ensure 'active' is written before IsEmpty is read.
			// As they are two different memory locations, we insert a barrier to guarantee ordering.
			Thread.MemoryBarrier();

			// Check if there is work to do
			if (queue.IsEmpty)
			{
				// Nothing to do, exit.
				break;
			}

			// Is work, can we set it as active again (via atomic Interlocked), prior to scheduling?
			if (Interlocked.Exchange(ref active, 1) == 1)
			{
				// Execute has been rescheduled already, exit.
				break;
			}

			// Is work, wasn't already scheduled so continue loop.
		}
	}

	private readonly struct Work
	{
		public readonly Action<object?> Callback;
		public readonly object? State;

		public Work(Action<object?> callback, object? state)
		{
			Callback = callback;
			State = state;
		}
	}
}
public sealed class PipelineConnectionFactory : IDisposable
{

	private readonly PipelineContext[] contextQueue;
	private readonly PipelineOptions options;
	private readonly MemoryPool<byte> _memoryPool;
	private readonly int contextQueueCount;

	// long to prevent overflow
	private long contextQueueIndex;

	public PipelineConnectionFactory(PipelineOptions options)
	{
		this.options = options;

		_memoryPool = options.CreateMemoryPool();
		contextQueueCount = options.IOQueueCount;

		var maxReadBufferSize = options.MaxReadBufferSize ?? 0;
		var maxWriteBufferSize = options.MaxWriteBufferSize ?? 0;
		var applicationScheduler = options.UnsafePreferInlineScheduling ? PipeScheduler.Inline : PipeScheduler.ThreadPool;

		if (contextQueueCount > 0)
		{
			contextQueue = new PipelineContext[contextQueueCount];

			for (var i = 0; i < contextQueueCount; i++)
			{
				var transportScheduler = options.UnsafePreferInlineScheduling ? PipeScheduler.Inline : new PipelineScheduler();
				// https://github.com/aspnet/KestrelHttpServer/issues/2573
				var awaiterScheduler = OperatingSystem.IsWindows() ? transportScheduler : PipeScheduler.Inline;

				contextQueue[i] = new PipelineContext()
				{
					//TlsOptions = options.TlsOptions,
					Scheduler = transportScheduler,
					InputOptions = new PipeOptions(_memoryPool, applicationScheduler, transportScheduler, maxReadBufferSize, maxReadBufferSize / 2, useSynchronizationContext: false),
					OutputOptions = new PipeOptions(_memoryPool, transportScheduler, applicationScheduler, maxWriteBufferSize, maxWriteBufferSize / 2, useSynchronizationContext: false),
					SenderPool = new PipelineConnectionSenderPool(awaiterScheduler)
				};
			}
		}
		else
		{
			var transportScheduler = options.UnsafePreferInlineScheduling ? PipeScheduler.Inline : PipeScheduler.ThreadPool;
			// https://github.com/aspnet/KestrelHttpServer/issues/2573
			var awaiterScheduler = OperatingSystem.IsWindows() ? transportScheduler : PipeScheduler.Inline;
			contextQueue = new PipelineContext[]
			{
					new PipelineContext()
					{
						//TlsOptions = options.TlsOptions,
						Scheduler = transportScheduler,
						InputOptions = new PipeOptions(_memoryPool, applicationScheduler, transportScheduler, maxReadBufferSize, maxReadBufferSize / 2, useSynchronizationContext: false),
						OutputOptions = new PipeOptions(_memoryPool, transportScheduler, applicationScheduler, maxWriteBufferSize, maxWriteBufferSize / 2, useSynchronizationContext: false),
						SenderPool = new PipelineConnectionSenderPool(awaiterScheduler)
					}
			};
			contextQueueCount = 1;
		}
	}
	public PipelineConnection Create(Socket socket)
	{
		var context = contextQueue[Interlocked.Increment(ref contextQueueIndex) % contextQueueCount];

		context.WaitForDataBeforeAllocatingBuffer = options.WaitForDataBeforeAllocatingBuffer;
		context.Socket = socket;

		return new PipelineConnection(context);
	}
	public void Dispose()
	{
		// Dispose the memory pool
		_memoryPool.Dispose();
	}
}
public sealed class PipelineConnection 
{

	private PipelineContext context;
	private Pipeline pipeline;

	public PipelineConnection(PipelineContext context)
	{
		this.context = context;
	}


	/// <inheritdoc />
	public EndPoint? RemoteEndpoint { get; }

	/// <inheritdoc />
	public EndPoint? LocalEndpoint { get; }

	/// <inheritdoc />
	public void Abort()
	{
		pipeline.Shutdown();
	}

	/// <inheritdoc />
	public Task AbortAsync()
	{
		Abort();
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public Pipeline Start()
	{
		pipeline = new Pipeline(context);

		if (!ThreadPool.UnsafeQueueUserWorkItem(pipeline, preferLocal: false))
		{
			// TODO: Need to add exception
		}

		return pipeline;
	}

	/// <inheritdoc />
	public Task<Pipeline> StartAsync()
	{
		return Task.FromResult(Start());
	}
	public void Dispose()
	{
		DisposeAsync().GetAwaiter().GetResult();
	}
	public ValueTask DisposeAsync()
	{
		Abort();
		return ValueTask.CompletedTask;
	}
}
public sealed class PipelineConnectionReceiver : PipelineConnectionAsyncEventArgs
{
	public PipelineConnectionReceiver(PipeScheduler pipeScheduler)
		: base(pipeScheduler) { }

	public ValueTask<PipelineOperationResult> ReceiveAsync(Socket socket, Memory<byte> buffer)
	{
		SetBuffer(buffer);

		if (socket.ReceiveAsync(this))
		{
			return new ValueTask<PipelineOperationResult>(this, 0);
		}

		var bytesTransferred = BytesTransferred;
		var error = SocketError;

		return error == SocketError.Success
			? new ValueTask<PipelineOperationResult>(new PipelineOperationResult(bytesTransferred))
			: new ValueTask<PipelineOperationResult>(new PipelineOperationResult(CreateException(error)));
	}
}
public sealed class PipelineConnectionSender : PipelineConnectionAsyncEventArgs
{
	private List<ArraySegment<byte>>? _bufferList;
	public PipelineConnectionSender(PipeScheduler pipeScheduler) : base(pipeScheduler) { }


	public ValueTask<PipelineOperationResult> SendAsync(Socket socket, in ReadOnlySequence<byte> buffers)
	{
		if (buffers.IsSingleSegment)
		{
			return SendAsync(socket, buffers.First);
		}

		SetBufferList(buffers);

		if (socket.SendAsync(this))
		{
			return new ValueTask<PipelineOperationResult>(this, 0);
		}

		var bytesTransferred = BytesTransferred;
		var error = SocketError;

		return error == SocketError.Success
			? new ValueTask<PipelineOperationResult>(new PipelineOperationResult(bytesTransferred))
			: new ValueTask<PipelineOperationResult>(new PipelineOperationResult(CreateException(error)));
	}

	public void Reset()
	{
		// We clear the buffer and buffer list before we put it back into the pool
		// it's a small performance hit but it removes the confusion when looking at dumps to see this still
		// holds onto the buffer when it's back in the pool
		if (BufferList != null)
		{
			BufferList = null;

			_bufferList?.Clear();
		}
		else
		{
			SetBuffer(null, 0, 0);
		}
	}

	private ValueTask<PipelineOperationResult> SendAsync(Socket socket, ReadOnlyMemory<byte> memory)
	{
		SetBuffer(MemoryMarshal.AsMemory(memory));

		if (socket.SendAsync(this))
		{
			return new ValueTask<PipelineOperationResult>(this, 0);
		}

		var bytesTransferred = BytesTransferred;
		var error = SocketError;

		return error == SocketError.Success
			? new ValueTask<PipelineOperationResult>(new PipelineOperationResult(bytesTransferred))
			: new ValueTask<PipelineOperationResult>(new PipelineOperationResult(CreateException(error)));
	}

	private void SetBufferList(in ReadOnlySequence<byte> buffer)
	{
		Debug.Assert(!buffer.IsEmpty);
		Debug.Assert(!buffer.IsSingleSegment);

		if (_bufferList == null)
		{
			_bufferList = new List<ArraySegment<byte>>();
		}

		foreach (var b in buffer)
		{
			_bufferList.Add(GetArray(b));
		}

		// The act of setting this list, sets the buffers in the internal buffer list
		BufferList = _bufferList;
	}

	private ArraySegment<byte> GetArray(ReadOnlyMemory<byte> memory)
	{
		if (!MemoryMarshal.TryGetArray(memory, out var result))
		{
			throw new InvalidOperationException("Buffer backed by array was expected");
		}
		return result;
	}
}
public sealed class PipelineConnectionSenderPool : IDisposable
{
	private const int MaxQueueSize = 1024; // REVIEW: Is this good enough?
	private readonly ConcurrentQueue<PipelineConnectionSender> _queue = new();
	private int _count;
	private readonly PipeScheduler _scheduler;
	private bool _disposed;

	public PipelineConnectionSenderPool(PipeScheduler scheduler)
	{
		_scheduler = scheduler;
	}

	public PipelineConnectionSender Rent()
	{
		if (_queue.TryDequeue(out var sender))
		{
			Interlocked.Decrement(ref _count);
			return sender;
		}
		return new PipelineConnectionSender(_scheduler);
	}
	public void Return(PipelineConnectionSender sender)
	{
		// This counting isn't accurate, but it's good enough for what we need to avoid using _queue.Count which could be expensive
		if (_disposed || Interlocked.Increment(ref _count) > MaxQueueSize)
		{
			Interlocked.Decrement(ref _count);
			sender.Dispose();
			return;
		}
		sender.Reset();
		_queue.Enqueue(sender);
	}
	public void Dispose()
	{
		if (!_disposed)
		{
			_disposed = true;
			while (_queue.TryDequeue(out var sender))
			{
				sender.Dispose();
			}
		}
	}
}

public abstract class PipelineConnectionAsyncEventArgs : SocketAsyncEventArgs, IValueTaskSource<PipelineOperationResult>
{
	private static readonly Action<object?> continuationCompleted = _ => { };
	private Action<object?>? continuation;
	private readonly PipeScheduler pipeScheduler;


	public PipelineConnectionAsyncEventArgs(PipeScheduler pipeScheduler) : base(unsafeSuppressExecutionContextFlow: true)
	{
		this.pipeScheduler = pipeScheduler;
	}

	protected override void OnCompleted(SocketAsyncEventArgs eventArgs)
	{
		var continuationReference = continuation;
		var continuationState = UserToken;

		if (continuationReference != null || (continuationReference = Interlocked.CompareExchange(ref continuation, continuationCompleted, null)) != null)
		{
			UserToken = null;
			continuation = continuationCompleted; // in case someone's polling IsCompleted
			pipeScheduler.Schedule(continuationReference, continuationState);
		}
	}

	public PipelineOperationResult GetResult(short token)
	{
		continuation = null;

		if (SocketError != SocketError.Success)
		{
			return new PipelineOperationResult(CreateException(SocketError));
		}

		return new PipelineOperationResult(BytesTransferred);
	}

	protected static SocketException CreateException(SocketError socketError)
	{
		return new SocketException((int)socketError);
	}

	public ValueTaskSourceStatus GetStatus(short token)
	{
		return !ReferenceEquals(continuation, continuationCompleted) ? ValueTaskSourceStatus.Pending :
				SocketError == SocketError.Success ? ValueTaskSourceStatus.Succeeded :
				ValueTaskSourceStatus.Faulted;
	}

	public void OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags)
	{
		UserToken = state;
		var prevContinuation = Interlocked.CompareExchange(ref this.continuation, continuation, null);
		if (ReferenceEquals(prevContinuation, continuationCompleted))
		{
			UserToken = null;
			ThreadPool.UnsafeQueueUserWorkItem(continuation, state, preferLocal: true);
		}
	}
}
public readonly struct PipelineOperationResult
{
	public readonly SocketException? Error;

	[MemberNotNullWhen(true, nameof(Error))]
	public readonly bool HasError => Error != null;

	public PipelineOperationResult(int bytesTransferred)
	{
		Error = null;
		BytesTransferred = bytesTransferred;
	}

	public PipelineOperationResult(SocketException exception)
	{
		Error = exception;
		BytesTransferred = 0;
	}


	public readonly bool IsSuccess => Error == null;
	public readonly int BytesTransferred;


	public bool IsNormalCompletion(out Exception exception)
	{
		exception = null;
		if (IsSuccess)
		{
			return true;
		}
		if (IsConnectionResetError(Error.SocketErrorCode))
		{
			// This could be ignored if _shutdownReason is already set.
			var ex = Error;
			return false;
		}
		if (IsConnectionAbortError(Error.SocketErrorCode))
		{
			// This exception should always be ignored because _shutdownReason should be set.
			exception = Error;
			return false;
		}
		// This is unexpected.
		exception = Error;
		return false;
	}

	private static bool IsConnectionResetError(SocketError errorCode)
	{
		return errorCode == SocketError.ConnectionReset ||
			   errorCode == SocketError.Shutdown ||
			   (errorCode == SocketError.ConnectionAborted && OperatingSystem.IsWindows());
	}
	private static bool IsConnectionAbortError(SocketError errorCode)
	{
		// Calling Dispose after ReceiveAsync can cause an "InvalidArgument" error on *nix.
		return errorCode == SocketError.OperationAborted ||
			   errorCode == SocketError.Interrupted ||
			   (errorCode == SocketError.InvalidArgument && !OperatingSystem.IsWindows());
	}
}


public sealed class PipelineMemoryPool : MemoryPool<byte>
{
	/// <summary>
	/// Thread-safe collection of blocks which are currently in the pool. A slab will pre-allocate all of the block tracking objects
	/// and add them to this collection. When memory is requested it is taken from here first, and when it is returned it is re-added.
	/// </summary>
	private readonly ConcurrentQueue<PipelineMemoryPoolBlock> blocks = new();
	/// <summary>
	/// This is part of implementing the IDisposable pattern.
	/// </summary>
	private bool isDisposed; // To detect redundant calls
	private readonly object disposeSync = new object();


	/// <summary>
	/// Max allocation block size for pooled blocks,
	/// larger values can be leased but they will be disposed after use rather than returned to the pool.
	/// </summary>
	public override int MaxBufferSize => BlockSize;
	/// <summary>
	/// The size of a block. 4096 is chosen because most operating systems use 4k pages.
	/// </summary>
	public static int BlockSize => 4096;

	/// <summary>
	/// This default value passed in to Rent to use the default value for the pool.
	/// </summary>
	private const int AnySize = -1;

	public override IMemoryOwner<byte> Rent(int size = AnySize)
	{
		if (size > BlockSize)
		{
			throw new ArgumentOutOfRangeException(nameof(size));
		}
		if (isDisposed)
		{
			throw new ObjectDisposedException("MemoryPool");
		}
		if (blocks.TryDequeue(out var block))
		{
			// block successfully taken from the stack - return it
			return block;
		}
		return new PipelineMemoryPoolBlock(this, BlockSize);
	}

	/// <summary>
	/// Called to return a block to the pool. Once Return has been called the memory no longer belongs to the caller, and
	/// Very Bad Things will happen if the memory is read of modified subsequently. If a caller fails to call Return and the
	/// block tracking object is garbage collected, the block tracking object's finalizer will automatically re-create and return
	/// a new tracking object into the pool. This will only happen if there is a bug in the server, however it is necessary to avoid
	/// leaving "dead zones" in the slab due to lost block tracking objects.
	/// </summary>
	/// <param name="block">The block to return. It must have been acquired by calling Lease on the same memory pool instance.</param>
	internal void Return(PipelineMemoryPoolBlock block)
	{
		if (!isDisposed)
		{
			blocks.Enqueue(block);
		}
	}
	protected override void Dispose(bool disposing)
	{
		if (isDisposed)
		{
			return;
		}
		lock (disposeSync)
		{
			isDisposed = true;

			if (disposing)
			{
				// Discard blocks in pool
				while (blocks.TryDequeue(out _))
				{

				}
			}
		}
	}
	public static MemoryPool<byte> Create() => new PipelineMemoryPool();
}
public sealed class PipelineMemoryPoolBlock : IMemoryOwner<byte>
{
	internal PipelineMemoryPoolBlock(PipelineMemoryPool pool, int length)
	{
		Pool = pool;
		var pinnedArray = GC.AllocateUninitializedArray<byte>(length, pinned: true);

		Memory = MemoryMarshal.CreateFromPinnedArray(pinnedArray, 0, pinnedArray.Length);
	}

	public PipelineMemoryPool Pool { get; }
	public Memory<byte> Memory { get; }
	public void Dispose()
	{
		Pool.Return(this);
	}
}
public sealed class PipelineStream : Stream
{
	private readonly bool _throwOnCancelled;
	private volatile bool _cancelCalled;
	private readonly PipeReader input;
	private readonly PipeWriter output;

	public PipelineStream(PipelineConnectionPipe pipe)
	{
		this.input = pipe.Input;
		this.output = pipe.Output;
	}

	public PipelineStream(PipeReader input, PipeWriter output)
	{
		this.input = input;
		this.output = output;
		this._throwOnCancelled = false;
	}


	public override bool CanRead => true;
	public override bool CanSeek => false;
	public override bool CanWrite => true;
	public override long Length => throw new NotSupportedException();
	public override long Position
	{
		get => throw new NotSupportedException();
		set => throw new NotSupportedException();
	}

	public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
	public override void SetLength(long value) => throw new NotSupportedException();
	public override int Read(byte[] buffer, int offset, int count)
	{
		ValueTask<int> vt = ReadAsyncInternal(new Memory<byte>(buffer, offset, count), default);
		return vt.IsCompleted ?
			vt.Result :
			vt.AsTask().GetAwaiter().GetResult();
	}
	public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default) => ReadAsyncInternal(new Memory<byte>(buffer, offset, count), cancellationToken).AsTask();
	public override ValueTask<int> ReadAsync(Memory<byte> destination, CancellationToken cancellationToken = default) => ReadAsyncInternal(destination, cancellationToken);
	public override void Write(byte[] buffer, int offset, int count) => WriteAsync(buffer, offset, count).GetAwaiter().GetResult();
	public override Task WriteAsync(byte[]? buffer, int offset, int count, CancellationToken cancellationToken)
	{
		return output.WriteAsync(buffer.AsMemory(offset, count), cancellationToken).GetAsTask();
	}
	public override ValueTask WriteAsync(ReadOnlyMemory<byte> source, CancellationToken cancellationToken = default)
	{
		return output.WriteAsync(source, cancellationToken).GetAsValueTask();
	}
	public override void Flush()
	{
		FlushAsync(CancellationToken.None).GetAwaiter().GetResult();
	}
	public override Task FlushAsync(CancellationToken cancellationToken) => output.FlushAsync(cancellationToken).GetAsTask();

	[AsyncMethodBuilder(typeof(PoolingAsyncValueTaskMethodBuilder<>))]
	private async ValueTask<int> ReadAsyncInternal(Memory<byte> destination, CancellationToken cancellationToken)
	{
		while (true)
		{
			var result = await input.ReadAsync(cancellationToken);
			var readableBuffer = result.Buffer;
			try
			{
				if (_throwOnCancelled && result.IsCanceled && _cancelCalled)
				{
					// Reset the bool
					_cancelCalled = false;
					throw new OperationCanceledException();
				}

				if (!readableBuffer.IsEmpty)
				{
					// buffer.Count is int
					var count = (int)Math.Min(readableBuffer.Length, destination.Length);
					readableBuffer = readableBuffer.Slice(0, count);
					readableBuffer.CopyTo(destination.Span);
					return count;
				}

				if (result.IsCompleted)
				{
					return 0;
				}
			}
			finally
			{
				input.AdvanceTo(readableBuffer.End, readableBuffer.End);
			}
		}
	}
	public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state) => TaskToApm.Begin(ReadAsync(buffer, offset, count), callback, state);
	public override int EndRead(IAsyncResult asyncResult) => TaskToApm.End<int>(asyncResult);
	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state) => TaskToApm.Begin(WriteAsync(buffer, offset, count), callback, state);
	public override void EndWrite(IAsyncResult asyncResult) => TaskToApm.End(asyncResult);




}


namespace System
{
	internal static class ValueTaskExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Task GetAsTask(this in ValueTask<FlushResult> valueTask)
		{
			// Try to avoid the allocation from AsTask
			if (valueTask.IsCompletedSuccessfully)
			{
				// Signal consumption to the IValueTaskSource
				valueTask.GetAwaiter().GetResult();
				return Task.CompletedTask;
			}
			else
			{
				return valueTask.AsTask();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ValueTask GetAsValueTask(this in ValueTask<FlushResult> valueTask)
		{
			// Try to avoid the allocation from AsTask
			if (valueTask.IsCompletedSuccessfully)
			{
				// Signal consumption to the IValueTaskSource
				valueTask.GetAwaiter().GetResult();
				return default;
			}
			else
			{
				return new ValueTask(valueTask.AsTask());
			}
		}
	}

	internal static class TaskToApm
	{
		/// <summary>
		/// Marshals the Task as an IAsyncResult, using the supplied callback and state
		/// to implement the APM pattern.
		/// </summary>
		/// <param name="task">The Task to be marshaled.</param>
		/// <param name="callback">The callback to be invoked upon completion.</param>
		/// <param name="state">The state to be stored in the IAsyncResult.</param>
		/// <returns>An IAsyncResult to represent the task's asynchronous operation.</returns>
		public static IAsyncResult Begin(Task task, AsyncCallback? callback, object? state) => new TaskAsyncResult(task, state, callback);

		/// <summary>Processes an IAsyncResult returned by Begin.</summary>
		/// <param name="asyncResult">The IAsyncResult to unwrap.</param>
		public static void End(IAsyncResult asyncResult)
		{
			if (asyncResult is TaskAsyncResult twar)
			{
				twar.task.GetAwaiter().GetResult();
				return;
			}

			ArgumentNullException.ThrowIfNull(asyncResult, nameof(asyncResult));
		}

		/// <summary>Processes an IAsyncResult returned by Begin.</summary>
		/// <param name="asyncResult">The IAsyncResult to unwrap.</param>
		public static TResult End<TResult>(IAsyncResult asyncResult)
		{
			if (asyncResult is TaskAsyncResult twar && twar.task is Task<TResult> task)
			{
				return task.GetAwaiter().GetResult();
			}

			throw new ArgumentNullException(nameof(asyncResult));
		}

		/// <summary>Provides a simple IAsyncResult that wraps a Task.</summary>
		/// <remarks>
		/// We could use the Task as the IAsyncResult if the Task's AsyncState is the same as the object state,
		/// but that's very rare, in particular in a situation where someone cares about allocation, and always
		/// using TaskAsyncResult simplifies things and enables additional optimizations.
		/// </remarks>
		internal sealed class TaskAsyncResult : IAsyncResult
		{
			/// <summary>The wrapped Task.</summary>
			internal readonly Task task;
			/// <summary>Callback to invoke when the wrapped task completes.</summary>
			private readonly AsyncCallback? callback;

			/// <summary>Initializes the IAsyncResult with the Task to wrap and the associated object state.</summary>
			/// <param name="task">The Task to wrap.</param>
			/// <param name="state">The new AsyncState value.</param>
			/// <param name="callback">Callback to invoke when the wrapped task completes.</param>
			internal TaskAsyncResult(Task task, object? state, AsyncCallback? callback)
			{
				Debug.Assert(task != null);
				this.task = task;
				AsyncState = state;

				if (task.IsCompleted)
				{
					// Synchronous completion.  Invoke the callback.  No need to store it.
					CompletedSynchronously = true;
					callback?.Invoke(this);
				}
				else if (callback != null)
				{
					// Asynchronous completion, and we have a callback; schedule it. We use OnCompleted rather than ContinueWith in
					// order to avoid running synchronously if the task has already completed by the time we get here but still run
					// synchronously as part of the task's completion if the task completes after (the more common case).
					this.callback = callback;
					this.task.ConfigureAwait(continueOnCapturedContext: false)
						 .GetAwaiter()
						 .OnCompleted(InvokeCallback); // allocates a delegate, but avoids a closure
				}
			}

			/// <summary>Invokes the callback.</summary>
			private void InvokeCallback()
			{
				Debug.Assert(!CompletedSynchronously);
				Debug.Assert(callback != null);
				callback.Invoke(this);
			}

			/// <summary>Gets a user-defined object that qualifies or contains information about an asynchronous operation.</summary>
			public object? AsyncState { get; }
			/// <summary>Gets a value that indicates whether the asynchronous operation completed synchronously.</summary>
			/// <remarks>This is set lazily based on whether the <see cref="task"/> has completed by the time this object is created.</remarks>
			public bool CompletedSynchronously { get; }
			/// <summary>Gets a value that indicates whether the asynchronous operation has completed.</summary>
			public bool IsCompleted => task.IsCompleted;
			/// <summary>Gets a <see cref="WaitHandle"/> that is used to wait for an asynchronous operation to complete.</summary>
			public WaitHandle AsyncWaitHandle => ((IAsyncResult)task).AsyncWaitHandle;
		}
	}
}
