<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "xunit"

void Main()
{
	RunTests();  // Call RunTests() or press Alt+Shift+T to initiate testing.
}

public readonly struct Password : IEquatable<Password>
{
	private static readonly char[] characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+[]{}|;:,.<>?/`~".ToCharArray();
	private static readonly byte[] salt = new byte[16]
	{
		0xAA, 0x38, 0x82, 0x8B, 0x89, 0x18, 0x7B, 0x02, 0xB3, 0x08, 0x87, 0xD7, 0xE0, 0x60, 0x2F, 0x79
	};

	public Password(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			throw new ArgumentException("Password cannot be null or empty", nameof(value));
		}
		
		Value = IsHashed(value) ? value : Hash(value);
	}

	/// <summary>
	/// The Hash value of the password.
	/// </summary>
	public string Value { get; }

	#region Overloads

	/// <inheritdoc />
	public override bool Equals(object? obj)
	{
		if (obj is Password password)
		{
			return Equals(password);
		}
		return false;
	}

	/// <inheritdoc />
	public override int GetHashCode()
	{
		return Value.GetHashCode();
	}

	/// <inheritdoc />
	public override string ToString()
	{
		return Value;
	}
	#endregion

	#region Interface Implementation
	/// <inheritdoc />
	public bool Equals(Password other)
	{
		// We just need to compare the hash value
		return Value == other.Value;
	}
	#endregion

	#region Operators

	public static implicit operator Password(string value)
	{
		return new Password(value);
	}

	public static implicit operator string(Password password)
	{
		return password.Value;
	}

	public static bool operator ==(Password left, Password right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(Password left, Password right)
	{
		return !left.Equals(right);
	}

	#endregion

	#region Static Helpers
	/// <summary>
	/// Generates a random text password.
	/// </summary>
	/// <param name="length"></param>
	/// <returns></returns>
	public static Password New(out string value, int length = 16)
	{
		var random = new Random();

		value = string.Create(length, characters, (span, arg) =>
		{
			var count = characters.Length;

			for (int i = 0; i < length; i++)
			{
				var index = random.Next(0, count);

				span[i] = characters[index];
			}
		});
		
		return value;
	}

	/// <summary>
	/// 
	/// </summary>
	public static string Hash(string value)
	{
		using (var sha256 = SHA256.Create())
		{
			var password = Encoding.UTF8.GetBytes(value);
			var salted = new byte[password.Length + salt.Length];

			// Concatenate password and salt
			Buffer.BlockCopy(password, 0, salted, 0, password.Length);
			Buffer.BlockCopy(salt, 0, salted, password.Length, salt.Length);

			// Hash the concatenated password and salt
			var hashedBytes = sha256.ComputeHash(salted);

			// Concatenate the salt and hashed password for storage
			var hashedPasswordWithSalt = new byte[hashedBytes.Length + salt.Length];
			Buffer.BlockCopy(salt, 0, hashedPasswordWithSalt, 0, salt.Length);
			Buffer.BlockCopy(hashedBytes, 0, hashedPasswordWithSalt, salt.Length, hashedBytes.Length);

			return Convert.ToBase64String(hashedPasswordWithSalt);
		}
	}

	#endregion

	#region Private
	
	private bool IsHashed(string value) =>
		value.Length == 64 && (
			Regex.IsMatch(value, "[0-9a-fA-F]+") ||     // Check if the string contains only hexadecimal characters
			Regex.IsMatch(value, "[A-Za-z0-9+/=]+"));   // Check if the string is base64 encoded

	#endregion
}


#region private::Tests

[Fact] 
void Test_Xunit() 
{
	string text;
	
	var password1 = Password.New(out text);
	var password2 = new Password(text);
	
	Assert.Equal(password1, password2);
}

#endregion