using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TOJAM12
{
	public interface Scene
	{
		void onTransition(Dictionary<string, object> parameters);
		void Initialize(TojamGame game);
		void LoadContent(TojamGame game);
		void Update(TojamGame game, GameTime gameTime);
		void Draw(TojamGame game, GameTime gameTime);

	}
}
