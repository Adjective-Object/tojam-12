using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOJAM12
{
    public class Command
    {
        public enum CommandType
        {
            Player = 0, // Command sent from a player
            Text = 1, // Command to just dump text
			PlayerJoined = 2, // Command for when a player joins
            PlayerInfo = 3, // Command to update player info
			PictureEvent = 4,
        }

        public String Data;
        public CommandType Type;
        public int PlayerId;
		public int SourcePlayerId;

        public Command(CommandType type, String data, int playerId, int sourcePlayerId = -1)
        {
            Data = data;
            Type = type;
            PlayerId = playerId;
			SourcePlayerId = sourcePlayerId;
        }

		public static Command PreparePlayerInfoCommand(Player p, int toPlayer)
		{
			List<String> values = new List<String>();

			values.Add(((int)p.carLocation).ToString());
			values.Add(((int)p.worldLocation).ToString());
			values.Add(p.health.ToString());
			values.Add(p.hunger.ToString());
			values.Add(p.thirst.ToString());
			values.Add(p.tired.ToString());
			values.Add(p.money.ToString());

			return new Command(Command.CommandType.PlayerInfo, String.Join(",", values), toPlayer);
		}

		public static void ParsePlayerInfoCommand(Player player, String data)
		{
			String[] values = data.Split(',');
			player.carLocation = (Player.CarLocation)(Int32.Parse(values[0]));
			player.worldLocation = (Int32.Parse(values[1]));
			player.health = Int32.Parse(values[2]);
			player.hunger = Int32.Parse(values[3]);
			player.thirst = Int32.Parse(values[4]);
			player.tired = Int32.Parse(values[5]);
			player.money = Int32.Parse(values[6]);
		}

		public static Command PreparePictureEventCommand(String evtName, Dictionary<string, object> arguments, int toPlayer)
		{
			List<String> values = new List<String>();

			values.Add((evtName));
			if (arguments != null)
			{
				foreach (String key in arguments.Keys)
				{
					values.Add(key + ":" + arguments[key]);
				}
			}

			return new Command(Command.CommandType.PictureEvent, String.Join(",", values), toPlayer);
		}

		public static CarPicture.PictureEvent ParsePictureEventCommand(String data)
		{
			String[] values = data.Split(',');
			String eventName = values[0];
			Dictionary<String, Object> arguments = new Dictionary<string, object>();
			Debug.WriteLine("Event " + eventName + ":");
			for (int i = 1; i < values.Length; i++)
			{
				String[] keyVal = values[i].Split(':');
				Debug.WriteLine("  " + keyVal[0] + ": " + keyVal[1]);
			    arguments[keyVal[0]] = keyVal[1];
			}

			CarPicture.PictureEvent e = new CarPicture.PictureEvent();
			e.name = eventName;
			e.arguments = arguments;
			return e;
		}


    }
}
