using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TOJAM12
{
	public class CarPicture : Entity
	{

		// static global behavior

		public enum Sky
		{
			None,
			Day,
			Night,
			Evening,
			Morning,
			Storm
		};
		static Dictionary<Sky, PicturePart> knownSkies = new Dictionary<Sky, PicturePart>();

		public enum Background
		{
			None,
            Driving,
            Driving2,
			Walmart,
            Walmart_Inside,
            GasStation,
            FruitStand
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
			knownSkies[Sky.None] = null;
			knownSkies[Sky.Day] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/SkyClear"));
			knownSkies[Sky.Night] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/SkyNight"));
			knownSkies[Sky.Morning] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/SkyMorning"));
			knownSkies[Sky.Evening] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/SkyEvening"));
			knownSkies[Sky.Storm] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/SkyStorm"));

			knownBackgrounds[Background.None] = null;
            knownBackgrounds[Background.Driving] = new CompoundPicturePart(
				new StaticImage(game.Content.Load<Texture2D>("backgrounds/RoadBasic")),
				new LoopingImage(new Texture2D[] {
					game.Content.Load<Texture2D>("backgrounds/YellowLine_01"),
					game.Content.Load<Texture2D>("backgrounds/YellowLine_02"),
					game.Content.Load<Texture2D>("backgrounds/YellowLine_03"),
				}, 300)
            );
            knownBackgrounds[Background.Driving2] = new CompoundPicturePart(
                new StaticImage(game.Content.Load<Texture2D>("backgrounds/RoadPines")),
                new LoopingImage(new Texture2D[] {
                    game.Content.Load<Texture2D>("backgrounds/YellowLine_01"),
                    game.Content.Load<Texture2D>("backgrounds/YellowLine_02"),
                    game.Content.Load<Texture2D>("backgrounds/YellowLine_03"),
                }, 300)
            );

            knownBackgrounds[Background.Walmart] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/RoadWalmart"));
            knownBackgrounds[Background.Walmart_Inside] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/RoadMountains"));
            knownBackgrounds[Background.GasStation] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/RoadGas"));
            knownBackgrounds[Background.FruitStand] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/RoadFruit"));

            knownMidgrounds[Midground.None] = null;
			knownMidgrounds[Midground.Car] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/Layer-1"));


			knownForegrounds[Foreground.None] = null;
		}

		// local behavior

		Rectangle bounds;

		PicturePart sky;

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

		public void SetSky(Sky s)
		{
			this.sky = knownSkies[s];
		}

		public void UpdateRenderTarget(TojamGame game, GameTime gameTime)
		{

			game.graphics.GraphicsDevice.SetRenderTarget(renderTarget);
			game.graphics.GraphicsDevice.Clear(Color.White);
			game.spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp);

			if (sky != null)
			{
				sky.Draw(this.bounds, game, gameTime);
			}

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
