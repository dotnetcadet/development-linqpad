<Query Kind="Program">
<Namespace>System</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>System.Threading.Tasks</Namespace>
<Namespace>System.Reflection</Namespace>
</Query>

void Main()
{

}

#region Assimalign.Cohesion.Logging(net8.0)
namespace Assimalign.Cohesion.Logging
{
	#region \
	public sealed class Logger
	{
	}
	public sealed class LoggerAdapter
	{
	}
	public enum LogLevel
	{
	    Trace = 0,
	    Debug = 1,
	    Information = 2,
	    Warning = 3,
	    Error = 4,
	    Critical = 5,
	    Event = 6,
	    None = 7
	}
	#endregion
	#region \Abstractions
	public interface ILogger
	{
	    void Log(LogLevel level, string message);
	    ILoggerBatch CreateLogBatch();
	}
	public interface ILoggerBatch : ILogger
	{
	    string BatchId { get; }
	}
	public interface ILoggerFactory
	{
	    ILogger Create(string loggerName);
	}
	public interface ILoggerGroup
	{
	}
	#endregion
	#region \Extensions
	public static class LoggerExtensions
	{
	    public static void Trace(this ILogger logger)
	    {
	    }
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
}
#endregion
