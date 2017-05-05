using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TOJAM12
{
	public class Player : Entity
	{
		float speed = 0.1f;
		Input input;
		Vector2 position;
		Texture2D texture;

		public Player()
		{
		}

		public void Initialize(Input i, Vector2 position, Texture2D texture)
		{
			this.input = i;
			this.position = position;
			this.texture = texture;
		}

		public void Update(TojamGame game, GameTime gameTime)
		{

			float ms = gameTime.ElapsedGameTime.Milliseconds;
			if (input.KeyDown(Key.LEFT))
			{
				this.position.X -= speed * ms;
			}

			if (input.KeyDown(Key.RIGHT))
			{
				this.position.X += speed * ms;
			}

			if (input.KeyDown(Key.UP))
			{
				this.position.Y -= speed * ms;
			}

			if (input.KeyDown(Key.DOWN))
			{
				this.position.Y += speed * ms;
			}
		}

		public void Draw(TojamGame game, GameTime gameTime)
		{
			game.spriteBatch.Draw(texture, 
                  new Rectangle(
					  (int)position.X,
                      (int)position.Y,
					  64,
                      64),
                  Color.White
			      );
		}

	}
}
