<Query Kind="Program">
  <NuGetReference>Azure.Identity</NuGetReference>
  <NuGetReference>Microsoft.Graph</NuGetReference>
  <Namespace>Microsoft.Graph</Namespace>
  <Namespace>Microsoft.Graph.Models</Namespace>
  <Namespace>Azure.Identity</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

const string ClientId = "";
const string ClientSecret = "";
const string TenantId = "";

async Task Main()
{
	var options = new ClientSecretCredentialOptions
	{
		AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
	};

	var credential = new ClientSecretCredential(
		TenantId,
		ClientId,
		ClientSecret,
		options);

	var client = new GraphServiceClient(
		credential,
		["https://graph.microsoft.com/.default"]);


	var users = await client.Users.GetAsync(options =>
	{
		options.QueryParameters.Select = ["Id", "UserPrincipalName", "identities"];
		options.QueryParameters.Top = 200;
	});

	foreach (var user in users.Value)
	{
		var values = user!.UserPrincipalName!.Split('@');

		if (Guid.TryParse(values[0], out var id))
		{
			var identity = user.Identities?.FirstOrDefault(id => id?.SignInType is not null && id.SignInType.Equals("emailAddress", StringComparison.OrdinalIgnoreCase));

			if (identity is not null)
			{
				var upn = FormatUpn(identity.IssuerAssignedId!, identity.Issuer!);

				try
				{
					var patch = await client.Users[user.Id]
						.PatchAsync(new()
						{
							UserPrincipalName = upn
						});
					upn.Dump();
				}
				catch
				{
					(upn + "	- error").Dump();
				}
			}
		}
	}
}

private string FormatUpn(string email, string issuer)
{
	return string.Join('@', email.Replace('@', '_'), issuer);
}