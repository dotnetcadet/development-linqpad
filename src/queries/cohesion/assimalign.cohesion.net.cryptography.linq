<Query Kind="Program">
<Namespace>System</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>System.Threading.Tasks</Namespace>
<Namespace>System.Security.Cryptography.X509Certificates</Namespace>
<Namespace>Assimalign.Cohesion.Net.Cryptography.Internal</Namespace>
<Namespace>System.Security</Namespace>
<Namespace>System.Security.Cryptography</Namespace>
<Namespace>System.IO</Namespace>
<Namespace>System.Runtime.Versioning</Namespace>
<Namespace>System.Xml.Linq</Namespace>
<Namespace>System.Diagnostics</Namespace>
<Namespace>System.Globalization</Namespace>
<Namespace>System.Text.RegularExpressions</Namespace>
<Namespace>System.Reflection</Namespace>
</Query>

void Main()
{

}

#region Assimalign.Cohesion.Net.Cryptography(net8.0)
namespace Assimalign.Cohesion.Net.Cryptography
{
	#region \
	public sealed class CertificateContext
	{
	}
	public sealed class CertificateManager : ICertificateManager
	{
	    private readonly CertificateManagerOptions options;
	    public CertificateManager(CertificateManagerOptions options)
	    {
	        this.options = options;
	    }
	    public ICertificateProvider GetMachineCertificateProvider(string storeName)
	    {
	        if (OperatingSystem.IsWindows())
	        {
	            return GetWindowsCertificateProvider(storeName, StoreLocation.LocalMachine);
	        }
	        if (OperatingSystem.IsLinux())
	        {
	            return GetUnixCertificateProvider(storeName, StoreLocation.LocalMachine);
	        }
	        if (OperatingSystem.IsMacOS())
	        {
	            return GetMacOsCertificateProvider(storeName, StoreLocation.LocalMachine);
	        }
	        throw new PlatformNotSupportedException();
	    }
	    public ICertificateProvider GetUserCertificateProvider(string storeName)
	    {
	        if (OperatingSystem.IsWindows())
	        {
	            return GetWindowsCertificateProvider(storeName, StoreLocation.CurrentUser);
	        }
	        if (OperatingSystem.IsLinux())
	        {
	            return GetUnixCertificateProvider(storeName, StoreLocation.CurrentUser);
	        }
	        if (OperatingSystem.IsMacOS())
	        {
	            return GetMacOsCertificateProvider(storeName, StoreLocation.CurrentUser);
	        }
	        throw new PlatformNotSupportedException();
	    }
	    private ICertificateProvider GetWindowsCertificateProvider(string storeName, StoreLocation storeLocation)
	    {
	        return new WindowsCertificateProvider(Enum.TryParse<StoreName>(storeName, true, out var name) ? name.ToString() : storeName, storeLocation);
	    }
	    private ICertificateProvider GetUnixCertificateProvider(string storeName, StoreLocation storeLocation)
	    {
	        return new UnixCertificateProvider(Enum.TryParse<StoreName>(storeName, true, out var name) ? name.ToString() : storeName, storeLocation);
	    }
	    private ICertificateProvider GetMacOsCertificateProvider(string storeName, StoreLocation storeLocation)
	    {
	        return new MacOsCertificateProvider(Enum.TryParse<StoreName>(storeName, true, out var name) ? name.ToString() : storeName, storeLocation);
	    }
	    public static ICertificateManager Create(Action<CertificateManagerOptions> configure)
	    {
	        var options = new CertificateManagerOptions();
	        configure.Invoke(options);
	        return new CertificateManager(options);
	    }
	}
	public sealed class CertificateManagerOptions
	{
	    public string ObjectId { get; set; } = "1.3.6.1.4.1.311.84.1.1";
	    public string ObjectFriendlyName { get; set; } = "Cohesion.Net Server Development Certificate";
	    public string EnhancedKeyUsageOid { get; set; } = "1.3.6.1.5.5.7.3.1";
	    public string EnhancedKeyUsageOidFriendlyName { get; set; } = "Server Authentication";
	}
	#endregion
	#region \Abstractions
	public interface ICertificateManager
	{
	    ICertificateProvider GetMachineCertificateProvider(string storeName);
	    ICertificateProvider GetUserCertificateProvider(string storeName);
	}
	public interface ICertificateProvider : IDisposable
	{
	    string StoreName { get; }
	    string StoreLocation { get; }
	    X509Store GetCertificateStore();
	    ICertificateResult CreateSelfSignedCertificate(string certificateSubject, string certificateDnsName, string certificateOid, string certificateOidName);
	    ICertificateResult GetCertificate(string thumbprint);
	    ICertificateResult SaveCertificate(X509Certificate2 certificate);
	    ICertificateResult UpdateCertificate(X509Certificate2 certificate);    
	    ICertificateResult ImportCertificate(FilePath path, string password);
	    ICertificateResult ExportCertificate(X509Certificate2 certificate, FilePath filePath);
	}
	public interface ICertificateResult
	{
	    X509Certificate2 Certificate { get; }
	    bool IsExportable { get; }
	    bool IsTrusted { get; }
	    bool IsActive { get; }
	    bool IsExpired { get; }
	    bool IsValid { get; }
	}
	#endregion
	#region \Exceptions
	public abstract class CertificateManagerException : Exception
	{
		public CertificateManagerException(string message) : base(message) { }
		public CertificateManagerException(string message, Exception innerException): base(message, innerException) { }
		internal static CertificateManagerException CertificateNotFound(string thumbprint)
		{
			return new CertificateNotFoundException(thumbprint);
		}
	}
	#endregion
	#region \Extensions
	public static class CertificateManagerExtensions
	{
	    //public static bool TryCreateOrGetDevelopmentCertificate(this ICertificateManager certificateManager, X509Certificate2 certificate)
	    //{
	    //    certificate = null;
	    //    var certificateProvider = certificateManager.GetUserCertificateProvider("");
	    //    certificateProvider.CreateSelfSignedCertificate()
	    //    using (RSA rsa = RSA.Create(2048))
	    //    {
	    //        // Create a certificate request
	    //        var certRequest = new CertificateRequest(
	    //            certificateName,
	    //            rsa,
	    //            HashAlgorithmName.SHA256,
	    //            RSASignaturePadding.Pkcs1);
	    //        // Add extensions (optional)
	    //        certRequest.CertificateExtensions.Add(
	    //            new X509BasicConstraintsExtension(false, false, 0, false));
	    //        certRequest.CertificateExtensions.Add(
	    //            new X509KeyUsageExtension(
	    //                X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment,
	    //                false));
	    //        certRequest.CertificateExtensions.Add(
	    //            new X509SubjectKeyIdentifierExtension(certRequest.PublicKey, false));
	    //        // Self-sign the certificate
	    //        certificate = certRequest.CreateSelfSigned(
	    //            DateTimeOffset.Now,
	    //            DateTimeOffset.Now.AddYears(1));
	    //        // Export the certificate to PFX format
	    //        byte[] pfxBytes = certificate.Export(X509ContentType.Pfx, "password");
	    //        // Save the certificate to a file
	    //        System.IO.File.WriteAllBytes("localhost.pfx", pfxBytes);
	    //        Console.WriteLine("Self-signed certificate created successfully!");
	    //    }
	    //}
	}
	#endregion
	#region \Internal
	[SupportedOSPlatform("MacOs")]
	internal sealed class MacOsCertificateProvider : CertificateProviderBase
	{
	    public MacOsCertificateProvider(string storeName, StoreLocation storeLocation) : base(storeName, storeLocation) { }
	    public override CertificateResult ExportCertificate(X509Certificate2 certificate, FilePath filePath)
	    {
	        throw new NotImplementedException();
	    }
	    public override CertificateResult ImportCertificate(FilePath filePath, string password)
	    {
	        if (!File.Exists(filePath.ToString()))
	        {
	            throw new Exception();
	        }
	        var certificate = new X509Certificate2(
	            filePath.ToString(), 
	            password,
	            X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);
	        return certificate;
	    }
	    public override CertificateResult SaveCertificate(X509Certificate2 certificate)
	    {
	        throw new NotImplementedException();
	    }
	    public override CertificateResult UpdateCertificate(X509Certificate2 certificate)
	    {
	        throw new NotImplementedException();
	    }
	}
	[SupportedOSPlatform("linux")]
	internal sealed class UnixCertificateProvider : CertificateProviderBase
	{
	    public UnixCertificateProvider(string storeName, StoreLocation storeLocation) : base(storeName, storeLocation) { }
	    public override CertificateResult ExportCertificate(X509Certificate2 certificate, FilePath filePath)
	    {
	        throw new NotImplementedException();
	    }
	    public override CertificateResult ImportCertificate(FilePath filePath, string password)
	    {
	        if (!File.Exists(filePath.ToString()))
	        {
	            throw new Exception();
	        }
	        var certificate = new X509Certificate2(
	            filePath.ToString(),
	            password,
	            X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);
	        return certificate;
	    }
	    public override CertificateResult SaveCertificate(X509Certificate2 certificate)
	    {
	        throw new NotImplementedException();
	    }
	    public override CertificateResult UpdateCertificate(X509Certificate2 certificate)
	    {
	        throw new NotImplementedException();
	    }
	}
	[SupportedOSPlatform("windows")]
	internal sealed class WindowsCertificateProvider : CertificateProviderBase
	{
	    public WindowsCertificateProvider(string storeName, StoreLocation storeLocation) 
	        : base(storeName, storeLocation) { }
	    public override CertificateResult ExportCertificate(X509Certificate2 certificate, FilePath filePath)
	    {
	        return certificate;
	        throw new NotImplementedException();
	    }
	    public override CertificateResult ImportCertificate(FilePath filePath, string password)
	    {
	        if (!File.Exists(filePath.ToString()))
	        {
	            throw new Exception();
	        }
	        var certificate = new X509Certificate2(
	            filePath.ToString(),
	            password,
	            X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);
	        return certificate;
	    }
	    public override CertificateResult SaveCertificate(X509Certificate2 certificate)
	    {
	        throw new NotImplementedException();
	    }
	    public override CertificateResult UpdateCertificate(X509Certificate2 certificate)
	    {
	        throw new NotImplementedException();
	    }
	}
	internal abstract class CertificateProviderBase : ICertificateProvider
	{
	    private const int CurrentCohesionNetCoreCertificateVersion = 2;
	    private const string CohesionNetHttpsOid = "1.3.6.1.4.1.311.84.1.1";
	    private const string CohesionNetHttpsOidFriendlyName = "Cohesion.Net Server HTTPS Development Certificate";
	    private const string ServerAuthenticationEnhancedKeyUsageOid = "1.3.6.1.5.5.7.3.1";
	    private const string ServerAuthenticationEnhancedKeyUsageOidFriendlyName = "Server Authentication";
	    private const int    RSAMinimumKeySizeInBits = 2048;
	    protected readonly X509Store store;
	    public CertificateProviderBase(string storeName, StoreLocation storeLocation)
	    {
	        StoreName = storeName;
	        StoreLocation = storeLocation.ToString();
	        store = new X509Store(storeName, storeLocation, OpenFlags.MaxAllowed);
	    }
	    public virtual string StoreName { get; }
	    public virtual string StoreLocation { get; }
	    public virtual X509Store GetCertificateStore() => this.store;
	    ICertificateResult ICertificateProvider.CreateSelfSignedCertificate(string certificateSubject, string certificateDnsName, string certificateOid, string certificateOidName) => CreateSelfSignedCertificate(certificateSubject, certificateDnsName, certificateOid, certificateOidName);
	    public virtual CertificateResult CreateSelfSignedCertificate(string certificateSubject, string certificateDnsName, string certificateOid, string certificateOidName)
	    {
	        var extensions = new List<X509Extension>();
	        var sanBuilder = new SubjectAlternativeNameBuilder();
	        sanBuilder.AddDnsName(certificateDnsName);
	        extensions.Add(new X509EnhancedKeyUsageExtension(new OidCollection()
	        {
	            new Oid(ServerAuthenticationEnhancedKeyUsageOid, ServerAuthenticationEnhancedKeyUsageOidFriendlyName)
	        }, critical: true));
	        extensions.Add(new X509BasicConstraintsExtension(certificateAuthority: false, hasPathLengthConstraint: false, pathLengthConstraint: 0, critical: true));
	        extensions.Add(new X509Extension(new AsnEncodedData(new Oid(certificateOid, certificateOidName), Encoding.ASCII.GetBytes(certificateOidName)), 
	            critical: false));
	        using var key = CreateKeyMaterial(RSAMinimumKeySizeInBits);
	        var request = new CertificateRequest(new X500DistinguishedName(certificateSubject), key, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
	        request.CertificateExtensions.Add(sanBuilder.Build(true));
	        request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature, true));
	        var now = DateTimeOffset.UtcNow;
	        var result = request.CreateSelfSigned(now, now.AddYears(1));
	        return result;
	        RSA CreateKeyMaterial(int minimumKeySize)
	        {
	            var rsa = RSA.Create(minimumKeySize);
	            if (rsa.KeySize < minimumKeySize)
	            {
	                throw new InvalidOperationException($"Failed to create a key with a size of {minimumKeySize} bits");
	            }
	            return rsa;
	        }
	    }
	    ICertificateResult ICertificateProvider.GetCertificate(string thumbprint) => GetCertificate(thumbprint);
	    public virtual CertificateResult GetCertificate(string thumbprint)
	    {
	        foreach (var certificate in store.Certificates.Where(cert => cert.Thumbprint == thumbprint))
	        {
	            CertificateResult result = certificate;
	            if (result.IsValid)
	            {
	                return result;
	            }
	        }
	        throw new Exception("No valid certificate was found");
	    }
	    public abstract CertificateResult SaveCertificate(X509Certificate2 certificate);
	    ICertificateResult ICertificateProvider.SaveCertificate(X509Certificate2 certificate) => this.SaveCertificate(certificate);
	    public abstract CertificateResult UpdateCertificate(X509Certificate2 certificate);
	    ICertificateResult ICertificateProvider.UpdateCertificate(X509Certificate2 certificate)=> this.UpdateCertificate(certificate);
	    public abstract CertificateResult ExportCertificate(X509Certificate2 certificate, FilePath filePath);
	    ICertificateResult ICertificateProvider.ExportCertificate(X509Certificate2 certificate, FilePath filePath) => ExportCertificate(certificate, filePath);
	    public abstract CertificateResult ImportCertificate(FilePath filePath, string password);
	    ICertificateResult ICertificateProvider.ImportCertificate(FilePath filePath, string password) => ImportCertificate(filePath, password);
	    public virtual void Dispose()
	    {
	        if (store.IsOpen)
	        {
	            store.Close();
	        }
	        store.Dispose();
	    }
	}
	internal readonly partial struct CertificateResult
	{
	    private readonly static bool isWindows = OperatingSystem.IsWindows();
	    private readonly static bool isMacOs = OperatingSystem.IsMacOS();
	    private readonly static bool isUnix = OperatingSystem.IsLinux();
	    public CertificateResult(X509Certificate2 certificate)
	    {
	        this.Certificate = certificate;
	    }
	    public X509Certificate2 Certificate { get; }
	    public bool IsValid => IsActive && IsExportable;
	    public bool IsActive => DateTimeOffset.Now >= Certificate.NotBefore && !IsExpired;
	    public bool IsExpired => DateTimeOffset.Now < Certificate.NotAfter;
	    public bool IsExportable
	    {
	        get
	        {
	            if (isWindows)
	            {
	                return IsWindowsCertificateExportable();
	            }
	            if (isUnix)
	            {
	                return IsLinuxCertificateExportable();
	            }
	            if (isMacOs)
	            {
	                return IsMacOsCertificateExportable();
	            }
	            throw new PlatformNotSupportedException();
	        }
	    }
	    public bool IsTrusted
	    {
	        get
	        {
	            if (isWindows)
	            {
	                return IsWindowsCertificateTrusted();
	            }
	            if (isUnix)
	            {
	                return IsLinuxCertificateTrusted();
	            }
	            if (isMacOs)
	            {
	                return IsMacOsCertificateTrusted();
	            }
	            throw new PlatformNotSupportedException();
	        }
	    }
	    public static implicit operator CertificateResult(X509Certificate2 certificate) => new CertificateResult(certificate);
	    public static implicit operator X509Certificate2(CertificateResult certificateResult) => certificateResult.Certificate;
	}
	internal readonly partial struct CertificateResult
	{
	    private const string MacOsCertificateSubjectRegex = "CN=(.*[^,]+).*";
	    private const string MacOSSystemKeyChain = "/Library/Keychains/System.keychain";
	    private const string MacOSFindCertificateCommandLine = "security";
	    private const string MacOSFindCertificateCommandLineArgumentsFormat = "find-certificate -c {0} -a -Z -p " + MacOSSystemKeyChain;
	    private const string MacOSFindCertificateOutputRegex = "SHA-1 hash: ([0-9A-Z]+)";
	    private static readonly TimeSpan MaxRegexTimeout = TimeSpan.FromMinutes(1);
	    private bool IsMacOsCertificateTrusted()
	    {
	        var match = Regex.Match(Certificate.Subject, MacOsCertificateSubjectRegex, RegexOptions.Singleline, MaxRegexTimeout);
	        if (!match.Success)
	        {
	            throw new InvalidOperationException($"Can't determine the subject for the certificate with subject '{Certificate.Subject}'.");
	        }
	        var subject = match.Groups[1].Value;
	        using var checkTrustProcess = Process.Start(
	            new ProcessStartInfo(
	                MacOSFindCertificateCommandLine, 
	                string.Format(CultureInfo.InvariantCulture, MacOSFindCertificateCommandLineArgumentsFormat, subject))
	        {
	            RedirectStandardOutput = true
	        });
	        var output = checkTrustProcess!.StandardOutput.ReadToEnd();
	        checkTrustProcess.WaitForExit();
	        var matches = Regex.Matches(output, MacOSFindCertificateOutputRegex, RegexOptions.Multiline, MaxRegexTimeout);
	        var hashes = matches.OfType<Match>().Select(m => m.Groups[1].Value).ToList();
	        var thumbprint = Certificate.Thumbprint;
	        return hashes.Any(h => string.Equals(h, thumbprint, StringComparison.Ordinal));
	    }
	    // Apparently there is no good way of checking on the
	    // underlying implementation if ti is exportable, so just return true.
	    private bool IsMacOsCertificateExportable()
	    {
	        return true;
	    }
	}
	internal readonly partial struct CertificateResult
	{
	    private bool IsLinuxCertificateExportable()
	    {
	        return true;
	    }
	    private bool IsLinuxCertificateTrusted()
	    {
	        return false;
	    }
	}
	internal readonly partial struct CertificateResult : ICertificateResult
	{
	    private bool IsWindowsCertificateTrusted()
	    {
	        throw new NotImplementedException();
	    }
	    private bool IsWindowsCertificateExportable()
	    {
	        using var privateKey = Certificate.GetRSAPrivateKey();
	        return (privateKey is RSACryptoServiceProvider cryptoServiceProvider && cryptoServiceProvider.CspKeyContainerInfo.Exportable) ||
	               (privateKey is RSACng cngPrivateKey && cngPrivateKey.Key.ExportPolicy == CngExportPolicies.AllowExport);
	    }   
	}
	#endregion
	#region \Internal\Exceptions
	internal sealed class CertificateNotFoundException : CertificateManagerException
	{
		public const string message = "No certificate was found for the given ThumbPrint: '{0}'";
		public CertificateNotFoundException(string thumbprint): base (string.Format(message, thumbprint)) { }
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
	#region C:\Source\repos\assimalign-cohesion\cohesion\libraries\Net\Core\src\Assimalign.Cohesion.Net.Core\Primitives\IO
	#endregion
}
#endregion
