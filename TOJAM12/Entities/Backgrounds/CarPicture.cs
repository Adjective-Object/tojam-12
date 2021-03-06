﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TOJAM12
{
	public class CarPicture : Entity
	{

		// static global behavior

		public class PictureEvent
		{
			public String name;
			public Dictionary<String, Object> arguments;
		}

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
            Title,
            Driving,
            Driving2,
            Driving3,
            DrivingFarm,
            DrivingApple,
            Farm,
			Walmart,
            Walmart_Inside,
            GasStation,
            FruitStand,
            AntiqueStore,
            BigApple,
            InsideBigApple,
            Algonquin,
			GameOver,
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
				}, 300, "driving"),

				// special animation of a sign on the side of a road
				// uses the "name" parameter of the passed in dict
				new SignAnimation(
					game.GameFont,
					game.Content.Load<Texture2D>("backgrounds/Sign"),
					game
				)
            );
            knownBackgrounds[Background.Driving2] = new CompoundPicturePart(
                new StaticImage(game.Content.Load<Texture2D>("backgrounds/RoadPines")),
                new LoopingImage(new Texture2D[] {
                    game.Content.Load<Texture2D>("backgrounds/YellowLine_01"),
                    game.Content.Load<Texture2D>("backgrounds/YellowLine_02"),
                    game.Content.Load<Texture2D>("backgrounds/YellowLine_03"),
                }, 300, "driving"),

				// special animation of a sign on the side of a road
				// uses the "name" parameter of the passed in dict
				new SignAnimation(
					game.GameFont,
					game.Content.Load<Texture2D>("backgrounds/Sign"),
					game
				)
            );

            knownBackgrounds[Background.Driving3] = new CompoundPicturePart(
                new StaticImage(game.Content.Load<Texture2D>("backgrounds/RoadAlgonquin_02")),
                new LoopingImage(new Texture2D[] {
                    game.Content.Load<Texture2D>("backgrounds/YellowLine_01"),
                    game.Content.Load<Texture2D>("backgrounds/YellowLine_02"),
                    game.Content.Load<Texture2D>("backgrounds/YellowLine_03"),
                }, 300, "driving"),

				// special animation of a sign on the side of a road
				// uses the "name" parameter of the passed in dict
				new SignAnimation(
					game.GameFont,
					game.Content.Load<Texture2D>("backgrounds/Sign"),
					game
				)
			);

            knownBackgrounds[Background.DrivingFarm] = new CompoundPicturePart(
                new StaticImage(game.Content.Load<Texture2D>("backgrounds/RoadFarm")),
                new LoopingImage(new Texture2D[] {
                    game.Content.Load<Texture2D>("backgrounds/YellowLine_01"),
                    game.Content.Load<Texture2D>("backgrounds/YellowLine_02"),
                    game.Content.Load<Texture2D>("backgrounds/YellowLine_03"),
                }, 300, "driving"),
			
				// special animation of a sign on the side of a road
				// uses the "name" parameter of the passed in dict
				new SignAnimation(
					game.GameFont,
					game.Content.Load<Texture2D>("backgrounds/Sign"),
					game
				)
			);

            knownBackgrounds[Background.DrivingApple] = new CompoundPicturePart(
                new StaticImage(game.Content.Load<Texture2D>("backgrounds/newroad")),
                new LoopingImage(new Texture2D[] {
                    game.Content.Load<Texture2D>("backgrounds/YellowLine_01"),
                    game.Content.Load<Texture2D>("backgrounds/YellowLine_02"),
                    game.Content.Load<Texture2D>("backgrounds/YellowLine_03"),
                }, 300, "driving"),

                // special animation of a sign on the side of a road
                // uses the "name" parameter of the passed in dict
                new SignAnimation(
                    game.GameFont,
                    game.Content.Load<Texture2D>("backgrounds/Sign"),
                    game
                )
            );

            knownBackgrounds[Background.Title] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/titlescreen"));
            knownBackgrounds[Background.Walmart] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/RoadWalmart"));
            knownBackgrounds[Background.Farm] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/RoadGoatFarm"));
            knownBackgrounds[Background.Walmart_Inside] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/InsideWalmart"));
            knownBackgrounds[Background.GasStation] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/RoadGas"));
            knownBackgrounds[Background.FruitStand] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/RoadFruit"));
            knownBackgrounds[Background.AntiqueStore] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/RoadAntique"));
            knownBackgrounds[Background.BigApple] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/roadTheBigApple"));
            knownBackgrounds[Background.InsideBigApple] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/InsideBigApple"));
            knownBackgrounds[Background.Algonquin] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/RoadAlgonquin_01"));
            knownBackgrounds[Background.GameOver] = new StaticImage(game.Content.Load<Texture2D>("backgrounds/gameover"));


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
				sky.Draw( this.bounds, game, gameTime);
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

		public void TriggerEvent(string eventName, Dictionary<String, Object> eventParameters = null)
		{
			if (sky != null) sky.TriggerEvent(eventName, eventParameters);
			if (background != null) background.TriggerEvent(eventName, eventParameters);
			if (midground != null) midground.TriggerEvent(eventName, eventParameters);
			if (foreground != null) foreground.TriggerEvent(eventName, eventParameters);

		}

		public void TriggerEvent(PictureEvent evt)
		{
			TriggerEvent(evt.name, evt.arguments);
		}

	}
}
