using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TOJAM12
{
    public class GameInstance
    {
        Network network;
        TojamGame game;

        Dictionary<int, Player> players = new Dictionary<int, Player>();
        int myPlayerId;

        public GameInstance(TojamGame game)
        {
			this.game = game;
            myPlayerId = 0;
        }

		public void Update()
        {
			if (network != null)
			{
				if (network.IsServer())
				{
					// Parse server generated messages
					foreach (Command command in network.GetLocalCommands())
					{
						ParseCommand(command);
					}
				}

				network.Update();
				foreach (Command command in network.GetCommands())
				{
					ParseCommand(command);
				}
			}
        }

        public void SendPlayerCommand(String command)
        {
			if (network != null)
			{
				if (network.IsServer())
				{
					ParseCommand(new Command(Command.CommandType.Player, command, 0));
				}
				else
				{
					network.SendCommand(new Command(Command.CommandType.Player, command, -1));
				}
			}
			else {
				ParseClientCommand(command);

			}
        }

        private void SendTextCommand(String command, int player)
        {

        }

		private void ParseClientCommand(string command)
		{
			// join or start server
			String[] tokens = command.ToLower().Split(' ');
			if (tokens.Length == 0) return;
			ChatScene chatScene = (ChatScene)game.GetScene(TojamGame.GameScenes.Chat);
			switch (tokens[0])
			{
				case "join":
					string ip = (tokens.Length > 1) ? tokens[1] : "10.206.236.146";
					IPAddress address;
					if (!IPAddress.TryParse(ip, out address))
					{
						chatScene.AddMessage("'" + ip + "' is not not an IP address");
						break;

					}
					chatScene.AddMessage("joining " + ip);
					network = new Network();
					network.Start(false, ip);
					break;
				case "host":
					network = new Network();
					network.Start(true, null);
                    string lip = GetLocalIPAddress();
					chatScene.AddMessage("started hosting on: " + lip);
					break;
				default:
					chatScene.AddMessage("you must 'join <ip>' a game or 'host' a game ");
					break;
			}

		}

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        private void ParseCommand(Command command)
        {
            //Decide what to do with command
            Console.WriteLine("Parsing command " + command.Type + " from Player " + command.PlayerId + " '" + command.Data + "'");

            // Player command is a command issued by the player
            // Only handled by server
            if (network.IsServer() && command.Type == Command.CommandType.Player)
            {
				ParsePlayerCommand(command);
            }
            if (network.IsServer() && command.Type == Command.CommandType.PlayerJoined)
            {
                int id = int.Parse(command.Data);
                players[id] = new Player("player_" + id);
                Debug.WriteLine("added Player with name " + players[id].name);
            }
            else if (command.Type == Command.CommandType.PlayerJoined)
            {
                myPlayerId = int.Parse(command.Data);
                Console.WriteLine("My player id: " + myPlayerId);
            }
            else if (command.Type == Command.CommandType.Text)
            {
                ((ChatScene)game.GetScene(TojamGame.GameScenes.Chat)).AddMessage(command.Data);
            }

        }

		private void ParsePlayerCommand(Command command)
		{
			command.Data = command.Data.Trim();
			String[] tokens = command.Data.Split(' ');
			if (tokens.Length <= 0) return;

			switch (tokens[0].ToUpper())
			{
				case "SAY":
					if (tokens.Length == 1)
						network.SendCommand(new Command(Command.CommandType.Text, players[command.PlayerId].name + " said nothing", Network.SEND_ALL));
					else
						network.SendCommand(new Command(Command.CommandType.Text, players[command.PlayerId].name + " said '" + command.Data.Substring(4) + "'", Network.SEND_ALL));
					break;
				case "SETNAME":
					if (tokens.Length != 2)
						network.SendCommand(new Command(Command.CommandType.Text, "give yourself a name", command.PlayerId));
					else
						players[command.PlayerId].name = tokens[1];
					break;
                case "ENTER":
                    if (tokens.Length > 1)
                    {
                        string destination = command.Data.Substring(6).ToUpper();
                        if (destination == "CAR")
                            network.SendCommand(new Command(Command.CommandType.Text, players[command.PlayerId].name + " got in the car", Network.SEND_ALL));
                        else if(destination.StartsWith("DRIVER"))
                            network.SendCommand(new Command(Command.CommandType.Text, players[command.PlayerId].name + " got in the driver's seat", Network.SEND_ALL));
                    }
                    else
                        network.SendCommand(new Command(Command.CommandType.Text, "Not sure what you're trying to enter...", command.PlayerId));
                    break;
				default:
					network.SendCommand(new Command(Command.CommandType.Text, "Don't know what '" + command.Data + "' means...", command.PlayerId));
					break;
			}
		}
    }
}
