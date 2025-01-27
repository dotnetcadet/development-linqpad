<Query Kind="Program">
<NuGetReference Version="8.0.0">System.IO.Pipelines</NuGetReference>
<Namespace>System</Namespace>
<Namespace>System.IO</Namespace>
<Namespace>System.IO.Pipelines</Namespace>
<Namespace>System.Buffers</Namespace>
<Namespace>System.Threading</Namespace>
<Namespace>System.Threading.Tasks</Namespace>
<Namespace>System.Runtime.CompilerServices</Namespace>
<Namespace>Assimalign.Cohesion.Net.Transports.Internal</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>Assimalign.Cohesion.Net</Namespace>
<Namespace>System.Net</Namespace>
<Namespace>System.Diagnostics</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>System.Net.Sockets</Namespace>
<Namespace>System.Collections.Concurrent</Namespace>
<Namespace>System.Runtime.InteropServices</Namespace>
<Namespace>System.Net.Quic</Namespace>
<Namespace>System.Threading.Tasks.Sources</Namespace>
<Namespace>System.Diagnostics.CodeAnalysis</Namespace>
<Namespace>System.Reflection</Namespace>
<Namespace>System.Runtime.Versioning</Namespace>
<Namespace>System.Net.Security</Namespace>
<Namespace>System.Security.Authentication</Namespace>
</Query>
#load ".\assimalign.cohesion.core"

void Main()
{

}

#region Assimalign.Cohesion.Net.Transports(net8.0)
namespace Assimalign.Cohesion.Net.Transports
{
	#region \
	public enum ConnectionState
	{
	    Unknown,
	    Running,
	    Aborted,
	}
	public sealed class PipeStream : Stream
	{
	    private readonly bool throwOnCanceled;
	    private volatile bool isCancelCalled;
	    private readonly PipeReader input;
	    private readonly PipeWriter output;
	    public PipeStream(ITransportConnectionPipe pipe) : this(pipe.Input, pipe.Output) 
	    {
	    }
	    public PipeStream(PipeReader input, PipeWriter output)
	    {
	        if (input is null)
	        {
	            throw new ArgumentNullException(nameof(input));
	        }
	        if (output is null)
	        {
	            throw new ArgumentNullException(nameof(output));
	        }
	        this.input = input;
	        this.output = output;
	        this.throwOnCanceled = false;
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
	        var valueTask = ReadAsyncInternal(new Memory<byte>(buffer, offset, count), default);
	        return valueTask.IsCompleted ?
	            valueTask.Result :
	            valueTask.AsTask().GetAwaiter().GetResult();
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
	                if (throwOnCanceled && result.IsCanceled && isCancelCalled)
	                {
	                    // Reset the bool
	                    isCancelCalled = false;
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
	public enum ProtocolType
	{
	    Unspecified = -1,
	    Icmp,
	    IcmpV6,
	    Igmp,
	    Ggp,
	    IPv6,
	    IPv4,
	    Tcp,
	    Pup,
	    Udp,
	    Idp,
	    ND,
	    Raw,
	    Ipx,
	    Spx,
	    SpxII,
	    Quic
	}
	public abstract class ClientTransport : ITransport
	{
	    public TransportType TransportType => TransportType.Client;
	    public abstract ProtocolType ProtocolType { get; }
	    public abstract TransportMiddlewareHandler Middleware { get; }
	    public abstract Task<ITransportConnection> ConnectAsync(CancellationToken cancellationToken = default);
	    public abstract void Dispose();
	    ITransportConnection ITransport.Initialize() => ConnectAsync().GetAwaiter().GetResult();
	    async Task<ITransportConnection> ITransport.InitializeAsync(CancellationToken cancellationToken) => await ConnectAsync(cancellationToken);    
	}
	public abstract class ServerTransport : ITransport
	{
	    public TransportType TransportType => TransportType.Server;
	    public abstract ProtocolType ProtocolType { get; }
	    public abstract TransportMiddlewareHandler Middleware { get; }
	    public abstract Task<ITransportConnection> AcceptOrListenAsync(CancellationToken cancellationToken = default);
	    public abstract void Dispose();
	    ITransportConnection ITransport.Initialize() => AcceptOrListenAsync().GetAwaiter().GetResult();
	    async Task<ITransportConnection> ITransport.InitializeAsync(CancellationToken cancellationToken) => await AcceptOrListenAsync(cancellationToken);
	}
	public sealed class TransportConnectionPipe : ITransportConnectionPipe
	{
	    private readonly Stream stream;
	    public TransportConnectionPipe(Stream stream)
	    {
	        if (stream is null)
	        {
	            throw new ArgumentNullException(nameof(stream));
	        }
	        this.stream = stream;
	        this.Input = PipeReader.Create(stream);
	        this.Output = PipeWriter.Create(stream);
	    }
	    public TransportConnectionPipe(PipeReader input, PipeWriter output)
	    {
	        if (input is null)
	        {
	            throw new ArgumentNullException(nameof(input));
	        }
	        if (output is null)
	        {
	            throw new ArgumentNullException(nameof(output));
	        }
	        Input = input;
	        Output = output;
	        stream = new PipeStream(this);
	    }
	    public PipeReader Input { get; }
	    public PipeWriter Output { get; }
	    public Stream GetStream() => stream;
	    public async ValueTask<ReadResult> ReadAsync()
	    {
	        var result = await Input.ReadAsync();
	        Input.AdvanceTo(
	            result.Buffer.Start,
	            result.Buffer.End);
	        //Input.AdvanceTo(result.Buffer.End);
	        return result;
	    }
	    public async ValueTask<FlushResult> WriteAsync(ReadOnlyMemory<byte> buffer)
	    {
	        var result = await Output.WriteAsync(buffer);
	        //Output.Advance(buffer.Length);
	        return result;
	    }
	}
	public sealed class TransportMiddlewareBuilder<TContext, TMiddleware> : ITransportMiddlewareBuilder
	    where TContext : ITransportContext
	    where TMiddleware : ITransportMiddleware
	{
	    private readonly Queue<ITransportMiddleware> middleware = new();
	    private int chainIndex;
	    public TransportMiddlewareBuilder<TContext, TMiddleware> UseNext<T>() where T: TMiddleware, new()
	    {
	        this.middleware.Enqueue(new T());
	        return this;
	    }
	    public TransportMiddlewareBuilder<TContext, TMiddleware> UseNext(TMiddleware middleware)
	    {
	        if (middleware is null)
	        {
	            throw new ArgumentNullException(nameof(middleware));
	        }
	        this.middleware.Enqueue(middleware);
	        return this;
	    }
	    public TransportMiddlewareBuilder<TContext, TMiddleware> UseNext(Func<TContext, TransportMiddlewareHandler, Task> middleware)
	    {
	        if (middleware is null)
	        {
	            throw new ArgumentNullException(nameof(middleware));
	        }
	        this.middleware.Enqueue(new TransportMiddlewareDefault((context, next) =>
	        {
	            return middleware.Invoke((TContext)context, next);
	        }));
	        return this;
	    }
	    ITransportMiddlewareBuilder ITransportMiddlewareBuilder.UseNext(ITransportMiddleware middleware)
	    {
	        if (middleware is null)
	        {
	            throw new ArgumentNullException(nameof(middleware));
	        }
	        this.middleware.Enqueue(middleware);
	        return this;
	    }
	    ITransportMiddlewareBuilder ITransportMiddlewareBuilder.UseNext(TransportMiddleware middleware)
	    {
	        if (middleware is null)
	        {
	            throw new ArgumentNullException(nameof(middleware));
	        }
	        this.middleware.Enqueue(new TransportMiddlewareDefault(middleware));
	        return this;
	    }
	    public TransportMiddlewareHandler Build()
	    {
	        var memoise = Cacher<ITransportMiddlewareBuilder, TransportMiddlewareHandler>.Memoise(builder =>
	        {
	            var root = new TransportMiddlewareHandler(context => Task.CompletedTask);
	            return middleware.Count == 0 ? root : Build(root);
	        });
	        return memoise.Invoke(this);
	    }
	    private TransportMiddlewareHandler Build(TransportMiddlewareHandler handler)
	    {
	        var middleware = this.middleware.Reverse().Skip(chainIndex).First();
	        var next = new TransportMiddlewareHandler(context =>
	        {
	            return middleware.InvokeAsync(context, handler);
	        });
	        if (chainIndex < this.middleware.Count - 1)
	        {
	            chainIndex++;
	            return Build(next);
	        }
	        chainIndex = 0;
	        return next;
	    }
	}
	public enum TransportType
	{
	    Client = 1,
	    Server = 2
	}
	#endregion
	#region \Abstractions
	public interface ITransport : IDisposable
	{
	    TransportType TransportType { get; }
	    ProtocolType ProtocolType { get; }
	    TransportMiddlewareHandler Middleware { get; }
	    ITransportConnection Initialize();
	    Task<ITransportConnection> InitializeAsync(CancellationToken cancellationToken = default);
	}
	public interface ITransportConnection : IThreadPoolWorkItem, IDisposable
	{
	    bool IsConnected { get; }
	    object? ConnectionData { get; }
	    ConnectionState State { get; }
	    ITransportConnectionPipe Pipe { get; }
	    EndPoint LocalEndPoint { get; }
	    EndPoint RemoteEndPoint { get; }
	    void Abort();
	    ValueTask AbortAsync();
	}
	public interface ITransportConnectionPipe : IDuplexPipe
	{
	    Stream GetStream();
	    ValueTask<ReadResult> ReadAsync();
	    ValueTask<FlushResult> WriteAsync(ReadOnlyMemory<byte> buffer);
	}
	public interface ITransportContext
	{
	    ITransportConnection Connection { get; }
	}
	public interface ITransportMiddleware
	{
	    Task InvokeAsync(ITransportContext context, TransportMiddlewareHandler next);
	}
	public interface ITransportMiddlewareBuilder
	{
	    ITransportMiddlewareBuilder UseNext(ITransportMiddleware middleware);
	    ITransportMiddlewareBuilder UseNext(TransportMiddleware middleware);
	    TransportMiddlewareHandler Build();
	}
	#endregion
	#region \Delegates
	public delegate Task TransportMiddleware(ITransportContext context, TransportMiddlewareHandler next);
	public delegate Task TransportMiddlewareHandler(ITransportContext context);
	public delegate void TransportTraceHandler(object? traceCode, object? connectionData, string? message = null);
	#endregion
	#region \EndPoints
	public class FileHandleEndPoint : EndPoint
	{
	    public FileHandleEndPoint(ulong fileHandle, FileHandleType fileHandleType)
	    {
	        FileHandle = fileHandle;
	        FileHandleType = fileHandleType;
	        switch (fileHandleType)
	        {
	            case FileHandleType.Auto:
	            case FileHandleType.Tcp:
	            case FileHandleType.Pipe:
	                break;
	            default:
	                throw new NotSupportedException();
	        }
	    }
	    public ulong FileHandle { get; }
	    public FileHandleType FileHandleType { get; }
	}
	public enum FileHandleType
	{
	    Auto,
	    Tcp,
	    Pipe
	}
	#endregion
	#region \Exceptions
	public abstract class TransportException : Exception
	{
	    public TransportException(string message) 
	        : base(message) { }
	    public TransportException(string message, Exception inner) 
	        : base(message, inner) { }
	}
	#endregion
	#region \Internal
	internal class TransportMiddlewareDefault : ITransportMiddleware
	{
	    private readonly TransportMiddleware middleware;
	    public TransportMiddlewareDefault(TransportMiddleware middleware)
	    {
	        if (middleware is null)
	        {
	            throw new ArgumentNullException(nameof(middleware));    
	        }
	        this.middleware = middleware;    
	    }
	    public int SequenceId { get; }
	    public Task InvokeAsync(ITransportContext context, TransportMiddlewareHandler next)
	    {
	        return middleware.Invoke(context, next);   
	    }
	}
	#endregion
	#region \Internal\Extensions
	internal static class TaskToApm
	{
	    public static IAsyncResult Begin(Task task, AsyncCallback? callback, object? state) => new TaskAsyncResult(task, state, callback);
	    public static void End(IAsyncResult asyncResult)
	    {
	        if (asyncResult is TaskAsyncResult twar)
	        {
	            twar.task.GetAwaiter().GetResult();
	            return;
	        }
	        ArgumentNullException.ThrowIfNull(asyncResult, nameof(asyncResult));
	    }
	    public static TResult End<TResult>(IAsyncResult asyncResult)
	    {
	        if (asyncResult is TaskAsyncResult twar && twar.task is Task<TResult> task)
	        {
	            return task.GetAwaiter().GetResult();
	        }
	        throw new ArgumentNullException(nameof(asyncResult));
	    }
	    internal sealed class TaskAsyncResult : IAsyncResult
	    {
	        internal readonly Task task;
	        private readonly AsyncCallback? callback;
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
	        private void InvokeCallback()
	        {
	            Debug.Assert(!CompletedSynchronously);
	            Debug.Assert(callback != null);
	            callback.Invoke(this);
	        }
	        public object? AsyncState { get; }
	        public bool CompletedSynchronously { get; }
	        public bool IsCompleted => task.IsCompleted;
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
	#endregion
	#region \Internal\Helpers
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
	        // Calling Dispose after ReadAsync can cause an "InvalidArgument" error on *nix.
	        return errorCode == SocketError.OperationAborted ||
	               errorCode == SocketError.Interrupted ||
	               (errorCode == SocketError.InvalidArgument && !OperatingSystem.IsWindows());
	    }
	}
	#endregion
	#region \Internal\MemoryPool
	internal class PipelineMemoryPool : MemoryPool<byte>
	{
	    private readonly ConcurrentQueue<PipelineMemoryPoolBlock> blocks = new();
	    private bool isDisposed; // To detect redundant calls
	    private readonly object disposeSync = new object();
	    public override int MaxBufferSize => BlockSize;
	    public static int BlockSize => 4096;
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
	    public PipelineMemoryPool Pool { get; }
	    public Memory<byte> Memory { get; }
	    public void Dispose()
	    {
	        Pool.Return(this);
	    }
	}
	#endregion
	#region \Internal\Quic
	internal class QuicTransportConnection : ITransportConnection
	{
	    public QuicTransportConnection()
    {
        
    }
	    public bool IsConnected => throw new NotImplementedException();
	    public object? ConnectionData => throw new NotImplementedException();
	    public ConnectionState State => throw new NotImplementedException();
	    public ITransportConnectionPipe Pipe => throw new NotImplementedException();
	    public EndPoint LocalEndPoint => throw new NotImplementedException();
	    public EndPoint RemoteEndPoint => throw new NotImplementedException();
	    public void Execute()
	    {
	        throw new NotImplementedException();
	    }
	    public void Abort()
	    {
	        throw new NotImplementedException();
	    }
	    public ValueTask AbortAsync()
	    {
	        throw new NotImplementedException();
	    }
	    public void Dispose()
	    {
	        throw new NotImplementedException();
	    }
	}
	#endregion
	#region \Internal\Sockets
	internal enum SocketTraceCode
	{
	    Paused,
	    Error,
	    Reset,
	    Resumed,
	    Finished
	}
	internal class SocketTransportConnection : ITransportConnection
	{
	    private readonly CancellationTokenSource connectionClosedTokenSource = new CancellationTokenSource();
	    private readonly TaskCompletionSource connectionClosingProcess = new TaskCompletionSource();
	    private readonly TransportTraceHandler trace;
	    private volatile bool isSocketDisposed;
	    private bool isConnectionClosed;
	    private readonly object @lock = new object();
	    private volatile ConnectionState state;
	    public SocketTransportConnection(SocketTransportConnectionSettings? settings)
	    {
	        if (settings is null)
	        {
	            throw new ArgumentNullException(nameof(settings));
	        }
	        var serverPipe = new Pipe(settings.InputOptions);
	        var clientPipe = new Pipe(settings.OutputOptions);
	        if (settings.IsServer)
	        {
	            this.Pipe = new TransportConnectionPipe(
	                serverPipe.Reader,
	                clientPipe.Writer);
	            this.Output = serverPipe.Writer;
	            this.Input = clientPipe.Reader;
	        }
	        else
	        {
	            this.Pipe = new TransportConnectionPipe(
	                clientPipe.Reader,
	                serverPipe.Writer);
	            this.Input = serverPipe.Reader;
	            this.Output = clientPipe.Writer;
	        }
	        this.trace = settings.OnTrace;
	        this.Socket = settings.Socket;
	        this.LocalEndPoint = settings?.Socket.LocalEndPoint!;
	        this.RemoteEndPoint = settings?.Socket.RemoteEndPoint!;
	        this.SenderPool = new SocketPipeSenderPool(settings!.SenderScheduler);
	        this.Receiver = new SocketPipeReceiver(settings.ReceiverScheduler);
	    }
	    public bool IsConnected => RemoteEndPoint is not null;
	    public object? ConnectionData { get; set; }
	    public ConnectionState State => this.state;
	    public ITransportConnectionPipe Pipe { get; set; }
	    public EndPoint LocalEndPoint { get; set; }
	    public EndPoint RemoteEndPoint { get; set; }
	    public Action OnDispose { get; set; } = default!;
	    public readonly PipeWriter Output;
	    public readonly PipeReader Input;
	    public readonly SocketPipeReceiver Receiver;
	    public readonly SocketPipeSenderPool SenderPool;
	    public readonly Socket Socket;
	    public Exception? ConnectionError;
	    public void Abort()
	    {
	        AbortAsync().GetAwaiter().GetResult();
	    }
	    public ValueTask AbortAsync()
	    {
	        lock (@lock)
	        {
	            if (isSocketDisposed)
	            {
	                return ValueTask.CompletedTask;
	            }
	            // Make sure to close the connection only after the _aborted flag is set.
	            // Without this, the RequestsCanBeAbortedMidRead test will sometimes fail when
	            // a BadHttpRequestException is thrown instead of a TaskCanceledException.
	            // ConnectionError should only be null if the output was completed gracefully, so no one should ever
	            // ever observe the nondescript ConnectionAbortedException except for connection middleware attempting
	            // to half close the connection which is currently unsupported.
	            // _shutdownReason = ConnectionError ?? new ConnectionAbortedException("The Socket transport's send loop completed gracefully.");
	            // _trace.ConnectionWriteFin(this, _shutdownReason.Message);
	            try
	            {
	                // Try to gracefully close the socket even for aborts to match libuv behavior.
	                Socket.Shutdown(SocketShutdown.Both);
	            }
	            catch
	            {
	                // Ignore any errors from Socket.Shutdown() since we're tearing down the connection anyway.
	            }
	            Socket.Dispose();
	            state = ConnectionState.Aborted;
	        }
	        return ValueTask.CompletedTask;
	    }
	    public void Dispose()
	    {
	        if (!isSocketDisposed)
	        {
	            throw new ObjectDisposedException(nameof(SocketTransportConnection));
	        }
	        Abort();
	        isSocketDisposed = true;
	        OnDispose?.Invoke();
	    }
	    public void Execute()
	    {
	        if (state != ConnectionState.Running)
	        {
	            _ = Receive();
	            _ = Send();
	            state = ConnectionState.Running;
	        }
	    }
	    public async Task Receive()
	    {
	        var error = default(Exception);
	        try
	        {
	            while (true)
	            {
	                // Ensure we have some reasonable amount of buffer space
	                var buffer = Output.GetMemory(PipelineMemoryPool.BlockSize / 2);
	                var result = await Receiver.ReceiveAsync(Socket, buffer);
	                if (result.BytesTransferred == 0)
	                {
	                    // FIN
	                    trace?.Invoke(SocketTraceCode.Finished, ConnectionData, "The remote host has finished sending data.");
	                    break;
	                }
	                Output.Advance(result.BytesTransferred);
	                var flushResultTask = Output.FlushAsync();
	                var flushResultTaskPaused = !flushResultTask.IsCompleted;
	                if (flushResultTask.IsCompleted)
	                {
	                    // TODO: Add 'Connection Paused' Trace
	                    trace?.Invoke(SocketTraceCode.Paused, ConnectionData, "The connection has been paused receiving data.");
	                }
	                var flushResult = await flushResultTask;
	                if (flushResultTaskPaused)
	                {
	                    trace?.Invoke(SocketTraceCode.Resumed, ConnectionData, "The connection has resumed receiving data.");
	                }
	                if (flushResult.IsCompleted || flushResult.IsCanceled)
	                {
	                    // ClientPipe consumer is shut down, do we stop writing
	                    break;
	                }
	            }
	        }
	        catch (SocketException exception) when (SocketHelper.IsConnectionResetError(exception.SocketErrorCode))
	        {
	            // This could be ignored if ConnectionError is already set.
	            error ??= exception;
	            // There's still a small chance that both DoReceive() and DoSend() can log the same connection reset.
	            // Both logs will have the same ConnectionId. I don't think it's worthwhile to lock just to avoid this.
	            if (!isSocketDisposed)
	            {
	                trace?.Invoke(SocketTraceCode.Reset, ConnectionData, exception.Message);
	            }
	        }
	        catch (Exception exception)
	        {
	            // This exception should always be ignored because ConnectionError should be set.
	            error = exception;
	            if ((exception is SocketException socketException && SocketHelper.IsConnectionAbortError(socketException.SocketErrorCode)) || exception is ObjectDisposedException)
	            {
	                if (!isSocketDisposed)
	                {
	                    // This is unexpected if the Socket hasn't been disposed yet.
	                    trace?.Invoke(SocketTraceCode.Error, ConnectionData, exception.Message);
	                }
	            }
	            else
	            {
	                trace?.Invoke(ConnectionData, SocketTraceCode.Error, $"A connection error occurred while receiving data: {exception.Message}");
	            }
	        }
	        finally
	        {
	            // If Shutdown() has already bee called, assume that was the reason ProcessReceives() exited.
	            Output.Complete(ConnectionError ?? error);
	            // Guard against scheduling this multiple times
	            if (!isConnectionClosed)
	            {
	                isConnectionClosed = true;
	                ThreadPool.UnsafeQueueUserWorkItem(state =>
	                {
	                    state.CancelConnectionClosedToken();
	                    state.connectionClosingProcess.TrySetResult();
	                }, this, preferLocal: false);
	                await connectionClosingProcess.Task;
	                Abort();
	            }
	        }
	    }
	    public async Task Send()
	    {
	        var error = default(Exception);
	        try
	        {
	            while (true)
	            {
	                var result = await Input.ReadAsync();
	                if (result.IsCanceled)
	                {
	                    break;
	                }
	                var buffer = result.Buffer;
	                if (!buffer.IsEmpty)
	                {
	                    var sender = SenderPool.Rent();
	                    sender.RemoteEndPoint ??= RemoteEndPoint;
	                    var transferResult = default(SocketPipeResult);
	                    switch (Socket.SocketType)
	                    {
	                        case SocketType.Stream: // Streams represent connection oriented sockets
	                            transferResult = await sender.SendAsync(Socket, buffer);
	                            break;
	                        case SocketType.Dgram: // Dgrams represent connectionless sockets
	                            transferResult = await sender.SendToAsync(Socket, buffer);
	                            break;
	                        default:
	                            throw new NotSupportedException();
	                    }
	                    if (transferResult.HasError)
	                    {
	                        if (SocketHelper.IsConnectionResetError(transferResult.Error.SocketErrorCode))
	                        {
	                            error = transferResult.Error;
	                            break;
	                        }
	                        if (SocketHelper.IsConnectionAbortError(transferResult.Error.SocketErrorCode))
	                        {
	                            error = transferResult.Error;
	                            break;
	                        }
	                        error = transferResult.Error;
	                    }
	                    // We don't return to the pool if there was an exception, and
	                    // we keep the sender assigned so that we can dispose it in StartAsync.
	                    SenderPool.Return(sender);
	                }
	                Input.AdvanceTo(buffer.End);
	                if (result.IsCompleted)
	                {
	                    break;
	                }
	            }
	        }
	        catch (SocketException exception)
	        when (SocketHelper.IsConnectionResetError(exception.SocketErrorCode))
	        {
	            error = new SocketConnectionResetException(exception.Message, exception);
	            trace?.Invoke(SocketTraceCode.Reset, ConnectionData, $"The connection was reset for the following reason: {error.Message}");
	        }
	        catch (Exception exception)
	        when ((exception is SocketException socketEx && SocketHelper.IsConnectionAbortError(socketEx.SocketErrorCode)) || exception is ObjectDisposedException)
	        {
	            // This should always be ignored since Shutdown() must have already been called by Abort().
	            error = exception;
	        }
	        catch (Exception exception)
	        {
	            error = exception;
	            trace?.Invoke(SocketTraceCode.Error, ConnectionData, $"A connection error occurred while sending data: {exception.Message}");
	        }
	        finally
	        {
	            Abort();
	            // Complete the output after disposing the socket
	            Input.Complete(error);
	            // Cancel any pending flushes so that the input loop is un-paused
	            Output.CancelPendingFlush();
	            ConnectionError = error;
	        }
	    }
	    private void CancelConnectionClosedToken()
	    {
	        try
	        {
	            connectionClosedTokenSource.Cancel();
	        }
	        catch (Exception)
	        {
	            //_trace.LogError(0, exception, $"Unexpected exception in {nameof(SocketConnection)}.{nameof(CancelConnectionClosedToken)}.");
	        }
	    }
	}
	internal sealed class SocketTransportConnectionSettings
	{
	    public bool IsServer { get; set; }
	    public EndPoint EndPoint { get; init; } = default!;
	    public Socket Socket { get; set; } = default!;
	    public PipeOptions InputOptions { get; init; } = default!;
	    public PipeOptions OutputOptions { get; init; } = default!;
	    public PipeScheduler ReceiverScheduler { get; init; } = default!;
	    public PipeScheduler SenderScheduler { get; init; } = default!;
	    public bool WaitForDataBeforeAllocatingBuffer { get; set; }
	    public TransportTraceHandler OnTrace { get; set; } = default!;
	    public static SocketTransportConnectionSettings[] GetIOQueueSettings(
	        int count,
	        bool unsafePreferInLineScheduling = false,
	        bool waitForDataBeforeAllocatingBuffer = false,
	        long? maxReadBufferSize = 0,
	        long? maxWriteBufferSize = 0,
	        TransportTraceHandler onTrace = default!)
	    {
	        var options = new SocketTransportConnectionSettings[count];
	        var memoryPool = PipelineMemoryPool.Create();
	        var applicationScheduler = unsafePreferInLineScheduling ?
	            PipeScheduler.Inline :
	            PipeScheduler.ThreadPool;
	        var transportScheduler = unsafePreferInLineScheduling ?
	            PipeScheduler.Inline :
	            new SocketPipeScheduler();
	        var awaiterScheduler = OperatingSystem.IsWindows() ?
	            transportScheduler :
	            PipeScheduler.Inline;
	        for (var i = 0; i < count; i++)
	        {
	            options[i] = new SocketTransportConnectionSettings()
	            {
	                IsServer = true,
	                ReceiverScheduler = transportScheduler,
	                SenderScheduler = awaiterScheduler,
	                InputOptions = new PipeOptions(
	                    memoryPool,
	                    applicationScheduler,
	                    transportScheduler,
	                    maxReadBufferSize ?? 0,
	                    maxReadBufferSize ?? 0 / 2,
	                    useSynchronizationContext: false),
	                OutputOptions = new PipeOptions(
	                    memoryPool,
	                    transportScheduler,
	                    applicationScheduler,
	                    maxWriteBufferSize ?? 0,
	                    maxWriteBufferSize ?? 0 / 2,
	                    useSynchronizationContext: false),
	                WaitForDataBeforeAllocatingBuffer = waitForDataBeforeAllocatingBuffer,
	                OnTrace = onTrace
	            };
	        }
	        return options;
	    }
	}
	#endregion
	#region \Internal\Sockets\AsyncEvents
	internal class SocketPipeAsyncArgs : SocketAsyncEventArgs, IValueTaskSource<SocketPipeResult>
	{
	    private static readonly Action<object?> continuationCompleted = _ => { };
	    private Action<object?>? continuation;
	    private readonly PipeScheduler pipeScheduler;
	    public SocketPipeAsyncArgs(PipeScheduler? pipeScheduler) : base(unsafeSuppressExecutionContextFlow: true)
	    {
	        if (pipeScheduler is null)
	        {
	            throw new ArgumentNullException(nameof(pipeScheduler));
	        }
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
	internal readonly struct SocketPipeResult
	{
	    public readonly SocketException Error = null!;
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
	        // Calling Dispose after ReadAsync can cause an "InvalidArgument" error on *nix.
	        return errorCode == SocketError.OperationAborted ||
	               errorCode == SocketError.Interrupted ||
	               (errorCode == SocketError.InvalidArgument && !OperatingSystem.IsWindows());
	    }
	}
	internal class SocketPipeScheduler : PipeScheduler, IThreadPoolWorkItem
	{
	    private readonly ConcurrentQueue<Work> queue;
	    private int active;
	    public SocketPipeScheduler()
	    {
	        this.queue = new ConcurrentQueue<Work>();
	    }
	    public override void Schedule(Action<object>? action, object? state)
	    {
	        queue.Enqueue(new Work(action!, state!));
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
	internal class SocketPipeSender : SocketPipeAsyncArgs
	{
	    private List<ArraySegment<byte>> bufferList = default!;
	    public SocketPipeSender(PipeScheduler pipeScheduler) : base(pipeScheduler)
	    {
	    }
	    public ValueTask<SocketPipeResult> SendToAsync(Socket socket, in ReadOnlySequence<byte> buffers)
	    {
	        if (buffers.IsSingleSegment)
	        {
	            return SendToAsync(socket, buffers.First);
	        }
	        SetBufferList(buffers);
	        if (socket.SendToAsync(this))
	        {
	            return new ValueTask<SocketPipeResult>(this, 0);
	        }
	        var bytesTransferred = BytesTransferred;
	        var error = SocketError;
	        return error == SocketError.Success
	            ? new ValueTask<SocketPipeResult>(new SocketPipeResult(bytesTransferred))
	            : new ValueTask<SocketPipeResult>(new SocketPipeResult(CreateException(error)));
	    }
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
	            bufferList?.Clear();
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
	    private ValueTask<SocketPipeResult> SendToAsync(Socket socket, ReadOnlyMemory<byte> memory)
	    {
	        SetBuffer(MemoryMarshal.AsMemory(memory));
	        if (socket.SendToAsync(this))
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
	        bufferList ??= new List<ArraySegment<byte>>();
	        foreach (var b in buffer)
	        {
	            bufferList.Add(GetArray(b));
	        }
	        // The act of setting this list, sets the buffers in the internal buffer list
	        BufferList = bufferList;
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
	    public IEnumerable<SocketPipeSender> Rent(int count)
	    {
	        for (int i = 0 ; i < count; i++)
	        {
	            if (_queue.TryDequeue(out var sender))
	            {
	                Interlocked.Decrement(ref _count);
	                yield return sender;
	            }
	            yield return new SocketPipeSender(_scheduler);
	        }
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
	#endregion
	#region \Internal\Sockets\Exceptions
	internal class SocketConnectionAbortedException
	{
	}
	internal class SocketConnectionResetException : TransportException
	{
	    public SocketConnectionResetException(string message) 
	        : base(message) { }
	    public SocketConnectionResetException(string message, Exception inner) 
	        : base(message, inner) { }
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
	#region \Transports
	public static class Transport
	{
	    public static ITransport CreateTcpClient(Action<TcpClientTransportOptions> configure) =>
	        TcpClientTransport.Create(configure);
	    public static ITransport CreateTcpServer(Action<TcpServerTransportOptions> configure) =>
	        TcpServerTransport.Create(configure);
	    public static ITransport CreateUdpClient(Action<UdpClientTransportOptions> configure) => 
	        UdpClientTransport.Create(configure);
	    public static ITransport CreateUdpServer(Action<UdpServerTransportOptions> configure) => 
	        UdpServerTransport.Create(configure);
	}
	#endregion
	#region \Transports\Quic
	[RequiresPreviewFeatures]
	[SupportedOSPlatform("windows")]
	[SupportedOSPlatform("linux")]
	[SupportedOSPlatform("macos")]
	[SupportedOSPlatform("osx")]
	public sealed class QuicClientTransport : ClientTransport
	{
	    public QuicClientTransport()
    {
            
    }
	    public override ProtocolType ProtocolType => throw new NotImplementedException();
	    public override TransportMiddlewareHandler Middleware => throw new NotImplementedException();
	    public override async Task<ITransportConnection> ConnectAsync(CancellationToken cancellationToken = default)
	    {
	        while (true)
	        {
	            try
	            {
	                // Client implementation: QuicConnection.ConnectAsync()
	                var connection = await QuicConnection.ConnectAsync(new()
	                {
	                }, cancellationToken);
	                var inboundStream = await connection.OpenOutboundStreamAsync(QuicStreamType.Bidirectional, cancellationToken);
	            }
	            catch (Exception exception)
	            {
	            }
	        }
	        throw new NotImplementedException();
	    }
	    public override void Dispose()
	    {
	        throw new NotImplementedException();
	    }
	}
	public sealed class QuicClientTransportContext : ITransportContext
	{
	    internal QuicClientTransportContext(QuicTransportConnection connection)
    {
        Connection = connection;
    }
	    public ITransportConnection Connection { get; }
	}
	public abstract class QuicClientTransportMiddleware : ITransportMiddleware
	{
	    public abstract Task InvokeAsync(QuicClientTransportContext context, TransportMiddlewareHandler next);
	    Task ITransportMiddleware.InvokeAsync(ITransportContext context, TransportMiddlewareHandler next)
	    {
	        return context is QuicClientTransportContext clientContext ?
	            InvokeAsync(clientContext, next) :
	            Task.CompletedTask;
	    }
	}
	public sealed class QuicClientTransportOptions
	{
	}
	[RequiresPreviewFeatures]
	[SupportedOSPlatform("windows")]
	[SupportedOSPlatform("linux")]
	[SupportedOSPlatform("macos")]
	[SupportedOSPlatform("osx")]
	public sealed class QuicServerTransport : ServerTransport, IAsyncDisposable
	{
	    private QuicListener listener;
	    private readonly QuicServerTransportOptions options;
	    private readonly List<ITransportConnection> connections = new();
	    public QuicServerTransport(QuicServerTransportOptions options)
    {
	        if (options is null)
	        {
	            throw new ArgumentNullException(nameof(options));
	        }
        this.options = options;
	        Middleware = options.Middleware;
    }
	    public IReadOnlyCollection<ITransportConnection> Connections => connections.AsReadOnly();
	    public override ProtocolType ProtocolType => ProtocolType.Quic;
	    public override TransportMiddlewareHandler Middleware { get; }
	    public override async Task<ITransportConnection> AcceptOrListenAsync(CancellationToken cancellationToken = default)
	    {
	        listener = await QuicListener.ListenAsync(new()
	        {
	            ListenEndPoint = options.EndPoint,
	            ListenBacklog = options.Backlog,
	            ConnectionOptionsCallback = (connection, info, cancellationToken) => ValueTask.FromResult(new QuicServerConnectionOptions()
	            {
	                ServerAuthenticationOptions = new()
	                {
	                    AllowRenegotiation = true,
	                    EnabledSslProtocols = SslProtocols.Tls13,
	                    ApplicationProtocols = new()
	                        {
	                            SslApplicationProtocol.Http3
	                        }
	                },
	                IdleTimeout = Timeout.InfiniteTimeSpan, // Kestrel manages connection lifetimes itself so it can send GoAway's.
	                MaxInboundBidirectionalStreams = 100,
	                MaxInboundUnidirectionalStreams = 10,
	                DefaultCloseErrorCode = 0,
	                DefaultStreamErrorCode = 0,
	            }),
	            ApplicationProtocols = new()
	            {
	                SslApplicationProtocol.Http3
	            }
	        }, cancellationToken);
	        while (true)
	        {
	            try
	            {
	                var connection = await listener.AcceptConnectionAsync(cancellationToken);
	                var stream = connection.AcceptInboundStreamAsync();
	                return new QuicTransportConnection();
	            }
	            catch (Exception exception)
	            {
	                continue;
	            }
	        }
	    }
	    public override void Dispose()
	    {
	        (this as IAsyncDisposable).DisposeAsync().GetAwaiter().GetResult();
	    }
	    ValueTask IAsyncDisposable.DisposeAsync()
	    {
	        return listener.DisposeAsync();
	    }
	    public static QuicServerTransport Create(Action<QuicServerTransportOptions> configure)
	    {
	        if (configure is null)
	        {
	            throw new ArgumentNullException(nameof(configure));
	        }
	        var options = new QuicServerTransportOptions();
	        configure.Invoke(options);
	        return new QuicServerTransport(options);
	    }
	}
	public sealed class QuicServerTransportContext : ITransportContext
	{
	    internal QuicServerTransportContext(QuicTransportConnection connection)
    {
        Connection = connection;
    }
	    public ITransportConnection Connection { get; }
	}
	public abstract class QuicServerTransportMiddleware : ITransportMiddleware
	{
	    public abstract Task InvokeAsync(QuicServerTransportContext context, TransportMiddlewareHandler next);
	    Task ITransportMiddleware.InvokeAsync(ITransportContext context, TransportMiddlewareHandler next)
	    {
	        throw new NotImplementedException();
	    }
	}
	public sealed class QuicServerTransportOptions
	{
	    private TransportTraceHandler onTrace = (code, data, message) => { };
	    private TransportMiddlewareHandler middleware = context => Task.CompletedTask;
	    public IPEndPoint EndPoint { get; set; } = new IPEndPoint(IPAddress.Loopback, 8080);
		public int Backlog { get; set; } = 512;
		public TransportMiddlewareHandler Middleware => this.middleware;
	    public TransportTraceHandler OnTrace => this.onTrace;
	    public void AddMiddleware(Action<TransportMiddlewareBuilder<QuicServerTransportContext, QuicServerTransportMiddleware>> configure)
	    {
	        if (configure is null)
	        {
	            throw new ArgumentNullException(nameof(configure));
	        }
	        var builder = new TransportMiddlewareBuilder<QuicServerTransportContext, QuicServerTransportMiddleware>();
	        configure.Invoke(builder);
	        middleware = builder.Build();
	    }
	}
	#endregion
	#region \Transports\Tcp
	public sealed class TcpClientTransport : ClientTransport
	{
	    private readonly TcpClientTransportOptions options;
	    private readonly SocketTransportConnectionSettings settings;
	    private Socket? socket;
	    private bool isDisposed;
	    private readonly List<ITransportConnection> connections = new();
	    public TcpClientTransport(TcpClientTransportOptions? options)
	    {
	        if (options is null)
	        {
	            throw new ArgumentNullException(nameof(options));
	        }
	        this.options = options;
	        this.settings = SocketTransportConnectionSettings.GetIOQueueSettings(
	            1,
	            options.UnsafePreferInLineScheduling,
	            options.WaitForDataBeforeAllocatingBuffer,
	            options.MaxReadBufferSize,
	            options.MaxWriteBufferSize,
	            options.OnTrace)[0];
	        this.Middleware = options.Middleware;
	    }
	    public override ProtocolType ProtocolType => ProtocolType.Tcp;
	    public override TransportMiddlewareHandler Middleware { get; }
	    public override async Task<ITransportConnection> ConnectAsync(CancellationToken cancellationToken = default)
	    {
	        if (socket is null)
	        {
	            socket = options.EndPoint switch
	            {
	                UnixDomainSocketEndPoint        => new Socket(options.EndPoint.AddressFamily, SocketType.Stream, System.Net.Sockets.ProtocolType.Unspecified),
	                /* 
	                    We're passing "ownsHandle: true" here even though we don't necessarily
	                    own the handle because Socket.Dispose will clean-up everything safely.
	                    If the handle was already closed or disposed then the socket will
	                    be torn down gracefully, and if the caller never cleans up their handle
	                    then we'll do it for them.
	                    If we don't do this then we run the risk of Kestrel hanging because the
	                    the underlying socket is never closed and the transport manager can hang
	                    when it attempts to stop.
	                */
	                FileHandleEndPoint fileHandle   => new Socket(new SafeSocketHandle((IntPtr)fileHandle.FileHandle, ownsHandle: true)),
	                _                               => new Socket(options.EndPoint.AddressFamily, SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp)
	            };
	            if (options.EndPoint is IPEndPoint ip && ip.Address == IPAddress.IPv6Any)
	            {
	                socket.DualMode = true;
	            }
	            if (socket.LocalEndPoint is IPEndPoint)
	            {
	                socket.NoDelay = options.NoDelay;
	            }
	        }
	        if (!socket.Connected)
	        {
	            await socket.ConnectAsync(options.EndPoint);
	            settings.Socket = socket;
	        }
	        while (true)
	        {
	            try
	            {
	                var connection = new SocketTransportConnection(settings);
	                var started = !ThreadPool.UnsafeQueueUserWorkItem(connection, true);
	                connections.Add(connection);
	                connection.OnDispose = () =>
	                {
	                    connections.Remove(connection);
	                };
	                await Middleware.Invoke(new TcpClientTransportContext(connection));
	                return connection;
	            }
	            catch (Exception)
	            {
	                socket.Dispose();
	                isDisposed = true;
	            }
	        }
	    }
	    public override void Dispose()
	    {
	        if (socket is not null && socket.Connected)
	        {
	            socket.Close();
	        }
	        if (socket is not null && !isDisposed)
	        {
	            socket.Dispose();
	        }
	    }
	    public static TcpClientTransport Create(Action<TcpClientTransportOptions> configure)
	    {
	        if (configure is null)
	        {
	            throw new ArgumentNullException(nameof(configure));
	        }
	        var options = new TcpClientTransportOptions();
	        configure.Invoke(options);
	        return new TcpClientTransport(options);
	    }
	}
	public sealed class TcpClientTransportContext : ITransportContext
	{
	    private readonly SocketTransportConnection connection;
	    internal TcpClientTransportContext(SocketTransportConnection connection)
	    {
	        this.connection = connection;
	    }
	    public ITransportConnection Connection => this.connection;
	    public void SetPipe(ITransportConnectionPipe pipe)
	    {
	        if (pipe is null)
	        {
	            throw new ArgumentNullException(nameof(pipe));
	        }
	        connection.Pipe = pipe;
	    }
	    public void SetConnectionData(object? data)
	    {
	        this.connection.ConnectionData = data;
	    }
	}
	public abstract class TcpClientTransportMiddleware : ITransportMiddleware
	{
	    public abstract Task InvokeAsync(TcpClientTransportContext context, TransportMiddlewareHandler next);
	    Task ITransportMiddleware.InvokeAsync(ITransportContext context, TransportMiddlewareHandler next)
	    {
	        return context is TcpClientTransportContext clientContext ?
	            InvokeAsync(clientContext, next) :
	            Task.CompletedTask;
	    }
	}
	public sealed class TcpClientTransportOptions
	{
		private TransportTraceHandler onTrace = (code, data, message) => { };
		private TransportMiddlewareHandler middleware = context => Task.CompletedTask;
		public EndPoint EndPoint { get; set; } = new IPEndPoint(IPAddress.Loopback, 8081);
		public bool WaitForDataBeforeAllocatingBuffer { get; set; } = true;
		public bool NoDelay { get; set; } = true;
		public long? MaxReadBufferSize { get; set; } = 1024 * 1024;
		public long? MaxWriteBufferSize { get; set; } = 64 * 1024;
		public bool UnsafePreferInLineScheduling { get; set; }
		public bool WaitOnPacketIngestion { get; set; } = true;
		public TransportMiddlewareHandler Middleware => this.middleware;
		public TransportTraceHandler OnTrace => this.onTrace;
		public void AddTraceHandler(TransportTraceHandler onTrace)
		{
			if (onTrace is null)
			{
				throw new ArgumentNullException(nameof(onTrace));
			}
			this.onTrace = onTrace;
		}
		public void AddTraceHandler<TConnectionData>(Action<TcpConnectionTraceCode, TConnectionData, string?> onTrace)
		{
			if (onTrace is null)
			{
				throw new ArgumentNullException(nameof(onTrace));
			}
			this.onTrace = (data, code, message) =>
			{
				if (data is TConnectionData connectionData && code is not null)
				{
					onTrace.Invoke((TcpConnectionTraceCode)code, connectionData, message);
				}
			};
		}
		public void AddMiddleware(Action<TransportMiddlewareBuilder<TcpClientTransportContext, TcpClientTransportMiddleware>> configure)
		{
			if (configure is null)
			{
				throw new ArgumentNullException(nameof(configure));
			}
			var builder = new TransportMiddlewareBuilder<TcpClientTransportContext, TcpClientTransportMiddleware>();
			configure.Invoke(builder);
			middleware = builder.Build();
		}
	}
	public enum TcpConnectionTraceCode
	{
	    Paused = SocketTraceCode.Paused,
	    Reset = SocketTraceCode.Reset,
	    Error = SocketTraceCode.Error,
	    Resumed = SocketTraceCode.Resumed,
	    Finished = SocketTraceCode.Finished
	}
	public sealed class TcpServerTransport : ServerTransport
	{
	    private readonly TcpServerTransportOptions options;
	    private readonly SocketTransportConnectionSettings[] settings;
	    private readonly int count;
	    private long index; // long to prevent overflow
	    private Socket? listener;
	    private readonly List<ITransportConnection> connections = new();
	    public TcpServerTransport() : this(new()) { }
	    public TcpServerTransport(TcpServerTransportOptions options)
	    {
	        if (options is null)
	        {
	            throw new ArgumentNullException(nameof(options));
	        }
	        this.options = options;
	        this.count = options.IOQueueCount > 0 ? options.IOQueueCount : 1;
	        this.settings = SocketTransportConnectionSettings.GetIOQueueSettings(
	            count,
	            options.UnsafePreferInLineScheduling,
	            options.WaitForDataBeforeAllocatingBuffer,
	            options.MaxReadBufferSize,
	            options.MaxWriteBufferSize,
	            options.OnTrace);
	        this.Middleware = options.Middleware;
	    }
	    public IReadOnlyCollection<ITransportConnection> Connections => this.connections.AsReadOnly();
	    public override ProtocolType ProtocolType => ProtocolType.Tcp;
	    public override TransportMiddlewareHandler Middleware { get; }
	    public override async Task<ITransportConnection> AcceptOrListenAsync(CancellationToken cancellationToken = default)
	    {
	        if (listener is null)
	        {
	            listener = options.EndPoint switch
	            {
	                UnixDomainSocketEndPoint        => new Socket(options.EndPoint.AddressFamily, SocketType.Stream, System.Net.Sockets.ProtocolType.Unspecified),
	                /* 
	                    We're passing "ownsHandle: true" here even though we don't necessarily
	                    own the handle because Socket.Dispose will clean-up everything safely.
	                    If the handle was already closed or disposed then the socket will
	                    be torn down gracefully, and if the caller never cleans up their handle
	                    then we'll do it for them.
	                    If we don't do this then we run the risk of Kestrel hanging because the
	                    the underlying socket is never closed and the transport manager can hang
	                    when it attempts to stop.
	                */
	                FileHandleEndPoint fileHandle   => new Socket(new SafeSocketHandle((IntPtr)fileHandle.FileHandle, ownsHandle: true)),
	                _                               => new Socket(options.EndPoint.AddressFamily, SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp)
	            };
	            if (options.EndPoint is IPEndPoint ip && ip.Address == IPAddress.IPv6Any)
	            {
	                listener.DualMode = true;
	            }
	            listener.Bind(options.EndPoint);
	            listener.Listen(options.Backlog);
	        }
	        while (true)
	        {
	            try
	            {
	                var socket = await listener.AcceptAsync(cancellationToken);
	                var settings = this.settings[Interlocked.Increment(ref index) % count];
	                if (socket.LocalEndPoint is IPEndPoint)
	                {
	                    socket.NoDelay = options.NoDelay;
	                }
	                settings.Socket = socket;
	                var connection =  new SocketTransportConnection(settings);
	                if (!ThreadPool.UnsafeQueueUserWorkItem(connection, false))
	                {
	                    throw new Exception();
	                }
	                connections.Add(connection);
	                connection.OnDispose = () =>
	                {
	                    connections.Remove(connection);
	                };
	                await Middleware.Invoke(new TcpServerTransportContext(connection));
	                return connection;
	            }
	            catch (ObjectDisposedException)
	            {
	                // return null;
	                continue;
	            }
	            catch (SocketException exception) when (exception.SocketErrorCode == SocketError.OperationAborted)
	            {
	                // return null;
	                continue; // A call was made to UnbindAsync/DisposeAsync just return null which signals we're done
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
	        listener?.Close();
	        listener?.Dispose();
	    }
	    public static TcpServerTransport Create(Action<TcpServerTransportOptions> configure)
	    {
	        if (configure is null)
	        {
	            throw new ArgumentNullException(nameof(configure));
	        }
	        var options = new TcpServerTransportOptions();
	        configure.Invoke(options);
	        return new TcpServerTransport(options);
	    }
	}
	public sealed class TcpServerTransportContext : ITransportContext
	{
	    private readonly SocketTransportConnection connection;
	    internal TcpServerTransportContext(SocketTransportConnection connection)
	    {
	        this.connection = connection;
	    }
	    public ITransportConnection Connection => this.connection;
	    public void SetPipe(ITransportConnectionPipe pipe)
	    {
	        if (pipe is null)
	        {
	            throw new ArgumentNullException(nameof(pipe));
	        }
	        connection.Pipe = pipe;
	    }
	    public void SetConnectionData(object? data)
	    {
	        this.connection.ConnectionData = data;
	    }
	}
	public abstract class TcpServerTransportMiddleware : ITransportMiddleware
	{
	    public abstract Task InvokeAsync(TcpServerTransportContext context, TransportMiddlewareHandler next);
	    Task ITransportMiddleware.InvokeAsync(ITransportContext context, TransportMiddlewareHandler next)
	    {
	        return context is TcpServerTransportContext tcpContext ? 
	            InvokeAsync(tcpContext, next) :
	            Task.CompletedTask;
	    }
	}
	public sealed class TcpServerTransportOptions
	{
		private TransportTraceHandler onTrace = (code, data, message) => { };
		private TransportMiddlewareHandler middleware = context => Task.CompletedTask;
		public EndPoint EndPoint { get; set; } = new IPEndPoint(IPAddress.Loopback, 8081);
		public int IOQueueCount { get; set; } = Math.Min(Environment.ProcessorCount, 16);
		public bool WaitForDataBeforeAllocatingBuffer { get; set; } = true;
		public bool NoDelay { get; set; } = true;
		public int Backlog { get; set; } = 512;
		public long? MaxReadBufferSize { get; set; } = 1024 * 1024;
		public long? MaxWriteBufferSize { get; set; } = 64 * 1024;
		public bool UnsafePreferInLineScheduling { get; set; }
		public bool WaitOnPacketIngestion { get; set; } = true;
		public TransportMiddlewareHandler Middleware => this.middleware;
		public TransportTraceHandler OnTrace => this.onTrace;
		public void AddTraceHandler(TransportTraceHandler onTrace)
		{
			if (onTrace is null)
			{
				throw new ArgumentNullException(nameof(onTrace));
			}
			this.onTrace = onTrace;
		}
		public void AddTraceHandler<TConnectionData>(Action<TcpConnectionTraceCode, TConnectionData, string?> onTrace)
		{
			if (onTrace is null)
			{
				throw new ArgumentNullException(nameof(onTrace));
			}
			this.onTrace = (data, code, message) =>
			{
				if (data is TConnectionData connectionData && code is not null)
				{
					onTrace.Invoke((TcpConnectionTraceCode)code, connectionData, message);
				}
			};
		}
		public void AddMiddleware(Action<TransportMiddlewareBuilder<TcpServerTransportContext, TcpServerTransportMiddleware>> configure)
		{
			if (configure is null)
			{
				throw new ArgumentNullException(nameof(configure));
			}
			var builder = new TransportMiddlewareBuilder<TcpServerTransportContext, TcpServerTransportMiddleware>();
			configure.Invoke(builder);
			middleware = builder.Build();
		}
	}
	#endregion
	#region \Transports\Udp
	public sealed class UdpClientTransport : ClientTransport
	{
	    private readonly UdpClientTransportOptions options;
	    private readonly SocketTransportConnectionSettings settings;
	    private Socket? socket;
	    private bool disposed;
	    private readonly List<ITransportConnection> connections = new();
	    public UdpClientTransport(UdpClientTransportOptions options)
	    {
	        if (options is null)
	        {
	            throw new ArgumentNullException(nameof(options));
	        }
	        this.options = options;
	        this.settings = SocketTransportConnectionSettings.GetIOQueueSettings(
	            1,
	            options.UnsafePreferInLineScheduling,
	            options.WaitForDataBeforeAllocatingBuffer,
	            options.MaxReadBufferSize,
	            options.MaxWriteBufferSize,
	            options.OnTrace)[0];
	        this.Middleware = options.Middleware;
	    }
	    public override ProtocolType ProtocolType => ProtocolType.Udp;
	    public override TransportMiddlewareHandler Middleware { get; }
	    public override async Task<ITransportConnection> ConnectAsync(CancellationToken cancellationToken = default)
	    {
	        if (disposed)
	        {
	            throw new ObjectDisposedException(nameof(UdpServerTransport));
	        }
	        if (socket is null)
	        {
	            socket = options.Endpoint switch
	            {
	                UnixDomainSocketEndPoint    => new Socket(options.Endpoint.AddressFamily, SocketType.Dgram, System.Net.Sockets.ProtocolType.Unspecified),
	                _                           => new Socket(options.Endpoint.AddressFamily, SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp)
	            };
	            if (options.Endpoint is IPEndPoint ip && ip.Address == IPAddress.IPv6Any)
	            {
	                socket.DualMode = true;
	            }
	        }
	        while (true)
	        {
	            try
	            {
	                var connection = new SocketTransportConnection(settings);
	                connections.Add(connection);
	                connection.OnDispose = () =>
	                {
	                    connections.Remove(connection);
	                };
	                var started = !ThreadPool.UnsafeQueueUserWorkItem(connection, true);
	                await Middleware.Invoke(new UdpClientTransportContext(connection));
	                return connection;
	            }
	            catch (Exception)
	            {
	            }
	        }
	    }
	    public override void Dispose()
	    {
	        if (disposed)
	        {
	            throw new ObjectDisposedException(nameof(UdpServerTransport));
	        }
	        if (socket is not null)
	        {
	            socket.Dispose();
	            disposed = true;
	        }
	    }
	    public static UdpClientTransport Create(Action<UdpClientTransportOptions> configure)
	    {
	        if (configure is null)
	        {
	            throw new ArgumentNullException(nameof(configure));
	        }
	        var options = new UdpClientTransportOptions();
	        configure.Invoke(options);
	        return new UdpClientTransport(options);
	    }
	}
	public sealed class UdpClientTransportContext : ITransportContext
	{
	    private readonly SocketTransportConnection connection;
	    internal UdpClientTransportContext(SocketTransportConnection connection)
	    {
	        this.connection = connection;
	    }
	    public ITransportConnection Connection => this.connection;
	    public void SetPipe(ITransportConnectionPipe pipe)
	    {
	        if (pipe is null)
	        {
	            throw new ArgumentNullException(nameof(pipe));
	        }
	        connection.Pipe = pipe;
	    }
	    public void SetConnectionData(object? data)
	    {
	        this.connection.ConnectionData = data;
	    }
	}
	public abstract class UdpClientTransportMiddleware : ITransportMiddleware
	{
	    public abstract Task InvokeAsync(UdpClientTransportContext context, TransportMiddlewareHandler next);
	    Task ITransportMiddleware.InvokeAsync(ITransportContext context, TransportMiddlewareHandler next)
	    {
	        if (context is UdpClientTransportContext udpContext)
	        {
	            return InvokeAsync(udpContext, next);
	        }
	        return Task.CompletedTask;
	    }
	}
	public sealed class UdpClientTransportOptions
	{
		private TransportTraceHandler onTrace = (code, data, message) => { };
		private TransportMiddlewareHandler middleware = context => Task.CompletedTask;
		public EndPoint Endpoint { get; set; } = new IPEndPoint(IPAddress.Loopback, 8081);
		public bool WaitForDataBeforeAllocatingBuffer { get; set; } = true;
		public long? MaxReadBufferSize { get; set; } = 1024 * 1024;
		public long? MaxWriteBufferSize { get; set; } = 64 * 1024;
		public bool UnsafePreferInLineScheduling { get; set; }
		public TransportMiddlewareHandler Middleware => this.middleware;
		public TransportTraceHandler OnTrace => this.onTrace;
		public void AddTraceHandler(TransportTraceHandler onTrace)
		{
			if (onTrace is null)
			{
				throw new ArgumentNullException(nameof(onTrace));
			}
			this.onTrace = onTrace;
		}
		public void AddTraceHandler<TConnectionData>(Action<UdpTraceCode, TConnectionData, string?> onTrace)
		{
			if (onTrace is null)
			{
				throw new ArgumentNullException(nameof(onTrace));
			}
			this.onTrace = (data, code, message) =>
			{
				if (data is TConnectionData connectionData && code is not null)
				{
					onTrace.Invoke((UdpTraceCode)code, connectionData, message);
				}
			};
		}
		public void AddMiddleware(Action<TransportMiddlewareBuilder<UdpClientTransportContext, UdpClientTransportMiddleware>> configure)
		{
			if (configure is null)
			{
				throw new ArgumentNullException(nameof(configure));
			}
			var builder = new TransportMiddlewareBuilder<UdpClientTransportContext, UdpClientTransportMiddleware>();
			configure.Invoke(builder);
			middleware = builder.Build();
		}
	}
	public sealed class UdpServerTransport : ServerTransport
	{
	    private readonly UdpServerTransportOptions options;
	    private readonly SocketTransportConnectionSettings[] settings;
	    private readonly int count;
	    private int index;
	    private Socket? socket;
	    private bool isDisposed;
	    private readonly List<ITransportConnection> connections = new();
	    public UdpServerTransport(UdpServerTransportOptions options)
	    {
	        if (options is null)
	        {
	            throw new ArgumentNullException(nameof(options));
	        }
	        this.options = options;
	        this.count = options.IOQueueCount > 0 ? options.IOQueueCount : 1;
	        this.settings = SocketTransportConnectionSettings.GetIOQueueSettings(
	            count,
	            options.UnsafePreferInLineScheduling,
	            options.WaitForDataBeforeAllocatingBuffer,
	            options.MaxReadBufferSize,
	            options.MaxWriteBufferSize,
	            options.OnTrace);
	        this.Middleware = options.Middleware;
	    }
	    public IReadOnlyCollection<ITransportConnection> Connections => this.connections;
	    public override ProtocolType ProtocolType => ProtocolType.Udp;
	    public override TransportMiddlewareHandler Middleware { get; }
	    public override async Task<ITransportConnection> AcceptOrListenAsync(CancellationToken cancellationToken = default)
	    {
	        if (isDisposed)
	        {
	            throw new ObjectDisposedException(nameof(UdpServerTransport));
	        }
	        if (socket is null)
	        {
	            socket = options.Endpoint switch
	            {
	                UnixDomainSocketEndPoint    => new Socket(options.Endpoint.AddressFamily, SocketType.Dgram, System.Net.Sockets.ProtocolType.Unspecified),
	                _                           => new Socket(options.Endpoint.AddressFamily, SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp)
	            };
	            if (options.Endpoint is IPEndPoint ip && ip.Address == IPAddress.IPv6Any)
	            {
	                socket.DualMode = true;
	            }
	            socket.Bind(options.Endpoint);
	        }
	        while (true)
	        {
	            try
	            {
	                var settings = this.settings[Interlocked.Increment(ref index) % count];
	                var connection = new SocketTransportConnection(settings);
	                connections.Add(connection);
	                connection.OnDispose = () =>
	                {
	                    connections.Remove(connection);
	                };
	                var started = !ThreadPool.UnsafeQueueUserWorkItem(connection, true);
	                await Middleware.Invoke(new UdpServerTransportContext(connection));
	                return connection;
	            }
	            catch (ObjectDisposedException)
	            {
	                // return null;
	                continue;
	            }
	            catch (SocketException exception) when (exception.SocketErrorCode == SocketError.OperationAborted)
	            {
	                // return null;
	                continue; // A call was made to UnbindAsync/DisposeAsync just return null which signals we're done
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
	        if (isDisposed)
	        {
	            throw new ObjectDisposedException(nameof(UdpServerTransport));
	        }
	        if (socket is not null)
	        {
	            socket.Dispose();
	            isDisposed = true;
	        }
	    }
	    public static UdpServerTransport Create(Action<UdpServerTransportOptions> configure)
	    {
	        if (configure is null)
	        {
	            throw new ArgumentNullException(nameof(configure));
	        }
	        var options = new UdpServerTransportOptions();
	        configure.Invoke(options);
	        return new UdpServerTransport(options);
	    }
	}
	public sealed class UdpServerTransportContext : ITransportContext
	{
	    private readonly SocketTransportConnection connection;
	    internal UdpServerTransportContext(SocketTransportConnection connection)
	    {
	        this.connection = connection;
	    }
	    public ITransportConnection Connection => this.connection;
	    public void SetRemoteEndPoint(EndPoint endpoint)
	    {
	        if (endpoint is null)
	        {
	            throw new ArgumentNullException(nameof(endpoint));
	        }
	        connection.RemoteEndPoint = endpoint;
	    }
	    public void SetPipe(ITransportConnectionPipe pipe)
	    {
	        if (pipe is null)
	        {
	            throw new ArgumentNullException(nameof(pipe));
	        }
	        connection.Pipe = pipe;
	    }
	    public void SetConnectionData(object? data)
	    {
	        this.connection.ConnectionData = data;
	    }
	}
	public abstract class UdpServerTransportMiddleware : ITransportMiddleware
	{
	    public abstract Task InvokeAsync(UdpServerTransportContext context, TransportMiddlewareHandler next);
	    Task ITransportMiddleware.InvokeAsync(ITransportContext context, TransportMiddlewareHandler next)
	    {
	        if (context is UdpServerTransportContext udpContext)
	        {
	            return InvokeAsync(udpContext, next);
	        }
	        return Task.CompletedTask;
	    }
	}
	public sealed class UdpServerTransportOptions
	{
		private TransportTraceHandler onTrace = (code, data, message) => { };
		private TransportMiddlewareHandler middleware = context => Task.CompletedTask;
		public EndPoint Endpoint { get; set; } = new IPEndPoint(IPAddress.Loopback, 8081);
		public int IOQueueCount { get; set; } = Math.Min(Environment.ProcessorCount, 16);
		public bool WaitForDataBeforeAllocatingBuffer { get; set; } = true;
		public long? MaxReadBufferSize { get; set; } = 1024 * 1024;
		public long? MaxWriteBufferSize { get; set; } = 64 * 1024;
		public bool UnsafePreferInLineScheduling { get; set; }
		public TransportMiddlewareHandler Middleware => this.middleware;
		public TransportTraceHandler OnTrace => this.onTrace;
		public void AddTraceHandler(TransportTraceHandler onTrace)
		{
			if (onTrace is null)
			{
				throw new ArgumentNullException(nameof(onTrace));
			}
			this.onTrace = onTrace;
		}
		public void AddTraceHandler<TConnectionData>(Action<UdpTraceCode, TConnectionData, string?> onTrace)
		{
			if (onTrace is null)
			{
				throw new ArgumentNullException(nameof(onTrace));
			}
			this.onTrace = (data, code, message) =>
			{
				if (data is TConnectionData connectionData && code is not null)
				{
					onTrace.Invoke((UdpTraceCode)code, connectionData, message);
				}
			};
		}
		public void AddMiddleware(Action<TransportMiddlewareBuilder<UdpServerTransportContext, UdpServerTransportMiddleware>> configure)
		{
			if (configure is null)
			{
				throw new ArgumentNullException(nameof(configure));
			}
			var builder = new TransportMiddlewareBuilder<UdpServerTransportContext, UdpServerTransportMiddleware>();
			configure.Invoke(builder);
			middleware = builder.Build();
		}
	}
	public enum UdpTraceCode
	{
	}
	#endregion
}
#endregion
