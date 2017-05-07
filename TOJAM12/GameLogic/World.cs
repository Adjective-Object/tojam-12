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
        public CarPicture.Background Background;
        public int Id;
        public bool IsDriveLocation;

        public int DriveLength;

        public Location DriveLocation;
        public Location WalkLocation;
        public Location(String name,CarPicture.Background bg, bool isDrive, int length = 0)
        {
            Id = 0;
            Name = name;
            Background = bg;
            IsDriveLocation = isDrive;
            DriveLength = length;
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
            Walmart.WalkLocation = InsideWalmart;
            InsideWalmart.WalkLocation = Walmart;
            Add(Walmart);
            Add(InsideWalmart);


            Location DriveApple = new Location("Road to Big Apple", CarPicture.Background.Driving, true, 5000);
            Add(DriveApple);
            Walmart.DriveLocation = DriveApple;

            Location BigApple = new Location("Big Apple", CarPicture.Background.BigApple, false);
            DriveApple.DriveLocation = BigApple;
            Add(BigApple);

            Location Drive1 = new Location("Road to Gas Station", CarPicture.Background.Driving, true, 5000);
            Add(Drive1);
            BigApple.DriveLocation = Drive1;

            Location GasStation = new Location("Gas Station", CarPicture.Background.GasStation, false);
            Drive1.DriveLocation = GasStation;
            Add(GasStation);

            Location Drive2 = new Location("Road To Fruit Station", CarPicture.Background.Driving2, true, 5000);
            Add(Drive2);
            GasStation.DriveLocation = Drive2;

            Location FruitStand = new Location("Fruit Stand", CarPicture.Background.FruitStand, false);
            Drive2.DriveLocation = FruitStand;
            Add(FruitStand);

            Location DriveAntique = new Location("Road To Antique Store", CarPicture.Background.Driving2, true, 5000);
            Add(DriveAntique);
            FruitStand.DriveLocation = DriveAntique;

            Location AntiqueStore = new Location("Antique Store", CarPicture.Background.AntiqueStore, false);
            DriveAntique.DriveLocation = AntiqueStore;
            Add(AntiqueStore);

            Location Drive3 = new Location("Road To Algonquin", CarPicture.Background.Driving3, true, 5000);
            Add(Drive3);
            AntiqueStore.DriveLocation = Drive3;
            Location Algonquin = new Location("Algonquin", CarPicture.Background.Algonquin, false);
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
