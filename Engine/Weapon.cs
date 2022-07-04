using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Weapon
    {
        // add Weapon properties
        public int ID { get; set; }
        public string Name { get; set; }

        public string NamePlural { get; set; }
        public string Category { get; set; }
        public int Damage { get; set; }
        public string AmmoType { get; set; }
    }
}
