<Query Kind="Program">
  <Namespace>System.Net.Sockets</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

void Main()
{
	// IP Supports both Stream and DGram Sockets (toggle between both)
	var socketType = SocketType.Stream;
	var address = Dns.GetHostEntry(Dns.GetHostName()).AddressList
			.Where((addr) => addr.AddressFamily == AddressFamily.InterNetwork)
			.Where((addr) => addr.GetAddressBytes()[0] != 127)
			.First();
			
	var endpoint = new IPEndPoint(address, 0);	
	var size= 4096;

	var serverThread = new Thread(new ThreadStart(async () =>
	{
		var socket = new Socket(
			endpoint.AddressFamily,
			socketType,
			ProtocolType.IP);

		try
		{
			socket.Bind(endpoint);
			
			Socket listener = socket;
			
			if (socket.SocketType == SocketType.Stream)
			{
				socket.Listen();
				listener = socket.Accept().Dump();
			}

			var sender = new IPEndPoint(IPAddress.Any, 8081) as EndPoint;
			while (true)
			{
				
				var buffer = new byte[size];

				// Receive Client Message
				var length = listener.ReceiveFrom(buffer, 0, size, SocketFlags.None, ref sender);

				// Decode Client Message
				Encoding.UTF8.GetString(buffer, 0, length).Dump();
				
				// Create Server Response
				var message = Encoding.UTF8.GetBytes("Hello from server");

				// Send Server Response
				listener.SendTo(message, 0, message.Length, SocketFlags.None, sender);
			}
		}
		catch (Exception exception)
		{
			exception.Dump();
			socket.Close();
			socket.Dispose();
		}
	}));
	var clientThread = new Thread(new ThreadStart(async () =>
	{
		var socket = new Socket(
			endpoint.AddressFamily,
			socketType,
			ProtocolType.IP);

		socket.Connect(endpoint);
		var sender = new IPEndPoint(IPAddress.Any, 8081) as EndPoint;
		try
		{
			while (true)
			{
				var message = Encoding.UTF8.GetBytes("Hello from client");
				var buffer = new byte[size];

				socket.SendTo(message, 0, message.Length, SocketFlags.None, endpoint);

				var length = socket.ReceiveFrom(buffer, 0, 4096, SocketFlags.None, ref sender);

				Encoding.UTF8.GetString(buffer, 0, length).Dump();

				Thread.Sleep(2000);
			}
		}

		catch (Exception exception)
		{
			exception.Dump();
			socket.Close();
			socket.Dispose();
		}
	}));

	serverThread.Start();
	//clientThread.Start();

	serverThread.Join();
}
