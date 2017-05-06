using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOJAM12
{
    public class Network
    {

        public const int SEND_ALL = -1;

        bool isServer;
        public bool IsServer()
        {
            return isServer;
        }
        NetServer server;

        NetPeer peer;

        bool connected;

        NetConnection[] connections;

        List<Command> commands;
        List<Command> localCommands;
        public bool Start(bool asServer, string ip)
        {
            commands = new List<Command>();
            localCommands = new List<Command>();

            connections = new NetConnection[3];

            connected = true;
            isServer = asServer;
            if (isServer)
            {
                Console.WriteLine("Server");
                NetPeerConfiguration config = new NetPeerConfiguration("TOJAM12") { Port = 12345 };
				SendCommand(new Command(Command.CommandType.PlayerJoined, "0", 0));
                server = new NetServer(config);
                server.Start();
                peer = server;
            }
            else
            {
                Console.WriteLine("Client");
                NetPeerConfiguration config = new NetPeerConfiguration("TOJAM12");
                NetClient client = new NetClient(config);
                client.Start();
                client.Connect(host: ip, port: 12345);
                peer = client;
            }
            return true;
        }

        public void SendCommand(Command command)
        {
            if (!connected)
                return;

            if (isServer && (command.PlayerId == 0 || command.PlayerId == -1))
                localCommands.Add(command);
            
            if (command.PlayerId != 0)
            {
                foreach (NetConnection connection in peer.Connections)
                {
                    if (command.PlayerId == -1 || (command.PlayerId > 0 && connection == connections[command.PlayerId - 1]))
                    {
                        NetOutgoingMessage sendMsg = peer.CreateMessage();
                        sendMsg.Write((Int32)command.Type);
                        sendMsg.Write(command.Data);
                        peer.SendMessage(sendMsg, connection, NetDeliveryMethod.ReliableOrdered);
                    }
                }
            }
        }

        public List<Command> GetLocalCommands()
        {
            return localCommands;
        }
        public List<Command> GetCommands()
        {
            return commands;
        }
        
        public void Update()
        {
            localCommands.Clear();
            commands.Clear();

            if (!connected)
                return;

            NetIncomingMessage message;
            while ((message = peer.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        // handle custom messages
                        int messageType = message.ReadInt32();
                        String messageData = message.ReadString();

                        int playerId = -1;
                        if (isServer)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                if (connections[i] != null && connections[i] == message.SenderConnection)
                                {
                                    playerId = i+1;
                                    break;
                                }
                            }
                        }
                        commands.Add(new Command((Command.CommandType)messageType, messageData, playerId));

                        break;

                    case NetIncomingMessageType.StatusChanged:
                        // handle connection status messages
                        Console.WriteLine(message.SenderConnection.RemoteEndPoint.Address.ToString() + ": Status Changed " + message.SenderConnection.Status.ToString());
                        if (isServer)
                        {
                            if (message.SenderConnection.Status == NetConnectionStatus.Connected)
                            {
                                bool addedConnection = false;
                                for (int i = 0; i < 3; i++)
                                {
                                    if (connections[i]  == null)
                                    {
                                        connections[i] = message.SenderConnection;
                                        Console.WriteLine("Added connection as player id " + (i+1));
										SendCommand(new Command(Command.CommandType.PlayerJoined, (i+1).ToString(), 0));
                                        SendCommand(new Command(Command.CommandType.PlayerJoined, (i + 1).ToString(), (i+1)));
                                        addedConnection = true;
                                        break;
                                    }
                                }
                                if (!addedConnection)
                                {
                                    Console.WriteLine("Max players reached, disconnecting");
                                    message.SenderConnection.Deny();
                                }
                            }
                        }
                        break;

                    case NetIncomingMessageType.DebugMessage:
                        Console.WriteLine(message.ReadString());
                        break;

                    case NetIncomingMessageType.WarningMessage:
                        Console.WriteLine("Warning: " + message.ReadString());
                        break;
                    default:
                        Console.WriteLine("unhandled message with type: " + message.MessageType);
                        break;
                }
            }
        }
    }
}
