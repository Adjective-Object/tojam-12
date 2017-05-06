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
			Road_Day,
			Road_DawnDusk,
			Road_Night,
			Store_Day,
			Store_DawnDusk,
			Store_Night,
			GasStation_Day,
			GasStation_DawnDusk,
			GasStation_Night,
			Grow_Op,
			Newmarket_Inuksuit,
			Vaughn_TRex,
			Innisfil_RockingHorse,
			Barrie_SpiritCatcher,
		};
		static Dictionary<Background, PicturePart> knownBackgrounds = new Dictionary<Background, PicturePart>();

		public enum Midground
		{
			None,
			Store_Car
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
			knownBackgrounds[Background.Road_Day] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/road_day"));
			knownBackgrounds[Background.Road_DawnDusk] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/road_dusk"));
			knownBackgrounds[Background.Road_Night] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/road_night"));

			knownBackgrounds[Background.Store_Day] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/store_day"));
			knownBackgrounds[Background.Store_DawnDusk] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/store_dusk"));
			knownBackgrounds[Background.Store_Night] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/store_night"));

			knownBackgrounds[Background.GasStation_Day] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/gas_station_day"));
			knownBackgrounds[Background.GasStation_DawnDusk] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/gas_station_dusk"));
			knownBackgrounds[Background.GasStation_Night] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/gas_station_night"));


			knownBackgrounds[Background.Grow_Op] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/grow_op"));
			knownBackgrounds[Background.Newmarket_Inuksuit] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/newmarket_inuksuit"));
			knownBackgrounds[Background.Vaughn_TRex]= new StaticImage(game.Content.Load<Texture2D>("backgrounds/vaughn_trex_day"));
			knownBackgrounds[Background.Innisfil_RockingHorse]= new StaticImage(game.Content.Load<Texture2D>("backgrounds/rocking_horse"));
			knownBackgrounds[Background.Barrie_SpiritCatcher]= new StaticImage(game.Content.Load<Texture2D>("backgrounds/barrie_spiritcatcher"));

			knownMidgrounds[Midground.None] = null;
			knownMidgrounds[Midground.Store_Car] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/store_car"));

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
			game.spriteBatch.Begin(SpriteSortMode.BackToFront);

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
