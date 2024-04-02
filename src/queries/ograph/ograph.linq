<Query Kind="Program">
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Security.Claims</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

void Main()
{
	Console.WriteLine("test");
}

public class QueryDocument
{

}

#region ValueObjects

public readonly partial struct HeaderValue :
	IList<string?>,
	IReadOnlyList<string?>,
	IEquatable<HeaderValue>,
	IEquatable<string?>,
	IEquatable<string?[]?>
{
	/// <summary>
	/// A readonly instance of the <see cref="HeaderValue"/> struct whose value is an empty string array.
	/// </summary>
	/// <remarks>
	/// In application code, this field is most commonly used to safely represent a <see cref="HeaderValue"/> that has null string values.
	/// </remarks>
	public static readonly HeaderValue Empty = new HeaderValue(Array.Empty<string>());

	private readonly object? values;

	/// <summary>
	/// Initializes a new instance of the <see cref="HeaderValue"/> structure using the specified string.
	/// </summary>
	/// <param name="value">A string value or <c>null</c>.</param>
	public HeaderValue(string? value)
	{
		values = value;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="HeaderValue"/> structure using the specified array of strings.
	/// </summary>
	/// <param name="values">A string array.</param>
	public HeaderValue(string?[]? values)
	{
		this.values = values;
	}

	/// <summary>
	/// Gets the number of <see cref="string"/> elements contained in this <see cref="HeaderValue" />.
	/// </summary>
	public int Count
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			// Take local copy of values so type checks remain valid even if the StringValues is overwritten in memory
			object? value = values;
			if (value is null)
			{
				return 0;
			}
			if (value is string)
			{
				return 1;
			}
			else
			{
				// Not string, not null, can only be string[]
				return Unsafe.As<string?[]>(value).Length;
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public string? Value
	{
		get
		{
			if (values is string str)
			{
				return str;
			}
			if (values is string[] strArr)
			{
				return string.Join(';', strArr);
			}

			return null;
		}
	}

	bool ICollection<string?>.IsReadOnly => true;

	/// <summary>
	/// Defines an implicit conversion of a given string to a <see cref="HeaderValue"/>.
	/// </summary>
	/// <param name="value">A string to implicitly convert.</param>
	public static implicit operator HeaderValue(string? value)
	{
		return new HeaderValue(value);
	}

	/// <summary>
	/// Defines an implicit conversion of a given string array to a <see cref="HeaderValue"/>.
	/// </summary>
	/// <param name="values">A string array to implicitly convert.</param>
	public static implicit operator HeaderValue(string?[]? values)
	{
		return new HeaderValue(values);
	}

	/// <summary>
	/// Defines an implicit conversion of a given <see cref="HeaderValue"/> to a string, with multiple values joined as a comma separated string.
	/// </summary>
	/// <remarks>
	/// Returns <c>null</c> where <see cref="HeaderValue"/> has been initialized from an empty string array or is <see cref="HeaderValue.Empty"/>.
	/// </remarks>
	/// <param name="values">A <see cref="HeaderValue"/> to implicitly convert.</param>
	public static implicit operator string?(HeaderValue values)
	{
		return values.GetStringValue();
	}

	/// <summary>
	/// Defines an implicit conversion of a given <see cref="HeaderValue"/> to a string array.
	/// </summary>
	/// <param name="value">A <see cref="HeaderValue"/> to implicitly convert.</param>
	public static implicit operator string?[]?(HeaderValue value)
	{
		return value.GetArrayValue();
	}





	/// <summary>
	/// Gets the <see cref="string"/> at index.
	/// </summary>
	/// <value>The string at the specified index.</value>
	/// <param name="index">The zero-based index of the element to get.</param>
	/// <exception cref="NotSupportedException">Set operations are not supported on readonly <see cref="HeaderValue"/>.</exception>
	string? IList<string?>.this[int index]
	{
		get => this[index];
		set => throw new NotSupportedException();
	}

	/// <summary>
	/// Gets the <see cref="string"/> at index.
	/// </summary>
	/// <value>The string at the specified index.</value>
	/// <param name="index">The zero-based index of the element to get.</param>
	public string? this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			// Take local copy of values so type checks remain valid even if the StringValues is overwritten in memory
			object? value = values;
			if (value is string str)
			{
				if (index == 0)
				{
					return str;
				}
			}
			else if (value != null)
			{
				// Not string, not null, can only be string[]
				return Unsafe.As<string?[]>(value)[index]; // may throw
			}

			return OutOfBounds(); // throws
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static string OutOfBounds()
	{
		return Array.Empty<string>()[0]; // throws
	}

	/// <summary>
	/// Converts the value of the current <see cref="HeaderValue"/> object to its equivalent string representation, with multiple values joined as a comma separated string.
	/// </summary>
	/// <returns>A string representation of the value of the current <see cref="HeaderValue"/> object.</returns>
	public override string ToString()
	{
		return GetStringValue() ?? string.Empty;
	}

	private string? GetStringValue()
	{
		// Take local copy of values so type checks remain valid even if the StringValues is overwritten in memory
		object? value = values;
		if (value is string s)
		{
			return s;
		}
		else
		{
			return GetStringValueFromArray(value);
		}

		static string? GetStringValueFromArray(object? value)
		{
			if (value is null)
			{
				return null;
			}

			Debug.Assert(value is string[]);
			// value is not null or string, array, can only be string[]
			string?[] values = Unsafe.As<string?[]>(value);
			return values.Length switch
			{
				0 => null,
				1 => values[0],
				_ => GetJoinedStringValueFromArray(values),
			};
		}

		static string GetJoinedStringValueFromArray(string?[] values)
		{
			// Calculate final length
			int length = 0;
			for (int i = 0; i < values.Length; i++)
			{
				string? value = values[i];
				// Skip null and empty values
				if (value != null && value.Length > 0)
				{
					if (length > 0)
					{
						// Add separator
						length++;
					}

					length += value.Length;
				}
			}
			// Create the new string
			return string.Create(length, values, (span, strings) =>
			{
				int offset = 0;
				// Skip null and empty values
				for (int i = 0; i < strings.Length; i++)
				{
					string? value = strings[i];
					if (value != null && value.Length > 0)
					{
						if (offset > 0)
						{
							// Add separator
							span[offset] = ',';
							offset++;
						}

						value.AsSpan().CopyTo(span.Slice(offset));
						offset += value.Length;
					}
				}
			});
		}
	}

	/// <summary>
	/// Creates a string array from the current <see cref="HeaderValue"/> object.
	/// </summary>
	/// <returns>A string array represented by this instance.</returns>
	/// <remarks>
	/// <para>If the <see cref="HeaderValue"/> contains a single string internally, it is copied to a new array.</para>
	/// <para>If the <see cref="HeaderValue"/> contains an array internally it returns that array instance.</para>
	/// </remarks>
	public string?[] ToArray()
	{
		return GetArrayValue() ?? Array.Empty<string>();
	}

	private string?[]? GetArrayValue()
	{
		// Take local copy of values so type checks remain valid even if the StringValues is overwritten in memory
		object? value = this.values;
		if (value is string[] values)
		{
			return values;
		}
		else if (value != null)
		{
			// value not array, can only be string
			return new[] { Unsafe.As<string>(value) };
		}
		else
		{
			return null;
		}
	}

	/// <summary>
	/// Returns the zero-based index of the first occurrence of an item in the <see cref="HeaderValue" />.
	/// </summary>
	/// <param name="item">The string to locate in the <see cref="HeaderValue"></see>.</param>
	/// <returns>the zero-based index of the first occurrence of <paramref name="item" /> within the <see cref="HeaderValue"></see>, if found; otherwise, -1.</returns>
	int IList<string?>.IndexOf(string? item)
	{
		return IndexOf(item);
	}

	private int IndexOf(string? item)
	{
		// Take local copy of values so type checks remain valid even if the StringValues is overwritten in memory
		object? value = this.values;
		if (value is string[] values)
		{
			for (int i = 0; i < values.Length; i++)
			{
				if (string.Equals(values[i], item, StringComparison.Ordinal))
				{
					return i;
				}
			}
			return -1;
		}

		if (value != null)
		{
			// value not array, can only be string
			return string.Equals(Unsafe.As<string>(value), item, StringComparison.Ordinal) ? 0 : -1;
		}

		return -1;
	}

	/// <summary>Determines whether a string is in the <see cref="HeaderValue" />.</summary>
	/// <param name="item">The <see cref="string"/> to locate in the <see cref="HeaderValue" />.</param>
	/// <returns>true if <paramref name="item">item</paramref> is found in the <see cref="HeaderValue" />; otherwise, false.</returns>
	bool ICollection<string?>.Contains(string? item)
	{
		return IndexOf(item) >= 0;
	}

	/// <summary>
	/// Copies the entire <see cref="HeaderValue" />to a string array, starting at the specified index of the target array.
	/// </summary>
	/// <param name="array">The one-dimensional <see cref="Array" /> that is the destination of the elements copied from. The <see cref="Array" /> must have zero-based indexing.</param>
	/// <param name="arrayIndex">The zero-based index in the destination array at which copying begins.</param>
	/// <exception cref="ArgumentNullException"><paramref name="array">array</paramref> is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex">arrayIndex</paramref> is less than 0.</exception>
	/// <exception cref="ArgumentException">The number of elements in the source <see cref="HeaderValue"></see> is greater than the available space from <paramref name="arrayIndex">arrayIndex</paramref> to the end of the destination <paramref name="array">array</paramref>.</exception>
	void ICollection<string?>.CopyTo(string?[] array, int arrayIndex)
	{
		CopyTo(array, arrayIndex);
	}

	private void CopyTo(string?[] array, int arrayIndex)
	{
		// Take local copy of values so type checks remain valid even if the StringValues is overwritten in memory
		object? value = this.values;
		if (value is string[] values)
		{
			Array.Copy(values, 0, array, arrayIndex, values.Length);
			return;
		}

		if (value != null)
		{
			if (array == null)
			{
				throw new ArgumentNullException(nameof(array));
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(arrayIndex));
			}
			if (array.Length - arrayIndex < 1)
			{
				throw new ArgumentException(
					$"'{nameof(array)}' is not long enough to copy all the items in the collection. Check '{nameof(arrayIndex)}' and '{nameof(array)}' length.");
			}

			// value not array, can only be string
			array[arrayIndex] = Unsafe.As<string>(value);
		}
	}

	void ICollection<string?>.Add(string? item) => throw new NotSupportedException();

	void IList<string?>.Insert(int index, string? item) => throw new NotSupportedException();

	bool ICollection<string?>.Remove(string? item) => throw new NotSupportedException();

	void IList<string?>.RemoveAt(int index) => throw new NotSupportedException();

	void ICollection<string?>.Clear() => throw new NotSupportedException();

	/// <summary>Retrieves an object that can iterate through the individual strings in this <see cref="HeaderValue" />.</summary>
	/// <returns>An enumerator that can be used to iterate through the <see cref="HeaderValue" />.</returns>
	public Enumerator GetEnumerator()
	{
		return new Enumerator(values);
	}

	/// <inheritdoc cref="GetEnumerator()" />
	IEnumerator<string?> IEnumerable<string?>.GetEnumerator()
	{
		return GetEnumerator();
	}

	/// <inheritdoc cref="GetEnumerator()" />
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	/// <summary>
	/// Indicates whether the specified <see cref="HeaderValue"/> contains no string values.
	/// </summary>
	/// <param name="value">The <see cref="HeaderValue"/> to test.</param>
	/// <returns>true if <paramref name="value">value</paramref> contains a single null or empty string or an empty array; otherwise, false.</returns>
	public static bool IsNullOrEmpty(HeaderValue value)
	{
		object? data = value.values;
		if (data is null)
		{
			return true;
		}
		if (data is string[] values)
		{
			return values.Length switch
			{
				0 => true,
				1 => string.IsNullOrEmpty(values[0]),
				_ => false,
			};
		}
		else
		{
			// Not array, can only be string
			return string.IsNullOrEmpty(Unsafe.As<string>(data));
		}
	}

	/// <summary>
	/// Concatenates two specified instances of <see cref="HeaderValue"/>.
	/// </summary>
	/// <param name="values1">The first <see cref="HeaderValue"/> to concatenate.</param>
	/// <param name="values2">The second <see cref="HeaderValue"/> to concatenate.</param>
	/// <returns>The concatenation of <paramref name="values1"/> and <paramref name="values2"/>.</returns>
	public static HeaderValue Concat(HeaderValue values1, HeaderValue values2)
	{
		int count1 = values1.Count;
		int count2 = values2.Count;

		if (count1 == 0)
		{
			return values2;
		}

		if (count2 == 0)
		{
			return values1;
		}

		var combined = new string[count1 + count2];
		values1.CopyTo(combined, 0);
		values2.CopyTo(combined, count1);
		return new HeaderValue(combined);
	}

	/// <summary>
	/// Concatenates specified instance of <see cref="HeaderValue"/> with specified <see cref="string"/>.
	/// </summary>
	/// <param name="values">The <see cref="HeaderValue"/> to concatenate.</param>
	/// <param name="value">The <see cref="string" /> to concatenate.</param>
	/// <returns>The concatenation of <paramref name="values"/> and <paramref name="value"/>.</returns>
	public static HeaderValue Concat(in HeaderValue values, string? value)
	{
		if (value == null)
		{
			return values;
		}

		int count = values.Count;
		if (count == 0)
		{
			return new HeaderValue(value);
		}

		var combined = new string[count + 1];
		values.CopyTo(combined, 0);
		combined[count] = value;
		return new HeaderValue(combined);
	}

	/// <summary>
	/// Concatenates specified instance of <see cref="string"/> with specified <see cref="HeaderValue"/>.
	/// </summary>
	/// <param name="value">The <see cref="string" /> to concatenate.</param>
	/// <param name="values">The <see cref="HeaderValue"/> to concatenate.</param>
	/// <returns>The concatenation of <paramref name="values"/> and <paramref name="values"/>.</returns>
	public static HeaderValue Concat(string? value, in HeaderValue values)
	{
		if (value == null)
		{
			return values;
		}

		int count = values.Count;
		if (count == 0)
		{
			return new HeaderValue(value);
		}

		var combined = new string[count + 1];
		combined[0] = value;
		values.CopyTo(combined, 1);
		return new HeaderValue(combined);
	}

	/// <summary>
	/// Determines whether two specified <see cref="HeaderValue"/> objects have the same values in the same order.
	/// </summary>
	/// <param name="left">The first <see cref="HeaderValue"/> to compare.</param>
	/// <param name="right">The second <see cref="HeaderValue"/> to compare.</param>
	/// <returns><c>true</c> if the value of <paramref name="left"/> is the same as the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
	public static bool Equals(HeaderValue left, HeaderValue right)
	{
		int count = left.Count;

		if (count != right.Count)
		{
			return false;
		}

		for (int i = 0; i < count; i++)
		{
			if (left[i] != right[i])
			{
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// Determines whether two specified <see cref="HeaderValue"/> have the same values.
	/// </summary>
	/// <param name="left">The first <see cref="HeaderValue"/> to compare.</param>
	/// <param name="right">The second <see cref="HeaderValue"/> to compare.</param>
	/// <returns><c>true</c> if the value of <paramref name="left"/> is the same as the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
	public static bool operator ==(HeaderValue left, HeaderValue right)
	{
		return Equals(left, right);
	}

	/// <summary>
	/// Determines whether two specified <see cref="HeaderValue"/> have different values.
	/// </summary>
	/// <param name="left">The first <see cref="HeaderValue"/> to compare.</param>
	/// <param name="right">The second <see cref="HeaderValue"/> to compare.</param>
	/// <returns><c>true</c> if the value of <paramref name="left"/> is different to the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
	public static bool operator !=(HeaderValue left, HeaderValue right)
	{
		return !Equals(left, right);
	}

	/// <summary>
	/// Determines whether this instance and another specified <see cref="HeaderValue"/> object have the same values.
	/// </summary>
	/// <param name="other">The string to compare to this instance.</param>
	/// <returns><c>true</c> if the value of <paramref name="other"/> is the same as the value of this instance; otherwise, <c>false</c>.</returns>
	public bool Equals(HeaderValue other) => Equals(this, other);

	/// <summary>
	/// Determines whether the specified <see cref="string"/> and <see cref="HeaderValue"/> objects have the same values.
	/// </summary>
	/// <param name="left">The <see cref="string"/> to compare.</param>
	/// <param name="right">The <see cref="HeaderValue"/> to compare.</param>
	/// <returns><c>true</c> if the value of <paramref name="left"/> is the same as the value of <paramref name="right"/>; otherwise, <c>false</c>. If <paramref name="left"/> is <c>null</c>, the method returns <c>false</c>.</returns>
	public static bool Equals(string? left, HeaderValue right) => Equals(new HeaderValue(left), right);

	/// <summary>
	/// Determines whether the specified <see cref="HeaderValue"/> and <see cref="string"/> objects have the same values.
	/// </summary>
	/// <param name="left">The <see cref="HeaderValue"/> to compare.</param>
	/// <param name="right">The <see cref="string"/> to compare.</param>
	/// <returns><c>true</c> if the value of <paramref name="left"/> is the same as the value of <paramref name="right"/>; otherwise, <c>false</c>. If <paramref name="right"/> is <c>null</c>, the method returns <c>false</c>.</returns>
	public static bool Equals(HeaderValue left, string? right) => Equals(left, new HeaderValue(right));

	/// <summary>
	/// Determines whether this instance and a specified <see cref="string"/>, have the same value.
	/// </summary>
	/// <param name="other">The <see cref="string"/> to compare to this instance.</param>
	/// <returns><c>true</c> if the value of <paramref name="other"/> is the same as this instance; otherwise, <c>false</c>. If <paramref name="other"/> is <c>null</c>, returns <c>false</c>.</returns>
	public bool Equals(string? other) => Equals(this, new HeaderValue(other));

	/// <summary>
	/// Determines whether the specified string array and <see cref="HeaderValue"/> objects have the same values.
	/// </summary>
	/// <param name="left">The string array to compare.</param>
	/// <param name="right">The <see cref="HeaderValue"/> to compare.</param>
	/// <returns><c>true</c> if the value of <paramref name="left"/> is the same as the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
	public static bool Equals(string?[]? left, HeaderValue right) => Equals(new HeaderValue(left), right);

	/// <summary>
	/// Determines whether the specified <see cref="HeaderValue"/> and string array objects have the same values.
	/// </summary>
	/// <param name="left">The <see cref="HeaderValue"/> to compare.</param>
	/// <param name="right">The string array to compare.</param>
	/// <returns><c>true</c> if the value of <paramref name="left"/> is the same as the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
	public static bool Equals(HeaderValue left, string?[]? right) => Equals(left, new HeaderValue(right));

	/// <summary>
	/// Determines whether this instance and a specified string array have the same values.
	/// </summary>
	/// <param name="other">The string array to compare to this instance.</param>
	/// <returns><c>true</c> if the value of <paramref name="other"/> is the same as this instance; otherwise, <c>false</c>.</returns>
	public bool Equals(string?[]? other) => Equals(this, new HeaderValue(other));

	/// <inheritdoc cref="Equals(HeaderValue, string)" />
	public static bool operator ==(HeaderValue left, string? right) => Equals(left, new HeaderValue(right));

	/// <summary>
	/// Determines whether the specified <see cref="HeaderValue"/> and <see cref="string"/> objects have different values.
	/// </summary>
	/// <param name="left">The <see cref="HeaderValue"/> to compare.</param>
	/// <param name="right">The <see cref="string"/> to compare.</param>
	/// <returns><c>true</c> if the value of <paramref name="left"/> is different to the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
	public static bool operator !=(HeaderValue left, string? right) => !Equals(left, new HeaderValue(right));

	/// <inheritdoc cref="Equals(string, HeaderValue)" />
	public static bool operator ==(string? left, HeaderValue right) => Equals(new HeaderValue(left), right);

	/// <summary>
	/// Determines whether the specified <see cref="string"/> and <see cref="HeaderValue"/> objects have different values.
	/// </summary>
	/// <param name="left">The <see cref="string"/> to compare.</param>
	/// <param name="right">The <see cref="HeaderValue"/> to compare.</param>
	/// <returns><c>true</c> if the value of <paramref name="left"/> is different to the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
	public static bool operator !=(string left, HeaderValue right) => !Equals(new HeaderValue(left), right);

	/// <inheritdoc cref="Equals(HeaderValue, string[])" />
	public static bool operator ==(HeaderValue left, string?[]? right) => Equals(left, new HeaderValue(right));

	/// <summary>
	/// Determines whether the specified <see cref="HeaderValue"/> and string array have different values.
	/// </summary>
	/// <param name="left">The <see cref="HeaderValue"/> to compare.</param>
	/// <param name="right">The string array to compare.</param>
	/// <returns><c>true</c> if the value of <paramref name="left"/> is different to the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
	public static bool operator !=(HeaderValue left, string?[]? right) => !Equals(left, new HeaderValue(right));

	/// <inheritdoc cref="Equals(string[], HeaderValue)" />
	public static bool operator ==(string?[]? left, HeaderValue right) => Equals(new HeaderValue(left), right);

	/// <summary>
	/// Determines whether the specified string array and <see cref="HeaderValue"/> have different values.
	/// </summary>
	/// <param name="left">The string array to compare.</param>
	/// <param name="right">The <see cref="HeaderValue"/> to compare.</param>
	/// <returns><c>true</c> if the value of <paramref name="left"/> is different to the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
	public static bool operator !=(string?[]? left, HeaderValue right) => !Equals(new HeaderValue(left), right);

	/// <summary>
	/// Determines whether the specified <see cref="HeaderValue"/> and <see cref="object"/>, which must be a
	/// <see cref="HeaderValue"/>, <see cref="string"/>, or array of <see cref="string"/>, have the same value.
	/// </summary>
	/// <param name="left">The <see cref="HeaderValue"/> to compare.</param>
	/// <param name="right">The <see cref="object"/> to compare.</param>
	/// <returns><c>true</c> if the <paramref name="left"/> object is equal to the <paramref name="right"/>; otherwise, <c>false</c>.</returns>
	public static bool operator ==(HeaderValue left, object? right) => left.Equals(right);

	/// <summary>
	/// Determines whether the specified <see cref="HeaderValue"/> and <see cref="object"/>, which must be a
	/// <see cref="HeaderValue"/>, <see cref="string"/>, or array of <see cref="string"/>, have different values.
	/// </summary>
	/// <param name="left">The <see cref="HeaderValue"/> to compare.</param>
	/// <param name="right">The <see cref="object"/> to compare.</param>
	/// <returns><c>true</c> if the <paramref name="left"/> object is equal to the <paramref name="right"/>; otherwise, <c>false</c>.</returns>
	public static bool operator !=(HeaderValue left, object? right) => !left.Equals(right);

	/// <summary>
	/// Determines whether the specified <see cref="object"/>, which must be a
	/// <see cref="HeaderValue"/>, <see cref="string"/>, or array of <see cref="string"/>, and specified <see cref="HeaderValue"/>,  have the same value.
	/// </summary>
	/// <param name="left">The <see cref="HeaderValue"/> to compare.</param>
	/// <param name="right">The <see cref="object"/> to compare.</param>
	/// <returns><c>true</c> if the <paramref name="left"/> object is equal to the <paramref name="right"/>; otherwise, <c>false</c>.</returns>
	public static bool operator ==(object? left, HeaderValue right) => right.Equals(left);

	/// <summary>
	/// Determines whether the specified <see cref="object"/> and <see cref="HeaderValue"/> object have the same values.
	/// </summary>
	/// <param name="left">The <see cref="object"/> to compare.</param>
	/// <param name="right">The <see cref="HeaderValue"/> to compare.</param>
	/// <returns><c>true</c> if the <paramref name="left"/> object is equal to the <paramref name="right"/>; otherwise, <c>false</c>.</returns>
	public static bool operator !=(object? left, HeaderValue right) => !right.Equals(left);

	/// <summary>
	/// Determines whether this instance and a specified object have the same value.
	/// </summary>
	/// <param name="obj">An object to compare with this object.</param>
	/// <returns><c>true</c> if the current object is equal to <paramref name="obj"/>; otherwise, <c>false</c>.</returns>
	public override bool Equals(object? obj)
	{
		if (obj == null)
		{
			return Equals(this, HeaderValue.Empty);
		}

		if (obj is string str)
		{
			return Equals(this, str);
		}

		if (obj is string[] array)
		{
			return Equals(this, array);
		}

		if (obj is HeaderValue stringValues)
		{
			return Equals(this, stringValues);
		}

		return false;
	}

	/// <inheritdoc />
	public override int GetHashCode()
	{
		object? value = this.values;
		if (value is string[] values)
		{
			if (Count == 1)
			{
				return Unsafe.As<string>(this[0])?.GetHashCode() ?? Count.GetHashCode();
			}
			int hashCode = 0;
			for (int i = 0; i < values.Length; i++)
			{
				var rol5 = ((uint)hashCode << 5) | ((uint)hashCode >> 27);
				hashCode = ((int)rol5 + hashCode) ^ values[i]?.GetHashCode() ?? 0;
			}
			return hashCode;
		}
		else
		{
			return Unsafe.As<string>(value)?.GetHashCode() ?? Count.GetHashCode();
		}
	}

	/// <summary>
	/// Enumerates the string values of a <see cref="HeaderValue" />.
	/// </summary>
	public struct Enumerator : IEnumerator<string?>
	{
		private readonly string?[]? _values;
		private int _index;
		private string? _current;

		internal Enumerator(object? value)
		{
			if (value is string str)
			{
				_values = null;
				_current = str;
			}
			else
			{
				_current = null;
				_values = Unsafe.As<string?[]>(value);
			}
			_index = 0;
		}

		public Enumerator(ref HeaderValue values) : this(values.values)
		{ }

		public bool MoveNext()
		{
			int index = _index;
			if (index < 0)
			{
				return false;
			}

			string?[]? values = _values;
			if (values != null)
			{
				if ((uint)index < (uint)values.Length)
				{
					_index = index + 1;
					_current = values[index];
					return true;
				}

				_index = -1;
				return false;
			}

			_index = -1; // sentinel value
			return _current != null;
		}

		public string? Current => _current;

		object? IEnumerator.Current => _current;

		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		public void Dispose()
		{
		}
	}
}
/// <summary>
///
/// </summary>
public readonly struct Host
{
	public Host(string value)
	{
		this.Value = value;
	}
	public Host(string host, int port)
	{
		if (host == null)
		{
			throw new ArgumentNullException(nameof(host));
		}
		if (port <= 0)
		{
			throw new ArgumentOutOfRangeException(nameof(port), "");
		}

		int index;
		if (!host.Contains('[')
			&& (index = host.IndexOf(':')) >= 0
			&& index < host.Length - 1
			&& host.IndexOf(':', index + 1) >= 0)
		{
			// IPv6 without brackets ::1 is the only type of host with 2 or more colons
			host = $"[{host}]";
		}

		this.Value = host + ":" + port.ToString(CultureInfo.InvariantCulture);
		this.Port = port;
	}

	public string Value { get; }
	public int? Port { get; }

	public override string ToString()
	{
		return Value;
	}
}



/// <summary>
/// Represents a given HTTP method.
/// </summary>
public readonly struct Method :
  IEquatable<Method>,
  IEqualityComparer<Method>
{
	private const string allowedCharacters = "abcdefghijklmnopqrstuvwxwzABCDEFGHIJKLMNOPQRSTUVWXYZ";
	public Method(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			throw new ArgumentNullException(nameof(value));
		}
		if (value.Any(character => !allowedCharacters.Contains(character)))
		{
			throw new Exception("Only Alphabetic characters are allowed as Method names");
		}
		Value = value.ToUpperInvariant();
	}

	/// <summary>
	///
	/// </summary>
	public string Value { get; }

	/// <inheritdoc />
	public override bool Equals([NotNullWhen(true)] object? instance)
	{
		if (instance is Method method)
		{
			return Equals(method);
		}

		return false;
	}

	/// <inheritdoc />
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	/// <inheritdoc />
	public override string ToString()
	{
		return Value;
	}

	/// <inheritdoc />
	public bool Equals(Method other)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public bool Equals(Method left, Method right)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public int GetHashCode([DisallowNull] Method instance)
	{
		throw new NotImplementedException();
	}

	public static implicit operator Method(string value) => new Method(value);
	public static implicit operator string(Method method) => method.Value;



	public static Method Get => "GET";
	public static Method Post => "POST";
	public static Method Put => "PUT";
	public static Method Delete => "DELETE";
	public static Method Patch => "PATCH";
	public static Method Head => "HEAD";
	public static Method Options => "OPTIONS";
	public static Method Trace => "TRACE";
}

/// <summary>
///
/// </summary>
public readonly struct Name :
	IEquatable<Name>,
	IEqualityComparer<Name>,
	IComparable<Name>
{
	// Allowed characters for name
	private const string expression = "^[a-zA-Z0-9]:-";

	public Name(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			throw new ArgumentNullException(nameof(value));
		}
		if (!Regex.IsMatch(value, expression))
		{
			throw new ArgumentException($"The following name: '{value}' contains invalid characters. Only the following characters are: [A-Z, a-z, 0-9]");
		}
		Value = value;
	}

	/// <summary>
	/// The raw name value.
	/// </summary>
	public readonly string Value { get; }

	/// <summary>
	/// Converts the string to pascal case.
	/// </summary>
	/// <returns></returns>
	public string ToPascalCase()
	{
		return string.Create(Value.Length, Value, (chars, name) =>
		{
			name.CopyTo(chars);

			for (int i = 0; i < chars.Length && (i != 1 || char.IsUpper(chars[i])); i++)
			{
				bool flag = i + 1 < chars.Length;
				if (i > 0 && flag && !char.IsUpper(chars[i + 1]))
				{
					if (chars[i + 1] == ' ')
					{
						chars[i] = char.ToUpperInvariant(chars[i]);
					}
					break;
				}
				chars[i] = char.ToUpperInvariant(chars[i]);
			}
		});
	}

	/// <summary>
	/// Converts the name to camal case
	/// </summary>
	/// <returns></returns>
	public string ToCamalCase()
	{
		return string.Create(Value.Length, Value, (chars, name) =>
		{
			name.CopyTo(chars);

			for (int i = 0; i < chars.Length && (i != 1 || char.IsUpper(chars[i])); i++)
			{
				bool flag = i + 1 < chars.Length;
				if (i > 0 && flag && !char.IsUpper(chars[i + 1]))
				{
					if (chars[i + 1] == ' ')
					{
						chars[i] = char.ToLowerInvariant(chars[i]);
					}
					break;
				}
				chars[i] = char.ToLowerInvariant(chars[i]);
			}
		});
	}

	/// <inheritdoc />
	public override string ToString()
	{
		return Value;
	}

	/// <inheritdoc />
	public override bool Equals([NotNullWhen(true)] object? instance)
	{
		if (instance is not Name name)
		{
			return false;
		}

		return this.Equals(name);
	}

	/// <inheritdoc />
	public override int GetHashCode()
	{
		// TODO: Need to revisit. Not sure if I want the HashCode for the name to be the same as the instace of the string.
		return Value.ToLowerInvariant().GetHashCode();
	}

	/// <inheritdoc />
	public bool Equals(Name name)
	{
		return Value.Equals(name.Value, StringComparison.OrdinalIgnoreCase);
	}

	/// <inheritdoc />
	public bool Equals(Name left, Name right)
	{
		return left.Equals(right);
	}

	/// <inheritdoc />
	public int GetHashCode([DisallowNull] Name name)
	{
		return name.GetHashCode();
	}

	/// <inheritdoc />
	public int CompareTo(Name name)
	{
		return Value.ToLowerInvariant().CompareTo(name.Value.ToLowerInvariant());
	}

	public static implicit operator Name(string value) => new Name(value);
	public static implicit operator string(Name name) => name.Value;
	public static bool operator ==(Name left, Name right) => left.Equals(right);
	public static bool operator !=(Name left, Name right) => !left.Equals(right);
	public static bool operator <(Name left, Name right) => left.CompareTo(right) < 0;
	public static bool operator >(Name left, Name right) => left.CompareTo(right) > 0;
	public static bool operator <=(Name left, Name right) => left.CompareTo(right) <= 0;
	public static bool operator >=(Name left, Name right) => left.CompareTo(right) >= 0;
}


public enum OperationType
{
	/// <summary>
	/// Represent state change.
	/// </summary>
	Command,
	/// <summary>
	/// Represent data retrieval asdf
	/// </summary>
	Query
}



[DebuggerDisplay("Path: /{ToString()}")]
public readonly struct Path : IEquatable<Path>, IEqualityComparer<Path>
{
	private const string invalidCharacters = @"<>*%&:\?";

	private readonly PathSegment[] segments;

	public Path(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			throw new ArgumentNullException(nameof(path));
		}
		//if (path.Any(c => invalidCharacters.Contains(c)))
		//{
		//  throw new ArgumentException($"The following path: '{path}' contains an invalid character. Disallowed Characters '{invalidCharacters}'.");
		//}
		segments = GetSegments(path);
	}

	private PathSegment[] GetSegments(string path)
	{
		var index = 0;
		var segments = new PathSegment[10];
		var segment = string.Empty;

		for (int i = 0; i < path.Length; i++)
		{
			var character = path[i];

			// Check if we reached the end of the current segment
			if (character == '/' || character == '\\')
			{
				// Let's skip leading slashes
				if (i == 0) continue;

				segments[index] = new PathSegment(segment);
				index++;
				segment = string.Empty;

				// Lets see if we reached the buffer in the array. Resize if reached.
				if (index == segments.Length)
				{
					Array.Resize(ref segments, 5);
				}
			}
			else
			{
				segment = segment + character;
			}
		}

		// Resizes segments to actual length
		Array.Resize(ref segments, index);

		return segments;
	}

	/// <summary>
	/// A collection of path segments.
	/// </summary>
	public PathSegment[] Segments
	{
		get
		{
			var copy = new PathSegment[segments.Length];
			segments.CopyTo(copy, 0);
			return copy;
		}
	}

	/// <inheritdoc />
	public override string ToString()
	{
		return string.Join("/", Segments.Select(s => s.Value));
	}

	/// <inheritdoc />
	public override int GetHashCode()
	{
		var hashCode = new HashCode();
		foreach (var segment in Segments)
		{
			hashCode.Add(segment);
		}
		return hashCode.ToHashCode();
	}

	/// <inheritdoc />
	public override bool Equals([NotNullWhen(true)] object? instance)
	{
		if (instance is Path path)
		{
			return Equals(path);
		}

		return false;
	}

	/// <inheritdoc />
	public bool Equals(Path path)
	{
		if (path.Segments.Length != Segments.Length)
		{
			return false;
		}
		for (int i = 0; i < Segments.Length; i++)
		{
			var incoming = path.Segments[i];
			var current = Segments[i];

			if (incoming.Value != current.Value)
			{
				return false;
			}
		}

		return true;
	}

	/// <inheritdoc />
	public bool Equals(Path left, Path right) => left.Equals(right);

	/// <inheritdoc />
	public int GetHashCode([DisallowNull] Path path)
	{
		return path.GetHashCode();
	}

	public static implicit operator Path(string path) => new Path(path);
	public static implicit operator string(Path path) => path.ToString();
}


[DebuggerDisplay("Path Segment: {Value}")]
public readonly struct PathSegment
{
	internal PathSegment(string value)
	{
		if (value is null)
		{
			throw new ArgumentNullException(nameof(value));
		}

		this.Value = value;
	}
	/// <summary>
	/// The raw segment value.
	/// </summary>
	public string Value { get; }
}


public readonly struct QueryValue
{
	public QueryValue(string value)
	{
		this.Value = value;
	}

	public string Value { get; }


	public static implicit operator QueryValue(string value) => new QueryValue(value);
	public static implicit operator string(QueryValue value) => value.Value;

}



[DebuggerDisplay("Route: /{ToString()}")]
public readonly struct Route :
	IEquatable<Route>,
	IEqualityComparer<Route>
{
	private readonly string route;
	private readonly RouteSegment[] segments;

	public Route(string route)
	{
		if (string.IsNullOrEmpty(route))
		{
			throw new ArgumentNullException(nameof(route));
		}
		this.route = route;
		this.segments = GetSegments(route);
	}

	private RouteSegment[] GetSegments(ReadOnlySpan<char> route)
	{
		var index = 0;
		var segments = new RouteSegment[10];
		var segment = string.Empty;

		for (int i = 0; i < route.Length; i++)
		{
			var character = route[i];

			// Check if we reached the end of the current segment
			if (character == '/' || character == '\\')
			{
				// Let's skip leading slashes
				if (i == 0) continue;

				segments[index] = new RouteSegment(segment);
				index++;
				segment = string.Empty;

				// Lets see if we reached the buffer in the array. Resize if reached.
				if (index == segments.Length)
				{
					Array.Resize(ref segments, 5);
				}
			}
			else
			{
				segment = segment + character;
			}
		}

		// Resizes segments to actual length
		Array.Resize(ref segments, index);

		return segments;
	}

	/// <summary>
	/// Gets the raw route value.
	/// </summary>
	public string Value => route;
	/// <summary>
	/// Gets a copy of the route segment.
	/// </summary>
	public RouteSegment[] Segments
	{
		get
		{
			// Let's only return copy
			var copy = new RouteSegment[segments.Length];
			segments.CopyTo(copy, 0);
			return copy;
		}
	}
	/// <summary>
	/// Returns a formatted route value.
	/// </summary>
	public override string ToString()
	{
		return "/" + string.Join('/', Segments.Select(x => x.Value));
	}
	/// <summary>
	///
	/// </summary>
	public override int GetHashCode()
	{
		var hashCode = new HashCode();
		foreach (var segment in Segments)
		{
			hashCode.Add(segment);
		}
		return hashCode.ToHashCode();
	}
	/// <summary>
	///
	/// </summary>
	public override bool Equals([NotNullWhen(true)] object? instance)
	{
		if (instance is Route route)
		{
			return Equals(route);
		}
		return false;
	}
	/// <summary>
	///
	/// </summary>
	public bool Equals(Route route)
	{
		var left = Segments;
		var right = route.Segments;

		if (left.Length != right.Length)
		{
			return false;
		}
		for (int i = 0; i < left.Length; i++)
		{
			if (!left[i].Equals(right[i]))
			{
				return false;
			}
		}
		return true;
	}
	/// <summary>
	///
	/// </summary>
	public bool Equals(Route left, Route right)
	{
		return left.Equals(right);
	}
	/// <summary>
	///
	/// </summary>
	public int GetHashCode([DisallowNull] Route instance)
	{
		return instance.GetHashCode();
	}
	/// <summary>
	///
	/// </summary>
	/// <param name="path"></param>
	public bool IsMatch(Path path)
	{
		return IsMatch(path);
	}
	/// <summary>
	/// Matches the Path to the Route
	/// </summary>
	/// <param name="path"></param>
	/// <param name="prefix"></param>
	/// <returns></returns>
	public bool IsMatch(Path path, string prefix = null)
	{
		for (int i = 0; i < segments.Length; i++)
		{

		}
		var pathSegments = path.Segments;
		var routeSegments = Segments;

		if (pathSegments.Length != routeSegments.Length)
		{
			return false;
		}

		for (int i = 0; i < pathSegments.Length; i++)
		{

		}



		//path.Segments
		//    path = path.ToString().Replace(prefix, "");
		//
		//    if (path.Segments.Length != Segments.Length)
		//    {
		//      return false;
		//    }
		//    for (int i = 0; i < Segments.Length; i++)
		//    {
		//      var routeSegment = Segments[i];
		//      var pathSegment = path.Segments[i];
		//
		//      if (routeSegment.IsParameter)
		//      {
		//        if (!routeSegment.IsValid(pathSegment))
		//        {
		//          return false;
		//        }
		//        continue;
		//      }
		//      if (!routeSegment.Value.Equals(pathSegment.Value, StringComparison.CurrentCultureIgnoreCase))
		//      {
		//        return false;
		//      }
		//    }
		return true;
	}

	/// <summary>
	///
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="paramName"></param>
	/// <returns></returns>
	public T GetRouteValue<T>(string paramName)
		where T : struct
	{
		for (int i = 0; i < segments.Length; i++)
		{

		}

		throw new InvalidOperationException($"No route value with parameter name: {paramName} was found");
	}

	public static implicit operator Route(string route) => new Route(route);
	public static implicit operator string(Route route) => route.ToString();
}


[DebuggerDisplay("Segment: {Value}")]
public readonly struct RouteSegment :
	IEquatable<RouteSegment>,
	IEqualityComparer<RouteSegment>
{
	//private readonly Func<string, >
	internal RouteSegment(string value)
	{
		if (value[0] == '{' && value[value.Length - 1] == '}')
		{
			var item = value.Substring(1, value.Length - 2);

			this.Value = value;
			this.SegmentType = RouteSegmentType.Parameter;
		}
		else
		{
			this.Value = value;
			this.SegmentType = RouteSegmentType.Literal;
		}
	}

	/// <summary>
	/// The raw segment value.
	/// </summary>
	public string Value { get; }
	/// <summary>
	/// A
	/// </summary>
	public RouteSegmentType SegmentType { get; }
	/// <summary>
	/// The index of the route
	/// </summary>
	public int OrdinalId { get; }
	/// <summary>
	///
	/// </summary>
	public bool Equals(RouteSegment other)
	{
		return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
	}
	/// <summary>
	///
	/// </summary>
	public bool Equals(RouteSegment right, RouteSegment left)
	{
		return right.Equals(left);
	}
	/// <summary>
	///
	/// </summary>
	public int GetHashCode(RouteSegment instance)
	{
		return instance.GetHashCode();
	}

	/// <inheritdoc />
	public override bool Equals([NotNullWhen(true)] object? instance)
	{
		if (instance is RouteSegment segment)
		{
			return Equals(segment);
		}
		return false;
	}

	/// <inheritdoc />
	public override int GetHashCode()
	{
		return HashCode.Combine(this, Value);
	}

	/// <inheritdoc />
	public override string ToString() => Value;


	/// <summary>
	///
	/// </summary>
	/// <param name="paramName"></param>
	/// <returns></returns>
	//public bool HasName(string paramName)
	//{

	//}

	/// <summary>
	///
	/// </summary>
	/// <param name="segment"></param>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException"></exception>
	public bool IsLiteralMatch(PathSegment segment)
	{
		if (SegmentType != RouteSegmentType.Literal)
		{
			throw new InvalidOperationException("");
		}

		return Value.Equals(segment.Value, StringComparison.OrdinalIgnoreCase);
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="segment"></param>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException"></exception>
	public bool IsParameterMatch(PathSegment segment)
	{
		if (SegmentType != RouteSegmentType.Parameter)
		{
			throw new InvalidOperationException("");
		}

		return true;
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="segment"></param>
	/// <returns></returns>
	public bool IsValid(PathSegment segment)
	{
		//segment.Value.Se


		return true;
	}
}



public enum RouteSegmentType
{
	Parameter,
	Literal
}


/// <summary>
/// Represents an HTTP Status Code.
/// </summary>
public readonly struct StatusCode :
	IEquatable<StatusCode>,
	IEqualityComparer<StatusCode>,
	IComparable<StatusCode>
{
	/// <summary>
	/// Represents the valid status supported by OGraph
	/// </summary>
	public static ReadOnlySpan<int> ValidStatusCodes => new int[]
	{
		200, // Ok
        201, // Created
        202, // Accepted
        204, // NotContent
        207, // MultiStatus
        400, // BadRequest
        401, // Unauthorized
        403, // Forbidden
        404, // NotFound
        405, // MethodNotAllowed
        406, // NotAcceptable
        408, // RequestTimeout
        409, // Conflict
        412, // PreconditionFailed
        414, // RequestUriTooLong
        415, // UnsupportedMediaType
        428, // PreconditionRequired
        429, // TooManyRequests
        500, // InternalServerError
        501, // NotImplemented
        502, // BadGateway
        503, // ServiceUnavailable
    };

	public StatusCode(int code)
	{
		if (!ValidStatusCodes.Contains(code))
		{
			throw new ArgumentOutOfRangeException(nameof(code), "The status code is not valid.");
		}
		this.Code = code;
	}

	/// <summary>
	///
	/// </summary>
	public int Code { get; }

	/// <inheritdoc />
	bool IEquatable<StatusCode>.Equals(StatusCode statusCode)
	{
		return this.Code == statusCode.Code;
	}

	/// <inheritdoc />
	bool IEqualityComparer<StatusCode>.Equals(StatusCode left, StatusCode right)
	{
		return left.Equals(right);
	}

	/// <inheritdoc />
	int IEqualityComparer<StatusCode>.GetHashCode(StatusCode statusCode)
	{
		return statusCode.GetHashCode();
	}

	/// <inheritdoc />
	int IComparable<StatusCode>.CompareTo(StatusCode statusCode)
	{
		return statusCode.Code.CompareTo(this.Code);
	}

	#region Overloads
	/// <inheritdoc />
	public override int GetHashCode()
	{
		return Code.GetHashCode();
	}

	/// <inheritdoc />
	public override string ToString()
	{
		return $"{Enum.GetName(typeof(HttpStatusCode), (HttpStatusCode)Code)} - {Code}";
	}

	/// <inheritdoc />
	public override bool Equals([NotNullWhen(true)] object? instance)
	{
		if (instance is StatusCode statusCode)
		{
			return Equals(statusCode);
		}
		return false;
	}
	#endregion

	#region Operators
	public static implicit operator StatusCode(int code) => new StatusCode(code);
	public static implicit operator int(StatusCode status) => status.Code;
	public static bool operator ==(StatusCode left, StatusCode right) => left.Equals(right);
	public static bool operator !=(StatusCode left, StatusCode right) => !left.Equals(right);
	public static bool operator >(StatusCode left, StatusCode right) => ((IComparable<StatusCode>)right).CompareTo(left) > 0;
	public static bool operator <(StatusCode left, StatusCode right) => ((IComparable<StatusCode>)right).CompareTo(left) < 0;
	public static bool operator >=(StatusCode left, StatusCode right) => ((IComparable<StatusCode>)right).CompareTo(left) >= 0;
	public static bool operator <=(StatusCode left, StatusCode right) => ((IComparable<StatusCode>)right).CompareTo(left) <= 0;
	#endregion

	#region Success Status Codes
	public static StatusCode Ok => new StatusCode(200);
	public static StatusCode Created => new StatusCode(201);
	public static StatusCode Accepted => new StatusCode(202);
	public static StatusCode NotContent => new StatusCode(204);
	public static StatusCode MultiStatus => new StatusCode(207);
	#endregion

	#region Bad Request Status Code
	public static StatusCode BadRequest => new StatusCode(400);
	public static StatusCode Unauthorized => new StatusCode(401);
	public static StatusCode Forbidden => new StatusCode(403);
	public static StatusCode NotFound => new StatusCode(404);
	public static StatusCode MethodNotAllowed => new StatusCode(405);
	public static StatusCode NotAcceptable => new StatusCode(406);
	public static StatusCode RequestTimeout => new StatusCode(408);
	public static StatusCode Conflict => new StatusCode(409);
	public static StatusCode PreconditionFailed => new StatusCode(412);
	public static StatusCode RequestUriTooLong => new StatusCode(414);
	public static StatusCode UnsupportedMediaType => new StatusCode(415);
	public static StatusCode PreconditionRequired => new StatusCode(428);
	public static StatusCode TooManyRequests => new StatusCode(429);

	#endregion

	#region Server Error Status Codes
	public static StatusCode InternalServerError => new StatusCode(500);
	public static StatusCode NotImplemented => new StatusCode(501);
	public static StatusCode BadGateway => new StatusCode(502);
	public static StatusCode ServiceUnavailable => new StatusCode(503);
	#endregion
}


/// <summary>
/// Types are categorized by their identifiers.
/// </summary>
public enum TypeIdentifier
{
	None,
	/// <summary>
	///
	/// </summary>
	Collection,
	/// <summary>
	///
	/// </summary>
	Complex,
	/// <summary>
	///
	/// </summary>
	Primitive,
	/// <summary>
	///
	/// </summary>
	Enum
}
#endregion

#region Delegates

/// <summary>
///
/// </summary>
/// <param name="context"></param>
/// <returns></returns>
public delegate Task<IOGraphResult> OGraphEdgeHandler(IOGraphEdgeContext context, CancellationToken cancellationToken = default);



/// <summary>
///
/// </summary>
/// <param name="context"></param>
/// <param name="next"></param>
/// <returns></returns>
public delegate Task<IOGraphResult> OGraphEdgeMiddleware(IOGraphEdgeContext context, OGraphEdgeHandler next);



/// <summary>
///
/// </summary>
/// <param name="context"></param>
/// <returns></returns>
public delegate Task<IOGraphResult> OGraphEdgeResolver(IOGraphEdgeContext context, CancellationToken cancellationToken = default);


/// <summary>
/// A wrapper delegate for executing operation middleware and resolver.
/// </summary>
/// <param name="context"></param>
/// <returns></returns>
public delegate Task<IOGraphResult> OGraphOperationHandler(IOGraphOperationContext context, CancellationToken cancellationToken = default);


/// <summary>
///
/// </summary>
/// <param name="context"></param>
/// <param name="next"></param>
/// <returns></returns>
public delegate Task<IOGraphResult> OGraphOperationMiddleware(IOGraphOperationContext context, OGraphOperationHandler next);


/// <summary>
///
/// </summary>
/// <param name="context"></param>
/// <returns></returns>
public delegate Task<IOGraphResult> OGraphOperationResolver(IOGraphOperationContext context, CancellationToken cancellationToken = default);



/// <summary>
///
/// </summary>
/// <param name="context"></param>
/// <returns></returns>
public delegate ValueTask<IOGraphResult> OGraphPropertyHandler(IOGraphPropertyContext context, CancellationToken cancellationToken = default);


/// <summary>
///
/// </summary>
/// <param name="context"></param>
/// <param name="next"></param>
/// <returns></returns>
public delegate ValueTask<IOGraphResult> OGraphPropertyMiddleware(IOGraphPropertyContext context, OGraphPropertyHandler next);



/// <summary>
///
/// </summary>
/// <param name="context"></param>
/// <returns></returns>
public delegate ValueTask<IOGraphResult> OGraphPropertyResolver(IOGraphPropertyContext context, CancellationToken cancellationToken = default);
#endregion

#region Abstractions
/// <summary>
/// The query options to use on execution.
/// </summary>
public abstract class OGraphQueryOptions
{
	/// <summary>
	/// Enables or disables sorting. Default is true;
	/// </summary>
	public bool CanSort { get; set; } = true;
	/// <summary>
	/// Enables or disables filtering. Default is true.
	/// </summary>
	public bool CanFilter { get; set; } = true;
	/// <summary>
	/// Enables or disables paging. Default is true.
	/// </summary>
	public bool CanPage { get; set; } = true;
	/// <summary>
	/// Enables or disables projections. Default is true.
	/// </summary>
	public bool CanProject { get; set; } = true;
	/// <summary>
	/// The max amount of nodes a user is allowed to retrieve.
	/// </summary>
	public int? MaxPageSize { get; set; }
	/// <summary>
	/// Sets the default page size on a query if none is provided.
	/// </summary>
	public int? DefaultPageSize { get; set; } = 100;
	// TODO: Need to evaluate this option. Sometimes for discovery it is nice to see what is returned in a query.
	/// <summary>
	/// Specifies whether to select all properties of no projections are supplied.
	/// </summary>
	//public bool DefaultProjectAll { get; set; }
	/// <summary>
	/// 
	/// </summary>
	public static OGraphQueryOptions Default => new DefaultOptions();


	private partial class DefaultOptions : OGraphQueryOptions { }
}
/// <summary>
/// Represents a single graph Model.
/// </summary>
public interface IOGraph
{
	/// <summary>
	/// The name of the graph model.
	/// </summary>
	Name Name { get; }
	/// <summary>
	/// A collection of node definitions within the OGraph Model.
	/// </summary>
	IOGraphNodeCollection Nodes { get; }
	/// <summary>
	/// Gets the edge collection.
	/// </summary>
	IOGraphEdgeCollection Edges { get; }
	/// <summary>
	/// Represents a collection of HTTP Operations
	/// </summary>
	IOGraphOperationCollection Operations { get; }
}


/// <summary>
/// A fluent builder for creating a <see cref="IOGraph"/> model.
/// </summary>
public interface IOGraphBuilder
{
	/// <summary>
	/// Adds a node to the model.
	/// </summary>
	/// <param label="node"></param>
	/// <returns></returns>
	IOGraphBuilder AddNode(IOGraphNode node);
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="TNode"></typeparam>
	/// <returns></returns>
	IOGraphBuilder AddNode<TNode>() where TNode : IOGraphNode, new();
	/// <summary>
	///
	/// </summary>
	/// <param name="edge"></param>
	/// <returns></returns>
	IOGraphBuilder AddEdge(IOGraphEdge edge);
	/// <summary>
	///
	/// </summary>
	/// <param name="configure"></param>
	/// <returns></returns>
	IOGraphBuilder AddEdge(Func<IOGraph, IOGraphEdge> configure);
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="TEdge"></typeparam>
	/// <returns></returns>
	IOGraphBuilder AddEdge<TEdge>() where TEdge : IOGraphEdge, new();
	/// <summary>
	/// Add a raw operation to the graph model.
	/// </summary>
	/// <param name="operation"></param>
	/// <returns></returns>
	IOGraphBuilder AddOperation(IOGraphOperation operation);
	/// <summary>
	/// Add a raw operation to the graph model.
	/// </summary>
	/// <param name="configure"></param>
	/// <returns></returns>
	IOGraphBuilder AddOperation(Func<IOGraph, IOGraphOperation> configure);
	/// <summary>
	/// Add a raw operation to the graph model.
	/// </summary>
	/// <typeparam name="TOperation"></typeparam>
	/// <returns></returns>
	IOGraphBuilder AddOperation<TOperation>() where TOperation : IOGraphOperation, new();
	/// <summary>
	/// Builds the graph model.
	/// </summary>
	/// <returns>OGraph Model</returns>
	IOGraph Build();
	/// <summary>
	///
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	IOGraphEdgeDescriptor AddEdge(Name name);
	/// <summary>
	///
	/// </summary>
	/// <param name="label"></param>
	/// <returns></returns>
	IOGraphNodeDescriptor AddNode(Name label);
	/// <summary>
	/// Adds a query operation.
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	IOGraphQueryOperationDescriptor AddQuery(Name name);
	/// <summary>
	/// Adds a command operation
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	IOGraphCommandOperationDescriptor AddCommand(Name name);

}


/// <summary>
/// An edge links two nodes together.
/// </summary>
/// <remarks>
/// An edge is also referred to as a Link.
/// </remarks>
public interface IOGraphEdge
{
	/// <summary>
	/// The name of the edge.
	/// </summary>
	Name Label { get; }
	/// <summary>
	/// The source node.
	/// </summary>
	IOGraphNode Source { get; }
	/// <summary>
	/// The target is the node in which is linked to the source.
	/// </summary>
	IOGraphNode Target { get; }
	/// <summary>
	/// Metadata for the edge.
	/// </summary>
	IOGraphMetadata Metadata { get; }
	/// <summary>
	/// The edge resolver.
	/// </summary>
	IOGraphEdgeResolver Resolver { get; }
	/// <summary>
	/// A collection of middleware that will be executed before the edge is resolved.
	/// </summary>
	IOGraphEdgeMiddlewareQueue Middleware { get; }
	/// <summary>
	/// Gets the OGraph query provider.
	/// </summary>
	IOGraphQueryProvider QueryProvider { get; }
	/// <summary>
	/// Gets the OGraph query options.
	/// </summary>
	OGraphQueryOptions QueryOptions { get; }
	/// <summary>
	/// Executes the edge.
	/// </summary>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<IOGraphResult> ExecuteAsync(IOGraphEdgeContext context, CancellationToken cancellationToken = default);
}


public interface IOGraphEdgeCollection : ICollection<IOGraphEdge>
{
	/// <summary>
	///
	/// </summary>
	/// <param name="label"></param>
	/// <param name="edge"></param>
	/// <returns></returns>
	bool TryGetEdge(Name label, out IOGraphEdge? edge);
}



/// <summary>
///
/// </summary>
public interface IOGraphEdgeContext
{
	/// <summary>
	/// Get's the OGraph Model.
	/// </summary>
	/// <returns></returns>
	IOGraph GetGraph();
	/// <summary>
	/// Gets edge currently being executed.
	/// </summary>
	/// <returns></returns>
	IOGraphEdge GetEdge();
	/// <summary>
	/// Gets the edge's target type.
	/// </summary>
	/// <returns><see cref="IOGraphEdge.Target"/></returns>
	IOGraphType GetEdgeTarget();
	/// <summary>
	/// Gets the edge's source type.
	/// </summary>
	/// <returns><see cref="IOGraphEdge.Source"/></returns>
	IOGraphType GetEdgeSource();
	/// <summary>
	/// Gets the parsed HTTP request query.
	/// </summary>
	/// <returns></returns>
	QueryDocument GetQuery();
	/// <summary>
	/// Gets the OGraph query options
	/// </summary>
	/// <returns></returns>
	OGraphQueryOptions GetQueryOptions();
	/// <summary>
	/// Gets the query provider for the Edge being executed.
	/// </summary>
	/// <returns></returns>
	IOGraphQueryProvider GetQueryProvider();
	/// <summary>
	/// Gets the Parent object in which the edge is being executed for. This represents the relationship
	/// between <typeparamref name="T"/> and the edge type being returned.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	T GetParent<T>();
	/// <summary>
	/// Gets a service from the <see cref="IServiceProvider"/>.
	/// </summary>
	/// <typeparam name="T">The service to return.</typeparam>
	/// <returns></returns>
	T? GetService<T>();
	/// <summary>
	/// Gets the current authenticated user or application if aailable.
	/// </summary>
	/// <returns></returns>
	ClaimsPrincipal GetClaimsPrincipal();
	/// <summary>
	/// Returns the service provider if available.
	/// </summary>
	IServiceProvider? ServiceProvider { get; }
	/// <summary>
	/// The incoming HTTP request.
	/// </summary>
	public IOGraphExecutorRequest Request { get; }
	/// <summary>
	/// The outgoing HTTP response.
	/// </summary>
	public IOGraphExecutorResponse Response { get; }
}



/// <summary>
/// A raw descriptor for defining an edge.
/// </summary>
public interface IOGraphEdgeDescriptor
{
	/// <summary>
	///
	/// </summary>
	/// <param name="label">The name of the node within the OGraph Model.</param>
	/// <returns></returns>
	IOGraphEdgeDescriptor UseTargetNode(Name name);
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="TNode"></typeparam>
	/// <returns></returns>
	IOGraphEdgeDescriptor UseTargetNode<TNode>() where TNode : IOGraphNode, new();
	/// <summary>
	///
	/// </summary>
	/// <param name="label"></param>
	/// <returns></returns>
	IOGraphEdgeDescriptor UseSourceNode(Name name);
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="TNode"></typeparam>
	/// <returns></returns>
	IOGraphEdgeDescriptor UseSourceNode<TNode>() where TNode : IOGraphNode, new();
	/// <summary>
	///
	/// </summary>
	/// <param name="key"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	IOGraphEdgeDescriptor UseMetadata(string key, object value);
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="TQueryProvider"></typeparam>
	/// <returns></returns>
	IOGraphEdgeDescriptor UseQueryProvider<TQueryProvider>() where TQueryProvider : IOGraphQueryProvider, new();
	/// <summary>
	///
	/// </summary>
	/// <param name="queryProvider"></param>
	/// <returns></returns>
	IOGraphEdgeDescriptor UseQueryProvider(IOGraphQueryProvider queryProvider);
	/// <summary>
	///
	/// </summary>
	/// <param name="options"></param>
	/// <returns>The current descriptor.</returns>
	IOGraphEdgeDescriptor UseQueryOptions(OGraphQueryOptions options);
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="TQueryOptions"></typeparam>
	/// <param name="configure"></param>
	/// <returns>The current descriptor.</returns>
	IOGraphEdgeDescriptor UseQueryOptions<TQueryOptions>(Action<TQueryOptions> configure) where TQueryOptions : OGraphQueryOptions, new();
	/// <summary>
	///
	/// </summary>
	/// <param name="configure"></param>
	/// <returns>The current descriptor.</returns>
	IOGraphEdgeDescriptor UseQueryOptions(Action<OGraphQueryOptions> configure);
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="TMiddleware"></typeparam>
	/// <returns></returns>
	IOGraphEdgeDescriptor UseMiddleware<TMiddleware>() where TMiddleware : IOGraphEdgeMiddleware, new();
	/// <summary>
	///
	/// </summary>
	/// <param name="middleware"></param>
	/// <returns></returns>
	IOGraphEdgeDescriptor UseMiddleware(IOGraphEdgeMiddleware middleware);
	/// <summary>
	///
	/// </summary>
	/// <param name="middleware"></param>
	/// <returns></returns>
	IOGraphEdgeDescriptor UseMiddleware(OGraphEdgeMiddleware middleware);
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="TResolver"></typeparam>
	/// <returns></returns>
	IOGraphEdgeDescriptor UseResolver<TResolver>() where TResolver : IOGraphEdgeResolver, new();
	/// <summary>
	///
	/// </summary>
	/// <param name="resolver"></param>
	/// <returns></returns>
	IOGraphEdgeDescriptor UseResolver(IOGraphEdgeResolver resolver);
	/// <summary>
	///
	/// </summary>
	/// <param name="resolver"></param>
	/// <returns></returns>
	IOGraphEdgeDescriptor UseResolver(OGraphEdgeResolver resolver);
}


/// <summary>
///
/// </summary>
public interface IOGraphEdgeMiddleware
{
	/// <summary>
	///
	/// </summary>
	/// <param name="context"></param>
	/// <param name="next"></param>
	/// <returns></returns>
	Task<IOGraphResult> InvokeAsync(IOGraphEdgeContext context, OGraphEdgeHandler next);
}



public interface IOGraphEdgeMiddlewareQueue : IEnumerable<IOGraphEdgeMiddleware>
{
	/// <summary>
	///
	/// </summary>
	int Count { get; }
	/// <summary>
	///
	/// </summary>
	bool IsReadOnly { get; }
	/// <summary>
	///
	/// </summary>
	/// <param name="middleware"></param>
	void Enqueue(IOGraphEdgeMiddleware middleware);
	/// <summary>
	///
	/// </summary>
	/// <param name="middleware"></param>
	void Dequeue(IOGraphEdgeMiddleware middleware);
	/// <summary>
	///
	/// </summary>
	/// <param name="resolver"></param>
	/// <returns></returns>
	OGraphEdgeHandler BuildHandlerChain(IOGraphEdgeResolver resolver);
}



/// <summary>
///
/// </summary>
public interface IOGraphEdgeResolver
{
	/// <summary>
	///
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	Task<IOGraphResult> InvokeAsync(IOGraphEdgeContext context, CancellationToken cancellationToken = default);
}



/// <summary>
///
/// </summary>
public interface IOGraphError
{
	/// <summary>
	/// The unique error code.
	/// </summary>
	string? Code { get; }
	/// <summary>
	/// The error message.
	/// </summary>
	string? Message { get; }
	/// <summary>
	///
	/// </summary>
	IOGraphErrorDetailsCollection? Details { get; }
}



public interface IOGraphErrorDetails
{
}



public interface IOGraphErrorDetailsCollection : IEnumerable<IOGraphError>
{
}



/// <summary>
///
/// </summary>
public interface IOGraphExecutor
{
	/// <summary>
	/// Executes the OGrpah request and returns a response to be sent back to the client.
	/// </summary>
	/// <param name="request"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<IOGraphExecutorResponse> ExecuteAsync(IOGraphExecutorRequest request, CancellationToken cancellationToken = default);
}


/// <summary>
///
/// </summary>
public interface IOGraphExecutorBuilder
{
	/// <summary>
	///
	/// </summary>
	/// <param name="configure"></param>
	/// <returns></returns>
	IOGraphExecutorBuilder AddGraph(Action<IOGraphBuilder> configure);
	/// <summary>
	///
	/// </summary>
	/// <param name="options"></param>
	/// <returns></returns>
	IOGraphExecutorBuilder AddOptions(Action<OGraphOptions> options);

	/// <summary>
	/// Builds the executor.
	/// </summary>
	/// <returns></returns>
	IOGraphExecutor Build();
}

public sealed class OGraphOptions
{
	public OGraphOptions()
	{

	}
	/// <summary>
	/// Specify the prefix to be used on all operation routes.
	/// </summary>
	public string? RoutePrefix { get; set; }
	/// <summary>
	/// 
	/// </summary>
	public string DefaultMediaType { get; set; } //= OGraphMediaType.Json;
	/// <summary>
	/// Specifies the <see cref="QueryParser"/> options.
	/// </summary>
	//public QueryParserOptions ParserOptions { get; set; } = new();
	/// <summary>
	/// 
	/// </summary>
	public IServiceProvider? ServiceProvider { get; set; }
	/// <summary>
	/// 
	/// </summary>
	public JsonSerializerOptions JsonSerializerOptions { get; set; } = new()
	{
		PropertyNameCaseInsensitive = true,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
	};
}

/// <summary>
///
/// </summary>
public interface IOGraphExecutorContext
{
	/// <summary>
	/// The authenticated user or application.
	/// </summary>
	ClaimsPrincipal? ClaimsPrincipal { get; }
	/// <summary>
	/// The incoming HTTP request
	/// </summary>
	public IOGraphExecutorRequest Request { get; }
	/// <summary>
	/// The outgoing HTTP response.
	/// </summary>
	public IOGraphExecutorResponse Response { get; }
}



/// <summary>
/// The incomin HTTP request.
/// </summary>
public interface IOGraphExecutorRequest
{
	/// <summary>
	/// The hose of the request.
	/// </summary>
	Host Host { get; }
	/// <summary>
	/// The request path
	/// </summary>
	Path Path { get; }
	/// <summary>
	/// The HTTP method of the request.
	/// </summary>
	Method Method { get; }
	/// <summary>
	/// The request query parameters.
	/// </summary>
	IOGraphQueryParamCollection Query { get; }
	/// <summary>
	/// The request headers.
	/// </summary>
	IOGraphHeaderCollection Headers { get; }
	/// <summary>
	/// The raw request body.
	/// </summary>
	Stream Body { get; }
}



/// <summary>
/// Represents an HTTP Response.
/// </summary>
public interface IOGraphExecutorResponse
{
	/// <summary>
	///
	/// </summary>
	StatusCode StatusCode { get; set; }
	/// <summary>
	/// A collection of headers to be returned with the response.
	/// </summary>
	IOGraphHeaderCollection Headers { get; }
	/// <summary>
	/// The response body to be written back to the client.
	/// </summary>
	Stream Body { get; }
}



/// <summary>
/// A collection of HTTP headers.
/// </summary>
public interface IOGraphHeaderCollection : IDictionary<string, HeaderValue>
{
	HeaderValue? Host { get; }
	HeaderValue? Accept { get; }
	HeaderValue? ContentType { get; }
	HeaderValue? ContentLength { get; }
}



/// <summary>
///
/// </summary>
public interface IOGraphMetadata : IReadOnlyDictionary<string, object>
{
	/// <summary>
	/// Checks whether the value is valid for serialization.
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	bool IsValid(object value);
}



/// <summary>
/// Represents a single entity and it's structure within the graph Model.
/// </summary>
/// <remarks>
/// A Node is also referred to as a Vertex.
/// </remarks>
public interface IOGraphNode
{
	/// <summary>
	/// Represents the label each node should contain.
	/// </summary>
	Name Label { get; }
	/// <summary>
	/// Represents arbitrary metadata that can be associated with this node.
	/// </summary>
	IOGraphMetadata Metadata { get; }
	/// <summary>
	/// The type being bound to this node.
	/// </summary>
	IOGraphType Type { get; }
	/// <summary>
	/// A collection of edges that are connected to this node.
	/// </summary>
	IOGraphEdgeCollection Edges { get; }
	/// <summary>
	/// A collection of operations bound to this node.
	/// </summary>
	IOGraphOperationCollection Operations { get; }
}


public interface IOGraphNodeCollection : ICollection<IOGraphNode>
{
	/// <summary>
	///
	/// </summary>
	/// <param name="label"></param>
	/// <returns></returns>
	bool TryFind(Name name);
	/// <summary>
	///
	/// </summary>
	/// <param name="Label"></param>
	/// <param name="node"></param>
	/// <returns></returns>
	bool TryGetNode(Name name, out IOGraphNode? node);
}



/// <summary>
///
/// </summary>
public interface IOGraphNodeDescriptor
{
	/// <summary>
	/// Sets the label of the node
	/// </summary>
	/// <param name="label"></param>
	/// <returns>The current descriptor.</returns>
	IOGraphNodeDescriptor UseLabel(Name name);
	/// <summary>
	///
	/// </summary>
	/// <param name="key"></param>
	/// <param name="value"></param>
	/// <returns>The current descriptor.</returns>
	IOGraphNodeDescriptor UseMetadata(string key, object value);
	/// <summary>
	///
	/// </summary>
	/// <param name="type"></param>
	/// <returns>The current descriptor.</returns>
	/// <remarks>
	/// The type being binded to the node should be a complex type.
	/// </remarks>
	IOGraphNodeDescriptor UseType(IOGraphType type);
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="TType"></typeparam>
	/// <returns></returns>
	IOGraphNodeDescriptor UseType<TType>() where TType : IOGraphType, new();
	/// <summary>
	/// Configures a complex type to be binded to the node.
	/// </summary>
	/// <param name="configure"></param>
	/// <returns></returns>
	IOGraphNodeDescriptor UseType(Action<IOGraphComplexTypeDescriptor> configure);
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="configure"></param>
	/// <returns></returns>
	IOGraphNodeDescriptor UseType<T>(Action<IOGraphComplexTypeDescriptor<T>> configure);
	/// <summary>
	///
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	IOGraphEdgeDescriptor AddEdge(Name name);

	IOGraphNodeDescriptor AddEdge<TEdge>() where TEdge : IOGraphEdge, new();

	IOGraphNodeDescriptor AddEdge(IOGraphEdge edge);

	IOGraphQueryOperationDescriptor AddQuery(Name operationName);
	IOGraphCommandOperationDescriptor AddCommand(Name operationName);


}





public interface IOGraphCommandOperation : IOGraphOperation
{

}


/// <summary>
/// Represents a single HTTP REST operation.
/// </summary>
/// <remarks>
/// An OGraph Operation represent the root
/// Operation -- resolves --> Root(s) -- resolves --> Identifier(s) -- resolves --> Root(s) -- resolves --> Operation(s)
/// </remarks>
public interface IOGraphOperation
{
	/// <summary>
	/// The name of the command.
	/// </summary>
	Name Name { get; }
	/// <summary>
	/// The route associated with this operation.
	/// </summary>
	Route Route { get; }
	/// <summary>
	/// The HTTP method.
	/// </summary>
	Method Method { get; }
	/// <summary>
	/// Specifies whether the operation is enabled.
	/// </summary>
	bool IsEnabled { get; }
	/// <summary>
	/// Specifies whether the operation is a command or query.
	/// </summary>
	OperationType OperationType { get; }
	/// <summary>
	/// Represents the node that is bound to this operation.
	/// </summary>
	IOGraphNode Node { get; }
	/// <summary>
	/// The resolver for the operation.
	/// </summary>
	IOGraphOperationResolver Resolver { get; }
	/// <summary>
	/// A first-in-first-out queue of middleware that will execute
	/// </summary>
	IOGraphOperationMiddlewareQueue Middleware { get; }
	/// <summary>
	/// The metadata for the operation.
	/// </summary>
	IOGraphMetadata Metadata { get; }
	/// <summary>
	///
	/// </summary>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<IOGraphResult> ExecuteAsync(IOGraphOperationContext context, CancellationToken cancellationToken = default);
}


public interface IOGraphQueryOperation : IOGraphOperation
{
	/// <summary>
	/// Gets the Query provider.
	/// </summary>
	IOGraphQueryProvider QueryProvider { get; }
	/// <summary>
	/// Gets the query options to be used for the query provider.
	/// </summary>
	OGraphQueryOptions QueryOptions { get; }

}


public interface IOGraphOperationCollection : ICollection<IOGraphOperation>
{

	bool TryGetOperation(Name name, out IOGraphOperation operation);
}



/// <summary>
///
/// </summary>
public interface IOGraphOperationContext
{
	/// <summary>
	/// Get's the OGraph Model.
	/// </summary>
	/// <returns><see cref="IOGraph"/></returns>
	IOGraph GetGraph();
	/// <summary>
	/// Get's the binded node for the given operation being executed.
	/// </summary>
	/// <returns></returns>
	IOGraphOperation GetOperation();
	/// <summary>
	/// Get's the HTTP request query.
	/// </summary>
	/// <returns></returns>
	QueryDocument GetQuery();
	/// <summary>
	/// Get's the OGraph query options
	/// </summary>
	/// <returns></returns>
	OGraphQueryOptions GetQueryOptions();
	/// <summary>
	///
	/// </summary>
	/// <returns></returns>
	IOGraphQueryProvider GetQueryProvider();
	/// <summary>
	/// Gets a service from the <see cref="IServiceProvider"/>.
	/// </summary>
	/// <typeparam name="T">The service to return.</typeparam>
	/// <returns></returns>
	T? GetService<T>();
	/// <summary>
	/// Gets the current authenticatted user if available.
	/// </summary>
	/// <returns></returns>
	ClaimsPrincipal GetClaimsPrincipal();
	/// <summary>
	/// Returns the service provider if available.
	/// </summary>
	IServiceProvider? ServiceProvider { get; }
	/// <summary>
	/// The incoming HTTP request.
	/// </summary>
	public IOGraphExecutorRequest Request { get; }
	/// <summary>
	/// The outgoing HTTP response.
	/// </summary>
	public IOGraphExecutorResponse Response { get; }
}


/// <summary>
///
/// </summary>
public interface IOGraphCommandOperationDescriptor
{
	/// <summary>
	/// Sets the name of the operation
	/// </summary>
	/// <param name="name">A string name.</param>
	/// <returns>The current descriptor.</returns>
	IOGraphCommandOperationDescriptor UseName(Name name);
	/// <summary>
	/// Sets the route to use for the operation.
	/// </summary>
	/// <param name="route">The route value.</param>
	/// <returns>The current descriptor.</returns>
	IOGraphCommandOperationDescriptor UseRoute(Route route);
	/// <summary>
	/// Sets the HTTP method for the operaiton.
	/// </summary>
	/// <param name="method">The HTTP Method</param>
	/// <returns>The current descriptor.</returns>
	IOGraphCommandOperationDescriptor UseMethod(Method method);
	/// <summary>
	/// Sets and exposes a generic query parameter.
	/// </summary>
	/// <param name="query"></param>
	/// <returns>The current descriptor.</returns>
	IOGraphCommandOperationDescriptor UseQueryParam(string paramKey);
	/// <summary>
	/// Binds a node to the operation.
	/// </summary>
	/// <remarks></remarks>
	/// <param name="label"></param>
	/// <returns>The current descriptor.</returns>
	IOGraphCommandOperationDescriptor UseNode(Name name);
	/// <summary>
	/// Binds a node to the operation.
	/// </summary>
	/// <typeparam name="TNode"></typeparam>
	/// <returns>The current descriptor.</returns>
	IOGraphCommandOperationDescriptor UseNode<TNode>() where TNode : IOGraphNode, new();

	/// <summary>
	///
	/// </summary>
	/// <typeparam name="TMiddleware"></typeparam>
	/// <returns>The current descriptor.</returns>
	IOGraphCommandOperationDescriptor UseMiddleware<TMiddleware>() where TMiddleware : IOGraphOperationMiddleware, new();
	/// <summary>
	///
	/// </summary>
	/// <param name="middleware"></param>
	/// <returns>The current descriptor.</returns>
	IOGraphCommandOperationDescriptor UseMiddleware(IOGraphOperationMiddleware middleware);
	/// <summary>
	///
	/// </summary>
	/// <param name="middleware"></param>
	/// <returns>The current descriptor.</returns>
	IOGraphCommandOperationDescriptor UseMiddleware(OGraphOperationMiddleware middleware);
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="TResolver"></typeparam>
	/// <returns>The current descriptor.</returns>
	IOGraphCommandOperationDescriptor UseResolver<TResolver>() where TResolver : IOGraphOperationResolver, new();
	/// <summary>
	///
	/// </summary>
	/// <param name="resolver"></param>
	/// <returns>The current descriptor.</returns>
	IOGraphCommandOperationDescriptor UseResolver(IOGraphOperationResolver resolver);
	/// <summary>
	///
	/// </summary>
	/// <param name="resolver"></param>
	/// <returns>The current descriptor.</returns>
	IOGraphCommandOperationDescriptor UseResolver(OGraphOperationResolver resolver);
}


public interface IOGraphQueryOperationDescriptor
{
	/// <summary>
	/// Sets the name of the operation
	/// </summary>
	/// <param name="name">A string name.</param>
	/// <returns>The current descriptor.</returns>
	IOGraphQueryOperationDescriptor UseName(Name name);
	/// <summary>
	/// Sets the route to use for the operation.
	/// </summary>
	/// <param name="route">The route value.</param>
	/// <returns>The current descriptor.</returns>
	IOGraphQueryOperationDescriptor UseRoute(Route route);
	/// <summary>
	/// Sets and exposes a generic query parameter.
	/// </summary>
	/// <param name="query"></param>
	/// <returns>The current descriptor.</returns>
	IOGraphQueryOperationDescriptor UseQueryParam(string paramKey);
	/// <summary>
	/// Binds a node to the operation.
	/// </summary>
	/// <remarks></remarks>
	/// <param name="label"></param>
	/// <returns>The current descriptor.</returns>
	IOGraphQueryOperationDescriptor UseNode(Name name);
	/// <summary>
	/// Binds a node to the operation.
	/// </summary>
	/// <typeparam name="TNode"></typeparam>
	/// <returns>The current descriptor.</returns>
	IOGraphQueryOperationDescriptor UseNode<TNode>() where TNode : IOGraphNode, new();
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="TMiddleware"></typeparam>
	/// <returns>The current descriptor.</returns>
	IOGraphQueryOperationDescriptor UseMiddleware<TMiddleware>() where TMiddleware : IOGraphOperationMiddleware, new();
	/// <summary>
	///
	/// </summary>
	/// <param name="middleware"></param>
	/// <returns>The current descriptor.</returns>
	IOGraphQueryOperationDescriptor UseMiddleware(IOGraphOperationMiddleware middleware);
	/// <summary>
	///
	/// </summary>
	/// <param name="middleware"></param>
	/// <returns>The current descriptor.</returns>
	IOGraphQueryOperationDescriptor UseMiddleware(OGraphOperationMiddleware middleware);
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="TResolver"></typeparam>
	/// <returns>The current descriptor.</returns>
	IOGraphQueryOperationDescriptor UseResolver<TResolver>() where TResolver : IOGraphOperationResolver, new();
	/// <summary>
	///
	/// </summary>
	/// <param name="resolver"></param>
	/// <returns>The current descriptor.</returns>
	IOGraphQueryOperationDescriptor UseResolver(IOGraphOperationResolver resolver);
	/// <summary>
	///
	/// </summary>
	/// <param name="resolver"></param>
	/// <returns>The current descriptor.</returns>
	IOGraphQueryOperationDescriptor UseResolver(OGraphOperationResolver resolver);
	/// <summary>
	/// Overrides the default query provider.
	/// </summary>
	/// <typeparam name="TQueryProvider"></typeparam>
	/// <returns>The current descriptor.</returns>
	IOGraphQueryOperationDescriptor UseQueryProvider<TQueryProvider>() where TQueryProvider : IOGraphQueryProvider, new();
	/// <summary>
	/// Overrides the default query provider.
	/// </summary>
	/// <param name="queryProvider"></param>
	/// <returns>The current descriptor.</returns>
	IOGraphQueryOperationDescriptor UseQueryProvider(IOGraphQueryProvider queryProvider);
	/// <summary>
	///
	/// </summary>
	/// <param name="options"></param>
	/// <returns>The current descriptor.</returns>
	IOGraphQueryOperationDescriptor UseQueryOptions(OGraphQueryOptions options);
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="TQueryOptions"></typeparam>
	/// <param name="configure"></param>
	/// <returns>The current descriptor.</returns>
	IOGraphQueryOperationDescriptor UseQueryOptions<TQueryOptions>(Action<TQueryOptions> configure) where TQueryOptions : OGraphQueryOptions, new();
	/// <summary>
	///
	/// </summary>
	/// <param name="configure"></param>
	/// <returns>The current descriptor.</returns>
	IOGraphQueryOperationDescriptor UseQueryOptions(Action<OGraphQueryOptions> configure);
}



/// <summary>
///
/// </summary>
public interface IOGraphOperationMiddleware
{
	/// <summary>
	///
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	Task<IOGraphResult> InvokeAsync(IOGraphOperationContext context, OGraphOperationHandler next);
}


public interface IOGraphOperationMiddlewareQueue : IEnumerable<IOGraphOperationMiddleware>
{
	/// <summary>
	///
	/// </summary>
	int Count { get; }
	/// <summary>
	///
	/// </summary>
	bool IsReadOnly { get; }
	/// <summary>
	///
	/// </summary>
	/// <param name="middleware"></param>
	void Enqueue(IOGraphOperationMiddleware middleware);
	/// <summary>
	///
	/// </summary>
	/// <param name="middleware"></param>
	void Dequeue(IOGraphOperationMiddleware middleware);
	/// <summary>
	/// Builds a handler that create invocation chain to execute middleware and resolver.
	/// </summary>
	/// <returns></returns>
	OGraphOperationHandler BuildHandlerChain(IOGraphOperationResolver resolver);
}



/// <summary>
///
/// </summary>
public interface IOGraphOperationResolver
{
	/// <summary>
	///
	/// </summary>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<IOGraphResult> InvokeAsync(IOGraphOperationContext context, CancellationToken cancellationToken = default);
}



/// <summary>
/// Computed properties are non filterable or sortable properties that are executed
/// after entities are returned from the query provider;
/// </summary>
//bool IsComputed { get; } // TODO: May need to come up with a different convention for managing filterable and sortable properties.

/// <summary>
///
/// </summary>
public interface IOGraphProperty
{
	/// <summary>
	/// The name of the property.
	/// </summary>
	Name Name { get; }
	/// <summary>
	/// The OGraph Property Type.
	/// </summary>
	IOGraphType Type { get; }
	/// <summary>
	/// Metadata of the property.
	/// </summary>
	IOGraphMetadata Metadata { get; }
	/// <summary>
	///
	/// </summary>
	IOGraphPropertyResolver Resolver { get; }
	/// <summary>
	/// The collection of middleware to execute before the resolver.
	/// </summary>
	IOGraphPropertyMiddlewareQueue Middleware { get; }
	/// <summary>
	///
	/// </summary>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<IOGraphResult> ExecuteAsync(IOGraphPropertyContext context, CancellationToken cancellationToken = default);
}



public interface IOGraphPropertyCollection : ICollection<IOGraphProperty>
{
	/// <summary>
	///
	/// </summary>
	/// <param name="name"></param>
	/// <param name="property"></param>
	/// <returns></returns>
	bool TryGetProperty(Name name, out IOGraphProperty? property);
}



/// <summary>
/// The context for the resolver being executed for the property.
/// </summary>
public interface IOGraphPropertyContext
{
	/// <summary>
	/// Gets the property's declaring type.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	T GetParent<T>();
	/// <summary>
	///
	/// </summary>
	/// <returns></returns>
	IOGraphType GetPropertyType();
	/// <summary>
	/// Gets a service from the <see cref="IServiceProvider"/>.
	/// </summary>
	/// <typeparam name="T">The service to return.</typeparam>
	/// <returns></returns>
	T? GetService<T>();
	/// <summary>
	/// Gets the current authenticated user or application if aailable.
	/// </summary>
	/// <returns></returns>
	ClaimsPrincipal GetClaimsPrincipal();
	/// <summary>
	/// Returns the service provider if available.
	/// </summary>
	IServiceProvider? ServiceProvider { get; }
	/// <summary>
	/// The incoming HTTP request.
	/// </summary>
	public IOGraphExecutorRequest Request { get; }
	/// <summary>
	/// The outgoing HTTP response.
	/// </summary>
	public IOGraphExecutorResponse Response { get; }
}


public interface IOGraphPropertyDescriptor
{
	/// <summary>
	/// Overrides the name of the current property.
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	IOGraphPropertyDescriptor UseName(Name name);
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="TType"></typeparam>
	/// <returns></returns>
	IOGraphPropertyDescriptor UseType<TType>() where TType : IOGraphType, new();
	/// <summary>
	///
	/// </summary>
	/// <param name="configure"></param>
	/// <returns></returns>
	IOGraphPropertyDescriptor UseType(Action<IOGraphComplexTypeDescriptor> configure);
	/// <summary>
	///
	/// </summary>
	/// <param name="middleware"></param>
	/// <returns></returns>
	IOGraphPropertyDescriptor UseMiddleware(OGraphPropertyMiddleware middleware);
	/// <summary>
	///
	/// </summary>
	/// <param name="middleware"></param>
	/// <returns></returns>
	IOGraphPropertyDescriptor UseMiddleware(IOGraphPropertyMiddleware middleware);
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="TMiddleware"></typeparam>
	/// <returns></returns>
	IOGraphPropertyDescriptor UseMiddleware<TMiddleware>() where TMiddleware : IOGraphPropertyMiddleware, new();
	/// <summary>
	///
	/// </summary>
	/// <param name="key"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	IOGraphPropertyDescriptor UseMetadata(string key, object value);
	/// <summary>
	///
	/// </summary>
	/// <param name="resolver"></param>
	/// <returns></returns>
	IOGraphPropertyDescriptor UseResolver(IOGraphPropertyResolver resolver);
	/// <summary>
	///
	/// </summary>
	/// <param name="resolver"></param>
	/// <returns></returns>
	IOGraphPropertyDescriptor UseResolver(OGraphPropertyResolver resolver);
}





/// <summary>
///
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IOGraphPropertyDescriptor<T>
{
	/// <summary>
	/// Overrides the name of the current property.
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	IOGraphPropertyDescriptor<T> UseName(Name name);
	/// <summary>
	///
	/// </summary>
	/// <param name="key"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	IOGraphPropertyDescriptor<T> UseMetadata(string key, object value);
	/// <summary>
	///
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	IOGraphPropertyDescriptor<T> UseType(IOGraphType type);
	/// <summary>
	/// Overrides the OGraph type for the current property.
	/// </summary>
	/// <typeparam name="TType"></typeparam>
	/// <returns></returns>
	IOGraphPropertyDescriptor<T> UseType<TType>() where TType : IOGraphType, new();
	/// <summary>
	///
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	IOGraphPropertyDescriptor<T> UseType(Action<IOGraphComplexTypeDescriptor<T>> action);
	/// <summary>
	///
	/// </summary>
	/// <param name="middleware"></param>
	/// <returns></returns>
	IOGraphPropertyDescriptor<T> UseMiddleware(OGraphPropertyMiddleware middleware);
	/// <summary>
	///
	/// </summary>
	/// <param name="middleware"></param>
	/// <returns></returns>
	IOGraphPropertyDescriptor<T> UseMiddleware(IOGraphPropertyMiddleware middleware);
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="TMiddleware"></typeparam>
	/// <returns></returns>
	IOGraphPropertyDescriptor<T> UseMiddleware<TMiddleware>() where TMiddleware : IOGraphPropertyMiddleware, new();

	/// <summary>
	///
	/// </summary>
	/// <typeparam name="TResolver"></typeparam>
	/// <returns></returns>
	IOGraphPropertyDescriptor<T> UseResolver<TResolver>() where TResolver : IOGraphPropertyResolver, new();
	/// <summary>
	///
	/// </summary>
	/// <param name="resolver"></param>
	/// <returns></returns>
	IOGraphPropertyDescriptor<T> UseResolver(IOGraphPropertyResolver resolver);
	/// <summary>
	///
	/// </summary>
	/// <param name="resolver"></param>
	/// <returns></returns>
	IOGraphPropertyDescriptor<T> UseResolver(OGraphPropertyResolver resolver);
}


public interface IOGraphPropertyMiddleware
{
	/// <summary>
	///
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	ValueTask<IOGraphResult> InvokeAsync(IOGraphPropertyContext context, OGraphPropertyHandler next);
}



public interface IOGraphPropertyMiddlewareQueue : IEnumerable<IOGraphPropertyMiddleware>
{

	/// <summary>
	///
	/// </summary>
	int Count { get; }
	/// <summary>
	///
	/// </summary>
	bool IsReadOnly { get; }
	/// <summary>
	///
	/// </summary>
	/// <param name="middleware"></param>
	void Enqueue(IOGraphPropertyMiddleware middleware);
	/// <summary>
	///
	/// </summary>
	/// <param name="middleware"></param>
	void Dequeue(IOGraphPropertyMiddleware middleware);
	/// <summary>
	///
	/// </summary>
	/// <param name="resolver"></param>
	/// <returns></returns>
	OGraphPropertyHandler BuildHandlerChain(IOGraphPropertyResolver resolver);
}



/// <summary>
///
/// </summary>
public interface IOGraphPropertyResolver
{
	/// <summary>
	///
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	ValueTask<IOGraphResult> InvokeAsync(IOGraphPropertyContext context, CancellationToken cancellationToken = default);
}



/// <summary>
///
/// </summary>
public interface IOGraphQueryContext
{
	/// <summary>
	/// The starting node/vertex of the query.
	/// </summary>
	IOGraphNode Node { get; }
	/// <summary>
	/// The paresed query from the request.
	/// </summary>
	QueryDocument Query { get; }
	/// <summary>
	///
	/// </summary>
	IServiceProvider ServiceProvider { get; }
	/// <summary>
	///
	/// </summary>
	Stream Stream { get; }
}



/// <summary>
/// A collection of query parameters.
/// </summary>
public interface IOGraphQueryParamCollection : IDictionary<string, QueryValue>
{
}



/// <summary>
///
/// </summary>
public interface IOGraphQueryProvider
{
	/// <summary>
	/// Represents the runtime type
	/// </summary>
	Type ElementType { get; }

	/// <summary>
	///
	/// </summary>
	/// <param name="context"></param>
	/// <param name="options"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<IOGraphQueryResult> ExecuteAsync(IOGraphQueryContext context, OGraphQueryOptions options, CancellationToken cancellationToken = default);
}


/// <summary>
/// A result
/// </summary>
public interface IOGraphResult
{
	/// <summary>
	///
	/// </summary>
	StatusCode StatusCode { get; }
}



public interface IOGraphErrorResult : IOGraphResult
{
	IOGraphError Error { get; }
}



/// <summary>
///
/// </summary>
public interface IOGraphPropertyResult : IOGraphResult
{
	/// <summary>
	///
	/// </summary>
	object Value { get; }
}



/// <summary>
///
/// </summary>
public interface IOGraphQueryResult : IOGraphResult
{
	/// <summary>
	/// The query error the occurred.
	/// </summary>
	IOGraphError Error { get; }
	IOGraphNodeCollection Nodes { get; }
}





public interface IOGraphEntryCollection : IEnumerable<IOGraphNodeCo>
{

}
public interface IOGraphNodeCo : IEnumerable<IOGraphQueryResultNodeEntry>
{
	IOGraphQueryResultNodeEdges Edges { get; }
}

public interface IOGraphQueryResultNodeEntry
{
	Name Key { get; }
	object Value { get; }
}
public interface IOGraphQueryResultNodeEdges : IDictionary<Name, IOGraphQueryResult>
{

}


public class Test
{
	public void Traverse()
	{

	}
}


/// <summary>
/// Represents collection types such as Arrays, Lists, etc.,
/// </summary>
public interface IOGraphCollectionType : IOGraphType
{
	/// <summary>
	/// Represents the item type that is contained inside the collection.
	/// </summary>
	IOGraphType ItemType { get; }
}



/// <summary>
///
/// </summary>
public interface IOGraphComplexType : IOGraphType
{
	/// <summary>
	/// A collection of properties
	/// </summary>
	IOGraphPropertyCollection Properties { get; }


	//string ResolveRuntimePropertyName(string propertyName);
}



/// <summary>
/// Types represent primitive, complex, or collection structure that can be
/// used to define a property, inputs, and outputs of operations within the graph.
/// </summary>
/// <remarks>
/// An <see cref="IOGraphType"/> represents a
/// </remarks>
public interface IOGraphType
{
	/// <summary>
	/// The name of the type.
	/// </summary>
	Name Name { get; }
	/// <summary>
	/// The identifier of the type.
	/// </summary>
	TypeIdentifier Identifier { get; }
	/// <summary>
	/// The underlying .NET Type.
	/// </summary>
	/// <remarks>
	/// All types must have a RuntimeType, even if it is a custom type.
	/// </remarks>
	Type? RuntimeType { get; }
	/// <summary>
	///
	/// </summary>
	bool IsNullable { get; }
	/// <summary>
	/// Checks whether the <paramref name="value"/> is assignable to the type.
	/// </summary>
	/// <remarks>
	/// <i>This usually entails checking the value against the underlying runtime type.</i>
	/// </remarks>
	/// <param name="value"></param>
	/// <returns></returns>
	bool IsAssignable(object value);
}


public interface IOGraphEnumType : IOGraphType
{
	/// <summary>
	///
	/// </summary>
	public EnumValue[] Values { get; }
}

/// <summary>
///
/// </summary>
public struct EnumValue
{
	public EnumValue(string name, object value)
	{
		Name = name;
		Value = value;
	}
	/// <summary>
	///
	/// </summary>
	public string Name { get; }
	/// <summary>
	///
	/// </summary>
	public object Value { get; }
}


public interface IOGraphPrimitiveType : IOGraphType
{

}


/// <summary>
///
/// </summary>
public interface IOGraphTypeCollection : IEnumerable<IOGraphType>
{
	/// <summary>
	///
	/// </summary>
	int Count { get; }
	/// <summary>
	///
	/// </summary>
	bool IsReadOnly { get; }
	/// <summary>
	///
	/// </summary>
	/// <param name="type"></param>
	void Add(IOGraphType type);
	/// <summary>
	///
	/// </summary>
	/// <param name="type"></param>
	void Remove(IOGraphType type);
	/// <summary>
	///
	/// </summary>
	/// <param name="name"></param>
	/// <param name="type"></param>
	/// <returns></returns>
	bool TryGet(Name name, out IOGraphType type);
}


/// <summary>
///
/// </summary>
public interface IOGraphComplexTypeDescriptor
{
	/// <summary>
	///
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	IOGraphPropertyDescriptor HasProperty(Name name);
}



public interface IOGraphComplexTypeDescriptor<T>
{
	/// <summary>
	///
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	IOGraphComplexTypeDescriptor<T> Ignore(Name name);
	/// <summary>
	/// Specify a property to be ignored.
	/// </summary>
	/// <typeparam name="TProperty"></typeparam>
	/// <param name="expression"></param>
	/// <returns></returns>
	IOGraphComplexTypeDescriptor<T> Ignore<TProperty>(Expression<Func<T, TProperty>> expression);
	/// <summary>
	///
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	IOGraphPropertyDescriptor HasProperty(Name name);
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="TProperty"></typeparam>
	/// <param name="expression"></param>
	/// <returns></returns>
	IOGraphPropertyDescriptor<TProperty> HasProperty<TProperty>(Expression<Func<T, TProperty>> expression);
}

#endregion

