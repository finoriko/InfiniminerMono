﻿using LiteNetLib;
using LiteNetLib.Utils;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Xna.Framework;

namespace InfiniminerServer
{
    public class InfiniminerNetServer : INetEventListener
    {
        private NetManager _netManager;
        public List<NetConnection> Connections { get; private set; }
        private readonly ConcurrentQueue<IncomingMessage> _messageQueue = new ConcurrentQueue<IncomingMessage>();

        public InfiniminerNetServer(NetConfiguration config)
        {
            Configuration = config;
            Connections = new List<NetConnection>();
            _netManager = new NetManager(this);
            _netManager.Start(config.Port);
        }

        public NetManager NetManager => _netManager;
        public NetConfiguration Configuration { get; private set; }

        public void Start()
        {
            // Already started in constructor
        }

        public void Shutdown()
        {
            _netManager.Stop();
        }

        public void SetMessageTypeEnabled(NetMessageType messageType, bool enabled)
        {
            // LiteNetLib handles this automatically
        }

        public NetBuffer CreateBuffer()
        {
            return new NetBuffer();
        }

        public bool ReadMessage(NetBuffer buffer, out NetMessageType messageType, out NetConnection sender)
        {
            if (_messageQueue.TryDequeue(out IncomingMessage msg))
            {
                messageType = msg.MessageType;
                sender = msg.Sender;
                buffer.SetData(msg.Data);
                return true;
            }

            messageType = NetMessageType.None;
            sender = null;
            return false;
        }

        public void SendMessage(NetBuffer buffer, NetConnection connection, NetChannel channel)
        {
            var writer = new NetDataWriter();
            writer.Put(buffer.GetData());
            connection.Peer.Send(writer, GetDeliveryMethod(channel));
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
                default:
                    return DeliveryMethod.ReliableUnordered;
            }
        }

        public void OnPeerConnected(NetPeer peer)
        {
            var connection = new NetConnection(peer);
            if (!Connections.Contains(connection))
            {
                Connections.Add(connection);
                _messageQueue.Enqueue(new IncomingMessage
                {
                    MessageType = NetMessageType.StatusChanged,
                    Sender = connection,
                    Data = new byte[0]
                });
            }
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            var connection = Connections.FirstOrDefault(c => c.Peer == peer);
            if (connection != null)
            {
                connection.ConnectionStatus = NetConnectionStatus.Disconnected;
                _messageQueue.Enqueue(new IncomingMessage
                {
                    MessageType = NetMessageType.StatusChanged,
                    Sender = connection,
                    Data = new byte[0]
                });
                Connections.Remove(connection);
            }
        }

        public void OnNetworkError(System.Net.IPEndPoint endPoint, System.Net.Sockets.SocketError socketError)
        {
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
        {
            var connection = Connections.FirstOrDefault(c => c.Peer == peer);
            if (connection != null)
            {
                var data = reader.GetRemainingBytes();
                _messageQueue.Enqueue(new IncomingMessage
                {
                    MessageType = NetMessageType.Data,
                    Sender = connection,
                    Data = data
                });
            }
        }

        public void OnNetworkReceiveUnconnected(System.Net.IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
        }

        public void OnConnectionRequest(ConnectionRequest request)
        {
            request.Accept();
        }

        public bool SanityCheck(NetConnection connection)
        {
            return Connections.Contains(connection);
        }

        public void PollEvents()
        {
            _netManager.PollEvents();
        }
    }

    // Compatibility classes to match Lidgren interface
    public class NetConfiguration
    {
        public int Port { get; set; } = 5565;
        public int MaxConnections { get; set; } = 16;
        public int MaximumConnections 
        { 
            get => MaxConnections; 
            set => MaxConnections = value; 
        }
        public string ApplicationIdentifier { get; set; } = "INFINIMINER";

        public NetConfiguration(string appIdentifier)
        {
            ApplicationIdentifier = appIdentifier;
        }
    }

    public class NetConnection : InfiniminerShared.NetConnection
    {
        public NetPeer Peer { get; private set; }
        public NetConnectionStatus ConnectionStatus { get; set; } = NetConnectionStatus.Connected;

        public NetConnection(NetPeer peer)
        {
            Peer = peer;
        }

        public override System.Net.IPEndPoint RemoteEndPoint => new System.Net.IPEndPoint(System.Net.IPAddress.Parse("127.0.0.1"), 5565);

        public void Approve()
        {
            // Connection is already approved by LiteNetLib
        }

        public void Disapprove(string reason)
        {
            Peer.Disconnect();
        }

        public override void Disconnect()
        {
            Peer.Disconnect();
        }

        public override object Status => ConnectionStatus;

        public override bool Equals(object obj)
        {
            return obj is NetConnection other && Peer.Equals(other.Peer);
        }

        public override int GetHashCode()
        {
            return Peer.GetHashCode();
        }
    }

    public enum NetConnectionStatus
    {
        Connected,
        Disconnected
    }

    public enum NetMessageType
    {
        None,
        StatusChanged,
        Data,
        ConnectionApproval
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

        public void Write(byte value) => _writer.Put(value);
        public void Write(string value) => _writer.Put(value);
        public void Write(byte[] value) => _writer.Put(value);
        public void Write(Vector3 value)
        {
            _writer.Put(value.X);
            _writer.Put(value.Y);
            _writer.Put(value.Z);
        }
        public void Write(uint value) => _writer.Put(value);
        public void Write(int value) => _writer.Put(value);
        public void Write(bool value) => _writer.Put(value);
        public void Write(float value) => _writer.Put(value);

        public byte ReadByte() => _reader.GetByte();
        public string ReadString() => _reader.GetString();
        public Vector3 ReadVector3() => new Vector3(_reader.GetFloat(), _reader.GetFloat(), _reader.GetFloat());
        public uint ReadUInt32() => _reader.GetUInt();
        public int ReadInt32() => _reader.GetInt();
        public bool ReadBoolean() => _reader.GetBool();
        public float ReadFloat() => _reader.GetFloat();

        public byte[] GetData() => _writer.Data;
        public byte[] ToArray() => _writer.Data;

        public void SetData(byte[] data)
        {
            _data = data;
            _reader = new NetDataReader(data);
        }
    }

    internal class IncomingMessage
    {
        public NetMessageType MessageType { get; set; }
        public NetConnection Sender { get; set; }
        public byte[] Data { get; set; }
    }
}