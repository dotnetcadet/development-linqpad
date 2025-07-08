<Query Kind="Program">
  <Namespace>System.Net.Sockets</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Security</Namespace>
</Query>


async Task Main()
{
//	var endpoint = new IPEndPoint(IPAddress.Loopback, 8081);
//	var endpoint1 = new IPEndPoint(IPAddress.Loopback, 8082);
//	var socket = new Socket(
//			endpoint.AddressFamily,
//			SocketType.Dgram,
//			ProtocolType.Udp
//		);
//
//	try
//	{
//		socket.Bind(endpoint);
//		socket.Bind(endpoint1);
//		//socket.Listen();
//		
//		//var connection = await socket.AcceptAsync();
//		//await socket.ConnectAsync(endpoint);
//
//	
//	}
//	catch (Exception exception)
//	{
//		socket.Close();
//		socket.Dispose();
//	}
//
//	socket.Close();
//	socket.Dispose();
	//	var endpoint = new IPEndPoint(IPAddress.Loopback, 8080);
//	var server = new Thread(new ThreadStart(async () =>
//	{
//
//		var socket = new Socket(
//			endpoint.AddressFamily,
//			SocketType.Dgram,
//			ProtocolType.Udp
//		);
//
//		socket.Bind(endpoint);
//		//socket.Listen();
//
//		try
//		{
//			//var connection = await socket.AcceptAsync();
//			
//			while (true)
//			{
//
//			}
//		}
//		catch (Exception exception)
//		{
//			socket.Close();
//			socket.Dispose();
//		}
//	}));
//	var client = new Thread(new ThreadStart(async () =>
//	{
//		var socket = new Socket(
//			endpoint.AddressFamily,
//			SocketType.Dgram,
//			ProtocolType.Udp
//		);
//
//		try
//		{
//			await socket.ConnectAsync(endpoint);
//
//			while (true)
//			{
//
//			}
//		}
//		catch (Exception exception)
//		{
//			socket.Close();
//			socket.Dispose();
//		}
//	}));
//
//	server.Start();
//	client.Start();
//
//	server.Join();
	//var endpoint = new IPEndPoint(IPAddress.Any, 8093);
	//var socket1 = new Socket(endpoint.AddressFamily, SocketType.Stream,ProtocolType.Tcp);
	//
	//socket1.Bind(endpoint);
	//socket1.Listen();

	
	UDPSocket s = new UDPSocket();
	s.Server("127.0.0.1", 8093);

	UDPSocket c = new UDPSocket();
	c.Client("127.0.0.1", 8093);
	c.Send("TEST!");
	c.Send("This is a test message");
	
	s.Send("Hello from server");

}

public class UDPSocket : IDisposable
{
	private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
	private const int bufSize = 8 * 1024;
	private State state = new State();
	private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
	private AsyncCallback recv = null;

	public class State
	{
		public byte[] buffer = new byte[bufSize];
	}

	public void Server(string address, int port)
	{
		_socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
		_socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
		Receive();
	}

	public void Client(string address, int port)
	{
		_socket.Connect(IPAddress.Parse(address), port);
		Receive();
	}

	public void Send(string text)
	{
		byte[] data = Encoding.ASCII.GetBytes(text);
		_socket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
		{
			State so = (State)ar.AsyncState;
			int bytes = _socket.EndSend(ar);
			Console.WriteLine("SEND: {0}, {1}", bytes, text);
		}, state);
	}

	private void Receive()
	{
		_socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
		{
			State so = (State)ar.AsyncState;
			int bytes = _socket.EndReceiveFrom(ar, ref epFrom);
			_socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
			Console.WriteLine("RECV: {0}: {1}, {2}", epFrom.ToString(), bytes, Encoding.ASCII.GetString(so.buffer, 0, bytes));
		}, state);
	}

	public void Dispose()
	{
		_socket.Close();
		_socket.Dispose();
	}
}