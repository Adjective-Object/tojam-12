using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TOJAM12
{
	public class GameScene : Scene
	{

		List<Player> entities = new List<Player>();
		Texture2D alex;

		public void onTransition(Dictionary<string, object> parameters)
		{
			// clear existing entity list
			entities.Clear();

			// pull input objects from parameters and initialize players from them
			for (int i = 0; i < 4; i++)
			{
				string key = "player" + (i + 1);
				if (!parameters.ContainsKey(key)) continue;

				Input input = (Input) (parameters[key]);
				if (input == null) continue;

				Player p = new Player();
				p.Initialize(input, new Vector2(100 + i * 100, 100), alex);
				entities.Add(p);
			}
		}

		public void Initialize(TojamGame game)
		{
			// TODO Load spritesheets for each player character
		}

		public void LoadContent(TojamGame game)
		{
			alex = game.Content.Load<Texture2D>("misc/alex");
		}

		public void Update(TojamGame game, GameTime gameTime)
		{
			foreach(Player p in entities) {
				p.Update(game, gameTime);
			}
		}

		public void Draw(TojamGame game, GameTime gameTime)
		{
			game.graphics.GraphicsDevice.Clear(Color.Gainsboro);
			game.spriteBatch.Begin(SpriteSortMode.Immediate);

			foreach (Player p in entities)
			{
				p.Draw(game, gameTime);
			}

			game.spriteBatch.End();
		}


	}
}
