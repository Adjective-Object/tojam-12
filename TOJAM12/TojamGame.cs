using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Lidgren.Network;

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

        NetServer server;
        NetPeer peer;

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{


            if (Console.ReadKey().Key == ConsoleKey.S)
            {
                Console.WriteLine("Server");
                var config = new NetPeerConfiguration("TOJAM12") { Port = 12345 };
                server = new NetServer(config);
                server.Start();
                peer = server;
            }
            else
            {
                var config = new NetPeerConfiguration("TOJAM12");
                NetClient client = new NetClient(config);
                client.Start();
                client.Connect(host: "127.0.0.1", port: 12345);
                peer = client;
            }

            
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
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    foreach (NetConnection connection in peer.Connections)
                    {
                        NetOutgoingMessage sendMsg = peer.CreateMessage("Pressed A");
                        peer.SendMessage(sendMsg, connection, NetDeliveryMethod.ReliableOrdered);
                    }
                }
			}


			activeScene.Update(this, gameTime);


            NetIncomingMessage message;
            while ((message = peer.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        // handle custom messages
                        Console.WriteLine("Message: " + message.ReadString());
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        // handle connection status messages
                        Console.WriteLine(message.SenderConnection.RemoteEndPoint.Address.ToString() + ": Status Changed " + message.SenderConnection.Status.ToString());
                        break;

                    case NetIncomingMessageType.DebugMessage:
                        // handle debug messages
                        // (only received when compiled in DEBUG mode)
                        Console.WriteLine(message.ReadString());
                        break;

                    /* .. */
                    default:
                        Console.WriteLine("unhandled message with type: "
                            + message.MessageType);
                        break;
                }
            }

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
