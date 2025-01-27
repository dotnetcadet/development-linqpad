<Query Kind="Program" />

using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.IO

void Main()
{
	var timestamp = DateTimeOffset.UtcNow;
	var sanBuilder = new SubjectAlternativeNameBuilder();
	
	sanBuilder.AddDnsName("localhost");

	using var ec = ECDsa.Create(ECCurve.NamedCurves.nistP256);
	var certificateRequest = new CertificateRequest("CN=localhost", ec, HashAlgorithmName.SHA256);
	// Adds purpose
	certificateRequest.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(new OidCollection
		{
			new("1.3.6.1.5.5.7.3.1") // serverAuth
        }, false));
	// Adds usage
	certificateRequest.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, false));
	// Adds subject alternate names
	certificateRequest.CertificateExtensions.Add(sanBuilder.Build());
	// Sign
	using var crt = certificateRequest.CreateSelfSigned(timestamp, timestamp.AddDays(14)); // 14 days is the max duration of a certificate for this
	
	var certificate = new X509Certificate2(crt.Export(X509ContentType.Pfx));
	
	certificate.Dump();
}

