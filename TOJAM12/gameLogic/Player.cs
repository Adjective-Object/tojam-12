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
            NotInCar = 0,
            DriversSeat = 1,
            FrontSeat = 2,
            BackLeft = 3,
            BackRight = 4
        }

        public static String GetCarLocationName(CarLocation location)
        {
            switch (location){
                case CarLocation.DriversSeat:
                    return "Driver's Seat";
                case CarLocation.FrontSeat:
                    return "Front Seat";
                case CarLocation.BackLeft:
                    return "Back Left Seat";
                case CarLocation.BackRight:
                    return "Back Right Seat";
                case CarLocation.NotInCar:
                    return "Outside Car";
                default:
                    return "ERROR";
            }
        }

        public CarLocation carLocation;
		public string name;
		public List<Item> inventory = new List<Item>();
		public int hunger = 100;
		public int tired = 100;
		public int thirst = 100;
		public int health = 100;

		public Player(string name)
		{
			this.name = name;
            carLocation = CarLocation.NotInCar;
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


		internal void HealHealth(int v)
		{
			this.health += health;
			if (this.health > 100)
			{
				this.health = 100;
			}

			if (this.health < 0)
			{
				this.health = 0;
			}
		}

		public bool ItemVerb(GameInstance g, string[] command)
		{
			Item.ItemParams args = new Item.ItemParams();
			args.c = command;
			args.g = g;
			args.p = this;
			for (int i = this.inventory.Count - 1; i >= 0; i--) 
			{
				Item item = this.inventory[i];
				if (!item.Matches(command[1])) continue;
				args.i = item;
				if (item.Verb(args)) return true;
			}
			return false;
		}

}
}
