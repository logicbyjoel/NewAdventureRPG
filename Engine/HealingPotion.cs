using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    // convert HealingPotion into a child class
    public class HealingPotion : Item
    {
        // reference the properties from the base class of Item
        public HealingPotion(int id, string name, string namePlural, int amountToHeal) : base(id ,name, namePlural)
        {
            AmountToHeal = amountToHeal;
        }

        // Add class propperties
        public int AmountToHeal { get; set; }
    }
}
