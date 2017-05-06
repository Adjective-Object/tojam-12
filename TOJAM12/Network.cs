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
        NetServer server;
        NetPeer peer;

        List<String> commands;
        public bool Start(bool isServer)
        {
            commands = new List<String>();
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
                NetPeerConfiguration config = new NetPeerConfiguration("TOJAM12");
                NetClient client = new NetClient(config);
                client.Start();
                client.Connect(host: "127.0.0.1", port: 12345);
                peer = client;
            }
            return true;
        }

        public void SendCommand(String command)
        {
            foreach (NetConnection connection in peer.Connections)
            {
                NetOutgoingMessage sendMsg = peer.CreateMessage(command);
                peer.SendMessage(sendMsg, connection, NetDeliveryMethod.ReliableOrdered);
            }
        }

        public List<String> GetCommands()
        {
            return commands;
        }
        
        public void Update()
        {
            commands.Clear();
            NetIncomingMessage message;
            while ((message = peer.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        // handle custom messages
                        String messageString = message.ReadString();
                        Console.WriteLine("Message: " + messageString);
                        commands.Add(messageString);
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        // handle connection status messages
                        Console.WriteLine(message.SenderConnection.RemoteEndPoint.Address.ToString() + ": Status Changed " + message.SenderConnection.Status.ToString());
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
