using System;
using Microsoft.Xna.Framework;

namespace TOJAM12
{
	public interface PicturePart
	{
		void Draw(Rectangle bounds, TojamGame game, GameTime gameTime);
	}
}
