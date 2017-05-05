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

		List<PlayerSelector> selectors = new List<PlayerSelector>();

		public void Initialize(TojamGame game)
		{
			// initialize bound controllers
			inputs.Add(new Input(Input.Type.Keyboard));
			inputs.Add(new Input(Input.Type.JoypadOne));
			inputs.Add(new Input(Input.Type.JoypadTwo));
			inputs.Add(new Input(Input.Type.JoypadThree));
			inputs.Add(new Input(Input.Type.JoypadFour));

			// do some selectors
			selectors.Add(new PlayerSelector(new Vector2(20, 200)));
			selectors.Add(new PlayerSelector(new Vector2(220, 200)));
			selectors.Add(new PlayerSelector(new Vector2(420, 200)));
            selectors.Add(new PlayerSelector(new Vector2(620, 200)));
			foreach (PlayerSelector selector in selectors)
			{
				selector.SetFont(renderFont);
			}
		}

		public void Draw(TojamGame game, GameTime gameTime)
		{
			game.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

			game.graphics.GraphicsDevice.Clear(Color.Salmon);

			foreach (PlayerSelector selector in selectors)
			{
				selector.Draw(game, gameTime);
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
				if (selectedPlayers[i].input.KeyPressed(Key.CANCEL)) 
				{
					selectedPlayers.RemoveAt(i);
				}

				else if (selectedPlayers[i].input.KeyPressed(Key.START) || selectedPlayers[i].input.KeyPressed(Key.CANCEL))
				{
					// transition into the actual game if 2+ players have joined
					if (selectedPlayers.Count >= 2)
					{
						StartGame(game);
					}
				}
			}


			// check for a player pressing start and put them in the boundInputs page
			for (int i= 0; i < inputs.Count; i++)
			{
				Input input = inputs[i];
				if (input.KeyPressed(Key.START) && selectedPlayers.TrueForAll(p => p.input != input))
				{
					PlayerSelection selection = new PlayerSelection(input, 0);
					selectedPlayers.Add(selection);
					selectors[selectedPlayers.Count - 1].SetSelection(selection, 1);
				}
			}

			// check for a player pressing start and put them in the boundInputs page
			for (int i=0; i< selectedPlayers.Count; i++)
			{
				PlayerSelection selection = selectedPlayers[i];
				if (selection.input.KeyPressed(Key.DOWN) || selection.input.KeyPressed(Key.RIGHT))
				{
					selection.playerSpriteId = (selection.playerSpriteId + 1) % PlayerCostume.playerCostumes.Count;
					selectors[i].SetSelection(selection, 1);
				}

				if (selection.input.KeyPressed(Key.UP) || selection.input.KeyPressed(Key.LEFT))
				{
					selection.playerSpriteId = (selection.playerSpriteId - 1 + PlayerCostume.playerCostumes.Count) % PlayerCostume.playerCostumes.Count;
					selectors[i].SetSelection(selection, -1);
				}

			}

			foreach (PlayerSelector selector in selectors)
			{
				selector.Update(game, gameTime);
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
