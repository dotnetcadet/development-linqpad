<Query Kind="Program">
  <NuGetReference>Microsoft.Identity.Client</NuGetReference>
  <Namespace>Microsoft.Identity.Client</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>






async Task Main()
{

	var tenant = "v3techdev.onmicrosoft.com";
	var clientId = "";
	var authority = $"https://v3techdev.b2clogin.com/tfp/dev.mint-ui-ehn.net/B2C_1_MINT_SIGN_IN/";
	var temp = "https://v3techdev.b2clogin.com/v3techdev.onmicrosoft.com/B2C_1_MINT_SIGN_IN";
	var scopes = new string[] { "https://mint-api.dev.mint-ui-ehn.net/Data.Access" }; // Adjust scope as needed
	
	var client2 = ConfidentialClientApplicationBuilder.Create("").Build();
	var client = PublicClientApplicationBuilder.Create(clientId)
		.WithB2CAuthority(authority)
		.Build();
		
		
	var username = "";
	var password = "";
		
	var result = await client
		.AcquireTokenByUsernamePassword(scopes, username, password)
		.ExecuteAsync();
		
		
		
		
}










#region Helpers 

public static class MsalExtensions
{
	public static UserAssertion ParseUserAssertion(this string jwt)
	{
		var parts = jwt.Split(' ');
		
		
		return new UserAssertion(parts[1]);
	}
}

#endregion

// You can define other methods, fields, classes and namespaces here
