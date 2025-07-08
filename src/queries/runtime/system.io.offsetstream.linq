<Query Kind="Program">
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	var encoding = Encoding.UTF8;
	
	var message = "My name is Chase Crawford";
	var bytes = encoding.GetBytes(message);
	
	
	using var memoryStream = new MemoryStream();
	
	memoryStream.Write(bytes, 0, bytes.Length);
	
	using var offset = new OffsetStream(memoryStream, 11, 14);
	
	var buffer = new byte[8];
	
	offset.Position = 6;
	offset.Read(buffer, 0, 8);
	
	encoding.GetString(buffer).Dump();
	
}



// length: 30 [15..25]
[DebuggerDisplay("length: {Length} [{Offset}..{Offset + Limit}]")]
public class OffsetStream : Stream
{
	private readonly Stream _stream;
	private readonly bool _leaveOpen;
	private readonly bool _isReadOnly;

	private bool _isDisposed;
	private long _limit;
	private long _offset;
	private long _position = 0;
	

	/// <summary>
	/// 
	/// </summary>
	/// <param name="stream"></param>
	/// <param name="offset">The offset from the current position of the stream.</param>
	/// <param name="limit">The length of bytes to limit reading and writing.</param>
	/// <param name="isReadOnly"></param>
	/// <param name="leaveOpen">Specifies whther to leave the underlying <paramref name="stream"/> open on dispose.</param>
	public OffsetStream(Stream stream, long offset = 0, long limit = 0, bool isReadOnly = false, bool leaveOpen = false)
	{
		_stream = stream;
		_leaveOpen = leaveOpen;
		_offset = offset;
		_limit = limit;
		_isReadOnly = isReadOnly;
	}


	/// <inheritdoc/>
	public override bool CanRead => _stream.CanRead;

	/// <inheritdoc/>
	public override bool CanSeek => _stream.CanSeek;

	/// <inheritdoc/>
	public override bool CanWrite => _stream.CanWrite && !IsReadOnly;

	/// <inheritdoc/>
	public override bool CanTimeout => _stream.CanTimeout;

	/// <inheritdoc/>
	public override long Length => _stream.Length;

	/// <inheritdoc/>
	public override long Position
	{
		get => _position;
		set
		{
			if (!_stream.CanSeek)
			{
				ThrowHelper.ThrowInvalidOperationException("Seeking is not allowed.");
			}
			
			// Check if exceeding boundary of offset
			if (value < 0 || value > _limit)
			{
				ThrowHelper.ThrowInvalidOperationException($"The position {value} cannot exceed boundary of {_offset + _limit}");
			}

			_stream.Position = (_offset + value);
			_position = value;
		}
	}

	/// <summary>
	/// Represents the starting stream position
	/// </summary>
	public long Offset => _offset;

	/// <summary>
	/// The allowed limit of bytes that can be read and written to the stream.
	/// </summary>
	public long Limit => _limit;

	/// <summary>
	/// The remaining bytes left to read withi the offset.
	/// </summary>
	public long Remaining => _limit - (_stream.Position - Offset);

	/// <summary>
	/// Specifies whether the stream is ReadOnly.
	/// </summary>
	public bool IsReadOnly { get; }

	/// <summary>
	/// Flushes the underlying stream.
	/// </summary>
	public override void Flush()
	{
		if (IsReadOnly)
		{
			ThrowHelper.ThrowInvalidOperationException("OffsetStream is read only.");
		}

		_stream.Flush();
	}

	public override Task FlushAsync(CancellationToken cancellationToken)
	{
		if (IsReadOnly)
		{
			ThrowHelper.ThrowInvalidOperationException("OffsetStream is read only.");
		}

		return _stream.FlushAsync(cancellationToken);
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (count < 1)
		{
			return 0;
		}
		if (count > Remaining)
		{
			ThrowHelper.ThrowInvalidOperationException("The count exceeds the remaining readable bytes.");
		}

		int bytesRead = _stream.Read(buffer, offset, count);
		
		_position =+ bytesRead;
		
		return bytesRead;
	}

	public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		if (count < 1)
		{
			return 0;
		}
		if (count > Remaining)
		{
			ThrowHelper.ThrowInvalidOperationException("The count exceeds the remaining readable bytes.");
		}

		Memory<byte> memory = buffer.AsMemory(offset, count);

		int bytesRead = await _stream.ReadAsync(memory, cancellationToken);
		
		_position =+ bytesRead;
		
		return bytesRead;
	}


	public override long Seek(long offset, SeekOrigin origin)
	{
		if (!_stream.CanSeek)
		{
			throw new InvalidOperationException("Stream is not seekable.");
		}

		long pos;

		switch (origin)
		{
			case SeekOrigin.Begin:
				pos = offset;
				break;

			case SeekOrigin.Current:
				pos = Position + offset;
				break;

			case SeekOrigin.End:
				pos = Length + offset;
				break;

			default:
				throw new InvalidOperationException();
		}

		if ((pos < 0) || (pos >= Length))
			throw new EndOfStreamException("OffsetStream reached begining/end of stream.");

		Position = pos;
//		_stream.Position = Offset + Pdosition;
//
//		if (_position > _length)
//			_length = _position;

		return pos;
	}


	/// <summary>
	/// Set length will readjust the limit
	/// </summary>
	/// <param name="limit"></param>
	public override void SetLength(long limit)
	{
		AssertReadOnly();

		// If greater than the underlying length of the stream we 
		// need to adjust the underlying stream length and the 
		if (limit > Length)
		{
			_stream.SetLength(limit);
		}

		_limit = limit;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		AssertReadOnly();
//
//		if (count < 1)
//			return;
//
//		if (_stream.CanSeek)
//			_stream.Position = _offset + _position;
//
//		_stream.Write(buffer, offset, count);
//		_position += count;
//
//		if (_position > _length)
//			_length = _position;
	}

	public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		AssertReadOnly();

		if (count < 1)
			return;

//		if (_stream.CanSeek)
//			_stream.Position = _offset + _position;
//
//		await _stream.WriteAsync(buffer.AsMemory(offset, count), cancellationToken);
//		_position += count;
//
//		if (_position > _length)
//			_length = _position;
	}

	public override async ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
	{
		AssertReadOnly();

		if (buffer.Length < 1)
			return;
//
//		if (_stream.CanSeek)
//			_stream.Position = _offset + _position;
//
//		await _stream.WriteAsync(buffer, cancellationToken);
//		_position += buffer.Length;
//
//		if (_position > _length)
//			_length = _position;
	}

	protected override void Dispose(bool disposing)
	{
		if (_isDisposed)
		{
			ThrowHelper.ThrowObjectDisposedException(nameof(OffsetStream));
		}

		if (disposing && !_leaveOpen && _stream is not null)
		{
			_stream?.Dispose();
		}

		_isDisposed = true;

		base.Dispose(disposing);
	}

	private void AssertReadOnly()
	{
		if (IsReadOnly)
		{
			ThrowHelper.ThrowInvalidOperationException("The stream is ReadOnly.");
		}
	}
}




internal static partial class ThrowHelper
{
	#region Arguments

	internal static void ThrowIfNull(object? value, string paramName)
	{
		if (value is null || (value is string str && string.IsNullOrEmpty(str)))
		{
			throw GetArgumentNullException(paramName);
		}
	}

	[DoesNotReturn]
	internal static void ThrowArgumentNullException(string paramName) =>
		throw GetArgumentNullException(paramName);

	[DoesNotReturn]
	internal static void ThrowArgumentNullException(string paramName, string message) =>
		throw GetArgumentNullException(paramName, message);

	[DoesNotReturn]
	internal static void ThrowArgumentException(string message) =>
		throw GetArgumentException(message);

	[DoesNotReturn]
	internal static void ThrowArgumentException(string message, string paramName) =>
		throw GetArgumentException(message, paramName);

	[DoesNotReturn]
	internal static void ThrowInvalidOperationException(string message) =>
		throw new InvalidOperationException(message);

	internal static ArgumentException GetArgumentException(string message) =>
		new ArgumentException(message);

	internal static ArgumentException GetArgumentException(string message, string paramName) =>
		new ArgumentException(message, paramName);

	internal static ArgumentNullException GetArgumentNullException(string paramName) =>
		new ArgumentNullException(paramName);

	internal static ArgumentNullException GetArgumentNullException(string paramName, string message) =>
		new ArgumentNullException(paramName, message);

	#endregion

	#region Threading

	[DoesNotReturn]
	internal static void ThrowObjectDisposedException(string objectName) =>
		throw GetObjectDisposedException(objectName);

	[DoesNotReturn]
	internal static void ThrowObjectDisposedException(string objectName, string message) =>
		throw GetObjectDisposedException(objectName, message);

	internal static ObjectDisposedException GetObjectDisposedException(string objectName) =>
		new ObjectDisposedException(objectName);

	internal static ObjectDisposedException GetObjectDisposedException(string objectName, string message) =>
		new ObjectDisposedException(objectName, message);

	#endregion

	#region IO

	[DoesNotReturn]
	internal static void ThrowEndOfStreamException(string message)
	{
		throw GetEndOfStreamException(message);
	}

	internal static EndOfStreamException GetEndOfStreamException(string message)
	{
		return new EndOfStreamException(message);
	}

	#endregion

	#region Json Serialization

	[DoesNotReturn]
	internal static void ThrowJsonException(string message) =>
		throw GetJsonException(message);

	internal static JsonException GetJsonException(string message) =>
		new JsonException(message);

	#endregion
}
