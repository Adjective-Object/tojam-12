using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TOJAM12
{
	public class Config
	{
		public bool isHost;
		public string ipAddress;

		private Config(Dictionary<string, string> vals)
		{
			isHost = vals.ContainsKey("isHost") && vals["isHost"] == "true";
			ipAddress = vals.ContainsKey("ip") ? vals["ip"] : null;

			Console.WriteLine("isHost = " + isHost);
			Console.WriteLine("ipAddress = " + ipAddress);
		}

		public static Config ReadFromFile(string filepath)
		{
			string line;
			Dictionary<string, string> config = new Dictionary<string, string>();

			try
			{
				System.IO.StreamReader file =
				   new System.IO.StreamReader(filepath);
				
				while ((line = file.ReadLine()) != null)
				{
					string[] parts = line.Trim().Split('=');
					if (parts.Length != 2) continue;
					config[parts[0]] = parts[1];
				}
			
				return new Config(config);
			}

			catch (IOException)
			{
				return new Config(config);
			}
		}
	}
}