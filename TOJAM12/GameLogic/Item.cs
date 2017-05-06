using System;
using System.Collections.Generic;

namespace TOJAM12
{

	public class Item
	{
		struct ItemAction
		{
			public String action;
			public Action<Player, Item> use;
			public ItemAction(String action, Action<Player, Item> use)
			{
				this.action = action;
				this.use = use;
			}
		}


		private static Item[] AllItems = new Item[] {
			new Item(
				new String[] { "water", "bottle", "water bottle", "flask" },
				new ItemAction[] {
					new ItemAction("drink", (Player p, Item i) => {
						p.inventory.Remove(i);
						p.HealHunger(5);
					}),
					new ItemAction("quaff", (Player p, Item i) => {
						p.inventory.Remove(i);
						p.HealHunger(5);
					}),
			}),
			new Item(
				new String[]{ "soda", "pop", "coke" },
				new ItemAction[] {
					new ItemAction("drink", (Player p, Item i) => {
						p.inventory.Remove(i);
						p.HealHunger(5);
					}),
			}),
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
				if (verb == itemAction.action)
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
