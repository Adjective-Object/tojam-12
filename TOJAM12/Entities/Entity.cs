using System;
using Microsoft.Xna.Framework;

namespace TOJAM12
{
	public interface Entity
	{
		void Update(TojamGame game, GameTime gameTime);
		void Draw(TojamGame game, GameTime gameTime);
	}
}
