using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TOJAM12
{
	public class LoopingImage : StaticImage
	{
		Texture2D[] textures;
		int frameRate;
		bool animating = true;

		public LoopingImage(Texture2D[] textures, int frameRate) : base(textures[0])
		{
			this.textures = textures;
			this.frameRate = frameRate;
		}

		public override void Draw(Rectangle bounds, TojamGame game, GameTime gameTime)
		{
			if (animating)
			{
				this.texture = textures[((int)gameTime.TotalGameTime.TotalMilliseconds / frameRate) % this.textures.Length];
			}

			base.Draw(bounds, game, gameTime);
		}

		public override void TriggerEvent(string eventName, Dictionary<string, object> eventParameters = null)
		{
			if (eventName == "stop")
			{
				this.animating = false;
			}

			if (eventName == "start")
			{
				this.animating = true;
			}
		}

	}
}
