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

		static ItemAction noHittingAnimals = new ItemAction(new String[] { "kick", "punch", "hit", "attack", "hurt" }, (args) =>
		{
			args.g.sendToPlayer(args.p, "Why would you do that?");
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
						int damage = random.Next(2, 5);
						args.p.HealHappyness(-damage);
						args.g.sendToPlayer(args.p, "you " + args.c[0] + " the " + args.c[1]);
						args.g.sendToPlayer(args.p, "Shards of broken glass cut into your hand");
						args.g.sendToPlayer(args.p, "You take " + damage + " damage");
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

		    // goat
			new Item(
				new String[]{ "goat", "billy", "animal" },
				new ItemAction[] {
					new ItemAction(new String[] {"pet"}, (args) => {
					if (!args.p.HasFlag("pet-billy")) {
						int happyInc = random.Next(3,6);
						args.g.sendToPlayer(args.p, "you pet the goat. Happiness +" + happyInc);
						args.p.SetFlag("pet-billy");
						args.p.HealHappyness(happyInc);
					} else {
						args.g.sendToPlayer(args.p, "yep, still a goat");
					}
					}),
					new ItemAction(new String[] {"look"}, (args) => {
						args.g.sendToPlayer(args.p, "A goat! It has weird goat eyes");
					}),
					noHittingAnimals,
				},
				15
			),

			new Item(
				new String[]{ "barn" },
				new ItemAction[] {
					new ItemAction(new String[] {"look"}, (args) => {
						args.g.sendToPlayer(args.p, "A big red barn! You wish you could go inside, but you can't. That would be tresspassing");
					})
				},
				15
			),

			new Item(
				new String[]{ "factory", "giftshop" },
				new ItemAction[] {
					new ItemAction(new String[] {"look"}, (args) => {
						args.g.sendToPlayer(args.p, "Is that an apple factory? That doesn't make sense..");
					})
				},
				15
			),

			new Item(
				new String[]{ "puddle", "gas" },
				new ItemAction[] {
					new ItemAction(new String[] {"look"}, (args) => {
						args.g.sendToPlayer(args.p, "you contemplate the state of the environment.. deep.");
					})
				},
				15
			),

			new Item(
				new String[]{ "sheep", "ram" },
				new ItemAction[] {
					new ItemAction(new String[] {"pet"}, (args) => {
                        if (!args.p.HasFlag("pet-sheep")) {
                            args.p.SetFlag("pet-sheep");
                            args.g.sendToPlayer(args.p, "you try to pet the sheep but it's too fluffy. You feel happier.");
                            args.p.HealHappyness(random.Next(1,3));
                        }
                    }),
					new ItemAction(new String[] {"look"}, (args) => {
						args.g.sendToPlayer(args.p, "gosh dang, that's a big 'ol fluffer");
					}),
					new ItemAction(new String[] {"lick"}, (args) => {
						args.g.sendToPlayer(args.p, "you get a hair stuck in your mouth. You feel unhappy.");
                        args.p.HealHappyness(-random.Next(1,3));
                    }),
					noHittingAnimals,
				},
				15
			),

            new Item(
                new String[]{ "pig" },
                new ItemAction[] {
                    new ItemAction(new String[] {"pet"}, (args) => {
                        if (!args.p.HasFlag("pet-pig")) {
                            args.p.SetFlag("pet-pig");
                            args.g.sendToPlayer(args.p, "The pig so cute. You feel happier.");
                            args.p.HealHappyness(random.Next(3,7));
                        }
                    }),
                    noHittingAnimals,
                },
                15
            ),

            new Item(
                new String[]{ "tractor" },
                new ItemAction[] {
                    new ItemAction(new String[] {"sit in"}, (args) => {
                        if (!args.p.HasFlag("ride-tractor")) {
                            args.p.SetFlag("ride-tractor");
                            args.g.sendToPlayer(args.p, "you take a seat in the tractor. You feel happier.");
                            args.p.HealHappyness(random.Next(1,3));
                        }
                    })
                },
                15
            ),

			// goose
			new Item(
				new String[]{ "goose", "mallard" },
				new ItemAction[] {
					new ItemAction(new String[] {"pet"}, (args) => {
					if (!args.p.HasFlag("pet-goose")) {
						if (random.Next(0, 10) < 8) {
							int happyInc = random.Next(1, 2);
							args.g.sendToPlayer(args.p, "you pet the goose. Happiness +" + happyInc);
							args.p.HealHappyness(happyInc);
						} else {
							int happyInc = random.Next(1, 5);
							args.g.sendToPlayer(args.p, "The goose attacks you! Happiness -" + happyInc);
							args.p.HealHappyness(-happyInc);
							args.p.SetFlag("pet-goose");
						}
					} else {
						args.g.sendToPlayer(args.p, "That goose is mean");
					}
					}),
					new ItemAction(new String[] {"look"}, (args) => {
					if (!args.p.HasFlag("pet-goose")) {
						args.g.sendToPlayer(args.p, "A cute goose");
					} else {
						args.g.sendToPlayer(args.p, "A mean goose");
					}
					}),
					noHittingAnimals,
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
