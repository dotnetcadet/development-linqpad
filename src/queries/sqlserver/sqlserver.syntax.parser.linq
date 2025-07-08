<Query Kind="Program">
  <NuGetReference>Microsoft.SqlServer.Management.SqlParser</NuGetReference>
  <Namespace>Microsoft.SqlServer.Management.SqlParser.Parser</Namespace>
</Query>

void Main()
{
	var query = @"Select   field * -1.0 AS Field1 from dbo.master where MAX(field1) > 200 AND (field = 'test' OR field3 = '3')";
	ParseSql(query).Dump();
	ParseResult result = Parser.Parse(query);
	
	result.Script.Dump();
}

public struct TokenInfo
{
	public int Start;
	public int End;
	public bool IsPairMatch;
	public bool IsExecAutoParamHelp;
	public string Sql;
	public Tokens Token;
}

IEnumerable<TokenInfo> ParseSql(string sql)
{
	ParseOptions parseOptions = new ParseOptions();
	Scanner scanner = new Scanner(parseOptions);

	int state = 0,
		start,
		end,
		lastTokenEnd = -1,
		token;

	bool isPairMatch, isExecAutoParamHelp;

	List<TokenInfo> tokens = new List<TokenInfo>();

	scanner.SetSource(sql, 0);

	while ((token = scanner.GetNext(ref state, out start, out end, out isPairMatch, out isExecAutoParamHelp)) != (int)Tokens.EOF)
	{
		TokenInfo tokenInfo =
			new TokenInfo()
			{
				Start = start,
				End = end,
				IsPairMatch = isPairMatch,
				IsExecAutoParamHelp = isExecAutoParamHelp,
				Sql = sql.Substring(start, end - start + 1),
				Token = (Tokens)token,
			};

		tokens.Add(tokenInfo);

		lastTokenEnd = end;
	}

	return tokens;
}