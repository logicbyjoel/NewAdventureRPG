﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Quest
    {
        public Quest(int id, string name, string description, int rewarExperiencePoints, int rewardGold)
        {
            ID = id;
            Name = name;
            Description = description;
            RewarExperiencePoints = rewarExperiencePoints;
            RewardGold = rewardGold;
            // instantiate a new list of quest completion items
            QuestCompletionItems = new List<QuestCompletionItem>();
        }

        // add properties for Quest
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RewarExperiencePoints { get; set; }
        public int RewardGold { get; set; }
        // add list of compoletion items per quest
        public List<QuestCompletionItem> QuestCompletionItems { get; set; }

        // new prop to store what item the player receives after completing a quest
        public Item RewardItem { get; set; }
    }
}
