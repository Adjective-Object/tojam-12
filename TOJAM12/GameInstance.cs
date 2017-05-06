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

        Player[] players;
        
        public GameInstance(TojamGame game)
        {
            this.game = game;

            players = new Player[4];
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
                command.Data = command.Data.Trim();
                String[] tokens = command.Data.Split(' ');
                if (tokens.Length > 0)
                {
                    if (tokens[0].ToUpper() == "SAY")
                    {
                        if (tokens.Length == 1)
                            network.SendCommand(new Command(Command.CommandType.Text, command.PlayerId + " said nothing", -1));
                        else
                            network.SendCommand(new Command(Command.CommandType.Text, command.PlayerId + " said '" + command.Data.Substring(4) + "'", -1));
                    }
                    else
                    {
                        network.SendCommand(new Command(Command.CommandType.Text, "Unknown Command", command.PlayerId));
                    }
                }
            }
            else if (command.Type == Command.CommandType.Text)
            {
                ((ChatScene)game.GetScene(TojamGame.GameScenes.Chat)).AddMessage(command.Data);
            }

            
            
        }
    }
}
