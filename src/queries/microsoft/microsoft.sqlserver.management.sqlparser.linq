<Query Kind="Program">
  <NuGetReference>Microsoft.SqlServer.Management.SqlParser</NuGetReference>
  <Namespace>System.Collections.ObjectModel</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <RuntimeVersion>6.0</RuntimeVersion>
</Query>

using System.IO;
using Microsoft.SqlServer.Management.SqlParser;
using Microsoft.SqlServer.Management.SqlParser.Parser;

private static Block[] blocks = new Block[50];

void Main()
{
	//var doc = Parser.Parse("Select c.FirstName, c.LastName From MyTable c").Dump();
	//
	//doc.Script.Children
	
	
	
	
	
}




public int BeginCapture()
{
	int i = 0;

	// 
	for (; i < blocks.Length; i++)
	{
		var block = blocks[i];

		if (!block.InUse)
		{
			block.InUse = true;
			block.Start = 0;
			block.StartLine = 0;
			
			blocks[i] = block;
			return i;
		}
	}

	Array.Resize(ref blocks, blocks.Length + 25);

	var item = blocks[i +1];
	
	item.InUse = true;
	
	return i + 1;

}




struct Block
{
	public bool InUse;

	public int Start;
	public int StartLine;
	public int End;
	public int EndLine;

	//public static implicit operator TokenBlock(Block block) => new TokenBlock();
}