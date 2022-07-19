using System;
using System.Collections.Generic;   // work with collections
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    // change access modifer to public
    public class Player : LivingCreature
    {
        // declare Player properties 
        // NOTE: public properties use Pascal casing

        public int Gold { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }

        // prop for updating the player's location at all times
        public Location CurrentLocation { get; set; }

        // declare props for InventoryItem, LootItem, PlayerQuest, and QuestCompletionItem classes
        // to hold lists contatining InventoryItem and PlayerQuest objects
        public List<InventoryItem> Inventory { get; set; }
        public List<PlayerQuest> Quests { get; set; }

        // add param constructor and reference base class props
        public Player(int currentHitPoints, int maximumHitPoints, int gold, int experiencePoints, int level) : base(currentHitPoints, maximumHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            Level = level;

            Inventory = new List<InventoryItem>();
            Quests = new List<PlayerQuest>();
        }

        // start refactoring code from SuperAdventure.cs > MyAdventureRPG SuperAdventure > MoveTo()
        public bool HasRequiredItemToEnterThisLocation(Location destination)
        {
            if(destination.ItemRequiredToEnter == null)
            {
                // no required item for this location, return true
                return true;
            }

            // check if the player has the required item in their inventory
            foreach (InventoryItem ii in Inventory)
            {
                if(ii.Details.ID == destination.ItemRequiredToEnter.ID)
                {
                    // found the requjired item, 
                    return true;
                }
            }

            // required item not found in inventory
            return false;
        }   // end HasRequiredItemToEnterThisLocation()

        public bool HasThisQuest(Quest quest)
        {
            // find the quest for this destination
            foreach (PlayerQuest playerQuest in Quests)
            {
                if(playerQuest.Details.ID == quest.ID)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CompletedThisQuest(Quest quest)
        {
            // check if qauest has been completed
            foreach(PlayerQuest playerQuest in Quests)
            {
                // find matching quest to current quest
                if (playerQuest.Details.ID == quest.ID)
                {
                    // return bool of player's quest
                    return playerQuest.IsCompleted;
                }
            }
            return false;
        }

        public bool HasAllQuestCompletionItems(Quest quest)
        {
            // check if player has all quest completion items required for this quest
            foreach (QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                // assume item will not be found
                bool foundItemInPlayersInventory = false;

                // check each item in player's inv, to see if they have it, and the minimum quanity
                foreach (InventoryItem ii in Inventory)
                {
                    if(ii.Details.ID == qci.Details.ID) // player has the item in inventory
                    {
                        foundItemInPlayersInventory = true;
                    }
                }
            }
        }

        public void RemoveQuestCompletionItems(Quest quest)
        {

        }

        public void AddItemToInventory(Item itemToAdd)
        {

        }

        public void MarkQuestCompleted(Quest quest)
        {

        }
    }
}
