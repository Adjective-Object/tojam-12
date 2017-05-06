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

		public enum BackgroundId
		{
			Car_Neutral = 0
		};

		// TODO fill this with entities
		List<Entity> backgrounds;
		BackgroundId currentBackgroundId = BackgroundId.Car_Neutral;
		Entity background;
		ChatLog chatLog;
		TextBox textBox;

		public void LoadContent(TojamGame game)
		{
			Debug.WriteLine("Chat : Load Content");
		}

		public void Initialize(TojamGame game)
		{
			Rectangle screenBounds = game.GraphicsDevice.PresentationParameters.Bounds;
			int messageBufferWidth = 200;

			ChatLogStyle style = new ChatLogStyle();
			style.font = game.GameFont;
			style.messagePadding = 20;
			style.externalPadding = 20;
			style.internalBounds = new Rectangle(
				screenBounds.Width - messageBufferWidth + style.externalPadding,
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

			backgrounds = new List<Entity>();
			DebugBackground b = new DebugBackground(
				new Rectangle(0, 0,
							  screenBounds.Width - messageBufferWidth,
							  screenBounds.Height - game.GameFont.LineSpacing
							 ));
			b.Initialize(game);
			backgrounds.Add(b);
			SetBackground(currentBackgroundId);
		}

		public void onTransition(Dictionary<string, object> parameters)
		{
			// do nothing on scene enter
		}

		public void SetBackground(BackgroundId backgroundId)
		{
			this.currentBackgroundId = backgroundId;
			background = backgrounds[(int) currentBackgroundId];
		}

		public void AddMessage(string message)
		{	// add a message to the chatlog
			this.chatLog.AppendMessage(message);
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
			game.graphics.GraphicsDevice.Clear(Color.Black);
			game.spriteBatch.Begin(SpriteSortMode.Immediate);

			background.Draw(game, gameTime);
			chatLog.Draw(game, gameTime);
			textBox.Draw(game, gameTime);

			game.spriteBatch.End();
		}
	}
}
