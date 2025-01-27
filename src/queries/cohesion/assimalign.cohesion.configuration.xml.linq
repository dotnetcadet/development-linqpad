<Query Kind="Program">
<Namespace>System</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>System.IO</Namespace>
<Namespace>Microsoft.Extensions.Configuration.Xml</Namespace>
<Namespace>Microsoft.Extensions.FileProviders</Namespace>
<Namespace>System.Runtime.Versioning</Namespace>
<Namespace>System.Security.Cryptography.Xml</Namespace>
<Namespace>System.Xml</Namespace>
<Namespace>System.Globalization</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>System.Reflection</Namespace>
</Query>
#load ".\assimalign.cohesion.configuration.fileextensions"
#load ".\assimalign.cohesion.configuration"
#load ".\assimalign.cohesion.core"

void Main()
{

}

#region Assimalign.Cohesion.Configuration.Xml(net8.0)
namespace Assimalign.Cohesion.Configuration.Xml
{
	#region \
	internal sealed class XmlConfigurationElement
	{
	    public string ElementName { get; }
	    public string? Name { get; }
	    public string SiblingName { get; }
	    public IDictionary<string, List<XmlConfigurationElement>>? ChildrenBySiblingName { get; set; }
	    public XmlConfigurationElement? SingleChild { get; set; }
	    public XmlConfigurationElementTextContent? TextContent { get; set; }
	    public List<XmlConfigurationElementAttributeValue>? Attributes { get; set; }
	    public XmlConfigurationElement(string elementName, string? name)
	    {
	        ThrowHelper.ThrowIfNull(elementName);
	        ElementName = elementName;
	        Name = name;
	        SiblingName = string.IsNullOrEmpty(Name) ? ElementName : ElementName + ":" + Name;
	    }
	}
	internal sealed class XmlConfigurationElementAttributeValue
	{
	    public XmlConfigurationElementAttributeValue(string attribute, string value, int? lineNumber, int? linePosition)
	    {
	        ThrowHelper.ThrowIfNull(attribute);
	        ThrowHelper.ThrowIfNull(value);
	        Attribute = attribute;
	        Value = value;
	        LineNumber = lineNumber;
	        LinePosition = linePosition;
	    }
	    public string Attribute { get; }
	    public string Value { get; }
	    public int? LineNumber { get; }
	    public int? LinePosition { get; }
	}
	internal sealed class XmlConfigurationElementTextContent
	{
	    public XmlConfigurationElementTextContent(string textContent, int? linePosition, int? lineNumber)
	    {
	        ThrowHelper.ThrowIfNull(textContent);
	        TextContent = textContent;
	        LineNumber = lineNumber;
	        LinePosition = linePosition;
	    }
	    public string TextContent { get; }
	    public int? LineNumber { get; }
	    public int? LinePosition { get; }
	}
	public static class XmlConfigurationExtensions
	{
	    public static IConfigurationBuilder AddXmlFile(this IConfigurationBuilder builder, string path)
	    {
	        return AddXmlFile(builder, provider: null, path: path, optional: false, reloadOnChange: false);
	    }
	    public static IConfigurationBuilder AddXmlFile(this IConfigurationBuilder builder, string path, bool optional)
	    {
	        return AddXmlFile(builder, provider: null, path: path, optional: optional, reloadOnChange: false);
	    }
	    public static IConfigurationBuilder AddXmlFile(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange)
	    {
	        return AddXmlFile(builder, provider: null, path: path, optional: optional, reloadOnChange: reloadOnChange);
	    }
	    public static IConfigurationBuilder AddXmlFile(this IConfigurationBuilder builder, IFileProvider? provider, string path, bool optional, bool reloadOnChange)
	    {
	        ThrowHelper.ThrowIfNull(builder);
	        if (string.IsNullOrEmpty(path))
	        {
	            throw new ArgumentException(SR.Error_InvalidFilePath, nameof(path));
	        }
	        return builder.AddXmlFile(s =>
	        {
	            s.FileProvider = provider;
	            s.Path = path;
	            s.Optional = optional;
	            s.ReloadOnChange = reloadOnChange;
	            s.ResolveFileProvider();
	        });
	    }
	    public static IConfigurationBuilder AddXmlFile(this IConfigurationBuilder builder, Action<XmlConfigurationSource>? configureSource)
	        => builder.Add(configureSource);
	    public static IConfigurationBuilder AddXmlStream(this IConfigurationBuilder builder, Stream stream)
	    {
	        ThrowHelper.ThrowIfNull(builder);
	        return builder.Add<XmlStreamConfigurationSource>(s => s.Stream = stream);
	    }
	}
	    public class XmlConfigurationProvider : FileConfigurationProvider
	    {
	        public XmlConfigurationProvider(XmlConfigurationSource source) : base(source) { }
	        internal XmlDocumentDecryptor Decryptor { get; set; } = XmlDocumentDecryptor.Instance;
	        public override void Load(Stream stream)
	        {
	            Data = XmlStreamConfigurationProvider.Read(stream, Decryptor);
	        }
	    }
	    public class XmlConfigurationSource : FileConfigurationSource
	    {
	        public override IConfigurationProvider Build(IConfigurationBuilder builder)
	        {
	            EnsureDefaults(builder);
	            return new XmlConfigurationProvider(this);
	        }
	    }
	    public class XmlDocumentDecryptor
	    {
	        public static readonly XmlDocumentDecryptor Instance = new XmlDocumentDecryptor();
	        private readonly Func<XmlDocument, EncryptedXml>? _encryptedXmlFactory;
	        // don't create an instance of this directly
	        protected XmlDocumentDecryptor()
	        {
	            // _encryptedXmlFactory stays null by default
	        }
	        // for testing only
	        internal XmlDocumentDecryptor(Func<XmlDocument, EncryptedXml> encryptedXmlFactory)
	        {
	            _encryptedXmlFactory = encryptedXmlFactory;
	        }
	        private static bool ContainsEncryptedData(XmlDocument document)
	        {
	            // EncryptedXml will simply decrypt the document in-place without telling
	            // us that it did so, so we need to perform a check to see if EncryptedXml
	            // will actually do anything. The below check for an encrypted data blob
	            // is the same one that EncryptedXml would have performed.
	            var namespaceManager = new XmlNamespaceManager(document.NameTable);
	            namespaceManager.AddNamespace("enc", "http://www.w3.org/2001/04/xmlenc#");
	            return (document.SelectSingleNode("//enc:EncryptedData", namespaceManager) != null);
	        }
	        public XmlReader CreateDecryptingXmlReader(Stream input, XmlReaderSettings? settings)
	        {
	            // XML-based configurations aren't really all that big, so we can buffer
	            // the whole thing in memory while we determine decryption operations.
	            var memStream = new MemoryStream();
	            input.CopyTo(memStream);
	            memStream.Position = 0;
	            // First, consume the entire XmlReader as an XmlDocument.
	            var document = new XmlDocument();
	            using (var reader = XmlReader.Create(memStream, settings))
	            {
	                document.Load(reader);
	            }
	            memStream.Position = 0;
	            if (ContainsEncryptedData(document))
	            {
	                // DecryptDocumentAndCreateXmlReader is not supported on 'browser',
	                // but we only call it depending on the input XML document. If the document
	                // is encrypted and this is running on 'browser', just let the PNSE throw.
	#pragma warning disable CA1416
	                return DecryptDocumentAndCreateXmlReader(document);
	#pragma warning restore CA1416
	            }
	            else
	            {
	                // If no decryption would have taken place, return a new fresh reader
	                // based on the memory stream (which doesn't need to be disposed).
	                return XmlReader.Create(memStream, settings);
	            }
	        }
	        [UnsupportedOSPlatform("browser")]
	        protected virtual XmlReader DecryptDocumentAndCreateXmlReader(XmlDocument document)
	        {
	            // Perform the actual decryption step, updating the XmlDocument in-place.
	            EncryptedXml encryptedXml = _encryptedXmlFactory?.Invoke(document) ?? new EncryptedXml(document);
	            encryptedXml.DecryptDocument();
	            // Finally, return the new XmlReader from the updated XmlDocument.
	            // Error messages based on this XmlReader won't show line numbers,
	            // but that's fine since we transformed the document anyway.
	            return document.CreateNavigator()!.ReadSubtree();
	        }
	    }
	    public class XmlStreamConfigurationProvider : StreamConfigurationProvider
	    {
	        private const string NameAttributeKey = "Name";
	        public XmlStreamConfigurationProvider(XmlStreamConfigurationSource source) : base(source) { }
	        public static IDictionary<string, string?> Read(Stream stream, XmlDocumentDecryptor decryptor)
	        {
	            var readerSettings = new XmlReaderSettings()
	            {
	                CloseInput = false, // caller will close the stream
	                DtdProcessing = DtdProcessing.Prohibit,
	                IgnoreComments = true,
	                IgnoreWhitespace = true
	            };
	            XmlConfigurationElement? root = null;
	            using (XmlReader reader = decryptor.CreateDecryptingXmlReader(stream, readerSettings))
	            {
	                // keep track of the tree we followed to get where we are (breadcrumb style)
	                var currentPath = new Stack<XmlConfigurationElement>();
	                XmlNodeType preNodeType = reader.NodeType;
	                while (reader.Read())
	                {
	                    switch (reader.NodeType)
	                    {
	                        case XmlNodeType.Element:
	                            var element = new XmlConfigurationElement(reader.LocalName, GetName(reader));
	                            if (currentPath.Count == 0)
	                            {
	                                root = element;
	                            }
	                            else
	                            {
	                                var parent = currentPath.Peek();
	                                // If parent already has a dictionary of children, update the collection accordingly
	                                if (parent.ChildrenBySiblingName != null)
	                                {
	                                    // check if this element has appeared before, elements are considered siblings if their SiblingName properties match
	                                    if (!parent.ChildrenBySiblingName.TryGetValue(element.SiblingName, out var siblings))
	                                    {
	                                        siblings = new List<XmlConfigurationElement>();
	                                        parent.ChildrenBySiblingName.Add(element.SiblingName, siblings);
	                                    }
	                                    siblings.Add(element);
	                                }
	                                else
	                                {
	                                    // Performance optimization: parents with a single child don't even initialize a dictionary
	                                    if (parent.SingleChild == null)
	                                    {
	                                        parent.SingleChild = element;
	                                    }
	                                    else
	                                    {
	                                        // If we encounter a second child after assigning "SingleChild", we clear SingleChild and initialize the dictionary
	                                        var children = new Dictionary<string, List<XmlConfigurationElement>>(StringComparer.OrdinalIgnoreCase);
	                                        // Special case: the first and second child have the same sibling name
	                                        if (string.Equals(parent.SingleChild.SiblingName, element.SiblingName, StringComparison.OrdinalIgnoreCase))
	                                        {
	                                            children.Add(element.SiblingName, new List<XmlConfigurationElement>
	                                            {
	                                                parent.SingleChild,
	                                                element
	                                            });
	                                        }
	                                        else
	                                        {
	                                            children.Add(parent.SingleChild.SiblingName, new List<XmlConfigurationElement> { parent.SingleChild });
	                                            children.Add(element.SiblingName, new List<XmlConfigurationElement> { element });
	                                        }
	                                        parent.ChildrenBySiblingName = children;
	                                        parent.SingleChild = null;
	                                    }
	                                }
	                            }
	                            currentPath.Push(element);
	                            ReadAttributes(reader, element);
	                            // If current element is self-closing
	                            if (reader.IsEmptyElement)
	                            {
	                                currentPath.Pop();
	                            }
	                            break;
	                        case XmlNodeType.EndElement:
	                            if (currentPath.Count != 0)
	                            {
	                                XmlConfigurationElement parent = currentPath.Pop();
	                                // If this EndElement node comes right after an Element node,
	                                // it means there is no text/CDATA node in current element
	                                if (preNodeType == XmlNodeType.Element)
	                                {
	                                    var lineInfo = reader as IXmlLineInfo;
	                                    var lineNumber = lineInfo?.LineNumber;
	                                    var linePosition = lineInfo?.LinePosition;
	                                    parent.TextContent = new XmlConfigurationElementTextContent(string.Empty, lineNumber, linePosition);
	                                }
	                            }
	                            break;
	                        case XmlNodeType.CDATA:
	                        case XmlNodeType.Text:
	                            if (currentPath.Count != 0)
	                            {
	                                var lineInfo = reader as IXmlLineInfo;
	                                var lineNumber = lineInfo?.LineNumber;
	                                var linePosition = lineInfo?.LinePosition;
	                                XmlConfigurationElement parent = currentPath.Peek();
	                                parent.TextContent = new XmlConfigurationElementTextContent(reader.Value, lineNumber, linePosition);
	                            }
	                            break;
	                        case XmlNodeType.XmlDeclaration:
	                        case XmlNodeType.ProcessingInstruction:
	                        case XmlNodeType.Comment:
	                        case XmlNodeType.Whitespace:
	                            // Ignore certain types of nodes
	                            break;
	                        default:
	                            throw new FormatException(SR.Format(SR.Error_UnsupportedNodeType, reader.NodeType, GetLineInfo(reader)));
	                    }
	                    preNodeType = reader.NodeType;
	                    // If this element is a self-closing element,
	                    // we pretend that we just processed an EndElement node
	                    // because a self-closing element contains an end within itself
	                    if (preNodeType == XmlNodeType.Element && reader.IsEmptyElement)
	                    {
	                        preNodeType = XmlNodeType.EndElement;
	                    }
	                }
	            }
	            return ProvideConfiguration(root);
	        }
	        public override void Load(Stream stream)
	        {
	            Data = Read(stream, XmlDocumentDecryptor.Instance);
	        }
	        private static string GetLineInfo(XmlReader reader)
	        {
	            var lineInfo = reader as IXmlLineInfo;
	            return lineInfo == null ? string.Empty :
	                SR.Format(SR.Msg_LineInfo, lineInfo.LineNumber, lineInfo.LinePosition);
	        }
	        private static void ReadAttributes(XmlReader reader, XmlConfigurationElement element)
	        {
	            if (reader.AttributeCount > 0)
	            {
	                element.Attributes = new List<XmlConfigurationElementAttributeValue>();
	            }
	            var lineInfo = reader as IXmlLineInfo;
	            for (int i = 0; i < reader.AttributeCount; i++)
	            {
	                reader.MoveToAttribute(i);
	                var lineNumber = lineInfo?.LineNumber;
	                var linePosition = lineInfo?.LinePosition;
	                // If there is a namespace attached to current attribute
	                if (!string.IsNullOrEmpty(reader.NamespaceURI))
	                {
	                    throw new FormatException(SR.Format(SR.Error_NamespaceIsNotSupported, GetLineInfo(reader)));
	                }
	                element.Attributes!.Add(new XmlConfigurationElementAttributeValue(reader.LocalName, reader.Value, lineNumber, linePosition));
	            }
	            // Go back to the element containing the attributes we just processed
	            reader.MoveToElement();
	        }
	        // The special attribute "Name" only contributes to prefix
	        // This method retrieves the Name of the element, if the attribute is present
	        // Unfortunately XmlReader.GetAttribute cannot be used, as it does not support looking for attributes in a case insensitive manner
	        private static string? GetName(XmlReader reader)
	        {
	            string? name = null;
	            while (reader.MoveToNextAttribute())
	            {
	                if (string.Equals(reader.LocalName, NameAttributeKey, StringComparison.OrdinalIgnoreCase))
	                {
	                    // If there is a namespace attached to current attribute
	                    if (!string.IsNullOrEmpty(reader.NamespaceURI))
	                    {
	                        throw new FormatException(SR.Format(SR.Error_NamespaceIsNotSupported, GetLineInfo(reader)));
	                    }
	                    name = reader.Value;
	                    break;
	                }
	            }
	            // Go back to the element containing the name we just processed
	            reader.MoveToElement();
	            return name;
	        }
	        private static IDictionary<string, string?> ProvideConfiguration(XmlConfigurationElement? root)
	        {
	            Dictionary<string, string?> configuration = new(StringComparer.OrdinalIgnoreCase);
	            if (root == null)
	            {
	                return configuration;
	            }
	            var rootPrefix = new Prefix();
	            // The root element only contributes to the prefix via its Name attribute
	            if (!string.IsNullOrEmpty(root.Name))
	            {
	                rootPrefix.Push(root.Name);
	            }
	            ProcessElementAttributes(rootPrefix, root);
	            ProcessElementContent(rootPrefix, root);
	            ProcessElementChildren(rootPrefix, root);
	            return configuration;
	            void ProcessElement(Prefix prefix, XmlConfigurationElement element)
	            {
	                ProcessElementAttributes(prefix, element);
	                ProcessElementContent(prefix, element);
	                ProcessElementChildren(prefix, element);
	            }
	            void ProcessElementAttributes(Prefix prefix, XmlConfigurationElement element)
	            {
	                // Add attributes to configuration values
	                if (element.Attributes != null)
	                {
	                    for (var i = 0; i < element.Attributes.Count; i++)
	                    {
	                        var attribute = element.Attributes[i];
	                        prefix.Push(attribute.Attribute);
	                        AddToConfiguration(prefix.AsString, attribute.Value, attribute.LineNumber, attribute.LinePosition);
	                        prefix.Pop();
	                    }
	                }
	            }
	            void ProcessElementContent(Prefix prefix, XmlConfigurationElement element)
	            {
	                // Add text content to configuration values
	                if (element.TextContent != null)
	                {
	                    var textContent = element.TextContent;
	                    AddToConfiguration(prefix.AsString, textContent.TextContent, textContent.LineNumber, textContent.LinePosition);
	                }
	            }
	            void ProcessElementChildren(Prefix prefix, XmlConfigurationElement element)
	            {
	                if (element.SingleChild != null)
	                {
	                    var child = element.SingleChild;
	                    ProcessElementChild(prefix, child, null);
	                    return;
	                }
	                if (element.ChildrenBySiblingName == null)
	                {
	                    return;
	                }
	                // Recursively walk through the children of this element
	                foreach (var childrenWithSameSiblingName in element.ChildrenBySiblingName.Values)
	                {
	                    if (childrenWithSameSiblingName.Count == 1)
	                    {
	                        var child = childrenWithSameSiblingName[0];
	                        ProcessElementChild(prefix, child, null);
	                    }
	                    else
	                    {
	                        // Multiple children with the same sibling name. Add the current index to the prefix
	                        for (int i = 0; i < childrenWithSameSiblingName.Count; i++)
	                        {
	                            var child = childrenWithSameSiblingName[i];
	                            ProcessElementChild(prefix, child, i);
	                        }
	                    }
	                }
	            }
	            void ProcessElementChild(Prefix prefix, XmlConfigurationElement child, int? index)
	            {
	                // Add element name to prefix
	                prefix.Push(child.ElementName);
	                // Add value of name attribute to prefix
	                var hasName = !string.IsNullOrEmpty(child.Name);
	                if (hasName)
	                {
	                    prefix.Push(child.Name);
	                }
	                // Add index to the prefix
	                if (index != null)
	                {
	                    prefix.Push(index.Value.ToString(CultureInfo.InvariantCulture));
	                }
	                ProcessElement(prefix, child);
	                // Remove index
	                if (index != null)
	                {
	                    prefix.Pop();
	                }
	                // Remove 'Name' attribute
	                if (hasName)
	                {
	                    prefix.Pop();
	                }
	                // Remove element name
	                prefix.Pop();
	            }
	            void AddToConfiguration(string key, string value, int? lineNumber, int? linePosition)
	            {
	#if NETSTANDARD2_1
	                if (!configuration.TryAdd(key, value))
	                {
	                    var lineInfo = lineNumber == null || linePosition == null
	                        ? string.Empty
	                        : SR.Format(SR.Msg_LineInfo, lineNumber.Value, linePosition.Value);
	                    throw new FormatException(SR.Format(SR.Error_KeyIsDuplicated, key, lineInfo));
	                }
	#else
	                if (configuration.ContainsKey(key))
	                {
	                    var lineInfo = lineNumber == null || linePosition == null
	                        ? string.Empty
	                        : SR.Format(SR.Msg_LineInfo, lineNumber.Value, linePosition.Value);
	                    throw new FormatException(SR.Format(SR.Error_KeyIsDuplicated, key, lineInfo));
	                }
	                configuration.Add(key, value);
	#endif
	            }
	        }
	    }
	    internal sealed class Prefix
	    {
	        private readonly StringBuilder _sb;
	        private readonly Stack<int> _lengths;
	        public Prefix()
	        {
	            _sb = new StringBuilder();
	            _lengths = new Stack<int>();
	        }
	        public string AsString => _sb.ToString();
	        public void Push(string value)
	        {
	            if (_sb.Length != 0)
	            {
	                _sb.Append(ConfigurationPath.KeyDelimiter);
	                _sb.Append(value);
	                _lengths.Push(value.Length + ConfigurationPath.KeyDelimiter.Length);
	            }
	            else
	            {
	                _sb.Append(value);
	                _lengths.Push(value.Length);
	            }
	        }
	        public void Pop()
	        {
	            var length = _lengths.Pop();
	            _sb.Remove(_sb.Length - length, length);
	        }
	    }
	public class XmlStreamConfigurationSource : StreamConfigurationSource
	{
	    public override IConfigurationProvider Build(IConfigurationBuilder builder)
	        => new XmlStreamConfigurationProvider(this);
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
}
#endregion
