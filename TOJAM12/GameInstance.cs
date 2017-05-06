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
        public GameInstance()
        {
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

            if (isServer)
                network.SendCommand(command);
        }
    }
}
