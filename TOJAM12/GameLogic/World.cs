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

        public Location DriveLocation;
        public Location WalkLocation;
        public Location(String name,CarPicture.Background bg)
        {
            Id = 0;
            Name = name;
            Background = bg;
        }
    }

    public class World
    {
        public List<Location> Locations;
        public World()
        {
            Locations = new List<Location>();

            Location Walmart = new Location("Walmart Parking Lot", CarPicture.Background.Walmart);
            Location InsideWalmart = new Location("Walmart", CarPicture.Background.Walmart_Inside);
            Walmart.WalkLocation = InsideWalmart;
            InsideWalmart.WalkLocation = Walmart;
            Add(Walmart);
            Add(InsideWalmart);
            Location Drive1 = new Location("Drive1", CarPicture.Background.Driving);
            Add(Drive1);
            Walmart.DriveLocation = Drive1;
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
