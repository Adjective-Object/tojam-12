using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace TOJAM12
{
	public class PlayerCostume
	{
		public static List<PlayerCostume> playerCostumes = new List<PlayerCostume>();
			
		public Texture2D texture;
		public string name;

		public PlayerCostume(Texture2D texture, string name)
		{
			this.texture = texture;
			this.name = name;
		}

		public static void LoadContent(TojamGame game)
		{
			Texture2D alex = game.Content.Load<Texture2D>("misc/alex");
			PlayerCostume alexCostume = new PlayerCostume(
				alex,
				"alexander biggs"
			);
			playerCostumes.Add(alexCostume);

			Texture2D baby = game.Content.Load<Texture2D>("misc/baby");
			PlayerCostume babyCostume = new PlayerCostume(
				baby,
				"a dumb baby"
			);
			playerCostumes.Add(babyCostume);

			Texture2D cheetoes = game.Content.Load<Texture2D>("misc/cheeotes");
			PlayerCostume cheetoesCostume = new PlayerCostume(
				cheetoes,
				"Flamin' Hot Cheetoes (tm)"
			);
			playerCostumes.Add(cheetoesCostume);

		}
	}

}
