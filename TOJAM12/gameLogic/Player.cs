using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TOJAM12
{
	public class Player
	{
		public string name;
		public List<Item> inventory = new List<Item>();
		public int hunger = 100;
		public int tired = 100;

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
	}
}
