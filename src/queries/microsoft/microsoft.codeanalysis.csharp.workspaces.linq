<Query Kind="Program">
  <NuGetReference>Microsoft.Build</NuGetReference>
  <NuGetReference>Microsoft.Build.Framework</NuGetReference>
  <NuGetReference>Microsoft.Build.Locator</NuGetReference>
  <NuGetReference Version="4.8.0">Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <NuGetReference Version="4.8.0">Microsoft.CodeAnalysis.CSharp.Workspaces</NuGetReference>
  <NuGetReference Version="4.8.0">Microsoft.CodeAnalysis.Workspaces.Common</NuGetReference>
  <NuGetReference Version="4.8.0">Microsoft.CodeAnalysis.Workspaces.MSBuild</NuGetReference>
  <Namespace>Microsoft.Build.Execution</Namespace>
  <Namespace>Microsoft.CodeAnalysis</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp.Syntax</Namespace>
  <Namespace>Microsoft.CodeAnalysis.MSBuild</Namespace>
  <Namespace>Microsoft.Build.Locator</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

string SolutionFile = @"C:\Source\repos\assimalign\cohesion\libraries\Http\Assimalign.Cohesion.Http\Assimalign.Cohesion.Http.sln";
string OutputPath = @"C:\Source\repos\dotnetcadet\development-linqpad\src\queries\cohesion";
string[] Projects = [
	"Assimalign.Cohesion.Http",
]; // The projects within the solution file to gen-up

async Task Main()
{
	
	if (!MSBuildLocator.IsRegistered)
	{
		MSBuildLocator.RegisterDefaults();
	}

	using var workspace = MSBuildWorkspace.Create();

	var solution = await workspace.OpenSolutionAsync(SolutionFile);

	foreach (var project in solution.Projects)
	{
		if (Projects.Contains(project!.Name, StringComparer.OrdinalIgnoreCase))
		{
			var builder = new LINQPadFileBuilder(OutputPath, project!.Name);
			
			await WriteProjectAsync(builder, solution, project);
		}
	}
}

private List<Guid> ProjectsProcessed { get; } = new();

private async Task WriteProjectAsync(LINQPadFileBuilder builder, Solution solution, Project project)
{
	if (ProjectsProcessed.Contains(project.Id.Id))
	{
		return;
	}
	var folders = project.FilePath!.Split('\\');
	var directory = string.Join('\\', folders.Take(folders.Length - 1));

	// Add Package References
	foreach (var reference in GetNugetPackages(project))
	{
		builder.WritePackageReference(reference.Item1, reference.Item2);
	}
	
	builder.WriteRegionStart(project.Name);
	builder.WriteNamespaceStart(project.DefaultNamespace!);

	// Group .cs Documents by Folder Paths
	var chunks = project.Documents
		.GroupBy(document =>
		{
			var paths = document.FilePath!.Split('\\');
			var folder = string.Join('\\', paths.Take(paths.Length - 1)).Replace(directory, "");

			if (string.IsNullOrEmpty(folder))
			{
				folder = "\\";
			}

			return folder;
		})
		.OrderBy(p => p.Key)
		.ToList();

	foreach (var chunk in chunks)
	{
		builder.WriteRegionStart(chunk.Key);
		foreach (var document in chunk)
		{
			var syntaxNode = await document.GetSyntaxRootAsync();

			// Usings
			foreach (var statement in GetNodes<UsingDirectiveSyntax>(syntaxNode))
			{
				var directive = statement!.Name!.ToFullString();

				if (statement.StaticKeyword.Value is not null)
				{
					builder.WriteUsingDirective($"static {directive}");
				}
				else
				{
					builder.WriteUsingDirective(directive);
				}
			}
			foreach (var item in GetNodes<InterfaceDeclarationSyntax>(syntaxNode))
			{
				var code = item.ToFullString();

				builder.WriteObject(code);
			}
			foreach (var item in GetNodes<ClassDeclarationSyntax>(syntaxNode).Where(p => IsNotNested(p.Parent)))
			{
				var code = item.ToFullString();

				builder.WriteObject(code);
			}
			foreach (var item in GetNodes<StructDeclarationSyntax>(syntaxNode).Where(p => IsNotNested(p.Parent)))
			{
				item.Dump();
				
				var code = item.ToFullString();

				builder.WriteObject(code);
			}
			foreach (var item in GetNodes<EnumDeclarationSyntax>(syntaxNode).Where(p => IsNotNested(p.Parent)))
			{
				var code = item.ToFullString();

				builder.WriteObject(code);
			}
			foreach (var item in GetNodes<DelegateDeclarationSyntax>(syntaxNode).Where(p => IsNotNested(p.Parent)))
			{
				var code = item.ToFullString();

				builder.WriteObject(code);
			}
			foreach (var item in GetNodes<RecordDeclarationSyntax>(syntaxNode).Where(p => IsNotNested(p.Parent)))
			{
				var code = item.ToFullString();

				builder.WriteObject(code);
			}
		}
		builder.WriteRegionEnd();
	}

	builder.WriteNamespaceEnd();
	builder.WriteRegionEnd();

	// Check for Project Dependencies
	foreach (var projRef in project.ProjectReferences)
	{
		var refProject = solution.GetProject(projRef.ProjectId)!;
		
		builder.WriteLoad(refProject.Name);
	}
	
	builder.Build();
	ProjectsProcessed.Add(project.Id.Id);
	
	foreach (var projRef in project.ProjectReferences)
	{
		var refProject = solution.GetProject(projRef.ProjectId)!;
		var refBuilder = new LINQPadFileBuilder(builder.Path, refProject.Name);

		await WriteProjectAsync(refBuilder, solution, refProject);
	}
}

private static bool IsNotNested(SyntaxNode? parent)
{
	return parent is not ClassDeclarationSyntax &&
			parent is not RecordDeclarationSyntax &&
			parent is not StructDeclarationSyntax;
}

private static IEnumerable<(string, string)> GetNugetPackages(Project project)
{
	var nuget = Path.Combine(
		Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
		".nuget\\packages");

	var references = project.MetadataReferences.Where(p => p.Display!.StartsWith(nuget, StringComparison.OrdinalIgnoreCase));

	foreach (var reference in references)
	{
		var directories = reference?.Display?.Replace(nuget, "").Split('\\')!;

		var version = directories[2];
		var package = directories[^1].Replace(".dll", "");

		yield return (package, version);
	}
}

public static IEnumerable<TNode> GetNodes<TNode>(SyntaxNode syntaxNode)
{
	var items = new List<TNode>();

	if (syntaxNode is TNode node)
	{
		items.Add(node);
	}
	else
	{
		foreach (var child in syntaxNode.ChildNodes())
		{
			items.AddRange(GetNodes<TNode>(child));
		}
	}

	return items;
}

public class LINQPadFileBuilder
{
	private readonly List<Action<StreamWriter>> onRefAdd = new();
	private readonly List<Action<StreamWriter>> onUsingAdd = new();
	private readonly Queue<Action<StreamWriter>> onObjectAdd = new();
	private readonly List<Action<StreamWriter>> onLoadAdd = new();
	private readonly HashSet<string> refCache = new();
	private readonly HashSet<string> usingsCache = new();

	private bool hasNamespace;

	public LINQPadFileBuilder(string path, string projectName)
	{
		Path = path;
		ProjectName = FormatProjectName(projectName);
	}
	
	private string[] KnownRefEndings = 
	[
		"(net5.0)",
		"(net6.0)",
		"(net7.0)",
		"(net8.0)",
		"(net9.0)",
	];
	
	private string FormatProjectName(string projectName)
	{
		var value = projectName;
		
		foreach (var ending in KnownRefEndings)
		{
			value = value.Replace(ending, "");
		}
		
		return value.ToLowerInvariant();
	}
	

	public string ProjectName { get; }
	public string Path { get; }

	public LINQPadFileBuilder WriteLoad(string projectName)
	{
		onLoadAdd.Add(writer =>
		{
			writer.Write('#');
			writer.Write("load");
			writer.Write(' ');
			writer.Write('"');
			writer.Write(".\\");
			writer.Write(FormatProjectName(projectName));
			writer.Write('"');
			writer.WriteLine();
		});
		return this;
	}
	public LINQPadFileBuilder WriteNamespaceStart(string name)
	{
		onObjectAdd.Enqueue(writer =>
		{
			writer.Write("namespace");
			writer.Write(' ');
			writer.WriteLine(name);
			writer.Write('{');
			writer.WriteLine();
			hasNamespace = true;
		});
		return this;
	}
	public LINQPadFileBuilder WriteNamespaceEnd()
	{
		onObjectAdd.Enqueue(writer =>
		{
			writer.Write('}');
			writer.WriteLine();
			hasNamespace = false;
		});
		return this;
	}
	public LINQPadFileBuilder WritePackageReference(string name, string version)
	{
		string[] prerelease = ["rc", "pre", "beta"];

		onRefAdd.Add(writer =>
		{
			if (refCache.Contains(name))
			{
				return;
			}
			refCache.Add(name);
			writer.Write("<NuGetReference");
			writer.Write(' ');
			if (prerelease.Any(value => version.Contains(value)))
			{
				writer.Write("Prerelease=\"");
				writer.Write("true");
				writer.Write("\"");
				writer.Write(' ');
			}
			writer.Write("Version=\"");
			writer.Write(version);
			writer.Write("\">");
			writer.Write(name);
			writer.Write("</NuGetReference>");
			writer.WriteLine();
		});

		return this;
	}
	public LINQPadFileBuilder WriteUsingDirective(string name)
	{
		onUsingAdd.Add(writer =>
		{
			var value = name;
			if (name.StartsWith("global"))
			{
				value = name.Split("::")[^1];
			}
			if (usingsCache.Contains(value))
			{
				return;
			}
			usingsCache.Add(value);
			writer.Write("<Namespace>");
			writer.Write(value);
			writer.Write("</Namespace>");
			writer.WriteLine();
		});

		return this;
	}
	public LINQPadFileBuilder WriteRegionStart(string name)
	{
		onObjectAdd.Enqueue(writer =>
		{
			if (hasNamespace)
			{
				writer.Write("	#region");
			}
			else
			{
				writer.Write("#region");
			}

			writer.Write(" ");
			writer.Write(name);
			writer.WriteLine();
		});
		return this;
	}
	public LINQPadFileBuilder WriteRegionEnd()
	{
		onObjectAdd.Enqueue(writer =>
		{
			if (hasNamespace)
			{
				writer.Write("	#endregion");
			}
			else
			{
				writer.Write("#endregion");
			}
			writer.WriteLine();
		});
		return this;
	}
	public LINQPadFileBuilder WriteObject(string code)
	{
		onObjectAdd.Enqueue(writer =>
		{
			var lines = code.Split(writer.NewLine);

			foreach (var line in lines)
			{
				var value = line.Trim();

				if (!value.StartsWith("///") && value != string.Empty)
				{
					writer.Write(string.Create(hasNamespace ? line.Length + 1 : line.Length, line, (span, line) =>
					{
						if (hasNamespace)
						{
							('\t' + line).CopyTo(span);
						}
						else
						{
							line.CopyTo(span);
						}
					}));
					writer.WriteLine();
				}
			}
		});

		return this;
	}

	public void Build()
	{
		using var file = File.Create(System.IO.Path.Combine(Path, $"{ProjectName}.linq"));
		using var writer = new StreamWriter(file, Encoding.UTF8, leaveOpen: true);

		writer.WriteLine("<Query Kind=\"Program\">");

		foreach (var action in onRefAdd)
		{
			action.Invoke(writer);
		}
		foreach (var action in onUsingAdd)
		{
			action.Invoke(writer);
		}

		writer.WriteLine("</Query>");

		foreach (var action in onLoadAdd)
		{
			action.Invoke(writer);
		}

		writer.WriteLine();
		writer.WriteLine("void Main()");
		writer.WriteLine("{");
		writer.WriteLine();
		writer.WriteLine("}");
		writer.WriteLine();

		foreach (var action in onObjectAdd)
		{
			action.Invoke(writer);
		}

		writer.Flush();
		file.Flush();
	}
}