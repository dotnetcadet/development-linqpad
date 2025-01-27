<Query Kind="Program">
  <NuGetReference>KubernetesClient</NuGetReference>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>k8s</Namespace>
  <Namespace>k8s.Models</Namespace>
</Query>

void Main()
{
	var config = KubernetesClientConfiguration.BuildConfigFromConfigObject(new()
	{
		
		Clusters = [
			new() {
				ClusterEndpoint = new() {
					Server
				}
			}
		]
	});
	var client = new Kubernetes(config);


	client.CreateNode(new()
	{
		
	});

	
	
	var deployment = new V1Deployment() {
	
	Spec = new() 
	{
		Template = new() {
		 
			Spec = new () {
			
				Containers = [
					new() {
						 Image = "",
						 
					}
				],
				
			},
			
		},
		Strategy = new V1DeploymentStrategy() {
			Type = ""
		}
	}
	
	};


	
	
}

