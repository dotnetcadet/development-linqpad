<Query Kind="Program" />

void Main()
{
	var (slots, attrs) = default(ComponetContext)!;


	new DefineComponent(builder =>
	{

	});

}


#region Virtual DOM
public enum ElementKind
{
	Document,
	Div,
	Form,
	Input,
	Span,
	Attribute,
}

public interface IElement
{
	ElementKind Kind { get; }
}
public interface ICompositeElement : IElement, IEnumerable<IElement>
{
	bool HasChildren { get; }
}
#endregion


public interface IComponentContext
{

}

public abstract class ComponetContext
{
	public string Attributes { get; set; }
	public IElement[] Slots { get; set; }

	public void Deconstruct(out string attrs)
	{
		attrs = Attributes;
	}
	public void Deconstruct(out IElement[] slots)
	{
		slots = Slots;
	}
	public void Deconstruct(out IElement[] slots, out string attrs)
	{
		attrs = Attributes;
		slots = Slots;
	}
}

public interface IComponent
{


	void Setup();
	
	void OnCreating();
	void OnCreated();
	void OnMounting();
	void Render();
	void OnMounted();
	
	void OnUnmounting();
	void OnUnmounted();



	IElement Render();

}




public class IComponentBuilder
{

}
public class DefineComponent
{
	public DefineComponent(Action<IComponentBuilder> builder)
	{

	}

}