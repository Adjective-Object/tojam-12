using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TOJAM12
{
	public class LoopingImage : StaticImage
	{
		Texture2D[] textures;
		int frameRate;

		public LoopingImage(Texture2D[] textures, int frameRate) : base(textures[0])
		{
			this.textures = textures;
			this.frameRate = frameRate;
		}

		public override void Draw(Rectangle bounds, TojamGame game, GameTime gameTime)
		{
			this.texture = textures[((int) gameTime.TotalGameTime.TotalMilliseconds / frameRate) % this.textures.Length];
			base.Draw(bounds, game, gameTime);
		}
	}
}
