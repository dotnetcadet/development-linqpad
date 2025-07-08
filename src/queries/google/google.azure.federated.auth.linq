<Query Kind="Program">
  <NuGetReference>Azure.Identity</NuGetReference>
  <NuGetReference>Google.Apis.Auth</NuGetReference>
  <NuGetReference>Microsoft.Graph</NuGetReference>
  <Namespace>Azure.Core</Namespace>
  <Namespace>Microsoft.Graph</Namespace>
  <Namespace>Microsoft.Graph.Models</Namespace>
  <Namespace>Microsoft.Graph.Users.Item.SendMail</Namespace>
  <Namespace>Microsoft.Identity.Client</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>


using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;

async Task Main()
{
	var serviceAccount = "";
	var clientAssertionCredentialOptions = new ClientAssertionCredentialOptions() 
	{
		ClientId =  "",
		TenantId = "",
		Authority = "https://login.microsoftonline.com/",
		TokenCallback = async () =>
		{
			var credentials = await GoogleCredential.GetApplicationDefaultAsync();
			if (!string.IsNullOrWhiteSpace(serviceAccount))
			{
				credentials = credentials.Impersonate(new ImpersonatedCredential.Initializer(serviceAccount));
			}
			var tokenOptions = OidcTokenOptions.FromTargetAudience("api://AzureADTokenExchange");
			var token = await credentials.GetOidcTokenAsync(tokenOptions);
			return await token.GetAccessTokenAsync();
		}
	};
	var clientAssertionCredential = new ClientAssertionCredential(clientAssertionCredentialOptions);
	var client = new GraphServiceClient(clientAssertionCredential);

	await client.Users[""]
		.SendMail
		.PostAsync(new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody()
		{
			Message = new()
			{
				Subject = "Test Email",
				Sender = new  Recipient
				{
					EmailAddress = new EmailAddress
					{
						Address = ""
					}
				},
				Body = new ItemBody
				{
					ContentType = BodyType.Text,
					Content = "Test Content"
				},
				ToRecipients = new List<Recipient>()
				{
					new Recipient
					{
						EmailAddress = new EmailAddress
						{
							Address = ""
						}
					}
				}
			}
		});

}


public delegate Task<string> ClientTokenCallback();

public sealed class ClientAssertionCredentialOptions
{
	public string ClientId { get; set; }
	public string TenantId { get; set; }
	public string Authority { get; set; }
	public ClientTokenCallback TokenCallback { get; set; }
}
public sealed class ClientAssertionCredential : TokenCredential
{
	private readonly ClientTokenCallback callback;
	private readonly string clientId;
	private readonly string tenantId;
	private readonly string authority;
	
	
	public ClientAssertionCredential(ClientAssertionCredentialOptions options)
	{
		this.clientId =  options.ClientId;
		this.tenantId =  options.TenantId;
		this.authority = options.Authority;
		this.callback = options.TokenCallback;
	}

	public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken = default)
	{
		return GetTokenImplAsync(requestContext, cancellationToken).GetAwaiter().GetResult();
	}
	public override ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken = default)
	{
		return GetTokenImplAsync(requestContext, cancellationToken);
	}
	private async ValueTask<AccessToken> GetTokenImplAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
	{
		var clientAsserrtion = await callback.Invoke();		

		try
		{
			// pass token as a client assertion to the confidential client app
			var app = ConfidentialClientApplicationBuilder
				.Create(this.clientId)
				.WithClientAssertion(clientAsserrtion)
				.Build();

			var authResult = await app.AcquireTokenForClient(requestContext.Scopes)
				.WithAuthority(this.authority + this.tenantId)
				.ExecuteAsync();

			AccessToken token = new AccessToken(
				authResult.AccessToken,
				authResult.ExpiresOn);

			return token;
		}
		catch (Exception ex)
		{
			throw (ex);
		}
	}
}
