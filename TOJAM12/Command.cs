using System;
using System.Collections.Generic;
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
            Text = 1 // Command to just dump text
        }

        public String Data;
        public CommandType Type;
        public int PlayerId;

        public Command(CommandType type, String data, int playerId)
        {
            Data = data;
            Type = type;
            PlayerId = playerId;
        }
    }
}
