<Query Kind="Program">
  <Reference Relative="..\..\..\..\..\v3technology\mint-backend\src\Mint.Database\bin\Debug\net8.0\Mint.Database.dll">C:\Source\repos\v3technology\mint-backend\src\Mint.Database\bin\Debug\net8.0\Mint.Database.dll</Reference>
  <Reference Relative="..\..\..\..\..\v3technology\mint-backend\src\Mint.Database\bin\Debug\net8.0\Mint.Domain.dll">C:\Source\repos\v3technology\mint-backend\src\Mint.Database\bin\Debug\net8.0\Mint.Domain.dll</Reference>
  <Reference Relative="..\..\..\..\..\v3technology\mint-backend\src\Mint.Database\bin\Debug\net8.0\Mint.Shared.dll">C:\Source\repos\v3technology\mint-backend\src\Mint.Database\bin\Debug\net8.0\Mint.Shared.dll</Reference>
  <NuGetReference>Azure.Identity</NuGetReference>
  <NuGetReference>CsvHelper</NuGetReference>
  <NuGetReference>Microsoft.Graph</NuGetReference>
  <Namespace>Azure.Identity</Namespace>
  <Namespace>CsvHelper</Namespace>
  <Namespace>CsvHelper.Configuration</Namespace>
  <Namespace>Microsoft.Graph</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

//#load "domain-driven-design\value-objects\value-object-password"

using System.Linq;
using Mint.Database;
using Mint.Domain;
using Mint.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

const string Connection = "";
const string ClientId = "";
const string ClientSecret = "";
const string TenantId = "";


async Task Main()
{

	var serviceProvider = new ServiceCollection()
		.AddDbContext<MintDbContext>((serviceProvider, options) =>
		{
			options.UseSqlServer(Connection, o => o.UseNetTopologySuite().UseHierarchyId());
		})
		.BuildServiceProvider();
	
	var dbContext = serviceProvider.GetRequiredService<MintDbContext>();

	var entries = new List<UserEntry>(GetEntries());
	
	var client = GetClient();
	
	foreach (var chunk in entries.Chunk(20))
	{
		var batchContent = new BatchRequestContentCollection(client);

		foreach (var entry in chunk)
		{
			string password;

			var pwd = Password.New(out password);

			var requestInfo = client
				.Users
				.ToPostRequestInformation(new()
				{

					DisplayName = $"{entry.FirstName} {entry.LastName}",
					AccountEnabled = true,
					GivenName = entry.FirstName,
					Surname = entry.LastName,
					PasswordProfile = new()
					{
						ForceChangePasswordNextSignIn = true,
						Password = password
					},
					Identities = new()
					{
					new()
					{
						SignInType = "emailAddress",
						Issuer = "v3techus.onmicrosoft.com",
						IssuerAssignedId = entry.Email
					}
					}
				});

			var requestId = await batchContent.AddBatchRequestStepAsync(requestInfo);
			
			entry.Password = pwd;
			entry.TextPassword = password;
			entry.RequestId = requestId;
		}

		var batchResponse = await client.Batch.PostAsync(batchContent);
		
		foreach(var item in chunk)
		{
			var entraUser = await batchResponse.GetResponseByIdAsync<Microsoft.Graph.Models.User>(item.RequestId);
			
			var user = new User
			{
				Id = (UserId)entraUser!.Id!,
				Password = item.Password,
				Info = new()
				{
					FirstName = item.FirstName,
					LastName = item.LastName,
					IsEnabled = true,
					Email = item.Email
				},
				Type = UserType.Provider
			};

			var organizations = dbContext.Organizations
				.Where(p => p.OrganizationId == new OrganizationId(item.OrganizationId))
				.AsAsyncEnumerable();

			await foreach (var organization in organizations)
			{
				user.Organizations.Add(organization);
			}

			var entry = await dbContext.Users.AddAsync(user);
			var entryChanges = await dbContext.SaveChangesAsync();
		}
	}

	var config = new CsvConfiguration(CultureInfo.InvariantCulture)
	{
		NewLine = Environment.NewLine
	};

	using var file = File.Open("C:\\Users\\chase\\Downloads\\users.csv", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
	using var streamWriter = new StreamWriter(file);
	using var writer = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
	
	
	writer.WriteRecords(entries);
	
	
	writer.Flush();
	
	writer.Dispose();
	streamWriter.Dispose();
	file.Dispose();
}

private IEnumerable<UserEntry> GetEntries()
{
	var config = new CsvConfiguration(CultureInfo.InvariantCulture)
	{
		NewLine = Environment.NewLine,
		HasHeaderRecord = false,
	};

	var stream = File.OpenRead("C:\\Users\\chase\\Downloads\\Connex Authorized Users - for Aadli.csv");

	var streamReader = new StreamReader(stream);
	using var csv = new CsvReader(streamReader, config, leaveOpen: true);

	csv.Context.RegisterClassMap<UserEntryMap>();

	var records = csv.GetRecords<UserEntry>();

	return records.ToArray();
}


private GraphServiceClient GetClient() 
{
	// using Azure.Identity;
	var options = new ClientSecretCredentialOptions
	{
		AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
	};

	var credential = new ClientSecretCredential(
		TenantId,
		ClientId,
		ClientSecret,
		options);

	return new GraphServiceClient(
		credential,
		["https://graph.microsoft.com/.default"]);

}


public class UserEntryMap : ClassMap<UserEntry>
{
	public UserEntryMap()
	{
		Map(p=>p.RequestId).Ignore();
		Map(p=>p.Password).Ignore();
		Map(p=>p.FirstName).Index(0);
		Map(p=>p.LastName).Index(1);
		Map(p=>p.Email).Index(2);
		Map(p=>p.OrganizationId).Index(3);
	}
}

public class UserEntry
{
	public string? RequestId { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? Email { get; set; } 
	public int OrganizationId {get; set;}
	public string? TextPassword { get; set; }
	public Password Password { get; set; }
	
}
