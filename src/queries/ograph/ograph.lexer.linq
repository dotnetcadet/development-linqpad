<Query Kind="Program">
  <Namespace>System.Buffers</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <RuntimeVersion>6.0</RuntimeVersion>
</Query>

private const string query = @" from('users')
		.filter(startsWith(firstName, 'c', true))
		.select(
			firstName
			lastName
			addresses {
				streetOne
				streetTwo
				city
				state
			}
		)
		.top(20)
		.skip(0)
		.summarize(
		
		)
		.resolve('addresses', 'a')
			.select('a.streetOne, a.streetTwo')
			.top(10)
			.skip(0)
			.summarize()
		.resolve('details')";

void Main()
{	
	var lexer = new TokenLexer(query.Select(x=> (byte)x).ToArray());
	
	while (lexer.HasNext)
	{
		var token = lexer.Next();
		
	}
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
internal enum TokenType : byte
{
	#region Other
	Unknown,
	End,
	Identifier, // Represents a Property, Entity Type. Usually is found in between other tokens
	Comma,
	Slash,
	Question,
	Dot,
	Star,
	Colon,
	Semicolon,
	WhiteSpace,
	Exclamation,
	OpenParenthesis,
	CloseParenthesis,
	OpenBracket,
	CloseBracket,
	#endregion
	
	#region Literals
	Null,
	Boolean,
	String,
	Integer,
	Long,
	Single,
	DateTime,
	Decimal,
	Double,
	Guid,
	Binary,
	DateTimeOffset,
	Duration,
	Geography,
	Geometry,
	#endregion
	
	#region Operators
	Equal,
	NotEqual,
	Plus,
	Minus,
	GreaterThan,
	GreaterThanOrEqual,
	LessThan,
	And,
	Or,
	Any,
	All,
	Alias,
	In,
	
	#endregion
	
	#region Keywords
	From,
	Select,
	Filter,
	Sort,
	Skip,
	Take,
	Resolve,
	Summarize,
	#endregion
	
	#region Function Calls
	Sum,
	StartsWith,
	EndsWith,
	Between,
	Left,
	Right,
	
	#endregion
}
internal struct Token
{
	internal int Start;
	internal int End;
	internal int Position;
	internal byte[] Sequence;
	internal object Value;	
	internal string Text => Encoding.UTF8.GetString(Sequence);
	internal TokenType TokenType;	
}
internal ref partial struct TokenLexer
{
	#region Operators
	internal static ReadOnlySpan<byte> Dot => new byte[] { (byte)'.' };
	internal static ReadOnlySpan<byte> WhiteSpace => new byte[] { (byte)' ' };
	internal static ReadOnlySpan<byte> SingleQuoteOperator => new byte[] { (byte)'\'' };
	internal static ReadOnlySpan<byte> DoubleQuote => new byte[] { (byte)'"' };
	internal static ReadOnlySpan<byte> StarOperator => new byte[] { (byte)'*' };
	internal static ReadOnlySpan<byte> EqualOperator => new byte[] { (byte)'=' };
	internal static ReadOnlySpan<byte> PlusOperator => new byte[] { (byte)'+' };
	internal static ReadOnlySpan<byte> MinusOperator => new byte[] { (byte)'-' };
	internal static ReadOnlySpan<byte> GreaterThanOperator => new byte[] { (byte)'>' };
	internal static ReadOnlySpan<byte> GreaterThanOrEqualOperator => new byte[] { (byte)'>', (byte)'=' };
	internal static ReadOnlySpan<byte> LessThanOperator => new byte[] { (byte)'<' };
	internal static ReadOnlySpan<byte> LessThanOrEqualOperator => new byte[] { (byte)'<', (byte)'=' };
	internal static ReadOnlySpan<byte> AndOperator => new byte[] { (byte)'a', (byte)'n', (byte)'d' };
	internal static ReadOnlySpan<byte> OrOperator => new byte[] { (byte)'o', (byte)'r' };
	#endregion
	
	#region Lierals
	internal static ReadOnlySpan<byte> BooleanTrueLiteral => Encoding.UTF8.GetBytes("true");
	internal static ReadOnlySpan<byte> BooleanFalseLiteral => Encoding.UTF8.GetBytes("false");
	#endregion
	
	#region Keywords
	internal static ReadOnlySpan<byte> FromClause => Encoding.UTF8.GetBytes("from");
	internal static ReadOnlySpan<byte> SelectClause => Encoding.UTF8.GetBytes("select");
	internal static ReadOnlySpan<byte> FilterClause => Encoding.UTF8.GetBytes("filter");
	internal static ReadOnlySpan<byte> ResolveClause => Encoding.UTF8.GetBytes("resolve");
	internal static ReadOnlySpan<byte> SkipClause => Encoding.UTF8.GetBytes("skip");
	internal static ReadOnlySpan<byte> SummarizeClause => Encoding.UTF8.GetBytes("summarize");
	#endregion
	
	#region Function Calls
	internal static ReadOnlySpan<byte> StartsWithFunction => Encoding.UTF8.GetBytes("startswith");
	
	#endregion
}
internal ref partial struct TokenLexer
{
	//private ReadOnlySequence<byte> currentSequence; // orginal sequence
	private ReadOnlySequence<byte> 	sequence;
	private ReadOnlySequence<byte>	remaining;
	
	private Token 					currentToken		= default;
	private ReadOnlySpan<byte> 		currentSpan 		= new byte[0];
	private long 					currentTokenStart 	= default;
	private long 					currentTokenEnd 	= default;


	public TokenLexer(byte[] query)
	{
		this.sequence = new ReadOnlySequence<byte>(query);
		this.remaining = sequence;
	}

	public Token CurrentToken => this.currentToken;
	public Token Peek()
	{
//		var previousPosition = sequenceReader.Consumed;
//
//		var token = Next();
//
//		var currentPosition = sequenceReader.Consumed;
//
//		sequenceReader.Rewind(currentPosition - previousPosition);

		return default;
	}
	public Token Next()
	{
		if (currentToken.TokenType != TokenType.End)
		{
			var sequenceReader = new SequenceReader<byte>(remaining);

			while (!sequenceReader.End)
			{
				sequenceReader.Advance(1);

				if (TryParse(ref sequenceReader, out var token))
				{
					remaining = sequenceReader.UnreadSequence;

					currentToken = token;
					
					return currentToken;
				}
			}
		}
		// If we reached here something is wrong witht the syntax
		throw new Exception("End of Sequence");
	}
	

	public bool HasNext => !sequence.IsEmpty;
	
	/*
		When parsing tokens considerations of higher and lower precedence is important
		
		string literal
			-> Check is DateTime Literal
			-> Check is DateOnly Lieral
			
			-> else escape and return String Literal
	
	*/
	
	private bool TryParse(ref SequenceReader<byte> sequenceReader, out Token token)
	{
		token = default;
		
		return 
			IsOther(ref sequenceReader, out token) || 
			IsKeyword(ref sequenceReader, out token) || 
			IsLiteral(ref sequenceReader, out token) ||
			IsOperator(ref sequenceReader, out token) ||
			IsFunctionCall(ref sequenceReader, out token) ||
			IsIdentifier(ref sequenceReader, out token); // Identifier needs be checked last
	}
	private bool IsOther(ref SequenceReader<byte> sequenceReader, out Token token)
	{
		token = default;
		
		var value = GetCurrentSpan(ref sequenceReader);
		
		if (WhiteSpace.SequenceEqual(value))
		{
			token = new Token()
			{
				Sequence = value.ToArray(),
				Value = (char)value[0],
				TokenType = TokenType.WhiteSpace
			};
			return true;
		}
		
		return false;
	}
	private bool IsKeyword(ref SequenceReader<byte> sequenceReader, out Token token)
	{
		token = default;

		var value = GetCurrentSpan(ref sequenceReader);

		if (FromClause.SequenceEqual(value))
		{
			token = new Token()
			{
				TokenType = TokenType.From
			};
			return true;
		}
		
		return false;

	}
	private bool IsLiteral(ref SequenceReader<byte> sequenceReader, out Token token)
	{
		token = default;
		
		var value = GetCurrentSpan(ref sequenceReader);
		
		// Check if the current span in the seqnece reader is one and it starts with a string literal single qupte
		if (value.Length == 1 && SingleQuoteOperator.SequenceEqual(value))
		{
			// Try to go to next 
			if (!sequenceReader.TryAdvanceTo((byte)'\''))
			{
				
			}
			
			var stringLiteral = GetCurrentSpan(ref sequenceReader);
		}
		if (BooleanFalseLiteral.SequenceEqual(value) || BooleanTrueLiteral.SequenceEqual(value))
		{
			
		}
		
		return false;
	}
	private bool IsOperator(ref SequenceReader<byte> sequenceReader, out Token token)
	{
		token = default;
		
		
		
		return false;
	}
	private bool IsIdentifier(ref SequenceReader<byte> sequenceReader, out Token token)
	{
		token = default;
		
		if (sequenceReader.IsNext(WhiteSpace) ||
			sequenceReader.IsNext(Dot))
		{
			token = new Token()
			{
				TokenType = TokenType.Identifier
			};
			return true;
		}

		return false;
	}
	private bool IsFunctionCall(ref SequenceReader<byte> sequenceReader, out Token token)
	{
		token = default;
		
		var value = GetCurrentSpan(ref sequenceReader);
		
		if (StartsWithFunction.SequenceEqual(value))
		{
			token = new Token()
			{
				
				TokenType = TokenType.StartsWith
			};
		}
		
		return false;
	}
	
	private ReadOnlySpan<byte> GetCurrentSpan(ref SequenceReader<byte> sequenceReader)
	{
		return sequenceReader.CurrentSpan.Slice(0, (int)sequenceReader.Consumed);
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
	
	public QueryFromStatementNode?	GetFromNode() => GetNode<QueryFromStatementNode>();
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




