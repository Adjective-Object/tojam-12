using Microsoft.Xna.Framework;
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

        public World world;
        Dictionary<int, Player> players = new Dictionary<int, Player>();
        int myPlayerId;

        int driveTime = 0;
        bool carIsDriving;
        int carLocation;
        int lastLocation = 0;

        int statsUpdate = 0;

        public GameInstance(TojamGame game)
        {
			this.game = game;
            myPlayerId = 0;
            carIsDriving = false;
            carLocation = 0;
            world = new World();
        }

        public bool GameStarted()
        {
            if (network != null && network.IsServer() && players[0] != null)
                return true;
            else if (network != null && !network.IsServer() && myPlayerId != 0)
                return true;

            return false;
        }

		public void Update(GameTime gameTime)
        {
			if (network != null)
			{

                if (carIsDriving)
                {
                    driveTime += gameTime.ElapsedGameTime.Milliseconds;

                    if (driveTime >= world.GetLocation(carLocation).DriveLength)
                    {
                        Console.WriteLine("Finished drive");
                        int newLocation = world.GetLocation(carLocation).DriveLocation.Id;
                        carLocation = newLocation;
                        foreach (Player p in players.Values)
                        {
                            if (p.carLocation != Player.CarLocation.NotInCar)
                                p.worldLocation = carLocation;
                        }
                        driveTime = 0;
                        carIsDriving = false;
                        network.SendCommand(new Command(Command.CommandType.Text, "You have reached your next stop " + world.GetLocation(newLocation).Name, Network.SEND_ALL));
                        SendAllPlayerInfoCommand();
                    }
                }

                statsUpdate += gameTime.ElapsedGameTime.Milliseconds;
                if (statsUpdate > 2000)
                {
                    statsUpdate = 0;
                    foreach (Player p in players.Values)
                    {
                        p.hunger--;
                        p.thirst--;

                        if (p.carLocation == Player.CarLocation.NotInCar ||
                            p.carLocation == Player.CarLocation.DriversSeat)
                            p.tired--;
                        else
                            p.tired += 1;
                        if (p.tired > 100)
                            p.tired = 100;

                        if (p.tired == 40 || p.hunger == 40 || p.thirst == 40)
                        {
                            network.SendCommand(new Command(Command.CommandType.Text, p.name + " is looking sick...", Network.SEND_ALL));
                        }

                        if (p.tired <= 0 || p.hunger <= 0 || p.thirst <= 0)
                        {
                            network.SendCommand(new Command(Command.CommandType.Text, p.name + " has died", Network.SEND_ALL));
                        }
                    }
                    SendAllPlayerInfoCommand();
                }

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

        private void SendAllPlayerInfoCommand()
        {
            foreach(int key in players.Keys)
            {
                SendPlayerInfoCommand(key, key);
            }
        }

        private void SendPlayerInfoCommand(int player, int toPlayer)
        {
            // Skip server player
            if (player == 0)
                return;

			List<String> values = new List<String>();
			// location, health, hunger, thirst, tired
			Player p = players[player];

			values.Add(((int)p.carLocation).ToString());
            values.Add(((int)p.worldLocation).ToString());
            values.Add(p.health.ToString());
			values.Add(p.hunger.ToString());
   			values.Add(p.thirst.ToString());
			values.Add(p.tired.ToString());

			network.SendCommand(new Command(Command.CommandType.PlayerInfo, String.Join(",", values), toPlayer));
        }

		private void ParsePlayerInfoCommand(Player player, String data)
		{
			String[] values = data.Split(',');
			player.carLocation = (Player.CarLocation) (Int32.Parse(values[0]));
            player.worldLocation = (Int32.Parse(values[1]));
            player.health = Int32.Parse(values[2]);
			player.hunger = Int32.Parse(values[3]);
			player.thirst = Int32.Parse(values[4]);
			player.tired = Int32.Parse(values[5]);

			Debug.Write("Player got updated info " + player);
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
					chatScene.AddMessage("you must 'join <ip>' or 'host' a game ");
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
                ((ChatScene)game.GetScene(TojamGame.GameScenes.Chat)).AddMessage(command.Data, command.SourcePlayerId);
			}
            else if (command.Type == Command.CommandType.PlayerInfo)
            {
				ParsePlayerInfoCommand(players[myPlayerId], command.Data);
            }

        }

		static string helpText =
			"Commands: say, setname, enter, inventory, help";

		private void ParsePlayerCommand(Command command)
		{
            command.Data = command.Data.Trim();
            String[] tokens = command.Data.Split(' ');
            if (tokens.Length <= 0) return;

            List<string> words = new List<string>();
            bool didsomething = false;

            string upper = command.Data.ToLower();
            words.AddRange(upper.Split(' '));

            if ((words.Contains("change") && words.Contains("name")) || (words.Contains("set") && words.Contains("name")))
            {
                if (words.Count > 2 && words[words.Count - 1] != "name")
                {
                    tokens = new string[] { "setname", words[words.Count - 1] };
                    //string oldname = players[command.PlayerId].name;
                    //players[command.PlayerId].name = words[words.Count - 1];
                    //network.SendCommand(new Command(Command.CommandType.Text, oldname + " changed their name to " + players[command.PlayerId].name, command.PlayerId));
                    //didsomething = true;
                }
            }
            if (words.Contains("weather"))
            {
                network.SendCommand(new Command(Command.CommandType.Text, "the current temperture is 28 degrees and partly cloudy", command.PlayerId));
                didsomething = true;
            }
            if (words.Contains("get") && words.Contains("in") && words.Contains("car"))
            {
                command.Data = "ENTER CAR";
                tokens = new string[] { "enter", "CAR" };
            }
            if ((words.Contains("get") || words.Contains("jump")) && words.Contains("out") && (words.Contains("car") || words.Contains("window")))
            {
                tokens = new string[] { "exit", "CAR", "window" };
            }
            if ((words.Contains("current") || words.Contains("what")) && (words.Contains("time")))
            {
                DateTime dt = DateTime.Now;
                string thetime = dt.Hour.ToString() + ":" + dt.Minute.ToString();
                network.SendCommand(new Command(Command.CommandType.Text, "the current time is " + thetime, command.PlayerId));
                didsomething = true;
            }
            if ((words.Contains("switch") || words.Contains("change")) && (words.Contains("seat") || words.Contains("seats") || words.Contains("spots") || words.Contains("spot")))
            {
                int loc = (int)players[command.PlayerId].carLocation;
                if(loc != 0)
                {
                    command.Data = "ENTER CAR";
                    tokens = new string[] { "enter", "CAR" };
                }
                else
                {
                    network.SendCommand(new Command(Command.CommandType.Text, "you are not in the car", command.PlayerId));
                    didsomething = true;
                }
            }
            if ((words.Contains("current") && words.Contains("location")) || (words.Contains("where")))
            {
                string loca = world.GetLocation(players[command.PlayerId].worldLocation).Name;
                
                network.SendCommand(new Command(Command.CommandType.Text, "you are currently " + loca, command.PlayerId));
                didsomething = true;
            }

            if ((words.Contains("start") || words.Contains("begin")) && words.Contains("driving"))
            {
                tokens = new string[] { "DRIVE", "CAR" };
            }

            if (!didsomething)
            {


                switch (tokens[0].ToUpper())
                {
                    case "SAY":
                        if (tokens.Length == 1)
                            network.SendCommand(new Command(Command.CommandType.Text, players[command.PlayerId].name + " said nothing", Network.SEND_ALL, command.PlayerId));
                        else
                            network.SendCommand(new Command(Command.CommandType.Text, players[command.PlayerId].name + " said '" + command.Data.Substring(4) + "'", Network.SEND_ALL, command.PlayerId));
                        break;

                    case "YELL":
                        if (tokens.Length == 1)
                            network.SendCommand(new Command(Command.CommandType.Text, players[command.PlayerId].name + " yelled nothing", Network.SEND_ALL, command.PlayerId));
                        else
                            network.SendCommand(new Command(Command.CommandType.Text, players[command.PlayerId].name + " yelled '" + command.Data.Substring(5).ToUpper() + "!'", Network.SEND_ALL, command.PlayerId));
                        break;

                    case "SETNAME":
                        if (tokens.Length != 2)
                            network.SendCommand(new Command(Command.CommandType.Text, "give yourself a name", command.PlayerId));
                        else
                        {
                            string oldname = players[command.PlayerId].name;
                            players[command.PlayerId].name = tokens[1];
                            network.SendCommand(new Command(Command.CommandType.Text, oldname + " changed their name to " + players[command.PlayerId].name, Network.SEND_ALL, command.PlayerId));
                        }
                        break;
                    case "DRIVE":
                        ParseDriveCommand(command);
                        break;
                    case "STOP":
                        ParseStopCommand(command);
                        break;
                    case "ENTER":
                        ParseEnterCommand(command.Data, command.PlayerId, tokens);
                        break;
                    case "LEAVE":
                    case "EXIT":
                        ParseExitCommand(command.Data, command.PlayerId, tokens);
                        break;
                    case "GIVEME!":
                        if (tokens.Length != 2)
                        {
                            network.SendCommand(new Command(Command.CommandType.Text, "takes 2 args", command.PlayerId));
                            break;
                        }
                        Item foundItem = Item.Get(tokens[1]);
                        if (foundItem == null)
                        {
                            network.SendCommand(new Command(Command.CommandType.Text, "no such item " + tokens[1], command.PlayerId));
                            break;
                        }

                        players[command.PlayerId].inventory.Add(foundItem);
                        network.SendCommand(new Command(Command.CommandType.Text, "I magically gave you a " + foundItem.GetPrimaryName(), command.PlayerId));
                        break;

                    case "MENU":
                    case "HELP":
                        network.SendCommand(new Command(Command.CommandType.Text, helpText, command.PlayerId));
                        break;

                    case "INV":
                    case "INVENTORY":
                        List<String> builder = new List<String>();
                        foreach (Item i in players[command.PlayerId].inventory)
                        {
                            builder.Add(i.GetPrimaryName());
                        }
                        if (builder.Count == 0)
                            network.SendCommand(new Command(Command.CommandType.Text, "You have: Nothing!", command.PlayerId));
                        else
                            network.SendCommand(new Command(Command.CommandType.Text, "You have: " + String.Join(", ", builder), command.PlayerId));
                        break;

                    default:
                        // perform action on player's inventory
                        Player p = players[command.PlayerId];
                        if (tokens.Length >= 2 && p.ItemVerb(this, tokens)) break;

                        // otherwise, don't
                        network.SendCommand(new Command(Command.CommandType.Text, "Don't know what '" + command.Data + "' means...", command.PlayerId));
                        break;
                }
            }

			foreach (int playerId in players.Keys)
			{
				SendPlayerInfoCommand(playerId, playerId);
			}
		}

		public void sendToPlayer(Player p, string message)
		{
			foreach (KeyValuePair<int, Player> pair in players)
			{
				if (p == null || pair.Value == p)
				{
					network.SendCommand(new Command(Command.CommandType.Text, message, pair.Key));
					return;
				}
			}
		}

        private void ParseStopCommand(Command command)
        {
            if (players[command.PlayerId].carLocation == Player.CarLocation.DriversSeat)
            {
                if (!carIsDriving)
                    network.SendCommand(new Command(Command.CommandType.Text, "The car is already stopped...", command.PlayerId));
                else
                {
                    carIsDriving = false;
                    network.SendCommand(new Command(Command.CommandType.Text, players[command.PlayerId].name + " stopped the car", Network.SEND_ALL));
                    /*
                    foreach (Player p in players.Values)
                    {
                        carLocation = world.GetLocation(lastLocation).DriveLocation.Id;
                        if (p.carLocation != Player.CarLocation.NotInCar)
                        {
                            p.worldLocation = carLocation;
                        }
                    }
                    */
                    SendAllPlayerInfoCommand();
                }
            }
            else
            {
                network.SendCommand(new Command(Command.CommandType.Text, "You're not in the driver's seat", command.PlayerId));
            }
        }
        private void ParseDriveCommand(Command command)
        {
            if (players[command.PlayerId].carLocation == Player.CarLocation.DriversSeat)
            {
                if (carIsDriving)
                    network.SendCommand(new Command(Command.CommandType.Text, "The car is already moving!!!", command.PlayerId));
                else
                {
                    if (world.GetLocation(carLocation).DriveLocation == null)
                    {
                        network.SendCommand(new Command(Command.CommandType.Text, "You are at your destination!", command.PlayerId));
                        return;
                    }
                    carIsDriving = true;
                    
                    if (!world.GetLocation(carLocation).IsDriveLocation)
                    {
                        carLocation = world.GetLocation(carLocation).DriveLocation.Id;
                    }

                    network.SendCommand(new Command(Command.CommandType.Text, players[command.PlayerId].name + " started the car and began to drive.", Network.SEND_ALL));

                    if (!world.GetLocation(carLocation).IsDriveLocation)
                    {
                        driveTime = 0;
                        lastLocation = players[command.PlayerId].worldLocation;
                    }

					List<String> abandonedPlayers = new List<String>();
					foreach (Player p in this.players.Values)
					{
                        if (p.carLocation == Player.CarLocation.NotInCar)
                            abandonedPlayers.Add(p.name);
                        else
                        {
                            p.worldLocation = carLocation;
                        }
					}
					if (abandonedPlayers.Count != 0)
					{
						// TODO gameovers? ingame locations?
						network.SendCommand(new Command(Command.CommandType.Text, String.Join(", ", abandonedPlayers) + " were left behind", Network.SEND_ALL));
					}
                    SendAllPlayerInfoCommand();
                }
            }
            else
            {
                network.SendCommand(new Command(Command.CommandType.Text, "You're not in the driver's seat", command.PlayerId));
            }
        }

        private void ParseExitCommand(String data, int PlayerId, String[] tokens)
        {
            if (carIsDriving)
            {
                network.SendCommand(new Command(Command.CommandType.Text, "The car is moving... you might die...", PlayerId));
                return;
            }

            if (players[PlayerId].carLocation != Player.CarLocation.NotInCar)
            {
                SetPlayerCarLocation(PlayerId, Player.CarLocation.NotInCar);
            }
            else
            {
                network.SendCommand(new Command(Command.CommandType.Text, "Nothing to exit...", PlayerId));
            }
        }

        private void ParseEnterCommand(String data, int PlayerId, String[] tokens)
        {
            if (carIsDriving)
            {
                network.SendCommand(new Command(Command.CommandType.Text, "The car is moving...", PlayerId));
                return;
            }

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

            if (location == Player.CarLocation.NotInCar)
                network.SendCommand(new Command(Command.CommandType.Text, players[playerId].name + " left the car", Network.SEND_ALL));
            else
                network.SendCommand(new Command(Command.CommandType.Text, players[playerId].name + " sat in the " + Player.GetCarLocationName(location), Network.SEND_ALL));

            // Dont send to server player, since already updated.
            if (playerId != 0)
                SendPlayerInfoCommand(playerId, playerId);
        }

		public Player GetMyPlayer() {
			return players.ContainsKey(myPlayerId) ? this.players[myPlayerId] : null;
		}

	}
}
