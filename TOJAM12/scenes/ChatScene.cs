using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TOJAM12.Entities;

namespace TOJAM12
{
	public class ChatScene : Scene
	{
		ChatLog chatLog;
		TextBox textBox;

		public void LoadContent(TojamGame game)
		{
			Debug.WriteLine("Chat : Load Content");
		}

		public void Initialize(TojamGame game)
		{
			Rectangle screenBounds = game.GraphicsDevice.PresentationParameters.Bounds;

			ChatLogStyle style = new ChatLogStyle();
			int messageBufferWidth = 200;
			style.font = game.GameFont;
			style.messagePadding = 20;
			style.externalPadding = 20;
			style.internalBounds = new Rectangle(
				screenBounds.Width - messageBufferWidth + style.externalPadding * 2,
				style.externalPadding,
				messageBufferWidth - style.externalPadding * 2,
				screenBounds.Height - style.externalPadding
			);
			style.backgroundColor = Color.Black;

			chatLog = new ChatLog(style);
			chatLog.Initialize(game);

			chatLog.AppendMessage("Welcome to Roadtrip Simulator 2018!");

			textBox = new TextBox(
				game.GameFont,
				new Rectangle(
					0,
					screenBounds.Height - game.GameFont.LineSpacing,
					screenBounds.Width - messageBufferWidth,
					game.GameFont.LineSpacing
				));
		}

		public void onTransition(Dictionary<string, object> parameters)
		{
			// do nothing on init
		}

		public void Update(TojamGame game, GameTime gameTime)
		{
			chatLog.Update(game, gameTime);
			textBox.Update(game, gameTime);

			// TODO process messages from the server

			// broadcast ready messages to the server
			if (Keyboard.GetState().IsKeyDown(Keys.Enter))
			{
				String textString = textBox.GetAndClear();
				if (textString != "")
				{
					game.gameInstance.SendPlayerCommand(textString);
				}
			}
		}

		public void Draw(TojamGame game, GameTime gameTime)
		{
			game.graphics.GraphicsDevice.Clear(Color.Bisque);
			game.spriteBatch.Begin(SpriteSortMode.Immediate);

			chatLog.Draw(game, gameTime);
			textBox.Draw(game, gameTime);

			game.spriteBatch.End();
		}
	}
}
