<Query Kind="Program">
  <NuGetReference>Microsoft.Build</NuGetReference>
  <NuGetReference>Microsoft.CodeAnalysis</NuGetReference>
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <NuGetReference Prerelease="true">StronglyTypedId.Attributes</NuGetReference>
  <Namespace>Microsoft.Build.Construction</Namespace>
  <Namespace>Microsoft.CodeAnalysis</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp.Syntax</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
</Query>

//const string csproj = @"C:\Source\repos\assimalign-cohesion\cohesion\libraries\Core\Configuration\src\Assimalign.Cohesion.Configuration";
const string linq = @"C:\Source\repos\dotnetcadet\development-linqpad\src\queries\temp.linq";

const string csproj = @"C:\Source\repos\v3technology\mint-backend\application\logging\src\Mint.Logging\Mint.Logging.csproj";

void Main()
{
	
	
	var serializer = new XmlSerializer(typeof(CSProject));

	using var proj = File.Open(csproj, FileMode.Open, FileAccess.Read, FileShare.Read);
	
	var pro = serializer.Deserialize(proj);
	
	pro.Dump();
	
	
	
	
	using var configs = new MemoryStream();
	using var content = new MemoryStream();

	using var configsWriter = new StreamWriter(configs);
	using var contentWriter = new StreamWriter(content);

	var queryConfigs = new QueryConfigs();

	contentWriter.Write("""
	void Main()
	{
	
	}
	
	""");

	foreach (var item in GetCSharpFiles("/", csproj))
	{
		var region = item.Item1;
		var files = item.Item2;

		contentWriter.WriteLine($"#region {region}");

		foreach (var file in files)
		{
			using var stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read);
			var buffer = new byte[stream.Length];
			stream.Read(buffer, 0, buffer.Length);
			var text = Encoding.UTF8.GetString(buffer);

			var syntaxTree = CSharpSyntaxTree.ParseText(text);
			var root = syntaxTree.GetRoot();


			if (root is CompilationUnitSyntax unit)
			{
				foreach (var statement in unit.Usings.Select(p => p.ToFullString()))
				{
					if (!queryConfigs.Namespace.Contains(statement))
					{
						queryConfigs.Namespace.Add(statement);
					}
				}
			}
		}

		contentWriter.WriteLine($"#endregion");
	}

	configsWriter.WriteLine($"<Query Kind=\"Program\">");
	
	foreach (var ns in queryConfigs.Namespace)
	{
		configsWriter.WriteLine($"<Namespace>{ns.Trim().TrimEnd(';').Replace(Environment.NewLine, "")}</Namespace>");
	}
	configsWriter.WriteLine($"</Query>");
	
	configsWriter.Flush();
	contentWriter.Flush();

	configs.Position = 0;
	content.Position = 0;

	using var linqDoc = File.Open(linq, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);

	configs.CopyTo(linqDoc);
	content.CopyTo(linqDoc);
	
	
	linqDoc.Position = 0;
	
	var reader = new StreamReader(linqDoc);
	
	reader.ReadToEnd().Dump();
}


public class QueryConfigs
{
	public List<string> Namespace { get; set; } = new List<string>();
}


private IEnumerable<(string, string[])> GetCSharpFiles(string parent, string path)
{
	var files = Directory.GetFiles(path, "*.cs");
	var directories = Directory.GetDirectories(path);

	yield return (parent, files);

	foreach (var directory in directories)
	{
		var parentPath = directory.Split('\\')[^1];

		if (parent.Length > 1)
		{
			parentPath = Path.Join(parent, parentPath);
		}
		else
		{
			parentPath = "/" + parentPath;
		}
		if (!directory.EndsWith("obj") && !directory.EndsWith("bin"))
		{
			foreach (var item in GetCSharpFiles(parentPath, directory))
			{
				yield return item;
			}
		}
	}
}



#region CSProj Models

[XmlRoot("Project")]
public class CSProject
{
	[XmlArray("ItemGroup")]
	[XmlArrayItem("PackageReference", typeof(CSProjectPackageReference))]
	public List<CSProjectReference> References { get; set; } = new();
}

public abstract class CSProjectReference
{
	[XmlAttribute("Include")]
	public string? Include { get; set; }
}

public class CSProjectPackageReference : CSProjectReference
{
	[XmlAttribute("Version")]
	public string? Version { get; set; }
}

#endregion
