using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace TOJAM12.Entities
{
    class TextBox : Entity
    {
        bool[] pressedKeys;
        String currentString;
		SpriteFont font;
		Rectangle bounds;

        public TextBox(SpriteFont spriteFont, Rectangle boxBounds)
        {
			font = spriteFont;
            currentString = "";
            pressedKeys = new bool[256];
			bounds = boxBounds;
        }

        public String GetAndClear()
        {
            String retString = currentString;
            currentString = "";
            pressedKeys = new bool[256];
            return retString;
        }
      
        public void Update(TojamGame game, GameTime gameTime)
        {
            bool[] curPressedKeys = new bool[256];
            foreach(Keys key in Keyboard.GetState().GetPressedKeys())
            {
                if ((int) key < 256)
                {
                    if (!pressedKeys[(int)key])
                    {
                        if (key == Keys.Back)
                        {
                            if (currentString.Length > 0)
                            {
                                currentString = currentString.Substring(0, currentString.Length - 1);
                            }
                        }
                        else if (key == Keys.Space)
                            currentString += " ";
                        else if ((key >= Keys.A && key <= Keys.Z))
                            currentString += key.ToString();
                        else if (key >= Keys.D0 && key <= Keys.D9)
                            currentString += (key - (Keys.D0)).ToString();

						if (font.MeasureString(currentString).X > bounds.Width)
						{
							currentString = currentString.Substring(0, currentString.Count() - 1);
						}
					}
                    curPressedKeys[(int)key] = true;
                }
            }
            pressedKeys = curPressedKeys;
        }

		public void Draw(TojamGame game, GameTime gameTime)
        {
            game.spriteBatch.DrawString(font, currentString, new Vector2(bounds.X, bounds.Y), Color.White);
        }
    }
}
