using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class QuestCompletionItem
    {

        // purpose: use as list property in Boss, Player, and Qust classes

        // add param. constructor
        public QuestCompletionItem(Item details, int quantity)
        {
            Details = details;
            Quantity = quantity;
        }
        // add properties
        public Item Details { get; set; }
        public int Quantity { get; set; }
        
    }
}
