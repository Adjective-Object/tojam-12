using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TOJAM12
{
	public class PlayerSelect : Scene
	{
		List<Texture2D> playerSprites = new List<Texture2D>(4);
		List<Input> inputs = new List<Input>(5);
		List<Input> boundInputs = new List<Input>(4);

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
			for (int i = 0; i < boundInputs.Count; i++)
			{
				game.spriteBatch.Draw(
					playerSprites[i],
					new Rectangle(i * 50, 0, playerSprites[i].Width, playerSprites[i].Height),
					Color.White
				);
			}
			game.spriteBatch.End();
		}

		public void LoadContent(TojamGame game)
		{
			Texture2D alex = game.Content.Load<Texture2D>("misc/alex");
			playerSprites.Add(alex);
			playerSprites.Add(alex);
			playerSprites.Add(alex);
			playerSprites.Add(alex);
		}

		public void onTransition(Dictionary<string, object> parameters)
		{
			// TODO idk what to do here
		}

		public void Update(TojamGame game, GameTime gameTime)
		{
			foreach (Input i in inputs)
			{
				i.Update();
			}

			// check for a player pressing start and put them in the boundInputs page
			foreach (Input i in inputs)
			{
				if (i.KeyPressed(Key.ENTER) && !(boundInputs.Contains(i)))
				{
					Debug.WriteLine("Player Joined");
					boundInputs.Add(i);
				}
			}

			// check for a player that has pressed start or something that hasn't
			for (int i = boundInputs.Count - 1; i >= 0; i--)
			{
				if (boundInputs[i].KeyPressed(Key.ESCAPE)) 
				{
					boundInputs.RemoveAt(i);
				}

				if (boundInputs[i].KeyPressed(Key.ENTER))
				{
					// transition into the actual game if 2+ players have joined
					if (boundInputs.Count >= 2)
					{
						game.SwitchScene(TojamGame.GameScenes.Game);
					}
				}
			}

		}
	}
}
