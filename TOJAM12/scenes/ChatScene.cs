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
		PlayerStatusIndicator playerStatusIndicator;

		Rectangle chatSceneDimensions = new Rectangle(0,0, 800, 430);
		RenderTarget2D renderTarget;

		public void LoadContent(TojamGame game)
		{
			Debug.WriteLine("Chat : Load Content");
			CarPicture.LoadContent(game);
		}

		public void Initialize(TojamGame game)
		{
			// get dimensions to size thingsa round
			renderTarget = new RenderTarget2D(game.GraphicsDevice, chatSceneDimensions.Width, chatSceneDimensions.Height);
			Rectangle screenBounds = chatSceneDimensions;
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
			chatLog.AppendMessage("Welcome to Algonquin Park Roadtrip Simulator 2018!", Color.White);
			chatLog.AppendMessage("Your goal is to reach Algonquin Park and have happy camping trip", Color.Gray);
			chatLog.AppendMessage("Type `help` to see commands", Color.Gray);
			chatLog.AppendMessage("Type 'join <ip>' to join a game, or 'host' to start a new one", Color.Gray);

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
			carPicture = new CarPicture(new Rectangle(0, 0, screenBounds.Width - messageBufferWidth, screenBounds.Height - game.GameFont.LineSpacing * 2- 5));
			carPicture.Initialize(game);
			carPicture.SetSky(CarPicture.Sky.Day);
			carPicture.SetBackground(CarPicture.Background.Title);
			//carPicture.TriggerEvent("town", new Dictionary<String, Object> { { "townName", "Algonquin" } });

			playerStatusIndicator = new PlayerStatusIndicator(game.GameFont, new Vector2(0, screenBounds.Height - game.GameFont.LineSpacing * 2));
		}

		public void onTransition(Dictionary<string, object> parameters)
		{
			// do nothing on scene enter
		}

		public CarPicture GetCarPicture()
		{
			return this.carPicture;
		}

		static Color[] PlayerColors = {
			new Color(99, 166, 159),
			new Color(242, 141, 88),
			new Color(195, 111, 242),
			new Color(214, 242, 145),
			new Color(242, 225, 172),
		};

		public void AddMessage(string message, int sourcePlayer = -1)
		{   // add a message to the chatlog
			Color c = sourcePlayer < 0 ? Color.White : PlayerColors[sourcePlayer % PlayerColors.Length];
			this.chatLog.AppendMessage(message, c);
		}

		public void Update(TojamGame game, GameTime gameTime)
		{
			chatLog.Update(game, gameTime);
			textBox.Update(game, gameTime);

            if (game.gameInstance.GameStarted())
            {
                if (game.gameInstance.GetMyPlayer().carLocation != Player.CarLocation.NotInCar)
                    carPicture.SetMidground(CarPicture.Midground.Car);
                else
                    carPicture.SetMidground(CarPicture.Midground.None);

                Location currentLocation = game.gameInstance.world.GetLocation(game.gameInstance.GetMyPlayer().worldLocation);
                carPicture.SetBackground(currentLocation.Background);
            }

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
			playerStatusIndicator.UpdateToPlayer(game, game.gameInstance.GetMyPlayer());
		}

		public void Draw(TojamGame game, GameTime gameTime)
		{
			game.graphics.GraphicsDevice.SetRenderTarget(this.renderTarget);
			game.graphics.GraphicsDevice.Clear(Color.Black);
			game.spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);

			carPicture.Draw(game, gameTime);
			chatLog.Draw(game, gameTime);
			textBox.Draw(game, gameTime);
			playerStatusIndicator.Draw(game, gameTime);

			game.spriteBatch.End();
			game.graphics.GraphicsDevice.SetRenderTarget(null);

			game.graphics.GraphicsDevice.Clear(Color.Black);
			game.spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);

			// deferred rendering

			Rectangle realScreenBounds = game.GraphicsDevice.PresentationParameters.Bounds;
			Rectangle renderTargetBounds = this.renderTarget.Bounds;
			float ratio = Math.Max((float)renderTarget.Width / realScreenBounds.Width, (float)renderTarget.Height / realScreenBounds.Height);
			Rectangle scaledRenderTarget = new Rectangle(
				0,
				0,
				(int) (this.renderTarget.Width * ratio),
				(int) (this.renderTarget.Height * ratio)
			);

			scaledRenderTarget.X = (realScreenBounds.Width - scaledRenderTarget.Width) / 2;
			scaledRenderTarget.Y = (realScreenBounds.Height - scaledRenderTarget.Height) / 2;

			game.spriteBatch.Draw(renderTarget, scaledRenderTarget, renderTargetBounds, Color.White);
			game.spriteBatch.End();

		}
	}
}
