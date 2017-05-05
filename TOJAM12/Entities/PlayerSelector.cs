using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TOJAM12
{
	public class PlayerSelector : TOJAM12.Entity
	{
		Vector2 position;
		SpriteFont font;

		Texture2D currentSprite;
		Texture2D previousSprite;
		string currentName;
		string previousName;
		float animationTime;
		int animationDirection = 0;
		float animationDuration = 0.2f;

		Vector2 introTextPosition = new Vector2(0,50);
		Vector2 restingTextPosition = new Vector2(50, 100);
		Vector2 exitingTextPosition = new Vector2(100, 50);

		Vector2 restingImagePosition = new Vector2(50, 20);
		Vector2 introImagePosition = new Vector2(50, 0);

		public PlayerSelector(Vector2 position)
		{
			this.position = position;
		}

		public void SetFont(SpriteFont spriteFont)
		{
			this.font = spriteFont;

		}

		public void SetSelection(PlayerSelection selection, int direction)
		{
			string selectedName;
			Texture2D selectedSprite;
			if (animationDirection == -1)
			{
				selectedName = previousName;
				selectedSprite = previousSprite;
			}
			else {
				selectedName = currentName;
				selectedSprite = currentSprite;
			}

			if (direction == -1)
			{
				currentName = selectedName;
				currentSprite = selectedSprite;

				// come in from the right
				previousName = PlayerCostume.playerCostumes[selection.playerSpriteId].name;
				previousSprite = PlayerCostume.playerCostumes[selection.playerSpriteId].texture;
			}
			else {
				// come in from the left
				previousName = selectedName;
				previousSprite = selectedSprite;

				currentName = PlayerCostume.playerCostumes[selection.playerSpriteId].name;
				currentSprite = PlayerCostume.playerCostumes[selection.playerSpriteId].texture;
			}

			animationDirection = direction;
			animationTime = 0;
		}

		public void Update(TojamGame game, GameTime gameTime)
		{
			// update animation positions
			animationTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
			if (animationTime > animationDuration) animationTime = animationDuration;
		}

		public void Draw(TojamGame game, GameTime gameTime)
		{
			// press Start to join
			if (currentName == null)
			{
				game.spriteBatch.DrawString(font, "Press Start to Join", this.position, Color.White);
			}
			else {
				// do some easing
				float effectiveTime = (float) Math.Sin(animationTime / animationDuration / (Math.PI / 4.0));
				if (animationDirection == -1)
				{
					effectiveTime = 1 - effectiveTime;
				}
				Debug.WriteLine(effectiveTime);

				Vector2 introImageInterpolated = (introImagePosition * (1 - effectiveTime)) + restingImagePosition * effectiveTime;

				// draw the first text
				Vector2 introInterpolated = (introTextPosition * (1 - effectiveTime)) + restingTextPosition * effectiveTime;
				game.spriteBatch.DrawString(font, currentName, this.position + introInterpolated, Color.White * effectiveTime);
				game.spriteBatch.Draw(currentSprite,
					  new Rectangle(
	                      (int) (position.X + introImageInterpolated.X),
 			              (int) (position.Y + introImageInterpolated.Y),
						  64,
						  64
						 ),
					  Color.White * effectiveTime);

				if (previousName != null)
				{
					// draw the previous name easing out
					Vector2 outroInterpolated = (restingTextPosition * (1 - effectiveTime)) + exitingTextPosition * effectiveTime;
					Vector2 outroImageInterpolated = (introImagePosition * effectiveTime) + restingImagePosition * (1 - effectiveTime);
					game.spriteBatch.DrawString(font, previousName, this.position + outroInterpolated, Color.White * (1 - effectiveTime));
					game.spriteBatch.Draw(
						previousSprite,
					 	new Rectangle(
						   (int)(position.X + outroImageInterpolated.X),
						   (int)(position.Y + outroImageInterpolated.Y),
	                      	64,
	                      	64
	                     ),
                      	Color.White * (1 - effectiveTime));
					
				}
			}
		}
	}
}
