using System;
using Microsoft.Xna.Framework;

namespace TOJAM12
{
	public class CompoundPicturePart : PicturePart
	{
		protected PicturePart[] parts;
		public CompoundPicturePart(params PicturePart[] parts)
		{
			this.parts = parts;
		}

		public void Draw(Rectangle bounds, TojamGame game, GameTime gameTime)
		{
			foreach (PicturePart part in parts)
			{
				part.Draw(bounds, game, gameTime);
			}
		}
	}
}
