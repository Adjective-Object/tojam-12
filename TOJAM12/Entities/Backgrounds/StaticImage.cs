using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TOJAM12
{
	public class StaticImage : PicturePart
	{
		protected Texture2D texture;

		public StaticImage(Texture2D texture)
		{
			this.texture = texture;
		}

		public virtual void Draw(Rectangle bounds, TojamGame game, GameTime gameTime)
		{
			Rectangle src = new Rectangle(0, 0, texture.Width, texture.Height);
			Rectangle dest = new Rectangle(0, 0, bounds.Width, bounds.Height);

			// fit dest ratio to a slice of src
			float scalingFactor = Math.Min(1.0f * src.Width / bounds.Width, 1.0f * src.Height / bounds.Height);
			Rectangle scaledDest = new Rectangle(0,0, (int)(scalingFactor * dest.Width), (int)(scalingFactor * dest.Height));
			scaledDest.X = - (scaledDest.Width - src.Width) / 2;
			scaledDest.Y = - (scaledDest.Height - src.Height) / 2;

			game.spriteBatch.Draw(
				this.texture,
				new Rectangle(0, 0, bounds.Width, bounds.Height),
				scaledDest,
				Color.White
			);
		}

		public virtual void TriggerEvent(string eventName, Dictionary<string, object> eventParameters = null)
		{
		}
	}
}
