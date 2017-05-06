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
            Location Drive1 = new Location("Drive1", CarPicture.Background.Driving, true, 5000);
            Add(Drive1);
            Walmart.DriveLocation = Drive1;

            Location GasStation = new Location("Gas Station", CarPicture.Background.GasStation, false);
            Drive1.DriveLocation = GasStation;
            Add(GasStation);
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
