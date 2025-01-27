<Query Kind="Program">
<NuGetReference Version="8.0.0">System.IO.Pipelines</NuGetReference>
<Namespace>System</Namespace>
<Namespace>System.Collections.Generic</Namespace>
<Namespace>System.Linq</Namespace>
<Namespace>System.Text</Namespace>
<Namespace>System.Threading.Tasks</Namespace>
<Namespace>Assimalign.Cohesion.Hosting</Namespace>
<Namespace>System.Net</Namespace>
<Namespace>System.Net.Sockets</Namespace>
<Namespace>System.Runtime.InteropServices</Namespace>
<Namespace>System.Security.Cryptography</Namespace>
<Namespace>System.Threading</Namespace>
<Namespace>System.Int32</Namespace>
<Namespace>System.IO</Namespace>
<Namespace>System.Diagnostics</Namespace>
<Namespace>System.Reflection</Namespace>
<Namespace>System.Runtime.CompilerServices</Namespace>
<Namespace>Assimalign.Cohesion.Net.Udt.Internal</Namespace>
</Query>
#load ".\assimalign.cohesion.configuration"
#load ".\assimalign.cohesion.hosting"
#load ".\assimalign.cohesion.logging"
#load ".\assimalign.cohesion.net.cryptography"
#load ".\assimalign.cohesion.net.transports"
#load ".\assimalign.cohesion.core"

void Main()
{

}

#region Assimalign.Cohesion.Net.Udt(net8.0)
namespace Assimalign.Cohesion.Net.Udt
{
	#region \
	public sealed class UdtClient
	{
	}
	#endregion
	#region \Extensions
	public static class UdtHostBuilderExtensions
	{
	    public static IHostBuilder AddUdtServer(this IHostBuilder builder, Action<UdtServerBuilder> configure)
	    {
	        if (configure is null)
	        {
	            throw new ArgumentNullException(nameof(configure));
	        }
	        var serverBuilder = new UdtServerBuilder();
	        configure.Invoke(serverBuilder);
	        return builder.AddService(((IHostServiceBuilder)serverBuilder).Build());
	    }
	}
	#endregion
	#region \Internal
	internal class PerfMon
	{
	    // global measurements
	    internal long msTimeStamp;                    // time since the UDT entity is started, in milliseconds
	    internal long pktSentTotal;                   // total number of sent data packets, including retransmissions
	    internal long pktRecvTotal;                   // total number of received packets
	    internal int pktSndLossTotal;                 // total number of lost packets (sender side)
	    internal int pktRcvLossTotal;                 // total number of lost packets (receiver side)
	    internal int pktRetransTotal;                 // total number of retransmitted packets
	    internal int pktSentACKTotal;                 // total number of sent ACK packets
	    internal int pktRecvACKTotal;                 // total number of received ACK packets
	    internal int pktSentNAKTotal;                 // total number of sent NAK packets
	    internal int pktRecvNAKTotal;                 // total number of received NAK packets
	    internal long usSndDurationTotal;             // total time duration when UDT is sending data (idle time exclusive)
	    // local measurements
	    internal long pktSent;                        // number of sent data packets, including retransmissions
	    internal long pktRecv;                        // number of received packets
	    internal int pktSndLoss;                      // number of lost packets (sender side)
	    internal int pktRcvLoss;                      // number of lost packets (receiver side)
	    internal int pktRetrans;                      // number of retransmitted packets
	    internal int pktSentACK;                      // number of sent ACK packets
	    internal int pktRecvACK;                      // number of received ACK packets
	    internal int pktSentNAK;                      // number of sent NAK packets
	    internal int pktRecvNAK;                      // number of received NAK packets
	    internal double mbpsSendRate;                 // sending rate in Mb/s
	    internal double mbpsRecvRate;                 // receiving rate in Mb/s
	    internal long usSndDuration;                  // busy sending time (i.e., idle time exclusive)
	    // instant measurements
	    internal double usPktSndPeriod;               // packet sending period, in microseconds
	    internal int pktFlowWindow;                   // flow window size, in number of packets
	    internal int pktCongestionWindow;             // congestion window size, in number of packets
	    internal int pktFlightSize;                   // number of packets on flight
	    internal double msRTT;                        // RTT, in milliseconds
	    internal double mbpsBandwidth;                // estimated bandwidth, in Mb/s
	    internal int byteAvailSndBuf;                 // available UDT sender buffer size
	    internal int byteAvailRcvBuf;                 // available UDT receiver buffer size
	};
	internal static class UdtAcknowledgementNumber
	{
	    public static int incack(int acknowledgemen)
	    {
	        return (acknowledgemen == m_iMaxAckSeqNo) ? 0 : acknowledgemen + 1;
	    }
	    public static int m_iMaxAckSeqNo = 0x7FFFFFFF;         // maximum ACK sub-sequence number used in UDT
	}
	internal class UdtAcknowledgementWindow
	{
	    int[] m_piACKSeqNo;       // Seq. No. for the ACK packet
	    int[] m_piACK;            // Data Seq. No. carried by the ACK packet
	    ulong[] m_pTimeStamp;      // The timestamp when the ACK was sent
	    int m_iSize;                 // Size of the ACK history window
	    int m_iHead;                 // Pointer to the lastest ACK record
	    int m_iTail;                 // Pointer to the oldest ACK record
	    public UdtAcknowledgementWindow(int size = 1024)
	    {
	        m_iSize = size;
	        m_piACKSeqNo = new int[m_iSize];
	        m_piACK = new int[m_iSize];
	        m_pTimeStamp = new ulong[m_iSize];
	        m_piACKSeqNo[0] = -1;
	    }
	    // Functionality:
	    //    Write an ACK record into the window.
	    // Parameters:
	    //    0) [in] seq: ACK seq. no.
	    //    1) [in] ack: DATA ACK no.
	    // Returned value:
	    //    None.
	    public void Store(int seq, int ack)
	    {
	        m_piACKSeqNo[m_iHead] = seq;
	        m_piACK[m_iHead] = ack;
	        m_pTimeStamp[m_iHead] = UdtTimer.getTime();
	        m_iHead = (m_iHead + 1) % m_iSize;
	        // overwrite the oldest ACK since it is not likely to be acknowledged
	        if (m_iHead == m_iTail)
	        {
	            m_iTail = (m_iTail + 1) % m_iSize;
	        }
	    }
	    // Functionality:
	    //    Search the ACK-2 "seq" in the window, find out the DATA "ack" and caluclate RTT .
	    // Parameters:
	    //    0) [in] seq: ACK-2 seq. no.
	    //    1) [out] ack: the DATA ACK no. that matches the ACK-2 no.
	    // Returned value:
	    //    RTT.
	    public int Acknowledge(int seq, ref int ack)
	    {
	        if (m_iHead >= m_iTail)
	        {
	            // Head has not exceeded the physical boundary of the window
	            for (int i = m_iTail, n = m_iHead; i < n; ++i)
	            {
	                // looking for indentical ACK Seq. No.
	                if (seq == m_piACKSeqNo[i])
	                {
	                    // return the Data ACK it carried
	                    ack = m_piACK[i];
	                    // calculate RTT
	                    int rtt = (int)(UdtTimer.getTime() - m_pTimeStamp[i]);
	                    if (i + 1 == m_iHead)
	                    {
	                        m_iTail = m_iHead = 0;
	                        m_piACKSeqNo[0] = -1;
	                    }
	                    else
	                    {
	                        m_iTail = (i + 1) % m_iSize;
	                    }
	                    return rtt;
	                }
	            }
	            // Bad input, the ACK node has been overwritten
	            return -1;
	        }
	        // Head has exceeded the physical window boundary, so it is behind tail
	        for (int j = m_iTail, n = m_iHead + m_iSize; j < n; ++j)
	        {
	            // looking for indentical ACK seq. no.
	            if (seq == m_piACKSeqNo[j % m_iSize])
	            {
	                // return Data ACK
	                j %= m_iSize;
	                ack = m_piACK[j];
	                // calculate RTT
	                int rtt = (int)(UdtTimer.getTime() - m_pTimeStamp[j]);
	                if (j == m_iHead)
	                {
	                    m_iTail = m_iHead = 0;
	                    m_piACKSeqNo[0] = -1;
	                }
	                else
	                {
	                    m_iTail = (j + 1) % m_iSize;
	                }
	                return rtt;
	            }
	        }
	        // bad input, the ACK node has been overwritten
	        return -1;
	    }
	}
	internal class UdtChannel
	{
	    AddressFamily m_iIPversion;                    // IP version
	    Socket m_socket;                 // socket descriptor
	    int m_iSndBufSize;                   // UDP sending buffer size
	    int m_iRcvBufSize;
	    public UdtChannel()
	    {
	        m_iIPversion = AddressFamily.InterNetwork;
	        m_iSndBufSize = 65536;
	        m_iRcvBufSize = 65536;
	    }
	    public UdtChannel(AddressFamily addressFamily)
	    {
	        m_iIPversion = addressFamily;
	        m_iSndBufSize = 65536;
	        m_iRcvBufSize = 65536;
	    }
	    public void Open(IPEndPoint addr)
	    {
	        // construct a socket
	        try
	        {
	            m_socket = new Socket(m_iIPversion, SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp);
	        }
	        catch (SocketException e)
	        {
	            throw new UdtException(1, 0, e.ErrorCode);
	        }
	        if (null != addr)
	        {
	            try
	            {
	                m_socket.Bind(addr);
	            }
	            catch (SocketException e)
	            {
	                throw new UdtException(1, 3, e.ErrorCode);
	            }
	        }
	        else
	        {
	            try
	            {
	                m_socket.Bind(new IPEndPoint(IPAddress.Any, 0));
	            }
	            catch (SocketException e)
	            {
	                throw new UdtException(1, 3, e.ErrorCode);
	            }
	        }
	        SetUdpSockOptions();
	    }
	    public void Open(Socket udpsock)
	    {
	        m_socket = udpsock;
	        SetUdpSockOptions();
	    }
	    void SetUdpSockOptions()
	    {
	        bool isMac = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
	        bool isBSD =
	#if NETSTANDARD2_0
	            false;
	#else
	            RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD);
	#endif
	        if (isMac || isBSD)
	        {
	            // BSD system will fail setsockopt if the requested buffer size exceeds system maximum value
	            int maxsize = 64000;
	            m_socket.ReceiveBufferSize = maxsize;
	            m_socket.SendBufferSize = maxsize;
	        }
	        else
	        {
	            m_socket.ReceiveBufferSize = m_iRcvBufSize;
	            m_socket.SendBufferSize = m_iSndBufSize;
	        }
	    }
	    public void Close()
	    {
	        m_socket.Close();
	    }
	    int getSndBufSize()
	    {
	        m_iSndBufSize = (int)m_socket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer);
	        return m_iSndBufSize;
	    }
	    int getRcvBufSize()
	    {
	        m_iRcvBufSize = (int)m_socket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer);
	        return m_iRcvBufSize;
	    }
	    public void setSndBufSize(int size)
	    {
	        m_iSndBufSize = size;
	    }
	    public void setRcvBufSize(int size)
	    {
	        m_iRcvBufSize = size;
	    }
	    public void getSockAddr(ref IPEndPoint addr)
	    {
	        addr = (IPEndPoint)m_socket.LocalEndPoint;
	    }
	    void getPeerAddr(ref IPEndPoint addr)
	    {
	        addr = (IPEndPoint)m_socket.RemoteEndPoint;
	    }
	    public int sendto(IPEndPoint addr, UdtPacket packet)
	    {
	        TraceSend(addr, packet);
	        // convert control information into network order
	        packet.ConvertControlInfoToNetworkOrder();
	        // convert packet header into network order
	        packet.ConvertHeaderToNetworkOrder();
	        byte[] data = packet.GetBytes();
	        int res = m_socket.SendTo(data, addr);
	        // convert back into local host order
	        packet.ConvertHeaderToHostOrder();
	        packet.ConvertControlInfoToHostOrder();
	        return res;
	    }
	    void TraceSend(IPEndPoint destination, UdtPacket packet)
	    {
	        return;
	        StringBuilder sb = new StringBuilder();
	        sb.Append(DateTime.Now.ToString("hh:mm:ss.fff"));
	        sb.AppendFormat(" SND {0} => {1}", m_socket.LocalEndPoint, destination);
	        sb.AppendLine();
	        sb.AppendLine(packet.ToString());
	        sb.AppendLine();
	        Console.WriteLine(sb.ToString());
	    }
	    void TraceRecv(IPEndPoint source, UdtPacket packet)
	    {
	        return;
	        StringBuilder sb = new StringBuilder();
	        sb.Append(DateTime.Now.ToString("hh:mm:ss.fff"));
	        sb.AppendFormat(" RCV {0} <= {1}", m_socket.LocalEndPoint, source);
	        sb.AppendLine();
	        sb.AppendLine(packet.ToString());
	        sb.AppendLine();
	        Console.WriteLine(sb.ToString());
	    }
	    public int recvfrom(ref IPEndPoint addr, UdtPacket packet)
	    {
	        try
	        {
	            if (!m_socket.Poll(10000, SelectMode.SelectRead))
	            {
	                return -1;
	            }
	        }
	        catch (SocketException sex)
	        {
	            return -1;
	        }
	        catch (ObjectDisposedException odex)
	        {
	            return -1;
	        }
	        byte[] bytes = new byte[UdtPacket.packetHeaderSize + packet.getLength()];
	        EndPoint source = addr;
	        int res;
	        try
	        {
	            res = m_socket.ReceiveFrom(bytes, ref source);
	        }
	        catch (SocketException sex)
	        {
	            return -1;
	        }
	        catch (ObjectDisposedException odex)
	        {
	            return -1;
	        }
	        addr = source as IPEndPoint;
	        if (res <= 0)
	        {
	            return -1;
	        }
	        bool success = packet.SetHeaderAndDataFromBytes(bytes, res);
	        if (!success)
	        {
	            return -1;
	        }
	        // convert back into local host order
	        packet.ConvertHeaderToHostOrder();
	        packet.ConvertControlInfoToHostOrder();
	        TraceRecv(addr, packet);
	        return packet.getLength();
	    }
	}
	internal class UdtCongestionControl
	{
	    public const UDTSOCKET INVALID_SOCK = -1;
	    public const int ERROR = -1;
	    const int m_iVersion = 4;
	    public static UdtUnited s_UDTUnited = new UdtUnited();               // UDT global management base
	    // Identification
	    public UDTSOCKET m_SocketID;                        // UDT socket number
	    public SocketType m_iSockType;                     // Type of the UDT connection (SOCK_STREAM or SOCK_DGRAM)
	    public UDTSOCKET m_PeerID;             // peer id, for multiplexer
	    // Packet sizes
	    int m_iPktSize;                              // Maximum/regular packet size, in bytes
	    public int m_iPayloadSize;                          // Maximum/regular payload size, in bytes
	    // Options
	    public int m_iMSS;                                  // Maximum Segment Size, in bytes
	    bool m_bSynSending;                          // Sending syncronization mode
	    public bool m_bSynRecving;                          // Receiving syncronization mode
	    public int m_iFlightFlagSize;                       // Maximum number of packets in flight from the peer side
	    int m_iSndBufSize;                           // Maximum UDT sender buffer size
	    int m_iRcvBufSize;                           // Maximum UDT receiver buffer size
	    LingerOption m_Linger;                             // Linger information on close
	    public int m_iUDPSndBufSize;                        // UDP sending buffer size
	    public int m_iUDPRcvBufSize;                        // UDP receiving buffer size
	    public AddressFamily m_iIPversion;                            // IP version
	    public bool m_bRendezvous;                          // Rendezvous connection mode
	    int m_iSndTimeOut;                           // sending timeout in milliseconds
	    int m_iRcvTimeOut;                           // receiving timeout in milliseconds
	    public bool m_bReuseAddr;              // reuse an exiting port or not, for UDP multiplexer
	    long m_llMaxBW;              // maximum data transfer rate (threshold)
	    // congestion control
	    UdtCongestionControlVirtualFactory m_pCCFactory;             // Factory class to create a specific CC instance
	    UdtCongestionControlBase m_pCC;                                  // congestion control class
	    public Dictionary<IPAddress, UdtInfoBlock> m_pCache = new Dictionary<IPAddress, UdtInfoBlock>();       // network information cache
	    // Status
	    volatile bool m_bListening;                  // If the UDT entit is listening to connection
	    volatile bool m_bConnecting;            // The short phase when connect() is called but not yet completed
	    public volatile bool m_bConnected;                  // Whether the connection is on or off
	    public volatile bool m_bClosing;                    // If the UDT entity is closing
	    volatile bool m_bShutdown;                   // If the peer side has shutdown the connection
	    public volatile bool m_bBroken;                     // If the connection has been broken
	    volatile bool m_bPeerHealth;                 // If the peer status is normal
	    bool m_bOpened;                              // If the UDT entity has been opened
	    public int m_iBrokenCounter;           // a counter (number of GC checks) to let the GC tag this socket as disconnected
	    int m_iEXPCount;                             // Expiration counter
	    int m_iBandwidth;                            // Estimated bandwidth, number of packets per second
	    int m_iRTT;                                  // RTT, in microseconds
	    int m_iRTTVar;                               // RTT variance
	    int m_iDeliveryRate;                // Packet arrival rate at the receiver side
	    public ulong m_ullLingerExpiration;     // Linger expiration time (for GC to close a socket with data in sending buffer)
	    public UdtHandshake m_ConnReq = new UdtHandshake();           // connection request
	    public UdtHandshake m_ConnRes = new UdtHandshake();           // connection response
	    public long m_llLastReqTime;            // last time when a connection request is sent
	    // Sending related data
	    public UdtSenderBuffer m_pSndBuffer;                    // Sender buffer
	    UdtSenderLossList m_pSndLossList;                // Sender loss list
	    UdtPacketTimeWindow m_pSndTimeWindow;            // Packet sending time window
	    /*volatile*/
	    ulong m_ullInterval;             // Inter-packet time, in CPU clock cycles
	    ulong m_ullTimeDiff;                      // aggregate difference in inter-packet time
	    volatile int m_iFlowWindowSize;              // Flow control window size
	    /*volatile*/
	    double m_dCongestionWindow;         // congestion window size
	    volatile int m_iSndLastAck;              // Last ACK received
	    volatile int m_iSndLastDataAck;          // The real last ACK that updates the sender buffer and loss list
	    volatile int m_iSndCurrSeqNo;            // The largest sequence number that has been sent
	    int m_iLastDecSeq;                       // Sequence number sent last decrease occurs
	    int m_iSndLastAck2;                      // Last ACK2 sent back
	    ulong m_ullSndLastAck2Time;               // The time when last ACK2 was sent back
	    public int m_iISN;                              // Initial Sequence Number
	    // Receiving related data
	    public UdtReceiverBuffer m_pRcvBuffer;                    // Receiver buffer
	    UdtReceiverLossList m_pRcvLossList;                // Receiver loss list
	    UdtAcknowledgementWindow m_pACKWindow;                    // ACK history window
	    UdtPacketTimeWindow m_pRcvTimeWindow;            // Packet arrival time window
	    int m_iRcvLastAck;                       // Last sent ACK
	    ulong m_ullLastAckTime;                   // Timestamp of last ACK
	    int m_iRcvLastAckAck;                    // Last sent ACK that has been acknowledged
	    int m_iAckSeqNo;                         // Last ACK sequence number
	    int m_iRcvCurrSeqNo;                     // Largest received sequence number
	    ulong m_ullLastWarningTime;               // Last time that a warning message is sent
	    int m_iPeerISN;                          // Initial Sequence Number of the peer side
	    // synchronization: mutexes and conditions
	    readonly object m_ConnectionLock = new object();            // used to synchronize connection operation
	    readonly EventWaitHandle m_SendBlockCond = new EventWaitHandle(false, EventResetMode.AutoReset);              // used to block "send" call
	    readonly object m_SendBlockLock = new object();             // lock associated to m_SendBlockCond
	    readonly object m_AckLock = new object();                   // used to protected sender's loss list when processing ACK
	    readonly EventWaitHandle m_RecvDataCond = new EventWaitHandle(false, EventResetMode.AutoReset);               // used to block "recv" when there is no data
	    readonly object m_RecvDataLock = new object();              // lock associated to m_RecvDataCond
	    readonly object m_SendLock = new object();                  // used to synchronize "send" call
	    readonly object m_RecvLock = new object();                  // used to synchronize "recv" call
	    // Trace
	    ulong m_StartTime;                        // timestamp when the UDT entity is started
	    long m_llSentTotal;                       // total number of sent data packets, including retransmissions
	    long m_llRecvTotal;                       // total number of received packets
	    int m_iSndLossTotal;                         // total number of lost packets (sender side)
	    int m_iRcvLossTotal;                         // total number of lost packets (receiver side)
	    int m_iRetransTotal;                         // total number of retransmitted packets
	    int m_iSentACKTotal;                         // total number of sent ACK packets
	    int m_iRecvACKTotal;                         // total number of received ACK packets
	    int m_iSentNAKTotal;                         // total number of sent NAK packets
	    int m_iRecvNAKTotal;                         // total number of received NAK packets
	    long m_llSndDurationTotal;       // total real time for sending
	    ulong m_LastSampleTime;                   // last performance sample time
	    long m_llTraceSent;                       // number of pakctes sent in the last trace interval
	    long m_llTraceRecv;                       // number of pakctes received in the last trace interval
	    int m_iTraceSndLoss;                         // number of lost packets in the last trace interval (sender side)
	    int m_iTraceRcvLoss;                         // number of lost packets in the last trace interval (receiver side)
	    int m_iTraceRetrans;                         // number of retransmitted packets in the last trace interval
	    int m_iSentACK;                              // number of ACKs sent in the last trace interval
	    int m_iRecvACK;                              // number of ACKs received in the last trace interval
	    int m_iSentNAK;                              // number of NAKs sent in the last trace interval
	    int m_iRecvNAK;                              // number of NAKs received in the last trace interval
	    long m_llSndDuration;            // real time for sending
	    long m_llSndDurationCounter;     // timers to record the sending duration
	    // Timers
	    ulong m_ullCPUFrequency;                  // CPU clock frequency, used for Timer, ticks per microsecond
	    public const int m_iSYNInterval = 10000;             // Periodical Rate Control Interval, 10000 microsecond
	    const int m_iSelfClockInterval = 64;       // ACK interval for self-clocking
	    ulong m_ullNextACKTime;          // Next ACK time, in CPU clock cycles, same below
	    ulong m_ullNextNAKTime;          // Next NAK time
	    /*volatile*/
	    ulong m_ullSYNInt;      // SYN interval
	    /*volatile*/
	    ulong m_ullACKInt;      // ACK interval
	    /*volatile*/
	    ulong m_ullNAKInt;      // NAK interval
	    /*volatile*/
	    ulong m_ullLastRspTime;     // time stamp of last response from the peer
	    ulong m_ullMinNakInt;            // NAK timeout lower bound; too small value can cause unnecessary retransmission
	    ulong m_ullMinExpInt;            // timeout lower bound threshold: too small timeout can cause problem
	    int m_iPktCount;                // packet counter for ACK
	    int m_iLightACKCount;           // light ACK counter
	    ulong m_ullTargetTime;           // scheduled time of next packet sending
	    // for UDP multiplexer
	    public SndQueue m_pSndQueue;          // packet sending queue
	    public RcvQueue m_pRcvQueue;         // packet receiving queue
	    public IPEndPoint m_pPeerAddr;          // peer address
	    public uint[] m_piSelfIP = new uint[4];         // local UDP IP address
	    public SNode m_pSNode;               // node information for UDT list used in snd queue
	    public RNode m_pRNode;               // node information for UDT list used in rcv queue
	    public UdtCongestionControl()
	    {
	        // Default UDT configurations
	        m_iMSS = 1500;
	        m_bSynSending = true;
	        m_bSynRecving = true;
	        m_iFlightFlagSize = 204800;
	        m_iSndBufSize = 65536;
	        m_iRcvBufSize = 65536; //Rcv buffer MUST NOT be bigger than Flight Flag size
	        m_Linger = new LingerOption(true, 180);
	        m_iUDPSndBufSize = 524288;
	        m_iUDPRcvBufSize = m_iRcvBufSize * m_iMSS;
	        m_iSockType = SocketType.Stream;
	        m_iIPversion = AddressFamily.InterNetwork;
	        m_bRendezvous = true;
	        m_iSndTimeOut = -1;
	        m_iRcvTimeOut = -1;
	        m_bReuseAddr = true;
	        m_llMaxBW = -1;
	        m_pCCFactory = new UdtCongestionControlFactory<UdtCongestionControl1>();
	        // Initial status
	        m_bOpened = false;
	        m_bListening = false;
	        m_bConnecting = false;
	        m_bConnected = false;
	        m_bClosing = false;
	        m_bShutdown = false;
	        m_bBroken = false;
	        m_bPeerHealth = true;
	        m_ullLingerExpiration = 0;
	    }
	    public UdtCongestionControl(UdtCongestionControl ancestor)
	    {
	        // Default UDT configurations
	        m_iMSS = ancestor.m_iMSS;
	        m_bSynSending = ancestor.m_bSynSending;
	        m_bSynRecving = ancestor.m_bSynRecving;
	        m_iFlightFlagSize = ancestor.m_iFlightFlagSize;
	        m_iSndBufSize = ancestor.m_iSndBufSize;
	        m_iRcvBufSize = ancestor.m_iRcvBufSize;
	        m_Linger = ancestor.m_Linger;
	        m_iUDPSndBufSize = ancestor.m_iUDPSndBufSize;
	        m_iUDPRcvBufSize = ancestor.m_iUDPRcvBufSize;
	        m_iSockType = ancestor.m_iSockType;
	        m_iIPversion = ancestor.m_iIPversion;
	        m_bRendezvous = ancestor.m_bRendezvous;
	        m_iSndTimeOut = ancestor.m_iSndTimeOut;
	        m_iRcvTimeOut = ancestor.m_iRcvTimeOut;
	        m_bReuseAddr = true;    // this must be true, because all accepted sockets shared the same port with the listener
	        m_llMaxBW = ancestor.m_llMaxBW;
	        m_pCCFactory = ancestor.m_pCCFactory.clone();
	        m_pCC = null;
	        m_pCache = ancestor.m_pCache;
	        // Initial status
	        m_bOpened = false;
	        m_bListening = false;
	        m_bConnecting = false;
	        m_bConnected = false;
	        m_bClosing = false;
	        m_bShutdown = false;
	        m_bBroken = false;
	        m_bPeerHealth = true;
	        m_ullLingerExpiration = 0;
	    }
	    ~UdtCongestionControl()
	    {
	        // release mutex/condtion variables
	        destroySynch();
	    }
	    public unsafe void setOpt(UdtOptions optName, int optval, UDTSOCKET socket)
	    {
	        setOpt(optName, &optval, socket);
	    }
	    public unsafe void setOpt(UdtOptions optName, void* optval, UDTSOCKET socket)
	    {
	        if (m_bBroken || m_bClosing)
	        {
	            throw new UdtException(2, 1, 0);
	        }
	        lock (m_ConnectionLock) 
	            lock (m_SendLock) 
	                lock (m_RecvLock)
	                {
	                    setOpt_unsafe(optName, optval, socket);
	                }
	    }
	    unsafe void setOpt_unsafe(UdtOptions optName, void* optval, UDTSOCKET socket)
	    {
	        switch (optName)
	        {
	            case UdtOptions.UDT_MSS:
	                if (m_bOpened)
	                {
	                    throw new UdtException(5, 1, 0);
	                }
	                if (*(int*)optval < (int)(28 + UdtHandshake.ContentSize))
	                {
	                    throw new UdtException(5, 3, 0);
	                }
	                m_iMSS = *(int*)optval;
	                // Packet size cannot be greater than UDP buffer size
	                if (m_iMSS > m_iUDPSndBufSize)
	                {
	                    m_iMSS = m_iUDPSndBufSize;
	                }
	                if (m_iMSS > m_iUDPRcvBufSize)
	                {
	                    m_iMSS = m_iUDPRcvBufSize;
	                }
	                break;
	            case UdtOptions.UDT_SNDSYN:
	                m_bSynSending = *(bool*)optval;
	                break;
	            case UdtOptions.UDT_RCVSYN:
	                m_bSynRecving = *(bool*)optval;
	                break;
	            case UdtOptions.UDT_CC:
	                if (m_bConnecting || m_bConnected)
	                {
	                    throw new UdtException(5, 1, 0);
	                }
	                //m_pCCFactory = (&(CCCVirtualFactory*)optval).clone();
	                break;
	            case UdtOptions.UDT_FC:
	                if (m_bConnecting || m_bConnected)
	                {
	                    throw new UdtException(5, 2, 0);
	                }
	                if (*(int*)optval < 1)
	                {
	                    throw new UdtException(5, 3);
	                }
	                // Mimimum recv flight flag size is 32 packets
	                if (*(int*)optval > 32)
	                {
	                    m_iFlightFlagSize = *(int*)optval;
	                }
	                else
	                {
	                    m_iFlightFlagSize = 32;
	                }
	                break;
	            case UdtOptions.UDT_SNDBUF:
	                if (m_bOpened)
	                {
	                    throw new UdtException(5, 1, 0);
	                }
	                if (*(int*)optval <= 0)
	                {
	                    throw new UdtException(5, 3, 0);
	                }
	                m_iSndBufSize = *(int*)optval / (m_iMSS - 28);
	                break;
	            case UdtOptions.UDT_RCVBUF:
	                if (m_bOpened)
	                {
	                    throw new UdtException(5, 1, 0);
	                }
	                if (*(int*)optval <= 0)
	                {
	                    throw new UdtException(5, 3, 0);
	                }
	                // Mimimum recv buffer size is 32 packets
	                if (*(int*)optval > (m_iMSS - 28) * 32)
	                {
	                    m_iRcvBufSize = *(int*)optval / (m_iMSS - 28);
	                }
	                else
	                {
	                    m_iRcvBufSize = 32;
	                }
	                // recv buffer MUST not be greater than FC size
	                if (m_iRcvBufSize > m_iFlightFlagSize)
	                {
	                    m_iRcvBufSize = m_iFlightFlagSize;
	                }
	                break;
	            case UdtOptions.UDT_LINGER:
	                m_Linger = ConvertLingerOption.FromVoidPointer(optval);
	                break;
	            case UdtOptions.UDP_SNDBUF:
	                if (m_bOpened)
	                {
	                    throw new UdtException(5, 1, 0);
	                }
	                m_iUDPSndBufSize = *(int*)optval;
	                if (m_iUDPSndBufSize < m_iMSS)
	                {
	                    m_iUDPSndBufSize = m_iMSS;
	                }
	                break;
	            case UdtOptions.UDP_RCVBUF:
	                if (m_bOpened)
	                {
	                    throw new UdtException(5, 1, 0);
	                }
	                m_iUDPRcvBufSize = *(int*)optval;
	                if (m_iUDPRcvBufSize < m_iMSS)
	                {
	                    m_iUDPRcvBufSize = m_iMSS;
	                }
	                break;
	            case UdtOptions.UDT_RENDEZVOUS:
	                if (m_bConnecting || m_bConnected)
	                {
	                    throw new UdtException(5, 1, 0);
	                }
	                m_bRendezvous = *(bool*)optval;
	                break;
	            case UdtOptions.UDT_SNDTIMEO:
	                m_iSndTimeOut = *(int*)optval;
	                break;
	            case UdtOptions.UDT_RCVTIMEO:
	                m_iRcvTimeOut = *(int*)optval;
	                break;
	            case UdtOptions.UDT_REUSEADDR:
	                if (m_bOpened)
	                {
	                    throw new UdtException(5, 1, 0);
	                }
	                m_bReuseAddr = *(bool*)optval;
	                break;
	            case UdtOptions.UDT_MAXBW:
	                m_llMaxBW = *(long*)optval;
	                break;
	            default:
	                throw new UdtException(5, 0, 0);
	        }
	    }
	    public unsafe void getOpt(UdtOptions optName, void* optval, ref int optlen)
	    {
	        lock (m_ConnectionLock)
	        {
	            getOpt_unsafe(optName, optval, ref optlen);
	        }
	    }
	    unsafe void getOpt_unsafe(UdtOptions optName, void* optval, ref int optlen)
	    {
	        switch (optName)
	        {
	            case UdtOptions.UDT_MSS:
	                *(int*)optval = m_iMSS;
	                optlen = sizeof(int);
	                break;
	            case UdtOptions.UDT_SNDSYN:
	                *(bool*)optval = m_bSynSending;
	                optlen = sizeof(bool);
	                break;
	            case UdtOptions.UDT_RCVSYN:
	                *(bool*)optval = m_bSynRecving;
	                optlen = sizeof(bool);
	                break;
	            case UdtOptions.UDT_CC:
	                if (!m_bOpened)
	                {
	                    throw new UdtException(5, 5, 0);
	                }
	                //*(CC**)optval = m_pCC;
	                //optlen = sizeof(CC*);
	                break;
	            case UdtOptions.UDT_FC:
	                *(int*)optval = m_iFlightFlagSize;
	                optlen = sizeof(int);
	                break;
	            case UdtOptions.UDT_SNDBUF:
	                *(int*)optval = m_iSndBufSize * (m_iMSS - 28);
	                optlen = sizeof(int);
	                break;
	            case UdtOptions.UDT_RCVBUF:
	                *(int*)optval = m_iRcvBufSize * (m_iMSS - 28);
	                optlen = sizeof(int);
	                break;
	            case UdtOptions.UDT_LINGER:
	                if (optlen < 5) //?? (int)(sizeof(LingerOption))) 
	                {
	                    throw new UdtException(5, 3, 0);
	                }
	                ConvertLingerOption.ToVoidPointer(m_Linger, optval);
	                optlen = 5; //??
	                break;
	            case UdtOptions.UDP_SNDBUF:
	                *(int*)optval = m_iUDPSndBufSize;
	                optlen = sizeof(int);
	                break;
	            case UdtOptions.UDP_RCVBUF:
	                *(int*)optval = m_iUDPRcvBufSize;
	                optlen = sizeof(int);
	                break;
	            case UdtOptions.UDT_RENDEZVOUS:
	                *(bool*)optval = m_bRendezvous;
	                optlen = sizeof(bool);
	                break;
	            case UdtOptions.UDT_SNDTIMEO:
	                *(int*)optval = m_iSndTimeOut;
	                optlen = sizeof(int);
	                break;
	            case UdtOptions.UDT_RCVTIMEO:
	                *(int*)optval = m_iRcvTimeOut;
	                optlen = sizeof(int);
	                break;
	            case UdtOptions.UDT_REUSEADDR:
	                *(bool*)optval = m_bReuseAddr;
	                optlen = sizeof(bool);
	                break;
	            case UdtOptions.UDT_MAXBW:
	                *(long*)optval = m_llMaxBW;
	                optlen = sizeof(long);
	                break;
	            case UdtOptions.UDT_STATE:
	                *(int*)optval = (int)s_UDTUnited.getStatus(m_SocketID);
	                optlen = sizeof(int);
	                break;
	            case UdtOptions.UDT_EVENT:
	                {
	                    int udtevent = 0;
	                    if (m_bBroken)
	                    {
	                        udtevent |= (int)UdtEpollOptions.UDT_EPOLL_ERR;
	                    }
	                    else
	                    {
	                        if (m_pRcvBuffer != null && (m_pRcvBuffer.getRcvDataSize() > 0))
	                        {
	                            udtevent |= (int)UdtEpollOptions.UDT_EPOLL_IN;
	                        }
	                        if (m_pSndBuffer != null && (m_iSndBufSize > m_pSndBuffer.getCurrBufSize()))
	                        {
	                            udtevent |= (int)UdtEpollOptions.UDT_EPOLL_OUT;
	                        }
	                    }
	                    *(int*)optval = udtevent;
	                    optlen = sizeof(int);
	                }
	                break;
	            case UdtOptions.UDT_SNDDATA:
	                if (m_pSndBuffer != null)
	                {
	                     *(int*)optval = m_pSndBuffer.getCurrBufSize();
	                }
	                else
	                {
	                    *(int*)optval = 0;
	                }
	                optlen = sizeof(int);
	                break;
	            case UdtOptions.UDT_RCVDATA:
	                if (m_pRcvBuffer != null)
	                {
	                    *(int*)optval = m_pRcvBuffer.getRcvDataSize();
	                }
	                else
	                {
	                    *(int*)optval = 0;
	                }
	                optlen = sizeof(int);
	                break;
	            default:
	                throw new UdtException(5, 0, 0);
	        }
	    }
	    public unsafe void open()
	    {
	        lock (m_ConnectionLock)
	        {
	            open_unsafe();
	        }
	    }
	    unsafe void open_unsafe()
	    {
	        // Initial sequence number, loss, acknowledgement, etc.
	        m_iPktSize = m_iMSS - 28;
	        m_iPayloadSize = m_iPktSize - UdtPacket.packetHeaderSize;
	        m_iEXPCount = 1;
	        m_iBandwidth = 1;
	        m_iDeliveryRate = 16;
	        m_iAckSeqNo = 0;
	        m_ullLastAckTime = 0;
	        // trace information
	        m_StartTime = UdtTimer.getTime();
	        m_llSentTotal = m_llRecvTotal = m_iSndLossTotal = m_iRcvLossTotal = m_iRetransTotal = m_iSentACKTotal = m_iRecvACKTotal = m_iSentNAKTotal = m_iRecvNAKTotal = 0;
	        m_LastSampleTime = UdtTimer.getTime();
	        m_llTraceSent = m_llTraceRecv = m_iTraceSndLoss = m_iTraceRcvLoss = m_iTraceRetrans = m_iSentACK = m_iRecvACK = m_iSentNAK = m_iRecvNAK = 0;
	        m_llSndDuration = m_llSndDurationTotal = 0;
	        // structures for queue
	        if (null == m_pSNode)
	        {
	            m_pSNode = new SNode();
	        }
	        m_pSNode.m_pUDT = this;
	        m_pSNode.m_llTimeStamp = 1;
	        m_pSNode.m_iHeapLoc = -1;
	        if (null == m_pRNode)
	        {
	            m_pRNode = new RNode();
	        }
	        m_pRNode.m_pUDT = this;
	        m_pRNode.m_llTimeStamp = 1;
	        //m_pRNode.m_pPrev = m_pRNode.m_pNext = null;
	        m_pRNode.m_bOnList = false;
	        m_iRTT = 10 * m_iSYNInterval;
	        m_iRTTVar = m_iRTT >> 1;
	        m_ullCPUFrequency = UdtTimer.getCPUFrequency();
	        // set up the timers
	        m_ullSYNInt = m_iSYNInterval * m_ullCPUFrequency;
	        // set minimum NAK and EXP timeout to 100ms
	        m_ullMinNakInt = 300000 * m_ullCPUFrequency;
	        m_ullMinExpInt = 300000 * m_ullCPUFrequency;
	        m_ullACKInt = m_ullSYNInt;
	        m_ullNAKInt = m_ullMinNakInt;
	        ulong currtime = UdtTimer.rdtsc();
	        m_ullLastRspTime = currtime;
	        m_ullNextACKTime = currtime + m_ullSYNInt;
	        m_ullNextNAKTime = currtime + m_ullNAKInt;
	        m_iPktCount = 0;
	        m_iLightACKCount = 1;
	        m_ullTargetTime = 0;
	        m_ullTimeDiff = 0;
	        // Now UDT is opened.
	        m_bOpened = true;
	    }
	    public void listen()
	    {
	        lock (m_ConnectionLock)
	        {
	            listen_unsafe();
	        }
	    }
	    void listen_unsafe()
	    {
	        if (!m_bOpened)
	        {
	            throw new UdtException(5, 0, 0);
	        }
	        if (m_bConnecting || m_bConnected)
	        {
	            throw new UdtException(5, 2, 0);
	        }
	        // listen can be called more than once
	        if (m_bListening)
	        {
	            return;
	        }
	        // if there is already another socket listening on the same port
	        if (m_pRcvQueue.setListener(this) < 0)
	        {
	            throw new UdtException(5, 11, 0);
	        }
	        m_bListening = true;
	    }
	    public unsafe void connect(IPEndPoint serv_addr)
	    {
	        lock (m_ConnectionLock)
	        {
	            connect_unsafe(serv_addr);
	        }
	    }
	    unsafe void connect_unsafe(IPEndPoint serv_addr)
	    {
	        if (!m_bOpened)
	        {
	            throw new UdtException(5, 0, 0);
	        }
	        if (m_bListening)
	        {
	            throw new UdtException(5, 2, 0);
	        }
	        if (m_bConnecting || m_bConnected)
	        {
	            throw new UdtException(5, 2, 0);
	        }
	        m_bConnecting = true;
	        // record peer/server address
	        m_pPeerAddr = serv_addr;
	        // register this socket in the rendezvous queue
	        // RendezevousQueue is used to temporarily store incoming handshake, non-rendezvous connections also require this function
	        ulong ttl = 3000000;
	        if (m_bRendezvous)
	        {
	            ttl *= 10;
	        }
	        ttl += UdtTimer.getTime();
	        m_pRcvQueue.registerConnector(m_SocketID, this, m_iIPversion, serv_addr, ttl);
	        // This is my current configurations
	        m_ConnReq.Version = m_iVersion;
	        m_ConnReq.SocketType = m_iSockType;
	        m_ConnReq.MaximumSegmentSize = m_iMSS;
	        m_ConnReq.FlowControlWindowSize = (m_iRcvBufSize < m_iFlightFlagSize) ? m_iRcvBufSize : m_iFlightFlagSize;
	        m_ConnReq.RequestType = (!m_bRendezvous) ? 1 : 0;
	        m_ConnReq.SocketId = m_SocketID;
	        ConvertIPAddress.ToUintArray(serv_addr.Address, ref m_ConnReq.m_piPeerIP);
	        // Random Initial Sequence Number
	        Random rnd = new Random();
	        m_iISN = m_ConnReq.InitialSequenceNumber = rnd.Next(1, UdtSequenceNumber.m_iMaxSeqNo);
	        m_iLastDecSeq = m_iISN - 1;
	        m_iSndLastAck = m_iISN;
	        m_iSndLastDataAck = m_iISN;
	        m_iSndCurrSeqNo = m_iISN - 1;
	        m_iSndLastAck2 = m_iISN;
	        m_ullSndLastAck2Time = UdtTimer.getTime();
	        // Inform the server my configurations.
	        UdtPacket request = new UdtPacket();
	        request.pack(m_ConnReq);
	        // ID = 0, connection request
	        request.SetId(0);
	        m_pSndQueue.sendto(serv_addr, request);
	        m_llLastReqTime = (long)UdtTimer.getTime();
	        // asynchronous connect, return immediately
	        if (!m_bSynRecving)
	        {
	            return;
	        }
	        // Wait for the negotiated configurations from the peer side.
	        UdtPacket response = new UdtPacket();
	        byte[] resdata = new byte[m_iPayloadSize];
	        response.pack(0, resdata);
	        UdtException e = new UdtException(0, 0);
	        while (!m_bClosing)
	        {
	            // avoid sending too many requests, at most 1 request per 250ms
	            if ((long)UdtTimer.getTime() - m_llLastReqTime > 250000)
	            {
	                request.pack(m_ConnReq);
	                if (m_bRendezvous)
	                {
	                    request.SetId(m_ConnRes.SocketId);
	                }
	                m_pSndQueue.sendto(serv_addr, request);
	                m_llLastReqTime = (long)UdtTimer.getTime();
	            }
	            response.setLength(m_iPayloadSize);
	            if (m_pRcvQueue.recvfrom(m_SocketID, response) > 0)
	            {
	                if (connect(response) <= 0)
	                {
	                    break;
	                }
	                // new request/response should be sent out immediately on receving a response
	                m_llLastReqTime = 0;
	            }
	            if (UdtTimer.getTime() > ttl)
	            {
	                // timeout
	                e = new UdtException(1, 1, 0);
	                break;
	            }
	        }
	        if (e.getErrorCode() == 0)
	        {
	            if (m_bClosing)                                                 // if the socket is closed before connection...
	            {
	                e = new UdtException(1);
	            }
	            else if (1002 == m_ConnRes.RequestType)                          // connection request rejected
	            {
	                e = new UdtException(1, 2, 0);
	            }
	            else if ((!m_bRendezvous) && (m_iISN != m_ConnRes.InitialSequenceNumber))      // secuity check
	            {
	                e = new UdtException(1, 4, 0);
	            }
	        }
	        if (e.getErrorCode() != 0)
	        {
	            throw e;
	        }
	    }
	    public int connect(UdtPacket response)
	    {
	        // this is the 2nd half of a connection request. If the connection is setup successfully this returns 0.
	        // returning -1 means there is an error.
	        // returning 1 or 2 means the connection is in process and needs more handshake
	        if (!m_bConnecting)
	        {
	            return -1;
	        }
	        if (m_bRendezvous && ((0 == response.getFlag()) || (1 == response.getType())) && (0 != m_ConnRes.SocketType))
	        {
	            //a data packet or a keep-alive packet comes, which means the peer side is already connected
	            // in this situation, the previously recorded response will be used
	            goto POST_CONNECT;
	        }
	        if ((1 != response.getFlag()) || (0 != response.getType()))
	        {
	            return -1;
	        }
	        m_ConnRes.Deserialize(response.GetDataBytes(), response.getLength());
	        if (m_bRendezvous)
	        {
	            // regular connect should NOT communicate with rendezvous connect
	            // rendezvous connect require 3-way handshake
	            if (1 == m_ConnRes.RequestType)
	            {
	                return -1;
	            }
	            if ((0 == m_ConnReq.RequestType) || (0 == m_ConnRes.RequestType))
	            {
	                m_ConnReq.RequestType = -1;
	                // the request time must be updated so that the next handshake can be sent out immediately.
	                m_llLastReqTime = 0;
	                return 1;
	            }
	        }
	        else
	        {
	            // set cookie
	            if (1 == m_ConnRes.RequestType)
	            {
	                m_ConnReq.RequestType = -1;
	                m_ConnReq.Cookie = m_ConnRes.Cookie;
	                m_llLastReqTime = 0;
	                return 1;
	            }
	        }
	    POST_CONNECT:
	        // Remove from rendezvous queue
	        m_pRcvQueue.removeConnector(m_SocketID);
	        // Re-configure according to the negotiated values.
	        m_iMSS = m_ConnRes.MaximumSegmentSize;
	        m_iFlowWindowSize = m_ConnRes.FlowControlWindowSize;
	        m_iPktSize = m_iMSS - 28;
	        m_iPayloadSize = m_iPktSize - UdtPacket.packetHeaderSize;
	        m_iPeerISN = m_ConnRes.InitialSequenceNumber;
	        m_iRcvLastAck = m_ConnRes.InitialSequenceNumber;
	        m_iRcvLastAckAck = m_ConnRes.InitialSequenceNumber;
	        m_iRcvCurrSeqNo = m_ConnRes.InitialSequenceNumber - 1;
	        m_PeerID = m_ConnRes.SocketId;
	        Array.Copy(m_ConnRes.m_piPeerIP, m_piSelfIP, 4);
	        // Prepare all data structures
	        try
	        {
	            m_pSndBuffer = new UdtSenderBuffer(32, m_iPayloadSize);
	            m_pRcvBuffer = new UdtReceiverBuffer(m_iRcvBufSize);
	            // after introducing lite ACK, the sndlosslist may not be cleared in time, so it requires twice space.
	            m_pSndLossList = new UdtSenderLossList(m_iFlowWindowSize * 2);
	            m_pRcvLossList = new UdtReceiverLossList(m_iFlightFlagSize);
	            m_pACKWindow = new UdtAcknowledgementWindow(1024);
	            m_pRcvTimeWindow = new UdtPacketTimeWindow(16, 64);
	            m_pSndTimeWindow = new UdtPacketTimeWindow();
	        }
	        catch (Exception e)
	        {
	            throw new UdtException(3, 2, 0);
	        }
	        UdtInfoBlock ib;
	        if (m_pCache.TryGetValue(m_pPeerAddr.Address, out ib))
	        {
	            m_iRTT = ib.m_iRTT;
	            m_iBandwidth = ib.m_iBandwidth;
	        }
	        m_pCC = m_pCCFactory.create();
	        m_pCC.m_UDT = m_SocketID;
	        m_pCC.setMSS(m_iMSS);
	        m_pCC.SetMaximumCongestionWindowSize(m_iFlowWindowSize);
	        m_pCC.setSndCurrSeqNo(m_iSndCurrSeqNo);
	        m_pCC.setRcvRate(m_iDeliveryRate);
	        m_pCC.setRTT(m_iRTT);
	        m_pCC.setBandwidth(m_iBandwidth);
	        m_pCC.Initialize();
	        m_ullInterval = (ulong)(m_pCC.m_dPktSndPeriod * m_ullCPUFrequency);
	        m_dCongestionWindow = m_pCC.m_dCWndSize;
	        // And, I am connected too.
	        m_bConnecting = false;
	        m_bConnected = true;
	        // register this socket for receiving data packets
	        m_pRNode.m_bOnList = true;
	        m_pRcvQueue.setNewEntry(this);
	        // acknowledge the management module.
	        s_UDTUnited.connect_complete(m_SocketID);
	        // acknowledde any waiting epolls to write
	        //s_UDTUnited.m_EPoll.update_events(m_SocketID, m_sPollID, EPOLLOpt.UDT_EPOLL_OUT, true);
	        return 0;
	    }
	    public unsafe void connect(IPEndPoint peer, UdtHandshake hs)
	    {
	        lock (m_ConnectionLock)
	        {
	            connect_unsafe(peer, hs);
	        }
	    }
	    unsafe void connect_unsafe(IPEndPoint peer, UdtHandshake hs)
	    {
	        // Uses the smaller MSS between the peers
	        if (hs.MaximumSegmentSize > m_iMSS)
	        {
	            hs.MaximumSegmentSize = m_iMSS;
	        }
	        else
	        {
	            m_iMSS = hs.MaximumSegmentSize;
	        }
	        // exchange info for maximum flow window size
	        m_iFlowWindowSize = hs.FlowControlWindowSize;
	        hs.FlowControlWindowSize = (m_iRcvBufSize < m_iFlightFlagSize) ? m_iRcvBufSize : m_iFlightFlagSize;
	        m_iPeerISN = hs.InitialSequenceNumber;
	        m_iRcvLastAck = hs.InitialSequenceNumber;
	        m_iRcvLastAckAck = hs.InitialSequenceNumber;
	        m_iRcvCurrSeqNo = hs.InitialSequenceNumber - 1;
	        m_PeerID = hs.SocketId;
	        hs.SocketId = m_SocketID;
	        // use peer's ISN and send it back for security check
	        m_iISN = hs.InitialSequenceNumber;
	        m_iLastDecSeq = m_iISN - 1;
	        m_iSndLastAck = m_iISN;
	        m_iSndLastDataAck = m_iISN;
	        m_iSndCurrSeqNo = m_iISN - 1;
	        m_iSndLastAck2 = m_iISN;
	        m_ullSndLastAck2Time = UdtTimer.getTime();
	        // this is a reponse handshake
	        hs.RequestType = -1;
	        // get local IP address and send the peer its IP address (because UDP cannot get local IP address)
	        Array.Copy(hs.m_piPeerIP, m_piSelfIP, 4);
	        ConvertIPAddress.ToUintArray(peer.Address, ref hs.m_piPeerIP);
	        m_iPktSize = m_iMSS - 28;
	        m_iPayloadSize = m_iPktSize - UdtPacket.packetHeaderSize;
	        // Prepare all structures
	        try
	        {
	            m_pSndBuffer = new UdtSenderBuffer(32, m_iPayloadSize);
	            m_pRcvBuffer = new UdtReceiverBuffer(m_iRcvBufSize);
	            m_pSndLossList = new UdtSenderLossList(m_iFlowWindowSize * 2);
	            m_pRcvLossList = new UdtReceiverLossList(m_iFlightFlagSize);
	            m_pACKWindow = new UdtAcknowledgementWindow(1024);
	            m_pRcvTimeWindow = new UdtPacketTimeWindow(16, 64);
	            m_pSndTimeWindow = new UdtPacketTimeWindow();
	        }
	        catch (Exception e)
	        {
	            throw new UdtException(3, 2, 0);
	        }
	        UdtInfoBlock ib;
	        if (m_pCache.TryGetValue(peer.Address, out ib))
	        {
	            m_iRTT = ib.m_iRTT;
	            m_iBandwidth = ib.m_iBandwidth;
	        }
	        m_pCC = m_pCCFactory.create();
	        m_pCC.m_UDT = m_SocketID;
	        m_pCC.setMSS(m_iMSS);
	        m_pCC.SetMaximumCongestionWindowSize(m_iFlowWindowSize);
	        m_pCC.setSndCurrSeqNo(m_iSndCurrSeqNo);
	        m_pCC.setRcvRate(m_iDeliveryRate);
	        m_pCC.setRTT(m_iRTT);
	        m_pCC.setBandwidth(m_iBandwidth);
	        m_pCC.Initialize();
	        m_ullInterval = (ulong)(m_pCC.m_dPktSndPeriod * m_ullCPUFrequency);
	        m_dCongestionWindow = m_pCC.m_dCWndSize;
	        m_pPeerAddr = peer;
	        // And of course, it is connected.
	        m_bConnected = true;
	        // register this socket for receiving data packets
	        m_pRNode.m_bOnList = true;
	        m_pRcvQueue.setNewEntry(this);
	        //send the response to the peer, see listen() for more discussions about this
	        UdtPacket response = new UdtPacket();
	        response.pack(hs);
	        response.SetId(m_PeerID);
	        m_pSndQueue.sendto(peer, response);
	    }
	    public unsafe void close()
	    {
	        if (!m_bOpened)
	        {
	            return;
	        }
	        if (m_Linger.Enabled)
	        {
	            ulong entertime = UdtTimer.getTime();
	            while (!m_bBroken && m_bConnected && (m_pSndBuffer.getCurrBufSize() > 0) && (UdtTimer.getTime() - entertime < (ulong)m_Linger.LingerTime * 1000000))
	            {
	                // linger has been checked by previous close() call and has expired
	                if (m_ullLingerExpiration >= entertime)
	                {
	                    break;
	                }
	                if (!m_bSynSending)
	                {
	                    // if this socket enables asynchronous sending, return immediately and let GC to close it later
	                    if (0 == m_ullLingerExpiration)
	                    {
	                        m_ullLingerExpiration = entertime + (ulong)m_Linger.LingerTime * 1000000;
	                    }
	                    return;
	                }
	                System.Threading.Thread.Sleep(1);
	            }
	        }
	        // remove this socket from the snd queue
	        if (m_bConnected)
	        {
	            m_pSndQueue.m_pSndUList.remove(this);
	        }
	        // trigger any pending IO events.
	        //s_UDTUnited.m_EPoll.update_events(m_SocketID, m_sPollID, EPOLLOpt.UDT_EPOLL_ERR, true);
	        // then remove itself from all epoll monitoring
	        //try
	        //{
	        //    for (set<int>.iterator i = m_sPollID.begin(); i != m_sPollID.end(); ++i)
	        //        s_UDTUnited.m_EPoll.remove_usock(* i, m_SocketID);
	        //}
	        //catch (Exception e)
	        //{
	        //}
	        if (!m_bOpened)
	        {
	            return;
	        }
	        // Inform the threads handler to stop.
	        m_bClosing = true;
	        lock (m_ConnectionLock)
	        {
	            close_unsafe();
	        }
	        // waiting all send and recv calls to stop
	        lock (m_SendLock) lock (m_RecvLock)
	            { }
	        // CLOSED.
	        m_bOpened = false;
	    }
	    unsafe void close_unsafe()
	    {
	        // Signal the sender and recver if they are waiting for data.
	        releaseSynch();
	        if (m_bListening)
	        {
	            m_bListening = false;
	            m_pRcvQueue.removeListener(this);
	        }
	        else if (m_bConnecting)
	        {
	            m_pRcvQueue.removeConnector(m_SocketID);
	        }
	        if (m_bConnected)
	        {
	            if (!m_bShutdown)
	            {
	                sendCtrl(5);
	            }
	            m_pCC.Close();
	            // Store current connection information.
	            UdtInfoBlock ib;
	            if (!m_pCache.TryGetValue(m_pPeerAddr.Address, out ib))
	            {
	                ib = new UdtInfoBlock(m_pPeerAddr.Address);
	                m_pCache[m_pPeerAddr.Address] = ib;
	            }
	            ib.m_iRTT = m_iRTT;
	            ib.m_iBandwidth = m_iBandwidth;
	            m_bConnected = false;
	        }
	    }
	    public int send(byte[] data, int offset, int len)
	    {   /* error with major 5 and minor 10 meant "This operation is not supported in SOCK_DGRAM mode"
	        if (SocketType.Dgram == m_iSockType)
	            throw new UdtException(5, 10, 0);
	        */
	        // throw an exception if not connected
	        if (m_bBroken || m_bClosing)
	        {
	            throw new UdtException(2, 1, 0);
	        }
	        else if (!m_bConnected)
	        {
	            throw new UdtException(2, 2, 0);
	        }
	        if (len <= 0)
	        {
	            return 0;
	        }
	        if (offset + len > data.Length)
	        {
	            len = data.Length - offset;
	        }
	        lock (m_SendLock)
	        {
	            return send_unsafe(data, offset, len);
	        }
	    }
	    int send_unsafe(byte[] data, int offset, int len)
	    {
	        if (m_pSndBuffer.getCurrBufSize() == 0)
	        {
	            // delay the EXP timer to avoid mis-fired timeout
	            ulong currtime = UdtTimer.rdtsc();
	            m_ullLastRspTime = currtime;
	        }
	        if (m_iSndBufSize <= m_pSndBuffer.getCurrBufSize())
	        {
	            if (!m_bSynSending)
	            {
	                throw new UdtException(6, 1, 0);
	            }
	            else
	            {
	                // wait here during a blocking sending
	                if (m_iSndTimeOut < 0)
	                {
	                    while (!m_bBroken && m_bConnected && !m_bClosing && (m_iSndBufSize <= m_pSndBuffer.getCurrBufSize()) && m_bPeerHealth)
	                    {
	                        m_SendBlockCond.WaitOne(Timeout.Infinite);
	                    }
	                }
	                else
	                {
	                    ulong exptime = UdtTimer.getTime() + (ulong)m_iSndTimeOut * 1000;
	                    while (!m_bBroken && m_bConnected && !m_bClosing && (m_iSndBufSize <= m_pSndBuffer.getCurrBufSize()) && m_bPeerHealth && (UdtTimer.getTime() < exptime))
	                    {
	                        m_SendBlockCond.WaitOne((int)(exptime - UdtTimer.getTime()) / 1000);
	                    }
	                }
	                // check the connection status
	                if (m_bBroken || m_bClosing)
	                {
	                    throw new UdtException(2, 1, 0);
	                }
	                else if (!m_bConnected)
	                {
	                    throw new UdtException(2, 2, 0);
	                }
	                else if (!m_bPeerHealth)
	                {
	                    m_bPeerHealth = true;
	                    throw new UdtException(7);
	                }
	            }
	        }
	        if (m_iSndBufSize <= m_pSndBuffer.getCurrBufSize())
	        {
	            if (m_iSndTimeOut >= 0)
	            {
	                throw new UdtException(6, 3, 0);
	            }
	            return 0;
	        }
	        int size = (m_iSndBufSize - m_pSndBuffer.getCurrBufSize()) * m_iPayloadSize;
	        if (size > len)
	        {
	            size = len;
	        }
	        // record total time used for sending
	        if (0 == m_pSndBuffer.getCurrBufSize())
	        {
	            m_llSndDurationCounter = (long)UdtTimer.getTime();
	        }
	        // insert the user buffer into the sening list
	        m_pSndBuffer.addBuffer(data, offset, size);
	        // insert this socket to snd list if it is not on the list yet
	        m_pSndQueue.m_pSndUList.update(this, false);
	        //if (m_iSndBufSize <= m_pSndBuffer.getCurrBufSize())
	        //{
	        //    // write is not available any more
	        //    s_UDTUnited.m_EPoll.update_events(m_SocketID, m_sPollID, EPOLLOpt.UDT_EPOLL_OUT, false);
	        //}
	        return size;
	    }
	    public int recv(byte[] data, int offset, int len)
	    {   /* error with major 5 and minor 10 meant "This operation is not supported in SOCK_DGRAM mode"
	        if (SocketType.Dgram == m_iSockType)
	            throw new UdtException(5, 10, 0);
	        */
	        // throw an exception if not connected
	        if (!m_bConnected)
	        {
	            throw new UdtException(2, 2, 0);
	        }
	        else if ((m_bBroken || m_bClosing) && (0 == m_pRcvBuffer.getRcvDataSize()))
	        {
	            throw new UdtException(2, 1, 0);
	        }
	        if (len <= 0)
	        {
	            return 0;
	        }
	        lock (m_RecvLock)
	        {
	            return recv_unsafe(data, offset, len);
	        }
	    }
	    int recv_unsafe(byte[] data, int offset, int len)
	    {
	        if (0 == m_pRcvBuffer.getRcvDataSize())
	        {
	            if (!m_bSynRecving)
	            {
	                throw new UdtException(6, 2, 0);
	            }
	            else
	            {
	                if (m_iRcvTimeOut < 0)
	                {
	                    while (!m_bBroken && m_bConnected && !m_bClosing && (0 == m_pRcvBuffer.getRcvDataSize()))
	                    {
	                        m_RecvDataCond.WaitOne(Timeout.Infinite);
	                    }
	                }
	                else
	                {
	                    ulong enter_time = UdtTimer.getTime();
	                    while (!m_bBroken && m_bConnected && !m_bClosing && (0 == m_pRcvBuffer.getRcvDataSize()))
	                    {
	                        int diff = (int)(UdtTimer.getTime() - enter_time) / 1000;
	                        if (diff >= m_iRcvTimeOut)
	                        {
	                            break;
	                        }
	                        m_RecvDataCond.WaitOne(m_iRcvTimeOut - diff);
	                    }
	                }
	            }
	        }
	        // throw an exception if not connected
	        if (!m_bConnected)
	        {
	            throw new UdtException(2, 2, 0);
	        }
	        else if ((m_bBroken || m_bClosing) && (0 == m_pRcvBuffer.getRcvDataSize()))
	        {
	            throw new UdtException(2, 1, 0);
	        }
	        int res = m_pRcvBuffer.readBuffer(data, offset, len);
	        //if (m_pRcvBuffer.getRcvDataSize() <= 0)
	        //{
	        //    // read is not available any more
	        //    s_UDTUnited.m_EPoll.update_events(m_SocketID, m_sPollID, UDT_EPOLL_IN, false);
	        //}
	        if ((res <= 0) && (m_iRcvTimeOut >= 0))
	        {
	            throw new UdtException(6, 3, 0);
	        }
	        return res;
	    }
	    public int sendmsg(byte[] data, int offset, int len, int msttl, bool inorder)
	    {
	        if (SocketType.Stream == m_iSockType)
	        {
	            throw new UdtException(5, 9, 0);
	        }
	        // throw an exception if not connected
	        if (m_bBroken || m_bClosing)
	        {
	            throw new UdtException(2, 1, 0);
	        }
	        else if (!m_bConnected)
	        {
	            throw new UdtException(2, 2, 0);
	        }
	        if (len <= 0)
	        {
	            return 0;
	        }
	        if (len + offset > data.Length)
	        {
	            len = data.Length - offset;
	        }
	        if (len > m_iSndBufSize * m_iPayloadSize)
	        {
	            throw new UdtException(5, 12, 0);
	        }
	        lock (m_SendLock)
	        {
	            return sendmsg_unsafe(data, offset, len, msttl, inorder);
	        }
	    }
	    int sendmsg_unsafe(byte[] data, int offset, int len, int msttl, bool inorder)
	    {
	        if (m_pSndBuffer.getCurrBufSize() == 0)
	        {
	            // delay the EXP timer to avoid mis-fired timeout
	            m_ullLastRspTime = UdtTimer.rdtsc();
	        }
	        if ((m_iSndBufSize - m_pSndBuffer.getCurrBufSize()) * m_iPayloadSize < len)
	        {
	            if (!m_bSynSending)
	            {
	                throw new UdtException(6, 1, 0);
	            }
	            else
	            {
	                // wait here during a blocking sending
	                if (m_iSndTimeOut < 0)
	                {
	                    while (!m_bBroken && m_bConnected && !m_bClosing && ((m_iSndBufSize - m_pSndBuffer.getCurrBufSize()) * m_iPayloadSize < len))
	                    {
	                        m_SendBlockCond.WaitOne(Timeout.Infinite);
	                    }
	                }
	                else
	                {
	                    ulong exptime = UdtTimer.getTime() + (ulong)m_iSndTimeOut * 1000;
	                    while (!m_bBroken && m_bConnected && !m_bClosing && ((m_iSndBufSize - m_pSndBuffer.getCurrBufSize()) * m_iPayloadSize < len) && (UdtTimer.getTime() < exptime))
	                    {
	                        m_SendBlockCond.WaitOne((int)(exptime - UdtTimer.getTime()) / 1000);
	                    }
	                }
	                // check the connection status
	                if (m_bBroken || m_bClosing)
	                {
	                    throw new UdtException(2, 1, 0);
	                }
	                else if (!m_bConnected)
	                {
	                    throw new UdtException(2, 2, 0);
	                }
	            }
	        }
	        if ((m_iSndBufSize - m_pSndBuffer.getCurrBufSize()) * m_iPayloadSize < len)
	        {
	            if (m_iSndTimeOut >= 0)
	            {
	                throw new UdtException(6, 3, 0);
	            }
	            return 0;
	        }
	        // record total time used for sending
	        if (0 == m_pSndBuffer.getCurrBufSize())
	        {
	            m_llSndDurationCounter = (long)UdtTimer.getTime();
	        }
	        // insert the user buffer into the sening list
	        m_pSndBuffer.addBuffer(data, offset, len, msttl, inorder);
	        // insert this socket to the snd list if it is not on the list yet
	        m_pSndQueue.m_pSndUList.update(this, false);
	        //if (m_iSndBufSize <= m_pSndBuffer.getCurrBufSize())
	        //{
	        //    // write is not available any more
	        //    s_UDTUnited.m_EPoll.update_events(m_SocketID, m_sPollID, UDT_EPOLL_OUT, false);
	        //}
	        return len;
	    }
	    public int recvmsg(byte[] data, int len)
	    {
	        if (SocketType.Stream == m_iSockType)
	        {
	            throw new UdtException(5, 9, 0);
	        }
	        // throw an exception if not connected
	        if (!m_bConnected)
	        {
	            throw new UdtException(2, 2, 0);
	        }
	        if (len <= 0)
	        {
	            return 0;
	        }
	        lock (m_RecvLock)
	        {
	            return recvmsg_unsafe(data, len);
	        }
	    }
	    int recvmsg_unsafe(byte[] data, int len)
	    {
	        int res = 0;
	        if (m_bBroken || m_bClosing)
	        {
	            res = m_pRcvBuffer.readMsg(data, len);
	            //if (m_pRcvBuffer.getRcvMsgNum() <= 0)
	            //{
	            //    // read is not available any more
	            //    s_UDTUnited.m_EPoll.update_events(m_SocketID, m_sPollID, EPOLLOpt.UDT_EPOLL_IN, false);
	            //}
	            if (0 == res)
	            {
	                throw new UdtException(2, 1, 0);
	            }
	            else
	            {
	                return res;
	            }
	        }
	        if (!m_bSynRecving)
	        {
	            res = m_pRcvBuffer.readMsg(data, len);
	            if (0 == res)
	            {
	                throw new UdtException(6, 2, 0);
	            }
	            else
	            {
	                return res;
	            }
	        }
	        bool timeout = false;
	        do
	        {
	            if (m_iRcvTimeOut < 0)
	            {
	                while (!m_bBroken && m_bConnected && !m_bClosing && (0 == (res = m_pRcvBuffer.readMsg(data, len))))
	                {
	                    m_RecvDataCond.WaitOne(Timeout.Infinite);
	                }
	            }
	            else
	            {
	                timeout = !m_RecvDataCond.WaitOne(m_iRcvTimeOut);
	                res = m_pRcvBuffer.readMsg(data, len);
	            }
	            if (m_bBroken || m_bClosing)
	            {
	                throw new UdtException(2, 1, 0);
	            }
	            else if (!m_bConnected)
	            {
	                throw new UdtException(2, 2, 0);
	            }
	        }
	        while ((0 == res) && !timeout);
	        //if (m_pRcvBuffer.getRcvMsgNum() <= 0)
	        //{
	        //    // read is not available any more
	        //    s_UDTUnited.m_EPoll.update_events(m_SocketID, m_sPollID, UDT_EPOLL_IN, false);
	        //}
	        if ((res <= 0) && (m_iRcvTimeOut >= 0))
	        {
	            throw new UdtException(6, 3, 0);
	        }
	        return res;
	    }
	    public void sample(PerfMon perf, bool clear)
	    {
	        if (!m_bConnected)
	        {
	            throw new UdtException(2, 2, 0);
	        }
	        if (m_bBroken || m_bClosing)
	        {
	            throw new UdtException(2, 1, 0);
	        }
	        ulong currtime = UdtTimer.getTime();
	        perf.msTimeStamp = (long)(currtime - m_StartTime) / 1000;
	        perf.pktSent = m_llTraceSent;
	        perf.pktRecv = m_llTraceRecv;
	        perf.pktSndLoss = m_iTraceSndLoss;
	        perf.pktRcvLoss = m_iTraceRcvLoss;
	        perf.pktRetrans = m_iTraceRetrans;
	        perf.pktSentACK = m_iSentACK;
	        perf.pktRecvACK = m_iRecvACK;
	        perf.pktSentNAK = m_iSentNAK;
	        perf.pktRecvNAK = m_iRecvNAK;
	        perf.usSndDuration = m_llSndDuration;
	        perf.pktSentTotal = m_llSentTotal;
	        perf.pktRecvTotal = m_llRecvTotal;
	        perf.pktSndLossTotal = m_iSndLossTotal;
	        perf.pktRcvLossTotal = m_iRcvLossTotal;
	        perf.pktRetransTotal = m_iRetransTotal;
	        perf.pktSentACKTotal = m_iSentACKTotal;
	        perf.pktRecvACKTotal = m_iRecvACKTotal;
	        perf.pktSentNAKTotal = m_iSentNAKTotal;
	        perf.pktRecvNAKTotal = m_iRecvNAKTotal;
	        perf.usSndDurationTotal = m_llSndDurationTotal;
	        double interval = (double)(currtime - m_LastSampleTime);
	        perf.mbpsSendRate = (double)(m_llTraceSent) * m_iPayloadSize * 8.0 / interval;
	        perf.mbpsRecvRate = (double)(m_llTraceRecv) * m_iPayloadSize * 8.0 / interval;
	        perf.usPktSndPeriod = m_ullInterval / (double)m_ullCPUFrequency;
	        perf.pktFlowWindow = m_iFlowWindowSize;
	        perf.pktCongestionWindow = (int)m_dCongestionWindow;
	        perf.pktFlightSize = UdtSequenceNumber.seqlen(m_iSndLastAck, UdtSequenceNumber.incseq(m_iSndCurrSeqNo)) - 1;
	        perf.msRTT = m_iRTT / 1000.0;
	        perf.mbpsBandwidth = m_iBandwidth * m_iPayloadSize * 8.0 / 1000000.0;
	        if (Monitor.TryEnter(m_ConnectionLock))
	        {
	            perf.byteAvailSndBuf = (null == m_pSndBuffer) ? 0 : (m_iSndBufSize - m_pSndBuffer.getCurrBufSize()) * m_iMSS;
	            perf.byteAvailRcvBuf = (null == m_pRcvBuffer) ? 0 : m_pRcvBuffer.getAvailBufSize() * m_iMSS;
	            Monitor.Exit(m_ConnectionLock);
	        }
	        else
	        {
	            perf.byteAvailSndBuf = 0;
	            perf.byteAvailRcvBuf = 0;
	        }
	        if (clear)
	        {
	            m_llTraceSent = m_llTraceRecv = m_iTraceSndLoss = m_iTraceRcvLoss = m_iTraceRetrans = m_iSentACK = m_iRecvACK = m_iSentNAK = m_iRecvNAK = 0;
	            m_llSndDuration = 0;
	            m_LastSampleTime = currtime;
	        }
	    }
	    void CCUpdate()
	    {
	        m_ullInterval = (ulong)(m_pCC.m_dPktSndPeriod * m_ullCPUFrequency);
	        m_dCongestionWindow = m_pCC.m_dCWndSize;
	        if (m_llMaxBW <= 0)
	        {
	            return;
	        }
	        double minSP = 1000000.0 / ((double)m_llMaxBW / m_iMSS) * m_ullCPUFrequency;
	        if (m_ullInterval < minSP)
	        {
	            m_ullInterval = (ulong)minSP;
	        }
	    }
	    void destroySynch()
	    {
	        m_SendBlockCond.Close();
	        m_RecvDataCond.Close();
	    }
	    void releaseSynch()
	    {
	        m_SendBlockCond.Set();
	        bool gotLock = false;
	        try
	        {
	            Monitor.Enter(m_SendLock, ref gotLock);
	        }
	        finally
	        {
	            if (gotLock)
	            {
	                Monitor.Exit(m_SendLock);
	            }
	        }
	        m_RecvDataCond.Set();
	        gotLock = false;
	        try
	        {
	            Monitor.Enter(m_RecvLock, ref gotLock);
	        }
	        finally
	        {
	            if (gotLock)
	            {
	                Monitor.Exit(m_RecvLock);
	            }
	        }
	    }
	    unsafe void sendCtrl(int pkttype, void* lparam = null, void* rparam = null, int size = 0)
	    {
	        UdtPacket ctrlpkt = new UdtPacket();
	        switch (pkttype)
	        {
	            case 2: //010 - Acknowledgement
	                {
	                    int ack;
	                    // If there is no loss, the ACK is the current largest sequence number plus 1;
	                    // Otherwise it is the smallest sequence number in the receiver loss list.
	                    if (0 == m_pRcvLossList.getLossLength())
	                    {
	                        ack = UdtSequenceNumber.incseq(m_iRcvCurrSeqNo);
	                    }
	                    else
	                    {
	                        ack = m_pRcvLossList.getFirstLostSeq();
	                    }
	                    if (ack == m_iRcvLastAckAck)
	                    {
	                        break;
	                    }
	                    // send out a lite ACK
	                    // to save time on buffer processing and bandwidth/AS measurement, a lite ACK only feeds back an ACK number
	                    if (4 == size)
	                    {
	                        ctrlpkt.pack(pkttype, null, &ack, size);
	                        ctrlpkt.SetId(m_PeerID);
	                        m_pSndQueue.sendto(m_pPeerAddr, ctrlpkt);
	                        break;
	                    }
	                    ulong currtime = UdtTimer.rdtsc();
	                    // There are new received packets to acknowledge, update related information.
	                    if (UdtSequenceNumber.seqcmp(ack, m_iRcvLastAck) > 0)
	                    {
	                        int acksize = UdtSequenceNumber.seqoff(m_iRcvLastAck, ack);
	                        m_iRcvLastAck = ack;
	                        m_pRcvBuffer.ackData(acksize);
	                        // signal a waiting "recv" call if there is any data available
	                        if (m_bSynRecving)
	                        {
	                            m_RecvDataCond.Set();
	                        }
	                        // acknowledge any waiting epolls to read
	                        //s_UDTUnited.m_EPoll.update_events(m_SocketID, m_sPollID, EPOLLOpt.UDT_EPOLL_IN, true);
	                    }
	                    else if (ack == m_iRcvLastAck)
	                    {
	                        if ((currtime - m_ullLastAckTime) < ((ulong)(m_iRTT + 4 * m_iRTTVar) * m_ullCPUFrequency))
	                        {
	                            break;
	                        }
	                    }
	                    else
	                    {
	                        break;
	                    }
	                    // Send out the ACK only if has not been received by the sender before
	                    if (UdtSequenceNumber.seqcmp(m_iRcvLastAck, m_iRcvLastAckAck) > 0)
	                    {
	                        int[] data = new int[6];
	                        m_iAckSeqNo = UdtAcknowledgementNumber.incack(m_iAckSeqNo);
	                        data[0] = m_iRcvLastAck;
	                        data[1] = m_iRTT;
	                        data[2] = m_iRTTVar;
	                        data[3] = m_pRcvBuffer.getAvailBufSize();
	                        // a minimum flow window of 2 is used, even if buffer is full, to break potential deadlock
	                        if (data[3] < 2)
	                        {
	                            data[3] = 2;
	                        }
	                        if (currtime - m_ullLastAckTime > m_ullSYNInt)
	                        {
	                            data[4] = m_pRcvTimeWindow.getPktRcvSpeed();
	                            data[5] = m_pRcvTimeWindow.getBandwidth();
	                            ctrlpkt.pack(pkttype, m_iAckSeqNo, data);
	                            m_ullLastAckTime = UdtTimer.rdtsc();
	                        }
	                        else
	                        {
	                            ctrlpkt.pack(pkttype, m_iAckSeqNo, data, 4);
	                        }
	                        ctrlpkt.SetId(m_PeerID);
	                        m_pSndQueue.sendto(m_pPeerAddr, ctrlpkt);
	                        m_pACKWindow.Store(m_iAckSeqNo, m_iRcvLastAck);
	                        ++m_iSentACK;
	                        ++m_iSentACKTotal;
	                    }
	                    break;
	                }
	            case 6: //110 - Acknowledgement of Acknowledgement
	                ctrlpkt.pack(pkttype, lparam);
	                ctrlpkt.SetId(m_PeerID);
	                m_pSndQueue.sendto(m_pPeerAddr, ctrlpkt);
	                break;
	            case 3: //011 - Loss Report
	                {
	                    if (null != rparam)
	                    {
	                        if (1 == size)
	                        {
	                            // only 1 loss packet
	                            ctrlpkt.pack(pkttype, null, (int*)rparam + 1, 4);
	                        }
	                        else
	                        {
	                            // more than 1 loss packets
	                            ctrlpkt.pack(pkttype, null, rparam, 8);
	                        }
	                        ctrlpkt.SetId(m_PeerID);
	                        m_pSndQueue.sendto(m_pPeerAddr, ctrlpkt);
	                        ++m_iSentNAK;
	                        ++m_iSentNAKTotal;
	                    }
	                    else if (m_pRcvLossList.getLossLength() > 0)
	                    {
	                        // this is periodically NAK report; make sure NAK cannot be sent back too often
	                        // read loss list from the local receiver loss list
	                        int[] data = new int[m_iPayloadSize / 4];
	                        int losslen;
	                        m_pRcvLossList.getLossArray(data, out losslen, m_iPayloadSize / 4);
	                        if (0 < losslen)
	                        {
	                            ctrlpkt.pack(pkttype, data, losslen);
	                            ctrlpkt.SetId(m_PeerID);
	                            m_pSndQueue.sendto(m_pPeerAddr, ctrlpkt);
	                            ++m_iSentNAK;
	                            ++m_iSentNAKTotal;
	                        }
	                    }
	                    // update next NAK time, which should wait enough time for the retansmission, but not too long
	                    m_ullNAKInt = (ulong)(m_iRTT + 4 * m_iRTTVar) * m_ullCPUFrequency;
	                    int rcv_speed = m_pRcvTimeWindow.getPktRcvSpeed();
	                    if (rcv_speed > 0)
	                    {
	                        m_ullNAKInt += (ulong)(m_pRcvLossList.getLossLength() * 1000000 / rcv_speed) * m_ullCPUFrequency;
	                    }
	                    if (m_ullNAKInt < m_ullMinNakInt)
	                    {
	                        m_ullNAKInt = m_ullMinNakInt;
	                    }
	                    break;
	                }
	            case 4: //100 - Congestion Warning
	                ctrlpkt.pack(pkttype);
	                ctrlpkt.SetId(m_PeerID);
	                m_pSndQueue.sendto(m_pPeerAddr, ctrlpkt);
	                m_ullLastWarningTime = UdtTimer.rdtsc();
	                break;
	            case 1: //001 - Keep-alive
	                ctrlpkt.pack(pkttype);
	                ctrlpkt.SetId(m_PeerID);
	                m_pSndQueue.sendto(m_pPeerAddr, ctrlpkt);
	                break;
	            case 0: //000 - Handshake
	                ctrlpkt.pack(pkttype, null, rparam, UdtHandshake.ContentSize);
	                ctrlpkt.SetId(m_PeerID);
	                m_pSndQueue.sendto(m_pPeerAddr, ctrlpkt);
	                break;
	            case 5: //101 - Shutdown
	                ctrlpkt.pack(pkttype);
	                ctrlpkt.SetId(m_PeerID);
	                m_pSndQueue.sendto(m_pPeerAddr, ctrlpkt);
	                break;
	            case 7: //111 - Msg drop request
	                ctrlpkt.pack(pkttype, lparam, rparam, 8);
	                ctrlpkt.SetId(m_PeerID);
	                m_pSndQueue.sendto(m_pPeerAddr, ctrlpkt);
	                break;
	            case 8: //1000 - acknowledge the peer side a special error
	                ctrlpkt.pack(pkttype, lparam);
	                ctrlpkt.SetId(m_PeerID);
	                m_pSndQueue.sendto(m_pPeerAddr, ctrlpkt);
	                break;
	            case 32767: //0x7FFF - Resevered for future use
	                break;
	            default:
	                break;
	        }
	    }
	    public unsafe void processCtrl(UdtPacket ctrlpkt)
	    {
	        // Just heard from the peer, reset the expiration count.
	        m_iEXPCount = 1;
	        ulong currtime = UdtTimer.rdtsc();
	        m_ullLastRspTime = currtime;
	        switch (ctrlpkt.getType())
	        {
	            case 2: //010 - Acknowledgement
	                {
	                    int ack;
	                    // process a lite ACK
	                    if (4 == ctrlpkt.getLength())
	                    {
	                        ack = ctrlpkt.GetIntFromData(0);
	                        if (UdtSequenceNumber.seqcmp(ack, m_iSndLastAck) >= 0)
	                        {
	                            m_iFlowWindowSize -= UdtSequenceNumber.seqoff(m_iSndLastAck, ack);
	                            m_iSndLastAck = ack;
	                        }
	                        break;
	                    }
	                    // read ACK seq. no.
	                    ack = ctrlpkt.getAckSeqNo();
	                    // send ACK acknowledgement
	                    // number of ACK2 can be much less than number of ACK
	                    ulong now = UdtTimer.getTime();
	                    if ((now - m_ullSndLastAck2Time > m_iSYNInterval) || (ack == m_iSndLastAck2))
	                    {
	                        sendCtrl(6, &ack);
	                        m_iSndLastAck2 = ack;
	                        m_ullSndLastAck2Time = now;
	                    }
	                    // Got data ACK
	                    ack = ctrlpkt.GetIntFromData(0);
	                    // check the validation of the ack
	                    if (UdtSequenceNumber.seqcmp(ack, UdtSequenceNumber.incseq(m_iSndCurrSeqNo)) > 0)
	                    {
	                        //this should not happen: attack or bug
	                        m_bBroken = true;
	                        m_iBrokenCounter = 0;
	                        break;
	                    }
	                    if (UdtSequenceNumber.seqcmp(ack, m_iSndLastAck) >= 0)
	                    {
	                        // Update Flow Window Size, must update before and together with m_iSndLastAck
	                        m_iFlowWindowSize = ctrlpkt.GetIntFromData(3);
	                        m_iSndLastAck = ack;
	                    }
	                    // protect packet retransmission
	                    bool bLockTaken = false;
	                    Monitor.Enter(m_AckLock, ref bLockTaken);
	                    int offset = UdtSequenceNumber.seqoff(m_iSndLastDataAck, ack);
	                    if (offset <= 0)
	                    {
	                        // discard it if it is a repeated ACK
	                        if (bLockTaken)
	                        {
	                            Monitor.Exit(m_AckLock);
	                        }
	                        break;
	                    }
	                    // acknowledge the sending buffer
	                    m_pSndBuffer.ackData(offset);
	                    // record total time used for sending
	                    m_llSndDuration += (long)now - m_llSndDurationCounter;
	                    m_llSndDurationTotal += (long)now - m_llSndDurationCounter;
	                    m_llSndDurationCounter = (long)now;
	                    // update sending variables
	                    m_iSndLastDataAck = ack;
	                    m_pSndLossList.remove(UdtSequenceNumber.decseq(m_iSndLastDataAck));
	                    if (bLockTaken)
	                    {
	                        Monitor.Exit(m_AckLock);
	                    }
	                    if (m_bSynSending)
	                    {
	                        m_SendBlockCond.Set();
	                    }
	                    // acknowledde any waiting epolls to write
	                    //s_UDTUnited.m_EPoll.update_events(m_SocketID, m_sPollID, EPOLLOpt.UDT_EPOLL_OUT, true);
	                    // insert this socket to snd list if it is not on the list yet
	                    m_pSndQueue.m_pSndUList.update(this, false);
	                    // Update RTT
	                    int rtt = ctrlpkt.GetIntFromData(1);
	                    m_iRTTVar = (m_iRTTVar * 3 + Math.Abs(rtt - m_iRTT)) >> 2;
	                    m_iRTT = (m_iRTT * 7 + rtt) >> 3;
	                    m_pCC.setRTT(m_iRTT);
	                    if (ctrlpkt.getLength() > 16)
	                    {
	                        // Update Estimated Bandwidth and packet delivery rate
	                        if (ctrlpkt.GetIntFromData(4) > 0)
	                        {
	                            m_iDeliveryRate = (m_iDeliveryRate * 7 + ctrlpkt.GetIntFromData(4)) >> 3;
	                        }
	                        if (ctrlpkt.GetIntFromData(5) > 0)
	                        {
	                            m_iBandwidth = (m_iBandwidth * 7 + ctrlpkt.GetIntFromData(5)) >> 3;
	                        }
	                        m_pCC.setRcvRate(m_iDeliveryRate);
	                        m_pCC.setBandwidth(m_iBandwidth);
	                    }
	                    m_pCC.OnAcknowledgement(ack);
	                    CCUpdate();
	                    ++m_iRecvACK;
	                    ++m_iRecvACKTotal;
	                    break;
	                }
	            case 6: //110 - Acknowledgement of Acknowledgement
	                {
	                    int ack = -1;
	                    int rtt = -1;
	                    // update RTT
	                    rtt = m_pACKWindow.Acknowledge(ctrlpkt.getAckSeqNo(), ref ack);
	                    if (rtt <= 0)
	                    {
	                        break;
	                    }
	                    //if increasing delay detected...
	                    //   sendCtrl(4);
	                    // RTT EWMA
	                    m_iRTTVar = (m_iRTTVar * 3 + Math.Abs(rtt - m_iRTT)) >> 2;
	                    m_iRTT = (m_iRTT * 7 + rtt) >> 3;
	                    m_pCC.setRTT(m_iRTT);
	                    // update last ACK that has been received by the sender
	                    if (UdtSequenceNumber.seqcmp(ack, m_iRcvLastAckAck) > 0)
	                    {
	                        m_iRcvLastAckAck = ack;
	                    }
	                    break;
	                }
	            case 3: //011 - Loss Report
	                {
	                    int[] losslist = new int[ctrlpkt.getLength() / 4];
	                    Buffer.BlockCopy(ctrlpkt.GetDataBytes(), 0, losslist, 0, ctrlpkt.getLength());
	                    m_pCC.OnLoss(losslist, ctrlpkt.getLength() / 4);
	                    CCUpdate();
	                    bool secure = true;
	                    // decode loss list message and insert loss into the sender loss list
	                    for (int i = 0; i < losslist.Length; ++i)
	                    {
	                        if (0 != (losslist[i] & 0x80000000))
	                        {
	                            if ((UdtSequenceNumber.seqcmp(losslist[i] & 0x7FFFFFFF, losslist[i + 1]) > 0) || (UdtSequenceNumber.seqcmp(losslist[i + 1], m_iSndCurrSeqNo) > 0))
	                            {
	                                // seq_a must not be greater than seq_b; seq_b must not be greater than the most recent sent seq
	                                secure = false;
	                                break;
	                            }
	                            int num = 0;
	                            if (UdtSequenceNumber.seqcmp(losslist[i] & 0x7FFFFFFF, m_iSndLastAck) >= 0)
	                            {
	                                num = m_pSndLossList.insert(losslist[i] & 0x7FFFFFFF, losslist[i + 1]);
	                            }
	                            else if (UdtSequenceNumber.seqcmp(losslist[i + 1], m_iSndLastAck) >= 0)
	                            {
	                                num = m_pSndLossList.insert(m_iSndLastAck, losslist[i + 1]);
	                            }
	                            m_iTraceSndLoss += num;
	                            m_iSndLossTotal += num;
	                            ++i;
	                        }
	                        else if (UdtSequenceNumber.seqcmp(losslist[i], m_iSndLastAck) >= 0)
	                        {
	                            if (UdtSequenceNumber.seqcmp(losslist[i], m_iSndCurrSeqNo) > 0)
	                            {
	                                //seq_a must not be greater than the most recent sent seq
	                                secure = false;
	                                break;
	                            }
	                            int num = m_pSndLossList.insert(losslist[i], losslist[i]);
	                            m_iTraceSndLoss += num;
	                            m_iSndLossTotal += num;
	                        }
	                    }
	                    if (!secure)
	                    {
	                        //this should not happen: attack or bug
	                        m_bBroken = true;
	                        m_iBrokenCounter = 0;
	                        break;
	                    }
	                    // the lost packet (retransmission) should be sent out immediately
	                    m_pSndQueue.m_pSndUList.update(this);
	                    ++m_iRecvNAK;
	                    ++m_iRecvNAKTotal;
	                    break;
	                }
	            case 4: //100 - Delay Warning
	                    // One way packet delay is increasing, so decrease the sending rate
	                m_ullInterval = (ulong)Math.Ceiling(m_ullInterval * 1.125);
	                m_iLastDecSeq = m_iSndCurrSeqNo;
	                break;
	            case 1: //001 - Keep-alive
	                    // The only purpose of keep-alive packet is to tell that the peer is still alive
	                    // nothing needs to be done.
	                break;
	            case 0: //000 - Handshake
	                {
	                    UdtHandshake req = new UdtHandshake();
	                    req.Deserialize(ctrlpkt.GetDataBytes(), ctrlpkt.getLength());
	                    if ((req.RequestType > 0) || (m_bRendezvous && (req.RequestType != -2)))
	                    {
	                        // The peer side has not received the handshake message, so it keeps querying
	                        // resend the handshake packet
	                        UdtHandshake initdata = new UdtHandshake();
	                        initdata.InitialSequenceNumber = m_iISN;
	                        initdata.MaximumSegmentSize = m_iMSS;
	                        initdata.FlowControlWindowSize = m_iFlightFlagSize;
	                        initdata.RequestType = (!m_bRendezvous) ? -1 : -2;
	                        initdata.SocketId = m_SocketID;
	                        byte[] hs = new byte[m_iPayloadSize];
	                        int hs_size = m_iPayloadSize;
	                        initdata.Serialize(hs);
	                        fixed (byte* pHS = hs)
	                        {
	                            sendCtrl(0, null, pHS, hs_size);
	                        }
	                    }
	                    break;
	                }
	            case 5: //101 - Shutdown
	                m_bShutdown = true;
	                m_bClosing = true;
	                m_bBroken = true;
	                m_iBrokenCounter = 60;
	                // Signal the sender and recver if they are waiting for data.
	                releaseSynch();
	                UdtTimer.triggerEvent();
	                break;
	            case 7: //111 - Msg drop request
	                m_pRcvBuffer.dropMsg(ctrlpkt.getMsgSeq());
	                m_pRcvLossList.remove(ctrlpkt.GetIntFromData(0), ctrlpkt.GetIntFromData(1));
	                // move forward with current recv seq no.
	                if ((UdtSequenceNumber.seqcmp(ctrlpkt.GetIntFromData(0), UdtSequenceNumber.incseq(m_iRcvCurrSeqNo)) <= 0)
	                   && (UdtSequenceNumber.seqcmp(ctrlpkt.GetIntFromData(1), m_iRcvCurrSeqNo) > 0))
	                {
	                    m_iRcvCurrSeqNo = ctrlpkt.GetIntFromData(1);
	                }
	                break;
	            case 8: // 1000 - An error has happened to the peer side
	                    //int err_type = packet.getAddInfo();
	                // currently only this error is signalled from the peer side
	                // if recvfile() failes (e.g., due to disk fail), blcoked sendfile/send should return immediately
	                // giving the app a chance to fix the issue
	                m_bPeerHealth = false;
	                break;
	            case 32767: //0x7FFF - reserved and user defined messages
	                m_pCC.processCustomMsg(ctrlpkt);
	                CCUpdate();
	                break;
	            default:
	                break;
	        }
	    }
	    public unsafe int packData(UdtPacket packet, ref ulong ts)
	    {
	        int payload = 0;
	        bool probe = false;
	        ulong entertime = UdtTimer.rdtsc();
	        if ((0 != m_ullTargetTime) && (entertime > m_ullTargetTime))
	        {
	            m_ullTimeDiff += entertime - m_ullTargetTime;
	        }
	        // Loss retransmission always has higher priority.
	        packet.SetSequenceNumber(m_pSndLossList.getLostSeq());
	        if (packet.GetSequenceNumber() >= 0)
	        {
	            // protect m_iSndLastDataAck from updating by ACK processing
	            lock (m_AckLock)
	            {
	                int offset = UdtSequenceNumber.seqoff(m_iSndLastDataAck, packet.GetSequenceNumber());
	                if (offset < 0)
	                {
	                    return 0;
	                }
	                int msglen = 0;
	                byte[] data = null;
	                uint msgNo = 0;
	                payload = m_pSndBuffer.readData(ref data, offset, ref msgNo, out msglen);
	                packet.SetDataFromBytes(data, 0, payload);
	                packet.SetMessageNumber(msgNo);
	                if (-1 == payload)
	                {
	                    int[] seqpair = new int[2];
	                    seqpair[0] = packet.GetSequenceNumber();
	                    seqpair[1] = UdtSequenceNumber.incseq(seqpair[0], msglen);
	                    msgNo = packet.GetMessageNumber();
	                    fixed (int* pSeqpair = seqpair)
	                    {
	                        sendCtrl(7, &msgNo, pSeqpair, 8);
	                    }
	                    // only one msg drop request is necessary
	                    m_pSndLossList.remove(seqpair[1]);
	                    // skip all dropped packets
	                    if (UdtSequenceNumber.seqcmp(m_iSndCurrSeqNo, UdtSequenceNumber.incseq(seqpair[1])) < 0)
	                    {
	                        m_iSndCurrSeqNo = UdtSequenceNumber.incseq(seqpair[1]);
	                    }
	                    return 0;
	                }
	                else if (0 == payload)
	                {
	                    return 0;
	                }
	                ++m_iTraceRetrans;
	                ++m_iRetransTotal;
	            }
	        }
	        else
	        {
	            // If no loss, pack a new packet.
	            // check congestion/flow window limit
	            int cwnd = (m_iFlowWindowSize < (int)m_dCongestionWindow) ? m_iFlowWindowSize : (int)m_dCongestionWindow;
	            if (cwnd >= UdtSequenceNumber.seqlen(m_iSndLastAck, UdtSequenceNumber.incseq(m_iSndCurrSeqNo)))
	            {
	                byte[] data = null;
	                uint msgNo = 0;
	                payload = m_pSndBuffer.readData(ref data, ref msgNo);
	                if (0 != payload)
	                {
	                    packet.SetDataFromBytes(data, 0, payload);
	                    packet.SetMessageNumber(msgNo);
	                    m_iSndCurrSeqNo = UdtSequenceNumber.incseq(m_iSndCurrSeqNo);
	                    m_pCC.setSndCurrSeqNo(m_iSndCurrSeqNo);
	                    packet.SetSequenceNumber(m_iSndCurrSeqNo);
	                    // every 16 (0xF) packets, a packet pair is sent
	                    if (0 == (packet.GetSequenceNumber() & 0xF))
	                    {
	                        probe = true;
	                    }
	                }
	                else
	                {
	                    m_ullTargetTime = 0;
	                    m_ullTimeDiff = 0;
	                    ts = 0;
	                    return 0;
	                }
	            }
	            else
	            {
	                m_ullTargetTime = 0;
	                m_ullTimeDiff = 0;
	                ts = 0;
	                return 0;
	            }
	        }
	        packet.SetTimestamp((int)(UdtTimer.getTime() - m_StartTime));
	        packet.SetId(m_PeerID);
	        packet.setLength(payload);
	        m_pCC.OnPacketSent(packet);
	        m_pSndTimeWindow.onPktSent(packet.GetTimestamp());
	        ++m_llTraceSent;
	        ++m_llSentTotal;
	        if (probe)
	        {
	            // sends out probing packet pair
	            ts = entertime;
	            probe = false;
	        }
	        else
	        {
	            if (m_ullTimeDiff >= m_ullInterval)
	            {
	                ts = entertime;
	                m_ullTimeDiff -= m_ullInterval;
	            }
	            else
	            {
	                ts = entertime + m_ullInterval - m_ullTimeDiff;
	                m_ullTimeDiff = 0;
	            }
	        }
	        m_ullTargetTime = ts;
	        return payload;
	    }
	    public unsafe int processData(UdtUnit unit)
	    {
	        UdtPacket packet = unit.m_Packet;
	        // Just heard from the peer, reset the expiration count.
	        m_iEXPCount = 1;
	        ulong currtime = UdtTimer.rdtsc();
	        m_ullLastRspTime = currtime;
	        m_pCC.OnPacketReceived(packet);
	        ++m_iPktCount;
	        // update time information
	        m_pRcvTimeWindow.onPktArrival();
	        // check if it is probing packet pair
	        if (0 == (packet.GetSequenceNumber() & 0xF))
	        {
	            m_pRcvTimeWindow.probe1Arrival();
	        }
	        else if (1 == (packet.GetSequenceNumber() & 0xF))
	        {
	            m_pRcvTimeWindow.probe2Arrival();
	        }
	        ++m_llTraceRecv;
	        ++m_llRecvTotal;
	        int offset = UdtSequenceNumber.seqoff(m_iRcvLastAck, packet.GetSequenceNumber());
	        if ((offset < 0) || (offset >= m_pRcvBuffer.getAvailBufSize()))
	        {
	            return -1;
	        }
	        if (m_pRcvBuffer.addData(unit, offset) < 0)
	        {
	            return -1;
	        }
	        // Loss detection.
	        if (UdtSequenceNumber.seqcmp(packet.GetSequenceNumber(), UdtSequenceNumber.incseq(m_iRcvCurrSeqNo)) > 0)
	        {
	            // If loss found, insert them to the receiver loss list
	            m_pRcvLossList.insert(UdtSequenceNumber.incseq(m_iRcvCurrSeqNo), UdtSequenceNumber.decseq(packet.GetSequenceNumber()));
	            // pack loss list for NAK
	            int[] lossdata = new int[2];
	            lossdata[0] = (int)(UdtSequenceNumber.incseq(m_iRcvCurrSeqNo) | 0x80000000);
	            lossdata[1] = UdtSequenceNumber.decseq(packet.GetSequenceNumber());
	            // Generate loss report immediately.
	            fixed (int* pLossdata = lossdata)
	            {
	                sendCtrl(3, null, pLossdata, (UdtSequenceNumber.incseq(m_iRcvCurrSeqNo) == UdtSequenceNumber.decseq(packet.GetSequenceNumber())) ? 1 : 2);
	            }
	            int loss = UdtSequenceNumber.seqlen(m_iRcvCurrSeqNo, packet.GetSequenceNumber()) - 2;
	            m_iTraceRcvLoss += loss;
	            m_iRcvLossTotal += loss;
	        }
	        // This is not a regular fixed size packet...   
	        //an irregular sized packet usually indicates the end of a message, so send an ACK immediately   
	        if (packet.getLength() != m_iPayloadSize)
	        {
	            m_ullNextACKTime = UdtTimer.rdtsc();
	        }
	        // Update the current largest sequence number that has been received.
	        // Or it is a retransmitted packet, remove it from receiver loss list.
	        if (UdtSequenceNumber.seqcmp(packet.GetSequenceNumber(), m_iRcvCurrSeqNo) > 0)
	        {
	            m_iRcvCurrSeqNo = packet.GetSequenceNumber();
	        }
	        else
	        {
	            m_pRcvLossList.remove(packet.GetSequenceNumber());
	        }
	        return 0;
	    }
	    public int listen(IPEndPoint endPoint, UdtPacket packet)
	    {
	        if (m_bClosing)
	        {
	            return 1002;
	        }
	        if (packet.getLength() != UdtHandshake.ContentSize)
	        {
	            return 1004;
	        }
	        UdtHandshake hs = new UdtHandshake();
	        hs.Deserialize(packet.GetDataBytes(), packet.getLength());
	        IPHostEntry host = Dns.GetHostEntry(endPoint.Address); //TODO SocketException,ArgumentException
	        // SYN cookie
	        long timestamp = (long)(UdtTimer.getTime() - m_StartTime) / 60000000;  // secret changes every one minute
	        string cookiestr = string.Format("{0}:{1}:{2}", host.HostName, endPoint.Port, timestamp);
	        MD5 md5 = MD5.Create();
	        byte[] cookie = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(cookiestr));
	        if (1 == hs.RequestType)
	        {
	            hs.Cookie = BitConverter.ToInt32(cookie, 0);
	            packet.pack(hs);
	            packet.SetId(hs.SocketId);
	            m_pSndQueue.sendto(endPoint, packet);
	            return 0;
	        }
	        else
	        {
	            if (hs.Cookie != BitConverter.ToInt32(cookie, 0))
	            {
	                timestamp--;
	                cookiestr = string.Format("{0}:{1}:{2}", host.HostName, endPoint.Port, timestamp);
	                cookie = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(cookiestr));
	                if (hs.Cookie != BitConverter.ToInt32(cookie, 0))
	                {
	                    return -1;
	                }
	            }
	        }
	        int id = hs.SocketId;
	        // When a peer side connects in...
	        if ((1 == packet.getFlag()) && (0 == packet.getType()))
	        {
	            if ((hs.Version != m_iVersion) || (hs.SocketType != m_iSockType))
	            {
	                // mismatch, reject the request
	                hs.RequestType = 1002;
	                packet.pack(hs);
	                packet.SetId(id);
	                m_pSndQueue.sendto(endPoint, packet);
	            }
	            else
	            {
	                int result = s_UDTUnited.newConnection(m_SocketID, endPoint, hs);
	                if (result == -1)
	                {
	                    hs.RequestType = 1002;
	                }
	                // send back a response if connection failed or connection already existed
	                // new connection response should be sent in connect()
	                if (result != 1)
	                {
	                    packet.pack(hs);
	                    packet.SetId(id);
	                    m_pSndQueue.sendto(endPoint, packet);
	                }
	                else
	                {
	                    // a new connection has been created, enable epoll for write 
	                    //s_UDTUnited.m_EPoll.update_events(m_SocketID, m_sPollID, EPOLLOpt.UDT_EPOLL_OUT, true);
	                }
	            }
	        }
	        return hs.RequestType;
	    }
	    public unsafe void checkTimers()
	    {
	        // update CC parameters
	        CCUpdate();
	        //ulong minint = (ulong)(m_ullCPUFrequency * m_pSndTimeWindow.getMinPktSndInt() * 0.9);
	        //if (m_ullInterval < minint)
	        //   m_ullInterval = minint;
	        ulong currtime = UdtTimer.rdtsc();
	        if ((currtime > m_ullNextACKTime) || ((m_pCC.m_iACKInterval > 0) && (m_pCC.m_iACKInterval <= m_iPktCount)))
	        {
	            // ACK timer expired or ACK interval is reached
	            sendCtrl(2);
	            currtime = UdtTimer.rdtsc();
	            if (m_pCC.m_iACKPeriod > 0)
	            {
	                m_ullNextACKTime = currtime + (ulong)m_pCC.m_iACKPeriod * m_ullCPUFrequency;
	            }
	            else
	            {
	                m_ullNextACKTime = currtime + m_ullACKInt;
	            }
	            m_iPktCount = 0;
	            m_iLightACKCount = 1;
	        }
	        else if (m_iSelfClockInterval * m_iLightACKCount <= m_iPktCount)
	        {
	            //send a "light" ACK
	            sendCtrl(2, null, null, 4);
	            ++m_iLightACKCount;
	        }
	        // we are not sending back repeated NAK anymore and rely on the sender's EXP for retransmission
	        //if ((m_pRcvLossList.getLossLength() > 0) && (currtime > m_ullNextNAKTime))
	        //{
	        //   // NAK timer expired, and there is loss to be reported.
	        //   sendCtrl(3);
	        //
	        //   CTimer.rdtsc(currtime);
	        //   m_ullNextNAKTime = currtime + m_ullNAKInt;
	        //}
	        ulong next_exp_time;
	        if (m_pCC.m_bUserDefinedRTO)
	        {
	            next_exp_time = m_ullLastRspTime + (ulong)m_pCC.m_iRTO * m_ullCPUFrequency;
	        }
	        else
	        {
	            ulong exp_int = (ulong)(m_iEXPCount * (m_iRTT + 4 * m_iRTTVar) + m_iSYNInterval) * m_ullCPUFrequency;
	            if (exp_int < (ulong)m_iEXPCount * m_ullMinExpInt)
	            {
	                exp_int = (ulong)m_iEXPCount * m_ullMinExpInt;
	            }
	            next_exp_time = m_ullLastRspTime + exp_int;
	        }
	        if (currtime > next_exp_time)
	        {
	            // Haven't receive any information from the peer, is it dead?!
	            // timeout: at least 16 expirations and must be greater than 10 seconds
	            if ((m_iEXPCount > 16) && (currtime - m_ullLastRspTime > 5000000 * m_ullCPUFrequency))
	            {
	                //
	                // Connection is broken. 
	                // UDT does not signal any information about this instead of to stop quietly.
	                // Application will detect this when it calls any UDT methods next time.
	                //
	                m_bClosing = true;
	                m_bBroken = true;
	                m_iBrokenCounter = 30;
	                // update snd U list to remove this socket
	                m_pSndQueue.m_pSndUList.update(this);
	                releaseSynch();
	                // app can call any UDT API to learn the connection_broken error
	                //s_UDTUnited.m_EPoll.update_events(m_SocketID, m_sPollID, UDT_EPOLL_IN | UDT_EPOLL_OUT | UDT_EPOLL_ERR, true);
	                UdtTimer.triggerEvent();
	                return;
	            }
	            // sender: Insert all the packets sent after last received acknowledgement into the sender loss list.
	            // recver: Send out a keep-alive packet
	            if (m_pSndBuffer.getCurrBufSize() > 0)
	            {
	                if ((UdtSequenceNumber.incseq(m_iSndCurrSeqNo) != m_iSndLastAck) && (m_pSndLossList.getLossLength() == 0))
	                {
	                    // resend all unacknowledged packets on timeout, but only if there is no packet in the loss list
	                    int csn = m_iSndCurrSeqNo;
	                    int num = m_pSndLossList.insert(m_iSndLastAck, csn);
	                    m_iTraceSndLoss += num;
	                    m_iSndLossTotal += num;
	                }
	                m_pCC.OnTimeout();
	                CCUpdate();
	                // immediately restart transmission
	                m_pSndQueue.m_pSndUList.update(this);
	            }
	            else
	            {
	                sendCtrl(1);
	            }
	            ++m_iEXPCount;
	            // Reset last response time since we just sent a heart-beat.
	            m_ullLastRspTime = currtime;
	        }
	    }
	}
	internal class UdtCongestionControl1 : UdtCongestionControlBase
	{
	    int m_iRCInterval;          // UDT Rate control interval
	    ulong m_LastRCTime;      // last rate increase time
	    bool m_bSlowStart;          // if in slow start phase
	    int m_iLastAck;         // last ACKed seq no
	    bool m_bLoss;           // if loss happened since last rate increase
	    int m_iLastDecSeq;      // max pkt seq no sent out when last decrease happened
	    double m_dLastDecPeriod;        // value of pktsndperiod when last decrease happened
	    int m_iNAKCount;                     // NAK counter
	    int m_iDecRandom;                    // random threshold on decrease by number of loss events
	    int m_iAvgNAKNum;                    // average number of NAKs per congestion
	    int m_iDecCount;            // number of decreases in a congestion epoch
	    static Random m_random = new Random();
	    public override void Initialize()
	    {
	        m_iRCInterval = m_iSYNInterval;
	        m_LastRCTime = UdtTimer.getTime();
	        setACKTimer(m_iRCInterval);
	        m_bSlowStart = true;
	        m_iLastAck = m_iSndCurrSeqNo;
	        m_bLoss = false;
	        m_iLastDecSeq = UdtSequenceNumber.decseq(m_iLastAck);
	        m_dLastDecPeriod = 1;
	        m_iAvgNAKNum = 0;
	        m_iNAKCount = 0;
	        m_iDecRandom = 1;
	        m_dCWndSize = 16;
	        m_dPktSndPeriod = 1;
	    }
	    public override void OnAcknowledgement(int ack)
	    {
	        long B = 0;
	        double inc = 0;
	        // Note: 1/24/2012
	        // The minimum increase parameter is increased from "1.0 / m_iMSS" to 0.01
	        // because the original was too small and caused sending rate to stay at low level
	        // for long time.
	        const double min_inc = 0.01;
	        ulong currtime = UdtTimer.getTime();
	        if (currtime - m_LastRCTime < (ulong)m_iRCInterval)
	            return;
	        m_LastRCTime = currtime;
	        if (m_bSlowStart)
	        {
	            m_dCWndSize += UdtSequenceNumber.seqlen(m_iLastAck, ack);
	            m_iLastAck = ack;
	            if (m_dCWndSize > m_dMaxCWndSize)
	            {
	                m_bSlowStart = false;
	                if (m_iRcvRate > 0)
	                    m_dPktSndPeriod = 1000000.0 / m_iRcvRate;
	                else
	                    m_dPktSndPeriod = (m_iRTT + m_iRCInterval) / m_dCWndSize;
	            }
	        }
	        else
	            m_dCWndSize = m_iRcvRate / 1000000.0 * (m_iRTT + m_iRCInterval) + 16;
	        // During Slow Start, no rate increase
	        if (m_bSlowStart)
	            return;
	        if (m_bLoss)
	        {
	            m_bLoss = false;
	            return;
	        }
	        B = (long)(m_iBandwidth - 1000000.0 / m_dPktSndPeriod);
	        if ((m_dPktSndPeriod > m_dLastDecPeriod) && ((m_iBandwidth / 9) < B))
	            B = m_iBandwidth / 9;
	        if (B <= 0)
	            inc = min_inc;
	        else
	        {
	            // inc = max(10 ^ ceil(log10( B * MSS * 8 ) * Beta / MSS, 1/MSS)
	            // Beta = 1.5 * 10^(-6)
	            inc = Math.Pow(10.0, Math.Ceiling(Math.Log10(B * m_iMSS * 8.0))) * 0.0000015 / m_iMSS;
	            if (inc < min_inc)
	                inc = min_inc;
	        }
	        m_dPktSndPeriod = (m_dPktSndPeriod * m_iRCInterval) / (m_dPktSndPeriod * inc + m_iRCInterval);
	    }
	    public override void OnLoss(int[] losslist, int length)
	    {
	        //Slow Start stopped, if it hasn't yet
	        if (m_bSlowStart)
	        {
	            m_bSlowStart = false;
	            if (m_iRcvRate > 0)
	            {
	                // Set the sending rate to the receiving rate.
	                m_dPktSndPeriod = 1000000.0 / m_iRcvRate;
	                return;
	            }
	            // If no receiving rate is observed, we have to compute the sending
	            // rate according to the current window size, and decrease it
	            // using the method below.
	            m_dPktSndPeriod = m_dCWndSize / (m_iRTT + m_iRCInterval);
	        }
	        m_bLoss = true;
	        if (UdtSequenceNumber.seqcmp(losslist[0] & 0x7FFFFFFF, m_iLastDecSeq) > 0)
	        {
	            m_dLastDecPeriod = m_dPktSndPeriod;
	            m_dPktSndPeriod = Math.Ceiling(m_dPktSndPeriod * 1.125);
	            m_iAvgNAKNum = (int)Math.Ceiling(m_iAvgNAKNum * 0.875 + m_iNAKCount * 0.125);
	            m_iNAKCount = 1;
	            m_iDecCount = 1;
	            m_iLastDecSeq = m_iSndCurrSeqNo;
	            // remove global synchronization using randomization
	            m_iDecRandom = (int)Math.Ceiling(m_iAvgNAKNum * m_random.NextDouble());
	            if (m_iDecRandom < 1)
	                m_iDecRandom = 1;
	        }
	        else if ((m_iDecCount++ < 5) && (0 == (++m_iNAKCount % m_iDecRandom)))
	        {
	            // 0.875^5 = 0.51, rate should not be decreased by more than half within a congestion period
	            m_dPktSndPeriod = Math.Ceiling(m_dPktSndPeriod * 1.125);
	            m_iLastDecSeq = m_iSndCurrSeqNo;
	        }
	    }
	    public override void OnTimeout()
	    {
	        if (m_bSlowStart)
	        {
	            m_bSlowStart = false;
	            if (m_iRcvRate > 0)
	                m_dPktSndPeriod = 1000000.0 / m_iRcvRate;
	            else
	                m_dPktSndPeriod = m_dCWndSize / (m_iRTT + m_iRCInterval);
	        }
	        else
	        {
	            /*
	            m_dLastDecPeriod = m_dPktSndPeriod;
	            m_dPktSndPeriod = ceil(m_dPktSndPeriod * 2);
	            m_iLastDecSeq = m_iLastAck;
	            */
	        }
	    }
	}
	internal class UdtCongestionControlBase
	{
	    protected const int m_iSYNInterval = UdtCongestionControl.m_iSYNInterval;	// UDT constant parameter, SYN
	    public double m_dPktSndPeriod;              // Packet sending period, in microseconds
	    public double m_dCWndSize;                  // Congestion window size, in packets
	    protected int m_iBandwidth;           // estimated bandwidth, packets per second
	    protected double m_dMaxCWndSize;               // maximum cwnd size, in packets
	    protected int m_iMSS;             // Maximum Packet Size, including all packet headers
	    protected int m_iSndCurrSeqNo;        // current maximum seq no sent out
	    protected int m_iRcvRate;         // packet arrive rate at receiver side, packets per second
	    protected int m_iRTT;             // current estimated RTT, microsecond
	    protected string m_pcParam;            // user defined parameter
	    public UDTSOCKET m_UDT;                     // The UDT entity that this congestion control algorithm is bound to
	    public int m_iACKPeriod;                    // Periodical timer to send an ACK, in milliseconds
	    public int m_iACKInterval;                  // How many packets to send one ACK, in packets
	    public bool m_bUserDefinedRTO;              // if the RTO value is defined by users
	    public int m_iRTO;                          // RTO value, microseconds
	    PerfMon m_PerfInfo = new PerfMon();                 // protocol statistics information
	    public UdtCongestionControlBase()
	    {
	        m_dPktSndPeriod = 1.0;
	        m_dCWndSize = 16.0;
	        m_pcParam = null;
	        m_iACKPeriod = 0;
	        m_iACKInterval = 0;
	        m_bUserDefinedRTO = false;
	        m_iRTO = -1;
	    }
	    // Functionality:
	    //    Callback function to be called (only) at the start of a UDT connection.
	    //    note that this is different from CCC(), which is always called.
	    // Parameters:
	    //    None.
	    // Returned value:
	    //    None.
	    public virtual void Initialize() { }
	    // Functionality:
	    //    Callback function to be called when a UDT connection is closed.
	    // Parameters:
	    //    None.
	    // Returned value:
	    //    None.
	    public virtual void Close() { }
	    // Functionality:
	    //    Callback function to be called when an ACK packet is received.
	    // Parameters:
	    //    0) [in] ackno: the data sequence number acknowledged by this ACK.
	    // Returned value:
	    //    None.
	    public virtual void OnAcknowledgement(int seqno) { }
	    // Functionality:
	    //    Callback function to be called when a loss report is received.
	    // Parameters:
	    //    0) [in] losslist: list of sequence number of packets, in the format describled in packet.cpp.
	    //    1) [in] size: length of the loss list.
	    // Returned value:
	    //    None.
	    public virtual void OnLoss(int[] loss, int length) { }
	    // Functionality:
	    //    Callback function to be called when a timeout event occurs.
	    // Parameters:
	    //    None.
	    // Returned value:
	    //    None.
	    public virtual void OnTimeout() { }
	    // Functionality:
	    //    Callback function to be called when a data is sent.
	    // Parameters:
	    //    0) [in] seqno: the data sequence number.
	    //    1) [in] size: the payload size.
	    // Returned value:
	    //    None.
	    public virtual void OnPacketSent(UdtPacket packet) { }
	    // Functionality:
	    //    Callback function to be called when a data is received.
	    // Parameters:
	    //    0) [in] seqno: the data sequence number.
	    //    1) [in] size: the payload size.
	    // Returned value:
	    //    None.
	    public virtual void OnPacketReceived(UdtPacket packet) { }
	    // Functionality:
	    //    Callback function to Process a user defined packet.
	    // Parameters:
	    //    0) [in] pkt: the user defined packet.
	    // Returned value:
	    //    None.
	    public virtual void processCustomMsg(UdtPacket packet) { }
	    protected void setACKTimer(int msINT)
	    {
	        m_iACKPeriod = msINT > m_iSYNInterval ? m_iSYNInterval : msINT;
	    }
	    protected void setACKInterval(int pktINT)
	    {
	        m_iACKInterval = pktINT;
	    }
	    protected void setRTO(int usRTO)
	    {
	        m_bUserDefinedRTO = true;
	        m_iRTO = usRTO;
	    }
	    protected void sendCustomMsg(UdtPacket pkt)
	    {
	        UdtCongestionControl u = UdtCongestionControl.s_UDTUnited.lookup(m_UDT);
	        if (null != u)
	        {
	            pkt.SetId(u.m_PeerID);
	            u.m_pSndQueue.sendto(u.m_pPeerAddr, pkt);
	        }
	    }
	    protected PerfMon getPerfInfo()
	    {
	        try
	        {
	            UdtCongestionControl u = UdtCongestionControl.s_UDTUnited.lookup(m_UDT);
	            if (null != u)
	                u.sample(m_PerfInfo, false);
	        }
	        catch (Exception e)
	        {
	            return null;
	        }
	        return m_PerfInfo;
	    }
	    public void setMSS(int mss)
	    {
	        m_iMSS = mss;
	    }
	    public void setBandwidth(int bw)
	    {
	        m_iBandwidth = bw;
	    }
	    public void setSndCurrSeqNo(int seqno)
	    {
	        m_iSndCurrSeqNo = seqno;
	    }
	    public void setRcvRate(int rcvrate)
	    {
	        m_iRcvRate = rcvrate;
	    }
	    public void SetMaximumCongestionWindowSize(int cwnd)
	    {
	        m_dMaxCWndSize = cwnd;
	    }
	    public void setRTT(int rtt)
	    {
	        m_iRTT = rtt;
	    }
	    protected void SetUserParameter(string param)
	    {
	        m_pcParam = param;
	    }
	}
	internal class UdtCongestionControlFactory<T> : UdtCongestionControlVirtualFactory where T : new()
	{
	    public override UdtCongestionControlBase create()
	    {
	        return new T() as UdtCongestionControlBase;
	    }
	    public override UdtCongestionControlVirtualFactory clone()
	    {
	        return new UdtCongestionControlFactory<T>();
	    }
	}
	internal abstract class UdtCongestionControlVirtualFactory
	{
	    public abstract UdtCongestionControlBase create();
	    public abstract UdtCongestionControlVirtualFactory clone();
	}
	internal class UdtHandshake
	{
	    public const int ContentSize = 48;    // Size of hand shake data
	    public UdtHandshake()
	    {
	        for (int i = 0; i < 4; ++i)
	        {
	            m_piPeerIP[i] = 0;
	        }
	    }
	    public int Version { get; set; }
	    public SocketType SocketType { get; set; }
	    public int InitialSequenceNumber { get; set; }
	    public int MaximumSegmentSize { get; set; }
	    public int FlowControlWindowSize { get; set; }
	    public int RequestType { get; set; }
	    public int SocketId { get; set; }
	    public int Cookie { get; set; }
	    public uint[] m_piPeerIP = new uint[4];    // The IP address that the peer's UDP port is bound to
	    public unsafe void Serialize(byte[] buffer)
	    {
	        fixed (byte* pb = buffer)
	        {
	            int* p = (int*)(pb);
	            *p++ = Version;
	            *p++ = (int)SocketType;
	            *p++ = InitialSequenceNumber;
	            *p++ = MaximumSegmentSize;
	            *p++ = FlowControlWindowSize;
	            *p++ = RequestType;
	            *p++ = SocketId;
	            *p++ = Cookie;
	            for (int i = 0; i < 4; ++i)
	            {
	                *p++ = (int)m_piPeerIP[i];
	            }
	        }
	    }
	    public unsafe bool Deserialize(byte[] buffer, int size)
	    {
	        if (size < ContentSize)
	        {
	            return false;
	        }
	        fixed (byte* pb = buffer)
	        {
	            int* p = (int*)(pb);
	            Version = *p++;
	            SocketType = (SocketType)(*p++);
	            InitialSequenceNumber = *p++;
	            MaximumSegmentSize = *p++;
	            FlowControlWindowSize = *p++;
	            RequestType = *p++;
	            SocketId = *p++;
	            Cookie = *p++;
	            for (int i = 0; i < 4; ++i)
	            {
	                m_piPeerIP[i] = (uint)*p++;
	            }
	        }
	        return true;
	    }
	    public override string ToString()
	    {
	        string type = "connection request";
	        if (RequestType == 0)
	        {
	            type = "rendezvouz";
	        }
	        if (RequestType < 0)
	        {
	            type = "reponse";
	        }
	        if (RequestType == 1002)
	        {
	            type = "rejected request";
	        }
	        StringBuilder sb = new StringBuilder();
	        sb.AppendLine("  Version      " + Version);
	        sb.AppendLine("  Type         " + type);
	        sb.AppendLine("  Cookie       " + Cookie);
	        //sb.AppendLine("  Socket type  " + m_iType.ToString());
	        //sb.AppendLine("  Socket id    " + m_iID);
	        sb.AppendLine("  Initial seq# " + InitialSequenceNumber);
	        //sb.AppendLine("  MSS          " + m_iMSS);
	        //sb.AppendLine("  Flight size  " + m_iFlightFlagSize);
	        return sb.ToString();
	    }
	}
	internal class UdtInfoBlock
	{
	    uint[] m_piIP = new uint[4];      // IP address, machine read only, not human readable format
	    AddressFamily m_iIPversion;       // IP version
	    public ulong m_ullTimeStamp;    // last update time
	    public int m_iRTT;         // RTT
	    public int m_iBandwidth;       // estimated bandwidth
	    public int m_iLossRate;        // average loss rate
	    public int m_iReorderDistance; // packet reordering distance
	    public double m_dInterval;     // inter-packet time, congestion control
	    public double m_dCWnd;     // congestion window size, congestion control
	    public UdtInfoBlock(IPAddress address)
	    {
	        m_iIPversion = address.AddressFamily;
	        ConvertIPAddress.ToUintArray(address, ref m_piIP);
	    }
	    public override bool Equals(object value)
	    {
	        // Is null?
	        if (Object.ReferenceEquals(null, value))
	        {
	            return false;
	        }
	        // Is the same object?
	        if (Object.ReferenceEquals(this, value))
	        {
	            return true;
	        }
	        // Is the same type?
	        if (value.GetType() != this.GetType())
	        {
	            return false;
	        }
	        return IsEqual((UdtInfoBlock)value);
	    }
	    public bool Equals(UdtInfoBlock infoBlock)
	    {
	        if (Object.ReferenceEquals(null, infoBlock))
	        {
	            return false;
	        }
	        // Is the same object?
	        if (Object.ReferenceEquals(this, infoBlock))
	        {
	            return true;
	        }
	        return IsEqual(infoBlock);
	    }
	    public static bool operator ==(UdtInfoBlock infoBlockA, UdtInfoBlock infoBlockB)
	    {
	        if (Object.ReferenceEquals(infoBlockA, infoBlockB))
	        {
	            return true;
	        }
	        // Ensure that "numberA" isn't null
	        if (Object.ReferenceEquals(null, infoBlockA))
	        {
	            return false;
	        }
	        return (infoBlockA.Equals(infoBlockB));
	    }
	    public static bool operator !=(UdtInfoBlock infoBlockA, UdtInfoBlock infoBlockB)
	    {
	        return !(infoBlockA == infoBlockB);
	    }
	    public override int GetHashCode()
	    {
	        if (m_iIPversion == AddressFamily.InterNetwork)
	            return (int)m_piIP[0];
	        return (int)(m_piIP[0] + m_piIP[1] + m_piIP[2] + m_piIP[3]);
	    }
	    bool IsEqual(UdtInfoBlock infoBlock)
	    {
	        if (m_iIPversion != infoBlock.m_iIPversion)
	            return false;
	        else if (m_iIPversion == AddressFamily.InterNetwork)
	            return (m_piIP[0] == infoBlock.m_piIP[0]);
	        for (int i = 0; i < 4; ++i)
	        {
	            if (m_piIP[i] != infoBlock.m_piIP[i])
	                return false;
	        }
	        return true;
	    }
	}
	internal struct UdtIOVector
	{
	    public uint[] iov_base;
	    public int iov_len;
	}
	internal static class UdtMessageNumber
	{
	    public static int msgcmp(int msgno1, int msgno2)
	    {
	        return (Math.Abs(msgno1 - msgno2) < m_iMsgNoTH) ? (msgno1 - msgno2) : (msgno2 - msgno1);
	    }
	    public static int msglen(int msgno1, int msgno2)
	    {
	        return (msgno1 <= msgno2) ? (msgno2 - msgno1 + 1) : (msgno2 - msgno1 + m_iMaxMsgNo + 2);
	    }
	    public static int msgoff(int msgno1, int msgno2)
	    {
	        if (Math.Abs(msgno1 - msgno2) < m_iMsgNoTH)
	            return msgno2 - msgno1;
	        if (msgno1 < msgno2)
	            return msgno2 - msgno1 - m_iMaxMsgNo - 1;
	        return msgno2 - msgno1 + m_iMaxMsgNo + 1;
	    }
	    public static int incmsg(int msgno)
	    {
	        return (msgno == m_iMaxMsgNo) ? 0 : msgno + 1;
	    }
	    static int m_iMsgNoTH = 0xFFFFFFF;             // threshold for comparing msg. no.
	    public static int m_iMaxMsgNo = 0x1FFFFFFF;           // maximum message number used in UDT
	}
	internal class UdtMultiplexer
	{
	    internal SndQueue m_pSndQueue; // The sending queue
	    internal RcvQueue m_pRcvQueue; // The receiving queue
	    internal UdtChannel m_pChannel;   // The UDP channel for sending and receiving
	    internal UdtTimer m_pTimer;       // The timer
	    internal int m_iPort;            // The UDP port number of this multiplexer
	    internal AddressFamily m_iIPversion;       // IP version
	    internal int m_iMSS;         // Maximum Segment Size
	    internal int m_iRefCount;        // number of UDT instances that are associated with this multiplexer
	    internal bool m_bReusable;       // if this one can be shared with others
	    internal int m_iID;          // multiplexer ID
	}
	internal class UdtNetworkStream : Stream
	{
	    public UdtNetworkStream(UdtSocket socket)
	    {
	        mSocket = socket;
	    }
	    public override bool CanRead { get { return true; } }
	    public override bool CanSeek { get { return false; } }
	    public override bool CanWrite { get { return true; } }
	    public override long Length { get { throw new NotImplementedException(); } }
	    public override long Position { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }
	    public override void Flush()
	    {
	    }
	    public override int Read(byte[] buffer, int offset, int count)
	    {
	        return mSocket.Receive(buffer, offset, count);
	    }
	    public override long Seek(long offset, SeekOrigin origin)
	    {
	        throw new NotImplementedException();
	    }
	    public override void SetLength(long value)
	    {
	        throw new NotImplementedException();
	    }
	    public override void Write(byte[] buffer, int offset, int count)
	    {
	        mSocket.Send(buffer, offset, count);
	    }
	    UdtSocket mSocket;
	}
	internal unsafe class UdtPacket
	{
	    private enum ControlType
	    {
	        Handshake = 0,
	        KeepAlive = 1,
	        Ack = 2,
	        Nak = 3,
	        CongestionWarning = 4,
	        Shutdown = 5,
	        Ack2 = 6,
	        DropMessage = 7,
	        Error = 8,
	        UserType = 32767,
	    }
	    private const int sequenceNumberIndex = 0;                      // alias: sequence number
	    private const int messageNumberIndex = 1;                       // alias: message number
	    private const int timestampIndex = 2;                           // alias: timestamp
	    private const int idIndex = 3;                                  // alias: socket ID
	    public const int packetHeaderSize = 16;    // packet header size
	    private readonly UdtIOVector[] m_PacketVector = new UdtIOVector[2];             // The 2-demension vector of UDT packet [header, data]
	    public override string ToString()
	    {
	        StringBuilder stringBuilder = new StringBuilder();
	        if (getFlag() == 0)
	        {
	            byte[] data = GetDataBytes();
	            stringBuilder.AppendFormat("Data length {0} bytes", data != null ? data.Length : 0);
	            stringBuilder.AppendLine();
	            stringBuilder.AppendLine("  SeqNo     " + GetSequenceNumber());
	            stringBuilder.AppendLine("  MsgNo     " + GetMessageNumber());
	            stringBuilder.AppendLine("  Timestamp " + GetTimestamp());
	            stringBuilder.AppendLine("  SocketID  " + GetId());
	            stringBuilder.Append("  Data: {");
	            if (data != null)
	            {
	                for (int i = 0; i < Math.Min(data.Length, 10); ++i)
	                {
	                    stringBuilder.Append(data[i] + ",");
	                }
	                stringBuilder.Length = stringBuilder.Length - 1;
	            }
	            stringBuilder.AppendLine("}");
	        }
	        else if (getFlag() == 1)
	        {
	            int type = getType();
	            stringBuilder.AppendFormat("CTRL {0} ({1})", (UdtPacket.ControlType)type, type);
	            stringBuilder.AppendLine();
	            switch (type)
	            {
	                case 2: //0010 - Acknowledgement (ACK)
	                    stringBuilder.AppendFormat("  Ack sequence {0}", getAckSeqNo());
	                    stringBuilder.AppendLine();
	                    break;
	                case 6: //0110 - Acknowledgement of Acknowledgement (ACK-2)
	                    stringBuilder.AppendFormat("  Ack2 sequence {0}", getAckSeqNo());
	                    stringBuilder.AppendLine();
	                    break;
	                case 3: //0011 - Loss Report (NAK)
	                    break;
	                case 4: //0100 - Congestion Warning
	                    break;
	                case 1: //0001 - Keep-alive
	                    break;
	                case 0: //0000 - Handshake
	                    // control info filed is handshake info
	                    UdtHandshake handshake = new UdtHandshake();
	                    handshake.Deserialize(GetDataBytes(), UdtHandshake.ContentSize);
	                    stringBuilder.AppendFormat(handshake.ToString());
	                    stringBuilder.AppendLine();
	                    break;
	                case 5: //0101 - Shutdown
	                    break;
	                case 7: //0111 - Message Drop Request
	                    break;
	                case 8: //1000 - Error Signal from the Peer Side
	                        // Error type
	                     stringBuilder.AppendLine("Error: " + m_PacketVector[0].iov_base[messageNumberIndex].ToString());
	                    break;
	                case 32767: //0x7FFF - Reserved for user defined control packets
	                    break;
	                default:
	                    break;
	            }
	        }
	        return stringBuilder.ToString();
	    }
	    public UdtPacket()
	    {
	        m_PacketVector[0].iov_base = new uint[4];
	        m_PacketVector[0].iov_len = packetHeaderSize;
	        m_PacketVector[1].iov_base = null;
	        m_PacketVector[1].iov_len = 0;
	    }
	    ~UdtPacket()
	    {
	    }
	    public void Clone(UdtPacket source)
	    {
	        Buffer.BlockCopy(source.m_PacketVector[0].iov_base, 0, m_PacketVector[0].iov_base, 0, packetHeaderSize);
	        if (source.m_PacketVector[1].iov_base == null)
	        {
	            m_PacketVector[1].iov_base = null;
	            m_PacketVector[1].iov_len = source.m_PacketVector[1].iov_len;
	            return;
	        }
	        m_PacketVector[1].iov_base = new uint[source.m_PacketVector[1].iov_base.Length];
	        Buffer.BlockCopy(source.m_PacketVector[1].iov_base, 0, m_PacketVector[1].iov_base, 0, source.m_PacketVector[1].iov_len);
	        m_PacketVector[1].iov_len = source.m_PacketVector[1].iov_len;
	    }
	    public int GetSequenceNumber()
	    {
	        return (int)m_PacketVector[0].iov_base[sequenceNumberIndex];
	    }
	    public void SetSequenceNumber(int sequenceNumber)
	    {
	        m_PacketVector[0].iov_base[sequenceNumberIndex] = (uint)sequenceNumber;
	    }
	    public uint GetMessageNumber()
	    {
	        return m_PacketVector[0].iov_base[messageNumberIndex];
	    }
	    public void SetMessageNumber(uint messageNumber)
	    {
	        m_PacketVector[0].iov_base[messageNumberIndex] = messageNumber;
	    }
	    public int GetTimestamp()
	    {
	        return (int)m_PacketVector[0].iov_base[timestampIndex];
	    }
	    public void SetTimestamp(int timestamp)
	    {
	        m_PacketVector[0].iov_base[timestampIndex] = (uint)timestamp;
	    }
	    public int GetId()
	    {
	        return (int)m_PacketVector[0].iov_base[idIndex];
	    }
	    public void SetId(int id)
	    {
	        m_PacketVector[0].iov_base[idIndex] = (uint)id;
	    }
	    public byte[] GetBytes()
	    {
	        int dataLength = m_PacketVector[1].iov_len;
	        byte[] bytes = new byte[packetHeaderSize + dataLength];
	        Buffer.BlockCopy(m_PacketVector[0].iov_base, 0, bytes, 0, packetHeaderSize);
	        if (dataLength == 0 || m_PacketVector[1].iov_base == null)
	        {
	            return bytes;
	        }
	        Buffer.BlockCopy(m_PacketVector[1].iov_base, 0, bytes, packetHeaderSize, dataLength);
	        return bytes;
	    }
	    public byte[] GetHeaderBytes()
	    {
	        byte[] bytes = new byte[packetHeaderSize];
	        Buffer.BlockCopy(m_PacketVector[0].iov_base, 0, bytes, 0, packetHeaderSize);
	        return bytes;
	    }
	    public int GetDataBytes(int packetOffset, byte[] data, int dataOffset, int length)
	    {
	        if (m_PacketVector[1].iov_base == null)
	        {
	            return 0;
	        }
	        int bufferAvailable = data.Length - dataOffset;
	        if (bufferAvailable < length)
	        {
	            length = bufferAvailable;
	        }
	        Buffer.BlockCopy(m_PacketVector[1].iov_base, packetOffset, data, dataOffset, length);
	        return length;
	    }
	    public int GetIntFromData(int offset)
	    {
	        return (int)m_PacketVector[1].iov_base[offset];
	    }
	    public byte[] GetDataBytes()
	    {
	        if (m_PacketVector[1].iov_base == null)
	        {
	            return null;
	        }
	        int dataLength = m_PacketVector[1].iov_len;
	        if (dataLength <= 0)
	        {
	            return null;
	        }
	        byte[] bytes = new byte[dataLength];
	        Buffer.BlockCopy(m_PacketVector[1].iov_base, 0, bytes, 0, bytes.Length);
	        return bytes;
	    }
	    public bool SetHeaderAndDataFromBytes(byte[] bytes, int length)
	    {
	        if (length < packetHeaderSize)
	        {
	            return false;
	        }
	        Buffer.BlockCopy(bytes, 0, m_PacketVector[0].iov_base, 0, packetHeaderSize);
	        int dataLength = length - packetHeaderSize;
	        if (dataLength == 0)
	        {
	            m_PacketVector[1].iov_base = null;
	            m_PacketVector[1].iov_len = 0;
	            return true;
	        }
	        SetDataFromBytes(bytes, packetHeaderSize, dataLength);
	        return true;
	    }
	    public void SetDataFromBytes(byte[] bytes)
	    {
	        SetDataFromBytes(bytes, 0, bytes.Length);
	    }
	    public void SetDataFromBytes(byte[] bytes, int offset, int byteCount)
	    {
	        int intCount = byteCount / 4;
	        if (byteCount % 4 != 0)
	        {
	            ++intCount;
	        }
	        m_PacketVector[1].iov_base = new uint[intCount];
	        m_PacketVector[1].iov_len = byteCount;
	        Buffer.BlockCopy(bytes, offset, m_PacketVector[1].iov_base, 0, byteCount);
	    }
	    public void ConvertControlInfoToNetworkOrder()
	    {
	        if (getFlag() == 0)
	        {
	            return;
	        }
	        if (m_PacketVector[1].iov_base == null || m_PacketVector[1].iov_len == 0)
	        {
	            return;
	        }
	        for (int i = 0; i < m_PacketVector[1].iov_base.Length; ++i)
	        {
	            m_PacketVector[1].iov_base[i] =
	                (uint)IPAddress.HostToNetworkOrder((int)m_PacketVector[1].iov_base[i]);
	        }
	    }
	    public void ConvertControlInfoToHostOrder()
	    {
	        if (getFlag() == 0)
	        {
	            return;
	        }
	        if (m_PacketVector[1].iov_base == null || m_PacketVector[1].iov_len == 0)
	        {
	            return;
	        }
	        for (int i = 0; i < m_PacketVector[1].iov_base.Length; ++i)
	        {
	            m_PacketVector[1].iov_base[i] =
	                (uint)IPAddress.NetworkToHostOrder((int)m_PacketVector[1].iov_base[i]);
	        }
	    }
	    public void ConvertHeaderToNetworkOrder()
	    {
	        for (int i = 0; i < m_PacketVector[0].iov_base.Length; ++i)
	        {
	            m_PacketVector[0].iov_base[i] =
	                (uint)IPAddress.HostToNetworkOrder((int)m_PacketVector[0].iov_base[i]);
	        }
	    }
	    public void ConvertHeaderToHostOrder()
	    {
	        for (int i = 0; i < m_PacketVector[0].iov_base.Length; ++i)
	        {
	            m_PacketVector[0].iov_base[i] =
	                (uint)IPAddress.NetworkToHostOrder((int)m_PacketVector[0].iov_base[i]);
	        }
	    }
	    public int getLength()
	    {
	        return m_PacketVector[1].iov_len;
	    }
	    public void setLength(int len)
	    {
	        m_PacketVector[1].iov_len = len;
	    }
	    static UdtIOVector MakeIovec(void* rparam, int size)
	    {
	        UdtIOVector result = new UdtIOVector();
	        result.iov_len = size;
	        if (rparam == null)
	        {
	            return result;
	        }
	        result.iov_base = new uint[size >> 2];
	        uint* pIn = (uint*)rparam;
	        for (int i = 0; i < size >> 2; ++i)
	        {
	            result.iov_base[i] = *pIn++;
	        }
	        return result;
	    }
	    public void pack(UdtHandshake hs)
	    {
	        // TODO avoid this inefficient buffer creation
	        // we copy this buffer into another buffer later
	        byte[] bytes = new byte[UdtHandshake.ContentSize];
	        hs.Serialize(bytes);
	        pack(0, bytes);
	    }
	    public void pack(int pkttype, void* lparam)
	    {
	        if (pkttype != 6 && pkttype != 8)
	        {
	            throw new Exception("pkttype must be 6 or 8");
	        }
	        pack(pkttype, lparam, (void*)null, 0);
	    }
	    public void pack(int pkttype, byte[] rparam)
	    {
	        if (pkttype != 0)
	        {
	            throw new Exception("pkttype must be 0");
	        }
	        fixed (byte* prparam = rparam)
	        {
	            pack(pkttype, (void*)null, (void*)prparam, rparam.Length);
	        }
	    }
	    public void pack(int pkttype, int lparam, int[] rparam)
	    {
	        if (pkttype != 2)
	        {
	            throw new Exception("pkttype must be 2");
	        }
	        fixed (int* prparam = rparam)
	        {
	            pack(pkttype, &lparam, (void*)prparam, rparam.Length * 4);
	        }
	    }
	    public void pack(int pkttype, int lparam, int[] rparam, int length)
	    {
	        if (pkttype != 2)
	        {
	            throw new Exception("pkttype must be 2");
	        }
	        fixed (int* prparam = rparam)
	        {
	            pack(pkttype, &lparam, (void*)prparam, length * 4);
	        }
	    }
	    public void pack(int pkttype, int[] rparam, int length)
	    {
	        if (pkttype != 3)
	        {
	            throw new Exception("pkttype must be 3");
	        }
	        fixed (int* prparam = rparam)
	        {
	            pack(pkttype, (void*)null, (void*)prparam, length * 4);
	        }
	    }
	    public void pack(int pkttype)
	    {
	        if (pkttype != 1 && pkttype != 4 && pkttype != 5)
	        {
	            throw new Exception("pkttype must be 1, 4 or 5");
	        }
	        pack(pkttype, (void*)null, (void*)null, 0);
	    }
	    public void pack(int pkttype, void* lparam, void* rparam, int size)
	    {
	        // Set (bit-0 = 1) and (bit-1~15 = type)
	        m_PacketVector[0].iov_base[sequenceNumberIndex] = (uint)0x80000000 | (uint)(pkttype << 16);
	        // Set additional information and control information field
	        switch (pkttype)
	        {
	            case 2: //0010 - Acknowledgement (ACK)
	                    // ACK packet seq. no.
	                if (null != lparam)
	                {
	                    m_PacketVector[0].iov_base[messageNumberIndex] = *(uint*)lparam;
	                }
	                // data ACK seq. no. 
	                // optional: RTT (microsends), RTT variance (microseconds) advertised flow window size (packets), and estimated link capacity (packets per second)
	                m_PacketVector[1] = MakeIovec(rparam, size);
	                break;
	            case 6: //0110 - Acknowledgement of Acknowledgement (ACK-2)
	                    // ACK packet seq. no.
	                m_PacketVector[0].iov_base[messageNumberIndex] = *(uint*)lparam;
	                // control info field should be none
	                // but "writev" does not allow this
	                m_PacketVector[1] = MakeIovec(null, 4);
	                break;
	            case 3: //0011 - Loss Report (NAK)
	                    // loss list
	                m_PacketVector[1] = MakeIovec(rparam, size);
	                break;
	            case 4: //0100 - Congestion Warning
	                    // control info field should be none
	                    // but "writev" does not allow this
	                m_PacketVector[1] = MakeIovec(null, 4);
	                break;
	            case 1: //0001 - Keep-alive
	                    // control info field should be none
	                    // but "writev" does not allow this
	                m_PacketVector[1] = MakeIovec(null, 4);
	                break;
	            case 0: //0000 - Handshake
	                    // control info filed is handshake info
	                m_PacketVector[1] = MakeIovec(rparam, size);
	                break;
	            case 5: //0101 - Shutdown
	                    // control info field should be none
	                    // but "writev" does not allow this
	                m_PacketVector[1] = MakeIovec(null, 4);
	                break;
	            case 7: //0111 - Message Drop Request
	                    // msg id 
	                m_PacketVector[0].iov_base[messageNumberIndex] = *(uint*)lparam;
	                //first seq no, last seq no
	                m_PacketVector[1] = MakeIovec(rparam, size);
	                break;
	            case 8: //1000 - Error Signal from the Peer Side
	                    // Error type
	                m_PacketVector[0].iov_base[messageNumberIndex] = *(uint*)lparam;
	                // control info field should be none
	                // but "writev" does not allow this
	                m_PacketVector[1] = MakeIovec(null, 4);
	                break;
	            case 32767: //0x7FFF - Reserved for user defined control packets
	                        // for extended control packet
	                        // "lparam" contains the extended type information for bit 16 - 31
	                        // "rparam" is the control information
	                m_PacketVector[0].iov_base[sequenceNumberIndex] |= *(uint*)lparam;
	                if (null != rparam)
	                {
	                    m_PacketVector[1] = MakeIovec(rparam, size);
	                }
	                else
	                {
	                    m_PacketVector[1] = MakeIovec(null, 4);
	                }
	                break;
	            default:
	                break;
	        }
	    }
	    public UdtIOVector[] getPacketVector()
	    {
	        return m_PacketVector;
	    }
	    public int getFlag()
	    {
	        // read bit 0
	        return (int)(m_PacketVector[0].iov_base[sequenceNumberIndex] >> 31);
	    }
	    public int getType()
	    {
	        // read bit 1~15
	        return (int)((m_PacketVector[0].iov_base[sequenceNumberIndex] >> 16) & 0x00007FFF);
	    }
	    int getExtendedType()
	    {
	        // read bit 16~31
	        return (int)(m_PacketVector[0].iov_base[sequenceNumberIndex] & 0x0000FFFF);
	    }
	    public int getAckSeqNo()
	    {
	        // read additional information field
	        return (int)m_PacketVector[0].iov_base[messageNumberIndex];
	    }
	    public int getMsgBoundary()
	    {
	        // read [1] bit 0~1
	        return (int)(m_PacketVector[0].iov_base[messageNumberIndex] >> 30);
	    }
	    public bool getMsgOrderFlag()
	    {
	        // read [1] bit 2
	        return (1 == ((m_PacketVector[0].iov_base[messageNumberIndex] >> 29) & 1));
	    }
	    public int getMsgSeq()
	    {
	        // read [1] bit 3~31
	        return (int)(m_PacketVector[0].iov_base[messageNumberIndex] & 0x1FFFFFFF);
	    }
	}
	internal class UdtPacketTimeWindow
	{
	    public UdtPacketTimeWindow(int asize = 16, int psize = 16)
	    {
	        m_iAWSize = asize;
	        m_iPWSize = psize;
	        m_iMinPktSndInt = 1000000;
	        m_piPktWindow = new int[m_iAWSize];
	        m_piPktReplica = new int[m_iAWSize];
	        m_piProbeWindow = new int[m_iPWSize];
	        m_piProbeReplica = new int[m_iPWSize];
	        m_LastArrTime = UdtTimer.getTime();
	        for (int i = 0; i < m_iAWSize; ++i)
	            m_piPktWindow[i] = 1000000;
	        for (int k = 0; k < m_iPWSize; ++k)
	            m_piProbeWindow[k] = 1000;
	    }
	    // Functionality:
	    //    read the minimum packet sending interval.
	    // Parameters:
	    //    None.
	    // Returned value:
	    //    minimum packet sending interval (microseconds).
	    public int getMinPktSndInt()
	    {
	        return m_iMinPktSndInt;
	    }
	    // Functionality:
	    //    Calculate the packes arrival speed.
	    // Parameters:
	    //    None.
	    // Returned value:
	    //    Packet arrival speed (packets per second).
	    public int getPktRcvSpeed()
	    {
	        // get median value, but cannot change the original value order in the window
	        Array.Copy(m_piPktWindow, m_piPktReplica, m_iAWSize - 1); // why -1 ???
	        Array.Sort(m_piPktReplica); // need -1 here ???
	        int median = m_piPktReplica[m_iAWSize / 2];
	        int count = 0;
	        int sumMicrosecond = 0;
	        int upper = median << 3;
	        int lower = median >> 3;
	        // median filtering
	        for (int i = 0, n = m_iAWSize; i < n; ++i)
	        {
	            if ((m_piPktWindow[i] < upper) && (m_piPktWindow[i] > lower))
	            {
	                ++count;
	                sumMicrosecond += m_piPktWindow[i];
	            }
	        }
	        double packetsPerMicrosecond = (double)count / sumMicrosecond;
	        // claculate speed, or return 0 if not enough valid value
	        if (count > (m_iAWSize >> 1))
	            return (int)Math.Ceiling(1000000 * packetsPerMicrosecond);
	        else
	            return 0;
	    }
	    // Functionality:
	    //    Estimate the bandwidth.
	    // Parameters:
	    //    None.
	    // Returned value:
	    //    Estimated bandwidth (packets per second).
	    public int getBandwidth()
	    {
	        // get median value, but cannot change the original value order in the window
	        Array.Copy(m_piProbeWindow, m_piProbeReplica, m_iPWSize - 1); // why -1 ???
	        Array.Sort(m_piProbeReplica); // need -1 here ???
	        int median = m_piProbeReplica[m_iPWSize / 2];
	        int count = 1;
	        int sum = median;
	        int upper = median << 3;
	        int lower = median >> 3;
	        // median filtering
	        for (int i = 0, n = m_iPWSize; i < n; ++i)
	        {
	            if ((m_piProbeWindow[i] < upper) && (m_piProbeWindow[i] > lower))
	            {
	                ++count;
	                sum += m_piProbeWindow[i];
	            }
	        }
	        return (int)Math.Ceiling(1000000.0 / ((double)sum / (double)count));
	    }
	    // Functionality:
	    //    Record time information of a packet sending.
	    // Parameters:
	    //    0) currtime: timestamp of the packet sending.
	    // Returned value:
	    //    None.
	    public void onPktSent(int currtime)
	    {
	        int interval = currtime - m_iLastSentTime;
	        if ((interval < m_iMinPktSndInt) && (interval > 0))
	            m_iMinPktSndInt = interval;
	        m_iLastSentTime = currtime;
	    }
	    // Functionality:
	    //    Record time information of an arrived packet.
	    // Parameters:
	    //    None.
	    // Returned value:
	    //    None.
	    public void onPktArrival()
	    {
	        m_CurrArrTime = UdtTimer.getTime();
	        // record the packet interval between the current and the last one
	        m_piPktWindow[m_iPktWindowPtr] = (int)(m_CurrArrTime - m_LastArrTime);
	        // the window is logically circular
	        ++m_iPktWindowPtr;
	        if (m_iPktWindowPtr == m_iAWSize)
	            m_iPktWindowPtr = 0;
	        // remember last packet arrival time
	        m_LastArrTime = m_CurrArrTime;
	    }
	    // Functionality:
	    //    Record the arrival time of the first probing packet.
	    // Parameters:
	    //    None.
	    // Returned value:
	    //    None.
	    public void probe1Arrival()
	    {
	        m_ProbeTime = UdtTimer.getTime();
	    }
	    // Functionality:
	    //    Record the arrival time of the second probing packet and the interval between packet pairs.
	    // Parameters:
	    //    None.
	    // Returned value:
	    //    None.
	    public void probe2Arrival()
	    {
	        m_CurrArrTime = UdtTimer.getTime();
	        // record the probing packets interval
	        m_piProbeWindow[m_iProbeWindowPtr] = (int)(m_CurrArrTime - m_ProbeTime);
	        // the window is logically circular
	        ++m_iProbeWindowPtr;
	        if (m_iProbeWindowPtr == m_iPWSize)
	            m_iProbeWindowPtr = 0;
	    }
	    int m_iAWSize;               // size of the packet arrival history window
	    int[] m_piPktWindow;          // packet information window
	    int[] m_piPktReplica;
	    int m_iPktWindowPtr;         // position pointer of the packet info. window.
	    int m_iPWSize;               // size of probe history window size
	    int[] m_piProbeWindow;        // record inter-packet time for probing packet pairs
	    int[] m_piProbeReplica;
	    int m_iProbeWindowPtr;       // position pointer to the probing window
	    int m_iLastSentTime;         // last packet sending time
	    int m_iMinPktSndInt;         // Minimum packet sending interval
	    ulong m_LastArrTime;      // last packet arrival time
	    ulong m_CurrArrTime;      // current packet arrival time
	    ulong m_ProbeTime;        // arrival time of the first probing packet
	}
	internal class UdtReceiverBuffer
	{
	    UdtUnit[] m_pUnit;                     // pointer to the protocol buffer
	    int m_iSize;                         // size of the protocol buffer
	    int m_iStartPos;                     // the head position for I/O (inclusive)
	    int m_iLastAckPos;                   // the last ACKed position (exclusive)
	                                         // EMPTY: m_iStartPos = m_iLastAckPos   FULL: m_iStartPos = m_iLastAckPos + 1
	    int m_iMaxPos;          // the furthest data position
	    int m_iNotch;           // the starting read point of the first unit
	    public UdtReceiverBuffer(int bufsize)
	    {
	        m_iSize = bufsize;
	        m_iStartPos = 0;
	        m_iLastAckPos = 0;
	        m_iMaxPos = 0;
	        m_iNotch = 0;
	        m_pUnit = new UdtUnit[m_iSize];
	        for (int i = 0; i < m_iSize; ++i)
	            m_pUnit[i] = null;
	    }
	    ~UdtReceiverBuffer()
	    {
	        for (int i = 0; i < m_iSize; ++i)
	        {
	            if (null != m_pUnit[i])
	            {
	                m_pUnit[i].m_iFlag = 0;
	            }
	        }
	    }
	    public int addData(UdtUnit unit, int offset)
	    {
	        int pos = (m_iLastAckPos + offset) % m_iSize;
	        if (offset > m_iMaxPos)
	            m_iMaxPos = offset;
	        if (null != m_pUnit[pos])
	            return -1;
	        m_pUnit[pos] = unit;
	        unit.m_iFlag = 1;
	        return 0;
	    }
	    public int readBuffer(byte[] data, int offset, int len)
	    {
	        int p = m_iStartPos;
	        int lastack = m_iLastAckPos;
	        int rs = len;
	        while ((p != lastack) && (rs > 0))
	        {
	            int unitsize = m_pUnit[p].m_Packet.getLength() - m_iNotch;
	            if (unitsize > rs)
	                unitsize = rs;
	            unitsize = m_pUnit[p].m_Packet.GetDataBytes(m_iNotch, data, offset, unitsize);
	            offset += unitsize;
	            if ((rs > unitsize) || (rs == m_pUnit[p].m_Packet.getLength() - m_iNotch))
	            {
	                UdtUnit tmp = m_pUnit[p];
	                m_pUnit[p] = null;
	                tmp.m_iFlag = 0;
	                if (++p == m_iSize)
	                    p = 0;
	                m_iNotch = 0;
	            }
	            else
	                m_iNotch += rs;
	            rs -= unitsize;
	        }
	        m_iStartPos = p;
	        return len - rs;
	    }
	    public void ackData(int len)
	    {
	        m_iLastAckPos = (m_iLastAckPos + len) % m_iSize;
	        m_iMaxPos -= len;
	        if (m_iMaxPos < 0)
	            m_iMaxPos = 0;
	        UdtTimer.triggerEvent();
	    }
	    public int getAvailBufSize()
	    {
	        // One slot must be empty in order to tell the difference between "empty buffer" and "full buffer"
	        return m_iSize - getRcvDataSize() - 1;
	    }
	    public int getRcvDataSize()
	    {
	        if (m_iLastAckPos >= m_iStartPos)
	            return m_iLastAckPos - m_iStartPos;
	        return m_iSize + m_iLastAckPos - m_iStartPos;
	    }
	    public void dropMsg(int msgno)
	    {
	        for (int i = m_iStartPos, n = (m_iLastAckPos + m_iMaxPos) % m_iSize; i != n; i = (i + 1) % m_iSize)
	            if ((null != m_pUnit[i]) && (msgno == m_pUnit[i].m_Packet.GetMessageNumber()))
	                m_pUnit[i].m_iFlag = 3;
	    }
	    public int readMsg(byte[] data, int len)
	    {
	        int p = 0;
	        int q = 0;
	        bool passack = false;
	        if (!scanMsg(ref p, ref q, ref passack))
	            return 0;
	        int rs = len;
	        int dataOffset = 0;
	        while (p != (q + 1) % m_iSize)
	        {
	            byte[] allData = m_pUnit[p].m_Packet.GetDataBytes();
	            int unitsize = allData.Length;
	            if ((rs >= 0) && (unitsize > rs))
	                unitsize = rs;
	            if (unitsize > 0)
	            {
	                Array.Copy(allData, 0, data, dataOffset, unitsize);
	                dataOffset += unitsize;
	                rs -= unitsize;
	            }
	            if (!passack)
	            {
	                UdtUnit tmp = m_pUnit[p];
	                m_pUnit[p] = null;
	                tmp.m_iFlag = 0;
	            }
	            else
	                m_pUnit[p].m_iFlag = 2;
	            if (++p == m_iSize)
	                p = 0;
	        }
	        if (!passack)
	            m_iStartPos = (q + 1) % m_iSize;
	        return len - rs;
	    }
	    int getRcvMsgNum()
	    {
	        int p = 0;
	        int q = 0;
	        bool passack = false;
	        return scanMsg(ref p, ref q, ref passack) ? 1 : 0;
	    }
	    bool scanMsg(ref int p, ref int q, ref bool passack)
	    {   
	        // empty buffer
	        if ((m_iStartPos == m_iLastAckPos) && (m_iMaxPos <= 0))
	            return false;
	        //skip all bad msgs at the beginning
	        while (m_iStartPos != m_iLastAckPos)
	        {
	            if (null == m_pUnit[m_iStartPos])
	            {
	                if (++m_iStartPos == m_iSize)
	                    m_iStartPos = 0;
	                continue;
	            }
	            if ((1 == m_pUnit[m_iStartPos].m_iFlag) && (m_pUnit[m_iStartPos].m_Packet.getMsgBoundary() > 1))
	            {
	                bool good = true;
	                // look ahead for the whole message
	                for (int i = m_iStartPos; i != m_iLastAckPos;)
	                {
	                    if ((null == m_pUnit[i]) || (1 != m_pUnit[i].m_iFlag))
	                    {
	                        good = false;
	                        break;
	                    }
	                    if ((m_pUnit[i].m_Packet.getMsgBoundary() == 1) || (m_pUnit[i].m_Packet.getMsgBoundary() == 3))
	                        break;
	                    if (++i == m_iSize)
	                        i = 0;
	                }
	                if (good)
	                    break;
	            }
	            UdtUnit tmp = m_pUnit[m_iStartPos];
	            m_pUnit[m_iStartPos] = null;
	            tmp.m_iFlag = 0;
	            if (++m_iStartPos == m_iSize)
	                m_iStartPos = 0;
	        }
	        p = -1;                  // message head
	        q = m_iStartPos;         // message tail
	        passack = m_iStartPos == m_iLastAckPos;
	        bool found = false;
	        // looking for the first message
	        for (int i = 0, n = m_iMaxPos + getRcvDataSize(); i <= n; ++i)
	        {
	            if ((null != m_pUnit[q]) && (1 == m_pUnit[q].m_iFlag))
	            {
	                switch (m_pUnit[q].m_Packet.getMsgBoundary())
	                {
	                    case 3: // 11
	                        p = q;
	                        found = true;
	                        break;
	                    case 2: // 10
	                        p = q;
	                        break;
	                    case 1: // 01
	                        if (p != -1)
	                            found = true;
	                        break;
	                }
	            }
	            else
	            {
	                // a hole in this message, not valid, restart search
	                p = -1;
	            }
	            if (found)
	            {
	                // the msg has to be ack'ed or it is allowed to read out of order, and was not read before
	                if (!passack || !m_pUnit[q].m_Packet.getMsgOrderFlag())
	                    break;
	                found = false;
	            }
	            if (++q == m_iSize)
	                q = 0;
	            if (q == m_iLastAckPos)
	                passack = true;
	        }
	        // no msg found
	        if (!found)
	        {
	            // if the message is larger than the receiver buffer, return part of the message
	            if ((p != -1) && ((q + 1) % m_iSize == p))
	                found = true;
	        }
	        return found;
	    }
	}
	internal class UdtReceiverLossList
	{
	    int[] m_piData1;                  // sequence number starts
	    int[] m_piData2;                  // sequence number ends
	    int[] m_piNext;                       // next node in the list
	    int[] m_piPrior;                      // prior node in the list;
	    int m_iHead;                         // first node in the list
	    int m_iTail;                         // last node in the list;
	    int m_iLength;                       // loss length
	    int m_iSize;                         // size of the static array
	    public UdtReceiverLossList(int size)
	    {
	        m_iHead = -1;
	        m_iTail = -1;
	        m_iLength = 0;
	        m_iSize = size;
	        m_piData1 = new int[m_iSize];
	        m_piData2 = new int[m_iSize];
	        m_piNext = new int[m_iSize];
	        m_piPrior = new int[m_iSize];
	        // -1 means there is no data in the node
	        for (int i = 0; i < size; ++i)
	        {
	            m_piData1[i] = -1;
	            m_piData2[i] = -1;
	        }
	    }
	    public void insert(int seqno1, int seqno2)
	    {
	        // Data to be inserted must be larger than all those in the list
	        // guaranteed by the UDT receiver
	        if (0 == m_iLength)
	        {
	            // insert data into an empty list
	            m_iHead = 0;
	            m_iTail = 0;
	            m_piData1[m_iHead] = seqno1;
	            if (seqno2 != seqno1)
	                m_piData2[m_iHead] = seqno2;
	            m_piNext[m_iHead] = -1;
	            m_piPrior[m_iHead] = -1;
	            m_iLength += UdtSequenceNumber.seqlen(seqno1, seqno2);
	            return;
	        }
	        // otherwise searching for the position where the node should be
	        int offset = UdtSequenceNumber.seqoff(m_piData1[m_iHead], seqno1);
	        int loc = (m_iHead + offset) % m_iSize;
	        if ((-1 != m_piData2[m_iTail]) && (UdtSequenceNumber.incseq(m_piData2[m_iTail]) == seqno1))
	        {
	            // coalesce with prior node, e.g., [2, 5], [6, 7] becomes [2, 7]
	            loc = m_iTail;
	            m_piData2[loc] = seqno2;
	        }
	        else
	        {
	            // create new node
	            m_piData1[loc] = seqno1;
	            if (seqno2 != seqno1)
	                m_piData2[loc] = seqno2;
	            m_piNext[m_iTail] = loc;
	            m_piPrior[loc] = m_iTail;
	            m_piNext[loc] = -1;
	            m_iTail = loc;
	        }
	        m_iLength += UdtSequenceNumber.seqlen(seqno1, seqno2);
	    }
	    public bool remove(int seqno)
	    {
	        if (0 == m_iLength)
	            return false;
	        // locate the position of "seqno" in the list
	        int offset = UdtSequenceNumber.seqoff(m_piData1[m_iHead], seqno);
	        if (offset < 0)
	            return false;
	        int loc = (m_iHead + offset) % m_iSize;
	        if (seqno == m_piData1[loc])
	        {
	            // This is a seq. no. that starts the loss sequence
	            if (-1 == m_piData2[loc])
	            {
	                // there is only 1 loss in the sequence, delete it from the node
	                if (m_iHead == loc)
	                {
	                    m_iHead = m_piNext[m_iHead];
	                    if (-1 != m_iHead)
	                        m_piPrior[m_iHead] = -1;
	                }
	                else
	                {
	                    m_piNext[m_piPrior[loc]] = m_piNext[loc];
	                    if (-1 != m_piNext[loc])
	                        m_piPrior[m_piNext[loc]] = m_piPrior[loc];
	                    else
	                        m_iTail = m_piPrior[loc];
	                }
	                m_piData1[loc] = -1;
	            }
	            else
	            {
	                // there are more than 1 loss in the sequence
	                // move the node to the next and update the starter as the next loss inSeqNo(seqno)
	                // find next node
	                int j = (loc + 1) % m_iSize;
	                // remove the "seqno" and change the starter as next seq. no.
	                m_piData1[j] = UdtSequenceNumber.incseq(m_piData1[loc]);
	                // process the sequence end
	                if (UdtSequenceNumber.seqcmp(m_piData2[loc], UdtSequenceNumber.incseq(m_piData1[loc])) > 0)
	                    m_piData2[j] = m_piData2[loc];
	                // remove the current node
	                m_piData1[loc] = -1;
	                m_piData2[loc] = -1;
	                // update list pointer
	                m_piNext[j] = m_piNext[loc];
	                m_piPrior[j] = m_piPrior[loc];
	                if (m_iHead == loc)
	                    m_iHead = j;
	                else
	                    m_piNext[m_piPrior[j]] = j;
	                if (m_iTail == loc)
	                    m_iTail = j;
	                else
	                    m_piPrior[m_piNext[j]] = j;
	            }
	            m_iLength--;
	            return true;
	        }
	        // There is no loss sequence in the current position
	        // the "seqno" may be contained in a previous node
	        // searching previous node
	        int i = (loc - 1 + m_iSize) % m_iSize;
	        while (-1 == m_piData1[i])
	            i = (i - 1 + m_iSize) % m_iSize;
	        // not contained in this node, return
	        if ((-1 == m_piData2[i]) || (UdtSequenceNumber.seqcmp(seqno, m_piData2[i]) > 0))
	            return false;
	        if (seqno == m_piData2[i])
	        {
	            // it is the sequence end
	            if (seqno == UdtSequenceNumber.incseq(m_piData1[i]))
	                m_piData2[i] = -1;
	            else
	                m_piData2[i] = UdtSequenceNumber.decseq(seqno);
	        }
	        else
	        {
	            // split the sequence
	            // construct the second sequence from SequenceNumber.incseq(seqno) to the original sequence end
	            // located at "loc + 1"
	            loc = (loc + 1) % m_iSize;
	            m_piData1[loc] = UdtSequenceNumber.incseq(seqno);
	            if (UdtSequenceNumber.seqcmp(m_piData2[i], m_piData1[loc]) > 0)
	                m_piData2[loc] = m_piData2[i];
	            // the first (original) sequence is between the original sequence start to SequenceNumber.decseq(seqno)
	            if (seqno == UdtSequenceNumber.incseq(m_piData1[i]))
	                m_piData2[i] = -1;
	            else
	                m_piData2[i] = UdtSequenceNumber.decseq(seqno);
	            // update the list pointer
	            m_piNext[loc] = m_piNext[i];
	            m_piNext[i] = loc;
	            m_piPrior[loc] = i;
	            if (m_iTail == i)
	                m_iTail = loc;
	            else
	                m_piPrior[m_piNext[loc]] = loc;
	        }
	        m_iLength--;
	        return true;
	    }
	    public bool remove(int seqno1, int seqno2)
	    {
	        if (seqno1 <= seqno2)
	        {
	            for (int i = seqno1; i <= seqno2; ++i)
	                remove(i);
	        }
	        else
	        {
	            for (int j = seqno1; j < UdtSequenceNumber.m_iMaxSeqNo; ++j)
	                remove(j);
	            for (int k = 0; k <= seqno2; ++k)
	                remove(k);
	        }
	        return true;
	    }
	    bool find(int seqno1, int seqno2)
	    {
	        if (0 == m_iLength)
	            return false;
	        int p = m_iHead;
	        while (-1 != p)
	        {
	            if ((UdtSequenceNumber.seqcmp(m_piData1[p], seqno1) == 0) ||
	                ((UdtSequenceNumber.seqcmp(m_piData1[p], seqno1) > 0) && (UdtSequenceNumber.seqcmp(m_piData1[p], seqno2) <= 0)) ||
	                ((UdtSequenceNumber.seqcmp(m_piData1[p], seqno1) < 0) && (m_piData2[p] != -1) && UdtSequenceNumber.seqcmp(m_piData2[p], seqno1) >= 0))
	                return true;
	            p = m_piNext[p];
	        }
	        return false;
	    }
	    public int getLossLength()
	    {
	        return m_iLength;
	    }
	    public int getFirstLostSeq()
	    {
	        if (0 == m_iLength)
	            return -1;
	        return m_piData1[m_iHead];
	    }
	    public void getLossArray(int[] array, out int len, int limit)
	    {
	        len = 0;
	        int i = m_iHead;
	        while ((len < limit - 1) && (-1 != i))
	        {
	            array[len] = m_piData1[i];
	            if (-1 != m_piData2[i])
	            {
	                // there are more than 1 loss in the sequence
	                array[len] = (int)((uint)array[len] | 0x80000000);
	                ++len;
	                array[len] = m_piData2[i];
	            }
	            ++len;
	            i = m_piNext[i];
	        }
	    }
	}
	internal class UdtSenderBuffer
	{
	    object m_BufLock = new object();           // used to synchronize buffer operation
	    class Block
	    {
	        internal byte[] m_pcData;                   // pointer to the data block
	        internal int m_iLength;                    // length of the block
	        internal uint m_iMsgNo;                 // message number
	        internal ulong m_OriginTime;            // original request time
	        internal int m_iTTL;                       // time to live (milliseconds)
	    }
	    List<Block> mBlockList = new List<Block>();
	    int m_iLastBlock = 0;
	    int m_iCurrentBlock = 0;
	    int m_iFirstBlock = 0;
	    uint m_iNextMsgNo;                // next message number
	    int m_iSize;                // buffer size (number of packets)
	    int m_iMSS;                          // maximum seqment/packet size
	    int m_iCount;           // number of used blocks
	    public UdtSenderBuffer(int size, int mss)
	    {
	        m_iSize = size;
	        m_iMSS = mss;
	        // circular linked list for out bound packets
	        for (int i = 0; i < m_iSize; ++i)
	        {
	            Block block = new Block();
	            block.m_iMsgNo = 0;
	            block.m_pcData = new byte[m_iMSS];
	            mBlockList.Add(block);
	        }
	    }
	    // Functionality:
	    //    Insert a user buffer into the sending list.
	    // Parameters:
	    //    0) [in] data: pointer to the user data block.
	    //    1) [in] len: size of the block.
	    //    2) [in] ttl: time to live in milliseconds
	    //    3) [in] order: if the block should be delivered in order, for DGRAM only
	    // Returned value:
	    //    None.
	    public void addBuffer(byte[] data, int offset, int len, int ttl = -1, bool order = false)
	    {
	        int size = len / m_iMSS;
	        if ((len % m_iMSS) != 0)
	            size++;
	        // dynamically increase sender buffer
	        while (size + m_iCount >= m_iSize)
	            increase();
	        ulong time = UdtTimer.getTime();
	        uint inorder = Convert.ToUInt32(order);
	        inorder <<= 29;
	        for (int i = 0; i < size; ++i)
	        {
	            Block s = mBlockList[m_iLastBlock];
	            IncrementBlockIndex(ref m_iLastBlock);
	            int pktlen = len - i * m_iMSS;
	            if (pktlen > m_iMSS)
	                pktlen = m_iMSS;
	            Array.Copy(data, i * m_iMSS + offset, s.m_pcData, 0, pktlen);
	            s.m_iLength = pktlen;
	            s.m_iMsgNo = m_iNextMsgNo | inorder;
	            if (i == 0)
	                s.m_iMsgNo |= 0x80000000;
	            if (i == size - 1)
	                s.m_iMsgNo |= 0x40000000;
	            s.m_OriginTime = time;
	            s.m_iTTL = ttl;
	        }
	        lock (m_BufLock)
	        {
	            m_iCount += size;
	        }
	        m_iNextMsgNo++;
	        if (m_iNextMsgNo == UdtMessageNumber.m_iMaxMsgNo)
	            m_iNextMsgNo = 1;
	    }
	    public int readData(ref byte[] data, ref uint msgno)
	    {
	        // No data to read
	        if (m_iCurrentBlock == m_iLastBlock)
	            return 0;
	        data = mBlockList[m_iCurrentBlock].m_pcData;
	        int readlen = mBlockList[m_iCurrentBlock].m_iLength;
	        msgno = mBlockList[m_iCurrentBlock].m_iMsgNo;
	        IncrementBlockIndex(ref m_iCurrentBlock);
	        return readlen;
	    }
	    public int readData(ref byte[] data, int offset, ref uint msgno, out int msglen)
	    {
	        msglen = 0;
	        lock (m_BufLock)
	        {
	            int blockIndex = m_iFirstBlock;
	            IncrementBlockIndex(ref blockIndex, offset);
	            Block p = mBlockList[blockIndex];
	            if ((p.m_iTTL >= 0) && ((UdtTimer.getTime() - p.m_OriginTime) / 1000 > (ulong)p.m_iTTL))
	            {
	                msgno = p.m_iMsgNo & 0x1FFFFFFF;
	                msglen = 1;
	                IncrementBlockIndex(ref blockIndex);
	                p = mBlockList[blockIndex];
	                bool move = false;
	                while (msgno == (p.m_iMsgNo & 0x1FFFFFFF))
	                {
	                    if (blockIndex == m_iCurrentBlock)
	                        move = true;
	                    IncrementBlockIndex(ref blockIndex);
	                    p = mBlockList[blockIndex];
	                    if (move)
	                        m_iCurrentBlock = blockIndex;
	                    msglen++;
	                }
	                return -1;
	            }
	            data = p.m_pcData;
	            int readlen = p.m_iLength;
	            msgno = p.m_iMsgNo;
	            return readlen;
	        }
	    }
	    void IncrementBlockIndex(ref int blockIndex, int offset = 1)
	    {
	        blockIndex = (blockIndex + offset) % mBlockList.Count;
	    }
	    public void ackData(int offset)
	    {
	        lock (m_BufLock)
	        {
	            IncrementBlockIndex(ref m_iFirstBlock, offset);
	            m_iCount -= offset;
	            UdtTimer.triggerEvent();
	        }
	    }
	    public int getCurrBufSize()
	    {
	        return m_iCount;
	    }
	    void increase()
	    {
	        int unitsize = m_iSize;
	        for (int i = 0; i < unitsize; ++i)
	        {
	            Block block = new Block();
	            block.m_iMsgNo = 0;
	            block.m_pcData = new byte[m_iMSS];
	            mBlockList.Add(block);
	        }
	        m_iSize += unitsize;
	    }
	}
	internal class UdtSenderLossList
	{
	    int[] m_piData1;                  // sequence number starts
	    int[] m_piData2;                  // seqnence number ends
	    int[] m_piNext;                       // next node in the list
	    int m_iHead;                         // first node
	    int m_iLength;                       // loss length
	    int m_iSize;                         // size of the static array
	    int m_iLastInsertPos;                // position of last insert node
	    object m_ListLock = new object();          // used to synchronize list operation
	    public UdtSenderLossList(int size)
	    {
	        m_iHead = -1;
	        m_iLength = 0;
	        m_iSize = size;
	        m_iLastInsertPos = -1;
	        m_piData1 = new int[m_iSize];
	        m_piData2 = new int[m_iSize];
	        m_piNext = new int[m_iSize];
	        // -1 means there is no data in the node
	        for (int i = 0; i < size; ++i)
	        {
	            m_piData1[i] = -1;
	            m_piData2[i] = -1;
	        }
	    }
	    public int insert(int seqno1, int seqno2)
	    {
	        lock (m_ListLock)
	        {
	            return insert_unsafe(seqno1, seqno2);
	        }
	    }
	    int insert_unsafe(int seqno1, int seqno2)
	    {
	        if (0 == m_iLength)
	        {
	            // insert data into an empty list
	            m_iHead = 0;
	            m_piData1[m_iHead] = seqno1;
	            if (seqno2 != seqno1)
	                m_piData2[m_iHead] = seqno2;
	            m_piNext[m_iHead] = -1;
	            m_iLastInsertPos = m_iHead;
	            m_iLength += UdtSequenceNumber.seqlen(seqno1, seqno2);
	            return m_iLength;
	        }
	        // otherwise find the position where the data can be inserted
	        int origlen = m_iLength;
	        int offset = UdtSequenceNumber.seqoff(m_piData1[m_iHead], seqno1);
	        int loc = (m_iHead + offset + m_iSize) % m_iSize;
	        if (offset < 0)
	        {
	            // Insert data prior to the head pointer
	            m_piData1[loc] = seqno1;
	            if (seqno2 != seqno1)
	                m_piData2[loc] = seqno2;
	            // new node becomes head
	            m_piNext[loc] = m_iHead;
	            m_iHead = loc;
	            m_iLastInsertPos = loc;
	            m_iLength += UdtSequenceNumber.seqlen(seqno1, seqno2);
	        }
	        else if (offset > 0)
	        {
	            if (seqno1 == m_piData1[loc])
	            {
	                m_iLastInsertPos = loc;
	                // first seqno is equivlent, compare the second
	                if (-1 == m_piData2[loc])
	                {
	                    if (seqno2 != seqno1)
	                    {
	                        m_iLength += UdtSequenceNumber.seqlen(seqno1, seqno2) - 1;
	                        m_piData2[loc] = seqno2;
	                    }
	                }
	                else if (UdtSequenceNumber.seqcmp(seqno2, m_piData2[loc]) > 0)
	                {
	                    // new seq pair is longer than old pair, e.g., insert [3, 7] to [3, 5], becomes [3, 7]
	                    m_iLength += UdtSequenceNumber.seqlen(m_piData2[loc], seqno2) - 1;
	                    m_piData2[loc] = seqno2;
	                }
	                else
	                    // Do nothing if it is already there
	                    return 0;
	            }
	            else
	            {
	                // searching the prior node
	                int i;
	                if ((-1 != m_iLastInsertPos) && (UdtSequenceNumber.seqcmp(m_piData1[m_iLastInsertPos], seqno1) < 0))
	                    i = m_iLastInsertPos;
	                else
	                    i = m_iHead;
	                while ((-1 != m_piNext[i]) && (UdtSequenceNumber.seqcmp(m_piData1[m_piNext[i]], seqno1) < 0))
	                    i = m_piNext[i];
	                if ((-1 == m_piData2[i]) || (UdtSequenceNumber.seqcmp(m_piData2[i], seqno1) < 0))
	                {
	                    m_iLastInsertPos = loc;
	                    // no overlap, create new node
	                    m_piData1[loc] = seqno1;
	                    if (seqno2 != seqno1)
	                        m_piData2[loc] = seqno2;
	                    m_piNext[loc] = m_piNext[i];
	                    m_piNext[i] = loc;
	                    m_iLength += UdtSequenceNumber.seqlen(seqno1, seqno2);
	                }
	                else
	                {
	                    m_iLastInsertPos = i;
	                    // overlap, coalesce with prior node, insert(3, 7) to [2, 5], ... becomes [2, 7]
	                    if (UdtSequenceNumber.seqcmp(m_piData2[i], seqno2) < 0)
	                    {
	                        m_iLength += UdtSequenceNumber.seqlen(m_piData2[i], seqno2) - 1;
	                        m_piData2[i] = seqno2;
	                        loc = i;
	                    }
	                    else
	                        return 0;
	                }
	            }
	        }
	        else
	        {
	            m_iLastInsertPos = m_iHead;
	            // insert to head node
	            if (seqno2 != seqno1)
	            {
	                if (-1 == m_piData2[loc])
	                {
	                    m_iLength += UdtSequenceNumber.seqlen(seqno1, seqno2) - 1;
	                    m_piData2[loc] = seqno2;
	                }
	                else if (UdtSequenceNumber.seqcmp(seqno2, m_piData2[loc]) > 0)
	                {
	                    m_iLength += UdtSequenceNumber.seqlen(m_piData2[loc], seqno2) - 1;
	                    m_piData2[loc] = seqno2;
	                }
	                else
	                    return 0;
	            }
	            else
	                return 0;
	        }
	        // coalesce with next node. E.g., [3, 7], ..., [6, 9] becomes [3, 9] 
	        while ((-1 != m_piNext[loc]) && (-1 != m_piData2[loc]))
	        {
	            int i = m_piNext[loc];
	            if (UdtSequenceNumber.seqcmp(m_piData1[i], UdtSequenceNumber.incseq(m_piData2[loc])) <= 0)
	            {
	                // coalesce if there is overlap
	                if (-1 != m_piData2[i])
	                {
	                    if (UdtSequenceNumber.seqcmp(m_piData2[i], m_piData2[loc]) > 0)
	                    {
	                        if (UdtSequenceNumber.seqcmp(m_piData2[loc], m_piData1[i]) >= 0)
	                            m_iLength -= UdtSequenceNumber.seqlen(m_piData1[i], m_piData2[loc]);
	                        m_piData2[loc] = m_piData2[i];
	                    }
	                    else
	                        m_iLength -= UdtSequenceNumber.seqlen(m_piData1[i], m_piData2[i]);
	                }
	                else
	                {
	                    if (m_piData1[i] == UdtSequenceNumber.incseq(m_piData2[loc]))
	                        m_piData2[loc] = m_piData1[i];
	                    else
	                        m_iLength--;
	                }
	                m_piData1[i] = -1;
	                m_piData2[i] = -1;
	                m_piNext[loc] = m_piNext[i];
	            }
	            else
	                break;
	        }
	        return m_iLength - origlen;
	    }
	    public void remove(int seqno)
	    {
	        lock (m_ListLock)
	        {
	            remove_unsafe(seqno);
	        }
	    }
	    void remove_unsafe(int seqno)
	    {
	        if (0 == m_iLength)
	            return;
	        // Remove all from the head pointer to a node with a larger seq. no. or the list is empty
	        int offset = UdtSequenceNumber.seqoff(m_piData1[m_iHead], seqno);
	        int loc = (m_iHead + offset + m_iSize) % m_iSize;
	        if (0 == offset)
	        {
	            // It is the head. Remove the head and point to the next node
	            loc = (loc + 1) % m_iSize;
	            if (-1 == m_piData2[m_iHead])
	                loc = m_piNext[m_iHead];
	            else
	            {
	                m_piData1[loc] = UdtSequenceNumber.incseq(seqno);
	                if (UdtSequenceNumber.seqcmp(m_piData2[m_iHead], UdtSequenceNumber.incseq(seqno)) > 0)
	                    m_piData2[loc] = m_piData2[m_iHead];
	                m_piData2[m_iHead] = -1;
	                m_piNext[loc] = m_piNext[m_iHead];
	            }
	            m_piData1[m_iHead] = -1;
	            if (m_iLastInsertPos == m_iHead)
	                m_iLastInsertPos = -1;
	            m_iHead = loc;
	            m_iLength--;
	        }
	        else if (offset > 0)
	        {
	            int h = m_iHead;
	            if (seqno == m_piData1[loc])
	            {
	                // target node is not empty, remove part/all of the seqno in the node.
	                int temp = loc;
	                loc = (loc + 1) % m_iSize;
	                if (-1 == m_piData2[temp])
	                    m_iHead = m_piNext[temp];
	                else
	                {
	                    // remove part, e.g., [3, 7] becomes [], [4, 7] after remove(3)
	                    m_piData1[loc] = UdtSequenceNumber.incseq(seqno);
	                    if (UdtSequenceNumber.seqcmp(m_piData2[temp], m_piData1[loc]) > 0)
	                        m_piData2[loc] = m_piData2[temp];
	                    m_iHead = loc;
	                    m_piNext[loc] = m_piNext[temp];
	                    m_piNext[temp] = loc;
	                    m_piData2[temp] = -1;
	                }
	            }
	            else
	            {
	                // target node is empty, check prior node
	                int i = m_iHead;
	                while ((-1 != m_piNext[i]) && (UdtSequenceNumber.seqcmp(m_piData1[m_piNext[i]], seqno) < 0))
	                    i = m_piNext[i];
	                loc = (loc + 1) % m_iSize;
	                if (-1 == m_piData2[i])
	                    m_iHead = m_piNext[i];
	                else if (UdtSequenceNumber.seqcmp(m_piData2[i], seqno) > 0)
	                {
	                    // remove part/all seqno in the prior node
	                    m_piData1[loc] = UdtSequenceNumber.incseq(seqno);
	                    if (UdtSequenceNumber.seqcmp(m_piData2[i], m_piData1[loc]) > 0)
	                        m_piData2[loc] = m_piData2[i];
	                    m_piData2[i] = seqno;
	                    m_piNext[loc] = m_piNext[i];
	                    m_piNext[i] = loc;
	                    m_iHead = loc;
	                }
	                else
	                    m_iHead = m_piNext[i];
	            }
	            // Remove all nodes prior to the new head
	            while (h != m_iHead)
	            {
	                if (m_piData2[h] != -1)
	                {
	                    m_iLength -= UdtSequenceNumber.seqlen(m_piData1[h], m_piData2[h]);
	                    m_piData2[h] = -1;
	                }
	                else
	                    m_iLength--;
	                m_piData1[h] = -1;
	                if (m_iLastInsertPos == h)
	                    m_iLastInsertPos = -1;
	                h = m_piNext[h];
	            }
	        }
	    }
	    public int getLossLength()
	    {
	        lock (m_ListLock)
	        {
	            return m_iLength;
	        }
	    }
	    public int getLostSeq()
	    {
	        if (0 == m_iLength)
	            return -1;
	        lock (m_ListLock)
	        {
	            if (0 == m_iLength)
	                return -1;
	            if (m_iLastInsertPos == m_iHead)
	                m_iLastInsertPos = -1;
	            // return the first loss seq. no.
	            int seqno = m_piData1[m_iHead];
	            // head moves to the next node
	            if (-1 == m_piData2[m_iHead])
	            {
	                //[3, -1] becomes [], and head moves to next node in the list
	                m_piData1[m_iHead] = -1;
	                m_iHead = m_piNext[m_iHead];
	            }
	            else
	            {
	                // shift to next node, e.g., [3, 7] becomes [], [4, 7]
	                int loc = (m_iHead + 1) % m_iSize;
	                m_piData1[loc] = UdtSequenceNumber.incseq(seqno);
	                if (UdtSequenceNumber.seqcmp(m_piData2[m_iHead], m_piData1[loc]) > 0)
	                    m_piData2[loc] = m_piData2[m_iHead];
	                m_piData1[m_iHead] = -1;
	                m_piData2[m_iHead] = -1;
	                m_piNext[loc] = m_piNext[m_iHead];
	                m_iHead = loc;
	            }
	            m_iLength--;
	            return seqno;
	        }
	    }
	}
	internal class UdtSequenceNumber
	{
	    public static int seqcmp(int seq1, int seq2)
	    { 
	        return (Math.Abs(seq1 - seq2) < m_iSeqNoTH) ? (seq1 - seq2) : (seq2 - seq1);
	    }
	    public static int seqlen(int seq1, int seq2)
	    {
	        return (seq1 <= seq2) ? (seq2 - seq1 + 1) : (seq2 - seq1 + m_iMaxSeqNo + 2);
	    }
	    public static int seqoff(int seq1, int seq2)
	    {
	        if (Math.Abs(seq1 - seq2) < m_iSeqNoTH)
	            return seq2 - seq1;
	        if (seq1 < seq2)
	            return seq2 - seq1 - m_iMaxSeqNo - 1;
	        return seq2 - seq1 + m_iMaxSeqNo + 1;
	    }
	    public static int incseq(int seq)
	    {
	        return (seq == m_iMaxSeqNo) ? 0 : seq + 1;
	    }
	    public static int decseq(int seq)
	    {
	        return (seq == 0) ? m_iMaxSeqNo : seq - 1;
	    }
	    public static int incseq(int seq, int inc)
	    {
	        return (m_iMaxSeqNo - seq >= inc) ? seq + inc : seq - m_iMaxSeqNo + inc - 1;
	    }
	    public static int m_iSeqNoTH = 0x3FFFFFFF;             // threshold for comparing seq. no.
	    public static int m_iMaxSeqNo = 0x7FFFFFFF;            // maximum sequence number used in UDT
	}
	public class UdtSocket
	{
	    public UdtSocket(AddressFamily addressFamily, SocketType socketType)
	    {
	        UdtCongestionControl.s_UDTUnited.startup();
	        try
	        {
	            mSocketId = UdtCongestionControl.s_UDTUnited.newSocket(addressFamily, socketType);
	            mLocalEndPoint = new IPEndPoint(IPAddress.Any, 0);
	        }
	        catch (UdtException udtException)
	        {
	            throw new Exception($"Problem when initializing new socket type {socketType}", udtException);
	        }
	    }
	    public int Bind(Socket socket)
	    {
	        try
	        {
	            int status = UdtCongestionControl.s_UDTUnited.bind(mSocketId, socket);
	            mLocalEndPoint = (IPEndPoint)socket.LocalEndPoint;
	            return status;
	        }
	        catch (UdtException udtException)
	        {
	            throw new Exception("Problem when binding to existing socket", udtException);
	        }
	    }
	    public int Bind(IPEndPoint serverAddress)
	    {
	        try
	        {
	            int status = UdtCongestionControl.s_UDTUnited.bind(mSocketId, serverAddress);
	            mLocalEndPoint = serverAddress;
	            return status;
	        }
	        catch (UdtException udtException)
	        {
	            throw new Exception($"Problem when binding to server address {serverAddress}", udtException);
	        }
	    }
	    public int Listen(int maxConnections)
	    {
	        try
	        {
	            return UdtCongestionControl.s_UDTUnited.listen(mSocketId, maxConnections);
	        }
	        catch (UdtException udtException)
	        {
	            throw new Exception($"Problem when listening with {maxConnections}", udtException);
	        }
	    }
	    public UdtSocket Accept()
	    {
	        try
	        {
	            IPEndPoint clientEndPoint = null;
	            int clientSocketId = UdtCongestionControl.s_UDTUnited.accept(mSocketId, ref clientEndPoint);
	            if (clientSocketId == UdtCongestionControl.INVALID_SOCK)
	            {
	                return null;
	            }
	            return new UdtSocket(clientSocketId, clientEndPoint, mLocalEndPoint);
	        }
	        catch (UdtException udtException)
	        {
	            throw new Exception("Problem when accepting socket", udtException);
	        }
	    }
	    public int Connect(IPEndPoint server)
	    {
	        try
	        {
	            int status = UdtCongestionControl.s_UDTUnited.connect(mSocketId, server);
	            mRemoteEndPoint = server;
	            return status;
	        }
	        catch (UdtException udtException)
	        {
	            throw new Exception($"Problem when connecting to server endpoint {server}", udtException);
	        }
	    }
	    public bool IsConnected()
	    {
	        return UdtCongestionControl.s_UDTUnited.getStatus(mSocketId) == UdtStatus.Connected;
	    }
	    public int Send(byte[] data, int offset, int length)
	    {
	        try
	        {
	            UdtCongestionControl udt = UdtCongestionControl.s_UDTUnited.lookup(mSocketId);
	            return udt.send(data, offset, length);
	        }
	        catch (UdtException udtException)
	        {
	            throw new Exception("Problem when sending data", udtException);
	        }
	    }
	    public int Receive(byte[] data, int offset, int length)
	    {
	        try
	        {
	            UdtCongestionControl udt = UdtCongestionControl.s_UDTUnited.lookup(mSocketId);
	            return udt.recv(data, offset, length);
	        }
	        catch (UdtException udtException)
	        {
	            throw new Exception("Problem when receiving data", udtException);
	        }
	    }
	    public int Close()
	    {
	        try
	        {
	            return UdtCongestionControl.s_UDTUnited.close(mSocketId);
	        }
	        catch (UdtException udtException)
	        {
	            throw new Exception("Problem when closing socket", udtException);
	        }
	    }
	    public IPEndPoint LocalEndPoint { get { return mLocalEndPoint; } }
	    public IPEndPoint RemoteEndPoint { get { return mRemoteEndPoint; } }
	    UdtSocket(int iSocketID, IPEndPoint localEndPoint, IPEndPoint remoteEndPoint)
	    {
	        mSocketId = iSocketID;
	        mLocalEndPoint = localEndPoint;
	        mRemoteEndPoint = remoteEndPoint;
	    }
	    int mSocketId;
	    IPEndPoint mLocalEndPoint;
	    IPEndPoint mRemoteEndPoint;
	    delegate int ReceiveDelegate(byte[] buffer, int offset, int count);
	    ReceiveDelegate mReceiveDelegate = null;
	    delegate int SendDelegate(byte[] buffer, int offset, int count);
	    SendDelegate mSendDelegate = null;
	}
	internal class UdtSocketInternal
	{
	    public UdtStatus m_Status;                       // current socket state
	    public ulong m_TimeStamp;                     // time when the socket is closed
	    public AddressFamily m_iIPversion;                         // IP version
	    public IPEndPoint m_pSelfAddr;                    // pointer to the local address of the socket
	    public IPEndPoint m_pPeerAddr;                    // pointer to the peer address of the socket
	    public UDTSOCKET m_SocketID;                     // socket ID
	    public UDTSOCKET m_ListenSocket;                 // ID of the listener socket; 0 means this is an independent socket
	    public UDTSOCKET m_PeerID;                       // peer socket ID
	    public int m_iISN;                           // initial sequence number, used to tell different connection from same IP:port
	    public UdtCongestionControl m_pUDT;                             // pointer to the UDT entity
	    public HashSet<UDTSOCKET> m_pQueuedSockets;    // set of connections waiting for accept()
	    public HashSet<UDTSOCKET> m_pAcceptSockets;    // set of accept()ed connections
	    public EventWaitHandle m_AcceptCond = new EventWaitHandle(false, EventResetMode.AutoReset);// used to block "accept" call
	    public object m_AcceptLock = new object();             // mutex associated to m_AcceptCond
	    public uint m_uiBackLog;                 // maximum number of connections in queue
	    public int m_iMuxID;                             // multiplexer ID
	    public object m_ControlLock = new object();            // lock this socket exclusively for control APIs: bind/listen/connect
	    public UdtSocketInternal()
	    {
	        m_Status = UdtStatus.Initializing;
	        m_iMuxID = -1;
	    }
	    public void Close()
	    {
	        m_AcceptCond.Close();
	    }
	}
	internal class UdtTimer
	{
	    ulong m_ullSchedTime;             // next schedulled time
	    static ulong s_ullCPUFrequency = readCPUFrequency();// CPU frequency : clock cycles per microsecond
	    EventWaitHandle m_TickCond = new EventWaitHandle(false, EventResetMode.AutoReset);
	    object m_TickLock = new object();
	    static EventWaitHandle m_EventCond = new EventWaitHandle(false, EventResetMode.AutoReset);
	    static object m_EventLock = new object();
	    static bool m_bUseMicroSecond = false; // sepcial handling if timer frequency is low (< 10 ticks per microsecond)
	    public UdtTimer()
	    {
	    }
	    public static ulong rdtsc()
	    {
	        if (m_bUseMicroSecond)
	        {
	            return getTime();
	        }
	        return (ulong)Stopwatch.GetTimestamp();
	    }
	    public void Stop()
	    {
	        m_TickCond.Close();
	    }
	    static ulong readCPUFrequency()
	    {
	        long ticksPerSecond = Stopwatch.Frequency;
	        long ticksPerMicroSecond = ticksPerSecond / 1000000L;
	        if (ticksPerMicroSecond < 10)
	        {
	            m_bUseMicroSecond = true;
	            return 1;
	        }
	        return (ulong)ticksPerMicroSecond;
	    }
	    public static ulong getCPUFrequency()
	    {
	        // ticks per microsecond
	        return (ulong)s_ullCPUFrequency;
	    }
	    void sleep(ulong interval)
	    {
	        ulong t = rdtsc();
	        // sleep next "interval" time
	        sleepto(t + interval);
	    }
	    public void sleepto(ulong nexttime)
	    {
	        // Use class member such that the method can be interrupted by others
	        m_ullSchedTime = nexttime;
	        ulong t = rdtsc();
	        while (t < m_ullSchedTime)
	        {
	            m_TickCond.WaitOne(1);
	            t = rdtsc();
	        }
	    }
	    public void interrupt()
	    {
	        // schedule the sleepto time to the current CCs, so that it will stop
	        m_ullSchedTime = rdtsc();
	        tick();
	    }
	    public void tick()
	    {
	        m_TickCond.Set();
	    }
	    public static ulong getTime()
	    {
	        // microsecond resolution
	        return (ulong)DateTime.Now.Ticks / 10;
	    }
	    public static void triggerEvent()
	    {
	        m_EventCond.Set();
	    }
	    static void waitForEvent()
	    {
	        m_EventCond.WaitOne(1);
	    }
	    static void sleep()
	    {
	        Thread.Sleep(1);
	    }
	}
	internal class UdtUnit
	{
	    public UdtPacket m_Packet = new UdtPacket();       // packet
	    public int m_iFlag;            // 0: free, 1: occupied, 2: msg read but not freed (out-of-order), 3: msg dropped
	};
	internal class UdtUnited
	{
	    Dictionary<UDTSOCKET, UdtSocketInternal> m_Sockets = new Dictionary<UDTSOCKET, UdtSocketInternal>();       // stores all the socket structures
	    object m_ControlLock = new object();                    // used to synchronize UDT API
	    object m_IDLock = new object();                         // used to synchronize ID generation
	    UDTSOCKET m_SocketID;                             // seed to generate a new unique socket ID
	    Dictionary<long, HashSet<UDTSOCKET>> m_PeerRec = new Dictionary<long, HashSet<UDTSOCKET>>();// record sockets from peers to avoid repeated connection request, int64_t = (socker_id << 30) + isn
	    //pthread_key_t m_TLSError;                         // thread local error record (last error)
	    Dictionary<uint, UdtException> m_mTLSRecord;
	    object m_TLSLock = new object();
	    Dictionary<int, UdtMultiplexer> m_mMultiplexer = new Dictionary<UDTSOCKET, UdtMultiplexer>();      // UDP multiplexer
	    object m_MultiplexerLock = new object();
	    Dictionary<IPAddress, UdtInfoBlock> m_pCache = new Dictionary<IPAddress, UdtInfoBlock>();            // UDT network information cache
	    volatile bool m_bClosing;
	    object m_GCStopLock = new object();
	    EventWaitHandle m_GCStopCond = new EventWaitHandle(false, EventResetMode.AutoReset);
	    object m_InitLock = new object();
	    int m_iInstanceCount;               // number of startup() called by application
	    Dictionary<UDTSOCKET, UdtSocketInternal> m_ClosedSockets = new Dictionary<UDTSOCKET, UdtSocketInternal>();   // temporarily store closed sockets
	    static Random m_random = new Random();
	    public UdtUnited()
	    {
	        // Socket ID MUST start from a random value
	        m_SocketID = 1 + (int)((1 << 30) * m_random.NextDouble());
	        //m_TLSError = TlsAlloc();
	    }
	    ~UdtUnited()
	    {
	        //TlsFree(m_TLSError);
	    }
	    public int startup()
	    {
	        lock (m_InitLock)
	        {
	            ++m_iInstanceCount;
	            return 0;
	        }
	    }
	    public int cleanup()
	    {
	        lock (m_InitLock)
	        {
	            if (--m_iInstanceCount > 0)
	            {
	                return 0;
	            }
	        }
	        m_bClosing = true;
	        return 0;
	    }
	    public UDTSOCKET newSocket(AddressFamily af, SocketType type)
	    {
	        if ((type != SocketType.Stream) && (type != SocketType.Dgram))
	        {
	            throw new UdtException(5, 3, 0);
	        }
	        UdtSocketInternal ns = new UdtSocketInternal();
	        ns.m_pUDT = new UdtCongestionControl();
	        ns.m_pSelfAddr = new IPEndPoint(IPAddress.Any, 0);
	        lock (m_IDLock)
	        {
	            ns.m_SocketID = --m_SocketID;
	        }
	        ns.m_Status = UdtStatus.Initializing;
	        ns.m_ListenSocket = 0;
	        ns.m_pUDT.m_SocketID = ns.m_SocketID;
	        ns.m_pUDT.m_iSockType = type;
	        ns.m_pUDT.m_iIPversion = af;
	        ns.m_pUDT.m_pCache = m_pCache;
	        // protect the m_Sockets structure.
	        lock (m_ControlLock)
	        {
	            m_Sockets[ns.m_SocketID] = ns;
	        }
	        return ns.m_SocketID;
	    }
	    public int newConnection(UDTSOCKET listen, IPEndPoint peer, UdtHandshake hs)
	    {
	        UdtSocketInternal ns = null;
	        UdtSocketInternal ls = locate(listen);
	        if (null == ls)
	        {
	            return -1;
	        }
	        // if this connection has already been processed
	        if (null != (ns = locate(peer, hs.SocketId, hs.InitialSequenceNumber)))
	        {
	            if (ns.m_pUDT.m_bBroken)
	            {
	                // last connection from the "peer" address has been broken
	                ns.m_Status = UdtStatus.Closed;
	                ns.m_TimeStamp = UdtTimer.getTime();
	                lock (ls.m_AcceptLock)
	                {
	                    ls.m_pQueuedSockets.Remove(ns.m_SocketID);
	                    ls.m_pAcceptSockets.Remove(ns.m_SocketID);
	                }
	            }
	            else
	            {
	                // connection already exist, this is a repeated connection request
	                // respond with existing HS information
	                hs.InitialSequenceNumber = ns.m_pUDT.m_iISN;
	                hs.MaximumSegmentSize = ns.m_pUDT.m_iMSS;
	                hs.FlowControlWindowSize = ns.m_pUDT.m_iFlightFlagSize;
	                hs.RequestType = -1;
	                hs.SocketId = ns.m_SocketID;
	                return 0;
	                //except for this situation a new connection should be started
	            }
	        }
	        // exceeding backlog, refuse the connection request
	        if (ls.m_pQueuedSockets.Count >= ls.m_uiBackLog)
	        {
	            return -1;
	        }
	        ns = new UdtSocketInternal();
	        ns.m_pUDT = new UdtCongestionControl(ls.m_pUDT);
	        ns.m_pSelfAddr = new IPEndPoint(IPAddress.Any, 0);
	        ns.m_pPeerAddr = peer;
	        lock (m_IDLock)
	        {
	            ns.m_SocketID = --m_SocketID;
	        }
	        ns.m_ListenSocket = listen;
	        ns.m_iIPversion = ls.m_iIPversion;
	        ns.m_pUDT.m_SocketID = ns.m_SocketID;
	        ns.m_PeerID = hs.SocketId;
	        ns.m_iISN = hs.InitialSequenceNumber;
	        int error = 0;
	        try
	        {
	            // bind to the same addr of listening socket
	            ns.m_pUDT.open();
	            updateMux(ns, ls);
	            ns.m_pUDT.connect(peer, hs);
	        }
	        catch (Exception e)
	        {
	            error = 1;
	            goto ERR_ROLLBACK;
	        }
	        ns.m_Status = UdtStatus.Connected;
	        // copy address information of local node
	        ns.m_pUDT.m_pSndQueue.m_pChannel.getSockAddr(ref ns.m_pSelfAddr);
	        ConvertIPAddress.ToUintArray(ns.m_pSelfAddr.Address, ref ns.m_pUDT.m_piSelfIP);
	        // protect the m_Sockets structure.
	        lock (m_ControlLock)
	        {
	            m_Sockets[ns.m_SocketID] = ns;
	            HashSet<int> sockets;
	            if (!m_PeerRec.TryGetValue((ns.m_PeerID << 30) + ns.m_iISN, out sockets))
	            {
	                sockets = new HashSet<int>();
	                m_PeerRec.Add((ns.m_PeerID << 30) + ns.m_iISN, sockets);
	            }
	            sockets.Add(ns.m_SocketID);
	        }
	        lock (ls.m_AcceptLock)
	        {
	            ls.m_pQueuedSockets.Add(ns.m_SocketID);
	        }
	        // acknowledge users waiting for new connections on the listening socket
	        //m_EPoll.update_events(listen, ls.m_pUDT.m_sPollID, UDT_EPOLL_IN, true);
	        UdtTimer.triggerEvent();
	    ERR_ROLLBACK:
	        if (error > 0)
	        {
	            ns.m_pUDT.close();
	            ns.m_Status = UdtStatus.Closed;
	            ns.m_TimeStamp = UdtTimer.getTime();
	            return -1;
	        }
	        // wake up a waiting accept() call
	        ls.m_AcceptCond.Set();
	        return 1;
	    }
	    public UdtCongestionControl lookup(UDTSOCKET u)
	    {
	        // protects the m_Sockets structure
	        lock (m_ControlLock)
	        {
	            UdtSocketInternal socket;
	            if (!m_Sockets.TryGetValue(u, out socket) || socket.m_Status == UdtStatus.Closed)
	            {
	                throw new UdtException(5, 4, 0);
	            }
	            return socket.m_pUDT;
	        }
	    }
	    public UdtStatus getStatus(UDTSOCKET u)
	    {
	        // protects the m_Sockets structure
	        lock (m_ControlLock)
	        {
	            UdtSocketInternal socket;
	            if (m_Sockets.TryGetValue(u, out socket))
	            {
	                if (socket.m_pUDT.m_bBroken)
	                {
	                    return UdtStatus.Broken;
	                }
	                return socket.m_Status;
	            }
	            if (m_ClosedSockets.ContainsKey(u))
	            {
	                return UdtStatus.Closed;
	            }
	            return UdtStatus.NonExist;
	        }
	    }
	    public int bind(UDTSOCKET u, IPEndPoint name)
	    {
	        UdtSocketInternal s = locate(u);
	        if (null == s)
	        {
	            throw new UdtException(5, 4, 0);
	        }
	        lock (s.m_ControlLock)
	        {
	            // cannot bind a socket more than once
	            if (UdtStatus.Initializing != s.m_Status)
	            {
	                throw new UdtException(5, 0, 0);
	            }
	            s.m_pUDT.open();
	            updateMux(s, name);
	            s.m_Status = UdtStatus.Opened;
	            // copy address information of local node
	            s.m_pUDT.m_pSndQueue.m_pChannel.getSockAddr(ref s.m_pSelfAddr);
	            return 0;
	        }
	    }
	    public int bind(UDTSOCKET u, Socket udpsock)
	    {
	        UdtSocketInternal s = locate(u);
	        if (null == s)
	        {
	            throw new UdtException(5, 4, 0);
	        }
	        lock (s.m_ControlLock)
	        {
	            // cannot bind a socket more than once
	            if (UdtStatus.Initializing != s.m_Status)
	            {
	                throw new UdtException(5, 0, 0);
	            }
	            IPEndPoint name = (IPEndPoint)udpsock.LocalEndPoint;
	            s.m_pUDT.open();
	            updateMux(s, name, udpsock);
	            s.m_Status = UdtStatus.Opened;
	            // copy address information of local node
	            s.m_pUDT.m_pSndQueue.m_pChannel.getSockAddr(ref s.m_pSelfAddr);
	            return 0;
	        }
	    }
	    public int listen(UDTSOCKET u, int backlog)
	    {
	        UdtSocketInternal s = locate(u);
	        if (null == s)
	        {
	            throw new UdtException(5, 4, 0);
	        }
	        lock (s.m_ControlLock)
	        {
	            // do nothing if the socket is already listening
	            if (UdtStatus.Listening == s.m_Status)
	            {
	                return 0;
	            }
	            // a socket can listen only if is in UDTSTATUS.OPENED status
	            if (UdtStatus.Opened != s.m_Status)
	            {
	                throw new UdtException(5, 5, 0);
	            }
	            // listen is not supported in rendezvous connection setup
	            if (s.m_pUDT.m_bRendezvous)
	            {
	                throw new UdtException(5, 7, 0);
	            }
	            if (backlog <= 0)
	            {
	                throw new UdtException(5, 3, 0);
	            }
	            s.m_uiBackLog = (uint)backlog;
	            s.m_pQueuedSockets = new HashSet<UDTSOCKET>();
	            s.m_pAcceptSockets = new HashSet<UDTSOCKET>();
	            s.m_pUDT.listen();
	            s.m_Status = UdtStatus.Listening;
	            return 0;
	        }
	    }
	    public UDTSOCKET accept(UDTSOCKET listen, ref IPEndPoint addr)
	    {
	        if (null != addr)
	        {
	            throw new UdtException(5, 3, 0);
	        }
	        UdtSocketInternal ls = locate(listen);
	        if (ls == null)
	        {
	            throw new UdtException(5, 4, 0);
	        }
	        // the "listen" socket must be in UDTSTATUS.LISTENING status
	        if (UdtStatus.Listening != ls.m_Status)
	        {
	            throw new UdtException(5, 6, 0);
	        }
	        // no "accept" in rendezvous connection setup
	        if (ls.m_pUDT.m_bRendezvous)
	        {
	            throw new UdtException(5, 7, 0);
	        }
	        UDTSOCKET u = UdtCongestionControl.INVALID_SOCK;
	        bool accepted = false;
	        // !!only one conection can be set up each time!!
	        while (!accepted)
	        {
	            lock (ls.m_AcceptLock)
	            {
	                if (ls.m_pQueuedSockets.Count > 0)
	                {
	                    HashSet<UDTSOCKET>.Enumerator e = ls.m_pQueuedSockets.GetEnumerator();
	                    e.MoveNext();
	                    u = e.Current;
	                    ls.m_pAcceptSockets.Add(u);
	                    ls.m_pQueuedSockets.Remove(u);
	                    accepted = true;
	                }
	                else if (!ls.m_pUDT.m_bSynRecving)
	                {
	                    accepted = true;
	                }
	            }
	            if (!accepted & (UdtStatus.Listening == ls.m_Status))
	            {
	                ls.m_AcceptCond.WaitOne(Timeout.Infinite);
	            }
	            if ((UdtStatus.Listening != ls.m_Status) || ls.m_pUDT.m_bBroken)
	            {
	                // Send signal to other threads that are waiting to accept.
	                ls.m_AcceptCond.Set();
	                accepted = true;
	            }
	            //if (ls.m_pQueuedSockets.Count == 0)
	            //    m_EPoll.update_events(listen, ls.m_pUDT.m_sPollID, UDT_EPOLL_IN, false);
	        }
	        if (u == UdtCongestionControl.INVALID_SOCK)
	        {
	            // non-blocking receiving, no connection available
	            if (!ls.m_pUDT.m_bSynRecving)
	            {
	                throw new UdtException(6, 2, 0);
	            }
	            // listening socket is closed
	            throw new UdtException(5, 6, 0);
	        }
	        addr = locate(u).m_pPeerAddr;
	        return u;
	    }
	    public int connect(UDTSOCKET u, IPEndPoint name)
	    {
	        UdtSocketInternal s = locate(u);
	        if (null == s)
	        {
	            throw new UdtException(5, 4, 0);
	        }
	        lock (s.m_ControlLock)
	        {
	            // a socket can "connect" only if it is in INIT or UDTSTATUS.OPENED status
	            if (UdtStatus.Initializing == s.m_Status)
	            {
	                if (!s.m_pUDT.m_bRendezvous)
	                {
	                    s.m_pUDT.open();
	                    updateMux(s);
	                    s.m_Status = UdtStatus.Opened;
	                }
	                else
	                {
	                    throw new UdtException(5, 8, 0);
	                }
	            }
	            // connect_complete() may be called before connect() returns.
	            // So we need to update the status before connect() is called,
	            // otherwise the status may be overwritten with wrong value (CONNECTED vs. CONNECTING).
	            s.m_Status = UdtStatus.Connecting;
	            try
	            {
	                s.m_pUDT.connect(name);
	            }
	            catch (UdtException e)
	            {
	                s.m_Status = UdtStatus.Opened;
	                throw e;
	            }
	            // record peer address
	            s.m_pPeerAddr = name;
	            return 0;
	        }
	    }
	    public void connect_complete(UDTSOCKET u)
	    {
	        UdtSocketInternal s = locate(u);
	        if (null == s)
	        {
	            throw new UdtException(5, 4, 0);
	        }
	        // copy address information of local node
	        // the local port must be correctly assigned BEFORE CUDT.connect(),
	        // otherwise if connect() fails, the multiplexer cannot be located by garbage collection and will cause leak
	        s.m_pUDT.m_pSndQueue.m_pChannel.getSockAddr(ref s.m_pSelfAddr);
	        ConvertIPAddress.ToUintArray(s.m_pSelfAddr.Address, ref s.m_pUDT.m_piSelfIP);
	        s.m_Status = UdtStatus.Connected;
	    }
	    public int close(UDTSOCKET u)
	    {
	        UdtSocketInternal s = locate(u);
	        if (null == s)
	        {
	            throw new UdtException(5, 4, 0);
	        }
	        lock (s.m_ControlLock)
	        {
	            if (s.m_Status == UdtStatus.Listening)
	            {
	                if (s.m_pUDT.m_bBroken)
	                {
	                    return 0;
	                }
	                s.m_TimeStamp = UdtTimer.getTime();
	                s.m_pUDT.m_bBroken = true;
	                // broadcast all "accept" waiting
	                s.m_AcceptCond.Set();
	                return 0;
	            }
	            s.m_pUDT.close();
	            // synchronize with garbage collection.
	            lock (m_ControlLock)
	            {
	                // since "s" is located before m_ControlLock, locate it again in case it became invalid
	                if (!m_Sockets.TryGetValue(u, out s) || s.m_Status == UdtStatus.Closed)
	                {
	                    return 0;
	                }
	                s.m_Status = UdtStatus.Closed;
	                // a socket will not be immediated removed when it is closed
	                // in order to prevent other methods from accessing invalid address
	                // a timer is started and the socket will be removed after approximately 1 second
	                s.m_TimeStamp = UdtTimer.getTime();
	                m_Sockets.Remove(s.m_SocketID);
	                m_ClosedSockets.Add(s.m_SocketID, s);
	                UdtTimer.triggerEvent();
	                return 0;
	            }
	        }
	    }
	    //            int CUDTUnited.getpeername(const UDTSOCKET u, sockaddr*name, int* namelen)
	    //{
	    //                if (CONNECTED != getStatus(u))
	    //                    throw new UdtException(2, 2, 0);
	    //                UdtSocket* s = locate(u);
	    //                if (null == s)
	    //                    throw new UdtException(5, 4, 0);
	    //                if (!s.m_pUDT.m_bConnected || s.m_pUDT.m_bBroken)
	    //                    throw new UdtException(2, 2, 0);
	    //                if (AF_INET == s.m_iIPversion)
	    //                    *namelen = sizeof(sockaddr_in);
	    //                else
	    //                    *namelen = sizeof(sockaddr_in6);
	    //                // copy address information of peer node
	    //                memcpy(name, s.m_pPeerAddr, *namelen);
	    //                return 0;
	    //            }
	    //            int CUDTUnited.getsockname(const UDTSOCKET u, sockaddr*name, int* namelen)
	    //{
	    //                UdtSocket* s = locate(u);
	    //                if (null == s)
	    //                    throw new UdtException(5, 4, 0);
	    //                if (s.m_pUDT.m_bBroken)
	    //                    throw new UdtException(5, 4, 0);
	    //                if (INIT == s.m_Status)
	    //                    throw new UdtException(2, 2, 0);
	    //                if (AF_INET == s.m_iIPversion)
	    //                    *namelen = sizeof(sockaddr_in);
	    //                else
	    //                    *namelen = sizeof(sockaddr_in6);
	    //                // copy address information of local node
	    //                memcpy(name, s.m_pSelfAddr, *namelen);
	    //                return 0;
	    //            }
	    internal UdtSocketInternal locate(UDTSOCKET u)
	    {
	        lock (m_ControlLock)
	        {
	            UdtSocketInternal s;
	            if (!m_Sockets.TryGetValue(u, out s) || s.m_Status == UdtStatus.Closed)
	            {
	                return null;
	            }
	            return s;
	        }
	    }
	    UdtSocketInternal locate(IPEndPoint peer, UDTSOCKET id, int isn)
	    {
	        lock (m_ControlLock)
	        {
	            HashSet<int> sockets;
	            if (!m_PeerRec.TryGetValue((id << 30) + isn, out sockets))
	            {
	                return null;
	            }
	            foreach (int iSocket in sockets)
	            {
	                UdtSocketInternal socket;
	                if (!m_Sockets.TryGetValue(iSocket, out socket))
	                {
	                    continue;
	                }
	                if (socket.m_pPeerAddr.Equals(peer))
	                {
	                    return socket;
	                }
	            }
	            return null;
	        }
	    }
	    public void checkBrokenSockets()
	    {
	        lock (m_ControlLock)
	        {
	            checkBrokenSockets_unsafe();
	        }
	    }
	    void checkBrokenSockets_unsafe()
	    {
	        // set of sockets To Be Closed and To Be Removed
	        List<UDTSOCKET> tbc = new List<UDTSOCKET>();
	        List<UDTSOCKET> tbr = new List<UDTSOCKET>();
	        foreach (KeyValuePair<UDTSOCKET, UdtSocketInternal> item in m_Sockets)
	        {
	            // check broken connection
	            if (item.Value.m_pUDT.m_bBroken)
	            {
	                if (item.Value.m_Status == UdtStatus.Listening)
	                {
	                    // for a listening socket, it should wait an extra 3 seconds in case a client is connecting
	                    if (UdtTimer.getTime() - item.Value.m_TimeStamp < 3000000)
	                    {
	                        continue;
	                    }
	                }
	                else if ((item.Value.m_pUDT.m_pRcvBuffer != null) && (item.Value.m_pUDT.m_pRcvBuffer.getRcvDataSize() > 0) && (item.Value.m_pUDT.m_iBrokenCounter-- > 0))
	                {
	                    // if there is still data in the receiver buffer, wait longer
	                    continue;
	                }
	                //close broken connections and start removal timer
	                item.Value.m_Status = UdtStatus.Closed;
	                item.Value.m_TimeStamp = UdtTimer.getTime();
	                tbc.Add(item.Key);
	                m_ClosedSockets[item.Key] = item.Value;
	                // remove from listener's queue
	                UdtSocketInternal listenSocket;
	                if (!m_Sockets.TryGetValue(item.Value.m_ListenSocket, out listenSocket))
	                {
	                    if (!m_ClosedSockets.TryGetValue(item.Value.m_ListenSocket, out listenSocket))
	                    {
	                        continue;
	                    }
	                }
	                Monitor.Enter(listenSocket.m_AcceptLock);
	                listenSocket.m_pQueuedSockets.Remove(item.Value.m_SocketID);
	                listenSocket.m_pAcceptSockets.Remove(item.Value.m_SocketID);
	                Monitor.Exit(listenSocket.m_AcceptLock);
	            }
	        }
	        foreach (KeyValuePair<UDTSOCKET, UdtSocketInternal> j in m_ClosedSockets)
	        {
	            if (j.Value.m_pUDT.m_ullLingerExpiration > 0)
	            {
	                // asynchronous close: 
	                if ((null == j.Value.m_pUDT.m_pSndBuffer) || (0 == j.Value.m_pUDT.m_pSndBuffer.getCurrBufSize()) || (j.Value.m_pUDT.m_ullLingerExpiration <= UdtTimer.getTime()))
	                {
	                    j.Value.m_pUDT.m_ullLingerExpiration = 0;
	                    j.Value.m_pUDT.m_bClosing = true;
	                    j.Value.m_TimeStamp = UdtTimer.getTime();
	                }
	            }
	            // timeout 1 second to destroy a socket AND it has been removed from RcvUList
	            if ((UdtTimer.getTime() - j.Value.m_TimeStamp  > 1000000) && ((null == j.Value.m_pUDT.m_pRNode) || !j.Value.m_pUDT.m_pRNode.m_bOnList))
	            {
	                tbr.Add(j.Key);
	            }
	        }
	        // move closed sockets to the ClosedSockets structure
	        foreach (UDTSOCKET k in tbc)
	        {
	            m_Sockets.Remove(k);
	        }
	        // remove those timeout sockets
	        foreach (UDTSOCKET l in tbr)
	        {
	            removeSocket(l);
	        }
	    }
	    void removeSocket(UDTSOCKET u)
	    {
	        UdtSocketInternal closedSocket;
	        if (!m_ClosedSockets.TryGetValue(u, out closedSocket))
	        {
	            return;
	        }
	        // decrease multiplexer reference count, and remove it if necessary
	        int mid = closedSocket.m_iMuxID;
	        if (null != closedSocket.m_pQueuedSockets)
	        {
	            Monitor.Enter(closedSocket.m_AcceptLock);
	            // if it is a listener, close all un-accepted sockets in its queue and remove them later
	            foreach (UDTSOCKET q in closedSocket.m_pQueuedSockets)
	            {
	                m_Sockets[q].m_pUDT.m_bBroken = true;
	                m_Sockets[q].m_pUDT.close();
	                m_Sockets[q].m_TimeStamp = UdtTimer.getTime();
	                m_Sockets[q].m_Status = UdtStatus.Closed;
	                m_ClosedSockets[q] = m_Sockets[q];
	                m_Sockets.Remove(q);
	            }
	           Monitor.Exit(closedSocket.m_AcceptLock);
	        }
	        // remove from peer rec
	        HashSet<int> sockets;
	        if (m_PeerRec.TryGetValue((closedSocket.m_PeerID << 30) + closedSocket.m_iISN, out sockets))
	        {
	            sockets.Remove(u);
	            if (sockets.Count == 0)
	            {
	                m_PeerRec.Remove(closedSocket.m_PeerID);
	            }
	        }
	        // delete this one
	        closedSocket.m_pUDT.close();
	        closedSocket.Close();
	        m_ClosedSockets.Remove(u);
	        UdtMultiplexer m;
	        if (!m_mMultiplexer.TryGetValue(mid, out m))
	        {
	            //something is wrong!!!
	            return;
	        }
	        m.m_iRefCount--;
	        if (0 == m.m_iRefCount)
	        {
	            m.m_pChannel.Close();
	            m.m_pSndQueue.Close();
	            m.m_pRcvQueue.Close();
	            m.m_pTimer.Stop();
	            m_mMultiplexer.Remove(mid);
	        }
	    }
	    //            void setError(UdtException e)
	    //{
	    //                CGuard tg(m_TLSLock);
	    //                delete(UdtException *)TlsGetValue(m_TLSError);
	    //                TlsSetValue(m_TLSError, e);
	    //                m_mTLSRecord[GetCurrentThreadId()] = e;
	    //            }
	    //            UdtException getError()
	    //{
	    //                CGuard tg(m_TLSLock);
	    //                if (null == TlsGetValue(m_TLSError))
	    //                {
	    //                    UdtException* e = new UdtException;
	    //                    TlsSetValue(m_TLSError, e);
	    //                    m_mTLSRecord[GetCurrentThreadId()] = e;
	    //                }
	    //                return (UdtException*)TlsGetValue(m_TLSError);
	    //            }
	    //            void checkTLSValue()
	    //{
	    //                CGuard tg(m_TLSLock);
	    //                vector<DWORD> tbr;
	    //                for (map<DWORD, UdtException*>.iterator i = m_mTLSRecord.begin(); i != m_mTLSRecord.end(); ++i)
	    //                {
	    //                    HANDLE h = OpenThread(THREAD_QUERY_INFORMATION, FALSE, i.first);
	    //                    if (null == h)
	    //                    {
	    //                        tbr.push_back(i.first);
	    //                        break;
	    //                    }
	    //                    if (WAIT_OBJECT_0 == WaitForSingleObject(h, 0))
	    //                    {
	    //                        delete i.second;
	    //                        tbr.push_back(i.first);
	    //                    }
	    //                    CloseHandle(h);
	    //                }
	    //                for (vector<DWORD>.iterator j = tbr.begin(); j != tbr.end(); ++j)
	    //                    m_mTLSRecord.erase(*j);
	    //            }
	    void updateMux(UdtSocketInternal s, IPEndPoint addr = null, Socket udpsock = null)
	    {
	        lock (m_ControlLock)
	        {
	            UdtMultiplexer m;
	            if ((s.m_pUDT.m_bReuseAddr) && (null != addr))
	            {
	                int port = addr.Port;
	                // find a reusable address
	                foreach (KeyValuePair<int, UdtMultiplexer> item in m_mMultiplexer)
	                {
	                    // reuse the existing multiplexer
	                    m = item.Value;
	                    if ((m.m_iIPversion == s.m_pUDT.m_iIPversion) && (m.m_iMSS == s.m_pUDT.m_iMSS) && m.m_bReusable)
	                    {
	                        if (m.m_iPort == port)
	                        {
	                            // reuse the existing multiplexer
	                            ++m.m_iRefCount;
	                            s.m_pUDT.m_pSndQueue = m.m_pSndQueue;
	                            s.m_pUDT.m_pRcvQueue = m.m_pRcvQueue;
	                            s.m_iMuxID = m.m_iID;
	                            return;
	                        }
	                    }
	                }
	            }
	            // a new multiplexer is needed
	            m = new UdtMultiplexer();
	            m.m_iMSS = s.m_pUDT.m_iMSS;
	            m.m_iIPversion = s.m_pUDT.m_iIPversion;
	            m.m_iRefCount = 1;
	            m.m_bReusable = s.m_pUDT.m_bReuseAddr;
	            m.m_iID = s.m_SocketID;
	            m.m_pChannel = new UdtChannel(s.m_pUDT.m_iIPversion);
	            m.m_pChannel.setSndBufSize(s.m_pUDT.m_iUDPSndBufSize);
	            m.m_pChannel.setRcvBufSize(s.m_pUDT.m_iUDPRcvBufSize);
	            try
	            {
	                if (null != udpsock)
	                {
	                    m.m_pChannel.Open(udpsock);
	                }
	                else
	                {
	                    m.m_pChannel.Open(addr);
	                }
	            }
	            catch (UdtException e)
	            {
	                m.m_pChannel.Close();
	                throw e;
	            }
	            IPEndPoint sa = new IPEndPoint(IPAddress.Any, 0);
	            m.m_pChannel.getSockAddr(ref sa);
	            m.m_iPort = sa.Port;
	            m.m_pTimer = new UdtTimer();
	            m.m_pSndQueue = new SndQueue();
	            m.m_pSndQueue.init(m.m_pChannel, m.m_pTimer);
	            m.m_pRcvQueue = new RcvQueue();
	            m.m_pRcvQueue.init(32, s.m_pUDT.m_iPayloadSize, m.m_iIPversion, 1024, m.m_pChannel, m.m_pTimer);
	            m_mMultiplexer[m.m_iID] = m;
	            s.m_pUDT.m_pSndQueue = m.m_pSndQueue;
	            s.m_pUDT.m_pRcvQueue = m.m_pRcvQueue;
	            s.m_iMuxID = m.m_iID;
	        }
	    }
	    void updateMux(UdtSocketInternal s, UdtSocketInternal ls)
	    {
	        lock (m_ControlLock)
	        {
	            int port = ls.m_pSelfAddr.Port;
	            // find the listener's address
	            foreach (KeyValuePair<int, UdtMultiplexer> item in m_mMultiplexer)
	            {
	                if (item.Value.m_iPort == port)
	                {
	                    // reuse the existing multiplexer
	                    UdtMultiplexer multiplexer = item.Value;
	                    ++multiplexer.m_iRefCount;
	                    s.m_pUDT.m_pSndQueue = multiplexer.m_pSndQueue;
	                    s.m_pUDT.m_pRcvQueue = multiplexer.m_pRcvQueue;
	                    s.m_iMuxID = multiplexer.m_iID;
	                    return;
	                }
	            }
	        }
	    }
	}
	#endregion
	#region \Internal\Exceptions
	internal class UdtException : Exception
	{
	    int m_iMajor;
	    int m_iMinor;
	    int m_iErrno;
	    public UdtException(int major = 0, int minor = 0, int err = -1)
	        : base(getErrorMessage(major, minor))
	    {
	        m_iMajor = major;
	        m_iMinor = minor;
	        if (-1 == err)
	        {
	            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
	            {
	                m_iErrno = Marshal.GetLastWin32Error();
	            }
	            // TODO handle non-windows error
	        }
	        else
	        {
	            m_iErrno = err;
	        }
	    }
	    private static string getErrorMessage(int major, int minor)
	    {
	        // translate "Major:Minor" code into text message.
	        string strMsg = string.Empty;
	        switch (major)
	        {
	            case 0:
	                strMsg = "Success";
	                break;
	            case 1:
	                strMsg = "Connection setup failure";
	                switch (minor)
	                {
	                    case 1:
	                        strMsg += ": connection time out";
	                        break;
	                    case 2:
	                        strMsg += ": connection rejected";
	                        break;
	                    case 3:
	                        strMsg += ": unable to create/configure UDP socket";
	                        break;
	                    case 4:
	                        strMsg += ": abort for security reasons";
	                        break;
	                    default:
	                        break;
	                }
	                break;
	            case 2:
	                switch (minor)
	                {
	                    case 1:
	                        strMsg = "Connection was broken";
	                        break;
	                    case 2:
	                        strMsg = "Connection does not exist";
	                        break;
	                    default:
	                        break;
	                }
	                break;
	            case 3:
	                strMsg = "System resource failure";
	                switch (minor)
	                {
	                    case 1:
	                        strMsg += ": unable to create new threads";
	                        break;
	                    case 2:
	                        strMsg += ": unable to allocate buffers";
	                        break;
	                    default:
	                        break;
	                }
	                break;
	            case 4:
	                strMsg = "File system failure";
	                switch (minor)
	                {
	                    case 1:
	                        strMsg += ": cannot seek read position";
	                        break;
	                    case 2:
	                        strMsg += ": failure in read";
	                        break;
	                    case 3:
	                        strMsg += ": cannot seek write position";
	                        break;
	                    case 4:
	                        strMsg += ": failure in write";
	                        break;
	                    default:
	                        break;
	                }
	                break;
	            case 5:
	                strMsg = "Operation not supported";
	                switch (minor)
	                {
	                    case 1:
	                        strMsg += ": Cannot do this operation on a BOUND socket";
	                        break;
	                    case 2:
	                        strMsg += ": Cannot do this operation on a CONNECTED socket";
	                        break;
	                    case 3:
	                        strMsg += ": Bad parameters";
	                        break;
	                    case 4:
	                        strMsg += ": Invalid socket ID";
	                        break;
	                    case 5:
	                        strMsg += ": Cannot do this operation on an UNBOUND socket";
	                        break;
	                    case 6:
	                        strMsg += ": Socket is not in listening state";
	                        break;
	                    case 7:
	                        strMsg += ": Listen/accept is not supported in rendezous connection setup";
	                        break;
	                    case 8:
	                        strMsg += ": Cannot call connect on UNBOUND socket in rendezvous connection setup";
	                        break;
	                    case 9:
	                        strMsg += ": This operation is not supported in SOCK_STREAM mode";
	                        break;
	                    case 10:
	                        strMsg += ": This operation is not supported in SOCK_DGRAM mode";
	                        break;
	                    case 11:
	                        strMsg += ": Another socket is already listening on the same port";
	                        break;
	                    case 12:
	                        strMsg += ": Message is too large to send (it must be less than the UDT send buffer size)";
	                        break;
	                    case 13:
	                        strMsg += ": Invalid epoll ID";
	                        break;
	                    default:
	                        break;
	                }
	                break;
	            case 6:
	                strMsg = "Non-blocking call failure";
	                switch (minor)
	                {
	                    case 1:
	                        strMsg += ": no buffer available for sending";
	                        break;
	                    case 2:
	                        strMsg += ": no data available for reading";
	                        break;
	                    default:
	                        break;
	                }
	                break;
	            case 7:
	                strMsg = "The peer side has signalled an error";
	                break;
	            default:
	                strMsg = "Unknown error";
	                break;
	        }
	        //        // Adding "errno" information
	        //        if ((0 != m_iMajor) && (0 < m_iErrno))
	        //        {
	        //            strMsg += ": ";
	        //# ifndef WIN32
	        //            char errmsg[1024];
	        //            if (strerror_r(m_iErrno, errmsg, 1024) == 0)
	        //                strMsg += errmsg;
	        //#else
	        //            LPVOID lpMsgBuf;
	        //            FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS, NULL, m_iErrno, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), (LPTSTR) & lpMsgBuf, 0, NULL);
	        //            strMsg += (char*)lpMsgBuf;
	        //            LocalFree(lpMsgBuf);
	        //#endif
	        //        }
	        return strMsg;
	    }
	    public int getErrorCode()
	    {
	        return m_iMajor * 1000 + m_iMinor;
	    }
	    void clear()
	    {
	        m_iMajor = 0;
	        m_iMinor = 0;
	        m_iErrno = 0;
	    }
	    const int SUCCESS = 0;
	    const int ECONNSETUP = 1000;
	    const int ENOSERVER = 1001;
	    const int ECONNREJ = 1002;
	    const int ESOCKFAIL = 1003;
	    const int ESECFAIL = 1004;
	    const int ECONNFAIL = 2000;
	    const int ECONNLOST = 2001;
	    const int ENOCONN = 2002;
	    const int ERESOURCE = 3000;
	    const int ETHREAD = 3001;
	    const int ENOBUF = 3002;
	    const int EFILE = 4000;
	    const int EINVRDOFF = 4001;
	    const int ERDPERM = 4002;
	    const int EINVWROFF = 4003;
	    const int EWRPERM = 4004;
	    const int EINVOP = 5000;
	    const int EBOUNDSOCK = 5001;
	    const int ECONNSOCK = 5002;
	    const int EINVPARAM = 5003;
	    const int EINVSOCK = 5004;
	    const int EUNBOUNDSOCK = 5005;
	    const int ENOLISTEN = 5006;
	    const int ERDVNOSERV = 5007;
	    const int ERDVUNBOUND = 5008;
	    const int ESTREAMILL = 5009;
	    const int EDGRAMILL = 5010;
	    const int EDUPLISTEN = 5011;
	    const int ELARGEMSG = 5012;
	    const int EINVPOLLID = 5013;
	    const int EASYNCFAIL = 6000;
	    const int EASYNCSND = 6001;
	    const int EASYNCRCV = 6002;
	    const int ETIMEOUT = 6003;
	    const int EPEERERR = 7000;
	    const int EUNKNOWN = -1;
	}
	#endregion
	#region \Internal\Flags
	internal enum UdtEpollOptions
	{
	    // this values are defined same as linux epoll.h
	    // so that if system values are used by mistake, they should have the same effect
	    UDT_EPOLL_IN = 0x1,
	    UDT_EPOLL_OUT = 0x4,
	    UDT_EPOLL_ERR = 0x8
	};
	internal enum UdtOptions
	{
	    UDT_MSS,             // the Maximum Transfer Unit
	    UDT_SNDSYN,          // if sending is blocking
	    UDT_RCVSYN,          // if receiving is blocking
	    UDT_CC,              // custom congestion control algorithm
	    UDT_FC,              // Flight flag size (window size)
	    UDT_SNDBUF,          // maximum buffer in sending queue
	    UDT_RCVBUF,          // UDT receiving buffer size
	    UDT_LINGER,          // waiting for unsent data when closing
	    UDP_SNDBUF,          // UDP sending buffer size
	    UDP_RCVBUF,          // UDP receiving buffer size
	    UDT_MAXMSG,          // maximum datagram message size
	    UDT_MSGTTL,          // time-to-live of a datagram message
	    UDT_RENDEZVOUS,      // rendezvous connection mode
	    UDT_SNDTIMEO,        // send() timeout
	    UDT_RCVTIMEO,        // recv() timeout
	    UDT_REUSEADDR,       // reuse an existing port or create a new one
	    UDT_MAXBW,           // maximum bandwidth (bytes per second) that the connection can use
	    UDT_STATE,           // current socket state, see UDTSTATUS, read only
	    UDT_EVENT,           // current avalable events associated with the socket
	    UDT_SNDDATA,         // size of data in the sending buffer
	    UDT_RCVDATA          // size of data available for recv
	};
	internal enum UdtStatus
	{
	    Initializing = 1,
	    Opened,
	    Listening,
	    Connecting,
	    Connected,
	    Broken,
	    Closing,
	    Closed,
	    NonExist
	}
	#endregion
	#region \Internal\Queue
	internal class RcvQueue
	{
	    RcvUList m_pRcvUList = new RcvUList();     // List of UDT instances that will read packets from the queue
	    UdtChannel m_pChannel;       // UDP channel for receving packets
	    UdtTimer m_pTimer;           // shared timer with the snd queue
	    int m_iPayloadSize;                  // packet payload size
	    volatile bool m_bClosing;            // closing the workder
	    EventWaitHandle m_ExitCond;
	    object m_LSLock;
	    UdtCongestionControl m_pListener;                                   // pointer to the (unique, if any) listening UDT entity
	    RendezvousQueue m_pRendezvousQueue = new RendezvousQueue();                // The list of sockets in rendezvous mode
	    List<UdtCongestionControl> m_vNewEntry = new List<UdtCongestionControl>();                      // newly added entries, to be inserted
	    object m_IDLock;
	    Dictionary<int, Queue<UdtPacket>> m_mBuffer = new Dictionary<int, Queue<UdtPacket>>();  // temporary buffer for rendezvous connection request
	    object m_PassLock;
	    EventWaitHandle m_PassCond;
	    Thread m_WorkerThread;
	    Dictionary<int, UdtCongestionControl> m_hash = new Dictionary<int, UdtCongestionControl>();
	    public RcvQueue()
	    {
	        m_PassLock = new object();
	        m_PassCond = new EventWaitHandle(false, EventResetMode.AutoReset);
	        m_LSLock = new object();
	        m_IDLock = new object();
	        m_ExitCond = new EventWaitHandle(false, EventResetMode.AutoReset);
	    }
	    public void Close()
	    {
	        m_bClosing = true;
	        if (null != m_WorkerThread)
	            m_ExitCond.WaitOne(Timeout.Infinite);
	        m_PassCond.Close();
	        m_ExitCond.Close();
	    }
	    public void init(int qsize, int payload, AddressFamily version, int hsize, UdtChannel cc, UdtTimer t)
	    {
	        m_iPayloadSize = payload;
	        m_pChannel = cc;
	        m_pTimer = t;
	        m_WorkerThread = new Thread(worker);
	        m_WorkerThread.IsBackground = true;
	        m_WorkerThread.Start(this);
	    }
	    static void worker(object param)
	    {
	        RcvQueue self = param as RcvQueue;
	        if (self == null)
	            return;
	        IPEndPoint addr = new IPEndPoint(IPAddress.Any, 0);
	        UdtCongestionControl u = null;
	        int id;
	        while (!self.m_bClosing)
	        {
	            self.m_pTimer.tick();
	            // check waiting list, if new socket, insert it to the list
	            while (self.ifNewEntry())
	            {
	                UdtCongestionControl ne = self.getNewEntry();
	                if (null != ne)
	                {
	                    self.m_pRcvUList.insert(ne);
	                    self.m_hash.Add(ne.m_SocketID, ne);
	                }
	            }
	            // find next available slot for incoming packet
	            UdtUnit unit = new UdtUnit();
	            unit.m_Packet.setLength(self.m_iPayloadSize);
	            // reading next incoming packet, recvfrom returns -1 is nothing has been received
	            if (self.m_pChannel.recvfrom(ref addr, unit.m_Packet) < 0)
	                goto TIMER_CHECK;
	            id = unit.m_Packet.GetId();
	            // ID 0 is for connection request, which should be passed to the listening socket or rendezvous sockets
	            if (0 == id)
	            {
	                if (null != self.m_pListener)
	                    self.m_pListener.listen(addr, unit.m_Packet);
	                else if (null != (u = self.m_pRendezvousQueue.retrieve(addr, ref id)))
	                {
	                    // asynchronous connect: call connect here
	                    // otherwise wait for the UDT socket to retrieve this packet
	                    if (!u.m_bSynRecving)
	                        u.connect(unit.m_Packet);
	                    else
	                    {
	                        UdtPacket newPacket = new UdtPacket();
	                        newPacket.Clone(unit.m_Packet);
	                        self.storePkt(id, newPacket);
	                    }
	                }
	            }
	            else if (id > 0)
	            {
	                if (self.m_hash.TryGetValue(id, out u))
	                {
	                    if (addr.Equals(u.m_pPeerAddr))
	                    {
	                        if (u.m_bConnected && !u.m_bBroken && !u.m_bClosing)
	                        {
	                            if (0 == unit.m_Packet.getFlag())
	                                u.processData(unit);
	                            else
	                                u.processCtrl(unit.m_Packet);
	                            u.checkTimers();
	                            self.m_pRcvUList.update(u);
	                        }
	                    }
	                }
	                else if (null != (u = self.m_pRendezvousQueue.retrieve(addr, ref id)))
	                {
	                    if (!u.m_bSynRecving)
	                        u.connect(unit.m_Packet);
	                    else
	                    {
	                        UdtPacket newPacket = new UdtPacket();
	                        newPacket.Clone(unit.m_Packet);
	                        self.storePkt(id, newPacket);
	                    }
	                }
	            }
	        TIMER_CHECK:
	            // take care of the timing event for all UDT sockets
	            ulong currtime = UdtTimer.rdtsc();
	            ulong ctime = currtime - 100000 * UdtTimer.getCPUFrequency();
	            for (int i = 0; i < self.m_pRcvUList.m_nodeList.Count; ++i)
	            {
	                RNode ul = self.m_pRcvUList.m_nodeList[0];
	                if (ul.m_llTimeStamp >= ctime)
	                    break;
	                u = ul.m_pUDT;
	                if (u.m_bConnected && !u.m_bBroken && !u.m_bClosing)
	                {
	                    u.checkTimers();
	                    self.m_pRcvUList.update(u);
	                }
	                else
	                {
	                    // the socket must be removed from Hash table first, then RcvUList
	                    self.m_hash.Remove(u.m_SocketID);
	                    self.m_pRcvUList.remove(u);
	                    u.m_pRNode.m_bOnList = false;
	                }
	            }
	            // Check connection requests status for all sockets in the RendezvousQueue.
	            self.m_pRendezvousQueue.updateConnStatus();
	        }
	        self.m_ExitCond.Set();
	    }
	    public int recvfrom(int id, UdtPacket packet)
	    {
	        bool gotLock = false;
	        Monitor.Enter(m_PassLock, ref gotLock);
	        Queue<UdtPacket> packetQueue;
	        if (!m_mBuffer.TryGetValue(id, out packetQueue))
	        {
	            if (gotLock)
	                Monitor.Exit(m_PassLock);
	            m_PassCond.WaitOne(1000);
	            lock (m_PassLock)
	            {
	                if (!m_mBuffer.TryGetValue(id, out packetQueue))
	                {
	                    packet.setLength(-1);
	                    return -1;
	                }
	            }
	        }
	        if (gotLock && Monitor.IsEntered(m_PassLock))
	            Monitor.Exit(m_PassLock);
	        // retrieve the earliest packet
	        UdtPacket newpkt = packetQueue.Peek();
	        if (packet.getLength() < newpkt.getLength())
	        {
	            packet.setLength(-1);
	            return -1;
	        }
	        // copy packet content
	        packet.Clone(newpkt);
	        packetQueue.Dequeue();
	        if (packetQueue.Count == 0)
	        {
	            lock (m_PassLock)
	            {
	                m_mBuffer.Remove(id);
	            }
	        }
	        return packet.getLength();
	    }
	    public int setListener(UdtCongestionControl u)
	    {
	        lock (m_LSLock)
	        {
	            if (null != m_pListener)
	                return -1;
	            m_pListener = u;
	            return 0;
	        }
	    }
	    public void removeListener(UdtCongestionControl u)
	    {
	        lock (m_LSLock)
	        {
	            if (u == m_pListener)
	                m_pListener = null;
	        }
	    }
	    public void registerConnector(int id, UdtCongestionControl u, AddressFamily ipv, IPEndPoint addr, ulong ttl)
	    {
	        m_pRendezvousQueue.insert(id, u, ipv, addr, ttl);
	    }
	    public void removeConnector(int id)
	    {
	        m_pRendezvousQueue.remove(id);
	        lock (m_PassLock)
	        {
	            m_mBuffer.Remove(id);
	        }
	    }
	    public void setNewEntry(UdtCongestionControl u)
	    {
	        lock (m_IDLock)
	        {
	            m_vNewEntry.Add(u);
	        }
	    }
	    bool ifNewEntry()
	    {
	        return !(m_vNewEntry.Count == 0);
	    }
	    UdtCongestionControl getNewEntry()
	    {
	        lock (m_IDLock)
	        {
	            if (m_vNewEntry.Count == 0)
	                return null;
	            UdtCongestionControl u = m_vNewEntry[0];
	            m_vNewEntry.RemoveAt(0);
	            return u;
	        }
	    }
	    void storePkt(int id, UdtPacket pkt)
	    {
	        lock (m_PassLock)
	        {
	            Queue<UdtPacket> packetQueue;
	            if (!m_mBuffer.TryGetValue(id, out packetQueue))
	            {
	                packetQueue = new Queue<UdtPacket>();
	                packetQueue.Enqueue(pkt);
	                m_mBuffer.Add(id, packetQueue);
	                m_PassCond.Set();
	            }
	            else
	            {
	                //avoid storing too many packets, in case of malfunction or attack
	                if (packetQueue.Count > 16)
	                    return;
	                packetQueue.Enqueue(pkt);
	            }
	        }
	    }
	}
	internal class RcvUList
	{
	    public List<RNode> m_nodeList = new List<RNode>();
	    public void insert(UdtCongestionControl u)
	    {
	        RNode n = u.m_pRNode;
	        n.m_llTimeStamp = UdtTimer.rdtsc();
	        // always insert at the end for RcvUList
	        m_nodeList.Add(n);
	    }
	    public void remove(UdtCongestionControl u)
	    {
	        RNode n = u.m_pRNode;
	        if (!n.m_bOnList)
	            return;
	        m_nodeList.Remove(n);
	    }
	    public void update(UdtCongestionControl u)
	    {
	        RNode n = u.m_pRNode;
	        if (!n.m_bOnList)
	            return;
	        RNode match = m_nodeList.Find(x => x.Equals(n));
	        if (match.Equals(default(RNode)))
	            return;
	        match.m_llTimeStamp = UdtTimer.rdtsc();
	    }
	}
	internal class RendezvousQueue
	{
	    struct CRL
	    {
	        internal int m_iID;            // UDT socket ID (self)
	        internal UdtCongestionControl m_pUDT;           // UDT instance
	        internal AddressFamily m_iIPversion;                 // IP version
	        internal IPEndPoint m_pPeerAddr;      // UDT sonnection peer address
	        internal ulong m_ullTTL;          // the time that this request expires
	    };
	    List<CRL> m_lRendezvousID = new List<CRL>();      // The sockets currently in rendezvous mode
	    object m_RIDVectorLock = new object();
	    public void insert(int id, UdtCongestionControl u, AddressFamily ipv, IPEndPoint addr, ulong ttl)
	    {
	        CRL r;
	        r.m_iID = id;
	        r.m_pUDT = u;
	        r.m_iIPversion = ipv;
	        r.m_pPeerAddr = addr;
	        r.m_ullTTL = ttl;
	        lock (m_RIDVectorLock)
	        {
	            m_lRendezvousID.Add(r);
	        }
	    }
	    public void remove(int id)
	    {
	        lock (m_RIDVectorLock)
	        {
	            for (int i = 0; i < m_lRendezvousID.Count; ++i)
	            {
	                if (m_lRendezvousID[i].m_iID == id)
	                {
	                    m_lRendezvousID.RemoveAt(i);
	                    return;
	                }
	            }
	        }
	    }
	    public UdtCongestionControl retrieve(IPEndPoint addr, ref int id)
	    {
	        lock (m_RIDVectorLock)
	        {
	            foreach (CRL crl in m_lRendezvousID)
	            {
	                if (crl.m_pPeerAddr.Equals(addr) && (id == 0) || (id == crl.m_iID))
	                {
	                    id = crl.m_iID;
	                    return crl.m_pUDT;
	                }
	            }
	            return null;
	        }
	    }
	    public void updateConnStatus()
	    {
	        if (m_lRendezvousID.Count == 0)
	            return;
	        lock (m_RIDVectorLock)
	        {
	            foreach (CRL crl in m_lRendezvousID)
	            {
	                // avoid sending too many requests, at most 1 request per 250ms
	                if (UdtTimer.getTime() - (ulong)crl.m_pUDT.m_llLastReqTime > 250000)
	                {
	                    //if (Timer.getTime() >= crl.m_ullTTL)
	                    //{
	                    //    // connection timer expired, acknowledge app via epoll
	                    //    i->m_pUDT->m_bConnecting = false;
	                    //    CUDT::s_UDTUnited.m_EPoll.update_events(i->m_iID, i->m_pUDT->m_sPollID, UDT_EPOLL_ERR, true);
	                    //    continue;
	                    //}
	                    UdtPacket request = new UdtPacket();
	                    request.pack(crl.m_pUDT.m_ConnReq);
	                    // ID = 0, connection request
	                    request.SetId(!crl.m_pUDT.m_bRendezvous ? 0 : crl.m_pUDT.m_ConnRes.SocketId);
	                    crl.m_pUDT.m_pSndQueue.sendto(crl.m_pPeerAddr, request);
	                    crl.m_pUDT.m_llLastReqTime = (long)UdtTimer.getTime();
	                }
	            }
	        }
	    }
	}
	internal class RNode
	{
	    public UdtCongestionControl m_pUDT;                // Pointer to the instance of CUDT socket
	    public ulong m_llTimeStamp;      // Time Stamp
	    public bool m_bOnList;              // if the node is already on the list
	};
	internal class SndQueue
	{
	    public SndUList m_pSndUList;     // List of UDT instances for data sending
	    public UdtChannel m_pChannel;                // The UDP channel for data sending
	    UdtTimer m_pTimer;           // Timing facility
	    object m_WindowLock;
	    EventWaitHandle m_WindowCond;
	    volatile bool m_bClosing;       // closing the worker
	    EventWaitHandle m_ExitCond;
	    Thread m_WorkerThread;
	    public SndQueue()
	    {
	        m_WindowLock = new object();
	        m_WindowCond = new EventWaitHandle(false, EventResetMode.AutoReset);
	        m_ExitCond = new EventWaitHandle(false, EventResetMode.AutoReset);
	    }
	    public void Close()
	    {
	        m_bClosing = true;
	        m_WindowCond.Set();
	        if (null != m_WorkerThread)
	            m_ExitCond.WaitOne(Timeout.Infinite);
	        m_WindowCond.Close();
	        m_ExitCond.Close();
	    }
	    public void init(UdtChannel c, UdtTimer t)
	    {
	        m_pChannel = c;
	        m_pTimer = t;
	        m_pSndUList = new SndUList();
	        m_pSndUList.m_pWindowLock = m_WindowLock;
	        m_pSndUList.m_pWindowCond = m_WindowCond;
	        m_pSndUList.m_pTimer = m_pTimer;
	        m_WorkerThread = new Thread(worker);
	        m_WorkerThread.IsBackground = true;
	        m_WorkerThread.Start(this);
	    }
	    static void worker(object param)
	    {
	        SndQueue self = param as SndQueue;
	        if (self == null)
	            return;
	        while (!self.m_bClosing)
	        {
	            ulong ts = self.m_pSndUList.getNextProcTime();
	            if (ts > 0)
	            {
	                // wait until next processing time of the first socket on the list
	                ulong currtime = UdtTimer.rdtsc();
	                if (currtime < ts)
	                    self.m_pTimer.sleepto(ts);
	                // it is time to send the next pkt
	                IPEndPoint addr = null;
	                UdtPacket pkt = new UdtPacket();
	                if (self.m_pSndUList.pop(ref addr, ref pkt) < 0)
	                    continue;
	                self.m_pChannel.sendto(addr, pkt);
	            }
	            else
	            {
	                // wait here if there is no sockets with data to be sent
	                self.m_WindowCond.WaitOne(Timeout.Infinite);
	            }
	        }
	        self.m_ExitCond.Set();
	    }
	    public int sendto(IPEndPoint addr, UdtPacket packet)
	    {
	        // send out the packet immediately (high priority), this is a control packet
	        m_pChannel.sendto(addr, packet);
	        return packet.getLength();
	    }
	}
	internal class SndUList
	{
	    object m_ListLock = new object();
	    public object m_pWindowLock;
	    public EventWaitHandle m_pWindowCond;
	    SNode[] m_pHeap;           // The heap array
	    int m_iArrayLength;         // physical length of the array
	    int m_iLastEntry;           // position of last entry on the heap array
	    public UdtTimer m_pTimer;
	    public SndUList()
	    {
	        m_iArrayLength = 4096;
	        m_iLastEntry = -1;
	        m_pHeap = new SNode[m_iArrayLength];
	    }
	    public void insert(ulong ts, UdtCongestionControl u)
	    {
	        lock (m_ListLock)
	        {
	            // increase the heap array size if necessary
	            if (m_iLastEntry == m_iArrayLength - 1)
	            {
	                Array.Resize(ref m_pHeap, m_iArrayLength * 2);
	                m_iArrayLength *= 2;
	            }
	            insert_(ts, u);
	        }
	    }
	    public void update(UdtCongestionControl u, bool reschedule = true)
	    {
	        lock (m_ListLock)
	        {
	            SNode n = u.m_pSNode;
	            if (n.m_iHeapLoc >= 0)
	            {
	                if (!reschedule)
	                    return;
	                if (n.m_iHeapLoc == 0)
	                {
	                    n.m_llTimeStamp = 1;
	                    m_pTimer.interrupt();
	                    return;
	                }
	                remove_(u);
	            }
	            insert_(1, u);
	        }
	    }
	    public int pop(ref IPEndPoint addr, ref UdtPacket pkt)
	    {
	        lock (m_ListLock)
	        {
	            if (-1 == m_iLastEntry)
	                return -1;
	            // no pop until the next schedulled time
	            ulong ts = UdtTimer.rdtsc();
	            if (ts < m_pHeap[0].m_llTimeStamp)
	                return -1;
	            UdtCongestionControl u = m_pHeap[0].m_pUDT;
	            remove_(u);
	            if (!u.m_bConnected || u.m_bBroken)
	                return -1;
	            // pack a packet from the socket
	            if (u.packData(pkt, ref ts) <= 0)
	                return -1;
	            addr = u.m_pPeerAddr;
	            // insert a new entry, ts is the next processing time
	            if (ts > 0)
	                insert_(ts, u);
	            return 1;
	        }
	    }
	    public void remove(UdtCongestionControl u)
	    {
	        lock (m_ListLock)
	        {
	            remove_(u);
	        }
	    }
	    public ulong getNextProcTime()
	    {
	        lock (m_ListLock)
	        {
	            if (-1 == m_iLastEntry)
	                return 0;
	            return m_pHeap[0].m_llTimeStamp;
	        }
	    }
	    void insert_(ulong ts, UdtCongestionControl u)
	    {
	        SNode n = u.m_pSNode;
	        // do not insert repeated node
	        if (n.m_iHeapLoc >= 0)
	            return;
	        m_iLastEntry++;
	        m_pHeap[m_iLastEntry] = n;
	        n.m_llTimeStamp = ts;
	        int q = m_iLastEntry;
	        int p = q;
	        while (p != 0)
	        {
	            p = (q - 1) >> 1;
	            if (m_pHeap[p].m_llTimeStamp > m_pHeap[q].m_llTimeStamp)
	            {
	                SNode t = m_pHeap[p];
	                m_pHeap[p] = m_pHeap[q];
	                m_pHeap[q] = t;
	                t.m_iHeapLoc = q;
	                q = p;
	            }
	            else
	                break;
	        }
	        n.m_iHeapLoc = q;
	        // an earlier event has been inserted, wake up sending worker
	        if (n.m_iHeapLoc == 0)
	            m_pTimer.interrupt();
	        // first entry, activate the sending queue
	        if (0 == m_iLastEntry)
	        {
	            m_pWindowCond.Set();
	        }
	    }
	    void remove_(UdtCongestionControl u)
	    {
	        SNode n = u.m_pSNode;
	        if (n.m_iHeapLoc >= 0)
	        {
	            // remove the node from heap
	            m_pHeap[n.m_iHeapLoc] = m_pHeap[m_iLastEntry];
	            m_iLastEntry--;
	            m_pHeap[n.m_iHeapLoc].m_iHeapLoc = n.m_iHeapLoc;
	            int q = n.m_iHeapLoc;
	            int p = q * 2 + 1;
	            while (p <= m_iLastEntry)
	            {
	                if ((p + 1 <= m_iLastEntry) && (m_pHeap[p].m_llTimeStamp > m_pHeap[p + 1].m_llTimeStamp))
	                    p++;
	                if (m_pHeap[q].m_llTimeStamp > m_pHeap[p].m_llTimeStamp)
	                {
	                    SNode t = m_pHeap[p];
	                    m_pHeap[p] = m_pHeap[q];
	                    m_pHeap[p].m_iHeapLoc = p;
	                    m_pHeap[q] = t;
	                    m_pHeap[q].m_iHeapLoc = q;
	                    q = p;
	                    p = q * 2 + 1;
	                }
	                else
	                    break;
	            }
	            n.m_iHeapLoc = -1;
	        }
	        // the only event has been deleted, wake up immediately
	        if (0 == m_iLastEntry)
	            m_pTimer.interrupt();
	    }
	}
	internal class SNode
	{
	    public UdtCongestionControl m_pUDT;       // Pointer to the instance of CUDT socket
	    public ulong m_llTimeStamp;      // Time Stamp
	    public int m_iHeapLoc;     // location on the heap, -1 means not on the heap
	}
	internal class UnitQueue
	{
	    struct QEntry
	    {
	        internal UdtUnit[] m_pUnit;     // unit queue
	        internal byte[][] m_pBuffer;        // data buffer
	        internal int m_iSize;        // size of each queue
	    }
	    List<QEntry> mEntries = new List<QEntry>();
	    int m_iCurrEntry = 0;
	    int m_iLastEntry = 0;
	    int m_iAvailUnit;         // recent available unit
	    int m_iAvailableQueue;
	    int m_iSize;            // total size of the unit queue, in number of packets
	    public int m_iCount;       // total number of valid packets in the queue
	    int m_iMSS;         // unit buffer size
	    AddressFamily m_iIPversion;  // IP version
	    UnitQueue()
	    {
	        m_iSize = 0;
	        m_iCount = 0;
	        m_iMSS = 0;
	        m_iIPversion = 0;
	    }
	    ~UnitQueue()
	    {
	    }
	    int init(int size, int mss, AddressFamily version)
	    {
	        QEntry tempq = new QEntry();
	        UdtUnit[] tempu = new UdtUnit[size];
	        byte[][] tempb = new byte[size][];
	        for (int i = 0; i < size; ++i)
	        {
	            tempb[i] = new byte[mss];
	            tempu[i] = new UdtUnit();
	            tempu[i].m_iFlag = 0;
	            tempu[i].m_Packet.SetDataFromBytes(tempb[i]);
	        }
	        tempq.m_pUnit = tempu;
	        tempq.m_pBuffer = tempb;
	        tempq.m_iSize = size;
	        m_iSize = size;
	        m_iMSS = mss;
	        m_iIPversion = version;
	        mEntries.Add(tempq);
	        return 0;
	    }
	    int increase()
	    {
	        // adjust/correct m_iCount
	        int real_count = 0;
	        for (int q = 0; q < mEntries.Count; ++q)
	        {
	            UdtUnit[] units = mEntries[q].m_pUnit;
	            for (int u = mEntries[q].m_iSize; u < units.Length; ++u)
	                if (units[u].m_iFlag != 0)
	                    ++real_count;
	        }
	        m_iCount = real_count;
	        if ((double)m_iCount / m_iSize < 0.9)
	            return -1;
	        // all queues have the same size
	        int size = mEntries[0].m_iSize;
	        QEntry tempq = new QEntry();
	        UdtUnit[] tempu = new UdtUnit[size];
	        byte[][] tempb = new byte[size][];
	        for (int i = 0; i < size; ++i)
	        {
	            tempb[i] = new byte[m_iMSS];
	            tempu[i].m_iFlag = 0;
	            tempu[i].m_Packet.SetDataFromBytes(tempb[i]);
	        }
	        tempq.m_pUnit = tempu;
	        tempq.m_pBuffer = tempb;
	        tempq.m_iSize = size;
	        mEntries.Add(tempq);
	        m_iSize += size;
	        return 0;
	    }
	    int shrink()
	    {
	        // currently queue cannot be shrunk.
	        return -1;
	    }
	    UdtUnit getNextAvailUnit()
	    {
	        if (m_iCount * 10 > m_iSize * 9)
	            increase();
	        if (m_iCount >= m_iSize)
	            return null;
	        QEntry entrance = mEntries[m_iCurrEntry];
	        //do
	        //{
	        //    QEntry currentEntry = mEntries[m_iCurrEntry];
	        //    Unit sentinel = currentEntry.m_pUnit[currentEntry.m_iSize - 1];
	        //    for (CUnit* sentinel = m_pCurrQueue.m_pUnit + m_pCurrQueue.m_iSize - 1; m_pAvailUnit != sentinel; ++m_pAvailUnit)
	        //        if (m_pAvailUnit.m_iFlag == 0)
	        //            return m_pAvailUnit;
	        //    if (m_pCurrQueue.m_pUnit.m_iFlag == 0)
	        //    {
	        //        m_pAvailUnit = m_pCurrQueue.m_pUnit;
	        //        return m_pAvailUnit;
	        //    }
	        //    m_pCurrQueue = m_pCurrQueue.m_pNext;
	        //    m_pAvailUnit = m_pCurrQueue.m_pUnit;
	        //} while (m_pCurrQueue != entrance);
	        increase();
	        return null;
	    }
	}
	#endregion
	#region \Internal\Utilities
	internal static class ConvertIPAddress
	{
	    // TODO use BitConverter/Block.Copy in this class
	    public static void ToUintArray(IPAddress ipAddress, ref uint[] outAddress)
	    {
	        byte[] bytes = ipAddress.GetAddressBytes();
	        if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
	        {
	            // TODO addressBytes length must be 4 in this case
	            outAddress[0] = (uint)((bytes[3] << 24) + (bytes[2] << 16) + (bytes[1] << 8) + bytes[0]);
	            return;
	        }
	        // TODO addresFamily must by InterNetworkV6
	        // addressBytesLenth must be 16
	        outAddress[3] = (uint)((bytes[15] << 24) + (bytes[14] << 16) + (bytes[13] << 8) + bytes[12]);
	        outAddress[2] = (uint)((bytes[11] << 24) + (bytes[10] << 16) + (bytes[9] << 8) + bytes[8]);
	        outAddress[1] = (uint)((bytes[7] << 24) + (bytes[6] << 16) + (bytes[5] << 8) + bytes[4]);
	        outAddress[0] = (uint)((bytes[3] << 24) + (bytes[2] << 16) + (bytes[1] << 8) + bytes[0]);
	    }
	}
	internal static class ConvertLingerOption
	{
	    public unsafe static LingerOption FromVoidPointer(void* option)
	    {
	        bool* pEnabled = (bool*)option;
	        bool bEnabled = *pEnabled;
	        int* pTime = (int*)(++pEnabled);
	        int timeSeconds = *pTime;
	        return new LingerOption(bEnabled, timeSeconds);
	    }
	    public unsafe static void ToVoidPointer(LingerOption lingerOption, void* option)
	    {
	        bool* pEnabled = (bool*)option;
	        *pEnabled = lingerOption.Enabled;
	        int* pTime = (int*)(++pEnabled);
	        *pTime = lingerOption.LingerTime;
	    }
	}
	#endregion
	#region \obj\Debug\net8.0
	#endregion
	#region \Properties
	#endregion
	#region \Server
	public sealed class UdtServer : IHostService
	{
	    internal UdtServer(UdtServerOptions options)
	    {
	        //this.State = new UdtServerState();
	    }
	    public Task StartAsync(CancellationToken cancellationToken = default)
	    {
	        return Task.CompletedTask;
	    }
	    public Task StopAsync(CancellationToken cancellationToken = default)
	    {
	        return Task.CompletedTask;
	    }
	}
	public sealed class UdtServerBuilder : IHostServiceBuilder
	{
	    IHostService IHostServiceBuilder.Build()
	    {
	        return new UdtServer(default);
	    }
	}
	public sealed class UdtServerOptions
	{
	}
	#endregion
}
#endregion
