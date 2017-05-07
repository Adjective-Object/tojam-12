using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TOJAM12
{
	public class SignAnimation : PicturePart
	{
		int animationStartTime = 0;
		int time = 0;
		bool animating = false;

		float signScale = 4;
		int animationLength= 8000;
		Vector2 startPoint = new Vector2(440, 140);
		Vector2 endPoint = new Vector2(1320, 300);
		Vector2 textCenter;
		float startScale = 0.1f;
		float endScale = 10;

		RenderTarget2D target;

		Texture2D sign;
		TojamGame game;
		SpriteFont font;

		public SignAnimation(SpriteFont font, Texture2D sign, TojamGame game)
		{
			this.sign = sign;
			this.game = game;
			this.font = font;

			target = new RenderTarget2D(game.GraphicsDevice, (int) (sign.Width * signScale), (int)(sign.Height * signScale));
			textCenter = new Vector2(target.Width / 2, 25);
		}

		public void Draw(Rectangle bounds, TojamGame game, GameTime gameTime)
		{
			if (animating)
			{
				time += (int)gameTime.ElapsedGameTime.Milliseconds;
			}

			int millis = time - animationStartTime;
			if (millis > animationLength)
			{
				animating = false;
				return;
			}

			float frac = 1.0f * millis / animationLength;
			Vector2 interpolatedPosition = startPoint * (1 - frac) + endPoint * frac;
			interpolatedPosition -= new Vector2(target.Width, target.Height);
			float scale = startScale * (1 - frac) + endScale * frac;
			game.spriteBatch.Draw(
				target, 
			    new Rectangle(
					(int) interpolatedPosition.X,
					(int) interpolatedPosition.Y,
					(int) (scale * target.Width),
					(int) (scale * target.Height)
				),
				new Rectangle(
					0,
				    0,
					target.Width,
					target.Height
				),
				Color.White
			);

		}

		public void TriggerEvent(string eventName, Dictionary<string, object> eventParameters = null)
		{
			if (eventName == "town")
			{
				String townName = (String)eventParameters["townName"];
				this.UpdateTexture(townName);
				animating = true;
				animationStartTime = time;
			}

			if (eventName == "stop")
			{
				animating = false;
			}

		}

		private void UpdateTexture(string name)
		{
			game.GraphicsDevice.SetRenderTarget(this.target);
			game.GraphicsDevice.Clear(Color.Transparent);
			game.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

			game.spriteBatch.Draw(sign, new Rectangle(0, 0, target.Width, target.Height), new Rectangle(0, 0, sign.Width, sign.Height), Color.White);
			Vector2 strDims = this.font.MeasureString(name);

			game.spriteBatch.DrawString(font, name, this.textCenter - strDims/2, Color.White);

			game.spriteBatch.End();
			game.GraphicsDevice.SetRenderTarget(null);
		}
	}
}
