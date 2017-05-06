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
					network.SendCommand(new Command(Command.CommandType.Player, command, Network.SEND_ALL));
				}
			}
			else {
				ParseClientCommand(command);

			}
        }

        private void SendPlayerInfoCommand(int player, int toPlayer)
        {
            network.SendCommand(new Command(Command.CommandType.PlayerInfo, ((int)players[player].carLocation).ToString(), toPlayer));
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
                players[myPlayerId] = new Player("SELF");
            }
            else if (command.Type == Command.CommandType.Text)
            {
                ((ChatScene)game.GetScene(TojamGame.GameScenes.Chat)).AddMessage(command.Data);
			}
            else if (command.Type == Command.CommandType.PlayerInfo)
            {
                players[myPlayerId].carLocation = (Player.CarLocation)Int32.Parse(command.Data);
                Console.WriteLine("My location is now " + Player.GetCarLocationName(players[myPlayerId].carLocation));
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
                    ParseEnterCommand(command.Data, command.PlayerId, tokens);
                    break;
				default:
					network.SendCommand(new Command(Command.CommandType.Text, "Don't know what '" + command.Data + "' means...", command.PlayerId));
					break;
			}
		}

        private void ParseEnterCommand(String data, int PlayerId, String[] tokens)
        {
            if (tokens.Length > 1)
            {
                string destination = data.Substring(6).ToUpper();
                if (destination == "CAR")
                {
                    // Find a free seat
                    for(int i = 1; i < 5; i++)
                    {
                        if (GetSeatPlayer((Player.CarLocation)i) == -1)
                        {
                            SetPlayerCarLocation(PlayerId, (Player.CarLocation)i);
                            return;
                        }
                    }
                }
                else if (destination.StartsWith("DRIVER"))
                {
                    int seatPlayer = GetSeatPlayer(Player.CarLocation.DriversSeat);
                    if (seatPlayer == -1)
                    {
                        SetPlayerCarLocation(PlayerId, Player.CarLocation.DriversSeat);
                        return;
                    }else
                    {
                        network.SendCommand(new Command(Command.CommandType.Text, players[seatPlayer].name + " is already in the driver's seat", PlayerId));
                        return;
                    }

                }
            }
            
            network.SendCommand(new Command(Command.CommandType.Text, "Not sure what you're trying to enter...", PlayerId));
        }

        private int GetSeatPlayer(Player.CarLocation location)
        {
            foreach (KeyValuePair<int, Player> entry in players)
            {
                if (entry.Value.carLocation == location)
                {
                    return entry.Key;
                }
            }
            return -1;
        }

        private void SetPlayerCarLocation(int playerId, Player.CarLocation location)
        {
            players[playerId].carLocation = location;
            network.SendCommand(new Command(Command.CommandType.Text, players[playerId].name + " sat in the " + Player.GetCarLocationName(location), Network.SEND_ALL));

            // Dont send to server player, since already updated.
            if (playerId != 0)
                SendPlayerInfoCommand(playerId, playerId);
        }
    }
}
