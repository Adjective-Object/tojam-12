using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TOJAM12
{
	public class GameScene : Scene
	{

		List<Entity> entities = new List<Entity>();

		public void onTransition(Dictionary<string, object> parameters)
		{
			// clear existing entity list
			entities.Clear();

			// pull input objects from parameters and initialize players from them
			for (int i = 0; i < 4; i++)
			{
				string key = "player" + (i + 1);
				if (!parameters.ContainsKey(key)) continue;

				PlayerSelection selection = (PlayerSelection) (parameters[key]);
				if (selection == null) continue;

				Player p = new Player();
				p.Initialize(selection.input, new Vector2(100 + i * 100, 100), PlayerCostume.playerCostumes[selection.playerSpriteId].texture);
				entities.Add(p);
			}
		}

		public void Initialize(TojamGame game)
		{
			// TODO Load spritesheets for each player character
		}

		public void LoadContent(TojamGame game)
		{
		}

		public void Update(TojamGame game, GameTime gameTime)
		{
			foreach(Entity e in entities) {
				e.Update(game, gameTime);
			}
		}

		public void Draw(TojamGame game, GameTime gameTime)
		{
			game.graphics.GraphicsDevice.Clear(Color.Gainsboro);
			game.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

			foreach (Entity e in entities)
			{
				e.Draw(game, gameTime);
			}

			game.spriteBatch.End();
		}


	}
}
