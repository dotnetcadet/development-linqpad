<Query Kind="Program">
  <Namespace>System.Buffers</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <RuntimeVersion>6.0</RuntimeVersion>
</Query>

private const string query = @".from('users', 'u')
		.filter(startsWith(u.firstName, 'c', true))
		.select('u.firstName, u.lastName')
		.top(20)
		.skip(0)
		.resolve('addresses', 'a')
			.select('a.streetOne, a.streetTwo')
			.top(10)
			.skip(0)
			.summarize()
		.resolve('details')";

void Main()
{
	var lexer = new TokenLexer(query.Select(x=> (byte)x).ToArray().AsSpan());
	
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


// This
#region Step 01: Lexer (Parsing Incoming Text) into Query Tokens

internal abstract class LexerException : Exception
{
	public LexerException() { }
	public LexerException(string message) : base(message) { }
	public LexerException(string message, Exception innerException) : base(message, innerException) { }
	
	
	
}

internal enum TokenType : byte
{
	/// <summary>Unknown.</summary>
	Unknown,

	/// <summary>End of text.</summary>
	End,

	/// <summary>'=' - equality character.</summary>
	Equal,

	/// <summary>Identifier.</summary>
	Identifier,

	/// <summary>NullLiteral.</summary>
	NullLiteral,

	/// <summary>BooleanLiteral.</summary>
	BooleanLiteral,

	/// <summary>StringLiteral.</summary>
	StringLiteral,

	/// <summary>IntegerLiteral.</summary>
	IntegerLiteral,

	/// <summary>Int64 literal.</summary>
	Int64Literal,

	/// <summary>Single literal.</summary>
	SingleLiteral,

	/// <summary>DateTime literal.</summary>
	DateTimeLiteral,

	/// <summary>Decimal literal.</summary>
	DecimalLiteral,

	/// <summary>Double literal.</summary>
	DoubleLiteral,

	/// <summary>GUID literal.</summary>
	GuidLiteral,

	/// <summary>Binary literal.</summary>
	BinaryLiteral,

	/// <summary>DateTimeOffset literal.</summary>
	DateTimeOffsetLiteral,

	/// <summary>Duration literal.</summary>
	DurationLiteral,

	/// <summary>Exclamation.</summary>
	Exclamation =  (byte)'!',

	/// <summary>OpenParen.</summary>
	OpenParen = (byte)'(',

	/// <summary>CloseParen.</summary>
	CloseParen = (byte)')',

	/// <summary>Comma.</summary>
	Comma = (byte)',',

	/// <summary>Minus.</summary>
	Minus = (byte)'-',

	/// <summary>Slash.</summary>
	Slash,

	/// <summary>Question.</summary>
	Question = (byte)'?',

	/// <summary>Dot.</summary>
	Dot = (byte)'.',

	/// <summary>Star.</summary>
	Star = (byte)'*',

	/// <summary>Colon.</summary>
	Colon = (byte)':',

	/// <summary>Semicolon</summary>
	Semicolon = (byte)';',

	/// <summary>Spatial Literal</summary>
	GeographylLiteral,

	/// <summary>Geometry Literal</summary>
	GeometryLiteral,

	/// <summary>Whitespace</summary>
	WhiteSpace
}
internal struct Token
{
	internal int Position;
	internal byte[] RawValue;
	internal TokenType TokenType;
	
	internal string RawText => Encoding.UTF8.GetString(RawValue);
}
internal ref partial struct TokenLexer
{
	private delegate bool TryRead(ref SequenceReader<byte> reader, out byte output);

	private SequenceReader<byte> sequenceReader;
	
	private Token current = default;

	public TokenLexer(byte[] query)
	{
		this.sequenceReader = new SequenceReader<byte>(new ReadOnlySequence<byte>(query));
	}

	public Token Current => this.current;
	public Token Next()
	{
		current = Parse((ref SequenceReader<byte> reader, out byte output) => reader.TryRead(out output));

		return current;
	}
	public Token Peek()
	{
		current = Parse((ref SequenceReader<byte> reader, out byte output) => reader.TryPeek(out output));
		
		return current;
	}

	public bool HasNext => sequenceReader.Remaining > 0;
	
	private Token Parse(TryRead reader)
	{
		while (reader.Invoke(ref sequenceReader, out var value))
		{
			switch (value)
			{
				case (byte)'.': 
				{
					return new Token()
					{
						Position = sequenceReader.Position.GetInteger(),
						TokenType = TokenType.Dot,						
						RawValue = sequenceReader.CurrentSpan.ToArray()
					};
				}
				case (byte)'(':
				{
					return new Token()
					{
						
					};
				}
			}
		}
		
		return default;
	}



//	private bool TryGetOpenParan(byte value, out Token token)
//	{
//		token = default;
//
//		if (value == (byte)'(')
//		{
//			return new
//		}
//
//
//		return false;
//	}






	//public int Depth { get; }











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
//public class QueryFunctionNode : QueryNode
//{
//	
//}

#endregion



public class QueryParser
{
	//private ref TokenLexer lexer;
	
}

