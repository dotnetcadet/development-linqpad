<Query Kind="Program">
  <Namespace>System.Net.Quic</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Security.Authentication</Namespace>
</Query>

async Task Main()
{
	var isRunning = true;

	var quic = await QuicListener.ListenAsync(new()
	{
		ListenEndPoint = new IPEndPoint(IPAddress.Loopback, 8080),
		ListenBacklog = 512,
		ConnectionOptionsCallback = async (connection, info, cancellationToken) =>
		{
			return new QuicServerConnectionOptions()
			{
				ServerAuthenticationOptions = new()
				{
					AllowRenegotiation = true,
					EnabledSslProtocols = SslProtocols.Tls13,
					ApplicationProtocols = new()
					{
						System.Net.Security.SslApplicationProtocol.Http3
					}
				},
				IdleTimeout = Timeout.InfiniteTimeSpan, // Kestrel manages connection lifetimes itself so it can send GoAway's.
				MaxInboundBidirectionalStreams = 100,
				MaxInboundUnidirectionalStreams = 10,
				DefaultCloseErrorCode = 0,
				DefaultStreamErrorCode = 0,
			};
		},
		ApplicationProtocols = new()
		{
			System.Net.Security.SslApplicationProtocol.Http3
		}
	});


	while (isRunning)
	{
		var connection = await quic.AcceptConnectionAsync();
		

		var stream = await connection.AcceptInboundStreamAsync();

	}
	
	await quic.DisposeAsync();
}

// You can define other methods, fields, classes and namespaces here