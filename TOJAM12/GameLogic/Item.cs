using System;
using System.Collections.Generic;

namespace TOJAM12
{

	public class Item
	{
		public struct ItemAction
		{
			public HashSet<String> actions;
			public Action<Player, Item> use;
			public ItemAction(String[] actions, Action<Player, Item> use)
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
					new ItemAction(new String[] {"drink", "quaff"}, (Player p, Item i) => {
						p.inventory.Remove(i);
						p.HealThirst(10);
						p.inventory.Add(Item.Get("bottle"));
					}),
			}),

			// soda
			new Item(
				new String[]{ "soda", "pop", "coke" },
				new ItemAction[] {
					new ItemAction(new String[] {"drink", "quaff"}, (Player p, Item i) => {
						p.inventory.Remove(i);
						p.HealThirst(5);
						p.HealTired(2);
						p.inventory.Add(Item.Get("bottle"));
					}),
			}),

			new Item(
				new String[]{ "bottle", "empty", "empty bottle", "glass bottle" },
				new ItemAction[] {}
			),
		};

		private HashSet<String> itemNames;
		private ItemAction[] actions;

		public Item(string[] names, ItemAction[] actions)
		{
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

		public bool Verb(Player p, string verb)
		{
			foreach (ItemAction itemAction in this.actions)
			{
				if (itemAction.actions.Contains(verb))
				{
					itemAction.use(p, this);
					return true;
				}
			}
			return false;
		}

		public static Item Get(String name)
		{
			foreach (Item item in AllItems)
			{
				if (item.Matches(name)) {
					return item;
				}
			}
			return null;
		}

	}
}
