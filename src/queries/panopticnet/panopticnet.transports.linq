<Query Kind="Program">
  <NuGetReference>System.IO.Pipelines</NuGetReference>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.IO.Pipelines</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Buffers</Namespace>
  <Namespace>System.Threading.Tasks.Sources</Namespace>
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
</Query>


async Task Main()
{
	var serverThread = new Thread(new ThreadStart(async () =>
	{
		var transport = new TcpServerTransport(new());
		var connection = await transport.AcceptAsync();
		var pipeline = await connection.StartAsync();

		while (true)
		{
			try
			{

				var content = await pipeline.ReadAsync();
				content.Dump();

				await pipeline.SendAsync("Hello from server");
			}
			catch (Exception exception)
			{
				exception.Dump();
				connection.Abort();
			}
		}
	}));
	var clientThread = new Thread(new ThreadStart(async () =>
	{
		var client = new TcpClientTransport(new());
		var connection = await client.ConnectAsync();
		var pipeline = await connection.StartAsync();

		while (true)
		{
			try
			{

				await pipeline.SendAsync("Hellow from client");

				var content = await pipeline.ReadAsync();
				content.Dump();

				Thread.Sleep(3000);
			}
			catch (Exception exception)
			{
				exception.Dump();
				connection.Abort();
			}
		}
	}));

	serverThread.Start();
	clientThread.Start();
	
	serverThread.Join();
}

#region Interfaces
public interface ITransport : IDisposable
{
	ITransportConnection Initialize();
	Task<ITransportConnection> InitializeAsync(CancellationToken cancellationToken = default);
}
public interface ITransportConnection
{

	EndPoint LocalEndpoint { get; }
	EndPoint RemoteEndpoint { get; }
	ITransportPipeline Start();
	Task<ITransportPipeline> StartAsync();
	void Abort();
	Task AbortAsync();
}
public interface ITransportPipeline : IThreadPoolWorkItem
{
	ITransportPipe Client { get; }
	ITransportPipe Server { get; }
}
public interface ITransportPipe : IDuplexPipe
{
	Stream GetStream();
}
#endregion

#region Abstractions

/// <summary>
/// Represents a transport to be used for an underlying client.
/// </summary>
public abstract class ClientTransport : ITransport
{
	/// <summary>
	/// A method that connects to a remote host (server) and returns a <see cref="ITransportConnection"/> object.
	/// </summary>
	/// <returns></returns>
	public abstract Task<ClientTransportConnection> ConnectAsync(CancellationToken cancellationToken = default);

	/// <inheritdoc />
	public abstract void Dispose();



	ITransportConnection ITransport.Initialize() => ConnectAsync().GetAwaiter().GetResult();
	async Task<ITransportConnection> ITransport.InitializeAsync(CancellationToken cancellationToken) => await ConnectAsync(cancellationToken);
}
public abstract class ClientTransportConnection : ITransportConnection
{
	public virtual EndPoint LocalEndpoint { get; }
	public virtual EndPoint RemoteEndpoint { get; }


	public abstract ClientTransportPipeline Start();
	public abstract Task<ClientTransportPipeline> StartAsync();
	public abstract void Abort();
	public abstract Task AbortAsync();


	ITransportPipeline ITransportConnection.Start() => Start();
	Task<ITransportPipeline> ITransportConnection.StartAsync() => Task.FromResult<ITransportPipeline>(Start());
}
public abstract class ClientTransportPipeline : ITransportPipeline
{
	/// <summary>
	/// Represents incoming data from the server.
	/// </summary>
	public abstract ITransportPipe Client { get; }
	/// <summary>
	/// Represents outgoing data to the server.
	/// </summary>
	public abstract ITransportPipe Server { get; }


	public virtual async Task SendAsync(string message)
	{
		var bytes = Encoding.UTF8.GetBytes(message);
		var memory = Client.Output.GetMemory(bytes.Length);

		bytes.CopyTo(memory);

		Client.Output.Advance(bytes.Length);

		var flushResultTask = Client.Output.FlushAsync();
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
			throw new Exception();
		}
	}

	public virtual async Task<string> ReadAsync()
	{
		var result = await Client.Input.ReadAsync();
		var buffer = result.Buffer.ToArray();

		Client.Input.AdvanceTo(result.Buffer.End);

		return Encoding.UTF8.GetString(buffer);

	}


	public abstract void Execute();
}

public abstract class ServerTransport : ITransport
{

	public abstract Task<ServerTransportConnection> AcceptAsync(CancellationToken cancellationToken = default);
	public abstract void Dispose();


	ITransportConnection ITransport.Initialize() => AcceptAsync().GetAwaiter().GetResult();
	async Task<ITransportConnection> ITransport.InitializeAsync(CancellationToken cancellationToken) => await AcceptAsync(cancellationToken);
}
public abstract class ServerTransportConnection : ITransportConnection
{
	public virtual EndPoint LocalEndpoint { get; }
	public virtual EndPoint RemoteEndpoint { get; }

	public abstract ServerTransportPipeline Start();
	public abstract Task<ServerTransportPipeline> StartAsync();
	public abstract void Abort();
	public abstract Task AbortAsync();


	ITransportPipeline ITransportConnection.Start() => Start();
	Task<ITransportPipeline> ITransportConnection.StartAsync() => Task.FromResult<ITransportPipeline>(Start());
}
public abstract class ServerTransportPipeline : ITransportPipeline
{
	public abstract ITransportPipe Client { get; }
	public abstract ITransportPipe Server { get; }



	public virtual async Task SendAsync(string message)
	{
		var bytes = Encoding.UTF8.GetBytes(message);
		var memory = Server.Output.GetMemory(bytes.Length);
		
	 	bytes.CopyTo(memory);
		
		Server.Output.Advance(bytes.Length);
		
		var flushResultTask = Server.Output.FlushAsync();
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
			throw new Exception();
		}
		

	}

	public virtual async Task<string> ReadAsync()
	{
		var result = await Server.Input.ReadAsync();
		var buffer = result.Buffer.ToArray();
		
		Server.Input.AdvanceTo(result.Buffer.End);

		return Encoding.UTF8.GetString(buffer);

	}


	//public virtual async Task ReadLinesAsync(LineReader reader)
	//{
	//	if (reader is null)
	//	{
	//		throw new ArgumentNullException(nameof(reader));
	//	}
	//	try
	//	{
	//		while (true)
	//		{
	//			var index = 0;
	//			var result = await Server.Input.ReadAsync();
	//			
	//			foreach (var line in ReadLine(result.Buffer))
	//			{
	//				index = index + line.Length;
	//				reader.Invoke(new LineValue(line), CancellationToken.None);
	//			}
	//		
	//			
	//			
	//		}
	//	}
	//	catch(TaskCanceledException cancellation)
	//	{
	//		
	//	}
	//}
	//
	//
	//private bool ReadLine(ReadOnlySequence<byte> sequence, out ReadOnlySequence<byte> remaining, out byte[] buffer)
	//{
	//	buffer = default;
	//	remaining = default;
	//	
	//	var sequenceReader = new SequenceReader<byte>(sequence);
	//	if (sequenceReader.TryReadTo(out ReadOnlySpan<byte> span, new ReadOnlySpan<byte>(new byte[] { (byte)'\r', (byte)'\n' })))
	//	{
	//		buffer = span.ToArray();
	//		remaining = sequenceReader.;
	//	}
	//	
	//}
	public abstract void Execute();
}

public readonly struct LineValue
{
	public LineValue(byte[] line)
	{
		this.Line = line;
	}
	public byte[] Line {get;}
}
public delegate void LineReader(LineValue value, CancellationToken cancelationToken);


#endregion

#region Tcp Server & Client

public sealed class TcpClientTransport : ClientTransport
{

	private readonly TcpClientTransportOptions options;
	private readonly SocketConnectionFactory factory;

	public TcpClientTransport(TcpClientTransportOptions options)
	{
		if (options is null)
		{
			throw new ArgumentNullException(nameof(options));
		}

		this.options = options;
		this.factory = new SocketConnectionFactory(new SocketOptions()
		{
			NoDelay = options.NoDelay,
			Endpoint = options.Endpoint,
			MaxReadBufferSize = options.MaxReadBufferSize,
			MaxWriteBufferSize = options.MaxWriteBufferSize,
			WaitOnPacketInjestion = options.WaitOnPacketInjestion,
			IOQueueCount = options.IOQueueCount,
			UnsafePreferInlineScheduling = options.UnsafePreferInlineScheduling,
			WaitForDataBeforeAllocatingBuffer = options.WaitForDataBeforeAllocatingBuffer
		});
	}

	public override async Task<ClientTransportConnection> ConnectAsync(CancellationToken cancellationToken = default)
	{
		while (true)
		{
			try
			{
				var socket = default(Socket);

				switch (options.Endpoint)
				{
					case UnixDomainSocketEndPoint unix:
						{
							socket = new Socket(unix.AddressFamily, SocketType.Stream, ProtocolType.Unspecified);
							break;
						}
					case IPEndPoint ip:
						{
							socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
							if (ip.Address == IPAddress.IPv6Any) // Expects IPv6Any to bind to both IPv6 and IPv4
							{
								socket.DualMode = true;
							}
							break;
						}
					default:
						{
							socket = new Socket(options.Endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
							break;
						}
				}

				if (socket.LocalEndPoint is IPEndPoint)
				{
					socket.NoDelay = options.NoDelay;
				}

				await socket.ConnectAsync(options.Endpoint);

				return factory.CreateClientConnection(socket);
			}
			catch (Exception exception)
			{

			}
		}
	}

	public override void Dispose()
	{

	}
}
public sealed class TcpClientTransportOptions
{
	/// <summary>
	/// The endpoint in which the socket should listen on.
	/// </summary>
	public EndPoint Endpoint { get; set; } = new IPEndPoint(IPAddress.Loopback, 8081);
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
}
public sealed class TcpServerTransport : ServerTransport
{
	private readonly TcpServerTransportOptions options;

	private Socket listener;
	private readonly SocketConnectionFactory factory;

	public TcpServerTransport(TcpServerTransportOptions options)
	{
		if (options is null)
		{
			throw new ArgumentNullException(nameof(options));
		}

		this.options = options;
		this.factory = new SocketConnectionFactory(new SocketOptions()
		{
			Backlog = options.Backlog,
			NoDelay = options.NoDelay,
			Endpoint = options.Endpoint,
			MaxReadBufferSize = options.MaxReadBufferSize,
			MaxWriteBufferSize = options.MaxWriteBufferSize,
			WaitOnPacketInjestion = options.WaitOnPacketInjestion,
			IOQueueCount = options.IOQueueCount,
			UnsafePreferInlineScheduling = options.UnsafePreferInlineScheduling,
			WaitForDataBeforeAllocatingBuffer = options.WaitForDataBeforeAllocatingBuffer

		});
		Initialize();
	}

	private void Initialize()
	{
		try
		{
			switch (options.Endpoint)
			{
				//case FileHandleEndPoint fileHandle:
				//    {
				//        /* 
				//            We're passing "ownsHandle: true" here even though we don't necessarily
				//            own the handle because Socket.Dispose will clean-up everything safely.
				//            If the handle was already closed or disposed then the socket will
				//            be torn down gracefully, and if the caller never cleans up their handle
				//            then we'll do it for them.

				//            If we don't do this then we run the risk of Kestrel hanging because the
				//            the underlying socket is never closed and the transport manager can hang
				//            when it attempts to stop.
				//        */
				//        listener = new Socket(
				//            new SafeSocketHandle((IntPtr)fileHandle.FileHandle, ownsHandle: true)
				//        );
				//        break;
				//    }
				case UnixDomainSocketEndPoint unix:
					{
						listener = new Socket(unix.AddressFamily, SocketType.Stream, ProtocolType.Unspecified);
						listener.Bind(options.Endpoint);
						break;
					}
				case IPEndPoint ip:
					{
						listener = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

						if (ip.Address == IPAddress.IPv6Any) // Expects IPv6Any to bind to both IPv6 and IPv4
						{
							listener.DualMode = true;
						}
						listener.Bind(options.Endpoint);
						break;
					}
				default:
					{
						listener = new Socket(options.Endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
						listener.Bind(options.Endpoint);
						break;
					}
			}
		}
		catch (SocketException exception) when (exception.SocketErrorCode == SocketError.AddressAlreadyInUse)
		{
			//throw new HttpSocketTransportExcpetion(exception.Message, exception);
		}

		listener.Listen(options.Backlog);
	}

	public override async Task<ServerTransportConnection> AcceptAsync(CancellationToken cancellationToken = default)
	{
		while (true)
		{
			try
			{
				var socket = await listener.AcceptAsync(cancellationToken);

				if (socket.LocalEndPoint is IPEndPoint)
				{
					socket.NoDelay = options.NoDelay;
				}

				return factory.CreateServerConnection(socket);
			}
			catch (ObjectDisposedException)
			{
				return null;
			}
			catch (SocketException exception) when (exception.SocketErrorCode == SocketError.OperationAborted)
			{
				return null; // A call was made to UnbindAsync/DisposeAsync just return null which signals we're done
			}
			catch (SocketException)
			{

			}
			catch (Exception)
			{

			}
		}
	}

	public override void Dispose()
	{
		listener.Close();
		listener.Dispose();
	}
}
public sealed class TcpServerTransportOptions
{
	/// <summary>
	/// The endpoint in which the socket should listen on.
	/// </summary>
	public EndPoint Endpoint { get; set; } = new IPEndPoint(IPAddress.Loopback, 8081);
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
}

#endregion

#region Udp Server & Client

public sealed class UdpClientTransport : ClientTransport
{
	private readonly Socket socket;
	private readonly UdpClientTransportOptions options;
	
	public UdpClientTransport(UdpClientTransportOptions options)
	{
		socket = new Socket(
			AddressFamily.InterNetwork,
			SocketType.Dgram,
			ProtocolType.Udp);
		this.options  = options;
	}
	
	public override async Task<ClientTransportConnection> ConnectAsync(CancellationToken cancellationToken = default)
	{
		try
		{
			await socket.ConnectAsync(options.EndPoint);
		}
		catch
		{
			
		}
	}

	public override void Dispose()
	{
		throw new NotImplementedException();
	}
}
public sealed class UdpClientTransportOptions
{
	public EndPoint EndPoint {get; set; } = new IPEndPoint(IPAddress.Any, 8081);
}

#endregion

#region IP4 Client & Server

#endregion

#region Other
internal class PipelineMemoryPool : MemoryPool<byte>
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
internal class PipelineMemoryPoolBlock : IMemoryOwner<byte>
{
	internal PipelineMemoryPoolBlock(PipelineMemoryPool pool, int length)
	{
		Pool = pool;

		var pinnedArray = GC.AllocateUninitializedArray<byte>(length, pinned: true);

		Memory = MemoryMarshal.CreateFromPinnedArray(pinnedArray, 0, pinnedArray.Length);
	}

	/// <summary>
	/// Back-reference to the memory pool which this block was allocated from. It may only be returned to this pool.
	/// </summary>
	public PipelineMemoryPool Pool { get; }

	public Memory<byte> Memory { get; }

	public void Dispose()
	{
		Pool.Return(this);
	}
}

public sealed class PipeStream : Stream
{
	private readonly bool _throwOnCancelled;
	private volatile bool _cancelCalled;
	private readonly PipeReader input;
	private readonly PipeWriter output;

	public PipeStream(ITransportPipe pipe) : this(pipe.Input, pipe.Output) { }
	public PipeStream(PipeReader input, PipeWriter output)
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

internal static class SocketHelper
{
	internal static bool IsConnectionResetError(SocketError errorCode)
	{
		return errorCode == SocketError.ConnectionReset ||
			   errorCode == SocketError.Shutdown ||
			   (errorCode == SocketError.ConnectionAborted && OperatingSystem.IsWindows());
	}
	internal static bool IsConnectionAbortError(SocketError errorCode)
	{
		// Calling Dispose after ReceiveAsync can cause an "InvalidArgument" error on *nix.
		return errorCode == SocketError.OperationAborted ||
			   errorCode == SocketError.Interrupted ||
			   (errorCode == SocketError.InvalidArgument && !OperatingSystem.IsWindows());
	}
}
namespace System
{
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
}
#endregion

internal readonly struct SocketPipeResult
{
	public readonly SocketException Error = null;


	[MemberNotNullWhen(true, nameof(Error))]
	public readonly bool HasError => Error != null;

	public SocketPipeResult(int bytesTransferred)
	{
		BytesTransferred = bytesTransferred;
	}

	public SocketPipeResult(SocketException exception)
	{
		Error = exception;
		BytesTransferred = 0;
	}

	public readonly bool IsSuccess => Error == null;
	public readonly int BytesTransferred;


	public bool IsNormalCompletion(out Exception? exception)
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
internal class SocketPipeAsyncArgs : SocketAsyncEventArgs, IValueTaskSource<SocketPipeResult>
{
	private static readonly Action<object?> continuationCompleted = _ => { };
	private Action<object?>? continuation;
	private readonly PipeScheduler pipeScheduler;


	public SocketPipeAsyncArgs(PipeScheduler pipeScheduler) : base(unsafeSuppressExecutionContextFlow: true)
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

	public SocketPipeResult GetResult(short token)
	{
		continuation = null;

		if (SocketError != SocketError.Success)
		{
			return new SocketPipeResult(CreateException(SocketError));
		}

		return new SocketPipeResult(BytesTransferred);
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
internal class SocketPipeReceiver : SocketPipeAsyncArgs
{
	public SocketPipeReceiver(PipeScheduler pipeScheduler)
		   : base(pipeScheduler) { }

	public ValueTask<SocketPipeResult> ReceiveAsync(Socket socket, Memory<byte> buffer)
	{
		SetBuffer(buffer);

		if (socket.ReceiveAsync(this))
		{
			return new ValueTask<SocketPipeResult>(this, 0);
		}

		var bytesTransferred = BytesTransferred;
		var error = SocketError;

		return error == SocketError.Success
			? new ValueTask<SocketPipeResult>(new SocketPipeResult(bytesTransferred))
			: new ValueTask<SocketPipeResult>(new SocketPipeResult(CreateException(error)));
	}
}
internal class SocketPipeSender : SocketPipeAsyncArgs
{
	private List<ArraySegment<byte>> _bufferList;

	public SocketPipeSender(PipeScheduler pipeScheduler) : base(pipeScheduler) { }


	public ValueTask<SocketPipeResult> SendAsync(Socket socket, in ReadOnlySequence<byte> buffers)
	{
		if (buffers.IsSingleSegment)
		{
			return SendAsync(socket, buffers.First);
		}

		SetBufferList(buffers);

		if (socket.SendAsync(this))
		{
			return new ValueTask<SocketPipeResult>(this, 0);
		}

		var bytesTransferred = BytesTransferred;
		var error = SocketError;

		return error == SocketError.Success
			? new ValueTask<SocketPipeResult>(new SocketPipeResult(bytesTransferred))
			: new ValueTask<SocketPipeResult>(new SocketPipeResult(CreateException(error)));
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


	private ValueTask<SocketPipeResult> SendAsync(Socket socket, ReadOnlyMemory<byte> memory)
	{
		SetBuffer(MemoryMarshal.AsMemory(memory));

		if (socket.SendAsync(this))
		{
			return new ValueTask<SocketPipeResult>(this, 0);
		}

		var bytesTransferred = BytesTransferred;
		var error = SocketError;

		return error == SocketError.Success
			? new ValueTask<SocketPipeResult>(new SocketPipeResult(bytesTransferred))
			: new ValueTask<SocketPipeResult>(new SocketPipeResult(CreateException(error)));
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
internal class SocketPipeSenderPool : IDisposable
{
	private const int MaxQueueSize = 1024; // REVIEW: Is this good enough?

	private readonly ConcurrentQueue<SocketPipeSender> _queue = new();
	private int _count;
	private readonly PipeScheduler _scheduler;
	private bool _disposed;

	public SocketPipeSenderPool(PipeScheduler scheduler)
	{
		_scheduler = scheduler;
	}

	public SocketPipeSender Rent()
	{
		if (_queue.TryDequeue(out var sender))
		{
			Interlocked.Decrement(ref _count);
			return sender;
		}
		return new SocketPipeSender(_scheduler);
	}
	public void Return(SocketPipeSender sender)
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

internal class SocketPipe : ITransportPipe
{
	private readonly Stream stream;

	public SocketPipe(PipeReader input, PipeWriter output)
	{
		this.Input = input;
		this.Output = output;
		this.stream = new PipeStream(this);
	}

	public PipeReader Input { get; }
	public PipeWriter Output { get; }
	public Stream GetStream() => stream;
}
internal class SocketPipeScheduler : PipeScheduler, IThreadPoolWorkItem
{
	private readonly ConcurrentQueue<Work> queue;
	private int active;

	public SocketPipeScheduler()
	{
		this.queue = new ConcurrentQueue<Work>();
	}

	public override void Schedule(Action<object> action, object state)
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
		public readonly Action<object> Callback;
		public readonly object State;

		public Work(Action<object> callback, object state)
		{
			Callback = callback;
			State = state;
		}
	}
}

internal class SocketOptions
{
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
}
internal class SocketConnectionFactory : IDisposable
{
	private readonly SocketConnectionContext[] contextQueue;
	private readonly SocketOptions options;
	private readonly MemoryPool<byte> _memoryPool;
	private readonly int contextQueueCount;

	// long to prevent overflow
	private long contextQueueIndex;

	public SocketConnectionFactory(SocketOptions options)
	{
		this.options = options;

		_memoryPool = PipelineMemoryPool.Create();
		contextQueueCount = options.IOQueueCount;

		var maxReadBufferSize = options.MaxReadBufferSize ?? 0;
		var maxWriteBufferSize = options.MaxWriteBufferSize ?? 0;
		var applicationScheduler = options.UnsafePreferInlineScheduling ? PipeScheduler.Inline : PipeScheduler.ThreadPool;

		if (contextQueueCount > 0)
		{
			contextQueue = new SocketConnectionContext[contextQueueCount];

			for (var i = 0; i < contextQueueCount; i++)
			{
				var transportScheduler = options.UnsafePreferInlineScheduling ? PipeScheduler.Inline : new SocketPipeScheduler();
				// https://github.com/aspnet/KestrelHttpServer/issues/2573
				var awaiterScheduler = OperatingSystem.IsWindows() ? transportScheduler : PipeScheduler.Inline;

				contextQueue[i] = new SocketConnectionContext()
				{
					//TlsOptions = options.TlsOptions,
					Scheduler = transportScheduler,
					InputOptions = new PipeOptions(_memoryPool, applicationScheduler, transportScheduler, maxReadBufferSize, maxReadBufferSize / 2, useSynchronizationContext: false),
					OutputOptions = new PipeOptions(_memoryPool, transportScheduler, applicationScheduler, maxWriteBufferSize, maxWriteBufferSize / 2, useSynchronizationContext: false),
					SenderPool = new SocketPipeSenderPool(awaiterScheduler)
				};
			}
		}
		else
		{
			var transportScheduler = options.UnsafePreferInlineScheduling ? PipeScheduler.Inline : PipeScheduler.ThreadPool;
			// https://github.com/aspnet/KestrelHttpServer/issues/2573
			var awaiterScheduler = OperatingSystem.IsWindows() ? transportScheduler : PipeScheduler.Inline;
			contextQueue = new SocketConnectionContext[]
			{
					new SocketConnectionContext()
					{
						//TlsOptions = options.TlsOptions,
						Scheduler = transportScheduler,
						InputOptions = new PipeOptions(_memoryPool, applicationScheduler, transportScheduler, maxReadBufferSize, maxReadBufferSize / 2, useSynchronizationContext: false),
						OutputOptions = new PipeOptions(_memoryPool, transportScheduler, applicationScheduler, maxWriteBufferSize, maxWriteBufferSize / 2, useSynchronizationContext: false),
						SenderPool = new SocketPipeSenderPool(awaiterScheduler)
					}
			};
			contextQueueCount = 1;
		}
	}
	public ServerTransportConnection CreateServerConnection(Socket socket)
	{
		var context = contextQueue[Interlocked.Increment(ref contextQueueIndex) % contextQueueCount];

		context.WaitForDataBeforeAllocatingBuffer = options.WaitForDataBeforeAllocatingBuffer;
		context.Socket = socket;

		return new ServerSocketConnection(context);
	}

	public ClientTransportConnection CreateClientConnection(Socket socket)
	{
		var context = contextQueue[Interlocked.Increment(ref contextQueueIndex) % contextQueueCount];

		context.WaitForDataBeforeAllocatingBuffer = options.WaitForDataBeforeAllocatingBuffer;
		context.Socket = socket;

		return new ClientSocketConnection(context);
	}
	public void Dispose()
	{
		// Dispose the memory pool
		_memoryPool.Dispose();
	}
}
internal sealed class SocketConnectionContext
{
	public Socket Socket { get; set; }
	//public TlsConnectionPipelineOptions? TlsOptions { get; init; }
	public SocketPipeSenderPool SenderPool { get; set; }
	public PipeOptions InputOptions { get; init; } = default!;
	public PipeOptions OutputOptions { get; init; } = default!;
	public PipeScheduler Scheduler { get; init; } = default!;
	public MemoryPool<byte> MemmoryPool { get; set; }
	public bool WaitForDataBeforeAllocatingBuffer { get; set; }
}


internal class ServerSocketConnection : ServerTransportConnection
{
	private bool isRunning;

	private readonly ServerSocketPipeline pipeline;

	public ServerSocketConnection(SocketConnectionContext context)
	{
		this.pipeline = new(context);
	}

	public override ServerTransportPipeline Start()
	{
		return StartAsync().GetAwaiter().GetResult();
	}

	public override Task<ServerTransportPipeline> StartAsync()
	{
		if (isRunning)
		{
			throw new InvalidOperationException("The Pipeline has already been initialized.");
		}
		if (!ThreadPool.UnsafeQueueUserWorkItem(pipeline, true))
		{
			throw new Exception();
		}
		isRunning = true;

		return Task.FromResult<ServerTransportPipeline>(pipeline);
	}


	public override void Abort()
	{
		AbortAsync().GetAwaiter().GetResult();
	}

	public override Task AbortAsync()
	{
		pipeline.Shutdown();
		return Task.CompletedTask;
	}
}
internal class ServerSocketPipeline : ServerTransportPipeline
{
	private static readonly int MinAllocBufferSize = PipelineMemoryPool.BlockSize / 2;

	private readonly CancellationTokenSource _connectionClosedTokenSource = new CancellationTokenSource();
	private readonly TaskCompletionSource connectionClosingProcess = new TaskCompletionSource();

	// Will store the threads currently sending and receiving data.
	private Task receiving;
	private Task sending;

	private readonly Socket socket;
	private volatile bool isSocketDisposed;
	private bool isConnectionClosed;
	private readonly object _shutdownLock = new object();

	// Let's save the initial pipes
	private readonly SocketPipe client;
	private readonly SocketPipe server;

	private SocketPipeReceiver receiver;
	private SocketPipeSender sender;
	private SocketPipeSenderPool senderPool;

	private Exception shutdownReason;

	public ServerSocketPipeline(SocketConnectionContext context)
	{
		this.senderPool = context.SenderPool;
		this.socket = context.Socket;
		this.receiver = new SocketPipeReceiver(context.Scheduler);

		var serverPipe = new Pipe(context.InputOptions);
		var clientPipe = new Pipe(context.OutputOptions);

		this.Server = server = new SocketPipe(
			serverPipe.Reader,
			clientPipe.Writer);

		this.Client = client = new SocketPipe(
			clientPipe.Reader,  // client reader - 
			serverPipe.Writer); // server writer

	}

	public override ITransportPipe Client { get; }
	public override ITransportPipe Server { get; }

	public override void Execute()
	{
		receiving = StartReceivingAsync();
		sending = StartSendingAsync();
	}

	private async Task StartReceivingAsync()
	{
		Exception? receivingError = null;

		try
		{
			while (true)
			{
				// Ensure we have some reasonable amount of buffer space
				var receiverBuffer = Client.Output.GetMemory(MinAllocBufferSize);
				var receiverResult = await receiver.ReceiveAsync(socket, receiverBuffer);

				if (receiverResult.BytesTransferred == 0)
				{
					// FIN
					//_trace.ConnectionReadFin(this);
					break;
				}

				Client.Output.Advance(receiverResult.BytesTransferred);

				var flushResultTask = Client.Output.FlushAsync();
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
		catch (SocketException exception) when (SocketHelper.IsConnectionResetError(exception.SocketErrorCode))
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

			if ((exception is SocketException socketException && SocketHelper.IsConnectionAbortError(socketException.SocketErrorCode)) || exception is ObjectDisposedException)
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
			Client.Output.Complete(shutdownReason ?? receivingError);

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
	private async Task StartSendingAsync()
	{
		Exception? shutdownReason = null;
		Exception? unexpectedError = null;

		try
		{
			while (true)
			{
				var result = await Client.Input.ReadAsync();
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
						if (SocketHelper.IsConnectionResetError(transferResult.Error.SocketErrorCode))
						{
							var ex = transferResult.Error;

							break;
						}
						if (SocketHelper.IsConnectionAbortError(transferResult.Error.SocketErrorCode))
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

				Client.Input.AdvanceTo(buffer.End);

				if (result.IsCompleted)
				{
					break;
				}
			}
		}
		catch (SocketException exception) when (SocketHelper.IsConnectionResetError(exception.SocketErrorCode))
		{
			//shutdownReason = new ConnectionResetException(ex.Message, ex);
			//  _trace.ConnectionReset(this);
		}
		catch (Exception exception)
		when ((exception is SocketException socketEx && SocketHelper.IsConnectionAbortError(socketEx.SocketErrorCode)) || exception is ObjectDisposedException)
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
			Server.Input.Complete(unexpectedError);

			// Cancel any pending flushes so that the input loop is un-paused
			Client.Output.CancelPendingFlush();
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

			//State = PipelineState.Shutdown;
		}
	}
}

internal class ClientSocketConnection : ClientTransportConnection
{
	private bool isRunning;

	private readonly ClientSocketPipeline pipeline;

	public ClientSocketConnection(SocketConnectionContext context)
	{
		this.pipeline = new(context);
	}

	public override ClientTransportPipeline Start()
	{
		return StartAsync().GetAwaiter().GetResult();
	}

	public override Task<ClientTransportPipeline> StartAsync()
	{
		if (isRunning)
		{
			throw new InvalidOperationException("The Pipeline has already been initialized.");
		}
		if (!ThreadPool.UnsafeQueueUserWorkItem(pipeline, true))
		{
			throw new Exception();
		}
		isRunning = true;

		return Task.FromResult<ClientTransportPipeline>(pipeline);
	}


	public override void Abort()
	{
		AbortAsync().GetAwaiter().GetResult();
	}

	public override Task AbortAsync()
	{
		//pipeline.Shutdown();
		return Task.CompletedTask;
	}




}
internal class ClientSocketPipeline : ClientTransportPipeline
{

	private static readonly int MinAllocBufferSize = PipelineMemoryPool.BlockSize / 2;

	private readonly CancellationTokenSource _connectionClosedTokenSource = new CancellationTokenSource();
	private readonly TaskCompletionSource connectionClosingProcess = new TaskCompletionSource();

	// Will store the threads currently sending and receiving data.
	private Task receiving;
	private Task sending;

	private readonly Socket socket;
	private volatile bool isSocketDisposed;
	private bool isConnectionClosed;
	private readonly object _shutdownLock = new object();

	// Let's save the initial pipes
	private readonly SocketPipe client;
	private readonly SocketPipe server;

	private SocketPipeReceiver receiver;
	private SocketPipeSender sender;
	private SocketPipeSenderPool senderPool;

	private Exception shutdownReason;

	public ClientSocketPipeline(SocketConnectionContext context)
	{
		this.senderPool = context.SenderPool;
		this.socket = context.Socket;
		this.receiver = new SocketPipeReceiver(context.Scheduler);

		var serverPipe = new Pipe(context.InputOptions);
		var clientPipe = new Pipe(context.OutputOptions);

		this.Server = server = new SocketPipe(
			serverPipe.Reader,
			clientPipe.Writer);

		this.Client = client = new SocketPipe(
			clientPipe.Reader,  // client reader - 
			serverPipe.Writer); // server writer
	}

	public override ITransportPipe Client { get; }
	public override ITransportPipe Server { get; }

	public override void Execute()
	{
		receiving 	= StartReceivingAsync();
		sending 	= StartSendingAsync();
	}

	private async Task StartReceivingAsync()
	{
		Exception? receivingError = null;

		try
		{
			while (true)
			{
				// Ensure we have some reasonable amount of buffer space
				var receiverBuffer = Server.Output.GetMemory(MinAllocBufferSize);
				var receiverResult = await receiver.ReceiveAsync(socket, receiverBuffer);

				if (receiverResult.BytesTransferred == 0)
				{
					// FIN
					//_trace.ConnectionReadFin(this);
					break;
				}

				Server.Output.Advance(receiverResult.BytesTransferred);

				var flushResultTask = Server.Output.FlushAsync();
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
		catch (SocketException exception) when (SocketHelper.IsConnectionResetError(exception.SocketErrorCode))
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

			if ((exception is SocketException socketException && SocketHelper.IsConnectionAbortError(socketException.SocketErrorCode)) || exception is ObjectDisposedException)
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
			Server.Output.Complete(shutdownReason ?? receivingError);

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
	private async Task StartSendingAsync()
	{
		Exception? shutdownReason = null;
		Exception? unexpectedError = null;

		try
		{
			while (true)
			{
				var result = await Server.Input.ReadAsync();
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
						if (SocketHelper.IsConnectionResetError(transferResult.Error.SocketErrorCode))
						{
							var ex = transferResult.Error;

							break;
						}
						if (SocketHelper.IsConnectionAbortError(transferResult.Error.SocketErrorCode))
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

				Server.Input.AdvanceTo(buffer.End);

				if (result.IsCompleted)
				{
					break;
				}
			}
		}
		catch (SocketException exception) when (SocketHelper.IsConnectionResetError(exception.SocketErrorCode))
		{
			//shutdownReason = new ConnectionResetException(ex.Message, ex);
			//  _trace.ConnectionReset(this);
		}
		catch (Exception exception)
		when ((exception is SocketException socketEx && SocketHelper.IsConnectionAbortError(socketEx.SocketErrorCode)) || exception is ObjectDisposedException)
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
			Client.Input.Complete(unexpectedError);

			// Cancel any pending flushes so that the input loop is un-paused
			Server.Output.CancelPendingFlush();
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

			//State = PipelineState.Shutdown;
		}
	}
}
