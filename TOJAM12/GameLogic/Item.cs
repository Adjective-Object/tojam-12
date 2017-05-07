using System;
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

		static Random random = new Random();

		private static List<Item> AllItems = new List<Item> {
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
				},
				5
			),

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
				},
				10
			),

			new Item(
				new String[]{ "bottle", "empty", "empty bottle", "glass bottle" },
				new ItemAction[] {
					new ItemAction(new String[] {"smash", "break"}, (args) => {
						args.p.inventory.Remove(args.i);
						int damage = random.Next(7, 15);
					    args.p.HealHappyness(damage);
						args.g.sendToPlayer(args.p, "you " + args.c[0] + " the " + args.c[1]);
						args.g.sendToPlayer(args.p, "Shards of broken glass fly everywhere");
						args.g.sendToPlayer(args.p, "You feel joy.");
					}),
				},
                1
			),

            // burger
			new Item(
                new String[]{ "burger", "hamburger" },
                new ItemAction[] {
                    new ItemAction(new String[] {"eat"}, (args) => {
                        args.p.inventory.Remove(args.i);
                        args.p.HealHunger(10);
                        args.p.HealTired(5);
                        args.g.sendToPlayer(args.p, "you ate the burger");
                    }),
                    pour
                },
                10
            ),

            // apple pie
			new Item(
                new String[]{ "applepie", "apple pie" },
                new ItemAction[] {
                    new ItemAction(new String[] {"eat"}, (args) => {
                        args.p.inventory.Remove(args.i);
                        args.p.HealHunger(25);
                        args.p.HealTired(25);
                        args.g.sendToPlayer(args.p, "you ate the apple pie");
                    }),
                    pour
                },
                20
            ),

            // potion
			new Item(
                new String[]{ "potion" },
                new ItemAction[] {
                    new ItemAction(new String[] {"drink", "quaff"}, (args) => {
                        args.p.inventory.Remove(args.i);
                        Random rand = new Random();
                        args.p.happyness = rand.Next(5, 100);
                        args.p.thirst = rand.Next(5, 100);
                        args.p.tired = rand.Next(5, 100);
                        args.p.inventory.Add(Item.Get("bottle"));
                        args.g.sendToPlayer(args.p, "you feel weird...");
                    }),
                    pour
                },
                15
            ),
        };


		public static Item Get(String name, List<Item> items = null)
		{
            if (items == null)
                items = AllItems;

			foreach (Item item in items)
			{
				if (item.Matches(name))
				{
					return item;
				}
			}
			return null;
		}

		internal static IEnumerable<Item> GetPurchaseableItems()
		{
			return AllItems;
		}

		//////////////////
		//////////////////
		//////////////////

		private HashSet<String> itemNames = new HashSet<String>();
		private String primaryName;
		private ItemAction[] actions;
		private int price ;

		public Item(string[] names, ItemAction[] actions, int price = 0)
		{
			primaryName = names[0];
			foreach (string name in names)
			{
				itemNames.Add(name);
			}
			this.actions = actions;
			this.price = price;
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

		public int GetPrice()
		{
			return this.price;
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
