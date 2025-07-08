<Query Kind="Program">
  <NuGetReference>Microsoft.OpenApi.Readers</NuGetReference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>Microsoft.OpenApi.Models</Namespace>
</Query>

using Microsoft.OpenApi.Readers;

public class CodeGeneratorOptions
{
	public string Directory { get; set; }
	public string RootNamespace { get; set; }
	public string TypePrefix { get; set; } = "";
	public string PathOfModels { get; set; } = "Models";
	public string PathOfAbstractions { get; set; } = "Abstractions";

	public bool CleanDirectory { get; set; } = true; // removes all the files in the directory before creating new files

	public IList<string> UsingStatments { get; } = new List<string>()
	{
		"System",
		"System.Threading",
		"System.Threading.Tasks",
		"System.Collections",
		"System.Collections.Generic"
	};

	public IList<string> ExcludeParameters { get; } = new List<string>();
	public IList<Func<string, string>> ParameterNameRules { get; } = new List<Func<string, string>>();
	public IList<Func<string, string>> InterfaceNameRules { get; } = new List<Func<string, string>>();
	public IList<Func<string, string>> ClassNameRules { get; } = new List<Func<string, string>>();
	public IList<Func<string, string>> PropertyNameRules { get; } = new List<Func<string, string>>();
}

async Task Main()
{
	var options = new CodeGeneratorOptions()
	{
		Directory = @"",
		RootNamespace = "TxEdX.EdFi",
		TypePrefix = "EdFi"
	};
	options.PropertyNameRules.Add(propertyName => {
		if (propertyName =="_etag")
		{
			return "ETag";
		}
		return propertyName;
	});
	options.ClassNameRules.Add(value =>
	{
		var current = value
			.Replace("edfi_", "", StringComparison.CurrentCultureIgnoreCase)
			.Replace("Tpdm_", "", StringComparison.CurrentCultureIgnoreCase);

		if (value.StartsWith("Tpdm_"))
		{
			return $"Tpdm{ConvertFirstCharToUpper(current)}";
		}

		return ConvertFirstCharToUpper(current);
	});
	options.InterfaceNameRules.Add(value =>
	{
		return value.Replace("Ed-fi", "", StringComparison.CurrentCultureIgnoreCase);
	});

	options.ExcludeParameters.Add("totalCount");
	options.ParameterNameRules.Add(parameterName =>{
		return parameterName.Replace("-", "");
	});

	using var client = new HttpClient();
	using var request = new HttpRequestMessage()
	{
		RequestUri = new Uri(""),
		Method = HttpMethod.Get
	};

	var response = await client.SendAsync(request);
	if (response.IsSuccessStatusCode)
	{
		var content = await response.Content.ReadAsStreamAsync();
		var reader = new OpenApiStreamReader();
		var result = await reader.ReadAsync(content);

		await GenerateAsync(result, options);
	}
}

private Task GenerateAsync(ReadResult result, CodeGeneratorOptions options)
{
	return Task.WhenAll(new[]
	{
		GenerateModelsAsync(result, options),
		GenerateAbstractionsAsync(result, options)
	});
}
private Task GenerateModelsAsync(ReadResult result, CodeGeneratorOptions options)
{
	var directory = GenerateDirectory(options, "Models");

	if (options.CleanDirectory)
	{
		foreach (var dirFile in directory.GetFiles())
		{
			dirFile.Delete();
		}
	}

	var schemas = result.OpenApiDocument.Components.Schemas;

	foreach (var schema in schemas)
	{
		var className = schema.Key;

		// Let's run class naming rules, if any
		foreach (var rule in options.ClassNameRules)
		{
			className = rule.Invoke(className);
		}
		var builder = new StringBuilder();
		foreach (var statement in options.UsingStatments)
		{
			builder.Append($"using {statement.Replace("using", "").Replace(";", "")};");
			builder.AppendLine();
		}
		if (options.UsingStatments.Any())
		{
			builder.AppendLine();
		}
		builder.Append($"namespace {string.Join('.', options.RootNamespace.Replace("namespace", "").Replace(";", ""), "Models")};");
		builder.AppendLine();
		builder.AppendLine();
		builder.Append("public class");
		builder.Append($" {options.TypePrefix}{ConvertFirstCharToUpper(className)}");
		builder.AppendLine();
		builder.Append("{");
		builder.AppendLine();
		foreach (var property in schema.Value.Properties)
		{
			var propertyName = property.Key;

			// Let's run property naming rules, if any
			foreach (var rule in options.PropertyNameRules)
			{
				propertyName = rule.Invoke(propertyName);
			}
			builder.Append($"	/// <summary>");
			builder.AppendLine();
			builder.Append($"	/// {property.Value.Description}");
			builder.AppendLine();
			builder.Append($"	/// </summary>");
			builder.AppendLine();
			if (property.Value.Deprecated)
			{
				builder.Append("	[Obsolete]");
				builder.AppendLine();
			}
			builder.Append($"	public {GetPropertyType(options, schema.Value, property.Value, property.Key)} {ConvertFirstCharToUpper(propertyName)} {{ get; set; }}");
			builder.AppendLine();
		}

		builder.Append("}");
		builder.AppendLine();

		var filePath = Path.Join(directory.FullName, $"{options.TypePrefix}{ConvertFirstCharToUpper(className)}.cs");
		using var file = File.Create(filePath);

		var buffer = Encoding.UTF8.GetBytes(builder.ToString());

		file.Write(buffer, 0, buffer.Length);
		file.Close();
	}

	return Task.CompletedTask;
}
private Task GenerateAbstractionsAsync(ReadResult result, CodeGeneratorOptions options)
{
	var directory1 = GenerateDirectory(options, "Abstractions");
	var directory2 = GenerateDirectory(options, "Internal");

	#region Generate Core Types
	var file = default(FileStream);
	var buffer = default(byte[]);
	var builder = new StringBuilder();

	builder.Append($"namespace {options.RootNamespace.Replace("namespace", "").Replace(";", "")};");
	builder.AppendLine();
	builder.AppendLine();
	builder.Append($"public interface I{options.TypePrefix}ClientResult");
	builder.AppendLine();
	builder.Append("{");
	builder.AppendLine();
	builder.Append("	bool IsSuccess { get; }");
	builder.AppendLine();
	builder.Append($"	I{options.TypePrefix}ClientError Error {{ get; }}");
	builder.AppendLine();
	builder.Append("}");
	builder.AppendLine();
	builder.AppendLine();
	builder.Append($"public interface I{options.TypePrefix}ClientResult<T> : I{options.TypePrefix}ClientResult");
	builder.AppendLine();
	builder.Append("{");
	builder.AppendLine();
	builder.Append("	T Data { get; }");
	builder.AppendLine();
	builder.Append("}");

	buffer = Encoding.UTF8.GetBytes(builder.ToString());
	file = File.Create(Path.Join(directory1.FullName, $"I{options.TypePrefix}ClientResult.cs"));
	file.Write(buffer, 0, buffer.Length);
	file.Close();
	file.Dispose();

	builder.Clear();
	builder.Append($"using System.Collections.Generic;");
	builder.AppendLine();
	builder.AppendLine();
	builder.Append($"namespace {options.RootNamespace.Replace("namespace", "").Replace(";", "")};");
	builder.AppendLine();
	builder.AppendLine();
	builder.Append($"public interface I{options.TypePrefix}ClientCollectionResult<T> : I{options.TypePrefix}ClientResult");
	builder.AppendLine();
	builder.Append("{");
	builder.AppendLine();
	builder.Append("	long Total { get; }");
	builder.AppendLine();
	builder.Append("	IEnumerable<T> Data { get; }");
	builder.AppendLine();
	builder.Append("}");
	builder.AppendLine();

	buffer = Encoding.UTF8.GetBytes(builder.ToString());
	file = File.Create(Path.Join(directory1.FullName, $"I{options.TypePrefix}ClientResult.Collection.cs"));
	file.Write(buffer, 0, buffer.Length);
	file.Close();
	file.Dispose();

	builder.Clear();
	builder.Append($"namespace {options.RootNamespace.Replace("namespace", "").Replace(";", "")};");
	builder.AppendLine();
	builder.AppendLine();
	builder.Append($"public interface I{options.TypePrefix}ClientError");
	builder.AppendLine();
	builder.Append("{");
	builder.AppendLine();
	builder.Append("	string Code { get; }");
	builder.AppendLine();
	builder.Append("	string Message { get; }");
	builder.AppendLine();
	builder.Append("}");
	builder.AppendLine();

	buffer = Encoding.UTF8.GetBytes(builder.ToString());
	file = File.Create(Path.Join(directory1.FullName, $"I{options.TypePrefix}ClientError.cs"));
	file.Write(buffer, 0, buffer.Length);
	file.Close();
	file.Dispose();


	builder.Clear();
	builder.Append($"using System.Collections.Generic;");
	builder.AppendLine();
	builder.AppendLine();
	builder.Append($"namespace {options.RootNamespace.Replace("namespace", "").Replace(";", "")};");
	builder.AppendLine();
	builder.AppendLine();
	builder.Append($"public interface I{options.TypePrefix}ClientFactory");
	builder.AppendLine();
	builder.Append("{");
	builder.AppendLine();
	builder.Append($"	I{options.TypePrefix}Client Create(string clientName);");
	builder.AppendLine();
	builder.Append("}");
	builder.AppendLine();

	buffer = Encoding.UTF8.GetBytes(builder.ToString());
	file = File.Create(Path.Join(directory1.FullName, $"I{options.TypePrefix}ClientFactory.cs"));
	file.Write(buffer, 0, buffer.Length);
	file.Close();
	file.Dispose();

	var internalDirectory = GenerateDirectory(options, "Internal");

	builder.Clear();
	builder.Append($"namespace {options.RootNamespace.Replace("namespace", "").Replace(";", "")}.Internal;");
	builder.AppendLine();
	builder.AppendLine();
	builder.Append($"internal class {options.TypePrefix}ClientResult<T> : I{options.TypePrefix}ClientResult<T>");
	builder.AppendLine();
	builder.Append("{");
	builder.AppendLine();
	builder.Append("	public bool IsSuccess => Error is null;");
	builder.AppendLine();
	builder.Append("	public T Data { get; init; }");
	builder.AppendLine();
	builder.Append($"	public I{options.TypePrefix}ClientError Error {{ get; init; }}");
	builder.AppendLine();
	builder.Append("}");
	builder.AppendLine();
	builder.AppendLine();
	builder.Append($"internal class {options.TypePrefix}ClientResult : I{options.TypePrefix}ClientResult");
	builder.AppendLine();
	builder.Append("{");
	builder.AppendLine();
	builder.Append("	public bool IsSuccess => Error is null;");
	builder.AppendLine();
	builder.Append($"	public I{options.TypePrefix}ClientError? Error {{ get; init; }}");
	builder.AppendLine();
	builder.Append("}");

	buffer = Encoding.UTF8.GetBytes(builder.ToString());
	file = File.Create(Path.Join(internalDirectory.FullName, $"{options.TypePrefix}ClientResult.cs"));
	file.Write(buffer, 0, buffer.Length);
	file.Close();
	file.Dispose();


	builder.Clear();
	builder.Append($"using System.Collections.Generic;");
	builder.AppendLine();
	builder.AppendLine();
	builder.Append($"namespace {options.RootNamespace.Replace("namespace", "").Replace(";", "")}.Internal;");
	builder.AppendLine();
	builder.AppendLine();
	builder.Append($"internal class {options.TypePrefix}ClientCollectionResult<T> : I{options.TypePrefix}ClientCollectionResult<T>");
	builder.AppendLine();
	builder.Append("{");
	builder.AppendLine();
	builder.Append("	public bool IsSuccess => Error is null;");
	builder.AppendLine();
	builder.Append("	public long Total { get; init; }");
	builder.AppendLine();
	builder.Append("	public IEnumerable<T> Data { get; init; }");
	builder.AppendLine();
	builder.Append($"	public I{options.TypePrefix}ClientError? Error {{ get; init; }}");
	builder.AppendLine();
	builder.Append("}");
	builder.AppendLine();

	buffer = Encoding.UTF8.GetBytes(builder.ToString());
	file = File.Create(Path.Join(internalDirectory.FullName, $"{options.TypePrefix}ClientResult.Collection.cs"));
	file.Write(buffer, 0, buffer.Length);
	file.Close();
	file.Dispose();

	builder.Clear();
	builder.Append($"namespace {options.RootNamespace.Replace("namespace", "").Replace(";", "")}.Internal;");
	builder.AppendLine();
	builder.AppendLine();
	builder.Append($"internal class {options.TypePrefix}ClientError : I{options.TypePrefix}ClientError");
	builder.AppendLine();
	builder.Append("{");
	builder.AppendLine();
	builder.Append("	public string? Code { get; init; }");
	builder.AppendLine();
	builder.Append("	public string? Message { get; init; }");
	builder.AppendLine();
	builder.Append("}");
	builder.AppendLine();

	buffer = Encoding.UTF8.GetBytes(builder.ToString());
	file = File.Create(Path.Join(internalDirectory.FullName, $"{options.TypePrefix}ClientError.cs"));
	file.Write(buffer, 0, buffer.Length);
	file.Close();
	file.Dispose();

	#endregion

	var operations = result.OpenApiDocument.Paths
		.GroupBy(item =>
		{
			var key = string.Empty;
			foreach (var split in item.Key.Trim('/').Split('/'))
			{
				if (!split.Contains('{'))
				{
					key = string.Join('/', key, split);
				}
			}
			return key;
		});

	return Task.WhenAll(
		GenerateClientImplementationAsync(operations, options, result),
		GenerateClientAbstractionAsync(operations, options, result, directory1),
		Task.WhenAll(operations.SelectMany(x =>
		{
			return new Task[]
			{
				GenerateOperationAbstractionsAsync(x, options, result, directory1),
				GenerateOperationImplementationsAsync(x, options, result, directory2),
				
			};
		})));
}
private Task GenerateOperationImplementationsAsync(
	IGrouping<string, KeyValuePair<string, OpenApiPathItem>> item,
	CodeGeneratorOptions options,
	ReadResult result,
	DirectoryInfo directory)
{
	var baseName = string.Concat(item.Key.TrimStart('/').Split('/').Select(item =>
	{
		return ConvertFirstCharToUpper(item);
	}));
	foreach (var rule in options.InterfaceNameRules)
	{
		baseName = rule.Invoke(baseName);
	}

	var requestBuilderName = $"{options.TypePrefix}{baseName}RequestBuilder";
	var requestBuilderCollectionName = $"{options.TypePrefix}{baseName}CollectionRequestBuilder";

	var cbuilder = new StringBuilder(); // Collection Builder Abstraction
	var sbuilder = new StringBuilder(); // Single Builder Abstractions

	// Start Single Request Builder
	foreach (var statement in options.UsingStatments)
	{
		sbuilder.Append($"using {statement.Replace("using", "").Replace(";", "")};");
		sbuilder.AppendLine();
	}
	if (options.UsingStatments.Any())
	{
		sbuilder.AppendLine();
	}
	sbuilder.Append($"namespace {options.RootNamespace.Replace("namespace", "").Replace(";", "")}.Internal;");
	sbuilder.AppendLine();
	sbuilder.AppendLine();
	sbuilder.Append($"using {string.Join('.', options.RootNamespace.Replace("namespace", "").Replace(";", ""), "Models")};");
	sbuilder.AppendLine();
	sbuilder.AppendLine();
	sbuilder.Append($"internal class {requestBuilderName} : {options.TypePrefix}RequestBuilderBase,");
	sbuilder.AppendLine();
	sbuilder.Append($"	I{requestBuilderName}");
	sbuilder.AppendLine();
	sbuilder.Append("{");
	sbuilder.AppendLine();


	// Start Collection Request Builder
	foreach (var statement in options.UsingStatments)
	{
		cbuilder.Append($"using {statement.Replace("using", "").Replace(";", "")};");
		cbuilder.AppendLine();
	}
	if (options.UsingStatments.Any())
	{
		cbuilder.AppendLine();
	}
	cbuilder.Append($"namespace {options.RootNamespace.Replace("namespace", "").Replace(";", "")}.Internal;");
	cbuilder.AppendLine();
	cbuilder.AppendLine();
	cbuilder.Append($"using {string.Join('.', options.RootNamespace.Replace("namespace", "").Replace(";", ""), "Models")};");
	cbuilder.AppendLine();
	cbuilder.AppendLine();
	cbuilder.Append($"internal class {requestBuilderCollectionName} : {options.TypePrefix}RequestBuilderBase,");
	cbuilder.AppendLine();
	cbuilder.Append($"	I{requestBuilderCollectionName}");
	cbuilder.AppendLine();
	cbuilder.Append("{");
	cbuilder.AppendLine();
	cbuilder.Append($$"""
		public {{requestBuilderCollectionName}}()
		{
			Path = "{{item.Key}}";
		}
	""");
	cbuilder.AppendLine();
	cbuilder.Append($$"""
		public I{{requestBuilderName}} this[string id]
		{
			get 
			{
				if (string.IsNullOrEmpty(id))
				{
					throw new ArgumentNullException(nameof(id));
				}
				return new {{requestBuilderName}}
				{
					Options = base.Options,
					Client = base.Client,
					Path = string.Join('/', base.Path, id)
				};
			}
		}
	""");
	cbuilder.AppendLine();

	foreach (var path in item)
	{
		foreach (var operation in path.Value.Operations)
		{
			if (operation.Key == OperationType.Post)
			{
				var inputReferenceName = string.Empty;
				var outputReferenceName = string.Empty;

				// Get Request Body
				if (operation.Value?.RequestBody?.Reference is not null)
				{
					inputReferenceName = operation.Value?.RequestBody?.Reference.Id;
				}
				else if (operation.Value?.RequestBody?.Content is not null)
				{
					var content = operation.Value?.RequestBody?.Content;

					if (content.TryGetValue("application/json", out var mediaType))
					{
						if (mediaType.Schema.Reference is not null && mediaType.Schema.Reference.Id is not null)
						{
							inputReferenceName = mediaType.Schema.Reference.Id;
						}
					}
				}

				// Get Response Model
				var outputReference = operation.Value.Responses
					?.Where(x => x.Key.StartsWith("20"))
					?.Select(x => x.Value.Content)
					?.Where(x => x is not null && x.TryGetValue("application/json", out var mediaType) && mediaType?.Schema?.Reference is not null)
					?.SelectMany(x => x?.Values)
					?.FirstOrDefault()
					?.Schema?.Reference;

				outputReferenceName = outputReference is null ? string.Empty : outputReference.Id;

				foreach (var rule in options.ClassNameRules)
				{
					inputReferenceName = rule.Invoke(inputReferenceName);
					outputReferenceName = rule.Invoke(outputReferenceName);
				}

				inputReferenceName = $"{options.TypePrefix}{ConvertFirstCharToUpper(inputReferenceName)}";
				outputReferenceName = $"{options.TypePrefix}{ConvertFirstCharToUpper(outputReferenceName)}";

				// Let's check if there is an response model
				var output = outputReference is null ? "" : $"<{outputReferenceName}>";
				if (operation.Value.Deprecated)
				{
					cbuilder.Append("	[Obsolete]");
					cbuilder.AppendLine();
				}
				cbuilder.Append($$"""	
					public Task<I{{options.TypePrefix}}ClientResult{{output}}> CreateAsync({{inputReferenceName}} value)
					{
						if (value is null)
						{
							throw new ArgumentNullException(nameof(value));
						}
						return base.CreateAsync(value);
					}
				""");
				cbuilder.AppendLine();

				continue;
				//throw new Exception("Unable to determine reference type for operation");
			}
			if (operation.Key == OperationType.Put)
			{
				var inputReferenceName = string.Empty;
				var outputReferenceName = string.Empty;

				// Get Request Body
				if (operation.Value?.RequestBody?.Reference is not null)
				{
					inputReferenceName = operation.Value?.RequestBody?.Reference.Id;
				}
				else if (operation.Value?.RequestBody?.Content is not null)
				{
					var content = operation.Value?.RequestBody?.Content;

					if (content.TryGetValue("application/json", out var mediaType))
					{
						if (mediaType.Schema.Reference is not null && mediaType.Schema.Reference.Id is not null)
						{
							inputReferenceName = mediaType.Schema.Reference.Id;
						}
					}
				}

				// Get Response Model
				var outputReference = operation.Value.Responses
					?.Where(x => x.Key.StartsWith("20"))
					?.Select(x => x.Value.Content)
					?.Where(x => x is not null && x.TryGetValue("application/json", out var mediaType) && mediaType?.Schema?.Reference is not null)
					?.SelectMany(x => x?.Values)
					?.FirstOrDefault()
					?.Schema?.Reference;

				outputReferenceName = outputReference is null ? string.Empty : outputReference.Id;

				foreach (var rule in options.ClassNameRules)
				{
					inputReferenceName = rule.Invoke(inputReferenceName);
					outputReferenceName = rule.Invoke(outputReferenceName);
				}

				inputReferenceName = $"{options.TypePrefix}{ConvertFirstCharToUpper(inputReferenceName)}";
				outputReferenceName = $"{options.TypePrefix}{ConvertFirstCharToUpper(outputReferenceName)}";

				var output = outputReference is null ? "" : $"<{outputReferenceName}>";
				if (operation.Value.Deprecated)
				{
					sbuilder.Append("	[Obsolete]");
					sbuilder.AppendLine();
				}
				sbuilder.Append($$"""	
					public Task<I{{options.TypePrefix}}ClientResult{{output}}> UpdateAsync({{inputReferenceName}} value)
					{
						if (value == null)
						{
							throw new ArgumentNullException(nameof(value));
						}
						return base.UpdateAsync(value);
					}
				""");
				sbuilder.AppendLine();
				continue;
				//throw new Exception("Unable to determine reference type for operation");
			}
			if (operation.Key == OperationType.Delete)
			{
				if (operation.Value.Deprecated)
				{
					sbuilder.Append("	[Obsolete]");
					sbuilder.AppendLine();
				}
				sbuilder.Append($$"""
					public Task<I{{options.TypePrefix}}ClientResult> DeleteAsync()
					{
						return base.DeleteAsync();
					}
				""");
				sbuilder.AppendLine();

				continue;
				//throw new Exception("Unable to determine reference type for operation");
			}
			if (operation.Key == OperationType.Get)
			{
				var outputSchema = operation.Value.Responses
					?.Where(x => x.Key.StartsWith("20"))
					?.Select(x => x.Value.Content)
					?.Where(x => x is not null && x.TryGetValue("application/json", out var mediaType) && mediaType?.Schema is not null)
					?.SelectMany(x => x?.Values)
					?.FirstOrDefault()
					?.Schema;


				if (outputSchema.Items is not null && outputSchema.Items.Reference is not null)
				{
					var referenceName = outputSchema.Items.Reference.Id;

					foreach (var rule in options.ClassNameRules)
					{
						referenceName = rule.Invoke(referenceName);
					}

					foreach (var parameter in operation.Value.Parameters)
					{
						var parameterName = parameter.Name;
						
						if (parameter.Schema is null || options.ExcludeParameters.Any(x => x.Equals(parameter.Name, StringComparison.CurrentCultureIgnoreCase)))
						{
							// Skip for now
							continue;
						}
						
						foreach (var rule in options.ParameterNameRules)
						{
							parameterName = rule.Invoke(parameterName);
						}
						if (parameter.Deprecated)
						{
							cbuilder.Append("	[Obsolete]");
							cbuilder.AppendLine();
						}
						cbuilder.Append($$"""
							public I{{requestBuilderCollectionName}} Set{{ConvertFirstCharToUpper(parameterName)}}({{GetParameterType(options, parameter.Schema)}} value)
							{
								Query["{{parameter.Name}}"] = value.ToString();
								
								return this;
							}
						""");
						cbuilder.AppendLine();
					}
					if (operation.Value.Deprecated)
					{
						cbuilder.Append("	[Obsolete]");
						cbuilder.AppendLine();
					}
					cbuilder.Append($$"""
						public Task<I{{options.TypePrefix}}ClientCollectionResult<{{options.TypePrefix}}{{ConvertFirstCharToUpper(referenceName)}}>> QueryAsync()
						{
							return base.QueryAsync<{{options.TypePrefix}}{{ConvertFirstCharToUpper(referenceName)}}>();
						}
					""");
					cbuilder.AppendLine();
				}
				else if (outputSchema.Reference is not null)
				{
					var referenceName = outputSchema.Reference.Id;

					foreach (var rule in options.ClassNameRules)
					{
						referenceName = rule.Invoke(referenceName);
					}
					if (operation.Value.Deprecated)
					{
						sbuilder.Append("	[Obsolete]");
						sbuilder.AppendLine();
					}
					sbuilder.Append($$"""
						public Task<I{{options.TypePrefix}}ClientResult<{{options.TypePrefix}}{{ConvertFirstCharToUpper(referenceName)}}>> GetAsync()
						{
							return base.GetAsync<{{options.TypePrefix}}{{ConvertFirstCharToUpper(referenceName)}}>();
						}
					""");
					sbuilder.AppendLine();
				}
				else
				{
					throw new Exception();
				}
			}
		}
	}

	cbuilder.Append("}");
	cbuilder.AppendLine();

	sbuilder.Append("}");
	sbuilder.AppendLine();

	var filePath1 = Path.Join(directory.FullName, $"{requestBuilderName}.cs");
	var filePath2 = Path.Join(directory.FullName, $"{requestBuilderName}.Collection.cs");

	using var file1 = File.Create(filePath1);
	using var file2 = File.Create(filePath2);

	var buffer1 = Encoding.UTF8.GetBytes(sbuilder.ToString());
	var buffer2 = Encoding.UTF8.GetBytes(cbuilder.ToString());

	file1.Write(buffer1, 0, buffer1.Length);
	file1.Close();

	file2.Write(buffer2, 0, buffer2.Length);
	file2.Close();


	return Task.CompletedTask;
}
private Task GenerateOperationAbstractionsAsync(
	IGrouping<string, KeyValuePair<string, OpenApiPathItem>> item,
	CodeGeneratorOptions options,
	ReadResult result,
	DirectoryInfo directory)
{
	var baseName = string.Concat(item.Key.TrimStart('/').Split('/').Select(item =>
	{
		return ConvertFirstCharToUpper(item);
	}));
	foreach (var rule in options.InterfaceNameRules)
	{
		baseName = rule.Invoke(baseName);
	}

	var requestBuilderName = $"I{options.TypePrefix}{baseName}RequestBuilder";
	var requestBuilderCollectionName = $"I{options.TypePrefix}{baseName}CollectionRequestBuilder";

	var cbuilder = new StringBuilder(); // Collection Builder Abstraction
	var sbuilder = new StringBuilder(); // Single Builder Abstractions

	// Start Single Request Builder
	foreach (var statement in options.UsingStatments)
	{
		sbuilder.Append($"using {statement.Replace("using", "").Replace(";", "")};");
		sbuilder.AppendLine();
	}
	if (options.UsingStatments.Any())
	{
		sbuilder.AppendLine();
	}
	sbuilder.Append($"namespace {options.RootNamespace.Replace("namespace", "").Replace(";", "")};");
	sbuilder.AppendLine();
	sbuilder.AppendLine();
	sbuilder.Append($"using {string.Join('.', options.RootNamespace.Replace("namespace", "").Replace(";", ""), "Models")};");
	sbuilder.AppendLine();
	sbuilder.AppendLine();
	sbuilder.Append($"public interface {requestBuilderName}");
	sbuilder.AppendLine();
	sbuilder.Append("{");
	sbuilder.AppendLine();

	// Start Collection Request Builder
	foreach (var statement in options.UsingStatments)
	{
		cbuilder.Append($"using {statement.Replace("using", "").Replace(";", "")};");
		cbuilder.AppendLine();
	}
	if (options.UsingStatments.Any())
	{
		cbuilder.AppendLine();
	}
	cbuilder.Append($"namespace {options.RootNamespace.Replace("namespace", "").Replace(";", "")};");
	cbuilder.AppendLine();
	cbuilder.AppendLine();
	cbuilder.Append($"using {string.Join('.', options.RootNamespace.Replace("namespace", "").Replace(";", ""), "Models")};");
	cbuilder.AppendLine();
	cbuilder.AppendLine();
	cbuilder.Append($"public interface {requestBuilderCollectionName}");
	cbuilder.AppendLine();
	cbuilder.Append("{");
	cbuilder.AppendLine();
	cbuilder.Append($"	{requestBuilderName} this[string id] {{ get; }}");
	cbuilder.AppendLine();
	cbuilder.AppendLine();

	foreach (var path in item)
	{
		foreach (var operation in path.Value.Operations)
		{
			if (operation.Key == OperationType.Post)
			{
				var inputReferenceName = string.Empty;
				var outputReferenceName = string.Empty;

				// Get Request Body
				if (operation.Value?.RequestBody?.Reference is not null)
				{
					inputReferenceName = operation.Value?.RequestBody?.Reference.Id;
				}
				else if (operation.Value?.RequestBody?.Content is not null)
				{
					var content = operation.Value?.RequestBody?.Content;

					if (content.TryGetValue("application/json", out var mediaType))
					{
						if (mediaType.Schema.Reference is not null && mediaType.Schema.Reference.Id is not null)
						{
							inputReferenceName = mediaType.Schema.Reference.Id;
						}
					}
				}

				// Get Response Model
				var outputReference = operation.Value.Responses
					?.Where(x => x.Key.StartsWith("20"))
					?.Select(x => x.Value.Content)
					?.Where(x => x is not null && x.TryGetValue("application/json", out var mediaType) && mediaType?.Schema?.Reference is not null)
					?.SelectMany(x => x?.Values)
					?.FirstOrDefault()
					?.Schema?.Reference;

				outputReferenceName = outputReference is null ? string.Empty : outputReference.Id;

				foreach (var rule in options.ClassNameRules)
				{
					inputReferenceName = rule.Invoke(inputReferenceName);
					outputReferenceName = rule.Invoke(outputReferenceName);
				}

				inputReferenceName = $"{options.TypePrefix}{ConvertFirstCharToUpper(inputReferenceName)}";
				outputReferenceName = $"{options.TypePrefix}{ConvertFirstCharToUpper(outputReferenceName)}";

				// Let's check if there is an response model
				var output = outputReference is null ? "" : $"<{outputReferenceName}>";
				cbuilder.Append($"	/// <summary>");
				cbuilder.AppendLine();
				cbuilder.Append($"	/// {operation.Value.Description}");
				cbuilder.AppendLine();
				cbuilder.Append($"	/// </summary>");
				cbuilder.AppendLine();
				if (operation.Value.Deprecated)
				{
					cbuilder.Append("	[Obsolete]");
					cbuilder.AppendLine();
				}
				cbuilder.Append($"	Task<I{options.TypePrefix}ClientResult{output}> CreateAsync({inputReferenceName} value);");
				cbuilder.AppendLine();

				continue;
				//throw new Exception("Unable to determine reference type for operation");
			}
			if (operation.Key == OperationType.Put)
			{
				var inputReferenceName = string.Empty;
				var outputReferenceName = string.Empty;

				// Get Request Body
				if (operation.Value?.RequestBody?.Reference is not null)
				{
					inputReferenceName = operation.Value?.RequestBody?.Reference.Id;
				}
				else if (operation.Value?.RequestBody?.Content is not null)
				{
					var content = operation.Value?.RequestBody?.Content;

					if (content.TryGetValue("application/json", out var mediaType))
					{
						if (mediaType.Schema.Reference is not null && mediaType.Schema.Reference.Id is not null)
						{
							inputReferenceName = mediaType.Schema.Reference.Id;
						}
					}
				}

				// Get Response Model
				var outputReference = operation.Value.Responses
					?.Where(x => x.Key.StartsWith("20"))
					?.Select(x => x.Value.Content)
					?.Where(x => x is not null && x.TryGetValue("application/json", out var mediaType) && mediaType?.Schema?.Reference is not null)
					?.SelectMany(x => x?.Values)
					?.FirstOrDefault()
					?.Schema?.Reference;

				outputReferenceName = outputReference is null ? string.Empty : outputReference.Id;

				foreach (var rule in options.ClassNameRules)
				{
					inputReferenceName = rule.Invoke(inputReferenceName);
					outputReferenceName = rule.Invoke(outputReferenceName);
				}

				inputReferenceName = $"{options.TypePrefix}{ConvertFirstCharToUpper(inputReferenceName)}";
				outputReferenceName = $"{options.TypePrefix}{ConvertFirstCharToUpper(outputReferenceName)}";

				var output = outputReference is null ? "" : $"<{outputReferenceName}>";
				sbuilder.Append($"	/// <summary>");
				sbuilder.AppendLine();
				sbuilder.Append($"	/// {operation.Value.Description}");
				sbuilder.AppendLine();
				sbuilder.Append($"	/// </summary>");
				sbuilder.AppendLine();
				if (operation.Value.Deprecated)
				{
					sbuilder.Append("	[Obsolete]");
					sbuilder.AppendLine();
				}
				sbuilder.Append($"	Task<I{options.TypePrefix}ClientResult{output}> UpdateAsync({inputReferenceName} value);");
				sbuilder.AppendLine();
				continue;
				//throw new Exception("Unable to determine reference type for operation");
			}
			if (operation.Key == OperationType.Delete)
			{
				sbuilder.Append($"	/// <summary>");
				sbuilder.AppendLine();
				sbuilder.Append($"	/// {operation.Value.Description}");
				sbuilder.AppendLine();
				sbuilder.Append($"	/// </summary>");
				sbuilder.AppendLine();
				if (operation.Value.Deprecated)
				{
					sbuilder.Append("	[Obsolete]");
					sbuilder.AppendLine();
				}
				sbuilder.Append($"	Task<I{options.TypePrefix}ClientResult> DeleteAsync();");
				sbuilder.AppendLine();

				continue;
				//throw new Exception("Unable to determine reference type for operation");
			}
			if (operation.Key == OperationType.Get)
			{
				var outputSchema = operation.Value.Responses
					?.Where(x => x.Key.StartsWith("20"))
					?.Select(x => x.Value.Content)
					?.Where(x => x is not null && x.TryGetValue("application/json", out var mediaType) && mediaType?.Schema is not null)
					?.SelectMany(x => x?.Values)
					?.FirstOrDefault()
					?.Schema;


				if (outputSchema.Items is not null && outputSchema.Items.Reference is not null)
				{
					var referenceName = outputSchema.Items.Reference.Id;

					foreach (var rule in options.ClassNameRules)
					{
						referenceName = rule.Invoke(referenceName);
					}

					foreach (var parameter in operation.Value.Parameters)
					{
						var parameterName = parameter.Name;
						
						if (parameter.Schema is null || options.ExcludeParameters.Any(x => x.Equals(parameter.Name, StringComparison.CurrentCultureIgnoreCase)))
						{
							// Skip for now
							continue;
						}
						foreach (var rule in options.ParameterNameRules)
						{
							parameterName = rule.Invoke(parameterName);
						}
						cbuilder.Append($"	/// <summary>");
						cbuilder.AppendLine();
						cbuilder.Append($"	/// {parameter.Description}");
						cbuilder.AppendLine();
						cbuilder.Append($"	/// </summary>");
						cbuilder.AppendLine();
						if (parameter.Deprecated)
						{
							cbuilder.Append("	[Obsolete]");
							cbuilder.AppendLine();
						}
						cbuilder.Append($"	{requestBuilderCollectionName} Set{ConvertFirstCharToUpper(parameterName)}({GetParameterType(options, parameter.Schema)} value);");
						cbuilder.AppendLine();
					}

					cbuilder.Append($"	/// <summary>");
					cbuilder.AppendLine();
					cbuilder.Append($"	/// {operation.Value.Description}");
					cbuilder.AppendLine();
					cbuilder.Append($"	/// </summary>");
					cbuilder.AppendLine();
					if (operation.Value.Deprecated)
					{
						cbuilder.Append("	[Obsolete]");
						cbuilder.AppendLine();
					}
					cbuilder.Append($"	Task<I{options.TypePrefix}ClientCollectionResult<{options.TypePrefix}{ConvertFirstCharToUpper(referenceName)}>> QueryAsync();");
					cbuilder.AppendLine();
				}
				else if (outputSchema.Reference is not null)
				{
					var referenceName = outputSchema.Reference.Id;

					foreach (var rule in options.ClassNameRules)
					{
						referenceName = rule.Invoke(referenceName);
					}
					sbuilder.Append($"	/// <summary>");
					sbuilder.AppendLine();
					sbuilder.Append($"	/// {operation.Value.Description}");
					sbuilder.AppendLine();
					sbuilder.Append($"	/// </summary>");
					sbuilder.AppendLine();
					if (operation.Value.Deprecated)
					{
						sbuilder.Append("	[Obsolete]");
						sbuilder.AppendLine();
					}
					sbuilder.Append($"	Task<I{options.TypePrefix}ClientResult<{options.TypePrefix}{ConvertFirstCharToUpper(referenceName)}>> GetAsync();");
					sbuilder.AppendLine();
				}
				else
				{
					throw new Exception();
				}
			}
		}
	}

	cbuilder.Append("}");
	cbuilder.AppendLine();

	sbuilder.Append("}");
	sbuilder.AppendLine();

	var filePath1 = Path.Join(directory.FullName, $"{requestBuilderName}.cs");
	var filePath2 = Path.Join(directory.FullName, $"{requestBuilderName}.Collection.cs");

	using var file1 = File.Create(filePath1);
	using var file2 = File.Create(filePath2);

	var buffer1 = Encoding.UTF8.GetBytes(sbuilder.ToString());
	var buffer2 = Encoding.UTF8.GetBytes(cbuilder.ToString());

	file1.Write(buffer1, 0, buffer1.Length);
	file1.Close();

	file2.Write(buffer2, 0, buffer2.Length);
	file2.Close();


	return Task.CompletedTask;
}
private Task GenerateClientAbstractionAsync(
	IEnumerable<IGrouping<string, KeyValuePair<string, OpenApiPathItem>>> grouping,
	CodeGeneratorOptions options,
	ReadResult result,
	DirectoryInfo directory)
{
	var builder = new StringBuilder();

	foreach (var statement in options.UsingStatments)
	{
		builder.Append($"using {statement.Replace("using", "").Replace(";", "")};");
		builder.AppendLine();
	}
	if (options.UsingStatments.Any())
	{
		builder.AppendLine();
	}
	builder.Append($"namespace {options.RootNamespace.Replace("namespace", "").Replace(";", "")};");
	builder.AppendLine();
	builder.AppendLine();
	builder.Append($"public interface I{options.TypePrefix}Client");
	builder.AppendLine();
	builder.Append("{");
	builder.AppendLine();

	foreach (var group in grouping)
	{
		var baseName = string.Concat(group.Key.TrimStart('/').Split('/').Select(item =>
		{
			return ConvertFirstCharToUpper(item);
		}));
		foreach (var rule in options.InterfaceNameRules)
		{
			baseName = rule.Invoke(baseName);
		}

		builder.Append($"	I{options.TypePrefix}{baseName}CollectionRequestBuilder {baseName} {{ get; }}");
		builder.AppendLine();
	}
	
	builder.Append("}");

	var filePath = Path.Join(directory.FullName, $"I{options.TypePrefix}Client.cs");
	using var file = File.Create(filePath);

	var buffer = Encoding.UTF8.GetBytes(builder.ToString());

	file.Write(buffer, 0, buffer.Length);
	file.Close();

	return Task.CompletedTask;
}
private Task GenerateClientImplementationAsync(
	IEnumerable<IGrouping<string, KeyValuePair<string, OpenApiPathItem>>> grouping,
	CodeGeneratorOptions options,
	ReadResult result)
{
	var builder = new StringBuilder();

	foreach (var statement in options.UsingStatments)
	{
		builder.Append($"using {statement.Replace("using", "").Replace(";", "")};");
		builder.AppendLine();
	}
	builder.Append($"using System.Net.Http;");
	builder.AppendLine();
	if (options.UsingStatments.Any())
	{
		builder.AppendLine();
	}
	builder.Append($"namespace {options.RootNamespace.Replace("namespace", "").Replace(";", "")};");
	builder.AppendLine();
	builder.AppendLine();
	builder.Append($"using {options.RootNamespace.Replace("namespace", "").Replace(";", "")}.Internal;");
	builder.AppendLine();
	builder.AppendLine();
	builder.Append($"public sealed class {options.TypePrefix}Client : I{options.TypePrefix}Client");
	builder.AppendLine();
	builder.Append("{");
	builder.AppendLine();
	builder.Append($$"""
	
		private readonly HttpClient client;
		private readonly {{options.TypePrefix}}ClientOptions options;
		
		public {{options.TypePrefix}}Client({{options.TypePrefix}}ClientOptions options, HttpClient client)
		{
			if (options is null)
			{
				throw new ArgumentNullException(nameof(options));
			}
			if (client is null)
			{
				throw new ArgumentNullException(nameof(client));
			}
			this.options = options;
			this.client = client;
		}
		
	""");
	builder.AppendLine();
	foreach (var group in grouping)
	{
		var baseName = string.Concat(group.Key.TrimStart('/').Split('/').Select(item =>
		{
			return ConvertFirstCharToUpper(item);
		}));
		foreach (var rule in options.InterfaceNameRules)
		{
			baseName = rule.Invoke(baseName);
		}

		builder.Append($$"""
			public I{{options.TypePrefix}}{{baseName}}CollectionRequestBuilder {{baseName}} =>
				GetRequestBuilder<{{options.TypePrefix}}{{baseName}}CollectionRequestBuilder>();
		""");
		builder.AppendLine();
	}
	builder.Append($$"""
		private TRequestBuilder GetRequestBuilder<TRequestBuilder>() 
			where TRequestBuilder : {{options.TypePrefix}}RequestBuilderBase, new()
		{
			return new TRequestBuilder()
			{
				Client = this.client,
				Options = this.options
			};
		}
		
	""");
	builder.Append("}");

	var filePath = Path.Join(options.Directory, $"{options.TypePrefix}Client.cs");
	using var file = File.Create(filePath);

	var buffer = Encoding.UTF8.GetBytes(builder.ToString());

	file.Write(buffer, 0, buffer.Length);
	file.Close();

	return Task.CompletedTask;
}


private DirectoryInfo GenerateDirectory(CodeGeneratorOptions options, string path)
{
	var directoryPath = Path.Join(options.Directory, path);
	var directory = Directory.CreateDirectory(directoryPath);
	if (options.CleanDirectory)
	{
		foreach (var dirFile in directory.GetFiles())
		{
			dirFile.Delete();
		}
	}
	return directory;
}
private string GetParameterType(CodeGeneratorOptions options, OpenApiSchema propertySchema)
{
	switch (propertySchema.Type)
	{
		case "string" when propertySchema.Format is not null && propertySchema.Format.Equals("date"):
			{
				return "DateOnly";
			}
		case "string" when propertySchema.Format is not null && propertySchema.Format.Equals("date-time"):
			{
				return "DateTime";
			}
		case "string":
			{
				return "string";
			}
		case "number" when propertySchema.Format is not null && propertySchema.Format.Equals("double"):
			{
				return "double";
			}
		case "number" when propertySchema.Format is not null && propertySchema.Format.Equals("int16"):
			{
				return "short";
			}
		case "number" when propertySchema.Format is not null && propertySchema.Format.Equals("int32"):
			{
				return "int";
			}
		case "number" when propertySchema.Format is not null && propertySchema.Format.Equals("int64"):
			{
				return "long";
			}
		case "number":
			{
				return "int";
			}
		case "integer" when propertySchema.Format is not null && propertySchema.Format.Equals("double"):
			{
				return "double";
			}
		case "integer" when propertySchema.Format is not null && propertySchema.Format.Equals("int16"):
			{
				return "short";
			}
		case "integer" when propertySchema.Format is not null && propertySchema.Format.Equals("int32"):
			{
				return "int";
			}
		case "integer" when propertySchema.Format is not null && propertySchema.Format.Equals("int64"):
			{
				return "long";
			}
		case "integer":
			{
				return "int";
			}
		case "boolean":
			{
				return "bool";
			}
		case "object":
			{
				var className = propertySchema.Reference.Id;

				// Let's run class naming rules, if any
				foreach (var rule in options.ClassNameRules)
				{
					className = rule.Invoke(className);
				}

				return $"{options.TypePrefix}{ConvertFirstCharToUpper(className)}?";
			}
			//		case "array":
			//			{
			//				var arraySchema = propertySchema.Items;
			//
			//				return $"IEnumerable<{GetParameterType(options, objectSchema, arraySchema)}>?";
			//			}
	}

	throw new Exception();
}
private string GetPropertyType(CodeGeneratorOptions options, OpenApiSchema objectSchema, OpenApiSchema propertySchema, string propertName)
{
	switch (propertySchema.Type)
	{
		case "string" when propertySchema.Format is not null && propertySchema.Format.Equals("date"):
			{
				return objectSchema.Required.Contains(propertName) ? "DateOnly" : "DateOnly?";
			}
		case "string" when propertySchema.Format is not null && propertySchema.Format.Equals("date-time"):
			{
				return objectSchema.Required.Contains(propertName) ? "DateTime" : "DateTime?";
			}
		case "string":
			{
				return objectSchema.Required.Contains(propertName) ? "string" : "string?";
			}
		case "number" when propertySchema.Format is not null && propertySchema.Format.Equals("double"):
			{
				return objectSchema.Required.Contains(propertName) ? "double" : "double?";
			}
		case "number" when propertySchema.Format is not null && propertySchema.Format.Equals("int16"):
			{
				return objectSchema.Required.Contains(propertName) ? "short" : "short?";
			}
		case "number" when propertySchema.Format is not null && propertySchema.Format.Equals("int32"):
			{
				return objectSchema.Required.Contains(propertName) ? "int" : "int?";
			}
		case "number" when propertySchema.Format is not null && propertySchema.Format.Equals("int64"):
			{
				return objectSchema.Required.Contains(propertName) ? "long" : "long?";
			}
		case "number":
			{
				return objectSchema.Required.Contains(propertName) ? "int" : "int?";
			}

		case "integer" when propertySchema.Format is not null && propertySchema.Format.Equals("double"):
			{
				return objectSchema.Required.Contains(propertName) ? "double" : "double?";
			}
		case "integer" when propertySchema.Format is not null && propertySchema.Format.Equals("int16"):
			{
				return objectSchema.Required.Contains(propertName) ? "short" : "short?";
			}
		case "integer" when propertySchema.Format is not null && propertySchema.Format.Equals("int32"):
			{
				return objectSchema.Required.Contains(propertName) ? "int" : "int?";
			}
		case "integer" when propertySchema.Format is not null && propertySchema.Format.Equals("int64"):
			{
				return objectSchema.Required.Contains(propertName) ? "long" : "long?";
			}
		case "integer":
			{
				return objectSchema.Required.Contains(propertName) ? "int" : "int?";
			}
		case "boolean":
			{
				return objectSchema.Required.Contains(propertName) ? "bool" : "bool?";
			}
		case "object":
			{
				var className = propertySchema.Reference.Id; ;

				// Let's run class naming rules, if any
				foreach (var rule in options.ClassNameRules)
				{
					className = rule.Invoke(className);
				}

				return $"{options.TypePrefix}{ConvertFirstCharToUpper(className)}?";
			}
		case "array":
			{

				var arraySchema = propertySchema.Items;

				return $"IEnumerable<{GetPropertyType(options, objectSchema, arraySchema, propertName)}>?";
			}
	}

	throw new Exception();
}
private string ConvertFirstCharToUpper(string value)
{
	return new string(value.Select((character, index) => index == 0 ?
		char.ToUpper(character) :
		character
	).ToArray());
}
private string ConvertFirstCharToLower(string value)
{
	return new string(value.Select((character, index) => index == 0 ?
		char.ToLower(character) :
		character
	).ToArray());
}