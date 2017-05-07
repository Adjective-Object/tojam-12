using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TOJAM12
{
	public interface PicturePart
	{
		void Draw(Rectangle bounds, TojamGame game, GameTime gameTime);
		void TriggerEvent(string eventName, Dictionary<String, Object> eventParameters = null);
	}
}
