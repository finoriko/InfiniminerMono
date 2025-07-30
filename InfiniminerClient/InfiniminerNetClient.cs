using LiteNetLib;
using LiteNetLib.Utils;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Net;
using System.Linq;
using Microsoft.Xna.Framework;
using InfiniminerShared;

namespace InfiniminerMono
{
    public class NetClient : INetEventListener
    {
        private NetManager _netManager;
        private NetPeer _serverPeer;
        private readonly ConcurrentQueue<IncomingMessage> _messageQueue = new ConcurrentQueue<IncomingMessage>();
        public NetConnectionStatus Status { get; private set; } = NetConnectionStatus.Disconnected;

        public NetClient(NetConfiguration config)
        {
            _netManager = new NetManager(this);
            _netManager.Start();
        }

        public void Start()
        {
            // Already started in constructor
        }

        public void SetMessageTypeEnabled(NetMessageType messageType, bool enabled)
        {
            // LiteNetLib handles this automatically
        }

        public NetBuffer CreateBuffer()
        {
            return new NetBuffer();
        }

        public bool ReadMessage(NetBuffer buffer, out NetMessageType messageType)
        {
            if (_messageQueue.TryDequeue(out IncomingMessage msg))
            {
                messageType = msg.MessageType;
                buffer.SetData(msg.Data);
                return true;
            }

            messageType = NetMessageType.None;
            return false;
        }

        public void Connect(IPEndPoint serverEndPoint, byte[] data)
        {
            var writer = new NetDataWriter();
            writer.Put(data);
            _serverPeer = _netManager.Connect(serverEndPoint, writer);
            Status = NetConnectionStatus.Connecting;
        }

        public void DiscoverLocalServers(int port)
        {
            _netManager.SendBroadcast(new NetDataWriter(), port);
        }

        public void SendMessage(NetBuffer buffer, NetChannel channel)
        {
            if (_serverPeer != null && _serverPeer.ConnectionState == ConnectionState.Connected)
            {
                var writer = new NetDataWriter();
                writer.Put(buffer.GetData());
                _serverPeer.Send(writer, GetDeliveryMethod(channel));
            }
        }

        private DeliveryMethod GetDeliveryMethod(NetChannel channel)
        {
            switch (channel)
            {
                case NetChannel.ReliableInOrder1:
                case NetChannel.ReliableInOrder2:
                case NetChannel.ReliableInOrder3:
                    return DeliveryMethod.ReliableOrdered;
                case NetChannel.UnreliableInOrder1:
                    return DeliveryMethod.Sequenced;
                case NetChannel.ReliableUnordered:
                default:
                    return DeliveryMethod.ReliableUnordered;
            }
        }

        public void PollEvents()
        {
            _netManager.PollEvents();
        }

        public void Disconnect()
        {
            _serverPeer?.Disconnect();
            Status = NetConnectionStatus.Disconnected;
        }

        public void Shutdown(string reason)
        {
            Disconnect();
            _netManager.Stop();
        }

        public void OnPeerConnected(NetPeer peer)
        {
            if (peer == _serverPeer)
            {
                Status = NetConnectionStatus.Connected;
                _messageQueue.Enqueue(new IncomingMessage
                {
                    MessageType = NetMessageType.StatusChanged,
                    Data = new byte[0]
                });
            }
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            if (peer == _serverPeer)
            {
                Status = NetConnectionStatus.Disconnected;
                _messageQueue.Enqueue(new IncomingMessage
                {
                    MessageType = NetMessageType.StatusChanged,
                    Data = new byte[0]
                });
            }
        }

        public void OnNetworkError(IPEndPoint endPoint, System.Net.Sockets.SocketError socketError)
        {
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
        {
            if (peer == _serverPeer)
            {
                var data = reader.GetRemainingBytes();
                _messageQueue.Enqueue(new IncomingMessage
                {
                    MessageType = NetMessageType.Data,
                    Data = data
                });
            }
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            if (messageType == UnconnectedMessageType.Broadcast)
            {
                _messageQueue.Enqueue(new IncomingMessage
                {
                    MessageType = NetMessageType.ServerDiscovered,
                    Data = reader.GetRemainingBytes()
                });
            }
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
        }

        public void OnConnectionRequest(ConnectionRequest request)
        {
            // Client doesn't handle connection requests
        }
    }

    // Compatibility classes to match Lidgren interface
    public class NetConfiguration
    {
        public string ApplicationIdentifier { get; set; } = "INFINIMINER";

        public NetConfiguration(string appIdentifier)
        {
            ApplicationIdentifier = appIdentifier;
        }
    }

    public enum NetConnectionStatus
    {
        Disconnected,
        Connecting,
        Connected
    }

    public enum NetMessageType
    {
        None,
        StatusChanged,
        Data,
        ConnectionApproval,
        ConnectionRejected,
        ServerDiscovered
    }

    public enum NetChannel
    {
        ReliableInOrder1,
        ReliableInOrder2, 
        ReliableInOrder3,
        UnreliableInOrder1,
        ReliableUnordered
    }

    public class NetBuffer
    {
        private NetDataWriter _writer = new NetDataWriter();
        private NetDataReader _reader;
        private byte[] _data;

        public int LengthBytes => _data?.Length ?? 0;
        public int Position => _reader?.Position ?? 0;

        public void Write(byte value) => _writer.Put(value);
        public void Write(string value) => _writer.Put(value);
        public void Write(Vector3 value)
        {
            _writer.Put(value.X);
            _writer.Put(value.Y);
            _writer.Put(value.Z);
        }
        public void Write(uint value) => _writer.Put(value);
        public void Write(ushort value) => _writer.Put(value);
        public void Write(int value) => _writer.Put(value);
        public void Write(bool value) => _writer.Put(value);
        public void Write(float value) => _writer.Put(value);
        public void Write(IPEndPoint endPoint)
        {
            _writer.Put(endPoint.Address.GetAddressBytes());
            _writer.Put(endPoint.Port);
        }

        public byte ReadByte() => _reader.GetByte();
        public string ReadString() => _reader.GetString();
        public Vector3 ReadVector3() => new Vector3(_reader.GetFloat(), _reader.GetFloat(), _reader.GetFloat());
        public uint ReadUInt32() => _reader.GetUInt();
        public ushort ReadUInt16() => _reader.GetUShort();
        public int ReadInt32() => _reader.GetInt();
        public bool ReadBoolean() => _reader.GetBool();
        public float ReadFloat() => _reader.GetFloat();
        public byte[] ReadBytes(int count) => _reader.GetBytesWithLength();
        public IPEndPoint ReadIPEndPoint()
        {
            var addressBytes = _reader.GetBytesWithLength();
            var port = _reader.GetInt();
            return new IPEndPoint(new IPAddress(addressBytes), port);
        }

        public byte[] GetData() => _writer.Data;
        public byte[] ToArray() => _writer.Data;

        public void SetData(byte[] data)
        {
            _data = data;
            _reader = new NetDataReader(data);
        }
    }

    public class NetConnection : InfiniminerShared.NetConnection
    {
        private IPEndPoint _remoteEndPoint;

        public NetConnection(IPEndPoint remoteEndPoint)
        {
            _remoteEndPoint = remoteEndPoint;
        }

        public override IPEndPoint RemoteEndPoint => _remoteEndPoint;

        public override void Disconnect()
        {
            // Client-side disconnect would be handled by the NetClient
        }

        public override object Status => NetConnectionStatus.Connected;
    }

    internal class IncomingMessage
    {
        public NetMessageType MessageType { get; set; }
        public byte[] Data { get; set; }
    }
}