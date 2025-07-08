<Query Kind="Program">
  <Namespace>System.Buffers</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

void Main()
{
	"myNameIsChase".ConvertToPascalCase().Dump();
	"MyNameIsChase".ConvertToCamalCase().Dump();
	"MyNameIsChase".ConvertToKebabCase().Dump();
	"MyNameIsChase".ConvertToKebabCase(false).Dump();
	"MyNameIsChase".ConvertToSnakeCase().Dump();
	"MyNameIsChase".ConvertToSnakeCase(false).Dump();
}

public enum StringConversionStrategy
{
	/// <summary>
	/// No strategy is applied.
	/// </summary>
	None,
	/// <summary>
	/// Converts the given value to 
	/// </summary>
	OnlyAlphaNumeric,
}

public static class StringConversionExtensions
{
	/// <summary>
	/// Converts a string value to camal case.
	/// </summary>
	public static string ConvertToCamalCase(this string value, StringConversionStrategy strategy = StringConversionStrategy.None)
	{
		var chars = strategy.Equals(StringConversionStrategy.OnlyAlphaNumeric) ? 
			value.Where(c => char.IsLetter(c) || char.IsDigit(c)).ToArray() :
			value.ToCharArray();

		return string.Create(chars.Length, value, (span, value) =>
		{
			value.CopyTo(span);

			for (int i = 0; i < span.Length && (i != 1 || char.IsUpper(span[i])); i++)
			{
				var flag = i + 1 < span.Length;

				if (i > 0 && flag && !char.IsUpper(span[i + 1]))
				{
					if (span[i + 1] == ' ')
					{
						span[i] = char.ToLowerInvariant(span[i]);
					}
					break;
				}
				span[i] = char.ToLowerInvariant(span[i]);
			}
		});
	}

	/// <summary>
	/// Converts a string value to pascal case.
	/// </summary>
	public static string ConvertToPascalCase(this string value, StringConversionStrategy strategy = StringConversionStrategy.None)
	{
		var chars = strategy.Equals(StringConversionStrategy.OnlyAlphaNumeric) ?
			value.Where(c => char.IsLetter(c) || char.IsDigit(c)).ToArray() :
			value.ToCharArray();
			
		return string.Create(chars.Length, chars, (span, value) =>
		{
			value.CopyTo(span);

			for (int i = 0; i < span.Length && (i != 1 || char.IsLower(span[i])); i++)
			{
				bool flag = i + 1 < span.Length;
				if (i > 0 && flag && !char.IsLower(span[i + 1]))
				{
					if (span[i + 1] == ' ')
					{
						span[i] = char.ToUpperInvariant(span[i]);
					}
					break;
				}
				span[i] = char.ToUpperInvariant(span[i]);
			}
		});
	}

	/// <summary>
	/// Converts a string value to kebab casing.
	/// </summary>
	public static string ConvertToKebabCase(this string value, bool lowercase = true)
	{
		return ConvertStringValue('-', lowercase, value.AsSpan());
	}

	/// <summary>
	/// Converts a string to snake casing.
	/// </summary>
	public static string ConvertToSnakeCase(this string value, bool lowercase = true)
	{
		return ConvertStringValue('_', lowercase, value.AsSpan());
	}


	private enum SeparatorState
	{
		NotStarted,
		UppercaseLetter,
		LowercaseLetterOrDigit,
		SpaceSeparator
	}

	private static string ConvertStringValue(char separator, bool lowercase, ReadOnlySpan<char> chars)
	{
		char[] rentedBuffer = null;
		int num = (int)(1.2 * (double)chars.Length);
		Span<char> span = ((num > 128) ? ((Span<char>)(rentedBuffer = ArrayPool<char>.Shared.Rent(num))) : stackalloc char[128]);
		Span<char> destination2 = span;
		SeparatorState separatorState = SeparatorState.NotStarted;
		int charsWritten = 0;
		for (int i = 0; i < chars.Length; i++)
		{
			char c = chars[i];
			UnicodeCategory unicodeCategory = char.GetUnicodeCategory(c);
			switch (unicodeCategory)
			{
				case UnicodeCategory.UppercaseLetter:
					switch (separatorState)
					{
						case SeparatorState.LowercaseLetterOrDigit:
						case SeparatorState.SpaceSeparator:
							WriteChar(separator, ref destination2);
							break;
						case SeparatorState.UppercaseLetter:
							if (i + 1 < chars.Length && char.IsLower(chars[i + 1]))
							{
								WriteChar(separator, ref destination2);
							}
							break;
					}
					if (lowercase)
					{
						c = char.ToLowerInvariant(c);
					}
					WriteChar(c, ref destination2);
					separatorState = SeparatorState.UppercaseLetter;
					break;
				case UnicodeCategory.LowercaseLetter:
				case UnicodeCategory.DecimalDigitNumber:
					if (separatorState == SeparatorState.SpaceSeparator)
					{
						WriteChar(separator, ref destination2);
					}
					if (!lowercase && unicodeCategory == UnicodeCategory.LowercaseLetter)
					{
						c = char.ToUpperInvariant(c);
					}
					WriteChar(c, ref destination2);
					separatorState = SeparatorState.LowercaseLetterOrDigit;
					break;
				case UnicodeCategory.SpaceSeparator:
					if (separatorState != 0)
					{
						separatorState = SeparatorState.SpaceSeparator;
					}
					break;
				default:
					WriteChar(c, ref destination2);
					separatorState = SeparatorState.NotStarted;
					break;
			}
		}
		string result = destination2.Slice(0, charsWritten).ToString();
		if (rentedBuffer != null)
		{
			destination2.Slice(0, charsWritten).Clear();
			ArrayPool<char>.Shared.Return(rentedBuffer);
		}
		return result;
		void ExpandBuffer(ref Span<char> destination)
		{
			int minimumLength = checked(destination.Length * 2);
			char[] array = ArrayPool<char>.Shared.Rent(minimumLength);
			destination.CopyTo(array);
			if (rentedBuffer != null)
			{
				destination.Slice(0, charsWritten).Clear();
				ArrayPool<char>.Shared.Return(rentedBuffer);
			}
			rentedBuffer = array;
			destination = rentedBuffer;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void WriteChar(char value, ref Span<char> destination)
		{
			if (charsWritten == destination.Length)
			{
				ExpandBuffer(ref destination);
			}
			destination[charsWritten++] = value;
		}
	}

}