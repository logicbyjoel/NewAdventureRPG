using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class PlayerQuest
    {

        // Purpose: utilize as list property in Boss, Plyaer, and Quest classes

        // add param. constructor
        public PlayerQuest(Quest details)
        {
            Details = details;
            IsCompleted = false;
        }
        // add properties
        public Quest Details { get; set; }
        public bool IsCompleted { get; set; }
        
    }
}
