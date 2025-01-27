<Query Kind="Program">
<Namespace>System</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>System.Collections.Concurrent</Namespace>
<Namespace>Assimalign.OGraph.Syntax.Internal</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>System.Threading.Tasks</Namespace>
<Namespace>System.Threading</Namespace>
<Namespace>static System.Net.Mime.MediaTypeNames</Namespace>
<Namespace>System.Diagnostics</Namespace>
<Namespace>System.Collections</Namespace>
<Namespace>System.Buffers</Namespace>
<Namespace>System.Runtime.InteropServices</Namespace>
<Namespace>System.Security.Cryptography</Namespace>
<Namespace>System.Runtime.CompilerServices</Namespace>
<Namespace>System.Diagnostics.CodeAnalysis</Namespace>
<Namespace>System.Reflection</Namespace>
<Namespace>System.Collections.Immutable</Namespace>
<Namespace>System.Collections.ObjectModel</Namespace>
<Namespace>System.Linq.Expressions</Namespace>
</Query>

void Main()
{

}

#region Assimalign.OGraph.Syntax(net8.0)
namespace Assimalign.OGraph.Syntax
{
	#region \
	public sealed class QueryDocument
	{
		private readonly ConcurrentBag<Diagnostic> diagnostics;
		internal QueryDocument(
			string text,
			QueryNode root, 
			IEnumerable<Diagnostic> diagnostics)
		{
			this.Text = text;
			this.Root = root;
			this.diagnostics = new ConcurrentBag<Diagnostic>(diagnostics);
		}
		public string Text { get; }
		public bool IsValid => !Errors.Any();
		public QueryNode Root { get; }
		public IEnumerable<Diagnostic> Diagnostics => this.diagnostics.OrderBy(x => x.Severity).ThenBy(x => x.End);
		public IEnumerable<Diagnostic> Errors => Diagnostics.Where(x => x.Severity == DiagnosticSeverity.Error);
		public void AddDiagnostic(Diagnostic diagnostic)
		{
			if (diagnostics is null)
			{
				throw new ArgumentNullException(nameof(diagnostic));
			}
			diagnostics.Add(diagnostic);
		}
	}
	public sealed partial class QueryParser
	{
	    private ConstantNode ParseConstant(ref TokenLexer lexer, ParserContext context)
	    {
	        var token = lexer.Current;
	        return new ConstantNode(
	            token.Value.ToArray(),
	            token.Text,
	            Location.Create(
	                token.Line, 
	                token.Line, 
	                token.Start, 
	                token.End));
	    }
	}
	public sealed partial class QueryParser
	{
	    partial class ParserContext
	    {
	        private readonly Queue<Diagnostic> diagnostics = new();
	        internal ParserContext() { }
	        internal int Depth { get; set; }
	        internal string Path { get; set; } = "/";
	        internal QueryNode? Parent { get; set; }
	        internal bool ThrowExceptionOnDiagnosticError { get; init; }
	        internal Encoding Encoding { get; init; } = Encoding.UTF8;
	        internal IEnumerable<Diagnostic> Diagnostics => this.diagnostics;
	        internal void AddDiagnostic(Diagnostic diagnostic)
	        {
	            diagnostics.Enqueue(diagnostic);
	        }
	    }
	}
	public sealed partial class QueryParser
	{
	    private readonly QueryParserOptions options;
	    public QueryParser()
	    {
	        options = QueryParserOptions.Default;
	    }
	    public QueryParser(QueryParserOptions options)
	    {
	        if (options is null)
	        {
	            throw new ArgumentNullException(nameof(options));
	        }
	        this.options = options;
	    }
	    public QueryDocument Parse(string query)
	    {
	        try
	        {
	            var buffer = options.Encoding.GetBytes(query);
	            var lexer = new TokenLexer(buffer, new()
	            {
	                SkipCarriageReturn = true,
	                SkipLineFeed = true,
	                SkipTabs = true,
	                SkipWhiteSpace = true,
	                SkipComments = true,
	                Encoding = options.Encoding
	            });
	            var context = new ParserContext()
	            {
	                //Root = options.VertexFactory is null ? RootNode.Create(): new(options.VertexFactory.Invoke()),
	                Encoding = options.Encoding,
	                ThrowExceptionOnDiagnosticError = options.ThrowExceptionOnDiagnosticError
	            };
	            // NOTE: The Parser is responsible for only syntax diagnostics
	            //       Analyzers will be responsible for semantic diagnostics.
	            var node = ParseRoot(ref lexer, context);
	            var document = new QueryDocument(
	                query,
	                node!,
	                context.Diagnostics);
	            Analyze(document, options.AnalyzerTimeout);
	            return document;
	        }
	        catch (OperationCanceledException exception)
	        {
	            throw;
	        }
	        catch (TokenLexerException exception)
	        {
	            throw new QueryParserException(exception);
	        }
	    }
	    private void Analyze(QueryDocument document, TimeSpan timeout)
	    {
	        using var cancellationTokenSource = new CancellationTokenSource(timeout); // Max 10 seconds for analysis
	#if !DEBUG
	        cancellationTokenSource.Token.ThrowIfCancellationRequested();
	#endif
	        var analyzers = new List<Task>();
	        foreach (var analyzer in options.Analyzers)
	        {
	            analyzers.Add(analyzer.AnalyzeAsync(document, cancellationTokenSource.Token));
	        }
	        while (analyzers.Any())
	        {
	            var task = Task.WhenAny(analyzers);
	            while (!task.IsCompleted)
	            {
	                if (cancellationTokenSource.IsCancellationRequested)
	                {
	                    throw new OperationCanceledException(cancellationTokenSource.Token);
	                }
	            }
	            analyzers.Remove(task.Result);
	        }
	    }
	    #region Helper Methods
	    private void SkipToClosingBracket(ref TokenLexer lexer)
	    {
	        Token token;
	        while (lexer.TryNext(out token))
        {
            if (token.TokenType == TokenType.OpenBracket)
	            {
	                break;
	            }
        }
	    }
	    private void SkipToClosingParenthesis(ref TokenLexer lexer)
	    {
	        Token token;
	        while (lexer.TryNext(out token))
        {
            if (token.TokenType == TokenType.OpenBracket)
	            {
	                break;
	            }
        }
	    }
	    #endregion
	    private void AddInvalidTokenDiagnostic(ref TokenLexer lexer, ParserContext context)
	    {
	        context.AddDiagnostic(new Diagnostic()
	        {
	            Code = DiagnosticCode.G0006.ToString(),
	            Message = "Invalid Token.",
	            Start = lexer.Current.Start,
	            End = lexer.Current.End,
	            Line = lexer.Current.Line,
	            Location = DiagnosticLocation.Relative,
	            Severity = DiagnosticSeverity.Error,
	        });
	    }
	    private void AddEofDiagnostic(ref TokenLexer lexer, ParserContext context)
	    {
	        context.AddDiagnostic(new Diagnostic()
	        {
	            Code = DiagnosticCode.G0000.ToString(),
	            Message = "Unexpected EOF (End-Of-File).",
	            Start = lexer.Current.Start,
	            End = lexer.Current.End,
	            Line = lexer.Current.Line,
	            Location = DiagnosticLocation.Relative,
	            Severity = DiagnosticSeverity.Error,
	        });
	    }
	    private void AddExpectedOpenParenDiagnostic(ref TokenLexer lexer, ParserContext context)
	    {
	        context.AddDiagnostic(new Diagnostic()
	        {
	            Code = DiagnosticCode.G0001.ToString(),
	            Message = "Expected: '('",
	            Start = lexer.Current.Start,
	            End = lexer.Current.End,
	            Line = lexer.Current.Line,
	            Location = DiagnosticLocation.Absolute,
	            Severity = DiagnosticSeverity.Error,
	        });
	    }
	    private void AddExpectedOpenBracketDiagnostic(ref TokenLexer lexer, ParserContext context)
	    {
	        context.AddDiagnostic(new Diagnostic()
	        {
	            Code = DiagnosticCode.G0003.ToString(),
	            Message = "Expected: '{'",
	            Start = lexer.Current.Start,
	            End = lexer.Current.End,
	            Line = lexer.Current.Line,
	            Location = DiagnosticLocation.Absolute,
	            Severity = DiagnosticSeverity.Error,
	        });
	    }
	    private void AddExpectedDotSeparatorDiagnostic(ref TokenLexer lexer, ParserContext context)
	    {
	        context.AddDiagnostic(new Diagnostic()
	        {
	            Code = DiagnosticCode.G0005.ToString(),
	            Message = "Expected: '.'.",
	            Start = lexer.Current.Start,
	            End = lexer.Current.End,
	            Line = lexer.Current.Line,
	            Location = DiagnosticLocation.Relative,
	            Severity = DiagnosticSeverity.Error,
	        });
	    }
	    private void AddExpectedClosingParenDiagnostic(ref TokenLexer lexer, ParserContext context)
	    {
	        context.AddDiagnostic(new Diagnostic()
	        {
	            Code = DiagnosticCode.G0002.ToString(),
	            Message = "Expected: ')'.",
	            Start = lexer.Current.Start,
	            End = lexer.Current.End,
	            Line = lexer.Current.Line,
	            Location = DiagnosticLocation.Absolute,
	            Severity = DiagnosticSeverity.Error,
	        });
	    }
	    private void AddExpectedIntegerDiagnostic(ref TokenLexer lexer, ParserContext context)
	    {
	        context.AddDiagnostic(new Diagnostic()
	        {
	            Code = DiagnosticCode.G0008.ToString(),
	            Message = "Expected Integer.",
	            Start = lexer.Current.Start,
	            End = lexer.Current.End,
	            Line = lexer.Current.Line,
	            Location = DiagnosticLocation.Absolute,
	            Severity = DiagnosticSeverity.Error,
	        });
	    }
	    //protected void AddDuplicateKeywordDiagnostic(ref TokenLexer lexer, ParserContext context)
	    //{
	    //}
	}
	public sealed partial class QueryParser
	{
	    private FilterNode? ParseFilter(ref TokenLexer lexer, ParserContext context)
	    {
	        Token token = lexer.Current;
	        // Capture the dot notation if the previous node was chained.
	        if (lexer.Previous.TokenType == TokenType.Dot)
	        {
	            token = lexer.Previous;
	        }
	        Int32 start = token.Start;
	        Int32 startLine = token.Line;
	        Int32 end;
	        Int32 endLine;
	        // Ensure next token is an Open Parenthesis Block
	        if (!lexer.TryNext(out token))
	        {
	            AddEofDiagnostic(ref lexer, context);
	            return null;
	        }
	        if (token.TokenType != TokenType.OpenParenthesis || lexer.Previous.TokenType != TokenType.Filter)
	        {
	            AddExpectedOpenParenDiagnostic(ref lexer, context);
	            return null;
	        }
	        throw new NotImplementedException();
	    }
	}
	public sealed partial class QueryParser
	{
	    private PageNode? ParsePage(ref TokenLexer lexer, ParserContext context)
	    {
	        Token token = lexer.Current;
	        // Capture the dot notation if the previous node was chained.
	        if (lexer.Previous.TokenType == TokenType.Dot)
	        {
	            token = lexer.Previous;
	        }
	        Int32 start = token.Start;
	        Int32 startLine = token.Line;
	        Int32 end;
	        Int32 endLine;
	        ConstantNode? skip = null;
	        ConstantNode? take = null;
	        // Ensure next token is an Open Parenthesis Block
	        if (!lexer.TryNext(out token))
	        {
	            AddEofDiagnostic(ref lexer, context);
	            return null;
	        }
	        if (token.TokenType != TokenType.OpenParenthesis || lexer.Previous.TokenType != TokenType.Page)
	        {
	            AddExpectedOpenParenDiagnostic(ref lexer, context);
	            return null;
	        }
	        // Ensure next token is bracket block
	        if (lexer.TryPeek(out token) && token.TokenType != TokenType.OpenBracket)
	        {
	            AddExpectedOpenBracketDiagnostic(ref lexer, context);
	            return null;
	        }
	        // Parse Parenthesis Block
	        while (lexer.TryNext(out token))
	        {
	            if (token.TokenType == TokenType.CloseParenthesis)
	            {
	                // Capture ending position and line
	                end = token.End;
	                endLine = token.Line;
	                // If there is more token after the closing parenthesis and no dot separator, then error
	                if (lexer.TryNext(out token) && token.TokenType != TokenType.Dot)
	                {
	                    AddExpectedDotSeparatorDiagnostic(ref lexer, context);
	                }
	                var text = lexer.GetText(start, end);
	                var location = Location.Create(startLine, endLine, start, end);
	                return new PageNode(
	                    skip!, 
	                    take!,
	                    text,
	                    location);
	            }
	            while (lexer.TryNext(out token))
	            {
	                if (token.TokenType == TokenType.CloseBracket)
	                {
	                    break;
	                }
	                if (token.TokenType == TokenType.Skip)
	                {
	                    if (lexer.TryNext(out token) && token.TokenType != TokenType.Integer)
	                    {
	                        AddExpectedIntegerDiagnostic(ref lexer, context);
	                        return null;
	                    }
	                    skip = ParseConstant(ref lexer, context);
	                }
	                else if (token.TokenType == TokenType.Take)
	                {
	                    if (lexer.TryNext(out token) && token.TokenType != TokenType.Integer)
	                    {
	                        AddExpectedIntegerDiagnostic(ref lexer, context);
	                        return null;
	                    }
	                    take = ParseConstant(ref lexer, context);
	                }
	                else
	                {
	                    SkipToClosingBracket(ref lexer);
	                    break;
	                }
	            }
	        }
	        AddExpectedClosingParenDiagnostic(ref lexer, context);
	        return null;
	    }
	}
	public sealed partial class QueryParser
	{
	    private ProjectNode? ParseProject(ref TokenLexer lexer, ParserContext context)
	    {
	        Token token = lexer.Current;
	        // Capture the dot notation if the previous node was chained.
	        if (lexer.Previous.TokenType == TokenType.Dot)
	        {
	            token = lexer.Previous;
	        }
	        Int32 start = token.Start;
	        Int32 startLine = token.Line;
	        Int32 end;
	        Int32 endLine;
	        Location location;
	        String text;
	        // Ensure next token is an Open Parenthesis Block
	        if (!lexer.TryNext(out token))
	        {
	            AddEofDiagnostic(ref lexer, context);
	            return null;
	        }
	        if (token.TokenType != TokenType.OpenParenthesis || lexer.Previous.TokenType != TokenType.Project)
	        {
	            AddExpectedOpenParenDiagnostic(ref lexer, context);
	            return null;
	        }
	        // Ensure next token is bracket block
	        if (lexer.TryPeek(out token) && token.TokenType != TokenType.OpenBracket)
	        {
	            AddExpectedOpenBracketDiagnostic(ref lexer, context);
	            return null;
	        }
	        var properties = new List<PropertyNode>();
	        while (lexer.TryNext(out token))
	        {
	            if (token.TokenType == TokenType.CloseParenthesis)
	            {
	                // Capture ending position and line
	                end = lexer.Position;
	                endLine = lexer.Line;
	                text = lexer.GetText(start, end);
	                location = Location.Create(startLine, endLine, start, end);
	                // If there is more token after the closing parenthesis and no dot separator, then error
	                if (lexer.TryNext(out token) && token.TokenType != TokenType.Dot)
	                {
	                    AddExpectedDotSeparatorDiagnostic(ref lexer, context);
	                }
	                return new ProjectNode(
	                    properties,
	                    text,
	                    location);
	            }
	            while (lexer.TryNext(out token))
	            {
	                if (token.TokenType == TokenType.CloseBracket)
	                {
	                    break;
	                }
	                if (token.TokenType == TokenType.Identifier)
	                {
	                    var property = ParseProperty(ref lexer, context);
	                    if(property is null)
	                    {
	                        continue;
	                    }
	                    properties.Add(property);
	                }
	            }
	        }
	        AddExpectedClosingParenDiagnostic(ref lexer, context);
	        return null;
	    }
	}
	public sealed partial class QueryParser
	{
	    private PropertyNode? ParseProperty(ref TokenLexer lexer, ParserContext context)
	    {
	        Token token = lexer.Current;
	        Int32 start = lexer.Current.Start;
	        Int32 startLine = lexer.Current.Line;
	        Int32 end;
	        Int32 endLine;
	        String text;
	        Location location;
	        string? name = lexer.Current.Text;
	        string? alias;
	        // Ensure current token is Identifier
	        if (token.TokenType != TokenType.Identifier)
	        {
	            // TODO: Expected Identifier Diagnostic
	            return null;
	        }
	        // Check for an alias or nested property
	        if (lexer.TryPeek(out token) && token.TokenType == TokenType.Alias || token.TokenType == TokenType.OpenBracket)
	        {
	            token = lexer.Next();
	        }
	        else
	        {
	            end = lexer.Position;
	            endLine = lexer.Line;
	            text = lexer.GetText(start, end);
	            location = Location.Create(startLine, endLine, start, end);
	            return new PropertyNode(
	                name,
	                text,
	                location);
	        }
	        if (token.TokenType == TokenType.Alias)
	        {
	            if (!lexer.TryPeek(out token) || token.TokenType != TokenType.Identifier)
	            {
	                // TODO: Expected Identifier
	                return null;
	            }
	            token = lexer.Next();
	            alias = token.Text;
	            // Check for nested properties following alias
	            if (lexer.TryPeek(out token) && token.TokenType == TokenType.OpenBracket)
	            {
	                lexer.Skip();
	                var properties = new List<PropertyNode>();
	                while (lexer.TryNext(out token))
	                {
	                    if (token.TokenType == TokenType.CloseBracket)
	                    {
	                        end = lexer.Position;
	                        endLine = lexer.Line;
	                        text = lexer.GetText(start, end);
	                        location = Location.Create(startLine, endLine, start, end);
	                        return new PropertyNode(
	                            name,
	                            alias,
	                            properties,
	                            text,
	                            location);
	                    }
	                    if (token.TokenType == TokenType.Identifier)
	                    {
	                        var property = ParseProperty(ref lexer, context);
	                        if (property is null)
	                        {
	                            continue;
	                        }
	                        properties.Add(property);
	                    }
	                    else
	                    {
	                        SkipToClosingBracket(ref lexer);
	                        // TODO: Add Diagnostics
	                        end = lexer.Position;
	                        endLine = lexer.Line;
	                        text = lexer.GetText(start, end);
	                        location = Location.Create(startLine, endLine, start, end);
	                        return new PropertyNode(
	                            name,
	                            alias,
	                            properties,
	                            text,
	                            location);
	                    }
	                }
	            }
	            end = lexer.Position;
	            endLine = lexer.Line;
	            text = lexer.GetText(start, end);
	            location = Location.Create(startLine, endLine, start, end);
	            return new PropertyNode(
	                name,
	                alias,
	                text,
	                location);
	        }
	        if (token.TokenType == TokenType.OpenBracket)
	        {
	            var properties = new List<PropertyNode>();
	            while (lexer.TryNext(out token))
	            {
	                if (token.TokenType == TokenType.CloseBracket)
	                {
	                    end = lexer.Position;
	                    endLine = lexer.Line;
	                    text = lexer.GetText(start, end);
	                    location = Location.Create(startLine, endLine, start, end);
	                    return new PropertyNode(
	                        name,
	                        properties,
	                        text,
	                        location);
	                }
	                if (token.TokenType == TokenType.Identifier)
	                {
	                    var property = ParseProperty(ref lexer, context);
	                    if (property is null)
	                    {
	                        continue;
	                    }
	                    properties.Add(property!);
	                }
	                else
	                {
	                    SkipToClosingBracket(ref lexer);
	                    // TODO: Add Diagnostics
	                    end = lexer.Position;
	                    endLine = lexer.Line;
	                    text = lexer.GetText(start, end);
	                    location = Location.Create(startLine, endLine, start, end);
	                    return new PropertyNode(
	                        name,
	                        properties,
	                        text,
	                        location);
	                }
	            }
	        }
	        AddEofDiagnostic(ref lexer, context);
	        return null;
	    }
	}
	public sealed partial class QueryParser
	{
	    private RootNode? ParseRoot(ref TokenLexer lexer, ParserContext context)
	    {
	        Token token;
	        Int32 start = lexer.Current.Start;
	        Int32 startLine = lexer.Current.Line;
	        Int32 end;
	        Int32 endLine;
	        Location location;
	        String text;
	        if (lexer.TryPeek(out token) && token.TokenType == TokenType.Vertex)
	        {
	            lexer.Skip();
	            var vertex = ParseVertex(ref lexer, context);
	            if (vertex is null)
	            {
	                return null;
	            }
	            end = lexer.Position;
	            endLine = lexer.Line;
	            text = lexer.GetText(start, end);
	            location = Location.Create(startLine, endLine, start, end);
	            return new RootNode(
	                vertex,
	                text,
	                location);
	        }
	        var nodes = new List<QueryNode>();
	        while (lexer.TryNext(out token))
	        {
	            QueryNode? current = token.TokenType switch
	            {
	                TokenType.Project => ParseProject(ref lexer, context),
	                TokenType.Sort => ParseSort(ref lexer, context),
	                TokenType.Filter => ParseFilter(ref lexer, context),
	                TokenType.Page => ParsePage(ref lexer, context),
	                _ => null
	            };
	            if (current is null)
	            {
	                // TODO: Add unexpected token diagnostic
	                return null;
	            }
	            else
	            {
	                nodes.Add(current);
	            }
	        }
	        // Capture ending position and line
	        end = lexer.Position;
	        endLine = lexer.Line;
	        text = lexer.GetText(start, end);
	        location = Location.Create(start, endLine, start, end);
	        return new RootNode(
	            new VertexNode(nodes),
	            text,
	            location);
	    }
	}
	public sealed partial class QueryParser
	{
	    private SortNode? ParseSort(ref TokenLexer lexer, ParserContext context)
	    {
	        Token token = lexer.Current;
	        // Capture the dot notation if the previous node was chained.
	        if (lexer.Previous.TokenType == TokenType.Dot)
	        {
	            token = lexer.Previous;
	        }
	        Int32 start = token.Start;
	        Int32 startLine = token.Line;
	        Int32 end;
	        Int32 endLine;
	        // Ensure next token is an Open Parenthesis Block
	        if (!lexer.TryNext(out token))
	        {
	            AddEofDiagnostic(ref lexer, context);
	            return null;
	        }
	        if (token.TokenType != TokenType.OpenParenthesis || lexer.Previous.TokenType != TokenType.Page)
	        {
	            AddExpectedOpenParenDiagnostic(ref lexer, context);
	            return null;
	        }
	        // Ensure next token is bracket block
	        if (lexer.TryPeek(out token) && token.TokenType != TokenType.OpenBracket)
	        {
	            AddExpectedOpenBracketDiagnostic(ref lexer, context);
	            return null;
	        }
	        // Parse Parenthesis Block
	        while (lexer.TryNext(out token))
	        {
	            if (token.TokenType == TokenType.CloseParenthesis)
	            {
	                // If there is more token after the closing parenthesis and no dot separator, then error
	                if (lexer.TryPeek(out var peek) && peek.TokenType != TokenType.Dot)
	                {
	                    lexer.Next();
	                    AddExpectedDotSeparatorDiagnostic(ref lexer, context);
	                }
	            }
	        }
	        throw new NotImplementedException();
	    }
	}
	public sealed partial class QueryParser
	{
	    private VertexNode? ParseVertex(ref TokenLexer lexer, ParserContext context)
	    {
	        Token token;
	        Int32 start = lexer.Current.Start;
	        Int32 startLine = lexer.Current.Line;
	        Int32 end;
	        Int32 endLine;
	        VertexNode vertex;
	        ConstantNode? argument = null;
	        // Ensure next token is an Open Parenthesis Block
	        if (!lexer.TryNext(out token))
	        {
	            AddEofDiagnostic(ref lexer, context);
	            return null;
	        }
	        if (token.TokenType != TokenType.OpenParenthesis || 
	            lexer.Previous.TokenType != TokenType.Vertex)   // <-- This checks ensures that there is no invalid character
	        {                                                   // between the key word and the open parenthesis
	            AddExpectedOpenParenDiagnostic(ref lexer, context);
	            return null;
	        }
	        // Get Vertex Identifier
	        if (!lexer.TryNext(out token) || token.TokenType != TokenType.Identifier)
	        {
	            // TODO: Add Diagnostics
	            return null;
	        }
	        var hasNext = lexer.TryNext(out token);
	        if (!hasNext || (token.TokenType != TokenType.CloseParenthesis && token.TokenType != TokenType.Comma))
	        {
	            AddExpectedClosingParenDiagnostic(ref lexer, context);
	            return null;
	        }
	        // Check for Literal Argument 
	        if (token.TokenType == TokenType.Comma)
	        {
	            if (lexer.TryNext(out token) && (
	                token.TokenType == TokenType.String ||
	                token.TokenType == TokenType.FloatingPoint ||
	                token.TokenType == TokenType.Integer))
	            {
	                argument = ParseConstant(ref lexer, context);
	                lexer.TryNext(out token);
	            }
	        }
	        if (token.TokenType != TokenType.CloseParenthesis)
	        {
	            AddExpectedClosingParenDiagnostic(ref lexer, context);
	            return null;
	        }
	        vertex = argument is null
	            ? new VertexNode()
	            : new VertexNode(argument, []);
	        if (lexer.TryNext(out token) && token.TokenType != TokenType.Dot)
	        {
	            AddExpectedDotSeparatorDiagnostic(ref lexer, context);
	            return null;
	        }
	        while (lexer.TryNext(out token))
	        {
	            if (token.TokenType == TokenType.Edge)
	            {
	                if (!ParseEdge(ref lexer))
	                {
	                    return null;
	                }
	            }
	            else
	            {
	                QueryNode? node = token.TokenType switch
	                {
	                    TokenType.Page => ParsePage(ref lexer, context),
	                    TokenType.Sort => ParseSort(ref lexer, context),
	                    TokenType.Filter => ParseFilter(ref lexer, context),
	                    TokenType.Project => ParseProject(ref lexer, context),
	                    _ => null
	                };
	                if (node is null)
	                {
	                    // TODO: Add Diagnostics
	                    continue;
	                }
	                vertex.AddNode(node);
	            }
	        }
	        return vertex;
	        bool ParseEdge(ref TokenLexer lexer )
	        {
	            LabelNode? label = null;
	            LabelNode? alias = null;
	            VertexNode? target = null;
	            VertexNode? source = vertex;
	            string path = "";
	            Token token = lexer.Current;
	            // Capture the dot notation if the previous node was chained.
	            if (lexer.Previous.TokenType == TokenType.Dot)
	            {
	                token = lexer.Previous;
	            }
	            Int32 start = token.Start;
	            Int32 startLine = token.Line;
	            Int32 end;
	            Int32 endLine;
	            String text;
	            Location location;
	            // Ensure next token is an Open Parenthesis Block
	            if (!lexer.TryNext(out token))
	            {
	                AddEofDiagnostic(ref lexer, context);
	                return false;
	            }
	            if (token.TokenType != TokenType.OpenParenthesis || lexer.Previous.TokenType != TokenType.Edge)
	            {
	                AddExpectedOpenParenDiagnostic(ref lexer, context);
	                return false;
	            }
	            string? current = null;
	            while (lexer.TryNext(out token))
	            {
	                if (token.TokenType != TokenType.Identifier)
	                {
	                    // TODO: Add Diagnostics
	                    return false;
	                }
	                else
	                {
	                    current = token.Text;
	                    path += "/" + current;
	                }
	                if (!lexer.TryNext(out token))
	                {
	                    // TODO: Expected slash or comma
	                    return false;
	                }
	                if (token.TokenType == TokenType.Alias)
	                {
	                    if (!lexer.TryNext(out token) || token.TokenType != TokenType.Identifier)
	                    {
	                        // TODO: Add Diagnostics
	                    }
	                    alias = new LabelNode(token.Text);
	                    if (!lexer.TryNext(out token) || token.TokenType != TokenType.CloseParenthesis)
	                    {
	                        // TODO: Add Diagnostics
	                        return false;
	                    }
	                }
	                if (token.TokenType == TokenType.Slash)
	                {
	                    if (lexer.Previous.TokenType != TokenType.Identifier)
	                    {
	                        //TODO: Add Diagnostic - There can't be any other token between the slash and identifier
	                        return false;
	                    }
	                    var item = source!.Nodes.FirstOrDefault(p =>
	                    {
	                        return p is EdgeNode edge && edge?.Label?.Name == current;
	                    }) as EdgeNode;
	                    if (item is null)
	                    {
	                        // Invalid Path
	                        return false;
	                    }
	                    source = item.Target;
	                    continue;
	                }
	                else if (token.TokenType != TokenType.CloseParenthesis)
	                {
	                    AddExpectedClosingParenDiagnostic(ref lexer, context);
	                    return false;
	                }
	                else
	                {
	                    break;
	                }
	            }
	            target = new VertexNode();
	            label = new LabelNode(current!);
	            // 
	            if (!lexer.TryNext(out token))
	            {
	                return false;
	            }
	            if (token.TokenType != TokenType.Dot)
	            {
	                AddExpectedDotSeparatorDiagnostic(ref lexer, context);
	                return false;
	            }
	            while (lexer.TryNext(out token))
	            {
	                QueryNode? node = token.TokenType switch
	                {
	                    TokenType.Page => ParsePage(ref lexer, context),
	                    TokenType.Sort => ParseSort(ref lexer, context),
	                    TokenType.Filter => ParseFilter(ref lexer, context),
	                    TokenType.Project => ParseProject(ref lexer, context),
	                    _ => null
	                };
	                if (node is null)
	                {
	                    continue;
	                }
	                target.AddNode(node);
	                if (!lexer.TryPeek(out token) || token.TokenType == TokenType.Edge)
	                {
	                    end = lexer.Position;
	                    endLine = lexer.Line;
	                    text = lexer.GetText(start, end);
	                    location = Location.Create(startLine, endLine, start, end);
	                    source!.AddNode(new EdgeNode(
	                        label,
	                        source!,
	                        target,
	                        text,
	                        location,
	                        alias,
	                        path));
	                    return true;
	                }
	            }
	            return false;
	        }
	    }
	}
	public sealed class QueryParserOptions
	{
	    private readonly IList<QueryAnalyzer> analyzers;
	    public QueryParserOptions()
	    {
	        this.analyzers = new List<QueryAnalyzer>()
	        {
	            new InvalidNodeTypesInVertexAnalyzer()
	        };
	    }
	    public bool ThrowExceptionOnDiagnosticError { get; set; }
	    public Encoding Encoding { get; set; } = Encoding.UTF8;
	    public int MaxEdgeDepth { get; set; } = 5;
	    public string? StartingVertexName { get; set; }
	    public TimeSpan AnalyzerTimeout { get; set; } = TimeSpan.FromSeconds(5);
	    internal IEnumerable<QueryAnalyzer> Analyzers => this.analyzers;
	    public void AddAnalyzer(QueryAnalyzer analyzer)
	    {
	        if (analyzer is null)
	        {
	            throw new ArgumentNullException(nameof(analyzer));
	        }
	        analyzers.Add(analyzer);
	    }
	    public void AddAnalyzer<TAnalyzer>() where TAnalyzer : QueryAnalyzer, new()
	    {
	        AddAnalyzer(new TAnalyzer());
	    }
	    public static QueryParserOptions Default => new QueryParserOptions();
	}
	#endregion
	#region \Abstractions
	public interface IQueryNodeVisitor
	{
	    void Visit(QueryNode queryNode);
	    void Visit(VertexNode queryNode);
	    void Visit(FilterNode queryNode);
	    void Visit(ProjectNode queryNode);
	    void Visit(PageNode queryNode);
	    void Visit(SortNode queryNode);
	    void Visit(BinaryNode queryNode);
	    void Visit(PropertyNode queryNode);
	    void Visit(ParameterNode queryNode);
	    void Visit(FunctionCallNode queryNode);
	    void Visit(ConstantNode queryNode);
	    void Visit(EdgeNode queryNode);
	}
	public interface IQueryNodeVisitor<T>
	{
	    T Visit(QueryNode queryNode);
	    T Visit(VertexNode queryNode);
	    T Visit(FilterNode queryNode);
	    T Visit(ProjectNode queryNode);
	    T Visit(PageNode queryNode);
	    T Visit(SortNode queryNode);
	    T Visit(BinaryNode queryNode);
	    T Visit(PropertyNode queryNode);
	    T Visit(ParameterNode queryNode);
	    T Visit(FunctionCallNode queryNode);
	    T Visit(ConstantNode queryNode);
	    T Visit(EdgeNode queryNode);
	}
	#endregion
	#region \Analyzer
	public abstract class QueryAnalyzer
	{
	    public abstract Task AnalyzeAsync(QueryDocument document, CancellationToken cancellationToken = default);
	}
	#endregion
	#region \Diagnostics
	[DebuggerDisplay("{Severity}: ({Start}..{Start+Length}): {Message}")]
	public sealed partial class Diagnostic
	{
	    public Diagnostic() { }
	    public Diagnostic(string code, string? message, int start, int end, DiagnosticSeverity severity)
	    {
	        Code = code;
	        Message = message;
	        Start = start;
	        End = end;
	        Severity = severity;
	    }
	    internal Diagnostic(DiagnosticCode code, string? message, int start, int end, DiagnosticSeverity severity)
	    {
	        Code = code.ToString();
	        Message = message;
	        Start = start;
	        End = end;
	        Severity = severity;
	    }
	    public string? Code { get; init; }
	    public DiagnosticLocation? Location { get; init; }
	    public string? Message { get; init; }
	    public int? Length => End - Start;
	    public int? Start { get; init; }
	    public int? End { get; init; }
	    public int Line { get; init; } = 1;
	    public DiagnosticSeverity? Severity { get; init; }
	}
	public sealed partial class Diagnostic
	{
	    internal static Diagnostic UnexpectedEOF(int end)
	    {
	        return new Diagnostic()
	        {
	            Code = DiagnosticCode.G0000.ToString(),
	            Message = "Unexpected EOF (End-Of-File).",
	            Start = end,
	            End = end,
	            Location = DiagnosticLocation.Relative,
	            Severity = DiagnosticSeverity.Error,
	        };
	    }
	    internal static Diagnostic ExpectedOpeningParenthesis(int start, int end)
	    {
	        return new Diagnostic()
	        {
	            Code = DiagnosticCode.G0001.ToString(),
	            Message = "Expected: '('",
	            Start = start,
	            End = end,
	            Location = DiagnosticLocation.Absolute,
	            Severity = DiagnosticSeverity.Error,
	        };
	    }
	    internal static Diagnostic ExpectedClosingParenthesis(int start, int end)
	    {
	        return new Diagnostic()
	        {
	            Code = DiagnosticCode.G0002.ToString(),
	            Message = "Expected: ')'",
	            Start = start,
	            End = end,
	            Location = DiagnosticLocation.Absolute,
	            Severity = DiagnosticSeverity.Error,
	        };
	    }
	    internal static Diagnostic ExpectedOpeningBracket(int start, int end)
	    {
	        return new Diagnostic()
	        {
	            Code = DiagnosticCode.G0003.ToString(),
	            Message = "Expected: '{'",
	            Start = start,
	            End = end,
	            Location = DiagnosticLocation.Absolute,
	            Severity = DiagnosticSeverity.Error,
	        };
	    }
	    internal static Diagnostic ExpectedClosingBracket(int start, int end)
	    {
	        return new Diagnostic()
	        {
	            Code = DiagnosticCode.G0004.ToString(),
	            Message = "Expected: '}'",
	            Start = start,
	            End = end,
	            Location = DiagnosticLocation.Absolute,
	            Severity = DiagnosticSeverity.Error,
	        };
	    }
	    internal static Diagnostic ExpectedCommaSeparator(int start, int end)
	    {
	        return new Diagnostic()
	        {
	            Code = DiagnosticCode.G0005.ToString(),
	            Message = "Expected: ','",
	            Start = start,
	            End = end,
	            Location = DiagnosticLocation.Relative,
	            Severity = DiagnosticSeverity.Error,
	        };
	    }
	    internal static Diagnostic ExpectedDotSeparator(int start, int end)
	    {
	        return new Diagnostic()
	        {
	            Code = DiagnosticCode.G0005.ToString(),
	            Message = "Expected: '.'",
	            Start = start,
	            End = end,
	            Location = DiagnosticLocation.Relative,
	            Severity = DiagnosticSeverity.Error,
	        };
	    }
	    internal static Diagnostic InvalidToken(ref Token token)
	    {
	        return new Diagnostic()
	        {
	            Code = DiagnosticCode.G0007.ToString(),
	            Message = $"Invalid Token: {token}",
	            Start = token.Start,
	            End = token.End,
	            Location = DiagnosticLocation.Relative,
	            Severity = DiagnosticSeverity.Error,
	        };
	    }
	}
	internal static class DiagnosticCategory
	{
	    //private static readonly string Correctness => "Correctness";
	}
	internal enum DiagnosticCode
	{
	    G0000,
	    G0001,
	    G0002,
	    G0003,
	    G0004, 
	    G0005,
	    G0006,
	    G0007,
	    G0008,
	}
	public enum DiagnosticLocation
	{
	    // <summary>
	    Absolute,
	    Relative,
	    RelativeEnd
	}
	public enum DiagnosticSeverity
	{
	    None,
	    Warning,
	    Error,
	    Suggestion,
	    Information,
	    Hidden
	}
	#endregion
	#region \Exceptions
	public sealed class QueryParserException : Exception
	{
	    public QueryParserException(string message) : base(message) { }
	    internal QueryParserException(TokenLexerException exception) { }
	    internal static QueryParserException UnexpectedQueryNode(Type expected, Type actual)
	    {
	        throw new QueryParserException($"An invalid token was passed while paring. Expected '{expected}'. Actual '{actual}'.");
	    }
	}
	#endregion
	#region \Extensions
	public static class QueryNodeExtensions
	{
	    //public static bool TryGetProjection(this )
	}
	#endregion
	#region \Internal\Analyzers
	/*
	 This analyzer ensure that the only nodes in a Vertex Node are valid.
	 */
	internal class InvalidNodeTypesInVertexAnalyzer : QueryAnalyzer
	{
	    public override Task AnalyzeAsync(QueryDocument document, CancellationToken cancellationToken = default)
	    {
	        return Task.Run(() =>
	        {
	            //var root = document.Root;
	            //foreach (var vertexNode in root.GetNodesOfType<VertexNode>())
	            //{
	            //    foreach (var node in vertexNode.Nodes)
	            //    {
	            //        // These are the only acceptable nodes in 
	            //        if (node is not ProjectNode &&
	            //            node is not FilterNode &&
	            //            node is not PageNode &&
	            //            node is not SortNode)
	            //        {
	            //            document.AddDiagnostic(new Diagnostic()
	            //            {
	            //            });
	            //        }
	            //    }
	            //}
	        });
	    }
	}
	internal class InvalidPageConstantsAnalyzer : QueryAnalyzer
	{
	    public override Task AnalyzeAsync(QueryDocument document, CancellationToken cancellationToken = default)
	    {
	        return Task.Run(() =>
	        {
	            if (document.Root is not null)
	            {
	                foreach (var node in document.Root.GetNodesOfType<PageNode>())
	                {
	                    //node.Take.
	                }
	            }
	        });
	    }
	}
	internal class MissingProjectAnalyzer : QueryAnalyzer
	{
	    public override Task AnalyzeAsync(QueryDocument document, CancellationToken cancellationToken = default)
	    {
	        return Task.Run(() =>
	        {
	            if (document.Root is not null)
	            {
	                var vertices = document.Root.GetNodesOfType<VertexNode>();
	                foreach (var vertex in vertices)
	                {
	                    var projections = vertex.Nodes.OfType<ProjectNode>();
	                    if (!projections.Any())
	                    {
	                        // TODO: Add Missing Project Statement Diagnostic
	                    }
	                    if (projections.Count() > 1)
	                    {
	                        // TODO: 
	                    }
	                }
	            }
	        });
	    }
	}
	internal class UnknownRootVertexNodeAnalyzer : QueryAnalyzer
	{
	    public override Task AnalyzeAsync(QueryDocument document, CancellationToken cancellationToken = default)
	    {
	        return Task.Run(() =>
	        {
	            //var vertex = document.Root?.Vertex;
	            //if (vertex is { Label: not null, Label.Name: not null, Label.Name.Length: > 0 })
	            //{
	            //    // TODO: Add Unknown Vertex Diagnostics
	            //}
	        });
	    }
	}
	#endregion
	#region \Internal\Extensions
	internal static class ParserContextExtensions
	{
	    //internal static void AddUnexptedTokenError(this ParserContext context, ref TokenLexer lexer)
	    //{
	    //    context.AddDiagnostic(new Diagnostic()
	    //    {
	    //        Severity = DiagnosticSeverity.Error,
	    //        Location = DiagnosticLocation.Relative,
	    //        Start = lexer.Current.Start,
	    //        End = lexer.Current.End,
	    //        Message = $"Unexpected Token: {lexer.Current}"
	    //    });
	    //}
	    //internal static void AddUnexpectedEOFError(this ParserContext context, ref TokenLexer lexer)
	    //{
	    //    context.AddDiagnostic(new Diagnostic()
	    //    {
	    //        Code = DiagnosticCode.G0005.ToString(),
	    //        Message = $"Unexpected EOF (end-of-file) at '{lexer.Current.End}'.",
	    //        Start = lexer.Current.End,
	    //        End = lexer.Current.End,
	    //        Location = DiagnosticLocation.Relative,
	    //        Severity = DiagnosticSeverity.Error,
	    //    });
	    //}
	    //internal static void AddExpectedParenthesisDiagnosticError(this ParserContext context, int end)
	    //{
	    //}
	    //internal static void AddExpectedCommaSeparatorDiagnosticError(this ParserContext context, ref Token lexerToken)
	    //{
	    //    context.AddDiagnostic(new Diagnostic()
	    //    {
	    //        Severity = DiagnosticSeverity.Error,
	    //        Location = DiagnosticLocation.Absolute,
	    //        Start = lexerToken.Start,
	    //        End = lexerToken.End,
	    //        Message = $"Expected Comma"
	    //    });
	    //}
	}
	internal static class ParserExtensions
	{
	}
	internal static class SpanExtensions
	{
	    // This will cache the result of the specific memory. This will prevent constant re-checking of the same memory
	    private static ConcurrentDictionary<ReadOnlyMemory<byte>, (bool, FunctionType)> cache = new();
	    internal static byte[] GetBytes(string value) => Encoding.UTF8.GetBytes(value);
	    internal static ReadOnlySpan<KeyValuePair<FunctionType, byte[]>> Functions => new[]
	    {
	        new KeyValuePair<FunctionType, byte[]>(FunctionType.Contains, GetBytes("contains")),
	        new KeyValuePair<FunctionType, byte[]>(FunctionType.SubString, GetBytes("substring")),
	        new KeyValuePair<FunctionType, byte[]>(FunctionType.StartsWith, GetBytes("startswith")),
	    };
	    internal static bool IsFunction(this ReadOnlyMemory<byte> memory) => IsFunction(memory, out var functionType);
	    internal static bool IsFunction(this ReadOnlyMemory<byte> memory, out FunctionType functionType)
	    {
	        var result = cache.GetOrAdd(memory, mem =>
	        {
	            var identifier = mem.ToArray().Select(x => (byte)char.ToLowerInvariant((char)x));
	            foreach (var function in Functions)
	            {
	                if (identifier.SequenceEqual(function.Value))
	                {
	                    return (true, function.Key);
	                }
	            }
	            return (false, default(FunctionType));
	        });
	        functionType = result.Item2;
	        return result.Item1;
	    }   
	}
	#endregion
	#region \Internal\Lexer
	[DebuggerDisplay("{TokenType}: {Text} ")]
	internal readonly struct Token : IEquatable<Token>
	{
	    internal int Start { get; init; }
	    internal int End { get; init; }
	    internal int Line { get; init; }
	    internal ReadOnlyMemory<byte> Value { get; init; }
	    internal string Text { get; init; }
	    internal TokenType TokenType { get; init; }
	    internal bool IsKeyword
	    {
	        get
	        {
	            switch (TokenType)
	            {
	                case TokenType.Filter:
	                case TokenType.Project:
	                case TokenType.Sort:
	                case TokenType.Page:
	                case TokenType.Edge:
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
	    internal bool IsLiteral
	    {
	        get
	        {
	            switch (TokenType)
	            {
	                case TokenType.String:
	                case TokenType.FloatingPoint:
	                case TokenType.Boolean:
	                case TokenType.Integer:
	                case TokenType.Null:
	                    return true;
	                default:
	                    return false;
	            }
	        }
	    }
	    internal bool IsOperator
	    {
	        get
	        {
	            switch (TokenType)
	            {
	                case TokenType.Plus:
	                case TokenType.Minus:
	                case TokenType.Star:
	                case TokenType.Slash:
	                case TokenType.Equal:
	                case TokenType.NotEqual:
	                case TokenType.GreaterThan:
	                case TokenType.GreaterThanOrEqual:
	                case TokenType.LessThan:
	                case TokenType.LessThanOrEqual:
	                case TokenType.And:
	                case TokenType.Or:
	                    return true;
	                default:
	                    return false;
	            }
	        }
	    }
	    internal bool IsIdentifier => TokenType == TokenType.Identifier;
	    public override string ToString()
	    {
	        return $"{TokenType} - {Text}";
	    }
	    public override int GetHashCode()
	    {
	        return HashCode.Combine(typeof(Token), Value);
	    }
	    public override bool Equals(object? obj)
	    {
	        if (obj is Token token)
	        {
	            return Equals(token);
	        }
	        return false;
	    }
	    public bool Equals(Token other)
	    {
	        var left = Value.Span;
	        var right = other.Value.Span;
	        return left.SequenceEqual(right);
	    }
	}
	internal ref partial struct TokenLexer
	{
	    private readonly TokenLexerOptions options;
	    private readonly ReadOnlySpan<byte> data;
	    private ReadOnlySequence<byte> remaining;
	    private Token current = default;
	    private Token previous = default;
	    private int position = default;
	    // Tracks the line position, if any, as the query is parsed.
	    private int line = 1;
	    public TokenLexer(byte[] query)
	    {
	        this.options = TokenLexerOptions.Default;
	        this.data = query;
	        this.remaining = new ReadOnlySequence<byte>(query);
	    }
	    public TokenLexer(byte[] query, TokenLexerOptions options) : this(query)
	    {
	        this.options = options;
	    }
	    #region Public Methods
	    public bool HasNext
	    {
	        get
	        {
	            var sequence = remaining; // Need to copy sequence into variable since we do not want to advance to the next sequence
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
	                        case TokenType.Comment when options.SkipComments:
	                            if (sequence.IsEmpty)
	                            {
	                                return false;
	                            }
	                            sequenceReader = new(sequence); // Reset sequence
	                            break;
	                        default:
	                            return true;
	                    }
	                }
	            }
	            return false;
	        }
	    }
	    public readonly int Line => line;
	    public readonly Token Current => current;
	    public readonly Token Previous => previous;
	    public readonly int Position => position;
	    public ReadOnlySpan<byte> ValueSpan => data;
	    public Token Peek()
	    {
	        var sequence = remaining; // Need to copy sequence into variable since we do not want to advance
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
	                    case TokenType.Comment when options.SkipComments:
	                        sequenceReader = new(sequence); // Reset sequence
	                        break;
	                    default:
	                        return token;
	                }
	            }
	        }
	        // If we reached here something is wrong within the syntax
	        throw new TokenLexerException("End of Sequence. No more tokens available.");
	    }
	    public Token Next()
	    {
	        var reader = new SequenceReader<byte>(remaining);
	        while (!reader.End)
	        {
	            reader.Advance(1);
	            if (TryParse(ref reader, out var token))
	            {
	                remaining = reader.UnreadSequence;
	                position += (int)reader.Consumed;
	                previous = current;
	                current = token;
	                // Capture Line Number
	                if (current.TokenType == TokenType.LineFeed && previous.TokenType == TokenType.CarriageReturn)
	                {
	                    line++;
	                }
	                switch (token.TokenType)
	                {
	                    case TokenType.Tab when options.SkipTabs:
	                    case TokenType.LineFeed when options.SkipLineFeed:
	                    case TokenType.WhiteSpace when options.SkipWhiteSpace:
	                    case TokenType.CarriageReturn when options.SkipCarriageReturn:
	                    case TokenType.Comment when options.SkipComments:
	                        reader = new(remaining); // Reset sequence
	                        break;
	                    default:
	                        return current;
	                }
	            }
	        }
	        // If we reached here something is wrong within the syntax
	        throw new TokenLexerException("End of Sequence. No more tokens available.");
	    }
	    public void Skip()
	    {
	        Next();
	    }
	    public void Skip(int count)
	    {
	        for (int i = 0; i < count; i++)
	        {
	            Next();
	        }
	    }
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
	    public string GetText(int start, int end)
	    {
	        var encoding = options.Encoding;
	        var span = ValueSpan.Slice(start, end - start);
	        return encoding.GetString(span);
	    }
	    #endregion
	    #region Private Methods
	    private bool TryParse(ref SequenceReader<byte> sequenceReader, out Token token)
	    {
	        token = default;
	        TokenType tokenType;
	        // NOTE: Do not change this order. Conditional evaluation is from left to right on purpose
	        if (IsSeparator(ref sequenceReader, out tokenType) ||
	            IsKeyword(ref sequenceReader, out tokenType) ||
	            IsLiteral(ref sequenceReader, out tokenType) ||
	            IsComment(ref sequenceReader, out tokenType) ||
	            IsOperator(ref sequenceReader, out tokenType) ||
	            IsIdentifier(ref sequenceReader, out tokenType)) // Identifier needs be checked last
	        {
	            var value = sequenceReader.Slice().ToArray();
	            // Temporary Fix: Remove single/double quotes from string
	            if (tokenType == TokenType.String)
	            {
	                value = value.Skip(1).Take(value.Length - 2).ToArray();
	            }
	            token = new Token()
	            {
	                Value = value,
	                Text = options.Encoding.GetString(value),
	                TokenType = tokenType,
	                Line = line,
	                Start = position, // explicit casting from long to int. If query is bigger than the max value of an int then developer needs to re-think his life decisions.
	                End = (int)(position + sequenceReader.Consumed) - 1
	            };
	            return true;
	        }
	        return false;
	    }
	    private bool IsSeparator(ref SequenceReader<byte> sequenceReader, out TokenType tokenType)
	    {
	        tokenType = default;
	        // Separators are one byte long 
	        if (sequenceReader.Consumed != 1)
	        {
	            return false;
	        }
	        var value = sequenceReader.Slice().ToArray();
	        foreach (var separator in TokenSpans.Separators)
	        {
	            if (separator.Value.SequenceEqual(value))
	            {
	                tokenType = separator.Key;
	                return true;
	            }
	        }
	        return false;
	    }
	    private bool IsKeyword(ref SequenceReader<byte> sequenceReader, out TokenType tokenType)
	    {
	        tokenType = default;
	        if (sequenceReader.Consumed < 3 || sequenceReader.Consumed > 7)
	        {
	            return false;
	        }
	        var value = sequenceReader.SliceToLowerChar().ToArray();
	        foreach (var keyword in TokenSpans.Keywords)
	        {
	            if (keyword.Value.SequenceEqual(value) && (sequenceReader.IsEndOfFileNext() || sequenceReader.IsSeparatorNext()))
	            {
	                tokenType = keyword.Key;
	                return true;
	            }
	        }
	        return false;
	    }
	    private bool IsOperator(ref SequenceReader<byte> sequenceReader, out TokenType tokenType)
	    {
	        tokenType = default;
	        // No need to check operator since all operators are less than 4 bytes in length
	        if (sequenceReader.Consumed > 3)
	        {
	            return false;
	        }
	        var value = sequenceReader.SliceToLowerChar().ToArray();
	        foreach (var @operator in TokenSpans.Operators)
	        {
	            if (@operator.Value.SequenceEqual(value) && (sequenceReader.IsEndOfFileNext() || sequenceReader.IsSeparatorNext()))
	            {
	                tokenType = @operator.Key;
	                return true;
	            }
	        }
	        return false;
	    }
	    private bool IsLiteral(ref SequenceReader<byte> sequenceReader, out TokenType tokenType)
	    {
	        tokenType = default;
	        if (sequenceReader.Consumed == 1)
	        {
	            // Identify if string literal (single quoted)
	            if (sequenceReader.CurrentSpan[0] == (byte)'\'')
	            {
	                while (sequenceReader.TryRead(out var value))
	                {
	                    if (value.Equals((byte)'\''))
	                    {
	                        tokenType = TokenType.String;
	                        return true;
	                    }
	                }
	                throw new TokenLexerException("Invalid string format. Missing closing quote.")
	                {
	                    Position = (int)position
	                };
	            }
	            // Identify if string literal (double quoted)
	            if (sequenceReader.CurrentSpan[0] == (byte)'"')
	            {
	                while (sequenceReader.TryRead(out var value))
	                {
	                    if (value.Equals((byte)'"'))
	                    {
	                        tokenType = TokenType.String;
	                        return true;
	                    }
	                }
	                throw new TokenLexerException("Invalid string format. Missing closing quote.")
	                {
	                    Position = (int)position
	                };
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
	            var split = sequenceReader.IsEndOfFileNext() || sequenceReader.IsSeparatorNext();
	            foreach (var literal in TokenSpans.Literals)
	            {
	                if (literal.Value.SequenceEqual(value) && split)
	                {
	                    tokenType = literal.Key;
	                    return true;
	                }
	            }
	        }
	        return false;
	    }
	    private bool IsComment(ref SequenceReader<byte> sequenceReader, out TokenType tokenType)
	    {
	        tokenType = default;
	        if (sequenceReader.Consumed == 1 && sequenceReader.CurrentSpan[0] == '#')
	        {
	            tokenType = TokenType.Comment;
	            var previous = default(byte);
	            var current = default(byte);
	            while (sequenceReader.TryRead(out current))
	            {
	                if (previous == (byte)'\r' && current == '\n')
	                {
	                    sequenceReader.Rewind(2);
	                    break;
	                }
	                previous = current;
	            }
	            return true;
	        }
	        return false;
	    }
	    private bool IsIdentifier(ref SequenceReader<byte> sequenceReader, out TokenType tokenType)
	    {
	        tokenType = default;
	        // As the lexer loops through the sequence of bytes
	        if (sequenceReader.IsSeparatorNext() || sequenceReader.IsEndOfFileNext() || !sequenceReader.IsAlphaNumericCharNext()) // This is to account for any unknown char
	        {
	            // Let's check if the span starts with a variable identifier
	            if (sequenceReader.CurrentSpan[0] == '@')
	            {
	                tokenType = TokenType.Variable;
	                return true;
	            }
	            tokenType = TokenType.Identifier;
	            return true;
	        }
	        return false;
	    }
	    #endregion
	    #region Static Methods
	    public static TokenLexer Create(string query, TokenLexerOptions options)
	    {
	        if (string.IsNullOrEmpty(query))
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(query));
	        }
	        var bytes = options.Encoding.GetBytes(query);
	        return new TokenLexer(
	            bytes,
	            options);
	    }
	    #endregion
	}
	internal struct TokenLexerOptions
	{
	    public TokenLexerOptions()
	    {
	        this.Encoding = Encoding.UTF8;
	    }
	    internal bool SkipTabs { get; set; }
	    internal bool SkipWhiteSpace { get; set; }
	    internal bool SkipLineFeed { get; set; }
	    internal bool SkipCarriageReturn { get; set; }
	    internal bool SkipComments { get; set; }
	    internal Encoding Encoding { get; set; }
	    public static TokenLexerOptions Default => new();
	}
	internal static class TokenSpans
	{
	    internal static ReadOnlySpan<KeyValuePair<TokenType, byte[]>> Separators => new[]
	    {
	        new KeyValuePair<TokenType, byte[]>(TokenType.WhiteSpace, new byte[] {(byte)' ' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.Slash, new byte[] {(byte)'/' }), // This is a type of separator used for edge paths
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
	        new KeyValuePair<TokenType, byte[]>(TokenType.Dot,new byte[] { (byte)'.' })
	    };
	    internal static ReadOnlySpan<KeyValuePair<TokenType, byte[]>> Keywords => new[]
	    {
	        //new KeyValuePair<TokenType, byte[]>(TokenType.QueryRoot, new byte[] { (byte)'q', (byte)'u', (byte)'e', (byte)'r', (byte)'y' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.Filter, new byte[] { (byte)'f', (byte)'i', (byte)'l', (byte)'t', (byte)'e', (byte)'r' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.Project, new byte[] { (byte)'p', (byte)'r', (byte)'o', (byte)'j', (byte)'e', (byte)'c', (byte)'t' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.Edge, new byte[] { (byte)'e', (byte)'d', (byte)'g', (byte)'e' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.Vertex, new byte[] { (byte)'v', (byte)'e', (byte)'r', (byte)'t', (byte)'e', (byte)'x' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.Sort, new byte[] { (byte)'s', (byte)'o', (byte)'r', (byte)'t' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.Page, new byte[] { (byte)'p', (byte)'a', (byte)'g', (byte)'e' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.Ascending, new byte[] { (byte)'a', (byte)'s', (byte)'c' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.Descending, new byte[] { (byte)'d', (byte)'e', (byte)'s', (byte)'c' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.Take, new byte[] { (byte)'t', (byte)'a', (byte)'k', (byte)'e' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.Skip, new byte[] { (byte)'s', (byte)'k', (byte)'i', (byte)'p' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.Token, new byte[] { (byte)'t', (byte)'o', (byte)'k', (byte)'e', (byte)'n' })
	    };
	    internal static ReadOnlySpan<KeyValuePair<TokenType, byte[]>> Operators => new[]
	    {
	        // arithmetic operators
	        new KeyValuePair<TokenType, byte[]>(TokenType.Slash, new byte[] {(byte)'/' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.Plus, new byte[] { (byte)'+' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.Minus, new byte[] { (byte)'-' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.Star, new byte[] { (byte)'*' }),
	        // logical operators
	        new KeyValuePair<TokenType, byte[]>(TokenType.Equal, new byte[] { (byte)'e', (byte)'q' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.NotEqual, new byte[] { (byte)'n', (byte)'e', (byte)'q' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.GreaterThan, new byte[] { (byte)'g', (byte)'t' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.GreaterThanOrEqual, new byte[] { (byte)'g', (byte)'t', (byte)'e' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.LessThan, new byte[] { (byte)'l', (byte)'t' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.LessThanOrEqual, new byte[] { (byte)'l', (byte)'t', (byte)'e' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.Alias, new byte[] { (byte)'a', (byte)'s' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.And, new byte[] { (byte)'a', (byte)'n', (byte)'d' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.Or, new byte[] { (byte)'o', (byte)'r' })
	    };
	    internal static ReadOnlySpan<KeyValuePair<TokenType, byte[]>> Literals => new KeyValuePair<TokenType, byte[]>[]
	    {
	        new KeyValuePair<TokenType, byte[]>(TokenType.Boolean, new byte[] { (byte)'t', (byte)'r', (byte)'u', (byte)'e' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.Boolean, new byte[] { (byte)'f', (byte)'a', (byte)'l', (byte)'s', (byte)'e' }),
	        new KeyValuePair<TokenType, byte[]>(TokenType.Null, new byte[] { (byte)'n', (byte)'u', (byte)'l', (byte)'l' }),
	    };
	}
	// Reference for Syntax Tokens: https://codeforwin.org/2015/05/introduction-to-programming-tokens.html#Separators
	internal enum TokenType
	{
	    None = 0,
	    #region Other
	    Comment,
	    Identifier,
	    Variable,
	    #endregion
	    #region Literals (are constant values that are used for performing various operations and calculations)
	    Null, // Also a keyword
	    Boolean, // Also a keyword
	    Integer,
	    FloatingPoint,
	    String,
	    #endregion
	    #region Operators (A operator returns a binary expression)
	    Star, //
	    Slash,
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
	    #endregion
	    #region Separators
	    Tab,
	    LineFeed,
	    CarriageReturn,
	    Comma,
	    Question,
	    Dot,
	    Colon,
	    Semicolon,
	    WhiteSpace,
	    Exclamation,
	    OpenBracket,
	    CloseBracket,
	    OpenParenthesis,
	    CloseParenthesis,
	    #endregion
	    #region Keywords
	    Alias,
	    Project,
	    Filter,
	    Sort,
	    Page,
	    Edge,
	    Vertex,
	    Descending,
	    Ascending,
	    Take,
	    Skip,
	    Token
	    #endregion
	}
	#endregion
	#region \Internal\Lexer\Exceptions
	internal class TokenLexerException : Exception
	{
	    public TokenLexerException() { }
	    internal TokenLexerException(string message) : base(message) { }
	    internal TokenLexerException(string message, Exception innerException) : base(message, innerException) { }
	    public int Position { get; init; }
	}
	#endregion
	#region \Internal\Lexer\Extensions
	internal static class TokenLexerExtensions
	{
	    // Checks if the current consumed byte equals the given value
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    internal static bool ByteEquals(this ref SequenceReader<byte> sequenceReader, byte value)
	    {
	        return sequenceReader.CurrentSpan[(int)sequenceReader.Consumed] == value;
	    }
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    internal static bool IsSeparatorNext(this ref SequenceReader<byte> sequenceReader)
	    {
	        if (sequenceReader.TryPeek(out var value))
	        {
	            var array = new byte[] { value };
	            foreach (var separator in TokenSpans.Separators)
	            {
	                if (separator.Value.SequenceEqual(array))
	                {
	                    return true;
	                }
	            }
	        }
	        return false;
	    }
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    internal static bool IsAlphaNumericCharNext(this ref SequenceReader<byte> sequenceReader) => sequenceReader.TryPeek(out var value) && char.IsLetterOrDigit((char)value);
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    internal static bool IsEndOfFileNext(this ref SequenceReader<byte> sequenceReader) => sequenceReader.Remaining <= 0;
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    internal static ReadOnlySpan<byte> Slice(this ref SequenceReader<byte> sequenceReader)
	    {
	        return sequenceReader.CurrentSpan.Slice(0, (int)sequenceReader.Consumed);
	    }
	    // Slices current consumed sequence as span and convert all alpha-characters to lower case if not already
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    internal static ReadOnlySpan<byte> SliceToLowerChar(this ref SequenceReader<byte> sequenceReader)
	    {
	        var buffer = new byte[sequenceReader.Consumed];
	        for (int i = 0; i < sequenceReader.Consumed; i++)
	        {
	            buffer[i] = (byte)char.ToLower((char)sequenceReader.CurrentSpan[i]);
	        }
	        return buffer.AsSpan();
	    }
	}
	#endregion
	#region \Internal\Utilities
	internal static class ThrowHelper
	{
	    [DoesNotReturn]
	    internal static void ThrowInvalidOperationException(string message) =>
	        throw new InvalidOperationException(message);
	    [DoesNotReturn]
	    internal static void ThrowArgumentNullException(string paramName) => 
	        throw new ArgumentNullException(paramName);
	    [DoesNotReturn]
	    internal static void ThrowArgumentNullException(string paramName, string message) =>
	        throw new ArgumentNullException(paramName, message);
	    [DoesNotReturn]
	    internal static void ThrowArgumentException(string message) =>
	        throw new ArgumentException(message);
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
	#region \Properties
	#endregion
	#region \SyntaxNodes
	public readonly struct Location
	{
	    Location(int startLine, int endLine, int start, int end)
    {
        EndLine = endLine;
        StartLine = startLine;
        Start = start;
        End = end;
    }
	    public int Start { get; }
	    public int StartLine { get; }    
	    public int End { get; }
	    public int EndLine { get; }
	    public static Location Create(int startLine, int endLine, int start, int end) => 
	        new Location(startLine, endLine, start, end);
	}
	public sealed class BinaryNode : QueryNode
	{
	    internal BinaryNode() { }
	    public BinaryNode(QueryNode left, QueryNode right, BinaryOperatorType operatorType)
	    {
	        LeftOperand = left;
	        RightOperand = right;
	        OperatorType = operatorType;
	    }
	    public QueryNode? LeftOperand { get; init; }
	    public QueryNode? RightOperand { get; init; }
	    public BinaryOperatorType OperatorType { get; init; }
	    public override QueryNodeType NodeType => QueryNodeType.Binary;
	    public override void Accept(IQueryNodeVisitor visitor)
	    {
	        visitor.Visit(this);
	    }
	    public override T Accept<T>(IQueryNodeVisitor<T> visitor)
	    {
	        return visitor.Visit(this);
	    }
	    public override IEnumerable<TNode> GetNodesOfType<TNode>()
	    {
	        if (this is TNode node1)
	        {
	            yield return node1;
	        }
	        if (LeftOperand is not null)
	        {
	            foreach (var node2 in LeftOperand.GetNodesOfType<TNode>())
	            {
	                yield return node2;
	            }
	        }
	        if (RightOperand is not null)
	        {
	            foreach (var node3 in RightOperand.GetNodesOfType<TNode>())
	            {
	                yield return node3;
	            }
	        }
	    }
	}
	[DebuggerDisplay("{Text}")]
	public sealed class ConstantNode : QueryNode
	{
	    internal ConstantNode(byte[] value, string text, Location location) 
	        : base(text, location)
	    {
	        Value = value;
	    }
	    public byte[] Value { get; }
	    public override QueryNodeType NodeType => QueryNodeType.Constant;
	    public override void Accept(IQueryNodeVisitor visitor)
	    {
	        visitor.Visit(this);
	    }
	    public override T Accept<T>(IQueryNodeVisitor<T> visitor)
	    {
	        return visitor.Visit(this);
	    }
	    public override IEnumerable<TNode> GetNodesOfType<TNode>()
	    {
	        if (this is TNode node)
	        {
	            yield return node;
	        }
	    }
	    #region Parse
	    public string GetString() => Encoding.UTF8.GetString(Value);
	    public DateTime GetDateTime() => DateTime.Parse(GetString());
	    public DateOnly GetDate() => DateOnly.Parse(GetString());
	    public TimeOnly GetTime() => TimeOnly.Parse(GetString());
	    public short GetInt16() => short.Parse(GetString());
	    public int GetInt32() => int.Parse(GetString());
	    public long GetInt64() => long.Parse(GetString());
	    public float GetSingle() => float.Parse(GetString());
	    public decimal GetDecimal() => decimal.Parse(GetString());
	    public bool TryGetInt16(out short int16) => short.TryParse(GetString(), out int16);
	    public bool TryGetInt64(out long int64) => long.TryParse(GetString(), out int64);
	    public bool TryGetDateTime(out DateTime dateTime) => DateTime.TryParse(GetString(), out dateTime);
	    public bool TryGetTime(out TimeOnly time) => TimeOnly.TryParse(GetString(), out time);
	    public bool TryGetDate(out DateOnly date) => DateOnly.TryParse(GetString(), out date);
	    public bool TryGetDecimal(out decimal deci) => decimal.TryParse(GetString(), out deci);
	    public bool TryGetInt32(out int int32) => int.TryParse(GetString(), out int32);
	    public bool TryGetSingle(out float single) => float.TryParse(GetString(), out single);
	    #endregion
	}
	public abstract class QueryNode
	{
	    internal QueryNode() { }
	    internal QueryNode(string? text, Location location)
	    {
	        Text = text;
	        Location = location;
	    }
	    public abstract QueryNodeType NodeType { get; }
	    public virtual string? Text { get; }
	    public virtual Location Location { get; }
	    public virtual void Accept(IQueryNodeVisitor visitor)
	    {
	        visitor.Visit(this);
	    }
	    public virtual T Accept<T>(IQueryNodeVisitor<T> visitor)
	    {
	        return visitor.Visit(this);
	    }
	    public virtual IEnumerable<TNode> GetNodesOfType<TNode>() where TNode : QueryNode
	    {
	        return Array.Empty<TNode>();
	    }
	}
	public sealed class EdgeNode : QueryNode
	{
	    internal EdgeNode(LabelNode label, VertexNode source, VertexNode target, string text, Location location, LabelNode? alias = null, string? path = null): base(text, location) 
	    {
	        if (label is null) ThrowHelper.ThrowArgumentNullException(nameof(label));
	        if (source is null) ThrowHelper.ThrowArgumentNullException(nameof(source));
	        if (target is null) ThrowHelper.ThrowArgumentNullException(nameof(target));
	        Label = label;
	        Source = source;
	        Target = target;
	        Alias = alias;
	        Path = path;
	    }
	    public LabelNode Label { get; }
	    public VertexNode? Source { get; }
	    public VertexNode? Target { get; }
	    public LabelNode? Alias { get; }
	    public string? Path { get; }
	    public bool HasAlias => Alias is not null;
	    public override QueryNodeType NodeType => QueryNodeType.Edge;
	    public override void Accept(IQueryNodeVisitor visitor)
	    {
	        visitor.Visit(this);
	    }
	    public override T Accept<T>(IQueryNodeVisitor<T> visitor)
	    {
	        return visitor.Visit(this);
	    }
	    public override IEnumerable<TNode> GetNodesOfType<TNode>()
	    {
	        if (this is TNode edge)
	        {
	            yield return edge;
	        }
	        foreach (var node in Label.GetNodesOfType<TNode>())
	        {
	            yield return node;
	        }
	        foreach (var node1 in Source.GetNodesOfType<TNode>())
	        {
	            yield return node1;
	        }
	        foreach (var node2 in Target.GetNodesOfType<TNode>())
	        {
	            yield return node2;
	        }
	    }
	}
	public sealed class FilterNode : QueryNode
	{
	    public FilterNode(BinaryNode predicate)
	    {
	        if (predicate is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(predicate));
	        }
	        Predicate = predicate;
	    }
	    public BinaryNode? Predicate { get; }
	    public override QueryNodeType NodeType => QueryNodeType.Filter;
	    public override void Accept(IQueryNodeVisitor visitor)
	    {
	        visitor.Visit(this);
	    }
	    public override T Accept<T>(IQueryNodeVisitor<T> visitor)
	    {
	        return visitor.Visit(this);
	    }
	    public override IEnumerable<TNode> GetNodesOfType<TNode>()
	    {
	        if (this is TNode node)
	        {
	            yield return node;
	        }      
	        if (Predicate is not null)
	        {
	            foreach (var item in Predicate.GetNodesOfType<TNode>())
	            {
	                yield return item;
	            }
	        }
	    }
	}
	public sealed class FunctionCallNode : IdentifierNode
	{
	    public FunctionCallNode(string name, IEnumerable<ParameterNode> parameters) : base(name)
	    {
	        this.Parameters = parameters;
	    }
	    public FunctionType FunctionType { get; init; }
	    public IEnumerable<ParameterNode> Parameters { get; init; } = [];
	    public override QueryNodeType NodeType => QueryNodeType.FunctionCall;
	    public override void Accept(IQueryNodeVisitor visitor)
	    {
	        visitor.Visit(this);
	    }
	    public override T Accept<T>(IQueryNodeVisitor<T> visitor)
	    {
	        return visitor.Visit(this);
	    }
	    public override IEnumerable<TNode> GetNodesOfType<TNode>()
	    {
	        if (this is TNode node)
	        {
	            yield return node;
	        }
	        foreach (var parameter in Parameters)
	        {
	            foreach (var item in parameter.GetNodesOfType<TNode>())
	            {
	                yield return item;
	            }
	        }
	    }
	}
	public abstract class IdentifierNode : QueryNode
	{
	    public IdentifierNode(string name)
    {
        Name = name;
    }
	    internal IdentifierNode(string name, string text, Location location) 
        : base(text, location)
    {
        Name = name;
    }
	    public string? Name { get; }
	}
	[DebuggerDisplay("{Name}")]
	public sealed class LabelNode : IdentifierNode
	{
	    public LabelNode(string label) : base(label) { }
	    public LabelNode(string label, string alias) : this(label)
    {
        Alias = alias;
    }
	    public string? Alias { get; }
	    public bool HasAlias => !string.IsNullOrEmpty(Alias);
	    public override QueryNodeType NodeType => QueryNodeType.Label;
	    public override void Accept(IQueryNodeVisitor visitor)
	    {
	        visitor.Visit(this);
	    }
	    public override T Accept<T>(IQueryNodeVisitor<T> visitor)
	    {
	        return visitor.Visit(this);
	    }
	    public override IEnumerable<TNode> GetNodesOfType<TNode>()
	    {
	        if (this is TNode node)
	        {
	            yield return node;
	        }
	    }
	    public static LabelNode Empty() => new LabelNode(string.Empty);
	}
	public sealed class PageNode : QueryNode
	{
	    internal PageNode(ConstantNode skip, ConstantNode take, string text, Location location) 
        : base(text, location)
    {
	        Skip = skip;
	        Take = take;
	    }
	    public ConstantNode? Take { get; }
	    public ConstantNode? Skip { get; }
	    public override QueryNodeType NodeType => QueryNodeType.Page;
	    #region Overloads
	    public override void Accept(IQueryNodeVisitor visitor)
	    {
	        visitor.Visit(this);
	    }
	    public override T Accept<T>(IQueryNodeVisitor<T> visitor)
	    {
	        return visitor.Visit(this);
	    }
	    public override IEnumerable<TNode> GetNodesOfType<TNode>()
	    {
	        if (this is TNode node)
	        {
	            yield return node;
	        }
	        if (Take is not null)
	        {
	            foreach (var item in Take.GetNodesOfType<TNode>())
	            {
	                yield return item;
	            }
	        }
	        if (Skip is not null)
	        {
	            foreach (var item in Skip.GetNodesOfType<TNode>())
	            {
	                yield return item;
	            }
	        }
	    }
	    #endregion
	}
	public sealed class ParameterNode : QueryNode
	{
	    ParameterNode(QueryNode parameter)
	    {
	        if (parameter is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(parameter));
	        }
	        ParameterValue = parameter;
	    }
	    public ParameterNode(PropertyNode parameter) 
	        : this(parameter as QueryNode) { }
	    public ParameterNode(ConstantNode parameter)
	        : this(parameter as QueryNode) { }
	    public ParameterNode(FunctionCallNode parameter)
	        : this(parameter as QueryNode) { }
	    public QueryNode? ParameterValue { get; init; }
	    public ParameterType ParameterType => ParameterValue switch
	    {
	        ConstantNode        => ParameterType.Constant,
	        FunctionCallNode    => ParameterType.Function,
	        PropertyNode        => ParameterType.Property,
	        _                   => ParameterType.None
	    };
	    public override QueryNodeType NodeType => QueryNodeType.Parameter;
	    public override void Accept(IQueryNodeVisitor visitor)
	    {
	        visitor.Visit(this);
	    }
	    public override T Accept<T>(IQueryNodeVisitor<T> visitor)
	    {
	        return visitor.Visit(this);
	    }
	    public override IEnumerable<TNode> GetNodesOfType<TNode>()
	    {
	        if (this is TNode node)
	        {
	            yield return node;
	        }
	        if (ParameterValue is not null)
	        {
	            foreach (var node1 in ParameterValue.GetNodesOfType<TNode>())
	            {
	                yield return node1;
	            }
	        }
	    }
	}
	public sealed class ProjectNode : QueryNode
	{
	    public ProjectNode(IEnumerable<PropertyNode> properties, string text, Location location) 
	        : base(text, location)
	    {
	        if (properties is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(properties));
	        }
	        Properties = properties.ToImmutableList();
	    }
	    public IEnumerable<PropertyNode> Properties { get; }
	    #region Overloads
	    public override QueryNodeType NodeType => QueryNodeType.Project;
	    public override void Accept(IQueryNodeVisitor visitor)
	    {
	        visitor.Visit(this);
	    }
	    public override T Accept<T>(IQueryNodeVisitor<T> visitor)
	    {
	        return visitor.Visit(this);
	    }
	    public override IEnumerable<TNode> GetNodesOfType<TNode>()
	    {
	        if (this is TNode project)
	        {
	            yield return project;
	        }
	        foreach (var property in Properties)
	        {
	            foreach (var node in property.GetNodesOfType<TNode>())
	            {
	                yield return node;
	            }
	        }
	    }
	    #endregion
	}
	[DebuggerDisplay("{Name}")]
	public sealed class PropertyNode : IdentifierNode
	{
	    internal PropertyNode(string name, string text, Location location)
	        : base(name, text, location)
	    {
	        Children = [];
	    }
	    internal PropertyNode(string name, string alias, string text, Location location) 
	        : this(name, text, location)
	    {
	        Alias = alias;
	    }
	    internal PropertyNode(string name, IEnumerable<PropertyNode> children, string text, Location location)
	        : this(name, text, location)
	    {
	        Children = children.ToImmutableList();
	    }
	    internal PropertyNode(string name, string alias, IEnumerable<PropertyNode> children, string text, Location location) 
	        : this(name, alias, text, location)
	    {
	        Children = children.ToImmutableList();
	    }
	    public string? Alias { get; }
	    public IEnumerable<PropertyNode> Children { get; }
	    public bool HasChildren => Children.Any();
	    public bool HasAlias => !string.IsNullOrEmpty(Alias);
	    #region Overloads
	    public override QueryNodeType NodeType => QueryNodeType.Property;
	    public override void Accept(IQueryNodeVisitor visitor)
	    {
	        visitor.Visit(this);
	    }
	    public override T Accept<T>(IQueryNodeVisitor<T> visitor)
	    {
	        return visitor.Visit(this);
	    }
	    public override IEnumerable<TNode> GetNodesOfType<TNode>()
	    {
	        if (this is TNode node)
	        {
	            yield return node;
	        }
	        if (Children is not null)
	        {
	            foreach (var node1 in Children.SelectMany(x => x.GetNodesOfType<TNode>()))
	            {
	                yield return node1;
	            }
	        }
	    }
	    #endregion
	}
	public sealed class RootNode : QueryNode
	{
    /// <summary>
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(vertex));
	        }
        Vertex = vertex;
    }
	    public VertexNode Vertex { get; }
	    public override QueryNodeType NodeType => QueryNodeType.Root;
	    #region Overloads
	    public override void Accept(IQueryNodeVisitor visitor)
	    {
	        visitor.Visit(this);
	    }
	    public override T Accept<T>(IQueryNodeVisitor<T> visitor)
	    {
	        return visitor.Visit(this);
	    }
	    public override IEnumerable<TNode> GetNodesOfType<TNode>()
	    {
	        if (this is TNode root)
	        {
	            yield return root;
	        }
	        else if (Vertex is not null) // There should ever be one Root Node in the Tree. 
	        {
	            foreach (var node in Vertex.GetNodesOfType<TNode>())
	            {
	                yield return node;
	            }
	        }
	    }
	    #endregion
	}
	public sealed class SortNode : QueryNode
	{
	    public SortNode(PropertyNode property)
    {
        if (property is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(property));
	        }
        Identifier = property;
    }
	    public SortNode(PropertyNode property, SortDirection direction) : this(property)
    {
        Direction = direction;
    }
	    public SortNode(PropertyNode property, SortDirection direction, SortNode thenBy) : this(property, direction)
    {
        Direction = direction;
    }
	    public SortNode(FunctionCallNode function)
    {
        if (function is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(function));
	        }
        Identifier = function;
    }
	    public SortNode(FunctionCallNode functionCall, SortDirection direction) 
	        : this(functionCall)
	    {
	        Direction = direction;
	    }
	    public SortNode(FunctionCallNode functionCall, SortDirection direction, SortNode thenBy)
	        : this(functionCall, direction)
	    {
	        if (thenBy is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(thenBy));
	        }
	        ThenBy = thenBy;
	    }
	    public IdentifierNode? Identifier { get; }
	    public SortDirection Direction { get; }
	    public SortNode? ThenBy { get; }
	    public bool HasThenBy => ThenBy is not null;
	    public override QueryNodeType NodeType => QueryNodeType.Sort;
	    public override void Accept(IQueryNodeVisitor visitor)
	    {
	        visitor.Visit(this);
	    }
	    public override T Accept<T>(IQueryNodeVisitor<T> visitor)
	    {
	        return visitor.Visit(this);
	    }
	    public override IEnumerable<TNode> GetNodesOfType<TNode>()
	    {
	        if (this is TNode node)
	        {
	            yield return node;
	        }
	        if (Identifier is not null)
	        {
	            foreach (var node1 in Identifier.GetNodesOfType<TNode>())
	            {
	                yield return node1;
	            }
	        }
	        if (ThenBy is not null)
	        {
	            foreach (var node1 in ThenBy.GetNodesOfType<TNode>())
	            {
	                yield return node1;
	            }
	        }
	    }
	}
	public sealed class VertexNode : QueryNode
	{
    private readonly List<QueryNode> nodes;

    /// <summary>
	        this.nodes = new List<QueryNode>(nodes);
	    }
	    public VertexNode(ConstantNode argument, IEnumerable<QueryNode> nodes) 
	        : this(nodes)
	    {
	        if (argument is null)
	        {
	            ThrowHelper.ThrowArgumentNullException(nameof(argument));
	        }
	        Argument = argument;
	    }
	    public ConstantNode? Argument { get; }
	    public IEnumerable<QueryNode> Nodes =>
	#if NET7_0_OR_GREATER
	        nodes.AsReadOnly();
	#else
	        new ReadOnlyCollection<QueryNode>(nodes);
	#endif
	    public bool HasArgument => Argument is not null;
	    public override QueryNodeType NodeType => QueryNodeType.Vertex;
	    #region Overloads
	    public override void Accept(IQueryNodeVisitor visitor)
	    {
	        visitor.Visit(this);
	    }
	    public override T Accept<T>(IQueryNodeVisitor<T> visitor)
	    {
	        return visitor.Visit(this);
	    }
	    public override IEnumerable<TNode> GetNodesOfType<TNode>()
	    {
	        if (this is TNode vertex)
	        {
	            yield return vertex;
	        }
	        foreach (var node in Nodes)
	        {
	            foreach (var child in node.GetNodesOfType<TNode>())
	            {
	                yield return child;
	            }
	        }
	    }
	    #endregion
	    internal void AddNode(QueryNode queryNode)
	    {
	        this.nodes.Add(queryNode);
	    }
	}
	#endregion
	#region \SyntaxNodes\Enums
	public enum BinaryOperatorType
	{
	    None,
	    Star                = TokenType.Star,
	    Plus                = TokenType.Plus,
	    Minus               = TokenType.Minus,
	    Equal               = TokenType.Equal,
	    NotEqual            = TokenType.NotEqual,
	    GreaterThan         = TokenType.GreaterThan,
	    GreaterThanOrEqual  = TokenType.GreaterThanOrEqual,
	    LessThan            = TokenType.LessThan,
	    LessThanOrEqual     = TokenType.LessThanOrEqual,
	    And                 = TokenType.And,
	    Or                  = TokenType.Or
	}
	public enum FunctionType
	{
	    None,
	    // string functions
	    StartsWith,
	    EndsWith,
	    Contains,
	    Concat,
	    SubString,
	    PadRight,
	    PadLeft,
	    Trim,
	    TrimRight,
	    TrimLeft,
	    // array functions (with scalar output only)
	    Any,
	    All,
	    // other functions
	    //In,
	}
	public enum ParameterType
	{
	    None,
	    Property,
	    Function,
	    Constant
	}
	public enum QueryNodeType
	{
	    Root,
	    Vertex,
	    Edge,
	    Project,
	    Filter,
	    Sort,
	    Page,
	    Label,
	    //Attribute,
	    Property,
	    FunctionCall,
	    Parameter,
	    Constant,
	    Binary,
	}
	public enum SortDirection
	{
	    Ascending = TokenType.Ascending,
	    Descending = TokenType.Descending,
	}
	#endregion
	#region \Visitors
	public sealed class QueryableQueryVisitor<T> : IQueryNodeVisitor<IQueryable<T>>
	{
	    private IQueryable<T> queryable;
	    public QueryableQueryVisitor(IQueryable<T> queryable)
	    {
	        this.queryable = queryable;
	    }
	    public IQueryable<T> Visit(QueryNode queryNode)
	    {
	        return queryNode.Accept(this);
	    }
	    public IQueryable<T> Visit(VertexNode queryNode)
	    {
	        foreach (var node in queryNode.Nodes)
	        {
	           queryable = node.Accept(this);
	        }
	        return queryable;
	    }
	    public IQueryable<T> Visit(FilterNode queryNode)
	    {
	        return queryable;
	    }
	    public IQueryable<T> Visit(ProjectNode queryNode)
	    {
	        return queryable;
	    }
	    public IQueryable<T> Visit(PageNode queryNode)
	    {
	        var skip = queryNode.Skip.GetInt32();
	        var take = queryNode.Take.GetInt32();
	        var skipExpression = Expression.Call(
	            typeof(Queryable),
	            "Skip",
	            new Type[] { queryable.ElementType },
	            queryable.Expression,
	            Expression.Constant(skip));
	        var takeExpression = Expression.Call(
	            typeof(Queryable),
	            "Take",
	            new Type[] { queryable.ElementType },
	            skipExpression,
	            Expression.Constant(take));
	        queryable = queryable.Provider.CreateQuery<T>(takeExpression);
	        return queryable;
	    }
	    public IQueryable<T> Visit(SortNode queryNode)
	    {
	        return queryable;
	    }
	    public IQueryable<T> Visit(BinaryNode queryNode)
	    {
	        return queryable;
	    }
	    public IQueryable<T> Visit(PropertyNode queryNode)
	    {
	        return queryable;
	    }
	    public IQueryable<T> Visit(ParameterNode queryNode)
	    {
	        return queryable;
	    }
	    public IQueryable<T> Visit(FunctionCallNode queryNode)
	    {
	        return queryable;
	    }
	    public IQueryable<T> Visit(ConstantNode queryNode)
	    {
	        return queryable;
	    }
	    public IQueryable<T> Visit(EdgeNode queryNode)
	    {
	        return queryable;
	    }
	}
	public abstract class QueryVisitor<T> : IQueryNodeVisitor<T>
	{
	    public T Visit(QueryNode node) => node.Accept(this);
	    public virtual T Visit(VertexNode node)
	    {
	        throw new NotImplementedException();
	    }
	    public virtual T Visit(FilterNode node)
	    {
	        throw new NotImplementedException();
	    }
	    public virtual T Visit(ProjectNode node)
	    {
	        throw new NotImplementedException();
	    }
	    public virtual T Visit(PageNode node)
	    {
	        throw new NotImplementedException();
	    }
	    public virtual T Visit(SortNode node)
	    {
	        throw new NotImplementedException();
	    }
	    public virtual T Visit(BinaryNode node)
	    {
	        throw new NotImplementedException();
	    }
	    public virtual T Visit(PropertyNode node)
	    {
	        throw new NotImplementedException();
	    }
	    public virtual T Visit(ParameterNode node)
	    {
	        throw new NotImplementedException();
	    }
	    public virtual T Visit(FunctionCallNode node)
	    {
	        throw new NotImplementedException();
	    }
	    public virtual T Visit(ConstantNode node)
	    {
	        throw new NotImplementedException();
	    }
	    public T Visit(EdgeNode queryNode)
	    {
	        throw new NotImplementedException();
	    }
	}
	#endregion
}
#endregion
