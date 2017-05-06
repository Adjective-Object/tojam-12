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
        public bool Start(bool asServer)
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
                client.Connect(host: "110.206.236.146", port: 12345);
                peer = client;
            }
            return true;
        }

        public void SendCommand(Command command)
        {
            if (!connected)
                return;

            if (isServer && (command.PlayerId == 1 || command.PlayerId == 0))
                localCommands.Add(command);
            
            if (command.PlayerId != 1)
            {
                foreach (NetConnection connection in peer.Connections)
                {
                    if (command.PlayerId == 0 || (command.PlayerId > 1 && connection == connections[command.PlayerId - 2]))
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

                        int playerId = 0;
                        if (isServer)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                if (connections[i] != null && connections[i] == message.SenderConnection)
                                {
                                    playerId = i + 2;
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
                                for (int i = 0; i < 3; i++)
                                {
                                    if (connections[i]  == null)
                                    {
                                        connections[i] = message.SenderConnection;
                                        Console.WriteLine("Added connection as played id " + i);
                                        SendCommand(new Command(Command.CommandType.Text, "Player " + message.SenderConnection.RemoteEndPoint.Address.ToString() + " joined", 0));
                                        break;
                                    }
                                }
                            }
                        }
                        break;

                    case NetIncomingMessageType.DebugMessage:
                        // handle debug messages
                        // (only received when compiled in DEBUG mode)
                        Console.WriteLine(message.ReadString());
                        break;

                    /* .. */
                    default:
                        Console.WriteLine("unhandled message with type: " + message.MessageType);
                        break;
                }
            }
        }
    }
}
