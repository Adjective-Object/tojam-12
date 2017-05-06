﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TOJAM12
{

	public class Item
	{
		public class ItemParams
		{
			public Player p;
			public Item i;
			public String[] c;
			public GameInstance g; 
		}

		public class ItemAction
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

		static ItemAction pour = new ItemAction(new String[] { "pour" }, (args) =>
					{
						args.p.inventory.Remove(args.i);
						args.g.sendToPlayer(args.p, "you pour the " + args.c[1] + " out on the ground");
						args.p.inventory.Add(Item.Get("bottle"));
					});

		private static Item[] AllItems = new Item[] {
			// water
			new Item(
				new String[] { "water", "water bottle", "flask" },
				new ItemAction[] {
					new ItemAction(new String[] {"drink", "quaff"}, (args) => {
						args.p.inventory.Remove(args.i);
						args.p.HealThirst(10);
						args.p.inventory.Add(Item.Get("bottle"));
						args.g.sendToPlayer(args.p, "you " + args.c[0] + " the " + args.c[1]);
						Debug.WriteLine("inventory is currently: " + String.Join(", ", args.p.inventory));
					}),
					pour
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
					pour
				}),

			new Item(
				new String[]{ "bottle", "empty", "empty bottle", "glass bottle" },
				new ItemAction[] {}
			),
		};


		public static Item Get(String name)
		{
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

		override public String ToString()
		{
			return "Item(" + GetPrimaryName() + ")";
		}

		public override bool Equals(object obj)
		{
			Debug.WriteLine(this + " = " + obj + "? [" + this.GetHashCode() + ", " + obj.GetHashCode() + "]");
			return Object.ReferenceEquals(this, obj);
		}
	
	}
}
