using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOJAM12.Entities
{
    class TextBox : Entity
    {
        bool[] pressedKeys;
        String currentString;

        public TextBox()
        {
            currentString = "";
            pressedKeys = new bool[256];
        }

        public void Clear()
        {
            currentString = "";
            pressedKeys = new bool[256];
        }

        public void Update(TojamGame game, GameTime gameTime)
        {
            bool[] curPressedKeys = new bool[256];
            foreach(Keys key in Keyboard.GetState().GetPressedKeys())
            {
                if ((key >= Keys.A && key <= Keys.Z) || key == Keys.Space || key == Keys.Back)
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
                        else
                            currentString += key.ToString();

                        Console.WriteLine(currentString);
                    }
                    curPressedKeys[(int)key] = true;
                }
            }
            pressedKeys = curPressedKeys;
        }
        public void Draw(TojamGame game, GameTime gameTime)
        {
            game.spriteBatch.DrawString(game.GameFont, currentString, new Vector2(0, 0), Color.White);
        }
    }
}
