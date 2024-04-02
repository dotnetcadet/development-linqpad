<Query Kind="Program">
  <NuGetReference>CsvHelper</NuGetReference>
  <Namespace>CsvHelper.Configuration.Attributes</Namespace>
  <Namespace>CsvHelper</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>CsvHelper.Configuration</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
</Query>

private static string MapFile = @"C:\Users\chase\OneDrive\Documents\Expenses\expense-report-wellsfargo-etl.json";
private static string Output = @"C:\Users\chase\OneDrive\Documents\Expenses\wellsfargo\transactions.json";
private static Tuple<string, string>[] Inputs = new[]
{
	new Tuple<string, string>("Checking Account x7121", @"C:\Users\chase\Downloads\Checking1.csv"),
	new Tuple<string, string>("Credit Card Account x0236", @"C:\Users\chase\Downloads\CreditCard4.csv")
};

private List<Transaction> Transactions = new();

void Main()
{
	var options = new JsonSerializerOptions()
	{
		PropertyNameCaseInsensitive = true,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase
	};
	
	options.Converters.Add(new JsonStringEnumConverter());
	
	var maps = JsonSerializer.Deserialize<IEnumerable<TransactionMap>>(
		File.Open(MapFile, FileMode.Open, FileAccess.Read, FileShare.Read), options);
		

	foreach (var file in Inputs)
	{
		using var reader = new StreamReader(file.Item2);
		using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			HasHeaderRecord = false,
		});

		while (csv.Read())
		{
			var transaction = csv.GetRecord<Transaction>();

			transaction.Id = Guid.NewGuid();
			transaction.Account = file.Item1;
			transaction.TransactionType = transaction.Amount < 0 ? TransactionType.Withdrawal :TransactionType.Deposit;

			foreach (var map in maps)
			{
				if (
					(map.DescriptionContainsAny.Any() &&
					map.DescriptionContainsAny.Any(p => transaction.Description.Contains(p, StringComparison.OrdinalIgnoreCase))) || 
					(
						!string.IsNullOrEmpty(transaction.Description) && !string.IsNullOrEmpty(map.DescriptionContains) && 
						transaction.Description.Contains(map.DescriptionContains, StringComparison.OrdinalIgnoreCase))
					)
				{
					transaction.Business = map.Business;
					transaction.ExpenseType = map.ExpenseType;
					transaction.Categories.AddRange(map.Categories);
					break;
				}
			}
			
			Transactions.Add(transaction);
		}
	}
	
	Transactions.Count.Dump();

	using var stream = File.Open(Output, FileMode.Create);

	JsonSerializer.Serialize(stream, Transactions, options);
}



#region Models
public class Transaction
{
	[Ignore]
	public Guid? Id { get; set; }
	
	[Index(0)]
	public DateOnly? Date { get; set; }

	[Index(1)]
	public decimal? Amount { get; set; }

	[Index(4)]
	public string Description { get; set; }

	[Ignore]
	public string Business { get; set; }

	[Ignore]
	public TransactionType TransactionType { get; set; }

	[Ignore]
	public ExpenseType ExpenseType { get; set; }

	[Ignore]
	public string Account { get; set; }

	[Ignore]
	public List<TransactionCategory> Categories { get; set; } = new();
}
public class TransactionMap
{
	public string DescriptionContains { get; set; } 
	public IEnumerable<string> DescriptionContainsAny { get; set; } = new List<string>();
	public string Business { get; set; }
	public ExpenseType ExpenseType { get; set; }
	public IEnumerable<TransactionCategory> Categories { get; set; }
}
public enum ExpenseType
{
	None = 0,
	Fixed,
	Variable
}
public enum TransactionType
{
	Withdrawal,
	Deposit
}
public enum TransactionCategory
{
	Bill,
	Rent,
	Savings,
	Subscription,
	Entertainment,
	FoodAndDrinks,
	TakeOut,
	Restaruants,
	Shopping,
	Income,
	Groceries,
	Payment,
	Transportation,
	Medical,
	Utility,
	Necessary,
	Membership,
	Retirement,
	Insurance,
	HealthAndBeauty
}
#endregion