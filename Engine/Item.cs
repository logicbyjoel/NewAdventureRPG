using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Item
    {
        // add param constructor for this base class
        public Item(int id, string name, string namePlural)
        {
            ID = id;
            Name = name;
            NamePlural = namePlural;
        }
        // add Item properties
        public int ID { get; set; }
        public string Name { get; set; }    
        public string NamePlural { get; set; }

    }
}
