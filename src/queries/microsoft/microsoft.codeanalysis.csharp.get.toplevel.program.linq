<Query Kind="Program">
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.CodeAnalysis</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp.Syntax</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
</Query>




void Main()
{
	var syntaxTree = CSharpSyntaxTree.ParseText(@"
using System;
using System.IO;

Console.WriteLine(""Hello, World!"");
int x = 42;


public class Test
{

}




");

	var root = syntaxTree.GetRoot();
	
	if (TopLevelProgramFinder.HasTopLevelStatements(root))
	{
		var statements = TopLevelProgramFinder.GetTopLevelStatements(root);
		
		foreach (var statement in statements)
		{
			Console.WriteLine(statement.ToString());
		}
	}
}



public static class TopLevelProgramFinder
{
	public static bool HasTopLevelStatements(SyntaxNode node)
	{
		// Get the root CompilationUnitSyntax
		var compilationUnit = node.SyntaxTree.GetRoot() as CompilationUnitSyntax;

		if (compilationUnit == null)
		{
			return false;
		}

		// Check for GlobalStatementSyntax nodes
		return compilationUnit.Members.OfType<GlobalStatementSyntax>().Any();
	}

	public static IEnumerable<StatementSyntax> GetTopLevelStatements(SyntaxNode node)
	{
		var compilationUnit = node.SyntaxTree.GetRoot() as CompilationUnitSyntax;

		if (compilationUnit == null)
		{
			return Enumerable.Empty<StatementSyntax>();
		}

		// Return all top-level statements
		return compilationUnit.Members
			.OfType<GlobalStatementSyntax>()
			.Select(globalStatement => globalStatement.Statement);
	}
}