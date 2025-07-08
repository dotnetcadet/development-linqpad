<Query Kind="Program" />

void Main()
{
	var parser = new UriParser();
	var query = "$filter=firstName eq 'Chase'";
	var document = parser.Parse(query);
	var queryable = new List<Person>().AsQueryable();
	var visitor = new QueryableVisitor<Person>(queryable);
	
	visitor.Apply(document);
	
}

#region Syntax Query Parser

public class Entity

public interface IQueryParserContext<T>
{
	T Content { get; }
}
public abstract class QueryParser<T>
{
	public abstract QueryDocument Parse(T content);
}


public class QueryModel
{
	public string Name { get; set; }
	public QueryModelMember Members { get; set; }
}
public class QueryModelMember
{
	
}
public class QueryModelBuilder
{
	
}


public class UriParser : QueryParser<string>
{
	public override QueryDocument Parse(string content)
	{
		var segments = content.Split('&')
			.ToDictionary(key =>  key.Split('=')[0], value => value.Split('=')[1]);
			
		var filterNode = ParseFilter(segments["$filter"]);
			
		
		
		return default;
	}
	
	
	public FilterNode ParseFilter(string filter)
	{
		
		
		return default;
	}
}
#endregion

#region Syntax Query Nodes

public sealed class QueryDocument
{
	// All Top level nodes Should have root functions
	public IEnumerable<QueryNode> Nodes { get; init; }
}

public enum QueryNodeKind
{
	None = 0,
	Binary,
	Constant,
	Filter,
	Member
}
public abstract class QueryNode 
{
	public abstract QueryNodeKind Kind { get; }	
	public virtual T Accept<T>(IQueryVisitor<T> visitor)
	{
		return visitor.Visit(this);
	}
}

public enum BinaryNodeType 
{
	None = 0,
	And,
	Or,
	Equals,
	NotEquals,
	GreaterThan,
	GreaterThanOrEquals,
	LessThan,
	LessThanOrEquals
}
public class BinaryNode : QueryNode
{
	public BinaryNodeType BinaryType {get; set; }
	public override QueryNodeKind Kind => QueryNodeKind.Binary;	
	public QueryNode Left { get; set; }
	public QueryNode Right { get; set; }
}


public class FilterNode : QueryNode
{
	public override QueryNodeKind Kind => QueryNodeKind.Filter;
	public override T Accept<T>(IQueryVisitor<T> visitor)
	{
		return visitor.Visit(this);
	}
}

public class MemberNode : QueryNode
{
	public string Name { get; set; }
	public override QueryNodeKind Kind => QueryNodeKind.Member;
	public override T Accept<T>(IQueryVisitor<T> visitor)
	{
		return visitor.Visit(this);
	}
}
public class ConstantNode : QueryNode
{
	public override QueryNodeKind Kind => QueryNodeKind.Constant;
	public object Value { get; }
	public Type ValueType { get; set; }
	public override T Accept<T>(IQueryVisitor<T> visitor)
	{
		return visitor.Visit(this);
	}
}

#endregion

#region Syntax Query Node Visitor

public interface IQueryVisitor<out T>
{
	T Visit(QueryNode node);
	// Binary Nodes
	T Visit(BinaryNode node);
	
	
	
	
	T Visit(FilterNode node);
	T Visit(ConstantNode node);
	T Visit(MemberNode node);
}
public sealed class QueryableVisitor<T> : IQueryVisitor<Expression>
{
	private static readonly Type type = typeof(T);
	private readonly ParameterExpression parameter = Expression.Parameter(type);
	private readonly IQueryable<T> queryable;
	
	
	public QueryableVisitor(IQueryable<T> queryable)
	{
		this.queryable = queryable;		
	}
	
	// This will apply all the supported queryable functions
	public void Apply(QueryDocument document)
	{
		Expression expression;
		
		var filter = document.Nodes.FirstOrDefault(node => node is FilterNode);
		
		expression = this.Visit(filter);
		

	}

	
	public Expression Visit(QueryNode node)
	{
		return node.Accept(this);
	}
	
	public Expression Visit(FilterNode node) 
	{
		
		
		return node.Accept(this);
	}
	public Expression Visit(BinaryNode node)
	{
		var left = node.Left.Accept(this);
		var right = node.Right.Accept(this);
		
		
		if (node.BinaryType == BinaryNodeType.Equals)
		{
			var expression = Expression.Equal(left, right);
		}
		
		
		
		return node.Accept(this);
	}
	public Expression Visit(ConstantNode node) 
	{
		
		
		return node.Accept(this);
	}
	
	public Expression Visit(MemberNode node) 
	{
		return Expression.Property(parameter, node.Name);
	}
}

#endregion

#region Test Objects

public class Person
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
}

#endregion