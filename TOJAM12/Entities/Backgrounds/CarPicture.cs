using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TOJAM12
{
	public class CarPicture : Entity
	{

		// static global behavior

		public enum Background
		{
			None,
            Driving,
			Walmart
		};
		static Dictionary<Background, PicturePart> knownBackgrounds = new Dictionary<Background, PicturePart>();

		public enum Midground
		{
			None,
			Car
		};
		static Dictionary<Midground, PicturePart> knownMidgrounds = new Dictionary<Midground, PicturePart>();

		public enum Foreground
		{
			None,
		};
		static Dictionary<Foreground, PicturePart> knownForegrounds = new Dictionary<Foreground, PicturePart>();

		// load some shit fam
		public static void LoadContent(TojamGame game)
		{
			knownBackgrounds[Background.None] = null;
            knownBackgrounds[Background.Driving] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/RoadBasic"));
            knownBackgrounds[Background.Walmart] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/RoadWalmart"));


			knownMidgrounds[Midground.None] = null;
			knownMidgrounds[Midground.Car] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/Layer-1"));


			knownForegrounds[Foreground.None] = null;
		}

		// local behavior

		Rectangle bounds;

		// setting / scene
		PicturePart background;

		// car, etc
		PicturePart midground;

		// player's hands or something?
		PicturePart foreground;

		RenderTarget2D renderTarget;

		public CarPicture(Rectangle bounds)
		{
			this.bounds = bounds;
		}

		public void Initialize(TojamGame game)
		{
			this.renderTarget = new RenderTarget2D(game.GraphicsDevice, bounds.Width, bounds.Height);
		}

		public void SetBackground(Background b)
		{
			this.background = knownBackgrounds[b];
		}

		public void SetMidground(Midground m)
		{
			this.midground = knownMidgrounds[m];
		}

		public void SetForeground(Foreground f)
		{
			this.foreground = knownForegrounds[f];
		}

		public void UpdateRenderTarget(TojamGame game, GameTime gameTime)
		{

			game.graphics.GraphicsDevice.SetRenderTarget(renderTarget);
			game.graphics.GraphicsDevice.Clear(Color.White);
			game.spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp);

			if (background != null)
			{
				background.Draw(this.bounds, game, gameTime);
			}

			if (midground != null)
				midground.Draw(this.bounds, game, gameTime);

			if (foreground != null)
				foreground.Draw(this.bounds, game, gameTime);

			game.spriteBatch.End();

			game.graphics.GraphicsDevice.SetRenderTarget(null);
		}

		public void Update(TojamGame game, GameTime gameTime)
		{
			UpdateRenderTarget(game, gameTime);
		}

		public void Draw(TojamGame game, GameTime gameTime)
		{
			game.spriteBatch.Draw(renderTarget, this.bounds, Color.White);
		}

	}
}
