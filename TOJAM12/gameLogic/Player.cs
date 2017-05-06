using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TOJAM12
{
	public class Player
	{

        public enum CarLocation
        {
            DriversSeat = 0,
            FrontSeat = 1,
            BackLeft = 2,
            BackRight = 3
        }

        public CarLocation carLocation;
		public string name;
		public List<Item> inventory = new List<Item>();
		public int hunger = 100;
		public int tired = 100;
		public int thirst = 100;

		public Player(string name)
		{
			this.name = name;
		}

		public Item getItem(string itemName)
		{
			foreach (Item i in inventory) {
				if (i.Matches(itemName)) return i;
			}

			return null;
		}

		public void HealHunger(int hunger)
		{
			this.hunger += hunger;
			if (this.hunger > 100)
			{
				this.hunger = 100;
			}

			if (this.hunger < 0)
			{
				this.hunger = 0;
			}
		}

		public void HealThirst(int thirst)
		{
			this.thirst += thirst;
			if (this.thirst > 100)
			{
				this.thirst = 100;
			}

			if (this.thirst < 0)
			{
				this.thirst = 0;
			}
		}

		public void HealTired(int tired)
		{
			this.tired += tired;
			if (this.tired > 100)
			{
				this.tired = 100;
			}

			if (this.tired < 0)
			{
				this.tired = 0;
			}
		}

	}
}
