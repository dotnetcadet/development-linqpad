<Query Kind="Program">
  <Namespace>System.Security</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
  <Namespace>System.Security.Cryptography.X509Certificates</Namespace>
</Query>



void Main()
{
	try
	{
		X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
		store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
		X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
		X509Certificate2Collection fcollection = (X509Certificate2Collection)collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
		X509Certificate2Collection scollection = X509Certificate2UI.SelectFromCollection(fcollection, "Test Certificate Select", "Select a certificate from the following list to get information on that certificate", X509SelectionFlag.MultiSelection);
		Console.WriteLine("Number of certificates: {0}{1}", scollection.Count, Environment.NewLine);
		foreach (X509Certificate2 x509 in scollection)
		{
			X500DistinguishedName dname = new X500DistinguishedName(x509.SubjectName);
			Console.WriteLine("X500DistinguishedName: {0}{1}", dname.Name, Environment.NewLine);
			x509.Reset();
		}
		store.Close();
	}
	catch (CryptographicException)
	{
		Console.WriteLine("Information could not be written out for this certificate.");
	}
	//var content = "This is data I want to encrypt using a Hash Function.";
	//var hashSvc = SHA512.Create();
	
	CreateCertificate();
}

private void CreateCertificate()
{
	var certificateRsaKey = RSA.Create(2048);
	var certificateRequest = new CertificateRequest(
		"CN=Tester", 
		certificateRsaKey, 
		HashAlgorithmName.SHA512, 
		RSASignaturePadding.Pkcs1);
	
	certificateRequest.CertificateExtensions.Add(new X509KeyUsageExtension(
		X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature, 
		true));
		
	certificateRequest.CertificateExtensions.Add(
	
	var start = DateTimeOffset.UtcNow;
	var end = start.AddDays(90);
	
	var certificate = certificateRequest.CreateSelfSigned(start, end).Dump();
}

