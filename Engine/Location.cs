using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Location
    {
        // add Location properties
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // add additional properties with custom datatypes
        public Item ItemRequiredToEnter { get; set; }
        public Quest QuestAvailableHere { get; set; }
        public Boss BossLivingHere { get; set; }
        public Location LocationToNorth { get; set; }
        public Location LocationToEast {get; set;}
        public Location LocationToSouth { get; set;}
        public Location LocationToWest {get; set;}



        // declare parameterized constructor
        // edit constructor to accpet custom datatypes
        // NOTE: not all locations will have a N, E, S, and W value. therefore they are left out of the constructor definition
        public Location(int id, string name, string description, Item itemRequiredToEnter = null, Quest questAvailableHere = null, Boss bossLivingHere = null)
        {
            ID = id;
            Name = name;
            Description = description;
            ItemRequiredToEnter = itemRequiredToEnter;
            QuestAvailableHere = questAvailableHere;
            BossLivingHere = bossLivingHere;

        }
    }
}
