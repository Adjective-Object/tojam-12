using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOJAM12
{
    public class Location
    {
        public String Name;
		public String Description = null;
        public CarPicture.Background Background;
        public int Id;
        public bool IsDriveLocation;

        public int DriveLength;
        public bool IsExitable;

        public Location DriveLocation;
        public Location WalkLocation;

        public List<Item> PurchaseableItems;
		public List<Item> LocationItems;
        public Location(String name,CarPicture.Background bg, bool isDrive, int length = 0)
        {
            Id = 0;
            Name = name;
            Background = bg;
            IsDriveLocation = isDrive;
            DriveLength = length;

            PurchaseableItems = new List<Item>();
			LocationItems = new List<Item>();
            IsExitable = false;
        }

		public bool HasDescription()
		{
			return this.Description != null;
		}

}

    public class World
    {
        public List<Location> Locations;
        public World()
        {
            Locations = new List<Location>();

            Location Walmart = new Location("Walmart Parking Lot", CarPicture.Background.Walmart, false);
            Location InsideWalmart = new Location("Walmart", CarPicture.Background.Walmart_Inside, false);
            InsideWalmart.PurchaseableItems.Add(Item.Get("water"));
            InsideWalmart.PurchaseableItems.Add(Item.Get("soda"));
            InsideWalmart.PurchaseableItems.Add(Item.Get("burger"));
            InsideWalmart.PurchaseableItems.Add(Item.Get("bottle"));
            InsideWalmart.Description = "Shopping at Walmart eh?";
            InsideWalmart.IsExitable = true;
			Walmart.Description = "The parking lot of a Walmart. You're here to stock up on food before your trip.";
            Walmart.WalkLocation = InsideWalmart;
            InsideWalmart.WalkLocation = Walmart;
            Add(Walmart);
            Add(InsideWalmart);

            Location DriveFarm = new Location("Road to Farm", CarPicture.Background.DrivingFarm, true, 5000);
            Add(DriveFarm);
            Walmart.DriveLocation = DriveFarm;


            Location Farm = new Location("Farm", CarPicture.Background.Farm, false);
			Farm.LocationItems.Add(Item.Get("goat"));
			Farm.LocationItems.Add(Item.Get("sheep"));
			Farm.LocationItems.Add(Item.Get("barn"));
			Farm.Description = "You stop at a goat farm. There are goats and sheep grazing all around you!";
            DriveFarm.DriveLocation = Farm;
            Add(Farm);

            Location DriveApple = new Location("Road to Big Apple", CarPicture.Background.Driving, true, 5000);
            Add(DriveApple);
            Farm.DriveLocation = DriveApple;

            Location BigApple = new Location("Big Apple", CarPicture.Background.BigApple, false);
            DriveApple.DriveLocation = BigApple;
			BigApple.LocationItems.Add(Item.Get("goose"));
			BigApple.LocationItems.Add(Item.Get("factory"));
            BigApple.PurchaseableItems.Add(Item.Get("water"));
            BigApple.PurchaseableItems.Add(Item.Get("apple pie"));
			BigApple.Description = "Welcome to the Big Apple! It's not New York, It's the literal big apple. In Canada!";
            Add(BigApple);

            Location Drive1 = new Location("Road to Gas Station", CarPicture.Background.Driving, true, 5000);
            Add(Drive1);
            BigApple.DriveLocation = Drive1;

            Location GasStation = new Location("Gas Station", CarPicture.Background.GasStation, false);
            GasStation.PurchaseableItems.Add(Item.Get("soda"));
            GasStation.PurchaseableItems.Add(Item.Get("water"));
			BigApple.LocationItems.Add(Item.Get("puddle"));
			GasStation.Description = "You arrive at a gas station. Anyone need to use the bathroom? JK that's not implemented";
            Drive1.DriveLocation = GasStation;
            Add(GasStation);

            Location Drive2 = new Location("Road To Fruit Station", CarPicture.Background.Driving2, true, 5000);
            Add(Drive2);
            GasStation.DriveLocation = Drive2;

            Location FruitStand = new Location("Fruit Stand", CarPicture.Background.FruitStand, false);
			FruitStand.Description = "Hey look, a fruit stand. MM, tasty artisain fruits.";
            Drive2.DriveLocation = FruitStand;
            Add(FruitStand);

            Location DriveAntique = new Location("Road To Antique Store", CarPicture.Background.Driving2, true, 5000);
            Add(DriveAntique);
            FruitStand.DriveLocation = DriveAntique;
            
            Location AntiqueStore = new Location("Antique Store", CarPicture.Background.AntiqueStore, false);
            DriveAntique.DriveLocation = AntiqueStore;
			FruitStand.Description = "You decide to stop at a dusty old antique store.";
            AntiqueStore.PurchaseableItems.Add(Item.Get("potion"));
            Add(AntiqueStore);
            AntiqueStore.LocationItems.Add(Item.Get("tractor"));

            Location Drive3 = new Location("Road To Algonquin", CarPicture.Background.Driving3, true, 5000);
            Add(Drive3);
            AntiqueStore.DriveLocation = Drive3;
			Location Algonquin = new Location("Algonquin", CarPicture.Background.Algonquin, false);
			FruitStand.Description = "At last, you attive at algonquin! Just at that lake.";
            Drive3.DriveLocation = Algonquin;
            Add(Algonquin);
        }

        public void Add(Location location)
        {
            location.Id = Locations.Count;
            Locations.Add(location);
        }

        public Location GetLocation(int location)
        {
            return Locations[location];
        }
    }
}
