using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TOJAM12
{

	public class Item
	{
		public struct ItemParams
		{
			public Player p;
			public Item i;
			public String[] c;
			public GameInstance g; 
		}

		public struct ItemAction
		{
			public HashSet<String> actions;
			public Action<ItemParams> use;
			public ItemAction(String[] actions, Action<ItemParams> use)
			{
				this.actions = new HashSet<string>();
				foreach (String s in actions)
				{
					this.actions.Add(s);
				}
				this.use = use;
			}
		}

		private static Item[] AllItems = new Item[] {
			// water
			new Item(
				new String[] { "water", "bottle", "water bottle", "flask" },
				new ItemAction[] {
					new ItemAction(new String[] {"drink", "quaff"}, (args) => {
						args.p.inventory.Remove(args.i);
						args.p.HealThirst(10);
						args.p.inventory.Add(Item.Get("bottle"));
						args.g.sendToPlayer(args.p, "you " + args.c[0] + " the " + args.c[1]);
					}),

					new ItemAction(new String[] {"pour"}, (args) => {
						args.p.inventory.Remove(args.i);
						args.g.sendToPlayer(args.p, "you pour the " + args.c[1] + " out on the ground");
					}),

			}),

			// soda
			new Item(
				new String[]{ "soda", "pop", "coke" },
				new ItemAction[] {
					new ItemAction(new String[] {"drink", "quaff"}, (args) => {
						args.p.inventory.Remove(args.i);
						args.p.HealThirst(5);
						args.p.HealTired(2);
						args.p.inventory.Add(Item.Get("bottle"));
						args.g.sendToPlayer(args.p, "you " + args.c[0] + " the " + args.c[1]);
					}),
			}),

			new Item(
				new String[]{ "bottle", "empty", "empty bottle", "glass bottle" },
				new ItemAction[] {}
			),
		};


		public static Item Get(String name)
		{
			Debug.WriteLine("Get " + name);
			foreach (Item item in AllItems)
			{
				if (item.Matches(name))
				{
					return item;
				}
			}
			return null;
		}

		//////////////////
		//////////////////
		//////////////////

		private HashSet<String> itemNames = new HashSet<String>();
		private String primaryName;
		private ItemAction[] actions;

		public Item(string[] names, ItemAction[] actions)
		{
			primaryName = names[0];
			foreach (string name in names)
			{
				itemNames.Add(name);
			}
			this.actions = actions;
		}

		public bool Matches(string name)
		{
			return itemNames.Contains(name);
		}

		public bool Verb(ItemParams args)
		{
			foreach (ItemAction itemAction in this.actions)
			{
				if (itemAction.actions.Contains(args.c[0]))
				{
					itemAction.use(args);
					return true;
				}
			}
			return false;
		}


		public String GetPrimaryName()
		{
			return this.primaryName;
		}
	
	}
}
