<Query Kind="Program">
  <Namespace>System.Buffers</Namespace>
  <Namespace>System.Text.Json</Namespace>
</Query>

private const string query = @" test
	.filtER({
		firstName eq ""Chase"" and
		(
			startsWith(
				firstName, 'c', true
			) and 
			endsWith(
				lastName, 'e', true
			)
		) or 
		any(
			addresses,
			city eq 'Charlotte'
		)
	})
	.select({
		toLower(firstName) as firstName 
		toLower(lastName) as lastName
		concat(toUpper(firstName), ' ', toUpper(lastName)) as fullName
		addresses as userAddresses {
			streetOne
			streetTwo 
			city
			state 
		}
		auditEntry {
			createdBy
			createdDateTime
			updatedBy
			updatedDateTime
		}
	})
	.page({
		skip 0
		take 25.0
		token ''
	})
	.sort({
		firstName desc
		lastName asc
	})";

void Main()
{
	query.Length.Dump();
	var lexer = new TokenLexer(query.Select(x => (byte)x).ToArray(), new()
	{
		SkipCarriageReturn = true,
		SkipLineFeed = true,
		SkipTabs = true,
		SkipWhiteSpace = true,
	});

	var queue = new Queue<Token>();

	while (lexer.HasNext)
	{
		var peek = lexer.Peek();

		var token = lexer.Next();

		//if (peek != token)
		//{
		//	
		//}

		queue.Enqueue(token);
	}

	queue.Dump();
}

/*
	References: 
	- https://jack-vanlightly.com/blog/2016/2/3/creating-a-simple-tokenizer-lexer-in-c
	- https://jack-vanlightly.com/blog/2016/2/24/a-more-efficient-regex-tokenizer
	- https://zanlukaartic.medium.com/programming-your-own-simple-lexer-in-c-76cab5f7e39b
*/


#region Step 01: Lexer (Parsing Incoming Text) into Query Tokens

internal abstract class LexerException : Exception
{
	public LexerException() { }
	public LexerException(string message) : base(message) { }
	public LexerException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// 
/// </summary>
/// <remarks>
/// Tokens can be grouped into multiple categorization. 
/// For example the 'and' operator is also a keyword as it is reserved.
/// </remarks>
public enum TokenType : ulong
{
	#region Other
	/// <summary>
	/// Identifier is the name given to different programming elements. 
	/// It can be either a name given to a variable or a function or any other programming element which follow some basic naming conventions
	/// </summary>
	/// <remarks>
	/// The parser is responsible for identifying what 
	/// </remarks>
	Identifier,
	#endregion

	#region Separators
	Tab,
	LineFeed,
	CarriageReturn,
	Comma,
	Slash,
	Question,
	Dot,
	Colon,
	Semicolon,
	WhiteSpace,
	Exclamation,
	OpenParenthesis,
	CloseParenthesis,
	OpenBracket,
	CloseBracket,
	OpenSquareBracket,
	CloseSquareBracket,
	#endregion

	#region Literals (are constant values that are used for performing various operations and calculations)
	Null, // Also a keyword
	Boolean, // Also a keyword
	/// <summary>
	/// A integer is a literal which represents types such as:
	/// <see cref="short"/>,
	/// <see cref="int"/>,
	/// <see cref="long"/>,
	/// <see cref="ushort"/>,
	/// <see cref="uint"/>,
	/// <see cref="ulong"/>
	/// </summary>
	Integer,
	/// <summary>
	/// A floating point is a literal which represents types such as:
	/// <see cref="System.Single"/>,
	/// <see cref="decimal"/>,
	/// <see cref="double"/>
	/// </summary>
	FloatingPoint,
	/// <summary>
	/// A string is a literal that represents many types such as: 
	/// <see cref="String"/>,
	/// <see cref="Guid"/>,
	/// <see cref="TimeSpan"/>,
	/// <see cref="TimeOnly"/>,
	/// <see cref="DateOnly"/>,
	/// <see cref="DateTime"/>,
	/// <see cref="DateTimeOffset"/>
	/// </summary>
	String,
	#endregion

	#region Operators (A operator returns a binary expression)
	Star, // 
	Plus,
	Minus,
	Equal,
	NotEqual,
	GreaterThan,
	GreaterThanOrEqual,
	LessThan,
	LessThanOrEqual,
	And,
	Or,
	Any,
	All,
	Alias,
	In,
	#endregion

	#region Keywords
	Select,
	Filter,
	Sort,
	Page,
	Descending,
	Ascending,
	Take,
	Skip,
	Token
	#endregion
}
/// <summary>
/// 
/// </summary>
public readonly struct Token
{
	/// <summary>
	/// Specifies the start position for the given token within a sequence.
	/// </summary>
	public long Start { get; init; }
	/// <summary>
	/// Specifies the end position for the given token within a sequence.
	/// </summary>
	public long End { get; init; }
	/// <summary>
	/// The raw value as bytes.
	/// </summary>
	public byte[] Value { get; init; }
	/// <summary>
	/// The Value in bytes parsed as raw text with UTF8 encoding.
	/// </summary>
	public string ValueAsText => Encoding.UTF8.GetString(Value);
	/// <summary>
	/// Represents the token kind.
	/// </summary>
	public TokenType TokenType { get; init; }

	public bool IsKeyword
	{
		get
		{
			switch (TokenType)
			{
				case TokenType.Filter:
				case TokenType.Select:
				case TokenType.Sort:
				case TokenType.Page:
				case TokenType.Null:
				case TokenType.And:
				case TokenType.Or:
				case TokenType.Boolean:
				case TokenType.Alias:
				case TokenType.Ascending:
				case TokenType.Descending:
				case TokenType.Take:
				case TokenType.Skip:
				case TokenType.Token:
					return true;
				default:
					return false;
			}
		}
	}
	public bool IsLiteral
	{
		get
		{
			switch (TokenType)
			{
				case TokenType.String:
				case TokenType.FloatingPoint:
				case TokenType.Boolean:
				case TokenType.Integer:
					return true;
				default:
					return false;
			}
		}
	}
	public bool IsOperator
	{
		get
		{
			switch (TokenType)
			{
				case TokenType.Equal:
				case TokenType.NotEqual:
				case TokenType.GreaterThan:
				case TokenType.GreaterThanOrEqual:
				case TokenType.LessThan:
				case TokenType.LessThanOrEqual:
				case TokenType.And:
				case TokenType.Or:
				case TokenType.Alias:
					return true;
				default:
					return false;
			}
		}
	}

	/// <summary>
	/// Parses the given token data as <see cref="DateOnly"/>.
	/// </summary>
	/// <returns></returns>
	public DateOnly GetDate() => DateOnly.Parse(ValueAsText);
	public DateTime GetDateTime() => DateTime.Parse(ValueAsText);
	public DateTimeOffset GetDateTimeOffset() => DateTimeOffset.Parse(ValueAsText);

}
public struct TokenLexerOptions
{
	public bool SkipTabs { get; set; }
	public bool SkipWhiteSpace { get; set; }
	public bool SkipLineFeed { get; set; }
	public bool SkipCarriageReturn { get; set; }
}
/// <summary>
/// 
/// The Token Lexer (also known as a Tokenizer and)
/// </summary>
public ref partial struct TokenLexer
{
	private readonly TokenLexerOptions options;
	private ReadOnlySequence<byte> sequence; // Maintain Original Sequence
	private ReadOnlySequence<byte> remaining; //

	private Token current = default;
	private long currentPosition = default;

	public TokenLexer(byte[] query) : this(query, new()) { }
	public TokenLexer(byte[] query, TokenLexerOptions options)
	{
		this.options = options;
		this.sequence = new ReadOnlySequence<byte>(query);
		this.remaining = sequence;
	}

	#region Public Methods
	/// <summary>
	/// Specifies whether the Lexer has another token within the sequence.
	/// </summary>
	public bool HasNext => !remaining.IsEmpty;
	/// <summary>
	/// Is the current token that has been parsed.
	/// </summary>
	public Token Current => this.current;
	/// <summary>
	/// Peeks at the next token in the sequence.
	/// </summary>
	/// <returns></returns>
	/// <summary>
	/// Peeks at the next token in the sequence.
	/// </summary>
	/// <returns></returns>
	public Token Peek()
	{
		var sequence = remaining;
	forward:
		var sequenceReader = new SequenceReader<byte>(sequence);

		while (!sequenceReader.End)
		{
			sequenceReader.Advance(1);

			if (TryParse(ref sequenceReader, out var token))
			{
				sequence = sequenceReader.UnreadSequence;

				switch (token.TokenType)
				{
					case TokenType.Tab when options.SkipTabs:
					case TokenType.LineFeed when options.SkipLineFeed:
					case TokenType.WhiteSpace when options.SkipWhiteSpace:
					case TokenType.CarriageReturn when options.SkipCarriageReturn:
						goto forward;
				}

				return token;
			}
		}
		// If we reached here something is wrong within the syntax
		throw new Exception("End of Sequence");
	}
	/// <summary>
	/// Retrieves the next token in the sequence.
	/// </summary>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	public Token Next()
	{
		forward:
		var sequenceReader = new SequenceReader<byte>(remaining);

		while (!sequenceReader.End)
		{
			sequenceReader.Advance(1);

			if (TryParse(ref sequenceReader, out var token))
			{
				remaining = sequenceReader.UnreadSequence;
				currentPosition += sequenceReader.Consumed;
				current = token;

				switch (token.TokenType)
				{
					case TokenType.Tab when options.SkipTabs:
					case TokenType.LineFeed when options.SkipLineFeed:
					case TokenType.WhiteSpace when options.SkipWhiteSpace:
					case TokenType.CarriageReturn when options.SkipCarriageReturn:
						goto forward;
				}

				return current;
			}
		}

		// If we reached here something is wrong within the syntax
		throw new Exception("End of Sequence");
	}
	/// <summary>
	/// Tries to peek at the next token in the sequence.
	/// </summary>
	/// <param name="token"></param>
	/// <returns></returns>
	public bool TryPeek(out Token token)
	{
		token = default;
		if (HasNext)
		{
			token = Peek();
			return true;
		}
		return false;
	}
	/// <summary>
	/// Tries to retrieve the next token in the sequence.
	/// </summary>
	/// <param name="token"></param>
	/// <returns></returns>
	public bool TryNext(out Token token)
	{
		token = default;
		if (HasNext)
		{
			token = Next();
			return true;
		}
		return false;
	}
	#endregion

	#region Private Methods
	private bool TryParse(ref SequenceReader<byte> sequenceReader, out Token token)
	{
		token = default;
		TokenType tokenType;

		// NOTE: Do not change this order
		if (
			sequenceReader.IsOperator(out tokenType) 	||
			sequenceReader.IsKeyword(out tokenType) 	||
			sequenceReader.IsLiteral(out tokenType) 	||
			sequenceReader.IsSeparator(out tokenType) 	||
			sequenceReader.IsIdentifer(out tokenType)) // Identifier needs be checked last
		{
			token = GetToken(ref sequenceReader, tokenType);
			return true;
		}

		return false;
	}

	private Token GetToken(ref SequenceReader<byte> sequenceReader, TokenType tokenType)
	{
		return new Token()
		{
			Value = sequenceReader.Slice().ToArray(),
			TokenType = tokenType,
			Start = currentPosition,
			End = (currentPosition + sequenceReader.Consumed) - 1
		};
	}
	#endregion
}
internal static class TokenLexerExtensions
{
	#region Separators
	private static ReadOnlySpan<KeyValuePair<TokenType, byte[]>> separators => new KeyValuePair<TokenType, byte[]>[]
	{
		new KeyValuePair<TokenType, byte[]>(TokenType.WhiteSpace, new byte[] {(byte)' ' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.Tab, new byte[] {(byte)'\t' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.LineFeed, new byte[] {(byte)'\n' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.CarriageReturn, new byte[] {(byte)'\r' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.OpenParenthesis, new byte[] {(byte)'(' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.CloseParenthesis, new byte[] {(byte)')' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.OpenBracket,new byte[] { (byte)'{' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.CloseBracket, new byte[] {(byte)'}' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.Colon,new byte[] { (byte)':' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.Semicolon, new byte[] {(byte)';' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.Comma,new byte[] { (byte)',' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.Slash, new byte[] {(byte)'/' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.Dot,new byte[] { (byte)'.' })
	};
	public static bool IsSeparator(this ref SequenceReader<byte> sequenceReader, out TokenType tokenType)
	{
		tokenType = default;

		// Separators are one byte long 
		if (sequenceReader.Consumed != 1)
		{
			return false;
		}

		var value = sequenceReader.Slice().ToArray();

		foreach (var separator in separators)
		{
			if (separator.Value.SequenceEqual(value))
			{
				tokenType = separator.Key;
				return true;
			}
		}

		return false;
	}
	public static bool IsSeparatorNext(this ref SequenceReader<byte> sequenceReader)
	{
		if (sequenceReader.TryPeek(out var value))
		{
			var array = new byte[] { value };

			foreach (var separator in separators)
			{
				if (separator.Value.SequenceEqual(array))
				{
					return true;
				}
			}
		}

		return false;
	}
	public static bool IsSeparatorNext(this ref SequenceReader<byte> sequenceReader, params byte[] omit)
	{
		if (sequenceReader.TryPeek(out var value))
		{
			var array = new byte[] { value };
			var omits = omit.Select(x => new byte[] { x });

			foreach (var separator in separators)
			{
				if (omits.Any(x => x.SequenceEqual(separator.Value)))
				{
					continue;
				}
				if (separator.Value.SequenceEqual(array))
				{
					return true;
				}
			}
		}

		return false;
	}

	#endregion

	#region Keywords
	private static ReadOnlySpan<KeyValuePair<TokenType, byte[]>> keywords => new KeyValuePair<TokenType, byte[]>[]
	{
		new KeyValuePair<TokenType, byte[]>(TokenType.Filter, new byte[] { (byte)'f', (byte)'i', (byte)'l', (byte)'t', (byte)'e', (byte)'r' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.Select, new byte[] { (byte)'s', (byte)'e', (byte)'l', (byte)'e', (byte)'t' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.Sort, new byte[] { (byte)'s', (byte)'o', (byte)'r', (byte)'t' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.Page, new byte[] { (byte)'p', (byte)'a', (byte)'g', (byte)'e' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.Ascending, new byte[] { (byte)'a', (byte)'s', (byte)'c' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.Descending, new byte[] { (byte)'d', (byte)'e', (byte)'s', (byte)'c' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.Take, new byte[] { (byte)'t', (byte)'a', (byte)'k', (byte)'e' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.Skip, new byte[] { (byte)'s', (byte)'k', (byte)'i', (byte)'p' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.Token, new byte[] { (byte)'t', (byte)'o', (byte)'k', (byte)'e', (byte)'n' })
	};
	public static bool IsKeyword(this ref SequenceReader<byte> sequenceReader, out TokenType tokenType)
	{
		tokenType = default;

		if (sequenceReader.Consumed < 3 || sequenceReader.Consumed > 7)
		{
			return false;
		}

		var value = sequenceReader.SliceToLowerChar().ToArray();

		foreach (var keyword in keywords)
		{
			if (keyword.Value.SequenceEqual(value) &&
			   (sequenceReader.IsEndNext() || sequenceReader.IsSeparatorNext()))
			{
				tokenType = keyword.Key;
				return true;
			}
		}

		return false;
	}

	#endregion

	#region Operators
	private static ReadOnlySpan<KeyValuePair<TokenType, byte[]>> operators => new KeyValuePair<TokenType, byte[]>[]
	{
		new KeyValuePair<TokenType, byte[]>(TokenType.Plus, new byte[] { (byte)'+' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.Minus, new byte[] { (byte)'-' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.Star, new byte[] { (byte)'*' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.Equal, new byte[] { (byte)'e', (byte)'q' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.NotEqual, new byte[] { (byte)'n', (byte)'e', (byte)'q' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.GreaterThan, new byte[] { (byte)'g', (byte)'t' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.GreaterThanOrEqual, new byte[] { (byte)'g', (byte)'t', (byte)'e' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.LessThan, new byte[] { (byte)'l', (byte)'t' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.LessThanOrEqual, new byte[] { (byte)'l', (byte)'t', (byte)'e' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.Alias, new byte[] { (byte)'a', (byte)'s' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.And, new byte[] { (byte)'a', (byte)'n', (byte)'d' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.Or, new byte[] { (byte)'o', (byte)'r' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.In, new byte[] { (byte)'i', (byte)'n' }),
	};
	public static bool IsOperator(this ref SequenceReader<byte> sequenceReader, out TokenType tokenType)
	{
		tokenType = default;

		if (sequenceReader.Consumed > 3)
		{
			return false;
		}

		var value = sequenceReader.SliceToLowerChar().ToArray();

		foreach (var @operator in operators)
		{
			if (@operator.Value.SequenceEqual(value) && (sequenceReader.IsEndNext() || sequenceReader.IsSeparatorNext()))
			{
				tokenType = @operator.Key;
				return true;
			}
		}

		return false;
	}

	#endregion

	#region Literal
	private static ReadOnlySpan<KeyValuePair<TokenType, byte[]>> literals => new KeyValuePair<TokenType, byte[]>[]
	{
		new KeyValuePair<TokenType, byte[]>(TokenType.Boolean, new byte[] { (byte)'t', (byte)'r', (byte)'u', (byte)'e' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.Boolean, new byte[] { (byte)'f', (byte)'a', (byte)'l', (byte)'s', (byte)'e' }),
		new KeyValuePair<TokenType, byte[]>(TokenType.Null, new byte[] { (byte)'n', (byte)'u', (byte)'l', (byte)'l' }),
	};

	public static bool IsLiteral(this ref SequenceReader<byte> sequenceReader, out TokenType tokenType)
	{
		tokenType = default;

		if (sequenceReader.Consumed == 1)
		{
			// Identify if string literal
			if (sequenceReader.CurrentSpan[0] == (byte)'\'')
			{
				if (!sequenceReader.TryAdvanceTo((byte)'\''))
				{
					throw new Exception("Invalid string format");
				}
				tokenType = TokenType.String;
				return true;
			}

			// Identify if current value is digit
			if (char.IsDigit((char)sequenceReader.CurrentSpan[0]))
			{
				while (sequenceReader.TryRead(out var c))
				{
					if (!char.IsDigit((char)c) && c != (byte)'.' && c != (byte)'e') // Lets check that the current span includes acceptable char
					{
						sequenceReader.Rewind(1);
						break;
					}
				}

				var value = sequenceReader.Slice();

				foreach (var v in value)
				{
					if (v == (byte)'.')
					{
						tokenType = TokenType.FloatingPoint;
						return true;
					}
				}

				tokenType = TokenType.Integer;
				return true;
			}
		}
		// Identify keyword literals
		else
		{
			var value = sequenceReader.SliceToLowerChar().ToArray();

			foreach (var literal in literals)
			{
				if (literal.Value.SequenceEqual(value) && (sequenceReader.IsEndNext() || sequenceReader.IsSeparatorNext()))
				{
					tokenType = literal.Key;
					return true;
				}
			}
		}

		return false;
	}

	#endregion

	#region Identifier

	public static bool IsIdentifer(this ref SequenceReader<byte> sequenceReader, out TokenType tokenType)
	{
		tokenType = default;

		// As the lexer loops through the sequence of bytes
		if (sequenceReader.IsSeparatorNext() ||
			sequenceReader.IsEndNext() ||
			!sequenceReader.IsAlphaNumericCharNext()) // This is to account for any unknown char
		{
			tokenType = TokenType.Identifier;
			return true;
		}

		return false;
	}

	#endregion

	private static bool IsAlphaNumericCharNext(this ref SequenceReader<byte> sequenceReader)
	{
		return sequenceReader.TryPeek(out var value) && char.IsLetterOrDigit((char)value);
	}
	private static bool IsEndNext(this ref SequenceReader<byte> sequenceReader)
	{
		return sequenceReader.Remaining <= 0;
	}

	public static ReadOnlySpan<byte> Slice(this ref SequenceReader<byte> sequenceReader)
	{
		return sequenceReader.CurrentSpan.Slice(0, (int)sequenceReader.Consumed);
	}

	// Slices current consumed sequence as span and convert all alpha-characters 
	private static ReadOnlySpan<byte> SliceToLowerChar(this ref SequenceReader<byte> sequenceReader)
	{
		var buffer = new byte[sequenceReader.Consumed];

		for (int i = 0; i < sequenceReader.Consumed; i++)
		{
			buffer[i] = (byte)(char.ToLower((char)sequenceReader.CurrentSpan[i]));
		}

		return buffer.AsSpan();
	}

}

#endregion


// This Validates corrext token structure
#region Step 02: Parser (Parse Incoming Tokens) into Query Nodes
public enum QueryNodeType
{
	Document,
	Statement,
	Operator,
	Function,
	Constant,
	Binary,
	Parameter,
	Field,
	Alias
}

public interface IQueryNodeVisitor<T>
{
	T Visit(QueryNode node);
}
public abstract class QueryNode
{
	public abstract QueryNodeType NodeType { get; }

	public virtual T Accept<T>(IQueryNodeVisitor<T> visitor)
	{
		return visitor.Visit(this);
	}
}

public class QueryDocumentNode : QueryNode
{
	public QueryDocumentNode(IEnumerable<QueryNode> nodes)
	{
		this.Nodes = nodes.ToArray();
	}

	public QueryNode[] Nodes { get; init; }
	public override QueryNodeType NodeType => QueryNodeType.Document;

	public override T Accept<T>(IQueryNodeVisitor<T> visitor)
	{
		return visitor.Visit(this);
	}

	public QueryFromStatementNode? GetFromNode() => GetNode<QueryFromStatementNode>();
	public QuerySelectStatementNode? GetSelectNode() => GetNode<QuerySelectStatementNode>();
	public QueryFilterStatementNode? GetFilterNode() => GetNode<QueryFilterStatementNode>();

	private T GetNode<T>()
	{
		foreach (var node in Nodes)
		{
			if (node is T type)
			{
				return type;
			}
		}

		return default;
	}
}
public enum QueryStatmentType
{
	From,
	Select,
	Summarize, // Group BY Equivalent
	Filter,
	Top,
	Skip,
	Resolve,
}
public abstract class QueryStatementNode : QueryNode
{
	public abstract QueryStatmentType StatementType { get; }
}

public class QueryFromStatementNode : QueryStatementNode
{
	public QueryFromStatementNode() { }
	public QueryFromStatementNode(QueryConstantNode source)
	{
		this.Source = source;
	}
	public QueryFromStatementNode(QueryConstantNode source, QueryConstantNode sourceAlias)
	{
		this.Source = source;
		this.SourceAlias = sourceAlias;
	}

	public QueryConstantNode Source { get; init; }
	public QueryConstantNode? SourceAlias { get; init; }
	public override QueryNodeType NodeType => QueryNodeType.Statement;
	public override QueryStatmentType StatementType => QueryStatmentType.From;

	public override T Accept<T>(IQueryNodeVisitor<T> visitor)
	{
		return visitor.Visit(this);
	}
}
public class QuerySelectStatementNode : QueryStatementNode
{
	public override QueryStatmentType StatementType => QueryStatmentType.Select;
	public override QueryNodeType NodeType => QueryNodeType.Statement;

	public override T Accept<T>(IQueryNodeVisitor<T> visitor)
	{
		return visitor.Visit(this);
	}
}
public class QueryFilterStatementNode : QueryStatementNode
{
	public override QueryStatmentType StatementType => QueryStatmentType.Filter;
	public override QueryNodeType NodeType => QueryNodeType.Statement;

	public override T Accept<T>(IQueryNodeVisitor<T> visitor)
	{
		return visitor.Visit(this);
	}
}
//public class QuerySummarizeStatmentNode : QueryStatementNode
//{
//	
//}

public enum QueryOperatorType
{
	Equals,
	NotEquals,
	GreaterThan,
	GreaterThanOrEquals,
	LessThan,
	LessThanOrEquals,
	And,
	Or,

}
public class QueryBinaryNode : QueryNode
{
	public QueryBinaryNode() { }
	public QueryBinaryNode(QueryNode left, QueryNode right, QueryOperatorType operatorType)
	{
		this.Left = left;
		this.Right = right;
		this.OperatorType = operatorType;
	}

	public QueryNode Left { get; init; }
	public QueryNode Right { get; init; }
	public QueryOperatorType OperatorType { get; init; }
	public override QueryNodeType NodeType => QueryNodeType.Binary;

	public override T Accept<T>(IQueryNodeVisitor<T> visitor)
	{
		return visitor.Visit(this);
	}
}
public class QueryConstantNode : QueryNode
{
	public QueryConstantNode() { }
	public QueryConstantNode(string value)
	{
		this.Value = value;
	}

	public string Value { get; init; }
	public override QueryNodeType NodeType => QueryNodeType.Constant;


}


public enum QueryFunctionType
{
	Sum,

}

public class QueryParser
{
	//private ref TokenLexer lexer;

}
//public class QueryFunctionNode : QueryNode
//{
//	
//}

#endregion




