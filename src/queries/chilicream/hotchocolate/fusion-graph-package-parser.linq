<Query Kind="Program">
  <NuGetReference>HotChocolate.Fusion</NuGetReference>
  <Namespace>HotChocolate.Fusion</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>HotChocolate.Language</Namespace>
  <Namespace>HotChocolate.Language.Utilities</Namespace>
</Query>

async Task Main()
{
	using var package = FusionGraphPackage.Open(@"C:\Source\repos\v3technology\mint-backend\domains\root\src\Mint.Root.Graphql\Root.fgp", FileAccess.Read);
	var document = await package.GetFusionGraphAsync();

	
	foreach (var def in document.Definitions)
	{
		if (def is ObjectTypeDefinitionNode node)
		{
			foreach (var field in node.Fields)
			{
				field.Print().Dump();
			}
		}
	}
	
}

// You can define other methods, fields, classes and namespaces here
