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
        public int Level {
            // calculate a player's level and start at level 1
            get
            {   return ((ExperiencePoints / 100) + 1); }
            // remove 'set'. This is because we will never put a value into the Level property
            // NOTE: removing 'set' makes a property read-only
            //set; 
        }

        // prop for updating the player's location at all times
        public Location CurrentLocation { get; set; }

        // declare props for InventoryItem, LootItem, PlayerQuest, and QuestCompletionItem classes
        // to hold lists contatining InventoryItem and PlayerQuest objects
        public List<InventoryItem> Inventory { get; set; }
        public List<PlayerQuest> Quests { get; set; }

        // add param constructor and reference base class props
        public Player(int currentHitPoints, int maximumHitPoints, int gold, int experiencePoints) : base(currentHitPoints, maximumHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            // remove Level, because we removed the 'set' from this property
            // Level = level;

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

            // Change: use LINQ to check if the player has the location-specific required item in their inventory
            // NOTE: Exists() will check the items in specified list, to find any items matching the expression given. 
            // if the item is found, will return true; false otherwise
            // NOTE: this is a lambda expression being used
            return Inventory.Exists(ii => ii.Details.ID == destination.ItemRequiredToEnter.ID);


        }   // end HasRequiredItemToEnterThisLocation()

        public bool HasThisQuest(Quest quest)
        {
            // find the quest for this destination
            return Quests.Exists(pq => pq.Details.ID == quest.ID);

        }   // end HasThisQuest()

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

                        if(ii.Quantity < qci.Quantity)  // player does not have enough of this item to complete the quest
                        {
                            return false;
                        }
                    }
                }

                // player does not have any of this quest completion items in their inventory
                if (!foundItemInPlayersInventory)
                {
                    return false;
                }
            }

            // if we made it here, the player must hnave all the required items, and enough of them, to complete the quest
            return true;
        }   // end HasAllQuestCompletionItems()

        public void RemoveQuestCompletionItems(Quest quest)
        {
            // find all quest completion items to remove from inventory
            foreach (QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                foreach (InventoryItem ii in Inventory)
                {
                    if(ii.Details.ID == qci.Details.ID)
                    {
                        // subtract  the quantity from the play'ers inventory that was needed to complete the quest
                        ii.Quantity -= qci.Quantity;
                        break;
                    }
                }
            }
        }   // end RemoveQuestCompletionItems()

        public void AddItemToInventory(Item itemToAdd)
        {
            foreach (InventoryItem ii in Inventory)
            {
                // check for a matching item in inventory to add to its quantity
                if(ii.Details.ID == itemToAdd.ID)
                {
                    // item of same id is found in inventory, increment quantity
                    ii.Quantity++;

                    return; // item was added, exit the function
                }
            }
            // item was not found, add this item to their inventory as a new item, with quantity of 1
            Inventory.Add(new InventoryItem(itemToAdd, 1));
        }   // end AddItemToInventory()

        public void MarkQuestCompleted(Quest quest)
        {
            // check player's quest to locate the quest in question
            foreach (PlayerQuest playerQuest in Quests)
            {
                if(playerQuest.Details.ID == quest.ID)
                {
                    playerQuest.IsCompleted = true;
                    return; // we found teh quest, and marked it as complet. exit the function
                }
            }
        }   // end MarkQuestCompleted()
    }
}
