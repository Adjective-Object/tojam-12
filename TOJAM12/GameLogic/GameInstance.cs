using System;
using System.Collections.Generic;
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
        
        public GameInstance(TojamGame game)
        {
			this.game = game;
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
					ParseCommand(new Command(Command.CommandType.Player, command, 1));
				}
				else
				{
					network.SendCommand(new Command(Command.CommandType.Player, command, 0));
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
					chatScene.AddMessage("started hosting ");
					break;
				default:
					chatScene.AddMessage("you must 'join <ip>' a game or 'host' a game ");
					break;
			}

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
						network.SendCommand(new Command(Command.CommandType.Text, players[command.PlayerId].name + " said nothing", 0));
					else
						network.SendCommand(new Command(Command.CommandType.Text, players[command.PlayerId].name + " said '" + command.Data.Substring(4) + "'", 0));
					break;
				case "SETNAME":
					if (tokens.Length != 2)
						network.SendCommand(new Command(Command.CommandType.Text, "give yourself a name", command.PlayerId));
					else
						players[command.PlayerId].name = tokens[1];
					break;
				default:
					network.SendCommand(new Command(Command.CommandType.Text, "Unknown Command", command.PlayerId));
					break;
			}
		}
    }
}
