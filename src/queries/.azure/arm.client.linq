<Query Kind="Program">
  <NuGetReference>Azure.Identity</NuGetReference>
  <NuGetReference>Azure.ResourceManager</NuGetReference>
  <NuGetReference>Azure.ResourceManager.AppConfiguration</NuGetReference>
  <NuGetReference>Azure.ResourceManager.AppService</NuGetReference>
  <NuGetReference>Azure.ResourceManager.Authorization</NuGetReference>
  <NuGetReference>Azure.ResourceManager.KeyVault</NuGetReference>
  <NuGetReference>Azure.ResourceManager.ServiceBus</NuGetReference>
  <NuGetReference>Azure.ResourceManager.Storage</NuGetReference>
  <Namespace>Azure.Identity</Namespace>
  <Namespace>Azure.ResourceManager</Namespace>
  <Namespace>Azure.ResourceManager.AppConfiguration</Namespace>
  <Namespace>Azure.ResourceManager.Authorization</Namespace>
  <Namespace>Azure.ResourceManager.KeyVault</Namespace>
  <Namespace>Azure.ResourceManager.ServiceBus</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Azure.Core</Namespace>
  <Namespace>Azure</Namespace>
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

	var client =  new ArmClient(credential);
	

	


	var subscription = client.GetDefaultSubscription();
	var definitions = client.GetAuthorizationRoleDefinitions(subscription.Id);
	
	
	
	
	client.GetResourceGroupResource(ResourceIdentifier.Parse(""))
		.GetRoleAssignments()
		.Crea
		.GetAppConfigurationStore("");
		

	
	
	await foreach (var definition in definitions)
	{
		definition.
	}
}

// You can define other methods, fields, classes and namespaces here
