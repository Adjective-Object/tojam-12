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

        int basicTimer = 0;

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
            basicTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (basicTimer > 800) basicTimer = 0;

            KeyboardState kbs = Keyboard.GetState();
            bool shiftDown = false;
            if (kbs.IsKeyDown(Keys.LeftShift) || kbs.IsKeyDown(Keys.RightShift)) shiftDown = true;

            bool[] curPressedKeys = new bool[256];
            foreach(Keys key in Keyboard.GetState().GetPressedKeys())
            {

                if ((int)key < 256)
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
                        {
                            currentString += " ";
                        }
                        else if ((key >= Keys.A && key <= Keys.Z))
                        {
                            if (shiftDown) currentString += key.ToString().ToUpper();
                            else currentString += key.ToString().ToLower();
                        }
                        else if (key >= Keys.D0 && key <= Keys.D9)
                        {
                            if (shiftDown)
                            {
                                if (key == Keys.D1) currentString += "!";
                                else if (key == Keys.D2) currentString += "@";
                                else if (key == Keys.D3) currentString += "#";
                                else if (key == Keys.D4) currentString += "$";
                                else if (key == Keys.D5) currentString += "%";
                                else if (key == Keys.D6) currentString += "^";
                                else if (key == Keys.D7) currentString += "&";
                                else if (key == Keys.D8) currentString += "*";
                                else if (key == Keys.D9) currentString += "(";
                                else if (key == Keys.D0) currentString += ")";
                            }
                            else currentString += (key - (Keys.D0)).ToString();
                        }
                        else
                        {
                            if (key == Keys.OemMinus)   { if (shiftDown) currentString += "_"; else currentString += "-"; }
                            else if (key == Keys.OemComma)   { if (shiftDown) currentString += "<"; else currentString += ","; }
                            else if (key == Keys.OemPeriod)  { if (shiftDown) currentString += "<"; else currentString += "."; }
                            else if (key == Keys.OemPipe)    { if (shiftDown) currentString += "|"; else currentString += "\\"; }
                            else if (key == Keys.OemOpenBrackets)   { if (shiftDown) currentString += "{"; else currentString += "["; }
                            else if (key == Keys.OemCloseBrackets)   { if (shiftDown) currentString += "}"; else currentString += "]"; }
                            else if (key == Keys.OemSemicolon)   { if (shiftDown) currentString += ":"; else currentString += ";"; }
                            else if (key == Keys.OemQuotes)   { if (shiftDown) currentString += "\""; else currentString += "'"; }
                            else if (key == Keys.OemQuestion)   { if (shiftDown) currentString += "?"; else currentString += "/"; }
                            else if (key == Keys.OemPlus)   { if (shiftDown) currentString += "+"; else currentString += "="; }
                             
                            else if (key == Keys.NumPad0) currentString += "0";
                            else if (key == Keys.NumPad1) currentString += "1";
                            else if (key == Keys.NumPad2) currentString += "2";
                            else if (key == Keys.NumPad3) currentString += "3";
                            else if (key == Keys.NumPad4) currentString += "4";
                            else if (key == Keys.NumPad5) currentString += "5";
                            else if (key == Keys.NumPad6) currentString += "6";
                            else if (key == Keys.NumPad7) currentString += "7";
                            else if (key == Keys.NumPad8) currentString += "8";
                            else if (key == Keys.NumPad9) currentString += "9";
                             
                            else if (key == Keys.Pause) currentString += "=";
                            else if (key == Keys.Divide) currentString += "/";
                            else if (key == Keys.Multiply) currentString += "*";
                            else if (key == Keys.Subtract) currentString += "-";
                            else if (key == Keys.Add) currentString += "+";
                            else if (key == Keys.Decimal) currentString += ".";
                        }

                        //Console.WriteLine("Key to string: " + key.ToString());

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
            if(basicTimer < 500)
                game.spriteBatch.DrawString(font, "> " + currentString + "_", new Vector2(bounds.X, bounds.Y), Color.White);
            else
                game.spriteBatch.DrawString(font, "> " + currentString, new Vector2(bounds.X, bounds.Y), Color.White);
        }
    }
}
