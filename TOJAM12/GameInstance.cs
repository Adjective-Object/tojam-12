using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOJAM12
{
    public class GameInstance
    {
        Network network;
        bool isServer;
        TojamGame game;
        public GameInstance(TojamGame game)
        {
            this.game = game;
            isServer = true;
            network = new Network();
            network.Start(isServer);
        }
        public void Update()
        {
            network.Update();
            foreach(String command in network.GetCommands())
            {
                ParseCommand(command);
            }
        }

        public void SendPlayerCommand(String command)
        {
            if (isServer)
            {
                ParseCommand(command);
            }
            else
            {
                network.SendCommand(command);
            }
        }

        private void ParseCommand(String command)
        {
            //Decide what to do with command
            Console.WriteLine("Parsing command '" + command + "'");

            ((ChatScene)game.GetScene(TojamGame.GameScenes.Chat)).AddMessage(command);

            if (isServer)
                network.SendCommand(command);
        }
    }
}
