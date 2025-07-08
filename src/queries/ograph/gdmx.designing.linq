<Query Kind="Program">
  <Namespace>System.Xml.Serialization</Namespace>
</Query>

const string path = @"C:\Source\repos\assimalign\ograph\.designing\gdmx\gdmx.example.2.xml";

void Main()
{
	var serializer = new XmlSerializer(typeof(Gdm));
	
	using var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
	
	var model = (Gdm)serializer.Deserialize(stream);
	
	model.Dump();
}


[XmlRoot("Gdmx")]
public class Gdm
{
	[XmlAttribute("Version")]
	public string Version { get; set; }
	
	[XmlElement("Graph")]
	public List<GdmGraph> Graphs { get; set; }
}


public class GdmGraph
{
	[XmlAttribute("Namespace")]
	public string Namespace {get; set; }
	
	[XmlElement("Edge")]
	public List<GdmEdge> Edges { get; set; }
	
	[XmlElement("Vertex")]
	public List<GdmVertex> Vertices { get; set; }
}


public class GdmElement
{
	[XmlAttribute("Label")]
	public string Label { get; set; }
}


public class GdmEdge : GdmElement
{
	[XmlAttribute("Source")]
	public string Source { get; set; }
	
	
	[XmlAttribute("Target")]
	public string Target { get; set; }
}

public class GdmVertex : GdmElement
{
	
}