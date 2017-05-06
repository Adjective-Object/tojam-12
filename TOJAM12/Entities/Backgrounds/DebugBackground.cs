using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TOJAM12
{
	public class DebugBackground : Entity
	{
		Texture2D colorRect;
		Rectangle bounds;
		public DebugBackground(Rectangle screenBounds)
		{
			this.bounds = screenBounds;
		}

		public void Initialize(TojamGame game)
		{
			colorRect = new Texture2D(game.graphics.GraphicsDevice, 1,1);

			Color[] data = new Color[1];
			data[0] = Color.Chocolate;

				colorRect.SetData(data);
		}

		public void Draw(TojamGame game, GameTime gameTime)
		{
			game.spriteBatch.Draw(colorRect, bounds, Color.White);
		}

		public void Update(TojamGame game, GameTime gameTime)
		{
			// TODO animation updates?
		}
	}
}
