using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TOJAM12
{
	public class PlayerStatusIndicator : Entity
	{
		SpriteFont font;
		Vector2 position;
		String text;

		public PlayerStatusIndicator(SpriteFont font, Vector2 position)
		{
			this.font = font;
			this.position = position;
		}

		public void Draw(TojamGame game, GameTime gameTime)
		{
			game.spriteBatch.DrawString(this.font, this.text, this.position, Color.White);
		}

		public void Update(TojamGame game, GameTime gameTime)
		{
		}

		public void UpdateToPlayer(TojamGame game, Player p)
		{
			if (p == null)
			{
				this.text = "";
				return;
			}
			this.text = String.Format("HAPPYNESS = {0}     HUNGER = {1}     THIRST = {2}     TIRED = {3}    MONEY = {4}",
									  p.happyness, p.hunger, p.thirst, p.tired, p.money
			                         );
		}

	}
}
