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
		CarPicture carPicture;

		public void LoadContent(TojamGame game)
		{
			Debug.WriteLine("Chat : Load Content");
			CarPicture.LoadContent(game);
		}

		public void Initialize(TojamGame game)
		{
			// get dimensions to size thingsa round
			Rectangle screenBounds = game.GraphicsDevice.PresentationParameters.Bounds;
			int messageBufferWidth = 200;

			// build chatlog
			ChatLogStyle style = new ChatLogStyle();
			style.font = game.GameFont;
			style.linePadding = 1;
			style.messagePadding = 5;
			style.externalPadding = 10;
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

			// build textbox
			textBox = new TextBox(
				game.GameFont,
				new Rectangle(
					0,
					screenBounds.Height - game.GameFont.LineSpacing,
					screenBounds.Width - messageBufferWidth,
					game.GameFont.LineSpacing
				));

			// initialize displayable scene
			carPicture = new CarPicture(new Rectangle(0, 0, screenBounds.Width - messageBufferWidth, screenBounds.Height - game.GameFont.LineSpacing - 5));
			carPicture.Initialize(game);
			carPicture.SetBackground(CarPicture.Background.Walmart);
			carPicture.SetMidground(CarPicture.Midground.Store_Car);
		}

		public void onTransition(Dictionary<string, object> parameters)
		{
			// do nothing on scene enter
		}

		public CarPicture GetCarPicture()
		{
			return this.carPicture;
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

			carPicture.Update(game, gameTime);
		}

		public void Draw(TojamGame game, GameTime gameTime)
		{
			game.graphics.GraphicsDevice.Clear(Color.Black);
			game.spriteBatch.Begin(SpriteSortMode.Immediate);

			carPicture.Draw(game, gameTime);
			chatLog.Draw(game, gameTime);
			textBox.Draw(game, gameTime);

			game.spriteBatch.End();
		}
	}
}
