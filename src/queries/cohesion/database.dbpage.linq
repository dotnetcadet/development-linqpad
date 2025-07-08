<Query Kind="Program">
  <Namespace>System.Runtime.InteropServices</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
</Query>

unsafe void Main()
{
	byte[] items;
	byte[] bytes = new byte[Page.Size];

	for (int i = 0; i < (items = BitConverter.GetBytes((long)543928)).Length; i++)
	{
		bytes[i] = items[i];
	}

	var page = new Page(bytes);
	
	page.Id.Dump();
	
	using var stream = new MemoryStream();
	
	var memory = new Memory<byte>();
	
	System.Runtime.InteropServices.CollectionsMarshal.GetValueRefOrNullRef(new Dictionary<int,string>(), 8);
}



/// <summary>
/// 
/// </summary>
public readonly unsafe struct Page
{
	#region Constructors
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="pointer"></param>
	public Page(byte* pointer)
	{
		Pointer = pointer;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="bytes"></param>
	public Page(byte[] bytes)
	{
		fixed (byte* ptr = bytes)
		{
			Pointer = ptr;
		}
	}
	
	#endregion
	
	#region Properties/Fields

	/// <summary>
	/// The set page size in the number of bytes.
	/// </summary>
	public const int Size = 8192;

	/// <summary>
	/// The raw pointer of the 
	/// </summary>
	public readonly byte* Pointer;

	/// <summary>
	/// The page number of
	/// </summary>
	public long Id
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return ((Header*)Pointer)->Id;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			((Header*)Pointer)->Id = value;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public PageType Type 
	{
		get 
		{
			return ((Header*)Pointer)->Type;	
		}
		set 
		{
			((Header*)Pointer)->Type = value;
		}
	}
	
	public short Checksum
	{
		get 
		{
			return ((Header*)Pointer)->Checksum;	
		}
	}

	#endregion

	#region Methods
	
	public int GetOverflowSize()
	{
		return default;
	}

	public void CopyTo(in Page destination)
	{
		Unsafe.CopyBlock(
			destination.Pointer,
			Pointer, 
			(uint)(IsOverflow ? OverflowSize : Size));
	}
	
	public delegate void PageAction<T>(Span<T> span, T item);
	
	
	//public IEnumerable<T> Rows(Action< 




	#endregion
	
	#region Operators
	
	public static implicit operator Page(byte[] bytes)
	{
		return new Page(bytes);
	}
	
	#endregion
//
//	public bool IsOverflow
//	{
//		[MethodImpl(MethodImplOptions.AggressiveInlining)]
//		get { return (((Header*)Pointer)->Flags & PageFlags.Overflow) == PageFlags.Overflow; }
//	}

	public int OverflowSize
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get { return ((Header*)Pointer)->OverflowSize; }
		set { ((Header*)Pointer)->OverflowSize = value; }
	}


	//public Span<byte> AsSpan()
	//{
	//	return new Span<byte>(Pointer, IsOverflow ? OverflowSize + 96 : Size);
	//}


	#region Partials


	[StructLayout(LayoutKind.Explicit, Size = 96, Pack = 1)]
	partial struct Header
	{
		[FieldOffset(0)]
		public long Id;

		[FieldOffset(8)]
		public int OverflowSize;

		[FieldOffset(12)]
		public PageType Type;

		[FieldOffset(22)]
		public fixed byte Reserved1[9];

		[FieldOffset(32)] // used only if we aren't using crypto
		public short Checksum;

		[FieldOffset(32)]// used only when using crypto
		public fixed byte Nonce[16];

		[FieldOffset(48)]
		public fixed byte Mac[16];
	}
	
	#endregion
}

public readonly unsafe struct PageRow
{
	
}


[Flags]
public enum PageType : byte
{
	Catalog,
	Data,
	Index,
	Overflow,
	LargeObject,
	BinaryLargeObject
}
