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

        // declare parameterized constructor
        public Location(int id, string name, string description)
        {
            ID = id;
            Name = name;
            Description = description;
        }
    }
}
