using System;
using System.Collections.Generic;

namespace TOJAM12
{

	public class Item
	{
		private static Item[] AllItems = new Item[] {
			new Item(new String[]{ "water", "bottle", "water bottle", "flask"}),
			new Item(new String[]{ "soda", "pop", "coke" }),
		};

		private HashSet<String> itemNames;

		public Item(string[] names)
		{
			foreach (string name in names)
			{
				itemNames.Add(name);
			}
		}

		public bool Matches(string name)
		{
			return itemNames.Contains(name);
		}
	}
}
