using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TOJAM12
{
	public class PlayerSelection
	{
		public Input input;
		public int playerSpriteId;

		public PlayerSelection(Input i, int playerSpriteId)
		{
			this.input = i;
			this.playerSpriteId = playerSpriteId;
		}

	};

	public class PlayerSelectScene : Scene
	{
		List<Input> inputs = new List<Input>(5);
		List<PlayerSelection> selectedPlayers = new List<PlayerSelection>(4);
		SpriteFont renderFont;

		public void Initialize(TojamGame game)
		{
			// initialize bound controllers
			inputs.Add(new Input(Input.Type.Keyboard));
			inputs.Add(new Input(Input.Type.JoypadOne));
			inputs.Add(new Input(Input.Type.JoypadTwo));
			inputs.Add(new Input(Input.Type.JoypadThree));
			inputs.Add(new Input(Input.Type.JoypadFour));
		}

		public void Draw(TojamGame game, GameTime gameTime)
		{
			game.graphics.GraphicsDevice.Clear(Color.Bisque);

			game.spriteBatch.Begin(SpriteSortMode.Immediate);
			// Draw each current player
			for (int i = 0; i < selectedPlayers.Count; i++)
			{
				PlayerCostume costume = PlayerCostume.playerCostumes[selectedPlayers[i].playerSpriteId];

				game.spriteBatch.Draw(
					costume.texture,
					new Rectangle(i * 50, 0, 64, 64),
					Color.White
				);

				game.spriteBatch.DrawString(renderFont, costume.name, new Vector2(i*50, 80), Color.White);
			}
			game.spriteBatch.End();
		}

		public void LoadContent(TojamGame game)
		{

			renderFont = game.Content.Load<SpriteFont>("fonts/Cutive_Mono");
		}

		public void onTransition(Dictionary<string, object> parameters)
		{
			// TODO idk what to do here
		}

		public void Update(TojamGame game, GameTime gameTime)
		{
			// check for a player that has pressed start or something that hasn't
			for (int i = selectedPlayers.Count - 1; i >= 0; i--)
			{
				if (selectedPlayers[i].input.KeyPressed(Key.ESCAPE)) 
				{
					selectedPlayers.RemoveAt(i);
				}

				else if (selectedPlayers[i].input.KeyPressed(Key.ENTER))
				{
					// transition into the actual game if 2+ players have joined
					if (selectedPlayers.Count >= 2)
					{
						StartGame(game);
					}
				}
			}

		
			// check for a player pressing start and put them in the boundInputs page
			foreach (Input i in inputs)
			{
				if (i.KeyPressed(Key.ENTER) && selectedPlayers.TrueForAll(p => p.input != i))
				{
					PlayerSelection selection = new PlayerSelection(i, 0);
					selectedPlayers.Add(selection);
				}
			}

			// check for a player pressing start and put them in the boundInputs page
			foreach (PlayerSelection selection in selectedPlayers)
			{
				if (selection.input.KeyPressed(Key.DOWN))
				{
					selection.playerSpriteId = (selection.playerSpriteId + 1) % PlayerCostume.playerCostumes.Count;
				}

				if (selection.input.KeyPressed(Key.UP))
				{
					selection.playerSpriteId = (selection.playerSpriteId - 1 + PlayerCostume.playerCostumes.Count) % PlayerCostume.playerCostumes.Count;
				}

			}

		}

		private void StartGame(TojamGame game)
		{
			Dictionary<string, object> parameters = new Dictionary<string, object>();

			for (int i = 0; i < 4; i++)
			{
				parameters["player" + (i+1)] = selectedPlayers.Count > i ? selectedPlayers[i] : null;
			}

			game.SwitchScene(TojamGame.GameScenes.Game, parameters);
		}
	}
}
