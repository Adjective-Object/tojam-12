using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TOJAM12
{
	/** Abstract player input between keyboard and gamepad **/
	public class Input
	{
		protected Dictionary<Key, Boolean> pressedThisFrame;
		protected Dictionary<Key, Boolean> pressedLastFrame;

		static protected List<Input> allInstances = new List<Input>();
		public static List<Input> getAllInstances()
		{
			return allInstances;
		}

		public Input() {
			this.Initialize();
		}

		public Input(Type type)
		{
			this.setGamePad(type);
			this.Initialize();
		}

		public enum Type
		{
			Keyboard = -1,
			JoypadOne = PlayerIndex.One,
			JoypadTwo = PlayerIndex.Two,
			JoypadThree = PlayerIndex.Three,
			JoypadFour = PlayerIndex.Four,
		};

		public Boolean disabled = true;
		private Type padIndex = Type.Keyboard;

		public void setGamePad(Type playerIndex)
		{
			this.padIndex = playerIndex;
			this.disabled = false;
		}

		public void Initialize()
		{
			pressedThisFrame = new Dictionary<Key, Boolean>();
			pressedLastFrame = new Dictionary<Key, Boolean>();

			// initialize all entries to false
			foreach (Key k in Enum.GetValues(typeof(Key)))
			{
				pressedThisFrame.Add(k, false);
				pressedLastFrame.Add(k, false);
			}

			allInstances.Add(this);
		}

		public const float deadZone = 0.1f;

		public void Update()
		{
			// switch the last frame and current frame dictionaries
			Dictionary<Key, Boolean> tmp = pressedLastFrame;
			pressedLastFrame = pressedThisFrame;
			pressedThisFrame = tmp;

			// update input based on keyboard or joystick based on the index of this player
			if (padIndex == Type.Keyboard)
			{
				UpdateKeyboard();
			}
			else {
				UpdateJoypad();
			}

		}

		public void UpdateKeyboard()
		{
			// re-evaluate the current frame dictionary
			KeyboardState state = Keyboard.GetState();
			pressedThisFrame[Key.START] = state.IsKeyDown(Keys.Enter);
			pressedThisFrame[Key.CONFIRM] = state.IsKeyDown(Keys.Z);
			pressedThisFrame[Key.CANCEL] = state.IsKeyDown(Keys.X);
			pressedThisFrame[Key.UP] = state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W);
			pressedThisFrame[Key.DOWN] = state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S);
			pressedThisFrame[Key.LEFT] = state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A);
			pressedThisFrame[Key.RIGHT] = state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D);
			pressedThisFrame[Key.DEBUG] = state.IsKeyDown(Keys.Z);

		}
		public void UpdateJoypad()
		{
			KeyboardState state = Keyboard.GetState();
			GamePadState gps = GamePad.GetState((PlayerIndex)padIndex);

			// re-evaluate the current frame dictionary
			pressedThisFrame[Key.START] = gps.Buttons.Start == ButtonState.Pressed;
			pressedThisFrame[Key.CONFIRM] = gps.Buttons.A == ButtonState.Pressed;
			pressedThisFrame[Key.CANCEL] = gps.Buttons.B == ButtonState.Pressed;
			pressedThisFrame[Key.UP] = gps.ThumbSticks.Left.Y > deadZone;
			pressedThisFrame[Key.DOWN] = gps.ThumbSticks.Left.Y < -deadZone;
			pressedThisFrame[Key.LEFT] = gps.ThumbSticks.Left.X < - deadZone;
			pressedThisFrame[Key.RIGHT] = gps.ThumbSticks.Left.X > deadZone;
			pressedThisFrame[Key.DEBUG] = state.IsKeyDown(Keys.Z);
		}

		public Boolean KeyDown(Key k)
		{
			if (disabled)
			{
				return false;
			}
			return pressedThisFrame[k];
		}

		public Boolean KeyPressed(Key k)
		{
			if (disabled)
			{
				return false;
			}

			return (!pressedLastFrame[k]) && pressedThisFrame[k];
		}
	}

	public enum Key
	{
		UP, DOWN, LEFT, RIGHT, START, CONFIRM, CANCEL, DEBUG
	}
}
