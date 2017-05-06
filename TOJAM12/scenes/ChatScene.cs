using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TOJAM12
{
	public class ChatScene : Scene
	{
		SpriteFont renderFont;
		ChatLog chatLog;

		public void LoadContent(TojamGame game)
		{
			Debug.WriteLine("Chat : Load Content");
			renderFont = game.Content.Load<SpriteFont>("fonts/Cutive_Mono");
		}

		public void Initialize(TojamGame game)
		{
			Debug.WriteLine("Chat : Initialize");

			ChatLogStyle style = new ChatLogStyle();
			style.font = renderFont;
			style.internalBounds = new Rectangle(100, 10, 200, 300);
			style.messagePadding = 20;
			style.externalPadding = 20;

			chatLog = new ChatLog(style);
			chatLog.Initialize(game);

			chatLog.AppendMessage("hello, I would like some sausage");
			chatLog.AppendMessage("gimme that good sausage, fam");
			chatLog.AppendMessage("Is this enough text to force a line wrap? Find out next time on an exciting new episode of dragonball Z");
		}

		public void onTransition(Dictionary<string, object> parameters)
		{
			// do nothing on init
		}

		public void Update(TojamGame game, GameTime gameTime)
		{
			// TODO process messages from the server
			chatLog.Update(game, gameTime);
		}

		public void Draw(TojamGame game, GameTime gameTime)
		{
			game.graphics.GraphicsDevice.Clear(Color.Bisque);
			chatLog.Draw(game, gameTime);
		}
	}
}
