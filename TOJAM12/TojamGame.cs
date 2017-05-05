using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TOJAM12
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class TojamGame : Game
	{
		public GraphicsDeviceManager graphics;
		public SpriteBatch spriteBatch;

		// declaration of scenes in the game
		Scene[] scenes = {
			new PlayerSelectScene(),
			new GameScene(),
		};
		public enum GameScenes
		{
			PlayerSelect = 0,
			Game = 1,
			Results = 2,
		};

		GameScenes activeSceneType = GameScenes.PlayerSelect;
		Scene activeScene = null;

		public TojamGame()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			activeScene = scenes[(int) activeSceneType];
		}

		public void SwitchScene(GameScenes newSceneType, Dictionary<string, object> sceneParameters = null)
		{
			this.activeSceneType = newSceneType;
			Scene nextScene = scenes[(int)activeSceneType];
			nextScene.onTransition(sceneParameters);
			activeScene = nextScene;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();
			foreach (Scene s in scenes)
			{
				s.Initialize(this);
			}
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			PlayerCostume.LoadContent(this);

			foreach (Scene s in scenes)
			{
				s.LoadContent(this);
			}

			// for debug purposes, jump immediately into a 2p game
			//Dictionary<string, object> parameters = new Dictionary<string, object>();
			//parameters["player1"] = new Input(Input.Type.Keyboard);
			//parameters["player2"] = new Input(Input.Type.JoypadOne);
			//this.SwitchScene(GameScenes.Game, parameters);

		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();
#endif

			foreach (Input i in Input.getAllInstances())
			{
				i.Update();
			}


			activeScene.Update(this, gameTime);

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			//TODO: Add your drawing code here
			activeScene.Draw(this, gameTime);

			base.Draw(gameTime);
		}
	}
}
