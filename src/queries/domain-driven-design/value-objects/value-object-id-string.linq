<Query Kind="Program">
  <Namespace>System.Net</Namespace>
</Query>

void Main()
{
	Name name = "test";
	
	
	Expression.Constant(name, typeof(string)).Dump();
	
	var type = typeof(string);
	var methodInfo = type.GetMethod("Contains", BindingFlags.Public | BindingFlags.Instance, new Type[] { typeof(string)} );
	
	Expression.Call(methodInfo, Expression.Constant("a")).Dump();

}




public readonly struct StringId {
	
}