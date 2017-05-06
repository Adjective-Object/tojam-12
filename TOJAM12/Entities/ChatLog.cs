using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TOJAM12
{
	// style information for drawing text
	public class ChatLogStyle
	{
		public Rectangle internalBounds;
		public SpriteFont font;
		public int linePadding = 2;
		public int messagePadding = 5;
		public int externalPadding = 5;
		public Color color = Color.White;
		public Color backgroundColor = Color.Salmon;

		public Rectangle GetExternalBounds()
		{
			return new Rectangle(
				internalBounds.X - externalPadding,
				internalBounds.Y - externalPadding,
				internalBounds.Width + externalPadding,
				internalBounds.Height + externalPadding
			);
		}
	}

	// message structure
	public class Message
	{
		public ChatLogStyle style;
		public List<string> lines;
		public Message(ChatLogStyle style, string message)
		{
			this.lines = InsertNewlines(style, message);
		}

		public int getHeight(ChatLogStyle style)
		{
			return 
				style.font.LineSpacing * this.lines.Count +
				style.linePadding * (this.lines.Count - 1);
		}

		public void Draw(ChatLogStyle style, Point origin, TojamGame game)
		{
			for (int i = 0; i < lines.Count; i++)
			{
				Point drawOrigin = origin + new Point(0, (style.linePadding + style.font.LineSpacing) * i);
				game.spriteBatch.DrawString(
					style.font,
					lines[i],
					new Vector2(drawOrigin.X, drawOrigin.Y),
					style.color
				);
			}
		}

		/** Insert newlines to fit the text in the box*/
		private List<String> InsertNewlines(ChatLogStyle style, string message)
		{
			string[] words = message.Split(' ');
			List<String> lines = new List<String>();
			StringBuilder builder = new StringBuilder();
			foreach (string word in words)
			{
				String nextString = builder.ToString() + word + " ";
				Vector2 size = style.font.MeasureString(nextString);

				if (size.X > style.internalBounds.Width)
				{   // start a new line if it would be too long
					lines.Add(builder.ToString());
					builder.Clear();
				}

				builder.Append(word);
				builder.Append(" ");

			}
			if (builder.Length != 0)
			{
				lines.Add(builder.ToString());
			}

			Debug.WriteLine("generated list with lines.. ");
			foreach (string line in lines)
			{
				Debug.WriteLine("    " + line);
			}

			// concatenate lines
			return lines;
		}

	}

	/** Manages buffered renering of a number of messages */
	public class ChatLog : Entity
	{
		ChatLogStyle style;
		List<Message> messages = new List<Message>();
		int maxBuffer;
		RenderTarget2D renderTarget;
		bool dirty = false;

		public ChatLog(ChatLogStyle style, int maxBuffer = 100)
		{
			this.style = style;
			this.maxBuffer = maxBuffer;
							
		}

		public void AppendMessage(String message)
		{
			this.messages.Insert(0, new Message(style, message));
			this.dirty = true;
		}

		public void Initialize(TojamGame game)
		{
			Debug.WriteLine("ChatLog : making rendertarget");
			Rectangle externalBounds = style.GetExternalBounds();
			renderTarget = new RenderTarget2D(
				game.GraphicsDevice,
				externalBounds.Width,
				externalBounds.Height
			);
		}

		public void Update(TojamGame game, GameTime gameTime)
		{
			// re-render to the rendertarget if needed
			if (dirty)
			{
				this.UpdateTexture(game);
				this.dirty = false;
			}
		}

		private void UpdateTexture(TojamGame game)
		{
			Debug.WriteLine("updating saved texture");

			// set the rendertarget as background
			game.GraphicsDevice.SetRenderTarget(this.renderTarget);
			game.GraphicsDevice.Clear(style.backgroundColor);
			game.spriteBatch.Begin(SpriteSortMode.Immediate);

			// draw messages from the bottom to the top
			Point origin = new Point(style.externalPadding, style.internalBounds.Height - style.externalPadding);
			int i = 0;
			for (; i < messages.Count; i++)
			{
				origin.Y -= messages[i].getHeight(style);
				messages[i].Draw(style, origin, game);

				origin.Y -= style.messagePadding;
				if (origin.Y < style.internalBounds.Y)
				{
					break;
				}
			}

			// end batch and clear the rendertarget
			game.spriteBatch.End();
			game.GraphicsDevice.SetRenderTarget(null);

			// delete old messages that were not traversed
			while (i < messages.Count)
			{
				messages.RemoveAt(i);
			}
		}

		public void Draw(TojamGame game, GameTime gameTime)
		{
			// start a new batch to draw the buffer to the screen
			game.spriteBatch.Begin(SpriteSortMode.Immediate);
			game.spriteBatch.Draw(renderTarget, style.GetExternalBounds(), Color.White);
			game.spriteBatch.End();
		}
	}
}
