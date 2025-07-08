<Query Kind="Program">
  <NuGetReference>KubernetesClient</NuGetReference>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>k8s</Namespace>
  <Namespace>k8s.Models</Namespace>
  <Namespace>k8s.KubeConfigModels</Namespace>
</Query>

void Main()
{
	
	//var config = KubernetesClientConfiguration.BuildConfigFromConfigObject(new()
	//{
	//	Clusters = 
	//	[
	//		new Cluster() 
	//		{
	//			Name = "development",
	//			ClusterEndpoint = new ClusterEndpoint()
	//			{
	//				Server = "https://127.0.0.1:53366",
	//				
	//			},
	//		},
	//	],
	//});
	
	var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
	var client = new Kubernetes(config);
	
	client.ListNode().Dump();
	
	//client.CreateNamespace(new()
	//{
	//	Metadata = new() 
	//	{
	//		
	//	}
	//});
	
//	client.Crea

	var deployment = new V1Deployment()
	{
		Spec = new V1DeploymentSpec() 
		{
			Template = new V1PodTemplateSpec() 
			{
				Spec = new V1PodSpec() 
				{
					Containers = 
					[
						new V1Container() 
						{
							Image = "",
						}
					],
				},
			},
			Strategy = new V1DeploymentStrategy() 
			{
				Type = "",
			},
		},
	};


	
	
}

