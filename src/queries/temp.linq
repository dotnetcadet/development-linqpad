<Query Kind="Program">
<NuGetReference Version="8.0.0">Ardalis.SmartEnum</NuGetReference>
<NuGetReference Version="8.0.0">Ardalis.SmartEnum.SystemTextJson</NuGetReference>
<NuGetReference Version="6.4.2">AsyncKeyedLock</NuGetReference>
<NuGetReference Version="1.40.0">Azure.Core</NuGetReference>
<NuGetReference Version="1.4.1">Azure.Data.AppConfiguration</NuGetReference>
<NuGetReference Version="1.12.0">Azure.Identity</NuGetReference>
<NuGetReference Version="4.7.0">Azure.Messaging.EventGrid</NuGetReference>
<NuGetReference Version="4.6.0">Azure.Security.KeyVault.Secrets</NuGetReference>
<NuGetReference Version="1.2.0">CacheManager.Core</NuGetReference>
<NuGetReference Version="1.7.0">DnsClient</NuGetReference>
<NuGetReference Version="1.9.2">EasyCaching.Core</NuGetReference>
<NuGetReference Version="4.5.0">EFCoreSecondLevelCacheInterceptor</NuGetReference>
<NuGetReference Version="6.0.0">Microsoft.Bcl.AsyncInterfaces</NuGetReference>
<NuGetReference Version="5.1.5">Microsoft.Data.SqlClient</NuGetReference>
<NuGetReference Version="8.0.6">Microsoft.EntityFrameworkCore.Abstractions</NuGetReference>
<NuGetReference Version="8.0.6">Microsoft.EntityFrameworkCore</NuGetReference>
<NuGetReference Version="8.0.6">Microsoft.EntityFrameworkCore.Relational</NuGetReference>
<NuGetReference Version="8.0.6">Microsoft.EntityFrameworkCore.SqlServer.Abstractions</NuGetReference>
<NuGetReference Version="8.0.6">Microsoft.EntityFrameworkCore.SqlServer</NuGetReference>
<NuGetReference Version="8.0.6">Microsoft.EntityFrameworkCore.SqlServer.HierarchyId</NuGetReference>
<NuGetReference Version="8.0.6">Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite</NuGetReference>
<NuGetReference Version="7.3.0">Microsoft.Extensions.Configuration.AzureAppConfiguration</NuGetReference>
<NuGetReference Version="4.61.3">Microsoft.Identity.Client</NuGetReference>
<NuGetReference Version="4.61.3">Microsoft.Identity.Client.Extensions.Msal</NuGetReference>
<NuGetReference Version="6.35.0">Microsoft.IdentityModel.Abstractions</NuGetReference>
<NuGetReference Version="6.35.0">Microsoft.IdentityModel.JsonWebTokens</NuGetReference>
<NuGetReference Version="6.35.0">Microsoft.IdentityModel.Logging</NuGetReference>
<NuGetReference Version="6.35.0">Microsoft.IdentityModel.Protocols</NuGetReference>
<NuGetReference Version="6.35.0">Microsoft.IdentityModel.Protocols.OpenIdConnect</NuGetReference>
<NuGetReference Version="6.35.0">Microsoft.IdentityModel.Tokens</NuGetReference>
<NuGetReference Version="1.0.0">Microsoft.SqlServer.Server</NuGetReference>
<NuGetReference Version="160.1000.6">Microsoft.SqlServer.Types</NuGetReference>
<NuGetReference Version="2.5.0">NetTopologySuite</NuGetReference>
<NuGetReference Version="2.1.0">NetTopologySuite.IO.SqlServerBytes</NuGetReference>
<NuGetReference Version="1.0.0">System.ClientModel</NuGetReference>
<NuGetReference Version="6.35.0">System.IdentityModel.Tokens.Jwt</NuGetReference>
<NuGetReference Version="8.0.0">System.IO.Hashing</NuGetReference>
<NuGetReference Version="1.0.2">System.Memory.Data</NuGetReference>
<NuGetReference Version="6.0.0">System.Security.Cryptography.ProtectedData</NuGetReference>
<NuGetReference Version="8.0.0">Microsoft.Extensions.Caching.Abstractions</NuGetReference>
<NuGetReference Version="8.0.0">Microsoft.Extensions.Caching.Memory</NuGetReference>
<NuGetReference Version="8.0.0">Microsoft.Extensions.DependencyInjection.Abstractions</NuGetReference>
<NuGetReference Version="8.0.0">Microsoft.Extensions.DependencyInjection</NuGetReference>
<NuGetReference Version="8.0.0">Microsoft.Extensions.Logging.Abstractions</NuGetReference>
<NuGetReference Version="8.0.0">Microsoft.Extensions.Logging</NuGetReference>
<NuGetReference Version="8.0.0">Microsoft.Extensions.Options</NuGetReference>
<NuGetReference Version="8.0.0">Microsoft.Extensions.Primitives</NuGetReference>
<NuGetReference Prerelease="true" Version="1.0.0-beta08">StronglyTypedIds.Attributes</NuGetReference>
<Namespace>System.Reflection</Namespace>
<Namespace>Microsoft.Data.SqlClient</Namespace>
<Namespace>Microsoft.EntityFrameworkCore</Namespace>
<Namespace>Microsoft.EntityFrameworkCore.Metadata</Namespace>
<Namespace>Microsoft.Extensions.Caching.Memory</Namespace>
<Namespace>Microsoft.Extensions.Options</Namespace>
<Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
<Namespace>Mint.Configuration</Namespace>
<Namespace>System.Data</Namespace>
<Namespace>Microsoft.EntityFrameworkCore.ChangeTracking</Namespace>
<Namespace>Microsoft.EntityFrameworkCore.Storage.ValueConversion</Namespace>
<Namespace>System.Text.Json</Namespace>
<Namespace>Microsoft.EntityFrameworkCore.Metadata.Builders</Namespace>
<Namespace>Microsoft.EntityFrameworkCore.Infrastructure</Namespace>
<Namespace>Mint.Data.Persistence.Converters</Namespace>
<Namespace>Mint.Data.Persistence</Namespace>
<Namespace>System</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>System.IO</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Net.Http</Namespace>
<Namespace>System.Threading</Namespace>
<Namespace>System.Threading.Tasks</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>Azure.Identity</Namespace>
<Namespace>Microsoft.Extensions.Configuration</Namespace>
<Namespace>Microsoft.Extensions.Configuration.AzureAppConfiguration</Namespace>
<Namespace>Microsoft.AspNetCore.Builder</Namespace>
<Namespace>System.Diagnostics.CodeAnalysis</Namespace>
<Namespace>StronglyTypedIds</Namespace>
<Namespace>System.Text.Json.Serialization</Namespace>
<Namespace>NetTopologySuite.Geometries</Namespace>
<Namespace>Mint</Namespace>
<Namespace>System.Runtime.InteropServices</Namespace>
</Query>

void Main()
{

}

#region Mint.Data.Azure.SqlServer
namespace Mint.Data.Azure.SqlServer
{
	#region \
	
	public partial class MintDbContext(DbContextOptions<MintDbContext> options, IMemoryCache memoryCache) : DbContext(options)
	{
	    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
	    {
	        base.ConfigureConventions(builder);
	
	        // TODO: use reflection to do this...
	        builder.Properties<AccessibilityId>().HaveConversion<AccessibilityId.EfCoreValueConverter>();
	        builder.Properties<AddressId>().HaveConversion<AddressId.EfCoreValueConverter>();
	        builder.Properties<AvailabilityId>().HaveConversion<AvailabilityId.EfCoreValueConverter>();
	        builder.Properties<CommunicationId>().HaveConversion<CommunicationId.EfCoreValueConverter>();
	        builder.Properties<DocumentId>().HaveConversion<DocumentId.EfCoreValueConverter>();
	        builder.Properties<LocationId>().HaveConversion<LocationId.EfCoreValueConverter>();
	        builder.Properties<OrganizationId>().HaveConversion<OrganizationId.EfCoreValueConverter>();
	        builder.Properties<PatientId>().HaveConversion<PatientId.EfCoreValueConverter>();
	        builder.Properties<PatientFhirId>().HaveConversion<PatientFhirId.EfCoreValueConverter>();
	        builder.Properties<PatientRegionId>().HaveConversion<PatientRegionId.EfCoreValueConverter>();
	        builder.Properties<ProgramId>().HaveConversion<ProgramId.EfCoreValueConverter>();
	        builder.Properties<ProviderId>().HaveConversion<ProviderId.EfCoreValueConverter>();
	        builder.Properties<QualificationId>().HaveConversion<QualificationId.EfCoreValueConverter>();
	        builder.Properties<ProviderRoleId>().HaveConversion<ProviderRoleId.EfCoreValueConverter>();
	        builder.Properties<ServiceAtId>().HaveConversion<ServiceAtId.EfCoreValueConverter>();
	        builder.Properties<ServiceId>().HaveConversion<ServiceId.EfCoreValueConverter>();
	        builder.Properties<ValueSetId>().HaveConversion<ValueSetId.EfCoreValueConverter>();
	        builder.Properties<ValueSetItemId>().HaveConversion<ValueSetItemId.EfCoreValueConverter>();
	        builder.Properties<LookupId>().HaveConversion<LookupId.EfCoreValueConverter>();
	        builder.Properties<UserId>().HaveConversion<UserId.EfCoreValueConverter>();
	        builder.Properties<GroupId>().HaveConversion<GroupId.EfCoreValueConverter>();
	        builder.Properties<RoleId>().HaveConversion<RoleId.EfCoreValueConverter>();
	        builder.Properties<PermissionId>().HaveConversion<PermissionId.EfCoreValueConverter>();
	    }
	
	    protected override void OnModelCreating(ModelBuilder builder)
	    {
	        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	
	        // Map UserFunction
	        builder.HasDbFunction(() => ConvertToId(default, default))
	            .HasName("udf_ResolveCode")
	            .HasSchema("dbo");
	    }
	
	    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	    {
	        try
	        {
	            return await base.SaveChangesAsync(cancellationToken);
	        }
	        // 2627 - Violation of PRIMARY KEY constraint
	        catch (DbUpdateException exception) when (exception.InnerException is SqlException sql && sql.Number == 2627)
	        {
	            throw new DataException("The record already exists. Insertion is disallowed due to primary key violation.", 
	                MintErrorCode.DbIntegrityViolation);
	        }
	        catch (Exception exception)
	        {
	            throw new DataException("An unhandled error occurred while saving changes to the data layer.", 
	                MintErrorCode.DbUnhandledError, 
	                exception);
	        }
	    }
	    public override int SaveChanges()
	    {
	        try
	        {
	            return base.SaveChanges();
	        }
	        // 2627 - Violation of PRIMARY KEY constraint
	        catch (DbUpdateException exception) when (exception.InnerException is SqlException sql && sql.Number == 2627)
	        {
	            throw new DataException("The record already exists. Insertion is disallowed due to primary key violation.",
	                MintErrorCode.DbIntegrityViolation);
	        }
	        catch (Exception exception)
	        {
	            throw new DataException("An unhandled error occurred while saving changes to the data layer.",
	                MintErrorCode.DbUnhandledError,
	                exception);
	        }
	    }
	
	
	
	    [DbFunction("udf_ResolveCode", "dbo")]
	    public static int? ConvertToId(string categoryKey, string value)
	    {
	
	
	        // This method body is not actually executed; it's just a placeholder for EF Core to recognize the method signature.
	        return default; // throw new NotSupportedException();
	    }
	}
	
	
	
	public static class AuditableEntityExtension
	{
	    // TODO add logic to automatically set the Auditable fields...
	}
	
	
	
	public record ValueSetTypes(string Value)
	{
	    public static ValueSetTypes AdministrativeGender = new("AdministrativeGender");
	    public static ValueSetTypes OrganizationAffiliationRole = new("OrganizationAffiliationRole");
	    public static ValueSetTypes OrganizationType = new("OrganizationType");
	    public static ValueSetTypes AddressType = new("AddressType");
	    public static ValueSetTypes AddressUse = new("AddressUse");
	    public static ValueSetTypes ContactPointUse = new("ContactPointUse");
	    public static ValueSetTypes ContactPointSystem = new("ContactPointSystem");
	    public static ValueSetTypes LocationType = new("LocationType");
	    public static ValueSetTypes LocationMode = new("LocationMode");
	    public static ValueSetTypes LocationStatus = new("LocationStatus");
	    public static ValueSetTypes IdentifierType = new("IdentifierType");
	    public static ValueSetTypes IdentifierUse = new("IdentifierUse");
	    public static ValueSetTypes MaritalStatus = new("MaritalStatus");
	    public static ValueSetTypes PractitionerRole = new("PractitionerRole");
	    public static ValueSetTypes PractitionerSpecialty = new("PractitionerSpecialty");
	    public static ValueSetTypes ServiceCategory = new("ServiceCategory");
	    public static ValueSetTypes ServiceType = new("ServiceType");
	
	
	    public static implicit operator string(ValueSetTypes type) => type.Value;
	
	    public override string ToString() => Value.ToString();
	}
	
	public partial class MintDbContext : DbContext
	{
	    public virtual DbSet<Accessibility> Accessibilities { get; set; }
	    public virtual DbSet<Availability> Availabilities { get; set; }
	    public virtual DbSet<ValueSetItem> ValueSetItems { get; set; }
	
	
	    public virtual DbSet<Location> Locations { get; set; }
	    public virtual DbSet<Lookup> Lookups { get; set; }
	    public virtual DbSet<Organization> Organizations { get; set; }
	    public virtual DbSet<Program> Programs { get; set; }
	
	    // Providers
	    public virtual DbSet<Provider> Providers { get; set; }
	    public virtual DbSet<ProviderCommunication> ProviderCommunications { get; set; }
	    public virtual DbSet<ProviderAssignment> ProviderAssignments { get; set; }
	    public virtual DbSet<ProviderQualification> ProviderQualifications { get; set; }
	    public virtual DbSet<ProviderRole> ProvidersRoles { get; set; }
	    public virtual DbSet<ProviderExtension> ProviderExtensions { get; set; }
	
	    public virtual DbSet<RequiredDocument> RequiredDocuments { get; set; }
	    public virtual DbSet<Service> Services { get; set; }
	    public virtual DbSet<ServiceLanguage> ServiceLanguages { get; set; }
	    public virtual DbSet<Language> Languages { get; set; }
	
	    // Patients
	    public virtual DbSet<Patient> Patients { get; set; }
	    public virtual DbSet<PatientAddress> PatientAddresses { get; set; }
	    public virtual DbSet<PatientCommunication> PatientCommunications { get; set; }
	    public virtual DbSet<PatientFhirResource> PatientResources { get; set; }
	    public virtual DbSet<PatientIdentifier> PatientIdentifiers { get; set; }
	    public virtual DbSet<PatientInstance> PatientInstances { get; set; }
	    public virtual DbSet<PatientDocument> PatientDocuments { get; set; }
	
	
	    public virtual DbSet<User> Users { get; set; }
	    public virtual DbSet<Role> Roles { get; set; }
	    public virtual DbSet<Group> Groups { get; set; }
	    public virtual DbSet<Permission> Permissions { get; set; }
	
	
	
	    public const string OrganizationsCacheKey = nameof(OrganizationsCacheKey);
	
	
	
	    // Temporary solution will use SecondaryCache soon...
	    public IEnumerable<Organization> GetOrganizations()
	    {
	
	        IEnumerable<Organization> organizations;
	        if (memoryCache.TryGetValue<IEnumerable<Organization>>(OrganizationsCacheKey, out var source))
	        {
	            organizations = source;
	        }
	        else
	        {
	            var options = new MemoryCacheEntryOptions()
	                .SetSlidingExpiration(TimeSpan.FromMinutes(5))
	                .SetAbsoluteExpiration(TimeSpan.FromHours(1))
	                .SetPriority(CacheItemPriority.Normal);
	
	            organizations = Organizations.ToList();
	
	            memoryCache.Set(OrganizationsCacheKey, organizations);
	        }
	
	        return organizations!;
	
	    }
	
	    public void ClearOrganizationsCache()
	    {
	        memoryCache.Remove(OrganizationsCacheKey);
	    }
	
	    public Organization? GetOrganization(OrganizationId organizationId)     
	        => GetOrganizations().FirstOrDefault(o => o.Id == organizationId);
	    
	
	}
	#endregion
	#region \Extensions
	
	public static  class DatabaseProviderExtensions
	{
	    public static DatabaseProviderOptions UseSqlServer(this DatabaseProviderOptions builder)
	    {
	        builder.Services.AddMemoryCache();
	        builder.Services.AddPooledDbContextFactory<MintDbContext>((serviceProvider, options) =>
	        {
	            var settings = serviceProvider.GetRequiredService<IOptions<SqlServerOptions>>().Value;
	
	            options.UseSqlServer(settings.ConnectionString, contextOptions =>
	            {
	                contextOptions.UseNetTopologySuite();
	                contextOptions.UseHierarchyId();
	            });
	#if DEBUG
	            options.EnableDetailedErrors();
	            options.EnableSensitiveDataLogging();
	#endif
	        });
	        builder.Services.AddTransient(serviceProvider =>
	        {
	            return serviceProvider
	                .GetRequiredService<IDbContextFactory<MintDbContext>>()
	                .CreateDbContext();
	        });
	
	        return builder;
	    }
	}
	
	#endregion
	#region \Internal\Converters
	
	public class ListOfIdsConverter : ValueConverter<List<Guid>, string>
	{
	    public ListOfIdsConverter(ConverterMappingHints? mappingHints = null)
	        : base(
	            v => string.Join(',', v),
	            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList(),
	            mappingHints)
	    {
	    }
	}
	
	
	public class ListOfIdsComparer : ValueComparer<List<Guid>>
	{
	    public ListOfIdsComparer() : base(
	      (t1, t2) => t1!.SequenceEqual(t2!),
	      t => t.Select(x => x!.GetHashCode()).Aggregate((x, y) => x ^ y),
	      t => t)
	    {
	    }
	}
	
	// NOTE: SQLite doesn't support JSON columns yet. Otherwise, we'd prefer calling .ToJson() on the owned entity instead.
	public class ValueJsonConverter<T> : ValueConverter<T, string>
	{
	    public ValueJsonConverter(ConverterMappingHints? mappingHints = null)
	        : base(
	            v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
	            v => JsonSerializer.Deserialize<T>(v, JsonSerializerOptions.Default)!,
	            mappingHints)
	    {
	    }
	}
	
	
	// NOTE: SQLite doesn't support JSON columns yet. Otherwise, we'd prefer calling .ToJson() on the owned entity instead.
	public class ValueJsonComparer<T> : ValueComparer<T>
	{
	    public ValueJsonComparer() : base(
	      (l, r) => JsonSerializer.Serialize(l, JsonSerializerOptions.Default) == JsonSerializer.Serialize(r, JsonSerializerOptions.Default),
	      v => v == null ? 0 : JsonSerializer.Serialize(v, JsonSerializerOptions.Default).GetHashCode(),
	      v => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(v, JsonSerializerOptions.Default), JsonSerializerOptions.Default)!)
	    {
	    }
	}
	#endregion
	#region \Internal\EntityTypeConfigurations
	
	internal class AccessibilityEntityTypeConfig : EntityTypeConfigBase<Accessibility>
	{
	    public override void Configure(EntityTypeBuilder<Accessibility> builder)
	    {
	        builder.ToTable("Accessibilities", MintSchema);
	        builder.HasKey(e => e.Id);
	
	        builder.Property(e => e.Id)
	            .HasColumnName("AccessibilityId")
	            .UseIdentityColumn(1, 1)
	            .ValueGeneratedOnAdd();
	
	        builder.Property(e => e.LocationId)
	            .HasColumnName("LocationId");
	
	        builder.Property(e => e!.Description).HasColumnName("Description").HasColumnType("text");
	        builder.Property(e => e!.Details).HasColumnName("Details").HasColumnType("text");
	        builder.Property(e => e!.Url).HasColumnName("Url").HasMaxLength(1000);
	
	        builder.HasOne(d => d.Location)
	            .WithMany(p => p.Accessibilities)
	            .HasForeignKey(d => d.LocationId);
	    }
	}
	
	
	
	internal class AvailabilityEntityTypeConfig : EntityTypeConfigBase<Availability>
	{
	    public override void Configure(EntityTypeBuilder<Availability> builder)
	    {
	        builder.ToTable("Availabilities", MintSchema);
	        builder.HasKey(e => e.Id);
	
	        builder.Property(e => e.Id)
	            .HasColumnName("AvailabilityId")
	            .HasConversion<int>(p => p!.Value, p => p)
	            .UseIdentityColumn(1, 1)
	            .ValueGeneratedOnAdd();
	
	        builder.Property(e => e!.CreatedBy);
	        builder.Property(e => e!.Created);
	        builder.Property(e => e!.ModifiedBy);
	        builder.Property(e => e!.Modified);
	
	
	        //info.IsRequired();
	        builder.Property(e => e!.DayOfWeek).HasColumnName("DayOfWeek");
	        builder.Property(e => e!.IsAllDay).HasColumnName("IsAllDay");
	        builder.Property(e => e!.IsClosed).HasColumnName("IsClosed");
	        builder.Property(e => e!.StartTime).HasColumnName("StartTime").HasColumnType("TIME");
	        builder.Property(e => e!.EndTime).HasColumnName("EndTime").HasColumnType("TIME");
	
	
	        builder.HasMany(d => d.Locations)
	            .WithMany(p => p.Availabilities)
	            .UsingEntity<Dictionary<string, object>>("LocationsAvailabilities",
	                right =>
	                {
	                    return right.HasOne<Location>()
	                        .WithMany()
	                        .HasForeignKey("LocationId")
	                        .OnDelete(DeleteBehavior.ClientSetNull);
	                },
	                left =>
	                {
	                    return left.HasOne<Availability>()
	                        .WithMany()
	                        .HasForeignKey("AvailabilityId")
	                        .OnDelete(DeleteBehavior.ClientSetNull);
	                },
	                join =>
	                {
	                    join.HasKey("AvailabilityId", "LocationId");
	                    join.ToTable("LocationAvailability");
	                });
	
	        builder.HasMany(d => d.ProvidersRoles)
	            .WithMany(p => p.Availabilities)
	            .UsingEntity<Dictionary<string, object>>("ProvidersRolesAvailabilities",
	                right =>
	                {
	                    return right.HasOne<ProviderRole>()
	                        .WithMany()
	                        .HasForeignKey("ProviderRoleId")
	                        .OnDelete(DeleteBehavior.ClientSetNull);
	                },
	                left =>
	                {
	                    return left.HasOne<Availability>()
	                        .WithMany()
	                        .HasForeignKey("AvailabilityId")
	                        .OnDelete(DeleteBehavior.ClientSetNull);
	                },
	                join =>
	                {
	                    join.HasKey("AvailabilityId", "ProviderRoleId");
	                    join.ToTable("ProviderRoleAvailability");
	                });
	    }
	}
	
	
	internal abstract class EntityTypeConfigBase<T> : IEntityTypeConfiguration<T> where T : class
	{
	    internal const string MintSchema = "mint";
	    internal const string AdminSchema = "admin";
	
	
	    public abstract void Configure(EntityTypeBuilder<T> builder);
	}
	
	
	internal class GroupEntityTypeConfig : EntityTypeConfigBase<Group>
	{
	    public override void Configure(EntityTypeBuilder<Group> builder)
	    {
	        builder.ToTable("Groups", AdminSchema);
	        builder.HasKey(p => p.Id);
	
	        builder.Property(p => p.Id).HasColumnName("GroupId");
	        builder.OwnsOne(p => p.Info, info =>
	        {
	            info.Property(p => p!.Name).HasColumnName("Name");
	            info.Property(p => p!.Description).HasColumnName("Description");
	        });
	        builder.Property(p => p.Created);
	        builder.Property(p => p.CreatedBy);
	        builder.Property(p => p.Modified);
	        builder.Property(p => p.ModifiedBy);
	            
	        builder.HasMany(p => p.Owners)
	            .WithMany(p => p.OwnedGroups)
	            .UsingEntity<Dictionary<string, object>>(
	                "GroupsOwners",
	                right =>
	                {
	                    return right.HasOne<User>()
	                        .WithMany()
	                        .HasForeignKey("UserId")
	                        .OnDelete(DeleteBehavior.ClientSetNull);
	                },
	                left =>
	                {
	                    return left.HasOne<Group>()
	                        .WithMany()
	                        .HasForeignKey("GroupId")
	                        .OnDelete(DeleteBehavior.ClientSetNull);
	                },
	                join =>
	                {
	                    join.Property<UserId>("UserId");
	                    join.Property<GroupId>("GroupId");
	                    join.HasKey("UserId", "GroupId");
	                    join.ToTable("GroupsOwners", AdminSchema);
	                });
	
	        builder.HasMany(p => p.Roles)
	            .WithMany(p => p.Groups)
	            .UsingEntity<Dictionary<string, object>>(
	                "GroupsRolesAssignments",
	                right =>
	                {
	                    return right.HasOne<Role>()
	                        .WithMany()
	                        .HasForeignKey("RoleId")
	                        .OnDelete(DeleteBehavior.ClientSetNull);
	                },
	                left =>
	                {
	                    return left.HasOne<Group>()
	                        .WithMany()
	                        .HasForeignKey("GroupId")
	                        .OnDelete(DeleteBehavior.ClientSetNull);
	                },
	                join =>
	                {
	                    join.Property<RoleId>("RoleId");
	                    join.Property<GroupId>("GroupId");
	                    join.HasKey("GroupId", "RoleId");
	                    join.ToTable("GroupsRolesAssignments", AdminSchema);
	                });
	    }
	}
	
	
	
	
	internal class LanguageEntityTypeConfig : EntityTypeConfigBase<Language>
	{
	    public override void Configure(EntityTypeBuilder<Language> builder)
	    {
	        builder.ToTable("Languages", MintSchema);
	
	        builder.HasKey(e => e.Id);
	
	        builder.Property(e => e.Id)
	            .HasColumnName("LanguageId")
	            .UseIdentityColumn(1, 1)
	            .ValueGeneratedOnAdd();
	
	        builder.Property(e => e.LanguageCode).HasMaxLength(4000);
	        builder.Property(e => e.LanguageDisplay).HasMaxLength(4000);
	    }
	}
	
	
	
	
	
	internal class LocationEntityTypeConfig : EntityTypeConfigBase<Location>
	{
	    public override void Configure(EntityTypeBuilder<Location> builder)
	    {
	        builder.ToTable("Locations", MintSchema);
	        builder.HasKey(e => e.Id);
	
	        builder.Ignore("ImportKey");
	
	        builder.Property(e => e.Id)
	            .HasColumnName("LocationId")
	            .UseIdentityColumn(1, 1)
	            .ValueGeneratedOnAdd();
	
	
	        builder.Property(e => e!.LocationModeId).HasColumnName("LocationMode");
	        builder.Property(e => e!.LocationStatusId).HasColumnName("LocationStatus");
	        builder.Property(e => e!.LocationTypeId).HasColumnName("LocationType");
	
	        builder.OwnsOne(p => p.Address, address =>
	        {
	            address.Property(e => e!.AddressOne).HasColumnName("AddressOne").HasMaxLength(255);
	            address.Property(e => e!.AddressTwo).HasColumnName("AddressTwo").HasMaxLength(255);
	            address.Property(e => e!.AddressTypeId).HasColumnName("AddressType");
	            address.Property(e => e!.AddressUseId).HasColumnName("AddressUse");
	            //address.Property(e => e!.Coordinate).HasColumnName("Coordinate");
	            address.Property(e => e!.City).HasColumnName("City").HasMaxLength(255);
	            address.Property(e => e!.Country).HasColumnName("Country").HasMaxLength(255);
	            address.Property(e => e!.County).HasColumnName("County").HasMaxLength(255);
	            address.Property(e => e!.PostalCode).HasColumnName("PostalCode").HasMaxLength(255);
	            address.Property(e => e!.Region).HasColumnName("Region").HasMaxLength(255);
	            address.Property(e => e!.StateProvince).HasColumnName("StateProvince").HasMaxLength(255);
	
	            address.HasOne(d => d.AddressType).WithMany()
	                .HasForeignKey(d => d.AddressTypeId);
	
	            address.HasOne(d => d.AddressUse).WithMany()
	                .HasForeignKey(d => d.AddressUseId);
	        });
	
	        builder.HasOne(d => d.LocationMode).WithMany()
	            .HasForeignKey(d => d.LocationModeId)
	            .HasConstraintName("FK__Locations__LocationMode_Id");
	
	        builder.HasOne(d => d.LocationStatus).WithMany()
	            .HasForeignKey(d => d.LocationStatusId)
	            .HasConstraintName("FK__Locations__LocationStatus_Id");
	
	        //builder.HasOne(d => d.LocationType).WithMany()
	        //    .HasForeignKey(d => d.LocationTypeId)
	        //    .HasConstraintName("FK__Locations__LocationType_Id");
	
	        builder.HasOne(d => d.Organization).WithMany(p => p.Locations)
	            .HasForeignKey(d => d.OrganizationId)
	            .OnDelete(DeleteBehavior.ClientSetNull)
	            .HasConstraintName("FK__Locations__OrganizationId_OrganizationId");
	    }
	}
	
	internal class LookupEntityTypeConfig : EntityTypeConfigBase<Lookup>
	{
	    public override void Configure(EntityTypeBuilder<Lookup> builder)
	    {
	        builder.ToTable("Lookups", MintSchema);
	        builder.HasKey(e => e.Id);
	
	        builder.Property(e => e.Id).HasColumnName("LookupId").ValueGeneratedNever();
	        builder.Property(e => e.Description).HasMaxLength(500);
	        builder.Property(e => e.Label).HasMaxLength(50);
	        builder.Property(e => e.Lineage).HasMaxLength(4000);
	
	        builder.HasOne(d => d.Parent)
	            .WithMany(p => p.Children)
	            .HasForeignKey(d => d.ParentId);
	    }
	}
	
	
	
	
	internal class OrganizationEntityTypeConfig : EntityTypeConfigBase<Organization>
	{
	    public override void Configure(EntityTypeBuilder<Organization> builder)
	    {
	        builder.ToTable("Organizations", MintSchema);
	        builder.HasKey(e => e.Id);
	
	        builder.Property(e => e.Id)
	            .HasColumnName("OrganizationId")
	            .UseIdentityColumn(100000, 1)
	            .ValueGeneratedOnAdd();
	
	        builder.Ignore("ImportKey");
	
	        builder.Property(e => e.Name).HasMaxLength(255);
	        builder.Property(e => e.Description).HasColumnType("text");
	        builder.Property(e => e.LegalStatus).HasMaxLength(255);
	        builder.Property(e => e.Logo).HasMaxLength(255);
	        builder.Property(e => e.Uri).HasMaxLength(255);
	        builder.Property(e => e.Oid).HasMaxLength(500);
	        builder.Property(e => e.Phone).HasMaxLength(20);
	        builder.Property(e => e.AddressTypeId).HasColumnName("AddressType");
	        builder.Property(e => e.AddressUseId).HasColumnName("AddressUse");
	
	
	        builder.OwnsOne(p => p.Address, address =>
	        {
	            address.Property(e => e.AddressOne).HasColumnName("AddressOne").HasMaxLength(255);
	            address.Property(e => e.AddressTwo).HasColumnName("AddressTwo").HasMaxLength(255);
	            address.Property(e => e.AddressTypeId).HasColumnName("AddressType");
	            address.Property(e => e.AddressUseId).HasColumnName("AddressUse");
	            //address.Property(e => e.Coordinate).HasColumnName("Coordinate");
	            address.Property(e => e.City).HasColumnName("City").HasMaxLength(255);
	            address.Property(e => e.Country).HasColumnName("Country").HasMaxLength(255);
	            address.Property(e => e.County).HasColumnName("County").HasMaxLength(255);
	            address.Property(e => e.PostalCode).HasColumnName("PostalCode").HasMaxLength(255);
	            address.Property(e => e.Region).HasColumnName("Region").HasMaxLength(255);
	            address.Property(e => e.StateProvince).HasColumnName("StateProvince").HasMaxLength(255);
	
	            address.HasOne(d => d.AddressType)
	                .WithMany()
	                .HasForeignKey(d => d.AddressTypeId);
	
	            address.HasOne(d => d.AddressUse)
	                .WithMany()
	                .HasForeignKey(d => d.AddressUseId);
	        });
	
	        builder.HasMany(d => d.PatientInstances)
	           .WithOne(p => p.Organization)
	           .HasForeignKey(p => p.OrganizationId);
	
	        builder.HasOne(d => d.Parent)
	            .WithMany()
	            .HasForeignKey(d => d.ParentId);
	
	        builder.HasMany(p => p.ProvidersAssignments)
	            .WithOne(p => p.Organization)
	            .HasForeignKey(p => p.OrganizationId);
	    }
	}
	
	
	
	
	internal class PatientEntityTypeConfig : EntityTypeConfigBase<Patient>
	{
	    public override void Configure(EntityTypeBuilder<Patient> builder)
	    {
	        builder.ToTable("Patients", MintSchema);
	        builder.HasKey(e => e.Id);
	
	        builder.Property(e => e.Id)
	          .HasColumnName("PatientId")
	          .UseIdentityColumn(1, 1)
	          .ValueGeneratedOnAdd();
	
	        builder.Property(e => e.Id).ValueGeneratedOnAdd();
	        builder.Property(e => e.AccountNumber)
	            .HasMaxLength(10)
	            .IsFixedLength();
	        builder.Property(e => e.FirstName).HasMaxLength(50);
	        builder.Property(e => e.LastName).HasMaxLength(50);
	        builder.Property(e => e.MiddleName).HasMaxLength(50);        
	        builder.Property(e => e.Occupation).HasMaxLength(100);
	        builder.Property(e => e.TribalAffiliation).HasMaxLength(100);
	
	        builder.HasMany(d => d.Instances)
	           .WithOne(p => p.Patient)
	           .HasForeignKey(p => p.PatientId);
	
	    }
	}
	
	
	
	
	
	internal class PatientsAddressEntityTypeConfig : EntityTypeConfigBase<PatientAddress>
	{
	    public override void Configure(EntityTypeBuilder<PatientAddress> entity)
	    {
	        entity.ToTable("PatientsAddresses", MintSchema);
	        entity.HasKey(e => e.Id);
	
	        entity.Property(e => e.PatientId);
	        entity.Property(e => e.Id)
	            .HasColumnName("AddressId")
	            .UseIdentityColumn(100000, 1)
	            .ValueGeneratedOnAdd();
	
	        entity.Property(e => e.AddressOne).HasColumnName("AddressOne").HasMaxLength(255);
	        entity.Property(e => e.AddressTwo).HasColumnName("AddressTwo").HasMaxLength(255);
	        entity.Property(e => e.AddressTypeId).HasColumnName("AddressType");
	        entity.Property(e => e.AddressUseId).HasColumnName("AddressUse");
	        //entity.Property(e => e.Coordinate).HasColumnName("Coordinate");
	        entity.Property(e => e.City).HasColumnName("City").HasMaxLength(255);
	        entity.Property(e => e.Country).HasColumnName("Country").HasMaxLength(255);
	        entity.Property(e => e.County).HasColumnName("County").HasMaxLength(255);
	        entity.Property(e => e.PostalCode).HasColumnName("PostalCode").HasMaxLength(255);
	        entity.Property(e => e.Region).HasColumnName("Region").HasMaxLength(255);
	        entity.Property(e => e.StateProvince).HasColumnName("StateProvince").HasMaxLength(255);
	
	
	        entity.HasOne(d => d.Patient).WithMany(p => p.Addresses)
	            .HasForeignKey(d => d.PatientId)
	            .OnDelete(DeleteBehavior.Cascade);
	    }
	}
	
	
	
	
	internal class PatientCommunicationEntityTypeConfig : EntityTypeConfigBase<PatientCommunication>
	{
	    public override void Configure(EntityTypeBuilder<PatientCommunication> builder)
	    {
	
	        builder.ToTable("PatientsCommunications", MintSchema);
	        builder.HasKey(e => e.Id);
	
	        builder.Property(e => e.Id)
	            .HasColumnName("CommunicationId")
	            .UseIdentityColumn(100000, 1)
	            .ValueGeneratedOnAdd();
	
	        builder.Property(e => e.Rank).HasMaxLength(10).IsFixedLength();
	        builder.Property(p => p.PatientId);
	        builder.Property(p => p.UseId).HasColumnName("Use");
	        builder.Property(p => p.SystemId).HasColumnName("System");
	        builder.Property(e => e.Value).HasMaxLength(50);
	
	        builder.HasOne(d => d.Patient).WithMany(p => p.Communications)
	            .HasForeignKey(d => d.PatientId)
	            .OnDelete(DeleteBehavior.Cascade); ;
	
	        builder.HasOne(d => d.System).WithMany()
	            .HasForeignKey(d => d.SystemId);
	
	        builder.HasOne(d => d.Use).WithMany()
	            .HasForeignKey(d => d.UseId);
	
	    }
	}
	
	
	internal class PatientsDocumentsEntityTypeConfig : EntityTypeConfigBase<PatientDocument>
	{
	    public override void Configure(EntityTypeBuilder<PatientDocument> builder)
	    {
	        builder.ToTable("PatientsDocuments", MintSchema);
	        builder.HasKey(e => e.Id);
	
	
	        builder.HasOne(d => d.Type).WithMany()
	            .HasForeignKey(d => d.TypeId);
	
	        builder.HasOne(d => d.Patient).WithMany(x => x.Documents)
	            .HasForeignKey(d => d.PatientId)
	            .OnDelete(DeleteBehavior.Cascade);
	
	        builder.HasOne(d => d.Organization).WithMany()
	            .HasForeignKey(d => d.OrganizationId);
	    }
	}
	
	
	
	
	internal class PatientsFhirResourcesbuilderTypeConfig : EntityTypeConfigBase<PatientFhirResource>
	{
	    public override void Configure(EntityTypeBuilder<PatientFhirResource> builder)
	    {
	        builder.ToTable("PatientsFhirResources", MintSchema);
	        builder.HasKey(e => e.Id);
	
	        builder.Ignore("ProvenceId");
	
	        builder.Property(e => e.Id)
	            .ValueGeneratedOnAdd()
	            .UseIdentityColumn();
	        
	        builder.Property(e => e.ResourceType)
	            .HasMaxLength(4000)
	            .HasComputedColumnSql("(json_value([Resource],'$.resourceType'))", true);
	
	        builder.Property(e => e.RowVersion)
	            .IsRowVersion()
	            .IsConcurrencyToken();
	
	        builder.Property(e => e.SourceId)
	            .HasMaxLength(4000)
	            .HasComputedColumnSql("(json_value([Resource],'$.id'))", true);
	
	        builder.HasOne(d => d.Patient).WithMany(p => p.Resources)
	            .HasForeignKey(d => d.PatientId)
	            .OnDelete(DeleteBehavior.Cascade);
	
	
	    }
	}
	
	
	
	internal class PatientIdentifierEntityTypeConfig : EntityTypeConfigBase<PatientIdentifier>
	{
	    public override void Configure(EntityTypeBuilder<PatientIdentifier> builder)
	    {
	        builder.ToTable("PatientsIdentifiers", MintSchema);
	
	        builder.HasKey(e => e.Id).HasName("PK__PatientsIdentifiers__Id");
	
	        builder.Property(e => e.Id)
	            .UseIdentityColumn(10000, 1)
	            .ValueGeneratedOnAdd();
	
	        builder.Property(p => p.PatientId).HasColumnName("PatientId");
	
	        builder.Property(e => e.UseId)
	            .HasColumnName("Use");
	
	        builder.Property(e => e.Value).HasMaxLength(50);
	
	        //builder.HasOne(d => d.System).WithMany()
	        //    .HasForeignKey(d => d.SystemId)
	        //    .HasConstraintName("FK__PatientsIdentifiers__System_Id");
	
	        builder.HasOne(d => d.Use).WithMany()
	            .HasForeignKey(d => d.UseId);
	
	        builder.HasOne(d => d.Patient).WithMany(x => x.Identifiers)
	            .HasForeignKey(d => d.PatientId)
	            .OnDelete(DeleteBehavior.Cascade); ;
	    }
	}
	
	
	internal class PatientsInstancesEntityTypeConfig : EntityTypeConfigBase<PatientInstance>
	{
	    public override void Configure(EntityTypeBuilder<PatientInstance> builder)
	    {
	        builder.ToTable("PatientsInstances", MintSchema);
	        builder.HasKey(e => new
	        {
	            e.PatientId,
	            e.OrganizationId
	        });
	
	        builder.Property(e => e.MRN);
	
	
	        builder.HasOne(d => d.Patient).WithMany(x => x.Instances)
	            .HasForeignKey(d => d.PatientId)
	            .OnDelete(DeleteBehavior.Cascade); ;
	
	        builder.HasOne(d => d.Organization).WithMany(x => x.PatientInstances)
	            .HasForeignKey(d => d.OrganizationId)
	            .OnDelete(DeleteBehavior.ClientSetNull);
	    }
	}
	
	
	internal class ProgramEntityTypeConfig : EntityTypeConfigBase<Program>
	{
	    public override void Configure(EntityTypeBuilder<Program> builder)
	    {
	        builder.ToTable("Programs", MintSchema);
	        builder.HasKey(e => e.Id);
	
	        builder.Property(e => e.Id)
	            .HasConversion<int>(p => p!.Value, p => p)
	            .HasColumnName("ProgramId");
	
	        //info.IsRequired();
	        builder.Property(e => e!.AlternateName).HasColumnName("AlternateName").HasMaxLength(255);
	        builder.Property(e => e!.Description).HasColumnName("Description").HasColumnType("text");
	        builder.Property(e => e!.Name).HasColumnName("Name").HasMaxLength(255);
	
	
	        builder.HasOne(d => d.Organization)
	            .WithMany(p => p.Programs)
	            .HasForeignKey(d => d.OrganizationId);
	    }
	}
	
	
	internal class ProviderEntityTypeConfig : EntityTypeConfigBase<Provider>
	{
	    public override void Configure(EntityTypeBuilder<Provider> builder)
	    {
	        builder.ToTable("Providers", MintSchema);
	        builder.HasKey(e => e.Id);
	
	        builder.Property(e => e.Id)
	            .HasColumnName("ProviderId")
	            .UseIdentityColumn(1, 1)
	            .ValueGeneratedOnAdd();
	
	        //info.IsRequired();
	        //info.Property(e => e!.Avatar).HasColumnName("Avatar").HasMaxLength(255);
	        builder.Property(e => e!.Bio).HasColumnName("Bio").HasColumnType("text");
	        builder.Property(e => e!.Credential).HasColumnName("Credential").HasMaxLength(255);
	        builder.Property(e => e!.FirstName).HasColumnName("FirstName").HasMaxLength(255);
	        builder.Property(e => e!.LastName).HasColumnName("LastName").HasMaxLength(255);
	        builder.Property(e => e!.MedicalSchoolName).HasColumnName("MedicalSchoolName").HasMaxLength(255);
	        builder.Property(e => e!.MiddleInitial).HasColumnName("MiddleInitial").HasMaxLength(255);
	        builder.Property(e => e!.Npi).HasColumnName("Npi").HasColumnName("NPI");
	        builder.Property(e => e!.Oid).HasColumnName("Oid").HasMaxLength(500);
	        builder.Property(e => e!.Suffix).HasColumnName("Suffix").HasMaxLength(255);
	        builder.Property(e => e.PracticeFocus).HasColumnName("PracticeFocus").HasMaxLength(1000);
	        builder.Property(e => e.ProfessionalAffiliation).HasColumnName("ProfessionalAffiliation").HasMaxLength(500);
	        builder.Property(e => e.Telehealth).HasColumnName("Telehealth").HasMaxLength(500);
	        builder.Property(e => e.IsActive).HasColumnName("IsActive");
	        builder.Property(e => e.Gender).HasColumnName("Gender");
	        builder.Property(e => e.GraduationYear).HasColumnName("GraduationYear");        
	        builder.Property(e => e.Avatar).HasColumnName("Avatar").HasMaxLength(255);
	        builder.Property(e => e.DateOfBirth).HasColumnName("DateOfBirth");
	        builder.Property(e => e.ProviderTypeId).HasColumnName("ProviderType");
	
	        builder.Property(e => e.Specialty)
	         .HasMaxLength(500)
	         .HasConversion(
	              v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!), // Converts string[] to JSON string
	              v => JsonSerializer.Deserialize<string[]>(v, (JsonSerializerOptions)null!) // Converts JSON string to string[]
	         );
	
	
	        builder.HasOne(d => d.ProviderType)
	            .WithMany(p => p.Providers)
	            .HasForeignKey(d => d.ProviderTypeId)
	            .OnDelete(DeleteBehavior.ClientSetNull);
	    }
	}
	
	
	internal class ProvidersAssignmentsEntityTypeConfig : EntityTypeConfigBase<ProviderAssignment>
	{
	    public override void Configure(EntityTypeBuilder<ProviderAssignment> builder)
	    {
	        builder.ToTable("ProvidersAssignments", MintSchema);
	
	        builder.HasKey(e => e.AssignmentId);
	
	        builder.HasOne(d => d.Location)
	            .WithMany(p => p.ProvidersAssignments)
	            .HasForeignKey(d => d.LocationId);
	
	        builder.HasOne(d => d.Organization)
	            .WithMany(p => p.ProvidersAssignments)
	            .HasForeignKey(d => d.OrganizationId);
	
	        builder.HasOne(d => d.Provider)
	            .WithMany(p => p.Assignments)
	            .HasForeignKey(d => d.ProviderId);
	    }
	}
	
	internal class ProviderCommunicationEntityTypeConfig : EntityTypeConfigBase<ProviderCommunication>
	{
	    public override void Configure(EntityTypeBuilder<ProviderCommunication> builder)
	    {
	        builder.ToTable("ProvidersCommunications", MintSchema);
	        builder.HasKey(e => e.Id);
	
	        builder.Property(e => e.Id).HasColumnName("CommunicationId").ValueGeneratedNever();
	        builder.Property(e => e.Rank).HasMaxLength(10).IsFixedLength();
	        builder.Property(e => e.Value).HasMaxLength(50);
	        builder.Property(e => e.UseId).HasColumnName("Use");
	        builder.Property(e => e.SystemId).HasColumnName("System");
	
	        builder.HasOne(d => d.Provider)
	            .WithMany(p => p.Communications)
	            .HasForeignKey(d => d.ProviderId);
	
	        //builder.HasOne(d => d.System).WithMany()
	        //    .HasForeignKey(d => d.System)
	        //    .HasConstraintName("FK__ProvidersCommunications__System_Id");
	
	        //builder.HasOne(d => d.Use).WithMany()
	        //    .HasForeignKey(d => d.Use)
	        //    .HasConstraintName("FK__ProvidersCommunications__Use_Id");
	    }
	}
	
	
	
	
	internal class ProviderExtensionEntityTypeConfig : EntityTypeConfigBase<ProviderExtension>
	{
	    public override void Configure(EntityTypeBuilder<ProviderExtension> entity)
	    {
	        entity.HasKey(e => e.Id);
	
	        entity.ToTable("ProvidersExtensions", MintSchema);
	
	        entity.Property(e => e.Id).ValueGeneratedOnAdd();
	        entity.Property(e => e.Value).HasColumnName("Text").HasMaxLength(4000);
	        entity.Property(e => e.System).HasMaxLength(255);
	        entity.Property(e => e.Code).HasMaxLength(255);
	
	        entity.HasOne(d => d.Lookup).WithMany()
	            .HasForeignKey(d => d.LookupId);
	
	        //entity.HasOne(d => d.Provider).WithMany()
	        //    .HasForeignKey(d => d.ProviderId)
	        //    .OnDelete(DeleteBehavior.Cascade);
	
	    }
	}
	
	
	
	
	
	internal class ProvidersLanguageEntityTypeConfig : EntityTypeConfigBase<ProviderLanguage>
	{
	    public override void Configure(EntityTypeBuilder<ProviderLanguage> builder)
	    {
	        builder.ToTable("ProvidersLanguages", MintSchema);
	        builder.HasKey(e => new 
	        { 
	            e.ProviderId, 
	            e.LanguageId 
	        });
	
	
	        builder.Property(e => e.ProviderId).ValueGeneratedOnAdd();
	        builder.Property(e => e.Proficiency).HasMaxLength(4000);
	        builder.Property(e => e.ProficiencyCode).HasMaxLength(4000);
	
	        builder.HasOne(d => d.Language).WithMany()
	            .HasForeignKey(d => d.LanguageId)
	            .OnDelete(DeleteBehavior.ClientSetNull);
	
	        builder.HasOne(d => d.Provider).WithMany(p => p.Languages)
	            .HasForeignKey(d => d.ProviderId)
	            .OnDelete(DeleteBehavior.Cascade);
	
	    }
	}
	
	
	internal class ProviderQualificationEntityTypeConfig : EntityTypeConfigBase<ProviderQualification>
	{
	    public override void Configure(EntityTypeBuilder<ProviderQualification> builder)
	    {
	        builder.ToTable("ProvidersQualifications", MintSchema);
	        builder.HasKey(e => e.Id);
	
	        builder.Property(e => e.Id)
	            .HasColumnName("QualificationId")
	            .HasConversion<int>(p => p!.Value, p => p)
	            .UseIdentityColumn(1, 1)
	            .ValueGeneratedOnAdd();
	
	        builder.Property(e => e.Code);
	        builder.Property(e => e.Value);
	        builder.Property(e => e.StartDate);
	        builder.Property(e => e.EndDate);
	        builder.Property(e => e.ProviderId)
	            .HasConversion<int>(p => p!.Value, p => p);
	
	        builder.HasOne(d => d.Provider)
	            .WithMany(p => p.Qualifications)
	            .HasForeignKey(d => d.ProviderId);
	    }
	}
	
	
	
	
	internal class ProviderRoleEntityTypeConfig : EntityTypeConfigBase<ProviderRole>
	{
	    public override void Configure(EntityTypeBuilder<ProviderRole> builder)
	    {
	        builder.ToTable("ProvidersRoles", MintSchema);
	        builder.HasKey(e => e.Id);
	
	        builder.Property(e => e.Id)
	            .HasColumnName("RoleId")
	            .HasConversion<int>(p => p!.Value, p => p);
	
	        builder.Property(e => e!.Role).HasColumnName("Role");
	        builder.Property(e => e!.Specialty).HasColumnName("Specialty");
	        builder.Property(e => e!.AvailabilityExceptionsMessage).HasColumnName("AvailabilityExceptionsMessage").HasColumnType("text");
	
	
	    }
	}
	
	
	
	
	internal class RequiredDocumentEntityTypeConfig : EntityTypeConfigBase<RequiredDocument>
	{
	    public override void Configure(EntityTypeBuilder<RequiredDocument> builder)
	    {
	        builder.ToTable("RequiredDocuments", MintSchema);
	        builder.HasKey(e => e.Id);
	
	        builder.Property(e => e.Id)
	            .HasColumnName("RequiredDocumentId")
	            .HasConversion<int>(p => p!.Value, p => p);
	        builder.Property(e => e.Document).HasMaxLength(255);
	        builder.Property(e => e.Uri).HasMaxLength(255);
	
	        builder.HasOne(d => d.Service)
	            .WithMany(p => p.RequiredDocuments)
	            .HasForeignKey(d => d.ServiceId);
	    }
	}
	
	
	internal class RoleEntityTypeConfig : EntityTypeConfigBase<Role>
	{
	    public override void Configure(EntityTypeBuilder<Role> builder)
	    {
	        builder.ToTable("Roles", AdminSchema);
	        builder.HasKey(e => e.Id);
	
	        builder.Property(p => p.Id).HasColumnName("RoleId");
	        builder.Property(p => p.Type).HasColumnName("RoleType");
	        builder.OwnsOne(p => p.Info, info =>
	        {
	            info.Property(p => p!.Name).HasColumnName("Name");
	            info.Property(p => p!.Value).HasColumnName("Value");
	            info.Property(p => p!.Description).HasColumnName("Description");
	        });
	
	        builder.Ignore(p => p.Permissions);
	
	        //builder.HasMany(p => p.Permissions)
	        //    .WithOne(p => p.Role)
	        //    .HasForeignKey(p => p.Id);
	    }
	}
	
	
	
	
	internal class ServiceEntityTypeConfig : EntityTypeConfigBase<Service>
	{
	    public override void Configure(EntityTypeBuilder<Service> builder)
	    {
	        builder.ToTable("Services", MintSchema);
	        builder.HasKey(e => e.Id);
	
	
	        builder.Property(e => e.Id)
	            .HasColumnName("ServiceId")
	            .HasConversion<int>(p => p!.Value, p => p);
	
	
	        builder.HasIndex(e => e!.Name).IsUnique();
	        builder.Property(e => e!.Accreditations).HasColumnName("Accreditations").HasColumnType("text");
	        builder.Property(e => e!.Alert).HasColumnName("Alert").HasMaxLength(255);
	        builder.Property(e => e!.AlternateName).HasColumnName("AlternateName").HasMaxLength(255);
	        builder.Property(e => e!.ApplicationProcess).HasColumnName("ApplicationProcess").HasColumnType("text");
	        builder.Property(e => e!.CoverageArea).HasColumnName("CoverageArea").HasMaxLength(255);
	        builder.Property(e => e!.Description).HasColumnName("Description").HasColumnType("text");
	        builder.Property(e => e!.EligibilityDescription).HasColumnName("EligibilityDescription").HasColumnType("text");
	        builder.Property(e => e!.Email).HasColumnName("Email").HasMaxLength(255);
	        builder.Property(e => e!.FeesDescription).HasColumnName("FeesDescription").HasColumnType("text");
	        builder.Property(e => e!.InterpretationServices).HasColumnName("InterpretationServices").HasMaxLength(255);
	        builder.Property(e => e!.Licenses).HasColumnName("Licenses").HasMaxLength(255);
	        builder.Property(e => e!.Name).HasColumnName("Name").HasMaxLength(255);
	        builder.Property(e => e!.Url).HasColumnName("Url").HasMaxLength(255);
	
	        builder.HasOne(d => d.Program)
	            .WithMany(p => p.Services)
	            .HasForeignKey(d => d.ProgramId);
	
	        builder.HasMany(d => d.Locations).WithMany(p => p.Services)
	            .UsingEntity<Dictionary<string, object>>(
	                "ServicesLocation",
	                r => r.HasOne<Location>().WithMany()
	                    .HasForeignKey("LocationId")
	                    .OnDelete(DeleteBehavior.ClientSetNull)
	                    .HasConstraintName("FK__ServicesLocations__LocationId_LocationId"),
	                l => l.HasOne<Service>().WithMany()
	                    .HasForeignKey("ServiceId")
	                    .OnDelete(DeleteBehavior.ClientSetNull)
	                    .HasConstraintName("FK__ServicesLocations__ServiceId_ServiceId"),
	                j =>
	                {
	                    j.HasKey("ServiceId", "LocationId").HasName("PK__ServicesLocations__ServiceId_LocationId");
	                    j.ToTable("ServicesLocations");
	                });
	
	        builder.HasMany(d => d.Providers).WithMany(p => p.Services)
	                .UsingEntity<Dictionary<string, object>>(
	                    "ProvidersService",
	                    r => r.HasOne<Provider>().WithMany()
	                        .HasForeignKey("ProviderId")
	                        .OnDelete(DeleteBehavior.ClientSetNull)
	                        .HasConstraintName("FK__ProvidersServices__ProviderId_ProviderId"),
	                    l => l.HasOne<Service>().WithMany()
	                        .HasForeignKey("ServiceId")
	                        .OnDelete(DeleteBehavior.ClientSetNull)
	                        .HasConstraintName("FK_ProvidersServices_Services"),
	                    j =>
	                    {
	                        j.HasKey("ServiceId", "ProviderId").HasName("PK__ProvidersServices__ServiceAtId_ProviderId");
	                        j.ToTable("ProvidersServices");
	                    });
	    }
	}
	
	
	
	
	internal class ServiceLanguageEntityTypeConfig : EntityTypeConfigBase<ServiceLanguage>
	{
	    public override void Configure(EntityTypeBuilder<ServiceLanguage> builder)
	    {
	        builder.ToTable("ServicesLanguages", MintSchema);
	        builder.HasKey(e => new
	        {
	            e.ServiceId,
	            e.LanguageId
	        });
	
	
	        builder.Property(e => e.ServiceId).HasConversion<int>(p => p!.Value, p => p);
	
	
	        builder.HasOne(d => d.Service)
	            .WithMany(p => p.ServiceLanguages)
	            .HasForeignKey(d => d.ServiceId)
	            .OnDelete(DeleteBehavior.ClientSetNull);
	    }
	}
	
	
	internal class UserEntityTypeConfig : EntityTypeConfigBase<User>
	{
	    public override void Configure(EntityTypeBuilder<User> builder)
	    {
	        builder.ToTable("Users", AdminSchema);
	        builder.HasKey(e => e.Id);
	
	        builder.Property(p => p.OrganizationId);
	        builder.Property(p => p.Id).HasColumnName("UserId");
	        builder.Property(p => p.Type).HasColumnName("UserType");
	        builder.OwnsOne(p => p.Info, info =>
	        {
	            info.Property(p => p!.FirstName).HasColumnName("FirstName");
	            info.Property(p => p!.LastName).HasColumnName("LastName");
	            info.Property(p => p!.MiddleName).HasColumnName("MiddleName");
	            info.Property(p => p!.Email).HasColumnName("Email");
	            info.Property(p => p!.IsEnabled).HasColumnName("IsEnabled");
	            info.Property(p => p!.IsLocked).HasColumnName("IsLocked");
	            info.Property(p => p!.LastSignIn).HasColumnName("LastSignIn");
	        });
	
	        builder.Property(p => p.Created);
	        builder.Property(p => p.CreatedBy);
	        builder.Property(p => p.Modified);
	        builder.Property(p => p.ModifiedBy);
	
	        builder.HasMany(p => p.Roles)
	            .WithMany(p => p.Users)
	            .UsingEntity<Dictionary<string, object>>(
	                "UsersRolesAssignments",
	                right =>
	                {
	                    return right.HasOne<Role>()
	                        .WithMany()
	                        .HasForeignKey("RoleId")
	                        .OnDelete(DeleteBehavior.ClientCascade);
	                },
	                left =>
	                {
	                    return left.HasOne<User>()
	                        .WithMany()
	                        .HasForeignKey("UserId")
	                        .OnDelete(DeleteBehavior.ClientCascade);
	                },
	                join =>
	                {
	                    join.Property<RoleId>("RoleId");
	                    join.Property<UserId>("UserId");
	                    join.HasKey("UserId", "RoleId");
	                    join.ToTable("UsersRolesAssignments", AdminSchema);
	
	                });
	
	        builder.HasMany(p => p.Groups)
	            .WithMany(p => p.Members)
	            .UsingEntity<Dictionary<string, object>>(
	                "GroupsAssignments",
	                right =>
	                {
	                    return right.HasOne<Group>()
	                        .WithMany()
	                        .HasForeignKey("GroupId")
	                        .OnDelete(DeleteBehavior.ClientCascade);
	                },
	                left =>
	                {
	                    return left.HasOne<User>()
	                        .WithMany()
	                        .HasForeignKey("UserId")
	                        .OnDelete(DeleteBehavior.ClientCascade);
	                },
	                join =>
	                {
	                    join.Property<GroupId>("GroupId");
	                    join.Property<UserId>("UserId");
	                    join.HasKey("GroupId", "UserId");
	                    join.ToTable("GroupsAssignments", AdminSchema);
	                });
	
	        builder.HasMany(d => d.Organizations)
	            .WithMany(p => p.Users)
	            .UsingEntity<Dictionary<string, object>>(
	                "OrganizationsAssignments",
	                right =>
	                {
	                    return right.HasOne<Organization>()
	                        .WithMany()
	                        .HasForeignKey("OrganizationId")
	                        .OnDelete(DeleteBehavior.ClientCascade);
	                },
	                left =>
	                {
	                    return left.HasOne<User>()
	                        .WithMany()
	                        .HasForeignKey("UserId")
	                        .OnDelete(DeleteBehavior.ClientCascade);
	                },
	                join =>
	                {
	                    join.Property<OrganizationId>("OrganizationId");
	                    join.Property<UserId>("UserId");
	                    join.HasKey("UserId", "OrganizationId");
	                    join.ToTable("OrganizationsAssignments", AdminSchema);
	                });
	    }
	}
	
	
	
	
	internal class CodeConceptEntityTypeConfig : EntityTypeConfigBase<ValueSetItem>
	{
	    public override void Configure(EntityTypeBuilder<ValueSetItem> builder)
	    {
	        builder.ToTable("ValueSetItems", MintSchema);
	        builder.HasKey(e => e.Id);
	
	        builder.Property(e => e.Id)
	            .UseIdentityColumn(100000, 1)
	            .ValueGeneratedOnAdd();
	
	        builder.Property(e => e!.Code).HasColumnName("Code").HasMaxLength(50).IsRequired();
	        builder.Property(e => e!.Comments).HasColumnName("Comments").HasMaxLength(500);
	        builder.Property(e => e!.Definition).HasColumnName("Definition").HasMaxLength(500);
	        builder.Property(e => e!.Display).HasColumnName("Display").HasMaxLength(255).IsRequired();
	        builder.Property(e => e!.Source).HasColumnName("Source").HasMaxLength(50);
	    }
	}
	
	#endregion
	#region \Internal\Extensions
	
	public static class DatabaseFacadeExtensions
	{
	    public static T? SqlQueryJson<T>(this DatabaseFacade database, JsonSerializerOptions? options, string query, params object[] args)
	    {
	        var json = string.Join("", database.SqlQueryRaw<string>(query, args));
	
	        return JsonSerializer.Deserialize<T>(json, options);
	    }
	}
	
	
	internal static class PropertyBuilderExtensions
	{
	    // NOTE: SQLite doesn't support JSON columns yet. Otherwise, we'd prefer calling .ToJson() on the owned entity instead.
	    public static PropertyBuilder<T> HasValueJsonConverter<T>(this PropertyBuilder<T> propertyBuilder)
	    {
	        return propertyBuilder.HasConversion(
	            new ValueJsonConverter<T>(),
	            new ValueJsonComparer<T>());
	    }
	
	    public static PropertyBuilder<T> HasListOfIdsConverter<T>(this PropertyBuilder<T> propertyBuilder)
	    {
	        return propertyBuilder.HasConversion(
	            new ListOfIdsConverter(),
	            new ListOfIdsComparer());
	    }
	}
	
	#endregion
	#region \obj\Debug\net8.0
	#endregion
	#region \Services
	
	public class SystemDateTimeProvider : IDateTimeProvider
	{
	    public DateTime UtcNow => DateTime.UtcNow;
	}
	
	#endregion
}
#endregion
#region Mint.Configuration
namespace Mint.Configuration
{
	#region \
	
	public class MintOptions
	{
	    /// <summary>
	    /// Domain Settings
	    /// </summary>
	    public DomainOptions? Domains { get; set; }
	    /// <summary>
	    /// Represents the Identity settings
	    /// </summary>
	    public IdentityOptions? Identity { get; set; }
	    /// <summary>
	    /// Represents the data layer configurations.
	    /// </summary>
	    public DataOptions? Data { get; set; }
	    /// <summary>
	    /// Represents the integration layer configurations.
	    /// </summary>
	    public MessagingOptions? Messaging { get; set; }
	    /// <summary>
	    /// 
	    /// </summary>
	    public LoggingOptions? Logging { get; set; }
	}
	
	#endregion
	#region \Data
	
	public class DataOptions
	{
	    public SqlServerOptions? SqlServer { get; set; }
	    public StorageOptions? Storage { get; set; }
	}
	
	
	public class SqlServerOptions
	{
	    public string? ConnectionString { get; set; }
	}
	
	
	public class StorageOptions
	{
	    public Uri? TableServiceUri { get; set; }
	    public Uri? QueueServiceUri { get; set; }
	    public Uri? BlobServiceUri { get; set; }
	    public Uri? FileServiceUri { get; set; }
	}
	
	#endregion
	#region \Domains
	
	public class DomainOptions
	{
	    public CoreOptions? Core { get; set; }
	    public AdminOptions? Admin { get; set; }
	    public SupportOptions? Support { get; set; }
	}
	
	#endregion
	#region \Domains\Admin
	
	public class AdminClaimsMapperOptions
	{
	    public string? Username { get; set; }
	    public string? Password { get; set; }
	}
	
	
	public class AdminOptions
	{
	    public Uri? Uri { get; set; }
	    public AdminClaimsMapperOptions? ClaimsMapper { get; set; }
	}
	
	#endregion
	#region \Domains\Core
	
	public class CoreOptions
	{
	    public Uri? Uri { get; set; }
	}
	
	#endregion
	#region \Domains\Support
	
	public class SupportOptions
	{
	    public Uri? Uri { get; set; }
	}
	
	#endregion
	#region \Extensions
	
	public static class ConfigurationBuilderExtensions
	{
	    public static IConfigurationBuilder AddMintConfiguration(this IConfigurationBuilder builder)
	    {
	        var credentials = new DefaultAzureCredential();
	
	        // Local Development ONLY: Since a user account might be associated with multiple Azure Tenants we can speciy the Tenant for 
	        var tenantId = Environment.GetEnvironmentVariable("AzureTenantId", EnvironmentVariableTarget.Process);
	
	        if (!string.IsNullOrEmpty(tenantId))
	        {
	            credentials = new DefaultAzureCredential(new DefaultAzureCredentialOptions()
	            {
	                // If you want to point to a different environment, just change the tenant id and app configuration in the appsettings.Development.json.
	                // ! Setting Up Azure Credentials
	                //  1. Select Tools > Options > Azure Service Authentication
	                //  2. Select the account you want to use or add a new account
	                TenantId = tenantId,
	                ExcludeEnvironmentCredential = true,
	                ExcludeWorkloadIdentityCredential = true,
	                ExcludeAzurePowerShellCredential = true,
	                ExcludeAzureCliCredential = true,
	                ExcludeManagedIdentityCredential = true,
	                ExcludeAzureDeveloperCliCredential = true,
	                ExcludeSharedTokenCacheCredential = true
	            });
	        }
	
	        var uri = Environment.GetEnvironmentVariable("AzureAppConfiguration", EnvironmentVariableTarget.Process);
	
	        builder.AddAzureAppConfiguration(options =>
	        {
	            options.Connect(new Uri(uri!), credentials)
	                .ConfigureKeyVault(options =>
	                {
	                    options.SetCredential(credentials);
	                })
	                .Select(KeyFilter.Any, LabelFilter.Null);
	
	            // If the application is running in a deployment slot let's override any setting with the slot label
	            if (TryGetSlotLabel(out var label))
	            {
	                options.Select(KeyFilter.Any, label);
	            }
	        });
	
	        return builder;
	    }
	
	    private static bool TryGetSlotLabel(out string slot)
	    {
	        return (slot = Environment.GetEnvironmentVariable("Slot", EnvironmentVariableTarget.Process)!) is not null;
	    }
	}
	
	
	public static class ServiceCollectionExtensions
	{
	    public static IServiceCollection AddMintOptions(this IServiceCollection services)
	    {
	        services.AddOptions<MintOptions>();
	        services.AddBoundOptions<IdentityOptions>("Identity");
	
	        services.AddBoundOptions<DataOptions>("Data");
	        services.AddBoundOptions<SqlServerOptions>("Data:SqlServer");
	
	        services.AddBoundOptions<MessagingOptions>("Messaging");
	        services.AddBoundOptions<ServiceBusOptions>("Messaging:ServiceBus");
	
	        services.AddBoundOptions<DomainOptions>("Domain");
	        services.AddBoundOptions<CoreOptions>("Domain:Core");
	        services.AddBoundOptions<AdminOptions>("Domain:Admin");
	        services.AddBoundOptions<AdminClaimsMapperOptions>("Domain:Admin:ClaimsMapper");
	        services.AddBoundOptions<SupportOptions>("Domain:Support");
	
	        services.AddBoundOptions<LoggingOptions>("Logging");
	        services.AddBoundOptions<AppInsightsOptions>("Logging:AppInsights");
	
	        return services;
	    }
	
	
	    private static void AddBoundOptions<TOptions>(this IServiceCollection services, string sectionName)
	       where TOptions : class
	    {
	        services.AddOptions<TOptions>()
	            .Configure<IConfiguration>((options, configuration) =>
	            {
	                configuration.GetSection(sectionName).Bind(options);
	            });
	    }
	}
	
	public static class WebApplicationBuilderExtensions
	{
	    public static WebApplicationBuilder AddMintConfiguration(this WebApplicationBuilder builder)
	    {
	        if (!builder.Environment.EnvironmentName.Equals("Local"))
	        {
	            builder.Configuration.AddMintConfiguration();
	        }
	
	        builder.Services.AddMintOptions();
	
	        return builder;
	    }
	}
	
	#endregion
	#region \Identity
	
	public class IdentityOptions
	{
	    public string? TenantId { get; set; }
	    public string? Issuer { get; set; }
	    public string? Instance { get; set; }
	    public string? ClientId { get; set; }
	    public string? ClientSecret { get; set; }
	    public string? Domain { get; set; }
	    public string? SignUpSignInPolicyId { get; set; }
	}
	#endregion
	#region \Logging
	
	public class AppInsightsOptions
	{
	    public string? ConnectionString { get; set; }
	    public string? InstrumentationKey { get; set; }
	    public string? WorkspaceId { get; set; }
	}
	
	
	public class LoggingOptions
	{
	    public AppInsightsOptions? AppInsights { get; set; }
	}
	
	#endregion
	#region \Messaging
	
	public class MessagingOptions
	{
	    public ServiceBusOptions? ServiceBus { get; set; }
	    public EventGridOptions? EventGrid { get; set; }
	}
	
	
	public class EventGridOptions
	{
	}
	
	
	public class ServiceBusOptions
	{
	    public Uri? Uri { get; set; }
	    public string? FullyQualifiedDomainName { get; set; }
	}
	
	#endregion
	#region \obj\Debug\net8.0
	#endregion
}
#endregion
#region Mint.Data
namespace Mint.Data
{
	#region \
	
	public class DatabaseProviderOptions
	{
	    public IServiceCollection Services { get; init; } = default!;
	}
	
	#endregion
	#region \Exceptions
	
	public sealed class DataException : MintException
	{
	    public DataException(string message, MintErrorCode errorCode) 
	        : base(message, errorCode)
	    {
	    }
	
	    public DataException(string message, MintErrorCode errorCode, Exception innerException)
	        : base(message, errorCode, innerException)
	    {
	    }
	
	    public string? Service { get; init; }
	}
	
	#endregion
	#region \Extensions
	
	public static class ServiceCollectionExtensions
	{
	    public static IServiceCollection AddMintDatabase(this IServiceCollection services, Action<DatabaseProviderOptions> configure)
	    {
	        if (configure is null)
	        {
	            throw new ArgumentNullException(nameof(configure));
	        }
	        var options = new DatabaseProviderOptions()
	        {
	            Services = services
	        };
	
	        configure.Invoke(options);
	
	        return services;
	    }
	}
	
	
	public static class WebApplicationBuilderExtensions
	{
	    public static WebApplicationBuilder AddMintDatabase(this WebApplicationBuilder builder, Action<DatabaseProviderOptions> configure)
	    {
	        if (configure is null)
	        {
	            throw new ArgumentNullException(nameof(configure));
	        }
	
	        builder.Services.AddMintDatabase(configure);
	
	        return builder;
	    }
	}
	
	#endregion
	#region \obj\Debug\net8.0
	#endregion
}
#endregion
#region Mint
namespace Mint
{
	#region \
	
	public abstract class AuditableEntity : Entity, IAuditableEntity
	{
	    public DateTimeOffset? Created { get; set; }
	    public UserId? CreatedBy { get; set; }
	    public DateTimeOffset? Modified { get; set; }
	    public UserId? ModifiedBy { get; set; }
	}
	
	
	public abstract partial class Entity
	{
	
	}
	
	
	public abstract class Entity<TKey> : IEqualityComparer<TKey> 
	    where TKey : struct
	{
	    public virtual TKey Id { get; set; }
	
	
	    bool IEqualityComparer<TKey>.Equals(TKey x, TKey y)
	    {
	        throw new NotImplementedException();
	    }
	
	    int IEqualityComparer<TKey>.GetHashCode(TKey obj)
	    {
	        throw new NotImplementedException();
	    }
	}
	#endregion
	#region \Abstractions
	
	public interface IAddress
	{
	    int AddressId { get; set; }
	    string? Type { get; set; }
	    string? Use { get; set; }
	    string? City { get; set; }
	    long? Country { get; set; }
	    string? PostalCode { get; set; }
	    long? Region { get; set; }
	    string? State { get; set; }
	    string? StreetOne { get; set; }
	    string? StreetThree { get; set; }
	    string? StreetTwo { get; set; }
	}
	
	public interface IAuditableEntity
	{
	    DateTimeOffset? Created { get; set; }
	    UserId? CreatedBy { get; set; }
	    DateTimeOffset? Modified { get; set; }
	    UserId? ModifiedBy { get; set; }
	}
	
	public interface ICommunication
	{
	    public bool? IsPrimary { get; set; }
	
	    public string? Use { get; set; }
	
	    public string? System { get; set; }
	
	    public string? Value { get; set; }
	
	    public int? Rank { get; set; }
	}
	
	
	public interface IDateTimeProvider
	{
	    public DateTime UtcNow { get; }
	}
	
	#endregion
	#region \Domains
	
	public abstract class BaseCommunication : AuditableEntity
	{
	
	    public bool? IsPrimary { get; set; }
	
	    public ValueSetItemId? UseId { get; set; }
	
	    public ValueSetItemId? SystemId { get; set; }
	
	    public string? Value { get; set; }
	
	    public int? Rank { get; set; }
	
	    public DateTimeOffset? StartDate { get; set; }
	
	    public DateTimeOffset? EndDate { get; set; }
	
	    public virtual ValueSetItem? Use { get; set; }
	
	    public virtual ValueSetItem? System { get; set; }
	}
	
	public enum Domain
	{
	    Admin = 1000,
	    Organization = 2000,
	    Providers = 3000,
	    Patients = 4000,
	    Support = 5000
	}
	
	public enum DomainEvent
	{
	    UserCreated = 1001,
	    UserUpdated = 1002,
	    UserDeleted = 1003,
	    GroupCreated = 1004,
	    GroupUpdated = 1005,
	    GroupDeleted = 1006,
	    GroupMemberAdded,
	    GroupMemberRemoved,
	    GroupOwnerAdded,
	    GroupOwnerRemoved,
	
	    RoleAssigned,
	    RoleUnassigned
	}
	
	public class DomainException : MintException
	{
	    public DomainException(string message, MintErrorCode errorCode) 
	        : base(message, errorCode)
	    {
	    }
	    public DomainException(string message, MintErrorCode errorCode, Exception innerException) 
	        : base(message, errorCode, innerException)
	    {
	    }
	
	    [SetsRequiredMembers]
	    public DomainException(string message, MintErrorCode errorCode, Domain domain)
	         : base(message, errorCode)
	    {
	        Domain = domain;
	    }
	
	    public required Domain Domain { get; init; }
	
	    public static DomainException NotFound(Domain domain, string? message = null) =>
	        new DomainException(message ?? "The entity was not found.", MintErrorCode.NotFound, domain);
	}
	
	#endregion
	#region \Domains\Admin
	
	public class Group : AuditableEntity
	{
	    public GroupId Id { get; set; }
	    public GroupInfo? Info { get; set; }
	    public virtual ICollection<User> Members { get; set; } = new List<User>();
	    public virtual ICollection<User> Owners { get; set; } = new List<User>();
	    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
	}
	
	public class MyRole : Entity
	{
	    public RoleId Id { get; set; }
	    public RoleType? Type { get; set; }
	    public RoleInfo? Info { get; set; }
	    public bool IsInherited { get; set; }
	    public bool IsAssigned { get; set; }
	}
	
	
	public class Permission
	{
	    public RoleId RoleId { get; set; }
	    public PermissionId Id { get; set; }
	    public PermissionAction? Action { get; set; }
	    public string? Description { get; set; }
	    public virtual Role? Role { get; set; }
	}
	
	
	public class Role : Entity
	{
	    public RoleId Id { get; set; }
	    public RoleType? Type { get; set; }
	    public RoleInfo? Info { get; set; }
	    public virtual ICollection<User> Users { get; set; } = new List<User>();
	    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
	    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
	}
	
	public partial class User : AuditableEntity
	{
	    public UserId Id { get; set; }
	    public UserType? Type { get; set; }
	    public UserInfo? Info { get; set; }
	    public OrganizationId? OrganizationId { get; set; }
	    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
	    public virtual ICollection<Group> OwnedGroups { get; set; } = new List<Group>();
	    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
	    public virtual ICollection<Organization> Organizations { get; set; } = new List<Organization>();
	}
	#endregion
	#region \Domains\Admin\ValueObjects
	
	[StronglyTypedId(Template.Guid, "guid-efcore")]
	public partial struct GroupId
	{
	    public GroupId(string id)
	       : this(new Guid(id)) { }
	}
	
	
	public record class GroupInfo
	{
	    public string? Name { get; set; }
	    public string? Description { get; set; }
	}
	
	
	public enum PermissionAction
	{
	    Read,
	    Create,
	    Edit,
	    Delete
	}
	
	
	[StronglyTypedId(Template.Int, "int-efcore")]
	public partial struct PermissionId
	{
	}
	
	
	[StronglyTypedId(Template.Guid, "guid-efcore")]
	public partial struct RoleId
	{
	    private static ReadOnlySpan<Guid> identifiers => new[]
	    {
	        new Guid("12863333-5d89-4c2d-a83f-3c5ed268a635"),   // Patient Access
	        new Guid("db982fd6-2978-4286-b2e1-2d149bef190c"),   // Provider Access
	        new Guid("125c70ff-8913-48f1-b9a3-b700985e6be8"),   // Directory Access
	        new Guid("6c6f8993-7183-453e-9d3a-5884162c0a0b"),   // Users.ScopedAdmin
	        new Guid("6c6f8993-7183-453e-9d3a-5884162c0a0b"),   // Users.SuperAdmin
	        new Guid("b8738a05-48e2-4cad-95b4-2d2b45d6c945"),   // Organization Access
	        new Guid("01346fc1-a487-4d4e-822d-b9dfc983cd95"),   // Support Access
	        new Guid("2a230f7f-c9c2-4d20-85e8-8003f57d2f58"),   // System Management Access
	    };
	
	
	    public static bool IsPatientAccess(RoleId id) => identifiers[0].Equals(id);
	    public static bool IsProviderAccess(RoleId id) => identifiers[0].Equals(id);
	    public static bool IsDirectoryAccess(RoleId id) => identifiers[0].Equals(id);
	    public static bool IsUsersAccess(RoleId id) => identifiers[0].Equals(id);
	    public static bool IsOrganizationAccess(RoleId id) => identifiers[0].Equals(id);
	    public static bool IsSystemManagementAccess(RoleId id) => identifiers[0].Equals(id);
	
	    //public RoleId(Guid value)
	    //{
	    //    if (!identifiers.Contains(value))
	    //    {
	    //        throw new ArgumentException();
	    //    }
	    //    Value = value;
	    //}
	}
	
	
	public record class RoleInfo
	{
	    public string? Name { get; set; }
	    public string? Value { get; set; }
	    public string? Description { get; set; }
	}
	
	
	public enum RoleType
	{
	    Privileged = 1,
	    NonPrivileged = 2,
	    System = 3
	}
	
	
	[StronglyTypedId(Template.Guid, "guid-efcore")]
	public partial struct UserId
	{
	    public UserId(string id)
	       : this(new Guid(id)) { }
	}
	
	
	public record class UserInfo
	{
	    public string? FirstName { get; set; }
	    public string? LastName { get; set; }
	    public string? MiddleName { get; set; }
	    public string? Email { get; set; }
	    public bool? IsEnabled { get; set; }
	    public bool? IsLocked { get; set; }
	    public DateTimeOffset? LastSignIn { get; set; }
	}
	
	
	public enum UserType
	{
	    Standard = 1,
	    System = 2,
	    Test = 3
	}
	
	#endregion
	#region \Domains\Fhir\Exceptions
	
	internal class FhirException
	{
	}
	
	#endregion
	#region \Domains\Organizations
	
	public partial class Accessibility : Entity
	{
	    public AccessibilityId? Id { get; set; }
	    public string? Description { get; set; }
	    public string? Details { get; set; }
	    public string? Url { get; set; }
	    public LocationId? LocationId { get; set; }
	    public virtual Location? Location { get; set; }
	}
	
	public class Address : Entity
	{
	    public ValueSetItemId? AddressTypeId { get; set; }
	    public ValueSetItemId? AddressUseId { get; set; }
	    public string? AddressOne { get; set; }
	    public string? AddressTwo { get; set; }
	    public string? City { get; set; }
	    public string? PostalCode { get; set; }
	    public string? Country { get; set; }
	    public string? County { get; set; }
	    public string? Region { get; set; }
	    public string? StateProvince { get; set; }
	
	    //[HotChocolate.GraphQLIgnore]
	    //public Point? Coordinate { get; set; }
	    public ValueSetItem? AddressType { get; set; }
	    public ValueSetItem? AddressUse { get; set; }
	
	}
	
	
	public partial class Availability : AuditableEntity
	{
	    public AvailabilityId? Id { get; set; }
	    public int? DayOfWeek { get; set; }
	    public bool? IsAllDay { get; set; }
	    public TimeOnly? StartTime { get; set; }
	    public TimeOnly? EndTime { get; set; }
	    public bool? IsClosed { get; set; }
	    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
	    public virtual ICollection<ProviderRole> ProvidersRoles { get; set; } = new List<ProviderRole>();
	}
	
	
	public partial class Language
	{
	    public int Id { get; set; }
	    public string? LanguageCode { get; set; }
	    public string? LanguageDisplay { get; set; }
	
	}
	
	public partial class Location : AuditableEntity
	{
	    public LocationId Id { get; set; }
	    public OrganizationId OrganizationId { get; set; }
	    public string? Url { get; set; }
	    public string? Name { get; set; }
	    public string? AlternateName { get; set; }
	    public string? Phone { get; set; }
	    public string? Description { get; set; }
	    public string? Transportation { get; set; }
	    public int? LocationTypeId { get; set; }
	    public string? AreaOfFocus { get; set; }
	    public bool IsVisible { get; set; }
	    public ValueSetItemId? LocationStatusId { get; set; }
	    public ValueSetItemId? LocationModeId { get; set; }
	    public Address? Address { get; set; }
	    public ValueSetItem? LocationStatus { get; set; }
	    public ValueSetItem? LocationMode { get; set; }
	    public virtual Organization Organization { get; set; } = null!;
	    public virtual ICollection<ProviderAssignment> ProvidersAssignments { get; set; } = new List<ProviderAssignment>();
	    public virtual ICollection<Availability> Availabilities { get; set; } = new List<Availability>();
	    public virtual ICollection<Accessibility> Accessibilities { get; set; } = new List<Accessibility>();
	    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
	}
	
	public partial class Organization : AuditableEntity
	{
	    public OrganizationId Id { get; set; }
	    public OrganizationId? ParentId { get; set; }
	    public string? Name { get; set; }
	    public string? Description { get; set; }
	    public int? Type { get; set; }
	    public int? YearIncorporated { get; set; }
	    public string? LegalStatus { get; set; }
	    public string? Logo { get; set; }
	    public string? Uri { get; set; }
	    public string? Oid { get; set; }
	    public long? NPI { get; set; }
	    public string? Phone { get; set; }
	    // TODO this should be valuesetid
	    public int? AddressTypeId { get; set; }
	    public int? AddressUseId { get; set; }
	
	    public Address? Address { get; set; } = default!;
	    public virtual Organization? Parent { get; set; }
	    public virtual ICollection<User> Users { get; set; } = new List<User>();
	    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
	    public virtual ICollection<Program> Programs { get; set; } = new List<Program>();
	    public virtual ICollection<PatientInstance> PatientInstances { get; set; } = new List<PatientInstance>();
	    public virtual ICollection<ProviderAssignment> ProvidersAssignments { get; set; } = new List<ProviderAssignment>();
	    //public virtual ICollection<ProviderRole> ProvidersRoles { get; set; } = new List<ProviderRole>();
	    //public virtual ICollection<ProviderCommunication> Communications { get; set; } = new List<ProviderCommunication>();
	
	    public bool CompareOid(string input)
	        => CompareOid(input, this.Oid.ToString());
	
	
	    public static bool CompareOid(string? oid, string? input)
	    {
	        if (string.IsNullOrEmpty(oid) || string.IsNullOrEmpty(input))
	        {
	            return false;
	        }
	
	        // Compare the normalized input with the OID
	        return string.Equals(Normalize(input), Normalize(oid), StringComparison.OrdinalIgnoreCase);
	
	        static string Normalize(string text) => text.StartsWith("urn:oid:") ? text.Substring(8) : text;
	    }
	}
	
	
	public partial class Program
	{
	    public ProgramId? Id { get; set; }
	    public OrganizationId? OrganizationId { get; set; }
	    public string? Name { get; set; }
	    public string? AlternateName { get; set; }
	    public string? Description { get; set; }
	    public virtual Organization? Organization { get; set; }
	    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
	}
	
	
	public partial class RequiredDocument
	{
	    public DocumentId? Id { get; set; }
	    public ServiceId? ServiceId { get; set; }
	    public string? Document { get; set; }
	    public string? Uri { get; set; }
	    public virtual Service? Service { get; set; }
	}
	
	
	public partial class Service : AuditableEntity
	{
	    public ServiceId? Id { get; set; }
	    public string? Name { get; set; }
	    public string? AlternateName { get; set; }
	    public string? Description { get; set; }
	    public int? Populations { get; set; }
	    public string? Url { get; set; }
	    public string? Email { get; set; }
	    public int? Status { get; set; }
	    public string? InterpretationServices { get; set; }
	    public string? ApplicationProcess { get; set; }
	    public string? FeesDescription { get; set; }
	    public string? Accreditations { get; set; }
	    public string? EligibilityDescription { get; set; }
	    public string? Licenses { get; set; }
	    public string? Alert { get; set; }
	    public int? CategoryType { get; set; }
	    public int? ServiceType { get; set; }
	    public string? CoverageArea { get; set; }
	
	    public ProgramId? ProgramId { get; set; }
	    public virtual Program? Program { get; set; }
	    public virtual ICollection<RequiredDocument> RequiredDocuments { get; set; } = new List<RequiredDocument>();
	    public virtual ICollection<ServiceLanguage> ServiceLanguages { get; set; } = new List<ServiceLanguage>();
	    //public virtual ICollection<Communication> Communications { get; set; } = new List<Communication>();
	    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
	    public virtual ICollection<Provider> Providers { get; set; } = new List<Provider>();
	}
	
	
	public partial class ServiceLanguage
	{
	    public ServiceId? ServiceId { get; set; }
	    public int LanguageId { get; set; }
	    public virtual Service Service { get; set; } = null!;
	}
	
	
	public partial class ValueSet : Entity
	{
	    public ValueSetId? Id { get; set; }
	    public string? DefiningUrl { get; set; }
	    public string? Version { get; set; }
	    public string? Name { get; set; }
	    public string? Title { get; set; }
	    public string? Definition { get; set; }
	    public string? Oid { get; set; }
	    public virtual ICollection<ValueSetItem> ValueSetItems { get; set; } = new List<ValueSetItem>();
	}
	
	
	public partial class ValueSetHeader : Entity
	{
	    public int? Id { get; set; }
	    public string? DefiningUrl { get; set; }
	    public string? Version { get; set; }
	    public string? Name { get; set; }
	    public string? Title { get; set; }
	    public string? Definition { get; set; }
	    public string? Oid { get; set; }
	    public ValueSetItem? Items { get; set; }
	}
	
	
	public partial class ValueSetItem : Entity
	{
	    public ValueSetItemId Id { get; set; }
	    public string? Code { get; set; }
	    public string? Source { get; set; }
	    public string? Display { get; set; }
	    public string? Definition { get; set; }
	    public string? Comments { get; set; }
	    public int? ValueSetId { get; set; }
	    public virtual ValueSetHeader? ValueSet { get; set; }
	}
	
	#endregion
	#region \Domains\Organizations\Exceptions
	
	public class OrganizationAccessException : MintException
	{
	    public OrganizationAccessException(OrganizationId organizationId, UserId userId) : base($"Organization access denied.", MintErrorCode.OrganizationAccessUnauthorized)
	    {
	        this.Data["OrganizationId"] = organizationId;
	        this.Data["UserId"] = userId;
	    }
	}
	
	public class OrganizationNotFoundException : MintException
	{
	    public OrganizationNotFoundException(OrganizationId organizationId) : base($"Organization not found.", MintErrorCode.OrganizationNotFound)
	    {
	        this.Data["OrganizationId"] = organizationId;
	    }
	}
	#endregion
	#region \Domains\Organizations\ValueObjects
	
	[StronglyTypedId]
	public partial struct AccessibilityId
	{
	    public static implicit operator int(AccessibilityId id) => id.Value;
	    public static implicit operator AccessibilityId(int id) => new(id);
	}
	
	[StronglyTypedId]
	public partial struct AddressId
	{
	    public static implicit operator int(AddressId id) => id.Value;
	    public static implicit operator AddressId(int id) => new AddressId(id);
	}
	
	
	[StronglyTypedId]
	public partial struct AvailabilityId
	{
	    public static implicit operator int(AvailabilityId id) => id.Value;
	    public static implicit operator AvailabilityId(int id) => new AvailabilityId(id);
	}
	
	
	[StronglyTypedId]
	public partial struct CommunicationId
	{
	    public static implicit operator int(CommunicationId id) => id.Value;
	    public static implicit operator CommunicationId(int id) => new CommunicationId(id);
	}
	
	
	[StronglyTypedId]
	public partial struct LocationId
	{
	    //public static implicit operator int(LocationId id) => id.Value;
	    //public static implicit operator LocationId(int id) => new(id);
	}
	
	
	[StronglyTypedId]
	public partial struct OrganizationId
	{
	    public static implicit operator int(OrganizationId id) => id.Value;
	    public static implicit operator OrganizationId(int id) => new OrganizationId(id);
	}
	
	
	
	[StronglyTypedId]
	public partial struct ProgramId
	{
	    public static implicit operator int(ProgramId id) => id.Value;
	    public static implicit operator ProgramId(int id) => new ProgramId(id);
	}
	
	
	[StronglyTypedId]
	public partial struct ServiceAtId
	{
	    public static implicit operator int(ServiceAtId id) => id.Value;
	    public static implicit operator ServiceAtId(int id) => new ServiceAtId(id);
	}
	
	
	[StronglyTypedId]
	public partial struct ServiceId
	{
	    public static implicit operator int(ServiceId id) => id.Value;
	    public static implicit operator ServiceId(int id) => new ServiceId(id);
	}
	
	
	[StronglyTypedId]
	public partial struct ValueSetId
	{
	    public static implicit operator int(ValueSetId id) => id.Value;
	    public static implicit operator ValueSetId(int id) => new ValueSetId(id);
	}
	
	
	[StronglyTypedId]
	public partial struct ValueSetItemId
	{
	    public static implicit operator int(ValueSetItemId id) => id.Value;
	    public static implicit operator ValueSetItemId(int id) => new ValueSetItemId(id);
	}
	
	#endregion
	#region \Domains\Patients
	
	public partial class Patient : AuditableEntity
	{
	    public PatientId Id { get; set; }
	    public Guid? PatientKey { get; set; }
	    public PatientRegionId? RegionId { get; set; }    
	    public string? AccountNumber { get; set; }
	    public string? FirstName { get; set; }
	    public string? LastName { get; set; }
	    public string? MiddleName { get; set; }
	    public int? Gender { get; set; }
	    public int? SexualOrientation { get; set; }
	    public int? GenderIdentity { get; set; }
	    public int? PreferredLanguage { get; set; }
	    public DateOnly? DateOfBirth { get; set; }
	    public DateOnly? DateOfDeath { get; set; }
	    public int? Race { get; set; }
	    public int? Ethnicity { get; set; }
	    public string? TribalAffiliation { get; set; }
	    public string? Occupation { get; set; }
	    public int? OccupationIndustry { get; set; }
	
	    //public virtual ICollection<CareTeam> CareTeams { get; set; } = new List<CareTeam>();
	
	    //public virtual ICollection<Pathway> Pathways { get; set; } = new List<Pathway>();
	
	
	    //[GraphQLNonNullType]
	    public virtual ICollection<PatientAddress> Addresses { get; set; } = new List<PatientAddress>();
	
	
	    //[GraphQLNonNullType]
	    public virtual ICollection<PatientCommunication> Communications { get; set; } = new List<PatientCommunication>();
	
	
	    //[GraphQLNonNullType]
	    public virtual ICollection<PatientIdentifier> Identifiers { get; set; } = new List<PatientIdentifier>();
	
	
	
	    //[GraphQLNonNullType]
	    public virtual ICollection<PatientFhirResource> Resources { get; set; } = new List<PatientFhirResource>();
	
	
	   // [GraphQLNonNullType]
	    public virtual ICollection<PatientInstance> Instances { get; set; } = new List<PatientInstance>();
	
	    //[GraphQLNonNullType]
	    public virtual ICollection<PatientDocument> Documents { get; set; } = new List<PatientDocument>();
	
	}
	
	
	public partial class PatientAddress : Address, IAuditableEntity
	{
	    public AddressId Id { get; set; }
	
	    //[GraphQLIgnore]
	    public PatientId PatientId { get; set; }
	
	    public bool? IsActive { get; set; }
	
	    public DateTimeOffset? Created { get; set; }
	    public UserId? CreatedBy { get; set; }
	    public DateTimeOffset? Modified { get; set; }
	    public UserId? ModifiedBy { get; set; }
	
	    //[GraphQLIgnore]
	    public virtual Patient? Patient { get; set; }
	}
	
	
	public partial class PatientCommunication : BaseCommunication
	{
	    public CommunicationId Id { get; set; }
	
	    public PatientId PatientId { get; set; }
	
	    public Patient Patient { get; set; }
	}
	
	public class PatientDocument
	{
	    public int Id { get; set; }
	    public LookupId TypeId { get; set; }
	    public string ResourceId { get; set; }
	    public string Content { get; set; }
	    public PatientId PatientId { get; set; }
	    public OrganizationId OrganizationId { get; set; }
	    public string Title { get; set; }
	    public DateTime DocumentDate { get; set; }
	    public DateTimeOffset Created { get; set; }
	    public DateTimeOffset Modified { get; set; }
	    public virtual Lookup Type { get; set; }
	    public virtual Organization? Organization { get; set; }
	    public virtual Patient? Patient { get; set; }
	}
	
	public partial class PatientFhirResource
	{
	    public int Id { get; set; }
	
	    public PatientId PatientId { get; set; }
	
	    //[GraphQLName("content")]
	    public string Resource { get; set; } = default!;
	
	    public string ResourceType { get; set; } = default!;
	
	    public string? RequestUri { get; set; }
	
	    public string SourceId { get; set; } = default!;
	
	    public DateTimeOffset Created { get; set; } = new DateTime();
	
	    public DateTimeOffset Modified { get; set; } = new DateTime();
	
	    public byte[] RowVersion { get; set; } = null!;
	
	    public virtual Patient? Patient { get; set; }
	
	    public string? ProvenanceId { get; set;  }
	
	
	
	}
	
	
	public partial class PatientIdentifier
	{
	    public int Id { get; set; }
	
	    public PatientId PatientId { get; set; }
	
	    public ValueSetItemId? UseId { get; set; }
	
	    public string? System { get; set; }
	
	    public string Value { get; set; } = default!;
	
	    public virtual ValueSetItem? Use { get; set; }
	
	    public virtual Patient Patient { get; set; }
	}
	
	
	public class PatientInstance : AuditableEntity
	{
	    public PatientId PatientId { get; set; }
	    public OrganizationId OrganizationId { get; set; }
	    public UserId? UserId { get; set; }
	    public string? MRN { get; set; }
	    public virtual Organization? Organization { get; set; }
	    public virtual Patient? Patient { get; set; }
	    public virtual User? User { get; set; }
	}
	#endregion
	#region \Domains\Patients\Exceptions
	
	public class PatientNotFoundException : MintException
	{
	    public PatientNotFoundException(PatientId patientId) : base($"Patient not found.", MintErrorCode.PatientNotFound)
	    {
	        this.Data["PatientId"] = patientId;
	    }
	
	    public PatientNotFoundException(Guid patientKey) : base($"Patient not found.", MintErrorCode.PatientNotFound)
	    {
	        this.Data["PatientKey"] = patientKey;
	    }
	}
	#endregion
	#region \Domains\Patients\ValueObjects
	
	[StronglyTypedId]
	public partial struct DocumentId
	{
	    public static implicit operator int(DocumentId id) => id.Value;
	    public static implicit operator DocumentId(int id) => new DocumentId(id);
	}
	
	
	[StronglyTypedId(Template.String, "string-efcore")]
	public partial struct PatientFhirId
	{
	    public (string mrn, string oid) Extract()
	        => Extract(this.Value);
	
	    public static (string mrn, string oid) Extract(string base64EncodedString)
	    {
	        // Split the decoded string by '&'
	        string[] values = Base64Decode(base64EncodedString).Split('&');
	
	        return (values[0], values[1]);
	    }
	
	    static string Base64Decode(string base64EncodedData)
	    {
	        // Mint team stripes trailing '='
	        // Ensure the base64 string length is a multiple of 4
	        base64EncodedData = base64EncodedData.TrimEnd('=');
	        switch (base64EncodedData.Length % 4)
	        {
	            case 2: base64EncodedData += "=="; break;
	            case 3: base64EncodedData += "="; break;
	        }
	
	        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
	        return Encoding.UTF8.GetString(base64EncodedBytes);
	
	    }
	
	
	    public static PatientFhirId Create(string mrn, string oid)
	    {
	        var plainTextBytes = Encoding.UTF8.GetBytes($"{mrn}&{oid}");
	        return new PatientFhirId(Convert.ToBase64String(plainTextBytes).Replace("=",""));
	    }
	}
	
	[StronglyTypedId]
	public partial struct PatientId;
	
	[StronglyTypedId(Template.String, "string-efcore")]
	public partial struct PatientRegionId;
	#endregion
	#region \Domains\Providers
	
	public partial class Lookup
	{
	    public LookupId Id { get; set; }
	    public LookupId? ParentId { get; set; }
	    public string Label { get; set; } = null!;
	    public string? Description { get; set; }
	    public long? EnumFlag { get; set; }
	    public bool IsActive { get; set; }
	    public short SortOrder { get; set; }
	    public string? Lineage { get; set; }
	    public short? Depth { get; set; }
	    public virtual Lookup? Parent { get; set; }
	    public virtual ICollection<Lookup> Children { get; set; } = new List<Lookup>();
	    public virtual ICollection<Provider> Providers { get; set; } = new List<Provider>();
	}
	
	public partial class Provider : AuditableEntity
	{
	    public ProviderId Id { get; set; }
	    //public ProviderInfo? Info { get; set; }
	    public string? Npi { get; set; }
	    public string? Oid { get; set; }
	    public string? FirstName { get; set; }
	    public string? LastName { get; set; }
	    public string? MiddleInitial { get; set; }
	    public string[]? Specialty { get; set; }
	    public int? Gender { get; set; }
	    public DateOnly? DateOfBirth { get; set; }
	    public string? Bio { get; set; }
	    public string? Avatar { get; set; }
	    public string? Prefix { get; set; }
	    public string? Title { get; set; }
	    public string? Suffix { get; set; }
	    public string? Credential { get; set; }
	    public string? MedicalSchoolName { get; set; }
	    public short? GraduationYear { get; set; }
	    // for now making this into a string it appears they want B = Both, T = Telehealth, I = In-Person need to confirm
	    public string? Telehealth { get; set; }
	    public bool? IsActive { get; set; }
	
	    // TODO: this should be a lookup
	    public string? PracticeFocus { get; set; }
	    // TODO: this should be a lookup
	    public string? ProfessionalAffiliation { get; set; }
	
	    public LookupId? ProviderTypeId { get; set; }
	    public virtual Lookup? ProviderType { get; set; }
	    public virtual ICollection<ProviderAssignment> Assignments { get; set; } = new List<ProviderAssignment>();
	    public virtual ICollection<ProviderLanguage> Languages { get; set; } = new List<ProviderLanguage>();
	    public virtual ICollection<ProviderRole> Roles { get; set; } = new List<ProviderRole>();
	    public virtual ICollection<ProviderCommunication> Communications { get; set; } = new List<ProviderCommunication>();
	    public virtual ICollection<ProviderQualification> Qualifications { get; set; } = new List<ProviderQualification>();
	    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
	    public virtual ICollection<ProviderExtension> Extensions { get; set; } = new List<ProviderExtension>();
	}
	
	public partial class ProviderAssignment
	{
	    public int AssignmentId { get; set; }
	    public ProviderId ProviderId { get; set; }
	    public LocationId LocationId { get; set; }
	    public OrganizationId OrganizationId { get; set; }
	    public virtual Location? Location { get; set; }
	    public virtual Provider? Provider { get; set; }
	    public virtual Organization? Organization { get; set; }
	}
	
	
	public partial class ProviderCommunication : BaseCommunication
	{
	    public ProviderId? ProviderId { get; set; }
	    public CommunicationId Id { get; set; }
	    public virtual Provider? Provider { get; set; }
	}
	
	public class ProviderExtension
	{
	    public int Id { get; set; }
	    public ProviderId ProviderId { get; set; }
	    public LookupId LookupId { get; set; }
	    public string? Code { get; set; }
	    public string? System { get; set; }
	    public string? Value { get; set; }
	
	    //public virtual Provider Provider { get; set; } = default!;
	    public virtual Lookup Lookup { get; set; } = default!;
	}
	
	
	public partial class ProviderLanguage
	{
	    public ProviderId ProviderId { get; set; }
	    public string Proficiency { get; set; } = null!;
	    public string? ProficiencyCode { get; set; }
	    public int LanguageId { get; set; }
	    public virtual Language Language { get; set; } = null!;
	    public virtual Provider Provider { get; set; } = null!;
	}
	
	public partial class ProviderQualification
	{
	    public QualificationId? Id { get; set; }
	    public ProviderId? ProviderId { get; set; }
	    public int? Code { get; set; }
	    public string? Value { get; set; }
	    public DateTimeOffset? StartDate { get; set; }
	    public DateTimeOffset? EndDate { get; set; }
	    public virtual Provider? Provider { get; set; }
	}
	
	
	public partial class ProviderRole
	{
	    public ProviderRoleId? Id { get; set; }
	    public ProviderId? ProviderId { get; set; }
	    public OrganizationId? OrganizationId { get; set; }
	    public int? Role { get; set; }
	    public int? Specialty { get; set; }
	    public string? AvailabilityExceptionsMessage { get; set; }
	    public virtual Provider? Provider { get; set; }
	    public virtual Organization? Organization { get; set; }
	    public virtual ICollection<Availability> Availabilities { get; set; } = new List<Availability>();
	}
	#endregion
	#region \Domains\Providers\Exceptions
	    internal class ProviderException
	    {
	    }
	
	#endregion
	#region \Domains\Providers\ValueObjects
	
	[StronglyTypedId]
	public partial struct LookupId { }
	
	[StronglyTypedId]
	public partial struct ProviderId
	{
	    public static implicit operator int(ProviderId id) => id.Value;
	    public static implicit operator ProviderId(int id) => new ProviderId(id);
	}
	
	
	[StronglyTypedId]
	public partial struct ProviderRoleId
	{
	    public static implicit operator int(ProviderRoleId id) => id.Value;
	    public static implicit operator ProviderRoleId(int id) => new ProviderRoleId(id);
	}
	
	
	[StronglyTypedId]
	public partial struct QualificationId
	{
	    public static implicit operator int(QualificationId id) => id.Value;
	    public static implicit operator QualificationId(int id) => new QualificationId(id);
	}
	
	#endregion
	#region \Domains\Support
	
	public class BrowserActivityLogEntry : LogEntry
	{
	    public LogId Id { get; set; }
	    public DomainEvent Event { get; set; }
	    public UserId UserId { get; set; }
	    public IDictionary<string, string>? Details { get; set; }
	    public DateTimeOffset? Timestamp { get; set; }
	    public string? Version { get; set; }
	    public override LogEntryType EntryType => LogEntryType.Audit;
	}
	
	public class BrowserErrorLogEntry : LogEntry
	{
	    public LogId Id { get; set; }
	    public string? Host { get; set; }
	    public string? Page { get; set; }
	    public string? Message { get; set; }
	    public UserId UserId { get; set; }
	    public string? Browser { get; set; }
	    public string? ClientIp { get; set; }
	    public DateTimeOffset? Timestamp { get; set; }
	    public override LogEntryType EntryType => LogEntryType.Error;
	}
	
	
	public abstract class LogEntry: Entity
	{
	    /// <summary>
	    /// 
	    /// </summary>
	    public abstract LogEntryType EntryType { get; }
	}
	
	
	/// <summary>
	/// Represents an aggregate of page visits per day.
	/// </summary>
	public class DailyPageVisitSummaryLogEntry : LogEntry
	{
	    /// <summary>
	    /// 
	    /// </summary>
	    public string? App { get; set; }
	    /// <summary>
	    /// The page visited
	    /// </summary>
	    public string? Page { get; set; }
	    /// <summary>
	    /// the total count of visits
	    /// </summary>
	    public long? Count { get; set; }
	    /// <summary>
	    /// 
	    /// </summary>
	    public DateOnly? Date { get; set; }
	    /// <summary>
	    /// 
	    /// </summary>
	    public override LogEntryType EntryType => LogEntryType.Metrics;
	}
	
	
	public class DailyUserPageVisitSummaryLogEntry : LogEntry
	{
	    /// <summary>
	    /// 
	    /// </summary>
	    public string? App { get; set; }
	    /// <summary>
	    /// The page visited
	    /// </summary>
	    public string? Page { get; set; }
	    /// <summary>
	    /// 
	    /// </summary>
	    public DateOnly? Date { get; set; }
	    /// <summary>
	    /// 
	    /// </summary>
	    public UserId UserId { get; set; }
	    /// <summary>
	    /// The number of 
	    /// </summary>
	    public long? Count { get; set; }
	    
	    public override LogEntryType EntryType => LogEntryType.Metrics;
	}
	
	
	public class RequestLogEntry : LogEntry
	{
	    public int StatusCode { get; set; }
	    public string? Method { get; set; }
	    public string? Route { get; set; }
	    public string? Username { get; set; }
	    public UserId? UserId { get; set; }
	    public override LogEntryType EntryType => throw new NotImplementedException();
	}
	
	
	public class ServerActivityLogEntry : LogEntry
	{
	    public LogId Id { get; set; }
	    public DomainEvent Event { get; set; }
	    public UserId UserId { get; set; }
	    public IDictionary<string, string>? Details { get; set; } 
	    public DateTimeOffset? Timestamp { get; set; }
	    public string? Version { get; set; }
	    public override LogEntryType EntryType => LogEntryType.Audit;
	}
	
	
	public class ServerErrorLogEntry : LogEntry
	{
	    public LogId Id { get; set; }
	    public string? App { get; set; }
	    public string? Message { get; set; }
	    public UserId UserId { get; set; }
	    public DateTimeOffset? Timestamp { get; set; }
	    public override LogEntryType EntryType => LogEntryType.Error;
	}
	
	
	public class Ticket : AuditableEntity
	{
	}
	
	#endregion
	#region \Domains\Support\ValueObjects
	
	public enum LogEntryType
	{
	    Error,
	    Metrics,
	    Audit
	}
	
	
	public enum LogErrorSeverity
	{
	}
	
	
	[StronglyTypedId(Template.String)]
	public partial struct LogId
	{
	}
	
	
	[StronglyTypedId]
	public partial struct TicketId
	{
	}
	
	#endregion
	#region \Exceptions
	
	/*
	 * 1000 - 1999: Infrastructure Layer
	 * 2000 - 2999: Application Layer
	 * 3000 - 3999: Presentation Layer
	 * 4000 >     : Domain Layer 
	 */
	
	/// <summary>
	/// The origin of the exception.
	/// </summary>
	public enum MintErrorOrigin
	{
	    Unknown = 0,
	    Infrastructure = 1000,
	    Application = 2000,
	    Domain = 4000
	}
	
	
	public enum MintErrorCode
	{
	    #region Infrastructure
	    // Infrastructure: Data 
	    DbUnhandledError = MintErrorOrigin.Infrastructure,
	    DbConnectionFailure,
	    DbQueryTimeout,
	    DbIntegrityViolation, // Primary Key Constraint, Foreign Key Constraint
	
	    // Infrastructure: Messaging
	    MessagingGeneralError,
	    MessagingConnectionFailure,
	    MessageSendFailure,
	    MessageProcessingFailure,
	
	    // Infrastructure: Storage
	    StorageConnectionFailure,
	    #endregion
	
	    #region Application Errors
	    NotAuthorized = MintErrorOrigin.Application,
	    #endregion
	
	    #region Domain Errors
	    NotFound = MintErrorOrigin.Domain,
	    InvalidRequest,
	
	
	
	
	
	
	
	    Unknown = 4999,
	    Internal,
	
	    PatientNotFound,
	    OrganizationNotFound,
	    OrganizationAccessUnauthorized
	    #endregion
	}
	
	public class MintException : Exception
	{
	    protected MintException(string message, MintErrorCode errorCode)
	        : base(message) 
	    {
	        Code = errorCode;
	    }
	
	    protected MintException(string message, MintErrorCode errorCode, Exception? innerException)
	        : base(message, innerException)
	    {
	        Code = errorCode;
	    }
	
	    /// <summary>
	    /// The unique code for the exception.
	    /// </summary>
	    public MintErrorCode Code { get; }
	    /// <summary>
	    /// The origin of the exception.
	    /// </summary>
	    public MintErrorOrigin Origin => (MintErrorOrigin)(((int)Code / 1000) * 1000);
	
	
	
	
	    public static MintException Unknown(string message, Exception? innerException = null) =>
	        new MintException(message, MintErrorCode.Unknown, innerException);
	    public static MintException InvalidRequest(string message) =>
	        new MintException(message, MintErrorCode.InvalidRequest);
	
	}
	
	#endregion
	#region \Extensions
	
	public static class MintExceptionsExtensions
	{
	    public static string ToSnakeCasing(this MintErrorCode errorCode, bool lowercase = true)
	    {
	        return ConvertToSnakeCasing(errorCode.ToString());
	    }
	
	    public static string ToSnakeCasing(this MintErrorOrigin errorOrigin, bool lowercase = true)
	    {
	        return ConvertToSnakeCasing(errorOrigin.ToString());
	    }
	
	    private static string ConvertToSnakeCasing(string input) =>
	            string.Concat(input.Select((c, i) => i > 0 && char.IsUpper(c) ? "_" + c : c.ToString().ToUpper()));
	}
	
	#endregion
	#region \obj\Debug\net8.0
	#endregion
	#region \Properties
	#endregion
}
#endregion
