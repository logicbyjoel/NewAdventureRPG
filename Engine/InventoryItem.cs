using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    // Purpose of this class: utilize it as a list property on Player, Boss, and Quest
    public class InventoryItem
    {
        // add InvneotryItem constructor
        public InventoryItem(Item details, int quantity)
        {
            Details = details;
            Quantity = quantity;
        }
        // add InventoryItem props
        public Item Details { get; set; }
        public int Quantity { get; set; }

    }
}
